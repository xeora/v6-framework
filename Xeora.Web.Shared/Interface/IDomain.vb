Option Strict On

Namespace Xeora.Web.Shared
    Public Interface IDomain
        Inherits IDisposable

        ReadOnly Property Parent() As IDomain
        ReadOnly Property IDAccessTree() As String()
        ReadOnly Property DeploymentType() As DomainInfo.DeploymentTypes
        ReadOnly Property Settings() As IDomain.ISettings
        ReadOnly Property Language() As IDomain.ILanguage
        ReadOnly Property WebService() As IDomain.IWebService
        ReadOnly Property Children() As DomainInfo.DomainInfoCollection
        Function Render(ByVal TemplateID As String, ByVal MessageResult As ControlResult.Message, Optional UpdateBlockControlID As String = Nothing) As String

        Public Interface ISettings
            Inherits IDisposable
            ReadOnly Property Configurations() As IDomain.ISettings.IConfigurations
            ReadOnly Property Services() As IDomain.ISettings.IServices
            ReadOnly Property URLMappings() As IDomain.ISettings.IURLMappings

            Public Interface IConfigurations
                ReadOnly Property AuthenticationPage() As String
                ReadOnly Property DefaultPage() As String
                ReadOnly Property DefaultLanguage() As String
                ReadOnly Property DefaultCaching() As Globals.PageCaching.Types
                ReadOnly Property DefaultSecurityBind() As String
            End Interface

            Public Interface IServices
                ReadOnly Property ServiceItems() As IServiceItem.IServiceItemCollection

                Public Interface IServiceItem
                    Enum ServiceTypes
                        Template
                        WebService
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
                        Function GetServiceItem(ByVal ServiceType As ServiceTypes, ByVal ID As String) As IServiceItem
                        Function GetServiceItems(ByVal ServiceType As ServiceTypes) As IServiceItemCollection
                        Function GetAuthenticationKeys() As String()
                    End Interface
                End Interface
            End Interface

            Public Interface IURLMappings
                ReadOnly Property IsActive() As Boolean
                ReadOnly Property Items() As URLMapping.URLMappingItem.URLMappingItemCollection
            End Interface
        End Interface

        Public Interface ILanguage
            Inherits IDisposable
            ReadOnly Property ID() As String
            ReadOnly Property Name() As String
            ReadOnly Property Info() As DomainInfo.LanguageInfo
            Function [Get](ByVal TranslationID As String) As String
        End Interface

        Public Interface IWebService
            ReadOnly Property ReadSessionVariable(ByVal PublicKey As String, ByVal name As String) As Object
            Function CreateWebServiceAuthentication(ByVal ParamArray dItems() As DictionaryEntry) As String
            Function RenderWebService(ByVal ExecuteIn As String, ByVal TemplateID As String) As String
            Function GenerateWebServiceXML(ByRef MethodResult As Object) As String
        End Interface
    End Interface
End Namespace