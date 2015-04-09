Namespace XeoraCube.VSAddIn.Forms
    Public Class TypeSearch
        Inherits ISFormBase

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.lwControls.SmallImageList = Me.ilControls
        End Sub

        Private _ControlTypeString As String = String.Empty

        Public ReadOnly Property ControlTypeString() As String
            Get
                Return Me._ControlTypeString
            End Get
        End Property

        Public Overrides Sub FillList()
            Dim ControlTypeNames As String() = _
                    [Enum].GetNames(GetType(Globals.ControlTypes))

            For Each ControlTypeName As String In ControlTypeNames
                If String.Compare(ControlTypeName, Globals.ControlTypes.Unknown.ToString(), True) <> 0 Then
                    MyBase.lwControls.Items.Add(ControlTypeName, String.Empty, 0)
                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(ControlTypeName)
                End If
            Next

            MyBase.Sort()
        End Sub

        Public Overrides Sub AcceptSelection()
            If MyBase.lwControls.SelectedItems.Count > 0 Then
                Me._ControlTypeString = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._ControlTypeString = String.Empty
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.TypeSearch, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.ControlTypeString}, New AsyncCallback(Sub(aR As IAsyncResult) CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilControls As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TypeSearch))
            Me.ilControls = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilControls
            '
            Me.ilControls.ImageStream = CType(resources.GetObject("ilControls.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilControls.TransparentColor = System.Drawing.Color.Transparent
            Me.ilControls.Images.SetKeyName(0, "control.png")
            '
            'TypeSearch
            '
            Me.ClientSize = New System.Drawing.Size(184, 184)
            Me.Name = "TypeSearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace