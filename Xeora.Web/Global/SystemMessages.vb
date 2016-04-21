Option Strict On

Namespace Xeora.Web.Global
    Public Class SystemMessages
        Public Const IDMUSTBESET As String = "ID must be set"
        Public Const PATH_NOTEXISTS As String = "Path is not Exists"
        Public Const PATH_WRONGSTRUCTURE As String = "Path Structure is Wrong"

        Public Const PASSWORD_WRONG As String = "Password is Wrong"

        Public Const ESSENTIAL_CONFIGURATIONNOTFOUND As String = "ConfigurationXML file is not found"
        Public Const ESSENTIAL_CONTROLSXMLNOTFOUND As String = "ControlsXML file is not found"

        Public Const CONFIGURATIONCONTENT As String = "Configuration Content value can not be null"
        Public Const TRANSLATIONCONTENT As String = "Translation Content value can not be null"

        Public Const TEMPLATE_AUTH As String = "This Template Requires Authentication"
        Public Const TEMPLATE_NOFOUND As String = "{0} Named Template file is not found"

        Public Const TEMPLATE_IDMUSTBESET As String = "TemplateID must be set"

        Public Const WEBSERVICE_AUTH As String = "This Webservice Requires Authentication"

        Public Const SYSTEM_ERROROCCURED As String = "System Error Occured"
        Public Const SYSTEM_APPLICATIONLOADINGERROR As String = "Application Loading Error Occured"
    End Class
End Namespace