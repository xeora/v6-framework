Option Strict On

Namespace Xeora.Web.Shared.Service
    Public NotInheritable Class ScheduledTasksOperation
        Private Shared _Cache As IScheduledTasks = Nothing

        Public Function RegisterScheduleTask(ByVal ScheduleCallBack As IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As Date) As String
            Return ScheduledTasksOperation._Cache.RegisterScheduleTask(ScheduleCallBack, params, TaskExecutionTime)
        End Function

        Public Function RegisterScheduleTask(ByVal ScheduleCallBack As IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As TimeSpan) As String
            Return ScheduledTasksOperation._Cache.RegisterScheduleTask(ScheduleCallBack, params, TaskExecutionTime)
        End Function

        Public Sub UnRegisterScheduleTask(ByVal ScheduleID As String)
            ScheduledTasksOperation._Cache.UnRegisterScheduleTask(ScheduleID)
        End Sub

        Public Shared ReadOnly Property Instance() As IScheduledTasks
            Get
                If ScheduledTasksOperation._Cache Is Nothing Then
                    Try
                        Dim ThemeAsm As Reflection.Assembly, objTheme As Type

                        ThemeAsm = Reflection.Assembly.Load("Xeora.Web")
                        objTheme = ThemeAsm.GetType("Xeora.Web.Service.ScheduledTasks", False, True)

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