Option Strict On

Namespace Xeora.Web.Shared.Service
    Public Interface IScheduledTasks
        Delegate Sub ScheduleTaskHandler(ByVal params As Object())

        Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As Date) As String
        Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As TimeSpan) As String
        Sub UnRegisterScheduleTask(ByVal ScheduleID As String)

        Function PingToRemoteEndPoint() As Boolean
    End Interface
End Namespace