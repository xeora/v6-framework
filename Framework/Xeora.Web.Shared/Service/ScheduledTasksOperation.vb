Option Strict On

Namespace Xeora.Web.Shared.Service
    Public NotInheritable Class ScheduledTasksOperation
        Private Shared _Cache As IScheduledTasks = Nothing

        Public Function RegisterTask(ByVal CallBack As IScheduledTasks.TaskHandler, ByVal CallBackParams As Object(), ByVal ExecutionTime As DateTime) As String
            Return ScheduledTasksOperation._Cache.RegisterTask(CallBack, CallBackParams, ExecutionTime)
        End Function

        Public Function RegisterTask(ByVal CallBack As IScheduledTasks.TaskHandler, ByVal CallBackParams As Object(), ByVal ExecutionTime As TimeSpan) As String
            Return ScheduledTasksOperation._Cache.RegisterTask(CallBack, CallBackParams, ExecutionTime)
        End Function

        Public Sub UnRegisterTask(ByVal ID As String)
            ScheduledTasksOperation._Cache.UnRegisterTask(ID)
        End Sub

        Public Shared ReadOnly Property Instance() As IScheduledTasks
            Get
                If ScheduledTasksOperation._Cache Is Nothing Then
                    Try
                        Dim ThemeAsm As Reflection.Assembly, objTheme As Type

                        ThemeAsm = Reflection.Assembly.Load("Xeora.Web")
                        objTheme = ThemeAsm.GetType("Xeora.Web.Site.Service.ScheduledTasks", False, True)

                        ScheduledTasksOperation._Cache = CType(Activator.CreateInstance(objTheme), IScheduledTasks)
                    Catch ex As Exception
                        Throw New Exception("Communication Error! Scheduled Tasks Service is not accessable...", ex)
                    End Try
                End If

                Return ScheduledTasksOperation._Cache
            End Get
        End Property
    End Class
End Namespace