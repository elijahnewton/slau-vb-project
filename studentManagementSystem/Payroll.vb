Public Module Payroll
    Public Function MonthRange(year As Integer, month As Integer) As (StartDate As Date, EndDateExclusive As Date)
        Dim startDate As New Date(year, month, 1)
        Return (startDate, startDate.AddMonths(1))
    End Function

    Public Function MonthlyTotal(rows As IEnumerable(Of PayrollRow)) As Decimal
        Dim total As Decimal = 0D
        For Each row As PayrollRow In rows
            total += row.MonthlySalary
        Next
        Return total
    End Function
End Module
