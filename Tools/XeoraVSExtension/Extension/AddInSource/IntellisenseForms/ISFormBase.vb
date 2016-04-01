Namespace Xeora.VSAddIn.Forms
    Public MustInherit Class ISFormBase
        Public Delegate Sub EnteredTextDelegate(ByVal CurrentText As String, ByVal BeginningOffset As Integer, ByVal Selection As EnvDTE.TextSelection, ByVal RaiseIntelliSense As Boolean)
        Public Delegate Sub BackSpaceForEmptyTextDelegate(ByVal Selection As EnvDTE.TextSelection)

        Private _WindowHandler As IWin32Window
        Private _CurrentSelection As EnvDTE.TextSelection
        Private _BeginningOffset As Integer

        Public Sub New(ByVal Selection As EnvDTE.TextSelection, ByVal BeginningOffset As Integer)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me.TopMost = True

            Me._CurrentSelection = Selection
            Me._BeginningOffset = BeginningOffset
        End Sub

        Protected ReadOnly Property AddInLoader() As Xeora.VSAddIn.IAddInLoader
            Get
                Return AddInLoaderHelper.AddInLoader
            End Get
        End Property

        Protected ReadOnly Property WindowHandler As IWin32Window
            Get
                Return Me._WindowHandler
            End Get
        End Property

        Private _EnteredTextHandler As EnteredTextDelegate = Nothing
        Private _BackSpaceForEmptyTextHandler As BackSpaceForEmptyTextDelegate = Nothing

        Private _VisibleInLocation As System.Drawing.Point

        Public ReadOnly Property CurrentSelection As EnvDTE.TextSelection
            Get
                Return Me._CurrentSelection
            End Get
        End Property

        Public ReadOnly Property BeginningOffset As Integer
            Get
                Return Me._BeginningOffset
            End Get
        End Property

        Private _UseCloseChar As Char = "$"c

        Public Property UseCloseChar() As Char
            Get
                Return Me._UseCloseChar
            End Get
            Set(ByVal value As Char)
                Me._UseCloseChar = value
            End Set
        End Property

        Private _AcceptChar As Char = "$"c

        Public Property AcceptChar() As Char
            Get
                Return Me._AcceptChar
            End Get
            Set(ByVal value As Char)
                Me._AcceptChar = value
            End Set
        End Property

        Public ReadOnly Property CurrentCache() As String
            Get
                Return Me._CurrentCache
            End Get
        End Property

        Private _HandleResultDelegate As AddInControl.HandleResultDelegate

        Public Property HandleResultDelegate As AddInControl.HandleResultDelegate
            Get
                Return Me._HandleResultDelegate
            End Get
            Set(value As AddInControl.HandleResultDelegate)
                Me._HandleResultDelegate = value
            End Set
        End Property

        Private Sub SearchCacheChanges()
            If Me._BeginningOffset < Me._CurrentSelection.ActivePoint.LineCharOffset Then
                Dim ChangeDifference As Integer =
                    Me._CurrentSelection.ActivePoint.LineCharOffset - Me._BeginningOffset
                Dim editPoint As EnvDTE.EditPoint =
                    Me._CurrentSelection.ActivePoint.CreateEditPoint()

                editPoint.MoveToLineAndOffset(Me._CurrentSelection.ActivePoint.Line, Me._BeginningOffset)

                Me._CurrentCache = editPoint.GetText(ChangeDifference)
                Me._CurrentCacheHistoryIndex = 0
                Me._CacheHistory.Insert(0, Me._CurrentCache)

                Me.CacheChanged()
            End If
        End Sub

        Public Shadows Sub Show()
            ' OK, here is the deal
            ' We are trying to show the form, but i'm closing the empty form if there is nothing to show
            ' Because of this MyBase.Show() throws ObjectDisposedException.
            ' I'm covering with try just to handle my cheep coding.
            Try
                MyBase.Show() : MyBase.Focus()
            Catch ex As ObjectDisposedException
                ' Just handler Exception
            End Try
        End Sub

        Public Shadows Function ShowDialog(ByRef owner As IWin32Window) As DialogResult
            Me._WindowHandler = owner

            Dim DialogThread As System.Threading.Thread =
                New System.Threading.Thread(AddressOf Me.MybaseShowDialog)
            DialogThread.SetApartmentState(System.Threading.ApartmentState.STA)
            DialogThread.Start()
        End Function

        Private Function MybaseShowDialog() As DialogResult
            Return MyBase.ShowDialog(Me._WindowHandler)
        End Function

        Private _RaiseIntelliSense As Boolean = False

        Protected Property RaiseIntelliSense() As Boolean
            Get
                Return Me._RaiseIntelliSense
            End Get
            Set(value As Boolean)
                Me._RaiseIntelliSense = value
            End Set
        End Property

        Private _UnmatchedAutoCompleteCloseLimit As Integer = 0

        Protected Property UnmatchedAutoCompleteCloseLimit() As Integer
            Get
                Return Me._UnmatchedAutoCompleteCloseLimit
            End Get
            Set(value As Integer)
                Me._UnmatchedAutoCompleteCloseLimit = value
            End Set
        End Property

        Public WriteOnly Property VisibleInLocation() As System.Drawing.Point
            Set(ByVal value As System.Drawing.Point)
                Me._VisibleInLocation = value
            End Set
        End Property

        Public WriteOnly Property EnteredTextHandler() As EnteredTextDelegate
            Set(ByVal value As EnteredTextDelegate)
                Me._EnteredTextHandler = value
            End Set
        End Property

        Public WriteOnly Property BackSpaceForEmptyTextHandler() As BackSpaceForEmptyTextDelegate
            Set(ByVal value As BackSpaceForEmptyTextDelegate)
                Me._BackSpaceForEmptyTextHandler = value
            End Set
        End Property

        Public MustOverride Sub FillList()
        Public MustOverride Sub AcceptSelection()
        Public MustOverride Sub CancelSelection()
        Public MustOverride Sub HandleResult()

        Protected Sub Sort(Optional SortByValueIndex As Integer = 1, Optional CompareAsInteger As Boolean = False)
            Me.lwControls.ListViewItemSorter = New ControlIDSorter(SortByValueIndex, CompareAsInteger)
            Me.lwControls.Sorting = SortOrder.Ascending
            Me.lwControls.Sort()
        End Sub

        Private Class ControlIDSorter
            Implements IComparer

            Private _SortByValueIndex As Integer
            Private _CompareAsInteger As Boolean

            Public Sub New(ByVal SortByValueIndex As Integer, ByVal CompareAsInteger As Boolean)
                Me._SortByValueIndex = SortByValueIndex
                Me._CompareAsInteger = CompareAsInteger
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim lItem1 As ListViewItem = CType(x, ListViewItem)
                Dim lItem2 As ListViewItem = CType(y, ListViewItem)

                If Me._CompareAsInteger Then
                    Dim ComV1 As Integer = Integer.Parse(lItem1.SubItems.Item(Me._SortByValueIndex).Text)
                    Dim ComV2 As Integer = Integer.Parse(lItem2.SubItems.Item(Me._SortByValueIndex).Text)

                    If ComV1 < ComV2 Then Return -1
                    If ComV1 = ComV2 Then Return 0
                    If ComV1 > ComV2 Then Return 1
                Else
                    Return String.Compare(lItem1.SubItems.Item(Me._SortByValueIndex).Text, lItem2.SubItems.Item(Me._SortByValueIndex).Text)
                End If
            End Function
        End Class

        Protected Overridable Sub _AcceptSelection()
            If Me.lwControls.SelectedItems.Count = 1 Then
                Me.AcceptSelection()

                Me.Close(System.Windows.Forms.DialogResult.OK)
            Else
                Me._CancelSelection()
            End If
        End Sub

        Protected Overridable Sub _CancelSelection()
            Me.CancelSelection()

            Me.Close(System.Windows.Forms.DialogResult.Cancel)
        End Sub

        Public Shadows Sub Close(ByVal DialogResult As System.Windows.Forms.DialogResult)
            Me.DialogResult = DialogResult
            Me.HandleResult()

            MyBase.Close()
        End Sub

        Private Sub IntellisenseForm_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
            If Me.DialogResult = System.Windows.Forms.DialogResult.None Then Me._CancelSelection()
        End Sub

        Private Sub lwControls_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lwControls.DoubleClick
            If Me.lwControls.SelectedItems.Count = 1 Then
                Me._AcceptSelection()
            End If
        End Sub

        Private Sub lwControls_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lwControls.SelectedIndexChanged
            Me.PrepareVisibleSize(True)
        End Sub

        Private _CacheHistory As New Generic.List(Of String)
        Private _CurrentCacheHistoryIndex As Integer = -1
        Private _CurrentCache As String
        Private _IsModifierSent As Boolean = False

        Private Sub IntellisenseForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
            If e.Modifiers <> Keys.None Then Me._IsModifierSent = True Else Me._IsModifierSent = False

            Select Case e.KeyCode
                Case Keys.V
                    If e.Modifiers = Keys.Control Then
                        Dim ClipData As IDataObject =
                            Clipboard.GetDataObject()

                        If ClipData.GetDataPresent(GetType(String)) Then
                            Me._CurrentCache &= CType(ClipData.GetData(GetType(String)), String)
                            Me._CacheHistory.Add(Me._CurrentCache)

                            Me.CacheChanged()
                        End If
                    End If
                Case Keys.Z
                    If e.Modifiers = Keys.Control Then
                        If Me._CurrentCacheHistoryIndex > 0 Then
                            Me._CurrentCacheHistoryIndex -= 1
                            Me._CurrentCache = Me._CacheHistory.Item(Me._CurrentCacheHistoryIndex)
                        Else
                            Me._CurrentCache = String.Empty
                        End If

                        Me.CacheChanged()
                    End If
                Case Keys.Y
                    If e.Modifiers = Keys.Control Then
                        If Me._CurrentCacheHistoryIndex + 1 < Me._CacheHistory.Count Then
                            Me._CurrentCacheHistoryIndex += 1
                            Me._CurrentCache = Me._CacheHistory.Item(Me._CurrentCacheHistoryIndex)
                        End If

                        Me.CacheChanged()
                    End If
                Case Keys.Back
                    If String.IsNullOrEmpty(Me._CurrentCache) Then
                        If Not Me._BackSpaceForEmptyTextHandler Is Nothing Then Me._BackSpaceForEmptyTextHandler.Invoke(Me._CurrentSelection)

                        Me._CancelSelection()
                    Else
                        Me._CurrentCache = Me._CurrentCache.Substring(0, Me._CurrentCache.Length - 1)
                    End If
                Case Keys.Enter, Keys.Return, Keys.Tab
                    Me._AcceptSelection()
                Case Keys.Escape, Keys.Right, Keys.Left
                    Me._CancelSelection()
                Case Else
                    ' Skip Control key combinations cause it creates invisiable characters
                    If e.Modifiers = Keys.Control Then Me._IsModifierSent = True Else Me._IsModifierSent = False
            End Select
        End Sub

        Private Sub IntellisenseForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            If String.Compare(e.KeyChar, Me._AcceptChar) = 0 Then
                Me._AcceptSelection()
            Else
                If Not Me._IsModifierSent Then
                    If Asc(e.KeyChar) <> 8 Then
                        Me._CurrentCache &= e.KeyChar

                        Me._CurrentCacheHistoryIndex += 1
                        Me._CacheHistory.Insert(Me._CurrentCacheHistoryIndex, Me._CurrentCache)

                        Me.RebuildCacheHistory()
                    End If

                    Me.CacheChanged()
                End If
            End If
        End Sub

        Private Sub CacheChanged()
            If Not Me._EnteredTextHandler Is Nothing AndAlso
                Not Me._CurrentCache Is Nothing Then

                Me._EnteredTextHandler.BeginInvoke(Me._CurrentCache, Me._BeginningOffset, Me._CurrentSelection, Me._RaiseIntelliSense, New AsyncCallback(AddressOf Me.EnteredTextHandlerEnd), Me._EnteredTextHandler)
            End If

            If Not String.IsNullOrEmpty(Me._CurrentCache) Then
                Dim LWItem As ListViewItem =
                    Me.SearchListViewItem()

                If Not LWItem Is Nothing Then
                    LWItem.Selected = True
                    LWItem.Focused = True

                    Me.lwControls.EnsureVisible(LWItem.Index)
                Else
                    If Me._UnmatchedAutoCompleteCloseLimit > 0 AndAlso
                        Me._CurrentCache.Length >= Me._UnmatchedAutoCompleteCloseLimit Then

                        Me._CancelSelection()
                    End If
                End If
            End If
        End Sub

        Private Sub EnteredTextHandlerEnd(ByVal aI As IAsyncResult)
            CType(aI.AsyncState, EnteredTextDelegate).EndInvoke(aI)
        End Sub

        Private Function SearchListViewItem() As ListViewItem
            Dim rItem As ListViewItem = Nothing

            For Each item As ListViewItem In Me.lwControls.Items
                If TypeOf Me Is SpecialSearch Then
                    If item.SubItems(3).Text.IndexOf(Me._CurrentCache, 0, StringComparison.InvariantCultureIgnoreCase) = 0 Then _
                        rItem = item : Exit For
                Else
                    If item.SubItems(1).Text.IndexOf(Me._CurrentCache, 0, StringComparison.InvariantCultureIgnoreCase) = 0 Then _
                        rItem = item : Exit For
                End If
            Next

            Return rItem
        End Function

        Private Sub RebuildCacheHistory()
            For hC As Integer = Me._CacheHistory.Count - 1 To (Me._CurrentCacheHistoryIndex + 1) Step -1
                Me._CacheHistory.RemoveAt(hC)
            Next
        End Sub

        Private Sub PrepareVisibleSize(ByVal AccordingToSelected As Boolean)
            Dim HighestWidth As Integer, TotalHeight As Integer

            If AccordingToSelected Then
                If Me.lwControls.SelectedItems.Count > 0 Then
                    HighestWidth = Me.lwControls.Columns.Item(1).Width

                    Dim Size As System.Drawing.SizeF =
                        System.Drawing.Graphics.FromHwnd(Me.Handle).MeasureString(Me.lwControls.SelectedItems.Item(0).SubItems.Item(1).Text, Me.lwControls.SelectedItems.Item(0).SubItems.Item(1).Font)

                    If HighestWidth < (CType(Size.Width, Integer) + 20) Then
                        HighestWidth = CType(Size.Width, Integer) + 20

                        Me.lwControls.Columns.Item(1).Width = HighestWidth

                        ' Add First Column Width
                        HighestWidth += Me.lwControls.Columns.Item(0).Width

                        ' Add If vertical scrollbar will be visiable
                        If Me.lwControls.Items.Count > 9 Then HighestWidth += 30 Else HighestWidth += 4

                        Me.Size = New System.Drawing.Size(HighestWidth, Me.Size.Height)
                    End If
                End If
            Else
                For Each lwItem As System.Windows.Forms.ListViewItem In Me.lwControls.Items
                    Dim Size As System.Drawing.SizeF =
                        System.Drawing.Graphics.FromHwnd(Me.Handle).MeasureString(lwItem.SubItems.Item(1).Text, lwItem.SubItems.Item(1).Font)

                    If (Size.Width + 20) > HighestWidth Then HighestWidth = CType(Size.Width, Integer) + 20
                    If lwItem.Index < 10 Then TotalHeight += CType(Size.Height, Integer) + 4 Else Exit For
                Next

                Me.lwControls.Columns.Item(1).Width = HighestWidth

                ' Add First Column Width
                HighestWidth += Me.lwControls.Columns.Item(0).Width

                ' Add If vertical scrollbar will be visiable
                If Me.lwControls.Items.Count > 9 Then HighestWidth += 30 Else HighestWidth += 4 : TotalHeight += 4

                Me.Size = New System.Drawing.Size(HighestWidth, TotalHeight)
            End If
        End Sub

        Private Sub ISFormBase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Me.lwControls.Items.Count = 0 Then
                Me._CancelSelection()
            Else
                Me.Location = Me._VisibleInLocation
                Me.PrepareVisibleSize(False)
            End If
        End Sub

        Private Sub ISFormBase_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
            Me.lwControls.Focus()

            If Me.lwControls.Items.Count > 0 AndAlso
                Me.lwControls.SelectedItems.Count = 0 Then

                Me.lwControls.Items.Item(0).Selected = True
            End If

            Me.SearchCacheChanges()
        End Sub

        Protected Function LocateAssembly(ByRef sT As AddInLoader.SearchTypes, ByVal SearchPath As String, ByVal AssemblyID As String) As String
            Dim rString As String = String.Empty

            Select Case sT
                Case VSAddIn.AddInLoader.SearchTypes.Child
                    For Each AssemblyFile As String In IO.Directory.GetFiles(SearchPath, "*.dll")
                        If IO.Path.GetFileName(AssemblyFile).IndexOf(AssemblyID) = 0 Then _
                            sT = VSAddIn.AddInLoader.SearchTypes.Child : rString = IO.Path.GetDirectoryName(AssemblyFile) : Exit For
                    Next

                    If String.IsNullOrEmpty(rString) Then
                        For Each AssemblyFile As String In IO.Directory.GetFiles(IO.Path.Combine(SearchPath, "../../../Executables"), "*.dll")
                            If IO.Path.GetFileName(AssemblyFile).IndexOf(AssemblyID) = 0 Then _
                                sT = VSAddIn.AddInLoader.SearchTypes.Domain : rString = IO.Path.GetDirectoryName(AssemblyFile) : Exit For
                        Next

                        If String.IsNullOrEmpty(rString) Then sT = VSAddIn.AddInLoader.SearchTypes.Domain : rString = SearchPath
                    End If
                Case VSAddIn.AddInLoader.SearchTypes.Domain
                    For Each AssemblyFile As String In IO.Directory.GetFiles(SearchPath, "*.dll")
                        If IO.Path.GetFileName(AssemblyFile).IndexOf(AssemblyID) = 0 Then _
                            sT = VSAddIn.AddInLoader.SearchTypes.Domain : rString = IO.Path.GetDirectoryName(AssemblyFile) : Exit For
                    Next

                    If String.IsNullOrEmpty(rString) Then sT = VSAddIn.AddInLoader.SearchTypes.Domain : rString = SearchPath
            End Select

            Return rString
        End Function

        Private Sub ISFormBase_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
            'Me._AddInLoader.Unload()
        End Sub
    End Class
End Namespace