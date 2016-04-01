Option Strict On

Namespace Xeora.Web.Site.Service
    Public Class ScheduledTasks
        Inherits MarshalByRefObject
        Implements [Shared].Service.IScheduledTasks

        Private _ScheduledTasksID As String
        Private _RemoteScheduledTasksService As System.Runtime.Remoting.Channels.Tcp.TcpServerChannel = Nothing
        Private _IsScheduledTasksHost As Boolean

        Private _ScheduledTasks As New Hashtable

        Private _CheckProgressSync As New Object()
        Private _CheckIsInProgress As Boolean

        Public Sub New()
            If [Shared].Configurations.ScheduledTasksServicePort = -1 Then _
                Throw New System.Exception("Scheduled Tasks Service is not configured! Please assign a port number in app.config file.")

            Me._IsScheduledTasksHost = False
            Me._ScheduledTasksID = String.Format("ST_{0}", [Shared].Configurations.WorkingPath.WorkingPathID)

            Me.CreateScheduledTasksHostForRemoteConnections()
        End Sub

        Private Sub CreateScheduledTasksHostForRemoteConnections()
RETRYREMOTEBIND:
            Dim RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
            Dim RemoteScheduledTasks As [Shared].Service.IScheduledTasks =
                Me.CreateConnectionToRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)
            Me.DestroyConnectionFromRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)

            If RemoteScheduledTasks Is Nothing Then
                Me._ScheduledTasks = Hashtable.Synchronized(New Hashtable())

                Try
                    Dim serverProvider As New Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider
                    serverProvider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full

                    Me._RemoteScheduledTasksService =
                        New Runtime.Remoting.Channels.Tcp.TcpServerChannel(
                            Me._ScheduledTasksID, [Shared].Configurations.ScheduledTasksServicePort, serverProvider)

                    ' Register RemoteVariablePoolService to Remoting Service
                    Runtime.Remoting.Channels.ChannelServices.RegisterChannel(Me._RemoteScheduledTasksService, True)

                    ' Register VariablePool's Service Name
                    Runtime.Remoting.RemotingServices.Marshal(Me, Me._ScheduledTasksID, GetType(ScheduledTasks))
                    'System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType( _
                    '    GetType(VariablePoolOperationClass), _
                    '    VariablePoolID, _
                    '    Runtime.Remoting.WellKnownObjectMode.Singleton _
                    ')
                    Me._IsScheduledTasksHost = True
                Catch ex As System.Exception
                    Try
                        EventLog.WriteEntry("XeoraCube", ex.ToString(), EventLogEntryType.Error)
                    Catch exSub As System.Exception
                        ' Just Handle Request
                    End Try

                    Threading.Thread.Sleep(1000)

                    GoTo RETRYREMOTEBIND
                End Try
            End If
        End Sub

        Private Function CreateConnectionToRemoteScheduledTasks(ByRef RemoteScheduledTasksServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel) As [Shared].Service.IScheduledTasks
            Dim rRemoteScheduledTasks As [Shared].Service.IScheduledTasks = Nothing

            Dim ScheduledTasksExists As Boolean = False

            Try
                Dim clientProvider As New Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider

                RemoteScheduledTasksServiceConnection =
                    New Runtime.Remoting.Channels.Tcp.TcpClientChannel(Guid.NewGuid().ToString(), clientProvider)
                Runtime.Remoting.Channels.ChannelServices.RegisterChannel(RemoteScheduledTasksServiceConnection, True)

                rRemoteScheduledTasks =
                    CType(
                        Activator.GetObject(
                            GetType([Shared].Service.IScheduledTasks),
                            String.Format("tcp://{0}:{1}/{2}", Environment.MachineName, [Shared].Configurations.ScheduledTasksServicePort, Me._ScheduledTasksID)
                        ),
                        [Shared].Service.IScheduledTasks
                    )

                If Not rRemoteScheduledTasks Is Nothing Then
                    Dim PingError As Boolean = True
                    Dim PingThread As New Threading.Thread(Sub()
                                                               Try
                                                                   rRemoteScheduledTasks.PingToRemoteEndPoint()

                                                                   PingError = False
                                                               Catch ex As System.Exception
                                                                   PingError = True
                                                               End Try
                                                           End Sub)
                    PingThread.Start()
                    PingThread.Join(2000)

                    If PingError Then
                        PingThread.Abort()

                        Throw New Net.Sockets.SocketException()
                    End If
                End If

                ScheduledTasksExists = True
            Catch ex As System.Exception
                ScheduledTasksExists = False
            Finally
                If Not ScheduledTasksExists Then
                    rRemoteScheduledTasks = Nothing

                    If Not RemoteScheduledTasksServiceConnection Is Nothing Then
                        Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteScheduledTasksServiceConnection)

                        RemoteScheduledTasksServiceConnection = Nothing
                    End If
                End If
            End Try

            Return rRemoteScheduledTasks
        End Function

        Private Sub DestroyConnectionFromRemoteScheduledTasks(ByRef RemoteScheduledTasksServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel)
            Try
                If Not RemoteScheduledTasksServiceConnection Is Nothing Then _
                    Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteScheduledTasksServiceConnection)
            Catch ex As System.Exception
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
                ExecutionTime = Helper.Date.Format(
                                    Date.Now, Helper.Date.DateFormats.DateWithTime)

                Threading.Monitor.Enter(Me._ScheduledTasks.SyncRoot)
                Try
                    If Me._ScheduledTasks.ContainsKey(ExecutionTime) Then
                        ' We run scheduled tasks in a thread to avoid late timming for next queue taskinfos (If Exists)
                        Threading.ThreadPool.QueueUserWorkItem(
                            New Threading.WaitCallback(AddressOf Me.ExecuteScheduledCallBacks),
                            Me._ScheduledTasks.Item(ExecutionTime)
                        )

                        Me._ScheduledTasks.Remove(ExecutionTime)
                    End If
                Finally
                    Threading.Monitor.Exit(Me._ScheduledTasks.SyncRoot)
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
            Catch ex As System.Exception
                Helper.EventLogging.WriteToLog(ex)
            End Try
        End Sub

        Public Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As [Shared].Service.IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As Date) As String Implements [Shared].Service.IScheduledTasks.RegisterScheduleTask
            Dim rScheduleID As String = String.Empty

            If Not Me._IsScheduledTasksHost Then
                Dim RemoteScheduledTasksServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                Dim RemoteScheduledTasks As [Shared].Service.IScheduledTasks =
                    Me.CreateConnectionToRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)

                Try
                    rScheduleID = RemoteScheduledTasks.RegisterScheduleTask(ScheduleCallBack, params, TaskExecutionTime)
                Catch ex As System.Exception
                    Me.CreateScheduledTasksHostForRemoteConnections()

                    rScheduleID = Me.RegisterScheduleTask(ScheduleCallBack, params, TaskExecutionTime)
                End Try

                Me.DestroyConnectionFromRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)
            Else
                Dim ExecutionTime As Long =
                    Helper.Date.Format(TaskExecutionTime, Helper.Date.DateFormats.DateWithTime)
                Dim TaskInfo As New TaskInfos(ScheduleCallBack, params, TaskExecutionTime)

                Threading.Monitor.Enter(Me._ScheduledTasks.SyncRoot)
                Try
                    If Me._ScheduledTasks.ContainsKey(ExecutionTime) Then
                        Dim sTasks As TaskInfos() =
                            CType(Me._ScheduledTasks.Item(ExecutionTime), TaskInfos())

                        Array.Resize(sTasks, sTasks.Length + 1)
                        sTasks.SetValue(TaskInfo, sTasks.Length - 1)

                        Me._ScheduledTasks.Item(ExecutionTime) = sTasks
                    Else
                        Me._ScheduledTasks.Add(ExecutionTime, New TaskInfos() {TaskInfo})
                    End If
                Finally
                    Threading.Monitor.Exit(Me._ScheduledTasks.SyncRoot)
                End Try

                Threading.ThreadPool.QueueUserWorkItem(
                    New Threading.WaitCallback(AddressOf Me.CheckScheduledTasks))

                rScheduleID = TaskInfo.ScheduleID
            End If

            Return rScheduleID
        End Function

        Public Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As [Shared].Service.IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As System.TimeSpan) As String Implements [Shared].Service.IScheduledTasks.RegisterScheduleTask
            Return Me.RegisterScheduleTask(ScheduleCallBack, params, Date.Now.Add(TaskExecutionTime))
        End Function

        Public Sub UnRegisterScheduleTask(ByVal ScheduleID As String) Implements [Shared].Service.IScheduledTasks.UnRegisterScheduleTask
            If Not Me._IsScheduledTasksHost Then
                Dim RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                Dim RemoteScheduledTasks As [Shared].Service.IScheduledTasks =
                    Me.CreateConnectionToRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)

                Try
                    RemoteScheduledTasks.UnRegisterScheduleTask(ScheduleID)
                Catch ex As System.Exception
                    Me.CreateScheduledTasksHostForRemoteConnections()

                    Me.UnRegisterScheduleTask(ScheduleID)
                End Try

                Me.DestroyConnectionFromRemoteScheduledTasks(RemoteScheduledTasksServiceConnection)
            Else
                Threading.Monitor.Enter(Me._ScheduledTasks.SyncRoot)
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
                    Threading.Monitor.Exit(Me._ScheduledTasks.SyncRoot)
                End Try
            End If
        End Sub

        Public Function PingToRemoteEndPoint() As Boolean Implements [Shared].Service.IScheduledTasks.PingToRemoteEndPoint
            Return True
        End Function

        Public Overrides Function InitializeLifetimeService() As Object
            Return Nothing
        End Function

        Protected Overrides Sub Finalize()
            Try
                If Not Me._RemoteScheduledTasksService Is Nothing Then _
                    Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(Me._RemoteScheduledTasksService)
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try

            MyBase.Finalize()
        End Sub

        Private Class TaskInfos
            Private _ScheduleID As Guid
            Private _ScheduleCallBack As [Shared].Service.IScheduledTasks.ScheduleTaskHandler
            Private _ScheduleCallBackParams As Object()
            Private _TaskExecutionTime As Date

            Public Sub New(ByVal ScheduleCallBack As [Shared].Service.IScheduledTasks.ScheduleTaskHandler, ByVal ScheduleCallBackParams As Object(), ByVal TaskExecutionTime As Date)
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

            Public ReadOnly Property ScheduleCallBack() As [Shared].Service.IScheduledTasks.ScheduleTaskHandler
                Get
                    Return Me._ScheduleCallBack
                End Get
            End Property

            Public ReadOnly Property ScheduleCallBackParams() As Object()
                Get
                    Return Me._ScheduleCallBackParams
                End Get
            End Property

            Public ReadOnly Property TaskExecutionTime() As Date
                Get
                    Return Me._TaskExecutionTime
                End Get
            End Property
        End Class
    End Class
End Namespace
