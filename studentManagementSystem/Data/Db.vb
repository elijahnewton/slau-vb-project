Imports MySqlConnector

Public Module Db
    Private Const EmployeesTableSql As String = "
CREATE TABLE IF NOT EXISTS employees (
    id INT AUTO_INCREMENT PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(150) NULL,
    password_hash VARCHAR(128) NOT NULL,
    password_salt VARCHAR(64) NOT NULL,
    role VARCHAR(10) NOT NULL,
    position VARCHAR(100) NULL,
    monthly_salary DECIMAL(10,2) NOT NULL DEFAULT 0,
    hire_date DATE NULL,
    status VARCHAR(10) NOT NULL DEFAULT 'PENDING',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"

    Private Const WorkEntriesTableSql As String = "
CREATE TABLE IF NOT EXISTS work_entries (
    id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id INT NOT NULL,
    work_date DATE NOT NULL,
    hours DECIMAL(5,2) NOT NULL,
    note VARCHAR(255) NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_work_entries_employee FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    INDEX idx_work_entries_employee_date (employee_id, work_date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"

    Public Async Function OpenConnectionAsync() As Task(Of MySqlConnection)
        Dim connection As New MySqlConnection(AppSettings.GetConnectionString())
        Try
            Await connection.OpenAsync()
            Return connection
        Catch
            connection.Dispose()
            Throw
        End Try
    End Function

    Public Async Function InitializeAsync() As Task
        Using connection = Await OpenConnectionAsync()
            Using command As New MySqlCommand(EmployeesTableSql, connection)
                Await command.ExecuteNonQueryAsync()
            End Using

            Using command As New MySqlCommand(WorkEntriesTableSql, connection)
                Await command.ExecuteNonQueryAsync()
            End Using

            Await SeedHrAsync(connection)
        End Using
    End Function

    Private Async Function SeedHrAsync(connection As MySqlConnection) As Task
        Dim hrCount As Integer

        Using command As New MySqlCommand("SELECT COUNT(*) FROM employees WHERE role = 'HR'", connection)
            hrCount = Convert.ToInt32(Await command.ExecuteScalarAsync())
        End Using

        If hrCount > 0 Then Return

        Dim seed = AppSettings.GetSeedHr()
        If String.IsNullOrWhiteSpace(seed.Username) OrElse String.IsNullOrWhiteSpace(seed.Password) Then
            Throw New InvalidOperationException(
                "No HR account exists and SeedHr is not configured in appsettings.json.")
        End If

        Dim password = PasswordHasher.Create(seed.Password)

        Using command As New MySqlCommand(
            "INSERT INTO employees (full_name, username, email, password_hash, password_salt, role, position, monthly_salary, hire_date, status) " &
            "VALUES (@full_name, @username, NULL, @password_hash, @password_salt, 'HR', 'HR Manager', 0, @hire_date, 'ACTIVE')",
            connection)

            command.Parameters.AddWithValue("@full_name", "System Administrator")
            command.Parameters.AddWithValue("@username", seed.Username.Trim())
            command.Parameters.AddWithValue("@password_hash", password.Hash)
            command.Parameters.AddWithValue("@password_salt", password.Salt)
            command.Parameters.AddWithValue("@hire_date", Date.Today)
            Await command.ExecuteNonQueryAsync()
        End Using
    End Function
End Module
