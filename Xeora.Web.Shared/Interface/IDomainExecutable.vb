Option Strict On

Namespace Xeora.Web.Shared
    Public Interface IDomainExecutable
        ''' <summary>
        ''' Initialize Procedure will be called when Xeora Domain has been loaded.
        ''' </summary>
        Sub Initialize()

        ''' <summary>
        ''' PreExecute Procedure has been called right before the Xeora Executable Call
        ''' </summary>
        ''' <param name="ExecutionID">A Unique ID to tracked the Execution between PreExecute and PostExecute</param>
        ''' <param name="MI">The MethodInfo that will be called after PreExecute function. Modifications will effect the Executable Call</param>
        Sub PreExecute(ByVal ExecutionID As String, ByRef MI As Reflection.MethodInfo)

        ''' <summary>
        ''' PostExecute Procedure has been called right after the Xeora Executable Call
        ''' </summary>
        ''' <param name="ExecutionID">A Unique ID to tracked the Execution between PreExecute and PostExecute</param>
        ''' <param name="Result">The Result of the Xeora Executable Call (If any, otherwise null). Result will be changable</param>
        Sub PostExecute(ByVal ExecutionID As String, ByRef Result As Object)

        ''' <summary>
        ''' Finalize Procedure has been called right before the Xeora Domain unload.
        ''' Unload can happen in two ways. One is domain will reach the memory limit and 
        ''' try to do a garbage collection and executable will be unloaded, and the
        ''' second way is IIS is stopping and Xeora Framework will be unloaded.
        ''' </summary>
        Sub Finalize()

        ''' <summary>
        ''' URLResolver is called when URLMapping is active and defined resolutions would not
        ''' reach any success about resolving the request file path. If you have any custom
        ''' resolution for request file path, do it in this function and return the result.
        ''' </summary>
        ''' <param name="RequestFilePath">Requested File Path comes right after Application Root</param>
        ''' <returns>Return ResolvedMapped to proceed</returns>
        Function URLResolver(ByVal RequestFilePath As String) As URLMapping.ResolvedMapped
    End Interface
End Namespace