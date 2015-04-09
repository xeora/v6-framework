Option Strict On

Imports Microsoft.VisualStudio.CommandBars
Imports EnvDTE

Namespace XeoraCube.VSAddIn
    Public Class AddInControl
        Implements IWin32Window

        Private _applicationObject As EnvDTE80.DTE2
        Private _eventContainer As New Generic.List(Of CommandBarEvents)

        Private _solutionEvents As SolutionEvents
        Private _documentEvents As New Generic.Dictionary(Of String, DocumentEvents)
        Private _windowEvents As New Generic.Dictionary(Of String, WindowEvents)

        Public Sub New(ByRef application As EnvDTE80.DTE2, ByRef addInInst As AddIn)
            Me._applicationObject = application

            Try
                Dim xeoraCubeCustomCommand As EnvDTE.Command = _
                    Me._applicationObject.DTE.Commands.AddNamedCommand(addInInst, "GotoControlReferance", "Go To Definition", String.Empty, False)

                xeoraCubeCustomCommand.Bindings = New Object() {"Global::Ctrl+Shift+D,Ctrl+Shift+D"}
            Catch ex As Exception
                ' Just Handle Exceptions
                ' Probably command is already added...
            End Try

            For Each oCommandBar As CommandBar In CType(Me._applicationObject.CommandBars, CommandBars)
                If String.Compare(oCommandBar.Name, "HTML Context") = 0 OrElse _
                   String.Compare(oCommandBar.Name, "Code Window") = 0 Then
                    Dim oPopup As CommandBarPopup = _
                        CType( _
                            oCommandBar.Controls.Add( _
                                MsoControlType.msoControlPopup, _
                                System.Reflection.Missing.Value, _
                                System.Reflection.Missing.Value, 1, True),  _
                            CommandBarPopup _
                        )
                    oPopup.Caption = "Xeora³"

                    ' Goto Control Referance
                    Dim oControl As CommandBarButton = _
                        CType( _
                            oPopup.Controls.Add( _
                                MsoControlType.msoControlButton, _
                                System.Reflection.Missing.Value, _
                                System.Reflection.Missing.Value, 1, True),  _
                            CommandBarButton _
                        )
                    oControl.Caption = "Go To Definition"
                    oControl.ShortcutText = "Ctrl+Shift+D, Ctrl+Shift+D"

                    Dim cbEvent As CommandBarEvents = _
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
            Dim mainWindowEvent As WindowEvents = _
                CType(Me._applicationObject.Events.WindowEvents(Me._applicationObject.MainWindow), WindowEvents)

            AddHandler mainWindowEvent.WindowActivated, AddressOf Me.CheckContextMenu

            Me._windowEvents.Item(String.Format("{0}-{1}", Me._applicationObject.MainWindow.Caption, Me._applicationObject.MainWindow.Kind)) = mainWindowEvent

            Me.AssignHandlersToWindows(Me._applicationObject.Windows, Nothing)

            Dim documentEvent As DocumentEvents = _
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
                    Dim documentEvent As DocumentEvents = _
                        CType(Me._applicationObject.Events.DocumentEvents(document), DocumentEvents)

                    AddHandler documentEvent.DocumentClosing, AddressOf Me.DocumentClosing

                    Me._documentEvents.Item(String.Format("{0}-{1}", document.Name, document.Kind)) = documentEvent
                Next
            End If
        End Sub

        Private Sub AssignHandlersToWindows(ByRef windows As Windows, ByRef linkedWindows As LinkedWindows)
            If Not windows Is Nothing Then
                For Each window As Window In windows
                    Dim windowEvent As WindowEvents = _
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
                    Dim windowEvent As WindowEvents = _
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
            Dim documentEvent As DocumentEvents = _
                CType(Me._applicationObject.Events.DocumentEvents(document), DocumentEvents)

            AddHandler documentEvent.DocumentClosing, AddressOf Me.DocumentClosing

            Me._documentEvents.Item(String.Format("{0}-{1}", document.Name, document.Kind)) = documentEvent
        End Sub

        Private Sub DocumentClosing(ByVal document As Document)
            Me._windowEvents.Remove(String.Format("{0}-{1}", document.Name, document.Kind))
        End Sub

        Private Sub CheckContextMenu(ByVal GotFocus As Window, ByVal LostFocus As Window)
            If Not Me._applicationObject.ActiveDocument Is Nothing Then
                Dim ActiveDocFI As IO.FileInfo = _
                    New IO.FileInfo(Me._applicationObject.ActiveDocument.FullName)
                Dim IsControlMapXMLFile As Boolean = _
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
                        If String.Compare(ACI.AssemblyFile, AssemblyFile) = 0 AndAlso _
                            Date.Compare(ACI.AssemblyDate, AssemblyFileInfo.LastWriteTime) >= 0 Then

                            Select Case QueryType
                                Case QueryTypes.ClassListStatus
                                    If ACI.ClassListTouched Then rBoolean = True
                                Case QueryTypes.ClassProcedureListStatus
                                    rBoolean = True

                                    If Not String.IsNullOrEmpty(ClassID) Then
                                        Dim CO As AssemblyCacheInfo.ClassObject = _
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

                    If aCI.AssemblyType = AssemblyType AndAlso _
                        String.Compare(aCI.AssemblyPath, AssemblyPath) = 0 AndAlso _
                        String.Compare(aCI.AssemblyID, AssemblyID) = 0 Then

                        Me._AssemblyCache.RemoveAt(aCIC)
                    End If
                Next

                Me._AssemblyCache.Add( _
                    New AssemblyCacheInfo(AssemblyType, AssemblyPath, AssemblyID))
            End Sub

            Public Sub AddClassIDIntoAssemblyInfo(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyFile As String, ByVal ClassID As String)
                Dim tACI As AssemblyCacheInfo = Nothing

                For aCIC As Integer = Me._AssemblyCache.Count - 1 To 0 Step -1
                    Dim aCI As AssemblyCacheInfo = Me._AssemblyCache.Item(aCIC)

                    If aCI.AssemblyType = AssemblyType AndAlso _
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

                    If aCI.AssemblyType = AssemblyType AndAlso _
                        String.Compare(aCI.AssemblyFile, AssemblyFile) = 0 Then

                        tACI = aCI

                        Me._AssemblyCache.RemoveAt(aCIC)
                    End If
                Next

                If Not tACI Is Nothing Then
                    Dim CO As AssemblyCacheInfo.ClassObject = _
                        tACI.GetClass(ClassID)

                    If Not CO Is Nothing Then CO.AddProcedureInfo(ProcedureID, ProcedureParams) : tACI.AddClassobject(CO)

                    Me._AssemblyCache.Add(tACI)
                End If
            End Sub

            Public Function GetAssemblyIDs(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyPath As String) As String()
                Dim rStringList As New Generic.List(Of String)

                For Each aCI As AssemblyCacheInfo In Me._AssemblyCache
                    If AssemblyType = aCI.AssemblyType AndAlso _
                        String.Compare(AssemblyPath, aCI.AssemblyPath) = 0 Then _
                        rStringList.Add(aCI.AssemblyID)
                Next

                Return rStringList.ToArray()
            End Function

            Public Function GetAssemblyClassIDs(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyFile As String) As String()
                Dim rClassIDs As String() = New String() {}

                For Each aCI As AssemblyCacheInfo In Me._AssemblyCache
                    If AssemblyType = aCI.AssemblyType AndAlso _
                        String.Compare(AssemblyFile, aCI.AssemblyFile) = 0 Then _
                        rClassIDs = aCI.ClassIDList : Exit For
                Next

                Return rClassIDs
            End Function

            Public Function GetAssemblyClassProcedureInfos(ByVal AssemblyType As AddInLoader.SearchTypes, ByVal AssemblyFile As String, ByVal ClassID As String) As AssemblyCacheInfo.ClassObject.ClassProcedureInfo()
                Dim rProcedureInfos As New Generic.List(Of AssemblyCacheInfo.ClassObject.ClassProcedureInfo)

                For Each aCI As AssemblyCacheInfo In Me._AssemblyCache
                    If AssemblyType = aCI.AssemblyType AndAlso _
                        String.Compare(AssemblyFile, aCI.AssemblyFile) = 0 Then

                        Dim CO As AssemblyCacheInfo.ClassObject = _
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

                    Me._ClassObjects.Add( _
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

                            If String.Compare(CPI.ProcedureID, ProcedureID) = 0 AndAlso _
                                Me.ListCompare(CPI.ProcedureParams, ProcedureParams) Then _
                                Me._ClassProcedures.RemoveAt(cPIC)
                        Next

                        Me._ClassProcedures.Add( _
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

            Dim OriginalOffset As Integer = _
                editPoint.LineCharOffset

            Do Until editPoint.LineCharOffset = 1
                editPoint.CharLeft()

                LastString = editPoint.GetText(1)
                SearchID = String.Concat(LastString, SearchID)

                If String.IsNullOrWhiteSpace(LastString) OrElse _
                    (SearchID.Length = 2 AndAlso String.Compare(SearchID, "$$") = 0) Then
                    editPoint.CharRight(SearchID.Length)

                    SearchID = String.Empty

                    Exit Do
                End If

                Dim SearchMatch As System.Text.RegularExpressions.Match

                If IsXMLFile Then
                    SearchMatch = System.Text.RegularExpressions.Regex.Match(SearchID, "\<CallFunction\>")
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
            Dim StatementRegEx As String() = _
                New String() _
                { _
                    "\$((([#]+|[\^\-\+\*\~])?(\w+)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)\$|\w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?\:([\.\w\-]+\$|[\.\w\-]+\?[\.\w\-]+(\,((\|)?([#\.\^\-\+\*\~]*([\w+][^\$]*)|\=([\S+][^\$]*)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)?)*)?\$))|\}\:[\.\w\-]+\:\{|\}\:[\.\w\-]+\$", _
                    "\}:(?<ItemID>[\.\w\-]+)\$" _
                }

            Dim OriginalOffset As Integer = _
                editPoint.LineCharOffset

            Do Until editPoint.LineCharOffset = 1
                editPoint.CharLeft()

                LastString = editPoint.GetText(1)
                SearchID = String.Concat(LastString, SearchID)

                If String.IsNullOrWhiteSpace(LastString) OrElse _
                    (SearchID.Length = 2 AndAlso String.Compare(SearchID, "$$") = 0) Then
                    editPoint.CharRight(SearchID.Length)

                    SearchID = String.Empty

                    Exit Do
                End If

                For mC As Integer = 0 To 1
                    Dim SearchMatch As System.Text.RegularExpressions.Match = _
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
            Dim selection As EnvDTE.TextSelection = _
                CType(appEvents.ActiveDocument.Selection, EnvDTE.TextSelection)

            Dim cursorPoint As EnvDTE.VirtualPoint = _
                selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint = _
                cursorPoint.CreateEditPoint()

            Dim cursorLastPossion As Integer = _
                cursorPoint.LineCharOffset

            Dim ControlTypeReferance As Char, BeginOffset As Integer = -1, EndOffset As Integer = -1

            Dim ActiveDocFI As IO.FileInfo = _
                New IO.FileInfo(appEvents.ActiveDocument.FullName)
            Dim IsXMLFile As Boolean = _
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
                    Dim ActiveDocDI As IO.DirectoryInfo = _
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
                                Dim itemWindow As Window = _
                                    ProjItem.Open(Constants.vsViewKindTextView)
                                itemWindow.Activate()

                                CType(itemWindow.Document.Selection, TextSelection).StartOfDocument()
                                If Not CType(itemWindow.Document.Selection, TextSelection).FindText(String.Format("controlid=['""]{0}['""]", SearchID), vsFindOptions.vsFindOptionsRegularExpression) Then
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
                                        Dim docType As String = _
                                            MainProjectItem.Name.Substring(MainProjectItem.Name.LastIndexOf("."c) + 1)

                                        Dim PrevState As Boolean = MainProjectItem.IsOpen
                                        Dim itemWindow As Window = _
                                            MainProjectItem.Open(Constants.vsViewKindCode)

                                        Dim TS As TextSelection = _
                                            CType(itemWindow.Document.Selection, TextSelection)

                                        TS.EndOfDocument()
                                        Dim DocEndOffset As Integer = _
                                            TS.ActivePoint.AbsoluteCharOffset
                                        TS.StartOfDocument()

                                        Dim EP As EditPoint = _
                                            TS.ActivePoint.CreateEditPoint()

                                        Dim CodeContent As String = _
                                            EP.GetText(DocEndOffset)

                                        Dim LineNumber As Integer = _
                                            Me.GetDefinitionLineNumber(docType, "WebDynamics", String.Format("{0}+{1}", AssemblyName, ClassName), FunctionName, ParametersLength, CodeContent)

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

        Private Function GetDefinitionLineNumber(ByVal LanguageID As String, ByVal [Namespace] As String, ByVal JoinedTypeName As String, ByVal MethodName As String, ByVal ParameterLength As Integer, ByVal DocumentContent As String) As Integer
            Dim rInteger As Integer = -1

            Dim parser As ICSharpCode.NRefactory.IParser = Nothing

            Dim codeContentReader As New IO.StringReader(DocumentContent)
            Select Case LanguageID
                Case "vb"
                    parser = ICSharpCode.NRefactory.ParserFactory.CreateParser( _
                                ICSharpCode.NRefactory.SupportedLanguage.VBNet, _
                                codeContentReader)

                Case "cs"
                    parser = ICSharpCode.NRefactory.ParserFactory.CreateParser( _
                                ICSharpCode.NRefactory.SupportedLanguage.VBNet, _
                                codeContentReader)
            End Select

            If Not parser Is Nothing Then
                parser.ParseMethodBodies = False
                parser.Parse()
            End If

            codeContentReader.Close()

            If Not parser Is Nothing AndAlso Not parser.CompilationUnit Is Nothing Then
                Dim NodeList As Generic.List(Of ICSharpCode.NRefactory.Ast.INode) = _
                    parser.CompilationUnit.Children

                Dim NSFound As Boolean = False, TypeFound As Boolean = False, FuncFound As Boolean = False, TypeNames As String() = JoinedTypeName.Split("+"c), WorkingTypeID As Integer = 0
                Do While Not NodeList Is Nothing AndAlso NodeList.Count > 0
                    If NSFound AndAlso TypeFound AndAlso Not FuncFound Then
                        If TypeOf NodeList.Item(0) Is ICSharpCode.NRefactory.Ast.MethodDeclaration AndAlso _
                            String.Compare(CType(NodeList.Item(0), ICSharpCode.NRefactory.Ast.MethodDeclaration).Name, MethodName) = 0 AndAlso _
                            CType(NodeList.Item(0), ICSharpCode.NRefactory.Ast.MethodDeclaration).Parameters.Count = ParameterLength Then

                            FuncFound = True : rInteger = NodeList.Item(0).StartLocation.Line

                            Exit Do
                        End If
                    End If

                    If NSFound AndAlso Not TypeFound Then
                        If TypeOf NodeList.Item(0) Is ICSharpCode.NRefactory.Ast.TypeDeclaration AndAlso _
                            String.Compare(CType(NodeList.Item(0), ICSharpCode.NRefactory.Ast.TypeDeclaration).Name, TypeNames(WorkingTypeID)) = 0 Then

                            WorkingTypeID += 1

                            If WorkingTypeID = TypeNames.Length Then TypeFound = True

                            NodeList = NodeList.Item(0).Children : Continue Do
                        End If
                    End If

                    If TypeOf NodeList.Item(0) Is ICSharpCode.NRefactory.Ast.NamespaceDeclaration AndAlso _
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

        Public Sub event_TDAfterKeyPressed(ByRef owner As IWin32Window, ByVal KeyPress As String, ByVal Selection As EnvDTE.TextSelection, ByVal InStatementCompletion As Boolean)
            If owner Is Nothing Then owner = Me

            Dim FileName As String = Selection.Parent.Parent.ProjectItem.Name
            Dim TrackingChars As Char() = New Char() {"$"c, ":"c, "?"c, "."c, ">"c, "["c, "C"c}

            If InStatementCompletion OrElse String.IsNullOrEmpty(FileName) Then Exit Sub
            If Array.IndexOf(TrackingChars, KeyPress.Chars(0)) = -1 Then Exit Sub
            If Not Me.CheckIsXeoraCubeProject(Selection) Then Exit Sub

            ' Reach here if you still not Exited from Sub
            If String.Compare(IO.Path.GetExtension(FileName), ".htm", True) = 0 Then
                If String.Compare(KeyPress, "$") = 0 Then
                    Dim editPoint As EnvDTE.EditPoint = _
                        Selection.ActivePoint.CreateEditPoint()

                    If Me.CheckIsLastingStatement(editPoint) Then Exit Sub

                    Dim spForm As New XeoraCube.VSAddIn.Forms.SpecialSearch(Selection, Selection.ActivePoint.LineCharOffset)
                    spForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                    spForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                    spForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                    spForm.VisibleInLocation = Me.CalculateLocation(Selection)
                    spForm.FillList()
                    spForm.ShowDialog(owner)

                ElseIf String.Compare(KeyPress, "[") = 0 Then
                    Dim editPoint As EnvDTE.EditPoint = _
                        Selection.ActivePoint.CreateEditPoint()

                    Dim ControlTypeReferance As Char = "@"c, Offset As Integer = -1

                    Me.GetReferanceStringFromBegining(editPoint, ControlTypeReferance, Offset, False)

                    If Offset = -1 Then Exit Sub

                    Select Case ControlTypeReferance
                        Case "C"c
                            Dim cspForm As New XeoraCube.VSAddIn.Forms.ControlSearchForParenting(Selection, Offset)
                            cspForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            cspForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            cspForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            cspForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            cspForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            cspForm.FillList()
                            cspForm.ShowDialog(owner)

                    End Select
                ElseIf String.Compare(KeyPress, "C") = 0 Then
                    Dim editPoint As EnvDTE.EditPoint = _
                        Selection.ActivePoint.CreateEditPoint()

                    Dim ControlTypeReferance As Char = "@"c, Offset As Integer = -1

                    Me.GetReferanceStringFromBegining(editPoint, ControlTypeReferance, Offset, False)

                    If Offset = -1 Then Exit Sub

                    Select Case ControlTypeReferance
                        Case "C"c
                            Dim csForm As New XeoraCube.VSAddIn.Forms.ControlSearch(Selection, Offset)
                            csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            csForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            csForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            csForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            csForm.FillList()
                            csForm.ShowDialog(owner)

                    End Select
                ElseIf String.Compare(KeyPress, ":") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint = _
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint = _
                        cursorPoint.CreateEditPoint()

                    Dim ControlTypeReferance As Char = "@"c, Offset As Integer = -1

                    Dim ControlReferanceText As String = _
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

                            Dim csForm As New XeoraCube.VSAddIn.Forms.ControlSearch(Selection, Offset)
                            csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            csForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            csForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            csForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            csForm.FillList()
                            csForm.ShowDialog(owner)

                        Case "T"c, "P"c
                            Dim tsForm As New XeoraCube.VSAddIn.Forms.TemplateSearch(Selection, Offset)
                            tsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            tsForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            tsForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            tsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            tsForm.TemplatesPath = Me.GetTemplatePath(Selection)
                            tsForm.FillList()
                            tsForm.ShowDialog(owner)

                        Case "L"c
                            Dim tsForm As New XeoraCube.VSAddIn.Forms.TranslationSearch(Selection, Offset)
                            tsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            tsForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            tsForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            tsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            tsForm.TranslationsPath = Me.GetTranslationPath(Selection)
                            tsForm.FillList()
                            tsForm.ShowDialog(owner)

                        Case "F"c
                            Dim asForm As New XeoraCube.VSAddIn.Forms.AssemblySearch(Selection, Offset)
                            asForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                            asForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                            asForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                            asForm.VisibleInLocation = Me.CalculateLocation(Selection)
                            asForm.SearchType = Me.GetActiveItemType(Selection)
                            asForm.SearchPath = Me.GetAssemblyPath(Selection)
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
                    Dim cursorPoint As EnvDTE.VirtualPoint = _
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint = _
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String = _
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(LineText, "\$F(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?:[\.\w\-]+\?", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim csForm As New XeoraCube.VSAddIn.Forms.ClassSearch(Selection, cursorPoint.LineCharOffset)
                        csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        csForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        csForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        csForm.SearchType = Me.GetActiveItemType(Selection)
                        csForm.SearchPath = Me.GetAssemblyPath(Selection)
                        csForm.AssemblyID = mI.Value.Substring(mI.Value.IndexOf(":"c) + 1, mI.Value.Length - (mI.Value.IndexOf(":"c) + 2))
                        csForm.FillList()
                        csForm.ShowDialog(owner)

                    End If
                ElseIf String.Compare(KeyPress, ".") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint = _
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint = _
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String = _
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(LineText, "\$F(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?:[\.\w\-]+\?[\.\w\-]+", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim fsForm As New XeoraCube.VSAddIn.Forms.FunctionSearch(Selection, cursorPoint.LineCharOffset)
                        fsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        fsForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        fsForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        fsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        fsForm.SearchType = Me.GetActiveItemType(Selection)
                        fsForm.SearchPath = Me.GetAssemblyPath(Selection)
                        fsForm.AssemblyID = mI.Value.Substring(mI.Value.IndexOf(":"c) + 1, mI.Value.IndexOf("?"c) - (mI.Value.IndexOf(":"c) + 1))
                        fsForm.ClassID = mI.Value.Substring(mI.Value.IndexOf("?"c) + 1, mI.Value.IndexOf("."c) - (mI.Value.IndexOf("?"c) + 1))
                        fsForm.FillList()
                        fsForm.ShowDialog(owner)

                    End If
                End If
            ElseIf String.Compare(FileName, "ControlsMap.xml", True) = 0 Then
                If String.Compare(KeyPress, ">") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint = _
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint = _
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String = _
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(LineText, "\<CallFunction\>", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim asForm As New XeoraCube.VSAddIn.Forms.AssemblySearch(Selection, cursorPoint.LineCharOffset)
                        asForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        asForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        asForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        asForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        asForm.SearchType = Me.GetActiveItemType(Selection)
                        asForm.SearchPath = Me.GetAssemblyPath(Selection)
                        asForm.UseCloseChar = Nothing
                        asForm.FillList()
                        asForm.ShowDialog(owner)

                        GoTo SKIPOTHERS
                    End If

                    mI = System.Text.RegularExpressions.Regex.Match(LineText, "\<DefaultButtonID\>", Text.RegularExpressions.RegexOptions.RightToLeft)
                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim csForm As New XeoraCube.VSAddIn.Forms.ControlSearch(Selection, cursorPoint.LineCharOffset)
                        csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        csForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        csForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
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
                        Dim tsForm As New XeoraCube.VSAddIn.Forms.TypeSearch(Selection, cursorPoint.Line)
                        tsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        tsForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        tsForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
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
                    Dim cursorPoint As EnvDTE.VirtualPoint = _
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint = _
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String = _
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(LineText, "\<CallFunction\>[\.\w\-]+\?", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim csForm As New XeoraCube.VSAddIn.Forms.ClassSearch(Selection, cursorPoint.LineCharOffset)
                        csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        csForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        csForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        csForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        csForm.SearchType = Me.GetActiveItemType(Selection)
                        csForm.SearchPath = Me.GetAssemblyPath(Selection)
                        csForm.AssemblyID = mI.Value.Substring(mI.Value.IndexOf(":"c) + 1, mI.Value.Length - (mI.Value.IndexOf(":"c) + 1))
                        csForm.FillList()
                        csForm.ShowDialog(owner)

                    End If
                ElseIf String.Compare(KeyPress, ".") = 0 Then
                    Dim cursorPoint As EnvDTE.VirtualPoint = _
                        Selection.ActivePoint
                    Dim editPoint As EnvDTE.EditPoint = _
                        cursorPoint.CreateEditPoint()
                    editPoint.StartOfLine()

                    Dim LineText As String = _
                        editPoint.GetText(cursorPoint)

                    Dim mI As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(LineText, "\<CallFunction\>[\.\w\-]+\?[\.\w\-]+", Text.RegularExpressions.RegexOptions.RightToLeft)

                    If mI.Success AndAlso (mI.Index + mI.Length) = (cursorPoint.LineCharOffset - 1) Then
                        Dim fsForm As New XeoraCube.VSAddIn.Forms.FunctionSearch(Selection, cursorPoint.LineCharOffset)
                        fsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
                        fsForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
                        fsForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
                        fsForm.VisibleInLocation = Me.CalculateLocation(Selection)
                        fsForm.SearchType = Me.GetActiveItemType(Selection)
                        fsForm.SearchPath = Me.GetAssemblyPath(Selection)
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
            Dim cursorPoint As EnvDTE.VirtualPoint = _
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint = _
                cursorPoint.CreateEditPoint()

            Dim asForm As New XeoraCube.VSAddIn.Forms.AssemblySearch(Selection, cursorPoint.LineCharOffset)
            asForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
            asForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
            asForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
            asForm.VisibleInLocation = Me.CalculateLocation(Selection)
            asForm.SearchType = Me.GetActiveItemType(Selection)
            asForm.SearchPath = Me.GetAssemblyPath(Selection)
            asForm.UseCloseChar = CloseChar
            asForm.FillList()
            asForm.ShowDialog(owner)
        End Sub

        Private Sub ShowClassSearchFor(ByRef owner As IWin32Window, ByVal AssemblyID As String, ByVal CloseChar As Char, ByVal Selection As EnvDTE.TextSelection)
            Dim cursorPoint As EnvDTE.VirtualPoint = _
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint = _
                cursorPoint.CreateEditPoint()

            Dim csForm As New XeoraCube.VSAddIn.Forms.ClassSearch(Selection, cursorPoint.LineCharOffset)
            csForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
            csForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
            csForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
            csForm.VisibleInLocation = Me.CalculateLocation(Selection)
            csForm.SearchType = Me.GetActiveItemType(Selection)
            csForm.SearchPath = Me.GetAssemblyPath(Selection)
            csForm.AssemblyID = AssemblyID
            csForm.UseCloseChar = CloseChar
            csForm.FillList()
            csForm.ShowDialog(owner)
        End Sub

        Private Sub ShowFunctionSearchFor(ByRef owner As IWin32Window, ByVal AssemblyID As String, ByVal ClassID As String, ByVal CloseChar As Char, ByVal Selection As EnvDTE.TextSelection)
            Dim cursorPoint As EnvDTE.VirtualPoint = _
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint = _
                cursorPoint.CreateEditPoint()

            Dim fsForm As New XeoraCube.VSAddIn.Forms.FunctionSearch(Selection, cursorPoint.LineCharOffset)
            fsForm.HandleResultDelegate = New HandleResultDelegate(AddressOf Me.HandleResult)
            fsForm.BackSpaceForEmptyTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.BackSpaceForEmptyTextDelegate(AddressOf Me.BackSpaceForEmptyText)
            fsForm.EnteredTextHandler = New XeoraCube.VSAddIn.Forms.ISFormBase.EnteredTextDelegate(AddressOf Me.EnteredTextChange)
            fsForm.VisibleInLocation = Me.CalculateLocation(Selection)
            fsForm.SearchType = Me.GetActiveItemType(Selection)
            fsForm.SearchPath = Me.GetAssemblyPath(Selection)
            fsForm.AssemblyID = AssemblyID
            fsForm.ClassID = ClassID
            fsForm.UseCloseChar = CloseChar
            fsForm.FillList()
            fsForm.ShowDialog(owner)
        End Sub

        Private Function CalculateLocation(ByVal Selection As EnvDTE.TextSelection) As System.Drawing.Point
            ' Calculate the cursor position according to the document window
            ' Take care of scrolled content
            Dim LineHeightInPixel As Integer = 21
            Dim CharWidthInPixel As Integer = 9

            Dim DocumentLineNumberAtWindowTop As Integer = _
                Selection.ActivePoint.Line - Selection.TextPane.StartPoint.Line

            ' I don't understand why but 1 unit scrollbar movement, moves the LineCharOffset as 12 units
            ' Because of this, the begining value start with 1 and continue with addition 12 units
            ' It means;
            '   1 unit scroll = 1 + 12 units LineCharOffset Result
            '   2 units scroll = 1 + (12 * 2) units LineCharOffset Result
            '   Formula: 1 + (12 * n)

            ' Scroll Step = Character Step
            ' 6 = 10
            ' 7 = 12
            ' 8 = 14
            ' 9 = 15
            ' 10 = 17
            ' 12 = 20
            ' 18 = 30
            ' 24 = 40
            ' According to the above values I found 1.666 constant
            Dim SSCSC As Double = 1.666

            Dim DocumentLineCharOffsetAtWindowLeft As Integer = _
                Selection.ActivePoint.LineCharOffset - CType(Math.Ceiling(((Selection.TextPane.StartPoint.LineCharOffset - 1) \ 12) * SSCSC), Integer)

            Dim CursorLocationSizeFromTextPaneTop As Integer = DocumentLineNumberAtWindowTop * LineHeightInPixel
            Dim CursorLocationSizeFromTextPaneLeft As Integer = DocumentLineCharOffsetAtWindowLeft * CharWidthInPixel
            ' !---

            ' Prepare MainWindow Position on Screen
            Dim mWLeft As Integer = Selection.DTE.MainWindow.Left
            Dim mWTop As Integer = Selection.DTE.MainWindow.Top

            ' if window maximized, do not care the positions
            If Selection.DTE.MainWindow.WindowState = vsWindowState.vsWindowStateMaximize Then mWLeft = 0 : mWTop = 0
            ' !---

            ' Get the buggy ActiveWindows positions and do Left position calculation
            Dim dWLeft As Integer = Selection.DTE.ActiveWindow.Left
            Dim LeftCalculationLimit As Integer = dWLeft - mWLeft

            Dim CalculatedTop As Integer = 0, CalculatedLeft As Integer = 0

            ' Calculate the Top position of VS Document Window
            ' Initial value is 49 for window head border and also 30 for tab label of document window
            ' Add 25 to locate the intellisence under the line
            CalculatedTop = 49 + 30 + 25

            Dim commandBars As Microsoft.VisualStudio.CommandBars.CommandBars = _
                CType(Selection.DTE.CommandBars, Microsoft.VisualStudio.CommandBars.CommandBars)

            For Each cB As Microsoft.VisualStudio.CommandBars.CommandBar In commandBars
                If (cB.Position = MsoBarPosition.msoBarMenuBar OrElse cB.Position = MsoBarPosition.msoBarTop) AndAlso cB.Enabled AndAlso cB.Visible Then CalculatedTop += cB.Height
            Next
            ' !---

            ' Calculate the Left position of VS Document Window
            ' Just blind add 49 pixel for left located autohidden toolbar tabs
            ' and 55 pixel for line numbers, breakpoint and indicator columns
            CalculatedLeft = 49 + 55

            Dim LinkedWindows As EnvDTE.LinkedWindows = _
                Selection.DTE.MainWindow.LinkedWindows

            For Each tB As EnvDTE.Window In LinkedWindows
                If tB.Type <> vsWindowType.vsWindowTypeDocument AndAlso tB.Visible AndAlso _
                    tB.Left > 0 AndAlso (tB.Left - mWLeft) <= LeftCalculationLimit Then CalculatedLeft += tB.Width
            Next
            ' !---

            Dim xLoc As Integer = mWLeft + CalculatedLeft + CursorLocationSizeFromTextPaneLeft
            Dim yLoc As Integer = mWTop + CalculatedTop + CursorLocationSizeFromTextPaneTop

            Return New System.Drawing.Point(xLoc, yLoc)
        End Function

        Private Sub BackSpaceForEmptyText(ByVal Selection As EnvDTE.TextSelection)
            Dim editPoint As EnvDTE.EditPoint = _
                Selection.ActivePoint.CreateEditPoint()

            editPoint.CharLeft(1)
            editPoint.Delete(1)
        End Sub

        Private Sub EnteredTextChange(ByRef owner As IWin32Window, ByVal CurrentText As String, ByVal BeginningOffset As Integer, ByVal Selection As EnvDTE.TextSelection, ByVal RaiseIntelliSense As Boolean)
            Dim activePoint As EnvDTE.VirtualPoint = _
                Selection.ActivePoint
            Dim editPoint As EnvDTE.EditPoint = _
                activePoint.CreateEditPoint()

            editPoint.MoveToLineAndOffset(activePoint.Line, BeginningOffset)
            editPoint.Delete(activePoint)
            editPoint.Insert(CurrentText)

            If RaiseIntelliSense Then
                editPoint.CharLeft()

                Me.event_TDAfterKeyPressed(owner, editPoint.GetText(1), Selection, False)
            End If
        End Sub

        Public Delegate Sub HandleResultDelegate(ByRef owner As IWin32Window, ByVal IsAccepted As Boolean, ByVal BeginningOffset As Integer, ByVal Selection As EnvDTE.TextSelection, ByVal ISType As Globals.ISTypes, ByVal AcceptChar As Char, ByVal CloseChar As Char, ByVal Params As Object())

        Private Sub HandleResult(ByRef owner As IWin32Window, ByVal IsAccepted As Boolean, ByVal BeginningOffset As Integer, ByVal Selection As EnvDTE.TextSelection, ByVal ISType As Globals.ISTypes, ByVal AcceptChar As Char, ByVal CloseChar As Char, ByVal Params As Object())
            If IsAccepted Then
                Dim CloseChar_s As String

                If CloseChar = Nothing OrElse _
                    CloseChar = Chr(0) Then

                    CloseChar_s = String.Empty
                Else
                    CloseChar_s = CType(CloseChar, String)
                End If

                Dim AcceptChar_s As String

                If AcceptChar = Nothing OrElse _
                    AcceptChar = Chr(0) Then

                    AcceptChar_s = String.Empty
                Else
                    AcceptChar_s = CType(AcceptChar, String)
                End If

                Dim cursorPoint As EnvDTE.VirtualPoint = _
                    Selection.ActivePoint
                Dim editPoint As EnvDTE.EditPoint = _
                    cursorPoint.CreateEditPoint()

                editPoint.MoveToLineAndOffset(cursorPoint.Line, BeginningOffset)

                Select Case ISType
                    Case Globals.ISTypes.ControlSearch
                        Dim ControlType As Globals.ControlTypes = _
                            CType(Params(0), Globals.ControlTypes)
                        Dim ControlID As String = _
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
                        Dim TemplateID As String = _
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(TemplateID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", TemplateID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                    Case Globals.ISTypes.TranslationSearch
                        Dim TranslationID As String = _
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(TranslationID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", TranslationID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                    Case Globals.ISTypes.AssemblySearch
                        Dim AssemblyID As String = _
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(AssemblyID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", AssemblyID, AcceptChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                            Me.ShowClassSearchFor(owner, AssemblyID, CloseChar, Selection)
                        End If

                    Case Globals.ISTypes.ClassSearch
                        Dim AssemblyID As String = _
                            CType(Params(0), String)
                        Dim ClassID As String = _
                            CType(Params(1), String)

                        If Not String.IsNullOrEmpty(AssemblyID) AndAlso Not String.IsNullOrEmpty(ClassID) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", ClassID, AcceptChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                            Me.ShowFunctionSearchFor(owner, AssemblyID, ClassID, CloseChar, Selection)
                        End If

                    Case Globals.ISTypes.FunctionSearch
                        Dim FunctionID As String = _
                            CType(Params(0), String)
                        Dim FunctionParameters As String() = _
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
                        Dim ControlTypeString As String = _
                            CType(Params(0), String)

                        If Not String.IsNullOrEmpty(ControlTypeString) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", ControlTypeString, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                    Case Globals.ISTypes.OnFlyRequest
                        Dim ControlID As String = _
                            CType(Params(0), String)

                        editPoint.ReplaceText(cursorPoint, String.Format("{{}}:{0}{1}", ControlID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                        ' Locate Cursor Between Brackets
                        Dim StringLength As Integer = String.Format("}}:{0}{1}", ControlID, CloseChar_s).Length
                        cursorPoint.Parent.Selection.MoveToLineAndOffset(cursorPoint.Line, cursorPoint.LineCharOffset - StringLength)

                    Case Globals.ISTypes.PrimitiveStatement
                        Dim ControlID As String = _
                            CType(Params(0), String)

                        editPoint.ReplaceText(cursorPoint, String.Format("{{}}:{0}{1}", ControlID, CloseChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)

                        ' Locate Cursor Between Brackets
                        Dim StringLength As Integer = String.Format("}}:{0}{1}", ControlID, CloseChar_s).Length
                        cursorPoint.Parent.Selection.MoveToLineAndOffset(cursorPoint.Line, cursorPoint.LineCharOffset - StringLength)

                    Case Globals.ISTypes.SpecialPropertySearch
                        Dim SpecialPropertyType As Globals.SpecialPropertyTypes = _
                            CType(Params(0), Globals.SpecialPropertyTypes)
                        Dim SpecialPropertyID As String = _
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
                        End Select

                    Case Globals.ISTypes.ControlSearchForParenting
                        Dim ControlIDString As String = _
                            CType(Params(1), String)

                        If Not String.IsNullOrEmpty(ControlIDString) Then
                            editPoint.ReplaceText(cursorPoint, String.Format("{0}{1}", ControlIDString, AcceptChar_s), EnvDTE.vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers)
                        End If

                End Select
            End If
        End Sub

        Private Function CheckIsXeoraCubeProject(ByVal Selection As EnvDTE.TextSelection) As Boolean
            Dim rBoolean As Boolean = False

            Dim ActiveProject As EnvDTE.Project = _
                Selection.DTE.ActiveDocument.ProjectItem.ContainingProject

            Return Me.CheckProjectFolders(ActiveProject.ProjectItems)
        End Function

        Private Function CheckProjectFolders(ByVal pItems As EnvDTE.ProjectItems) As Boolean
            Dim rBoolean As Boolean = False

            Dim AddonsPath As String = String.Empty
            Dim PublicContentsPath As String = String.Empty
            Dim TranslationsPath As String = String.Empty
            Dim TemplatesPath As String = String.Empty

            For iC As Integer = 1 To pItems.Count
                If String.Compare(pItems.Item(iC).Name, "Addons") = 0 Then
                    PublicContentsPath = pItems.Item(iC).FileNames(0)
                ElseIf String.Compare(pItems.Item(iC).Name, "PublicContents") = 0 Then
                    PublicContentsPath = pItems.Item(iC).FileNames(0)
                ElseIf String.Compare(pItems.Item(iC).Name, "Translations") = 0 Then
                    TranslationsPath = pItems.Item(iC).FileNames(0)
                ElseIf String.Compare(pItems.Item(iC).Name, "Templates") = 0 Then
                    TemplatesPath = pItems.Item(iC).FileNames(0)
                Else
                    If ( _
                        String.IsNullOrEmpty(PublicContentsPath) OrElse _
                        String.IsNullOrEmpty(TranslationsPath) OrElse _
                        String.IsNullOrEmpty(TemplatesPath) _
                        ) AndAlso _
                        Not pItems.Item(iC).ProjectItems Is Nothing AndAlso _
                        pItems.Item(iC).ProjectItems.Count > 0 Then

                        rBoolean = Me.CheckProjectFolders(pItems.Item(iC).ProjectItems)
                    End If
                End If

                If rBoolean Then Exit For
            Next

            If Not rBoolean AndAlso Not String.IsNullOrEmpty(PublicContentsPath) AndAlso _
                Not String.IsNullOrEmpty(TranslationsPath) AndAlso _
                Not String.IsNullOrEmpty(TemplatesPath) Then

                rBoolean = True

                Dim FileNames As String() = IO.Directory.GetFiles(TranslationsPath, "*.xml")

                For Each fileName As String In FileNames
                    If Not IO.Directory.Exists( _
                        IO.Path.Combine(PublicContentsPath, IO.Path.GetFileNameWithoutExtension(fileName))) Then

                        rBoolean = False

                        Exit For
                    End If
                Next

                If rBoolean Then
                    FileNames = IO.Directory.GetFiles(TemplatesPath, "*.xml")

                    For Each fileName As String In FileNames
                        If String.Compare(IO.Path.GetFileNameWithoutExtension(fileName), "Configuration") <> 0 AndAlso _
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
            Dim sT As AddInLoader.SearchTypes = AddInLoader.SearchTypes.Theme

            Dim ActiveItemPath As String = _
                Selection.DTE.ActiveDocument.Path

            Dim pC As Integer = 4
            Dim dI As IO.DirectoryInfo = _
                New IO.DirectoryInfo(ActiveItemPath)
            Do
                dI = IO.Directory.GetParent(dI.FullName)

                pC -= 1
            Loop Until pC = 0

            If String.Compare(dI.Name, "Addons", True) = 0 Then _
                sT = AddInLoader.SearchTypes.Addon

            Return sT
        End Function

        Private Function GetTemplatePath(ByVal Selection As EnvDTE.TextSelection) As String
            Dim ActiveItemPath As String = _
                Selection.DTE.ActiveDocument.Path

            Return ActiveItemPath
        End Function

        Private Function GetTranslationPath(ByVal Selection As EnvDTE.TextSelection) As String
            Dim ActiveItemPath As String = _
                Selection.DTE.ActiveDocument.Path

            Return IO.Path.GetFullPath( _
                        IO.Path.Combine(ActiveItemPath, "../Translations") _
                    )
        End Function

        Private Function GetAssemblyPath(ByVal Selection As EnvDTE.TextSelection) As String
            Dim ActiveItemPath As String = _
                Selection.DTE.ActiveDocument.Path

            Return IO.Path.GetFullPath( _
                        IO.Path.Combine(ActiveItemPath, "../../Dlls") _
                    )
        End Function

        Public ReadOnly Property Handle As IntPtr Implements IWin32Window.Handle
            Get
                Return New System.IntPtr(Me._applicationObject.MainWindow.HWnd)
            End Get
        End Property

    End Class
End Namespace
