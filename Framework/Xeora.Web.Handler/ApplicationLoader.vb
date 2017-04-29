Option Strict On

Namespace Xeora.Web.Handler
    Friend Class ApplicationLoader
        Private _CacheRootLocation As String = String.Empty

        Private _ApplicationID As String = String.Empty
        Private _ApplicationLocation As String = String.Empty

        Public Sub New()
            Me._CacheRootLocation =
                IO.Path.Combine(
                    [Shared].Configurations.TemporaryRoot,
                    [Shared].Configurations.WorkingPath.WorkingPathID
                )
            If Not IO.Directory.Exists(Me._CacheRootLocation) Then _
                IO.Directory.CreateDirectory(Me._CacheRootLocation)
        End Sub

        Private Shared _Current As ApplicationLoader = Nothing
        Public Shared ReadOnly Property Current As ApplicationLoader
            Get
                If ApplicationLoader._Current Is Nothing Then _
                    ApplicationLoader._Current = New ApplicationLoader()

                Return ApplicationLoader._Current
            End Get
        End Property

        Public Sub Initialize()
            Me.LoadApplication()

            Dim FileSystemWatcher As New IO.FileSystemWatcher()

            With FileSystemWatcher
                .Path = IO.Path.GetFullPath(
                            IO.Path.Combine(
                                [Shared].Configurations.PhysicalRoot,
                                [Shared].Configurations.ApplicationRoot.FileSystemImplementation,
                                "Domains"
                            )
                        )
                .IncludeSubdirectories = True
                .Filter = "*.dll"
                .NotifyFilter = IO.NotifyFilters.LastWrite
                .EnableRaisingEvents = True
            End With
            AddHandler FileSystemWatcher.Changed, AddressOf Me.FilesModified
        End Sub

        Public ReadOnly Property ApplicationID As String
            Get
                Return Me._ApplicationID
            End Get
        End Property

        Private _LoaderThreadSync As New Object()
        Private _LoaderThread As Threading.Thread = Nothing
        Private Sub FilesModified(ByVal sender As Object, ByVal e As IO.FileSystemEventArgs)
            If Not Me._LoaderThread Is Nothing Then Exit Sub

            Threading.Monitor.Enter(Me._LoaderThreadSync)
            Try
                If Me._LoaderThread Is Nothing Then
                    Me._LoaderThread = New Threading.Thread(AddressOf Me.LoadApplication)
                    Me._LoaderThread.IsBackground = True
                    Me._LoaderThread.Start()
                End If
            Finally
                Threading.Monitor.Exit(Me._LoaderThreadSync)
            End Try
        End Sub

        Private Sub LoadApplication()
            Try
                Me.CleanUp()

                Me._ApplicationID = Guid.NewGuid().ToString()
                Me._ApplicationLocation =
                    IO.Path.Combine(Me._CacheRootLocation, Me._ApplicationID)
                If Not IO.Directory.Exists(Me._ApplicationLocation) Then _
                    IO.Directory.CreateDirectory(Me._ApplicationLocation)

                Dim DefaultDomainRootLocation As String =
                    IO.Path.GetFullPath(
                        IO.Path.Combine(
                            [Shared].Configurations.PhysicalRoot,
                            [Shared].Configurations.ApplicationRoot.FileSystemImplementation,
                            "Domains"
                        )
                    )
                Me.LoadDomainExecutables(DefaultDomainRootLocation)
            Catch ex As System.Exception
                Throw New System.Exception(String.Format("{0}!", [Global].SystemMessages.SYSTEM_APPLICATIONLOADINGERROR), ex)
            End Try

            ' Sleep to Prevent to run LoadApplication again.
            Threading.Thread.Sleep(10000) : Me._LoaderThread = Nothing
        End Sub

        Private Sub LoadDomainExecutables(ByVal DomainRootPath As String)
            Dim DomainsDI As New IO.DirectoryInfo(DomainRootPath)

            For Each DomainDI As IO.DirectoryInfo In DomainsDI.GetDirectories()
                Dim DomainExecutablesLocation As String =
                    IO.Path.Combine(DomainDI.FullName, "Executables")

                Dim DomainExecutablesDI As New IO.DirectoryInfo(DomainExecutablesLocation)

                If DomainExecutablesDI.Exists Then
                    For Each ExecutableFI As IO.FileInfo In DomainExecutablesDI.GetFiles()
                        Dim ApplicationLocationFI As IO.FileInfo =
                            New IO.FileInfo(IO.Path.Combine(Me._ApplicationLocation, ExecutableFI.Name))

                        If Not ApplicationLocationFI.Exists Then
                            Try
                                ExecutableFI.CopyTo(ApplicationLocationFI.FullName, True)
                            Catch ex As System.Exception
                                ' Just Handle Exceptions
                            End Try
                        End If
                    Next
                End If

                Dim DomainChildrenDI As IO.DirectoryInfo =
                    New IO.DirectoryInfo(IO.Path.Combine(DomainDI.FullName, "Addons"))

                If DomainChildrenDI.Exists Then Me.LoadDomainExecutables(DomainChildrenDI.FullName)
            Next
        End Sub

        Public Sub CleanUp()
            Manager.Assembly.ClearCache()

            Dim CacheRootDI As New IO.DirectoryInfo(Me._CacheRootLocation)

            If Not CacheRootDI.Exists Then Exit Sub

            For Each ApplicationDI As IO.DirectoryInfo In CacheRootDI.GetDirectories()
                If ApplicationDI.Name.Equals("PoolSessions") OrElse ApplicationDI.Name.Equals(Me._ApplicationID) Then Continue For

                ' Check if all files are in use
                Dim IsRemovable As Boolean = True

                For Each FileInfo As IO.FileInfo In ApplicationDI.GetFiles()
                    Dim CheckFS As IO.FileStream = Nothing

                    Try
                        CheckFS = FileInfo.OpenRead()
                    Catch ex As System.Exception
                        IsRemovable = False

                        Exit For
                    Finally
                        If Not CheckFS Is Nothing Then _
                            CheckFS.Close()
                    End Try
                Next

                If IsRemovable Then
                    Try
                        ApplicationDI.Delete(True)
                    Catch ex As System.Exception
                        ' Just Handle Exceptions
                    End Try
                End If
            Next
        End Sub
    End Class
End Namespace