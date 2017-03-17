Option Strict On

Namespace Xeora.Web.Shared

    Public Class MimeType
        Private Enum MimeLookups
            Type
            Extention
        End Enum

        Private Shared Function ResolveMime(ByVal MimeLookup As MimeLookups, ByVal SearchValue As String) As String
            Dim rString As String = String.Empty

            Select Case MimeLookup
                Case MimeLookups.Type
                    rString = "application/octet-stream"
                Case MimeLookups.Extention
                    rString = ".dat"
            End Select

            If String.IsNullOrEmpty(SearchValue) Then Return rString

            Try
                Dim HelperAsm As Reflection.Assembly, objHelper As Type
                Dim HelperInstance As Object = Nothing

                HelperAsm = Reflection.Assembly.Load("Xeora.Web")
                objHelper = HelperAsm.GetType("Xeora.Web.Helper.Registry")

                For Each _ctorInfo As Reflection.ConstructorInfo In objHelper.GetConstructors()
                    If _ctorInfo.IsConstructor AndAlso
                        _ctorInfo.IsPublic AndAlso
                        _ctorInfo.GetParameters().Length = 1 Then

                        HelperInstance = _ctorInfo.Invoke(New Object() {0})

                        Exit For
                    End If
                Next

                If Not HelperInstance Is Nothing Then
                    Dim AccessPathProp As Reflection.PropertyInfo =
                        HelperInstance.GetType().GetProperty("AccessPath", GetType(String))

                    Dim Result As Object = Nothing
                    Select Case MimeLookup
                        Case MimeLookups.Type
                            AccessPathProp.SetValue(HelperInstance, SearchValue, Nothing)
                            Result = HelperInstance.GetType().GetMethod("GetRegistryValue").Invoke(HelperInstance, New Object() {CType("Content Type", Object)})

                            If Result Is Nothing Then rString = "application/octet-stream" Else rString = CType(Result, String)
                        Case MimeLookups.Extention
                            AccessPathProp.SetValue(HelperInstance, String.Format("Mime\Database\Content Type\{0}", SearchValue), Nothing)
                            Result = HelperInstance.GetType().GetMethod("GetRegistryValue").Invoke(HelperInstance, New Object() {CType("Extension", Object)})

                            If Result Is Nothing Then rString = ".dat" Else rString = CType(Result, String)
                    End Select
                End If
            Catch ex As Exception
                ' Do Nothing Just Handle Exception
            End Try

            Return rString
        End Function

        Public Shared Function GetMime(ByVal FileExtension As String) As String
            If Not String.IsNullOrEmpty(FileExtension) AndAlso
                Not FileExtension.StartsWith(".") Then FileExtension = String.Format(".{0}", FileExtension)

            Return MimeType.ResolveMime(MimeLookups.Type, FileExtension)
        End Function

        Public Shared Function GetExtension(ByVal MimeType As String) As String
            Return [Shared].MimeType.ResolveMime(MimeLookups.Extention, MimeType)
        End Function
    End Class

End Namespace