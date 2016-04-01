Option Strict On

Namespace Xeora.Web.Controller.Directive.Control
    Public Class AttributeInfo
        Private _ID As String
        Private _Value As String

        Public Sub New(ByVal ID As String, ByVal Value As String)
            Me._ID = ID
            Me._Value = Value
        End Sub

        Public ReadOnly Property ID() As String
            Get
                Return Me._ID
            End Get
        End Property

        Public ReadOnly Property Value() As String
            Get
                Return Me._Value
            End Get
        End Property

        Public Class AttributeInfoCollection
            Inherits Generic.List(Of AttributeInfo)

            Public Sub New()
                MyBase.New()
            End Sub

            Public Shadows Sub Add(ByVal ID As String, ByVal Value As String)
                MyBase.Add(New AttributeInfo(ID, Value))
            End Sub

            Public Shadows Sub Add(ByVal Item As AttributeInfo)
                MyBase.Add(Item)
            End Sub

            Public Shadows Sub Remove(ByVal ID As String)
                For Each Item As AttributeInfo In Me
                    If String.Compare(ID, Item.ID, True) = 0 Then
                        MyBase.Remove(Item)

                        Exit For
                    End If
                Next
            End Sub

            Public Shadows Sub Remove(ByVal Item As AttributeInfo)
                MyBase.Remove(Item)
            End Sub

            Public Shadows Property Item(ByVal ID As String) As String
                Get
                    Dim rString As String = Nothing

                    For Each aI As AttributeInfo In Me
                        If String.Compare(ID, aI.ID, True) = 0 Then
                            rString = aI.Value

                            Exit For
                        End If
                    Next

                    Return rString
                End Get
                Set(ByVal Value As String)
                    Me.Remove(ID)
                    Me.Add(ID, Value)
                End Set
            End Property

            Public Shadows Property Item(ByVal Index As Integer) As AttributeInfo
                Get
                    Return MyBase.Item(Index)
                End Get
                Set(ByVal Value As AttributeInfo)
                    Me.RemoveAt(Index)
                    Me.Insert(Index, Value)
                End Set
            End Property

            Public Shadows Function ToString() As String
                Dim rSB As New Text.StringBuilder()

                Dim CompareCulture As New Globalization.CultureInfo("en-US")
                Dim AttributeValue As String

                For Each aI As AttributeInfo In Me
                    If CompareCulture.CompareInfo.Compare(aI.ID, "id", Globalization.CompareOptions.IgnoreCase) <> 0 Then
                        AttributeValue = aI.Value

                        If aI.ID Is Nothing OrElse
                            aI.ID.Trim().Length = 0 Then

                            rSB.AppendFormat(" {0}", AttributeValue)
                        Else
                            rSB.AppendFormat(" {0}=""{1}""", aI.ID, AttributeValue.Replace("""", "\"""))
                        End If
                    End If
                Next

                Return rSB.ToString()
            End Function
        End Class
    End Class
End Namespace