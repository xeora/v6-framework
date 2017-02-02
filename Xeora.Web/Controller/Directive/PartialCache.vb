Option Strict On

Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class PartialCache
        Inherits DirectiveControllerBase
        Implements IParsingRequires
        Implements IInstanceRequires

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested
        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.PartialCache, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            '' Check for Parent UpdateBlock
            'Dim WorkingControl As ControllerBase = Me.Parent

            'Do Until WorkingControl Is Nothing
            '    If TypeOf WorkingControl Is UpdateBlock Then _
            '        Throw New Exception.RequestBlockException()

            '    WorkingControl = WorkingControl.Parent
            'Loop
            '' !--

            Dim matchMI As Text.RegularExpressions.Match =
                Text.RegularExpressions.Regex.Match(Me.InsideValue, "PC~\d+\:\{")

            If matchMI.Success Then
                If String.Compare(matchMI.Value.Split("~"c)(0), "PC") = 0 Then
                    ' Cache Handling will be here!
                    Dim Instance As IDomain = Nothing
                    RaiseEvent InstanceRequested(Instance)

                    Dim UniqueCacheID As String = [Object].ProvideUniqueCacheID(Me, Instance)

                    If Me.Objects.ContainsKey(UniqueCacheID) Then
                        Me.DefineRenderedValue(CType(Me.Objects.Item(UniqueCacheID), [Object]).Content)
                    Else
                        Dim controlValueSplitted As String() =
                            Me.InsideValue.Split(":"c)
                        Dim BlockContent As String =
                            String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 2)

                        If Not BlockContent Is Nothing AndAlso
                            BlockContent.Trim().Length >= 2 Then

                            BlockContent = BlockContent.Substring(1, BlockContent.Length - 2)

                            RaiseEvent ParseRequested(BlockContent, Me)

                            Threading.Monitor.Enter(Me.Objects.SyncRoot)
                            Try
                                If Not Me.Objects.ContainsKey(UniqueCacheID) Then
                                    Dim CacheObject As [Object] =
                                        New [Object](UniqueCacheID, Me.Create())

                                    Me.Objects.Item(UniqueCacheID) = CacheObject
                                End If
                            Finally
                                Threading.Monitor.Exit(Me.Objects.SyncRoot)
                            End Try

                            Me.DefineRenderedValue(CType(Me.Objects.Item(UniqueCacheID), [Object]).Content)
                        End If
                    End If
                Else ' Standart Value
                    Throw New Exception.DirectivePointerException()
                End If
            Else
                Throw New Exception.UnknownDirectiveException()
            End If
        End Sub

        Private Shared _Objects As Hashtable = Nothing
        Private ReadOnly Property Objects As Hashtable
            Get
                If PartialCache._Objects Is Nothing Then _
                    PartialCache._Objects = Hashtable.Synchronized(New Hashtable())

                Return PartialCache._Objects
            End Get
        End Property

        Public Shared Sub ClearCache()
            PartialCache._Objects = Nothing
        End Sub

        Private Class [Object]
            Private _UniqueCacheID As String

            Private _Content As String
            Private _Date As Date

            Public Sub New(ByVal UniqueCacheID As String, ByVal Content As String)
                Me._UniqueCacheID = UniqueCacheID
                Me._Content = Content
                Me._Date = Date.Now
            End Sub

            Public Shared Function ProvideUniqueCacheID(ByVal PartialCache As PartialCache, ByVal Instance As IDomain) As String
                If PartialCache Is Nothing Then Throw New NullReferenceException("PartialCache Parameter must not be null!")

                Dim DomainIDAccessTreeString As String = String.Join(Of String)("\", Helpers.CurrentDomainIDAccessTree)
                Dim ServiceFullPath As String = String.Empty

                Dim WorkingObject As ControllerBase =
                    PartialCache.Parent

                Do
                    If TypeOf WorkingObject Is Template Then
                        ServiceFullPath = CType(WorkingObject, Template).ControlID

                        Exit Do
                    End If

                    WorkingObject = WorkingObject.Parent
                Loop Until WorkingObject Is Nothing

                Dim PositionID As Integer = -1
                Dim matchMI As Text.RegularExpressions.Match =
                    Text.RegularExpressions.Regex.Match(PartialCache.InsideValue, "PC~(?<PositionID>\d+)\:\{")

                If matchMI.Success Then Integer.TryParse(matchMI.Groups.Item("PositionID").Value, PositionID)

                If String.IsNullOrEmpty(Instance.Language.ID) OrElse String.IsNullOrEmpty(ServiceFullPath) OrElse PositionID = -1 Then _
                    Throw New Exception.ParseException()

                Return String.Format("{0}_{1}_{2}_{3}", DomainIDAccessTreeString, Instance.Language.ID, ServiceFullPath, PositionID)
            End Function

            Public ReadOnly Property UniqueCacheID As String
                Get
                    Return Me._UniqueCacheID
                End Get
            End Property

            Public ReadOnly Property Content As String
                Get
                    Return Me._Content
                End Get
            End Property

            Public ReadOnly Property [Date] As Date
                Get
                    Return Me._Date
                End Get
            End Property
        End Class
    End Class
End Namespace