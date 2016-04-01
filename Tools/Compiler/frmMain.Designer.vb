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
        Me.butBrowse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbLocation = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbPassword = New System.Windows.Forms.TextBox()
        Me.cbUsePassword = New System.Windows.Forms.CheckBox()
        Me.cbShowPassword = New System.Windows.Forms.CheckBox()
        Me.ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.butCompile = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'butBrowse
        '
        Me.butBrowse.Location = New System.Drawing.Point(712, 8)
        Me.butBrowse.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.butBrowse.Name = "butBrowse"
        Me.butBrowse.Size = New System.Drawing.Size(150, 44)
        Me.butBrowse.TabIndex = 0
        Me.butBrowse.Text = "Browse"
        Me.butBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 17)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(204, 25)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Domain Root Path : "
        '
        'tbLocation
        '
        Me.tbLocation.AllowDrop = True
        Me.tbLocation.Location = New System.Drawing.Point(218, 12)
        Me.tbLocation.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.tbLocation.Name = "tbLocation"
        Me.tbLocation.ReadOnly = True
        Me.tbLocation.Size = New System.Drawing.Size(474, 31)
        Me.tbLocation.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 67)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(203, 25)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Domain Password : "
        '
        'tbPassword
        '
        Me.tbPassword.Enabled = False
        Me.tbPassword.Location = New System.Drawing.Point(218, 62)
        Me.tbPassword.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.tbPassword.MaxLength = 50
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(474, 31)
        Me.tbPassword.TabIndex = 4
        '
        'cbUsePassword
        '
        Me.cbUsePassword.AutoSize = True
        Me.cbUsePassword.Location = New System.Drawing.Point(218, 112)
        Me.cbUsePassword.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.cbUsePassword.Name = "cbUsePassword"
        Me.cbUsePassword.Size = New System.Drawing.Size(182, 29)
        Me.cbUsePassword.TabIndex = 5
        Me.cbUsePassword.Text = "Use Password"
        Me.cbUsePassword.UseVisualStyleBackColor = True
        '
        'cbShowPassword
        '
        Me.cbShowPassword.AutoSize = True
        Me.cbShowPassword.Location = New System.Drawing.Point(418, 112)
        Me.cbShowPassword.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.cbShowPassword.Name = "cbShowPassword"
        Me.cbShowPassword.Size = New System.Drawing.Size(197, 29)
        Me.cbShowPassword.TabIndex = 6
        Me.cbShowPassword.Text = "Show Password"
        Me.cbShowPassword.UseVisualStyleBackColor = True
        '
        'ProgressBar
        '
        Me.ProgressBar.Location = New System.Drawing.Point(218, 192)
        Me.ProgressBar.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(478, 35)
        Me.ProgressBar.TabIndex = 7
        '
        'butCompile
        '
        Me.butCompile.Location = New System.Drawing.Point(712, 187)
        Me.butCompile.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.butCompile.Name = "butCompile"
        Me.butCompile.Size = New System.Drawing.Size(150, 44)
        Me.butCompile.TabIndex = 8
        Me.butCompile.Text = "Compile"
        Me.butCompile.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(106, 196)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(116, 25)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Progress : "
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(712, 131)
        Me.Button1.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(150, 44)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(886, 254)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butCompile)
        Me.Controls.Add(Me.ProgressBar)
        Me.Controls.Add(Me.cbShowPassword)
        Me.Controls.Add(Me.cbUsePassword)
        Me.Controls.Add(Me.tbPassword)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbLocation)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.butBrowse)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XeoraCube Domain Compiler"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents butBrowse As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbLocation As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox
    Friend WithEvents cbUsePassword As System.Windows.Forms.CheckBox
    Friend WithEvents cbShowPassword As System.Windows.Forms.CheckBox
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents butCompile As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
