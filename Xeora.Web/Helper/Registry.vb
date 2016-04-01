Option Strict On

Imports Microsoft

Namespace Xeora.Web.Helper
    Public Class Registry
        Private _regSection As Win32.RegistryKey
        Private _RegistryArea As RegistryAreas
        Private _AccessPath As String

        Public Enum RegistryAreas
            ClassesRoot
            CurrentConfig
            CurrentUser
            LocalMachine
            Users
        End Enum

        Public Sub New(ByVal RegistryArea As RegistryAreas)
            Me.New(RegistryArea, Nothing)
        End Sub

        Public Sub New(ByVal RegistryArea As RegistryAreas, ByVal AccessPath As String)
            Me._RegistryArea = RegistryArea
            Me._AccessPath = AccessPath
        End Sub

        Public Property AccessPath() As String
            Get
                Return Me._AccessPath
            End Get
            Set(ByVal Value As String)
                Me._AccessPath = Value
            End Set
        End Property

        Public Property RegistryArea() As RegistryAreas
            Get
                Return Me._RegistryArea
            End Get
            Set(ByVal Value As RegistryAreas)
                Me._RegistryArea = Value
            End Set
        End Property

        Private Sub OpenRegistryArea()
            If Me._AccessPath Is Nothing Then Throw New NoNullAllowedException("Access Path must not be null!")

            Select Case Me._RegistryArea
                Case RegistryAreas.ClassesRoot
                    Try
                        Me._regSection = Win32.Registry.ClassesRoot
                    Catch ex As system.Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.CurrentConfig
                    Try
                        Me._regSection = Win32.Registry.CurrentConfig
                    Catch ex As system.Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.CurrentUser
                    Try
                        Me._regSection = Win32.Registry.CurrentUser
                    Catch ex As System.Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.LocalMachine
                    Try
                        Me._regSection = Win32.Registry.LocalMachine
                    Catch ex As System.Exception
                        EventLogging.WriteToLog(ex)
                    End Try
                Case RegistryAreas.Users
                    Try
                        Me._regSection = Win32.Registry.Users
                    Catch ex As System.Exception
                        EventLogging.WriteToLog(ex)
                    End Try
            End Select
        End Sub

        Private Sub CloseRegistryArea()
            Try
                Me._regSection.Close()
            Catch ex As System.Exception
                EventLogging.WriteToLog(ex)
            End Try
        End Sub

        Public Function CheckIsRegistryExists() As Boolean
            Dim rBoolean As Boolean

            Try
                Me.OpenRegistryArea()

                rBoolean = (Not Me._regSection.OpenSubKey(Me._AccessPath) Is Nothing)
            Catch ex As System.Exception
                rBoolean = False

                EventLogging.WriteToLog(ex)
            Finally
                Me.CloseRegistryArea()
            End Try

            Return rBoolean
        End Function

        Public Function CheckIsRegistryValueExists(ByVal Key As String) As Boolean
            Dim rBoolean As Boolean

            If Me.CheckIsRegistryExists() Then
                Try
                    Me.OpenRegistryArea()

                    rBoolean = (Not Me._regSection.OpenSubKey(Me._AccessPath).GetValue(Key) Is Nothing)
                Catch ex As System.Exception
                    rBoolean = False

                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            Else
                rBoolean = False
            End If

            Return rBoolean
        End Function

        Public Function GetRegistryKeyNames() As String()
            Dim rStringList As String() = Nothing

            If Me.CheckIsRegistryValueExists(Me._AccessPath) Then
                Try
                    Me.OpenRegistryArea()

                    rStringList = Me._regSection.OpenSubKey(Me._AccessPath).GetSubKeyNames()
                Catch ex As System.Exception
                    rStringList = Nothing

                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If

            Return rStringList
        End Function

        Public Function GetRegistryValue(ByVal Key As String) As String
            Dim rString As String = Nothing

            If Me.CheckIsRegistryValueExists(Key) Then
                Try
                    Me.OpenRegistryArea()

                    rString = CType(Me._regSection.OpenSubKey(Me._AccessPath).GetValue(Key), String)
                Catch ex As System.Exception
                    rString = Nothing

                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If

            Return rString
        End Function

        Public Sub SetRegistryValue(ByVal Key As String, ByVal Value As String)
            If Not Me.CheckIsRegistryExists() Then
                Try
                    Me.OpenRegistryArea()

                    Me._regSection.CreateSubKey(Me._AccessPath)
                Catch ex As System.Exception
                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If

            Try
                Me.OpenRegistryArea()

                Me._regSection.OpenSubKey(Me._AccessPath, True).SetValue(Key, Value)
            Catch ex As System.Exception
                EventLogging.WriteToLog(ex)
            Finally
                Me.CloseRegistryArea()
            End Try
        End Sub

        Public Sub DeleteRegistry()
            If Me.CheckIsRegistryExists() Then
                Try
                    Me.OpenRegistryArea()

                    Me._regSection.DeleteSubKey(Me._AccessPath)
                Catch ex As System.Exception
                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If
        End Sub

        Public Sub DeleteRegistryValue(ByVal Key As String)
            If Me.CheckIsRegistryValueExists(Key) Then
                Try
                    Me.OpenRegistryArea()

                    Me._regSection.OpenSubKey(Me._AccessPath, True).DeleteValue(Key)
                Catch ex As System.Exception
                    EventLogging.WriteToLog(ex)
                Finally
                    Me.CloseRegistryArea()
                End Try
            End If
        End Sub
    End Class
End Namespace