Option Strict On
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive.Control
    Public Class LinkButton
        Inherits ControlBase
        Implements IHasText
        Implements IHasURL

        Private _Text As String
        Private _URL As String

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControlTypes.LinkButton, ContentArguments)

            Me._Text = String.Empty
            Me._URL = String.Empty
        End Sub

        Public Property URL() As String Implements IHasURL.URL
            Get
                Return Me._URL
            End Get
            Set(ByVal Value As String)
                Me._URL = Value
                If String.IsNullOrEmpty(Me._URL) Then Me._URL = String.Empty
            End Set
        End Property

        Public Property Text() As String Implements IHasText.Text
            Get
                Return Me._Text
            End Get
            Set(ByVal Value As String)
                Me._Text = Value
                If String.IsNullOrEmpty(Me._Text) Then Me._Text = String.Empty
            End Set
        End Property

        Public Overrides Sub Clone(ByRef Control As IControl)
            Control = New LinkButton(Me.DraftStartIndex, Me.DraftValue, Me.ContentArguments)
            MyBase.Clone(Control)

            With CType(Control, LinkButton)
                ._Text = Me._Text
                ._URL = Me._URL
            End With
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            If Not String.IsNullOrEmpty(Me.BoundControlID) Then
                If Me.IsRendered Then Exit Sub

                If Not Me.BoundControlRenderWaiting Then
                    Dim Controller As ControllerBase = Me

                    Do Until Controller.Parent Is Nothing
                        If TypeOf Controller.Parent Is ControllerBase AndAlso
                            TypeOf Controller.Parent Is INamable Then

                            If String.Compare(
                                CType(Controller.Parent, INamable).ControlID, Me.BoundControlID, True) = 0 Then

                                Throw New Exception.InternalParentException(Exception.InternalParentException.ChildDirectiveTypes.Control)
                            End If
                        End If

                        Controller = Controller.Parent
                    Loop

                    Me.RegisterToRenderCompletedOf(Me.BoundControlID)
                End If

                If TypeOf SenderController Is ControlBase AndAlso
                    TypeOf SenderController Is INamable Then

                    If String.Compare(
                        CType(SenderController, INamable).ControlID, Me.BoundControlID, True) <> 0 Then

                        Exit Sub
                    Else
                        Me.RenderInternal()
                    End If
                End If
            Else
                Me.RenderInternal()
            End If
        End Sub

        Private Sub RenderInternal()
            ' LinkButton Control does not have any ContentArguments, That's why it copies it's parent Arguments
            If Not Me.Parent Is Nothing Then _
                Me.ContentArguments.Replace(Me.Parent.ContentArguments)

            ' href attribute is disabled always, use url attribute instand of...
            Me.Attributes.Remove("href")
            Me.Attributes.Remove("value")

            ' Render Text Content
            Dim DummyControllerContainer As ControllerBase =
                ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
            Me.RequestParse(Me.Text, DummyControllerContainer)
            DummyControllerContainer.Render(Me)
            Me.Text = DummyControllerContainer.RenderedValue
            ' !--

            If Not Me.URL Is Nothing AndAlso
                Me.URL.Trim().Length > 0 Then

                If Me.URL.IndexOf("~/") = 0 Then
                    Me.URL = Me.URL.Remove(0, 2)
                    Me.URL = Me.URL.Insert(0, Configurations.ApplicationRoot.BrowserImplementation)
                ElseIf Me.URL.IndexOf("¨/") = 0 Then
                    Me.URL = Me.URL.Remove(0, 2)
                    Me.URL = Me.URL.Insert(0, Configurations.VirtualRoot)
                End If

                ' Render Text Content
                DummyControllerContainer = ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
                Me.RequestParse(Me.URL, DummyControllerContainer)
                DummyControllerContainer.Render(Me)
                Me.URL = DummyControllerContainer.RenderedValue
                ' !--
            End If

            If Not Me.BindInfo Is Nothing Then
                Me.URL = "#_action0"

                If Me.SecurityInfo.Disabled.IsSet AndAlso
                    Me.SecurityInfo.Disabled.Type = SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                    Me.Text = Me.SecurityInfo.Disabled.Value
                End If
            Else
                If Me.URL Is Nothing OrElse
                    (Not Me.URL Is Nothing AndAlso Me.URL.Trim().Length = 0) Then

                    Me.URL = "#_action1"
                End If
            End If

            ' Render Bind Parameters
            Me.RenderBindInfoParams()

            ' Check for Parent UpdateBlock
            Dim WorkingControl As ControllerBase = Me.Parent
            Dim ParentUpdateBlockID As String = String.Empty

            Do Until WorkingControl Is Nothing
                If TypeOf WorkingControl Is UpdateBlock Then
                    ParentUpdateBlockID = CType(WorkingControl, UpdateBlock).ControlID

                    Exit Do
                End If

                WorkingControl = WorkingControl.Parent
            Loop
            ' !--

            ' Define OnClick Server event for Button
            If Not Me.BindInfo Is Nothing Then
                If Not String.IsNullOrEmpty(ParentUpdateBlockID) OrElse
                    Me.BlockIDsToUpdate.Count > 0 Then

                    If Not Me.BlockIDsToUpdate.Contains(ParentUpdateBlockID) AndAlso
                        Me.UpdateLocalBlock Then

                        Me.BlockIDsToUpdate.Add(ParentUpdateBlockID)
                    End If

                    If Not String.IsNullOrEmpty(Me.Attributes.Item("onclick")) Then
                        Me.Attributes.Item("onclick") = String.Format("javascript:var eO=false;try{{{2}}}catch(ex){{eO=true}};if(!eO){{__XeoraJS.doRequest('{0}', '{1}')}};", String.Join(",", Me.BlockIDsToUpdate.ToArray()), Manager.Assembly.EncodeFunction(Helpers.HashCode, Me.BindInfo.ToString()), Me.Attributes.Item("onclick"))
                    Else
                        Me.Attributes.Item("onclick") = String.Format("javascript:__XeoraJS.doRequest('{0}', '{1}');", String.Join(",", Me.BlockIDsToUpdate.ToArray()), Manager.Assembly.EncodeFunction(Helpers.HashCode, Me.BindInfo.ToString()))
                    End If
                Else
                    If Not String.IsNullOrEmpty(Me.Attributes.Item("onclick")) Then
                        Me.Attributes.Item("onclick") = String.Format("javascript:var eO=false;try{{{1}}}catch(ex){{eO=true}};if(!eO){{__XeoraJS.postForm('{0}')}};", Manager.Assembly.EncodeFunction(Helpers.HashCode, Me.BindInfo.ToString()), Me.Attributes.Item("onclick"))
                    Else
                        Me.Attributes.Item("onclick") = String.Format("javascript:__XeoraJS.postForm('{0}');", Manager.Assembly.EncodeFunction(Helpers.HashCode, Me.BindInfo.ToString()))
                    End If
                End If
            End If
            ' !--

            ' Render Attributes
            For aC As Integer = Me.Attributes.Count - 1 To 0 Step -1
                Dim Item As AttributeInfo = Me.Attributes.Item(aC)

                DummyControllerContainer = ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
                Me.RequestParse(Item.Value, DummyControllerContainer)
                DummyControllerContainer.Render(Me)

                Me.Attributes.Item(aC) = New AttributeInfo(Item.Key, DummyControllerContainer.RenderedValue)
            Next
            ' !--

            If Me.SecurityInfo.Disabled.IsSet AndAlso
                Me.SecurityInfo.Disabled.Type = SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                Me.DefineRenderedValue(Me.SecurityInfo.Disabled.Value)
            Else
                Me.DefineRenderedValue(
                    String.Format(
                        "<a href=""{3}"" name=""{0}"" id=""{0}""{1}>{2}</a>",
                        Me.ControlID,
                        Attributes.ToString(),
                        Me.Text,
                        Me.URL
                    )
                )
            End If

            Me.UnRegisterFromRenderCompletedOf(Me.BoundControlID)
        End Sub
    End Class
End Namespace