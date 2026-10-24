Imports System.Diagnostics
Imports System.IO

Public Module SelfTest
    Public Function Run() As (Passed As Boolean, Report As String)
        Dim failures As New List(Of String)()

        Try
            Dim created = PasswordHasher.Create("correct horse battery staple")
            If Not PasswordHasher.Verify("correct horse battery staple", created.Hash, created.Salt) Then
                failures.Add("Password verification failed for the correct password.")
            End If

            If PasswordHasher.Verify("wrong password", created.Hash, created.Salt) Then
                failures.Add("Password verification accepted a wrong password.")
            End If
        Catch ex As Exception
            failures.Add("Password hashing threw: " & ex.Message)
        End Try

        Dim february = Payroll.MonthRange(2024, 2)
        If february.StartDate <> New Date(2024, 2, 1) Then
            failures.Add("MonthRange start date is wrong.")
        End If
        If february.EndDateExclusive <> New Date(2024, 3, 1) Then
            failures.Add("MonthRange end date is wrong.")
        End If
        If (february.EndDateExclusive - february.StartDate).Days <> 29 Then
            failures.Add("MonthRange did not account for a leap year.")
        End If

        Dim rows As New List(Of PayrollRow) From {
            New PayrollRow With {.MonthlySalary = 1000D},
            New PayrollRow With {.MonthlySalary = 2500.5D}
        }
        If Payroll.MonthlyTotal(rows) <> 3500.5D Then
            failures.Add("Payroll.MonthlyTotal returned the wrong total.")
        End If

        Dim report As String
        If failures.Count = 0 Then
            report = "Self-test passed."
        Else
            report = "Self-test failures:" & Environment.NewLine &
                     String.Join(Environment.NewLine, failures)
        End If

        Try
            File.WriteAllText(
                Path.Combine(Path.GetTempPath(), "employee-management-selftest.txt"),
                report)
        Catch ex As Exception
            Debug.WriteLine("Could not write self-test report: " & ex.Message)
        End Try

        Return (failures.Count = 0, report)
    End Function
End Module
