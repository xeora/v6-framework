Namespace XeoraCube.VSAddIn.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial MustInherit Class ISFormBase
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.lwControls = New System.Windows.Forms.ListView()
            Me.chIcons = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chControlName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SuspendLayout()
            '
            'lwControls
            '
            Me.lwControls.Activation = System.Windows.Forms.ItemActivation.OneClick
            Me.lwControls.CausesValidation = False
            Me.lwControls.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chIcons, Me.chControlName})
            Me.lwControls.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lwControls.FullRowSelect = True
            Me.lwControls.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
            Me.lwControls.Location = New System.Drawing.Point(0, 0)
            Me.lwControls.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.lwControls.MultiSelect = False
            Me.lwControls.Name = "lwControls"
            Me.lwControls.Size = New System.Drawing.Size(20, 20)
            Me.lwControls.TabIndex = 0
            Me.lwControls.TabStop = False
            Me.lwControls.UseCompatibleStateImageBehavior = False
            Me.lwControls.View = System.Windows.Forms.View.Details
            '
            'chIcons
            '
            Me.chIcons.Text = ""
            Me.chIcons.Width = 20
            '
            'chControlName
            '
            Me.chControlName.Text = ""
            Me.chControlName.Width = 0
            '
            'ISFormBase
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
            Me.CausesValidation = False
            Me.ClientSize = New System.Drawing.Size(20, 20)
            Me.ControlBox = False
            Me.Controls.Add(Me.lwControls)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.KeyPreview = True
            Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "ISFormBase"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.TopMost = True
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lwControls As System.Windows.Forms.ListView
        Friend WithEvents chIcons As System.Windows.Forms.ColumnHeader
        Friend WithEvents chControlName As System.Windows.Forms.ColumnHeader
    End Class
End Namespace