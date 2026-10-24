<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EmployeeDashboardForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabProfile = New System.Windows.Forms.TabPage()
        Me.lblFullNameCaption = New System.Windows.Forms.Label()
        Me.lblFullName = New System.Windows.Forms.Label()
        Me.lblUsernameCaption = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.lblEmailCaption = New System.Windows.Forms.Label()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.lblPositionCaption = New System.Windows.Forms.Label()
        Me.lblPosition = New System.Windows.Forms.Label()
        Me.lblSalaryCaption = New System.Windows.Forms.Label()
        Me.lblSalary = New System.Windows.Forms.Label()
        Me.lblHireDateCaption = New System.Windows.Forms.Label()
        Me.lblHireDate = New System.Windows.Forms.Label()
        Me.lblStatusCaption = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblProfileHint = New System.Windows.Forms.Label()
        Me.btnProfileRefresh = New System.Windows.Forms.Button()
        Me.tabTime = New System.Windows.Forms.TabPage()
        Me.dgvTime = New System.Windows.Forms.DataGridView()
        Me.pnlTimeBottom = New System.Windows.Forms.Panel()
        Me.lblWorkDate = New System.Windows.Forms.Label()
        Me.dtpWorkDate = New System.Windows.Forms.DateTimePicker()
        Me.lblWorkHours = New System.Windows.Forms.Label()
        Me.numHours = New System.Windows.Forms.NumericUpDown()
        Me.lblWorkNote = New System.Windows.Forms.Label()
        Me.txtWorkNote = New System.Windows.Forms.TextBox()
        Me.btnAddEntry = New System.Windows.Forms.Button()
        Me.btnDeleteEntry = New System.Windows.Forms.Button()
        Me.pnlTimeTop = New System.Windows.Forms.Panel()
        Me.lblTimeMonth = New System.Windows.Forms.Label()
        Me.dtpTimeMonth = New System.Windows.Forms.DateTimePicker()
        Me.btnTimeRefresh = New System.Windows.Forms.Button()
        Me.tabPay = New System.Windows.Forms.TabPage()
        Me.lblPayMonth = New System.Windows.Forms.Label()
        Me.dtpPayMonth = New System.Windows.Forms.DateTimePicker()
        Me.btnPayRefresh = New System.Windows.Forms.Button()
        Me.picPay = New System.Windows.Forms.PictureBox()
        Me.lblPaySalary = New System.Windows.Forms.Label()
        Me.lblPayHours = New System.Windows.Forms.Label()
        Me.lblPayDays = New System.Windows.Forms.Label()
        Me.lblPayHint = New System.Windows.Forms.Label()
        Me.pnlTop.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabProfile.SuspendLayout()
        Me.tabTime.SuspendLayout()
        CType(Me.dgvTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTimeBottom.SuspendLayout()
        CType(Me.numHours, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTimeTop.SuspendLayout()
        Me.tabPay.SuspendLayout()
        CType(Me.picPay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.btnLogout)
        Me.pnlTop.Controls.Add(Me.lblUser)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(900, 52)
        Me.pnlTop.TabIndex = 0
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblUser.Location = New System.Drawing.Point(16, 17)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(100, 19)
        Me.lblUser.TabIndex = 0
        Me.lblUser.Text = "Signed in as"
        '
        'btnLogout
        '
        Me.btnLogout.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.btnLogout.Location = New System.Drawing.Point(780, 10)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(100, 32)
        Me.btnLogout.TabIndex = 1
        Me.btnLogout.Text = "Log out"
        Me.btnLogout.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me.tabProfile)
        Me.tabMain.Controls.Add(Me.tabTime)
        Me.tabMain.Controls.Add(Me.tabPay)
        Me.tabMain.Location = New System.Drawing.Point(0, 52)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(900, 568)
        Me.tabMain.TabIndex = 1
        '
        'tabProfile
        '
        Me.tabProfile.Controls.Add(Me.btnProfileRefresh)
        Me.tabProfile.Controls.Add(Me.lblProfileHint)
        Me.tabProfile.Controls.Add(Me.lblStatus)
        Me.tabProfile.Controls.Add(Me.lblStatusCaption)
        Me.tabProfile.Controls.Add(Me.lblHireDate)
        Me.tabProfile.Controls.Add(Me.lblHireDateCaption)
        Me.tabProfile.Controls.Add(Me.lblSalary)
        Me.tabProfile.Controls.Add(Me.lblSalaryCaption)
        Me.tabProfile.Controls.Add(Me.lblPosition)
        Me.tabProfile.Controls.Add(Me.lblPositionCaption)
        Me.tabProfile.Controls.Add(Me.lblEmail)
        Me.tabProfile.Controls.Add(Me.lblEmailCaption)
        Me.tabProfile.Controls.Add(Me.lblUsername)
        Me.tabProfile.Controls.Add(Me.lblUsernameCaption)
        Me.tabProfile.Controls.Add(Me.lblFullName)
        Me.tabProfile.Controls.Add(Me.lblFullNameCaption)
        Me.tabProfile.Location = New System.Drawing.Point(4, 24)
        Me.tabProfile.Name = "tabProfile"
        Me.tabProfile.Padding = New System.Windows.Forms.Padding(3)
        Me.tabProfile.Size = New System.Drawing.Size(892, 540)
        Me.tabProfile.TabIndex = 0
        Me.tabProfile.Text = "Profile"
        Me.tabProfile.UseVisualStyleBackColor = True
        '
        'lblFullNameCaption
        '
        Me.lblFullNameCaption.AutoSize = True
        Me.lblFullNameCaption.ForeColor = System.Drawing.Color.DimGray
        Me.lblFullNameCaption.Location = New System.Drawing.Point(24, 24)
        Me.lblFullNameCaption.Name = "lblFullNameCaption"
        Me.lblFullNameCaption.Size = New System.Drawing.Size(62, 15)
        Me.lblFullNameCaption.TabIndex = 0
        Me.lblFullNameCaption.Text = "Full name"
        '
        'lblFullName
        '
        Me.lblFullName.AutoSize = True
        Me.lblFullName.Font = New System.Drawing.Font("Segoe UI", 13.0!, System.Drawing.FontStyle.Bold)
        Me.lblFullName.Location = New System.Drawing.Point(170, 18)
        Me.lblFullName.Name = "lblFullName"
        Me.lblFullName.Size = New System.Drawing.Size(60, 25)
        Me.lblFullName.TabIndex = 1
        Me.lblFullName.Text = "-"
        '
        'lblUsernameCaption
        '
        Me.lblUsernameCaption.AutoSize = True
        Me.lblUsernameCaption.ForeColor = System.Drawing.Color.DimGray
        Me.lblUsernameCaption.Location = New System.Drawing.Point(24, 65)
        Me.lblUsernameCaption.Name = "lblUsernameCaption"
        Me.lblUsernameCaption.Size = New System.Drawing.Size(60, 15)
        Me.lblUsernameCaption.TabIndex = 2
        Me.lblUsernameCaption.Text = "Username"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(170, 65)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(15, 15)
        Me.lblUsername.TabIndex = 3
        Me.lblUsername.Text = "-"
        '
        'lblEmailCaption
        '
        Me.lblEmailCaption.AutoSize = True
        Me.lblEmailCaption.ForeColor = System.Drawing.Color.DimGray
        Me.lblEmailCaption.Location = New System.Drawing.Point(24, 100)
        Me.lblEmailCaption.Name = "lblEmailCaption"
        Me.lblEmailCaption.Size = New System.Drawing.Size(36, 15)
        Me.lblEmailCaption.TabIndex = 4
        Me.lblEmailCaption.Text = "Email"
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(170, 100)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(15, 15)
        Me.lblEmail.TabIndex = 5
        Me.lblEmail.Text = "-"
        '
        'lblPositionCaption
        '
        Me.lblPositionCaption.AutoSize = True
        Me.lblPositionCaption.ForeColor = System.Drawing.Color.DimGray
        Me.lblPositionCaption.Location = New System.Drawing.Point(24, 135)
        Me.lblPositionCaption.Name = "lblPositionCaption"
        Me.lblPositionCaption.Size = New System.Drawing.Size(50, 15)
        Me.lblPositionCaption.TabIndex = 6
        Me.lblPositionCaption.Text = "Position"
        '
        'lblPosition
        '
        Me.lblPosition.AutoSize = True
        Me.lblPosition.Location = New System.Drawing.Point(170, 135)
        Me.lblPosition.Name = "lblPosition"
        Me.lblPosition.Size = New System.Drawing.Size(15, 15)
        Me.lblPosition.TabIndex = 7
        Me.lblPosition.Text = "-"
        '
        'lblSalaryCaption
        '
        Me.lblSalaryCaption.AutoSize = True
        Me.lblSalaryCaption.ForeColor = System.Drawing.Color.DimGray
        Me.lblSalaryCaption.Location = New System.Drawing.Point(24, 170)
        Me.lblSalaryCaption.Name = "lblSalaryCaption"
        Me.lblSalaryCaption.Size = New System.Drawing.Size(92, 15)
        Me.lblSalaryCaption.TabIndex = 8
        Me.lblSalaryCaption.Text = "Monthly salary"
        '
        'lblSalary
        '
        Me.lblSalary.AutoSize = True
        Me.lblSalary.Location = New System.Drawing.Point(170, 170)
        Me.lblSalary.Name = "lblSalary"
        Me.lblSalary.Size = New System.Drawing.Size(15, 15)
        Me.lblSalary.TabIndex = 9
        Me.lblSalary.Text = "-"
        '
        'lblHireDateCaption
        '
        Me.lblHireDateCaption.AutoSize = True
        Me.lblHireDateCaption.ForeColor = System.Drawing.Color.DimGray
        Me.lblHireDateCaption.Location = New System.Drawing.Point(24, 205)
        Me.lblHireDateCaption.Name = "lblHireDateCaption"
        Me.lblHireDateCaption.Size = New System.Drawing.Size(55, 15)
        Me.lblHireDateCaption.TabIndex = 10
        Me.lblHireDateCaption.Text = "Hire date"
        '
        'lblHireDate
        '
        Me.lblHireDate.AutoSize = True
        Me.lblHireDate.Location = New System.Drawing.Point(170, 205)
        Me.lblHireDate.Name = "lblHireDate"
        Me.lblHireDate.Size = New System.Drawing.Size(15, 15)
        Me.lblHireDate.TabIndex = 11
        Me.lblHireDate.Text = "-"
        '
        'lblStatusCaption
        '
        Me.lblStatusCaption.AutoSize = True
        Me.lblStatusCaption.ForeColor = System.Drawing.Color.DimGray
        Me.lblStatusCaption.Location = New System.Drawing.Point(24, 240)
        Me.lblStatusCaption.Name = "lblStatusCaption"
        Me.lblStatusCaption.Size = New System.Drawing.Size(39, 15)
        Me.lblStatusCaption.TabIndex = 12
        Me.lblStatusCaption.Text = "Status"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(170, 240)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(15, 15)
        Me.lblStatus.TabIndex = 13
        Me.lblStatus.Text = "-"
        '
        'lblProfileHint
        '
        Me.lblProfileHint.ForeColor = System.Drawing.Color.DimGray
        Me.lblProfileHint.Location = New System.Drawing.Point(26, 285)
        Me.lblProfileHint.Name = "lblProfileHint"
        Me.lblProfileHint.Size = New System.Drawing.Size(480, 30)
        Me.lblProfileHint.TabIndex = 14
        Me.lblProfileHint.Text = "Contact HR to update your position, salary, or account status."
        '
        'btnProfileRefresh
        '
        Me.btnProfileRefresh.Location = New System.Drawing.Point(26, 325)
        Me.btnProfileRefresh.Name = "btnProfileRefresh"
        Me.btnProfileRefresh.Size = New System.Drawing.Size(120, 30)
        Me.btnProfileRefresh.TabIndex = 15
        Me.btnProfileRefresh.Text = "Refresh"
        Me.btnProfileRefresh.UseVisualStyleBackColor = True
        '
        'tabTime
        '
        Me.tabTime.Controls.Add(Me.dgvTime)
        Me.tabTime.Controls.Add(Me.pnlTimeBottom)
        Me.tabTime.Controls.Add(Me.pnlTimeTop)
        Me.tabTime.Location = New System.Drawing.Point(4, 24)
        Me.tabTime.Name = "tabTime"
        Me.tabTime.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTime.Size = New System.Drawing.Size(892, 540)
        Me.tabTime.TabIndex = 1
        Me.tabTime.Text = "Work Time"
        Me.tabTime.UseVisualStyleBackColor = True
        '
        'dgvTime
        '
        Me.dgvTime.AllowUserToAddRows = False
        Me.dgvTime.AllowUserToDeleteRows = False
        Me.dgvTime.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTime.BackgroundColor = System.Drawing.Color.White
        Me.dgvTime.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTime.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTime.Location = New System.Drawing.Point(3, 49)
        Me.dgvTime.MultiSelect = False
        Me.dgvTime.Name = "dgvTime"
        Me.dgvTime.ReadOnly = True
        Me.dgvTime.RowHeadersVisible = False
        Me.dgvTime.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTime.Size = New System.Drawing.Size(886, 436)
        Me.dgvTime.TabIndex = 2
        '
        'pnlTimeBottom
        '
        Me.pnlTimeBottom.Controls.Add(Me.lblWorkDate)
        Me.pnlTimeBottom.Controls.Add(Me.dtpWorkDate)
        Me.pnlTimeBottom.Controls.Add(Me.lblWorkHours)
        Me.pnlTimeBottom.Controls.Add(Me.numHours)
        Me.pnlTimeBottom.Controls.Add(Me.lblWorkNote)
        Me.pnlTimeBottom.Controls.Add(Me.txtWorkNote)
        Me.pnlTimeBottom.Controls.Add(Me.btnAddEntry)
        Me.pnlTimeBottom.Controls.Add(Me.btnDeleteEntry)
        Me.pnlTimeBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlTimeBottom.Location = New System.Drawing.Point(3, 485)
        Me.pnlTimeBottom.Name = "pnlTimeBottom"
        Me.pnlTimeBottom.Size = New System.Drawing.Size(886, 52)
        Me.pnlTimeBottom.TabIndex = 1
        '
        'lblWorkDate
        '
        Me.lblWorkDate.AutoSize = True
        Me.lblWorkDate.Location = New System.Drawing.Point(8, 18)
        Me.lblWorkDate.Name = "lblWorkDate"
        Me.lblWorkDate.Size = New System.Drawing.Size(31, 15)
        Me.lblWorkDate.TabIndex = 0
        Me.lblWorkDate.Text = "Date"
        '
        'dtpWorkDate
        '
        Me.dtpWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpWorkDate.Location = New System.Drawing.Point(45, 14)
        Me.dtpWorkDate.Name = "dtpWorkDate"
        Me.dtpWorkDate.Size = New System.Drawing.Size(120, 23)
        Me.dtpWorkDate.TabIndex = 1
        '
        'lblWorkHours
        '
        Me.lblWorkHours.AutoSize = True
        Me.lblWorkHours.Location = New System.Drawing.Point(183, 18)
        Me.lblWorkHours.Name = "lblWorkHours"
        Me.lblWorkHours.Size = New System.Drawing.Size(39, 15)
        Me.lblWorkHours.TabIndex = 2
        Me.lblWorkHours.Text = "Hours"
        '
        'numHours
        '
        Me.numHours.DecimalPlaces = 2
        Me.numHours.Increment = New Decimal(New Integer() {25, 0, 0, 131072})
        Me.numHours.Location = New System.Drawing.Point(228, 14)
        Me.numHours.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
        Me.numHours.Minimum = New Decimal(New Integer() {25, 0, 0, 131072})
        Me.numHours.Name = "numHours"
        Me.numHours.Size = New System.Drawing.Size(70, 23)
        Me.numHours.TabIndex = 3
        Me.numHours.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'lblWorkNote
        '
        Me.lblWorkNote.AutoSize = True
        Me.lblWorkNote.Location = New System.Drawing.Point(316, 18)
        Me.lblWorkNote.Name = "lblWorkNote"
        Me.lblWorkNote.Size = New System.Drawing.Size(33, 15)
        Me.lblWorkNote.TabIndex = 4
        Me.lblWorkNote.Text = "Note"
        '
        'txtWorkNote
        '
        Me.txtWorkNote.Location = New System.Drawing.Point(355, 14)
        Me.txtWorkNote.MaxLength = 255
        Me.txtWorkNote.Name = "txtWorkNote"
        Me.txtWorkNote.Size = New System.Drawing.Size(250, 23)
        Me.txtWorkNote.TabIndex = 5
        '
        'btnAddEntry
        '
        Me.btnAddEntry.Location = New System.Drawing.Point(620, 10)
        Me.btnAddEntry.Name = "btnAddEntry"
        Me.btnAddEntry.Size = New System.Drawing.Size(120, 30)
        Me.btnAddEntry.TabIndex = 6
        Me.btnAddEntry.Text = "Add entry"
        Me.btnAddEntry.UseVisualStyleBackColor = True
        '
        'btnDeleteEntry
        '
        Me.btnDeleteEntry.Location = New System.Drawing.Point(748, 10)
        Me.btnDeleteEntry.Name = "btnDeleteEntry"
        Me.btnDeleteEntry.Size = New System.Drawing.Size(120, 30)
        Me.btnDeleteEntry.TabIndex = 7
        Me.btnDeleteEntry.Text = "Delete selected"
        Me.btnDeleteEntry.UseVisualStyleBackColor = True
        '
        'pnlTimeTop
        '
        Me.pnlTimeTop.Controls.Add(Me.lblTimeMonth)
        Me.pnlTimeTop.Controls.Add(Me.dtpTimeMonth)
        Me.pnlTimeTop.Controls.Add(Me.btnTimeRefresh)
        Me.pnlTimeTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTimeTop.Location = New System.Drawing.Point(3, 3)
        Me.pnlTimeTop.Name = "pnlTimeTop"
        Me.pnlTimeTop.Size = New System.Drawing.Size(886, 46)
        Me.pnlTimeTop.TabIndex = 0
        '
        'lblTimeMonth
        '
        Me.lblTimeMonth.AutoSize = True
        Me.lblTimeMonth.Location = New System.Drawing.Point(8, 16)
        Me.lblTimeMonth.Name = "lblTimeMonth"
        Me.lblTimeMonth.Size = New System.Drawing.Size(43, 15)
        Me.lblTimeMonth.TabIndex = 0
        Me.lblTimeMonth.Text = "Month"
        '
        'dtpTimeMonth
        '
        Me.dtpTimeMonth.CustomFormat = "MMMM yyyy"
        Me.dtpTimeMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTimeMonth.Location = New System.Drawing.Point(57, 12)
        Me.dtpTimeMonth.Name = "dtpTimeMonth"
        Me.dtpTimeMonth.ShowUpDown = True
        Me.dtpTimeMonth.Size = New System.Drawing.Size(140, 23)
        Me.dtpTimeMonth.TabIndex = 1
        '
        'btnTimeRefresh
        '
        Me.btnTimeRefresh.Location = New System.Drawing.Point(210, 10)
        Me.btnTimeRefresh.Name = "btnTimeRefresh"
        Me.btnTimeRefresh.Size = New System.Drawing.Size(95, 27)
        Me.btnTimeRefresh.TabIndex = 2
        Me.btnTimeRefresh.Text = "Refresh"
        Me.btnTimeRefresh.UseVisualStyleBackColor = True
        '
        'tabPay
        '
        Me.tabPay.Controls.Add(Me.lblPayMonth)
        Me.tabPay.Controls.Add(Me.dtpPayMonth)
        Me.tabPay.Controls.Add(Me.btnPayRefresh)
        Me.tabPay.Controls.Add(Me.picPay)
        Me.tabPay.Controls.Add(Me.lblPaySalary)
        Me.tabPay.Controls.Add(Me.lblPayHours)
        Me.tabPay.Controls.Add(Me.lblPayDays)
        Me.tabPay.Controls.Add(Me.lblPayHint)
        Me.tabPay.Location = New System.Drawing.Point(4, 24)
        Me.tabPay.Name = "tabPay"
        Me.tabPay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPay.Size = New System.Drawing.Size(892, 540)
        Me.tabPay.TabIndex = 2
        Me.tabPay.Text = "Pay"
        Me.tabPay.UseVisualStyleBackColor = True
        '
        'lblPayMonth
        '
        Me.lblPayMonth.AutoSize = True
        Me.lblPayMonth.Location = New System.Drawing.Point(24, 26)
        Me.lblPayMonth.Name = "lblPayMonth"
        Me.lblPayMonth.Size = New System.Drawing.Size(43, 15)
        Me.lblPayMonth.TabIndex = 0
        Me.lblPayMonth.Text = "Month"
        '
        'dtpPayMonth
        '
        Me.dtpPayMonth.CustomFormat = "MMMM yyyy"
        Me.dtpPayMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpPayMonth.Location = New System.Drawing.Point(73, 22)
        Me.dtpPayMonth.Name = "dtpPayMonth"
        Me.dtpPayMonth.ShowUpDown = True
        Me.dtpPayMonth.Size = New System.Drawing.Size(140, 23)
        Me.dtpPayMonth.TabIndex = 1
        '
        'btnPayRefresh
        '
        Me.btnPayRefresh.Location = New System.Drawing.Point(226, 20)
        Me.btnPayRefresh.Name = "btnPayRefresh"
        Me.btnPayRefresh.Size = New System.Drawing.Size(95, 27)
        Me.btnPayRefresh.TabIndex = 2
        Me.btnPayRefresh.Text = "Refresh"
        Me.btnPayRefresh.UseVisualStyleBackColor = True
        '
        'picPay
        '
        Me.picPay.Location = New System.Drawing.Point(24, 75)
        Me.picPay.Name = "picPay"
        Me.picPay.Size = New System.Drawing.Size(56, 56)
        Me.picPay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picPay.TabIndex = 7
        Me.picPay.TabStop = False
        '
        'lblPaySalary
        '
        Me.lblPaySalary.AutoSize = True
        Me.lblPaySalary.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblPaySalary.Location = New System.Drawing.Point(95, 88)
        Me.lblPaySalary.Name = "lblPaySalary"
        Me.lblPaySalary.Size = New System.Drawing.Size(220, 25)
        Me.lblPaySalary.TabIndex = 3
        Me.lblPaySalary.Text = "Monthly salary: 0.00"
        '
        'lblPayHours
        '
        Me.lblPayHours.AutoSize = True
        Me.lblPayHours.Location = New System.Drawing.Point(26, 150)
        Me.lblPayHours.Name = "lblPayHours"
        Me.lblPayHours.Size = New System.Drawing.Size(130, 15)
        Me.lblPayHours.TabIndex = 4
        Me.lblPayHours.Text = "Hours worked: 0.00"
        '
        'lblPayDays
        '
        Me.lblPayDays.AutoSize = True
        Me.lblPayDays.Location = New System.Drawing.Point(26, 175)
        Me.lblPayDays.Name = "lblPayDays"
        Me.lblPayDays.Size = New System.Drawing.Size(100, 15)
        Me.lblPayDays.TabIndex = 5
        Me.lblPayDays.Text = "Days worked: 0"
        '
        'lblPayHint
        '
        Me.lblPayHint.ForeColor = System.Drawing.Color.DimGray
        Me.lblPayHint.Location = New System.Drawing.Point(26, 210)
        Me.lblPayHint.Name = "lblPayHint"
        Me.lblPayHint.Size = New System.Drawing.Size(500, 30)
        Me.lblPayHint.TabIndex = 6
        Me.lblPayHint.Text = "Salary is fixed monthly and is not reduced by the number of hours worked."
        '
        'EmployeeDashboardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 620)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.pnlTop)
        Me.MinimumSize = New System.Drawing.Size(800, 560)
        Me.Name = "EmployeeDashboardForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Dashboard"
        CType(Me.picPay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabProfile.ResumeLayout(False)
        Me.tabProfile.PerformLayout()
        Me.tabTime.ResumeLayout(False)
        CType(Me.dgvTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTimeBottom.ResumeLayout(False)
        Me.pnlTimeBottom.PerformLayout()
        CType(Me.numHours, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTimeTop.ResumeLayout(False)
        Me.pnlTimeTop.PerformLayout()
        Me.tabPay.ResumeLayout(False)
        Me.tabPay.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents btnLogout As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabProfile As System.Windows.Forms.TabPage
    Friend WithEvents lblFullNameCaption As System.Windows.Forms.Label
    Friend WithEvents lblFullName As System.Windows.Forms.Label
    Friend WithEvents lblUsernameCaption As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents lblEmailCaption As System.Windows.Forms.Label
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents lblPositionCaption As System.Windows.Forms.Label
    Friend WithEvents lblPosition As System.Windows.Forms.Label
    Friend WithEvents lblSalaryCaption As System.Windows.Forms.Label
    Friend WithEvents lblSalary As System.Windows.Forms.Label
    Friend WithEvents lblHireDateCaption As System.Windows.Forms.Label
    Friend WithEvents lblHireDate As System.Windows.Forms.Label
    Friend WithEvents lblStatusCaption As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblProfileHint As System.Windows.Forms.Label
    Friend WithEvents btnProfileRefresh As System.Windows.Forms.Button
    Friend WithEvents tabTime As System.Windows.Forms.TabPage
    Friend WithEvents dgvTime As System.Windows.Forms.DataGridView
    Friend WithEvents pnlTimeBottom As System.Windows.Forms.Panel
    Friend WithEvents lblWorkDate As System.Windows.Forms.Label
    Friend WithEvents dtpWorkDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblWorkHours As System.Windows.Forms.Label
    Friend WithEvents numHours As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblWorkNote As System.Windows.Forms.Label
    Friend WithEvents txtWorkNote As System.Windows.Forms.TextBox
    Friend WithEvents btnAddEntry As System.Windows.Forms.Button
    Friend WithEvents btnDeleteEntry As System.Windows.Forms.Button
    Friend WithEvents pnlTimeTop As System.Windows.Forms.Panel
    Friend WithEvents lblTimeMonth As System.Windows.Forms.Label
    Friend WithEvents dtpTimeMonth As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnTimeRefresh As System.Windows.Forms.Button
    Friend WithEvents tabPay As System.Windows.Forms.TabPage
    Friend WithEvents lblPayMonth As System.Windows.Forms.Label
    Friend WithEvents dtpPayMonth As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnPayRefresh As System.Windows.Forms.Button
    Friend WithEvents picPay As System.Windows.Forms.PictureBox
    Friend WithEvents lblPaySalary As System.Windows.Forms.Label
    Friend WithEvents lblPayHours As System.Windows.Forms.Label
    Friend WithEvents lblPayDays As System.Windows.Forms.Label
    Friend WithEvents lblPayHint As System.Windows.Forms.Label
End Class
