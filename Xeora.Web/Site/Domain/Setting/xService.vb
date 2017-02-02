Option Strict On

Namespace Xeora.Web.Site.Setting
    Public Class xService
        Implements [Shared].IDomain.IxService

        Public Function CreatexServiceAuthentication(ByVal ParamArray dItems() As DictionaryEntry) As String Implements [Shared].IDomain.IxService.CreatexServiceAuthentication
            Dim rString As String = Nothing

            If Not dItems Is Nothing Then
                rString = Guid.NewGuid.ToString()

                Dim SessionInfo As New [Global].xServiceSessionInfo(rString, Date.Now)
                For Each dItem As DictionaryEntry In dItems
                    SessionInfo.AddSessionItem(dItem.Key.ToString(), dItem.Value)
                Next

                Me.VariablePool.Set(rString, SessionInfo)
            End If

            Return rString
        End Function

        Public ReadOnly Property ReadSessionVariable(ByVal PublicKey As String, ByVal name As String) As Object Implements [Shared].IDomain.IxService.ReadSessionVariable
            Get
                Dim rObject As Object = Nothing

                Dim SessionInfo As [Global].xServiceSessionInfo =
                    CType(
                        Me.VariablePool.Get(PublicKey),
                        [Global].xServiceSessionInfo
                    )

                If Not SessionInfo Is Nothing Then
                    Dim cfg As Configuration.Configuration =
                            System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                                System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                    Dim wConfig As System.Web.Configuration.SessionStateSection =
                        CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)
                    Dim TimeoutMinute As Integer = wConfig.Timeout.Minutes
                    If TimeoutMinute = 0 Then TimeoutMinute = 20

                    If SessionInfo.SessionDate.AddMinutes(TimeoutMinute) > Date.Now Then
                        rObject = SessionInfo.Item(name)

                        ' Keep session is active
                        SessionInfo.SessionDate = Date.Now
                        ' !--
                    Else
                        SessionInfo = Nothing
                    End If

                    Me.VariablePool.Set(PublicKey, SessionInfo)
                End If

                Return rObject
            End Get
        End Property

        Public Function RenderxService(ByVal ExecuteIn As String, ByVal ServiceID As String) As String Implements [Shared].IDomain.IxService.RenderxService
            ' call = Calling Function Providing in Query String
            Dim BindInfo As [Shared].Execution.BindInfo =
                [Shared].Execution.BindInfo.Make(
                    String.Format(
                        "{0}?{1}.{2},~execParams",
                        ExecuteIn,
                        ServiceID,
                        [Shared].Helpers.Context.Request.QueryString.Item("call"))
                )

            BindInfo.PrepareProcedureParameters(
                New [Shared].Execution.BindInfo.ProcedureParser(
                    Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                        ProcedureParameter.Value = [Shared].Helpers.Context.Request.Form.Item(ProcedureParameter.Key)
                    End Sub)
            )

            Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                Manager.Assembly.InvokeBind(BindInfo, Manager.Assembly.ExecuterTypes.Undefined)

            Return Me.GeneratexServiceXML(BindInvokeResult.InvokeResult)
        End Function

        Public Function GeneratexServiceXML(ByRef MethodResult As Object) As String Implements [Shared].IDomain.IxService.GeneratexServiceXML
            Dim xmlStream As New IO.StringWriter()
            Dim xmlWriter As New Xml.XmlTextWriter(xmlStream)

            ' Start Document Element
            Dim IsDone As Boolean

            [Shared].Execution.ExamMethodExecuted(MethodResult, IsDone)

            xmlWriter.WriteStartElement("ServiceResult")
            xmlWriter.WriteAttributeString("isdone", IsDone.ToString())

            xmlWriter.WriteStartElement("Item")

            If Not MethodResult Is Nothing Then
                If TypeOf MethodResult Is [Shared].ControlResult.RedirectOrder Then
                    xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)

                    xmlWriter.WriteCData(CType(MethodResult, [Shared].ControlResult.RedirectOrder).Location)
                ElseIf TypeOf MethodResult Is [Shared].ControlResult.Message Then
                    xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)
                    xmlWriter.WriteAttributeString("messagetype", CType(MethodResult, [Shared].ControlResult.Message).Type.ToString())

                    xmlWriter.WriteCData(CType(MethodResult, [Shared].ControlResult.Message).Message)
                ElseIf TypeOf MethodResult Is [Shared].ControlResult.Conditional Then
                    xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)

                    xmlWriter.WriteCData(CType(MethodResult, [Shared].ControlResult.Conditional).Result.ToString())
                ElseIf TypeOf MethodResult Is [Shared].ControlResult.VariableBlock Then
                    Dim VariableBlockResult As [Shared].ControlResult.VariableBlock =
                        CType(MethodResult, [Shared].ControlResult.VariableBlock)

                    xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)
                    xmlWriter.WriteAttributeString("cultureinfo", Globalization.CultureInfo.CurrentCulture.ToString())

                    xmlWriter.WriteStartElement("VariableList")
                    For Each key As String In VariableBlockResult.Keys
                        xmlWriter.WriteStartElement("Variable")
                        xmlWriter.WriteAttributeString("key", key)

                        If MethodResult.GetType().IsPrimitive Then
                            xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                            xmlWriter.WriteCData(CType(MethodResult, String))
                        Else
                            xmlWriter.WriteAttributeString("type", GetType(String).FullName)

                            xmlWriter.WriteCData(MethodResult.ToString())
                        End If

                        If VariableBlockResult.Item(key) Is Nothing Then
                            xmlWriter.WriteAttributeString("type", GetType(Object).FullName)
                            xmlWriter.WriteCData(String.Empty)
                        Else
                            If VariableBlockResult.Item(key).GetType().IsPrimitive Then
                                xmlWriter.WriteAttributeString("type", VariableBlockResult.Item(key).GetType().FullName)
                            Else
                                xmlWriter.WriteAttributeString("type", GetType(String).FullName)
                            End If

                            xmlWriter.WriteCData(
                                CType(MethodResult, [Shared].ControlResult.VariableBlock).Item(key).ToString()
                            )
                        End If

                        xmlWriter.WriteEndElement()
                    Next
                    xmlWriter.WriteEndElement()
                ElseIf TypeOf MethodResult Is [Shared].ControlResult.DirectDataAccess Then
                    Dim ex As New System.Exception("DirectDataAccess is not a transferable object!")

                    xmlWriter.WriteAttributeString("type", ex.GetType().FullName)
                    xmlWriter.WriteCData(ex.Message)
                ElseIf TypeOf MethodResult Is [Shared].ControlResult.PartialDataTable Then
                    xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)
                    xmlWriter.WriteAttributeString("total", CType(MethodResult, [Shared].ControlResult.PartialDataTable).Total.ToString())
                    xmlWriter.WriteAttributeString("cultureinfo", Globalization.CultureInfo.CurrentCulture.ToString())

                    Dim tDT As DataTable = CType(MethodResult, [Shared].ControlResult.PartialDataTable).Copy()

                    xmlWriter.WriteStartElement("Columns")
                    For Each dC As DataColumn In tDT.Columns
                        xmlWriter.WriteStartElement("Column")
                        xmlWriter.WriteAttributeString("name", dC.ColumnName)
                        xmlWriter.WriteAttributeString("type", dC.DataType.FullName)
                        xmlWriter.WriteEndElement()
                    Next
                    xmlWriter.WriteEndElement()

                    Dim rowIndex As Integer = 0

                    xmlWriter.WriteStartElement("Rows")
                    For Each dR As DataRow In tDT.Rows
                        xmlWriter.WriteStartElement("Row")
                        xmlWriter.WriteAttributeString("index", rowIndex.ToString())
                        For Each dC As DataColumn In tDT.Columns
                            xmlWriter.WriteStartElement("Item")
                            xmlWriter.WriteAttributeString("name", dC.ColumnName)
                            xmlWriter.WriteCData(dR.Item(dC.ColumnName).ToString())
                            xmlWriter.WriteEndElement()
                        Next
                        xmlWriter.WriteEndElement()

                        rowIndex += 1
                    Next
                    xmlWriter.WriteEndElement()

                    If Not CType(MethodResult, [Shared].ControlResult.PartialDataTable).Message Is Nothing Then
                        xmlWriter.WriteStartElement("Message")
                        xmlWriter.WriteAttributeString("messagetype", CType(MethodResult, [Shared].ControlResult.PartialDataTable).Message.Type.ToString())
                        xmlWriter.WriteCData(
                            CType(MethodResult, [Shared].ControlResult.PartialDataTable).Message.Message
                        )
                        xmlWriter.WriteEndElement()
                    End If
                ElseIf TypeOf MethodResult Is System.Exception Then
                    xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                    xmlWriter.WriteCData(CType(MethodResult, System.Exception).Message)
                Else
                    If MethodResult.GetType().IsPrimitive Then
                        xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                        xmlWriter.WriteCData(CType(MethodResult, String))
                    Else
                        Try
                            Dim SerializedValue As String =
                                SerializeHelpers.BinaryToBase64Serializer(MethodResult)

                            xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                            xmlWriter.WriteCData(SerializedValue)
                        Catch ex As System.Exception
                            xmlWriter.WriteAttributeString("type", ex.GetType().FullName)

                            xmlWriter.WriteCData(ex.Message)
                        End Try
                    End If
                End If
            Else
                xmlWriter.WriteAttributeString("type", GetType(String).FullName)
                xmlWriter.WriteCData(String.Empty)
            End If

            xmlWriter.WriteEndElement()

            ' End Document Element
            xmlWriter.WriteEndElement()

            xmlWriter.Flush()
            xmlWriter.Close()
            xmlStream.Close()

            Return xmlStream.ToString()
        End Function

        Private ReadOnly Property VariablePool() As [Shared].Service.VariablePoolOperation
            Get
                Return [Shared].Helpers.VariablePoolForxService
            End Get
        End Property

        Private Class SerializeHelpers
            Public Shared Function BinaryToBase64Serializer(ByVal [Object] As Object) As String
                Dim serializedBytes As Byte() = SerializeHelpers.BinarySerializer([Object])

                Return System.Convert.ToBase64String(serializedBytes)
            End Function

            Public Shared Function BinarySerializer(ByVal [Object] As Object) As Byte()
                Dim rByte As Byte()

                Dim BinaryFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                BinaryFormater.Binder = New Helper.OverrideBinder()

                Dim SerializationStream As New IO.MemoryStream

                BinaryFormater.Serialize(SerializationStream, [Object])

                rByte = CType(Array.CreateInstance(GetType(Byte), SerializationStream.Position), Byte())

                SerializationStream.Seek(0, IO.SeekOrigin.Begin)
                SerializationStream.Read(rByte, 0, rByte.Length)

                SerializationStream.Close()

                GC.SuppressFinalize(SerializationStream)

                Return rByte
            End Function

            Public Shared Function Base64ToBinaryDeSerializer(ByVal SerializedString As String) As Object
                Dim serializedBytes As Byte() = Convert.FromBase64String(SerializedString)

                Return SerializeHelpers.BinaryDeSerializer(serializedBytes)
            End Function

            Public Shared Function BinaryDeSerializer(ByVal SerializedBytes As Byte()) As Object
                Dim rObject As Object

                Dim BinaryFormater As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                BinaryFormater.Binder = New Helper.OverrideBinder()

                Dim SerializationStream As New IO.MemoryStream(SerializedBytes)

                rObject = BinaryFormater.Deserialize(SerializationStream)

                SerializationStream.Close()

                GC.SuppressFinalize(SerializationStream)

                Return rObject
            End Function
        End Class
    End Class
End Namespace