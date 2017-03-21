Option Strict On

Namespace Xeora.Web.Deployment
    Public NotInheritable Class InstanceFactory
        Implements IDisposable

        Private _Instances As Concurrent.ConcurrentDictionary(Of String, DomainDeployment)

        Public Sub New()
            Me._Instances = New Concurrent.ConcurrentDictionary(Of String, DomainDeployment)
        End Sub

        Private Shared _Current As InstanceFactory = Nothing
        Public Shared ReadOnly Property Current As InstanceFactory
            Get
                If InstanceFactory._Current Is Nothing Then _
                    InstanceFactory._Current = New InstanceFactory()

                Return InstanceFactory._Current
            End Get
        End Property

        Public Overloads Function GetOrCreate(ByVal DomainIDAccessTree As String()) As DomainDeployment
            Return Me.GetOrCreate(DomainIDAccessTree, Nothing)
        End Function

        Public Overloads Function GetOrCreate(ByVal DomainIDAccessTree As String(), ByVal LanguageID As String) As DomainDeployment
            Dim InstancenKey As String =
                String.Format("{0}_{1}", String.Join(Of String)("-", DomainIDAccessTree), LanguageID)
            Dim DomainDeployment As DomainDeployment = Nothing

            If Not Me._Instances.TryGetValue(InstancenKey, DomainDeployment) Then
                DomainDeployment = New DomainDeployment(DomainIDAccessTree, LanguageID)

                Me._Instances.TryAdd(InstancenKey, DomainDeployment)
            End If

            Return DomainDeployment
        End Function

        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                For Each Key As String In Me._Instances.Keys
                    Dim DomainDeployment As DomainDeployment = Nothing

                    Me._Instances.TryGetValue(Key, DomainDeployment)

                    If Not DomainDeployment Is Nothing Then DomainDeployment.Dispose()
                Next

                Me._Instances.Clear()
            End If

            Me.disposedValue = True
        End Sub

#Region "IDisposable Support"
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
        End Sub
#End Region
    End Class
End Namespace