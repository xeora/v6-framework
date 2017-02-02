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
        End Class

        Public Enum RequestTagFilteringTypes
            None
            OnlyForm
            OnlyQuery
            Both
        End Enum
    End Class
End Namespace