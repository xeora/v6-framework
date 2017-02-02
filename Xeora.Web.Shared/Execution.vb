Option Strict On

Namespace Xeora.Web.Shared
    <CLSCompliant(True)>
    Public Class Execution
        Public Overloads Shared Function CrossCall(ByVal ExecutableName As String, ByVal ClassName As String, ByVal ProcedureName As String, ByVal ParamArray ParameterValues As Object()) As Object
            Return Execution.CrossCall(Helpers.CurrentDomainIDAccessTree, ExecutableName, ClassName, ProcedureName, ParameterValues)
        End Function

        Public Overloads Shared Function CrossCall(ByVal DomainIDAccessTree As String(), ByVal ExecutableName As String, ByVal ClassName As String, ByVal ProcedureName As String, ByVal ParamArray ParameterValues As Object()) As Object
            Dim rInvokeResult As Object = Nothing
            Dim WebManagerAsm As Reflection.Assembly, objAssembly As Type

            WebManagerAsm = Reflection.Assembly.Load("Xeora.Web")
            objAssembly = WebManagerAsm.GetType("Xeora.Web.Manager.Assembly", False, True)

            Dim BindInfo As BindInfo
            If ParameterValues Is Nothing OrElse
                ParameterValues.Length = 0 Then

                BindInfo = BindInfo.Make(String.Format("{0}?{1}.{2}", ExecutableName, ClassName, ProcedureName))
            Else
                Dim RandomParamNaming As New Generic.List(Of String)
                For pC As Integer = 0 To ParameterValues.Length - 1
                    RandomParamNaming.Add(String.Format("PARAM{0}", pC))
                Next
                BindInfo = BindInfo.Make(String.Format("{0}?{1}.{2},{3}", ExecutableName, ClassName, ProcedureName, String.Join("|", RandomParamNaming.ToArray())))
            End If

            Try
                rInvokeResult = CType(
                                    objAssembly.InvokeMember(
                                        "InvokeBind",
                                        Reflection.BindingFlags.InvokeMethod,
                                        Nothing,
                                        Nothing,
                                        New Object() {BindInfo, ParameterValues}
                                    ),
                                    BindInvokeResult
                                ).InvokeResult
            Catch ex As Exception
                rInvokeResult = New Exception("CrossCall Execution Error!", ex)
            End Try

            Return rInvokeResult
        End Function

        Public Shared Sub ExamMethodExecuted(ByRef MethodResult As Object, ByRef IsDone As Boolean)
            If Not MethodResult Is Nothing AndAlso
                TypeOf MethodResult Is Exception Then

                IsDone = False
            Else
                IsDone = True
            End If
        End Sub

        Public Shared Function GetPrimitiveValue(ByRef MethodResult As Object) As String
            Dim rString As String = Nothing

            If Not MethodResult Is Nothing AndAlso
                (
                    MethodResult.GetType().IsPrimitive OrElse
                    TypeOf MethodResult Is String
                ) Then

                rString = CType(MethodResult, String)
            End If

            Return rString
        End Function

        <Serializable()>
        Public Class BindInfo
            Private _RequestHttpMethod As String

            Private _ExecutableName As String
            Private _ClassNames As String()
            Private _ProcedureName As String
            Private _ProcedureParams As ProcedureParameter()
            Private _ProcedureParamValues As Object() = Nothing

            Private _IsReady As Boolean
            Private _InstanceExecution As Boolean

            Private Sub New(ByVal ExecutableName As String, ByVal ClassNames As String(), ByVal ProcedureName As String, ByVal ProcedureParams As String())
                Me._ExecutableName = ExecutableName
                Me._ClassNames = ClassNames
                Me._ProcedureName = ProcedureName

                Me._ProcedureParams = Nothing
                If Not ProcedureParams Is Nothing Then
                    Me._ProcedureParams = CType(Array.CreateInstance(GetType(ProcedureParameter), ProcedureParams.Length), ProcedureParameter())

                    For pC As Integer = 0 To ProcedureParams.Length - 1
                        Me._ProcedureParams(pC) = New ProcedureParameter(ProcedureParams(pC))
                    Next
                End If

                Me._IsReady = False
                Me._InstanceExecution = False
            End Sub

            Public Property RequestHttpMethod() As String
                Get
                    If String.IsNullOrEmpty(Me._RequestHttpMethod) Then
                        Dim Context As System.Web.HttpContext = Helpers.Context

                        If Not Context Is Nothing Then
                            Me._RequestHttpMethod = Context.Request.HttpMethod
                        Else
                            Me._RequestHttpMethod = "GET"
                        End If
                    End If

                    Return Me._RequestHttpMethod
                End Get
                Set(ByVal value As String)
                    Me._RequestHttpMethod = value
                End Set
            End Property

            Public ReadOnly Property ExecutableName() As String
                Get
                    Return Me._ExecutableName
                End Get
            End Property

            Public ReadOnly Property ClassNames() As String()
                Get
                    Return Me._ClassNames
                End Get
            End Property

            Public ReadOnly Property ProcedureName() As String
                Get
                    Return Me._ProcedureName
                End Get
            End Property

            Public ReadOnly Property ProcedureParams() As ProcedureParameter()
                Get
                    Return Me._ProcedureParams
                End Get
            End Property

            Public ReadOnly Property ProcedureParamValues() As Object()
                Get
                    Return Me._ProcedureParamValues
                End Get
            End Property

            Public ReadOnly Property IsReady() As Boolean
                Get
                    Return Me._IsReady
                End Get
            End Property

            Public Property InstanceExecution() As Boolean
                Get
                    Return Me._InstanceExecution
                End Get
                Set(ByVal value As Boolean)
                    Me._InstanceExecution = value
                End Set
            End Property

            Public Sub OverrideProcedureParameters(ByVal ProcedureParams As String())
                Me._ProcedureParams = Nothing
                If Not ProcedureParams Is Nothing Then
                    Me._ProcedureParams = CType(Array.CreateInstance(GetType(ProcedureParameter), ProcedureParams.Length), ProcedureParameter())

                    For pC As Integer = 0 To ProcedureParams.Length - 1
                        Me._ProcedureParams(pC) = New ProcedureParameter(ProcedureParams(pC))
                    Next
                End If

                Me._ProcedureParamValues = Nothing
                Me._IsReady = False
            End Sub

            Public Delegate Sub ProcedureParser(ByRef ProcedureParameter As ProcedureParameter)
            Public Sub PrepareProcedureParameters(ByVal ProcedureParser As ProcedureParser)
                If Not ProcedureParser Is Nothing Then
                    If Not Me._ProcedureParams Is Nothing Then
                        Me._ProcedureParamValues = CType(Array.CreateInstance(GetType(Object), Me._ProcedureParams.Length), Object())

                        For pC As Integer = 0 To Me._ProcedureParams.Length - 1
                            ProcedureParser.Invoke(Me._ProcedureParams(pC))

                            Me._ProcedureParamValues(pC) = Me._ProcedureParams(pC).Value
                        Next
                    End If

                    Me._IsReady = True
                End If
            End Sub

            Public Shared Function Make(ByVal Bind As String) As BindInfo
                Dim rBindInfo As BindInfo = Nothing

                If Not String.IsNullOrEmpty(Bind) Then
                    Try
                        Dim SplittedBindInfo1 As String() = Bind.Split("?"c)

                        If SplittedBindInfo1.Length = 2 Then
                            Dim ExecutableName As String =
                                SplittedBindInfo1(0)

                            Dim SplittedBindInfo2 As String() =
                                SplittedBindInfo1(1).Split(","c)

                            Dim ClassNames As String()
                            Dim ProcedureName As String

                            Dim ClassProcSearch As String() =
                                SplittedBindInfo2(0).Split("."c)

                            If ClassProcSearch.Length = 1 Then
                                ClassNames = Nothing
                                ProcedureName = ClassProcSearch(0)
                            Else
                                ClassNames = CType(Array.CreateInstance(GetType(String), ClassProcSearch.Length - 1), String())
                                Array.Copy(ClassProcSearch, 0, ClassNames, 0, ClassNames.Length)

                                ProcedureName = ClassProcSearch(ClassProcSearch.Length - 1)
                            End If

                            Dim ProcedureParams As String() = Nothing
                            If SplittedBindInfo2.Length > 1 Then _
                                ProcedureParams = String.Join(",", SplittedBindInfo2, 1, SplittedBindInfo2.Length - 1).Split("|"c)

                            rBindInfo = New BindInfo(ExecutableName, ClassNames, ProcedureName, ProcedureParams)
                        End If
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try
                End If

                Return rBindInfo
            End Function

            Private Function ProvideProcedureParameters() As String
                Dim rString As New Text.StringBuilder()

                For pC As Integer = 0 To Me._ProcedureParams.Length - 1
                    rString.Append(Me._ProcedureParams(pC).Query)

                    If pC < (Me._ProcedureParams.Length - 1) Then rString.Append("|")
                Next

                Return rString.ToString()
            End Function

            Public Shadows Function ToString() As String
                Dim rString As String =
                    String.Format("{0}?{1}{2}{3}", Me._ExecutableName, String.Join(".", Me._ClassNames), IIf(Me._ClassNames Is Nothing, String.Empty, "."), Me._ProcedureName)
                If Not Me.ProcedureParams Is Nothing Then _
                    rString = String.Format("{0},{1}", rString, Me.ProvideProcedureParameters())

                Return rString
            End Function

            <Serializable()>
            Public Class ProcedureParameter
                Private _Key As String
                Private _Value As Object
                Private _Query As String

                Public Sub New(ByVal ProcedureParameter As String)
                    Me._Key = String.Empty
                    Me._Value = Nothing
                    Me._Query = String.Empty

                    If Not String.IsNullOrEmpty(ProcedureParameter) Then
                        Me._Query = ProcedureParameter

                        Dim OperatorChars As Char() = New Char() {"^"c, "~"c, "-"c, "+"c, "="c, "#"c, "*"c}

                        If Array.IndexOf(OperatorChars, ProcedureParameter.Chars(0)) > -1 Then
                            If ProcedureParameter.Chars(0) <> "#"c Then
                                Me._Key = ProcedureParameter.Substring(1)
                            Else
                                For cC As Integer = 0 To ProcedureParameter.Length - 1
                                    If ProcedureParameter.Chars(cC) <> "#"c Then
                                        Me._Key = ProcedureParameter.Substring(cC)

                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                End Sub

                Public ReadOnly Property Key() As String
                    Get
                        Return Me._Key
                    End Get
                End Property

                Public Property Value() As Object
                    Get
                        Return Me._Value
                    End Get
                    Set(ByVal value As Object)
                        Me._Value = value
                    End Set
                End Property

                Public ReadOnly Property Query() As String
                    Get
                        Return Me._Query
                    End Get
                End Property
            End Class
        End Class

        <CLSCompliant(True), Serializable()>
        Public Class BindInvokeResult
            Private _BindInfo As BindInfo
            Private _InvokeResult As Object
            Private _ReloadRequired As Boolean
            Private _ApplicationPath As String

            Public Sub New(ByVal BindInfo As BindInfo)
                Me._BindInfo = BindInfo
                Me._InvokeResult = New Exception("Null Method Result Exception!")
                Me._ReloadRequired = False
                Me._ApplicationPath = String.Empty
            End Sub

            Public ReadOnly Property BindInfo() As BindInfo
                Get
                    Return Me._BindInfo
                End Get
            End Property

            Public Property InvokeResult() As Object
                Get
                    Return Me._InvokeResult
                End Get
                Set(ByVal Value As Object)
                    Me._InvokeResult = Value
                End Set
            End Property

            Public Property ReloadRequired() As Boolean
                Get
                    Return Me._ReloadRequired
                End Get
                Set(value As Boolean)
                    Me._ReloadRequired = value
                End Set
            End Property

            Public Property ApplicationPath() As String
                Get
                    Return Me._ApplicationPath
                End Get
                Set(value As String)
                    Me._ApplicationPath = value
                End Set
            End Property
        End Class
    End Class
End Namespace