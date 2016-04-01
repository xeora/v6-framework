Option Strict On

Namespace Xeora.Web.Shared
    Public Class WebService
        Public Enum DataFlowTypes
            Output
            Input
        End Enum

        Public Shared Event TransferProgress(ByVal DataFlow As DataFlowTypes, ByVal Current As Long, Total As Long)

        Public Shared Function AuthenticateToWebService(ByVal WebServiceURL As String, ByVal AuthenticationFunction As String, ByVal Parameters As Parameters, ByRef IsAuthenticationDone As Boolean) As Object
            Dim rMethodResult As Object =
                WebService.CallWebService(WebServiceURL, AuthenticationFunction, Parameters)

            Execution.ExamMethodExecuted(rMethodResult, IsAuthenticationDone)

            If IsAuthenticationDone Then
                If TypeOf rMethodResult Is ControlResult.Message AndAlso
                    CType(rMethodResult, ControlResult.Message).Type <> ControlResult.Message.Types.Success Then _
                    IsAuthenticationDone = False
            End If

            Return rMethodResult
        End Function

        Public Overloads Shared Function CallWebService(ByVal WebServiceURL As String, ByVal [Function] As String, ByVal Parameters As Parameters) As Object
            Return WebService.CallWebService(WebServiceURL, [Function], Parameters, 60000)
        End Function

        Public Overloads Shared Function CallWebService(ByVal WebServiceURL As String, ByVal FunctionName As String, ByVal Parameters As Parameters, ByVal ResponseTimeout As Integer) As Object
            Dim rMethodResult As Object = Nothing

            Dim HttpWebRequest As Net.HttpWebRequest
            Dim HttpWebResponse As Net.HttpWebResponse

            Dim RequestMS As IO.MemoryStream = Nothing

            Try
                RequestMS = New IO.MemoryStream(
                                Text.Encoding.UTF8.GetBytes(
                                    String.Format("execParams={0}", System.Web.HttpUtility.UrlEncode(Parameters.ExecuteParametersXML))
                                )
                            )
                RequestMS.Seek(0, IO.SeekOrigin.Begin)

                Dim ResponseString As String
                Dim pageURL As String = String.Format("{0}?call={1}", WebServiceURL, FunctionName)

                ' Prepare Service Request Connection
                HttpWebRequest = CType(Net.HttpWebRequest.Create(pageURL), Net.HttpWebRequest)

                With HttpWebRequest
                    .Method = "POST"

                    .Timeout = ResponseTimeout
                    .Accept = "*/*"
                    .UserAgent = "Mozilla/4.0 (compatible; MSIE 5.00; Windows 98)"
                    .KeepAlive = False

                    .ContentType = "application/x-www-form-urlencoded"
                    .ContentLength = RequestMS.Length
                End With
                ' !--

                ' Post ExecuteParametersXML to the Web Service
                Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 512), Byte()), bC As Integer = 0, Current As Long = 0
                Dim TransferStream As IO.Stream = HttpWebRequest.GetRequestStream()

                Do
                    bC = RequestMS.Read(buffer, 0, buffer.Length)

                    If bC > 0 Then
                        Current += bC

                        TransferStream.Write(buffer, 0, bC)

                        RaiseEvent TransferProgress(DataFlowTypes.Output, Current, RequestMS.Length)
                    End If
                Loop Until bC = 0

                TransferStream.Close()
                ' !--

                HttpWebResponse = CType(HttpWebRequest.GetResponse(), Net.HttpWebResponse)

                ' Read and Parse Response Datas
                Dim resStream As IO.Stream = HttpWebResponse.GetResponseStream()

                ResponseString = String.Empty : Current = 0
                Do
                    bC = resStream.Read(buffer, 0, buffer.Length)

                    If bC > 0 Then
                        Current += bC

                        ResponseString &= Text.Encoding.UTF8.GetString(buffer, 0, bC)

                        RaiseEvent TransferProgress(DataFlowTypes.Input, Current, HttpWebResponse.ContentLength)
                    End If
                Loop Until bC = 0

                HttpWebResponse.Close()
                GC.SuppressFinalize(HttpWebResponse)

                rMethodResult = WebService.ParseWebServiceResult(ResponseString)
                ' !--
            Catch ex As Exception
                rMethodResult = New Exception("WebService Connection Error!", ex)
            Finally
                If Not RequestMS Is Nothing Then _
                    RequestMS.Close() : GC.SuppressFinalize(RequestMS)
            End Try

            Return rMethodResult
        End Function

        Private Shared Function ParseWebServiceResult(ByVal ResultXML As String) As Object
            Dim rMethodResult As Object = Nothing

            If String.IsNullOrEmpty(ResultXML) Then
                rMethodResult = New Exception("WebService Response Error!")
            Else
                Try
                    Dim xPathTextReader As IO.StringReader
                    Dim xPathDoc As Xml.XPath.XPathDocument = Nothing

                    xPathTextReader = New IO.StringReader(ResultXML)
                    xPathDoc = New Xml.XPath.XPathDocument(xPathTextReader)

                    Dim xPathNavigator As Xml.XPath.XPathNavigator
                    Dim xPathIter As Xml.XPath.XPathNodeIterator

                    xPathNavigator = xPathDoc.CreateNavigator()
                    xPathIter = xPathNavigator.Select("/ServiceResult")

                    Dim IsDone As Boolean

                    If xPathIter.MoveNext() Then
                        IsDone = Boolean.Parse(xPathIter.Current.GetAttribute("isdone", xPathIter.Current.NamespaceURI))

                        xPathIter = xPathNavigator.Select("/ServiceResult/Item")

                        If xPathIter.MoveNext() Then
                            Dim xType As String =
                                xPathIter.Current.GetAttribute("type", xPathIter.Current.NamespaceURI)

                            If Not String.IsNullOrEmpty(xType) Then
                                Select Case xType
                                    Case "RedirectOrder"
                                        rMethodResult = New ControlResult.RedirectOrder(
                                                                    xPathIter.Current.Value
                                                                )
                                    Case "Message"
                                        rMethodResult = New ControlResult.Message(
                                                                    xPathIter.Current.Value,
                                                                    CType(
                                                                        [Enum].Parse(
                                                                            GetType(ControlResult.Message.Types),
                                                                            xPathIter.Current.GetAttribute("messagetype", xPathIter.Current.NamespaceURI)
                                                                        ), ControlResult.Message.Types
                                                                    )
                                                                )
                                    Case "VariableBlock"
                                        Dim VariableBlock As New ControlResult.VariableBlock()

                                        Dim CultureInfo As Globalization.CultureInfo =
                                            New Globalization.CultureInfo(
                                                xPathIter.Current.GetAttribute("cultureinfo", xPathIter.Current.NamespaceURI))

                                        If xPathIter.Current.MoveToFirstChild() Then
                                            Dim xPathIter_V As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            If xPathIter_V.Current.MoveToFirstChild() Then
                                                Do
                                                    VariableBlock.Add(
                                                        xPathIter_V.Current.GetAttribute("key", xPathIter_V.Current.NamespaceURI),
                                                        Convert.ChangeType(
                                                            xPathIter_V.Current.Value.ToString(CultureInfo),
                                                            WebService.LoadTypeFromDomain(
                                                                AppDomain.CurrentDomain,
                                                                xPathIter_V.Current.GetAttribute("type", xPathIter_V.Current.NamespaceURI)
                                                            )
                                                        )
                                                    )
                                                Loop While xPathIter_V.Current.MoveToNext()
                                            End If
                                        End If
                                    Case "Conditional"
                                        rMethodResult = New ControlResult.Conditional(
                                                                CType(
                                                                    [Enum].Parse(
                                                                        GetType(ControlResult.Conditional.Conditions),
                                                                        xPathIter.Current.Value
                                                                    ),
                                                                    ControlResult.Conditional.Conditions
                                                                )
                                                            )
                                    Case "PartialDataTable"
                                        Dim PartialDataTable As New ControlResult.PartialDataTable()

                                        Dim Total As Integer = 0
                                        Integer.TryParse(
                                            xPathIter.Current.GetAttribute("total", xPathIter.Current.NamespaceURI), Total)
                                        Dim CultureInfo As Globalization.CultureInfo =
                                            New Globalization.CultureInfo(
                                                xPathIter.Current.GetAttribute("cultureinfo", xPathIter.Current.NamespaceURI))

                                        PartialDataTable.Locale = CultureInfo
                                        PartialDataTable.Total = Total

                                        If xPathIter.Current.MoveToFirstChild() Then
                                            Dim xPathIter_C As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            If xPathIter_C.Current.MoveToFirstChild() Then
                                                Do
                                                    PartialDataTable.Columns.Add(
                                                        xPathIter_C.Current.GetAttribute("name", xPathIter_C.Current.NamespaceURI),
                                                        WebService.LoadTypeFromDomain(
                                                            AppDomain.CurrentDomain,
                                                            xPathIter_C.Current.GetAttribute("type", xPathIter_C.Current.NamespaceURI))
                                                    )
                                                Loop While xPathIter_C.Current.MoveToNext()
                                            End If
                                        End If

                                        If xPathIter.Current.MoveToNext() Then
                                            Dim xPathIter_R As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            If xPathIter_R.Current.MoveToFirstChild() Then
                                                Dim xPathIter_RR As Xml.XPath.XPathNodeIterator
                                                Dim tDR As DataRow

                                                Do
                                                    tDR = PartialDataTable.NewRow()
                                                    xPathIter_RR = xPathIter_R.Clone()

                                                    If xPathIter_RR.Current.MoveToFirstChild() Then
                                                        Do
                                                            tDR.Item(
                                                                xPathIter_RR.Current.GetAttribute("name", xPathIter_RR.Current.NamespaceURI)
                                                            ) =
                                                                xPathIter_RR.Current.Value.ToString(CultureInfo)
                                                        Loop While xPathIter_RR.Current.MoveToNext()
                                                    End If

                                                    PartialDataTable.Rows.Add(tDR)
                                                Loop While xPathIter_R.Current.MoveToNext()
                                            End If
                                        End If

                                        If xPathIter.Current.MoveToNext() Then
                                            Dim xPathIter_E As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            PartialDataTable.Message =
                                                New ControlResult.Message(
                                                    xPathIter_E.Current.Value.ToString(CultureInfo),
                                                    CType(
                                                        [Enum].Parse(
                                                            GetType(ControlResult.Message.Types),
                                                            xPathIter_E.Current.GetAttribute("messagetype", xPathIter_E.Current.NamespaceURI)
                                                        ), ControlResult.Message.Types
                                                    )
                                                )
                                        End If

                                        rMethodResult = PartialDataTable
                                    Case Else
                                        Dim xTypeObject As Type =
                                            WebService.LoadTypeFromDomain(AppDomain.CurrentDomain, xType)

                                        If xTypeObject Is Nothing Then
                                            rMethodResult = xPathIter.Current.Value
                                        Else
                                            If WebService.SearchIsBaseType(xTypeObject, GetType(Exception)) Then
                                                rMethodResult = Activator.CreateInstance(xTypeObject, xPathIter.Current.Value, New Exception())
                                            Else
                                                If xTypeObject.IsPrimitive OrElse
                                                    xTypeObject Is GetType(Short) OrElse
                                                    xTypeObject Is GetType(Integer) OrElse
                                                    xTypeObject Is GetType(Long) Then

                                                    rMethodResult = Convert.ChangeType(
                                                                        xPathIter.Current.Value,
                                                                        xTypeObject,
                                                                        New Globalization.CultureInfo("en-US")
                                                                    )
                                                Else
                                                    Try
                                                        If Not String.IsNullOrEmpty(xPathIter.Current.Value) Then
                                                            Dim UnSerializedObject As Object =
                                                                Serializer.Base64ToBinary(xPathIter.Current.Value)

                                                            rMethodResult = UnSerializedObject
                                                        Else
                                                            rMethodResult = String.Empty
                                                        End If
                                                    Catch ex As Exception
                                                        rMethodResult = Activator.CreateInstance(ex.GetType(), ex.Message, New Exception())
                                                    End Try
                                                End If
                                            End If
                                        End If
                                End Select
                            End If
                        End If
                    Else
                        rMethodResult = New Exception("WebService Response Error!")
                    End If

                    ' Close Reader
                    xPathTextReader.Close()

                    ' Garbage Collection Cleanup
                    GC.SuppressFinalize(xPathTextReader)
                Catch ex As Exception
                    rMethodResult = New Exception("WebService Response Error!", ex)
                End Try
            End If

            Return rMethodResult
        End Function

        Private Shared Function SearchIsBaseType(ByVal [Type] As Type, ByVal SearchType As Type) As Boolean
            Dim rBoolean As Boolean = False

            Do
                If [Type] Is SearchType Then
                    rBoolean = True

                    Exit Do
                Else
                    [Type] = [Type].BaseType
                End If
            Loop Until [Type] Is Nothing OrElse [Type] Is GetType(Object)

            Return rBoolean
        End Function

        Private Shared Function LoadTypeFromDomain(ByVal AppDomain As AppDomain, ByVal SearchType As String) As Type
            Dim rType As Type =
                Type.GetType(SearchType)

            If rType Is Nothing Then
                Dim assms As Reflection.Assembly() =
                    AppDomain.GetAssemblies()

                For Each assm As Reflection.Assembly In assms
                    rType = assm.GetType(SearchType)

                    If Not rType Is Nothing Then Exit For
                Next
            End If

            Return rType
        End Function

        Public Class Parameters
            Inherits Generic.Dictionary(Of String, String)

            Private _PublicKey As String

            Public Sub New()
                Me._PublicKey = String.Empty
            End Sub

            Public Sub New(ByVal ExecuteParametersXML As String)
                Me.New()

                Me.ParseExecuteParametersXML(ExecuteParametersXML)
            End Sub

            Public ReadOnly Property ExecuteParametersXML() As String
                Get
                    Return Me.RenderExecuteParametersXML()
                End Get
            End Property

            Private Function RenderExecuteParametersXML() As String
                Dim xmlStream As New IO.StringWriter()
                Dim xmlWriter As New Xml.XmlTextWriter(xmlStream)

                Dim Enumerator As IDictionaryEnumerator =
                    Me.GetEnumerator()

                ' Start Document Element
                xmlWriter.WriteStartElement("ServiceParameters")

                If Not Me._PublicKey Is Nothing Then
                    xmlWriter.WriteStartElement("Item")
                    xmlWriter.WriteAttributeString("key", "PublicKey")

                    xmlWriter.WriteString(Me._PublicKey)
                    xmlWriter.WriteEndElement()
                End If

                Do While Enumerator.MoveNext()
                    xmlWriter.WriteStartElement("Item")
                    xmlWriter.WriteAttributeString("key", CType(Enumerator.Key, String))

                    xmlWriter.WriteCData(CType(Enumerator.Value, String))
                    xmlWriter.WriteEndElement()
                Loop

                ' End Document Element
                xmlWriter.WriteEndElement()

                xmlWriter.Flush()
                xmlWriter.Close()
                xmlStream.Close()

                Return xmlStream.ToString()
            End Function

            Private Sub ParseExecuteParametersXML(ByVal DataXML As String)
                Me.Clear()

                If Not String.IsNullOrEmpty(DataXML) Then
                    Try
                        Dim xPathTextReader As IO.StringReader
                        Dim xPathDoc As Xml.XPath.XPathDocument = Nothing

                        xPathTextReader = New IO.StringReader(DataXML)
                        xPathDoc = New Xml.XPath.XPathDocument(xPathTextReader)

                        Dim xPathNavigator As Xml.XPath.XPathNavigator
                        Dim xPathIter As Xml.XPath.XPathNodeIterator

                        xPathNavigator = xPathDoc.CreateNavigator()
                        xPathIter = xPathNavigator.Select("/ServiceParameters/Item")

                        Dim Key As String, Value As String

                        Do While xPathIter.MoveNext()
                            Key = xPathIter.Current.GetAttribute("key", xPathIter.Current.NamespaceURI)
                            Value = xPathIter.Current.Value

                            If String.Compare(Key, "PublicKey") = 0 Then
                                Me._PublicKey = Value
                            Else
                                Me.Item(Key) = Value
                            End If
                        Loop

                        ' Close Reader
                        xPathTextReader.Close()

                        ' Garbage Collection Cleanup
                        GC.SuppressFinalize(xPathTextReader)
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try
                End If
            End Sub

            Public Property PublicKey() As String
                Get
                    Return Me._PublicKey
                End Get
                Set(ByVal value As String)
                    Me._PublicKey = value
                End Set
            End Property
        End Class

        Private Class Serializer
            Public Shared Function BinaryToBase64(ByVal [Object] As Object) As String
                Dim serializedBytes As Byte() = Serializer.Serialize([Object])

                Return Convert.ToBase64String(serializedBytes)
            End Function

            Public Shared Function Serialize(ByVal [Object] As Object) As Byte()
                Dim rByte As Byte()

                Dim BinaryFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                BinaryFormater.Binder = New OverrideBinder()

                Dim SerializationStream As New IO.MemoryStream

                BinaryFormater.Serialize(SerializationStream, [Object])

                rByte = CType(Array.CreateInstance(GetType(Byte), SerializationStream.Position), Byte())

                SerializationStream.Seek(0, IO.SeekOrigin.Begin)
                SerializationStream.Read(rByte, 0, rByte.Length)

                SerializationStream.Close()

                GC.SuppressFinalize(SerializationStream)

                Return rByte
            End Function

            Public Shared Function Base64ToBinary(ByVal SerializedString As String) As Object
                Dim serializedBytes As Byte() = Convert.FromBase64String(SerializedString)

                Return Serializer.DeSerialize(serializedBytes)
            End Function

            Public Shared Function DeSerialize(ByVal SerializedBytes As Byte()) As Object
                Dim rObject As Object

                Dim BinaryFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                BinaryFormater.Binder = New OverrideBinder()

                Dim SerializationStream As New IO.MemoryStream(SerializedBytes)

                rObject = BinaryFormater.Deserialize(SerializationStream)

                SerializationStream.Close()

                GC.SuppressFinalize(SerializationStream)

                Return rObject
            End Function
        End Class
    End Class
End Namespace