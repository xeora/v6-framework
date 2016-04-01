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
            Private _ExecutableName As String
            Private _ClassName As String
            Private _ProcedureName As String
            Private _ProcedureParams As String()

            Private Sub New(ByVal ExecutableName As String, ByVal ClassName As String, ByVal ProcedureName As String, ByVal ProcedureParams As String())
                Me._ExecutableName = ExecutableName
                Me._ClassName = ClassName
                Me._ProcedureName = ProcedureName
                Me._ProcedureParams = ProcedureParams
            End Sub

            Public ReadOnly Property ExecutableName() As String
                Get
                    Return Me._ExecutableName
                End Get
            End Property

            Public ReadOnly Property ClassName() As String
                Get
                    Return Me._ClassName
                End Get
            End Property

            Public ReadOnly Property ProcedureName() As String
                Get
                    Return Me._ProcedureName
                End Get
            End Property

            Public ReadOnly Property ProcedureParams() As String()
                Get
                    Return Me._ProcedureParams
                End Get
            End Property

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

                            Dim ClassName As String =
                                SplittedBindInfo2(0).Split("."c)(0)
                            Dim ProcedureName As String =
                                SplittedBindInfo2(0).Split("."c)(1)

                            Dim ProcedureParams As String() = Nothing
                            If SplittedBindInfo2.Length > 1 Then _
                                ProcedureParams = String.Join(",", SplittedBindInfo2, 1, SplittedBindInfo2.Length - 1).Split("|"c)

                            rBindInfo = New BindInfo(ExecutableName, ClassName, ProcedureName, ProcedureParams)
                        End If
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try
                End If

                Return rBindInfo
            End Function

            Public Shadows Function ToString() As String
                Dim rString As String =
                    String.Format("{0}?{1}.{2}", Me._ExecutableName, Me._ClassName, Me._ProcedureName)
                If Not Me.ProcedureParams Is Nothing Then _
                    rString = String.Format("{0},{1}", rString, String.Join(Of String)("|", Me._ProcedureParams))

                Return rString
            End Function
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