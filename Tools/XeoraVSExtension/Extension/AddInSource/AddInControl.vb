Option Strict On

Imports Microsoft.VisualStudio.CommandBars
Imports EnvDTE

Namespace Xeora.VSAddIn
    Public Class AddInControl
        Implements IWin32Window

        Private _applicationObject As EnvDTE.DTE
        Private _eventContainer As New Generic.List(Of CommandBarEvents)

        Private _solutionEvents As SolutionEvents
        Private _documentEvents As New Generic.Dictionary(Of String, DocumentEvents)
        Private _windowEvents As New Generic.Dictionary(Of String, WindowEvents)

        Public Sub New(ByRef application As EnvDTE.DTE)
            Me._applicationObject = application

            For Each oCommandBar As CommandBar In CType(Me._applicationObject.CommandBars, CommandBars)
                If String.Compare(oCommandBar.Name, "HTML Context") = 0 OrElse
                   String.Compare(oCommandBar.Name, "Code Window") = 0 Then
                    Dim oPopup As CommandBarPopup =
                        CType(
                            oCommandBar.Controls.Add(
                                MsoControlType.msoControlPopup,
                                System.Reflection.Missing.Value,
                                System.Reflection.Missing.Value, 1, True),
                            CommandBarPopup
                        )
                    oPopup.Caption = "Xeora³"

                    ' Goto Control Referance
                    Dim oControl As CommandBarButton =
                        CType(
                            oPopup.Controls.Add(
                                MsoControlType.msoControlButton,
                                System.Reflection.Missing.Value,
                                System.Reflection.Missing.Value, 1, True),
                            CommandBarButton
                        )
                    oControl.Caption = "Go To Definition"
                    oControl.ShortcutText = "Ctrl+Shift+D, Ctrl+Shift+D"

                    Dim cbEvent As CommandBarEvents =
                        CType(Me._applicationObject.Events.CommandBarEvents(oControl), CommandBarEvents)

                    AddHandler cbEvent.Click, AddressOf Me.GotoControlReferance

                    Me._eventContainer.Add(cbEvent)
                End If
            Next

            Me._solutionEvents = CType(Me._applicationObject.Events.SolutionEvents(), SolutionEvents)

            AddHandler Me._solutionEvents.Opened, AddressOf Me.SolutionOpened
            AddHandler Me._solutionEvents.AfterClosing, AddressOf Me.SolutionClosed
        End Sub

        Private Sub SolutionOpened()
            Dim mainWindowEvent As WindowEvents =
                CType(Me._applicationObject.Events.WindowEvents(Me._applicationObject.MainWindow), WindowEvents)

            AddHandler mainWindowEvent.WindowActivated, AddressOf Me.CheckContextMenu

            Me._windowEvents.Item(String.Format("{0}-{1}", Me._applicationObject.MainWindow.Caption, Me._applicationObject.MainWindow.Kind)) = mainWindowEvent

            Me.AssignHandlersToWindows(Me._applicationObject.Windows, Nothing)

            Dim documentEvent As DocumentEvents =
                CType(Me._applicationObject.Events.DocumentEvents(), DocumentEvents)

            AddHandler documentEvent.DocumentOpened, AddressOf Me.DocumentOpened

            Me._documentEvents.Item("BASE-FIRST") = documentEvent

            Me.AssignHandlersToDocuments(Me._applicationObject.Documents)
        End Sub

        Private Sub SolutionClosed()
            Me._documentEvents = New Generic.Dictionary(Of String, DocumentEvents)
            Me._windowEvents = New Generic.Dictionary(Of String, WindowEvents)
        End Sub

        Private Sub AssignHandlersToDocuments(ByRef documents As Documents)
            If Not documents Is Nothing Then
                For Each document As Document In documents
                    Dim documentEvent As DocumentEvents =
                        CType(Me._applicationObject.Events.DocumentEvents(document), DocumentEvents)

                    AddHandler documentEvent.DocumentClosing, AddressOf Me.DocumentClosing

                    Me._documentEvents.Item(String.Format("{0}-{1}", document.Name, document.Kind)) = documentEvent
                Next
            End If
        End Sub

        Private Sub AssignHandlersToWindows(ByRef windows As EnvDTE.Windows, ByRef linkedWindows As LinkedWindows)
            If Not windows Is Nothing Then
                For Each window As Window In windows
                    Dim windowEvent As WindowEvents =
                        CType(Me._applicationObject.Events.WindowEvents(window), WindowEvents)

                    AddHandler windowEvent.WindowActivated, AddressOf Me.CheckContextMenu

                    Me._windowEvents.Item(String.Format("{0}-{1}", window.Caption, window.Kind)) = windowEvent

                    If Not window.LinkedWindowFrame Is Nothing Then
                        windowEvent = CType(Me._applicationObject.Events.WindowEvents(window.LinkedWindowFrame), WindowEvents)

                        AddHandler windowEvent.WindowActivated, AddressOf Me.CheckContextMenu

                        Me._windowEvents.Item(String.Format("{0}-{1}", window.LinkedWindowFrame.Caption, window.LinkedWindowFrame.Kind)) = windowEvent
                    End If

                    If Not window.LinkedWindows Is Nothing AndAlso window.LinkedWindows.Count > 0 Then _
                        Me.AssignHandlersToWindows(Nothing, window.LinkedWindows)
                Next
            End If

            If Not linkedWindows Is Nothing Then
                For Each window As Window In linkedWindows
                    Dim windowEvent As WindowEvents =
                        CType(Me._applicationObject.Events.WindowEvents(window), WindowEvents)

                    AddHandler windowEvent.WindowActivated, AddressOf Me.CheckContextMenu

                    Me._windowEvents.Item(String.Format("{0}-{1}", window.Caption, window.Kind)) = windowEvent

                    If Not window.LinkedWindowFrame Is Nothing Then
                        windowEvent = CType(Me._applicationObject.Events.WindowEvents(window.LinkedWindowFrame), WindowEvents)

                        AddHandler windowEvent.WindowActivated, AddressOf Me.CheckContextMenu

                        Me._windowEvents.Item(String.Format("{0}-{1}", window.LinkedWindowFrame.Caption, window.LinkedWindowFrame.Kind)) = windowEvent
                    End If

                    If Not window.LinkedWindows Is Nothing AndAlso window.LinkedWindows.Count > 0 Then _
                        Me.AssignHandlersToWindows(Nothing, window.LinkedWindows)
                Next
            End If
        End Sub

        Private Sub DocumentOpened(ByVal document As Document)
            Dim documentEvent As DocumentEvents =
                CType(Me._applicationObject.Events.DocumentEvents(document), DocumentEvents)

            AddHandler documentEvent.DocumentClosing, AddressOf Me.DocumentClosing

            Me._documentEvents.Item(String.Format("{0}-{1}", document.Name, document.Kind)) = documentEvent
        End Sub

        Private Sub DocumentClosing(ByVal document As Document)
            Me._windowEvents.Remove(String.Format("{0}-{1}", document.Name, document.Kind))
        End Sub

        Private Sub CheckContextMenu(ByVal GotFocus As Window, ByVal LostFocus As Window)
            If Not Me._applicationObject.ActiveDocument Is Nothing Then
                Dim ActiveDocFI As IO.FileInfo =
                    New IO.FileInfo(Me._applicationObject.ActiveDocument.FullName)
                Dim IsControlMapXMLFile As Boolean =
                    String.Compare(ActiveDocFI.Name, "ControlsMap.xml", True) = 0

                For Each oCommandBar As CommandBar In CType(Me._applicationObject.CommandBars, CommandBars)
                    If String.Compare(oCommandBar.Name, "Code Window") = 0 Then
                        For Each oCommandBarControl As CommandBarControl In oCommandBar.Controls
                            If String.Compare(oCommandBarControl.Caption, "Xeora³") = 0 Then
                                oCommandBarControl.Visible = IsControlMapXMLFile

                                Exit For
                            End If
                        Next

                        Exit For
                    End If
                Next
            End If
        End Sub

        Private Shared _AssemblyCacheObject As AssemblyCache = Nothing

        Public Shared ReadOnly Property AssemblyCacheObject As AssemblyCache
            Get
                If AddInControl._AssemblyCacheObject Is Nothing Then _
                    AddInControl._AssemblyCacheObject = New AssemblyCache

                Return AddInControl._AssemblyCacheObject
            End Get
        End Property

        Public Class AssemblyCache
            Private _AssemblyCache As Generic.List(Of AssemblyCacheInfo)

            Public Sub New()
                Me._AssemblyCache = New Generic.List(Of AssemblyCacheInfo)
            End Sub

            Public Enum QueryTypes
                None
                ClassListStatus
                ClassProcedureListStatus
            End Enum

            Public Function IsLatest(ByVal AssemblyPath As String, ByVal QueryType As QueryTypes, Optional ByVal ClassID As String = Nothing) As Boolean
                Dim rBoolean As Boolean = False

                For Each AssemblyFile As String In IO.Directory.GetFiles(AssemblyPath, "*.dll")
                    Dim AssemblyFileInfo As New IO.FileInfo(AssemblyFile)

                    For Each ACI As AssemblyCacheInfo In Me._AssemblyCache
                        If String.Compare(ACI.AssemblyFile, AssemblyFile) = 0 AndAlso
                            Date.Compare(ACI.AssemblyDate, AssemblyFileInfo.LastWriteTime) >= 0 Then

                            Select Case QueryType
                                Case QueryTypes.ClassListStatus
                                    If ACI.ClassListTouched Then rBoolean = True
                                Case QueryTypes.ClassProcedureListStatus
                                    rBoolean = True

                                    If Not String.IsNullOrEmpty(ClassID) Then
                                        Dim CO As AssemblyCacheInfo.ClassObject =
                                            ACI.GetClass(ClassID)

                                        If Not CO Is Nothing AndAlso Not CO.ProcedureListTouched Then rBoolean = False
                                    End If
                                Case Else
                                    rBoolean = True
                            End Select

                            Exit For
                        End If
                    Next
                Next

                Return rBoolean
            End Function

            Public Sub AddAssemblyInfo(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyPath As String, ByVal AssemblyID As String)
                For aCIC As Integer = Me._AssemblyCache.Count - 1 To 0 Step -1
                    Dim aCI As AssemblyCacheInfo = Me._AssemblyCache.Item(aCIC)

                    If aCI.AssemblyType = AssemblyType AndAlso
                        String.Compare(aCI.AssemblyPath, AssemblyPath) = 0 AndAlso
                        String.Compare(aCI.AssemblyID, AssemblyID) = 0 Then

                        Me._AssemblyCache.RemoveAt(aCIC)
                    End If
                Next

                Me._AssemblyCache.Add(
                    New AssemblyCacheInfo(AssemblyType, AssemblyPath, AssemblyID))
            End Sub

            Public Sub AddClassIDIntoAssemblyInfo(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyFile As String, ByVal ClassID As String)
                Dim tACI As AssemblyCacheInfo = Nothing

                For aCIC As Integer = Me._AssemblyCache.Count - 1 To 0 Step -1
                    Dim aCI As AssemblyCacheInfo = Me._AssemblyCache.Item(aCIC)

                    If aCI.AssemblyType = AssemblyType AndAlso
                        String.Compare(aCI.AssemblyFile, AssemblyFile) = 0 Then

                        tACI = aCI

                        Me._AssemblyCache.RemoveAt(aCIC)
                    End If
                Next

                If Not tACI Is Nothing Then
                    tACI.AddClassID(ClassID)

                    Me._AssemblyCache.Add(tACI)
                End If
            End Sub

            Public Sub AddClassProcedureInfoIntoAssemblyInfo(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyFile As String, ByVal ClassID As String, ByVal ProcedureID As String, ByVal ProcedureParams As String())
                Dim tACI As AssemblyCacheInfo = Nothing

                For aCIC As Integer = Me._AssemblyCache.Count - 1 To 0 Step -1
                    Dim aCI As AssemblyCacheInfo = Me._AssemblyCache.Item(aCIC)

                    If aCI.AssemblyType = AssemblyType AndAlso
                        String.Compare(aCI.AssemblyFile, AssemblyFile) = 0 Then

                        tACI = aCI

                        Me._AssemblyCache.RemoveAt(aCIC)
                    End If
                Next

                If Not tACI Is Nothing Then
                    Dim CO As AssemblyCacheInfo.ClassObject =
                        tACI.GetClass(ClassID)

                    If Not CO Is Nothing Then CO.AddProcedureInfo(ProcedureID, ProcedureParams) : tACI.AddClassobject(CO)

                    Me._AssemblyCache.Add(tACI)
                End If
            End Sub

            Public Function GetAssemblyIDs(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyPath As String) As String()
                Dim rStringList As New Generic.List(Of String)

                For Each aCI As AssemblyCacheInfo In Me._AssemblyCache
                    If AssemblyType = aCI.AssemblyType AndAlso
                        String.Compare(AssemblyPath, aCI.AssemblyPath) = 0 Then _
                        rStringList.Add(aCI.AssemblyID)
                Next

                Return rStringList.ToArray()
            End Function

            Public Function GetAssemblyClassIDs(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyFile As String) As String()
                Dim rClassIDs As String() = New String() {}

                For Each aCI As AssemblyCacheInfo In Me._AssemblyCache
                    If AssemblyType = aCI.AssemblyType AndAlso
                        String.Compare(AssemblyFile, aCI.AssemblyFile) = 0 Then _
                        rClassIDs = aCI.ClassIDList : Exit For
                Next

                Return rClassIDs
            End Function

            Public Function GetAssemblyClassProcedureInfos(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyFile As String, ByVal ClassID As String) As AssemblyCacheInfo.ClassObject.ClassProcedureInfo()
                Dim rProcedureInfos As New Generic.List(Of AssemblyCacheInfo.ClassObject.ClassProcedureInfo)

                For Each aCI As AssemblyCacheInfo In Me._AssemblyCache
                    If AssemblyType = aCI.AssemblyType AndAlso
                        String.Compare(AssemblyFile, aCI.AssemblyFile) = 0 Then

                        Dim CO As AssemblyCacheInfo.ClassObject =
                            aCI.GetClass(ClassID)

                        If Not CO Is Nothing Then rProcedureInfos.AddRange(CO.ClassProcedures) : Exit For
                    End If
                Next

                Return rProcedureInfos.ToArray()
            End Function

            Public Class AssemblyCacheInfo
                Private _AssemblyType As AddInLoader.SearchTypes
                Private _AssemblyPath As String
                Private _AssemblyID As String
                Private _AssemblyFile As String
                Private _AssemblyDate As Date

                Private _ClassListTouched As Boolean
                Private _ClassObjects As Generic.List(Of ClassObject)

                Public Sub New(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyPath As String, ByVal AssemblyID As String)
                    Me._AssemblyType = AssemblyType
                    Me._AssemblyPath = AssemblyPath
                    Me._AssemblyID = AssemblyID

                    Me._AssemblyFile = IO.Path.Combine(Me._AssemblyPath, String.Format("{0}.dll", Me._AssemblyID))

                    If IO.File.Exists(Me._AssemblyFile) Then Me._AssemblyDate = IO.File.GetLastWriteTime(Me._AssemblyFile) Else Me._AssemblyDate = Date.MaxValue

                    Me._ClassListTouched = False
                    Me._ClassObjects = New Generic.List(Of ClassObject)
                End Sub

                Public ReadOnly Property AssemblyType As AddInLoader.SearchTypes
                    Get
                        Return Me._AssemblyType
                    End Get
                End Property

                Public ReadOnly Property AssemblyPath As String
                    Get
                        Return Me._AssemblyPath
                    End Get
                End Property

                Public ReadOnly Property AssemblyID As String
                    Get
                        Return Me._AssemblyID
                    End Get
                End Property

                Public ReadOnly Property AssemblyFile As String
                    Get
                        Return Me._AssemblyFile
                    End Get
                End Property

                Public ReadOnly Property AssemblyDate As Date
                    Get
                        Return Me._AssemblyDate
                    End Get
                End Property

                Public ReadOnly Property ClassListTouched As Boolean
                    Get
                        Return Me._ClassListTouched
                    End Get
                End Property

                Public ReadOnly Property ClassIDList As String()
                    Get
                        Dim rClassIDList As New Generic.List(Of String)

                        For Each CO As ClassObject In Me._ClassObjects
                            rClassIDList.Add(CO.ClassID)
                        Next

                        Return rClassIDList.ToArray()
                    End Get
                End Property

                Public Function GetClass(ByVal ClassID As String) As ClassObject
                    Dim rCO As ClassObject = Nothing

                    For Each CO As ClassObject In Me._ClassObjects
                        If String.Compare(CO.ClassID, ClassID) = 0 Then _
                            rCO = CO : Exit For
                    Next

                    Return rCO
                End Function

                Public Sub AddClassID(ByVal ClassID As String)
                    For cOC As Integer = Me._ClassObjects.Count - 1 To 0 Step -1
                        Dim CO As ClassObject = Me._ClassObjects.Item(cOC)

                        If String.Compare(CO.ClassID, ClassID) = 0 Then _
                            Me._ClassObjects.RemoveAt(cOC)
                    Next

                    Me._ClassObjects.Add(
                        New ClassObject(ClassID))

                    Me._ClassListTouched = True
                End Sub

                Public Sub AddClassobject(ByVal ClassObject As ClassObject)
                    For cOC As Integer = Me._ClassObjects.Count - 1 To 0 Step -1
                        Dim CO As ClassObject = Me._ClassObjects.Item(cOC)

                        If String.Compare(CO.ClassID, ClassObject.ClassID) = 0 Then _
                            Me._ClassObjects.RemoveAt(cOC)
                    Next

                    Me._ClassObjects.Add(ClassObject)

                    Me._ClassListTouched = True
                End Sub

                Public Class ClassObject
                    Private _ClassID As String
                    Private _ClassProcedures As Generic.List(Of ClassProcedureInfo)

                    Private _ProcedureListTouched As Boolean

                    Public Sub New(ByVal ClassID As String)
                        Me._ClassID = ClassID
                        Me._ClassProcedures = New Generic.List(Of ClassProcedureInfo)

                        Me._ProcedureListTouched = False
                    End Sub

                    Public ReadOnly Property ClassID As String
                        Get
                            Return Me._ClassID
                        End Get
                    End Property

                    Public ReadOnly Property ClassProcedures As ClassProcedureInfo()
                        Get
                            Return Me._ClassProcedures.ToArray()
                        End Get
                    End Property

                    Public ReadOnly Property ProcedureListTouched As Boolean
                        Get
                            Return Me._ProcedureListTouched
                        End Get
                    End Property

                    Public Sub AddProcedureInfo(ByVal ProcedureID As String, ByVal ProcedureParams As String())
                        For cPIC As Integer = Me._ClassProcedures.Count - 1 To 0 Step -1
                            Dim CPI As ClassProcedureInfo = Me._ClassProcedures.Item(cPIC)

                            If String.Compare(CPI.ProcedureID, ProcedureID) = 0 AndAlso
                                Me.ListCompare(CPI.ProcedureParams, ProcedureParams) Then _
                                Me._ClassProcedures.RemoveAt(cPIC)
                        Next

                        Me._ClassProcedures.Add(
                            New ClassProcedureInfo(ProcedureID, ProcedureParams))

                        Me._ProcedureListTouched = True
                    End Sub

                    Private Function ListCompare(ByVal Params1 As String(), Params2 As String()) As Boolean
                        Dim rBoolean As Boolean = False

                        If Params1 Is Nothing AndAlso Params2 Is Nothing Then
                            rBoolean = True
                        ElseIf Params1 Is Nothing OrElse Params2 Is Nothing Then
                            rBoolean = False
                        ElseIf Params1.Length <> Params2.Length Then
                            rBoolean = False
                        ElseIf Params1.Length = Params2.Length Then
                            rBoolean = True

                            For pC As Integer = 0 To Params1.Length - 1
                                If String.Compare(Params1(pC), Params2(pC)) <> 0 Then rBoolean = False : Exit For
                            Next
                        End If

                        Return rBoolean
                    End Function

                    Public Class ClassProcedureInfo
                        Private _ProcedureID As String
                        Private _ProcedureParams As String()

                        Public Sub New(ByVal ProcedureID As String, ByVal ProcedureParams As String())
                            Me._ProcedureID = ProcedureID
                            Me._ProcedureParams = ProcedureParams
                        End Sub

                        Public ReadOnly Property ProcedureID As String
                            Get
                                Return Me._ProcedureID
                            End Get
                        End Property

                        Public ReadOnly Property ProcedureParams As String()
                            Get
                                Return Me._ProcedureParams
                            End Get
                        End Property
                    End Class
                End Class
            End Class
        End Class

        Private Function GetReferanceStringFromBegining(ByRef editPoint As EnvDTE.EditPoint, ByRef ControlTypeReferance As Char, ByRef Offset As Integer, ByVal IsXMLFile As Boolean) As String
            Dim SearchID As String = String.Empty
            Dim LastString As String = String.Empty

            Dim OriginalOffset As Integer =
                editPoint.LineCharOffset

            Do Until editPoint.LineCharOffset = 1
                editPoint.CharLeft()

                LastString = editPoint.GetText(1)
                SearchID = String.Concat(LastString, SearchID)

                If String.IsNullOrWhiteSpace(LastString) OrElse
                    (SearchID.Length = 2 AndAlso String.Compare(SearchID, "$$") = 0) Then
                    editPoint.CharRight(SearchID.Length)

                    SearchID = String.Empty

                    Exit Do
                End If

                Dim SearchMatch As System.Text.RegularExpressions.Match

                If IsXMLFile Then
                    SearchMatch = System.Text.RegularExpressions.Regex.Match(SearchID, "\<Bind\>")
                Else
                    SearchMatch = System.Text.RegularExpressions.Regex.Match(SearchID, "\$\w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?\:|\$C(\<\d+(\+)?\>)?\[|\$C")
                End If

                If SearchMatch.Success Then
                    If IsXMLFile Then
                        ControlTypeReferance = Char.MinValue
                    Else
                        ControlTypeReferance = SearchMatch.Value.Chars(1)
                    End If
                    Offset = editPoint.LineCharOffset + SearchMatch.Length

                    editPoint.CharRight(SearchMatch.Length)

                    Exit Do
                Else
                    If editPoint.LineCharOffset = 0 Then SearchID = String.Empty
                End If
            Loop

            Return SearchID
        End Function

        Private Function CheckIsLastingStatement(ByRef editPoint As EnvDTE.EditPoint) As Boolean
            Dim rBoolean As Boolean = False

            Dim SearchID As String = String.Empty
            Dim LastString As String = String.Empty

            ' Check for statements without block or block statement closings
            Dim StatementRegEx As String() =
                New String() _
                {
                    "\$((([#]+|[\^\-\+\*\~])?(\w+)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)\$|\w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?\:([\.\w\-]+\$|[\.\w\-]+\?[\.\w\-]+(\,((\|)?([#\.\^\-\+\*\~]*([\w+][^\$]*)|\=([\S+][^\$]*)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)?)*)?\$))|\}\:[\.\w\-]+\:\{|\}\:[\.\w\-]+\$",
                    "\}:(?<ItemID>[\.\w\-]+)\$"
                }

            Dim OriginalOffset As Integer =
                editPoint.LineCharOffset

            Do Until editPoint.LineCharOffset = 1
                editPoint.CharLeft()

                LastString = editPoint.GetText(1)
                SearchID = String.Concat(LastString, SearchID)

                If String.IsNullOrWhiteSpace(LastString) OrElse
                    (SearchID.Length = 2 AndAlso String.Compare(SearchID, "$$") = 0) Then
                    editPoint.CharRight(SearchID.Length)

                    SearchID = String.Empty

                    Exit Do
                End If

                For mC As Integer = 0 To 1
                    Dim SearchMatch As System.Text.RegularExpressions.Match =
                        System.Text.RegularExpressions.Regex.Match(SearchID, StatementRegEx(mC))

                    If SearchMatch.Success Then
                        rBoolean = (editPoint.LineCharOffset + SearchMatch.Length) = OriginalOffset

                        editPoint.CharRight(SearchMatch.Length)

                        Exit Do
                    Else
                        If editPoint.LineCharOffset = 0 Then SearchID = String.Empty
                    End If
                Next
            Loop

            Return rBoolean
        End Function

        Public Sub GotoControlReferance(ByVal CommandaBarControl As Object, ByRef handled As Boolean, ByRef cancelDefault As Boolean)
            Dim appEvents As EnvDTE.DTE = CType(Me._applicationObject, EnvDTE.DTE)
            If appEvents.ActiveDocument Is Nothing Then Exit Sub

            Dim selection As EnvDTE.TextSelection =
                CType(appEvents.ActiveDocument.Selection, EnvDTE.TextSelection)

            Dim cursorPoint As EnvDTE.VirtualPoint =
                selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint =
                cursorPoint.CreateEditPoint()

            Dim cursorLastPossion As Integer =
                cursorPoint.LineCharOffset

            Dim ControlTypeReferance As Char, BeginOffset As Integer = -1, EndOffset As Integer = -1

            Dim ActiveDocFI As IO.FileInfo =
                New IO.FileInfo(appEvents.ActiveDocument.FullName)
            Dim IsXMLFile As Boolean =
                String.Compare(ActiveDocFI.Name, "ControlsMap.xml", True) = 0

            Me.GetReferanceStringFromBegining(editPoint, ControlTypeReferance, BeginOffset, IsXMLFile)

            If BeginOffset = -1 OrElse (ControlTypeReferance <> "C"c AndAlso ControlTypeReferance <> "F"c AndAlso (IsXMLFile AndAlso ControlTypeReferance <> Char.MinValue)) Then Exit Sub

            Dim SearchID As String = String.Empty
            Do Until editPoint.LineCharOffset - 1 = editPoint.LineLength
                SearchID = String.Concat(SearchID, editPoint.GetText(1))

                If SearchID.Chars(SearchID.Length - 1) = ":"c OrElse SearchID.Chars(SearchID.Length - 1) = "$"c OrElse (IsXMLFile AndAlso SearchID.Chars(SearchID.Length - 1) = "<"c) Then _
                    EndOffset = editPoint.LineCharOffset : Exit Do

                editPoint.CharRight()
            Loop

            If EndOffset = -1 Then Exit Sub

            editPoint.MoveToLineAndOffset(cursorPoint.Line, BeginOffset)
            SearchID = editPoint.GetText(EndOffset - BeginOffset)

            ' Fix the cursor position to the begining one
            editPoint.MoveToLineAndOffset(cursorPoint.Line, cursorLastPossion)

            Select Case ControlTypeReferance
                Case "C"c
                    Dim ActiveDocDI As IO.DirectoryInfo =
                        New IO.DirectoryInfo(appEvents.ActiveDocument.Path)

                    For Each proj As Project In appEvents.Solution.Projects
                        Dim CacheList As New Generic.List(Of String), IsAddon As Boolean = False
                        Dim MainProjItem As ProjectItem
                        Do
                            Try
                                MainProjItem = proj.ProjectItems.Item(ActiveDocDI.Name)
                            Catch ex As Exception
                                MainProjItem = Nothing
                            End Try

                            If MainProjItem Is Nothing Then
                                CacheList.Insert(0, ActiveDocDI.Name)
                                If Not IsAddon AndAlso String.Compare(ActiveDocDI.Name, "Addons", True) = 0 Then IsAddon = True
                                ActiveDocDI = ActiveDocDI.Parent
                            Else : Exit Do
                            End If
                        Loop Until ActiveDocDI Is Nothing

                        If Not MainProjItem Is Nothing Then
                            Dim ProjItem As ProjectItem = MainProjItem

                            For Each item As String In CacheList
                                ProjItem = ProjItem.ProjectItems.Item(item)
                            Next
                            ProjItem = ProjItem.ProjectItems.Item("ControlsMap.xml")

                            Dim IsResearched As Boolean = False
