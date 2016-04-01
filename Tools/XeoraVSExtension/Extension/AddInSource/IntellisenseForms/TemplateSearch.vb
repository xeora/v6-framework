Namespace Xeora.VSAddIn.Forms
    Public Class TemplateSearch
        Inherits ISFormBase

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.lwControls.SmallImageList = ilTemplates
        End Sub

        Private _TemplatesPath As String = String.Empty
        Private _TemplateID As String = String.Empty

        Public WriteOnly Property TemplatesPath() As String
            Set(ByVal value As String)
                Me._TemplatesPath = value
            End Set
        End Property

        Public ReadOnly Property TemplateID() As String
            Get
                Return Me._TemplateID
            End Get
        End Property

        Public Overrides Sub FillList()
            Me._FillList(Me._TemplatesPath)

            Dim ParentDI As IO.DirectoryInfo =
                IO.Directory.GetParent(Me._TemplatesPath)
            If ParentDI.GetDirectories("Addons").Length = 0 Then _
                Me._FillList(IO.Path.GetFullPath(IO.Path.Combine(Me._TemplatesPath, "../../../Templates")))

            MyBase.Sort()
        End Sub

        Private Sub _FillList(ByVal TemplatesPath As String)
            Dim cFStream As IO.FileStream = Nothing

            Try
                Dim TemplateFileNames As String() =
                    IO.Directory.GetFiles(TemplatesPath, "*.htm")

                cFStream = New IO.FileStream(
                                IO.Path.Combine(TemplatesPath, "Configuration.xml"),
                                IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                Dim xPathDocument As New Xml.XPath.XPathDocument(cFStream)
                Dim xPathNavigator As Xml.XPath.XPathNavigator =
                    xPathDocument.CreateNavigator()
                Dim xPathIter As Xml.XPath.XPathNodeIterator

                Dim TemplateID As String, ImageIndex As Integer
                For Each TemplateFileName As String In TemplateFileNames
                    ImageIndex = 4
                    TemplateID = IO.Path.GetFileNameWithoutExtension(TemplateFileName)

                    xPathIter = xPathNavigator.Select(String.Format("/Settings/Services/Item[@type='template' and @id='{0}']", TemplateID))

                    If xPathIter.MoveNext() Then
                        Dim authentication As String, [overridable] As String, standalone As String
                        Dim authentication_b As Boolean, overridable_b As Boolean, standalone_b As Boolean

                        authentication = xPathIter.Current.GetAttribute("authentication", xPathIter.Current.NamespaceURI)
                        [overridable] = xPathIter.Current.GetAttribute("overridable", xPathIter.Current.NamespaceURI)
                        standalone = xPathIter.Current.GetAttribute("standalone", xPathIter.Current.NamespaceURI)

                        If String.IsNullOrEmpty(authentication) Then authentication = "false"
                        If String.IsNullOrEmpty([overridable]) Then [overridable] = "false"
                        If String.IsNullOrEmpty(standalone) Then standalone = "false"

                        Boolean.TryParse(authentication, authentication_b)
                        Boolean.TryParse([overridable], overridable_b)
                        Boolean.TryParse(standalone, standalone_b)

                        If standalone_b Then
                            If authentication_b Then
                                If overridable_b Then
                                    ImageIndex = 8
                                Else
                                    ImageIndex = 6
                                End If
                            Else
                                If overridable_b Then
                                    ImageIndex = 7
                                Else
                                    ImageIndex = 5
                                End If
                            End If
                        Else
                            If authentication_b Then
                                If overridable_b Then
                                    ImageIndex = 3
                                Else
                                    ImageIndex = 1
                                End If
                            Else
                                If overridable_b Then
                                    ImageIndex = 2
                                Else
                                    ImageIndex = 0
                                End If
                            End If
                        End If
                    End If

                    If Not MyBase.lwControls.Items.ContainsKey(TemplateID) Then
                        MyBase.lwControls.Items.Add(TemplateID, String.Empty, ImageIndex)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(TemplateID)
                    End If
                Next
            Catch ex As Exception
                ' Just Handle Exceptions
            Finally
                If Not cFStream Is Nothing Then cFStream.Close()
            End Try
        End Sub

        Public Overrides Sub AcceptSelection()
            If MyBase.lwControls.SelectedItems.Count > 0 Then
                Me._TemplateID = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._TemplateID = String.Empty
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = System.Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.TemplateSearch, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.TemplateID}, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                                                                                                                                                                                                                                               Try
                                                                                                                                                                                                                                                                                                   CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)
                                                                                                                                                                                                                                                                                               Catch ex As Exception
                                                                                                                                                                                                                                                                                                   ' Just handle to prevent crash
                                                                                                                                                                                                                                                                                               End Try
                                                                                                                                                                                                                                                                                           End Sub), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilTemplates As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TemplateSearch))
            Me.ilTemplates = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilTemplates
            '
            Me.ilTemplates.ImageStream = CType(resources.GetObject("ilTemplates.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilTemplates.TransparentColor = System.Drawing.Color.Transparent
            Me.ilTemplates.Images.SetKeyName(0, "0standart.png")
            Me.ilTemplates.Images.SetKeyName(1, "1standartwithauth.png")
            Me.ilTemplates.Images.SetKeyName(2, "2standartoverriable.png")
            Me.ilTemplates.Images.SetKeyName(3, "3standartoverriablewithauth.png")
            Me.ilTemplates.Images.SetKeyName(4, "4standartunregistered.png")
            Me.ilTemplates.Images.SetKeyName(5, "5standalone.png")
            Me.ilTemplates.Images.SetKeyName(6, "6standalonewithauth.png")
            Me.ilTemplates.Images.SetKeyName(7, "7standaloneoverriable.png")
            Me.ilTemplates.Images.SetKeyName(8, "8standaloneoverriablewithauth.png")
            '
            'TemplateSearch
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.ClientSize = New System.Drawing.Size(184, 184)
            Me.Name = "TemplateSearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace