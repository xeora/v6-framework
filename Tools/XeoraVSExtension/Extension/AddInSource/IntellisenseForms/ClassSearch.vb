Namespace Xeora.VSAddIn.Forms
    Public Class ClassSearch
        Inherits ISFormBase

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.AcceptChar = "."c
            MyBase.lwControls.SmallImageList = Me.ilClasses
        End Sub

        Private _SearchType As AddInLoader.SearchTypes = VSAddIn.AddInLoader.SearchTypes.Domain
        Private _SearchPath As String = String.Empty
        Private _AssemblyID As String = String.Empty

        Private _ClassID As String = String.Empty

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

        Public Property AssemblyID() As String
            Get
                Return Me._AssemblyID
            End Get
            Set(ByVal value As String)
                Me._AssemblyID = value
            End Set
        End Property

        Public ReadOnly Property ClassID() As String
            Get
                Return Me._ClassID
            End Get
        End Property

        Public Overrides Sub FillList()
            Try
                Dim SearchPath As String =
                    MyBase.LocateAssembly(Me._SearchType, Me._SearchPath, Me._AssemblyID)

                If AddInControl.AssemblyCacheObject.IsLatest(SearchPath, AddInControl.AssemblyCache.QueryTypes.ClassListStatus) Then
                    For Each ClassID As String In AddInControl.AssemblyCacheObject.GetAssemblyClassIDs(Me._SearchType, IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID)))
                        MyBase.lwControls.Items.Add(String.Empty, 0)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(ClassID)
                    Next
                Else
                    Dim QueryDll As String = IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID))

                    For Each ClassID As String In MyBase.AddInLoader.GetAssembliesClasses(Me._SearchType, QueryDll)
                        MyBase.lwControls.Items.Add(String.Empty, 0)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(ClassID)

                        AddInControl.AssemblyCacheObject.AddClassIDIntoAssemblyInfo(Me._SearchType, QueryDll, ClassID)
                    Next
                End If
            Catch ex As IO.FileNotFoundException
                ' It is possibly an Addon AssemblyID, Try Addon Paths
                Try
                    Dim AddonsPath As String =
                        IO.Path.GetFullPath(
                            IO.Path.Combine(Me.CurrentSelection.DTE.ActiveDocument.Path, "../Addons")
                        )

                    If IO.Directory.Exists(AddonsPath) Then
                        Dim IsExists As Boolean = False
                        Dim SearchPath As String = String.Empty

                        For Each AddonPath As String In IO.Directory.GetDirectories(AddonsPath)
                            SearchPath = IO.Path.Combine(AddonPath, "Executables")
                            Dim SearchFile As String =
                                IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID))

                            If IO.File.Exists(SearchFile) Then IsExists = True : Exit For
                        Next

                        If IsExists AndAlso Not String.IsNullOrEmpty(SearchPath) Then
                            If AddInControl.AssemblyCacheObject.IsLatest(SearchPath, AddInControl.AssemblyCache.QueryTypes.ClassListStatus) Then
                                For Each ClassID As String In AddInControl.AssemblyCacheObject.GetAssemblyClassIDs(Xeora.VSAddIn.AddInLoader.SearchTypes.Child, IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID)))
                                    MyBase.lwControls.Items.Add(String.Empty, 0)
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(ClassID)
                                Next
                            Else
                                Dim QueryDll As String = IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID))

                                For Each ClassID As String In MyBase.AddInLoader.GetAssembliesClasses(Xeora.VSAddIn.AddInLoader.SearchTypes.Child, QueryDll)
                                    MyBase.lwControls.Items.Add(String.Empty, 0)
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(ClassID)

                                    AddInControl.AssemblyCacheObject.AddClassIDIntoAssemblyInfo(Xeora.VSAddIn.AddInLoader.SearchTypes.Child, QueryDll, ClassID)
                                Next
                            End If
                        End If
                    End If
                Catch exSub As Exception
                    ' Just Handle Exceptions
                End Try
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try

            MyBase.Sort()
        End Sub

        Public Overrides Sub AcceptSelection()
            If MyBase.lwControls.SelectedItems.Count > 0 Then
                Me._ClassID = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._ClassID = String.Empty
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = System.Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.ClassSearch, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.AssemblyID, Me.ClassID}, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                                                                                                                                                                                                                                                        Try
                                                                                                                                                                                                                                                                                                            CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)
                                                                                                                                                                                                                                                                                                        Catch ex As Exception
                                                                                                                                                                                                                                                                                                            ' Just handle to prevent crash
                                                                                                                                                                                                                                                                                                        End Try
                                                                                                                                                                                                                                                                                                    End Sub), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilClasses As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ClassSearch))
            Me.ilClasses = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilClasses
            '
            Me.ilClasses.ImageStream = CType(resources.GetObject("ilClasses.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilClasses.TransparentColor = System.Drawing.Color.Transparent
            Me.ilClasses.Images.SetKeyName(0, "0class.png")
            '
            'ClassSearch
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.ClientSize = New System.Drawing.Size(184, 184)
            Me.Name = "ClassSearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace