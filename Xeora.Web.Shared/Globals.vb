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

            Private Shared _DefaultType As Hashtable = Hashtable.Synchronized(New Hashtable)
            Public Shared Property DefaultType() As Types
                Get
                    Dim rType As Types = Types.AllContent

                    If PageCaching._DefaultType.ContainsKey(Helpers.CurrentRequestID) Then _
                        rType = CType(PageCaching._DefaultType.Item(Helpers.CurrentRequestID), Types)

                    Return rType
                End Get
                Set(ByVal value As Types)
                    Threading.Monitor.Enter(PageCaching._DefaultType.SyncRoot)
                    Try
                        PageCaching._DefaultType.Item(Helpers.CurrentRequestID) = value
                    Finally
                        Threading.Monitor.Exit(PageCaching._DefaultType.SyncRoot)
                    End Try
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
                Dim rType As Types = PageCaching.DefaultType

                Select Case TypeQueryString
                    Case "L0"
                        rType = Types.AllContent
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