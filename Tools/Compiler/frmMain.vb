Public Class frmMain

    Private Sub butBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBrowse.Click
        Dim folderBrowse As New FolderBrowserDialog

        folderBrowse.Description = "Browse Theme Root Path"
        folderBrowse.ShowNewFolderButton = False
        folderBrowse.SelectedPath = Application.StartupPath

        If folderBrowse.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.tbLocation.Text = folderBrowse.SelectedPath
        End If
    End Sub

    Private Sub cbShowPassword_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowPassword.CheckedChanged
        If Me.cbShowPassword.Checked Then
            Me.tbPassword.PasswordChar = Nothing
        Else
            Me.tbPassword.PasswordChar = "*"
        End If
    End Sub

    Private _OutputFileLocation As String
    Private Sub butCompile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCompile.Click
        Dim folderBrowse As New FolderBrowserDialog

        folderBrowse.Description = "Output Folder Name"
        folderBrowse.ShowNewFolderButton = True
        folderBrowse.SelectedPath = Me.tbLocation.Text

        If folderBrowse.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim DI As New IO.DirectoryInfo(Me.tbLocation.Text)
            Me._OutputFileLocation = IO.Path.Combine( _
                                            folderBrowse.SelectedPath, _
                                            String.Format("{0}.swct", DI.Name))

            Dim ContinueToCompile As Boolean = True

            If IO.File.Exists(Me._OutputFileLocation) Then
                If MessageBox.Show("File Already Exist! Do You Want To Overwrite?", "QUESTION?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                    ContinueToCompile = False
                Else
                    Try
                        IO.File.Delete(Me._OutputFileLocation)
                    Catch ex As Exception
                        ' Just Handle Exception
                    End Try
                End If
            End If

            If ContinueToCompile Then
                Dim CreateThread As New Threading.Thread(AddressOf Me.CreateThemeFile)

                Me.butBrowse.Enabled = False
                Me.tbPassword.Enabled = False
                Me.cbUsePassword.Enabled = False
                Me.cbShowPassword.Enabled = False
                Me.butCompile.Enabled = False

                CreateThread.Start()
            End If
        End If
    End Sub

    Private Sub CreateThemeFile()
        Dim SWCTCompiler As Compiler

        If Me.cbUsePassword.Checked Then
            SWCTCompiler = New Compiler(Me.tbPassword.Text)
        Else
            SWCTCompiler = New Compiler(Nothing)
        End If

        Me.AddFiles(Me.tbLocation.Text, SWCTCompiler)

        AddHandler SWCTCompiler.Progress, New Compiler.ProgressEventHandler(AddressOf UpdateProgress)

        Dim FileStream As New IO.FileStream(Me._OutputFileLocation, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)

        SWCTCompiler.CreateThemeFile(FileStream)

        FileStream.Close() : GC.SuppressFinalize(FileStream)

        Me.butBrowse.Enabled = True
        Me.tbPassword.Enabled = Me.cbUsePassword.Checked
        Me.cbUsePassword.Enabled = True
        Me.cbShowPassword.Enabled = True
        Me.butCompile.Enabled = True
    End Sub

    Private Sub AddFiles(ByVal Path As String, ByRef SWCTCompilerObj As Compiler)
        For Each dPath As String In IO.Directory.GetDirectories(Path)
            Dim dI As New IO.DirectoryInfo(dPath)

            If String.Compare(dI.Name, "addons", True) <> 0 Then
                Me.AddFiles(dPath, SWCTCompilerObj)
            End If
        Next

        For Each filePath As String In IO.Directory.GetFiles(Path)
            SWCTCompilerObj.AddFile( _
                filePath.Replace(Me.tbLocation.Text, Nothing).Replace(IO.Path.GetFileName(filePath), Nothing), _
                filePath)
        Next
    End Sub

    Private Delegate Sub ShowPasswordDialogDelegate()
    Private Sub ShowPasswordDialog()
        If Me.InvokeRequired Then
            Me.Invoke(New ShowPasswordDialogDelegate(AddressOf Me.ShowPasswordDialog))
        Else
            Dim frmThemePassword As New frmThemePassword
            Dim Crypto As New Security.Cryptography.MD5CryptoServiceProvider

            frmThemePassword.tbThemePassword.Text = Convert.ToBase64String( _
                                                            Crypto.ComputeHash( _
                                                                System.Text.Encoding.UTF8.GetBytes(Me.tbPassword.Text) _
                                                            ) _
                                                        )
            frmThemePassword.Owner = Me
            frmThemePassword.ShowDialog()
        End If
    End Sub

    Private Sub UpdateProgress(ByVal Current As Integer, ByVal Total As Integer)
        Me.ProgressBar.Minimum = 0
        Me.ProgressBar.Maximum = Total

        Me.ProgressBar.Value = Current

        If Me.cbUsePassword.Checked AndAlso Current = Total Then Me.ShowPasswordDialog()
    End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim SWCTDecompiler As Decompiler

    '    Dim FileSelect As New OpenFileDialog

    '    If FileSelect.ShowDialog = Windows.Forms.DialogResult.OK Then
    '        Dim mm As New Security.Cryptography.MD5CryptoServiceProvider
    '        'MsgBox(Convert.ToBase64String(mm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Me.tbPassword.Text))))
    '        SWCTDecompiler = New Decompiler(FileSelect.FileName, mm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Me.tbPassword.Text)))

    '        'For Each fI As Decompiler.swctFileInfo In SWCTDecompiler.swctFilesList
    '        '    MsgBox(fI.Index & " - " & fI.RegistrationPath & " - " & fI.FileName & " - " & fI.Length & " - " & fI.CompressedLength)
    '        'Next

    '        Dim a As Decompiler.swctFileInfo = SWCTDecompiler.GetswctFileInfo("\PublicContents\en-EN\design\css\", "editor_content.css")

    '        MsgBox(a.Index & " - " & a.RegistrationPath & " - " & a.FileName & " - " & a.Length & " - " & a.CompressedLength)

    '        Dim ms As New IO.MemoryStream
    '        SWCTDecompiler.ReadFile(a.Index, a.CompressedLength, ms)

    '        If SWCTDecompiler.Authenticated Then
    '            Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 512), Byte())
    '            Dim bc As Integer, rtext As String = Nothing
    '            Do
    '                bc = ms.Read(buffer, 0, buffer.Length)

    '                If bc > 0 Then rtext &= System.Text.Encoding.UTF8.GetString(buffer, 0, bc)
    '            Loop While bc > 0

    '            ms.Close() : GC.SuppressFinalize(ms)

    '            MsgBox(rtext)
    '        Else
    '            MsgBox("Password Wrong!")
    '        End If
    '    End If
    'End Sub

    Private Sub cbUsePassword_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUsePassword.CheckedChanged
        Me.tbPassword.Enabled = Me.cbUsePassword.Checked
    End Sub

    Private Sub tbLocation_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tbLocation.DragEnter
        e.Effect = DragDropEffects.Link
    End Sub

    Private Sub tbLocation_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tbLocation.DragDrop
        Dim dataContent As Object = e.Data.GetData("FileName")

        If TypeOf dataContent Is System.Array AndAlso _
            CType(dataContent, System.Array).Length > 0 Then

            Dim ThemeLocation As String = CType( _
                       CType(dataContent, System.Array).GetValue(0), String)

            If IO.Directory.Exists(ThemeLocation) Then
                Dim dI As New IO.DirectoryInfo(ThemeLocation)

                Me.tbLocation.Text = dI.FullName
            End If
        End If
    End Sub
End Class
