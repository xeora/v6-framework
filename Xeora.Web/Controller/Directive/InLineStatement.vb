Option Strict On

Imports Xeora.Web.Global
Imports Xeora.Web.Shared
Imports Xeora.Web.Site

Namespace Xeora.Web.Controller.Directive
    Public Class InLineStatement
        Inherits DirectiveControllerBase
        Implements IParsingRequires
        Implements IInstanceRequires
        Implements INamable

        Private _ControlID As String

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested
        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.InLineStatement, ContentArguments)

            Me._ControlID = Me.CaptureControlID()
        End Sub

        Public ReadOnly Property ControlID As String Implements INamable.ControlID
            Get
                Return Me._ControlID
            End Get
        End Property

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            ' Parse Block Content
            Dim controlValueSplitted As String() =
                Me.InsideValue.Split(":"c)
            Dim BlockContent As String = String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1)

            ' Check This Control has a Content
            Dim idxCon As Integer = BlockContent.IndexOf(":"c)

            ' Get ControlID Accourding to idxCon Value -1 = no content, else has content
            If idxCon = -1 Then
                ' No Content

                Throw New Exception.GrammerException()
            End If

            ' ControlIDWithIndex Like ControlID~INDEX
            Dim ControlIDWithIndex As String = BlockContent.Substring(0, idxCon)

            Dim CoreContent As String = Nothing
            Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

            Dim OpeningTag As String = String.Format("{0}:{{", ControlIDWithIndex)
            Dim ClosingTag As String = String.Format("}}:{0}", ControlIDWithIndex)

            idxCoreContStart = BlockContent.IndexOf(OpeningTag) + OpeningTag.Length
            idxCoreContEnd = BlockContent.LastIndexOf(ClosingTag, BlockContent.Length)

            If idxCoreContStart = OpeningTag.Length AndAlso
                idxCoreContEnd = (BlockContent.Length - OpeningTag.Length) Then

                CoreContent = BlockContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                Dim NoCacheMarker As String = "!NOCACHE", NoCache As Boolean = False

                If CoreContent.IndexOf(NoCacheMarker) = 0 Then _
                    NoCache = True : CoreContent = CoreContent.Substring(NoCacheMarker.Length)

                If Not CoreContent Is Nothing AndAlso
                    CoreContent.Trim().Length > 0 Then

                    RaiseEvent ParseRequested(CoreContent, Me)

                    CoreContent = Me.Create()

                    Dim Instance As IDomain = Nothing
                    RaiseEvent InstanceRequested(Instance)

                    Dim MethodResultInfo As Object =
                        Manager.Assembly.ExecuteStatement(Instance.IDAccessTree, Me.ControlID, CoreContent.Trim(), NoCache)

                    If Not MethodResultInfo Is Nothing AndAlso
                        TypeOf MethodResultInfo Is System.Exception Then

                        Throw New Exception.ExecutionException(
                            CType(MethodResultInfo, System.Exception).Message,
                            CType(MethodResultInfo, System.Exception).InnerException
                        )
                    Else
                        Me.DefineRenderedValue([Shared].Execution.GetPrimitiveValue(MethodResultInfo))
                    End If
                Else
                    Throw New Exception.EmptyBlockException()
                End If
            Else
                Throw New Exception.GrammerException()
            End If
        End Sub
    End Class
End Namespace