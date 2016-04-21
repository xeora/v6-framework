Namespace Xeora.VSAddIn.Executable
    Public Interface ILoader
        Function FrameworkArchitecture(ByVal FrameworkBinLocation As String) As Reflection.ProcessorArchitecture
        Function GetAssemblies(ByVal SearchPath As String) As String()
        Function GetAssemblyClasses(ByVal AssemblyFileLocation As String, Optional ByVal ClassIDs As String() = Nothing) As String()
        Function GetAssemblyClassFunctions(ByVal AssemblyFileLocation As String, ByVal ClassIDs As String()) As Object()
    End Interface
End Namespace