Option Strict On

Namespace Xeora.Web.Helper
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
                Dim rString As String =
                    Configuration.ConfigurationManager.AppSettings.Item("LoggingPath")

                If String.IsNullOrEmpty(rString) Then
                    rString =
                        IO.Path.Combine(
                            [Shared].Configurations.ApplicationRoot.FileSystemImplementation,
                            "XeoraLogs"
                        )
                End If

                Return rString
            End Get
        End Property

        Private Function PrepareLoggingFileLocation(ByVal LoggingPath As String) As String
            If Not IO.Directory.Exists(LoggingPath) Then IO.Directory.CreateDirectory(LoggingPath)

            Return IO.Path.Combine(
                            LoggingPath,
                            String.Format(
                                "{0}.log", [Date].Format(
                                                [Date].DateFormats.OnlyDate
                                            ).ToString()
                                    )
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

        Public Overloads Shared Sub WriteToLog(ByVal ex As System.Exception)
            EventLogging.WriteToLog(ex, Nothing)
        End Sub

        Public Overloads Shared Sub WriteToLog(ByVal ex As System.Exception, ByVal LoggingPathIdentifier As String)
            If Not ex Is Nothing Then _
                EventLogging.WriteToLog(ex.ToString(), LoggingPathIdentifier)
        End Sub

        Public Overloads Shared Sub WriteToLog(ByVal LogText As String)
            EventLogging.WriteToLog(LogText, Nothing)
        End Sub

        Public Overloads Shared Sub WriteToLog(ByVal LogText As String, ByVal LoggingPathIdentifier As String)
            Dim logObj As New LoggingObject(
                                    EventLogging.QuickAccess.PrepareLogOutput(LogText),
                                    EventLogging.QuickAccess.LoggingPath
                                )

            Try
                EventLogging.QuickAccess.WriteToCache(logObj)
            Catch ex As system.Exception
                Try
                    If Not EventLog.SourceExists("XeoraCube") Then EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                    EventLog.WriteEntry("XeoraCube", LogText, EventLogEntryType.Error)
                Catch SystemLoggingEx As System.Exception
                    ' Just Handle Exception
                End Try
            End Try
        End Sub

        Public Shared Sub SetFlushTimerDuration(ByVal Minute As Integer)
            EventLogging.QuickAccess._FlushTimer = Minute

            If Not EventLogging.QuickAccess._FlushTimerThread Is Nothing Then
                EventLogging.QuickAccess._FlushTimerThread.Dispose()
                EventLogging.QuickAccess._FlushTimerThread = Nothing

                EventLogging.QuickAccess._FlushTimerThread = New Threading.Timer(New System.Threading.TimerCallback(AddressOf EventLogging.QuickAccess.FlushInternal), Nothing, (EventLogging.QuickAccess._FlushTimer * 60000), 0)
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
            Threading.Monitor.Enter(Me._LoggingThreadObject)
            Try
                Dim TotalSpan As TimeSpan =
                    Date.Now.Subtract(New Date(2000, 1, 1, 0, 0, 0))

                Me._LoggingCache.Item(TotalSpan.Ticks + Me._LoggingCache.Count) = logObj

                If Me._LoggingCache.Count >= Me._CacheLimit Then
                    Me.FlushInternal(Nothing)
                Else
                    If Me._FlushTimerThread Is Nothing Then _
                        Me._FlushTimerThread = New System.Threading.Timer(
                                                                New System.Threading.TimerCallback(AddressOf Me.FlushInternal),
                                                                Nothing,
                                                                (Me._FlushTimer * 60000),
                                                                0
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

            System.Threading.Monitor.Enter(Me._LoggingThreadObject)
            Try
                For Each key As Long In Me._LoggingCache.Keys
                    Dim objLog As LoggingObject =
                        Me._LoggingCache.Item(key)

                    buffer = System.Text.Encoding.UTF8.GetBytes(objLog.LogText)

                    loggingFS = New IO.FileStream(
                                    Me.PrepareLoggingFileLocation(objLog.LoggingPath),
                                    IO.FileMode.Append,
                                    IO.FileAccess.Write,
                                    IO.FileShare.ReadWrite
                                )
                    loggingFS.Write(buffer, 0, buffer.Length)
                    loggingFS.Close() : loggingFS = Nothing
                Next

                Me._LoggingCache.Clear()
            Catch ex As IO.IOException
                ' Possible reason is file in use. Do nothing and let it run again on next time
            Catch ex As System.Exception
                Throw
            Finally
                If Not loggingFS Is Nothing Then loggingFS.Close()

                Threading.Monitor.Exit(Me._LoggingThreadObject)
            End Try

            If Not state Is Nothing AndAlso TypeOf state Is Boolean AndAlso Not CType(state, Boolean) Then _
                Me._FlushTimerThread = New Threading.Timer(New Threading.TimerCallback(AddressOf Me.FlushInternal), Nothing, (Me._FlushTimer * 60000), 0)
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    Me.FlushInternal(True)
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