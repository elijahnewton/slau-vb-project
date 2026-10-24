Imports System.Diagnostics

Public Class LoginForm
    Private _ready As Boolean

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Icon = UiIcons.AppIcon()
        picLogo.Image = UiIcons.GetIcon("app", 48)
        UiIcons.StyleButton(btnLogin, "profile", 20)
    End Sub

    Private Async Sub LoginForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Await InitializeDatabaseAsync()
    End Sub

    Private Async Function InitializeDatabaseAsync() As Task
        btnLogin.Enabled = False
        lblStatus.Text = "Connecting to database..."

        Try
            Await Db.InitializeAsync()
            _ready = True
            btnLogin.Enabled = True
            lblStatus.Text = "Connected."
            txtUsername.Focus()
        Catch ex As Exception
            _ready = False
            lblStatus.Text = "Database unavailable."
            MessageBox.Show(
                Me,
                "Could not initialize the database." & Environment.NewLine & Environment.NewLine & ex.Message,
                "Database error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try
    End Function

    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If Not _ready Then
            MessageBox.Show(Me, "The database is not available. Check appsettings.json and restart.",
                            "Not connected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim username = txtUsername.Text.Trim()
        Dim password = txtPassword.Text

        If username.Length = 0 OrElse password.Length = 0 Then
            MessageBox.Show(Me, "Enter your username and password.", "Missing details",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        SetBusy(True)

        Try
            Dim user = Await EmployeeRepository.AuthenticateAsync(username)

            If user Is Nothing OrElse Not PasswordHasher.Verify(password, user.PasswordHash, user.PasswordSalt) Then
                MessageBox.Show(Me, "Invalid username or password.", "Sign in failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.SelectAll()
                txtPassword.Focus()
                Return
            End If

            If user.Status = AccountStatus.Pending Then
                MessageBox.Show(Me, "Your account is waiting for HR approval.", "Account pending",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If user.Status = AccountStatus.Inactive Then
                MessageBox.Show(Me, "Your account has been deactivated. Contact HR.", "Account inactive",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Session.CurrentUser = user
            ShowDashboard(user)
        Catch ex As Exception
            MessageBox.Show(Me, "Sign in failed." & Environment.NewLine & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
    End Sub

    Private Sub ShowDashboard(user As Employee)
        Me.Hide()
        Dim dashboard As Form = Nothing

        Try
            If user.Role = UserRole.Hr Then
                dashboard = New HrDashboardForm()
            Else
                dashboard = New EmployeeDashboardForm()
            End If

            dashboard.ShowDialog()
        Finally
            If dashboard IsNot Nothing Then dashboard.Dispose()
            Session.CurrentUser = Nothing
            txtPassword.Clear()
            Me.Show()
            txtUsername.Focus()
        End Try
    End Sub

    Private Sub lnkSignup_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkSignup.LinkClicked
        If Not _ready Then
            MessageBox.Show(Me, "The database is not available. Check appsettings.json and restart.",
                            "Not connected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using form As New SignupForm()
            If form.ShowDialog(Me) = DialogResult.OK Then
                txtUsername.Text = form.CreatedUsername
                txtPassword.Clear()
                txtPassword.Focus()
            End If
        End Using
    End Sub

    Private Sub lnkIcons_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkIcons.LinkClicked
        Try
            Process.Start(New ProcessStartInfo("https://www.flaticon.com") With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show(Me, "Could not open the browser." & Environment.NewLine & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetBusy(busy As Boolean)
        btnLogin.Enabled = Not busy AndAlso _ready
        txtUsername.Enabled = Not busy
        txtPassword.Enabled = Not busy
        lnkSignup.Enabled = Not busy
        Cursor = If(busy, Cursors.WaitCursor, Cursors.Default)
    End Sub
End Class
