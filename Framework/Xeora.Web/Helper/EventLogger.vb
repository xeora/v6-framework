Option Strict On

Namespace Xeora.Web.Helper
    Public NotInheritable Class EventLogger
        Implements IDisposable

        Private _CacheLimit As Integer = 100
        Private _FlushCycle As Integer = 5
        Private _FlushTimer As Threading.Timer = Nothing

        Private _ThreadObject As Object
        Private _Cache As Generic.SortedDictionary(Of Long, LogObject)

        Public Sub New()
            Me._ThreadObject = New Object()
            Me._Cache = New Generic.SortedDictionary(Of Long, LogObject)
        End Sub

        Private Shared _Instance As EventLogger = Nothing
        Private Shared ReadOnly Property Instance() As EventLogger
            Get
                If EventLogger._Instance Is Nothing Then _
                    EventLogger._Instance = New EventLogger()

                Return EventLogger._Instance
            End Get
        End Property

        Public Overloads Shared Sub Log(ByVal ex As System.Exception)
            If Not ex Is Nothing Then _
                EventLogger.Log(ex.ToString())
        End Sub

        Public Overloads Shared Sub Log(ByVal Content As String)
            Try
                EventLogger.Instance.WriteToCache(New LogObject(Content))
            Catch ex As System.Exception
                EventLogger.LogToSystemEvent(
                    String.Format(" --- Logging Exception --- {0}{0}{1}{0}{0} --- Original Log Content --- {2}",
                                  Environment.NewLine, ex.ToString(), Content),
                    EventLogEntryType.Error)
            End Try
        End Sub

        Public Shared Sub LogToSystemEvent(ByVal Content As String, ByVal Type As EventLogEntryType)
            Try
                If Not EventLog.SourceExists("XeoraCube") Then EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                EventLog.WriteEntry("XeoraCube", Content, Type)
            Catch ex As System.Exception
                ' Just Handle Exception
            End Try
        End Sub

        Public Shared Sub SetFlushCycle(ByVal Minute As Integer)
            EventLogger.Instance._FlushCycle = Minute

            If Not EventLogger.Instance._FlushTimer Is Nothing Then
                EventLogger.Instance._FlushTimer.Dispose()
                EventLogger.Instance._FlushTimer =
                    New Threading.Timer(
                        New Threading.TimerCallback(AddressOf EventLogger.Instance.FlushInternal),
                        Nothing,
                        (EventLogger.Instance._FlushCycle * 60000),
                        0
                    )
            End If
        End Sub

        Public Shared Sub SetFlushCacheLimit(ByVal CacheLimit As Integer)
            EventLogger.Instance._CacheLimit = CacheLimit
        End Sub

        Public Shared Sub Flush()
            EventLogger.Instance.FlushInternal(Nothing)
        End Sub

        Private Class LogObject
            Private _Content As String
            Private _DateTime As System.DateTime

            Public Sub New(ByVal Content As String)
                Me._Content = Content
                Me._DateTime = System.DateTime.Now
            End Sub

            Public ReadOnly Property Content() As String
                Get
                    Return Me._Content
                End Get
            End Property

            Public ReadOnly Property DateTime() As System.DateTime
                Get
                    Return Me._DateTime
                End Get
            End Property
        End Class

        Private Sub WriteToCache(ByVal logObj As LogObject)
            If Me._FlushTimer Is Nothing Then _
                Me._FlushTimer = New System.Threading.Timer(
                                                        New System.Threading.TimerCallback(AddressOf Me.FlushInternal),
                                                        Nothing,
                                                        (Me._FlushCycle * 60000),
                                                        0
                                                    )

            Threading.Monitor.Enter(Me._ThreadObject)
            Try
                Dim TotalSpan As TimeSpan =
                    Date.Now.Subtract(New Date(2000, 1, 1, 0, 0, 0))

                Me._Cache.Add(TotalSpan.Ticks, logObj)

                ' wait to prevent Ticks equality
                Threading.Thread.Sleep(1)
            Finally
                Threading.Monitor.Exit(Me._ThreadObject)
            End Try

            If Me._Cache.Count >= Me._CacheLimit Then _
                Threading.ThreadPool.QueueUserWorkItem(New Threading.WaitCallback(AddressOf Me.FlushInternal))
        End Sub

        Private Shared _OutputLocation As String = Nothing
        Private Sub FlushInternal(ByVal state As Object)
            If Not Me._FlushTimer Is Nothing Then
                Me._FlushTimer.Dispose()
                Me._FlushTimer = Nothing
            End If

            If String.IsNullOrEmpty(EventLogger._OutputLocation) Then
                Try
                    Dim RequestAsm As Reflection.Assembly, objRequest As Type

                    RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
                    objRequest = RequestAsm.GetType("Xeora.Web.Handler.RemoteInvoke", False, True)

                    Dim XeoraSettingsObject As Object =
                        objRequest.InvokeMember("XeoraSettings", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)

                    Dim WorkingObject As Object =
                        XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, XeoraSettingsObject, Nothing)

                    EventLogger._OutputLocation = CType(WorkingObject.GetType().InvokeMember("LoggingPath", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)
                Catch ex As System.Exception
                    EventLogger.LogToSystemEvent(ex.ToString(), EventLogEntryType.Error)
                End Try
            End If

            Dim LoggedKeys As New Generic.List(Of Long)
            Threading.Monitor.Enter(Me._ThreadObject)
            Try
                For Each key As Long In Me._Cache.Keys
                    Me.WriteToFile(
                        Me._Cache.Item(key))

                    LoggedKeys.Add(key)
                Next
            Catch ex As IO.IOException
                ' Possible reason is file in use. Do nothing and let it run again on next time
            Catch ex As System.Exception
                EventLogger.LogToSystemEvent(ex.ToString(), EventLogEntryType.Error)
            Finally
                If Me._Cache.Count = LoggedKeys.Count Then
                    Me._Cache.Clear()
                Else
                    For Each Key As Long In LoggedKeys
                        If Me._Cache.ContainsKey(Key) Then _
                            Me._Cache.Remove(Key)
                    Next
                End If

                Threading.Monitor.Exit(Me._ThreadObject)
            End Try
        End Sub

        Private Function PrepareOutputFileLocation(ByVal OutputLocation As String, ByVal LogDateTime As System.DateTime) As String
            If Not IO.Directory.Exists(OutputLocation) Then IO.Directory.CreateDirectory(OutputLocation)

            Return IO.Path.Combine(
                            OutputLocation,
                            String.Format(
                                "{0}.log", DateTime.Format(LogDateTime, True).ToString()
                                    )
                                )
        End Function

        Private Sub WriteToFile(ByVal logObj As LogObject)
            If logObj Is Nothing Then Exit Sub

            Dim sB As New Text.StringBuilder()

            sB.AppendLine("----------------------------------------------------------------------------")
            sB.AppendLine("Event Time --> " & String.Format("{0}.{1}", logObj.DateTime.ToString(), logObj.DateTime.Millisecond))
            sB.AppendLine("----------------------------------------------------------------------------")
            sB.Append(logObj.Content)
            sB.AppendLine()

            If String.IsNullOrEmpty(EventLogger._OutputLocation) Then
                EventLogger.LogToSystemEvent(sB.ToString(), EventLogEntryType.FailureAudit)
            Else
                Dim logFS As IO.Stream = Nothing
                Try
                    Dim buffer As Byte() = Text.Encoding.UTF8.GetBytes(sB.ToString())

                    logFS = New IO.FileStream(
                                    Me.PrepareOutputFileLocation(EventLogger._OutputLocation, logObj.DateTime),
                                    IO.FileMode.Append,
                                    IO.FileAccess.Write,
                                    IO.FileShare.ReadWrite
                                )
                    logFS.Write(buffer, 0, buffer.Length)
                Finally
                    If Not logFS Is Nothing Then logFS.Close()
                End Try
            End If
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    Me.FlushInternal(Nothing)
                End If
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
End Namespace