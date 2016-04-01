Namespace Xeora.VSAddIn
    Public Interface IAddInLoader
        Function GetAssemblies(ByVal sT As AddInLoader.SearchTypes, ByVal SearchPath As String) As String()
        Function GetAssembliesClasses(ByVal sT As AddInLoader.SearchTypes, ByVal AssemblyFileLocation As String) As String()
        Function GetAssembliesClassFunctions(ByVal sT As AddInLoader.SearchTypes, ByVal AssemblyFileLocation As String, ByVal ClassID As String) As Object()
    End Interface

    Public Class AddInLoader
        Inherits MarshalByRefObject
        Implements IAddInLoader

        Private _HandlerGuid As Guid
        Private _SearchType As SearchTypes = SearchTypes.Domain
        Private _FrameworkBinPath As String = Nothing
        Private _DomainDependenciesPath As String = Nothing
        Private _AddonDependenciesPath As String = Nothing

        Public Enum SearchTypes
            Domain
            Child
        End Enum

        Public Sub New()
            Me._HandlerGuid = Guid.NewGuid()

            AddHandler System.AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve, AddressOf Me._SearchDependencies
        End Sub

        Private Function _SearchDependencies(ByVal sender As Object, ByVal e As System.ResolveEventArgs) As System.Reflection.Assembly
            Dim rAssembly As System.Reflection.Assembly = Nothing

            Dim DllFileLocation As String = String.Empty
            Dim DllName As String =
                e.Name.Split(","c)(0).Trim()

            Select Case Me._SearchType
                Case SearchTypes.Child
                    DllFileLocation = IO.Path.Combine(Me._AddonDependenciesPath, String.Format("{0}.dll", DllName))

                    If Not IO.File.Exists(DllFileLocation) Then GoTo DOMAINSEARCH
                Case SearchTypes.Domain
