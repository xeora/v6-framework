Public Class frmThemePassword

    Private Sub tbThemePassword_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbThemePassword.Enter
        Me.tbThemePassword.SelectAll()
    End Sub

    Private Sub butGenKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butGenKey.Click
        Dim folderBrowse As New FolderBrowserDialog

        folderBrowse.Description = "Security Key File Save Location"
        folderBrowse.ShowNewFolderButton = True
        folderBrowse.SelectedPath = CType(Me.Owner, frmMain).tbLocation.Text

        If folderBrowse.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim KeyFileLocation As String = IO.Path.Combine( _
                                                folderBrowse.SelectedPath, _
                                                "security.key")

            Dim eO As Boolean = False
            If IO.File.Exists(KeyFileLocation) Then
                Try
                    IO.File.Delete(KeyFileLocation)
                Catch ex As Exception
                    eO = True

                    MessageBox.Show("Error Occured:" & Environment.NewLine & Environment.NewLine & ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If

            If Not eO Then
                Dim PasswordKey As Byte() = System.Convert.FromBase64String(Me.tbThemePassword.Text)

                Dim fS As New IO.FileStream(KeyFileLocation, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.None)

                fS.Write(PasswordKey, 0, PasswordKey.Length)
                fS.Close()

                GC.SuppressFinalize(fS)
            End If
        End If
    End Sub
End Class