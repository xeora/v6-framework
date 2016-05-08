Namespace Xeora.Web.Manager
    <System.Serializable()>
    Public Class Loader
        Inherits System.MarshalByRefObject
        Implements System.IDisposable

        Private _ExecutablesPath As String
        Private _ExecutableName As String

        Private _PostBackPath As String
        Private _AssemblyDll As System.Reflection.Assembly
        Private _XeoraControls As System.Type() = Nothing

        Private _MissingFileException As Boolean = False

        Private ExecutableInstance As Object = Nothing

        Public Sub New(ByVal ExecutablesPath As String, ByVal ExecutableName As String)
            If String.IsNullOrEmpty(ExecutablesPath) Then Throw New System.ArgumentNullException("ExecutablesPath must not be null!")
            If String.IsNullOrEmpty(ExecutableName) Then Throw New System.ArgumentNullException("ExecutableName must not be null!")

            AddHandler System.AppDomain.CurrentDomain.AssemblyResolve, AddressOf Me.AssemblyResolve

            Me._ExecutablesPath = ExecutablesPath
            Me._ExecutableName = ExecutableName

            Me._PostBackPath = System.IO.Path.Combine(
                                        Me._ExecutablesPath,
                                        String.Format("{0}.dll", Me._ExecutableName)
                                    )

            Try
                Me._AssemblyDll = System.Reflection.Assembly.LoadFile(Me._PostBackPath)
            Catch ex As System.IO.FileNotFoundException
                Me._MissingFileException = True
            Catch ex As System.Exception
                Throw
            End Try

            If Not Me._MissingFileException Then
                Try
                    Dim LoadedAssemblies As System.Reflection.Assembly() =
                        System.AppDomain.CurrentDomain.GetAssemblies()

                    If Not LoadedAssemblies Is Nothing Then
                        For Each Assembly As System.Reflection.Assembly In LoadedAssemblies
                            If String.Compare(Assembly.GetName().Name, "Xeora.Web.Shared") = 0 Then
                                Dim XeoraControlTypes As New System.Collections.Generic.List(Of System.Type)

                                For Each Type As System.Type In Assembly.GetTypes()
                                    If String.Compare(Type.Namespace, "Xeora.Web.Shared.ControlResult") = 0 Then _
                                        XeoraControlTypes.Add(Type)
                                Next

                                Me._XeoraControls = XeoraControlTypes.ToArray()

                                Exit For
                            End If
                        Next
                    End If
                Catch ex As System.Exception
                    ' Just Handle Exceptions
                End Try
            End If
        End Sub

        Private Function AssemblyResolve(ByVal sender As Object, ByVal args As System.ResolveEventArgs) As System.Reflection.Assembly
            Dim rAssembly As System.Reflection.Assembly = Nothing

            Try
                Dim AssemblyShortName As String =
                    args.Name.Substring(0, args.Name.IndexOf(","c))
                Dim AssemblyLocation As String =
                    System.IO.Path.Combine(
                        Me._ExecutablesPath,
                        String.Format("{0}.dll", AssemblyShortName)
                    )

                If System.IO.File.Exists(AssemblyLocation) Then
                    rAssembly = System.Reflection.Assembly.LoadFile(AssemblyLocation)
                Else
                    Dim DomainAssemblies As System.Reflection.Assembly() =
                        System.AppDomain.CurrentDomain.GetAssemblies()

                    For Each DomainAssembly As System.Reflection.Assembly In DomainAssemblies
                        If String.Compare(args.Name, DomainAssembly.FullName) = 0 Then
                            rAssembly = DomainAssembly

                            Exit For
                        End If
                    Next
                End If
            Catch ex As System.IO.FileNotFoundException
                Me._MissingFileException = True
            Catch ex As System.Exception
                Throw
            End Try

            Return rAssembly
        End Function

        Public ReadOnly Property MissingFileException() As Boolean
            Get
                Return Me._MissingFileException
            End Get
        End Property

        Public ReadOnly Property AssemblyName() As System.Reflection.AssemblyName
            Get
                Return Me._AssemblyDll.GetName()
            End Get
        End Property

        Public Function ExamInterface(ByVal InterfaceFullName As String) As Boolean
            Dim rBoolean As Boolean = False, AssemblyInterfaceList As System.Type()

            Dim AssemblyExTypes As System.Type() = Me._AssemblyDll.GetExportedTypes()

            If Not AssemblyExTypes Is Nothing Then
                For Each type As System.Type In AssemblyExTypes
                    AssemblyInterfaceList = type.GetInterfaces()

                    If Not AssemblyInterfaceList Is Nothing Then
                        For Each [interface] As System.Type In AssemblyInterfaceList
                            If String.Compare(
                                [interface].FullName, InterfaceFullName) = 0 Then

                                rBoolean = True

                                Exit For
                            End If
                        Next
                    End If

                    If rBoolean Then Exit For
                Next
            End If

            Return rBoolean
        End Function

        Public Function Invoke(ByVal ClassNames As String(), ByVal FunctionName As String, ByVal FunctionParams As Object(), ByVal ExecuterType As String) As Object
            Return Me.InvokeInternal(ClassNames, FunctionName, FunctionParams, ExecuterType)
        End Function

        Private Function InvokeInternal(ByVal ClassNames As String(), ByVal FunctionName As String, ByVal FunctionParams As Object(), ByVal ExecuterType As String) As Object
            If String.IsNullOrEmpty(FunctionName) Then Throw New System.ArgumentNullException("FunctionName must be defined!")
            If FunctionParams Is Nothing Then FunctionParams = New Object() {}

            Dim CompileErrorObject As String

            If Not ClassNames Is Nothing Then
                If FunctionParams.Length = 0 Then
                    CompileErrorObject = String.Format("{0}?{1}.{2}", Me._ExecutableName, String.Join(".", ClassNames), FunctionName)
                Else
                    CompileErrorObject = String.Format("{0}?{1}.{2},[Length:{3}]", Me._ExecutableName, String.Join(".", ClassNames), FunctionName, FunctionParams.Length)
                End If
            Else
                If FunctionParams.Length = 0 Then
                    CompileErrorObject = String.Format("{0}?{1}", Me._ExecutableName, FunctionName)
                Else
                    CompileErrorObject = String.Format("{0}?{1},[Length:{2}]", Me._ExecutableName, FunctionName, FunctionParams.Length)
                End If
            End If

            Dim rObject As Object = New System.Exception(String.Format("Executable Execution Error! RequestInfo: {0}", CompileErrorObject))

            Try
                Dim ExamInterface As System.Type =
                    Me._AssemblyDll.GetType(String.Format("Xeora.Domain.{0}", Me._ExecutableName), False, True)

                If ExamInterface Is Nothing Then
                    Throw New System.Exception("Assembly does not belong to any XeoraCube Domain or Addon!")
                Else
                    Dim InterfaceType As System.Type =
                        ExamInterface.GetInterface("IDomainExecutable", True)

                    If InterfaceType Is Nothing OrElse
                        Not InterfaceType.IsInterface OrElse
                        String.Compare(InterfaceType.FullName, "Xeora.Web.Shared.IDomainExecutable") <> 0 Then

                        Throw New System.Exception("Calling Assembly is not a XeoraCube Executable!")
                    Else
                        If ExecutableInstance Is Nothing Then
                            Try
                                ExecutableInstance = System.Activator.CreateInstance(ExamInterface)
                            Catch ex As System.Exception
                                Throw New System.Exception("Unable to create an instance of XeoraCube Executable!", ex)
                            End Try

                            ExecutableInstance.GetType().GetMethod("FirstTouch").Invoke(ExecutableInstance, Nothing)
                        End If
                    End If
                End If

                Dim AssemblyObject As System.Type, AssemblyMethod As System.Reflection.MethodInfo = Nothing

                If Not ClassNames Is Nothing Then
                    AssemblyObject = Me._AssemblyDll.GetType(String.Format("Xeora.Domain.{0}", String.Join("+", ClassNames)), True, True)
                Else
                    AssemblyObject = Me._AssemblyDll.GetType(String.Format("Xeora.Domain.{0}", Me._ExecutableName), True, True)
                End If

                AssemblyMethod = Me.GetAssemblyMethod(AssemblyObject, FunctionName, FunctionParams, ExecuterType)

                If Not AssemblyMethod Is Nothing Then
                    Dim ExecutionID As String = System.Guid.NewGuid().ToString()

                    ExecutableInstance.GetType().GetMethod("BeforeExecute").Invoke(ExecutableInstance, New Object() {ExecutionID, AssemblyMethod})

                    rObject = AssemblyMethod.Invoke(
                                        AssemblyObject,
                                        System.Reflection.BindingFlags.DeclaredOnly Or
                                        System.Reflection.BindingFlags.InvokeMethod,
                                        Nothing,
                                        FunctionParams,
                                        System.Threading.Thread.CurrentThread.CurrentCulture
                                    )

                    ExecutableInstance.GetType().GetMethod("AfterExecute").Invoke(ExecutableInstance, New Object() {ExecutionID, rObject})
                Else
                    Dim sB As New System.Text.StringBuilder

                    sB.AppendLine("Assembly does not have following procedure!")
                    sB.AppendLine("--------------------------------------------------")
                    sB.AppendFormat("ExecutableName: {0}", Me._ExecutableName) : sB.AppendLine()
                    sB.AppendFormat("ClassName: {0}", String.Join(".", ClassNames)) : sB.AppendLine()
                    sB.AppendFormat("FunctionName: {0}", FunctionName) : sB.AppendLine()
                    sB.AppendFormat("FunctionParamsLength: {0}",
                        Microsoft.VisualBasic.IIf(FunctionParams Is Nothing, "0", FunctionParams.Length)) : sB.AppendLine()

                    Throw New System.Reflection.TargetException(sB.ToString())
                End If
            Catch ex As System.Exception
                rObject = ex
            End Try

            Return rObject
        End Function

        Private Function GetAssemblyMethod(ByRef AssemblyObject As System.Type, ByVal FunctionName As String, ByRef FunctionParams As Object(), ByVal ExecuterType As String) As System.Reflection.MethodInfo
            Dim rAssemblyMethod As System.Reflection.MethodInfo = Nothing

            ' Sort and Filter Searching Function
            Dim AOMIs As System.Reflection.MethodInfo() = AssemblyObject.GetMethods()
            System.Array.Sort(AOMIs, New MethodInfoNameComparer(Nothing))

            Dim mIF As New MethodInfoFinder(FunctionName)
            Dim MIFIdx As Integer =
                System.Array.FindIndex(
                    AOMIs,
                    New System.Predicate(Of System.Reflection.MethodInfo)(AddressOf mIF.MethodInfoFinder)
                )
            Dim MILIdx As Integer =
                System.Array.FindLastIndex(
                    AOMIs,
                    New System.Predicate(Of System.Reflection.MethodInfo)(AddressOf mIF.MethodInfoFinder)
                )

            If MIFIdx > -1 AndAlso MILIdx >= MIFIdx Then
                Dim MethodInfos As System.Reflection.MethodInfo() =
                    CType(
                        System.Array.CreateInstance(
                            GetType(System.Reflection.MethodInfo),
                            (MILIdx - MIFIdx) + 1
                        ),
                        System.Reflection.MethodInfo()
                    )
                System.Array.Copy(AOMIs, MIFIdx, MethodInfos, 0, MethodInfos.Length)
                System.Array.Sort(MethodInfos, New MethodInfoParameterLengthComparer)

                Dim FunctionParams_ReBuild As Object()

                For mC As Integer = 0 To MethodInfos.Length - 1
                    FunctionParams_ReBuild =
                        CType(System.Array.CreateInstance(GetType(Object), FunctionParams.Length), Object())
                    System.Array.Copy(FunctionParams, FunctionParams_ReBuild, FunctionParams.Length)

                    Dim IsXeoraControl As Boolean =
                        Me.CheckFunctionResultTypeIsXeoraControl(MethodInfos(mC).ReturnType)
                    Dim mParams As System.Reflection.ParameterInfo() =
                        MethodInfos(mC).GetParameters()

                    Select Case ExecuterType
                        Case "Control"
                            If Not IsXeoraControl Then Continue For
                        Case "Other"
                            If IsXeoraControl Then
                                Select Case MethodInfos(mC).ReturnType.Name
                                    Case "RedirectOrder", "Message"
                                        ' These are exceptional controls
                                    Case Else
                                        Continue For
                                End Select
                            End If
                    End Select

                    If mParams.Length = 0 AndAlso
                        FunctionParams_ReBuild.Length = 0 Then

                        rAssemblyMethod = MethodInfos(mC) : FunctionParams = FunctionParams_ReBuild

                        Exit For
                    ElseIf mParams.Length > 0 AndAlso
                            mParams.Length <= FunctionParams_ReBuild.Length Then

                        Dim MatchComplete As Boolean = False
                        Dim IsExactMatch As Boolean() =
                            CType(System.Array.CreateInstance(GetType(Boolean), mParams.Length), Boolean())

                        For pC As Integer = 0 To mParams.Length - 1
                            If pC = mParams.Length - 1 Then
                                Dim CheckIsParamArrayDefined As Boolean =
                                    System.Attribute.IsDefined(mParams(pC), GetType(System.ParamArrayAttribute))

                                If CheckIsParamArrayDefined Then
                                    Dim ParamArrayValues As System.Array =
                                        System.Array.CreateInstance(
                                                mParams(pC).ParameterType.GetElementType(),
                                                (FunctionParams_ReBuild.Length - mParams.Length) + 1
                                            )

                                    For pavC As Integer = pC To FunctionParams_ReBuild.Length - 1
                                        Me.FixFunctionParameter(
                                            mParams(pC).ParameterType.GetElementType(), FunctionParams_ReBuild(pavC))

                                        ParamArrayValues.SetValue(
                                            FunctionParams_ReBuild(pavC),
                                            pavC - (mParams.Length - 1)
                                        )
                                    Next

                                    System.Array.Resize(FunctionParams_ReBuild, mParams.Length)
                                    FunctionParams_ReBuild(pC) = ParamArrayValues

                                    IsExactMatch(pC) = True : MatchComplete = True
                                Else
                                    IsExactMatch(pC) = Me.FixFunctionParameter(
                                                        mParams(pC).ParameterType, FunctionParams_ReBuild(pC))

                                    If mParams.Length = FunctionParams_ReBuild.Length AndAlso
                                        System.Array.IndexOf(IsExactMatch, False) = -1 Then MatchComplete = True
                                End If
                            Else
                                IsExactMatch(pC) = Me.FixFunctionParameter(
                                                    mParams(pC).ParameterType, FunctionParams_ReBuild(pC))
                            End If
                        Next

                        If MatchComplete AndAlso
                            System.Array.IndexOf(IsExactMatch, False) = -1 Then

                            rAssemblyMethod = MethodInfos(mC) : FunctionParams = FunctionParams_ReBuild

                            Exit For
                        End If
                    End If
                Next
            End If

            If rAssemblyMethod Is Nothing AndAlso
                Not AssemblyObject.BaseType Is Nothing Then

                rAssemblyMethod = Me.GetAssemblyMethod(AssemblyObject.BaseType, FunctionName, FunctionParams, ExecuterType)

                If Not rAssemblyMethod Is Nothing Then AssemblyObject = AssemblyObject.BaseType
            End If

            Return rAssemblyMethod
        End Function

        Private Function FixFunctionParameter(ByVal ParameterType As System.Type, ByRef FunctionParam As Object) As Boolean
            Dim rBoolean As Boolean = False

            If Not FunctionParam Is Nothing Then
                If ParameterType Is FunctionParam.GetType() Then
                    rBoolean = True
                Else
                    If String.Compare(ParameterType.FullName, GetType(Object).FullName, True) <> 0 Then
                        Try
                            FunctionParam = System.Convert.ChangeType(FunctionParam, ParameterType)

                            rBoolean = True
                        Catch ex As System.Exception
                            If TypeOf FunctionParam Is String AndAlso
                                String.IsNullOrEmpty(CType(FunctionParam, String)) AndAlso
                                (
                                    ParameterType.Equals(GetType(Byte)) OrElse
                                    ParameterType.Equals(GetType(SByte)) OrElse
                                    ParameterType.Equals(GetType(Short)) OrElse
                                    ParameterType.Equals(GetType(UShort)) OrElse
                                    ParameterType.Equals(GetType(Integer)) OrElse
                                    ParameterType.Equals(GetType(UInteger)) OrElse
                                    ParameterType.Equals(GetType(Long)) OrElse
                                    ParameterType.Equals(GetType(ULong)) OrElse
                                    ParameterType.Equals(GetType(Double)) OrElse
                                    ParameterType.Equals(GetType(Single))
                                ) Then

                                FunctionParam = 0

                                rBoolean = True
                            End If
                        End Try
                    Else
                        rBoolean = True
                    End If
                End If
            Else
                If ParameterType.Equals(GetType(Byte)) OrElse
                    ParameterType.Equals(GetType(SByte)) OrElse
                    ParameterType.Equals(GetType(Short)) OrElse
                    ParameterType.Equals(GetType(UShort)) OrElse
                    ParameterType.Equals(GetType(Integer)) OrElse
                    ParameterType.Equals(GetType(UInteger)) OrElse
                    ParameterType.Equals(GetType(Long)) OrElse
                    ParameterType.Equals(GetType(ULong)) OrElse
                    ParameterType.Equals(GetType(Double)) OrElse
                    ParameterType.Equals(GetType(Single)) Then

                    FunctionParam = 0
                End If

                rBoolean = True
            End If

            Return rBoolean
        End Function

        Private Function CheckFunctionResultTypeIsXeoraControl(ByVal MethodReturnType As System.Type) As Boolean
            Dim rResult As Boolean = False

            If Not Me._XeoraControls Is Nothing AndAlso
                Not MethodReturnType Is Nothing Then

                For Each XeoraType As System.Type In Me._XeoraControls
                    If XeoraType Is MethodReturnType Then rResult = True : Exit For
                Next
            End If

            Return rResult
        End Function

        Private Class MethodInfoFinder
            Private _SearchName As String

            Public Sub New(ByVal SearchName As String)
                Me._SearchName = SearchName
            End Sub

            Public Function MethodInfoFinder(ByVal mI As System.Reflection.MethodInfo) As Boolean
                Return String.Compare(Me._SearchName, mI.Name, True) = 0
            End Function
        End Class

        Private Class MethodInfoNameComparer
            Implements System.Collections.IComparer

            Private _CompareCultureInfo As System.Globalization.CultureInfo

            Public Sub New(ByVal CultureInfo As System.Globalization.CultureInfo)
                If CultureInfo Is Nothing Then
                    Me._CompareCultureInfo = New System.Globalization.CultureInfo("en-US")
                Else
                    Me._CompareCultureInfo = CultureInfo
                End If
            End Sub

            Public Overloads Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim x_obj As System.Reflection.MethodInfo = CType(x, System.Reflection.MethodInfo)
                Dim y_obj As System.Reflection.MethodInfo = CType(y, System.Reflection.MethodInfo)

                Return String.Compare(x_obj.Name, y_obj.Name,
                            Me._CompareCultureInfo,
                            System.Globalization.CompareOptions.IgnoreCase
                        )
            End Function
        End Class

        Private Class MethodInfoParameterLengthComparer
            Implements System.Collections.IComparer

            Public Overloads Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim x_obj As System.Reflection.MethodInfo = CType(x, System.Reflection.MethodInfo)
                Dim y_obj As System.Reflection.MethodInfo = CType(y, System.Reflection.MethodInfo)

                Dim x_obj_params As System.Reflection.ParameterInfo() = x_obj.GetParameters()
                Dim y_obj_params As System.Reflection.ParameterInfo() = y_obj.GetParameters()

                Dim rResult As Integer = 0

                If x_obj_params.Length > y_obj_params.Length Then
                    rResult = 1
                ElseIf x_obj_params.Length < y_obj_params.Length Then
                    rResult = -1
                End If

                Return rResult
            End Function
        End Class

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If Not ExecutableInstance Is Nothing Then _
                        ExecutableInstance.GetType().GetMethod("LastTouch").Invoke(ExecutableInstance, Nothing)
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements System.IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace