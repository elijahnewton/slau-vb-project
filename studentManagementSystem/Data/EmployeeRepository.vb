Imports MySqlConnector

Public Module EmployeeRepository
    Private Const SelectColumns As String =
        "id, full_name, username, email, '' AS password_hash, '' AS password_salt, " &
        "role, position, monthly_salary, hire_date, status"

    Public Async Function AuthenticateAsync(username As String) As Task(Of Employee)
        Dim sql As String =
            "SELECT id, full_name, username, email, password_hash, password_salt, role, " &
            "position, monthly_salary, hire_date, status " &
            "FROM employees WHERE username = @username LIMIT 1"

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@username", username)

                Using reader = Await command.ExecuteReaderAsync()
                    If Not Await reader.ReadAsync() Then Return Nothing
                    Return MapEmployee(reader)
                End Using
            End Using
        End Using
    End Function

    Public Async Function GetByIdAsync(id As Integer) As Task(Of Employee)
        Dim sql As String = $"SELECT {SelectColumns} FROM employees WHERE id = @id LIMIT 1"

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@id", id)

                Using reader = Await command.ExecuteReaderAsync()
                    If Not Await reader.ReadAsync() Then Return Nothing
                    Return MapEmployee(reader)
                End Using
            End Using
        End Using
    End Function

    Public Async Function GetAllAsync(search As String) As Task(Of List(Of Employee))
        Dim sql As String = $"SELECT {SelectColumns} FROM employees WHERE 1 = 1"

        If Not String.IsNullOrWhiteSpace(search) Then
            sql &= " AND (full_name LIKE @search OR username LIKE @search OR position LIKE @search)"
        End If

        sql &= " ORDER BY role, full_name"

        Dim result As New List(Of Employee)()

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                If Not String.IsNullOrWhiteSpace(search) Then
                    command.Parameters.AddWithValue("@search", "%" & search.Trim() & "%")
                End If

                Using reader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        result.Add(MapEmployee(reader))
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    Public Async Function InsertAsync(employee As Employee, password As String) As Task
        Dim hashed = PasswordHasher.Create(password)

        Dim sql As String =
            "INSERT INTO employees (full_name, username, email, password_hash, password_salt, role, " &
            "position, monthly_salary, hire_date, status) " &
            "VALUES (@full_name, @username, @email, @password_hash, @password_salt, @role, " &
            "@position, @monthly_salary, @hire_date, @status)"

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@full_name", employee.FullName.Trim())
                command.Parameters.AddWithValue("@username", employee.Username.Trim())
                command.Parameters.AddWithValue("@email", If(employee.Email, "").Trim())
                command.Parameters.AddWithValue("@password_hash", hashed.Hash)
                command.Parameters.AddWithValue("@password_salt", hashed.Salt)
                command.Parameters.AddWithValue("@role", RoleText(employee.Role))
                command.Parameters.AddWithValue("@position", If(employee.Position, "").Trim())
                command.Parameters.AddWithValue("@monthly_salary", employee.MonthlySalary)
                command.Parameters.AddWithValue("@hire_date", DbDate(employee.HireDate))
                command.Parameters.AddWithValue("@status", StatusText(employee.Status))

                Await command.ExecuteNonQueryAsync()
                employee.Id = CInt(command.LastInsertedId)
            End Using
        End Using
    End Function

    Public Async Function UpdateAsync(employee As Employee) As Task
        Dim sql As String =
            "UPDATE employees SET full_name = @full_name, username = @username, email = @email, " &
            "role = @role, position = @position, monthly_salary = @monthly_salary, " &
            "hire_date = @hire_date, status = @status WHERE id = @id"

        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@full_name", employee.FullName.Trim())
                command.Parameters.AddWithValue("@username", employee.Username.Trim())
                command.Parameters.AddWithValue("@email", If(employee.Email, "").Trim())
                command.Parameters.AddWithValue("@role", RoleText(employee.Role))
                command.Parameters.AddWithValue("@position", If(employee.Position, "").Trim())
                command.Parameters.AddWithValue("@monthly_salary", employee.MonthlySalary)
                command.Parameters.AddWithValue("@hire_date", DbDate(employee.HireDate))
                command.Parameters.AddWithValue("@status", StatusText(employee.Status))
                command.Parameters.AddWithValue("@id", employee.Id)

                Await command.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Public Async Function SetStatusAsync(id As Integer, status As AccountStatus) As Task
        Using connection = Await Db.OpenConnectionAsync()
            Using command As New MySqlCommand("UPDATE employees SET status = @status WHERE id = @id", connection)
                command.Parameters.AddWithValue("@status", StatusText(status))
                command.Parameters.AddWithValue("@id", id)
                Await command.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Private Function MapEmployee(reader As MySqlDataReader) As Employee
        Dim emailOrdinal = reader.GetOrdinal("email")
        Dim positionOrdinal = reader.GetOrdinal("position")
        Dim hireDateOrdinal = reader.GetOrdinal("hire_date")

        Dim hireDate As Date? = Nothing
        If Not reader.IsDBNull(hireDateOrdinal) Then
            hireDate = reader.GetDateTime(hireDateOrdinal)
        End If

        Return New Employee With {
            .Id = reader.GetInt32("id"),
            .FullName = reader.GetString("full_name"),
            .Username = reader.GetString("username"),
            .Email = If(reader.IsDBNull(emailOrdinal), "", reader.GetString(emailOrdinal)),
            .PasswordHash = reader.GetString("password_hash"),
            .PasswordSalt = reader.GetString("password_salt"),
            .Role = ParseRole(reader.GetString("role")),
            .Position = If(reader.IsDBNull(positionOrdinal), "", reader.GetString(positionOrdinal)),
            .MonthlySalary = reader.GetDecimal("monthly_salary"),
            .HireDate = hireDate,
            .Status = ParseStatus(reader.GetString("status"))
        }
    End Function

    Private Function DbDate(value As Date?) As Object
        If value.HasValue Then Return CObj(value.Value)
        Return CObj(DBNull.Value)
    End Function
End Module
