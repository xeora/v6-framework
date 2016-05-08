Namespace Xeora.VSAddIn.Executable
    Public Class Loader
        Inherits MarshalByRefObject
        Implements ILoader

        Private _HandlerGuid As Guid
        Private _FrameworkBinPath As String = Nothing
        Private _DomainDependenciesPath As String = Nothing

        Public Sub New()
            Me._HandlerGuid = Guid.NewGuid()

            AddHandler System.AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve, AddressOf Me._SearchDependencies
        End Sub

        Private Function _SearchDependencies(ByVal sender As Object, ByVal e As System.ResolveEventArgs) As System.Reflection.Assembly
            Dim rAssembly As System.Reflection.Assembly = Nothing

            Dim DllName As String =
                e.Name.Split(","c)(0).Trim()
            Dim DllFileLocation As String =
                IO.Path.Combine(Me._DomainDependenciesPath, String.Format("{0}.dll", DllName))

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

        Private Function GetAssemblyLoaded(ByVal SearchDependenciesPath As String, ByVal AssemblyFileName As String) As System.Reflection.Assembly
            Me._DomainDependenciesPath = SearchDependenciesPath

            ' Take Care The XeoraCube FrameWork Libraries Location
            Dim FrameworkBinLocationDI As IO.DirectoryInfo =
                New IO.DirectoryInfo(SearchDependenciesPath)
            Do
                If String.Compare(FrameworkBinLocationDI.Name, "Domains", True) = 0 Then
                    FrameworkBinLocationDI = FrameworkBinLocationDI.Parent

                    Me._FrameworkBinPath = IO.Path.Combine(FrameworkBinLocationDI.FullName, "bin")

                    Exit Do
                End If

                FrameworkBinLocationDI = FrameworkBinLocationDI.Parent
            Loop Until FrameworkBinLocationDI Is Nothing
            ' !---

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

        Public Function FrameworkArchitecture(ByVal FrameworkBinLocation As String) As Reflection.ProcessorArchitecture Implements ILoader.FrameworkArchitecture
            Dim PA As Reflection.ProcessorArchitecture = Reflection.ProcessorArchitecture.None

            If IO.Directory.Exists(FrameworkBinLocation) Then
                Me._FrameworkBinPath = FrameworkBinLocation

                For Each LibFile As String In IO.Directory.GetFiles(FrameworkBinLocation, "*.dll")
                    Try
                        ' Xeora.Web.RegularExpressions file is always x86 because of that, Ignore...
                        If LibFile.IndexOf("Xeora.Web.RegularExpressions") > -1 Then Continue For

                        Dim Assembly As Reflection.Assembly =
                            System.Reflection.Assembly.ReflectionOnlyLoadFrom(
                                Me.CopyAssembly(LibFile))

                        If Assembly.GetName().ProcessorArchitecture = Reflection.ProcessorArchitecture.Amd64 Then
                            PA = Assembly.GetName().ProcessorArchitecture
                        Else
                            PA = Reflection.ProcessorArchitecture.X86

                            Exit For
                        End If
                    Catch ex As Exception
                        PA = Reflection.ProcessorArchitecture.None
                    End Try
                Next
            End If

            Return PA
        End Function

        Public Function GetAssemblies(ByVal SearchPath As String) As String() Implements ILoader.GetAssemblies
            Dim rStringList As New Generic.List(Of String)

            Dim AssemblyID As String
            Dim AssemblyDll As System.Reflection.Assembly
            Dim DllFileNames As String() =
                IO.Directory.GetFiles(SearchPath, "*.dll")

            For Each DllFileLocation As String In DllFileNames
                AssemblyID = IO.Path.GetFileNameWithoutExtension(DllFileLocation)
                AssemblyDll = Me.GetAssemblyLoaded(SearchPath, IO.Path.GetFileName(DllFileLocation))

                If Not AssemblyDll Is Nothing Then
                    Dim Type As Type = AssemblyDll.GetType(String.Format("Xeora.Domain.{0}", AssemblyID))

                    If Not Type Is Nothing AndAlso Not Type.GetInterface("Xeora.Web.Shared.IDomainExecutable") Is Nothing Then _
                        rStringList.Add(AssemblyID)
                End If
            Next

            Return rStringList.ToArray()
        End Function

        Public Function GetAssemblyClasses(ByVal AssemblyFileLocation As String, Optional ByVal ClassIDs As String() = Nothing) As String() Implements ILoader.GetAssemblyClasses
            Dim rStringList As New List(Of String)

            Dim AssemblyID As String = IO.Path.GetFileNameWithoutExtension(AssemblyFileLocation)
            Dim AssemblyDll As Reflection.Assembly =
                Me.GetAssemblyLoaded(
                    IO.Path.GetDirectoryName(AssemblyFileLocation),
                    IO.Path.GetFileName(AssemblyFileLocation)
                )

            If AssemblyDll Is Nothing Then
                Throw New IO.FileNotFoundException()
            Else
                Dim AssemblyClasses As Type() =
                    AssemblyDll.GetTypes()

                For Each BaseClass As Type In AssemblyClasses
                    If String.Compare(BaseClass.Namespace, "Xeora.Domain") = 0 Then
                        If ClassIDs Is Nothing OrElse ClassIDs.Length = 0 Then
                            If String.Compare(BaseClass.Name, AssemblyID) <> 0 Then _
                                rStringList.Add(BaseClass.Name)
                        Else
                            If String.Compare(BaseClass.Name, ClassIDs(0)) = 0 Then
                                Dim SearchingClass As Type = BaseClass

                                For cC As Integer = 1 To ClassIDs.Length - 1
                                    SearchingClass = SearchingClass.GetNestedType(ClassIDs(cC))

                                    If SearchingClass Is Nothing Then Exit For
                                Next

                                If Not SearchingClass Is Nothing Then
                                    For Each nT As Type In SearchingClass.GetNestedTypes()
                                        If nT.IsNestedPublic Then rStringList.Add(nT.Name)
                                    Next
                                End If

                                Exit For
                            End If
                        End If
                    End If
                Next
            End If

            Return rStringList.ToArray()
        End Function

        Public Function GetAssemblyClassFunctions(ByVal AssemblyFileLocation As String, ByVal ClassIDs As String()) As Object() Implements ILoader.GetAssemblyClassFunctions
            Dim rObjectList As New Generic.List(Of Object()), tStringList As Generic.List(Of String)

            Dim AssemblyID As String = IO.Path.GetFileNameWithoutExtension(AssemblyFileLocation)
            Dim AssemblyDll As System.Reflection.Assembly =
                Me.GetAssemblyLoaded(
                    IO.Path.GetDirectoryName(AssemblyFileLocation),
                    IO.Path.GetFileName(AssemblyFileLocation)
                )

            If AssemblyDll Is Nothing Then
                Throw New IO.FileNotFoundException()
            Else
                Dim AssemblyClasses As Type() =
                    AssemblyDll.GetTypes()

                For Each BaseClass As Type In AssemblyClasses
                    If String.Compare(BaseClass.Namespace, "Xeora.Domain") = 0 Then
                        Dim SearchingClass As Type = Nothing

                        If ClassIDs Is Nothing OrElse ClassIDs.Length = 0 Then
                            If String.Compare(BaseClass.Name, AssemblyID) = 0 Then _
                                SearchingClass = BaseClass
                        Else
                            If String.Compare(BaseClass.Name, ClassIDs(0)) = 0 Then
                                SearchingClass = BaseClass

                                For cC As Integer = 1 To ClassIDs.Length - 1
                                    SearchingClass = SearchingClass.GetNestedType(ClassIDs(cC))

                                    If SearchingClass Is Nothing Then Exit For
                                Next
                            End If
                        End If


                        If Not SearchingClass Is Nothing Then
                            For Each mI As Reflection.MethodInfo In SearchingClass.GetMethods()
                                If mI.IsPublic AndAlso mI.IsStatic Then
                                    tStringList = New List(Of String)

                                    Try
                                        For Each pI As System.Reflection.ParameterInfo In mI.GetParameters()
                                            tStringList.Add(pI.Name)
                                        Next
                                    Catch ex As Exception
                                        tStringList.Add("~PARAMETERSARENOTCOMPILED~")
                                    End Try

                                    rObjectList.Add(New Object() {mI.Name, tStringList.ToArray()})
                                End If
                            Next

                            Exit For
                        End If
                    End If
                Next
            End If

            Return rObjectList.ToArray()
        End Function
    End Class
End Namespace