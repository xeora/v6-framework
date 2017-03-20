Option Strict On

Namespace Xeora.Web.Shared
    Public Interface IDomainControl
        ReadOnly Property Domain As IDomain
        Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal RequestedFilePath As String)
        Sub PushLanguageChange(ByVal LanguageID As String)
        Function QueryURLResolver(ByVal RequestFilePath As String) As URLMapping.ResolvedMapped
    End Interface

    Public Interface IDomain
        Inherits IDisposable

        ReadOnly Property Parent() As IDomain
        ReadOnly Property IDAccessTree() As String()
        ReadOnly Property DeploymentType() As DomainInfo.DeploymentTypes
        ReadOnly Property ContentsVirtualPath() As String
        ReadOnly Property Settings() As IDomain.ISettings
        ReadOnly Property Language() As IDomain.ILanguage
        ReadOnly Property xService() As IDomain.IxService
        ReadOnly Property Children() As DomainInfo.DomainInfoCollection
        Function Render(ByVal ServicePathInfo As ServicePathInfo, ByVal MessageResult As ControlResult.Message, Optional UpdateBlockControlID As String = Nothing) As String
        Function Render(ByVal XeoraContent As String, ByVal MessageResult As ControlResult.Message, Optional UpdateBlockControlID As String = Nothing) As String
        Sub ClearCache()

        Public Interface ISettings
            Inherits IDisposable
            ReadOnly Property Configurations() As IDomain.ISettings.IConfigurations
            ReadOnly Property Services() As IDomain.ISettings.IServices
            ReadOnly Property URLMappings() As IDomain.ISettings.IURLMappings

            Public Interface IConfigurations
                ReadOnly Property AuthenticationPage() As String
                ReadOnly Property DefaultPage() As String
                ReadOnly Property DefaultLanguage() As String
                ReadOnly Property DefaultCaching() As [Enum].PageCachingTypes
                ReadOnly Property DefaultSecurityBind() As String
            End Interface

            Public Interface IServices
                ReadOnly Property ServiceItems() As IServiceItem.IServiceItemCollection

                Public Interface IServiceItem
                    Enum ServiceTypes
                        Template
                        xService
                        xSocket
                    End Enum

                    Property ID() As String
                    Property MimeType() As String
                    Property Authentication() As Boolean
                    Property AuthenticationKeys() As String()
                    Property StandAlone() As Boolean
                    Property [Overridable]() As Boolean
                    Property ServiceType() As ServiceTypes
                    Property ExecuteIn() As String

                    Public Interface IServiceItemCollection
                        Function GetServiceItem(ByVal ID As String) As IServiceItem
                        Function GetServiceItems(ByVal ServiceType As ServiceTypes) As IServiceItemCollection
                        Function GetAuthenticationKeys() As String()
                    End Interface
                End Interface
            End Interface

            Public Interface IURLMappings
                ReadOnly Property IsActive() As Boolean
                ReadOnly Property ResolverExecutable() As String
                ReadOnly Property Items() As URLMapping.URLMappingItem.URLMappingItemCollection
            End Interface
        End Interface

        Public Interface ILanguage
            Inherits IDisposable
            ReadOnly Property ID() As String
            ReadOnly Property Name() As String
            ReadOnly Property Info() As DomainInfo.LanguageInfo
            Function [Get](ByVal TranslationID As String) As String

            Event ResolveTranslationRequested(ByVal TranslationID As String, ByRef Value As String)
        End Interface

        Public Interface IxService
            ReadOnly Property ReadSessionVariable(ByVal PublicKey As String, ByVal name As String) As Object
            Function CreatexServiceAuthentication(ByVal ParamArray dItems() As DictionaryEntry) As String
            Function RenderxService(ByVal ExecuteIn As String, ByVal ServiceID As String) As String
            Function GeneratexServiceXML(ByRef MethodResult As Object) As String
        End Interface
    End Interface
End Namespace