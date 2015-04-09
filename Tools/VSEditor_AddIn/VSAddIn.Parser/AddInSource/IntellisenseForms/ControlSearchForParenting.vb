Namespace XeoraCube.VSAddIn.Forms
    Public Class ControlSearchForParenting
        Inherits ISFormBase

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.AcceptChar = "]"c
            MyBase.UseCloseChar = Nothing
            MyBase.lwControls.SmallImageList = Me.ilControls
        End Sub

        Private _TemplatesPath As String = String.Empty

        Private _FilterByTypes As New Generic.List(Of Globals.ControlTypes)

        Private _ControlType As Globals.ControlTypes = Globals.ControlTypes.Unknown
        Private _ControlID As String = String.Empty

        Public WriteOnly Property TemplatesPath() As String
            Set(ByVal value As String)
                Me._TemplatesPath = value
            End Set
        End Property

        Public ReadOnly Property ControlType() As Globals.ControlTypes
            Get
                Return Me._ControlType
            End Get
        End Property

        Public ReadOnly Property ControlID() As String
            Get
                Return Me._ControlID
            End Get
        End Property

        Public ReadOnly Property FilterByTypes() As Generic.List(Of Globals.ControlTypes)
            Get
                Return Me._FilterByTypes
            End Get
        End Property

        Public Overrides Sub FillList()
            Me._FillList(Me._TemplatesPath)

            MyBase.Sort()
        End Sub

        Private Sub _FillList(ByVal TemplatesPath As String)
            Dim editCursor As EnvDTE.EditPoint = _
                MyBase.CurrentSelection.ActivePoint.CreateEditPoint()

            Dim CurrentLine As Integer = _
                editCursor.Line

            editCursor.EndOfDocument()
            Dim LastCharOffset As Integer = _
                editCursor.AbsoluteCharOffset

            editCursor.StartOfDocument()
            Dim PageContentText As String = _
                editCursor.GetText(LastCharOffset)

            editCursor.MoveToLineAndOffset(CurrentLine, MyBase.BeginningOffset)

            ' Search Controls in PageContentText
            Dim SearchMatches As System.Text.RegularExpressions.MatchCollection = _
                System.Text.RegularExpressions.Regex.Matches(PageContentText, "\$C(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?\:(?<ControlID>[\.\w\-]+)\:")

            For Each regexMatch As System.Text.RegularExpressions.Match In SearchMatches
                If regexMatch.Success Then
                    Dim ControlID As String = regexMatch.Groups.Item("ControlID").Value
                    Dim ControlType As Globals.ControlTypes = _
                        Me.GetControlType(TemplatesPath, ControlID)

                    If ControlType <> Globals.ControlTypes.Unknown Then
                        If Me._FilterByTypes.Count = 0 Then
                            If Not MyBase.lwControls.Items.ContainsKey(ControlID) Then
                                MyBase.lwControls.Items.Add(ControlID, String.Empty, ControlType)
                                MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(ControlID)
                            End If
                        Else
                            If Me._FilterByTypes.IndexOf(ControlType) > -1 AndAlso _
                                Not MyBase.lwControls.Items.ContainsKey(ControlID) Then

                                MyBase.lwControls.Items.Add(ControlID, String.Empty, ControlType)
                                MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(ControlID)
                            End If
                        End If
                    End If
                End If
            Next
        End Sub

        Private Function GetControlType(ByVal TemplatesPath As String, ByVal ControlID As String) As Globals.ControlTypes
            Dim rControlType As Globals.ControlTypes
            Dim cFStream As IO.FileStream = Nothing

            Try
                cFStream = New IO.FileStream( _
                                IO.Path.Combine(TemplatesPath, "ControlsMap.xml"), _
                                IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                Dim xPathDocument As New Xml.XPath.XPathDocument(cFStream)
                Dim xPathNavigator As Xml.XPath.XPathNavigator = _
                    xPathDocument.CreateNavigator()
                Dim xPathIter As Xml.XPath.XPathNodeIterator = _
                    xPathNavigator.Select("//Map")
                Dim xPathIter2 As Xml.XPath.XPathNodeIterator

                Do While xPathIter.MoveNext()
                    xPathIter2 = xPathIter.Clone()

                    ControlID = xPathIter.Current.GetAttribute("controlid", xPathIter.Current.NamespaceURI)
                    rControlType = Globals.ControlTypes.Unknown

                    If xPathIter2.Current.MoveToFirstChild() Then
                        Do
                            Select Case xPathIter2.Current.Name.ToLower(New System.Globalization.CultureInfo("en-US"))
                                Case "type"
                                    Dim xControlType As String = _
                                            xPathIter2.Current.Value

                                    rControlType = Globals.ParseControlType(xControlType)

                                    Exit Do
                            End Select
                        Loop While xPathIter.Current.MoveToNext()
                    End If
                Loop

                If rControlType = Globals.ControlTypes.Unknown Then
                    Dim ParentDI As IO.DirectoryInfo = _
                        IO.Directory.GetParent(Me._TemplatesPath)
                    If ParentDI.GetDirectories("Addons").Length = 0 Then _
                        rControlType = Me.GetControlType(IO.Path.GetFullPath(IO.Path.Combine(Me._TemplatesPath, "../../../../Templates")), ControlID)
                End If
            Catch ex As Exception
                ' Just Handle Exceptions
            Finally
                If Not cFStream Is Nothing Then cFStream.Close()
            End Try

            Return rControlType
        End Function

        Public Overrides Sub AcceptSelection()
            If MyBase.lwControls.SelectedItems.Count > 0 Then
                Me._ControlID = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text
                Me._ControlType = CType(MyBase.lwControls.SelectedItems.Item(0).ImageIndex, Globals.ControlTypes)
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._ControlType = Globals.ControlTypes.Unknown
            Me._ControlID = String.Empty
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.ControlSearchForParenting, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.ControlType, Me.ControlID}, New AsyncCallback(Sub(aR As IAsyncResult) CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilControls As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ControlSearch))
            Me.ilControls = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilControls
            '
            Me.ilControls.ImageStream = CType(resources.GetObject("ilControls.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilControls.TransparentColor = System.Drawing.Color.Transparent
            Me.ilControls.Images.SetKeyName(0, "0textbox.png")
            Me.ilControls.Images.SetKeyName(1, "1password.png")
            Me.ilControls.Images.SetKeyName(2, "2checkbox.png")
            Me.ilControls.Images.SetKeyName(3, "3button.png")
            Me.ilControls.Images.SetKeyName(4, "4radio.png")
            Me.ilControls.Images.SetKeyName(5, "5textarea.png")
            Me.ilControls.Images.SetKeyName(6, "6imagebutton.png")
            Me.ilControls.Images.SetKeyName(7, "7link.png")
            Me.ilControls.Images.SetKeyName(8, "8datalist.png")
            Me.ilControls.Images.SetKeyName(9, "9conditionalstatement.png")
            Me.ilControls.Images.SetKeyName(10, "10variableblock.png")
            '
            'ControlSearch
            '
            Me.ClientSize = New System.Drawing.Size(184, 184)
            Me.Name = "ControlSearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace