Namespace SolidDevelopment.Web.Managers
    <System.Serializable()> _
    Public Class PlugInsLoader
        Inherits System.MarshalByRefObject

        Private _PlugInsPath As String
        Private _PlugInsDllName As String

        Private _PostBackPath As String
        Private _AssemblyDll As System.Reflection.Assembly

        Public Sub New(ByVal PlugInsPath As String, ByVal PlugInsDllName As String)
            If String.IsNullOrEmpty(PlugInsPath) Then Throw New System.Exception("PlugInsPath can not be leave blank!")
            If String.IsNullOrEmpty(PlugInsDllName) Then Throw New System.Exception("PlugInsDllName can not be leave blank!")

            Me._PlugInsPath = PlugInsPath
            Me._PlugInsDllName = PlugInsDllName

            Me._PostBackPath = System.IO.Path.Combine( _
                                        Me._PlugInsPath, _
                                        String.Format("{0}.dll", Me._PlugInsDllName) _
                                    )
            Me._AssemblyDll = System.Reflection.Assembly.LoadFrom(Me._PostBackPath)
        End Sub

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
                            If String.Compare( _
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

        Public Function Invoke(ByVal CallClassName As String, ByVal CallFunctionName As String, ByVal CallFunctionParams As Object()) As Object
            Return Me.InvokeInternal(CallClassName, CallFunctionName, CallFunctionParams)
        End Function

        Private Function InvokeInternal(ByVal CallClassName As String, ByVal CallFunctionName As String, ByVal CallFunctionParams As Object()) As Object
            If String.IsNullOrEmpty(CallFunctionName) Then Throw New System.Exception("CallFunctionName can not be leave blank!")
            If CallFunctionParams Is Nothing Then Throw New System.Exception("CallFunctionParams can not be leave blank!")

            Dim CompileErrorObject As String

            If Not CallClassName Is Nothing Then
                CompileErrorObject = String.Format("{0}?{1}.{2},[Length:{3}]", Me._PlugInsDllName, CallClassName, CallFunctionName, CallFunctionParams.Length)
            Else
                CompileErrorObject = String.Format("{0}?{1},[Length:{2}]", Me._PlugInsDllName, CallFunctionName, CallFunctionParams.Length)
            End If

            Dim rObject As Object = New System.Exception(String.Format("PlugIns Execution Error! RequestInfo: {0}", CompileErrorObject))

            Try
                Dim ExamInterface As System.Type = _
                    Me._AssemblyDll.GetType(String.Format("WebDynamics.{0}", Me._PlugInsDllName), False, True)

                If ExamInterface Is Nothing Then
                    Throw New System.Exception("Calling Assembly is not a XeoraCube PlugIn or Addon!")
                Else
                    Dim InterfaceType As System.Type = _
                        ExamInterface.GetInterface("ITheme", True)
                    If InterfaceType Is Nothing Then InterfaceType = ExamInterface.GetInterface("IAddon", True)

                    If InterfaceType Is Nothing OrElse _
                        Not InterfaceType.IsInterface OrElse _
                        ( _
                            String.Compare(InterfaceType.FullName, "SolidDevelopment.Web.PGlobals+PlugInMarkers+ITheme") <> 0 AndAlso _
                            String.Compare(InterfaceType.FullName, "SolidDevelopment.Web.PGlobals+PlugInMarkers+IAddon") <> 0 _
                        ) Then

                        Throw New System.Exception("Calling Assembly is not a XeoraCube PlugIn!")
                    End If
                End If

                Dim AssemblyObject As System.Type, AssemblyMethod As System.Reflection.MethodInfo = Nothing

                If Not CallClassName Is Nothing Then
                    AssemblyObject = Me._AssemblyDll.GetType(String.Format("WebDynamics.{0}+{1}", Me._PlugInsDllName, CallClassName), True, True)
                Else
                    AssemblyObject = Me._AssemblyDll.GetType(String.Format("WebDynamics.{0}", Me._PlugInsDllName), True, True)
                End If

                AssemblyMethod = Me.GetAssemblyMethod(AssemblyObject, CallFunctionName, CallFunctionParams)

                If Not AssemblyMethod Is Nothing Then
                    rObject = AssemblyMethod.Invoke( _
                                        AssemblyObject, _
                                        System.Reflection.BindingFlags.DeclaredOnly Or _
                                        System.Reflection.BindingFlags.InvokeMethod, _
                                        Nothing, _
                                        CallFunctionParams, _
                                        System.Threading.Thread.CurrentThread.CurrentCulture _
                                    )
                Else
                    Dim sB As New System.Text.StringBuilder

                    sB.AppendLine("Calling Assembly Class is not containing Function!")
                    sB.AppendLine("--------------------------------------------------")
                    sB.AppendFormat("PlugInsDllName: {0}", Me._PlugInsDllName) : sB.AppendLine()
                    sB.AppendFormat("CallClassName: {0}", CallClassName) : sB.AppendLine()
                    sB.AppendFormat("CallFunctionName: {0}", CallFunctionName) : sB.AppendLine()
                    sB.AppendFormat("CallFunctionParamsLength: {0}", _
                        Microsoft.VisualBasic.IIf(CallFunctionParams Is Nothing, "0", CallFunctionParams.Length)) : sB.AppendLine()

                    Throw New System.Exception(sB.ToString())
                End If
            Catch ex As System.Exception
                rObject = ex
            End Try

            Return rObject
        End Function

        Private Function GetAssemblyMethod(ByRef AssemblyObject As System.Type, ByVal CallFunctionName As String, ByRef CallFunctionParams As Object()) As System.Reflection.MethodInfo
            Dim rAssemblyMethod As System.Reflection.MethodInfo = Nothing

            ' Sort and Filter Searching Function
            Dim AOMIs As System.Reflection.MethodInfo() = AssemblyObject.GetMethods()
            System.Array.Sort(AOMIs, New MethodInfoNameComparer(Nothing))

            Dim mIF As New MethodInfoFinder(CallFunctionName)
            Dim MIFIdx As Integer = _
                System.Array.FindIndex( _
                    AOMIs, _
                    New System.Predicate(Of System.Reflection.MethodInfo)(AddressOf mIF.MethodInfoFinder) _
                )
            Dim MILIdx As Integer = _
                System.Array.FindLastIndex( _
                    AOMIs, _
                    New System.Predicate(Of System.Reflection.MethodInfo)(AddressOf mIF.MethodInfoFinder) _
                )

            If MIFIdx > -1 AndAlso MILIdx >= MIFIdx Then
                Dim MethodInfos As System.Reflection.MethodInfo() = _
                    CType( _
                        System.Array.CreateInstance( _
                            GetType(System.Reflection.MethodInfo), _
                            (MILIdx - MIFIdx) + 1 _
                        ),  _
                        System.Reflection.MethodInfo() _
                    )
                System.Array.Copy(AOMIs, MIFIdx, MethodInfos, 0, MethodInfos.Length)
                System.Array.Sort(MethodInfos, New MethodInfoParameterLengthComparer)

                Dim CallFunctionParams_ReBuild As Object()

                For mC As Integer = 0 To MethodInfos.Length - 1
                    CallFunctionParams_ReBuild = _
                        CType(System.Array.CreateInstance(GetType(Object), CallFunctionParams.Length), Object())
                    System.Array.Copy(CallFunctionParams, CallFunctionParams_ReBuild, CallFunctionParams.Length)

                    Dim mParams As System.Reflection.ParameterInfo() = _
                        MethodInfos(mC).GetParameters()

                    If mParams.Length = 0 AndAlso _
                        CallFunctionParams_ReBuild.Length = 0 Then

                        rAssemblyMethod = MethodInfos(mC) : CallFunctionParams = CallFunctionParams_ReBuild

                        Exit For
                    ElseIf mParams.Length > 0 AndAlso _
                            mParams.Length <= CallFunctionParams_ReBuild.Length Then

                        Dim MatchComplete As Boolean = False
                        Dim IsExactMatch As Boolean() = _
                            CType(System.Array.CreateInstance(GetType(Boolean), mParams.Length), Boolean())

                        For pC As Integer = 0 To mParams.Length - 1
                            If pC = mParams.Length - 1 Then
                                Dim CheckIsParamArrayDefined As Boolean = _
                                    System.Attribute.IsDefined(mParams(pC), GetType(System.ParamArrayAttribute))

                                If CheckIsParamArrayDefined Then
                                    Dim ParamArrayValues As System.Array = _
                                        System.Array.CreateInstance( _
                                                mParams(pC).ParameterType.GetElementType(), _
                                                (CallFunctionParams_ReBuild.Length - mParams.Length) + 1 _
                                            )

                                    For pavC As Integer = pC To CallFunctionParams_ReBuild.Length - 1
                                        Me.FixFunctionParameter( _
                                            mParams(pC).ParameterType.GetElementType(), CallFunctionParams_ReBuild(pavC))

                                        ParamArrayValues.SetValue( _
                                            CallFunctionParams_ReBuild(pavC), _
                                            pavC - (mParams.Length - 1) _
                                        )
                                    Next

                                    System.Array.Resize(CallFunctionParams_ReBuild, mParams.Length)
                                    CallFunctionParams_ReBuild(pC) = ParamArrayValues

                                    IsExactMatch(pC) = True : MatchComplete = True
                                Else
                                    IsExactMatch(pC) = Me.FixFunctionParameter( _
                                                        mParams(pC).ParameterType, CallFunctionParams_ReBuild(pC))

                                    If mParams.Length = CallFunctionParams_ReBuild.Length AndAlso _
                                        System.Array.IndexOf(IsExactMatch, False) = -1 Then MatchComplete = True
                                End If
                            Else
                                IsExactMatch(pC) = Me.FixFunctionParameter( _
                                                    mParams(pC).ParameterType, CallFunctionParams_ReBuild(pC))
                            End If
                        Next

                        If MatchComplete AndAlso _
                            System.Array.IndexOf(IsExactMatch, False) = -1 Then

                            rAssemblyMethod = MethodInfos(mC) : CallFunctionParams = CallFunctionParams_ReBuild

                            Exit For
                        End If
                    End If
                Next
            End If

            If rAssemblyMethod Is Nothing AndAlso _
                Not AssemblyObject.BaseType Is Nothing Then

                rAssemblyMethod = Me.GetAssemblyMethod(AssemblyObject.BaseType, CallFunctionName, CallFunctionParams)

                If Not rAssemblyMethod Is Nothing Then AssemblyObject = AssemblyObject.BaseType
            End If

            Return rAssemblyMethod
        End Function

        Private Function FixFunctionParameter(ByVal ParameterType As System.Type, ByRef CallFunctionParam As Object) As Boolean
            Dim rBoolean As Boolean = False

            If Not CallFunctionParam Is Nothing Then
                If ParameterType Is CallFunctionParam.GetType() Then
                    rBoolean = True
                Else
                    If String.Compare(ParameterType.FullName, GetType(Object).FullName, True) <> 0 Then
                        Try
                            CallFunctionParam = System.Convert.ChangeType(CallFunctionParam, ParameterType)

                            rBoolean = True
                        Catch ex As System.Exception
                            If TypeOf CallFunctionParam Is String AndAlso _
                                String.IsNullOrEmpty(CType(CallFunctionParam, String)) AndAlso _
                                ( _
                                    ParameterType.Equals(GetType(Byte)) OrElse _
                                    ParameterType.Equals(GetType(SByte)) OrElse _
                                    ParameterType.Equals(GetType(Short)) OrElse _
                                    ParameterType.Equals(GetType(UShort)) OrElse _
                                    ParameterType.Equals(GetType(Integer)) OrElse _
                                    ParameterType.Equals(GetType(UInteger)) OrElse _
                                    ParameterType.Equals(GetType(Long)) OrElse _
                                    ParameterType.Equals(GetType(ULong)) OrElse _
                                    ParameterType.Equals(GetType(Double)) OrElse _
                                    ParameterType.Equals(GetType(Single)) _
                                ) Then

                                CallFunctionParam = 0

                                rBoolean = True
                            End If
                        End Try
                    Else
                        rBoolean = True
                    End If
                End If
            Else
                If ParameterType.Equals(GetType(Byte)) OrElse _
                    ParameterType.Equals(GetType(SByte)) OrElse _
                    ParameterType.Equals(GetType(Short)) OrElse _
                    ParameterType.Equals(GetType(UShort)) OrElse _
                    ParameterType.Equals(GetType(Integer)) OrElse _
                    ParameterType.Equals(GetType(UInteger)) OrElse _
                    ParameterType.Equals(GetType(Long)) OrElse _
                    ParameterType.Equals(GetType(ULong)) OrElse _
                    ParameterType.Equals(GetType(Double)) OrElse _
                    ParameterType.Equals(GetType(Single)) Then

                    CallFunctionParam = 0
                End If

                rBoolean = True
            End If

            Return rBoolean
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

                Return String.Compare(x_obj.Name, y_obj.Name, _
                            Me._CompareCultureInfo, _
                            System.Globalization.CompareOptions.IgnoreCase _
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
    End Class
End Namespace