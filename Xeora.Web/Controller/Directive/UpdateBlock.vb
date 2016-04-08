Option Strict On

Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class UpdateBlock
        Inherits DirectiveControllerBase
        Implements IParsingRequires
        Implements IInstanceRequires
        Implements INamable

        Private _ControlID As String = String.Empty

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested
        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
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

                Dim RenderOnRequestMarker As String = "!RENDERONREQUEST"

                If CoreContent.IndexOf(RenderOnRequestMarker) = 0 Then
                    Dim Instance As IDomain = Nothing
                    RaiseEvent InstanceRequested(Instance)

                    If String.Compare(Me.UpdateBlockControlID, Me.ControlID) = 0 Then
                        CoreContent = CoreContent.Substring(RenderOnRequestMarker.Length)
                    Else
                        Me.DefineRenderedValue(String.Format("<div id=""{0}""></div>", Me.ControlID))

                        Exit Sub
                    End If
                End If

                If Not CoreContent Is Nothing AndAlso
                    CoreContent.Trim().Length > 0 Then

                    RaiseEvent ParseRequested(CoreContent, Me)

                    If Me.IsUpdateBlockRequest AndAlso Me.InRequestedUpdateBlock Then
                        Me.DefineRenderedValue(Me.Create())
                        Me.UpdateBlockRendered = True
                    Else
                        Me.DefineRenderedValue(String.Format("<div id=""{0}"">{1}</div>", Me.ControlID, Me.Create()))
                    End If
                Else
                    Throw New Exception.EmptyBlockException()
                End If
            Else
                Throw New Exception.ParseException()
            End If
        End Sub
    End Class
End Namespace