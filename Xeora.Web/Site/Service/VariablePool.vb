Option Strict On

Namespace Xeora.Web.Site.Service
    Public Class VariablePool
        Inherits MarshalByRefObject
        Implements [Shared].Service.IVariablePool

        Private Const APPLICATIONSESSIONKEYID As String = "000000000000000000000000_00000000"

        Private _VariablePoolID As String
        Private _RemoteVariablePoolService As System.Runtime.Remoting.Channels.Tcp.TcpServerChannel = Nothing
        Private _VariablePoolTypeStatus As [Shared].Service.IVariablePool.VariablePoolTypeStatus

        ' Cache Variables In this object Until They are Confirmed
        Private _VariableCache As Hashtable
        Private _FileAccessQueues As Hashtable

        Private _SynchronizedObjects As Hashtable

        Private _VariableTimeout As Integer
        Private _Timer As Timers.Timer = Nothing

        Private _PoolLockFS As IO.FileStream = Nothing
        Private _PoolLockFileLocation As String =
            IO.Path.Combine(
                [Shared].Configurations.TemporaryRoot,
                String.Format(
                        "{0}{1}vp.lock",
                        [Shared].Configurations.WorkingPath.WorkingPathID,
                        IO.Path.DirectorySeparatorChar
                    )
            )

        Private _RemoteVariablePoolServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel = Nothing
        Private _VariablePool As [Shared].Service.IVariablePool = Nothing

        Public Sub New()
            Me._VariablePoolID = String.Format("VP_{0}", [Shared].Configurations.WorkingPath.WorkingPathID)

            Me.RemakeVariablePoolConnections()
        End Sub

        Public ReadOnly Property VariablePoolType As [Shared].Service.IVariablePool.VariablePoolTypeStatus Implements [Shared].Service.IVariablePool.VariablePoolType
            Get
                Return Me._VariablePoolTypeStatus
            End Get
        End Property

        Private Sub RemakeVariablePoolConnections()
            Me.CreateVariablePoolHostForRemoteConnections()

            ' Let's set our pointer. If it is client it will be a remote connection tunnel or will throw exception
            Me._VariablePool = Me.CreateConnectionToRemoteVariablePool(Me._RemoteVariablePoolServiceConnection)

            ' VariablePool does not exists? So, hmmm, lets wait for a second, sometimes a second change so many things...
            If Me._VariablePoolTypeStatus = [Shared].Service.IVariablePool.VariablePoolTypeStatus.Client AndAlso
                Me._VariablePool Is Nothing Then _
                Threading.Thread.Sleep(1000)
        End Sub

        Private Sub CreateVariablePoolHostForRemoteConnections()
            Try
                ' Pool Lock File Accessiable
                Me._PoolLockFS = New IO.FileStream(Me._PoolLockFileLocation, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)

                ' No Exception so Create Remoting Service
                Dim serverProvider As New Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider
                serverProvider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full

                Me._RemoteVariablePoolService =
                    New Runtime.Remoting.Channels.Tcp.TcpServerChannel(
                        Me._VariablePoolID, [Shared].Configurations.VariablePoolServicePort, serverProvider)

                ' Register RemoteVariablePoolService to Remoting Service
                Runtime.Remoting.Channels.ChannelServices.RegisterChannel(Me._RemoteVariablePoolService, True)

                ' Register VariablePool's Service Name
                Runtime.Remoting.RemotingServices.Marshal(Me, Me._VariablePoolID, GetType(VariablePool))

                ' No Exception So Mark As Host and Create Local Variables
                Me._VariablePoolTypeStatus = [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host

                ' Get the configuration section and set timeout and CookieMode values.
                Dim cfg As Configuration.Configuration =
                    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                        System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                Dim wConfig As System.Web.Configuration.SessionStateSection =
                    CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)

                Me._VariableTimeout = CInt(wConfig.Timeout.TotalMinutes)
                Me._Timer = New Timers.Timer(Me._VariableTimeout * 60 * 1000)
                AddHandler Me._Timer.Elapsed, New Timers.ElapsedEventHandler(AddressOf Me.RemoveExpiredVariableData)
                Me._Timer.AutoReset = True
                Me._Timer.Start()

                Me._VariableCache = Hashtable.Synchronized(New Hashtable)
                Me._FileAccessQueues = Hashtable.Synchronized(New Hashtable)

                Me._SynchronizedObjects = Hashtable.Synchronized(New Hashtable)
            Catch ex As IO.IOException
                Me._VariablePoolTypeStatus = [Shared].Service.IVariablePool.VariablePoolTypeStatus.Client
            Catch ex As System.Exception
                If Not Me._PoolLockFS Is Nothing Then
                    Me._PoolLockFS.Close()

                    Me._PoolLockFS = Nothing
                End If

                Throw
            End Try
        End Sub

        Private Function CreateConnectionToRemoteVariablePool(ByRef RemoteVariablePoolServiceConnection As Runtime.Remoting.Channels.Tcp.TcpClientChannel) As [Shared].Service.IVariablePool
            Dim rRemoteVariablePool As [Shared].Service.IVariablePool = Nothing

            If Me._VariablePoolTypeStatus = [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host Then
                rRemoteVariablePool = Me
            Else
                Dim VariablePoolExists As Boolean = False

                Try
                    Dim clientProvider As New Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider

                    RemoteVariablePoolServiceConnection =
                        New Runtime.Remoting.Channels.Tcp.TcpClientChannel(Guid.NewGuid().ToString(), clientProvider)
                    Runtime.Remoting.Channels.ChannelServices.RegisterChannel(RemoteVariablePoolServiceConnection, True)

                    rRemoteVariablePool =
                        CType(
                            Activator.GetObject(
                                GetType([Shared].Service.IVariablePool),
                                String.Format("tcp://{0}:{1}/{2}", Environment.MachineName, [Shared].Configurations.VariablePoolServicePort, Me._VariablePoolID)
                            ),
                            [Shared].Service.IVariablePool
                        )

                    If Not rRemoteVariablePool Is Nothing Then
                        Dim PingError As Boolean = True
                        Dim PingThread As New Threading.Thread(Sub()
                                                                   Try
                                                                       rRemoteVariablePool.PingToRemoteEndPoint()

                                                                       PingError = False
                                                                   Catch ex As System.Exception
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

                            Throw New Net.Sockets.SocketException()
                        End If
                    End If

                    VariablePoolExists = True
                Catch ex As System.Exception
                    VariablePoolExists = False
                Finally
                    If Not VariablePoolExists Then
                        rRemoteVariablePool = Nothing

                        If Not RemoteVariablePoolServiceConnection Is Nothing Then
                            Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteVariablePoolServiceConnection)

                            RemoteVariablePoolServiceConnection = Nothing
                        End If

                        '' Try to get the ownership of being host
                        'Try
                        '    Me.CreateVariablePoolHostForRemoteConnections()

                        '    If Me._VariablePoolTypeStatus = VariablePoolTypeStatus.Host Then _
                        '        rRemoteVariablePool = Me
                        'Catch ex As Exception
                        '    Throw ex
                        'End Try
                    End If
                End Try
            End If

            Return rRemoteVariablePool
        End Function

        'Private Sub DestroyConnectionFromRemoteVariablePool(ByRef RemoteVariablePoolServiceConnection As System.Runtime.Remoting.Channels.Tcp.TcpClientChannel)
        '    Try
        '        If Not RemoteVariablePoolServiceConnection Is Nothing Then _
        '            System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(RemoteVariablePoolServiceConnection)
        '    Catch ex As Exception
        '        ' Just Handle Exceptions
        '    End Try
        'End Sub

        Private ReadOnly Property FileAccessQueue(ByVal SessionKeyID As String) As Concurrent.ConcurrentQueue(Of String)
            Get
                Dim rQueue As Concurrent.ConcurrentQueue(Of String)

                Threading.Monitor.Enter(Me._FileAccessQueues.SyncRoot)
                Try
                    If Me._FileAccessQueues.ContainsKey(SessionKeyID) Then
                        rQueue = CType(Me._FileAccessQueues.Item(SessionKeyID), Concurrent.ConcurrentQueue(Of String))
                    Else
                        rQueue = New Concurrent.ConcurrentQueue(Of String)

                        Me._FileAccessQueues.Item(SessionKeyID) = rQueue
                    End If
                Finally
                    Threading.Monitor.Exit(Me._FileAccessQueues.SyncRoot)
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
                    Threading.Monitor.Enter(Me._SynchronizedObjects.SyncRoot)
                    Try
                        rObject = New Object()

                        Me._SynchronizedObjects.Item(String.Format("{0}_{1}", SessionKeyID, name)) = rObject
                    Finally
                        Threading.Monitor.Exit(Me._SynchronizedObjects.SyncRoot)
                    End Try
                End If

                Return rObject
            End Get
            Set(ByVal value As Object)
                Threading.Monitor.Enter(Me._SynchronizedObjects.SyncRoot)
                Try
                    If value Is Nothing Then
                        Me._SynchronizedObjects.Remove(String.Format("{0}_{1}", SessionKeyID, name))
                    Else
                        Me._SynchronizedObjects.Item(String.Format("{0}_{1}", SessionKeyID, name)) = value
                    End If
                Finally
                    Threading.Monitor.Exit(Me._SynchronizedObjects.SyncRoot)
                End Try
            End Set
        End Property

        Private Function ReadVariableValue(ByVal SessionKeyID As String, ByVal name As String) As Byte()
            Dim rValue As Byte() =
                Me.ReadVariableValueFromCache(SessionKeyID, name)

            If rValue Is Nothing Then
                rValue = Me.ReadVariableValueFromFile(SessionKeyID, name)

                If Not rValue Is Nothing Then _
                    Me.WriteVariableValueToCache(SessionKeyID, name, rValue)
            End If

            Return rValue
        End Function

        Private Function ReadVariableValueFromCache(ByVal SessionKeyID As String, ByVal name As String) As Byte()
            Dim rValue As Byte() = Nothing

            If Me._VariableCache.ContainsKey(SessionKeyID) Then
                Dim SessionKeysObject As Object() =
                    CType(Me._VariableCache.Item(SessionKeyID), Object())

                If Not SessionKeysObject Is Nothing AndAlso
                    SessionKeysObject.Length = 2 Then

                    Dim SessionKeysDate As Date = CType(SessionKeysObject(0), Date)
                    SessionKeysDate = SessionKeysDate.AddMinutes(Me._VariableTimeout)

                    If Date.Compare(SessionKeysDate, Date.Now) >= 0 Then
                        Dim SessionKeysHash As Hashtable = CType(SessionKeysObject(1), Hashtable)

                        If SessionKeysHash.ContainsKey(name) AndAlso
                            Not SessionKeysHash.Item(name) Is Nothing Then _
                            rValue = CType(SessionKeysHash.Item(name), Byte())
                    End If
                End If
            End If

            Return rValue
        End Function

        Private Sub WriteVariableValueToCache(ByVal SessionKeyID As String, ByVal name As String, ByVal value As Byte())
            Threading.Monitor.Enter(Me._VariableCache.SyncRoot)
            Try
                Dim SessionKeysObject As Object() = Nothing

                If Me._VariableCache.ContainsKey(SessionKeyID) Then _
                    SessionKeysObject = CType(Me._VariableCache.Item(SessionKeyID), Object())
                If SessionKeysObject Is Nothing Then SessionKeysObject = New Object() {Date.Now, New Hashtable()}

                Dim SessionKeysHash As Hashtable =
                    CType(SessionKeysObject(1), Hashtable)

                If value Is Nothing Then
                    If SessionKeysHash.ContainsKey(name) Then
                        SessionKeysHash.Remove(name)

                        ' Do Also Immediate File Session Information Destroy
                        Me.WriteVariableValueToFile(SessionKeyID, name, Nothing)
                    End If
                Else
                    SessionKeysHash.Item(name) = value
                End If

                Me._VariableCache.Item(SessionKeyID) = New Object() {Date.Now, SessionKeysHash}
            Finally
                Threading.Monitor.Exit(Me._VariableCache.SyncRoot)
            End Try
        End Sub

        Private Function IsVariableExpired(ByRef vpsFS As IO.FileStream) As Boolean
            Dim rBoolean As Boolean = True

            If Not vpsFS Is Nothing AndAlso
                vpsFS.CanSeek AndAlso vpsFS.CanRead Then

                ' "Expires: " length is 9
                ' it's value length is 14 bytes
                Dim bytes As Byte() = CType(Array.CreateInstance(GetType(Byte), 14), Byte())

                vpsFS.Seek(9, IO.SeekOrigin.Begin)
                vpsFS.Read(bytes, 0, 14)

                Dim ExpiresDateLong As Long, ExpiresDate As Date
                Long.TryParse(
                    Text.Encoding.UTF8.GetString(bytes), ExpiresDateLong)
                ExpiresDate = Helper.Date.Format(ExpiresDateLong)

                If Date.Compare(ExpiresDate, Date.Now) >= 0 Then rBoolean = False
            Else
                Throw New IO.IOException("File Access is not suitable to check variable expiration!")
            End If

            Return rBoolean
        End Function

        Private Sub ExtendVariableLife(ByRef vpsFS As IO.FileStream)
            If Not vpsFS Is Nothing AndAlso
                vpsFS.CanSeek AndAlso vpsFS.CanWrite Then

                ' "Expires: YYYYMMDDhhmmss" length is 23 + NewLineBytes
                Dim bytes As Byte() =
                    Text.Encoding.UTF8.GetBytes(
                        String.Format("Expires: {0}{1}", Helper.Date.Format(Date.Now.AddMinutes(Me._VariableTimeout), Helper.Date.DateFormats.DateWithTime), Environment.NewLine))

                vpsFS.Seek(0, IO.SeekOrigin.Begin)
                vpsFS.Write(bytes, 0, bytes.Length)
            Else
                Throw New IO.IOException("File Access is not suitable to extend the variable life!")
            End If
        End Sub

        Private Function ReadVariableValueFromFile(ByVal SessionKeyID As String, ByVal name As String) As Byte()
            Dim rValue As Byte() = Nothing

            Dim vpService As String =
                        IO.Path.Combine(
                            [Shared].Configurations.TemporaryRoot,
                            String.Format(
                                    "{0}{2}PoolSessions{2}{1}{2}vmap.dat",
                                    [Shared].Configurations.WorkingPath.WorkingPathID,
                                    SessionKeyID,
                                    IO.Path.DirectorySeparatorChar
                                )
                        )

            If IO.File.Exists(vpService) Then
                Dim RequestKey As String =
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
            Dim vpService As String =
                    IO.Path.Combine(
                        [Shared].Configurations.TemporaryRoot,
                        String.Format(
                                "{0}{2}PoolSessions{2}{1}{2}vmap.dat",
                                [Shared].Configurations.WorkingPath.WorkingPathID,
                                SessionKeyID,
                                IO.Path.DirectorySeparatorChar
                            )
                    )

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(vpService)) Then _
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(vpService))

            Dim RequestKey As String =
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

                        vmapSR = New IO.StreamReader(vpsFS, Text.Encoding.UTF8)

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
                            vmapSW = New IO.StreamWriter(vpsFS, Text.Encoding.UTF8)
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

        Public Function GetVariableFromPool(ByVal SessionKeyID As String, ByVal name As String) As Byte() Implements [Shared].Service.IVariablePool.GetVariableFromPool
            Dim rBytes As Byte() = Nothing

            Select Case Me._VariablePoolTypeStatus
                Case [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host
                    If Not String.IsNullOrWhiteSpace(name) Then
                        Dim syncObject As Object = Me.SynchronizedObject(SessionKeyID, name)

                        Threading.Monitor.Enter(syncObject)
                        Try
                            Me.SynchronizedObject(SessionKeyID, name) = syncObject

                            rBytes = Me.ReadVariableValue(SessionKeyID, name)

                            If Not rBytes Is Nothing AndAlso rBytes.Length = 0 Then rBytes = Nothing
                        Finally
                            Threading.Monitor.Exit(syncObject)
                            Me.SynchronizedObject(SessionKeyID, name) = syncObject
                        End Try
                    End If
                Case Else
                    Dim eO As Boolean = False
                    Try
                        If Not Me._VariablePool Is Nothing AndAlso Me._VariablePool.PingToRemoteEndPoint() Then
                            rBytes = Me._VariablePool.GetVariableFromPool(SessionKeyID, name)
                        Else
                            Throw New Net.Sockets.SocketException()
                        End If
                    Catch ex As System.Exception
                        eO = True
                    End Try

                    If eO Then
                        Me.RemakeVariablePoolConnections()

                        rBytes = Me.GetVariableFromPool(SessionKeyID, name)
                    End If
            End Select

            Return rBytes
        End Function

        Public Sub DoMassRegistration(ByVal SessionKeyID As String, ByVal serializedSerializableDictionary As Byte()) Implements [Shared].Service.IVariablePool.DoMassRegistration
            Select Case Me._VariablePoolTypeStatus
                Case [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host
                    If Not serializedSerializableDictionary Is Nothing AndAlso
                    serializedSerializableDictionary.Length > 0 Then

                        Dim forStream As IO.Stream = Nothing
                        Dim tObject As Object = Nothing

                        Try
                            Dim binFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                            binFormater.Binder = New Helper.OverrideBinder()

                            forStream = New IO.MemoryStream(serializedSerializableDictionary)

                            tObject = binFormater.Deserialize(forStream)
                        Catch ex As System.Exception
                            ' Just Handle Exceptions
                        Finally
                            If Not forStream Is Nothing Then
                                forStream.Close()
                                GC.SuppressFinalize(forStream)
                            End If
                        End Try

                        If Not tObject Is Nothing AndAlso
                            TypeOf tObject Is [Shared].Service.VariablePoolOperation.SerializableDictionary Then

                            Dim sD As [Shared].Service.VariablePoolOperation.SerializableDictionary =
                                CType(tObject, [Shared].Service.VariablePoolOperation.SerializableDictionary)

                            For Each KVP As [Shared].Service.VariablePoolOperation.SerializableDictionary.SerializableKeyValuePair In sD
                                Me.RegisterVariableToPool(SessionKeyID, KVP.Key, KVP.Value)
                            Next
                        End If
                    End If
                Case Else
                    Dim eO As Boolean = False
                    Try
                        If Not Me._VariablePool Is Nothing AndAlso Me._VariablePool.PingToRemoteEndPoint() Then
                            Me._VariablePool.DoMassRegistration(SessionKeyID, serializedSerializableDictionary)
                        Else
                            Throw New Net.Sockets.SocketException()
                        End If
                    Catch ex As System.Exception
                        eO = True
                    End Try

                    If eO Then
                        Me.RemakeVariablePoolConnections()

                        Me.DoMassRegistration(SessionKeyID, serializedSerializableDictionary)
                    End If
            End Select
        End Sub

        'Private Delegate Sub RegisterVariableToPoolDelegate(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte())
        Public Sub RegisterVariableToPool(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte()) Implements [Shared].Service.IVariablePool.RegisterVariableToPool
            Select Case Me._VariablePoolTypeStatus
                Case [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host
                    If String.IsNullOrWhiteSpace(name) Then Exit Sub

                    Dim syncObject As Object = Me.SynchronizedObject(SessionKeyID, name)

                    Threading.Monitor.Enter(syncObject)
                    Try
                        Me.SynchronizedObject(SessionKeyID, name) = syncObject

                        ' Register Variable to Pool
                        Me.WriteVariableValueToCache(SessionKeyID, name, serializedValue)
                    Finally
                        Threading.Monitor.Exit(syncObject)
                        Me.SynchronizedObject(SessionKeyID, name) = syncObject
                    End Try
                Case Else
                    Dim eO As Boolean = False
                    Try
                        If Not Me._VariablePool Is Nothing AndAlso Me._VariablePool.PingToRemoteEndPoint() Then
                            Me._VariablePool.RegisterVariableToPool(SessionKeyID, name, serializedValue)
                        Else
                            Throw New Net.Sockets.SocketException()
                        End If
                    Catch ex As System.Exception
                        eO = True
                    End Try

                    If eO Then
                        Me.RemakeVariablePoolConnections()

                        Me.RegisterVariableToPool(SessionKeyID, name, serializedValue)
                    End If
            End Select
        End Sub

        Public Sub UnRegisterVariableFromPool(ByVal SessionKeyID As String, ByVal name As String) Implements [Shared].Service.IVariablePool.UnRegisterVariableFromPool
            Select Case Me._VariablePoolTypeStatus
                Case [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host
                    Me.RegisterVariableToPool(SessionKeyID, name, Nothing)
                Case Else
                    Dim eO As Boolean = False
                    Try
                        If Not Me._VariablePool Is Nothing AndAlso Me._VariablePool.PingToRemoteEndPoint() Then
                            Me._VariablePool.UnRegisterVariableFromPool(SessionKeyID, name)
                        Else
                            Throw New Net.Sockets.SocketException()
                        End If
                    Catch ex As System.Exception
                        eO = True
                    End Try

                    If eO Then
                        Me.RemakeVariablePoolConnections()

                        Me.UnRegisterVariableFromPool(SessionKeyID, name)
                    End If
            End Select
        End Sub

        Public Sub TransferRegistrations(ByVal FromSessionKeyID As String, ByVal CurrentSessionKeyID As String) Implements [Shared].Service.IVariablePool.TransferRegistrations
            Select Case Me._VariablePoolTypeStatus
                Case [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host
                    If Me._VariableCache.ContainsKey(FromSessionKeyID) Then _
                        Me._VariableCache.Item(CurrentSessionKeyID) =
                            Me._VariableCache.Item(FromSessionKeyID)
                Case Else
                    Dim eO As Boolean = False
                    Try
                        If Not Me._VariablePool Is Nothing AndAlso Me._VariablePool.PingToRemoteEndPoint() Then
                            Me._VariablePool.TransferRegistrations(FromSessionKeyID, CurrentSessionKeyID)
                        Else
                            Throw New Net.Sockets.SocketException()
                        End If
                    Catch ex As System.Exception
                        eO = True
                    End Try

                    If eO Then
                        Me.RemakeVariablePoolConnections()

                        Me.TransferRegistrations(FromSessionKeyID, CurrentSessionKeyID)
                    End If
            End Select
        End Sub

        Private Sub ConfirmRegistrations() 'Implements PGlobals.Execution.IVariablePool.ConfirmRegistrations
            Select Case Me._VariablePoolTypeStatus
                Case [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host
                    Threading.Monitor.Enter(Me._VariableCache.SyncRoot)
                    Try
                        For Each SessionKeyID As String In Me._VariableCache.Keys
                            Dim SessionKeysObject As Object() =
                                CType(Me._VariableCache.Item(SessionKeyID), Object())

                            If Not SessionKeysObject Is Nothing AndAlso
                                SessionKeysObject.Length = 2 Then

                                Dim SessionKeysDate As Date =
                                    CType(SessionKeysObject(0), Date)
                                SessionKeysDate = SessionKeysDate.AddMinutes(Me._VariableTimeout)

                                If Date.Compare(SessionKeysDate, Date.Now) >= 0 Then
                                    Dim SessionKeysHash As Hashtable =
                                        CType(SessionKeysObject(1), Hashtable)

                                    If Not SessionKeysHash Is Nothing Then
                                        For Each varName As String In SessionKeysHash.Keys
                                            Try
                                                Me.WriteVariableValueToFile(SessionKeyID, varName, CType(SessionKeysHash.Item(varName), Byte()))
                                            Catch ex As System.Exception
                                                ' Write To File System Generally Occurs
                                                ' However, this application is already correpted. That's why just log to see what's going on...
                                                Try
                                                    If Not EventLog.SourceExists("XeoraCube") Then EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                                                    EventLog.WriteEntry("XeoraCube",
                                                        " --- Variable Pool Registration Exception --- " & Environment.NewLine & Environment.NewLine &
                                                        ex.ToString(),
                                                        EventLogEntryType.Error
                                                    )
                                                Catch ex02 As System.Exception
                                                    ' Just Handle Exception
                                                End Try
                                            End Try
                                        Next
                                    End If
                                End If
                            End If
                        Next
                    Finally
                        Threading.Monitor.Exit(Me._VariableCache.SyncRoot)
                    End Try

            End Select
        End Sub

        Public Sub DoCleanUp() Implements [Shared].Service.IVariablePool.DoCleanUp
            Select Case Me._VariablePoolTypeStatus
                Case [Shared].Service.IVariablePool.VariablePoolTypeStatus.Host
                    ' First Clean Up Cache
                    Threading.Monitor.Enter(Me._VariableCache.SyncRoot)
                    Try
                        Dim CleanUpSessionKeysIDList As New Generic.List(Of String)

                        For Each SessionKeyID As String In Me._VariableCache.Keys
                            If String.Compare(VariablePool.APPLICATIONSESSIONKEYID, SessionKeyID) = 0 Then Continue For

                            Dim SessionKeysObject As Object() =
                                CType(Me._VariableCache.Item(SessionKeyID), Object())

                            If Not SessionKeysObject Is Nothing AndAlso
                                SessionKeysObject.Length = 2 Then

                                Dim SessionKeysDate As Date = CType(SessionKeysObject(0), Date)
                                SessionKeysDate = SessionKeysDate.AddMinutes(Me._VariableTimeout)

                                If Date.Compare(SessionKeysDate, Date.Now) <= 0 Then _
                                    CleanUpSessionKeysIDList.Add(SessionKeyID)
                            Else
                                CleanUpSessionKeysIDList.Add(SessionKeyID)
                            End If
                        Next

                        For Each SessionKeyID As String In CleanUpSessionKeysIDList
                            If Me._VariableCache.ContainsKey(SessionKeyID) Then _
                                Me._VariableCache.Remove(SessionKeyID)
                        Next
                    Finally
                        Threading.Monitor.Exit(Me._VariableCache.SyncRoot)
                    End Try

                    ' Now Do the same thing for variable cache files
                    Dim vpLocation As String =
                        IO.Path.Combine(
                            [Shared].Configurations.TemporaryRoot,
                            String.Format(
                                "{0}{1}PoolSessions{1}",
                                [Shared].Configurations.WorkingPath.WorkingPathID,
                                IO.Path.DirectorySeparatorChar
                            )
                        )

                    If IO.Directory.Exists(vpLocation) Then
                        For Each Path As String In IO.Directory.GetDirectories(vpLocation)
                            Dim IsExpired As Boolean = False

                            Dim vpsFS As IO.FileStream = Nothing
                            Dim vpService As String =
                                IO.Path.Combine(
                                    vpLocation,
                                    String.Format(
                                        "{0}{1}vmap.dat",
                                        IO.Path.GetFileName(Path),
                                        IO.Path.DirectorySeparatorChar
                                    )
                                )

                            Try
                                vpsFS = New IO.FileStream(vpService, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                                IsExpired = Me.IsVariableExpired(vpsFS)
                            Catch ex As System.Exception
                                ' Just Handle Exceptions
                            Finally
                                If Not vpsFS Is Nothing Then vpsFS.Close()
                            End Try

                            If IsExpired Then
                                Try
                                    IO.Directory.Delete(Path, True)
                                Catch ex As System.Exception
                                    ' Just Handle Exceptions
                                    ' If unable to remove, just leave the trash where it is...
                                End Try
                            End If
                        Next
                    End If

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

        Public Function PingToRemoteEndPoint() As Boolean Implements [Shared].Service.IVariablePool.PingToRemoteEndPoint
            Return True
        End Function

        Public Overrides Function InitializeLifetimeService() As Object
            Return Nothing
        End Function

        Protected Overrides Sub Finalize()
            Me.ConfirmRegistrations()

            If Not Me._Timer Is Nothing Then
                Me._Timer.Enabled = False
                Me._Timer.Dispose()
            End If

            Try
                If Not Me._RemoteVariablePoolServiceConnection Is Nothing Then _
                    Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(Me._RemoteVariablePoolServiceConnection)
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try

            Try
                If Not Me._RemoteVariablePoolService Is Nothing Then _
                    Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(Me._RemoteVariablePoolService)
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try

            If Not Me._PoolLockFS Is Nothing Then
                Me._PoolLockFS.Close()

                Try
                    IO.File.Delete(Me._PoolLockFileLocation)
                Catch ex As System.Exception
                    ' Just Handle Exceptions
                End Try
            End If

            MyBase.Finalize()
        End Sub
    End Class
End Namespace
