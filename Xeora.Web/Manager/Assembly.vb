Option Strict On

Imports Xeora.Web.Global

Namespace Xeora.Web.Manager
    Public Class [Assembly]
        Public Enum ExecuterTypes
            Control
            Other
            Undefined
        End Enum

        Public Shared Function EncodeFunction(ByVal EncodingHashCode As String, ByVal BindFunctionForEncoding As String) As String
            Dim SplitEF As String() =
                BindFunctionForEncoding.Split(","c)

            ' First Part of Encoded Bind Function
            '   [AssemblyName]?[ClassName].[FunctionName]
            Dim DecodedBF01 As String = SplitEF(0)
            ' Second Part of Encoded Bind Function (Parameters)
            '   [Parameter]|[Parameter]|...
            Dim IsDecodedBF02Exist As Boolean = False
            Dim DecodedBF02 As String = String.Empty

            If SplitEF.Length = 2 Then DecodedBF02 = SplitEF(1) : IsDecodedBF02Exist = True

            ' FP will be Encoded with Base64
            '   Base64 contains encrypted Data
            '   EncData = XOR HashCode applied on to deflated compression
            Dim EncodedCF01 As String = String.Empty
            ' Parameters will be Encoded with Base64
            Dim EncodedCF02 As String = String.Empty

            If Not String.IsNullOrEmpty(DecodedBF02) Then
                EncodedCF02 = Convert.ToBase64String(
                                    Text.Encoding.UTF8.GetBytes(DecodedBF02)
                                )
            End If

            Dim GZipHelperStream As IO.MemoryStream = Nothing
            Dim GZipStream As IO.Compression.GZipStream = Nothing
            Try
                Dim rbuffer As Byte() = Text.Encoding.UTF8.GetBytes(DecodedBF01)

                GZipHelperStream = New IO.MemoryStream

                Try
                    GZipStream = New IO.Compression.GZipStream(GZipHelperStream, IO.Compression.CompressionMode.Compress, True)

                    GZipStream.Write(rbuffer, 0, rbuffer.Length)
                    GZipStream.Flush()
                Finally
                    If Not GZipStream Is Nothing Then GZipStream.Close()
                End Try

                GZipHelperStream.Seek(0, IO.SeekOrigin.Begin)

                ' EncData Dec. Process
                Dim ByteCoded As Byte, bC As Integer = 0
                Do Until GZipHelperStream.Position = GZipHelperStream.Length
                    ByteCoded = CType(GZipHelperStream.ReadByte(), Byte)

                    ByteCoded = ByteCoded Xor CType(
                                                    Asc(EncodingHashCode.Chars(bC Mod EncodingHashCode.Length)),
                                                    Byte
                                                )

                    GZipHelperStream.Seek(-1, IO.SeekOrigin.Current)
                    GZipHelperStream.WriteByte(ByteCoded)

                    bC += 1
                Loop
                ' !--

                rbuffer = CType(Array.CreateInstance(GetType(Byte), GZipHelperStream.Length), Byte())

                GZipHelperStream.Seek(0, IO.SeekOrigin.Begin)
                GZipHelperStream.Read(rbuffer, 0, rbuffer.Length)

                EncodedCF01 = Convert.ToBase64String(rbuffer)
            Finally
                If Not GZipHelperStream Is Nothing Then
                    GZipHelperStream.Close()

                    GC.SuppressFinalize(GZipHelperStream)
                End If
            End Try

            Dim rEncodedString As String =
                CType(
                    IIf(
                        IsDecodedBF02Exist,
                        String.Format("{0},{1},{2}", EncodingHashCode, System.Web.HttpUtility.UrlEncode(EncodedCF01), System.Web.HttpUtility.UrlEncode(EncodedCF02)),
                        String.Format("{0},{1}", EncodingHashCode, System.Web.HttpUtility.UrlEncode(EncodedCF01))
                    ),
                    String
                )

            Return rEncodedString
        End Function

        Public Shared Function DecodeFunction(ByVal EncodedBindFunction As String) As String
            Dim SplitEF As String() =
                EncodedBindFunction.Split(","c)

            Dim DecodedBF01 As String = String.Empty, DecodedBF02 As String
            Dim EncodingHashCode As String = SplitEF(0)
            ' First Part of Encoded Bind Function
            '   [AssemblyName]?[ClassName].[FunctionName]
            ' FP is Encoded with Base64
            '   Base64 contains encrypted Data
            '   EncData = XOR HashCode applied on to deflated compression
            Dim EncodedBF01 As String = SplitEF(1)
            If Not EncodedBF01.Contains("+") AndAlso
                EncodedBF01.Contains("%") Then EncodedBF01 = System.Web.HttpUtility.UrlDecode(EncodedBF01)
            ' Second Part of Encoded Call Function (Parameters)
            '   [Parameter]|[Parameter]|...
            ' Parameters are Encoded with Base64
            Dim EncodedBF02 As String = String.Empty
            If SplitEF.Length = 3 Then
                EncodedBF02 = SplitEF(2)

                If Not EncodedBF02.Contains("+") AndAlso
                    EncodedBF02.Contains("%") Then EncodedBF02 = System.Web.HttpUtility.UrlDecode(EncodedBF02)
            End If

            Dim EncodedText As New Text.StringBuilder

            Dim bC As Integer, buffer As Byte() =
                Convert.FromBase64String(EncodedBF01)

            ' EncData to DecData Process
            For bC = 0 To buffer.Length - 1
                buffer(bC) = buffer(bC) Xor CType(
                                                    Asc(EncodingHashCode.Chars(bC Mod EncodingHashCode.Length)),
                                                    Byte
                                                )
            Next
            ' !--

            Dim GZipHelperStream As IO.MemoryStream = Nothing
            Dim GZipStream As IO.Compression.GZipStream = Nothing

            Try
                Dim rbuffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 512), Byte())

                ' Prepare Content Stream
                GZipHelperStream = New IO.MemoryStream()
                GZipHelperStream.Write(buffer, 0, buffer.Length)
                GZipHelperStream.Flush()
                GZipHelperStream.Seek(0, IO.SeekOrigin.Begin)
                ' !--

                GZipStream = New IO.Compression.GZipStream(GZipHelperStream, IO.Compression.CompressionMode.Decompress, True)

                Do
                    bC = GZipStream.Read(rbuffer, 0, rbuffer.Length)

                    If bC > 0 Then EncodedText.Append(
                                        Text.Encoding.UTF8.GetString(rbuffer, 0, bC)
                                    )
                Loop While bC > 0
            Finally
                If Not GZipStream Is Nothing Then GZipStream.Close()

                If Not GZipHelperStream Is Nothing Then
                    GZipHelperStream.Close()

                    GC.SuppressFinalize(GZipHelperStream)
                End If
            End Try

            DecodedBF01 = EncodedText.ToString()

            ' Decode The Parameters Part
            Dim rDecodedString As String
            If Not String.IsNullOrEmpty(EncodedBF02) Then
                DecodedBF02 = Text.Encoding.UTF8.GetString(
                                    Convert.FromBase64String(EncodedBF02)
                                )

                rDecodedString = String.Format("{0},{1}", DecodedBF01, DecodedBF02)
            Else
                rDecodedString = String.Format("{0}", DecodedBF01)
            End If

            Return rDecodedString
        End Function

        ' This function is for external call out side of the project DO NOT DISABLE IT
        Public Overloads Shared Function InvokeBind(ByVal BindInfo As [Shared].Execution.BindInfo) As [Shared].Execution.BindInvokeResult
            Return [Assembly].InvokeBind(BindInfo, ExecuterTypes.Undefined)
        End Function

        Private Shared _ExecutableLibrary As New Concurrent.ConcurrentDictionary(Of String, Loader)
        Private Shared _PrivateBinPath As String = Nothing
        Public Overloads Shared Function InvokeBind(ByVal BindInfo As [Shared].Execution.BindInfo, ByVal ExecuterType As ExecuterTypes) As [Shared].Execution.BindInvokeResult
            If BindInfo Is Nothing Then Throw New NoNullAllowedException("Requires bind!")
            ' Check if BindInfo Parameters has been parsed!
            If Not BindInfo.IsReady Then Throw New System.Exception("Bind Parameters shoud be parsed first!")

            Dim rBindInvokeResult As [Shared].Execution.BindInvokeResult =
                New [Shared].Execution.BindInvokeResult(BindInfo)

            Dim ApplicationPath As String =
                IO.Path.Combine(
                    [Shared].Configurations.TemporaryRoot,
                    String.Format("{0}{2}{1}",
                        [Shared].Configurations.WorkingPath.WorkingPathID,
                        [Shared].Helpers.Context.Content.Item("ApplicationID"),
                        IO.Path.DirectorySeparatorChar
                    )
                )

            Dim AssemblyKey As String =
                String.Format("KEY-{0}_{1}", ApplicationPath, BindInfo.ExecutableName)

            Dim ExecutableLoader As Loader = Nothing

            If Assembly._ExecutableLibrary.ContainsKey(AssemblyKey) Then
                ExecutableLoader = Assembly._ExecutableLibrary.Item(AssemblyKey)
            Else
                ExecutableLoader = New Loader(ApplicationPath, BindInfo.ExecutableName)

                If ExecutableLoader.MissingFileException Then
                    rBindInvokeResult.InvokeResult = Nothing
                    rBindInvokeResult.ReloadRequired = True
                    rBindInvokeResult.ApplicationPath = ApplicationPath

                    GoTo QUICKEXIT
                End If

                Assembly._ExecutableLibrary.TryAdd(AssemblyKey, ExecutableLoader)
            End If

            Try
                ' Invoke must use the same appdomain because of the context sync price
                Dim InvokedObject As Object =
                    ExecutableLoader.Invoke(BindInfo.RequestHttpMethod, BindInfo.ClassNames, BindInfo.ProcedureName, BindInfo.ProcedureParamValues, BindInfo.InstanceExecution, ExecuterType.ToString())

                If TypeOf InvokedObject Is System.Exception Then
                    rBindInvokeResult.InvokeResult = CType(InvokedObject, System.Exception)
                Else
                    rBindInvokeResult.InvokeResult = InvokedObject
                End If
            Catch ex As System.Exception
                rBindInvokeResult.InvokeResult = ex
            End Try

            If Not rBindInvokeResult.InvokeResult Is Nothing AndAlso
                TypeOf rBindInvokeResult.InvokeResult Is System.Exception Then

                Helper.EventLogger.Log(
                    CType(rBindInvokeResult.InvokeResult, System.Exception))
            End If
