Imports MySqlConnector

Public Class EmployeeEditForm
    Private ReadOnly _employee As Employee
    Private ReadOnly _isEdit As Boolean

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Sub New(employee As Employee)
        InitializeComponent()
        _employee = employee
        _isEdit = employee IsNot Nothing
    End Sub

    Private Sub EmployeeEditForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Icon = UiIcons.AppIcon()
        UiIcons.StyleButton(btnSave, "approve", 18)

        cmbRole.Items.Clear()
        cmbRole.Items.Add("Employee")
        cmbRole.Items.Add("HR")

        cmbStatus.Items.Clear()
        cmbStatus.Items.Add("Pending")
        cmbStatus.Items.Add("Active")
        cmbStatus.Items.Add("Inactive")

        If _isEdit Then
            Text = "Edit Account"
            lblTitle.Text = "Edit Account"
            txtFullName.Text = _employee.FullName
            txtUsername.Text = _employee.Username
            txtEmail.Text = _employee.Email
            txtPosition.Text = _employee.Position
            cmbRole.SelectedIndex = If(_employee.Role = UserRole.Hr, 1, 0)
            cmbStatus.SelectedIndex = StatusIndex(_employee.Status)

            Dim salary = Math.Min(Math.Max(_employee.MonthlySalary, numSalary.Minimum), numSalary.Maximum)
            numSalary.Value = salary

            If _employee.HireDate.HasValue Then
                dtpHireDate.Value = _employee.HireDate.Value
                dtpHireDate.Checked = True
            Else
                dtpHireDate.Checked = False
            End If

            pnlPassword.Visible = False
            ClientSize = New Size(ClientSize.Width, 500)
            btnSave.Top = 430
            btnCancel.Top = 430
        Else
            Text = "Add Account"
            lblTitle.Text = "Add Account"
            cmbRole.SelectedIndex = 0
            cmbStatus.SelectedIndex = 1
        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim fullName = txtFullName.Text.Trim()
        Dim username = txtUsername.Text.Trim()
        Dim email = txtEmail.Text.Trim()
        Dim position = txtPosition.Text.Trim()
        Dim role = If(cmbRole.SelectedIndex = 1, UserRole.Hr, UserRole.Employee)
        Dim status = SelectedStatus()

        If fullName.Length < 2 Then
            MessageBox.Show(Me, "Enter a full name.", "Missing details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFullName.Focus()
            Return
        End If

        If username.Length < 3 Then
            MessageBox.Show(Me, "Username must be at least 3 characters.", "Invalid username",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsername.Focus()
            Return
        End If

        If username.IndexOfAny(New Char() {" "c, ControlChars.Tab}) >= 0 Then
            MessageBox.Show(Me, "Username cannot contain spaces.", "Invalid username",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsername.Focus()
            Return
        End If

        If email.Length > 0 AndAlso Not email.Contains("@") Then
            MessageBox.Show(Me, "Enter a valid email address or leave the field empty.", "Invalid email",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEmail.Focus()
            Return
        End If

        If _isEdit AndAlso _employee.Id = Session.CurrentUser.Id Then
            If role <> UserRole.Hr Then
                MessageBox.Show(Me, "You cannot remove your own HR role.", "Not allowed",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If status <> AccountStatus.Active Then
                MessageBox.Show(Me, "You cannot deactivate your own account.", "Not allowed",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        Dim password = ""
        If Not _isEdit Then
            password = txtPassword.Text
            If password.Length < 8 Then
                MessageBox.Show(Me, "Password must be at least 8 characters.", "Weak password",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.Focus()
                Return
            End If

            If password <> txtConfirm.Text Then
                MessageBox.Show(Me, "Passwords do not match.", "Passwords do not match",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtConfirm.Focus()
                Return
            End If
        End If

        Dim hireDate As Date? = Nothing
        If dtpHireDate.Checked Then hireDate = dtpHireDate.Value.Date

        Dim target As New Employee With {
            .Id = If(_isEdit, _employee.Id, 0),
            .FullName = fullName,
            .Username = username,
            .Email = email,
            .Role = role,
            .Position = position,
            .MonthlySalary = numSalary.Value,
            .HireDate = hireDate,
            .Status = status
        }

        SetBusy(True)

        Try
            If _isEdit Then
                Await EmployeeRepository.UpdateAsync(target)
            Else
                Await EmployeeRepository.InsertAsync(target, password)
            End If

            DialogResult = DialogResult.OK
            Close()
        Catch ex As MySqlException When ex.Number = 1062
            MessageBox.Show(Me, "That username is already taken.", "Username unavailable",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsername.Focus()
        Catch ex As Exception
            MessageBox.Show(Me, "Could not save the account." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
    End Sub

    Private Function SelectedStatus() As AccountStatus
        Select Case cmbStatus.SelectedIndex
            Case 1
                Return AccountStatus.Active
            Case 2
                Return AccountStatus.Inactive
            Case Else
                Return AccountStatus.Pending
        End Select
    End Function

    Private Function StatusIndex(status As AccountStatus) As Integer
        Select Case status
            Case AccountStatus.Active
                Return 1
            Case AccountStatus.Inactive
                Return 2
            Case Else
                Return 0
        End Select
    End Function

    Private Sub SetBusy(busy As Boolean)
        btnSave.Enabled = Not busy
        btnCancel.Enabled = Not busy
        UseWaitCursor = busy
    End Sub
End Class
