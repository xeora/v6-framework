Namespace XeoraCube.VSAddIn.Forms
    Public Class AssemblySearch
        Inherits ISFormBase

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.AcceptChar = "?"c
            MyBase.lwControls.SmallImageList = Me.ilAssembly
        End Sub

        Private _SearchType As AddInLoader.SearchTypes = VSAddIn.AddInLoader.SearchTypes.Theme
        Private _SearchPath As String = String.Empty
        Private _AssemblyID As String = String.Empty

        Public WriteOnly Property SearchType() As AddInLoader.SearchTypes
            Set(ByVal value As AddInLoader.SearchTypes)
                Me._SearchType = value
            End Set
        End Property

        Public WriteOnly Property SearchPath() As String
            Set(ByVal value As String)
                If String.IsNullOrEmpty(value) Then
                    Me._SearchPath = String.Empty
                Else
                    Me._SearchPath = IO.Path.GetFullPath(value)
                End If
            End Set
        End Property

        Public ReadOnly Property AssemblyID() As String
            Get
                Return Me._AssemblyID
            End Get
        End Property

        Public Overrides Sub FillList()
            Try
                If Me._SearchType = VSAddIn.AddInLoader.SearchTypes.Addon Then
                    Dim QueryPath As String = IO.Path.GetFullPath(IO.Path.Combine(Me._SearchPath, "../../../../Dlls"))

                    If AddInControl.AssemblyCacheObject.IsLatest(QueryPath, AddInControl.AssemblyCache.QueryTypes.None) Then
                        For Each AssemblyID As String In AddInControl.AssemblyCacheObject.GetAssemblyIDs(VSAddIn.AddInLoader.SearchTypes.Theme, QueryPath)
                            MyBase.lwControls.Items.Add(String.Empty, 0)
                            MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(AssemblyID)
                        Next
                    Else
                        For Each AssemblyID As String In MyBase.AddInLoader.GetAssemblies(VSAddIn.AddInLoader.SearchTypes.Theme, QueryPath)
                            MyBase.lwControls.Items.Add(String.Empty, 0)
                            MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(AssemblyID)

                            AddInControl.AssemblyCacheObject.AddAssemblyInfo( _
                                VSAddIn.AddInLoader.SearchTypes.Theme, _
                                IO.Path.GetFullPath(IO.Path.Combine(Me._SearchPath, "../../../../Dlls")), _
                                AssemblyID _
                            )
                        Next
                    End If
                End If

                If AddInControl.AssemblyCacheObject.IsLatest(Me._SearchPath, AddInControl.AssemblyCache.QueryTypes.None) Then
                    For Each AssemblyID As String In AddInControl.AssemblyCacheObject.GetAssemblyIDs(Me._SearchType, Me._SearchPath)
                        MyBase.lwControls.Items.Add(String.Empty, 0)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(AssemblyID)
                    Next
                Else
                    For Each AssemblyID As String In MyBase.AddInLoader.GetAssemblies(Me._SearchType, Me._SearchPath)
                        MyBase.lwControls.Items.Add(String.Empty, 0)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(AssemblyID)

                        AddInControl.AssemblyCacheObject.AddAssemblyInfo( _
                            Me._SearchType, _
                            Me._SearchPath, _
                            AssemblyID _
                        )
                    Next
                End If

                If MyBase.lwControls.Items.Count = 0 AndAlso _
                    Me._SearchType = VSAddIn.AddInLoader.SearchTypes.Theme Then

                    Dim AddonsPath As String = _
                        IO.Path.GetFullPath( _
                            IO.Path.Combine(Me.CurrentSelection.DTE.ActiveDocument.Path, "../Addons") _
                        )

                    If IO.Directory.Exists(AddonsPath) Then
                        For Each AddonPath As String In IO.Directory.GetDirectories(AddonsPath)
                            Dim SearchPath As String = IO.Path.Combine(AddonPath, "Dlls")

                            If AddInControl.AssemblyCacheObject.IsLatest(SearchPath, AddInControl.AssemblyCache.QueryTypes.None) Then
                                For Each AssemblyID As String In AddInControl.AssemblyCacheObject.GetAssemblyIDs(VSAddIn.AddInLoader.SearchTypes.Addon, SearchPath)
                                    MyBase.lwControls.Items.Add(String.Empty, 0)
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(AssemblyID)
                                Next
                            Else
                                For Each AssemblyID As String In MyBase.AddInLoader.GetAssemblies(VSAddIn.AddInLoader.SearchTypes.Addon, SearchPath)
                                    MyBase.lwControls.Items.Add(String.Empty, 0)
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(AssemblyID)

                                    AddInControl.AssemblyCacheObject.AddAssemblyInfo( _
                                        VSAddIn.AddInLoader.SearchTypes.Addon, _
                                        SearchPath, _
                                        AssemblyID _
                                    )
                                Next
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try

            'MyBase.Sort()
        End Sub

        Public Overrides Sub AcceptSelection()
            If MyBase.lwControls.SelectedItems.Count > 0 Then
                Me._AssemblyID = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._AssemblyID = String.Empty
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = System.Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.AssemblySearch, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.AssemblyID}, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                                                                                                                                                                                                                                               Try
                                                                                                                                                                                                                                                                                                   CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)
                                                                                                                                                                                                                                                                                               Catch ex As Exception
                                                                                                                                                                                                                                                                                                   ' Just handle to prevent crash
                                                                                                                                                                                                                                                                                               End Try
                                                                                                                                                                                                                                                                                           End Sub), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilAssembly As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AssemblySearch))
            Me.ilAssembly = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilAssembly
            '
            Me.ilAssembly.ImageStream = CType(resources.GetObject("ilAssembly.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilAssembly.TransparentColor = System.Drawing.Color.Transparent
            Me.ilAssembly.Images.SetKeyName(0, "0assembly.ico")
            '
            'AssemblySearch
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.ClientSize = New System.Drawing.Size(184, 184)
            Me.Name = "AssemblySearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace