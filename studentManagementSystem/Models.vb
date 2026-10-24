Public Enum UserRole
    Employee
    Hr
End Enum

Public Enum AccountStatus
    Pending
    Active
    Inactive
End Enum

Public Class Employee
    Public Property Id As Integer
    Public Property FullName As String = ""
    Public Property Username As String = ""
    Public Property Email As String = ""
    Public Property PasswordHash As String = ""
    Public Property PasswordSalt As String = ""
    Public Property Role As UserRole = UserRole.Employee
    Public Property Position As String = ""
    Public Property MonthlySalary As Decimal
    Public Property HireDate As Date?
    Public Property Status As AccountStatus = AccountStatus.Pending
End Class

Public Class WorkEntry
    Public Property Id As Integer
    Public Property EmployeeId As Integer
    Public Property EmployeeName As String = ""
    Public Property WorkDate As Date
    Public Property Hours As Decimal
    Public Property Note As String = ""
End Class

Public Class PayrollRow
    Public Property EmployeeId As Integer
    Public Property FullName As String = ""
    Public Property Position As String = ""
    Public Property MonthlySalary As Decimal
    Public Property TotalHours As Decimal
    Public Property DaysWorked As Integer
End Class

Public Class EmployeeOption
    Public Property Id As Integer
    Public Property Name As String = ""

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Module DomainText
    Public Function RoleText(role As UserRole) As String
        Return If(role = UserRole.Hr, "HR", "EMPLOYEE")
    End Function

    Public Function ParseRole(value As String) As UserRole
        If String.Equals(value, "HR", StringComparison.OrdinalIgnoreCase) Then Return UserRole.Hr
        Return UserRole.Employee
    End Function

    Public Function StatusText(status As AccountStatus) As String
        Select Case status
            Case AccountStatus.Active
                Return "ACTIVE"
            Case AccountStatus.Inactive
                Return "INACTIVE"
            Case Else
                Return "PENDING"
        End Select
    End Function

    Public Function StatusDisplay(status As AccountStatus) As String
        Select Case status
            Case AccountStatus.Active
                Return "Active"
            Case AccountStatus.Inactive
                Return "Inactive"
            Case Else
                Return "Pending"
        End Select
    End Function

    Public Function ParseStatus(value As String) As AccountStatus
        If String.Equals(value, "ACTIVE", StringComparison.OrdinalIgnoreCase) Then Return AccountStatus.Active
        If String.Equals(value, "INACTIVE", StringComparison.OrdinalIgnoreCase) Then Return AccountStatus.Inactive
        Return AccountStatus.Pending
    End Function
End Module
