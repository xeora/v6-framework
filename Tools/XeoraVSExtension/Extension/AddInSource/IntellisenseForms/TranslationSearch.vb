Namespace XeoraCube.VSAddIn.Forms
    Public Class TranslationSearch
        Inherits ISFormBase

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.lwControls.SmallImageList = ilTranslations
        End Sub

        Private _TranslationsPath As String = String.Empty
        Private _TranslationID As String = String.Empty

        Public WriteOnly Property TranslationsPath() As String
            Set(ByVal value As String)
                Me._TranslationsPath = value
            End Set
        End Property

        Public ReadOnly Property TranslationID() As String
            Get
                Return Me._TranslationID
            End Get
        End Property

        Public Overrides Sub FillList()
            Me._FillList(Me._TranslationsPath)

            Dim ParentDI As IO.DirectoryInfo = _
                IO.Directory.GetParent(Me._TranslationsPath)
            If ParentDI.GetDirectories("Addons").Length = 0 Then _
                Me._FillList(IO.Path.GetFullPath(IO.Path.Combine(Me._TranslationsPath, "../../../../Translations")))

            MyBase.Sort()
        End Sub

        Private Sub _FillList(ByVal TranslationsPath As String)
            Dim cFStream As IO.FileStream = Nothing

            Try
                Dim TranslationFileNames As String() = _
                    IO.Directory.GetFiles(TranslationsPath, "*.xml")

                Dim TranslationCompile As New Generic.Dictionary(Of String, Integer)
                For Each TranslationFileName As String In TranslationFileNames
                    Try
                        cFStream = New IO.FileStream( _
                                        TranslationFileName, IO.FileMode.Open, _
                                        IO.FileAccess.Read, IO.FileShare.ReadWrite)
                        Dim xPathDocument As New Xml.XPath.XPathDocument(cFStream)
                        Dim xPathNavigator As Xml.XPath.XPathNavigator = _
                            xPathDocument.CreateNavigator()
                        Dim xPathIter As Xml.XPath.XPathNodeIterator

                        xPathIter = xPathNavigator.Select("/translations/translation")

                        Do While xPathIter.MoveNext()
                            Dim TransID As String = _
                                xPathIter.Current.GetAttribute("id", xPathIter.Current.NamespaceURI)

                            If TranslationCompile.ContainsKey(TransID) Then _
                                TranslationCompile.Item(TransID) += 1 Else TranslationCompile.Add(TransID, 1)
                        Loop
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    Finally
                        If Not cFStream Is Nothing Then cFStream.Close()
                    End Try
                Next

                Dim ImageIndex As Integer = 0
                For Each TransIDKey As String In TranslationCompile.Keys
                    If TranslationCompile.Item(TransIDKey) = TranslationFileNames.Length AndAlso _
                        Not MyBase.lwControls.Items.ContainsKey(TransIDKey) Then

                        MyBase.lwControls.Items.Add(TransIDKey, String.Empty, ImageIndex)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(TransIDKey)
                    End If
                Next
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try
        End Sub

        Public Overrides Sub AcceptSelection()
            If MyBase.lwControls.SelectedItems.Count > 0 Then
                Me._TranslationID = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._TranslationID = String.Empty
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = System.Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.TranslationSearch, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.TranslationID}, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                                                                                                                                                                                                                                                     Try
                                                                                                                                                                                                                                                                                                         CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)
                                                                                                                                                                                                                                                                                                     Catch ex As Exception
                                                                                                                                                                                                                                                                                                         ' Just handle to prevent crash
                                                                                                                                                                                                                                                                                                     End Try
                                                                                                                                                                                                                                                                                                 End Sub), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilTranslations As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TranslationSearch))
            Me.ilTranslations = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilTranslations
            '
            Me.ilTranslations.ImageStream = CType(resources.GetObject("ilTranslations.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilTranslations.TransparentColor = System.Drawing.Color.Transparent
            Me.ilTranslations.Images.SetKeyName(0, "0translation.png")
            '
            'TranslationSearch
            '
            Me.ClientSize = New System.Drawing.Size(184, 184)
            Me.Name = "TranslationSearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace