Option Strict On
Imports Xeora.Web.Deployment
Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class Template
        Inherits DirectiveControllerBase
        Implements IParsingRequires
        Implements INamable
        Implements IDeploymentAccessRequires
        Implements IInstanceRequires

        Private _ControlID As String

        Public Event ParseRequested(ByVal DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested
        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested
        Public Event DeploymentAccessRequested(ByRef WorkingInstance As IDomain, ByRef DomainDeployment As DomainDeployment) Implements IDeploymentAccessRequires.DeploymentAccessRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.Template, ContentArguments)

            Me._ControlID = Me.CaptureControlID()
        End Sub

        Public ReadOnly Property ControlID As String Implements INamable.ControlID
            Get
                Return Me._ControlID
            End Get
        End Property

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            ' Template should always included with security check in render process in UpdateBlockRequest because
            ' UpdateBlock can be located under a template included in another template

            Dim Instance As IDomain = Nothing
            RaiseEvent InstanceRequested(Instance)

            Dim WorkingInstance As IDomain = Instance

            Dim ServiceItem As IDomain.ISettings.IServices.IServiceItem = Nothing
            Do
                ServiceItem =
                    WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                        IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                        Me.ControlID
                    )

                If ServiceItem Is Nothing Then WorkingInstance = WorkingInstance.Parent
            Loop Until WorkingInstance Is Nothing OrElse Not ServiceItem Is Nothing

            If Not ServiceItem Is Nothing Then
                ' If Overridable so check children if there is any exists which overrides
                If ServiceItem.Overridable Then
                    WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance)

                    If Not WorkingInstance Is Nothing Then
                        Dim OverridableServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                            WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                                Me.ControlID
                            )

                        ' Check overriding serviceitem requires authentication but does not have authenticationkeys. So add the current one to the new one
                        If Not OverridableServiceItem Is Nothing Then
                            If OverridableServiceItem.Authentication AndAlso OverridableServiceItem.AuthenticationKeys.Length = 0 Then
                                OverridableServiceItem.AuthenticationKeys = ServiceItem.AuthenticationKeys
                            End If

                            ServiceItem = OverridableServiceItem
                        End If
                    End If
                End If

                If ServiceItem.Authentication Then
                    Dim LocalAuthenticationNotAccepted As Boolean = False

                    For Each AuthKey As String In ServiceItem.AuthenticationKeys
                        If Helpers.Context.Session.Contents.Item(AuthKey) Is Nothing Then
                            LocalAuthenticationNotAccepted = True

                            Exit For
                        End If
                    Next

                    If Not LocalAuthenticationNotAccepted Then
                        Me.RenderInternal(WorkingInstance, SenderController)
                    Else
                        Dim SystemMessage As String = Instance.Language.Get("TEMPLATE_AUTH")

                        If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = SystemMessages.TEMPLATE_AUTH

                        Me.DefineRenderedValue("<div style='width:100%; font-weight:bolder; color:#CC0000; text-align:center'>" & SystemMessage & "!</div>")
                    End If
                Else
                    Me.RenderInternal(WorkingInstance, SenderController)
                End If
            End If
        End Sub

        Private Function SearchChildrenThatOverrides(ByRef WorkingInstance As [Shared].IDomain) As [Shared].IDomain
            Dim rDomainInstance As IDomain = Nothing

            For Each ChildDomain As IDomain In WorkingInstance.Children
                If ChildDomain.Settings.Services.ServiceItems.GetServiceItem(
                        IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                        Me.ControlID) Is Nothing Then

                    If ChildDomain.Children.Count > 0 Then
                        rDomainInstance = Me.SearchChildrenThatOverrides(ChildDomain)

                        If Not rDomainInstance Is Nothing Then Exit For
                    End If
                Else
                    If ChildDomain.Children.Count > 0 Then _
                        rDomainInstance = Me.SearchChildrenThatOverrides(ChildDomain)

                    If rDomainInstance Is Nothing Then rDomainInstance = ChildDomain

                    Exit For
                End If
            Next

            Return rDomainInstance
        End Function

        Private Sub RenderInternal(ByRef WorkingInstance As IDomain, ByRef SenderController As ControllerBase)
            Dim TemplateContent As String = String.Empty

            Dim DomainDeployment As DomainDeployment = Nothing
            RaiseEvent DeploymentAccessRequested(WorkingInstance, DomainDeployment)
            If Not DomainDeployment Is Nothing Then
                Try
                    TemplateContent = DomainDeployment.ProvideTemplateContent(Me.ControlID)

                    RaiseEvent ParseRequested(TemplateContent, Me)

                    Me.DefineRenderedValue(Me.Create())
                Catch ex As System.Exception
                    Throw New System.Exception("Parsing Error!", ex)
                End Try
            Else
                Throw New System.Exception("Domain Deployment access is failed!")
            End If
        End Sub
    End Class
End Namespace