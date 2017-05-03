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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbDomainsPath = New System.Windows.Forms.TextBox()
        Me.butBrowse = New System.Windows.Forms.Button()
        Me.lbProjectDomains = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbExamResults = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 17)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(183, 26)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Domains Path : "
        '
        'tbDomainsPath
        '
        Me.tbDomainsPath.AllowDrop = True
        Me.tbDomainsPath.Location = New System.Drawing.Point(200, 12)
        Me.tbDomainsPath.Margin = New System.Windows.Forms.Padding(6)
        Me.tbDomainsPath.Name = "tbDomainsPath"
        Me.tbDomainsPath.ReadOnly = True
        Me.tbDomainsPath.Size = New System.Drawing.Size(698, 31)
        Me.tbDomainsPath.TabIndex = 1
        '
        'butBrowse
        '
        Me.butBrowse.Location = New System.Drawing.Point(914, 8)
        Me.butBrowse.Margin = New System.Windows.Forms.Padding(6)
        Me.butBrowse.Name = "butBrowse"
        Me.butBrowse.Size = New System.Drawing.Size(150, 44)
        Me.butBrowse.TabIndex = 2
        Me.butBrowse.Text = "Browse"
        Me.butBrowse.UseVisualStyleBackColor = True
        '
        'lbProjectDomains
        '
        Me.lbProjectDomains.FormattingEnabled = True
        Me.lbProjectDomains.ItemHeight = 25
        Me.lbProjectDomains.Location = New System.Drawing.Point(30, 96)
        Me.lbProjectDomains.Margin = New System.Windows.Forms.Padding(6)
        Me.lbProjectDomains.Name = "lbProjectDomains"
        Me.lbProjectDomains.Size = New System.Drawing.Size(236, 454)
        Me.lbProjectDomains.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label2.Location = New System.Drawing.Point(24, 65)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(188, 26)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Project Domains"
        '
        'tbExamResults
        '
        Me.tbExamResults.Location = New System.Drawing.Point(282, 96)
        Me.tbExamResults.Margin = New System.Windows.Forms.Padding(6)
        Me.tbExamResults.Multiline = True
        Me.tbExamResults.Name = "tbExamResults"
        Me.tbExamResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbExamResults.Size = New System.Drawing.Size(778, 454)
        Me.tbExamResults.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label3.Location = New System.Drawing.Point(282, 65)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(237, 26)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Domain Exam Result"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(282, 565)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(6)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(782, 19)
        Me.ProgressBar1.TabIndex = 7
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1086, 606)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbExamResults)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lbProjectDomains)
        Me.Controls.Add(Me.butBrowse)
        Me.Controls.Add(Me.tbDomainsPath)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XeoraCube Domain Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbDomainsPath As System.Windows.Forms.TextBox
    Friend WithEvents butBrowse As System.Windows.Forms.Button
    Friend WithEvents lbProjectDomains As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbExamResults As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar

End Class