RESEARCHPOINT:
                            If Not ProjItem Is Nothing Then
                                Dim PrevState As Boolean = ProjItem.IsOpen
                                Dim itemWindow As Window =
                                    ProjItem.Open(Constants.vsViewKindTextView)
                                itemWindow.Activate()

                                CType(itemWindow.Document.Selection, TextSelection).StartOfDocument()
                                If Not CType(itemWindow.Document.Selection, TextSelection).FindText(String.Format("Control id=['""]{0}['""]", SearchID), vsFindOptions.vsFindOptionsRegularExpression) Then
                                    If Not PrevState Then itemWindow.Close()

                                    If IsAddon AndAlso Not IsResearched Then
                                        IsResearched = True

                                        ProjItem = MainProjItem
                                        For Each item As String In CacheList
                                            If String.Compare(item, "Addons", True) = 0 Then
                                                ProjItem = ProjItem.ProjectItems.Item("Templates")

                                                Exit For
                                            Else
                                                ProjItem = ProjItem.ProjectItems.Item(item)
                                            End If
                                        Next
                                        ProjItem = ProjItem.ProjectItems.Item("ControlsMap.xml")

                                        GoTo RESEARCHPOINT
                                    End If
                                End If
                            End If

                            Exit For
                        End If
                    Next
                Case "F"c, Char.MinValue
                    If Not String.IsNullOrWhiteSpace(SearchID) Then
                        Dim AssemblyName_s As String() = SearchID.Split("?"c)
                        Dim AssemblyName As String = AssemblyName_s(0)

                        If AssemblyName_s.Length = 2 Then
                            Dim ClassName_s As String() = AssemblyName_s(1).Split("."c)
                            Dim ClassName As String = ClassName_s(0)

                            If ClassName_s.Length = 2 Then
                                Dim FunctionName_s As String() = ClassName_s(1).Split(","c)
                                Dim FunctionName As String = FunctionName_s(0)
                                Dim ParametersLength As Integer = 0

                                If FunctionName_s.Length = 2 Then _
                                    ParametersLength = FunctionName_s(1).Split("|"c).Length

                                Dim MainProjectItem As ProjectItem
                                For Each proj As Project In appEvents.Solution.Projects
                                    MainProjectItem = Me.SearchProjectItemRecursive(proj.ProjectItems, New String() {String.Format("{0}.vb", AssemblyName), String.Format("{0}.cs", AssemblyName)})

                                    If Not MainProjectItem Is Nothing Then
                                        Dim docType As String =
                                            MainProjectItem.Name.Substring(MainProjectItem.Name.LastIndexOf("."c) + 1)

                                        Dim PrevState As Boolean = MainProjectItem.IsOpen
                                        Dim itemWindow As Window =
                                            MainProjectItem.Open(Constants.vsViewKindCode)

                                        Dim TS As TextSelection =
                                            CType(itemWindow.Document.Selection, TextSelection)

                                        TS.EndOfDocument()
                                        Dim DocEndOffset As Integer =
                                            TS.ActivePoint.AbsoluteCharOffset
                                        TS.StartOfDocument()

                                        Dim EP As EditPoint =
                                            TS.ActivePoint.CreateEditPoint()

                                        Dim CodeContent As String =
                                            EP.GetText(DocEndOffset)

                                        Dim LineNumber As Integer =
                                            Me.GetDefinitionLineNumber(docType, "Xeora.Domain", String.Format("{0}+{1}", AssemblyName, ClassName), FunctionName, ParametersLength, CodeContent)

                                        If LineNumber > -1 Then
                                            TS.MoveToLineAndOffset(LineNumber, 1)

                                            itemWindow.Activate()
                                        Else
                                            If Not PrevState Then itemWindow.Close()
                                        End If

                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
            End Select
        End Sub

        Public Sub PrepareProject()
            Dim Projects As Projects =
                CType(Me._applicationObject.Solution.Projects, Projects)

            If Projects.Count = 0 Then Exit Sub

            Dim ProjectList As New ProjectList()

            For Each Project As Project In Projects
                If String.Compare(Project.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") = 0 Then _
                    ProjectList.ProjectList.Add(Project.Name, Project.FullName)
            Next

            ProjectList.ShowDialog(Me)

            If ProjectList.DialogResult = DialogResult.OK Then
                Dim ProjectWorking As Project = Nothing
                For Each ProjectWorking In Projects
                    If String.Compare(ProjectWorking.FullName, ProjectList.SelectedProject.Value) = 0 Then _
                        Exit For
                Next

                Dim ProjectSettings As New ProjectSettings()
                If ProjectSettings.ShowDialog(Me) = DialogResult.OK Then
                    Dim ProjectAlreadyExists As Boolean = False
                    For Each ProjectItem As ProjectItem In ProjectWorking.ProjectItems
                        If String.Compare(ProjectItem.Name, "Domains") = 0 Then
                            For Each SubProjectItem As ProjectItem In ProjectItem.ProjectItems
                                If String.Compare(SubProjectItem.Name, ProjectSettings.DomainID, True) = 0 Then _
                                    ProjectAlreadyExists = True : Exit For
                            Next

                            Exit For
                        End If
                    Next

                    If ProjectAlreadyExists Then
                        If MessageBox.Show(Me,
                            String.Format("Xeora Domain ({0}) is already exists! Do you want override on it?", ProjectSettings.DomainID),
                            "Question?",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then

                            Exit Sub
                        End If
                    End If

                    If Not ProjectWorking Is Nothing Then
                        If String.IsNullOrEmpty(CType(ProjectWorking.Properties.Item("StartPage").Value, String)) Then _
                            ProjectWorking.Properties.Item("StartPage").Value = "/"
                        If String.Compare(CType(ProjectWorking.Properties.Item("StartAction").Value, String), "0") = 0 Then _
                            ProjectWorking.Properties.Item("StartAction").Value = "1"

                        Dim WorkingFolder As String =
                            ProjectList.SelectedProject.Value
                        Dim SW As IO.StreamWriter

                        Dim BinProjectItem As ProjectItem =
                            Me.CheckProjectItemExists(ProjectWorking.ProjectItems, "bin")
                        Dim ReleasePulled As Boolean = (Not BinProjectItem Is Nothing) AndAlso (BinProjectItem.ProjectItems.Count >= 7)
                        If BinProjectItem Is Nothing Then _
                            BinProjectItem = ProjectWorking.ProjectItems.AddFolder("bin")
                        Dim ProjectItem As ProjectItem =
                            Me.CheckProjectItemExists(ProjectWorking.ProjectItems, "Domains")
                        Dim NoOtherDomainExists As Boolean = ProjectItem Is Nothing
                        If ProjectItem Is Nothing Then _
                            ProjectItem = ProjectWorking.ProjectItems.AddFolder("Domains")

                        Dim DomainProjectItem As ProjectItem =
                            Me.CheckProjectItemExists(ProjectWorking.ProjectItems, String.Format("Domains\{0}", ProjectSettings.DomainID))
                        If Not DomainProjectItem Is Nothing Then
                            Try
                                DomainProjectItem.Remove()
                            Catch ex As Exception
                                ' It is for debug exception skip purposes!
                            End Try

                            Dim DomainLocation As String =
                                IO.Path.Combine(WorkingFolder, "Domains", ProjectSettings.DomainID)

                            Try
                                If IO.Directory.Exists(DomainLocation) Then _
                                    IO.Directory.Delete(DomainLocation)
                            Catch ex As Exception
                                ' Just Handle Exceptions
                            End Try
                        End If
                        ProjectItem = ProjectItem.ProjectItems.AddFolder(ProjectSettings.DomainID)

                        ProjectItem.ProjectItems.AddFolder("Executables")

                        Dim SubProjectItem As ProjectItem =
                        ProjectItem.ProjectItems.AddFolder("Contents").ProjectItems.AddFolder(ProjectSettings.LanguageID)
                        Dim StyleFileLocation As String =
                        IO.Path.Combine(WorkingFolder, "Domains", ProjectSettings.DomainID, "Contents", ProjectSettings.LanguageID, "styles.css")
                        SW = IO.File.CreateText(StyleFileLocation)
                        SW.WriteLine("/* Default CSS Stylesheet for a New Xeora Web Application project */")
                        SW.Close()
                        SubProjectItem.ProjectItems.AddFromFile(StyleFileLocation)
                        ' favicon.ico should be located here

                        SubProjectItem = ProjectItem.ProjectItems.AddFolder("Languages")
                        Dim TranslationFileLocation As String =
                        IO.Path.Combine(WorkingFolder, "Domains", ProjectSettings.DomainID, "Languages", String.Format("{0}.xml", ProjectSettings.LanguageID))
                        SW = IO.File.CreateText(TranslationFileLocation)
                        SW.WriteLine("<?xml version=""1.0"" encoding=""utf-8""?>")
                        SW.WriteLine(String.Format("<language name=""{0}"" code=""{1}"">", ProjectSettings.LanguageName, ProjectSettings.LanguageID))
                        SW.WriteLine("  <translation id=""TEMPLATE_IDMUSTBESET"">TemplateID must be set</translation>")
                        SW.WriteLine("  <translation id=""CONTROLSMAPNOTFOUND"">ControlsMapXML file does not exists</translation>")
                        SW.WriteLine("  <translation id=""CONFIGURATIONNOTFOUND"">ConfigurationXML file does not exists</translation>")
                        SW.WriteLine("  <translation id=""TEMPLATE_NOFOUND"">{0} name Template file does not exists</translation>")
                        SW.WriteLine("  <translation id=""TEMPLATE_AUTH"">This Template requires authentication</translation>")
                        SW.WriteLine("  <translation id=""SITETITLE"">Hello, I'm Xeora!</translation>")
                        SW.WriteLine("")
                        SW.WriteLine("</language>")
                        SW.Close()
                        SubProjectItem.ProjectItems.AddFromFile(TranslationFileLocation)

                        SubProjectItem = ProjectItem.ProjectItems.AddFolder("Templates")
                        Dim ControlsMapFileLocation As String =
                        IO.Path.Combine(WorkingFolder, "Domains", ProjectSettings.DomainID, "Templates", "ControlsMap.xml")
                        SW = IO.File.CreateText(ControlsMapFileLocation)
                        SW.WriteLine("<?xml version=""1.0"" encoding=""utf-8""?>")
                        SW.WriteLine("<ControlsMap />")
                        SW.Close()
                        SubProjectItem.ProjectItems.AddFromFile(ControlsMapFileLocation)

                        Dim ConfigurationFileLocation As String =
                        IO.Path.Combine(WorkingFolder, "Domains", ProjectSettings.DomainID, "Templates", "Configuration.xml")
                        SW = IO.File.CreateText(ConfigurationFileLocation)
                        SW.WriteLine("<?xml version=""1.0"" encoding=""utf-8""?>")
                        SW.WriteLine("<Settings>")
                        SW.WriteLine("  <Configuration>")
                        SW.WriteLine("      <Item key=""authenticationpage"" value=""main"" />")
                        SW.WriteLine("      <Item key=""defaultpage"" value=""main"" />")
                        SW.WriteLine(String.Format("      <Item key=""defaultlanguage"" value=""{0}"" />", ProjectSettings.LanguageID))
                        SW.WriteLine(String.Format("      <Item key=""defaultcaching"" value=""{0}"" />", ProjectSettings.CachingType))
                        SW.WriteLine("  </Configuration>")
                        SW.WriteLine("  <Services>")
                        SW.WriteLine("      <AuthenticationKeys />")
                        SW.WriteLine("      <Item type=""template"" id=""main"" />")
                        SW.WriteLine("  </Services>")
                        SW.WriteLine("</Settings>")
                        SW.Close()
                        SubProjectItem.ProjectItems.AddFromFile(ConfigurationFileLocation)

                        Dim DefaultTemplateFileLocation As String =
                        IO.Path.Combine(WorkingFolder, "Domains", ProjectSettings.DomainID, "Templates", "main.htm")
                        SW = IO.File.CreateText(DefaultTemplateFileLocation)
                        SW.WriteLine("$S:HelloXeora:{!NOCACHE")
                        SW.WriteLine("  return ""Hello, Xeora Framework is ready!"";")
                        SW.WriteLine("}:HelloXeora$")
                        SW.Close()
                        SubProjectItem.ProjectItems.AddFromFile(DefaultTemplateFileLocation)

                        ProjectItem = Me.CheckProjectItemExists(ProjectWorking.ProjectItems, "web.config")
                        Dim WebConfigFileLocation As String =
                            IO.Path.Combine(WorkingFolder, "web.config")

                        If ProjectItem Is Nothing OrElse NoOtherDomainExists Then
                            SW = IO.File.CreateText(WebConfigFileLocation)
                            SW.WriteLine("<?xml version=""1.0""?>")
                            SW.WriteLine("<configuration>")
                            SW.WriteLine("  <appSettings>")
                            SW.WriteLine("      <!--  You have to put ""\"" at the end of the value attribute of ApplicationRoot")
                            SW.WriteLine("            If there is not ""\"" exists...")
                            SW.WriteLine("")
                            SW.WriteLine("            LoggingRoot Directory HAVE TO HAVE the PERMISSION to write And read file for ASP.NET account...")
                            SW.WriteLine("      -->")
                            SW.WriteLine(String.Format("      <add key=""DefaultDomain"" value=""{0}"" />", ProjectSettings.DomainID))
                            SW.WriteLine(String.Format("      <add key=""VirtualRoot"" value=""{0}"" /> <!-- You may need to set to the proper virtual directory path according to your IIS settings -->", ProjectSettings.VirtualPath))
                            SW.WriteLine("      <add key=""PyhsicalRoot"" value=""" & WorkingFolder & """ />")
                            SW.WriteLine("      <add key=""ApplicationRoot"" value="".\"" />")
                            SW.WriteLine(String.Format("      <add key=""Debugging"" value=""{0}"" />", ProjectSettings.DebuggingActive.ToString()))
                            SW.WriteLine(String.Format("      <add key=""VariablePoolServicePort"" value=""{0}"" /> <!-- VariablePoolServicePort should be unique for each Xeora Application in the server -->", ProjectSettings.VariablePoolServicePort))
                            SW.WriteLine(String.Format("      <add key=""ScheduledTasksServicePort"" value=""{0}"" />", ProjectSettings.ScheduledTasksServicePort))
                            SW.WriteLine("      <add key=""Bandwidth"" value=""0"" />")
                            SW.WriteLine("      <add key=""RequestTagFiltering"" value=""None"" />")
                            SW.WriteLine("      <add key=""RequestTagFilteringItems"" value=""&gt;script"" />")
                            SW.WriteLine("      <add key=""RequestTagFilteringExceptions"" value="""" />")
                            SW.WriteLine("      <add key=""LoggingPath"" value=""" & IO.Path.Combine(WorkingFolder, "XeoraLogs") & """ />")
                            SW.WriteLine("  </appSettings>")
                            SW.WriteLine("  <connectionStrings />")
                            SW.WriteLine("  <system.web>")
                            SW.WriteLine("      <httpRuntime executionTimeout = ""86400"" maxRequestLength=""2048000"" requestValidationMode=""2.0"" />")
                            SW.WriteLine("      <!-- ")
                            SW.WriteLine("          Set compilation debug=""True"" To insert debugging ")
                            SW.WriteLine("          symbols into the compiled page. Because this ")
                            SW.WriteLine("          affects performance, set this value to true only ")
                            SW.WriteLine("          during development.")
                            SW.WriteLine("      -->")
                            SW.WriteLine("      <compilation debug=""true"" />")
                            SW.WriteLine("      <!--")
                            SW.WriteLine("          The <authentication> section enables configuration ")
                            SW.WriteLine("          of the security authentication mode used by ")
                            SW.WriteLine("          ASP.NET to identify an incoming user. ")
                            SW.WriteLine("      -->")
                            SW.WriteLine("      <authentication mode=""Windows"" />")
                            SW.WriteLine("      <sessionState mode=""Off"" stateNetworkTimeout=""10"" timeout=""20"" compressionEnabled=""false"" />")
                            SW.WriteLine("      <!--")
                            SW.WriteLine("          The <customErrors> section enables configuration ")
                            SW.WriteLine("          of what to do if/when an unhandled error occurs ")
                            SW.WriteLine("          during the execution of a request. Specifically, ")
                            SW.WriteLine("          it enables developers To configure html Error pages ")
                            SW.WriteLine("          To be displayed In place Of a Error stack trace.")
                            SW.WriteLine("")
                            SW.WriteLine("          <customErrors mode=""RemoteOnly"" defaultRedirect=""GenericErrorPage.htm"">")
                            SW.WriteLine("              <Error statusCode=""403"" redirect=""NoAccess.htm"" />")
                            SW.WriteLine("              <Error statusCode=""404"" redirect=""FileNotFound.htm"" />")
                            SW.WriteLine("          </customErrors>")
                            SW.WriteLine("      -->")
                            SW.WriteLine("  </system.web>")
                            SW.WriteLine("  <system.webServer>")
                            SW.WriteLine("      <handlers>")
                            SW.WriteLine("          <add name=""XeoraCubeHandler"" path=""*"" verb=""*"" type=""Xeora.Web.Handler.RequestHandlerFactory"" resourceType=""Unspecified"" requireAccess=""Script"" preCondition=""integratedMode"" />")
                            SW.WriteLine("      </handlers>")
                            SW.WriteLine("      <modules>")
                            SW.WriteLine("          <add name=""XeoraCubeModule"" type=""Xeora.Web.Handler.RequestModule"" />")
                            SW.WriteLine("      </modules>")
                            SW.WriteLine("  </system.webServer>")
                            SW.WriteLine("</configuration>")
                            SW.Close()

                            ProjectWorking.ProjectItems.AddFromFile(WebConfigFileLocation)
                            'Else
                            '    Dim ConfigurationFI As New IO.FileInfo(IO.Path.Combine(WorkingFolder, "web.config"))
                            '    Dim VDMForConfig As New Web.Configuration.VirtualDirectoryMapping(ConfigurationFI.DirectoryName, True, ConfigurationFI.Name)
                            '    Dim WCFM As New Web.Configuration.WebConfigurationFileMap()
                            '    WCFM.VirtualDirectories.Add("/", VDMForConfig)

                            '    Dim Configuration As System.Configuration.Configuration =
                            '        Web.Configuration.WebConfigurationManager.OpenMappedWebConfiguration(WCFM, "/")

                            '    Configuration.AppSettings.Settings.Item("DefaultDomain").Value = ProjectSettings.DomainID
                            '    Configuration.AppSettings.Settings.Item("VirtualRoot").Value = ProjectSettings.VirtualPath
                            '    Configuration.AppSettings.Settings.Item("Debugging").Value = ProjectSettings.DebuggingActive.ToString()
                            '    Configuration.AppSettings.Settings.Item("VariablePoolServicePort").Value = ProjectSettings.VariablePoolServicePort
                            '    Configuration.AppSettings.Settings.Item("ScheduledTasksServicePort").Value = ProjectSettings.ScheduledTasksServicePort

                            '    Dim SystemWebServerHandlerSection As Web.Configuration.HttpHandlersSection =
                            '        CType(Configuration.GetSection("system.webServer/handlers"), Web.Configuration.HttpHandlersSection)

                            '    Dim HandlerExists As Boolean = False
                            '    For Each handler As Web.Configuration.HttpHandlerAction In SystemWebServerHandlerSection.Handlers
                            '        If String.Compare(handler.Type, "Xeora.Web.Handler.RequestHandlerFactory") = 0 Then _
                            '            HandlerExists = True : Exit For
                            '    Next
                            '    If Not HandlerExists Then
                            '        SystemWebServerHandlerSection.Handlers.Add(
                            '            New Web.Configuration.HttpHandlerAction("*", "Xeora.Web.Handler.RequestHandlerFactory", "*"))
                            '    End If

                            '    Dim SystemWebServerModuleSection As Web.Configuration.HttpModulesSection =
                            '        CType(Configuration.GetSection("system.webServer/modules"), Web.Configuration.HttpModulesSection)

                            '    Dim ModuleExists As Boolean = False
                            '    For Each [module] As Web.Configuration.HttpModuleAction In SystemWebServerModuleSection.Modules
                            '        If String.Compare([module].Type, "Xeora.Web.Handler.RequestModule") = 0 Then _
                            '            ModuleExists = True Exit For
                            '    Next
                            '    If Not ModuleExists Then
                            '        SystemWebServerModuleSection.Modules.Add(
                            '            New Web.Configuration.HttpModuleAction("XeoraCubeModule", "Xeora.Web.Handler.RequestModule"))
                            '    End If

                            '    Configuration.Save()
                        End If

                        If Not ReleasePulled Then
                            Dim DownloadProgress As New DownloadProgress()
                            DownloadProgress.StartDownloading(IO.Path.Combine(WorkingFolder, "bin"), ProjectSettings.Use64bitRelease, Me)

                            If DownloadProgress.DialogResult = DialogResult.OK Then
                                For Each File As String In DownloadProgress.DownloadedFiles
                                    Dim ItemExists As ProjectItem =
                                        Me.CheckProjectItemExists(BinProjectItem.ProjectItems, File)
                                    If Not ItemExists Is Nothing Then
                                        Try
                                            ItemExists.Remove()
                                        Catch ex As Exception
                                            ' This exception handling is for debug limitation
                                        End Try
                                    End If

                                    BinProjectItem.ProjectItems.AddFromFile(IO.Path.Combine(WorkingFolder, "bin", File))
                                Next
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub RePullRelease()
            Dim Projects As Projects =
                CType(Me._applicationObject.Solution.Projects, Projects)

            If Projects.Count = 0 Then Exit Sub

            Dim ProjectList As New ProjectList()

            For Each Project As Project In Projects
                If String.Compare(Project.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") = 0 Then _
                    ProjectList.ProjectList.Add(Project.Name, Project.FullName)
            Next

            ProjectList.ShowDialog(Me)

            If ProjectList.DialogResult = DialogResult.OK Then
                Dim WorkingFolder As String =
                    ProjectList.SelectedProject.Value

                Dim ProjectWorking As Project = Nothing
                For Each ProjectWorking In Projects
                    If String.Compare(ProjectWorking.FullName, ProjectList.SelectedProject.Value) = 0 Then _
                        Exit For
                Next

                Dim BinProjectItem As ProjectItem = Nothing, DomainsProjectItem As ProjectItem = Nothing
                For Each ProjectItem As ProjectItem In ProjectWorking.ProjectItems
                    If String.Compare(ProjectItem.Name, "Bin", True) = 0 Then _
                        BinProjectItem = ProjectItem

                    If String.Compare(ProjectItem.Name, "Domains") = 0 Then _
                        DomainsProjectItem = ProjectItem
                Next

                If Not BinProjectItem Is Nothing AndAlso Not DomainsProjectItem Is Nothing Then
                    ' TODO: System should find which version it has and which CPU version installed
                    Dim DownloadProgress As New DownloadProgress()
                    DownloadProgress.StartDownloading(IO.Path.Combine(WorkingFolder, "bin"), True, Me)

                    If DownloadProgress.DialogResult = DialogResult.OK Then
                        For Each File As String In DownloadProgress.DownloadedFiles
                            Dim ItemExists As ProjectItem =
                                Me.CheckProjectItemExists(BinProjectItem.ProjectItems, File)
                            If Not ItemExists Is Nothing Then
                                Try
                                    ItemExists.Remove()
                                Catch ex As Exception
                                    ' This exception handling is for debug limitation
                                End Try
                            End If

                            BinProjectItem.ProjectItems.AddFromFile(IO.Path.Combine(WorkingFolder, "bin", File))
                        Next
                    End If
                End If
            End If
        End Sub

        Private Function CheckProjectItemExists(ByVal SearchingProjectItems As ProjectItems, ByVal SearchPath As String) As ProjectItem
            Dim rProjectItem As ProjectItem = Nothing
            Dim SearchPaths As String() = SearchPath.Split("\"c)

            Dim WorkingProjectItems As ProjectItems = SearchingProjectItems
            For Each SP As String In SearchPaths
                For Each WPI As ProjectItem In WorkingProjectItems
                    If String.Compare(WPI.Name, SP, True) = 0 Then
                        rProjectItem = WPI
                        WorkingProjectItems = WPI.ProjectItems

                        Exit For
                    End If
                Next
            Next

            Return rProjectItem
        End Function

        Private Function GetDefinitionLineNumber(ByVal LanguageID As String, ByVal [Namespace] As String, ByVal JoinedTypeName As String, ByVal MethodName As String, ByVal ParameterLength As Integer, ByVal DocumentContent As String) As Integer
            Dim rInteger As Integer = -1

            Dim parser As ICSharpCode.NRefactory.IParser = Nothing

            Dim codeContentReader As New IO.StringReader(DocumentContent)
            Select Case LanguageID
                Case "vb"
                    parser = ICSharpCode.NRefactory.ParserFactory.CreateParser(
                                ICSharpCode.NRefactory.SupportedLanguage.VBNet,
                                codeContentReader)

                Case "cs"
                    parser = ICSharpCode.NRefactory.ParserFactory.CreateParser(
                                ICSharpCode.NRefactory.SupportedLanguage.VBNet,
                                codeContentReader)
            End Select

            If Not parser Is Nothing Then
                parser.ParseMethodBodies = False
                parser.Parse()
            End If

            codeContentReader.Close()

            If Not parser Is Nothing AndAlso Not parser.CompilationUnit Is Nothing Then
                Dim NodeList As Generic.List(Of ICSharpCode.NRefactory.Ast.INode) =
                    parser.CompilationUnit.Children

                Dim NSFound As Boolean = False, TypeFound As Boolean = False, FuncFound As Boolean = False, TypeNames As String() = JoinedTypeName.Split("+"c), WorkingTypeID As Integer = 0
                Do While Not NodeList Is Nothing AndAlso NodeList.Count > 0
                    If NSFound AndAlso TypeFound AndAlso Not FuncFound Then
                        If TypeOf NodeList.Item(0) Is ICSharpCode.NRefactory.Ast.MethodDeclaration AndAlso
                            String.Compare(CType(NodeList.Item(0), ICSharpCode.NRefactory.Ast.MethodDeclaration).Name, MethodName) = 0 AndAlso
                            CType(NodeList.Item(0), ICSharpCode.NRefactory.Ast.MethodDeclaration).Parameters.Count = ParameterLength Then

                            FuncFound = True : rInteger = NodeList.Item(0).StartLocation.Line

                            Exit Do
                        End If
                    End If

                    If NSFound AndAlso Not TypeFound Then
                        If TypeOf NodeList.Item(0) Is ICSharpCode.NRefactory.Ast.TypeDeclaration AndAlso
                            String.Compare(CType(NodeList.Item(0), ICSharpCode.NRefactory.Ast.TypeDeclaration).Name, TypeNames(WorkingTypeID)) = 0 Then

                            WorkingTypeID += 1

                            If WorkingTypeID = TypeNames.Length Then TypeFound = True

                            NodeList = NodeList.Item(0).Children : Continue Do
                        End If
                    End If

                    If TypeOf NodeList.Item(0) Is ICSharpCode.NRefactory.Ast.NamespaceDeclaration AndAlso
                        String.Compare(CType(NodeList.Item(0), ICSharpCode.NRefactory.Ast.NamespaceDeclaration).Name, [Namespace]) = 0 Then

                        NSFound = True

                        NodeList = NodeList.Item(0).Children : Continue Do
                    End If

                    NodeList.RemoveAt(0)
                Loop
            End If

            Return rInteger
        End Function

        Private Function SearchProjectItemRecursive(ByVal ProjectItems As ProjectItems, ByVal SearchingNames As String()) As ProjectItem
            Dim rProjectItem As ProjectItem = Nothing

            For Each projItem As ProjectItem In ProjectItems
                If Array.IndexOf(SearchingNames, projItem.Name) > -1 Then
                    rProjectItem = projItem

                    Exit For
                Else
                    If Not projItem.ProjectItems Is Nothing AndAlso projItem.ProjectItems.Count > 0 Then _
                        rProjectItem = Me.SearchProjectItemRecursive(projItem.ProjectItems, SearchingNames)

                    If Not rProjectItem Is Nothing Then Exit For
                End If
            Next

            Return rProjectItem
        End Function

        Public Sub event_TDAfterKeyPressed(ByVal KeyPress As String, ByVal Selection As EnvDTE.TextSelection, ByVal InStatementCompletion As Boolean)
            Dim owner As IWin32Window = Me

            Dim FileName As String = Selection.Parent.Parent.ProjectItem.Name
            Dim TrackingChars As Char() = New Char() {"$"c, ":"c, "?"c, "."c, ">"c, "["c, "C"c}

            If InStatementCompletion OrElse String.IsNullOrEmpty(FileName) Then Exit Sub
            If Array.IndexOf(TrackingChars, KeyPress.Chars(0)) = -1 Then Exit Sub
            If Not Me.CheckIsXeoraCubeProject(Selection) Then Exit Sub

            ' Reach here if you still not Exited from Sub
            If String.Compare(IO.Path.GetExtension(FileName), ".htm", True) = 0 Then
                If String.Compare(KeyPress, "$") = 0 Then
                    Dim editPoint As EnvDTE.EditPoint =
                        Selection.ActivePoint.CreateEditPoint()

                    If Me.CheckIsLastingStatement(editPoint) Then Exit Sub

                    Dim spForm As New Xeora.VSAddIn.Forms.SpecialSearch(Selection, Selection.ActivePoint.LineCharOffset)
                    spForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                    spForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                    spForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                    spForm.VisibleInLocation = Me.CalculateLocation(Selection)
                    spForm.FillList()
                    spForm.ShowDialog(owner)

                ElseIf String.Compare(KeyPress, "[") = 0 Then
                    Dim editPoint As EnvDTE.EditPoint =
                        Selection.ActivePoint.CreateEditPoint()

                    Dim ControlTypeReferance As Char = "@"c, Offset As Integer = -1

                    Me.GetReferanceStringFromBegining(editPoint, ControlTypeReferance, Offset, False)

                    If Offset = -1 Then Exit Sub

                    Select Case ControlTypeReferance
                        Case "C"c
                            Dim cspForm As New Xeora.VSAddIn.Forms.ControlSearchForParenting(Selection, Offset)
                            cspForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            cspForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            cspForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            cspForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            cspForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            cspForm.FillList()
                            cspForm.ShowDialog(owner)

                    End Select
                ElseIf String.Compare(KeyPress, "C") = 0 Then
                    Dim editPoint As EnvDTE.EditPoint =
                        Selection.ActivePoint.CreateEditPoint()

                    Dim ControlTypeReferance As Char = "@"c, Offset As Integer = -1

                    Me.GetReferanceStringFromBegining(editPoint, ControlTypeReferance, Offset, False)

                    If Offset = -1 Then Exit Sub

                    Select Case ControlTypeReferance
                        Case "C"c
                            Dim csForm As New Xeora.VSAddIn.Forms.ControlSearch(Selection, Offset)
                            csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            csForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            csForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            csForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            csForm.FillList()
                            csForm.ShowDialog(owner)

                    End Select
                ElseIf String.Compare(KeyPress, ":") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint =
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint =
                        cursorPoint.CreateEditPoint()

                    Dim ControlTypeReferance As Char = "@"c, Offset As Integer = -1

                    Dim ControlReferanceText As String =
                        Me.GetReferanceStringFromBegining(editPoint, ControlTypeReferance, Offset, False)

                    If Offset = -1 Then Exit Sub

                    Select Case ControlTypeReferance
                        Case "C"c
                            Dim PeriodOccured As Boolean = False

                            For cC As Integer = 0 To ControlReferanceText.Length - 1
                                If ControlReferanceText.Chars(cC) = ":"c Then
                                    If PeriodOccured Then Exit Sub

                                    PeriodOccured = True
                                End If
                            Next

                            Dim csForm As New Xeora.VSAddIn.Forms.ControlSearch(Selection, Offset)
                            csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            csForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            csForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            csForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            csForm.FillList()
                            csForm.ShowDialog(owner)

                        Case "T"c, "P"c
                            Dim tsForm As New Xeora.VSAddIn.Forms.TemplateSearch(Selection, Offset)
                            tsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            tsForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            tsForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            tsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            tsForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            tsForm.FillList()
                            tsForm.ShowDialog(owner)

                        Case "L"c
                            Dim tsForm As New Xeora.VSAddIn.Forms.TranslationSearch(Selection, Offset)
                            tsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            tsForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            tsForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            tsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            tsForm.TranslationsPath = Me.GetLanguagePath(Selection)
                            tsForm.FillList()
                            tsForm.ShowDialog(owner)

                        Case "F"c
                            Dim asForm As New Xeora.VSAddIn.Forms.AssemblySearch(Selection, Offset)
                            asForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            asForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            asForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            asForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            asForm.SearchType = Me.GetActiveItemType(Selection)
                            asForm.SearchPath = Me.GetExecutablesPath(Selection)
                            asForm.FillList()
                            asForm.ShowDialog(owner)

                        Case "H"c
                            Dim SearchID As String = String.Empty, EndOffset As Integer = -1
                            Do Until editPoint.LineCharOffset - 1 = editPoint.LineLength
                                SearchID = String.Concat(SearchID, editPoint.GetText(1))

                                If SearchID.Chars(SearchID.Length - 1) = ":"c Then _
                                    EndOffset = editPoint.LineCharOffset : Exit Do

                                editPoint.CharRight()
                            Loop

                            If EndOffset = -1 Then Exit Sub

                            editPoint.MoveToLineAndOffset(cursorPoint.Line, Offset)
                            SearchID = editPoint.GetText(EndOffset - Offset)

                            Me.HandleResult(owner, True, cursorPoint.LineCharOffset, Selection, Globals.ISTypes.OnFlyRequest, "$"c, "$"c, New Object() {SearchID})

                        Case "S"c
                            Dim SearchID As String = String.Empty, EndOffset As Integer = -1
                            Do Until editPoint.LineCharOffset - 1 = editPoint.LineLength
                                SearchID = String.Concat(SearchID, editPoint.GetText(1))

                                If SearchID.Chars(SearchID.Length - 1) = ":"c Then _
                                    EndOffset = editPoint.LineCharOffset : Exit Do

                                editPoint.CharRight()
                            Loop

                            If EndOffset = -1 Then Exit Sub

                            editPoint.MoveToLineAndOffset(cursorPoint.Line, Offset)
                            SearchID = editPoint.GetText(EndOffset - Offset)

                            Me.HandleResult(owner, True, cursorPoint.LineCharOffset, Selection, Globals.ISTypes.PrimitiveStatement, "$"c, "$"c, New Object() {SearchID})

                    End Select
                ElseIf String.Compare(KeyPress, "?") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint =
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint =
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String =
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match =
                        System.Text.RegularExpressions.Regex.Match(LineText, "\$F(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?:[\.\w\-]+\?", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim csForm As New Xeora.VSAddIn.Forms.ClassSearch(Selection, cursorPoint.LineCharOffset)
                        csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        csForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        csForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        csForm.SearchType = Me.GetActiveItemType(Selection)
                        csForm.SearchPath = Me.GetExecutablesPath(Selection)
                        csForm.AssemblyID = mI.Value.Substring(mI.Value.IndexOf(":"c) + 1, mI.Value.Length - (mI.Value.IndexOf(":"c) + 2))
                        csForm.FillList()
                        csForm.ShowDialog(owner)

                    End If
                ElseIf String.Compare(KeyPress, ".") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint =
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint =
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String =
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match =
                        System.Text.RegularExpressions.Regex.Match(LineText, "\$F(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?:[\.\w\-]+\?[\.\w\-]+", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim fsForm As New Xeora.VSAddIn.Forms.FunctionSearch(Selection, cursorPoint.LineCharOffset)
                        fsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        fsForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        fsForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        fsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        fsForm.SearchType = Me.GetActiveItemType(Selection)
                        fsForm.SearchPath = Me.GetExecutablesPath(Selection)
                        fsForm.AssemblyID = mI.Value.Substring(mI.Value.IndexOf(":"c) + 1, mI.Value.IndexOf("?"c) - (mI.Value.IndexOf(":"c) + 1))
                        fsForm.ClassID = mI.Value.Substring(mI.Value.IndexOf("?"c) + 1, mI.Value.IndexOf("."c) - (mI.Value.IndexOf("?"c) + 1))
                        fsForm.FillList()
                        fsForm.ShowDialog(owner)

                    End If
                End If
            ElseIf String.Compare(FileName, "ControlsMap.xml", True) = 0 Then
                If String.Compare(KeyPress, ">") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint =
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint =
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String =
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match =
                        System.Text.RegularExpressions.Regex.Match(LineText, "\<Bind\>", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim asForm As New Xeora.VSAddIn.Forms.AssemblySearch(Selection, cursorPoint.LineCharOffset)
                        asForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        asForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        asForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        asForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        asForm.SearchType = Me.GetActiveItemType(Selection)
                        asForm.SearchPath = Me.GetExecutablesPath(Selection)
                        asForm.UseCloseChar = Nothing
                        asForm.FillList()
                        asForm.ShowDialog(owner)

                        GoTo SKIPOTHERS
                    End If

                    mI = System.Text.RegularExpressions.Regex.Match(LineText, "\<DefaultButtonID\>", Text.RegularExpressions.RegexOptions.RightToLeft)
                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim csForm As New Xeora.VSAddIn.Forms.ControlSearch(Selection, cursorPoint.LineCharOffset)
                        csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        csForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        csForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        csForm.TemplatesPath = Me.GetTemplatePath(Selection)
                        csForm.FilterByTypes.Add(Globals.ControlTypes.Button)
                        csForm.FilterByTypes.Add(Globals.ControlTypes.Link)
                        csForm.FilterByTypes.Add(Globals.ControlTypes.Image)
                        csForm.UseCloseChar = Nothing
                        csForm.FillList()
                        csForm.ShowDialog(owner)

                        GoTo SKIPOTHERS
                    End If

                    mI = System.Text.RegularExpressions.Regex.Match(LineText, "\<Type\>", Text.RegularExpressions.RegexOptions.RightToLeft)
                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim tsForm As New Xeora.VSAddIn.Forms.TypeSearch(Selection, cursorPoint.LineCharOffset)
                        tsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        tsForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        tsForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        tsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        tsForm.UseCloseChar = Nothing
                        tsForm.FillList()
                        tsForm.ShowDialog(owner)

                        GoTo SKIPOTHERS
                    End If
SKIPOTHERS:
                    'If editPoint.LineCharOffset - 3 > 0 Then

                    '    Select Case readText
                    '        Case "$C"
                    '            Dim csForm As New SolidDevelopment.VSAddIn.Forms.ControlSearch
                    '            csForm.BackSpaceForEmptyTextHandler = New SolidDevelopment.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                    '            csForm.EnteredTextHandler = New SolidDevelopment.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                    '            csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                    '            csForm.TemplatesPath = TemplatesPath
                    '            csForm.FillList()

                    '            Dim tt As New ThreadTracker(csForm, New ThreadTracker.HandleResultDelegate(AddressOf Me.HandleResult))
                    '            tt.Track()

                    '            csForm.Show()

                    '        Case "$T"
                    '            Dim tsForm As New SolidDevelopment.VSAddIn.Forms.TemplateSearch
                    '            tsForm.BackSpaceForEmptyTextHandler = New SolidDevelopment.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                    '            tsForm.EnteredTextHandler = New SolidDevelopment.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                    '            tsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                    '            tsForm.TemplatesPath = TemplatesPath
                    '            tsForm.FillList()

                    '            Dim tt As New ThreadTracker(tsForm, New ThreadTracker.HandleResultDelegate(AddressOf Me.HandleResult))
                    '            tt.Track()

                    '            tsForm.Show()

                    '        Case "$F"


                    '    End Select
                    'End If
                ElseIf String.Compare(KeyPress, "?") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint =
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint =
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String =
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match =
                        System.Text.RegularExpressions.Regex.Match(LineText, "\<Bind\>[\.\w\-]+\?", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim csForm As New Xeora.VSAddIn.Forms.ClassSearch(Selection, cursorPoint.LineCharOffset)
                        csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        csForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        csForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        csForm.SearchType = Me.GetActiveItemType(Selection)
                        csForm.SearchPath = Me.GetExecutablesPath(Selection)
                        csForm.AssemblyID = mI.Value.Substring(mI.Value.IndexOf(":"c) + 1, mI.Value.Length - (mI.Value.IndexOf(":"c) + 1))
                        csForm.FillList()
                        csForm.ShowDialog(owner)

                    End If
                ElseIf String.Compare(KeyPress, ".") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint =
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint =
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String =
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match =
                        System.Text.RegularExpressions.Regex.Match(LineText, "\<Bind\>[\.\w\-]+\?[\.\w\-]+", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim fsForm As New Xeora.VSAddIn.Forms.FunctionSearch(Selection, cursorPoint.LineCharOffset)
                        fsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        fsForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        fsForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        fsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        fsForm.SearchType = Me.GetActiveItemType(Selection)
                        fsForm.SearchPath = Me.GetExecutablesPath(Selection)
                        fsForm.AssemblyID = mI.Value.Substring(mI.Value.IndexOf(":"c) + 1, mI.Value.IndexOf("?"c) - (mI.Value.IndexOf(":"c) + 1))
                        fsForm.ClassID = mI.Value.Substring(mI.Value.IndexOf("?"c) + 1, mI.Value.IndexOf("."c) - (mI.Value.IndexOf("?"c) + 1))
                        fsForm.UseCloseChar = Nothing
                        fsForm.FillList()
                        fsForm.ShowDialog(owner)

                    End If
                End If
            End If
        End Sub

        Private Sub ShowAssemblySearchFor(ByRef owner As IWin32Window, ByVal CloseChar As Char, ByVal Selection As EnvDTE.TextSelection)
            Dim cursorPoint As EnvDTE.VirtualPoint =
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint =
                cursorPoint.CreateEditPoint()

            Dim asForm As New Xeora.VSAddIn.Forms.AssemblySearch(Selection, cursorPoint.LineCharOffset)
            asForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
            asForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
            asForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
            asForm.VisibleInLocation = Me.CalculateLocation(Selection)
            asForm.SearchType = Me.GetActiveItemType(Selection)
            asForm.SearchPath = Me.GetExecutablesPath(Selection)
            asForm.UseCloseChar = CloseChar
            asForm.FillList()
            asForm.ShowDialog(owner)
        End Sub

        Private Sub ShowClassSearchFor(ByRef owner As IWin32Window, ByVal AssemblyID As String, ByVal CloseChar As Char, ByVal Selection As EnvDTE.TextSelection)
            Dim cursorPoint As EnvDTE.VirtualPoint =
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint =
                cursorPoint.CreateEditPoint()

            Dim csForm As New Xeora.VSAddIn.Forms.ClassSearch(Selection, cursorPoint.LineCharOffset)
            csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
            csForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
            csForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
            csForm.VisibleInLocation = Me.CalculateLocation(Selection)
            csForm.SearchType = Me.GetActiveItemType(Selection)
            csForm.SearchPath = Me.GetExecutablesPath(Selection)
            csForm.AssemblyID = AssemblyID
            csForm.UseCloseChar = CloseChar
            csForm.FillList()
            csForm.ShowDialog(owner)
        End Sub

        Private Sub ShowFunctionSearchFor(ByRef owner As IWin32Window, ByVal AssemblyID As String, ByVal ClassID As String, ByVal CloseChar As Char, ByVal Selection As EnvDTE.TextSelection)
            Dim cursorPoint As EnvDTE.VirtualPoint =
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint =
                cursorPoint.CreateEditPoint()

            Dim fsForm As New Xeora.VSAddIn.Forms.FunctionSearch(Selection, cursorPoint.LineCharOffset)
            fsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
            fsForm.BackSpaceForEmptyTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
            fsForm.EnteredTextHandler = New Xeora.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
            fsForm.VisibleInLocation = Me.CalculateLocation(Selection)
            fsForm.SearchType = Me.GetActiveItemType(Selection)
            fsForm.SearchPath = Me.GetExecutablesPath(Selection)
            fsForm.AssemblyID = AssemblyID
            fsForm.ClassID = ClassID
            fsForm.UseCloseChar = CloseChar
            fsForm.FillList()
            fsForm.ShowDialog(owner)
        End Sub

        Private Function CalculateLocation(ByVal Selection As TextSelection) As Drawing.Point
            ' screen dpi to calculate the pixel
            Dim graph As Drawing.Graphics =
                Drawing.Graphics.FromHwnd(CType(Selection.DTE.MainWindow.HWnd, IntPtr))

            ' Calculate the cursor position according to the active window
            ' Selection.TextPane.Height = how many twips (lines) not pixel. 
            ' 1 twip is 1/20 point. point = pixel * (dpi / 96)
            ' So pixel calculation will be (1 twip * 20) / (dpi / 96) = pixel
            ' Selection.DTE.ActiveWindow.Height = how many pixel...
            Dim LineHeightInPixel As Integer =
                CType((Selection.DTE.ActiveWindow.Height / (Selection.TextPane.Height - 1)) * (graph.DpiY / 96.0F), Integer)
            ' Selection.DTE.ActiveWindow.Width = how many pixel...
            ' Selection.TextPane.Width = how many columns in twips measure not pixel

            ' Left breakpoint marking area, 7 is constant
            Dim TextPaneUnit As Double =
                Selection.DTE.ActiveWindow.Width / Selection.TextPane.Width
            Dim TextPaneLeftSide As Integer =
                CType(TextPaneUnit * 7, Integer)
            'Dim Constant As Double = (72.0F / TextPaneLeftSide) / (graph.DpiX / 96.0F)

            'Dim TextPaneLeftSide As Integer =
            '    CType(Selection.TextPane.Width * 20 / (graph.DpiX / 96.0F), Integer)
            'TextPaneLeftSide -= Selection.DTE.ActiveWindow.Width
            'TextPaneLeftSide = CType(TextPaneLeftSide * (graph.DpiX / 96.0F), Integer) * -1

            'Dim CharWidthInPixel As Integer = CType((ActiveWindowInsideWidth / Selection.TextPane.Width) * (graph.DpiX / 96.0F), Integer)

            ' Take care of scrolled content
            Dim CalculatedTop As Integer =
                (Selection.ActivePoint.Line - Selection.TextPane.StartPoint.Line) * LineHeightInPixel

            Dim LineCharOffset As Integer = (Selection.TextPane.StartPoint.LineCharOffset - 1) \ 12
            LineCharOffset = Selection.ActivePoint.LineCharOffset - LineCharOffset

            Dim CalculatedLeft As Integer = CType(TextPaneLeftSide * (graph.DpiY / 96.0F), Integer)
            CalculatedLeft += CType(((LineCharOffset * TextPaneUnit) * Selection.DTE.ActiveWindow.Width) / Selection.TextPane.Width, Integer)
            ' !---

            ' Calculate the Top position of VS Document Window
            ' Initial value is 54 for window head border and also 30 for tab label of document window
            ' Add 24 to locate the intellisence under the line
            'CalculatedTop += CType((54 + 30 + 24) * (graph.DpiY / 96.0F), Integer)

            ' Calculate the Left position of VS Document Window
            ' Just blind add 64 pixel for left located autohidden toolbar tabs
            ' and 54 pixel for line numbers, breakpoint and indicator columns of document
            ' LeftCompareLimit will be used to compare the toolbar locations
            'Dim LeftCompareLimit As Integer = CType((64 * (graph.DpiX / 96.0F)), Integer)
            'CalculatedLeft += CType((64 + 54) * (graph.DpiX / 96.0F), Integer)

            ' Prepare MainWindow Position on Screen
            'Dim mWLeft As Integer = Selection.DTE.MainWindow.Left
            'Dim mWTop As Integer = Selection.DTE.MainWindow.Top

            ' if window maximized, do not care the positions
            'If Selection.DTE.MainWindow.WindowState = vsWindowState.vsWindowStateMaximize Then mWLeft = 0 : mWTop = 0
            ' !---

            'CalculatedTop += mWTop
            'CalculatedLeft += mWLeft

            '' Calculate CommandBar height
            'Dim commandBars As CommandBars =
            '    CType(Selection.DTE.CommandBars, CommandBars)

            'For Each cB As CommandBar In commandBars
            '    If (cB.Position = MsoBarPosition.msoBarMenuBar OrElse cB.Position = MsoBarPosition.msoBarTop) AndAlso cB.Enabled AndAlso cB.Visible Then CalculatedTop += cB.Height
            'Next
            '' !---

            'Dim LinkedWindows As LinkedWindows =
            '    Selection.DTE.MainWindow.LinkedWindows

            '' linkedwindows ordered in counter clockwise (i think)
            '' first is always active document, then left toolbars, bottom toolbars then right toolbars.
            '' bottom toolbars are the same size with active document, because of that when i catch the window
            '' with the same width with active document, I understand that i already passed the left toolbars.
            '' So i dont need to calculate left total anymore I know it is discussting solution but at least it 
            '' Is working for now.. If you have better solution do so..
            'For Each tB As Window In LinkedWindows
            '    If tB.Type = vsWindowType.vsWindowTypeDocument Then Continue For

            '    If tB.Left <= LeftCompareLimit AndAlso Not tB.AutoHides AndAlso
            '        tB.Width <> Selection.DTE.ActiveDocument.ActiveWindow.Width Then

            '        CalculatedLeft += tB.Width
            '    End If
            'Next
            ' !---

            Dim ActiveWindow As Window = Selection.DTE.ActiveWindow

            Do While Not ActiveWindow Is Nothing
                CalculatedLeft += ActiveWindow.Left
                CalculatedTop += ActiveWindow.Top

                ActiveWindow = ActiveWindow.LinkedWindowFrame
            Loop

            Return New Drawing.Point(CType(CalculatedLeft / (graph.DpiX / 96.0F), Integer), CType(CalculatedTop / (graph.DpiY / 96.0F), Integer))
        End Function

        Private Sub BackSpaceForEmptyText(ByVal Selection As EnvDTE.TextSelection)
            Dim editPoint As EnvDTE.EditPoint =
                Selection.ActivePoint.CreateEditPoint()

            editPoint.CharLeft(1)
            editPoint.Delete(1)
        End Sub

        Private Sub EnteredTextChange(ByVal CurrentText As String, ByVal BeginningOffset As Integer, ByVal Selection As EnvDTE.TextSelection, ByVal RaiseIntelliSense As Boolean)
            Dim activePoint As EnvDTE.VirtualPoint =
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint =
                activePoint.CreateEditPoint()

            editPoint.MoveToLineAndOffset(activePoint.Line, BeginningOffset)
            editPoint.Delete(activePoint)
            editPoint.Insert(CurrentText)

            If RaiseIntelliSense Then
                editPoint.CharLeft()

                Me.event_TDAfterKeyPressed(editPoint.GetText(1), Selection, False)
            End If
        End Sub

        Public Delegate Sub HandleResultDelegate(ByRef owner As IWin32Window, ByVal IsAccepted As Boolean, ByVal BeginningOffset As Integer, ByVal Selection As EnvDTE.TextSelection, ByVal ISType As Globals.ISTypes, ByVal AcceptChar As Char, ByVal CloseChar As Char, ByVal Params As Object())

        Private Sub HandleResult(ByRef owner As IWin32Window, ByVal IsAccepted As Boolean, ByVal BeginningOffset As Integer, ByVal Selection As EnvDTE.TextSelection, ByVal ISType As Globals.ISTypes, ByVal AcceptChar As Char, ByVal CloseChar As Char, ByVal Params As Object())
            If IsAccepted Then
                Dim CloseChar_s As String

                If CloseChar = Nothing OrElse
                    CloseChar = Chr(0) Then

                    CloseChar_s = String.Empty
                Else
                    CloseChar_s = CType(CloseChar, String)
                End If

                Dim AcceptChar_s As String

                If AcceptChar = Nothing OrElse
                    AcceptChar = Chr(0) Then

                    AcceptChar_s = String.Empty
                Else
                    AcceptChar_s = CType(AcceptChar, String)
                End If

                Dim cursorPoint As EnvDTE.VirtualPoint =
                    Selection.ActivePoint
                Dim editPoint As EnvDTE.EditPoint =
                    cursorPoint.CreateEditPoint()

                editPoint.MoveToLineAndOffset(cursorPoint.Line, BeginningOffset)

                Select Case ISType
                    Case Globals.ISTypes.ControlSearch
                        Dim ControlType As Globals.ControlTypes =
                            CType(Params(0), Globals.ControlTypes)
                        Dim ControlID As String =
                            CType(Params(1), String)

                        Select Case ControlType
                            Case Globals.ControlTypes.Button, Globals.ControlTypes.Checkbox, Globals.ControlTypes.Image, Globals.ControlTypes.Link, Globals.ControlTypes.Password, Globals.ControlTypes.Radio, Globals.ControlTypes.Textarea, Globals.ControlTypes.Textbox
                                editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", ControlID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                            Case Globals.ControlTypes.ConditionalStatement, Globals.ControlTypes.DataList, Globals.ControlTypes.VariableBlock
                                editPoint.ReplaceText(cursorPoint, String.Format("{0}:{{}}:{0}{1}", ControlID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                                ' Locate Cursor Between Brackets
                                Dim StringLength As Integer = String.Format("}}:{0}{1}", ControlID, CloseChar_s).Length
                                cursorPoint.Parent.Selection.MoveToLineAndOffset(cursorPoint.Line, cursorPoint.LineCharOffset - StringLength)
                        End Select

                    Case Globals.ISTypes.TemplateSearch
                        Dim TemplateID As String =
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(TemplateID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", TemplateID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                    Case Globals.ISTypes.TranslationSearch
                        Dim TranslationID As String =
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(TranslationID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", TranslationID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                    Case Globals.ISTypes.AssemblySearch
                        Dim AssemblyID As String =
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(AssemblyID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", AssemblyID, AcceptChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                            Me.ShowClassSearchFor(owner, AssemblyID, CloseChar, Selection)
                        End If

                    Case Globals.ISTypes.ClassSearch
                        Dim AssemblyID As String =
                            CType(Params(0), String)
                        Dim ClassID As String =
                            CType(Params(1), String)

                        If Not String.IsNullOrEmpty(AssemblyID) AndAlso Not String.IsNullOrEmpty(ClassID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", ClassID, AcceptChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                            Me.ShowFunctionSearchFor(owner, AssemblyID, ClassID, CloseChar, Selection)
                        End If

                    Case Globals.ISTypes.FunctionSearch
                        Dim FunctionID As String =
                            CType(Params(0), String)
                        Dim FunctionParameters As String() =
                            CType(Params(1), String())

                        If Not String.IsNullOrEmpty(FunctionID) AndAlso Not FunctionParameters Is Nothing Then
                            If FunctionParameters.Length = 0 Then
                                editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", FunctionID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                            Else
                                Dim CompiledParameterNames As String = String.Empty

                                For Each pN As String In FunctionParameters
                                    CompiledParameterNames &= String.Format("{0}|", pN)
                                Next
                                CompiledParameterNames = CompiledParameterNames.Substring(0, CompiledParameterNames.Length - 1)

                                editPoint.ReplaceText(cursorPoint, String.Format("{0},{1}{2}", FunctionID, CompiledParameterNames, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                            End If
                        End If

                    Case Globals.ISTypes.TypeSearch
                        Dim ControlTypeString As String =
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(ControlTypeString) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", ControlTypeString, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                    Case Globals.ISTypes.OnFlyRequest
                        Dim ControlID As String =
                            CType(Params(0), String)

                        editPoint.ReplaceText(cursorPoint, String.Format("{{}}:{0}{1}", ControlID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                        ' Locate Cursor Between Brackets
                        Dim StringLength As Integer = String.Format("}}:{0}{1}", ControlID, CloseChar_s).Length
                        cursorPoint.Parent.Selection.MoveToLineAndOffset(cursorPoint.Line, cursorPoint.LineCharOffset - StringLength)

                    Case Globals.ISTypes.PrimitiveStatement
                        Dim ControlID As String =
                            CType(Params(0), String)

                        editPoint.ReplaceText(cursorPoint, String.Format("{{}}:{0}{1}", ControlID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                        ' Locate Cursor Between Brackets
                        Dim StringLength As Integer = String.Format("}}:{0}{1}", ControlID, CloseChar_s).Length
                        cursorPoint.Parent.Selection.MoveToLineAndOffset(cursorPoint.Line, cursorPoint.LineCharOffset - StringLength)

                    Case Globals.ISTypes.SpecialPropertySearch
                        Dim SpecialPropertyType As Globals.SpecialPropertyTypes =
                            CType(Params(0), Globals.SpecialPropertyTypes)
                        Dim SpecialPropertyID As String =
                            CType(Params(1), String)

                        Select Case SpecialPropertyType
                            Case Globals.SpecialPropertyTypes.Single
                                editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", SpecialPropertyID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                            Case Globals.SpecialPropertyTypes.Block
                                editPoint.ReplaceText(cursorPoint, String.Format("{0}:{{}}:{0}{1}", SpecialPropertyID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                                ' Locate Cursor Between Brackets
                                Dim StringLength As Integer = String.Format("}}:{0}{1}", SpecialPropertyID, CloseChar_s).Length
                                cursorPoint.Parent.Selection.MoveToLineAndOffset(cursorPoint.Line, cursorPoint.LineCharOffset - StringLength)

                                If String.Compare(SpecialPropertyID, "XF") = 0 Then Me.ShowAssemblySearchFor(owner, Nothing, Selection)
                            Case Globals.SpecialPropertyTypes.Control
                                editPoint.ReplaceText(cursorPoint, String.Format("{0}:", SpecialPropertyID), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                                Me.event_TDAfterKeyPressed(":", Selection, False)
                        End Select

                    Case Globals.ISTypes.ControlSearchForParenting
                        Dim ControlIDString As String =
                            CType(Params(1), String)

                        If Not String.IsNullOrEmpty(ControlIDString) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", ControlIDString, AcceptChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                End Select
            End If
        End Sub

        Private Function CheckIsXeoraCubeProject(ByVal Selection As EnvDTE.TextSelection) As Boolean
            Dim rBoolean As Boolean = False

            Dim ActiveProject As EnvDTE.Project =
                Selection.DTE.ActiveDocument.ProjectItem.ContainingProject

            Return Me.CheckProjectFolders(ActiveProject.ProjectItems)
        End Function

        Private Function CheckProjectFolders(ByVal pItems As EnvDTE.ProjectItems) As Boolean
            Dim rBoolean As Boolean = False

            Dim AddonsPath As String = String.Empty
            Dim ContentsPath As String = String.Empty
            Dim LanguagesPath As String = String.Empty
            Dim TemplatesPath As String = String.Empty
            Dim ExecutablesPath As String = String.Empty

            For iC As Integer = 1 To pItems.Count
                If String.Compare(pItems.Item(iC).Name, "Addons") = 0 Then
                    AddonsPath = pItems.Item(iC).FileNames(0)
                ElseIf String.Compare(pItems.Item(iC).Name, "Contents") = 0 Then
                    ContentsPath = pItems.Item(iC).FileNames(0)
                ElseIf String.Compare(pItems.Item(iC).Name, "Languages") = 0 Then
                    LanguagesPath = pItems.Item(iC).FileNames(0)
                ElseIf String.Compare(pItems.Item(iC).Name, "Templates") = 0 Then
                    TemplatesPath = pItems.Item(iC).FileNames(0)
                ElseIf String.Compare(pItems.Item(iC).Name, "Executables") = 0 Then
                    ExecutablesPath = pItems.Item(iC).FileNames(0)
                Else
                    If (
                        String.IsNullOrEmpty(ContentsPath) OrElse
                        String.IsNullOrEmpty(LanguagesPath) OrElse
                        String.IsNullOrEmpty(TemplatesPath) OrElse
                        String.IsNullOrEmpty(ExecutablesPath)
                        ) AndAlso
                        Not pItems.Item(iC).ProjectItems Is Nothing AndAlso
                        pItems.Item(iC).ProjectItems.Count > 0 Then

                        rBoolean = Me.CheckProjectFolders(pItems.Item(iC).ProjectItems)
                    End If
                End If

                If rBoolean Then Exit For
            Next

            If Not rBoolean AndAlso Not String.IsNullOrEmpty(ContentsPath) AndAlso
                Not String.IsNullOrEmpty(LanguagesPath) AndAlso
                Not String.IsNullOrEmpty(TemplatesPath) Then

                rBoolean = True

                Dim FileNames As String() = IO.Directory.GetFiles(LanguagesPath, "*.xml")

                For Each fileName As String In FileNames
                    If Not IO.Directory.Exists(
                        IO.Path.Combine(ContentsPath, IO.Path.GetFileNameWithoutExtension(fileName))) Then

                        rBoolean = False

                        Exit For
                    End If
                Next

                If rBoolean Then
                    FileNames = IO.Directory.GetFiles(TemplatesPath, "*.xml")

                    For Each fileName As String In FileNames
                        If String.Compare(IO.Path.GetFileNameWithoutExtension(fileName), "Configuration") <> 0 AndAlso
                            String.Compare(IO.Path.GetFileNameWithoutExtension(fileName), "ControlsMap") <> 0 Then

                            rBoolean = False

                            Exit For
                        End If
                    Next
                End If
            End If

            Return rBoolean
        End Function

        Private Function GetActiveItemType(ByVal Selection As EnvDTE.TextSelection) As AddInLoader.SearchTypes
            Dim sT As AddInLoader.SearchTypes = AddInLoader.SearchTypes.Domain

            Dim ActiveItemPath As String =
                Selection.DTE.ActiveDocument.Path

            Dim pC As Integer = 4
            Dim dI As IO.DirectoryInfo =
                New IO.DirectoryInfo(ActiveItemPath)
            Do
                dI = IO.Directory.GetParent(dI.FullName)

                pC -= 1
            Loop Until pC = 0

            If String.Compare(dI.Name, "Addons", True) = 0 Then _
                sT = AddInLoader.SearchTypes.Child

            Return sT
        End Function

        Private Function GetTemplatePath(ByVal Selection As EnvDTE.TextSelection) As String
            Dim ActiveItemPath As String =
                Selection.DTE.ActiveDocument.Path

            Return ActiveItemPath
        End Function

        Private Function GetLanguagePath(ByVal Selection As EnvDTE.TextSelection) As String
            Dim ActiveItemPath As String =
                Selection.DTE.ActiveDocument.Path

            Return IO.Path.GetFullPath(
                        IO.Path.Combine(ActiveItemPath, "../Languages")
                    )
        End Function

        Private Function GetExecutablesPath(ByVal Selection As EnvDTE.TextSelection) As String
            Dim ActiveItemPath As String =
                Selection.DTE.ActiveDocument.Path

            Return IO.Path.GetFullPath(
                        IO.Path.Combine(ActiveItemPath, "../Executables")
                    )
        End Function

        Public ReadOnly Property Handle As IntPtr Implements IWin32Window.Handle
            Get
                Return New System.IntPtr(Me._applicationObject.MainWindow.HWnd)
            End Get
        End Property

    End Class
End Namespace