DOMAINSEARCH:
                    DllFileLocation = IO.Path.Combine(Me._DomainDependenciesPath, String.Format("{0}.dll", DllName))
            End Select

            If Not IO.File.Exists(DllFileLocation) Then
                DllFileLocation = IO.Path.Combine(
                                    Me._FrameworkBinPath,
                                    String.Format("{0}.dll", DllName))

                If Not IO.File.Exists(DllFileLocation) Then DllFileLocation = String.Empty
            End If

            If Not String.IsNullOrEmpty(DllFileLocation) Then
                rAssembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(Me.CopyAssembly(DllFileLocation))
            Else
                Try
                    rAssembly = System.Reflection.Assembly.ReflectionOnlyLoad(e.Name)
                Catch ex As Exception
                    rAssembly = Nothing
                End Try
            End If

            Return rAssembly
        End Function

        Private _IsTempLocationGenerated As Boolean = False
        Private Function PrepareTempLocation() As String
            Dim rString As String

            rString = IO.Path.Combine(Environment.GetEnvironmentVariable("TEMP"), String.Format("XeoraCubeAddInTemp\{0}", Me._HandlerGuid.ToString()))

            If Not Me._IsTempLocationGenerated Then
                If IO.Directory.Exists(rString) Then
                    Me._HandlerGuid = Guid.NewGuid()

                    rString = Me.PrepareTempLocation()
                Else
                    IO.Directory.CreateDirectory(rString)

                    Me._IsTempLocationGenerated = True
                End If
            End If

            Return rString
        End Function

        Private Function CopyAssembly(ByVal AssemblyFileLocation As String) As String
            Dim rString As String =
                IO.Path.Combine(Me.PrepareTempLocation(), IO.Path.GetFileName(AssemblyFileLocation))

            If Not IO.File.Exists(rString) Then IO.File.Copy(AssemblyFileLocation, rString)

            Return rString
        End Function

        Private Function GetAssemblyLoaded(ByVal sT As SearchTypes, ByVal SearchDependenciesPath As String, ByVal AssemblyFileName As String) As System.Reflection.Assembly
            Me._SearchType = sT

            Select Case Me._SearchType
                Case SearchTypes.Child
                    Me._AddonDependenciesPath = SearchDependenciesPath

                    Dim SearchingDirectory As String = SearchDependenciesPath
                    Dim DomainExecutablesFound As Boolean = False
                    Dim FrameWorkBinPathFound As Boolean = False

                    Do
                        For Each Directory As String In IO.Directory.GetDirectories(SearchingDirectory)
                            If String.Compare(IO.Path.GetFileName(Directory), "Executables") = 0 Then
                                Me._DomainDependenciesPath = Directory

                                DomainExecutablesFound = True
                            End If

                            If String.Compare(IO.Path.GetFileName(Directory), "Domains") = 0 Then
                                Me._FrameworkBinPath = IO.Path.Combine(IO.Path.GetDirectoryName(Directory), "bin")

                                FrameWorkBinPathFound = True
                            End If
                        Next

                        If Not DomainExecutablesFound OrElse Not FrameWorkBinPathFound Then SearchingDirectory = IO.Path.GetDirectoryName(SearchingDirectory)
                    Loop Until String.IsNullOrEmpty(SearchingDirectory) OrElse (DomainExecutablesFound AndAlso FrameWorkBinPathFound)

                Case SearchTypes.Domain
                    Me._AddonDependenciesPath = Nothing
                    Me._DomainDependenciesPath = SearchDependenciesPath

                    Dim SearchingDirectory As String = SearchDependenciesPath
                    Dim FrameWorkBinPathFound As Boolean = False

                    Do
                        For Each Directory As String In IO.Directory.GetDirectories(SearchingDirectory)
                            If String.Compare(IO.Path.GetFileName(Directory), "Domains") = 0 Then
                                Me._FrameworkBinPath = IO.Path.Combine(IO.Path.GetDirectoryName(Directory), "bin")

                                FrameWorkBinPathFound = True
                            End If
                        Next

                        If Not FrameWorkBinPathFound Then SearchingDirectory = IO.Path.GetDirectoryName(SearchingDirectory)
                    Loop Until String.IsNullOrEmpty(SearchingDirectory) OrElse FrameWorkBinPathFound
            End Select

            Dim rAssembly As System.Reflection.Assembly = Nothing
            Dim AssemblyName As System.Reflection.AssemblyName = Nothing

            Try
                AssemblyName =
                    System.Reflection.AssemblyName.GetAssemblyName(
                        IO.Path.Combine(SearchDependenciesPath, AssemblyFileName)
                    )
            Catch ex As Exception
                ' It is probably not an .net dll.
                AssemblyName = Nothing
            End Try

            If Not AssemblyName Is Nothing Then
                For Each asm As System.Reflection.Assembly In System.AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                    If String.Compare(asm.GetName().Name, AssemblyName.Name, True) = 0 Then
                        rAssembly = asm

                        Exit For
                    End If
                Next

                If rAssembly Is Nothing Then
                    rAssembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(
                                    Me.CopyAssembly(
                                        IO.Path.Combine(SearchDependenciesPath, AssemblyFileName)
                                    )
                                )
                End If
            End If

            Return rAssembly
        End Function

        Public Function GetAssemblies(ByVal sT As SearchTypes, ByVal SearchPath As String) As String() Implements IAddInLoader.GetAssemblies
            Dim rStringList As New Generic.List(Of String)

            Dim AssemblyID As String
            Dim AssemblyDll As System.Reflection.Assembly
            Dim DllFileNames As String() =
                IO.Directory.GetFiles(SearchPath, "*.dll")

            For Each DllFileLocation As String In DllFileNames
                AssemblyID = IO.Path.GetFileNameWithoutExtension(DllFileLocation)
                AssemblyDll = Me.GetAssemblyLoaded(sT, SearchPath, IO.Path.GetFileName(DllFileLocation))

                If Not AssemblyDll Is Nothing Then
                    Dim Type As Type = AssemblyDll.GetType(String.Format("Xeora.Domain.{0}", AssemblyID))

                    If Not Type Is Nothing AndAlso Not Type.GetInterface("Xeora.Web.Shared.IDomainExecutable") Is Nothing Then _
                        rStringList.Add(AssemblyID)
                End If
            Next

            Return rStringList.ToArray()
        End Function

        Public Function GetAssembliesClasses(ByVal sT As SearchTypes, ByVal AssemblyFileLocation As String) As String() Implements IAddInLoader.GetAssembliesClasses
            Dim rStringList As New Generic.List(Of String)

            Dim AssemblyID As String = IO.Path.GetFileNameWithoutExtension(AssemblyFileLocation)
            Dim AssemblyDll As System.Reflection.Assembly =
                Me.GetAssemblyLoaded(
                    sT,
                    IO.Path.GetDirectoryName(AssemblyFileLocation),
                    IO.Path.GetFileName(AssemblyFileLocation)
                )

            If AssemblyDll Is Nothing Then
                Throw New IO.FileNotFoundException()
            Else
                For Each nT As System.Type In AssemblyDll.GetType(String.Format("Xeora.Domain.{0}", AssemblyID)).GetNestedTypes()
                    If nT.IsNestedPublic Then rStringList.Add(nT.Name)
                Next
            End If

            Return rStringList.ToArray()
        End Function

        Public Function GetAssembliesClassFunctions(ByVal sT As SearchTypes, ByVal AssemblyFileLocation As String, ByVal ClassID As String) As Object() Implements IAddInLoader.GetAssembliesClassFunctions
            Dim rObjectList As New Generic.List(Of Object()), tStringList As Generic.List(Of String)

            Dim AssemblyID As String = IO.Path.GetFileNameWithoutExtension(AssemblyFileLocation)
            Dim AssemblyDll As System.Reflection.Assembly =
                Me.GetAssemblyLoaded(
                    sT,
                    IO.Path.GetDirectoryName(AssemblyFileLocation),
                    IO.Path.GetFileName(AssemblyFileLocation)
                )

            If AssemblyDll Is Nothing Then
                Throw New IO.FileNotFoundException()
            Else
                For Each mI As System.Reflection.MethodInfo In AssemblyDll.GetType(String.Format("Xeora.Domain.{0}+{1}", AssemblyID, ClassID)).GetMethods()
                    If mI.IsPublic AndAlso mI.IsStatic Then
                        tStringList = New Generic.List(Of String)

                        Try
                            For Each pI As System.Reflection.ParameterInfo In mI.GetParameters()
                                tStringList.Add(pI.Name)
                            Next
                        Catch ex As Exception
                            tStringList.Add("~PARAMETERSNOTCOMPILED~")
                        End Try

                        rObjectList.Add(New Object() {mI.Name, tStringList.ToArray()})
                    End If
                Next
            End If

            Return rObjectList.ToArray()
        End Function
    End Class
End Namespace