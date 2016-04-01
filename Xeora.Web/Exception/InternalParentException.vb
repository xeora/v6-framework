Option Strict On

Namespace Xeora.Web.Exception
    Public Class InternalParentException
        Inherits System.Exception

        Public Enum ChildDirectiveTypes
            Execution
            Control
        End Enum

        Public Sub New(ByVal ChildDirectiveType As ChildDirectiveTypes)
            MyBase.New(String.Format("Parented {0} must not be located inside its parent!", ChildDirectiveType.ToString()))
        End Sub
    End Class
End Namespace