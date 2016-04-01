Option Strict On

Public Class frmMain

    Private Sub butBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBrowse.Click
        Dim pathBrowse As New FolderBrowserDialog

        pathBrowse.ShowNewFolderButton = False
        pathBrowse.Description = "Select XeoraCube Domains Path"

        If String.IsNullOrEmpty(Me.tbDomainsPath.Text) Then
            pathBrowse.SelectedPath = Application.StartupPath
        Else
            pathBrowse.SelectedPath = Me.tbDomainsPath.Text
        End If

        If pathBrowse.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.tbDomainsPath.Text = pathBrowse.SelectedPath

            Me.FillDomainList()
        End If
    End Sub

    Private Sub FillDomainList()
        Dim dI As New IO.DirectoryInfo(Me.tbDomainsPath.Text)
        Dim CheckedList As System.Collections.Generic.List(Of String)

        For Each lDI As IO.DirectoryInfo In dI.GetDirectories()
            Dim pDI As IO.DirectoryInfo() = lDI.GetDirectories()

            If pDI.Length = 4 Or pDI.Length = 5 Then
                CheckedList = New System.Collections.Generic.List(Of String)

                For Each ilDI As IO.DirectoryInfo In pDI
                    If (
                        String.Compare(ilDI.Name, "Addons") = 0 OrElse
                        String.Compare(ilDI.Name, "Contents") = 0 OrElse
                        String.Compare(ilDI.Name, "Executables") = 0 OrElse
                        String.Compare(ilDI.Name, "Languages") = 0 OrElse
                        String.Compare(ilDI.Name, "Templates") = 0) AndAlso
                        CheckedList.IndexOf(ilDI.Name) = -1 Then

                        CheckedList.Add(ilDI.Name)
                    Else
                        Exit For
                    End If
                Next

                If CheckedList.Count = 4 Or CheckedList.Count = 5 Then Me.lbProjectDomains.Items.Add(lDI.Name)
            End If
        Next
    End Sub

    Private Sub ExamToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.lbProjectDomains.SelectedIndex > -1 Then
            Dim selectedThemePath As String = IO.Path.Combine(Me.tbDomainsPath.Text, Me.lbProjectDomains.SelectedItem.ToString())

            Dim templatesPath As String = IO.Path.Combine(selectedThemePath, "Templates")
            Dim languagesPath As String = IO.Path.Combine(selectedThemePath, "Languages")

            Dim cMapFilePath As String = IO.Path.Combine(templatesPath, "ControlsMap.xml")

            If Not IO.File.Exists(cMapFilePath) Then
                MessageBox.Show("No Control Mapping File found!")

                Exit Sub
            End If

            System.Configuration.ConfigurationManager.AppSettings.Set("PyhsicalRoot", Me.tbDomainsPath.Text.Replace("\Domains", Nothing))

            Dim CurrentTranslation As String = CType(sender, MenuItem).Text
            CurrentTranslation = CurrentTranslation.Substring(CurrentTranslation.IndexOf("["c) + 1, CurrentTranslation.IndexOf("]"c) - CurrentTranslation.IndexOf("["c) - 1)
            Dim languageFilePath As String = IO.Path.Combine(languagesPath, String.Format("{0}.xml", CurrentTranslation))

            Me.ProcessIntegrity(cMapFilePath, templatesPath, languageFilePath)
        End If
    End Sub

    Private Sub ProcessIntegrity(ByVal cMapFilePath As String, ByVal templatesPath As String, ByVal languageFilePath As String)
        Dim xPathFileStream As IO.FileStream = Nothing
        Dim xPathDoc As Xml.XPath.XPathDocument = Nothing
        Dim languageXPathNavigator As Xml.XPath.XPathNavigator

        Try
            Dim cSR As IO.StreamReader = New IO.StreamReader(cMapFilePath, System.Text.Encoding.UTF8)
            Dim cFileContent As String = cSR.ReadToEnd()

            cSR.Close()

            xPathFileStream = New IO.FileStream(languageFilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            xPathDoc = New Xml.XPath.XPathDocument(xPathFileStream)

            languageXPathNavigator = xPathDoc.CreateNavigator()

            Dim xPathIter As Xml.XPath.XPathNodeIterator

            xPathIter = languageXPathNavigator.Select("//translation")

            Me.tbExamResults.Text = Nothing
            Dim translationID As String, NotExists As Boolean
            Do While xPathIter.MoveNext()
                NotExists = True
                translationID = xPathIter.Current.GetAttribute("id", xPathIter.Current.BaseURI)

                For Each tFI As IO.FileInfo In New IO.DirectoryInfo(templatesPath).GetFiles("*.htm")
                    Dim sR As IO.StreamReader = tFI.OpenText()
                    Dim fileContent As String = sR.ReadToEnd()

                    sR.Close()

                    If fileContent.IndexOf(String.Format("$L:{0}", translationID)) <> -1 Then
                        NotExists = False

                        Exit For
                    End If
                Next

                If NotExists Then
                    If cFileContent.IndexOf(String.Format("$L:{0}", translationID)) = -1 Then Me.tbExamResults.Text &= String.Format(" --> {0}{1}", translationID, Environment.NewLine)
                End If
            Loop
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lbProjectDomains_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbProjectDomains.MouseDown
        If e.Button = MouseButtons.Right AndAlso
            Me.lbProjectDomains.SelectedIndex > -1 Then

            Me.lbProjectDomains.ContextMenu = New ContextMenu()
            Me.lbProjectDomains.ContextMenu.MenuItems.Add("Exam")

            Configuration.ConfigurationManager.AppSettings.Set("PyhsicalRoot", Me.tbDomainsPath.Text.Replace("\Domains", Nothing))

            For Each DomainInfo As Xeora.Web.Shared.DomainInfo In Xeora.Web.Shared.Helpers.Domains
                If String.Compare(DomainInfo.ID, Me.lbProjectDomains.SelectedItem.ToString()) = 0 Then
                    For Each Language As Xeora.Web.Shared.DomainInfo.LanguageInfo In DomainInfo.Languages
                        Me.lbProjectDomains.ContextMenu.MenuItems.Item(0).MenuItems.Add(
                            String.Format("{0} [{1}]", Language.Name, Language.ID), New System.EventHandler(AddressOf Me.ExamToolStripMenuItem_Click))
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub tbDomainsPath_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles tbDomainsPath.DragEnter
        e.Effect = DragDropEffects.Link
    End Sub

    Private Sub tbDomainsPath_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tbDomainsPath.DragDrop
        Dim dataContent As Object = e.Data.GetData("FileName")

        If TypeOf dataContent Is System.Array AndAlso
            CType(dataContent, System.Array).Length > 0 Then

            Dim ThemeLocation As String = CType(
                       CType(dataContent, System.Array).GetValue(0), String)

            If IO.Directory.Exists(ThemeLocation) Then
                Dim dI As New IO.DirectoryInfo(ThemeLocation)

                Me.tbDomainsPath.Text = dI.FullName
                Me.FillDomainList()
            End If
        End If
    End Sub
End Class