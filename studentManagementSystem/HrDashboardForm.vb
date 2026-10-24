Imports System.Linq

Public Class HrDashboardForm
    Private _employees As New List(Of Employee)()
    Private _entries As New List(Of WorkEntry)()
    Private _payroll As New List(Of PayrollRow)()
    Private _tabIcons As ImageList

    Private Async Sub HrDashboardForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session.CurrentUser Is Nothing Then
            Close()
            Return
        End If

        ApplyIcons()
        lblUser.Text = $"Signed in as {Session.CurrentUser.FullName} ({Session.CurrentUser.Username})"
        ConfigureGrids()

        dtpTimeMonth.Value = Date.Today
        dtpPayrollMonth.Value = Date.Today

        Await WithErrorAsync(AddressOf LoadEmployeesAsync, "Could not load accounts.")
        Await WithErrorAsync(AddressOf LoadTimeAsync, "Could not load work time.")
        Await WithErrorAsync(AddressOf LoadPayrollAsync, "Could not load payroll.")
    End Sub

    Private Sub ApplyIcons()
        Icon = UiIcons.AppIcon()

        _tabIcons = New ImageList()
        components.Add(_tabIcons)
        UiIcons.StyleTabs(tabMain, _tabIcons, "employees", "clock", "payroll")

        UiIcons.StyleButton(btnSearch, "search")
        UiIcons.StyleButton(btnRefreshEmployees, "refresh")
        UiIcons.StyleButton(btnAdd, "add")
        UiIcons.StyleButton(btnEdit, "edit")
        UiIcons.StyleButton(btnApprove, "approve")
        UiIcons.StyleButton(btnDeactivate, "block")
        UiIcons.StyleButton(btnTimeRefresh, "refresh")
        UiIcons.StyleButton(btnTimeDelete, "delete")
        UiIcons.StyleButton(btnPayrollLoad, "refresh")
        UiIcons.StyleButton(btnLogout, "logout")
    End Sub

    Private Sub ConfigureGrids()
        dgvEmployees.Columns.Clear()
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .Visible = False})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Name", .HeaderText = "Name", .FillWeight = 150})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Username", .HeaderText = "Username", .FillWeight = 100})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Role", .HeaderText = "Role", .FillWeight = 60})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Position", .HeaderText = "Position", .FillWeight = 130})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Salary", .HeaderText = "Salary", .FillWeight = 80})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "HireDate", .HeaderText = "Hire date", .FillWeight = 80})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Status", .HeaderText = "Status", .FillWeight = 70})
        dgvEmployees.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Email", .HeaderText = "Email", .FillWeight = 150})
        dgvEmployees.Columns("Salary").DefaultCellStyle.Format = "N2"
        dgvEmployees.Columns("Salary").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        dgvTime.Columns.Clear()
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .Visible = False})
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "WorkDate", .HeaderText = "Date", .FillWeight = 70})
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "EmployeeName", .HeaderText = "Employee", .FillWeight = 180})
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Hours", .HeaderText = "Hours", .FillWeight = 60})
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Note", .HeaderText = "Note", .FillWeight = 300})
        dgvTime.Columns("Hours").DefaultCellStyle.Format = "N2"
        dgvTime.Columns("Hours").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        dgvPayroll.Columns.Clear()
        dgvPayroll.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Name", .HeaderText = "Employee", .FillWeight = 180})
        dgvPayroll.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Position", .HeaderText = "Position", .FillWeight = 140})
        dgvPayroll.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Salary", .HeaderText = "Monthly salary", .FillWeight = 100})
        dgvPayroll.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Hours", .HeaderText = "Hours worked", .FillWeight = 90})
        dgvPayroll.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Days", .HeaderText = "Days worked", .FillWeight = 80})
        dgvPayroll.Columns("Salary").DefaultCellStyle.Format = "N2"
        dgvPayroll.Columns("Salary").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvPayroll.Columns("Hours").DefaultCellStyle.Format = "N2"
        dgvPayroll.Columns("Hours").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvPayroll.Columns("Days").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        For Each grid As DataGridView In {dgvEmployees, dgvTime, dgvPayroll}
            For Each column As DataGridViewColumn In grid.Columns
                column.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        Next
    End Sub

    Private Async Function LoadEmployeesAsync() As Task
        _employees = Await EmployeeRepository.GetAllAsync(txtSearch.Text)

        dgvEmployees.Rows.Clear()
        For Each employee As Employee In _employees
            Dim hireDate = If(employee.HireDate.HasValue, employee.HireDate.Value.ToString("yyyy-MM-dd"), "")
            dgvEmployees.Rows.Add(
                employee.Id,
                employee.FullName,
                employee.Username,
                If(employee.Role = UserRole.Hr, "HR", "Employee"),
                employee.Position,
                employee.MonthlySalary,
                hireDate,
                StatusDisplay(employee.Status),
                employee.Email)
        Next

        FillEmployeeOptions()
    End Function

    Private Sub FillEmployeeOptions()
        Dim previousId As Integer = 0
        Dim previous = TryCast(cmbEmployee.SelectedItem, EmployeeOption)
        If previous IsNot Nothing Then previousId = previous.Id

        Dim options As New List(Of EmployeeOption) From {
            New EmployeeOption With {.Id = 0, .Name = "All employees"}
        }

        For Each employee As Employee In _employees.Where(Function(item) item.Role = UserRole.Employee)
            options.Add(New EmployeeOption With {.Id = employee.Id, .Name = employee.FullName})
        Next

        cmbEmployee.DataSource = Nothing
        cmbEmployee.DataSource = options
        cmbEmployee.DisplayMember = "Name"

        For index As Integer = 0 To options.Count - 1
            If options(index).Id = previousId Then
                cmbEmployee.SelectedIndex = index
                Return
            End If
        Next

        cmbEmployee.SelectedIndex = 0
    End Sub

    Private Async Function LoadTimeAsync() As Task
        Dim selectedOption = TryCast(cmbEmployee.SelectedItem, EmployeeOption)
        Dim employeeId As Integer = If(selectedOption Is Nothing, 0, selectedOption.Id)
        Dim range = Payroll.MonthRange(dtpTimeMonth.Value.Year, dtpTimeMonth.Value.Month)

        _entries = Await WorkEntryRepository.GetAllAsync(range.StartDate, range.EndDateExclusive, employeeId)

        dgvTime.Rows.Clear()
        For Each entry As WorkEntry In _entries
            dgvTime.Rows.Add(entry.Id, entry.WorkDate.ToString("yyyy-MM-dd"), entry.EmployeeName, entry.Hours, entry.Note)
        Next
    End Function

    Private Async Function LoadPayrollAsync() As Task
        Dim range = Payroll.MonthRange(dtpPayrollMonth.Value.Year, dtpPayrollMonth.Value.Month)
        _payroll = Await WorkEntryRepository.GetPayrollAsync(range.StartDate, range.EndDateExclusive)

        dgvPayroll.Rows.Clear()
        For Each row As PayrollRow In _payroll
            dgvPayroll.Rows.Add(row.FullName, row.Position, row.MonthlySalary, row.TotalHours, row.DaysWorked)
        Next

        lblPayrollTotal.Text = $"Total monthly payroll: {Payroll.MonthlyTotal(_payroll):N2}"
    End Function

    Private Function SelectedEmployee() As Employee
        If dgvEmployees.CurrentRow Is Nothing Then Return Nothing

        Dim id = Convert.ToInt32(dgvEmployees.CurrentRow.Cells("Id").Value)
        Return _employees.FirstOrDefault(Function(item) item.Id = id)
    End Function

    Private Async Function AddAccountAsync() As Task
        Using form As New EmployeeEditForm(Nothing)
            If form.ShowDialog(Me) = DialogResult.OK Then
                Await LoadEmployeesAsync()
            End If
        End Using
    End Function

    Private Async Function EditAccountAsync() As Task
        Dim employee = SelectedEmployee()
        If employee Is Nothing Then
            MessageBox.Show(Me, "Select an account first.", "No selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Using form As New EmployeeEditForm(employee)
            If form.ShowDialog(Me) = DialogResult.OK Then
                Await LoadEmployeesAsync()
                Await LoadTimeAsync()
                Await LoadPayrollAsync()
            End If
        End Using
    End Function

    Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click, btnRefreshEmployees.Click
        Await WithErrorAsync(AddressOf LoadEmployeesAsync, "Could not load accounts.")
    End Sub

    Private Async Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await WithErrorAsync(AddressOf LoadEmployeesAsync, "Could not load accounts.")
        End If
    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Await WithErrorAsync(AddressOf AddAccountAsync, "Could not add the account.")
    End Sub

    Private Async Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Await WithErrorAsync(AddressOf EditAccountAsync, "Could not edit the account.")
    End Sub

    Private Async Sub dgvEmployees_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployees.CellDoubleClick
        If e.RowIndex >= 0 Then
            Await WithErrorAsync(AddressOf EditAccountAsync, "Could not edit the account.")
        End If
    End Sub

    Private Async Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        Dim employee = SelectedEmployee()
        If employee Is Nothing Then
            MessageBox.Show(Me, "Select an account first.", "No selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If employee.Status = AccountStatus.Active Then
            MessageBox.Show(Me, "This account is already active.", "Already active",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show(Me, $"Approve {employee.FullName}?", "Approve account",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            UseWaitCursor = True
            Await EmployeeRepository.SetStatusAsync(employee.Id, AccountStatus.Active)
            Await LoadEmployeesAsync()
        Catch ex As Exception
            MessageBox.Show(Me, "Could not approve the account." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    Private Async Sub btnDeactivate_Click(sender As Object, e As EventArgs) Handles btnDeactivate.Click
        Dim employee = SelectedEmployee()
        If employee Is Nothing Then
            MessageBox.Show(Me, "Select an account first.", "No selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If employee.Id = Session.CurrentUser.Id Then
            MessageBox.Show(Me, "You cannot deactivate your own account.", "Not allowed",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If employee.Status = AccountStatus.Inactive Then
            MessageBox.Show(Me, "This account is already inactive.", "Already inactive",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show(Me, $"Deactivate {employee.FullName}? They will no longer be able to sign in.",
                           "Deactivate account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If

        Try
            UseWaitCursor = True
            Await EmployeeRepository.SetStatusAsync(employee.Id, AccountStatus.Inactive)
            Await LoadEmployeesAsync()
            Await LoadPayrollAsync()
        Catch ex As Exception
            MessageBox.Show(Me, "Could not deactivate the account." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    Private Async Sub btnTimeRefresh_Click(sender As Object, e As EventArgs) Handles btnTimeRefresh.Click
        Await WithErrorAsync(AddressOf LoadTimeAsync, "Could not load work time.")
    End Sub

    Private Async Sub btnTimeDelete_Click(sender As Object, e As EventArgs) Handles btnTimeDelete.Click
        If dgvTime.CurrentRow Is Nothing Then
            MessageBox.Show(Me, "Select a work entry first.", "No selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim entryId = Convert.ToInt32(dgvTime.CurrentRow.Cells("Id").Value)
        If MessageBox.Show(Me, "Delete this work entry?", "Delete entry",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If

        Try
            UseWaitCursor = True
            Dim deleted = Await WorkEntryRepository.DeleteAsync(entryId, Nothing)
            If Not deleted Then
                MessageBox.Show(Me, "The entry was not found. Refresh and try again.", "Not found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            Await LoadTimeAsync()
            Await LoadPayrollAsync()
        Catch ex As Exception
            MessageBox.Show(Me, "Could not delete the entry." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    Private Async Sub btnPayrollLoad_Click(sender As Object, e As EventArgs) Handles btnPayrollLoad.Click
        Await WithErrorAsync(AddressOf LoadPayrollAsync, "Could not load payroll.")
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Session.CurrentUser = Nothing
        Close()
    End Sub

    Private Async Function WithErrorAsync(operation As Func(Of Task), context As String) As Task
        Try
            UseWaitCursor = True
            Await operation()
        Catch ex As Exception
            MessageBox.Show(Me, context & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UseWaitCursor = False
        End Try
    End Function
End Class
