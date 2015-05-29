Option Strict On

Namespace SolidDevelopment.Web.Managers
    Public Class VariablePoolOperationClass
        Inherits MarshalByRefObject
        Implements PGlobals.Execution.IVariablePool

        Private Enum VariablePoolTypeStatus
            Host
            Client
            Testing
            None
        End Enum

        Private _VariablePoolID As String
        Private _RemoteVariablePoolService As System.Runtime.Remoting.Channels.Tcp.TcpServerChannel = Nothing
        Private _VariablePoolTypeStatus As VariablePoolTypeStatus

        ' Cache Variables In this object Until They are Confirmed
        Private _VariableCache As Hashtable
        Private _FileAccessQueues As Hashtable

        Private _SynchronizedObjects As Hashtable

        Private _VariableTimeout As Integer
        Private _Timer As Timers.Timer = Nothing

        Public Sub New()
            Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None
            Me._VariablePoolID = String.Format("VP_{0}", SolidDevelopment.Web.Configurations.WorkingPath.WorkingPathID)

            Me.CreateVariablePoolHostForRemoteConnections()

            If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Host Then
                ' Get the configuration section and set timeout and CookieMode values.
                Dim cfg As System.Configuration.Configuration = _
                    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration( _
                                System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                Dim wConfig As System.Web.Configuration.SessionStateSection = _
                    CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)

                Me._VariableTimeout = CInt(wConfig.Timeout.TotalMinutes)
                Me._Timer = New Timers.Timer(Me._VariableTimeout * 60 * 1000)
                AddHandler Me._Timer.Elapsed, New Timers.ElapsedEventHandler(AddressOf Me.RemoveExpiredVariableData)
                Me._Timer.AutoReset = True
                Me._Timer.Start()
            End If
        End Sub

        Private Sub CreateVariablePoolHostForRemoteConnections()
            Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
            Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)
            Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)

            If RemoteVariablePool Is Nothing Then
                Me._VariableCache = Hashtable.Synchronized(New Hashtable)
                Me._FileAccessQueues = Hashtable.Synchronized(New Hashtable)

                Me._SynchronizedObjects = Hashtable.Synchronized(New Hashtable)

                Try
                    Dim serverProvider As New System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider
                    serverProvider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full

                    Me._RemoteVariablePoolService = _
                        New System.Runtime.Remoting.Channels.Tcp.TcpServerChannel( _
                            Me._VariablePoolID, Configurations.VariablePoolServicePort, serverProvider)

                    ' Register RemoteVariablePoolService to Remoting Service
                    System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(Me._RemoteVariablePoolService, True)

                    ' Register VariablePool's Service Name
                    System.Runtime.Remoting.RemotingServices.Marshal(Me, Me._VariablePoolID, GetType(VariablePoolOperationClass))
                    'System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType( _
                    '    GetType(VariablePoolOperationClass), _
                    '    VariablePoolID, _
                    '    Runtime.Remoting.WellKnownObjectMode.Singleton _
                    ')
                    Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Host
                Catch ex As Exception
                    Try
                        System.Diagnostics.EventLog.WriteEntry("XeoraCube", ex.ToString(), EventLogEntryType.Error)
                    Catch exSub As Exception
                        ' Just Handle Request
                    End Try

                    Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None
                End Try
            Else
                Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Client
            End If
        End Sub

        Private Function CreateConnectionToRemoteVariablePool(ByRef RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel) As PGlobals.Execution.IVariablePool
            Do While Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Testing
                Threading.Thread.Sleep(50)
            Loop

            If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Testing

            Dim rRemoteVariablePool As PGlobals.Execution.IVariablePool = Nothing
            Dim VariablePoolExists As Boolean = False

            Try
                Dim clientProvider As New System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider

                RemoteVariablePoolServiceConnection = _
                    New System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(Guid.NewGuid().ToString(), clientProvider)
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(RemoteVariablePoolServiceConnection, True)

                rRemoteVariablePool = _
                    CType( _
                        Activator.GetObject( _
                            GetType(PGlobals.Execution.IVariablePool), _
                            String.Format("tcp://{0}:{1}/{2}", System.Environment.MachineName, Configurations.VariablePoolServicePort, Me._VariablePoolID) _
                        ),  _
                        PGlobals.Execution.IVariablePool _
                    )

                If Not rRemoteVariablePool Is Nothing Then
                    Dim PingError As Boolean = True
                    Dim PingThread As New Threading.Thread(Sub()
                                                               Try
                                                                   rRemoteVariablePool.PingToRemoteEndPoint()

                                                                   PingError = False
                                                               Catch ex As Exception
                                                                   PingError = True
                                                               End Try
                                                           End Sub)

                    PingThread.Priority = Threading.ThreadPriority.Highest
                    PingThread.Start()

                    Do While PingThread.ThreadState = Threading.ThreadState.Unstarted
                        Threading.Thread.Sleep(50)
                    Loop

                    PingThread.Join(10000)

                    If PingError Then
                        PingThread.Abort()

                        Throw New Exception()
                    End If
                End If

                VariablePoolExists = True

                If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Host Then _
                    rRemoteVariablePool = Me
            Catch ex As Exception
                VariablePoolExists = False
            Finally
                If Not VariablePoolExists OrElse _
                    (VariablePoolExists AndAlso Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Host) Then

                    If Not VariablePoolExists Then _
                        rRemoteVariablePool = Nothing

                    If Not RemoteVariablePoolServiceConnection Is Nothing Then
                        System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteVariablePoolServiceConnection)

                        RemoteVariablePoolServiceConnection = Nothing
                    End If
                End If
            End Try

            Return rRemoteVariablePool
        End Function

        Private Sub DestroyConnectionFromRemoteVariablePool(ByRef RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel)
            Try
                If Not RemoteVariablePoolServiceConnection Is Nothing Then _
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteVariablePoolServiceConnection)
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try
        End Sub

        Private ReadOnly Property FileAccessQueue(ByVal SessionKeyID As String) As Concurrent.ConcurrentQueue(Of String)
            Get
                Dim rQueue As Concurrent.ConcurrentQueue(Of String)

                System.Threading.Monitor.Enter(Me._FileAccessQueues.SyncRoot)
                Try
                    If Me._FileAccessQueues.ContainsKey(SessionKeyID) Then
                        rQueue = CType(Me._FileAccessQueues.Item(SessionKeyID), Concurrent.ConcurrentQueue(Of String))
                    Else
                        rQueue = New Concurrent.ConcurrentQueue(Of String)

                        Me._FileAccessQueues.Item(SessionKeyID) = rQueue
                    End If
                Finally
                    System.Threading.Monitor.Exit(Me._FileAccessQueues.SyncRoot)
                End Try

                Return rQueue
            End Get
        End Property

        Private Property SynchronizedObject(ByVal SessionKeyID As String, ByVal name As String) As Object
            Get
                Dim rObject As Object

                If Me._SynchronizedObjects.ContainsKey(String.Format("{0}_{1}", SessionKeyID, name)) Then
                    rObject = Me._SynchronizedObjects.Item(String.Format("{0}_{1}", SessionKeyID, name))
                Else
                    System.Threading.Monitor.Enter(Me._SynchronizedObjects.SyncRoot)
                    Try
                        rObject = New Object

                        Me._SynchronizedObjects.Item(String.Format("{0}_{1}", SessionKeyID, name)) = rObject
                    Finally
                        System.Threading.Monitor.Exit(Me._SynchronizedObjects.SyncRoot)
                    End Try
                End If

                Return rObject
            End Get
            Set(ByVal value As Object)
                System.Threading.Monitor.Enter(Me._SynchronizedObjects.SyncRoot)
                Try
                    If value Is Nothing Then
                        Me._SynchronizedObjects.Remove(String.Format("{0}_{1}", SessionKeyID, name))
                    Else
                        Me._SynchronizedObjects.Item(String.Format("{0}_{1}", SessionKeyID, name)) = value
                    End If
                Finally
                    System.Threading.Monitor.Exit(Me._SynchronizedObjects.SyncRoot)
                End Try
            End Set
        End Property

        Private Function ReadVariableValue(ByVal SessionKeyID As String, ByVal name As String) As Byte()
            Dim rValue As Byte() = _
                Me.ReadVariableValueFromCache(SessionKeyID, name)

            If rValue Is Nothing Then rValue = Me.ReadVariableValueFromFile(SessionKeyID, name)

            Return rValue
        End Function

        Private Function ReadVariableValueFromCache(ByVal SessionKeyID As String, ByVal name As String) As Byte()
            Dim rValue As Byte() = Nothing

            If Me._VariableCache.ContainsKey(String.Format("{0}_{1}", SessionKeyID, name)) Then _
                rValue = CType(Me._VariableCache.Item(String.Format("{0}_{1}", SessionKeyID, name)), Byte())

            Return rValue
        End Function

        Private Sub WriteVariableValueToCache(ByVal SessionKeyID As String, ByVal name As String, ByVal value As Byte())
            System.Threading.Monitor.Enter(Me._VariableCache.SyncRoot)
            Try
                If Me._VariableCache.ContainsKey(String.Format("{0}_{1}", SessionKeyID, name)) Then
                    If value Is Nothing Then
                        Me._VariableCache.Item(String.Format("{0}_{1}", SessionKeyID, name)) = New Byte() {}
                    Else
                        Me._VariableCache.Item(String.Format("{0}_{1}", SessionKeyID, name)) = value
                    End If
                Else
                    If value Is Nothing Then
                        Me._VariableCache.Add(String.Format("{0}_{1}", SessionKeyID, name), New Byte() {})
                    Else
                        Me._VariableCache.Add(String.Format("{0}_{1}", SessionKeyID, name), value)
                    End If
                End If
            Finally
                System.Threading.Monitor.Exit(Me._VariableCache.SyncRoot)
            End Try
        End Sub

        Private Function IsVariableExpired(ByRef vpsFS As IO.FileStream) As Boolean
            Dim rBoolean As Boolean = True

            If Not vpsFS Is Nothing AndAlso _
                vpsFS.CanSeek AndAlso vpsFS.CanRead Then

                ' "Expires: " length is 9
                ' it's value length is 14 bytes
                Dim bytes As Byte() = CType(Array.CreateInstance(GetType(Byte), 14), Byte())

                vpsFS.Seek(9, IO.SeekOrigin.Begin)
                vpsFS.Read(bytes, 0, 14)

                Dim ExpiresDateTimeLong As Long, ExpiresDateTime As Date
                Long.TryParse( _
                    System.Text.Encoding.UTF8.GetString(bytes), ExpiresDateTimeLong)
                ExpiresDateTime = Helpers.DateTime.FormatDateTime(ExpiresDateTimeLong)

                If Date.Compare(ExpiresDateTime, Date.Now) >= 0 Then rBoolean = False
            Else
                Throw New Exception("File Access is not suitable to check variable expiration!")
            End If

            Return rBoolean
        End Function

        Private Sub ExtendVariableLife(ByRef vpsFS As IO.FileStream)
            If Not vpsFS Is Nothing AndAlso _
                vpsFS.CanSeek AndAlso vpsFS.CanWrite Then

                ' "Expires: YYYYMMDDhhmmss" length is 23 + NewLineBytes
                Dim bytes As Byte() = _
                    System.Text.Encoding.UTF8.GetBytes( _
                        String.Format("Expires: {0}{1}", Helpers.DateTime.FormatDateTime(Date.Now.AddMinutes(Me._VariableTimeout), Helpers.DateTime.DateTimeFormat.DateTime), Environment.NewLine))

                vpsFS.Seek(0, IO.SeekOrigin.Begin)
                vpsFS.Write(bytes, 0, bytes.Length)
            Else
                Throw New IO.IOException("File Access is not suitable to extend the variable life!")
            End If
        End Sub

        Private Function ReadVariableValueFromFile(ByVal SessionKeyID As String, ByVal name As String) As Byte()
            Dim rValue As Byte() = Nothing

            Dim vpService As String = _
                        IO.Path.Combine( _
                            SolidDevelopment.Web.Configurations.TemporaryRoot, _
                            String.Format( _
                                    "{0}{2}PoolSessions{2}{1}{2}vmap.dat", _
                                    Configurations.WorkingPath.WorkingPathID, _
                                    SessionKeyID, _
                                    IO.Path.DirectorySeparatorChar _
                                ) _
                        )

            If IO.File.Exists(vpService) Then
                Dim RequestKey As String = _
                    String.Format("{0}_{1}", Date.Now.Ticks, name)
                Me.FileAccessQueue(SessionKeyID).Enqueue(RequestKey)

                Dim vpsFS As IO.FileStream = Nothing
                Dim vmapSR As IO.StreamReader = Nothing

                Dim QueueKey As String = String.Empty
                Dim mapID As String = String.Empty

                Do While Me.FileAccessQueue(SessionKeyID).TryPeek(QueueKey)
                    If String.Compare(RequestKey, QueueKey) = 0 Then
                        Try
                            vpsFS = New IO.FileStream(vpService, IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)

                            If Me.IsVariableExpired(vpsFS) Then
                                vpsFS.Close()

                                For Each FilePath As String In IO.Directory.GetFiles(IO.Path.GetDirectoryName(vpService))
                                    If String.Compare(FilePath, vpService) = 0 Then Continue For

                                    IO.File.Delete(FilePath)
                                Next

                                vpsFS = New IO.FileStream(vpService, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)
                            End If

                            Me.ExtendVariableLife(vpsFS)

                            vmapSR = New IO.StreamReader(vpsFS, System.Text.Encoding.UTF8)

                            Dim SearchLine As String
                            Do Until vmapSR.EndOfStream
                                SearchLine = vmapSR.ReadLine()

                                If String.Compare(SearchLine, name, True) = 0 Then
                                    mapID = vmapSR.ReadLine()
                                    mapID = mapID.Replace(Chr(9), "")

                                    Exit Do
                                End If
                            Loop
                        Finally
                            If Not vpsFS Is Nothing Then vpsFS.Close()
                        End Try

                        If Not String.IsNullOrEmpty(mapID) Then
                            vpService = vpService.Remove(vpService.Length - 8)
                            vpService = vpService.Insert(vpService.Length, String.Format("{0}.dat", mapID))

                            Dim FI As New IO.FileInfo(vpService)

                            If FI.Exists Then
                                Try
                                    vpsFS = FI.Open(IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                                    rValue = CType(Array.CreateInstance(GetType(Byte), FI.Length), Byte())
                                    vpsFS.Read(rValue, 0, rValue.Length)
                                Finally
                                    If Not vpsFS Is Nothing Then vpsFS.Close()
                                End Try
                            End If
                        End If

                        Me.FileAccessQueue(SessionKeyID).TryDequeue(QueueKey)

                        Exit Do
                    End If
                Loop
            End If

            Return rValue
        End Function

        Private Sub WriteVariableValueToFile(ByVal SessionKeyID As String, ByVal name As String, ByVal value As Byte())
            Dim vpService As String = _
                    IO.Path.Combine( _
                        SolidDevelopment.Web.Configurations.TemporaryRoot, _
                        String.Format( _
                                "{0}{2}PoolSessions{2}{1}{2}vmap.dat", _
                                Configurations.WorkingPath.WorkingPathID, _
                                SessionKeyID, _
                                IO.Path.DirectorySeparatorChar _
                            ) _
                    )

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(vpService)) Then _
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(vpService))

            Dim RequestKey As String = _
                    String.Format("{0}_{1}", Date.Now.Ticks, name)
            Me.FileAccessQueue(SessionKeyID).Enqueue(RequestKey)

            Dim vpsFS As IO.FileStream = Nothing
            Dim vmapSR As IO.StreamReader = Nothing
            Dim vmapSW As IO.StreamWriter = Nothing

            Dim vpsVFS As IO.FileStream = Nothing

            Dim QueueKey As String = String.Empty
            Dim mapID As String = Guid.NewGuid().ToString(), IsFound As Boolean = False

            Do While Me.FileAccessQueue(SessionKeyID).TryPeek(QueueKey)
                If String.Compare(RequestKey, QueueKey) = 0 Then
                    Try
                        vpsFS = New IO.FileStream(vpService, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.Read)

                        Me.ExtendVariableLife(vpsFS)

                        vmapSR = New IO.StreamReader(vpsFS, System.Text.Encoding.UTF8)

                        Dim SearchLine As String
                        Do Until vmapSR.EndOfStream
                            SearchLine = vmapSR.ReadLine()

                            If String.Compare(SearchLine, name, True) = 0 Then
                                mapID = vmapSR.ReadLine()
                                mapID = mapID.Replace(Chr(9), "")

                                IsFound = True

                                Exit Do
                            Else
                                ' Skip the MapID line which is not matching
                                vmapSR.ReadLine()
                            End If
                        Loop

                        If Not IsFound Then
                            vmapSW = New IO.StreamWriter(vpsFS, System.Text.Encoding.UTF8)
                            vmapSW.BaseStream.Seek(0, IO.SeekOrigin.End)

                            vmapSW.WriteLine(name)
                            vmapSW.WriteLine(String.Format("{0}{1}", Chr(9), mapID))
                            vmapSW.Flush()
                        End If

                        vpService = vpService.Remove(vpService.Length - 8)
                        vpService = vpService.Insert(vpService.Length, String.Format("{0}.dat", mapID))

                        If value Is Nothing OrElse value.Length = 0 Then
                            If IO.File.Exists(vpService) Then IO.File.Delete(vpService)
                        Else
                            vpsVFS = New IO.FileStream(vpService, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.Read)

                            vpsVFS.Write(value, 0, value.Length)
                            vpsVFS.Flush()
                        End If
                    Finally
                        If Not vpsVFS Is Nothing Then vpsVFS.Close()
                        If Not vpsFS Is Nothing Then vpsFS.Close()
                    End Try

                    Me.FileAccessQueue(SessionKeyID).TryDequeue(QueueKey)

                    Exit Do
                End If
            Loop
        End Sub

        Public Function GetVariableFromPool(ByVal SessionKeyID As String, ByVal name As String) As Byte() Implements PGlobals.Execution.IVariablePool.GetVariableFromPool
            Dim rBytes As Byte() = Nothing

            Select Case Me._VariablePoolTypeStatus
                Case VariablePoolTypeStatus.Host
                    If Not String.IsNullOrWhiteSpace(name) Then
                        Dim syncObject As Object = Me.SynchronizedObject(SessionKeyID, name)

                        System.Threading.Monitor.Enter(syncObject)
                        Try
                            Me.SynchronizedObject(SessionKeyID, name) = syncObject

                            rBytes = Me.ReadVariableValue(SessionKeyID, name)

                            If Not rBytes Is Nothing AndAlso rBytes.Length = 0 Then rBytes = Nothing
                        Finally
                            System.Threading.Monitor.Exit(syncObject)
                            Me.SynchronizedObject(SessionKeyID, name) = syncObject
                        End Try
                    End If
                Case VariablePoolTypeStatus.None
                    Me.CreateVariablePoolHostForRemoteConnections()

                    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                        Threading.Thread.Sleep(1000)

                    rBytes = Me.GetVariableFromPool(SessionKeyID, name)
                Case Else
                    Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                    Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                        Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)

                    Try
                        rBytes = RemoteVariablePool.GetVariableFromPool(SessionKeyID, name)
                    Catch ex As Exception
                        Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None

                        rBytes = Me.GetVariableFromPool(SessionKeyID, name)
                    End Try

                    Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)
            End Select

            Return rBytes
        End Function

        Public Sub DoMassRegistration(ByVal SessionKeyID As String, ByVal serializedSerializableDictionary As Byte()) Implements PGlobals.Execution.IVariablePool.DoMassRegistration
            Select Case Me._VariablePoolTypeStatus
                Case VariablePoolTypeStatus.Host
                    If Not serializedSerializableDictionary Is Nothing AndAlso _
                    serializedSerializableDictionary.Length > 0 Then

                        Dim forStream As IO.Stream = Nothing
                        Dim tObject As Object = Nothing

                        Try
                            Dim binFormater As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                            binFormater.Binder = New SolidDevelopment.Web.Helpers.OverrideBinder()

                            forStream = New IO.MemoryStream(serializedSerializableDictionary)

                            tObject = binFormater.Deserialize(forStream)
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        Finally
                            If Not forStream Is Nothing Then
                                forStream.Close()
                                GC.SuppressFinalize(forStream)
                            End If
                        End Try

                        If Not tObject Is Nothing AndAlso _
                            TypeOf tObject Is PGlobals.Execution.SerializableDictionary Then

                            Dim sD As PGlobals.Execution.SerializableDictionary = _
                                CType(tObject, PGlobals.Execution.SerializableDictionary)

                            For Each KVP As PGlobals.Execution.SerializableDictionary.SerializableKeyValuePair In sD
                                Me.RegisterVariableToPool(SessionKeyID, KVP.Key, KVP.Value)
                            Next
                        End If
                    End If
                Case VariablePoolTypeStatus.None
                    Me.CreateVariablePoolHostForRemoteConnections()

                    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                        Threading.Thread.Sleep(1000)

                    Me.DoMassRegistration(SessionKeyID, serializedSerializableDictionary)
                Case Else
                    Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                    Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                        Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)

                    Try
                        RemoteVariablePool.DoMassRegistration(SessionKeyID, serializedSerializableDictionary)
                    Catch ex As Exception
                        Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None

                        Me.DoMassRegistration(SessionKeyID, serializedSerializableDictionary)
                    End Try

                    Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)
            End Select
        End Sub

        Private Delegate Sub RegisterVariableToPoolDelegate(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte())
        Public Sub RegisterVariableToPool(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte()) Implements PGlobals.Execution.IVariablePool.RegisterVariableToPool
            Select Case Me._VariablePoolTypeStatus
                Case VariablePoolTypeStatus.Host
                    If String.IsNullOrWhiteSpace(name) Then Exit Sub

                    Dim syncObject As Object = Me.SynchronizedObject(SessionKeyID, name)

                    System.Threading.Monitor.Enter(syncObject)
                    Try
                        Me.SynchronizedObject(SessionKeyID, name) = syncObject

                        ' Register Variable to Pool
                        Me.WriteVariableValueToCache(SessionKeyID, name, serializedValue)
                    Finally
                        System.Threading.Monitor.Exit(syncObject)
                        Me.SynchronizedObject(SessionKeyID, name) = syncObject
                    End Try
                Case VariablePoolTypeStatus.None
                    Me.CreateVariablePoolHostForRemoteConnections()

                    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                        Threading.Thread.Sleep(1000)

                    Me.RegisterVariableToPool(SessionKeyID, name, serializedValue)
                Case Else
                    Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                    Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                        Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)

                    Try
                        RemoteVariablePool.RegisterVariableToPool(SessionKeyID, name, serializedValue)
                    Catch ex As Exception
                        Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None

                        Me.RegisterVariableToPool(SessionKeyID, name, serializedValue)
                    End Try

                    Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)
            End Select
        End Sub

        Public Sub UnRegisterVariableFromPool(ByVal SessionKeyID As String, ByVal name As String) Implements PGlobals.Execution.IVariablePool.UnRegisterVariableFromPool
            Select Case Me._VariablePoolTypeStatus
                Case VariablePoolTypeStatus.Host
                    Me.RegisterVariableToPool(SessionKeyID, name, Nothing)
                Case VariablePoolTypeStatus.None
                    Me.CreateVariablePoolHostForRemoteConnections()

                    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                        Threading.Thread.Sleep(1000)

                    Me.UnRegisterVariableFromPool(SessionKeyID, name)
                Case Else
                    Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                    Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                        Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)

                    Try
                        RemoteVariablePool.UnRegisterVariableFromPool(SessionKeyID, name)
                    Catch ex As Exception
                        Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None

                        Me.UnRegisterVariableFromPool(SessionKeyID, name)
                    End Try

                    Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)
            End Select
        End Sub

        Public Sub TransferRegistrations(ByVal FromSessionKeyID As String, ByVal CurrentSessionKeyID As String) Implements PGlobals.Execution.IVariablePool.TransferRegistrations
            Select Case Me._VariablePoolTypeStatus
                Case VariablePoolTypeStatus.Host
                    Me.ConfirmRegistrations(FromSessionKeyID)

                    Dim vpServiceFrom As String = _
                                IO.Path.Combine( _
                                    SolidDevelopment.Web.Configurations.TemporaryRoot, _
                                    String.Format( _
                                            "{0}{2}PoolSessions{2}{1}", _
                                            Configurations.WorkingPath.WorkingPathID, _
                                            FromSessionKeyID, _
                                            IO.Path.DirectorySeparatorChar _
                                        ) _
                                )

                    If IO.Directory.Exists(vpServiceFrom) Then
                        Dim vpServiceTo As String = _
                                IO.Path.Combine( _
                                    SolidDevelopment.Web.Configurations.TemporaryRoot, _
                                    String.Format( _
                                            "{0}{2}PoolSessions{2}{1}", _
                                            Configurations.WorkingPath.WorkingPathID, _
                                            CurrentSessionKeyID, _
                                            IO.Path.DirectorySeparatorChar _
                                        ) _
                                )

                        Dim dI As New IO.DirectoryInfo(vpServiceFrom)
                        If Not dI.Exists Then dI.Create()

                        For Each fI As IO.FileInfo In dI.GetFiles("*.dat")
                            Try
                                fI.CopyTo( _
                                    IO.Path.Combine(vpServiceTo, fI.Name), True)
                            Catch ex As Exception
                                ' Just Handle Exceptions
                            End Try
                        Next
                    End If
                Case VariablePoolTypeStatus.None
                    Me.CreateVariablePoolHostForRemoteConnections()

                    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                        Threading.Thread.Sleep(1000)

                    Me.TransferRegistrations(FromSessionKeyID, CurrentSessionKeyID)
                Case Else
                    Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                    Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                        Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)

                    Try
                        RemoteVariablePool.TransferRegistrations(FromSessionKeyID, CurrentSessionKeyID)
                    Catch ex As Exception
                        Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None

                        Me.TransferRegistrations(FromSessionKeyID, CurrentSessionKeyID)
                    End Try

                    Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)
            End Select
        End Sub

        Public Sub ConfirmRegistrations(ByVal SessionKeyID As String) Implements PGlobals.Execution.IVariablePool.ConfirmRegistrations
            Select Case Me._VariablePoolTypeStatus
                Case VariablePoolTypeStatus.Host
                    Dim tKeyList As New System.Collections.Generic.List(Of String)

                    System.Threading.Monitor.Enter(Me._VariableCache.SyncRoot)
                    Try
                        For Each key As String In Me._VariableCache.Keys
                            If key.IndexOf(SessionKeyID) > -1 Then
                                Dim eO As Boolean = False
                                Dim varName As String = key.Substring(SessionKeyID.Length + 1)

                                Try
                                    Me.WriteVariableValueToFile(SessionKeyID, varName, CType(Me._VariableCache.Item(key), Byte()))
                                Catch ex As Exception
                                    ' Write To File System Generally Occurs
                                    ' Because of that, keep this variable in the memory
                                    eO = True

                                    Try
                                        If Not System.Diagnostics.EventLog.SourceExists("XeoraCube") Then System.Diagnostics.EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                                        System.Diagnostics.EventLog.WriteEntry("XeoraCube", _
                                            " --- Variable Pool Registration Exception --- " & Environment.NewLine & Environment.NewLine & _
                                            ex.ToString(), _
                                            EventLogEntryType.Error _
                                        )
                                    Catch ex02 As Exception
                                        ' Just Handle Exception
                                    End Try
                                End Try

                                If Not eO Then tKeyList.Add(key)
                            End If
                        Next

                        For Each LI As String In tKeyList
                            If Me._VariableCache.ContainsKey(LI) Then Me._VariableCache.Remove(LI)
                        Next
                    Finally
                        System.Threading.Monitor.Exit(Me._VariableCache.SyncRoot)
                    End Try
                Case VariablePoolTypeStatus.None
                    Me.CreateVariablePoolHostForRemoteConnections()

                    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                        Threading.Thread.Sleep(1000)

                    Me.ConfirmRegistrations(SessionKeyID)
                Case Else
                    Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                    Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                        Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)

                    Try
                        RemoteVariablePool.ConfirmRegistrations(SessionKeyID)
                    Catch ex As Exception
                        Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None

                        Me.ConfirmRegistrations(SessionKeyID)
                    End Try

                    Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)
            End Select
        End Sub

        Public Sub DoCleanUp() Implements PGlobals.Execution.IVariablePool.DoCleanUp
            Select Case Me._VariablePoolTypeStatus
                Case VariablePoolTypeStatus.Host
                    Dim vpLocation As String = _
                        IO.Path.Combine( _
                            SolidDevelopment.Web.Configurations.TemporaryRoot, _
                            String.Format( _
                                "{0}{1}PoolSessions{1}", _
                                Configurations.WorkingPath.WorkingPathID, _
                                IO.Path.DirectorySeparatorChar _
                            ) _
                        )

                    If IO.Directory.Exists(vpLocation) Then
                        For Each Path As String In IO.Directory.GetDirectories(vpLocation)
                            Dim IsExpired As Boolean = False

                            Dim vpsFS As IO.FileStream = Nothing
                            Dim vpService As String = _
                                IO.Path.Combine( _
                                    vpLocation, _
                                    String.Format( _
                                        "{0}{1}vmap.dat", _
                                        IO.Path.GetFileName(Path), _
                                        IO.Path.DirectorySeparatorChar _
                                    ) _
                                )

                            Try
                                vpsFS = New IO.FileStream(vpService, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                                IsExpired = Me.IsVariableExpired(vpsFS)
                            Catch ex As Exception
                                ' Just Handle Exceptions
                            Finally
                                If Not vpsFS Is Nothing Then vpsFS.Close()
                            End Try

                            If IsExpired Then
                                Try
                                    IO.Directory.Delete(Path, True)
                                Catch ex As Exception
                                    ' Just Handle Exceptions
                                    ' If unable to remove, just leave the trash where it is...
                                End Try
                            End If
                        Next
                    End If
                Case VariablePoolTypeStatus.None
                    Me.CreateVariablePoolHostForRemoteConnections()

                    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None Then _
                        Threading.Thread.Sleep(1000)

                    Me.DoCleanUp()
                Case Else
                    Dim RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
                    Dim RemoteVariablePool As PGlobals.Execution.IVariablePool = _
                        Me.CreateConnectionToRemoteVariablePool(RemoteVariablePoolServiceConnection)

                    Try
                        RemoteVariablePool.DoCleanUp()
                    Catch ex As Exception
                        Me._VariablePoolTypeStatus = VariablePoolTypeStatus.None

                        Me.DoCleanUp()
                    End Try

                    Me.DestroyConnectionFromRemoteVariablePool(RemoteVariablePoolServiceConnection)
            End Select
        End Sub

        ' Recursivly remove expired session data from session collection.
        Private _VariablePruning As Boolean = False
        Private Sub RemoveExpiredVariableData(ByVal sender As Object, ByVal e As EventArgs)
            If Me._VariablePruning Then Exit Sub

            Me._VariablePruning = True
            Me.DoCleanUp()
            Me._VariablePruning = False
        End Sub

        Public Function PingToRemoteEndPoint() As Boolean Implements PGlobals.Execution.IVariablePool.PingToRemoteEndPoint
            Return True
        End Function

        Public Overrides Function InitializeLifetimeService() As Object
            Return Nothing
        End Function

        Protected Overrides Sub Finalize()
            If Not Me._Timer Is Nothing Then
                Me._Timer.Enabled = False
                Me._Timer.Dispose()
            End If

            Try
                If Not Me._RemoteVariablePoolService Is Nothing Then _
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(Me._RemoteVariablePoolService)
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try

            MyBase.Finalize()
        End Sub
    End Class
End Namespace
