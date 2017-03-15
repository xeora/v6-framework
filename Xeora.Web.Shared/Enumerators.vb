Option Strict On

Namespace Xeora.Web.Shared
    Public Class Enumerators
        Public Enum PageCachingTypes
            AllContent
            AllContentCookiless
            TextsOnly
            TextsOnlyCookiless
            NoCache
            NoCacheCookiless
        End Enum

        Public Enum RequestTagFilteringTypes
            None
            OnlyForm
            OnlyQuery
            Both
        End Enum
    End Class
End Namespace