﻿Option Strict On
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive.Control
    Public Class Button
        Inherits ControlBase
        Implements IHasText
        Implements IUpdateBlocks

        Private _Text As String
        Private _BlockIDsToUpdate As Generic.List(Of String)
        Private _UpdateLocalBlock As Boolean

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ContentArguments)

            Me._Text = String.Empty
            Me._BlockIDsToUpdate = New Generic.List(Of String)
            Me._UpdateLocalBlock = True
        End Sub

        Public Property Text() As String Implements IHasText.Text
            Get
                Return Me._Text
            End Get
            Set(ByVal Value As String)
                Me._Text = Value
                If String.IsNullOrEmpty(Me._Text) Then Me._Text = String.Empty
            End Set
        End Property

        Public Property UpdateLocalBlock() As Boolean Implements IUpdateBlocks.UpdateLocalBlock
            Get
                Return Me._UpdateLocalBlock
            End Get
            Set(ByVal Value As Boolean)
                Me._UpdateLocalBlock = Value
            End Set
        End Property

        Public ReadOnly Property BlockIDsToUpdate() As Generic.List(Of String) Implements IUpdateBlocks.BlockIDsToUpdate
            Get
                Return Me._BlockIDsToUpdate
            End Get
        End Property

        Public Overrides Sub Clone(ByRef Control As IControl)
            Control = New Button(Me.DraftStartIndex, Me.DraftValue, Me.ContentArguments)
            MyBase.Clone(Control)

            With CType(Control, Button)
                ._Text = Me._Text
                ._BlockIDsToUpdate.AddRange(Me._BlockIDsToUpdate.ToArray())
                ._UpdateLocalBlock = Me._UpdateLocalBlock
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
            ' Button Control does not have any ContentArguments, That's why it copies it's parent Arguments
            If Not Me.Parent Is Nothing Then _
                Me.ContentArguments.Replace(Me.Parent.ContentArguments)

            ' Remove value attribute for Button Because it has Text Property
            Me.Attributes.Remove("value")

            ' Render Text Content
            Dim DummyControllerContainer As ControllerBase =
                ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
            Me.RequestParse(Me.Text, DummyControllerContainer)
            DummyControllerContainer.Render(Me)
            Me.Text = DummyControllerContainer.RenderedValue
            ' !--

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
                        Me.Attributes.Item("onclick") = String.Format("javascript:var eO=false;try{{{2}}}catch(ex){{eO=true}};if(!eO){{__XeoraJS.update('{0}', '{1}')}};", String.Join(",", Me.BlockIDsToUpdate.ToArray()), Manager.Assembly.EncodeFunction(Helpers.Context.Request.HashCode, Me.BindInfo.ToString()), Me.Attributes.Item("onclick"))
                    Else
                        Me.Attributes.Item("onclick") = String.Format("javascript:__XeoraJS.update('{0}', '{1}');", String.Join(",", Me.BlockIDsToUpdate.ToArray()), Manager.Assembly.EncodeFunction(Helpers.Context.Request.HashCode, Me.BindInfo.ToString()))
                    End If
                Else
                    If Not String.IsNullOrEmpty(Me.Attributes.Item("onclick")) Then
                        Me.Attributes.Item("onclick") = String.Format("javascript:var eO=false;try{{{1}}}catch(ex){{eO=true}};if(!eO){{__XeoraJS.post('{0}')}};", Manager.Assembly.EncodeFunction(Helpers.Context.Request.HashCode, Me.BindInfo.ToString()), Me.Attributes.Item("onclick"))
                    Else
                        Me.Attributes.Item("onclick") = String.Format("javascript:__XeoraJS.post('{0}');", Manager.Assembly.EncodeFunction(Helpers.Context.Request.HashCode, Me.BindInfo.ToString()))
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

            If Me.Security.Disabled.IsSet AndAlso
                Me.Security.Disabled.Type = SecurityInfo.DisabledClass.DisabledTypes.Dynamic Then

                Me.DefineRenderedValue(Me.Security.Disabled.Value)
            Else
                Me.DefineRenderedValue(
                    String.Format(
                        "<input type=""button"" name=""{0}"" id=""{0}""{1}{2}>",
                        Me.ControlID,
                        IIf(Not Me.Text Is Nothing, String.Format(" value=""{0}""", Me.Text), String.Empty),
                        Attributes.ToString()
                    )
                )
            End If

            Me.UnRegisterFromRenderCompletedOf(Me.BoundControlID)
        End Sub
    End Class
End Namespace