Option Strict On

Namespace Xeora.Web.Shared.Service
    Public Interface IScheduledTasks
        Delegate Sub TaskHandler(ByVal params As Object())

        Overloads Function RegisterTask(ByVal ScheduleCallBack As TaskHandler, ByVal params As Object(), ByVal ExecutionTime As DateTime) As String
        Overloads Function RegisterTask(ByVal ScheduleCallBack As TaskHandler, ByVal params As Object(), ByVal ExecutionTime As TimeSpan) As String
        Sub UnRegisterTask(ByVal ID As String)

        Function PingToRemoteEndPoint() As Boolean
    End Interface
End Namespace