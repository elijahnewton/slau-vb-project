<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HrDashboardForm
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
        Me.tabEmployees = New System.Windows.Forms.TabPage()
        Me.dgvEmployees = New System.Windows.Forms.DataGridView()
        Me.pnlEmployeesTop = New System.Windows.Forms.Panel()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnRefreshEmployees = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnApprove = New System.Windows.Forms.Button()
        Me.btnDeactivate = New System.Windows.Forms.Button()
        Me.tabTime = New System.Windows.Forms.TabPage()
        Me.dgvTime = New System.Windows.Forms.DataGridView()
        Me.pnlTimeTop = New System.Windows.Forms.Panel()
        Me.lblEmployee = New System.Windows.Forms.Label()
        Me.cmbEmployee = New System.Windows.Forms.ComboBox()
        Me.lblTimeMonth = New System.Windows.Forms.Label()
        Me.dtpTimeMonth = New System.Windows.Forms.DateTimePicker()
        Me.btnTimeRefresh = New System.Windows.Forms.Button()
        Me.btnTimeDelete = New System.Windows.Forms.Button()
        Me.tabPayroll = New System.Windows.Forms.TabPage()
        Me.dgvPayroll = New System.Windows.Forms.DataGridView()
        Me.pnlPayrollTop = New System.Windows.Forms.Panel()
        Me.lblPayrollMonth = New System.Windows.Forms.Label()
        Me.dtpPayrollMonth = New System.Windows.Forms.DateTimePicker()
        Me.btnPayrollLoad = New System.Windows.Forms.Button()
        Me.lblPayrollTotal = New System.Windows.Forms.Label()
        Me.pnlTop.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabEmployees.SuspendLayout()
        CType(Me.dgvEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEmployeesTop.SuspendLayout()
        Me.tabTime.SuspendLayout()
        CType(Me.dgvTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTimeTop.SuspendLayout()
        Me.tabPayroll.SuspendLayout()
        CType(Me.dgvPayroll, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPayrollTop.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.btnLogout)
        Me.pnlTop.Controls.Add(Me.lblUser)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1040, 52)
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
        Me.btnLogout.Location = New System.Drawing.Point(920, 10)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(100, 32)
        Me.btnLogout.TabIndex = 1
        Me.btnLogout.Text = "Log out"
        Me.btnLogout.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me.tabEmployees)
        Me.tabMain.Controls.Add(Me.tabTime)
        Me.tabMain.Controls.Add(Me.tabPayroll)
        Me.tabMain.Location = New System.Drawing.Point(0, 52)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1040, 628)
        Me.tabMain.TabIndex = 1
        '
        'tabEmployees
        '
        Me.tabEmployees.Controls.Add(Me.dgvEmployees)
        Me.tabEmployees.Controls.Add(Me.pnlEmployeesTop)
        Me.tabEmployees.Location = New System.Drawing.Point(4, 24)
        Me.tabEmployees.Name = "tabEmployees"
        Me.tabEmployees.Padding = New System.Windows.Forms.Padding(3)
        Me.tabEmployees.Size = New System.Drawing.Size(1032, 600)
        Me.tabEmployees.TabIndex = 0
        Me.tabEmployees.Text = "Employees"
        Me.tabEmployees.UseVisualStyleBackColor = True
        '
        'dgvEmployees
        '
        Me.dgvEmployees.AllowUserToAddRows = False
        Me.dgvEmployees.AllowUserToDeleteRows = False
        Me.dgvEmployees.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.dgvEmployees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvEmployees.BackgroundColor = System.Drawing.Color.White
        Me.dgvEmployees.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEmployees.Location = New System.Drawing.Point(0, 46)
        Me.dgvEmployees.MultiSelect = False
        Me.dgvEmployees.Name = "dgvEmployees"
        Me.dgvEmployees.ReadOnly = True
        Me.dgvEmployees.RowHeadersVisible = False
        Me.dgvEmployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmployees.Size = New System.Drawing.Size(1032, 554)
        Me.dgvEmployees.TabIndex = 1
        '
        'pnlEmployeesTop
        '
        Me.pnlEmployeesTop.Controls.Add(Me.txtSearch)
        Me.pnlEmployeesTop.Controls.Add(Me.btnSearch)
        Me.pnlEmployeesTop.Controls.Add(Me.btnRefreshEmployees)
        Me.pnlEmployeesTop.Controls.Add(Me.btnAdd)
        Me.pnlEmployeesTop.Controls.Add(Me.btnEdit)
        Me.pnlEmployeesTop.Controls.Add(Me.btnApprove)
        Me.pnlEmployeesTop.Controls.Add(Me.btnDeactivate)
        Me.pnlEmployeesTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlEmployeesTop.Location = New System.Drawing.Point(3, 3)
        Me.pnlEmployeesTop.Name = "pnlEmployeesTop"
        Me.pnlEmployeesTop.Size = New System.Drawing.Size(1026, 43)
        Me.pnlEmployeesTop.TabIndex = 0
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(6, 9)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(220, 23)
        Me.txtSearch.TabIndex = 0
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(232, 8)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(85, 26)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnRefreshEmployees
        '
        Me.btnRefreshEmployees.Location = New System.Drawing.Point(325, 8)
        Me.btnRefreshEmployees.Name = "btnRefreshEmployees"
        Me.btnRefreshEmployees.Size = New System.Drawing.Size(95, 26)
        Me.btnRefreshEmployees.TabIndex = 2
        Me.btnRefreshEmployees.Text = "Refresh"
        Me.btnRefreshEmployees.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(428, 8)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(90, 26)
        Me.btnAdd.TabIndex = 3
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(526, 8)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 26)
        Me.btnEdit.TabIndex = 4
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnApprove
        '
        Me.btnApprove.Location = New System.Drawing.Point(609, 8)
        Me.btnApprove.Name = "btnApprove"
        Me.btnApprove.Size = New System.Drawing.Size(85, 26)
        Me.btnApprove.TabIndex = 5
        Me.btnApprove.Text = "Approve"
        Me.btnApprove.UseVisualStyleBackColor = True
        '
        'btnDeactivate
        '
        Me.btnDeactivate.Location = New System.Drawing.Point(702, 8)
        Me.btnDeactivate.Name = "btnDeactivate"
        Me.btnDeactivate.Size = New System.Drawing.Size(110, 26)
        Me.btnDeactivate.TabIndex = 6
        Me.btnDeactivate.Text = "Deactivate"
        Me.btnDeactivate.UseVisualStyleBackColor = True
        '
        'tabTime
        '
        Me.tabTime.Controls.Add(Me.dgvTime)
        Me.tabTime.Controls.Add(Me.pnlTimeTop)
        Me.tabTime.Location = New System.Drawing.Point(4, 24)
        Me.tabTime.Name = "tabTime"
        Me.tabTime.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTime.Size = New System.Drawing.Size(1032, 600)
        Me.tabTime.TabIndex = 1
        Me.tabTime.Text = "Work Time"
        Me.tabTime.UseVisualStyleBackColor = True
        '
        'dgvTime
        '
        Me.dgvTime.AllowUserToAddRows = False
        Me.dgvTime.AllowUserToDeleteRows = False
        Me.dgvTime.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.dgvTime.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTime.BackgroundColor = System.Drawing.Color.White
        Me.dgvTime.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTime.Location = New System.Drawing.Point(0, 46)
        Me.dgvTime.MultiSelect = False
        Me.dgvTime.Name = "dgvTime"
        Me.dgvTime.ReadOnly = True
        Me.dgvTime.RowHeadersVisible = False
        Me.dgvTime.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTime.Size = New System.Drawing.Size(1032, 554)
        Me.dgvTime.TabIndex = 1
        '
        'pnlTimeTop
        '
        Me.pnlTimeTop.Controls.Add(Me.lblEmployee)
        Me.pnlTimeTop.Controls.Add(Me.cmbEmployee)
        Me.pnlTimeTop.Controls.Add(Me.lblTimeMonth)
        Me.pnlTimeTop.Controls.Add(Me.dtpTimeMonth)
        Me.pnlTimeTop.Controls.Add(Me.btnTimeRefresh)
        Me.pnlTimeTop.Controls.Add(Me.btnTimeDelete)
        Me.pnlTimeTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTimeTop.Location = New System.Drawing.Point(3, 3)
        Me.pnlTimeTop.Name = "pnlTimeTop"
        Me.pnlTimeTop.Size = New System.Drawing.Size(1026, 43)
        Me.pnlTimeTop.TabIndex = 0
        '
        'lblEmployee
        '
        Me.lblEmployee.AutoSize = True
        Me.lblEmployee.Location = New System.Drawing.Point(6, 13)
        Me.lblEmployee.Name = "lblEmployee"
        Me.lblEmployee.Size = New System.Drawing.Size(61, 15)
        Me.lblEmployee.TabIndex = 0
        Me.lblEmployee.Text = "Employee"
        '
        'cmbEmployee
        '
        Me.cmbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEmployee.Location = New System.Drawing.Point(73, 9)
        Me.cmbEmployee.Name = "cmbEmployee"
        Me.cmbEmployee.Size = New System.Drawing.Size(220, 23)
        Me.cmbEmployee.TabIndex = 1
        '
        'lblTimeMonth
        '
        Me.lblTimeMonth.AutoSize = True
        Me.lblTimeMonth.Location = New System.Drawing.Point(310, 13)
        Me.lblTimeMonth.Name = "lblTimeMonth"
        Me.lblTimeMonth.Size = New System.Drawing.Size(43, 15)
        Me.lblTimeMonth.TabIndex = 2
        Me.lblTimeMonth.Text = "Month"
        '
        'dtpTimeMonth
        '
        Me.dtpTimeMonth.CustomFormat = "MMMM yyyy"
        Me.dtpTimeMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTimeMonth.Location = New System.Drawing.Point(359, 9)
        Me.dtpTimeMonth.Name = "dtpTimeMonth"
        Me.dtpTimeMonth.ShowUpDown = True
        Me.dtpTimeMonth.Size = New System.Drawing.Size(140, 23)
        Me.dtpTimeMonth.TabIndex = 3
        '
        'btnTimeRefresh
        '
        Me.btnTimeRefresh.Location = New System.Drawing.Point(512, 8)
        Me.btnTimeRefresh.Name = "btnTimeRefresh"
        Me.btnTimeRefresh.Size = New System.Drawing.Size(95, 26)
        Me.btnTimeRefresh.TabIndex = 4
        Me.btnTimeRefresh.Text = "Refresh"
        Me.btnTimeRefresh.UseVisualStyleBackColor = True
        '
        'btnTimeDelete
        '
        Me.btnTimeDelete.Location = New System.Drawing.Point(615, 8)
        Me.btnTimeDelete.Name = "btnTimeDelete"
        Me.btnTimeDelete.Size = New System.Drawing.Size(110, 26)
        Me.btnTimeDelete.TabIndex = 5
        Me.btnTimeDelete.Text = "Delete entry"
        Me.btnTimeDelete.UseVisualStyleBackColor = True
        '
        'tabPayroll
        '
        Me.tabPayroll.Controls.Add(Me.dgvPayroll)
        Me.tabPayroll.Controls.Add(Me.pnlPayrollTop)
        Me.tabPayroll.Location = New System.Drawing.Point(4, 24)
        Me.tabPayroll.Name = "tabPayroll"
        Me.tabPayroll.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPayroll.Size = New System.Drawing.Size(1032, 600)
        Me.tabPayroll.TabIndex = 2
        Me.tabPayroll.Text = "Payroll"
        Me.tabPayroll.UseVisualStyleBackColor = True
        '
        'dgvPayroll
        '
        Me.dgvPayroll.AllowUserToAddRows = False
        Me.dgvPayroll.AllowUserToDeleteRows = False
        Me.dgvPayroll.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.dgvPayroll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPayroll.BackgroundColor = System.Drawing.Color.White
        Me.dgvPayroll.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvPayroll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPayroll.Location = New System.Drawing.Point(0, 46)
        Me.dgvPayroll.MultiSelect = False
        Me.dgvPayroll.Name = "dgvPayroll"
        Me.dgvPayroll.ReadOnly = True
        Me.dgvPayroll.RowHeadersVisible = False
        Me.dgvPayroll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPayroll.Size = New System.Drawing.Size(1032, 554)
        Me.dgvPayroll.TabIndex = 1
        '
        'pnlPayrollTop
        '
        Me.pnlPayrollTop.Controls.Add(Me.lblPayrollMonth)
        Me.pnlPayrollTop.Controls.Add(Me.dtpPayrollMonth)
        Me.pnlPayrollTop.Controls.Add(Me.btnPayrollLoad)
        Me.pnlPayrollTop.Controls.Add(Me.lblPayrollTotal)
        Me.pnlPayrollTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlPayrollTop.Location = New System.Drawing.Point(3, 3)
        Me.pnlPayrollTop.Name = "pnlPayrollTop"
        Me.pnlPayrollTop.Size = New System.Drawing.Size(1026, 43)
        Me.pnlPayrollTop.TabIndex = 0
        '
        'lblPayrollMonth
        '
        Me.lblPayrollMonth.AutoSize = True
        Me.lblPayrollMonth.Location = New System.Drawing.Point(6, 13)
        Me.lblPayrollMonth.Name = "lblPayrollMonth"
        Me.lblPayrollMonth.Size = New System.Drawing.Size(43, 15)
        Me.lblPayrollMonth.TabIndex = 0
        Me.lblPayrollMonth.Text = "Month"
        '
        'dtpPayrollMonth
        '
        Me.dtpPayrollMonth.CustomFormat = "MMMM yyyy"
        Me.dtpPayrollMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpPayrollMonth.Location = New System.Drawing.Point(55, 9)
        Me.dtpPayrollMonth.Name = "dtpPayrollMonth"
        Me.dtpPayrollMonth.ShowUpDown = True
        Me.dtpPayrollMonth.Size = New System.Drawing.Size(140, 23)
        Me.dtpPayrollMonth.TabIndex = 1
        '
        'btnPayrollLoad
        '
        Me.btnPayrollLoad.Location = New System.Drawing.Point(208, 8)
        Me.btnPayrollLoad.Name = "btnPayrollLoad"
        Me.btnPayrollLoad.Size = New System.Drawing.Size(75, 26)
        Me.btnPayrollLoad.TabIndex = 2
        Me.btnPayrollLoad.Text = "Load"
        Me.btnPayrollLoad.UseVisualStyleBackColor = True
        '
        'lblPayrollTotal
        '
        Me.lblPayrollTotal.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.lblPayrollTotal.AutoSize = False
        Me.lblPayrollTotal.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblPayrollTotal.Location = New System.Drawing.Point(736, 12)
        Me.lblPayrollTotal.Name = "lblPayrollTotal"
        Me.lblPayrollTotal.Size = New System.Drawing.Size(280, 20)
        Me.lblPayrollTotal.TabIndex = 3
        Me.lblPayrollTotal.Text = "Total monthly payroll: 0.00"
        Me.lblPayrollTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'HrDashboardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1040, 680)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.pnlTop)
        Me.MinimumSize = New System.Drawing.Size(900, 600)
        Me.Name = "HrDashboardForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HR Dashboard"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabEmployees.ResumeLayout(False)
        CType(Me.dgvEmployees, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEmployeesTop.ResumeLayout(False)
        Me.pnlEmployeesTop.PerformLayout()
        Me.tabTime.ResumeLayout(False)
        CType(Me.dgvTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTimeTop.ResumeLayout(False)
        Me.pnlTimeTop.PerformLayout()
        Me.tabPayroll.ResumeLayout(False)
        CType(Me.dgvPayroll, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPayrollTop.ResumeLayout(False)
        Me.pnlPayrollTop.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents btnLogout As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabEmployees As System.Windows.Forms.TabPage
    Friend WithEvents dgvEmployees As System.Windows.Forms.DataGridView
    Friend WithEvents pnlEmployeesTop As System.Windows.Forms.Panel
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnRefreshEmployees As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnApprove As System.Windows.Forms.Button
    Friend WithEvents btnDeactivate As System.Windows.Forms.Button
    Friend WithEvents tabTime As System.Windows.Forms.TabPage
    Friend WithEvents dgvTime As System.Windows.Forms.DataGridView
    Friend WithEvents pnlTimeTop As System.Windows.Forms.Panel
    Friend WithEvents lblEmployee As System.Windows.Forms.Label
    Friend WithEvents cmbEmployee As System.Windows.Forms.ComboBox
    Friend WithEvents lblTimeMonth As System.Windows.Forms.Label
    Friend WithEvents dtpTimeMonth As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnTimeRefresh As System.Windows.Forms.Button
    Friend WithEvents btnTimeDelete As System.Windows.Forms.Button
    Friend WithEvents tabPayroll As System.Windows.Forms.TabPage
    Friend WithEvents dgvPayroll As System.Windows.Forms.DataGridView
    Friend WithEvents pnlPayrollTop As System.Windows.Forms.Panel
    Friend WithEvents lblPayrollMonth As System.Windows.Forms.Label
    Friend WithEvents dtpPayrollMonth As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnPayrollLoad As System.Windows.Forms.Button
    Friend WithEvents lblPayrollTotal As System.Windows.Forms.Label
End Class
