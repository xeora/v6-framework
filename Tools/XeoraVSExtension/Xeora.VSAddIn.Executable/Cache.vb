Namespace Xeora.VSAddIn.Executable
    Public Class Cache
        Private Shared _self As Cache = Nothing
        Private _Cache As List(Of CacheInfo)

        Public Sub New()
            Me._Cache = New List(Of CacheInfo)
        End Sub

        Public Shared ReadOnly Property Instance As Cache
            Get
                If Cache._self Is Nothing Then Cache._self = New Cache()

                Return Cache._self
            End Get
        End Property

        Public Enum QueryTypes
            None
            Classes
            Methods
        End Enum

        Public Function IsLatest(ByVal AssemblyPath As String, ByVal QueryType As QueryTypes, Optional ByVal ClassIDs As String() = Nothing) As Boolean
            Dim rBoolean As Boolean = False

            For Each AssemblyFile As String In IO.Directory.GetFiles(AssemblyPath, "*.dll")
                Dim AssemblyFileInfo As New IO.FileInfo(AssemblyFile)

                For Each cI As CacheInfo In Me._Cache
                    If String.Compare(cI.AssemblyFile, AssemblyFile) = 0 AndAlso
                        Date.Compare(cI.AssemblyDate, AssemblyFileInfo.LastWriteTime) >= 0 Then

                        Select Case QueryType
                            Case QueryTypes.Classes
                                rBoolean = cI.BaseClass.ClassesTouched

                                If Not ClassIDs Is Nothing Then
                                    Dim CO As CacheInfo.ClassInfo =
                                        cI.Find(ClassIDs)

                                    If CO Is Nothing OrElse (Not CO Is Nothing AndAlso Not CO.ClassesTouched) Then rBoolean = False
                                End If

                            Case QueryTypes.Methods
                                rBoolean = True

                                If Not ClassIDs Is Nothing Then
                                    Dim CO As CacheInfo.ClassInfo =
                                        cI.Find(ClassIDs)

                                    If CO Is Nothing OrElse (Not CO Is Nothing AndAlso Not CO.MethodsTouched) Then rBoolean = False
                                End If
                            Case Else
                                rBoolean = True
                        End Select

                        Exit For
                    End If
                Next
            Next

            Return rBoolean
        End Function

        Public Function GetIDs(ByVal AssemblyPath As String) As String()
            Dim rStringList As New List(Of String)

            For Each cI As CacheInfo In Me._Cache
                If String.Compare(AssemblyPath, cI.AssemblyPath) = 0 Then _
                    rStringList.Add(cI.AssemblyID)
            Next

            Return rStringList.ToArray()
        End Function

        Public Function AddInfo(ByVal AssemblyPath As String, ByVal AssemblyID As String) As CacheInfo
            For cIC As Integer = Me._Cache.Count - 1 To 0 Step -1
                Dim aCI As CacheInfo = Me._Cache.Item(cIC)

                If String.Compare(aCI.AssemblyPath, AssemblyPath) = 0 AndAlso
                    String.Compare(aCI.AssemblyID, AssemblyID) = 0 Then

                    Me._Cache.RemoveAt(cIC)
                End If
            Next

            Dim rCacheInfo As New CacheInfo(AssemblyPath, AssemblyID)

            Me._Cache.Add(rCacheInfo)

            Return rCacheInfo
        End Function

        Public Function GetInfo(ByVal AssemblyPath As String, ByVal AssemblyID As String) As CacheInfo
            Dim rCacheInfo As CacheInfo = Nothing

            For Each Item As CacheInfo In Me._Cache
                If String.Compare(Item.AssemblyPath, AssemblyPath) = 0 AndAlso
                    String.Compare(Item.AssemblyID, AssemblyID) = 0 Then

                    rCacheInfo = Item

                    Exit For
                End If
            Next

            Return rCacheInfo
        End Function

        Public Class CacheInfo
            Private _AssemblyPath As String
            Private _AssemblyID As String
            Private _AssemblyFile As String
            Private _AssemblyDate As Date

            Private _ClassInfo As ClassInfo

            Public Sub New(ByVal AssemblyPath As String, ByVal AssemblyID As String)
                Me._AssemblyPath = AssemblyPath
                Me._AssemblyID = AssemblyID

                Me._AssemblyFile = IO.Path.Combine(Me._AssemblyPath, String.Format("{0}.dll", Me._AssemblyID))

                If IO.File.Exists(Me._AssemblyFile) Then Me._AssemblyDate = IO.File.GetLastWriteTime(Me._AssemblyFile) Else Me._AssemblyDate = Date.MaxValue

                Me._ClassInfo = New ClassInfo(AssemblyID)
            End Sub

            Public ReadOnly Property AssemblyPath As String
                Get
                    Return Me._AssemblyPath
                End Get
            End Property

            Public ReadOnly Property AssemblyID As String
                Get
                    Return Me._AssemblyID
                End Get
            End Property

            Public ReadOnly Property AssemblyFile As String
                Get
                    Return Me._AssemblyFile
                End Get
            End Property

            Public ReadOnly Property AssemblyDate As Date
                Get
                    Return Me._AssemblyDate
                End Get
            End Property

            Public ReadOnly Property BaseClass As ClassInfo
                Get
                    Return Me._ClassInfo
                End Get
            End Property

            Public Function Find(ByVal ClassIDs As String()) As ClassInfo
                Dim rCO As CacheInfo.ClassInfo =
                   Me.BaseClass

                For Each ClassID As String In ClassIDs
                    Dim IsFound As Boolean = False
                    For Each ClassInfo As CacheInfo.ClassInfo In rCO.Classes
                        If String.Compare(ClassInfo.ID, ClassID) = 0 Then
                            rCO = ClassInfo
                            IsFound = True

                            Exit For
                        End If
                    Next

                    If Not IsFound Then rCO = Nothing : Exit For
                Next

                Return rCO
            End Function

            Public Class ClassInfo
                Private _ID As String
                Private _Classes As List(Of ClassInfo)
                Private _Methods As List(Of MethodInfo)

                Private _ClassesTouched As Boolean
                Private _MethodsTouched As Boolean

                Public Sub New(ByVal ID As String)
                    Me._ID = ID
                    Me._Classes = New List(Of ClassInfo)
                    Me._Methods = New List(Of MethodInfo)

                    Me._ClassesTouched = False
                    Me._MethodsTouched = False
                End Sub

                Public ReadOnly Property ID As String
                    Get
                        Return Me._ID
                    End Get
                End Property

                Public ReadOnly Property Classes As ClassInfo()
                    Get
                        Return Me._Classes.ToArray()
                    End Get
                End Property

                Public Function AddClassInfo(ByVal ID As String) As ClassInfo
                    For cIC As Integer = Me._Classes.Count - 1 To 0 Step -1
                        Dim cI As ClassInfo = Me._Classes.Item(cIC)

                        If String.Compare(cI.ID, ID) = 0 Then _
                            Me._Classes.RemoveAt(cIC)
                    Next

                    Dim rClassInfo As New ClassInfo(ID)

                    Me._Classes.Add(rClassInfo)
                    Me._ClassesTouched = True

                    Return rClassInfo
                End Function

                Public ReadOnly Property ClassesTouched As Boolean
                    Get
                        Return Me._ClassesTouched
                    End Get
                End Property

                Public ReadOnly Property Methods As MethodInfo()
                    Get
                        Return Me._Methods.ToArray()
                    End Get
                End Property

                Public Function AddMethodInfo(ByVal ID As String, ByVal Params As String()) As MethodInfo
                    For mIC As Integer = Me._Methods.Count - 1 To 0 Step -1
                        Dim mI As MethodInfo = Me._Methods.Item(mIC)

                        If String.Compare(mI.ID, ID) = 0 AndAlso
                            Me.ListCompare(mI.Params, Params) Then _
                            Me._Methods.RemoveAt(mIC)
                    Next

                    Dim rMethodInfo As New MethodInfo(ID, Params)

                    Me._Methods.Add(rMethodInfo)
                    Me._MethodsTouched = True

                    Return rMethodInfo
                End Function

                Public ReadOnly Property MethodsTouched As Boolean
                    Get
                        Return Me._MethodsTouched
                    End Get
                End Property

                Private Function ListCompare(ByVal Params1 As String(), Params2 As String()) As Boolean
                    Dim rBoolean As Boolean = False

                    If Params1 Is Nothing AndAlso Params2 Is Nothing Then
                        rBoolean = True
                    ElseIf Params1 Is Nothing OrElse Params2 Is Nothing Then
                        rBoolean = False
                    ElseIf Params1.Length <> Params2.Length Then
                        rBoolean = False
                    ElseIf Params1.Length = Params2.Length Then
                        rBoolean = True

                        For pC As Integer = 0 To Params1.Length - 1
                            If String.Compare(Params1(pC), Params2(pC)) <> 0 Then rBoolean = False : Exit For
                        Next
                    End If

                    Return rBoolean
                End Function

                Public Class MethodInfo
                    Private _ID As String
                    Private _Params As String()

                    Public Sub New(ByVal ID As String, ByVal Params As String())
                        Me._ID = ID
                        Me._Params = Params
                    End Sub

                    Public ReadOnly Property ID As String
                        Get
                            Return Me._ID
                        End Get
                    End Property

                    Public ReadOnly Property Params As String()
                        Get
                            Return Me._Params
                        End Get
                    End Property
                End Class
            End Class
        End Class
    End Class
End Namespace