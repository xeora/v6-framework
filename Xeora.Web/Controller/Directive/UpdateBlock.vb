Option Strict On

Namespace Xeora.Web.Controller.Directive
    Public Class UpdateBlock
        Inherits DirectiveControllerBase
        Implements IParsingRequires
        Implements INamable

        Private _ControlID As String = String.Empty

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.UpdateBlock, ContentArguments)

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

            ' Check for Parent UpdateBlock
            Dim WorkingControl As ControllerBase = Me.Parent

            Do Until WorkingControl Is Nothing
                If TypeOf WorkingControl Is UpdateBlock Then _
                    Throw New Exception.RequestBlockException()

                WorkingControl = WorkingControl.Parent
            Loop
            ' !--

            Dim ContentDescription As [Global].ContentDescription =
                New [Global].ContentDescription(Me.InsideValue)

            Dim BlockContent As String =
                ContentDescription.Parts.Item(0)

            Dim RenderOnRequestMarker As String = "!RENDERONREQUEST"

            If BlockContent.IndexOf(RenderOnRequestMarker) = 0 Then
                If String.Compare(Me.UpdateBlockControlID, Me.ControlID) = 0 Then
                    BlockContent = BlockContent.Substring(RenderOnRequestMarker.Length)
                Else
                    Me.DefineRenderedValue(String.Format("<div id=""{0}""></div>", Me.ControlID))

                    Exit Sub
                End If
            End If

            RaiseEvent ParseRequested(BlockContent, Me)

            If Me.IsUpdateBlockRequest AndAlso Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(Me.Create())
                Me.UpdateBlockRendered = True
            Else
                Me.DefineRenderedValue(
                    String.Format(
                        "<div id=""{0}"">{1}</div>",
                        Me.ControlID,
                        Me.Create()
                    )
                )
            End If
        End Sub
    End Class
End Namespace