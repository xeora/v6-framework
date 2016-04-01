Option Strict On

Namespace Xeora.Web.Shared
    Public Class OverrideBinder
        Inherits Runtime.Serialization.SerializationBinder

        Private Shared _AssemblyCache As New Generic.Dictionary(Of String, Reflection.Assembly)

        Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As Type
            Dim typeToDeserialize As Type = Nothing

            Dim sShortAssemblyName As String =
                assemblyName.Substring(0, assemblyName.IndexOf(","c))

            If OverrideBinder._AssemblyCache.ContainsKey(sShortAssemblyName) Then
                typeToDeserialize = Me.GetDeserializeType(OverrideBinder._AssemblyCache.Item(sShortAssemblyName), typeName)
            Else
                Dim ayAssemblies As Reflection.Assembly() = AppDomain.CurrentDomain.GetAssemblies()

                For Each ayAssembly As Reflection.Assembly In ayAssemblies
                    If sShortAssemblyName = ayAssembly.FullName.Substring(0, assemblyName.IndexOf(","c)) Then
                        OverrideBinder._AssemblyCache.Add(sShortAssemblyName, ayAssembly)

                        typeToDeserialize = Me.GetDeserializeType(ayAssembly, typeName)

                        Exit For
                    End If
                Next
            End If

            Return typeToDeserialize
        End Function

        Private Function GetDeserializeType(ByVal assembly As Reflection.Assembly, typeName As String) As Type
            Dim rTypeToDeserialize As Type = Nothing

            Dim remainAssemblyNames As String() = Nothing
            Dim typeName_L As String = Me.GetTypeFullNames(typeName, remainAssemblyNames)

            Dim tempType As Type =
                assembly.GetType(typeName_L)

            If Not tempType Is Nothing AndAlso tempType.IsGenericType Then
                Dim typeParameters As New Generic.List(Of Type)

                For Each remainAssemblyName As String In remainAssemblyNames
                    Dim eBI As Integer =
                        remainAssemblyName.LastIndexOf("]"c)
                    Dim qAssemblyName As String, qTypeName As String

                    If eBI = -1 Then
                        qTypeName = remainAssemblyName.Split(","c)(0)
                        qAssemblyName = remainAssemblyName.Substring(qTypeName.Length + 2)
                    Else
                        qTypeName = remainAssemblyName.Substring(0, eBI + 1)
                        qAssemblyName = remainAssemblyName.Substring(eBI + 3)
                    End If

                    typeParameters.Add(Me.BindToType(qAssemblyName, qTypeName))
                Next

                rTypeToDeserialize = tempType.MakeGenericType(typeParameters.ToArray())
            Else
                rTypeToDeserialize = tempType
            End If

            Return rTypeToDeserialize
        End Function

        Private Function GetTypeFullNames(ByVal typeName As String, ByRef remainAssemblyNames As String()) As String
            Dim rString As String

            Dim bI As Integer = typeName.IndexOf("["c, 0)

            If bI = -1 Then
                rString = typeName
                remainAssemblyNames = New String() {}
            Else
                rString = typeName.Substring(0, bI)

                Dim fullNameList_L As New Generic.List(Of String)
                Dim remainFullName As String =
                    typeName.Substring(bI + 1, typeName.Length - (bI + 1) - 1)

                Dim eI As Integer = 0, bIc As Integer = 0 : bI = 0
                Do
                    bI = remainFullName.IndexOf("["c, bI)

                    If bI > -1 Then
                        eI = remainFullName.IndexOf("]"c, bI + 1)
                        bIc = remainFullName.IndexOf("["c, bI + 1)

                        If bIc > -1 AndAlso bIc < eI Then
                            Do While bIc > -1 AndAlso bIc < eI
                                bIc = remainFullName.IndexOf("["c, bIc + 1)

                                If bIc > -1 AndAlso bIc < eI Then _
                                eI = remainFullName.IndexOf("]"c, eI + 1)
                            Loop

                            eI = remainFullName.IndexOf("]"c, eI + 1)
                        End If

                        fullNameList_L.Add(remainFullName.Substring(bI + 1, eI - (bI + 1)))

                        bI = eI + 1
                    End If
                Loop Until bI = -1

                remainAssemblyNames = fullNameList_L.ToArray()
            End If

            Return rString
        End Function
    End Class
End Namespace