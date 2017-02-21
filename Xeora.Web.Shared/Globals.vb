Option Strict On

Namespace Xeora.Web.Shared
    Public Class Globals
        Public Class PageCaching
            Public Enum Types
                AllContent ' is always default on functions
                AllContentCookiless
                TextsOnly
                TextsOnlyCookiless
                NoCache
                NoCacheCookiless
            End Enum

            Private Shared _DefaultType As New Concurrent.ConcurrentDictionary(Of String, Types)
            Public Shared Property DefaultType() As Types
                Get
                    Dim rType As Types

                    If Not PageCaching._DefaultType.TryGetValue(Helpers.CurrentRequestID, rType) Then _
                        rType = Types.AllContent

                    Return rType
                End Get
                Set(ByVal value As Types)
                    PageCaching._DefaultType.TryAdd(Helpers.CurrentRequestID, value)
                End Set
            End Property
        End Class

        Public Enum RequestTagFilteringTypes
            None
            OnlyForm
            OnlyQuery
            Both
        End Enum
    End Class
End Namespace