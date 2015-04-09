<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbThemePath = New System.Windows.Forms.TextBox
        Me.butBrowse = New System.Windows.Forms.Button
        Me.lbProjectThemes = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbExamResults = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Theme Path : "
        '
        'tbThemePath
        '
        Me.tbThemePath.AllowDrop = True
        Me.tbThemePath.Location = New System.Drawing.Point(97, 6)
        Me.tbThemePath.Name = "tbThemePath"
        Me.tbThemePath.ReadOnly = True
        Me.tbThemePath.Size = New System.Drawing.Size(354, 20)
        Me.tbThemePath.TabIndex = 1
        '
        'butBrowse
        '
        Me.butBrowse.Location = New System.Drawing.Point(457, 4)
        Me.butBrowse.Name = "butBrowse"
        Me.butBrowse.Size = New System.Drawing.Size(75, 23)
        Me.butBrowse.TabIndex = 2
        Me.butBrowse.Text = "Browse"
        Me.butBrowse.UseVisualStyleBackColor = True
        '
        'lbProjectThemes
        '
        Me.lbProjectThemes.FormattingEnabled = True
        Me.lbProjectThemes.Location = New System.Drawing.Point(15, 50)
        Me.lbProjectThemes.Name = "lbProjectThemes"
        Me.lbProjectThemes.Size = New System.Drawing.Size(120, 238)
        Me.lbProjectThemes.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Project Themes"
        '
        'tbExamResults
        '
        Me.tbExamResults.Location = New System.Drawing.Point(141, 50)
        Me.tbExamResults.Multiline = True
        Me.tbExamResults.Name = "tbExamResults"
        Me.tbExamResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbExamResults.Size = New System.Drawing.Size(391, 238)
        Me.tbExamResults.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label3.Location = New System.Drawing.Point(141, 34)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Theme Exam Result"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(141, 294)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(391, 10)
        Me.ProgressBar1.TabIndex = 7
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(543, 315)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbExamResults)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lbProjectThemes)
        Me.Controls.Add(Me.butBrowse)
        Me.Controls.Add(Me.tbThemePath)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Solid Web Content Theme Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbThemePath As System.Windows.Forms.TextBox
    Friend WithEvents butBrowse As System.Windows.Forms.Button
    Friend WithEvents lbProjectThemes As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbExamResults As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar

End Class
