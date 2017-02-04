Option Strict On

Namespace Xeora.Web.Site
    Public Class DomainControl
        Implements [Shared].IDomainControl
        Implements IDisposable

        Private _RequestID As String
        Private _Domain As [Shared].IDomain
        Private Shared _ReferenceTable As Hashtable

        Private _ServicePathInfo As [Shared].ServicePathInfo
        Private _ServiceType As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes
        Private _IsAuthenticationRequired As Boolean
        Private _IsWorkingAsStandAlone As Boolean

        Private _MimeType As String
        Private _ExecuteIn As String
        Private _ServiceResult As String

        Public Sub New(ByVal RequestID As String, ByVal DomainIDAccessTree As String(), ByVal LanguageID As String, ByVal OverrideCurrentDomainLanguageID As Boolean)
            Me._RequestID = RequestID

            Me._Domain = New Domain(DomainIDAccessTree, LanguageID)

            Me._ServicePathInfo = Nothing
            Me._MimeType = String.Empty
            Me._ExecuteIn = String.Empty
            Me._IsAuthenticationRequired = False
            Me._IsWorkingAsStandAlone = False

            Me._ServiceResult = String.Empty

            [Shared].Globals.PageCaching.DefaultType = Me._Domain.Settings.Configurations.DefaultCaching
            [Shared].Helpers.CurrentDomainIDAccessTree = Me._Domain.IDAccessTree
            If OverrideCurrentDomainLanguageID Then _
                [Shared].Helpers.CurrentDomainLanguageID = Me._Domain.Language.ID

            If DomainControl._ReferenceTable Is Nothing Then _
                DomainControl._ReferenceTable = Hashtable.Synchronized(New Hashtable())

            Threading.Monitor.Enter(DomainControl._ReferenceTable.SyncRoot)
            Try
                DomainControl._ReferenceTable.Item(RequestID) = Me
            Finally
                Threading.Monitor.Exit(DomainControl._ReferenceTable.SyncRoot)
            End Try
        End Sub

        Public Shared ReadOnly Property QuickAccess(ByVal RequestID As String) As [Shared].IDomainControl
            Get
                If String.IsNullOrEmpty(RequestID) OrElse
                    Not DomainControl._ReferenceTable.ContainsKey(RequestID) Then _
                    Return Nothing

                Return CType(DomainControl._ReferenceTable.Item(RequestID), [Shared].IDomainControl)
            End Get
        End Property

        Public ReadOnly Property Domain() As [Shared].IDomain Implements [Shared].IDomainControl.Domain
            Get
                Return Me._Domain
            End Get
        End Property

        Public ReadOnly Property XeoraJSVersion() As String
            Get
                Return My.Settings.ScriptVersion
            End Get
        End Property

        Public Sub ProvideXeoraJSStream(ByRef FileStream As IO.Stream)
            Dim CurrentAssembly As Reflection.Assembly =
                Reflection.Assembly.GetExecutingAssembly()

            FileStream = CurrentAssembly.GetManifestResourceStream(
                                String.Format("_sps_v{0}.js", Me.XeoraJSVersion))
        End Sub

        Public Property ServicePathInfo() As [Shared].ServicePathInfo
            Get
                Return Me._ServicePathInfo
            End Get
            Set(ByVal Value As [Shared].ServicePathInfo)
                Me._ServicePathInfo = Value

                Me.PrepareServiceSettings()
            End Set
        End Property

        Public ReadOnly Property ServiceType() As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes
            Get
                Return Me._ServiceType
            End Get
        End Property

        Public ReadOnly Property ServiceMimeType() As String
            Get
                Return Me._MimeType
            End Get
        End Property

        Public ReadOnly Property SocketEndPoint() As [Shared].Execution.BindInfo
            Get
                Dim rBindInfo As [Shared].Execution.BindInfo = Nothing

                If Me._ServiceType = [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xSocket AndAlso
                    Not String.IsNullOrEmpty(Me._ExecuteIn) Then _
                    rBindInfo = [Shared].Execution.BindInfo.Make(Me._ExecuteIn)

                Return rBindInfo
            End Get
        End Property

        Public ReadOnly Property IsAuthenticationRequired() As Boolean
            Get
                Return Me._IsAuthenticationRequired
            End Get
        End Property

        Public ReadOnly Property IsWorkingAsStandAlone() As Boolean
            Get
                Return Me._IsWorkingAsStandAlone
            End Get
        End Property

        Public ReadOnly Property URLMapping() As [Shared].URLMapping
            Get
                Dim rURLMapping As New [Shared].URLMapping

                rURLMapping.IsActive = Me._Domain.Settings.URLMappings.IsActive
                rURLMapping.Items.AddRange(Me._Domain.Settings.URLMappings.Items)

                For Each ChildDI As [Shared].DomainInfo In Me._Domain.Children
                    Dim ChildDomainIDAccessTree As String() =
                        New String(Me._Domain.IDAccessTree.Length) {}
                    Array.Copy(Me._Domain.IDAccessTree, 0, ChildDomainIDAccessTree, 0, Me._Domain.IDAccessTree.Length)
                    ChildDomainIDAccessTree(ChildDomainIDAccessTree.Length - 1) = ChildDI.ID

                    Dim WorkingInstance As [Shared].IDomain =
                        New Domain(ChildDomainIDAccessTree, Me._Domain.Language.ID)

                    rURLMapping.IsActive = rURLMapping.IsActive Or WorkingInstance.Settings.URLMappings.IsActive

                    If WorkingInstance.Settings.URLMappings.IsActive Then
                        For Each mItem As [Shared].URLMapping.URLMappingItem In WorkingInstance.Settings.URLMappings.Items
                            Dim sItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                                WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                    [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                                    mItem.ResolveInfo.ServicePathInfo.FullPath
                                )

                            If sItem Is Nothing Then
                                rURLMapping.Items.Add(mItem)
                            Else
                                If sItem.Overridable Then
                                    For mIC_r As Integer = 0 To rURLMapping.Items.Count - 1
                                        If String.Compare(rURLMapping.Items.Item(mIC_r).ResolveInfo.ServicePathInfo.FullPath, mItem.ResolveInfo.ServicePathInfo.FullPath, True) = 0 Then
                                            rURLMapping.Items.RemoveAt(mIC_r)

                                            Exit For
                                        End If
                                    Next

                                    rURLMapping.Items.Add(mItem)
                                End If
                            End If
                        Next
                    End If

                    WorkingInstance.Dispose()
                Next

                If rURLMapping.IsActive Then
                    If rURLMapping.Items.Count = 0 Then rURLMapping.IsActive = False
                Else
                    rURLMapping.Items.Clear()
                End If

                Return rURLMapping
            End Get
        End Property

        Public Sub RenderService(ByVal MessageResult As [Shared].ControlResult.Message, ByVal UpdateBlockControlID As String)
            If Me._ServicePathInfo Is Nothing Then
                Dim SystemMessage As String = Me._Domain.Language.Get("TEMPLATE_IDMUSTBESET")

                If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = [Global].SystemMessages.TEMPLATE_IDMUSTBESET

                Throw New System.Exception(SystemMessage & "!")
            End If

            Select Case Me._ServiceType
                Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                    Me._ServiceResult = Me._Domain.Render(Me._ServicePathInfo, MessageResult, UpdateBlockControlID)
                Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService
                    If Me._IsAuthenticationRequired Then
                        Dim PostedExecuteParameters As [Shared].xService.Parameters =
                            New [Shared].xService.Parameters([Shared].Helpers.Context.Request.Form.Item("execParams"))

                        If Not PostedExecuteParameters.PublicKey Is Nothing Then
                            Me._IsAuthenticationRequired = False

                            Dim WorkingInstance As [Shared].IDomain = Me._Domain

                            Dim ServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem = Nothing
                            Do
                                ServiceItem =
                                    WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                        [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService,
                                        Me._ServicePathInfo.FullPath
                                    )

                                If ServiceItem Is Nothing Then WorkingInstance = WorkingInstance.Parent
                            Loop Until WorkingInstance Is Nothing OrElse Not ServiceItem Is Nothing

                            If Not ServiceItem Is Nothing Then
                                If ServiceItem.Authentication AndAlso
                                    ServiceItem.AuthenticationKeys.Length = 0 Then

                                    ServiceItem.AuthenticationKeys =
                                        Me._Domain.Settings.Services.ServiceItems.GetAuthenticationKeys()
                                End If

                                For Each AuthKey As String In ServiceItem.AuthenticationKeys
                                    If Me._Domain.xService.ReadSessionVariable(PostedExecuteParameters.PublicKey, AuthKey) Is Nothing Then
                                        Me._IsAuthenticationRequired = True

                                        Exit For
                                    End If
                                Next
                            Else
                                Throw New NullReferenceException("Xeora Configuration does not contain any xService definition for this request!")
                            End If
                        End If
                    End If

                    If Not Me._IsAuthenticationRequired Then
                        Me._ServiceResult = Me._Domain.xService.RenderxService(Me._ExecuteIn, Me._ServicePathInfo.ServiceID)
                    Else
                        Dim MethodResult As Object =
                            New Security.SecurityException(
                                [Global].SystemMessages.XSERVICE_AUTH
                            )

                        Me._ServiceResult = Me._Domain.xService.GeneratexServiceXML(MethodResult)
                    End If
            End Select
        End Sub

        Public ReadOnly Property ServiceResult As String
            Get
                Return Me._ServiceResult
            End Get
        End Property

        Public Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal RequestedFilePath As String) Implements [Shared].IDomainControl.ProvideFileStream
            Dim WorkingInstance As Domain =
                CType(Me._Domain, Domain)
            Do
                WorkingInstance.ProvideFileStream(FileStream, RequestedFilePath)

                If FileStream Is Nothing Then WorkingInstance = CType(WorkingInstance.Parent, Domain)
            Loop Until WorkingInstance Is Nothing OrElse Not FileStream Is Nothing
        End Sub

        Public Sub PushLanguageChange(ByVal LanguageID As String) Implements [Shared].IDomainControl.PushLanguageChange
            CType(Me._Domain, Domain).PushLanguageChange(LanguageID)

            [Shared].Helpers.CurrentDomainLanguageID = Me._Domain.Language.ID
        End Sub

        Public Function QueryURLResolver(ByVal RequestFilePath As String) As [Shared].URLMapping.ResolvedMapped Implements [Shared].IDomainControl.QueryURLResolver
            Dim rResolvedMapped As [Shared].URLMapping.ResolvedMapped = Nothing

            If Me._Domain.Settings.URLMappings.IsActive AndAlso
                Not String.IsNullOrEmpty(Me._Domain.Settings.URLMappings.ResolverExecutable) Then

                Dim ResolverBindInfo As [Shared].Execution.BindInfo =
                    [Shared].Execution.BindInfo.Make(
                        String.Format("{0}?URLResolver,rfp", Me._Domain.Settings.URLMappings.ResolverExecutable))
                ResolverBindInfo.PrepareProcedureParameters(
                    New [Shared].Execution.BindInfo.ProcedureParser(
                        Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                            ProcedureParameter.Value = RequestFilePath
                        End Sub)
                )
                ResolverBindInfo.InstanceExecution = True

                Dim ResolverBindInvokeResult As [Shared].Execution.BindInvokeResult =
                    Manager.Assembly.InvokeBind(ResolverBindInfo, Manager.Assembly.ExecuterTypes.Undefined)

                If Not ResolverBindInvokeResult.ReloadRequired AndAlso
                    TypeOf ResolverBindInvokeResult.InvokeResult Is [Shared].URLMapping.ResolvedMapped Then

                    rResolvedMapped = CType(ResolverBindInvokeResult.InvokeResult, [Shared].URLMapping.ResolvedMapped)
                End If
            End If

            Return rResolvedMapped
        End Function

        ' Cache for performance consideration
        Private Shared _AvailableDomains As [Shared].DomainInfo.DomainInfoCollection = Nothing
        Public Shared Function GetAvailableDomains() As [Shared].DomainInfo.DomainInfoCollection
            If DomainControl._AvailableDomains Is Nothing Then
                Dim rDomainInfoCollection As New [Shared].DomainInfo.DomainInfoCollection()

                Try
                    Dim DomainDI As IO.DirectoryInfo =
                        New IO.DirectoryInfo(
                            IO.Path.Combine(
                                [Shared].Configurations.PhysicalRoot,
                                String.Format("{0}Domains", [Shared].Configurations.ApplicationRoot.FileSystemImplementation)
                            )
                        )

                    For Each DI As IO.DirectoryInfo In DomainDI.GetDirectories()
                        Dim Languages As [Shared].DomainInfo.LanguageInfo() =
                            Deployment.DomainDeployment.AvailableLanguageInfos(New String() {DI.Name})

                        Dim DomainDeployment As Deployment.DomainDeployment =
                            New Deployment.DomainDeployment(New String() {DI.Name}, Languages(0).ID)

                        Dim DomainInfo As New [Shared].DomainInfo(DomainDeployment.DeploymentType, DI.Name, Languages)
                        DomainInfo.Children.AddRange(DomainDeployment.Children)

                        rDomainInfoCollection.Add(DomainInfo)
                    Next

                    DomainControl._AvailableDomains = rDomainInfoCollection
                Catch ex As System.Exception
                    ' Just Handle Exceptions No Action Taken
                End Try
            End If

            Return DomainControl._AvailableDomains
        End Function

        Public Sub ClearCache()
            CType(Me._Domain, Domain).ClearCache()
        End Sub

        Private Sub PrepareServiceSettings()
            If Me._ServicePathInfo Is Nothing Then _
                Me._ServicePathInfo = [Shared].ServicePathInfo.Parse(Me._Domain.Settings.Configurations.DefaultPage)

            Dim WorkingInstance As [Shared].IDomain = Me._Domain

            ' Check ServiceFullPath is for Template
            If CType(Me._Domain, Domain).CheckTemplateExists(Me._ServicePathInfo.FullPath) Then
                ' This is a Template Request
                If String.Compare(Me._ServicePathInfo.FullPath, Me._Domain.Settings.Configurations.AuthenticationPage, True) <> 0 Then
                    ' This is not an AuthenticationPage Request
                    Dim ServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                        Me._Domain.Settings.Services.ServiceItems.GetServiceItem(
                            [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                            Me._ServicePathInfo.FullPath)

                    If Not ServiceItem Is Nothing Then
                        If ServiceItem.Overridable Then
                            WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance, Me._ServicePathInfo.FullPath)

                            ' If not null, it means WorkingInstance contains a service definition
                            If Not WorkingInstance Is Nothing Then
                                Dim OverridableServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                                    WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                        [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                                        Me._ServicePathInfo.FullPath
                                    )

                                ' Check overriding serviceitem requires authentication but does not have authenticationkeys. So add the current one to the new one
                                If OverridableServiceItem.Authentication AndAlso OverridableServiceItem.AuthenticationKeys.Length = 0 Then
                                    OverridableServiceItem.AuthenticationKeys = ServiceItem.AuthenticationKeys
                                End If

                                ServiceItem = OverridableServiceItem
                            End If
                        End If

                        Me._ServiceType = ServiceItem.ServiceType
                        If ServiceItem.Authentication Then
                            For Each AuthKey As String In ServiceItem.AuthenticationKeys
                                If [Shared].Helpers.Context.Session.Contents.Item(AuthKey) Is Nothing Then
                                    Me._IsAuthenticationRequired = True

                                    Exit For
                                End If
                            Next
                        End If
                        Me._IsWorkingAsStandAlone = ServiceItem.StandAlone
                        Me._ExecuteIn = ServiceItem.ExecuteIn
                        Me._MimeType = ServiceItem.MimeType
                    Else
                        Throw New NullReferenceException("Xeora Configuration does not contain any service definition for this request!")
                    End If
                Else
                    ' This is an AuthenticationPage Request
                    Dim ServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                        Me._Domain.Settings.Services.ServiceItems.GetServiceItem(
                            [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                            Me._ServicePathInfo.FullPath)

                    If Not ServiceItem Is Nothing Then
                        Me._ServiceType = ServiceItem.ServiceType
                        ' Overrides that page does not need authentication even it's been marked as authentication required in Configuration definition
                        Me._IsAuthenticationRequired = False
                        Me._IsWorkingAsStandAlone = ServiceItem.StandAlone
                        Me._ExecuteIn = ServiceItem.ExecuteIn
                        Me._MimeType = ServiceItem.MimeType

                        If ServiceItem.Overridable Then
                            WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance, Me._ServicePathInfo.FullPath)

                            ' If not null, it means WorkingInstance contains a service definition
                            If Not WorkingInstance Is Nothing Then
                                Dim OverridableServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                                    WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                        [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                                        Me._ServicePathInfo.FullPath
                                    )

                                ' Check overriding serviceitem requires authentication but does not have authenticationkeys. So add the current one to the new one
                                If OverridableServiceItem.Authentication AndAlso OverridableServiceItem.AuthenticationKeys.Length = 0 Then
                                    OverridableServiceItem.AuthenticationKeys = ServiceItem.AuthenticationKeys
                                End If

                                ServiceItem = OverridableServiceItem
                            End If
                        End If
                    Else
                        Throw New NullReferenceException("Xeora Configuration does not contain any service definition for this request!")
                    End If
                End If
            Else
                ' This is a xSocket or xService request or ChildDomain Template, xSocket or xService Request
                ' Check first if it is a xService or not
                Dim ServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                    Me._Domain.Settings.Services.ServiceItems.GetServiceItem(
                        [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService,
                        Me._ServicePathInfo.FullPath)

                If Not ServiceItem Is Nothing Then
                    ' This is a xService Request
                    If ServiceItem.Overridable Then
                        WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance, Me._ServicePathInfo.FullPath)

                        ' If not null, it means WorkingInstance contains a service definition
                        If Not WorkingInstance Is Nothing Then
                            Dim OverridableServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                                WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                    [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService,
                                    Me._ServicePathInfo.FullPath
                                )

                            ' Overrides xService Definition
                            If Not OverridableServiceItem Is Nothing Then ServiceItem = OverridableServiceItem
                        End If
                    End If

                    Me._ServiceType = ServiceItem.ServiceType
                    Me._IsAuthenticationRequired = ServiceItem.Authentication
                    Me._IsWorkingAsStandAlone = ServiceItem.StandAlone
                    Me._ExecuteIn = ServiceItem.ExecuteIn
                    Me._MimeType = ServiceItem.MimeType
                Else
                    ServiceItem =
                        Me._Domain.Settings.Services.ServiceItems.GetServiceItem(
                            [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xSocket,
                            Me._ServicePathInfo.FullPath)

                    If Not ServiceItem Is Nothing Then
                        ' This is a xService Request
                        If ServiceItem.Overridable Then
                            WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance, Me._ServicePathInfo.FullPath)

                            ' If not null, it means WorkingInstance contains a service definition
                            If Not WorkingInstance Is Nothing Then
                                Dim OverridableServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                                WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                    [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xSocket,
                                    Me._ServicePathInfo.FullPath
                                )

                                ' Overrides xService Definition
                                If Not OverridableServiceItem Is Nothing Then ServiceItem = OverridableServiceItem
                            End If
                        End If

                        Me._ServiceType = ServiceItem.ServiceType
                        Me._IsAuthenticationRequired = ServiceItem.Authentication
                        Me._IsWorkingAsStandAlone = ServiceItem.StandAlone
                        Me._ExecuteIn = ServiceItem.ExecuteIn
                        Me._MimeType = ServiceItem.MimeType
                    Else
                        ' This is not xService or socket but it can be a template, xSocket or xService in Children
                        ' First Check if related Service Request exists in Children.
                        ' TODO: First most deep match returns. However, there should be some priority in the same depth
                        WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance, Me._ServicePathInfo.FullPath)

                        If Not WorkingInstance Is Nothing Then
                            ' Set the Working domain as child domain for this call because call requires the child domain access!
                            Me._Domain = WorkingInstance

                            ' Okay Something Exists. But is it a Template or xService
                            ' First Check if it is a Template
                            Dim ChildServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                                WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                    [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                                    Me._ServicePathInfo.FullPath
                                )

                            If Not ChildServiceItem Is Nothing Then
                                ' Okay this is a child Template
                                ServiceItem = ChildServiceItem
                            Else
                                ' Hmm Let me check for xService and xSocket So
                                ChildServiceItem =
                                    WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                        [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService,
                                        Me._ServicePathInfo.FullPath
                                    )

                                If Not ChildServiceItem Is Nothing Then
                                    ' Okay this is a child xService
                                    ServiceItem = ChildServiceItem
                                Else
                                    ChildServiceItem =
                                        WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(
                                            [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xSocket,
                                            Me._ServicePathInfo.FullPath
                                        )

                                    If Not ChildServiceItem Is Nothing Then
                                        ' Okay this is a child Socket
                                        ServiceItem = ChildServiceItem
                                    Else
                                        '' Nothing found Anywhere!
                                        '[Shared].Helpers.Context.Response.StatusCode = 404

                                        Me._ServicePathInfo = Nothing

                                        Exit Sub
                                    End If
                                End If
                            End If
                        End If

                        If Not ServiceItem Is Nothing Then
                            ' Let work on Found Service Item in Children

                            Me._ServiceType = ServiceItem.ServiceType
                            If ServiceItem.Authentication Then
                                For Each AuthKey As String In ServiceItem.AuthenticationKeys
                                    If [Shared].Helpers.Context.Session.Contents.Item(AuthKey) Is Nothing Then
                                        Me._IsAuthenticationRequired = True

                                        Exit For
                                    End If
                                Next
                            End If
                            Me._IsWorkingAsStandAlone = ServiceItem.StandAlone
                            Me._ExecuteIn = ServiceItem.ExecuteIn
                            Me._MimeType = ServiceItem.MimeType
                        Else
                            Me._ServicePathInfo = Nothing
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function SearchChildrenThatOverrides(ByRef WorkingInstance As [Shared].IDomain, ByVal ServiceFullPath As String) As [Shared].IDomain
            Dim rDomainInstance As [Shared].IDomain = Nothing

            For Each ChildDI As [Shared].DomainInfo In WorkingInstance.Children
                Dim ChildDomainIDAccessTree As String() =
                        New String(Me._Domain.IDAccessTree.Length) {}
                Array.Copy(Me._Domain.IDAccessTree, 0, ChildDomainIDAccessTree, 0, Me._Domain.IDAccessTree.Length)
                ChildDomainIDAccessTree(ChildDomainIDAccessTree.Length - 1) = ChildDI.ID

                rDomainInstance = New Domain(ChildDomainIDAccessTree, Me._Domain.Language.ID)

                If rDomainInstance.Settings.Services.ServiceItems.GetServiceItem(
                        [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template,
                        ServiceFullPath) Is Nothing Then

                    If rDomainInstance.Children.Count > 0 Then
                        rDomainInstance = Me.SearchChildrenThatOverrides(rDomainInstance, ServiceFullPath)

                        If Not rDomainInstance Is Nothing Then Exit For
                    End If
                Else
                    If rDomainInstance.Children.Count > 0 Then _
                        rDomainInstance = Me.SearchChildrenThatOverrides(rDomainInstance, ServiceFullPath)

                    If rDomainInstance Is Nothing Then _
                        rDomainInstance = New Domain(rDomainInstance.IDAccessTree, WorkingInstance.Language.ID)

                    Exit For
                End If
            Next

            Return rDomainInstance
        End Function

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                Me._Domain.Dispose()

                Threading.Monitor.Enter(DomainControl._ReferenceTable.SyncRoot)
                Try
                    If DomainControl._ReferenceTable.ContainsKey(Me._RequestID) Then _
                        DomainControl._ReferenceTable.Remove(Me._RequestID)
                Finally
                    Threading.Monitor.Exit(DomainControl._ReferenceTable.SyncRoot)
                End Try
            End If

            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace