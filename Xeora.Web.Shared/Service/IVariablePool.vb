Option Strict On

Namespace Xeora.Web.Shared.Service
    Public Interface IVariablePool
        Enum VariablePoolTypeStatus
            Host
            Client
        End Enum

        Function PingToRemoteEndPoint() As Boolean
        Function GetVariableFromPool(ByVal SessionKeyID As String, ByVal name As String) As Byte()

        Sub DoMassRegistration(ByVal SessionKeyID As String, ByVal serializedSerializableDictionary As Byte())
        Sub RegisterVariableToPool(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte())
        'Sub RegisterVariableToPoolAsync(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte())
        Sub UnRegisterVariableFromPool(ByVal SessionKeyID As String, ByVal name As String)
        'Sub UnRegisterVariableFromPoolAsync(ByVal SessionKeyID As String, ByVal name As String)
        Sub TransferRegistrations(ByVal FromSessionKeyID As String, ByVal CurrentSessionKeyID As String)
        'Sub ConfirmRegistrations(ByVal SessionKeyID As String)
        Sub DoCleanUp()

        ReadOnly Property VariablePoolType As VariablePoolTypeStatus
    End Interface
End Namespace