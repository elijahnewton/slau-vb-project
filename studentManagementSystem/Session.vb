Public Module Session
    Public Property CurrentUser As Employee

    Public ReadOnly Property IsHr As Boolean
        Get
            Return CurrentUser IsNot Nothing AndAlso CurrentUser.Role = UserRole.Hr
        End Get
    End Property
End Module
