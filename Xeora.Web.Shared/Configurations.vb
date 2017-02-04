Option Strict On

Namespace Xeora.Web.Shared
    Public Class Configurations
        Private Shared _xeoraSettings As Object = Nothing
        Private Shared ReadOnly Property XeoraSettingsObject() As Object
            Get
                If Configurations._xeoraSettings Is Nothing Then
                    Try
                        Dim RequestAsm As Reflection.Assembly, objRequest As Type

                        RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
                        objRequest = RequestAsm.GetType("Xeora.Web.Handler.RequestModule", False, True)

                        Configurations._xeoraSettings =
                            objRequest.InvokeMember("XeoraSettings", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
                    Catch ex As Exception
                        Throw New Reflection.TargetInvocationException("Xeora Settings are unable to read!", ex)
                    End Try
                End If

                Return Configurations._xeoraSettings
            End Get
        End Property

        Public Shared ReadOnly Property DefaultDomain() As String()
            Get
                Dim WorkingObject As Object =
                    Configurations.XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                Return CType(WorkingObject.GetType().InvokeMember("DefaultDomain", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String())
            End Get
        End Property

        Public Shared ReadOnly Property PhysicalRoot() As String
            Get
                Dim WorkingObject As Object =
                    Configurations.XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                Return CType(WorkingObject.GetType().InvokeMember("PhysicalRoot", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)
            End Get
        End Property

        Public Shared ReadOnly Property VirtualRoot() As String
            Get
                Dim WorkingObject As Object =
                    Configurations.XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                Dim virRootValue As String =
                    CType(WorkingObject.GetType().InvokeMember("VirtualRoot", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                If String.IsNullOrEmpty(virRootValue) Then
                    virRootValue = "/"
                Else
                    virRootValue = virRootValue.Replace("\"c, "/"c)

                    If virRootValue.IndexOf("/"c) <> 0 Then _
                        virRootValue = String.Format("/{0}", virRootValue)

                    If String.Compare(virRootValue.Substring(virRootValue.Length - 1), "/") <> 0 Then _
                        virRootValue = String.Format("{0}/", virRootValue)
                End If

                Return virRootValue
            End Get
        End Property

        Private Shared _VariablePoolServicePort As Short = 0
        Public Shared ReadOnly Property VariablePoolServicePort() As Short
            Get
                If Configurations._VariablePoolServicePort = 0 Then
                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("ServicePort", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Configurations._VariablePoolServicePort = CType(WorkingObject.GetType().InvokeMember("VariablePool", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), Short)

                    If Configurations._VariablePoolServicePort = 0 Then _
                        Configurations._VariablePoolServicePort = 12005
                End If

                Return Configurations._VariablePoolServicePort
            End Get
        End Property

        Private Shared _ScheduledTasksServicePort As Short = 0
        Public Shared ReadOnly Property ScheduledTasksServicePort() As Short
            Get
                If Configurations._ScheduledTasksServicePort = 0 Then
                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("ServicePort", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Configurations._ScheduledTasksServicePort = CType(WorkingObject.GetType().InvokeMember("ScheduledTasks", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), Short)

                    If Configurations._ScheduledTasksServicePort = 0 Then _
                        Configurations._ScheduledTasksServicePort = -1
                End If

                Return Configurations._ScheduledTasksServicePort
            End Get
        End Property

        Public Class ApplicationRootFormat
            Private _FileSystemImplementation As String
            Private _BrowserImplementation As String

            Public Sub New()
                Me._FileSystemImplementation = String.Empty
                Me._BrowserImplementation = String.Empty
            End Sub

            Public Property FileSystemImplementation() As String
                Get
                    Return Me._FileSystemImplementation
                End Get
                Friend Set(ByVal value As String)
                    Me._FileSystemImplementation = value
                End Set
            End Property

            Public Property BrowserImplementation() As String
                Get
                    Return Me._BrowserImplementation
                End Get
                Friend Set(ByVal value As String)
                    Me._BrowserImplementation = value
                End Set
            End Property
        End Class

        Private Shared _ApplicationRootFormat As ApplicationRootFormat = Nothing
        Public Shared ReadOnly Property ApplicationRoot() As ApplicationRootFormat
            Get
                Dim rApplicationRootFormat As ApplicationRootFormat = Configurations._ApplicationRootFormat

                If rApplicationRootFormat Is Nothing Then
                    rApplicationRootFormat = New ApplicationRootFormat()

                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Dim appRootValue As String =
                        CType(WorkingObject.GetType().InvokeMember("ApplicationRoot", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                    If String.IsNullOrEmpty(appRootValue) Then
                        rApplicationRootFormat.FileSystemImplementation = ".\"
                        rApplicationRootFormat.BrowserImplementation = Configurations.VirtualRoot
                    Else
                        If appRootValue.IndexOf("\"c) = 0 Then _
                            appRootValue = String.Format(".{0}", appRootValue)

                        If appRootValue.IndexOf(".\") <> 0 Then _
                            appRootValue = String.Format(".\{0}", appRootValue)

                        If String.Compare(appRootValue.Substring(appRootValue.Length - 1), "\") <> 0 Then _
                            appRootValue = String.Format("{0}\", appRootValue)

                        rApplicationRootFormat.FileSystemImplementation = appRootValue
                        rApplicationRootFormat.BrowserImplementation =
                            String.Format("{0}{1}", Configurations.VirtualRoot, appRootValue.Substring(2).Replace("\"c, "/"c))
                    End If

                    Configurations._ApplicationRootFormat = rApplicationRootFormat
                End If

                Return rApplicationRootFormat
            End Get
        End Property

        Public Class WorkingPathFormat
            Private _WorkingPath As String
            Private _WorkingPathID As String

            Public Sub New()
                Me._WorkingPath = String.Empty
                Me._WorkingPathID = String.Empty
            End Sub

            Public Property WorkingPath() As String
                Get
                    Return Me._WorkingPath
                End Get
                Friend Set(ByVal value As String)
                    Me._WorkingPath = value
                End Set
            End Property

            Public Property WorkingPathID() As String
                Get
                    Return Me._WorkingPathID
                End Get
                Friend Set(ByVal value As String)
                    Me._WorkingPathID = value
                End Set
            End Property
        End Class

        Private Shared _WorkingPathFormat As WorkingPathFormat = Nothing
        Public Shared ReadOnly Property WorkingPath() As WorkingPathFormat
            Get
                Dim rWorkingPathFormat As WorkingPathFormat = Configurations._WorkingPathFormat

                If rWorkingPathFormat Is Nothing Then
                    rWorkingPathFormat = New WorkingPathFormat()

                    Dim wPath As String =
                        IO.Path.Combine(Configurations.PhysicalRoot, Configurations.ApplicationRoot.FileSystemImplementation)
                    rWorkingPathFormat.WorkingPath = wPath

                    For Each match As Text.RegularExpressions.Match In Text.RegularExpressions.Regex.Matches(wPath, "\W")
                        If match.Success Then
                            wPath = wPath.Remove(match.Index, match.Length)
                            wPath = wPath.Insert(match.Index, "_")
                        End If
                    Next
                    rWorkingPathFormat.WorkingPathID = wPath

                    Configurations._WorkingPathFormat = rWorkingPathFormat
                End If

                Return rWorkingPathFormat
            End Get
        End Property

        Private Shared _Debugging As String = Nothing
        Public Shared ReadOnly Property Debugging() As Boolean
            Get
                Dim rValue As Boolean = False

                If String.IsNullOrEmpty(Configurations._Debugging) Then
                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Configurations._Debugging = CType(WorkingObject.GetType().InvokeMember("Debugging", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                    If String.IsNullOrEmpty(Configurations._Debugging) Then _
                        Configurations._Debugging = Boolean.FalseString
                End If

                If Not Boolean.TryParse(Configurations._Debugging, rValue) Then rValue = True

                Return rValue
            End Get
        End Property

        Private Shared _LogHTTPExceptions As String = Nothing
        Public Shared ReadOnly Property LogHTTPExceptions() As Boolean
            Get
                Dim rValue As Boolean = True

                If String.IsNullOrEmpty(Configurations._LogHTTPExceptions) Then
                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Configurations._LogHTTPExceptions = CType(WorkingObject.GetType().InvokeMember("LogHTTPExceptions", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                    If String.IsNullOrEmpty(Configurations._LogHTTPExceptions) Then _
                        Configurations._LogHTTPExceptions = Boolean.TrueString
                End If

                If Not Boolean.TryParse(Configurations._LogHTTPExceptions, rValue) Then rValue = True

                Return rValue
            End Get
        End Property

        Public Shared ReadOnly Property RequestTagFiltering() As Globals.RequestTagFilteringTypes
            Get
                Dim rRequestTagFilteringType As Globals.RequestTagFilteringTypes =
                    Globals.RequestTagFilteringTypes.None

                Dim WorkingObject As Object =
                    Configurations.XeoraSettingsObject.GetType().InvokeMember("RequestTagFilter", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                Dim _RequestTagFiltering As String =
                    CType(WorkingObject.GetType().InvokeMember("Direction", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)
                Dim _RequestTagFilteringItems As String =
                    CType(WorkingObject.GetType().InvokeMember("Items", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                If Not String.IsNullOrEmpty(_RequestTagFiltering) AndAlso
                    Not String.IsNullOrEmpty(_RequestTagFilteringItems) Then

                    Try
                        rRequestTagFilteringType =
                            CType(
                                [Enum].Parse(
                                    GetType(Globals.RequestTagFilteringTypes),
                                    _RequestTagFiltering
                                ), Globals.RequestTagFilteringTypes
                            )
                    Catch ex As Exception
                        ' Just Handle Exception
                    End Try
                End If

                Return rRequestTagFilteringType
            End Get
        End Property

        Private Shared _RequestTagFilteringItemList As String() = Nothing
        Public Shared ReadOnly Property RequestTagFilteringItems() As String()
            Get
                Dim rStringList As String() = Configurations._RequestTagFilteringItemList

                If rStringList Is Nothing Then
                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("RequestTagFilter", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Dim _RequestTagFilteringItems As String =
                        CType(WorkingObject.GetType().InvokeMember("Items", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                    If Not String.IsNullOrEmpty(_RequestTagFilteringItems) Then
                        Dim RequestTagFilteringItemList As New Generic.List(Of String)

                        Try
                            For Each item As String In _RequestTagFilteringItems.Split("|"c)
                                If Not item Is Nothing AndAlso item.Trim().Length > 0 Then _
                                    RequestTagFilteringItemList.Add(item.Trim().ToLower(New Globalization.CultureInfo("en-US")))
                            Next

                            rStringList = RequestTagFilteringItemList.ToArray()
                        Catch ex As Exception
                            ' Just Handle Exception
                        End Try
                    End If
                    If rStringList Is Nothing Then rStringList = New String() {}
                End If

                Return rStringList
            End Get
        End Property

        Private Shared _RequestTagFilteringExceptionsList As String() = Nothing
        Public Shared ReadOnly Property RequestTagFilteringExceptions() As String()
            Get
                Dim rStringList As String() = Configurations._RequestTagFilteringExceptionsList

                If rStringList Is Nothing Then
                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("RequestTagFilter", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Dim _RequestTagFilteringExceptions As String =
                        CType(WorkingObject.GetType().InvokeMember("Exceptions", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                    If Not String.IsNullOrEmpty(_RequestTagFilteringExceptions) Then
                        Dim RequestTagFilteringExceptionList As New Generic.List(Of String)

                        Try
                            For Each item As String In _RequestTagFilteringExceptions.Split("|"c)
                                If Not item Is Nothing AndAlso item.Trim().Length > 0 Then _
                                    RequestTagFilteringExceptionList.Add(item.Trim().ToLower(New Globalization.CultureInfo("en-US")))
                            Next

                            rStringList = RequestTagFilteringExceptionList.ToArray()
                        Catch ex As Exception
                            ' Just Handle Exception
                        End Try
                    End If
                    If rStringList Is Nothing Then rStringList = New String() {}
                End If

                Return rStringList
            End Get
        End Property

        Private Shared _TemporaryRoot As String = Nothing
        Public Shared ReadOnly Property TemporaryRoot() As String
            Get
                If Configurations._TemporaryRoot Is Nothing Then
                    Configurations._TemporaryRoot = IO.Path.GetTempPath()

                    If String.IsNullOrEmpty(Configurations._TemporaryRoot) Then _
                        Configurations._TemporaryRoot = IO.Path.Combine(Configurations.PhysicalRoot, "tmp")

                    If Not IO.Directory.Exists(Configurations._TemporaryRoot) Then _
                        IO.Directory.CreateDirectory(Configurations._TemporaryRoot)
                End If

                Return Configurations._TemporaryRoot
            End Get
        End Property

        Private Shared _UseHTML5Header As String = Nothing
        Public Shared ReadOnly Property UseHTML5Header() As Boolean
            Get
                Dim rValue As Boolean = False

                If String.IsNullOrEmpty(Configurations._UseHTML5Header) Then
                    Dim WorkingObject As Object =
                        Configurations.XeoraSettingsObject.GetType().InvokeMember("Main", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, Configurations.XeoraSettingsObject, Nothing)

                    Configurations._UseHTML5Header = CType(WorkingObject.GetType().InvokeMember("UseHTML5Header", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.GetProperty, Nothing, WorkingObject, Nothing), String)

                    If String.IsNullOrEmpty(Configurations._UseHTML5Header) Then _
                        Configurations._UseHTML5Header = Boolean.FalseString
                End If

                If Not Boolean.TryParse(Configurations._UseHTML5Header, rValue) Then rValue = True

                Return rValue
            End Get
        End Property
    End Class
End Namespace