Option Strict On

Namespace Xeora.Web.Controller.Directive
    Public Class EncodedExecution
        Inherits DirectiveControllerBase
        Implements IParsingRequires

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.EncodedExecution, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            Dim ContentDescription As [Global].ContentDescription =
                New [Global].ContentDescription(Me.InsideValue)

            ' EncodedExecution does not have any ContentArguments, That's why it copies it's parent Arguments
            If Not Me.Parent Is Nothing Then _
                Me.ContentArguments.Replace(Me.Parent.ContentArguments)

            Dim BlockContent As String =
                ContentDescription.Parts.Item(0)

            RaiseEvent ParseRequested(BlockContent, Me)

            BlockContent = Me.Create()
            BlockContent = BlockContent.Trim()

            If String.IsNullOrEmpty(BlockContent) Then _
                Throw New Exception.EmptyBlockException()

            ' Check for Parent UpdateBlock
            Dim WorkingControl As ControllerBase = Me.Parent
            Dim ParentUpdateBlockID As String = String.Empty

            Do Until WorkingControl Is Nothing
                If TypeOf WorkingControl Is UpdateBlock Then
                    ParentUpdateBlockID = CType(WorkingControl, UpdateBlock).ControlID

                    Exit Do
                End If

                WorkingControl = WorkingControl.Parent
            Loop
            ' !--

            If Not String.IsNullOrEmpty(ParentUpdateBlockID) Then
                Me.DefineRenderedValue(
                    String.Format("javascript:__XeoraJS.update('{0}', '{1}');", ParentUpdateBlockID, Manager.Assembly.EncodeFunction([Shared].Helpers.Context.Request.HashCode, BlockContent.Trim()))
                )
            Else
                Me.DefineRenderedValue(
                    String.Format("javascript:__XeoraJS.post('{0}');", Manager.Assembly.EncodeFunction([Shared].Helpers.Context.Request.HashCode, BlockContent.Trim()))
                )
            End If
        End Sub
    End Class
End Namespace
