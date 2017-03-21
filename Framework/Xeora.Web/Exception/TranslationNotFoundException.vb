Option Strict On

Namespace Xeora.Web.Exception
    Public Class TranslationNotFoundException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Language file does not have the translation!")
        End Sub
    End Class
End Namespace