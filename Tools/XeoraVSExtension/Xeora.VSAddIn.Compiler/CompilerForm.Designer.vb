Namespace Xeora.VSAddIn.Tools
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class CompilerForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
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
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.cbShowPassword = New System.Windows.Forms.CheckBox()
            Me.ProgressBar = New System.Windows.Forms.ProgressBar()
            Me.butCompile = New System.Windows.Forms.Button()
            Me.dgvDomains = New System.Windows.Forms.DataGridView()
            Me.Selected = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.Domain = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.cbSecure = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.PasswordText = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PasswordHidden = New System.Windows.Forms.DataGridViewTextBoxColumn()
            CType(Me.dgvDomains, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cbShowPassword
            '
            Me.cbShowPassword.AutoSize = True
            Me.cbShowPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
            Me.cbShowPassword.Location = New System.Drawing.Point(23, 381)
            Me.cbShowPassword.Margin = New System.Windows.Forms.Padding(6)
            Me.cbShowPassword.Name = "cbShowPassword"
            Me.cbShowPassword.Size = New System.Drawing.Size(219, 33)
            Me.cbShowPassword.TabIndex = 6
            Me.cbShowPassword.Text = "Show Password"
            Me.cbShowPassword.UseVisualStyleBackColor = True
            '
            'ProgressBar
            '
            Me.ProgressBar.Location = New System.Drawing.Point(268, 380)
            Me.ProgressBar.Margin = New System.Windows.Forms.Padding(6)
            Me.ProgressBar.Name = "ProgressBar"
            Me.ProgressBar.Size = New System.Drawing.Size(524, 35)
            Me.ProgressBar.TabIndex = 7
            '
            'butCompile
            '
            Me.butCompile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
            Me.butCompile.Location = New System.Drawing.Point(804, 374)
            Me.butCompile.Margin = New System.Windows.Forms.Padding(6)
            Me.butCompile.Name = "butCompile"
            Me.butCompile.Size = New System.Drawing.Size(150, 44)
            Me.butCompile.TabIndex = 8
            Me.butCompile.Text = "Compile"
            Me.butCompile.UseVisualStyleBackColor = True
            '
            'dgvDomains
            '
            Me.dgvDomains.AllowUserToAddRows = False
            Me.dgvDomains.AllowUserToDeleteRows = False
            DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
            DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvDomains.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
            Me.dgvDomains.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvDomains.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Selected, Me.Domain, Me.cbSecure, Me.PasswordText, Me.PasswordHidden})
            Me.dgvDomains.Location = New System.Drawing.Point(23, 22)
            Me.dgvDomains.Name = "dgvDomains"
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.875!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
            DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvDomains.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
            Me.dgvDomains.RowTemplate.Height = 33
            Me.dgvDomains.Size = New System.Drawing.Size(931, 344)
            Me.dgvDomains.TabIndex = 10
            '
            'Selected
            '
            Me.Selected.HeaderText = ""
            Me.Selected.Name = "Selected"
            Me.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            Me.Selected.Width = 50
            '
            'Domain
            '
            Me.Domain.HeaderText = "Domain ID"
            Me.Domain.Name = "Domain"
            Me.Domain.ReadOnly = True
            Me.Domain.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            Me.Domain.Width = 250
            '
            'cbSecure
            '
            Me.cbSecure.HeaderText = "Secure"
            Me.cbSecure.Name = "cbSecure"
            Me.cbSecure.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            Me.cbSecure.Width = 120
            '
            'PasswordText
            '
            Me.PasswordText.HeaderText = "Password"
            Me.PasswordText.MaxInputLength = 50
            Me.PasswordText.Name = "PasswordText"
            Me.PasswordText.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            Me.PasswordText.Width = 400
            '
            'PasswordHidden
            '
            Me.PasswordHidden.HeaderText = ""
            Me.PasswordHidden.Name = "PasswordHidden"
            Me.PasswordHidden.Visible = False
            '
            'CompilerForm
            '
            Me.AcceptButton = Me.butCompile
            Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(978, 436)
            Me.Controls.Add(Me.dgvDomains)
            Me.Controls.Add(Me.butCompile)
            Me.Controls.Add(Me.ProgressBar)
            Me.Controls.Add(Me.cbShowPassword)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Margin = New System.Windows.Forms.Padding(6)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "CompilerForm"
            Me.ShowIcon = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "XeoraCube Domain Compiler"
            CType(Me.dgvDomains, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents cbShowPassword As System.Windows.Forms.CheckBox
        Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar
        Friend WithEvents butCompile As System.Windows.Forms.Button
        Friend WithEvents dgvDomains As DataGridView
        Friend WithEvents Selected As DataGridViewCheckBoxColumn
        Friend WithEvents Domain As DataGridViewTextBoxColumn
        Friend WithEvents cbSecure As DataGridViewCheckBoxColumn
        Friend WithEvents PasswordText As DataGridViewTextBoxColumn
        Friend WithEvents PasswordHidden As DataGridViewTextBoxColumn
    End Class
End Namespace