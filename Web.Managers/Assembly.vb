Option Strict On

Namespace SolidDevelopment.Web.Managers
    Public Class [Assembly]
        Public Shared Sub PrepareArguments(ByVal ArgumentInfos As Globals.ArgumentInfo.ArgumentInfoCollection, ByVal ArgumentNameList As String(), ByRef ArgumentValueList As Object())
            ' Fix Possible Null Post for ArgumentValueList
            ArgumentValueList = New Object() {}

            If Not ArgumentNameList Is Nothing Then
                ' Fix Null Arguments Variable
                If ArgumentInfos Is Nothing Then ArgumentInfos = New Globals.ArgumentInfo.ArgumentInfoCollection

                Dim rArgumentValueList As New ArrayList

                For Each ArgumentName As String In ArgumentNameList
                    If Not ArgumentName Is Nothing AndAlso _
                        ArgumentName.Trim().Length > 0 Then

                        Select Case ArgumentName.Chars(0)
                            Case "^"c ' QueryString
                                Dim QueryValue As String = _
                                    SolidDevelopment.Web.General.Context.Request.QueryString.Item(ArgumentName.Substring(1))

                                Select Case SolidDevelopment.Web.Configurations.RequestTagFiltering
                                    Case PGlobals.RequestTagFilteringTypes.OnlyQuery, PGlobals.RequestTagFilteringTypes.Both
                                        Dim ArgumentNameForCompare As String = _
                                            ArgumentName.Substring(1).ToLower(New Globalization.CultureInfo("en-US"))

                                        If Array.IndexOf(SolidDevelopment.Web.Configurations.RequestTagFilteringExceptions, ArgumentNameForCompare) = -1 Then _
                                            QueryValue = Assembly.CleanHTMLTags(QueryValue, SolidDevelopment.Web.Configurations.RequestTagFilteringItems)
                                End Select

                                rArgumentValueList.Add(CObj(QueryValue))
                            Case "~"c ' Form Post
                                ' File Post is not supporting on XML Http Requests
                                Dim RequestFilesKeys As String() = _
                                    SolidDevelopment.Web.General.Context.Request.Files.AllKeys
                                Dim RequestFileObjects As New System.Collections.Generic.List(Of System.Web.HttpPostedFile)

                                For kC As Integer = 0 To RequestFilesKeys.Length - 1
                                    If String.Compare(RequestFilesKeys(kC), ArgumentName.Substring(1), True) = 0 Then
                                        RequestFileObjects.Add( _
                                                SolidDevelopment.Web.General.Context.Request.Files.Item(kC) _
                                            )
                                    End If
                                Next
                                ' !--

                                If RequestFileObjects.Count = 0 Then
                                    If String.Compare(General.Context.Request.HttpMethod, "POST", True) = 0 Then
                                        Dim ControlSIM As Hashtable = _
                                            CType( _
                                                SolidDevelopment.Web.General.GetVariable("_sys_ControlSIM"),  _
                                                Hashtable _
                                            )

                                        If Not ControlSIM Is Nothing AndAlso _
                                            ControlSIM.ContainsKey(ArgumentName.Substring(1)) Then

                                            rArgumentValueList.Add( _
                                                ControlSIM.Item(ArgumentName.Substring(1)) _
                                            )
                                        Else
                                            Dim FormValue As String = _
                                                SolidDevelopment.Web.General.Context.Request.Form.Item(ArgumentName.Substring(1))

                                            Select Case SolidDevelopment.Web.Configurations.RequestTagFiltering
                                                Case PGlobals.RequestTagFilteringTypes.OnlyForm, PGlobals.RequestTagFilteringTypes.Both
                                                    Dim ArgumentNameForCompare As String = _
                                                        ArgumentName.Substring(1).ToLower(New Globalization.CultureInfo("en-US"))

                                                    If Array.IndexOf(SolidDevelopment.Web.Configurations.RequestTagFilteringExceptions, ArgumentNameForCompare) = -1 Then _
                                                        FormValue = Assembly.CleanHTMLTags(FormValue, SolidDevelopment.Web.Configurations.RequestTagFilteringItems)
                                            End Select

                                            rArgumentValueList.Add(CObj(FormValue))
                                        End If
                                    Else
                                        rArgumentValueList.Add(Nothing)
                                    End If

                                Else
                                    Select Case RequestFileObjects.Count
                                        Case 1
                                            rArgumentValueList.Add( _
                                                    CObj(RequestFileObjects.Item(0)) _
                                                )
                                        Case Else
                                            rArgumentValueList.Add( _
                                                    RequestFileObjects.ToArray() _
                                                )
                                    End Select
                                End If
                            Case "-"c ' Session
                                rArgumentValueList.Add( _
                                        SolidDevelopment.Web.General.Context.Session.Contents.Item(ArgumentName.Substring(1)) _
                                    )
                            Case "+"c ' Cookie
                                If SolidDevelopment.Web.General.Context.Request.Cookies.Item(ArgumentName.Substring(1)) Is Nothing Then
                                    rArgumentValueList.Add(Nothing)
                                Else
                                    rArgumentValueList.Add( _
                                            CObj(SolidDevelopment.Web.General.Context.Request.Cookies.Item(ArgumentName.Substring(1)).Value) _
                                        )
                                End If
                            Case "="c ' Value String near '='
                                rArgumentValueList.Add( _
                                        CObj(ArgumentName.Substring(1)) _
                                    )
                            Case "#"c ' Data Field Value
                                Dim searchVariableCollection As Globals.ArgumentInfo.ArgumentInfoCollection = ArgumentInfos
                                Dim searchVariableName As String = ArgumentName

                                Do
                                    searchVariableName = searchVariableName.Substring(1)

                                    If searchVariableName.IndexOf("#") = 0 Then
                                        searchVariableCollection = searchVariableCollection.Parent
                                    Else
                                        Exit Do
                                    End If
                                Loop Until searchVariableCollection Is Nothing

                                If Not searchVariableCollection Is Nothing Then
                                    Dim argItem As Globals.ArgumentInfo = _
                                        searchVariableCollection.Item(searchVariableName)

                                    If Not argItem.Value Is Nothing AndAlso _
                                        Not argItem.Value.GetType() Is GetType(System.DBNull) Then

                                        rArgumentValueList.Add(argItem.Value)
                                    Else
                                        rArgumentValueList.Add(Nothing)
                                    End If
                                Else
                                    rArgumentValueList.Add(Nothing)
                                End If

                            Case "*"c ' Search in All orderby : [InData, DataField, Session, Form Post, QueryString, Cookie]
                                Dim searchArgName As String = ArgumentName.Substring(1)
                                Dim searchArgValue As String

                                ' Search InDatas
                                searchArgValue = CType(SolidDevelopment.Web.General.GetVariable(ArgumentName), String)

                                ' Search In DataFields
                                If String.IsNullOrEmpty(searchArgValue) Then searchArgValue = CType(ArgumentInfos.Item(searchArgName).Value, String)

                                ' Search In Session
                                If String.IsNullOrEmpty(searchArgValue) Then searchArgValue = CType(SolidDevelopment.Web.General.Context.Session.Contents.Item(searchArgName), String)

                                ' Search In Form Post [Do not support File Posts]
                                If String.IsNullOrEmpty(searchArgValue) AndAlso _
                                    String.Compare(General.Context.Request.HttpMethod, "POST", True) = 0 Then

                                    Dim ControlSIM As Hashtable = _
                                        CType( _
                                            SolidDevelopment.Web.General.GetVariable("_sys_ControlSIM"),  _
                                            Hashtable _
                                        )

                                    If Not ControlSIM Is Nothing AndAlso _
                                        ControlSIM.ContainsKey(searchArgName) Then

                                        searchArgValue = CType(ControlSIM.Item(searchArgName), String)
                                    Else
                                        searchArgValue = SolidDevelopment.Web.General.Context.Request.Form.Item(searchArgName)
                                    End If
                                Else
                                    searchArgValue = Nothing
                                End If

                                ' Search QueryString
                                If String.IsNullOrEmpty(searchArgValue) Then searchArgValue = SolidDevelopment.Web.General.Context.Request.QueryString.Item(searchArgName)

                                ' Cookie
                                If String.IsNullOrEmpty(searchArgValue) AndAlso _
                                    Not SolidDevelopment.Web.General.Context.Request.Cookies.Item(searchArgName) Is Nothing Then

                                    searchArgValue = SolidDevelopment.Web.General.Context.Request.Cookies.Item(searchArgName).Value
                                End If

                                rArgumentValueList.Add(CObj(searchArgValue))
                            Case Else ' Search in Values Set for Current Request Session
                                If ArgumentName.IndexOf("@"c) > 0 Then
                                    Dim ArgumentQueryObjectName As String = _
                                        ArgumentName.Substring(ArgumentName.IndexOf("@"c) + 1)

                                    Dim ArgumentQueryObject As Object = Nothing

                                    Select Case ArgumentQueryObjectName.Chars(0)
                                        Case "-"c
                                            ArgumentQueryObject = SolidDevelopment.Web.General.Context.Session.Contents.Item(ArgumentQueryObjectName.Substring(1))
                                        Case "#"c
                                            Dim searchContentInfo As Globals.ArgumentInfo.ArgumentInfoCollection = ArgumentInfos
                                            Dim searchVariableName As String = ArgumentQueryObjectName

                                            Do
                                                searchVariableName = searchVariableName.Substring(1)

                                                If searchVariableName.IndexOf("#") = 0 Then
                                                    searchContentInfo = searchContentInfo.Parent
                                                Else
                                                    Exit Do
                                                End If
                                            Loop Until searchContentInfo Is Nothing

                                            If Not searchContentInfo Is Nothing Then
                                                Dim argItem As Globals.ArgumentInfo = _
                                                    searchContentInfo.Item(searchVariableName)

                                                If Not argItem.Value Is Nothing Then ArgumentQueryObject = argItem.Value
                                            End If
                                        Case Else
                                            ArgumentQueryObject = SolidDevelopment.Web.General.GetVariable(ArgumentQueryObjectName)
                                    End Select

                                    If Not ArgumentQueryObject Is Nothing Then
                                        Dim ArgumentCallList As String() = ArgumentName.Substring(0, ArgumentName.IndexOf("@"c)).Split("."c)
                                        Dim ArgumentValue As Object

                                        Try
                                            For Each ArgumentCall As String In ArgumentCallList
                                                If Not ArgumentQueryObject Is Nothing Then
                                                    ArgumentQueryObject = ArgumentQueryObject.GetType().InvokeMember(ArgumentCall, Reflection.BindingFlags.GetProperty, Nothing, ArgumentQueryObject, Nothing)
                                                Else
                                                    Exit For
                                                End If
                                            Next

                                            ArgumentValue = ArgumentQueryObject
                                        Catch ex As Exception
                                            ArgumentValue = Nothing
                                        End Try

                                        rArgumentValueList.Add(CObj(ArgumentValue))
                                    Else
                                        rArgumentValueList.Add(Nothing)
                                    End If
                                Else
                                    rArgumentValueList.Add(SolidDevelopment.Web.General.GetVariable(ArgumentName))
                                End If
                        End Select
                    Else
                        rArgumentValueList.Add(Nothing)
                    End If
                Next

                ArgumentValueList = CType(rArgumentValueList.ToArray(GetType(Object)), Object())
            End If
        End Sub

        Public Shared Function EncodeCallFunction(ByVal EncodingHashCode As String, ByVal CallFunctionForEncoding As String) As String
            Dim SplitECF As String() = _
                CallFunctionForEncoding.Split(","c)

            ' First Part of Encoded Call Function
            '   [AssemblyName]?[ClassName].[FunctionName]
            Dim DecodedCF01 As String = SplitECF(0)
            ' Second Part of Encoded Call Function (Parameters)
            '   [Parameter]|[Parameter]|...
            Dim IsDecodedCF02Exist As Boolean = False
            Dim DecodedCF02 As String = String.Empty

            If SplitECF.Length = 2 Then DecodedCF02 = SplitECF(1) : IsDecodedCF02Exist = True

            ' FP will be Encoded with Base64
            '   Base64 contains encrypted Data
            '   EncData = XOR HashCode applied on to deflated compression
            Dim EncodedCF01 As String = String.Empty
            ' Parameters will be Encoded with Base64
            Dim EncodedCF02 As String = String.Empty

            If Not String.IsNullOrEmpty(DecodedCF02) Then
                EncodedCF02 = System.Convert.ToBase64String( _
                                    System.Text.Encoding.UTF8.GetBytes(DecodedCF02) _
                                )
            End If

            Dim GZipHelperStream As IO.MemoryStream = Nothing
            Dim GZipStream As IO.Compression.GZipStream = Nothing
            Try
                Dim rbuffer As Byte() = System.Text.Encoding.UTF8.GetBytes(DecodedCF01)

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

                    ByteCoded = ByteCoded Xor CType( _
                                                    Asc(EncodingHashCode.Chars(bC Mod EncodingHashCode.Length)), _
                                                    Byte _
                                                )

                    GZipHelperStream.Seek(-1, IO.SeekOrigin.Current)
                    GZipHelperStream.WriteByte(ByteCoded)

                    bC += 1
                Loop
                ' !--

                rbuffer = CType(Array.CreateInstance(GetType(Byte), GZipHelperStream.Length), Byte())

                GZipHelperStream.Seek(0, IO.SeekOrigin.Begin)
                GZipHelperStream.Read(rbuffer, 0, rbuffer.Length)

                EncodedCF01 = System.Convert.ToBase64String(rbuffer)
            Finally
                If Not GZipHelperStream Is Nothing Then
                    GZipHelperStream.Close()

                    GC.SuppressFinalize(GZipHelperStream)
                End If
            End Try

            Dim rEncodedString As String = _
                CType( _
                    IIf( _
                        IsDecodedCF02Exist, _
                        String.Format("{0},{1},{2}", EncodingHashCode, System.Web.HttpUtility.UrlEncode(EncodedCF01), System.Web.HttpUtility.UrlEncode(EncodedCF02)), _
                        String.Format("{0},{1}", EncodingHashCode, System.Web.HttpUtility.UrlEncode(EncodedCF01)) _
                    ), _
                    String _
                )

            Return rEncodedString
        End Function

        Public Shared Function DecodeCallFunction(ByVal EncodedCallFunction As String) As String
            Dim SplitECF As String() = _
                EncodedCallFunction.Split(","c)

            Dim DecodedCF01 As String = String.Empty, DecodedCF02 As String
            Dim EncodingHashCode As String = SplitECF(0)
            ' First Part of Encoded Call Function
            '   [AssemblyName]?[ClassName].[FunctionName]
            ' FP is Encoded with Base64
            '   Base64 contains encrypted Data
            '   EncData = XOR HashCode applied on to deflated compression
            Dim EncodedCF01 As String = SplitECF(1)
            If Not EncodedCF01.Contains("+") AndAlso _
                EncodedCF01.Contains("%") Then EncodedCF01 = System.Web.HttpUtility.UrlDecode(EncodedCF01)
            ' Second Part of Encoded Call Function (Parameters)
            '   [Parameter]|[Parameter]|...
            ' Parameters are Encoded with Base64
            Dim EncodedCF02 As String = String.Empty
            If SplitECF.Length = 3 Then
                EncodedCF02 = SplitECF(2)

                If Not EncodedCF02.Contains("+") AndAlso _
                    EncodedCF02.Contains("%") Then EncodedCF02 = System.Web.HttpUtility.UrlDecode(EncodedCF02)
            End If

            Dim EncodedText As New System.Text.StringBuilder

            Dim bC As Integer, buffer As Byte() = _
                    System.Convert.FromBase64String(EncodedCF01)

            ' EncData to DecData Process
            For bC = 0 To buffer.Length - 1
                buffer(bC) = buffer(bC) Xor CType( _
                                                    Asc(EncodingHashCode.Chars(bC Mod EncodingHashCode.Length)), _
                                                    Byte _
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

                    If bC > 0 Then EncodedText.Append( _
                                        System.Text.Encoding.UTF8.GetString(rbuffer, 0, bC) _
                                    )
                Loop While bC > 0
            Finally
                If Not GZipStream Is Nothing Then GZipStream.Close()

                If Not GZipHelperStream Is Nothing Then
                    GZipHelperStream.Close()

                    GC.SuppressFinalize(GZipHelperStream)
                End If
            End Try

            DecodedCF01 = EncodedText.ToString()

            ' Decode The Parameters Part
            Dim rDecodedString As String
            If Not String.IsNullOrEmpty(EncodedCF02) Then
                DecodedCF02 = System.Text.Encoding.UTF8.GetString( _
                                    System.Convert.FromBase64String(EncodedCF02) _
                                )

                rDecodedString = String.Format("{0},{1}", DecodedCF01, DecodedCF02)
            Else
                rDecodedString = String.Format("{0}", DecodedCF01)
            End If

            Return rDecodedString
        End Function

        Public Shared Function CleanHTMLTags(ByVal Content As String, ByVal CleaningTags As String()) As String
            Dim RegExSearch As System.Text.RegularExpressions.Regex

            If Not String.IsNullOrEmpty(Content) AndAlso _
                Not CleaningTags Is Nothing AndAlso _
                CleaningTags.Length > 0 Then

                Dim SearchType As Integer = 0, tContent As String = String.Empty
                Dim regMatchs As System.Text.RegularExpressions.MatchCollection

                For Each CleaningTag As String In CleaningTags
                    If CleaningTag.IndexOf(">"c) = 0 Then
                        RegExSearch = New System.Text.RegularExpressions.Regex( _
                                        String.Format("<{0}(\s+[^>]*)*>", CleaningTag.Substring(1)) _
                                    )
                        SearchType = 1
                    Else
                        RegExSearch = New System.Text.RegularExpressions.Regex( _
                                        String.Format("<{0}(\s+[^>]*)*(/)?>", CleaningTag) _
                                    )
                        SearchType = 0
                    End If

                    regMatchs = RegExSearch.Matches(Content)

                    If regMatchs.Count > 0 Then
                        Dim LastSearchIndex As Integer = 0
                        For Each regMatch As System.Text.RegularExpressions.Match In regMatchs
                            tContent &= Content.Substring(LastSearchIndex, regMatch.Index - LastSearchIndex)

                            Select Case SearchType
                                Case 1
                                    Dim tailRegExSearch As New System.Text.RegularExpressions.Regex( _
                                        String.Format("</{0}>", CleaningTag.Substring(1)))
                                    Dim tailRegMatch As System.Text.RegularExpressions.Match = _
                                        tailRegExSearch.Match(Content, LastSearchIndex)

                                    If tailRegMatch.Success Then
                                        LastSearchIndex = tailRegMatch.Index + tailRegMatch.Length
                                    Else
                                        LastSearchIndex = regMatch.Index + regMatch.Length
                                    End If
                                Case Else
                                    LastSearchIndex = regMatch.Index + regMatch.Length
                            End Select
                        Next
                        tContent &= Content.Substring(LastSearchIndex)
                        Content = tContent
                    End If
                Next
            End If
            
            Return Content
        End Function

        Public Overloads Shared Function AssemblePostBackInformation(ByVal AssembleInfo As String) As PGlobals.Execution.AssembleResultInfo
            Return [Assembly].AssemblePostBackInformation(Nothing, Nothing, AssembleInfo, Nothing)
        End Function

        Public Overloads Shared Function AssemblePostBackInformation(ByVal AssembleInfo As String, ByVal ArgumentInfos As Globals.ArgumentInfo.ArgumentInfoCollection) As PGlobals.Execution.AssembleResultInfo
            Return [Assembly].AssemblePostBackInformation(Nothing, Nothing, AssembleInfo, ArgumentInfos)
        End Function

        Private Shared _DllObjectLibrary As New Hashtable
        Private Shared _PrivateBinPath As String = Nothing
        Public Overloads Shared Function AssemblePostBackInformation(ByVal ThemeID As String, ByVal AddonID As String, ByVal AssembleInfo As String, ByVal ArgumentInfos As Globals.ArgumentInfo.ArgumentInfoCollection) As PGlobals.Execution.AssembleResultInfo
            Dim rAssembleResultInfo As PGlobals.Execution.AssembleResultInfo

            ' Load All Addons of Theme if ThemeID and AddonID is not Pass Empty
            If Not String.IsNullOrEmpty(ThemeID) AndAlso _
                Not String.IsNullOrEmpty(AddonID) Then

                Assembly.QueryThemeAddons(ThemeID, Nothing)
            End If
            ' ! ----

            Dim tSplitterInfo1 As String(), tSplitterInfo2 As String()
            Dim AsDllName As String = Nothing, AsClassName As String = Nothing, AsFunctionName As String = Nothing, AsFunctionParams As String() = Nothing

            tSplitterInfo1 = AssembleInfo.Split("?"c)

            If tSplitterInfo1.Length = 2 Then
                AsDllName = tSplitterInfo1(0)

                tSplitterInfo2 = tSplitterInfo1(1).Split(","c)

                AsClassName = tSplitterInfo2(0).Split("."c)(0)
                AsFunctionName = tSplitterInfo2(0).Split("."c)(1)

                If tSplitterInfo2.Length > 1 Then AsFunctionParams = String.Join(",", tSplitterInfo2, 1, tSplitterInfo2.Length - 1).Split("|"c)
            End If

            rAssembleResultInfo = New PGlobals.Execution.AssembleResultInfo(AsDllName, AsClassName, AsFunctionName, AsFunctionParams)

            Dim ArgumentValueList As Object() = New Object() {}
            Assembly.PrepareArguments(ArgumentInfos, rAssembleResultInfo.FunctionParams, ArgumentValueList)

            Dim PlugInsPath As String = _
                    IO.Path.Combine( _
                        SolidDevelopment.Web.Configurations.TemporaryRoot, _
                        String.Format("{0}{2}{1}", _
                            Configurations.WorkingPath.WorkingPathID, _
                            General.Context.Items.Item("ApplicationID"), _
                            IO.Path.DirectorySeparatorChar _
                        ) _
                    )

            ' Leave this Absolute as like this, check the InvokedObject explanation
            If Assembly._PrivateBinPath Is Nothing Then _
                Assembly._PrivateBinPath = System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath

            If Assembly._PrivateBinPath.IndexOf(PlugInsPath) = -1 Then
                System.AppDomain.CurrentDomain.AppendPrivatePath(PlugInsPath)

                Assembly._PrivateBinPath = System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath
            End If

            Dim AssemblyKey As String = _
                String.Format("KEY-{0}_{1}", PlugInsPath, AsDllName)

            Dim PlugInsAppDomain As System.AppDomain = Nothing
            Dim PlugInsLoader As SolidDevelopment.Web.Managers.PlugInsLoader = Nothing

            If Assembly._DllObjectLibrary.ContainsKey(AssemblyKey) Then
                PlugInsLoader = CType(Assembly._DllObjectLibrary.Item(AssemblyKey), Managers.PlugInsLoader)
            Else
                PlugInsLoader = New PlugInsLoader(PlugInsPath, AsDllName)

                If Not Assembly._DllObjectLibrary.ContainsKey(AssemblyKey) Then Assembly._DllObjectLibrary.Add(AssemblyKey, PlugInsLoader)
            End If

            Try
                ' Invoke must use the same appdomain because of the context sync price
                Dim InvokedObject As Object = PlugInsLoader.Invoke(AsClassName, AsFunctionName, ArgumentValueList)

                If TypeOf InvokedObject Is Exception Then
                    rAssembleResultInfo.MethodResult = CType(InvokedObject, Exception)
                Else
                    rAssembleResultInfo.MethodResult = InvokedObject
                End If
            Catch ex As Exception
                rAssembleResultInfo.MethodResult = ex
            End Try

            If Not rAssembleResultInfo.MethodResult Is Nothing AndAlso _
                TypeOf rAssembleResultInfo.MethodResult Is Exception AndAlso _
                Configurations.Debugging Then

                Try
                    SolidDevelopment.Web.Helpers.EventLogging.WriteToLog( _
                            CType( _
                                rAssembleResultInfo.MethodResult, Exception).ToString() _
                        )
                Catch exLogging As Exception
                    Try
                        If Not System.Diagnostics.EventLog.SourceExists("XeoraCube") Then System.Diagnostics.EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                        System.Diagnostics.EventLog.WriteEntry("XeoraCube", _
                            CType( _
                                rAssembleResultInfo.MethodResult, Exception).ToString(), _
                                EventLogEntryType.Error _
                        )
                    Catch exLogging02 As Exception
                        ' Just Handle Exception
                    End Try
                End Try
            End If

            Return rAssembleResultInfo
        End Function

        Private Shared Function GetAssemblyMethod(ByRef AssemblyObject As System.Type, ByVal CallFunctionName As String, ByVal CallFunctionParams As Object()) As System.Reflection.MethodInfo
            Dim rAssemblyMethod As System.Reflection.MethodInfo = Nothing

            For Each mI As System.Reflection.MethodInfo In AssemblyObject.GetMethods()
                If String.Compare(mI.Name, CallFunctionName, True) = 0 AndAlso _
                    ( _
                        mI.GetParameters().Length = CallFunctionParams.Length OrElse _
                        ( _
                            mI.GetParameters().Length = 1 AndAlso _
                            mI.GetParameters()(0).ParameterType Is GetType(Object()) _
                        ) _
                    ) Then

                    rAssemblyMethod = mI

                    Exit For
                End If
            Next

            If rAssemblyMethod Is Nothing AndAlso _
                Not AssemblyObject.BaseType Is Nothing Then

                rAssemblyMethod = Assembly.GetAssemblyMethod(AssemblyObject.BaseType, CallFunctionName, CallFunctionParams)

                If Not rAssemblyMethod Is Nothing Then AssemblyObject = AssemblyObject.BaseType
            End If

            Return rAssemblyMethod
        End Function

        Public Shared Function ExecuteStatement(ByVal ThemeID As String, ByVal AddonID As String, ByVal StatementBlockID As String, ByVal Statement As String) As Object
            Dim rMethodResult As Object = Nothing

            Dim objAssembly As System.Reflection.Assembly
            Dim CallFunction As String = Nothing

            Try
                Dim BlockKey As String = String.Format("BLOCKCALL_{0}_{1}_{2}", ThemeID, AddonID, StatementBlockID.Replace(".NOCACHE", Nothing).Replace("."c, "_"c))

                objAssembly = [Assembly].PrepareExecutionDll(BlockKey, Statement, (StatementBlockID.Length - StatementBlockID.LastIndexOf(".NOCACHE")) = 8)

                If Not objAssembly Is Nothing Then
                    Dim AssemblyObject As System.Type = _
                        objAssembly.CreateInstance(String.Format("ExternalCall.{0}", BlockKey)).GetType()
                    Dim MethodObject As System.Reflection.MethodInfo = _
                        AssemblyObject.GetMethod("ExternalCallMethod")

                    rMethodResult = MethodObject.Invoke(AssemblyObject, _
                                                        Reflection.BindingFlags.DeclaredOnly Or _
                                                        Reflection.BindingFlags.InvokeMethod, _
                                                        Nothing, Nothing, Nothing)
                End If
            Catch ex As Exception
                rMethodResult = ex
            End Try

            Return rMethodResult
        End Function

        Private Shared _ThemeAddons As New Hashtable
        Public Shared Sub QueryThemeAddons(ByVal ThemeID As String, ByRef AddonInfos As PGlobals.ThemeInfo.AddonInfo())
            If Assembly._ThemeAddons.ContainsKey(ThemeID) Then
                AddonInfos = CType(Assembly._ThemeAddons.Item(ThemeID), PGlobals.ThemeInfo.AddonInfo())
            Else
                AddonInfos = New PGlobals.ThemeInfo.AddonInfo() {}

                Dim DllTempLocation As String = _
                    IO.Path.Combine( _
                        SolidDevelopment.Web.Configurations.TemporaryRoot, _
                        String.Format("{0}{2}{1}", _
                            Configurations.WorkingPath.WorkingPathID, _
                            General.Context.Items.Item("ApplicationID"), _
                            IO.Path.DirectorySeparatorChar _
                        ) _
                    )

                Dim AddonsPath As String = _
                        IO.Path.Combine( _
                            Configurations.PyhsicalRoot, _
                            String.Format("{0}Themes{2}{1}{2}Addons", Configurations.ApplicationRoot.FileSystemImplementation, ThemeID, IO.Path.DirectorySeparatorChar) _
                        )

                If IO.Directory.Exists(AddonsPath) Then
                    For Each AddonPath As String In IO.Directory.GetDirectories(AddonsPath)
                        If IO.Directory.Exists( _
                            IO.Path.Combine(AddonPath, "Dlls")) Then

                            Dim AddonDeploymentType As PGlobals.ThemeBase.DeploymentTypes, AddonName As String, AddonVersion As String = Nothing, AddonPassword As Byte() = Nothing
                            For Each dlls As String In IO.Directory.GetFiles(IO.Path.Combine(AddonPath, "Dlls"), "*.dll")
                                If Assembly.ExamAddonDll(dlls, AddonVersion) Then
                                    AddonName = New IO.DirectoryInfo(AddonPath).Name

                                    ' Search Security Key
                                    If IO.File.Exists(IO.Path.Combine(AddonPath, "security.key")) Then
                                        Dim fI As New IO.FileInfo(IO.Path.Combine(AddonPath, "security.key"))
                                        Dim fS As IO.FileStream = Nothing

                                        Try
                                            fS = fI.Open(IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                                            AddonPassword = CType(Array.CreateInstance(GetType(Byte), fI.Length), Byte())

                                            fS.Read(AddonPassword, 0, AddonPassword.Length)
                                        Finally
                                            If Not fS Is Nothing Then fS.Close()
                                        End Try
                                    End If
                                    ' !---

                                    ' Search Compiled Content File
                                    If IO.File.Exists(IO.Path.Combine(AddonPath, "Content.swct")) Then
                                        AddonDeploymentType = PGlobals.ThemeBase.DeploymentTypes.Release
                                    Else
                                        AddonDeploymentType = PGlobals.ThemeBase.DeploymentTypes.Development
                                    End If
                                    ' !---

                                    ' Create Application Path
                                    If Not IO.Directory.Exists(DllTempLocation) Then IO.Directory.CreateDirectory(DllTempLocation)

                                    AddonDeployment.ExtractApplication(ThemeID, AddonName, AddonPassword, DllTempLocation)

                                    Array.Resize(AddonInfos, AddonInfos.Length + 1)
                                    AddonInfos(AddonInfos.Length - 1) = _
                                                    New PGlobals.ThemeInfo.AddonInfo(AddonDeploymentType, AddonName, AddonVersion, AddonPassword)

                                    Exit For
                                End If
                            Next
                        End If
                    Next
                End If

                If Not Assembly._ThemeAddons.ContainsKey(ThemeID) Then Assembly._ThemeAddons.Add(ThemeID, AddonInfos)
            End If
        End Sub

        Private Shared Function ExamAddonDll(ByVal dllLocation As String, ByRef Version As String) As Boolean
            Dim AssemblyAppDomain As System.AppDomain = Nothing, PlugInsLoader As SolidDevelopment.Web.Managers.PlugInsLoader = Nothing

            Dim FI As New IO.FileInfo(dllLocation)

            Assembly.LoadPlugInsLoader( _
                        FI.Directory.FullName, _
                        IO.Path.GetFileNameWithoutExtension(FI.Name), _
                        AssemblyAppDomain, _
                        PlugInsLoader _
                    )

            Dim IsInterfaceFound As Boolean = False
            If Not PlugInsLoader Is Nothing Then _
                IsInterfaceFound = PlugInsLoader.ExamInterface("SolidDevelopment.Web.PGlobals+PlugInMarkers+IAddon")

            Dim ApplicationCachePath As String = _
                IO.Path.Combine( _
                    AssemblyAppDomain.SetupInformation.CachePath, _
                    AssemblyAppDomain.SetupInformation.ApplicationName _
                )

            System.AppDomain.Unload(AssemblyAppDomain)

            ' CleanUp Addon Temporary Files In a Thread for not waiting their cleanup process finished...
            Threading.ThreadPool.QueueUserWorkItem( _
                New System.Threading.WaitCallback(AddressOf Assembly.CleanUpAppDomainTemporaryFiles), ApplicationCachePath)
            ' !--

            Return IsInterfaceFound
        End Function

        Private Shared Sub CleanUpAppDomainTemporaryFiles(ByVal state As Object)
            Try
                IO.Directory.Delete(CType(state, String), True)
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try
        End Sub

        Private Shared Sub LoadPlugInsLoader(ByVal PlugInsPath As String, ByVal DllName As String, ByRef AssemblyAppDomain As System.AppDomain, ByRef PlugInsLoader As SolidDevelopment.Web.Managers.PlugInsLoader)
            Dim AssemblyAppDomainFriendlyName As String = _
                String.Format("{0}_{1}", System.AppDomain.CurrentDomain.FriendlyName, Date.Now.Ticks.ToString())

            Dim AssemblyAppDomainSetup As New System.AppDomainSetup

            With AssemblyAppDomainSetup
                .ApplicationName = Guid.NewGuid().ToString()
                .ApplicationBase = System.AppDomain.CurrentDomain.BaseDirectory
                .PrivateBinPath = String.Format("{0};{1}", System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath, PlugInsPath)
                .PrivateBinPathProbe = "*"

                .CachePath = System.AppDomain.CurrentDomain.SetupInformation.CachePath
                .ConfigurationFile = System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
                .DisallowCodeDownload = True
                .DynamicBase = System.AppDomain.CurrentDomain.SetupInformation.DynamicBase
                .ShadowCopyDirectories = .PrivateBinPath
                .ShadowCopyFiles = "true"
            End With

            AssemblyAppDomain = _
                        System.AppDomain.CreateDomain( _
                            AssemblyAppDomainFriendlyName, _
                            Nothing, _
                            AssemblyAppDomainSetup _
                        )
            AddHandler AssemblyAppDomain.UnhandledException, New UnhandledExceptionEventHandler(AddressOf Assembly.OnUnhandledAppDomainExceptions)

            Try
                Dim LoaderName As New System.Reflection.AssemblyName
                LoaderName.CodeBase = IO.Path.Combine( _
                                            System.AppDomain.CurrentDomain.BaseDirectory, _
                                            String.Format("bin{0}WebPlugInsLoader.dll", IO.Path.DirectorySeparatorChar) _
                                        )

                Dim LoaderDll As System.Reflection.Assembly = AssemblyAppDomain.Load(LoaderName)
                Dim LoaderType As Type = LoaderDll.GetExportedTypes()(0)

                PlugInsLoader = _
                    CType( _
                        AssemblyAppDomain.CreateInstanceAndUnwrap( _
                            LoaderDll.FullName, _
                            LoaderType.FullName, _
                            True, _
                            Reflection.BindingFlags.CreateInstance, _
                            Nothing, _
                            New Object() { _
                                PlugInsPath, _
                                DllName _
                            }, _
                            System.Globalization.CultureInfo.CurrentCulture, _
                            Nothing, _
                            Nothing _
                        ), _
                        SolidDevelopment.Web.Managers.PlugInsLoader _
                    )
            Catch ex As Exception
                PlugInsLoader = Nothing
            End Try
        End Sub

        ' For Logging Purposes UnHandled Application Domain Exception Event Function
        Private Shared Sub OnUnhandledAppDomainExceptions(ByVal source As Object, ByVal args As UnhandledExceptionEventArgs)
            If Not args.ExceptionObject Is Nothing AndAlso _
                TypeOf args.ExceptionObject Is Exception Then

                Try
                    If Not System.Diagnostics.EventLog.SourceExists("XeoraCube") Then System.Diagnostics.EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                    System.Diagnostics.EventLog.WriteEntry("XeoraCube", _
                        " --- Loaded PlugIn Exception --- " & Environment.NewLine & Environment.NewLine & _
                        CType( _
                            args.ExceptionObject, Exception).ToString(), _
                            EventLogEntryType.Error _
                    )
                Catch ex As Exception
                    ' Just Handle Exception
                End Try
            End If
        End Sub
        ' !---

        Public Shared Sub ClearCache()
            If Not Assembly._ThemeAddons Is Nothing Then Assembly._ThemeAddons.Clear()
            If Not Assembly._DllObjectLibrary Is Nothing Then Assembly._DllObjectLibrary.Clear()
            If Not Assembly._ExecutionDlls Is Nothing Then Assembly._ExecutionDlls.Clear()
        End Sub

        Private Shared _ExecutionDlls As New Hashtable
        Private Shared Function PrepareExecutionDll(ByVal BlockKey As String, ByVal Statement As String, ByVal NoCache As Boolean) As System.Reflection.Assembly
            Dim rAssembly As System.Reflection.Assembly = Nothing

            If NoCache Then
                If Assembly._ExecutionDlls.ContainsKey(BlockKey) Then
                    Assembly._ExecutionDlls.Remove(BlockKey)
                End If
            End If

            If Not Statement Is Nothing Then
                If Assembly._ExecutionDlls.ContainsKey(BlockKey) Then
                    rAssembly = CType(Assembly._ExecutionDlls.Item(BlockKey), System.Reflection.Assembly)
                Else
                    Dim CodeBlock As New System.Text.StringBuilder()

                    CodeBlock.AppendLine("using System;")
                    CodeBlock.AppendLine("using System.Data;")
                    CodeBlock.AppendLine("using System.Xml;")
                    CodeBlock.AppendLine("namespace ExternalCall")
                    CodeBlock.AppendLine("{")
                    CodeBlock.AppendFormat("public class {0}", BlockKey)
                    CodeBlock.AppendLine("{")
                    CodeBlock.AppendLine("public static object ExternalCallMethod()")
                    CodeBlock.AppendLine("{")
                    CodeBlock.AppendFormat("{0}", Statement)
                    CodeBlock.AppendLine("} // method")
                    CodeBlock.AppendLine("} // class")
                    CodeBlock.AppendLine("} // namespace")

                    Dim CSharpProvider As New Microsoft.CSharp.CSharpCodeProvider()

                    Dim objCompiler As System.CodeDom.Compiler.CodeDomProvider = _
                        System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp")

                    Dim CompilerParams As New System.CodeDom.Compiler.CompilerParameters()

                    CompilerParams.GenerateExecutable = False
                    CompilerParams.GenerateInMemory = True
                    CompilerParams.IncludeDebugInformation = False
                    CompilerParams.ReferencedAssemblies.Add("system.dll")
                    CompilerParams.ReferencedAssemblies.Add("system.data.dll")
                    CompilerParams.ReferencedAssemblies.Add("system.xml.dll")

                    Dim CompilerResults As System.CodeDom.Compiler.CompilerResults

                    CompilerResults = objCompiler.CompileAssemblyFromSource(CompilerParams, CodeBlock.ToString())

                    If CompilerResults.Errors.HasErrors Then
                        Dim sB As New System.Text.StringBuilder()

                        For Each cRE As System.CodeDom.Compiler.CompilerError In CompilerResults.Errors
                            sB.AppendLine(cRE.ErrorText)
                        Next

                        Throw New Exception(sB.ToString())
                    End If

                    rAssembly = CompilerResults.CompiledAssembly

                    If Not Assembly._ExecutionDlls.ContainsKey(BlockKey) Then Assembly._ExecutionDlls.Add(BlockKey, rAssembly)
                End If
            End If

            Return rAssembly
        End Function

    End Class
End Namespace
