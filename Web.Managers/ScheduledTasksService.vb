Option Strict On

Namespace SolidDevelopment.Web.Managers
    Public Class ScheduledTasksOperationClass
        Inherits MarshalByRefObject
        Implements PGlobals.Execution.IScheduledTasks

        Private _ScheduledTasksID As String
        Private _RemoteScheduledTasksService As System.Runtime.Remoting.Channels.Tcp.TcpServerChannel = Nothing
        Private _IsScheduledTasksHost As Boolean

        Private _ScheduledTasks As New Hashtable

        Private _CheckProgressSync As New Object()
        Private _CheckIsInProgress As Boolean

        Public Sub New()
            If Configurations.ScheduledTasksServicePort = -1 Then _
                Throw New Exception("Scheduled Tasks Service is not configured! Please assign a port number in app.config file.")

            Me._IsScheduledTasksHost = False
            Me._ScheduledTasksID = String.Format("ST_{0}", SolidDevelopment.Web.Configurations.WorkingPath.WorkingPathID)

            Me.CreateScheduledTasksHostForRemoteConnections()
        End Sub

        Private Sub CreateScheduledTasksHostForRemoteConnections()
RETRYREMOTEBIND:
            Dim RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
            Dim RemoteScheduledTasks As PGlobals.Execution.IScheduledTasks = _
                Me.CreateConnectionToRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)
            Me.DestroyConnectionFromRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)

            If RemoteScheduledTasks Is Nothing Then
                Me._ScheduledTasks = Hashtable.Synchronized(New Hashtable())

                Try
                    Dim serverProvider As New System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider
                    serverProvider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full

                    Me._RemoteScheduledTasksService = _
                        New System.Runtime.Remoting.Channels.Tcp.TcpServerChannel( _
                            Me._ScheduledTasksID, Configurations.ScheduledTasksServicePort, serverProvider)

                    ' Register RemoteVariablePoolService to Remoting Service
                    System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(Me._RemoteScheduledTasksService, True)

                    ' Register VariablePool's Service Name
                    System.Runtime.Remoting.RemotingServices.Marshal(Me, Me._ScheduledTasksID, GetType(ScheduledTasksOperationClass))
                    'System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType( _
                    '    GetType(VariablePoolOperationClass), _
                    '    VariablePoolID, _
                    '    Runtime.Remoting.WellKnownObjectMode.Singleton _
                    ')
                    Me._IsScheduledTasksHost = True
                Catch ex As Exception
                    Try
                        System.Diagnostics.EventLog.WriteEntry("XeoraCube", ex.ToString(), EventLogEntryType.Error)
                    Catch exSub As Exception
                        ' Just Handle Request
                    End Try

                    Threading.Thread.Sleep(1000)

                    GoTo RETRYREMOTEBIND
                End Try
            End If
        End Sub

        Private Function CreateConnectionToRemoteScheduledTasks(ByRef RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel) As PGlobals.Execution.IScheduledTasks
            Dim rRemoteScheduledTasks As PGlobals.Execution.IScheduledTasks = Nothing

            Dim ScheduledTasksExists As Boolean = False

            Try
                Dim clientProvider As New System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider

                RemoteScheduledTasksServiceConnection = _
                    New System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(Guid.NewGuid().ToString(), clientProvider)
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(RemoteScheduledTasksServiceConnection, True)

                rRemoteScheduledTasks = _
                    CType( _
                        Activator.GetObject( _
                            GetType(PGlobals.Execution.IScheduledTasks), _
                            String.Format("tcp://{0}:{1}/{2}", System.Environment.MachineName, Configurations.ScheduledTasksServicePort, Me._ScheduledTasksID) _
                        ),  _
                        PGlobals.Execution.IScheduledTasks _
                    )

                If Not rRemoteScheduledTasks Is Nothing Then
                    Dim PingError As Boolean = True
                    Dim PingThread As New Threading.Thread(Sub()
                                                               Try
                                                                   rRemoteScheduledTasks.PingToRemoteEndPoint()

                                                                   PingError = False
                                                               Catch ex As Exception
                                                                   PingError = True
                                                               End Try
                                                           End Sub)
                    PingThread.Start()
                    PingThread.Join(2000)

                    If PingError Then
                        PingThread.Abort()

                        Throw New Exception()
                    End If
                End If

                ScheduledTasksExists = True
            Catch ex As Exception
                ScheduledTasksExists = False
            Finally
                If Not ScheduledTasksExists Then
                    rRemoteScheduledTasks = Nothing

                    If Not RemoteScheduledTasksServiceConnection Is Nothing Then
                        System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteScheduledTasksServiceConnection)

                        RemoteScheduledTasksServiceConnection = Nothing
                    End If
                End If
            End Try

            Return rRemoteScheduledTasks
        End Function

        Private Sub DestroyConnectionFromRemoteScheduledTasks(ByRef RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel)
            Try
                If Not RemoteScheduledTasksServiceConnection Is Nothing Then _
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteScheduledTasksServiceConnection)
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try
        End Sub

        Private Sub CheckScheduledTasks(ByVal state As Object)
            SyncLock Me._CheckProgressSync
                If Me._CheckIsInProgress Then Exit Sub
            End SyncLock

            SyncLock Me._CheckProgressSync
                Me._CheckIsInProgress = True
            End SyncLock

            Dim ExecutionTime As Long
            Do
                ExecutionTime = SolidDevelopment.Web.Helpers.DateTime.FormatDateTime( _
                                    Date.Now, SolidDevelopment.Web.Helpers.DateTime.DateTimeFormat.DateTime)

                System.Threading.Monitor.Enter(Me._ScheduledTasks.SyncRoot)
                Try
                    If Me._ScheduledTasks.ContainsKey(ExecutionTime) Then
                        ' We run scheduled tasks in a thread to avoid late timming for next queue taskinfos (If Exists)
                        System.Threading.ThreadPool.QueueUserWorkItem( _
                            New System.Threading.WaitCallback(AddressOf Me.ExecuteScheduledCallBacks), _
                            Me._ScheduledTasks.Item(ExecutionTime) _
                        )

                        Me._ScheduledTasks.Remove(ExecutionTime)
                    End If
                Finally
                    System.Threading.Monitor.Exit(Me._ScheduledTasks.SyncRoot)
                End Try

                If Me._ScheduledTasks.Count = 0 Then Exit Do

                Threading.Thread.Sleep(1000)
            Loop While True

            SyncLock Me._CheckProgressSync
                Me._CheckIsInProgress = False
            End SyncLock
        End Sub

        Private Sub ExecuteScheduledCallBacks(ByVal TaskInfos As Object)
            Dim sTasks As TaskInfos() = CType(TaskInfos, TaskInfos())

            For Each tI As TaskInfos In sTasks
                tI.ScheduleCallBack.BeginInvoke(tI.ScheduleCallBackParams, New AsyncCallback(AddressOf Me.EndScheduleCallBack), tI)
            Next
        End Sub

        Private Sub EndScheduleCallBack(ByVal AsyncResult As IAsyncResult)
            Try
                CType(AsyncResult.AsyncState, TaskInfos).ScheduleCallBack.EndInvoke(AsyncResult)
            Catch ex As Exception
                SolidDevelopment.Web.Helpers.EventLogging.WriteToLog(ex)
            End Try
        End Sub

        Public Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As PGlobals.Execution.IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As Date) As String Implements PGlobals.Execution.IScheduledTasks.RegisterScheduleTask
            Dim rScheduleID As String = String.Empty

            If Not Me._IsScheduledTasksHost Then
                Dim RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                Dim RemoteScheduledTasks As PGlobals.Execution.IScheduledTasks = _
                    Me.CreateConnectionToRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)

                Try
                    rScheduleID = RemoteScheduledTasks.RegisterScheduleTask(ScheduleCallBack, params, TaskExecutionTime)
                Catch ex As Exception
                    Me.CreateScheduledTasksHostForRemoteConnections()

                    rScheduleID = Me.RegisterScheduleTask(ScheduleCallBack, params, TaskExecutionTime)
                End Try

                Me.DestroyConnectionFromRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)
            Else
                Dim ExecutionTime As Long = _
                    SolidDevelopment.Web.Helpers.DateTime.FormatDateTime(TaskExecutionTime, SolidDevelopment.Web.Helpers.DateTime.DateTimeFormat.DateTime)
                Dim TaskInfo As New TaskInfos(ScheduleCallBack, params, TaskExecutionTime)

                System.Threading.Monitor.Enter(Me._ScheduledTasks.SyncRoot)
                Try
                    If Me._ScheduledTasks.ContainsKey(ExecutionTime) Then
                        Dim sTasks As TaskInfos() = _
                            CType(Me._ScheduledTasks.Item(ExecutionTime), TaskInfos())

                        Array.Resize(sTasks, sTasks.Length + 1)
                        sTasks.SetValue(TaskInfo, sTasks.Length - 1)

                        Me._ScheduledTasks.Item(ExecutionTime) = sTasks
                    Else
                        Me._ScheduledTasks.Add(ExecutionTime, New TaskInfos() {TaskInfo})
                    End If
                Finally
                    System.Threading.Monitor.Exit(Me._ScheduledTasks.SyncRoot)
                End Try

                System.Threading.ThreadPool.QueueUserWorkItem( _
                    New System.Threading.WaitCallback(AddressOf Me.CheckScheduledTasks))

                rScheduleID = TaskInfo.ScheduleID
            End If

            Return rScheduleID
        End Function

        Public Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As PGlobals.Execution.IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As System.TimeSpan) As String Implements PGlobals.Execution.IScheduledTasks.RegisterScheduleTask
            Return Me.RegisterScheduleTask(ScheduleCallBack, params, Date.Now.Add(TaskExecutionTime))
        End Function

        Public Sub UnRegisterScheduleTask(ByVal ScheduleID As String) Implements PGlobals.Execution.IScheduledTasks.UnRegisterScheduleTask
            If Not Me._IsScheduledTasksHost Then
                Dim RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                Dim RemoteScheduledTasks As PGlobals.Execution.IScheduledTasks = _
                    Me.CreateConnectionToRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)

                Try
                    RemoteScheduledTasks.UnRegisterScheduleTask(ScheduleID)
                Catch ex As Exception
                    Me.CreateScheduledTasksHostForRemoteConnections()

                    Me.UnRegisterScheduleTask(ScheduleID)
                End Try

                Me.DestroyConnectionFromRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)
            Else
                System.Threading.Monitor.Enter(Me._ScheduledTasks.SyncRoot)
                Try
                    Dim dEnumerator As IDictionaryEnumerator = Me._ScheduledTasks.GetEnumerator()
                    Dim sTaskList As Generic.List(Of TaskInfos) = Nothing, sTasks As TaskInfos()

                    Dim IsExists As Boolean = False
                    Do While dEnumerator.MoveNext()
                        sTasks = CType(dEnumerator.Value, TaskInfos())

                        IsExists = False : sTaskList = New Generic.List(Of TaskInfos)
                        For Each tI As TaskInfos In sTasks
                            If String.Compare(tI.ScheduleID, ScheduleID) = 0 Then IsExists = True Else sTaskList.Add(tI)
                        Next

                        If IsExists Then
                            If sTaskList.Count = 0 Then
                                Me._ScheduledTasks.Remove(CType(dEnumerator.Key, Long))
                            Else
                                Me._ScheduledTasks.Item(CType(dEnumerator.Key, Long)) = sTaskList.ToArray()
                            End If

                            Exit Do
                        End If
                    Loop
                Finally
                    System.Threading.Monitor.Exit(Me._ScheduledTasks.SyncRoot)
                End Try
            End If
        End Sub

        Public Function PingToRemoteEndPoint() As Boolean Implements PGlobals.Execution.IScheduledTasks.PingToRemoteEndPoint
            Return True
        End Function

        Public Overrides Function InitializeLifetimeService() As Object
            Return Nothing
        End Function

        Protected Overrides Sub Finalize()
            Try
                If Not Me._RemoteScheduledTasksService Is Nothing Then _
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(Me._RemoteScheduledTasksService)
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try

            MyBase.Finalize()
        End Sub

        Private Class TaskInfos
            Private _ScheduleID As Guid
            Private _ScheduleCallBack As PGlobals.Execution.IScheduledTasks.ScheduleTaskHandler
            Private _ScheduleCallBackParams As Object()
            Private _TaskExecutionTime As DateTime

            Public Sub New(ByVal ScheduleCallBack As PGlobals.Execution.IScheduledTasks.ScheduleTaskHandler, ByVal ScheduleCallBackParams As Object(), ByVal TaskExecutionTime As DateTime)
                Me._ScheduleID = Guid.NewGuid()
                Me._ScheduleCallBack = ScheduleCallBack
                Me._ScheduleCallBackParams = ScheduleCallBackParams
                Me._TaskExecutionTime = TaskExecutionTime
            End Sub

            Public ReadOnly Property ScheduleID() As String
                Get
                    Return Me._ScheduleID.ToString()
                End Get
            End Property

            Public ReadOnly Property ScheduleCallBack() As PGlobals.Execution.IScheduledTasks.ScheduleTaskHandler
                Get
                    Return Me._ScheduleCallBack
                End Get
            End Property

            Public ReadOnly Property ScheduleCallBackParams() As Object()
                Get
                    Return Me._ScheduleCallBackParams
                End Get
            End Property

            Public ReadOnly Property TaskExecutionTime() As DateTime
                Get
                    Return Me._TaskExecutionTime
                End Get
            End Property
        End Class
    End Class
End Namespace
