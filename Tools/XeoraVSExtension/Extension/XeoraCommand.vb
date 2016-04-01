Imports System
Imports System.ComponentModel.Design
Imports Microsoft.VisualStudio.Shell
Imports EnvDTE
Imports EnvDTE80

''' <summary>
''' Command handler
''' </summary>
Public NotInheritable Class XeoraCommand

    ''' <summary>
    ''' Command ID.
    ''' </summary>
    Public Const CommandId As Integer = 256

    ''' <summary>
    ''' Command menu group (command set GUID).
    ''' </summary>
    Public Shared ReadOnly CommandSet As New Guid("4c16f99f-8402-4b02-be4e-88a3cfdbbb55")

    ''' <summary>
    ''' VS Package that provides this command, not null.
    ''' </summary>
    Private ReadOnly package As Package

    Private _applicationObject As DTE = Nothing
    Private _addInControl As Xeora.VSAddIn.AddInControl

    Private WithEvents _textDocumentKeyPressEvents As EnvDTE80.TextDocumentKeyPressEvents

    ''' <summary>
    ''' Initializes a new instance of the <see cref="XeoraCommand"/> class.
    ''' Adds our command handlers for menu (the commands must exist in the command table file)
    ''' </summary>
    ''' <param name="package">Owner package, not null.</param>
    Private Sub New(package As Package)
        If package Is Nothing Then Throw New ArgumentNullException("package")
        Me.package = package

        Dim commandService As OleMenuCommandService =
            CType(Me.ServiceProvider.GetService(GetType(IMenuCommandService)), OleMenuCommandService)
        If Not commandService Is Nothing Then
            commandService.AddCommand(
                New MenuCommand(
                    AddressOf Me.GoToReferanceCallback,
                    New CommandID(CommandSet, CommandId)
                )
            )
        End If

        If Me._applicationObject Is Nothing Then
            Me._applicationObject = CType(Me.ServiceProvider.GetService(GetType(DTE)), DTE)

            Dim appEvents As EnvDTE80.Events2 = CType(Me._applicationObject.Events, Events2)
            Me._textDocumentKeyPressEvents = CType(appEvents.TextDocumentKeyPressEvents(Nothing), EnvDTE80.TextDocumentKeyPressEvents)

            Me._addInControl = New Xeora.VSAddIn.AddInControl(Me._applicationObject)

            AddHandler Me._textDocumentKeyPressEvents.AfterKeyPress, New _dispTextDocumentKeyPressEvents_AfterKeyPressEventHandler(AddressOf Me._addInControl.event_TDAfterKeyPressed)

            Xeora.VSAddIn.AddInLoaderHelper.CreateAppDomain()
        End If
    End Sub

    ''' <summary>
    ''' Gets the instance of the command.
    ''' </summary>
    Public Shared Property Instance As XeoraCommand

    ''' <summary>
    ''' Get service provider from the owner package.
    ''' </summary>
    Private ReadOnly Property ServiceProvider As IServiceProvider
        Get
            Return Me.package
        End Get
    End Property

    ''' <summary>
    ''' Initializes the singleton instance of the command.
    ''' </summary>
    ''' <param name="package">Owner package, Not null.</param>
    Public Shared Sub Initialize(package As Package)
        Instance = New XeoraCommand(package)
    End Sub

    ''' <summary>
    ''' This function is the callback used to execute the command when the menu item is clicked.
    ''' See the constructor to see how the menu item is associated with this function using
    ''' OleMenuCommandService service and MenuCommand class.
    ''' </summary>
    ''' <param name="sender">Event sender.</param>
    ''' <param name="e">Event args.</param>
    Private Sub GoToReferanceCallback(sender As Object, e As EventArgs)
        Me._addInControl.GotoControlReferance(Nothing, False, False)
    End Sub
End Class
