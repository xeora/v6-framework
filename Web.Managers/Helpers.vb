Option Strict On

Imports Microsoft.Win32

Namespace SolidDevelopment.Web.Helpers
    '************************** DateTime Action Class *****************************

    Public Class DateTime
        Public Enum DateTimeFormat As Byte
            [Date] = 0
            DateTime = 1
        End Enum

        Public Overloads Shared Function FormatDateTime(Optional ByVal DTF As DateTimeFormat = DateTimeFormat.DateTime) As Long
            Return DateTime.FormatDateTime(Date.Now, DTF)
        End Function

        Public Overloads Shared Function FormatDateTime(ByVal vDateTime As System.DateTime, Optional ByVal DTF As DateTimeFormat = DateTimeFormat.DateTime) As Long
            Dim tDateTime As System.DateTime = vDateTime
            Dim tDay As String, tMonth As String, tYear As String, tHour As String, tMinute As String, tSecond As String, rString As String

            tDay = tDateTime.Day.ToString()
            If CType(tDay, Integer) < 10 Then tDay = "0" & tDay

            tMonth = tDateTime.Month.ToString()
            If CType(tMonth, Integer) < 10 Then tMonth = "0" & tMonth

            tYear = tDateTime.Year.ToString()

            tHour = tDateTime.Hour.ToString()
            If CType(tHour, Integer) < 10 Then tHour = "0" & tHour

            tMinute = tDateTime.Minute.ToString()
            If CType(tMinute, Integer) < 10 Then tMinute = "0" & tMinute

            tSecond = tDateTime.Second.ToString()
            If CType(tSecond, Integer) < 10 Then tSecond = "0" & tSecond

            rString = tYear & tMonth & tDay

            Select Case DTF
                Case DateTimeFormat.DateTime
                    rString &= tHour & tMinute & tSecond
            End Select

            Return CType(rString, Long)
        End Function

        Public Overloads Shared Function FormatDateTime(ByVal vDateTime As Long) As System.DateTime
            Dim dtString As String = CType(vDateTime, String)

            If (dtString.Length >= 5 AndAlso dtString.Length <= 8) OrElse _
                (dtString.Length >= 11 AndAlso dtString.Length <= 14) Then

                If dtString.Length >= 5 AndAlso dtString.Length <= 8 Then
                    dtString = dtString.PadLeft(8, "0"c)
                ElseIf dtString.Length >= 11 AndAlso dtString.Length <= 14 Then
                    dtString = dtString.PadLeft(14, "0"c)
                End If
            Else
                Throw New Exception("Long value must have 8 or between 14 steps according to its type!")
            End If

            Dim rDate As Date

            If dtString.Length = 14 Then
                rDate = New Date( _
                            Integer.Parse(dtString.Substring(0, 4)), _
                            Integer.Parse(dtString.Substring(4, 2)), _
                            Integer.Parse(dtString.Substring(6, 2)), _
                            Integer.Parse(dtString.Substring(8, 2)), _
                            Integer.Parse(dtString.Substring(10, 2)), _
                            Integer.Parse(dtString.Substring(12, 2)) _
                        )
            Else
                rDate = New Date( _
                           Integer.Parse(dtString.Substring(0, 4)), _
                           Integer.Parse(dtString.Substring(4, 2)), _
                           Integer.Parse(dtString.Substring(6, 2)) _
                       )
            End If

            Return rDate
        End Function
    End Class

    '********************** DateTime Action Class Finished ************************

    '*********************** ErrorHandle Action Class *****************************

    Public NotInheritable Class EventLogging
        Implements IDisposable

        Private _CacheLimit As Integer = 1000
        Private _FlushTimer As Integer = 5
        Private _FlushTimerThread As System.Threading.Timer = Nothing

        Public Sub New()
            Me._LoggingThreadObject = New Object
            Me._LoggingCache = New Generic.SortedDictionary(Of Long, LoggingObject)
        End Sub

        Private ReadOnly Property LoggingPath() As String
            Get
                Dim rString As String = _
                    System.Configuration.ConfigurationManager.AppSettings.Item("LoggingPath")

                If String.IsNullOrEmpty(rString) Then
                    rString = _
                        IO.Path.Combine( _
                            SolidDevelopment.Web.Configurations.ApplicationRoot.FileSystemImplementation, _
                            "XeoraLogs" _
                        )
                End If

                Return rString
            End Get
        End Property

        Private Function PrepareLoggingFileLocation(ByVal LoggingPath As String) As String
            If Not IO.Directory.Exists(LoggingPath) Then IO.Directory.CreateDirectory(LoggingPath)

            Return IO.Path.Combine( _
                            LoggingPath, _
                            String.Format( _
                                "{0}.log", Helpers.DateTime.FormatDateTime( _
                                                Helpers.DateTime.DateTimeFormat.Date _
                                            ).ToString() _
                                    ) _
                                )
        End Function

        Private Function PrepareLogOutput(ByVal LogText As String) As String
            Dim sB As New Text.StringBuilder()
            Dim CurrentDate As Date = Date.Now

            sB.AppendLine("----------------------------------------------------------------------------")
            sB.AppendLine("Event Logging Time --> " & String.Format("{0}.{1}", CurrentDate.ToString(), CurrentDate.Millisecond))
            sB.AppendLine("----------------------------------------------------------------------------")
            sB.Append(LogText)
            sB.AppendLine()

            Return sB.ToString()
        End Function

        Private Shared _QuickAccess As EventLogging = Nothing
        Private Shared ReadOnly Property QuickAccess() As EventLogging
            Get
                If EventLogging._QuickAccess Is Nothing Then _
                    EventLogging._QuickAccess = New EventLogging

                Return EventLogging._QuickAccess
            End Get
        End Property

        Public Overloads Shared Sub WriteToLog(ByVal ex As Exception)
            EventLogging.WriteToLog(ex, Nothing)
        End Sub

        Public Overloads Shared Sub WriteToLog(ByVal ex As Exception, ByVal LoggingPathIdentifier As String)
            If Not ex Is Nothing Then _
                EventLogging.WriteToLog(ex.ToString(), LoggingPathIdentifier)
        End Sub

        Public Overloads Shared Sub WriteToLog(ByVal LogText As String)
            EventLogging.WriteToLog(LogText, Nothing)
        End Sub

        Public Overloads Shared Sub WriteToLog(ByVal LogText As String, ByVal LoggingPathIdentifier As String)
            Dim logObj As New LoggingObject( _
                                    EventLogging.QuickAccess.PrepareLogOutput(LogText), _
                                    EventLogging.QuickAccess.LoggingPath _
                                )

            EventLogging.QuickAccess.WriteToCache(logObj)
        End Sub

        Public Shared Sub SetFlushTimerDuration(ByVal Minute As Integer)
            EventLogging.QuickAccess._FlushTimer = Minute

            If Not EventLogging.QuickAccess._FlushTimerThread Is Nothing Then
                EventLogging.QuickAccess._FlushTimerThread.Dispose()
                EventLogging.QuickAccess._FlushTimerThread = Nothing

                EventLogging.QuickAccess._FlushTimerThread = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf EventLogging.QuickAccess.FlushInternal), Nothing, (EventLogging.QuickAccess._FlushTimer * 60000), 0)
            End If
        End Sub

        Public Shared Sub SetFlushCacheLimit(ByVal CacheLimit As Integer)
            EventLogging.QuickAccess._CacheLimit = CacheLimit
        End Sub

        Public Shared Sub Flush()
            EventLogging.QuickAccess.FlushInternal(Nothing)
        End Sub

        Private Class LoggingObject
            Private _LogText As String
            Private _LoggingPath As String

            Public Sub New(ByVal LogText As String, ByVal LoggingPath As String)
                Me._LogText = LogText
                Me._LoggingPath = LoggingPath
            End Sub

            Public ReadOnly Property LogText() As String
                Get
                    Return Me._LogText
                End Get
            End Property

            Public ReadOnly Property LoggingPath() As String
                Get
                    Return Me._LoggingPath
                End Get
            End Property
        End Class

        Private _LoggingThreadObject As Object
        Private _LoggingCache As Generic.SortedDictionary(Of Long, LoggingObject) = Nothing

        Private Sub WriteToCache(ByVal logObj As LoggingObject)
            Try
                System.Threading.Monitor.Enter(Me._LoggingThreadObject)

                Dim TotalSpan As TimeSpan = _
                    Date.Now.Subtract(New Date(2000, 1, 1, 0, 0, 0))

                Me._LoggingCache.Item(TotalSpan.Ticks + Me._LoggingCache.Count) = logObj

                If Me._LoggingCache.Count >= Me._CacheLimit Then
                    Me.FlushInternal(Nothing)
                Else
                    If Me._FlushTimerThread Is Nothing Then _
                        Me._FlushTimerThread = New System.Threading.Timer( _
                                                                New System.Threading.TimerCallback(AddressOf Me.FlushInternal), _
                                                                Nothing, _
                                                                (Me._FlushTimer * 60000), _
                                                                0 _
                                                            )
                End If
            Finally
                System.Threading.Monitor.Exit(Me._LoggingThreadObject)
            End Try
        End Sub

        Private Sub FlushInternal(ByVal state As Object)
            If Not Me._FlushTimerThread Is Nothing Then
                Me._FlushTimerThread.Dispose()
                Me._FlushTimerThread = Nothing
            End If

            Dim loggingFS As IO.Stream = Nothing, buffer As Byte()

            Try
                System.Threading.Monitor.Enter(Me._LoggingThreadObject)

                For Each key As Long In Me._LoggingCache.Keys
                    Dim objLog As LoggingObject = _
                        Me._LoggingCache.Item(key)

                    buffer = System.Text.Encoding.UTF8.GetBytes(objLog.LogText)

                    loggingFS = New IO.FileStream( _
                                    Me.PrepareLoggingFileLocation(objLog.LoggingPath), _
                                    IO.FileMode.Append, _
                                    IO.FileAccess.Write, _
                                    IO.FileShare.ReadWrite _
                                )
                    loggingFS.Write(buffer, 0, buffer.Length)
                    loggingFS.Close() : loggingFS = Nothing
                Next

                Me._LoggingCache.Clear()
            Catch ex As IO.IOException
                ' Possible reason is file in use. Do nothing and let it run again on next time
            Catch ex As Exception
                Throw ex
            Finally
                If Not loggingFS Is Nothing Then loggingFS.Close()

                System.Threading.Monitor.Exit(Me._LoggingThreadObject)
            End Try

            If Not state Is Nothing AndAlso TypeOf state Is Boolean AndAlso Not CType(state, Boolean) Then _
                Me._FlushTimerThread = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf Me.FlushInternal), Nothing, (Me._FlushTimer * 60000), 0)
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    Me.FlushInternal(True)
                End If

                ' TODO: free shared unmanaged resources
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Protected Overrides Sub Finalize()
            Me.Dispose()
            MyBase.Finalize()
        End Sub
    End Class

    '******************* ErrorHandle Action Class Finished ************************

    '************************ RegistryManager Action Class ***************************

    Public Class RegistryManager
        Private _regSection As RegistryKey
        Private _RegistryArea As RegistryAreas
        Private _AccessPath As String

        Public Enum RegistryAreas As Integer
            ClassesRoot = 0
            CurrentConfig = 1
            CurrentUser = 2
            LocalMachine = 3
            Users = 4
        End Enum

        Public Sub New(ByVal RegistryArea As RegistryAreas)
            Me.New(RegistryArea, Nothing)
        End Sub

        Public Sub New(ByVal RegistryArea As RegistryAreas, ByVal AccessPath As String)
            Me._RegistryArea = RegistryArea
            Me._AccessPath = AccessPath
        End Sub

        Public Property AccessPath() As String
            Get
                Return Me._AccessPath
            End Get
            Set(ByVal Value As String)
                Me._AccessPath = Value
            End Set
        End Property

        Public Property RegistryArea() As RegistryAreas
            Get
                Return Me._RegistryArea
            End Get
            Set(ByVal Value As RegistryAreas)
                Me._RegistryArea = Value
            End Set
        End Property

        Private Sub OpenRegistryArea()
            If Me._AccessPath Is Nothing Then Throw New Exception("Access Path can not be null!")

            Select Case Me._RegistryArea
                Case RegistryAreas.ClassesRoot
                    Try
                        Me._regSection = Registry.ClassesRoot
                    Catch ex As Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.CurrentConfig
                    Try
                        Me._regSection = Registry.CurrentConfig
                    Catch ex As Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.CurrentUser
                    Try
                        Me._regSection = Registry.CurrentUser
                    Catch ex As Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.LocalMachine
                    Try
                        Me._regSection = Registry.LocalMachine
                    Catch ex As Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.Users
                    Try
                        Me._regSection = Registry.Users
                    Catch ex As Exception
                        EventLogging.WriteToLog(ex)
                    End Try
            End Select
        End Sub

        Private Sub CloseRegistryArea()
            Try
                Me._regSection.Close()
            Catch ex As Exception
                EventLogging.WriteToLog(ex)
            End Try
        End Sub

        Public Function CheckIsRegistryExists() As Boolean
            Dim rBoolean As Boolean

            Try
                Me.OpenRegistryArea()

                rBoolean = (Not Me._regSection.OpenSubKey(Me._AccessPath) Is Nothing)
            Catch ex As Exception
                rBoolean = False

                EventLogging.WriteToLog(ex)
            Finally
                Me.CloseRegistryArea()
            End Try

            Return rBoolean
        End Function

        Public Function CheckIsRegistryValueExists(ByVal Key As String) As Boolean
            Dim rBoolean As Boolean

            If Me.CheckIsRegistryExists() Then
                Try
                    Me.OpenRegistryArea()

                    rBoolean = (Not Me._regSection.OpenSubKey(Me._AccessPath).GetValue(Key) Is Nothing)
                Catch ex As Exception
                    rBoolean = False

                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            Else
                rBoolean = False
            End If

            Return rBoolean
        End Function

        Public Function GetRegistryKeyNames() As String()
            Dim rStringList As String() = Nothing

            If Me.CheckIsRegistryValueExists(Me._AccessPath) Then
                Try
                    Me.OpenRegistryArea()

                    rStringList = Me._regSection.OpenSubKey(Me._AccessPath).GetSubKeyNames()
                Catch ex As Exception
                    rStringList = Nothing

                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If

            Return rStringList
        End Function

        Public Function GetRegistryValue(ByVal Key As String) As String
            Dim rString As String = Nothing

            If Me.CheckIsRegistryValueExists(Key) Then
                Try
                    Me.OpenRegistryArea()

                    rString = CType(Me._regSection.OpenSubKey(Me._AccessPath).GetValue(Key), String)
                Catch ex As Exception
                    rString = Nothing

                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If

            Return rString
        End Function

        Public Sub SetRegistryValue(ByVal Key As String, ByVal Value As String)
            If Not Me.CheckIsRegistryExists() Then
                Try
                    Me.OpenRegistryArea()

                    Me._regSection.CreateSubKey(Me._AccessPath)
                Catch ex As Exception
                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If

            Try
                Me.OpenRegistryArea()

                Me._regSection.OpenSubKey(Me._AccessPath, True).SetValue(Key, Value)
            Catch ex As Exception
                EventLogging.WriteToLog(ex)
            Finally
                Me.CloseRegistryArea()
            End Try
        End Sub

        Public Sub DeleteRegistry()
            If Me.CheckIsRegistryExists() Then
                Try
                    Me.OpenRegistryArea()

                    Me._regSection.DeleteSubKey(Me._AccessPath)
                Catch ex As Exception
                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If
        End Sub

        Public Sub DeleteRegistryValue(ByVal Key As String)
            If Me.CheckIsRegistryValueExists(Key) Then
                Try
                    Me.OpenRegistryArea()

                    Me._regSection.OpenSubKey(Me._AccessPath, True).DeleteValue(Key)
                Catch ex As Exception
                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If
        End Sub
    End Class

    '***************** RegistryManager Action Class Finished ***************************

    '************************ OverrideBinder Class ***************************

    Public Class OverrideBinder
        Inherits System.Runtime.Serialization.SerializationBinder

        Private Shared _AssemblyCache As New Generic.Dictionary(Of String, System.Reflection.Assembly)

        Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As System.Type
            Dim typeToDeserialize As Type = Nothing

            Dim sShortAssemblyName As String = _
                assemblyName.Substring(0, assemblyName.IndexOf(","c))

            If OverrideBinder._AssemblyCache.ContainsKey(sShortAssemblyName) Then
                typeToDeserialize = Me.GetDeserializeType(OverrideBinder._AssemblyCache.Item(sShortAssemblyName), typeName)
            Else
                Dim ayAssemblies As System.Reflection.Assembly() = AppDomain.CurrentDomain.GetAssemblies()

                For Each ayAssembly As System.Reflection.Assembly In ayAssemblies
                    If sShortAssemblyName = ayAssembly.FullName.Substring(0, assemblyName.IndexOf(","c)) Then
                        typeToDeserialize = Me.GetDeserializeType(ayAssembly, typeName)

                        OverrideBinder._AssemblyCache.Add(sShortAssemblyName, ayAssembly)

                        Exit For
                    End If
                Next
            End If

            Return typeToDeserialize
        End Function

        Private Function GetDeserializeType(ByVal assembly As System.Reflection.Assembly, typeName As String) As Type
            Dim rTypeToDeserialize As Type = Nothing

            Dim remainAssemblyNames As String() = Nothing
            Dim typeName_L As String = Me.GetTypeFullNames(typeName, remainAssemblyNames)

            Dim tempType As System.Type = _
                assembly.GetType(typeName_L)

            If Not tempType Is Nothing AndAlso tempType.IsGenericType Then
                Dim typeParameters As New Generic.List(Of Type)

                For Each remainAssemblyName As String In remainAssemblyNames
                    Dim eBI As Integer = _
                        remainAssemblyName.LastIndexOf("]"c)
                    Dim qAssemblyName As String, qTypeName As String

                    If eBI = -1 Then
                        qTypeName = remainAssemblyName.Split(","c)(0)
                        qAssemblyName = remainAssemblyName.Substring(qTypeName.Length + 2)
                    Else
                        qTypeName = remainAssemblyName.Substring(0, eBI + 1)
                        qAssemblyName = remainAssemblyName.Substring(eBI + 3)
                    End If

                    typeParameters.Add(Me.BindToType(qAssemblyName, qTypeName))
                Next

                rTypeToDeserialize = tempType.MakeGenericType(typeParameters.ToArray())
            Else
                rTypeToDeserialize = tempType
            End If

            Return rTypeToDeserialize
        End Function

        Private Function GetTypeFullNames(ByVal typeName As String, ByRef remainAssemblyNames As String()) As String
            Dim rString As String

            Dim bI As Integer = typeName.IndexOf("["c, 0)

            If bI = -1 Then
                rString = typeName
                remainAssemblyNames = New String() {}
            Else
                rString = typeName.Substring(0, bI)

                Dim fullNameList_L As New Generic.List(Of String)
                Dim remainFullName As String = _
                    typeName.Substring(bI + 1, typeName.Length - (bI + 1) - 1)

                Dim eI As Integer = 0, bIc As Integer = 0 : bI = 0
                Do
                    bI = remainFullName.IndexOf("["c, bI)

                    If bI > -1 Then
                        eI = remainFullName.IndexOf("]"c, bI + 1)
                        bIc = remainFullName.IndexOf("["c, bI + 1)

                        If bIc > -1 AndAlso bIc < eI Then
                            Do While bIc > -1 AndAlso bIc < eI
                                bIc = remainFullName.IndexOf("["c, bIc + 1)

                                If bIc > -1 AndAlso bIc < eI Then _
                                    eI = remainFullName.IndexOf("]"c, eI + 1)
                            Loop

                            eI = remainFullName.IndexOf("]"c, eI + 1)
                        End If

                        fullNameList_L.Add(remainFullName.Substring(bI + 1, eI - (bI + 1)))

                        bI = eI + 1
                    End If
                Loop Until bI = -1

                remainAssemblyNames = fullNameList_L.ToArray()
            End If

            Return rString
        End Function
    End Class

    '******************* OverrideBinder Class Finished ************************
End Namespace