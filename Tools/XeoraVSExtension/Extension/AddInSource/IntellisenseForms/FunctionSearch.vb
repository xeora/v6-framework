Namespace Xeora.VSAddIn.Forms
    Public Class FunctionSearch
        Inherits ISFormBase

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            MyBase.New(Selection, BeginningOffset)

            Me.InitializeComponent()

            MyBase.lwControls.SmallImageList = Me.ilFunctions
        End Sub

        Private _SearchType As AddInLoader.SearchTypes = VSAddIn.AddInLoader.SearchTypes.Domain
        Private _SearchPath As String = String.Empty
        Private _AssemblyID As String = String.Empty
        Private _ClassID As String = String.Empty

        Private _FunctionID As String = String.Empty
        Private _FunctionParameterNames As String() = Nothing

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

        Public WriteOnly Property AssemblyID() As String
            Set(ByVal value As String)
                Me._AssemblyID = value
            End Set
        End Property

        Public WriteOnly Property ClassID() As String
            Set(ByVal value As String)
                Me._ClassID = value
            End Set
        End Property

        Public ReadOnly Property FunctionID() As String
            Get
                Return Me._FunctionID
            End Get
        End Property

        Public ReadOnly Property FunctionParameterNames() As String()
            Get
                Return Me._FunctionParameterNames
            End Get
        End Property

        Private _FunctionParametersList As New Generic.Dictionary(Of String, String())

        Public Overrides Sub FillList()
            Try
                Dim SearchPath As String =
                    MyBase.LocateAssembly(Me._SearchType, Me._SearchPath, Me._AssemblyID)

                If AddInControl.AssemblyCacheObject.IsLatest(SearchPath, AddInControl.AssemblyCache.QueryTypes.ClassProcedureListStatus, Me._ClassID) Then
                    For Each cPI As AddInControl.AssemblyCache.AssemblyCacheInfo.ClassObject.ClassProcedureInfo In AddInControl.AssemblyCacheObject.GetAssemblyClassProcedureInfos(Me._SearchType, IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID)), Me._ClassID)
                        Dim MethodGuid As Guid = Guid.NewGuid()

                        MyBase.lwControls.Items.Add(String.Empty, 0)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(cPI.ProcedureID)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(MethodGuid.ToString())

                        Try
                            Me._FunctionParametersList.Add(MethodGuid.ToString(), cPI.ProcedureParams)
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        End Try
                    Next
                Else
                    Dim QueryDll As String = IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID))

                    For Each item As Object() In MyBase.AddInLoader.GetAssembliesClassFunctions(Me._SearchType, QueryDll, Me._ClassID)
                        Dim MethodGuid As Guid = Guid.NewGuid()

                        MyBase.lwControls.Items.Add(String.Empty, 0)
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(CType(item(0), String))
                        MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(MethodGuid.ToString())

                        Try
                            Me._FunctionParametersList.Add(MethodGuid.ToString(), CType(item(1), String()))
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        End Try

                        AddInControl.AssemblyCacheObject.AddClassProcedureInfoIntoAssemblyInfo(Me._SearchType, QueryDll, Me._ClassID, CType(item(0), String), CType(item(1), String()))
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
                            If AddInControl.AssemblyCacheObject.IsLatest(SearchPath, AddInControl.AssemblyCache.QueryTypes.ClassProcedureListStatus, Me._ClassID) Then
                                For Each cPI As AddInControl.AssemblyCache.AssemblyCacheInfo.ClassObject.ClassProcedureInfo In AddInControl.AssemblyCacheObject.GetAssemblyClassProcedureInfos(Xeora.VSAddIn.AddInLoader.SearchTypes.Child, IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID)), Me._ClassID)
                                    Dim MethodGuid As Guid = Guid.NewGuid()

                                    MyBase.lwControls.Items.Add(String.Empty, 0)
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(cPI.ProcedureID)
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(MethodGuid.ToString())

                                    Try
                                        Me._FunctionParametersList.Add(MethodGuid.ToString(), cPI.ProcedureParams)
                                    Catch exFuncSub As Exception
                                        ' Just Handle Exceptions
                                    End Try
                                Next
                            Else
                                Dim QueryDll As String = IO.Path.Combine(SearchPath, String.Format("{0}.dll", Me._AssemblyID))

                                For Each item As Object() In MyBase.AddInLoader.GetAssembliesClassFunctions(Xeora.VSAddIn.AddInLoader.SearchTypes.Child, QueryDll, Me._ClassID)
                                    Dim MethodGuid As Guid = Guid.NewGuid()

                                    MyBase.lwControls.Items.Add(String.Empty, 0)
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(CType(item(0), String))
                                    MyBase.lwControls.Items(MyBase.lwControls.Items.Count - 1).SubItems.Add(MethodGuid.ToString())

                                    Try
                                        Me._FunctionParametersList.Add(MethodGuid.ToString(), CType(item(1), String()))
                                    Catch exFuncSub As Exception
                                        ' Just Handle Exceptions
                                    End Try

                                    AddInControl.AssemblyCacheObject.AddClassProcedureInfoIntoAssemblyInfo(Me._SearchType, QueryDll, Me._ClassID, CType(item(0), String), CType(item(1), String()))
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
                Me._FunctionID = MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text
                Me._FunctionParameterNames = Me._FunctionParametersList.Item(MyBase.lwControls.SelectedItems.Item(0).SubItems.Item(2).Text)
            Else
                Me.CancelSelection()
            End If
        End Sub

        Public Overrides Sub CancelSelection()
            Me._FunctionID = String.Empty
            Me._FunctionParameterNames = Nothing
        End Sub

        Public Overrides Sub HandleResult()
            MyBase.HandleResultDelegate.BeginInvoke(MyBase.WindowHandler, Me.DialogResult = System.Windows.Forms.DialogResult.OK, Me.BeginningOffset, Me.CurrentSelection, Globals.ISTypes.FunctionSearch, Me.AcceptChar, Me.UseCloseChar, New Object() {Me.FunctionID, Me.FunctionParameterNames}, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                                                                                                                                                                                                                                                                          Try
                                                                                                                                                                                                                                                                                                                              CType(aR.AsyncState, AddInControl.HandleResultDelegate).EndInvoke(Nothing, aR)
                                                                                                                                                                                                                                                                                                                          Catch ex As Exception
                                                                                                                                                                                                                                                                                                                              ' Just handle to prevent crash
                                                                                                                                                                                                                                                                                                                          End Try
                                                                                                                                                                                                                                                                                                                      End Sub), MyBase.HandleResultDelegate)
        End Sub

#Region " Form Designer Generated Codes "
        Friend WithEvents ilFunctions As System.Windows.Forms.ImageList
        Private components As System.ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FunctionSearch))
            Me.ilFunctions = New System.Windows.Forms.ImageList(Me.components)
            Me.SuspendLayout()
            '
            'ilFunctions
            '
            Me.ilFunctions.ImageStream = CType(resources.GetObject("ilFunctions.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilFunctions.TransparentColor = System.Drawing.Color.Transparent
            Me.ilFunctions.Images.SetKeyName(0, "0function.png")
            '
            'FunctionSearch
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.ClientSize = New System.Drawing.Size(184, 184)
            Me.Name = "FunctionSearch"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
End Namespace