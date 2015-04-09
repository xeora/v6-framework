<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmThemePassword
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
        Me.tbThemePassword = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.butGenKey = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'tbThemePassword
        '
        Me.tbThemePassword.Location = New System.Drawing.Point(114, 3)
        Me.tbThemePassword.Name = "tbThemePassword"
        Me.tbThemePassword.Size = New System.Drawing.Size(166, 20)
        Me.tbThemePassword.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Encoded Password : "
        '
        'butGenKey
        '
        Me.butGenKey.Location = New System.Drawing.Point(192, 26)
        Me.butGenKey.Name = "butGenKey"
        Me.butGenKey.Size = New System.Drawing.Size(87, 21)
        Me.butGenKey.TabIndex = 2
        Me.butGenKey.Text = "Generate Key"
        Me.butGenKey.UseVisualStyleBackColor = True
        '
        'frmThemePassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(288, 49)
        Me.Controls.Add(Me.butGenKey)
        Me.Controls.Add(Me.tbThemePassword)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmThemePassword"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Theme Password"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbThemePassword As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents butGenKey As System.Windows.Forms.Button
End Class
