Option Strict On

Namespace Xeora.Web.Shared
    Public Class ServicePathInfo
        Private _PathTree As Generic.LinkedList(Of String)
        Private _ServiceID As String

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Private Sub New(ByVal ServiceID As String)
            Me._PathTree = New Generic.LinkedList(Of String)
            Me._ServiceID = ServiceID
        End Sub

        Public ReadOnly Property PathTree As Generic.LinkedList(Of String)
            Get
                Return Me._PathTree
            End Get
        End Property

        Public ReadOnly Property ServiceID As String
            Get
                Return Me._ServiceID
            End Get
        End Property

        Private _FullPath As String = Nothing
        Public ReadOnly Property FullPath As String
            Get
                If Me._FullPath Is Nothing Then
                    Dim PathTreeArr As String() =
                        CType(Array.CreateInstance(GetType(String), Me._PathTree.Count), String())
                    Me._PathTree.CopyTo(PathTreeArr, 0)

                    Me._FullPath = String.Join("/", PathTreeArr)

                    If Not String.IsNullOrEmpty(Me._FullPath) Then _
                        Me._FullPath = String.Concat(Me._FullPath, "/")

                    Me._FullPath = String.Concat(Me._FullPath, Me._ServiceID)
                End If

                Return Me._FullPath
            End Get
        End Property

        Public Shared Function Parse(ByVal FullPath As String) As ServicePathInfo
            Dim RequestPaths As String() = FullPath.Split("/"c)

            Dim rServicePathInfo As New ServicePathInfo(RequestPaths(RequestPaths.Length - 1))
            For pC As Integer = 0 To RequestPaths.Length - 2
                rServicePathInfo.PathTree.AddLast(RequestPaths(pC))
            Next

            Return rServicePathInfo
        End Function
    End Class
End Namespace