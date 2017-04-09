Namespace Xeora.VSAddIn.Tools
    Public Class CompilerForm

        Private Sub cbShowPassword_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowPassword.CheckedChanged
            For Each dgvRow As DataGridViewRow In dgvDomains.Rows

            Next
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

            If folderBrowse.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                Dim DI As New IO.DirectoryInfo(Me.tbLocation.Text)
                Me._OutputFileLocation =
                    IO.Path.Combine(
                        folderBrowse.SelectedPath,
                        "Content.xeora")

                Dim ContinueToCompile As Boolean = True

                If IO.File.Exists(Me._OutputFileLocation) Then
                    If MessageBox.Show("File Already Exist! Do You Want To Overwrite?", "QUESTION?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = System.Windows.Forms.DialogResult.No Then
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
                    Dim CreateThread As New Threading.Thread(AddressOf Me.CreateDomainFile)

                    Me.butBrowse.Enabled = False
                    Me.tbPassword.Enabled = False
                    Me.cbUsePassword.Enabled = False
                    Me.cbShowPassword.Enabled = False
                    Me.butCompile.Enabled = False

                    CreateThread.Start()
                End If
            End If
        End Sub

        Private Sub CreateDomainFile()
            Dim XeoraCompiler As Compiler

            If Me.cbUsePassword.Checked Then
                XeoraCompiler = New Compiler(Me.tbPassword.Text)
            Else
                XeoraCompiler = New Compiler(Nothing)
            End If

            Me.AddFiles(Me.tbLocation.Text, XeoraCompiler)

            AddHandler XeoraCompiler.Progress, New Compiler.ProgressEventHandler(AddressOf UpdateProgress)

            Dim FileStream As New IO.FileStream(Me._OutputFileLocation, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)

            XeoraCompiler.CreateDomainFile(FileStream)

            FileStream.Close() : GC.SuppressFinalize(FileStream)

            Me.butBrowse.Enabled = True
            Me.tbPassword.Enabled = Me.cbUsePassword.Checked
            Me.cbUsePassword.Enabled = True
            Me.cbShowPassword.Enabled = True
            Me.butCompile.Enabled = True
        End Sub

        Private Sub AddFiles(ByVal Path As String, ByRef XeoraCompilerObj As Compiler)
            For Each dPath As String In IO.Directory.GetDirectories(Path)
                Dim dI As New IO.DirectoryInfo(dPath)

                If String.Compare(dI.Name, "addons", True) <> 0 AndAlso
                String.Compare(dI.Name, "executables", True) <> 0 Then
                    Me.AddFiles(dPath, XeoraCompilerObj)
                End If
            Next

            For Each filePath As String In IO.Directory.GetFiles(Path)
                XeoraCompilerObj.AddFile(
                    filePath.Replace(Me.tbLocation.Text, Nothing).Replace(IO.Path.GetFileName(filePath), Nothing),
                    filePath)
            Next
        End Sub

        Private Sub UpdateProgress(ByVal Current As Integer, ByVal Total As Integer)
            Me.ProgressBar.Minimum = 0
            Me.ProgressBar.Maximum = Total

            Me.ProgressBar.Value = Current

            If Me.cbUsePassword.Checked AndAlso Current = Total Then
                Dim KeyFileLocation As String =
                    IO.Path.Combine(
                        IO.Path.GetDirectoryName(Me._OutputFileLocation),
                        "Content.secure")

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
                    Dim Crypto As New Security.Cryptography.MD5CryptoServiceProvider
                    Dim PasswordKey As Byte() =
                        System.Convert.FromBase64String(
                            Convert.ToBase64String(
                                Crypto.ComputeHash(
                                    System.Text.Encoding.UTF8.GetBytes(Me.tbPassword.Text)
                                )
                            ))

                    Dim fS As New IO.FileStream(KeyFileLocation, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.None)

                    fS.Write(PasswordKey, 0, PasswordKey.Length)
                    fS.Close()

                    GC.SuppressFinalize(fS)
                End If
            End If
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

        Private Sub cbUsePassword_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Me.tbPassword.Enabled = Me.cbUsePassword.Checked
        End Sub

        Private Sub tbLocation_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs)
            e.Effect = DragDropEffects.Link
        End Sub

        Private Sub tbLocation_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs)
            Dim dataContent As Object = e.Data.GetData("FileName")

            If TypeOf dataContent Is System.Array AndAlso
                CType(dataContent, System.Array).Length > 0 Then

                Dim DomainLocation As String = CType(
                    CType(dataContent, System.Array).GetValue(0), String)

                If IO.Directory.Exists(DomainLocation) Then
                    Dim dI As New IO.DirectoryInfo(DomainLocation)

                    Me.tbLocation.Text = dI.FullName
                End If
            End If
        End Sub
    End Class
End Namespace