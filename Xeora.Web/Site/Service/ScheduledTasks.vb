Option Strict On

Namespace Xeora.Web.Site.Service
    Public Class ScheduledTasks
        Inherits MarshalByRefObject
        Implements [Shared].Service.IScheduledTasks

        Private _TasksID As String
        Private _RemoteTasksService As System.Runtime.Remoting.Channels.Tcp.TcpServerChannel = Nothing
        Private _IsTasksHost As Boolean

        Private _Tasks As New Hashtable

        Private _CheckProgressSync As New Object()
        Private _CheckIsInProgress As Boolean

        Public Sub New()
            If [Shared].Configurations.ScheduledTasksServicePort = -1 Then _
                Throw New System.Exception("Scheduled Tasks Service is not configured! Please assign a port number in app.config file.")

            Me._IsTasksHost = False
            Me._TasksID = String.Format("ST_{0}", [Shared].Configurations.WorkingPath.WorkingPathID)

            Me.CreateTasksHostForRemoteConnections()
        End Sub

        Private Sub CreateTasksHostForRemoteConnections()
RETRYREMOTEBIND:
            Dim RemoteScheduledTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
            Dim RemoteScheduledTasks As [Shared].Service.IScheduledTasks =
                Me.CreateConnectionToRemoteTasks(RemoteScheduledTasksServiceConnection)
            Me.DestroyConnectionFromRemoteTasks(RemoteScheduledTasksServiceConnection)

            If RemoteScheduledTasks Is Nothing Then
                Me._Tasks = Hashtable.Synchronized(New Hashtable())

                Try
                    Dim serverProvider As New Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider
                    serverProvider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full

                    Me._RemoteTasksService =
                        New Runtime.Remoting.Channels.Tcp.TcpServerChannel(
                            Me._TasksID, [Shared].Configurations.ScheduledTasksServicePort, serverProvider)

                    ' Register RemoteVariablePoolService to Remoting Service
                    Runtime.Remoting.Channels.ChannelServices.RegisterChannel(Me._RemoteTasksService, True)

                    ' Register VariablePool's Service Name
                    Runtime.Remoting.RemotingServices.Marshal(Me, Me._TasksID, GetType(ScheduledTasks))
                    'System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType( _
                    '    GetType(VariablePoolOperationClass), _
                    '    VariablePoolID, _
                    '    Runtime.Remoting.WellKnownObjectMode.Singleton _
                    ')
                    Me._IsTasksHost = True
                Catch ex As System.Exception
                    Helper.EventLogger.Log(ex.ToString())

                    Threading.Thread.Sleep(1000)

                    GoTo RETRYREMOTEBIND
                End Try
            End If
        End Sub

        Private Function CreateConnectionToRemoteTasks(ByRef RemoteTasksServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel) As [Shared].Service.IScheduledTasks
            Dim rRemoteTasks As [Shared].Service.IScheduledTasks = Nothing

            Dim TasksExists As Boolean = False

            Try
                Dim clientProvider As New Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider

                RemoteTasksServiceConnection =
                    New Runtime.Remoting.Channels.Tcp.TcpClientChannel(Guid.NewGuid().ToString(), clientProvider)
                Runtime.Remoting.Channels.ChannelServices.RegisterChannel(RemoteTasksServiceConnection, True)

                rRemoteTasks =
                    CType(
                        Activator.GetObject(
                            GetType([Shared].Service.IScheduledTasks),
                            String.Format("tcp://{0}:{1}/{2}", Environment.MachineName, [Shared].Configurations.ScheduledTasksServicePort, Me._TasksID)
                        ),
                        [Shared].Service.IScheduledTasks
                    )

                If Not rRemoteTasks Is Nothing Then
                    Dim PingError As Boolean = True
                    Dim PingThread As New Threading.Thread(Sub()
                                                               Try
                                                                   rRemoteTasks.PingToRemoteEndPoint()

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

                TasksExists = True
            Catch ex As System.Exception
                TasksExists = False
            Finally
                If Not TasksExists Then
                    rRemoteTasks = Nothing

                    If Not RemoteTasksServiceConnection Is Nothing Then
                        Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteTasksServiceConnection)

                        RemoteTasksServiceConnection = Nothing
                    End If
                End If
            End Try

            Return rRemoteTasks
        End Function

        Private Sub DestroyConnectionFromRemoteTasks(ByRef RemoteTasksServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel)
            Try
                If Not RemoteTasksServiceConnection Is Nothing Then _
                    Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteTasksServiceConnection)
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try
        End Sub

        Private Sub CheckTasks(ByVal state As Object)
            SyncLock Me._CheckProgressSync
                If Me._CheckIsInProgress Then Exit Sub

                Me._CheckIsInProgress = True
            End SyncLock

            Dim ExecutionTime As Long
            Do
                ExecutionTime = Helper.DateTime.Format(DateTime.Now)

                Threading.Monitor.Enter(Me._Tasks.SyncRoot)
                Try
                    If Me._Tasks.ContainsKey(ExecutionTime) Then
                        ' We run scheduled tasks in a thread to avoid late timming for next queue taskinfos (If Exists)
                        Threading.ThreadPool.QueueUserWorkItem(
                            New Threading.WaitCallback(AddressOf Me.ExecuteCallBacks),
                            CType(Me._Tasks.Item(ExecutionTime), Generic.List(Of TaskInfo)).ToArray()
                        )

                        Me._Tasks.Remove(ExecutionTime)
                    End If
                Finally
                    Threading.Monitor.Exit(Me._Tasks.SyncRoot)
                End Try

                If Me._Tasks.Count = 0 Then Exit Do

                Threading.Thread.Sleep(1000)
            Loop While True

            SyncLock Me._CheckProgressSync
                Me._CheckIsInProgress = False
            End SyncLock
        End Sub

        Private Sub ExecuteCallBacks(ByVal TaskInfos As Object)
            Dim sTasks As TaskInfo() = CType(TaskInfos, TaskInfo())

            For Each tI As TaskInfo In sTasks
                tI.CallBack.BeginInvoke(tI.CallBackParams, New AsyncCallback(AddressOf Me.EndTaskCallBack), tI)
            Next
        End Sub

        Private Sub EndTaskCallBack(ByVal AsyncResult As IAsyncResult)
            Try
                CType(AsyncResult.AsyncState, TaskInfo).CallBack.EndInvoke(AsyncResult)
            Catch ex As System.Exception
                Helper.EventLogger.Log(ex)
            End Try
        End Sub

        Public Overloads Function RegisterTask(ByVal CallBack As [Shared].Service.IScheduledTasks.TaskHandler, ByVal CallBackParams As Object(), ByVal ExecutionTime As DateTime) As String Implements [Shared].Service.IScheduledTasks.RegisterTask
            Dim rID As String = String.Empty

            If Not Me._IsTasksHost Then
                Dim RemoteTasksServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                Dim RemoteTasks As [Shared].Service.IScheduledTasks =
                    Me.CreateConnectionToRemoteTasks(RemoteTasksServiceConnection)

                Try
                    rID = RemoteTasks.RegisterTask(CallBack, CallBackParams, ExecutionTime)
                Catch ex As System.Exception
                    Me.CreateTasksHostForRemoteConnections()

                    rID = Me.RegisterTask(CallBack, CallBackParams, ExecutionTime)
                End Try

                Me.DestroyConnectionFromRemoteTasks(RemoteTasksServiceConnection)
            Else
                Dim ExecutionTime_L As Long =
                    Helper.DateTime.Format(ExecutionTime)
                Dim TaskInfo As New TaskInfo(CallBack, CallBackParams, ExecutionTime)

                Threading.Monitor.Enter(Me._Tasks.SyncRoot)
                Try
                    If Me._Tasks.ContainsKey(ExecutionTime_L) Then
                        Dim sTasks As Generic.List(Of TaskInfo) =
                            CType(Me._Tasks.Item(ExecutionTime_L), Generic.List(Of TaskInfo))
                        sTasks.Add(TaskInfo)

                        Me._Tasks.Item(ExecutionTime_L) = sTasks
                    Else
                        Me._Tasks.Add(ExecutionTime_L, New Generic.List(Of TaskInfo)({TaskInfo}))
                    End If
                Finally
                    Threading.Monitor.Exit(Me._Tasks.SyncRoot)
                End Try

                Threading.ThreadPool.QueueUserWorkItem(
                    New Threading.WaitCallback(AddressOf Me.CheckTasks))

                rID = TaskInfo.ID
            End If

            Return rID
        End Function

        Public Overloads Function RegisterTask(ByVal CallBack As [Shared].Service.IScheduledTasks.TaskHandler, ByVal CallBackParams As Object(), ByVal ExecutionTime As System.TimeSpan) As String Implements [Shared].Service.IScheduledTasks.RegisterTask
            Return Me.RegisterTask(CallBack, CallBackParams, DateTime.Now.Add(ExecutionTime))
        End Function

        Public Sub UnRegisterTask(ByVal ID As String) Implements [Shared].Service.IScheduledTasks.UnRegisterTask
            If Not Me._IsTasksHost Then
                Dim RemoteTasksServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                Dim RemoteTasks As [Shared].Service.IScheduledTasks =
                    Me.CreateConnectionToRemoteTasks(RemoteTasksServiceConnection)

                Try
                    RemoteTasks.UnRegisterTask(ID)
                Catch ex As System.Exception
                    Me.CreateTasksHostForRemoteConnections()

                    Me.UnRegisterTask(ID)
                End Try

                Me.DestroyConnectionFromRemoteTasks(RemoteTasksServiceConnection)
            Else
                Dim RemovedKey As Long

                Threading.Monitor.Enter(Me._Tasks.SyncRoot)
                Try
                    Dim dEnumerator As IDictionaryEnumerator = Me._Tasks.GetEnumerator()
                    Dim sTasks As Generic.List(Of TaskInfo)

                    Dim HasFound As Boolean = False
                    Do While dEnumerator.MoveNext()
                        sTasks = CType(dEnumerator.Value, Generic.List(Of TaskInfo))

                        For tC As Integer = 0 To sTasks.Count - 1
                            If String.Compare(sTasks(tC).ID, ID) = 0 Then
                                sTasks.RemoveAt(tC)

                                HasFound = True : Exit For
                            End If
                        Next

                        If HasFound Then
                            If sTasks.Count = 0 Then
                                RemovedKey = CType(dEnumerator.Key, Long)
                            Else
                                Me._Tasks.Item(CType(dEnumerator.Key, Long)) = sTasks
                            End If

                            Exit Do
                        End If
                    Loop

                    If Me._Tasks.ContainsKey(RemovedKey) Then _
                        Me._Tasks.Remove(RemovedKey)
                Finally
                    Threading.Monitor.Exit(Me._Tasks.SyncRoot)
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
                If Not Me._RemoteTasksService Is Nothing Then _
                    Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(Me._RemoteTasksService)
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try

            MyBase.Finalize()
        End Sub

        Private Class TaskInfo
            Private _ID As Guid
            Private _CallBack As [Shared].Service.IScheduledTasks.TaskHandler
            Private _CallBackParams As Object()
            Private _ExecutionTime As DateTime

            Public Sub New(ByVal CallBack As [Shared].Service.IScheduledTasks.TaskHandler, ByVal CallBackParams As Object(), ByVal ExecutionTime As DateTime)
                Me._ID = Guid.NewGuid()
                Me._CallBack = CallBack
                Me._CallBackParams = CallBackParams
                Me._ExecutionTime = ExecutionTime
            End Sub

            Public ReadOnly Property ID() As String
                Get
                    Return Me._ID.ToString()
                End Get
            End Property

            Public ReadOnly Property CallBack() As [Shared].Service.IScheduledTasks.TaskHandler
                Get
                    Return Me._CallBack
                End Get
            End Property

            Public ReadOnly Property CallBackParams() As Object()
                Get
                    Return Me._CallBackParams
                End Get
            End Property

            Public ReadOnly Property ExecutionTime() As DateTime
                Get
                    Return Me._ExecutionTime
                End Get
            End Property
        End Class
    End Class
End Namespace
