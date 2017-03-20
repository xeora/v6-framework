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

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
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

            ' Gather Parent Authentication Keys
            Dim AuthenticationKeys As New Generic.List(Of String)
            Dim ServiceItem As IDomain.ISettings.IServices.IServiceItem =
                WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(Me.ControlID)

            If ServiceItem Is Nothing Then _
                Throw New Security.SecurityException(String.Format("Service definition of {0} has not been found!", Me.ControlID))

            Dim CachedServiceItem As IDomain.ISettings.IServices.IServiceItem = ServiceItem
            Do Until WorkingInstance Is Nothing
                If Not CachedServiceItem Is Nothing Then
                    For Each Key As String In CachedServiceItem.AuthenticationKeys
                        Dim IsExists As Boolean = False
                        For Each Item As String In AuthenticationKeys
                            If String.Compare(Item, Key, True) = 0 Then IsExists = True : Exit For
                        Next
                        If Not IsExists Then AuthenticationKeys.Add(Key)
                    Next
                End If

                WorkingInstance = WorkingInstance.Parent

                If Not WorkingInstance Is Nothing Then _
                    CachedServiceItem = WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(Me.ControlID)
            Loop
            ServiceItem.AuthenticationKeys = AuthenticationKeys.ToArray()

            WorkingInstance = Instance
            CachedServiceItem = ServiceItem

            Do While CachedServiceItem.Overridable
                WorkingInstance = Me.SearchChildrenThatOverrides(Instance, WorkingInstance)

                ' If not null, it means WorkingInstance contains a service definition which will override
                If Not WorkingInstance Is Nothing Then
                    Instance = WorkingInstance
                    CachedServiceItem = WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(Me.ControlID)

                    ' Merge or set the authenticationkeys
                    If CachedServiceItem.Authentication Then
                        If CachedServiceItem.AuthenticationKeys.Length = 0 Then
                            CachedServiceItem.AuthenticationKeys = ServiceItem.AuthenticationKeys
                        Else
                            ' Merge
                            Dim Keys As String() =
                                CType(Array.CreateInstance(GetType(String), CachedServiceItem.AuthenticationKeys.Length + ServiceItem.AuthenticationKeys.Length), String())

                            Array.Copy(CachedServiceItem.AuthenticationKeys, 0, Keys, 0, CachedServiceItem.AuthenticationKeys.Length)
                            Array.Copy(ServiceItem.AuthenticationKeys, 0, Keys, CachedServiceItem.AuthenticationKeys.Length, ServiceItem.AuthenticationKeys.Length)

                            CachedServiceItem.AuthenticationKeys = Keys
                        End If
                    End If

                    ServiceItem = CachedServiceItem
                Else
                    Exit Do
                End If
            Loop

            If ServiceItem.Authentication Then
                Dim LocalAuthenticationNotAccepted As Boolean = False

                For Each AuthKey As String In ServiceItem.AuthenticationKeys
                    If Helpers.Context.Session.Item(AuthKey) Is Nothing Then
                        LocalAuthenticationNotAccepted = True

                        Exit For
                    End If
                Next

                If Not LocalAuthenticationNotAccepted Then
                    Me.RenderInternal(Instance, SenderController)
                Else
                    Dim SystemMessage As String = Instance.Language.Get("TEMPLATE_AUTH")

                    If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = SystemMessages.TEMPLATE_AUTH

                    Me.DefineRenderedValue("<div style='width:100%; font-weight:bolder; color:#CC0000; text-align:center'>" & SystemMessage & "!</div>")
                End If
            Else
                Me.RenderInternal(Instance, SenderController)
            End If
        End Sub

        Private Function SearchChildrenThatOverrides(ByRef OriginalInstance As IDomain, ByRef WorkingInstance As IDomain) As IDomain
            If WorkingInstance Is Nothing Then Return Nothing

            Dim ChildDomainIDAccessTree As New Generic.List(Of String)
            ChildDomainIDAccessTree.AddRange(WorkingInstance.IDAccessTree)

            For Each ChildDI As DomainInfo In WorkingInstance.Children
                ChildDomainIDAccessTree.Add(ChildDI.ID)

                Dim rDomainInstance As IDomain =
                    New Site.Domain(ChildDomainIDAccessTree.ToArray(), OriginalInstance.Language.ID)
                Dim ServiceItem As IDomain.ISettings.IServices.IServiceItem =
                    rDomainInstance.Settings.Services.ServiceItems.GetServiceItem(Me.ControlID)

                If ServiceItem Is Nothing OrElse
                    ServiceItem.ServiceType <> IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template Then

                    If rDomainInstance.Children.Count > 0 Then
                        rDomainInstance = Me.SearchChildrenThatOverrides(OriginalInstance, rDomainInstance)

                        If Not rDomainInstance Is Nothing Then Return rDomainInstance
                    End If
                Else
                    Return rDomainInstance
                End If

                rDomainInstance.Dispose()
                ChildDomainIDAccessTree.RemoveAt(ChildDomainIDAccessTree.Count - 1)
            Next

            Return Nothing
        End Function

        Private Sub RenderInternal(ByRef WorkingInstance As IDomain, ByRef SenderController As ControllerBase)
            Dim TemplateContent As String = String.Empty

            ' Template does not have any ContentArguments, That's why it copies it's parent Arguments
            If Not Me.Parent Is Nothing Then _
                Me.ContentArguments.Replace(Me.Parent.ContentArguments)

            Dim DomainDeployment As DomainDeployment = Nothing
            RaiseEvent DeploymentAccessRequested(WorkingInstance, DomainDeployment)
            If DomainDeployment Is Nothing Then _
                Throw New System.Exception("Domain Deployment access is failed!")

            Try
                TemplateContent = DomainDeployment.ProvideTemplateContent(Me.ControlID)

                RaiseEvent ParseRequested(TemplateContent, Me)

                Me.DefineRenderedValue(Me.Create())
            Catch ex As System.Exception
                Throw New System.Exception("Parsing Error!", ex)
            End Try
        End Sub
    End Class
End Namespace