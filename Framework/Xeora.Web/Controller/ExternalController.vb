Option Strict On

Imports Xeora.Web.Controller.Directive
Imports Xeora.Web.Deployment
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller
    Public Class ExternalController
        Inherits ControllerBase
        Implements IParsingRequires

        Public Event ParseRequested(ByVal DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControllerTypes.Renderless, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Not Me.Parent Is Nothing Then _
                Throw New Exception.HasParentException()

            Try
                RaiseEvent ParseRequested(Me.DraftValue, Me)

                Me.DefineRenderedValue(Me.Create())
            Catch ex As System.Exception
                Throw New System.Exception("Parsing Error!", ex)
            End Try
        End Sub
    End Class
End Namespace