Imports MySqlConnector

Public Class SignupForm
    Public ReadOnly Property CreatedUsername As String
        Get
            Return _createdUsername
        End Get
    End Property

    Private Sub SignupForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Icon = UiIcons.AppIcon()
        UiIcons.StyleButton(btnCreate, "add", 18)
    End Sub

    Private Async Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim fullName = txtFullName.Text.Trim()
        Dim username = txtUsername.Text.Trim()
        Dim email = txtEmail.Text.Trim()
        Dim password = txtPassword.Text
        Dim confirm = txtConfirm.Text

        If fullName.Length < 2 Then
            MessageBox.Show(Me, "Enter your full name.", "Missing details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFullName.Focus()
            Return
        End If

        If username.Length < 3 Then
            MessageBox.Show(Me, "Username must be at least 3 characters.", "Missing details",
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

        If password.Length < 8 Then
            MessageBox.Show(Me, "Password must be at least 8 characters.", "Weak password",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Focus()
            Return
        End If

        If password <> confirm Then
            MessageBox.Show(Me, "Passwords do not match.", "Passwords do not match",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtConfirm.Focus()
            Return
        End If

        Dim employee As New Employee With {
            .FullName = fullName,
            .Username = username,
            .Email = email,
            .Role = UserRole.Employee,
            .Status = AccountStatus.Pending
        }

        SetBusy(True)

        Try
            Await EmployeeRepository.InsertAsync(employee, password)
            MessageBox.Show(Me, "Account created. HR must approve it before you can sign in.",
                            "Account created", MessageBoxButtons.OK, MessageBoxIcon.Information)
            _createdUsername = username
            DialogResult = DialogResult.OK
            Close()
        Catch ex As MySqlException When ex.Number = 1062
            MessageBox.Show(Me, "That username is already taken.", "Username unavailable",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsername.Focus()
        Catch ex As Exception
            MessageBox.Show(Me, "Could not create the account." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
    End Sub

    Private _createdUsername As String = ""

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub SetBusy(busy As Boolean)
        btnCreate.Enabled = Not busy
        btnCancel.Enabled = Not busy
        Cursor = If(busy, Cursors.WaitCursor, Cursors.Default)
    End Sub
End Class
