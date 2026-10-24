Imports System.Globalization
Imports MySqlConnector

Public Module WorkEntryRepository
    Public Async Function AddAsync(employeeId As Integer, workDate As Date, hours As Decimal, note As String) As Task
        If hours <= 0D OrElse hours > 24D Then
            Throw New ArgumentException("Hours must be greater than 0 and at most 24.")
        End If

        If workDate.Date > Date.Today Then
            Throw New ArgumentException("Work date cannot be in the future.")
        End If

        Using connection = Await Db.OpenConnectionAsync()
            Dim existingHours = Await DailyHoursAsync(connection, employeeId, workDate.Date)
            If existingHours + hours > 24D Then
                Throw New InvalidOperationException(
                    $"Total hours for {workDate:yyyy-MM-dd} cannot exceed 24.")
            End If

            Dim sql As String =
                "INSERT INTO work_entries (employee_id, work_date, hours, note) " &
                "VALUES (@employee_id, @work_date, @hours, @note)"

            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@employee_id", employeeId)
                command.Parameters.AddWithValue("@work_date", workDate.Date)
                command.Parameters.AddWithValue("@hours", hours)
                command.Parameters.AddWithValue("@note", If(note, "").Trim())
                Await command.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Public Async Function GetForEmployeeAsync(employeeId As Integer, startDate As Date, endDateExclusive As Date) As Task(Of List(Of WorkEntry))
        Dim sql As String =
            "SELECT w.id, w.employee_id, e.full_name AS employee_name, w.work_date, w.hours, w.note " &
            "FROM work_entries w JOIN employees e ON e.id = w.employee_id " &
            "WHERE w.employee_id = @employee_id AND w.work_date >= @start_date AND w.work_date < @end_date " &
            "ORDER BY w.work_date DESC, w.id DESC"

        Return Await QueryAsync(sql, startDate, endDateExclusive, employeeId)
    End Function

    Public Async Function GetAllAsync(startDate As Date, endDateExclusive As Date, employeeId As Integer) As Task(Of List(Of WorkEntry))
        Dim sql As String =
            "SELECT w.id, w.employee_id, e.full_name AS employee_name, w.work_date, w.hours, w.note " &
            "FROM work_entries w JOIN employees e ON e.id = w.employee_id " &
            "WHERE w.work_date >= @start_date AND w.work_date < @end_date"

        If employeeId > 0 Then
            sql &= " AND w.employee_id = @employee_id"
        End If

        sql &= " ORDER BY w.work_date DESC, e.full_name, w.id DESC"

        Return Await QueryAsync(sql, startDate, endDateExclusive, employeeId)
    End Function

    Public Async Function DeleteAsync(id As Integer, ownerEmployeeId As Integer?) As Task(Of Boolean)
        Dim sql As String = "DELETE FROM work_entries WHERE id = @id"

        If ownerEmployeeId.HasValue Then
            sql &= " AND employee_id = @employee_id"
        End If

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@id", id)
                If ownerEmployeeId.HasValue Then
                    command.Parameters.AddWithValue("@employee_id", ownerEmployeeId.Value)
                End If

                Dim affected = Await command.ExecuteNonQueryAsync()
                Return affected > 0
            End Using
        End Using
    End Function

    Public Async Function GetPayrollAsync(startDate As Date, endDateExclusive As Date) As Task(Of List(Of PayrollRow))
        Dim sql As String =
            "SELECT e.id AS employee_id, e.full_name, COALESCE(e.position, '') AS position, e.monthly_salary, " &
            "COALESCE(SUM(w.hours), 0) AS total_hours, COUNT(DISTINCT w.work_date) AS days_worked " &
            "FROM employees e " &
            "LEFT JOIN work_entries w ON w.employee_id = e.id AND w.work_date >= @start_date AND w.work_date < @end_date " &
            "WHERE e.role = 'EMPLOYEE' AND e.status = 'ACTIVE' " &
            "GROUP BY e.id, e.full_name, e.position, e.monthly_salary " &
            "ORDER BY e.full_name"

        Dim result As New List(Of PayrollRow)()

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@start_date", startDate)
                command.Parameters.AddWithValue("@end_date", endDateExclusive)

                Using reader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        result.Add(New PayrollRow With {
                            .EmployeeId = reader.GetInt32("employee_id"),
                            .FullName = reader.GetString("full_name"),
                            .Position = reader.GetString("position"),
                            .MonthlySalary = reader.GetDecimal("monthly_salary"),
                            .TotalHours = reader.GetDecimal("total_hours"),
                            .DaysWorked = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("days_worked")), CultureInfo.InvariantCulture)
                        })
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    Private Async Function QueryAsync(sql As String, startDate As Date, endDateExclusive As Date, employeeId As Integer) As Task(Of List(Of WorkEntry))
        Dim result As New List(Of WorkEntry)()

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@start_date", startDate)
                command.Parameters.AddWithValue("@end_date", endDateExclusive)
                If sql.Contains("@employee_id", StringComparison.Ordinal) Then
                    command.Parameters.AddWithValue("@employee_id", employeeId)
                End If

                Using reader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        result.Add(New WorkEntry With {
                            .Id = reader.GetInt32("id"),
                            .EmployeeId = reader.GetInt32("employee_id"),
                            .EmployeeName = reader.GetString("employee_name"),
                            .WorkDate = reader.GetDateTime("work_date"),
                            .Hours = reader.GetDecimal("hours"),
                            .Note = If(reader.IsDBNull(reader.GetOrdinal("note")), "", reader.GetString("note"))
                        })
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    Private Async Function DailyHoursAsync(connection As MySqlConnection, employeeId As Integer, workDate As Date) As Task(Of Decimal)
        Dim sql As String =
            "SELECT COALESCE(SUM(hours), 0) FROM work_entries " &
            "WHERE employee_id = @employee_id AND work_date = @work_date"

        Using command As New MySqlCommand(sql, connection)
            command.Parameters.AddWithValue("@employee_id", employeeId)
            command.Parameters.AddWithValue("@work_date", workDate)

            Dim value = Await command.ExecuteScalarAsync()
            Return Convert.ToDecimal(value, CultureInfo.InvariantCulture)
        End Using
    End Function
End Module
