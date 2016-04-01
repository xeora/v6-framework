Namespace Xeora.VSAddIn.Forms
    Public Class SpecialSearch
        Inherits ISFormBase

        Private _PropertyList As String() = New String() {}

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.lwControls.SmallImageList = Me.ilControls
            MyBase.UnmatchedAutoCompleteCloseLimit = 4

            Me._PropertyList = New String() {
                "0:DomainContents:DomainContentsMarker:S",
                "1:C:Control:C",
                "2:F:ServerFunction:C",
                "3:XF:ClientFunction:B",
                "4:H:OnFlyRequest:C",
                "5:L:Translation:C",
                "6:T:Template:C",
                "7:P:TemplateWithSameVariablePool:C",
                "8:MessageInformation:MessageInformation:B",
                "9:S:InLineStatement:C",
                "10:PageRenderDuration:PageRenderDuration:S"
            }
        End Sub

        Private _TemplatesPath As String = String.Empty

        Private _FilterByTypes As New Generic.List(Of Globals.SpecialPropertyTypes)

        Private _SpecialPropertyType As Globals.SpecialPropertyTypes = Globals.SpecialPropertyTypes.Single
        Private _SpecialPropertyID As String = String.Empty

        Public ReadOnly Property SpecialPropertyType() As Globals.SpecialPropertyTypes
            Get
                Return Me._SpecialPropertyType
            End Get
        End Property

        Public ReadOnly Property SpecialPropertyID() As String
            Get
                Return Me._SpecialPropertyID
            End Get
        End Property

        Public ReadOnly Property FilterByTypes() As Generic.List(Of Globals.SpecialPropertyTypes)
            Get
                Return Me._FilterByTypes
            End Get
        End Property

        Public Overrides Sub FillList()
            For Each Item As String In Me._PropertyList
                Dim PropertyPriority As Integer, PropertyID As String, PropertyName As String, PropertyType As Globals.SpecialPropertyTypes

                PropertyPriority = Integer.Parse(Item.Split(":"c)(0))
                PropertyID = Item.Split(":"c)(1)
                PropertyName = Item.Split(":"c)(2)

                Select Case Item.Split(":"c)(3)
                    Case "S"
                        PropertyType = Globals.SpecialPropertyTypes.Single
                    Case "B"
                        PropertyType = Globals.SpecialPropertyTypes.Block
                    Case "C"
                        PropertyType = Globals.SpecialPropertyTypes.Control
                End Select

                If Me._FilterByTypes.Count = 0 Then
                    If Not MyBase.lwControls.Items.ContainsKey(PropertyID) Then
                        MyBase.lwControls.Items.Add(PropertyName, String.Empty, PropertyType)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(PropertyName)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(PropertyPriority.ToString())
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(PropertyID)
                    End If
                Else
                    If Me._FilterByTypes.IndexOf(PropertyType) > -1 AndAlso
                        Not MyBase.lwControls.Items.ContainsKey(PropertyID) Then

                        MyBase.lwControls.Items.Add(PropertyName, String.Empty, PropertyType)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(PropertyName)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(PropertyPriority.ToString())
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(PropertyID)
                    End If
                End If
            Next

            MyBase.Sort(2, True)
        End Sub

        Public Overrides Sub AcceptSelection()
            If MyBase.lwControls.SelectedItems.Count > 0 Then
                Me._SpecialPropertyID = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(3).Text
                Me._SpecialPropertyType = CType(MyBase.lwControls.SelectedItems.Item(0).ImageIndex, Globals.SpecialPropertyTypes)
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._SpecialPropertyType = Globals.SpecialPropertyTypes.Unknown
            Me._SpecialPropertyID = String.Empty
        End Sub

        Private Sub SpecialSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            ' In Turkish Keyboard
            ' SemiColon appers with Shift + Period
            ' Squared Brackets appers with Menu (ALT GR) + D8

            If (e.Shift AndAlso e.KeyCode = Keys.OemPeriod) OrElse (e.Alt AndAlso e.KeyCode = Keys.D8) Then MyBase.RaiseIntelliSense = True
        End Sub

        Private Sub SpecialSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
            Select Case e.KeyChar
                Case ":"c
                    If (Not Me.CurrentCache Is Nothing AndAlso Me.CurrentCache.Length > 0) OrElse
                        MyBase.lwControls.SelectedItems.Count > 0 Then

                        Dim CurrentCache As String = Me.CurrentCache
                        If String.IsNullOrEmpty(CurrentCache) Then
                            CurrentCache = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(2).Text
                        Else
                            CurrentCache = CurrentCache.Substring(0, CurrentCache.Length - 1)
                        End If

                        Dim IsMatched As Boolean = False

                        For Each [Property] As String In Me._PropertyList
                            Dim PropertyID As String =
                                [Property].Split(":"c)(0)

                            If PropertyID.IndexOf(CurrentCache) = 0 AndAlso
                                String.Compare([Property].Split(":"c)(1), "B") = 0 Then IsMatched = True : Exit For
                        Next

                        If IsMatched Then MyBase._AcceptSelection() Else MyBase._CancelSelection()
                    Else
                        MyBase._CancelSelection()
                    End If
                Case "^"c, "~"c, "-"c, "+"c, "="c, "#"c, "*"c, "<"c, "["c
                    MyBase._CancelSelection()
            End Select
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = System.Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.SpecialPropertySearch, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.SpecialPropertyType, Me.SpecialPropertyID}, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                                                                                                                                                                                                                                                                                     Try
                                                                                                                                                                                                                                                                                                                                         CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)
                                                                                                                                                                                                                                                                                                                                     Catch ex As Exception
                                                                                                                                                                                                                                                                                                                                         ' Just handle to prevent crash
                                                                                                                                                                                                                                                                                                                                     End Try
                                                                                                                                                                                                                                                                                                                                 End Sub), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilControls As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SpecialSearch))
            Me.ilControls = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilControls
            '
            Me.ilControls.ImageStream = CType(resources.GetObject("ilControls.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilControls.TransparentColor = System.Drawing.Color.Transparent
            Me.ilControls.Images.SetKeyName(0, "0single.png")
            Me.ilControls.Images.SetKeyName(1, "1block.png")
            Me.ilControls.Images.SetKeyName(2, "control.png")
            '
            'SpecialSearch
            '
            Me.ClientSize = New System.Drawing.Size(232, 184)
            Me.Name = "SpecialSearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace