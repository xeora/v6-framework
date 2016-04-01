Option Strict On

Namespace Xeora.Web.Controller
    Public MustInherit Class ControllerBase
        Private _Parent As ControllerBase
        Private _Children As ControllerCollection
        Private _DraftStartIndex As Integer
        Private _DraftValue As String
        Private _InsideValue As String
        Private _ControllerType As ControllerTypes
        Private _ContentArguments As [Global].ArgumentInfo.ArgumentInfoCollection

        Private _UpdateBlockRendered As Boolean
        Private _UpdateBlockControlID As String
        Private _MessageResult As [Shared].ControlResult.Message

        Private _BoundControlRenderWaiting As Boolean
        Private _MultiCastDelegate As [Delegate]

        Protected _IsRendered As Boolean = False
        Protected _RenderedValue As String = String.Empty

        Public Enum ControllerTypes
            [Property]
            Directive
            Renderless
        End Enum

        Private Delegate Sub RenderCompletedDelegate(ByRef SenderController As ControllerBase)

        Public MustOverride Sub Render(ByRef SenderController As ControllerBase)

        Protected Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ControllerType As ControllerTypes, ByVal ContentArguments As [Global].ArgumentInfo.ArgumentInfoCollection)
            Me._Parent = Nothing
            Me._Children = New ControllerCollection(Me)
            Me._DraftStartIndex = DraftStartIndex
            Me._DraftValue = DraftValue
            Me._InsideValue = DraftValue

            Me._ControllerType = ControllerType
            Me._ContentArguments = New [Global].ArgumentInfo.ArgumentInfoCollection()

            ' Remove block signs, this value must not be null
            If Not String.IsNullOrEmpty(Me._InsideValue) AndAlso
                Me._InsideValue.Length > 2 AndAlso
                Me._InsideValue.Chars(0) = "$" AndAlso
                Me._InsideValue.Chars(Me._InsideValue.Length - 1) = "$" Then

                Me._InsideValue = Me._InsideValue.Substring(1, Me._InsideValue.Length - 2)
                Me._InsideValue = Me._InsideValue.Trim()
            End If
            ' !--

            If Not ContentArguments Is Nothing Then _
                Me._ContentArguments = ContentArguments
        End Sub

        Public ReadOnly Property Parent() As ControllerBase
            Get
                Return Me._Parent
            End Get
        End Property

        Public ReadOnly Property Children() As ControllerCollection
            Get
                Return Me._Children
            End Get
        End Property

        Public ReadOnly Property DraftStartIndex() As Integer
            Get
                Return Me._DraftStartIndex
            End Get
        End Property

        Public ReadOnly Property DraftLength() As Integer
            Get
                Return Me._DraftValue.Length
            End Get
        End Property

        Public ReadOnly Property DraftEndIndex() As Integer
            Get
                Return (Me._DraftStartIndex + Me._DraftValue.Length)
            End Get
        End Property

        Public ReadOnly Property DraftValue() As String
            Get
                Return Me._DraftValue
            End Get
        End Property

        Public ReadOnly Property InsideValue() As String
            Get
                Return Me._InsideValue
            End Get
        End Property

        Public ReadOnly Property ControllerType() As ControllerTypes
            Get
                Return Me._ControllerType
            End Get
        End Property

        Public ReadOnly Property ContentArguments() As [Global].ArgumentInfo.ArgumentInfoCollection
            Get
                Return Me._ContentArguments
            End Get
        End Property

        ' This value should set at the top most Parent and read from it also
        Public Property UpdateBlockRendered() As Boolean
            Get
                Dim ControllerBase As ControllerBase = Me

                Do Until ControllerBase.Parent Is Nothing
                    ControllerBase = ControllerBase.Parent
                Loop

                Return ControllerBase._UpdateBlockRendered
            End Get
            Set(ByVal value As Boolean)
                Dim ControllerBase As ControllerBase = Me

                Do Until ControllerBase.Parent Is Nothing
                    ControllerBase = ControllerBase.Parent
                Loop

                ControllerBase._UpdateBlockRendered = value
            End Set
        End Property

        ' This value should set at the top most Parent and read from it also
        Public Property UpdateBlockControlID() As String
            Get
                Dim ControllerBase As ControllerBase = Me

                Do Until ControllerBase.Parent Is Nothing
                    ControllerBase = ControllerBase.Parent
                Loop

                Return ControllerBase._UpdateBlockControlID
            End Get
            Set(ByVal value As String)
                Dim ControllerBase As ControllerBase = Me

                Do Until ControllerBase.Parent Is Nothing
                    ControllerBase = ControllerBase.Parent
                Loop

                ControllerBase._UpdateBlockControlID = value
            End Set
        End Property

        ' This value should set at the top most Parent and read from it also
        Public Property MessageResult() As [Shared].ControlResult.Message
            Get
                Dim ControllerBase As ControllerBase = Me

                Do Until ControllerBase.Parent Is Nothing
                    ControllerBase = ControllerBase.Parent
                Loop

                Return ControllerBase._MessageResult
            End Get
            Set(ByVal value As [Shared].ControlResult.Message)
                Dim ControllerBase As ControllerBase = Me

                Do Until ControllerBase.Parent Is Nothing
                    ControllerBase = ControllerBase.Parent
                Loop

                ControllerBase._MessageResult = value
            End Set
        End Property

        Public ReadOnly Property IsUpdateBlockRequest() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me.UpdateBlockControlID)
            End Get
        End Property

        Public ReadOnly Property InRequestedUpdateBlock() As Boolean
            Get
                Dim rBoolean As Boolean =
                    String.IsNullOrEmpty(Me.UpdateBlockControlID)

                If Not rBoolean Then
                    Dim ControllerBase As ControllerBase = Me

                    Do Until ControllerBase.Parent Is Nothing
                        If TypeOf ControllerBase Is Directive.UpdateBlock Then
                            rBoolean = (String.Compare(CType(ControllerBase, Directive.UpdateBlock).ControlID, Me.UpdateBlockControlID) = 0)

                            Exit Do
                        End If

                        ControllerBase = ControllerBase.Parent
                    Loop
                End If

                Return rBoolean
            End Get
        End Property

        Public ReadOnly Property IsRendered() As Boolean
            Get
                Return Me._IsRendered
            End Get
        End Property

        Public ReadOnly Property RenderedValue() As String
            Get
                Return Me._RenderedValue
            End Get
        End Property

        Protected Function Create() As String
            ' If Controller is already rendered, then you don't need to render again, just turn the rendered value...
            If Me.IsRendered Then Return Me.RenderedValue

            Dim BoundControlRenderWaitingControls As New Generic.Dictionary(Of Integer, ControllerBase)
            Dim WorkingInsideValue As New Text.StringBuilder()

            ' this loop can no be for each because enumerator may be modified while it is rendering..
            For cC As Integer = 0 To Me.Children.Count - 1
                Dim Child As ControllerBase = Me.Children.Item(cC)

                Try
                    Child.Render(Me)

                    If Child.BoundControlRenderWaiting Then _
                        BoundControlRenderWaitingControls.Add(WorkingInsideValue.Length, Child)

                    If Me.IsUpdateBlockRequest Then
                        If Me.InRequestedUpdateBlock OrElse Me.UpdateBlockRendered Then _
                        WorkingInsideValue.Append(Child.RenderedValue)
                    Else
                        WorkingInsideValue.Append(Child.RenderedValue)
                    End If
                Catch ex As Exception.ReloadRequiredException
                    Throw
                Catch ex As System.Exception
                    If [Shared].Configurations.Debugging Then
                        Dim ExceptionString As String = Nothing

                        Do
                            ExceptionString =
                                String.Format(
                                    "<div align='left' style='border: solid 1px #660000; background-color: #FFFFFF'><div align='left' style='font-weight: bolder; color:#FFFFFF; background-color:#CC0000; padding: 4px;'>{0}</div><br><div align='left' style='padding: 4px'>{1}{2}</div></div>", ex.Message, ex.Source, IIf(Not ExceptionString Is Nothing, "<hr size='1px' />" & ExceptionString, Nothing))

                            ex = ex.InnerException
                        Loop Until ex Is Nothing

                        WorkingInsideValue.Append(ExceptionString)
                    End If
                End Try
            Next

            ' Complete Bound Render Waiting Controls Render
            Dim Enumerator As Generic.IEnumerator(Of Generic.KeyValuePair(Of Integer, ControllerBase)) =
                BoundControlRenderWaitingControls.GetEnumerator()

            Do While Enumerator.MoveNext()
                WorkingInsideValue.Insert(Enumerator.Current.Key, Enumerator.Current.Value.RenderedValue)
            Loop

            Return WorkingInsideValue.ToString()
        End Function

        Protected Sub DefineRenderedValue(ByVal value As String)
            If Me._IsRendered Then Exit Sub

            Me._RenderedValue = value
            Me._IsRendered = True

            Me.FireRenderCompleted()
        End Sub

        Private Sub FireRenderCompleted()
            Dim ControllerBase As ControllerBase = Me

            Do Until ControllerBase.Parent Is Nothing
                ControllerBase = ControllerBase.Parent
            Loop

            If Not ControllerBase._MultiCastDelegate Is Nothing Then _
                CType(ControllerBase._MultiCastDelegate, RenderCompletedDelegate).Invoke(Me)
        End Sub

        Protected ReadOnly Property BoundControlRenderWaiting() As Boolean
            Get
                Return Me._BoundControlRenderWaiting
            End Get
        End Property

        Protected Sub RegisterToRenderCompleted()
            If Me._BoundControlRenderWaiting Then Exit Sub
            Me._MultiCastDelegate = New RenderCompletedDelegate(AddressOf Me.Render)

            Dim ControllerBase As ControllerBase = Me

            Do Until ControllerBase.Parent Is Nothing
                ControllerBase = ControllerBase.Parent
            Loop

            If ControllerBase._MultiCastDelegate Is Nothing Then
                ControllerBase._MultiCastDelegate = CType(Me._MultiCastDelegate.Clone(), [Delegate])
            Else
                ControllerBase._MultiCastDelegate =
                    [Delegate].Combine(ControllerBase._MultiCastDelegate, Me._MultiCastDelegate)
            End If

            Me._BoundControlRenderWaiting = True
        End Sub

        Protected Sub UnRegisterFromRenderCompleted()
            If Not Me._BoundControlRenderWaiting Then Exit Sub

            Dim ControllerBase As ControllerBase = Me

            Do Until ControllerBase.Parent Is Nothing
                ControllerBase = ControllerBase.Parent
            Loop

            If Not ControllerBase._MultiCastDelegate Is Nothing Then
                ControllerBase._MultiCastDelegate =
                    [Delegate].Remove(ControllerBase._MultiCastDelegate, Me._MultiCastDelegate)
            End If

            Me._BoundControlRenderWaiting = False
            Me._MultiCastDelegate = Nothing
        End Sub

        Protected Shared Function ProvideDummyController(ByRef Parent As ControllerBase, ByVal ContentArguments As [Global].ArgumentInfo.ArgumentInfoCollection) As ControllerBase
            Dim DummyControllerContainer As New RenderlessController(0, String.Empty, ContentArguments)
            DummyControllerContainer._Parent = Parent

            Return DummyControllerContainer
        End Function

        Public Shared Function CaptureControllerType(ByVal DraftValue As String) As ControllerTypes
            Dim rControllerType As ControllerTypes = ControllerTypes.Renderless

            If Not String.IsNullOrEmpty(DraftValue) Then
                Dim CPIDMatch As Text.RegularExpressions.Match =
                    Text.RegularExpressions.Regex.Match(DraftValue, "\$((\w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?)|(\w+))\:")

                If CPIDMatch.Success Then
                    rControllerType = ControllerTypes.Directive
                Else
                    rControllerType = ControllerTypes.Property
                End If
            End If

            Return rControllerType
        End Function

        Public Class ControllerCollection
            Inherits Generic.List(Of ControllerBase)

            Private _ParentHolder As ControllerBase

            Public Sub New(ByRef Parent As ControllerBase)
                Me._ParentHolder = Parent
            End Sub

            Public Shadows Sub Add(ByVal Item As ControllerBase)
                Item._Parent = Me._ParentHolder

                MyBase.Add(Item)
            End Sub

            Public Shadows Sub AddRange(ByVal collection As ControllerCollection)
                For Each item As ControllerBase In collection
                    item._Parent = Me._ParentHolder
                Next

                MyBase.AddRange(collection)
            End Sub
        End Class
    End Class
End Namespace