QUICKEXIT:
            Return rBindInvokeResult
        End Function

        Public Shared Function ExecuteStatement(ByVal DomainIDAccessTree As String(), ByVal StatementBlockID As String, ByVal Statement As String, ByVal NoCache As Boolean) As Object
            Dim rMethodResult As Object = Nothing

            Dim objAssembly As Reflection.Assembly

            Try
                Dim BlockKey As String = String.Format("BLOCKCALL_{0}_{1}", String.Join(Of String)("_", DomainIDAccessTree), StatementBlockID.Replace("."c, "_"c))

                objAssembly = [Assembly].PrepareInLineStatementExecutable(BlockKey, Statement, NoCache)

                If objAssembly Is Nothing Then _
                    Throw New Exception.GrammerException()

                Dim AssemblyObject As Type =
                    objAssembly.CreateInstance(String.Format("InLineStatement.{0}", BlockKey)).GetType()
                Dim MethodObject As Reflection.MethodInfo =
                    AssemblyObject.GetMethod("Execute")

                rMethodResult = MethodObject.Invoke(AssemblyObject,
                                                    Reflection.BindingFlags.DeclaredOnly Or
                                                    Reflection.BindingFlags.InvokeMethod,
                                                    Nothing, Nothing, Nothing)
            Catch ex As System.Exception
                rMethodResult = ex
            End Try

            Return rMethodResult
        End Function

        Public Shared Sub ClearCache()
            If Not Assembly._ExecutableLibrary Is Nothing Then
                For Each Key As String In Assembly._ExecutableLibrary.Keys
                    Assembly._ExecutableLibrary.Item(Key).Dispose()
                Next

                Assembly._ExecutableLibrary.Clear()
            End If
            If Not Assembly._InLineStatementExecutables Is Nothing Then Assembly._InLineStatementExecutables.Clear()
        End Sub

        Private Shared _InLineStatementExecutables As New Hashtable
        Private Shared Function PrepareInLineStatementExecutable(ByVal BlockKey As String, ByVal Statement As String, ByVal NoCache As Boolean) As Reflection.Assembly
            Dim rAssembly As Reflection.Assembly = Nothing

            If NoCache AndAlso Assembly._InLineStatementExecutables.ContainsKey(BlockKey) Then _
                Assembly._InLineStatementExecutables.Remove(BlockKey)

            If Not Statement Is Nothing Then
                If Assembly._InLineStatementExecutables.ContainsKey(BlockKey) Then
                    rAssembly = CType(Assembly._InLineStatementExecutables.Item(BlockKey), Reflection.Assembly)
                Else
                    Dim CodeBlock As New Text.StringBuilder()

                    CodeBlock.AppendLine("using System;")
                    CodeBlock.AppendLine("using System.Data;")
                    CodeBlock.AppendLine("using System.Xml;")
                    CodeBlock.AppendLine("namespace InLineStatement")
                    CodeBlock.AppendLine("{")
                    CodeBlock.AppendFormat("public class {0}", BlockKey)
                    CodeBlock.AppendLine("{")
                    CodeBlock.AppendLine("public static object Execute()")
                    CodeBlock.AppendLine("{")
                    CodeBlock.AppendFormat("{0}", Statement)
                    CodeBlock.AppendLine("} // method")
                    CodeBlock.AppendLine("} // class")
                    CodeBlock.AppendLine("} // namespace")

                    Dim CSharpProvider As New Microsoft.CSharp.CSharpCodeProvider()

                    Dim objCompiler As CodeDom.Compiler.CodeDomProvider =
                        CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp")

                    Dim CompilerParams As New CodeDom.Compiler.CompilerParameters()

                    CompilerParams.GenerateExecutable = False
                    CompilerParams.GenerateInMemory = True
                    CompilerParams.IncludeDebugInformation = False

                    Dim CurrentDomainAssemblies As Reflection.Assembly() =
                        AppDomain.CurrentDomain.GetAssemblies()

                    For Each Assembly As Reflection.Assembly In CurrentDomainAssemblies
                        CompilerParams.ReferencedAssemblies.Add(Assembly.Location)
                    Next

                    Dim CompilerResults As CodeDom.Compiler.CompilerResults

                    CompilerResults = objCompiler.CompileAssemblyFromSource(CompilerParams, CodeBlock.ToString())

                    If CompilerResults.Errors.HasErrors Then
                        Dim sB As New Text.StringBuilder()

                        For Each cRE As CodeDom.Compiler.CompilerError In CompilerResults.Errors
                            sB.AppendLine(cRE.ErrorText)
                        Next

                        Throw New System.Exception(sB.ToString())
                    End If

                    rAssembly = CompilerResults.CompiledAssembly

                    If Not Assembly._InLineStatementExecutables.ContainsKey(BlockKey) Then Assembly._InLineStatementExecutables.Add(BlockKey, rAssembly)
                End If
            End If

            Return rAssembly
        End Function

    End Class
End Namespace
