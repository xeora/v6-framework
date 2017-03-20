Option Strict On

Namespace Xeora.Web.Shared.Service
    <CLSCompliant(True)>
    Public NotInheritable Class VariablePoolOperation
        Private Shared _Cache As IVariablePool = Nothing

        Private _SessionKeyID As String

        Public Sub New(ByVal SessionKeyID As String)
            If VariablePoolOperation._Cache Is Nothing Then
                Try
                    Dim RequestAsm As Reflection.Assembly, objRequest As Type

                    RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
                    objRequest = RequestAsm.GetType("Xeora.Web.Handler.RemoteInvoke", False, True)

                    VariablePoolOperation._Cache =
                        CType(objRequest.InvokeMember("VariablePool", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing), IVariablePool)
                Catch ex As Exception
                    Throw New Reflection.TargetInvocationException("Communication Error! Variable Pool is not accessable...", ex)
                End Try
            End If

            Me._SessionKeyID = SessionKeyID
        End Sub

        Public Sub [Set](ByVal Key As String, ByVal Value As Object)
            If Not String.IsNullOrWhiteSpace(Key) AndAlso Key.Length > 128 Then _
                Throw New ArgumentOutOfRangeException("Key must not be longer than 128 characters!")

            If Value Is Nothing Then
                Me.UnRegisterVariableFromPool(Key)
            Else
                Me.RegisterVariableToPool(Key, Value)
            End If
        End Sub

        Public Function [Get](ByVal Key As String) As Object
            Return Me.GetVariableFromPool(Key)
        End Function

        Public Sub Transfer(ByVal FromSessionID As String)
            Me.TransferRegistrations(
                String.Format("{0}_{1}", FromSessionID, Helpers.Context.Request.HashCode))
        End Sub

        Private Function GetVariableFromPool(ByVal Key As String) As Object
            If String.IsNullOrEmpty(Me._SessionKeyID) Then Throw New ArgumentNullException("SessionID must not be null!")

            Dim rObject As Object =
               VariablePoolPreCache.GetCachedVariable(Me._SessionKeyID, Key)

            If rObject Is Nothing Then
                Dim serializedValue As Byte() =
                    VariablePoolOperation._Cache.GetVariableFromPool(Me._SessionKeyID, Key)

                If Not serializedValue Is Nothing Then
                    Dim forStream As IO.Stream = Nothing

                    Try
                        Dim binFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                        binFormater.Binder = New OverrideBinder()

                        forStream = New IO.MemoryStream(serializedValue)

                        rObject = binFormater.Deserialize(forStream)

                        VariablePoolPreCache.CacheVariable(Me._SessionKeyID, Key, rObject)
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    Finally
                        If Not forStream Is Nothing Then
                            forStream.Close()
                            GC.SuppressFinalize(forStream)
                        End If
                    End Try
                End If
            End If

            Return rObject
        End Function

        Private Sub RegisterVariableToPool(ByVal Key As String, ByVal Value As Object)
            If String.IsNullOrEmpty(Me._SessionKeyID) Then Throw New ArgumentNullException("SessionID must not be null!")

            VariablePoolPreCache.CleanCachedVariables(Me._SessionKeyID, Key)

            Dim serializedValue As Byte() = New Byte() {}
            Dim forStream As IO.Stream = Nothing

            Try
                forStream = New IO.MemoryStream()

                Dim binFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                binFormater.Serialize(forStream, Value)

                serializedValue = CType(forStream, IO.MemoryStream).ToArray()
            Catch ex As Exception
                ' Just Handle Exceptions
            Finally
                If Not forStream Is Nothing Then
                    forStream.Close()
                    GC.SuppressFinalize(forStream)
                End If
            End Try

            VariablePoolOperation._Cache.RegisterVariableToPool(Me._SessionKeyID, Key, serializedValue)
        End Sub

        Private Sub UnRegisterVariableFromPool(ByVal Key As String)
            VariablePoolPreCache.CleanCachedVariables(Me._SessionKeyID, Key)

            ' Unregister Variable From Pool Immidiately. 
            ' Otherwise it will cause cache reload in the same domain call
            VariablePoolOperation._Cache.UnRegisterVariableFromPool(Me._SessionKeyID, Key)
        End Sub

        Private Sub TransferRegistrations(ByVal FromSessionID As String)
            If String.IsNullOrEmpty(Me._SessionKeyID) Then _
                Throw New ArgumentNullException("ToSessionID must not be null!")
            If String.IsNullOrEmpty(FromSessionID) Then _
                Throw New ArgumentNullException("FromSessionID must not be null!")

            VariablePoolOperation._Cache.TransferRegistrations(FromSessionID, Me._SessionKeyID)
        End Sub

        Private Function SerializeNameValuePairs(ByVal NameValuePairs As Generic.Dictionary(Of String, Object)) As Byte()
            Dim SerializableDictionary As New SerializableDictionary()

            Dim serializedValue As Byte() = New Byte() {}

            If Not NameValuePairs Is Nothing Then
                Dim forStream As IO.Stream = Nothing

                For Each VariableName As String In NameValuePairs.Keys
                    serializedValue = New Byte() {} : forStream = Nothing

                    Try
                        forStream = New IO.MemoryStream

                        Dim binFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                        binFormater.Serialize(forStream, NameValuePairs.Item(VariableName))

                        serializedValue =
                            CType(forStream, IO.MemoryStream).ToArray()

                        SerializableDictionary.Add(
                            New SerializableDictionary.SerializableKeyValuePair(VariableName, serializedValue))
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    Finally
                        If Not forStream Is Nothing Then
                            forStream.Close()
                            GC.SuppressFinalize(forStream)
                        End If
                    End Try
                Next

                serializedValue = New Byte() {} : forStream = Nothing

                Try
                    forStream = New IO.MemoryStream()

                    Dim binFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                    binFormater.Serialize(forStream, SerializableDictionary)

                    serializedValue = CType(forStream, IO.MemoryStream).ToArray()
                Catch ex As Exception
                    ' Just Handle Exceptions
                Finally
                    If Not forStream Is Nothing Then
                        forStream.Close()
                        GC.SuppressFinalize(forStream)
                    End If
                End Try
            End If

            Return serializedValue
        End Function

        ' This class required to eliminate the mass request to VariablePool.
        ' VariablePool registration requires serialization...
        ' Use PreCache for only read keys do not use for variable registration!
        ' It is suitable for repeating requests...
        Private Class VariablePoolPreCache
            Private Shared _VariablePreCache As Hashtable = Nothing

            Public Shared ReadOnly Property VariablePreCache() As Hashtable
                Get
                    If VariablePoolPreCache._VariablePreCache Is Nothing Then _
                        VariablePoolPreCache._VariablePreCache = Hashtable.Synchronized(New Hashtable)

                    Return VariablePoolPreCache._VariablePreCache
                End Get
            End Property

            Public Shared Function GetCachedVariable(ByVal SessionKeyID As String, ByVal Key As String) As Object
                Dim rObject As Object = Nothing

                Threading.Monitor.Enter(VariablePoolPreCache.VariablePreCache.SyncRoot)
                Try
                    If VariablePoolPreCache.VariablePreCache.ContainsKey(SessionKeyID) Then
                        Dim NameValuePairs As Generic.Dictionary(Of String, Object) =
                            CType(VariablePoolPreCache.VariablePreCache.Item(SessionKeyID), Generic.Dictionary(Of String, Object))

                        If Not NameValuePairs Is Nothing AndAlso
                            NameValuePairs.ContainsKey(Key) AndAlso
                            Not NameValuePairs.Item(Key) Is Nothing Then _
                                rObject = NameValuePairs.Item(Key)
                    End If
                Finally
                    Threading.Monitor.Exit(VariablePoolPreCache.VariablePreCache.SyncRoot)
                End Try

                Return rObject
            End Function

            Public Shared Sub CacheVariable(ByVal SessionKeyID As String, ByVal Key As String, ByVal Value As Object)
                Threading.Monitor.Enter(VariablePoolPreCache.VariablePreCache.SyncRoot)
                Try
                    Dim NameValuePairs As Generic.Dictionary(Of String, Object) = Nothing
                    If VariablePoolPreCache.VariablePreCache.ContainsKey(SessionKeyID) Then
                        NameValuePairs = CType(VariablePoolPreCache.VariablePreCache.Item(SessionKeyID), Generic.Dictionary(Of String, Object))

                        If Value Is Nothing Then
                            If NameValuePairs.ContainsKey(Key) AndAlso Not NameValuePairs.Remove(Key) Then _
                                NameValuePairs.Item(Key) = Nothing
                        Else
                            NameValuePairs.Item(Key) = Value
                        End If

                        VariablePoolPreCache.VariablePreCache.Item(SessionKeyID) = NameValuePairs
                    Else
                        If Not Value Is Nothing Then
                            NameValuePairs = New Generic.Dictionary(Of String, Object)
                            NameValuePairs.Add(Key, Value)

                            VariablePoolPreCache.VariablePreCache.Add(SessionKeyID, NameValuePairs)
                        End If
                    End If
                Finally
                    Threading.Monitor.Exit(VariablePoolPreCache.VariablePreCache.SyncRoot)
                End Try
            End Sub

            Public Shared Sub CleanCachedVariables(ByVal SessionKeyID As String, ByVal Key As String)
                Threading.Monitor.Enter(VariablePoolPreCache.VariablePreCache.SyncRoot)
                Try
                    If VariablePoolPreCache.VariablePreCache.ContainsKey(SessionKeyID) Then
                        Dim NameValuePairs As Generic.Dictionary(Of String, Object) =
                            CType(VariablePoolPreCache.VariablePreCache.Item(SessionKeyID), Generic.Dictionary(Of String, Object))

                        If NameValuePairs.ContainsKey(Key) AndAlso Not NameValuePairs.Remove(Key) Then _
                            NameValuePairs.Item(Key) = Nothing

                        VariablePoolPreCache.VariablePreCache.Item(SessionKeyID) = NameValuePairs
                    End If
                Finally
                    Threading.Monitor.Exit(VariablePoolPreCache.VariablePreCache.SyncRoot)
                End Try
            End Sub
        End Class

        <CLSCompliant(True), Serializable()>
        Public Class SerializableDictionary
            Inherits Generic.List(Of SerializableKeyValuePair)

            <Serializable()>
            Public Class SerializableKeyValuePair
                Private _Key As String
                Private _Value As Byte()

                Public Sub New(ByVal Key As String, ByVal Value As Byte())
                    Me._Key = Key
                    Me._Value = Value
                End Sub

                Public ReadOnly Property Key() As String
                    Get
                        Return Me._Key
                    End Get
                End Property

                Public ReadOnly Property Value() As Byte()
                    Get
                        Return Me._Value
                    End Get
                End Property
            End Class
        End Class
    End Class
End Namespace