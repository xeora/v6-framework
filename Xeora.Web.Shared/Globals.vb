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

            Private Shared _DefaultType As Types = Types.AllContent
            Public Shared Property DefaultType() As Types
                Get
                    Return PageCaching._DefaultType
                End Get
                Set(ByVal value As Types)
                    PageCaching._DefaultType = value
                End Set
            End Property

            Public Shared Function ParseForQueryString(ByVal Type As Types) As String
                Dim rTypeString As String = "L0"

                Select Case Type
                    Case Types.AllContentCookiless
                        rTypeString = "L0XC"
                    Case Types.TextsOnly
                        rTypeString = "L1"
                    Case Types.TextsOnlyCookiless
                        rTypeString = "L1XC"
                    Case Types.NoCache
                        rTypeString = "L2"
                    Case Types.NoCacheCookiless
                        rTypeString = "L2XC"
                End Select

                Return rTypeString
            End Function

            Public Shared Function ParseFromQueryString(ByVal TypeQueryString As String) As Types
                Dim rType As Types = PageCaching._DefaultType

                Select Case TypeQueryString
                    Case "L0XC"
                        rType = Types.AllContentCookiless
                    Case "L1"
                        rType = Types.TextsOnly
                    Case "L1XC"
                        rType = Types.TextsOnlyCookiless
                    Case "L2"
                        rType = Types.NoCache
                    Case "L2XC"
                        rType = Types.NoCacheCookiless
                End Select

                Return rType
            End Function
        End Class

        Public Enum RequestTagFilteringTypes
            None
            OnlyForm
            OnlyQuery
            Both
        End Enum
    End Class
End Namespace