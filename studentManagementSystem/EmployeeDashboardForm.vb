Imports System.Linq

Public Class EmployeeDashboardForm
    Private _entries As New List(Of WorkEntry)()
    Private _tabIcons As ImageList

    Private Async Sub EmployeeDashboardForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session.CurrentUser Is Nothing Then
            Close()
            Return
        End If

        ApplyIcons()
        ConfigureGrid()

        dtpTimeMonth.Value = Date.Today
        dtpPayMonth.Value = Date.Today
        dtpWorkDate.Value = Date.Today

        Await WithErrorAsync(AddressOf RefreshProfileAsync, "Could not load your profile.")
        Await WithErrorAsync(AddressOf LoadTimeAsync, "Could not load your work time.")
        Await WithErrorAsync(AddressOf LoadPayAsync, "Could not load your pay.")
    End Sub

    Private Sub ApplyIcons()
        Icon = UiIcons.AppIcon()

        _tabIcons = New ImageList()
        components.Add(_tabIcons)
        UiIcons.StyleTabs(tabMain, _tabIcons, "profile", "clock", "payroll")

        UiIcons.StyleButton(btnProfileRefresh, "refresh")
        UiIcons.StyleButton(btnTimeRefresh, "refresh")
        UiIcons.StyleButton(btnAddEntry, "add")
        UiIcons.StyleButton(btnDeleteEntry, "delete")
        UiIcons.StyleButton(btnPayRefresh, "refresh")
        UiIcons.StyleButton(btnLogout, "logout")

        picPay.Image = UiIcons.GetIcon("payroll", 56)
    End Sub

    Private Sub ConfigureGrid()
        dgvTime.Columns.Clear()
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .Visible = False})
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "WorkDate", .HeaderText = "Date", .FillWeight = 70})
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Hours", .HeaderText = "Hours", .FillWeight = 50})
        dgvTime.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Note", .HeaderText = "Note", .FillWeight = 300})
        dgvTime.Columns("Hours").DefaultCellStyle.Format = "N2"
        dgvTime.Columns("Hours").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        For Each column As DataGridViewColumn In dgvTime.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Async Function RefreshProfileAsync() As Task
        Dim current = Await EmployeeRepository.GetByIdAsync(Session.CurrentUser.Id)
        If current Is Nothing Then
            Throw New InvalidOperationException("Your account could not be found.")
        End If

        Session.CurrentUser = current
        UpdateProfile()
    End Function

    Private Sub UpdateProfile()
        Dim user = Session.CurrentUser
        lblUser.Text = $"Signed in as {user.FullName} ({user.Username})"
        lblFullName.Text = user.FullName
        lblUsername.Text = user.Username
        lblEmail.Text = If(String.IsNullOrWhiteSpace(user.Email), "-", user.Email)
        lblPosition.Text = If(String.IsNullOrWhiteSpace(user.Position), "Not assigned", user.Position)
        lblSalary.Text = user.MonthlySalary.ToString("N2")
        lblHireDate.Text = If(user.HireDate.HasValue, user.HireDate.Value.ToString("yyyy-MM-dd"), "-")
        lblStatus.Text = StatusDisplay(user.Status)
    End Sub

    Private Async Function LoadTimeAsync() As Task
        Dim range = Payroll.MonthRange(dtpTimeMonth.Value.Year, dtpTimeMonth.Value.Month)
        _entries = Await WorkEntryRepository.GetForEmployeeAsync(
            Session.CurrentUser.Id, range.StartDate, range.EndDateExclusive)

        dgvTime.Rows.Clear()
        For Each entry As WorkEntry In _entries
            dgvTime.Rows.Add(entry.Id, entry.WorkDate.ToString("yyyy-MM-dd"), entry.Hours, entry.Note)
        Next
    End Function

    Private Async Function LoadPayAsync() As Task
        Dim range = Payroll.MonthRange(dtpPayMonth.Value.Year, dtpPayMonth.Value.Month)
        Dim entries = Await WorkEntryRepository.GetForEmployeeAsync(
            Session.CurrentUser.Id, range.StartDate, range.EndDateExclusive)

        Dim totalHours = entries.Sum(Function(entry) entry.Hours)
        Dim daysWorked = entries.Select(Function(entry) entry.WorkDate.Date).Distinct().Count()

        lblPaySalary.Text = $"Monthly salary: {Session.CurrentUser.MonthlySalary:N2}"
        lblPayHours.Text = $"Hours worked: {totalHours:N2}"
        lblPayDays.Text = $"Days worked: {daysWorked}"
    End Function

    Private Async Sub btnAddEntry_Click(sender As Object, e As EventArgs) Handles btnAddEntry.Click
        Try
            UseWaitCursor = True
            Await WorkEntryRepository.AddAsync(
                Session.CurrentUser.Id, dtpWorkDate.Value.Date, numHours.Value, txtWorkNote.Text)
            txtWorkNote.Clear()
            Await LoadTimeAsync()
            Await LoadPayAsync()
        Catch ex As Exception
            MessageBox.Show(Me, "Could not add the work entry." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    Private Async Sub btnDeleteEntry_Click(sender As Object, e As EventArgs) Handles btnDeleteEntry.Click
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
            Dim deleted = Await WorkEntryRepository.DeleteAsync(entryId, Session.CurrentUser.Id)
            If Not deleted Then
                MessageBox.Show(Me, "The entry was not found. Refresh and try again.", "Not found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            Await LoadTimeAsync()
            Await LoadPayAsync()
        Catch ex As Exception
            MessageBox.Show(Me, "Could not delete the entry." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    Private Async Sub btnTimeRefresh_Click(sender As Object, e As EventArgs) Handles btnTimeRefresh.Click
        Await WithErrorAsync(AddressOf LoadTimeAsync, "Could not load your work time.")
    End Sub

    Private Async Sub btnPayRefresh_Click(sender As Object, e As EventArgs) Handles btnPayRefresh.Click
        Await WithErrorAsync(AddressOf RefreshPayAsync, "Could not load your pay.")
    End Sub

    Private Async Function RefreshPayAsync() As Task
        Await RefreshProfileAsync()
        Await LoadPayAsync()
    End Function

    Private Async Sub btnProfileRefresh_Click(sender As Object, e As EventArgs) Handles btnProfileRefresh.Click
        Await WithErrorAsync(AddressOf RefreshProfileAsync, "Could not load your profile.")
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
