# Implementation From Scratch

This guide rebuilds the Employee Management System in a new Visual Basic WinForms project using
the normal Visual Studio **Windows Forms designer** and the **code editor**. It is written against
the current project in this folder, which is the authoritative reference for behavior.

The finished application has five forms:

| Form | Role | Purpose |
| --- | --- | --- |
| `LoginForm` | both | Signs in HR and employees; opens signup. |
| `SignupForm` | employee | Creates a pending employee account. |
| `HrDashboardForm` | HR | Manages accounts, work time, and payroll. |
| `EmployeeDashboardForm` | employee | Shows profile, work time, and pay. |
| `EmployeeEditForm` | HR | Adds or edits an account. |

Follow the sections in order. The form layout tables are complete enough to rebuild the UI by
drag-and-drop; the complete source code is in Appendix A at the end of this document.

---

## 1. Before you start

You need:

- Windows 10 or 11.
- Visual Studio 2022 17.14+ or Visual Studio 2026, with the **.NET desktop development** workload.
- The .NET 10 SDK. The project targets `net10.0-windows`.
- MySQL 5.7+ or 8.x, running and reachable.
- A MySQL user that can connect to the application database and create tables. If the user cannot
  create tables, run `schema.sql` first with an administrative account.
- The icon PNGs from the existing `Icons` folder, or new icons with the same names. The UI works
  without them, but buttons and tabs will have no images.

Use these Visual Studio windows:

- **Solution Explorer**: add forms and files.
- **Toolbox**: drag controls onto a form.
- **Properties**: set `(Name)`, `Text`, `Location`, `Size`, `Font`, `Anchor`, `Dock`, and so on.
- **Document Outline** (`View > Other Windows > Document Outline`): verify which panel or tab page
  owns a control.
- **Code editor** (`F7` or double-click a control): add event handlers and non-visual classes.

The application does **not** use Entity Framework. It uses `MySqlConnector` and hand-written SQL.

---

## 2. Create the Visual Studio project

### 2.1 Create the project

1. Choose **File > New > Project**.
2. Select **Visual Basic > Windows > Windows Forms App**.
3. Name the project `studentManagementSystem`.
4. Choose `.NET 10.0` as the target framework.
5. Create the project.
6. Delete the template `Form1.vb` and `Form1.Designer.vb` after you create `LoginForm`. Do not keep
   an unused form in the project.

The forms in this guide must be added with **Project > Add New Item > Windows Form**. Do not add a
plain class and rename it: a plain class has no designer and Visual Studio will not open it as a
form.

### 2.2 Add folders and the NuGet package

In Solution Explorer, add these folders:

```
Data
Security
Icons
```

Install the MySQL driver:

1. **Tools > NuGet Package Manager > Manage NuGet Packages for Solution**.
2. Browse for `MySqlConnector`.
3. Install version `2.6.2` (or a compatible newer version) into the project.

### 2.3 Set project properties

Open **Project > studentManagementSystem Properties**:

- **Application**:
  - Application type: **Windows Forms App**.
  - Startup form: `LoginForm` (after the form exists).
  - Enable visual styles: checked.
  - Single instance: unchecked.
  - Shutdown mode: **When main form closes**.
- **Compile**:
  - `Option Strict`: **On**.
  - `Option Infer`: **On**.
- **References**:
  - Imported namespaces: check `System.Data`, `System.Drawing`, and `System.Windows.Forms`.

The resulting project file should look like this. The form metadata groups are what make the
`.Designer.vb` files open in the designer; Visual Studio normally adds them when you use **Add New
Item > Windows Form**, but verify them if a form opens as plain code.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <StartupObject>Sub Main</StartupObject>
    <UseWindowsForms>true</UseWindowsForms>
    <MyType>WindowsForms</MyType>
    <OptionStrict>On</OptionStrict>
    <OptionInfer>On</OptionInfer>
  </PropertyGroup>

  <ItemGroup>
    <Import Include="System.Data" />
    <Import Include="System.Drawing" />
    <Import Include="System.Windows.Forms" />
  </ItemGroup>

  <ItemGroup>
    <Compile Update="My Project\Application.Designer.vb">
      <DesignTime>True</DesignTime>
      <AutoGen>True</AutoGen>
      <DependentUpon>Application.myapp</DependentUpon>
    </Compile>
  </ItemGroup>

  <ItemGroup>
    <None Update="My Project\Application.myapp">
      <Generator>MyApplicationCodeGenerator</Generator>
      <LastGenOutput>Application.Designer.vb</LastGenOutput>
    </None>
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="MySqlConnector" Version="2.6.2" />
  </ItemGroup>

  <ItemGroup>
    <Compile Update="LoginForm.vb">
      <SubType>Form</SubType>
    </Compile>
    <Compile Update="LoginForm.Designer.vb">
      <DependentUpon>LoginForm.vb</DependentUpon>
    </Compile>
    <Compile Update="SignupForm.vb">
      <SubType>Form</SubType>
    </Compile>
    <Compile Update="SignupForm.Designer.vb">
      <DependentUpon>SignupForm.vb</DependentUpon>
    </Compile>
    <Compile Update="HrDashboardForm.vb">
      <SubType>Form</SubType>
    </Compile>
    <Compile Update="HrDashboardForm.Designer.vb">
      <DependentUpon>HrDashboardForm.vb</DependentUpon>
    </Compile>
    <Compile Update="EmployeeDashboardForm.vb">
      <SubType>Form</SubType>
    </Compile>
    <Compile Update="EmployeeDashboardForm.Designer.vb">
      <DependentUpon>EmployeeDashboardForm.vb</DependentUpon>
    </Compile>
    <Compile Update="EmployeeEditForm.vb">
      <SubType>Form</SubType>
    </Compile>
    <Compile Update="EmployeeEditForm.Designer.vb">
      <DependentUpon>EmployeeEditForm.vb</DependentUpon>
    </Compile>
  </ItemGroup>

  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include="Icons\*.png">
      <LogicalName>Icons.%(Filename)%(Extension)</LogicalName>
    </EmbeddedResource>
  </ItemGroup>

</Project>
```

### 2.4 Add `appsettings.json`

Add a JSON file named `appsettings.json` at the project root. Set its **Copy to Output Directory**
property to **Copy if newer** (the project file snippet above does this).

```json
{
  "ConnectionStrings": {
    "EmployeeDb": ""
  },
  "SeedHr": {
    "Username": "admin",
    "Password": "Admin@123"
  }
}
```

For a real deployment, put a connection string like this in `EmployeeDb` and change the seed
password before the first run:

```
Server=localhost;Port=3306;Database=employee_management;User ID=employee_app;Password=CHANGE_ME;
```

Never commit a real password to source control. The application reads `appsettings.json` from the
folder containing the executable, not from the project root directly. If the file is missing from
`bin\Debug\net10.0-windows`, the app reports a missing configuration file.

### 2.5 Add the icons

1. Put these 13 PNG files in the `Icons` folder:

   ```
   app.png          employees.png   clock.png       payroll.png
   profile.png      add.png         edit.png        approve.png
   block.png        search.png      refresh.png     delete.png
   logout.png
   ```

2. Keep the file names lowercase and exact. The code looks for embedded resources named
   `Icons.<name>.png`, for example `Icons.app.png`.
3. The project file's `EmbeddedResource` item embeds the folder. Do not rely on the default resource
   name that Visual Studio might assign; the explicit `LogicalName` above is required.
4. Rebuild the project after adding or renaming icons.

The current project uses Flaticon icons. Keep `Icons/ATTRIBUTION.md` and the **Icons by Flaticon**
link on the login screen. See Appendix C for the list.

### 2.6 Designer rules that prevent layout problems

Apply these rules to every form **before** you add controls:

1. Select the form, then set:
   - `Font`: `Segoe UI, 9pt`
   - `AutoScaleMode`: `Font`
   - `AutoScaleDimensions`: `7, 15` (Visual Studio normally fills this automatically after the font
     and scale mode are set)
2. Do not set a larger form font. On a machine whose Windows default UI font is 14.25pt, WinForms
   otherwise scales the whole form by 1.58x, which clips the right side of the dashboard and makes
   toolbar buttons unreadable.
3. Set the form font **first**, then add controls. Child controls inherit the form font unless the
   guide says otherwise.
4. Do not edit `.Designer.vb` by hand. Use the Properties window. The generated code is shown in this
   guide only as a description of the result.
5. Use `ClientSize`, not the outer `Size`, for form dimensions. The border adds a few pixels.
6. Use `Location` and `Size` exactly as listed. The coordinates are in design units at 9pt.
7. Use **Format > Order > Bring to Front/Send to Back** if a panel covers a control.

---

## 3. Create the MySQL database

### 3.1 Run the schema

The easiest path is to run `schema.sql` in MySQL Workbench or the `mysql` client. The database name
used by the sample configuration is `employee_management`.

```sql
CREATE DATABASE IF NOT EXISTS employee_management
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE employee_management;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS work_entries (
    id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id INT NOT NULL,
    work_date DATE NOT NULL,
    hours DECIMAL(5,2) NOT NULL,
    note VARCHAR(255) NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_work_entries_employee
        FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    INDEX idx_work_entries_employee_date (employee_id, work_date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
```

The application also creates these tables on startup with `CREATE TABLE IF NOT EXISTS`. The database
itself must already exist, because the connection string selects it. If the application user lacks
`CREATE` permission, run the schema manually.

### 3.2 Seed the first HR account

`Db.InitializeAsync` checks whether any row with `role = 'HR'` exists. If none exists, it creates:

- Full name: `System Administrator`
- Username: `SeedHr.Username`
- Password: `SeedHr.Password`, hashed with PBKDF2-SHA256
- Role: `HR`
- Position: `HR Manager`
- Status: `ACTIVE`
- Hire date: today

The default seed is `admin` / `Admin@123` in the sample file. Change it before the first run. Once an
HR account exists, changing `SeedHr` does not update it.

### 3.3 Data conventions

- `employees.role` is stored as `EMPLOYEE` or `HR`.
- `employees.status` is stored as `PENDING`, `ACTIVE`, or `INACTIVE`.
- New signups are `EMPLOYEE` + `PENDING`; they cannot sign in until HR approves them.
- Salary is a fixed monthly amount. Payroll does not calculate an hourly rate.
- `work_entries.employee_id` has `ON DELETE CASCADE`; deleting an employee removes their entries.
- All SQL is parameterized.

---

## 4. Add the non-visual source files

Use **Project > Add New Item > Class** (or **Module**) and paste the complete file contents from
Appendix A. Do not add these as forms.

| File | What it contains |
| --- | --- |
| `Models.vb` | `UserRole`, `AccountStatus`, `Employee`, `WorkEntry`, `PayrollRow`, `EmployeeOption`, and the `DomainText` helpers. |
| `Session.vb` | The signed-in `Employee` and an `IsHr` helper. |
| `Payroll.vb` | Month range calculation and monthly salary total. |
| `Security/PasswordHasher.vb` | PBKDF2-SHA256 password hashing and constant-time verification. |
| `Data/AppSettings.vb` | Reads `appsettings.json` from the executable folder. |
| `Data/Db.vb` | Opens connections, creates tables, seeds the first HR account. |
| `Data/EmployeeRepository.vb` | Authentication and employee CRUD. |
| `Data/WorkEntryRepository.vb` | Work-entry CRUD, daily-limit validation, payroll query. |
| `UiIcons.vb` | Loads and resizes embedded PNG icons. |
| `SelfTest.vb` | Password, month-range, and payroll-total checks. |
| `ApplicationEvents.vb` | `--self-test` entry point and unhandled-exception dialog. |

### 4.1 Key points to preserve

- `PasswordHasher` uses 100,000 iterations, a 16-byte random salt, a 32-byte hash, and
  `CryptographicOperations.FixedTimeEquals`.
- `AppSettings` throws a clear error when `EmployeeDb` is empty, contains `CHANGE_ME`, or when
  `appsettings.json` is missing or invalid.
- `Db.InitializeAsync` is safe to call repeatedly. It creates both tables and seeds HR only when the
  HR count is zero.
- `EmployeeRepository.AuthenticateAsync` selects the real password hash and salt. The list query
  deliberately selects empty strings for those columns and maps them into `Employee` objects.
- `WorkEntryRepository.AddAsync` rejects `hours <= 0`, `hours > 24`, future dates, and a daily total
  above 24 when existing entries are included.
- `WorkEntryRepository.GetPayrollAsync` includes only active employees and returns fixed monthly
  salary plus total hours and distinct days worked.

### 4.2 `UiIcons.vb`

The icon helper is central to the UI. Copy it exactly. It caches images by `name:size`, so repeated
tab and button styling does not reload files.

```vb
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Reflection

Public Module UiIcons
    Private ReadOnly Cache As New Dictionary(Of String, Image)()
    Private _appIcon As Icon

    Public Function GetIcon(name As String, size As Integer) As Image
        Dim key = name & ":" & size.ToString()
        If Cache.ContainsKey(key) Then Return Cache(key)

        Using iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Icons." & name & ".png")
            If iconStream Is Nothing Then Return Nothing

            Using source = Image.FromStream(iconStream)
                Dim bitmap As New Bitmap(size, size, PixelFormat.Format32bppArgb)
                Using g = Graphics.FromImage(bitmap)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality
                    g.SmoothingMode = SmoothingMode.HighQuality
                    g.Clear(Color.Transparent)
                    g.DrawImage(source, 0, 0, size, size)
                End Using

                Cache(key) = bitmap
                Return bitmap
            End Using
        End Using
    End Function

    Public Function AppIcon() As Icon
        If _appIcon Is Nothing Then
            Dim image = GetIcon("app", 64)
            If image IsNot Nothing Then
                Using bitmap As New Bitmap(image)
                    _appIcon = Icon.FromHandle(bitmap.GetHicon())
                End Using
            End If
        End If

        Return _appIcon
    End Function

    Public Sub StyleButton(button As Button, iconName As String, Optional size As Integer = 16)
        Dim image = GetIcon(iconName, size)
        If image Is Nothing Then Return

        button.Image = image
        button.ImageAlign = ContentAlignment.MiddleLeft
        button.TextImageRelation = TextImageRelation.ImageBeforeText
        button.Padding = New Padding(3, 0, 3, 0)
    End Sub

    Public Sub StyleTabs(tabs As TabControl, imageList As ImageList, ParamArray iconNames As String())
        imageList.ImageSize = New Size(20, 20)
        imageList.ColorDepth = ColorDepth.Depth32Bit

        For Each name As String In iconNames
            imageList.Images.Add(name, GetIcon(name, 20))
        Next

        tabs.ImageList = imageList
        For index As Integer = 0 To Math.Min(tabs.TabPages.Count, iconNames.Length) - 1
            tabs.TabPages(index).ImageIndex = index
        Next
    End Sub
End Module
```

Icon images are assigned in each form's `Load` handler, not in the designer. The designer therefore
shows empty `PictureBox` and button-image slots. That is expected.

---

## 5. Build the forms in the designer

### 5.1 General recipe

For each form:

1. **Project > Add New Item > Windows Form**. Name it exactly as listed.
2. Set the form properties in the table at the start of that form's section.
3. Drag controls from the **Toolbox**. Set `(Name)` first, then the other properties.
4. Use **Format > Align** and the Properties window to match the coordinates. Small differences are
   acceptable only if the layout still fits; the listed coordinates are the tested layout.
5. Set tab order with **View > Tab Order**, following the `Tab` column where provided.
6. Open the code editor (`F7`) and paste the complete code-behind from Appendix A, or write the
   handlers listed in the section. VB binds them through the `Handles` clause, so you do not have to
   use the Events pane. `Async Sub` is valid for event handlers.
7. Build the project (`Ctrl+Shift+B`) after each form. A failed build prevents the designer from
   opening other forms.

Labels are `AutoSize = True` unless the table says otherwise. Buttons use the default visual style
(`UseVisualStyleBackColor = True`) unless stated. Do not set button images in the designer.

### 5.2 `LoginForm`

**Form properties**

| Property | Value |
| --- | --- |
| `(Name)` | `LoginForm` |
| `ClientSize` | `410, 340` |
| `Font` | `Segoe UI, 9pt` |
| `AutoScaleMode` | `Font` |
| `AutoScaleDimensions` | `7, 15` |
| `FormBorderStyle` | `FixedSingle` |
| `MaximizeBox` | `False` |
| `StartPosition` | `CenterScreen` |
| `Text` | `Employee Management - Sign In` |
| `AcceptButton` | `btnLogin` |

**Controls** (all parented to the form)

| Control | Type | Location | Size | Text / properties |
| --- | --- | --- | --- | --- |
| `picLogo` | PictureBox | `30, 18` | `48, 48` | `SizeMode = Zoom`; no image in designer |
| `lblTitle` | Label | `92, 22` | AutoSize | `Employee Management`; Segoe UI 20 Bold |
| `lblSubtitle` | Label | `94, 64` | AutoSize | `Sign in to continue`; `ForeColor = DimGray` |
| `lblUsername` | Label | `32, 100` | AutoSize | `Username` |
| `txtUsername` | TextBox | `32, 120` | `346, 23` | `MaxLength = 50` |
| `lblPassword` | Label | `32, 160` | AutoSize | `Password` |
| `txtPassword` | TextBox | `32, 180` | `346, 23` | `MaxLength = 200`; `UseSystemPasswordChar = True` |
| `btnLogin` | Button | `32, 224` | `346, 40` | `Sign In`; Segoe UI 10; `Enabled = False` |
| `lnkSignup` | LinkLabel | `32, 276` | AutoSize | `New employee? Create an account` |
| `lblStatus` | Label | `32, 306` | AutoSize | `Connecting to database...`; `ForeColor = DimGray` |
| `lnkIcons` | LinkLabel | `292, 308` | AutoSize | `Icons by Flaticon`; Segoe UI 8; `ForeColor = DimGray` |

**Code-behind**

Paste `LoginForm.vb`. It must:

- Set the form icon, logo, and login button icon on `Load`.
- Disable login until `Db.InitializeAsync()` succeeds on `Shown`.
- Show `Connected.` and focus the username on success; show a database error dialog on failure.
- Validate non-empty username/password.
- Authenticate, verify the PBKDF2 hash, and reject pending or inactive accounts.
- Put the signed-in employee in `Session.CurrentUser`.
- Hide the login form while the appropriate dashboard is open with `ShowDialog()`, then restore the
  login form, clear the password, and focus the username.
- Open `SignupForm` modally and prefill the new username after a successful signup.
- Open `https://www.flaticon.com` from the attribution link using
  `ProcessStartInfo` with `UseShellExecute = True`.
- Toggle controls with `SetBusy`.

**Event handlers**

| Event | Handler |
| --- | --- |
| `Load` | `LoginForm_Load` |
| `Shown` | `LoginForm_Shown` |
| `btnLogin.Click` | `btnLogin_Click` |
| `lnkSignup.LinkClicked` | `lnkSignup_LinkClicked` |
| `lnkIcons.LinkClicked` | `lnkIcons_LinkClicked` |

### 5.3 `SignupForm`

**Form properties**

| Property | Value |
| --- | --- |
| `(Name)` | `SignupForm` |
| `ClientSize` | `430, 485` |
| `Font` | `Segoe UI, 9pt` |
| `AutoScaleMode` | `Font` |
| `AutoScaleDimensions` | `7, 15` |
| `FormBorderStyle` | `FixedDialog` |
| `MaximizeBox` | `False` |
| `MinimizeBox` | `False` |
| `ShowInTaskbar` | `False` |
| `StartPosition` | `CenterParent` |
| `Text` | `Employee Sign Up` |
| `AcceptButton` | `btnCreate` |
| `CancelButton` | `btnCancel` |

**Controls** (all parented to the form)

| Control | Type | Location | Size | Text / properties |
| --- | --- | --- | --- | --- |
| `lblTitle` | Label | `30, 20` | AutoSize | `Create Employee Account`; Segoe UI 15 Bold |
| `lblHint` | Label | `32, 55` | `360, 30` | Pending-approval text; `ForeColor = DimGray` |
| `lblFullName` | Label | `32, 90` | AutoSize | `Full name` |
| `txtFullName` | TextBox | `32, 110` | `360, 23` | `MaxLength = 100` |
| `lblUsername` | Label | `32, 145` | AutoSize | `Username` |
| `txtUsername` | TextBox | `32, 165` | `360, 23` | `MaxLength = 50` |
| `lblEmail` | Label | `32, 200` | AutoSize | `Email (optional)` |
| `txtEmail` | TextBox | `32, 220` | `360, 23` | `MaxLength = 150` |
| `lblPassword` | Label | `32, 255` | AutoSize | `Password (at least 8 characters)` |
| `txtPassword` | TextBox | `32, 275` | `360, 23` | `MaxLength = 200`; `UseSystemPasswordChar = True` |
| `lblConfirm` | Label | `32, 310` | AutoSize | `Confirm password` |
| `txtConfirm` | TextBox | `32, 330` | `360, 23` | `MaxLength = 200`; `UseSystemPasswordChar = True` |
| `btnCreate` | Button | `32, 380` | `360, 40` | `Create Account`; Segoe UI 10 |
| `btnCancel` | Button | `32, 430` | `360, 30` | `Cancel` |

**Code-behind**

Paste `SignupForm.vb`. It contains:

- A parameterless `Public Sub New()` for the designer.
- A public read-only `CreatedUsername` property, backed by `_createdUsername`.
- `Load` icon and create-button icon.
- Validation: full name at least 2 characters, username at least 3 and no spaces/tabs, optional
  email containing `@`, password at least 8 characters, matching confirmation.
- `EmployeeRepository.InsertAsync` with `Role = Employee`, `Status = Pending`.
- MySQL duplicate-key handling for error number `1062` ("username already taken").
- `DialogResult.OK` and `Close()` after success; `SetBusy` toggles both buttons and the wait cursor.

**Event handlers**

| Event | Handler |
| --- | --- |
| `Load` | `SignupForm_Load` |
| `btnCreate.Click` | `btnCreate_Click` |

`btnCancel` uses the form's `CancelButton`; no separate handler is required.

### 5.4 `HrDashboardForm`

**Form properties**

| Property | Value |
| --- | --- |
| `(Name)` | `HrDashboardForm` |
| `ClientSize` | `1040, 680` |
| `MinimumSize` | `900, 600` |
| `Font` | `Segoe UI, 9pt` |
| `AutoScaleMode` | `Font` |
| `AutoScaleDimensions` | `7, 15` |
| `StartPosition` | `CenterScreen` |
| `Text` | `HR Dashboard` |

**Top bar**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `pnlTop` | Panel | form | `0, 0` | `1040, 52` | `Dock = Top` |
| `lblUser` | Label | `pnlTop` | `16, 17` | AutoSize | `Signed in as`; Segoe UI 10 Bold |
| `btnLogout` | Button | `pnlTop` | `920, 10` | `100, 32` | `Log out`; `Anchor = Top, Right` |

**Tab control**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `tabMain` | TabControl | form | `0, 52` | `1040, 628` | `Anchor = Top, Bottom, Left, Right` |
| `tabEmployees` | TabPage | `tabMain` | designer default | `1032, 600` | `Employees`; `Padding = 3` |
| `tabTime` | TabPage | `tabMain` | designer default | `1032, 600` | `Work Time`; `Padding = 3` |
| `tabPayroll` | TabPage | `tabMain` | designer default | `1032, 600` | `Payroll`; `Padding = 3` |

Add `tabMain` first, then `pnlTop`, as in the designer. `pnlTop` docks to the top; `tabMain` is
anchored and starts at `y = 52`.

**Employees tab**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `pnlEmployeesTop` | Panel | `tabEmployees` | `3, 3` | `1026, 43` | `Dock = Top` |
| `txtSearch` | TextBox | `pnlEmployeesTop` | `6, 9` | `220, 23` | search text |
| `btnSearch` | Button | `pnlEmployeesTop` | `232, 8` | `85, 26` | `Search` |
| `btnRefreshEmployees` | Button | `pnlEmployeesTop` | `325, 8` | `95, 26` | `Refresh` |
| `btnAdd` | Button | `pnlEmployeesTop` | `428, 8` | `90, 26` | `Add` |
| `btnEdit` | Button | `pnlEmployeesTop` | `526, 8` | `75, 26` | `Edit` |
| `btnApprove` | Button | `pnlEmployeesTop` | `609, 8` | `85, 26` | `Approve` |
| `btnDeactivate` | Button | `pnlEmployeesTop` | `702, 8` | `110, 26` | `Deactivate` |
| `dgvEmployees` | DataGridView | `tabEmployees` | `0, 46` | `1032, 554` | `Anchor = Top, Bottom, Left, Right` |

**Work Time tab**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `pnlTimeTop` | Panel | `tabTime` | `3, 3` | `1026, 43` | `Dock = Top` |
| `lblEmployee` | Label | `pnlTimeTop` | `6, 13` | AutoSize | `Employee` |
| `cmbEmployee` | ComboBox | `pnlTimeTop` | `73, 9` | `220, 23` | `DropDownStyle = DropDownList` |
| `lblTimeMonth` | Label | `pnlTimeTop` | `310, 13` | AutoSize | `Month` |
| `dtpTimeMonth` | DateTimePicker | `pnlTimeTop` | `359, 9` | `140, 23` | `Format = Custom`; `CustomFormat = MMMM yyyy`; `ShowUpDown = True` |
| `btnTimeRefresh` | Button | `pnlTimeTop` | `512, 8` | `95, 26` | `Refresh` |
| `btnTimeDelete` | Button | `pnlTimeTop` | `615, 8` | `110, 26` | `Delete entry` |
| `dgvTime` | DataGridView | `tabTime` | `0, 46` | `1032, 554` | `Anchor = Top, Bottom, Left, Right` |

**Payroll tab**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `pnlPayrollTop` | Panel | `tabPayroll` | `3, 3` | `1026, 43` | `Dock = Top` |
| `lblPayrollMonth` | Label | `pnlPayrollTop` | `6, 13` | AutoSize | `Month` |
| `dtpPayrollMonth` | DateTimePicker | `pnlPayrollTop` | `55, 9` | `140, 23` | `Format = Custom`; `CustomFormat = MMMM yyyy`; `ShowUpDown = True` |
| `btnPayrollLoad` | Button | `pnlPayrollTop` | `208, 8` | `75, 26` | `Load` |
| `lblPayrollTotal` | Label | `pnlPayrollTop` | `736, 12` | `280, 20` | `Total monthly payroll: 0.00`; `AutoSize = False`; Segoe UI 10 Bold; `TextAlign = MiddleRight`; `Anchor = Top, Right` |
| `dgvPayroll` | DataGridView | `tabPayroll` | `0, 46` | `1032, 554` | `Anchor = Top, Bottom, Left, Right` |

**DataGridView settings** for all three grids:

```
AllowUserToAddRows = False
ReadOnly = True
RowHeadersVisible = False
SelectionMode = FullRowSelect
AutoSizeColumnsMode = Fill
BorderStyle = None
```

Leave the grids with **no columns** in the designer. `ConfigureGrids()` creates the columns at
runtime. The hidden `Id` column stores the database key for edit/delete operations. Grid columns
are not sortable; salary/hours columns use `N2` formatting and right alignment. The payroll total
label must have `AutoSize = False` and a 280px width, otherwise it can disappear behind the month
controls when the form is wide.

**Code-behind**

Paste `HrDashboardForm.vb`. It must:

- Close immediately if `Session.CurrentUser` is `Nothing`.
- Set the form icon, create a 20x20 `ImageList` for the tab icons, and apply employee/clock/payroll
  icons to the three tabs.
- Apply button icons: search, refresh, add, edit, approve, block, refresh, delete, refresh, logout.
- Set the header text to `Signed in as <full name> (<username>)`.
- Configure the three grids exactly as described.
- Load employees, work time, and payroll asynchronously, with error dialogs.
- Search by name/username/position; Enter in the search box triggers the search.
- Populate the Work Time employee filter with an "All employees" option plus active/inactive
  employee options; preserve the previous selection when possible.
- Add/edit accounts through `EmployeeEditForm`; refresh all views after editing.
- Approve an account after confirmation; reject an already-active account.
- Deactivate an account after confirmation; refuse to deactivate the signed-in HR account and
  reject an already-inactive account.
- Delete any work entry after confirmation; refresh work time and payroll.
- Load payroll for the selected month and update `lblPayrollTotal` with `Payroll.MonthlyTotal`.
- Log out by clearing `Session.CurrentUser` and closing the form.

**Event handlers**

| Event | Handler |
| --- | --- |
| `Load` | `HrDashboardForm_Load` |
| `btnSearch.Click`, `btnRefreshEmployees.Click` | `btnSearch_Click` |
| `txtSearch.KeyDown` | `txtSearch_KeyDown` |
| `btnAdd.Click` | `btnAdd_Click` |
| `btnEdit.Click` | `btnEdit_Click` |
| `dgvEmployees.CellDoubleClick` | `dgvEmployees_CellDoubleClick` |
| `btnApprove.Click` | `btnApprove_Click` |
| `btnDeactivate.Click` | `btnDeactivate_Click` |
| `btnTimeRefresh.Click` | `btnTimeRefresh_Click` |
| `btnTimeDelete.Click` | `btnTimeDelete_Click` |
| `btnPayrollLoad.Click` | `btnPayrollLoad_Click` |
| `btnLogout.Click` | `btnLogout_Click` |

### 5.5 `EmployeeDashboardForm`

**Form properties**

| Property | Value |
| --- | --- |
| `(Name)` | `EmployeeDashboardForm` |
| `ClientSize` | `900, 620` |
| `MinimumSize` | `800, 560` |
| `Font` | `Segoe UI, 9pt` |
| `AutoScaleMode` | `Font` |
| `AutoScaleDimensions` | `7, 15` |
| `StartPosition` | `CenterScreen` |
| `Text` | `Employee Dashboard` |

**Top bar**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `pnlTop` | Panel | form | `0, 0` | `900, 52` | `Dock = Top` |
| `lblUser` | Label | `pnlTop` | `16, 17` | AutoSize | `Signed in as`; Segoe UI 10 Bold |
| `btnLogout` | Button | `pnlTop` | `780, 10` | `100, 32` | `Log out`; `Anchor = Top, Right` |

**Tab control**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `tabMain` | TabControl | form | `0, 52` | `900, 568` | `Anchor = Top, Bottom, Left, Right` |
| `tabProfile` | TabPage | `tabMain` | default | `892, 540` | `Profile`; `Padding = 3` |
| `tabTime` | TabPage | `tabMain` | default | `892, 540` | `Work Time`; `Padding = 3` |
| `tabPay` | TabPage | `tabMain` | default | `892, 540` | `Pay`; `Padding = 3` |

Add `tabMain` first, then `pnlTop`.

**Profile tab**

| Control | Type | Location | Size | Text / properties |
| --- | --- | --- | --- | --- |
| `lblFullNameCaption` | Label | `24, 24` | AutoSize | `Full name`; `ForeColor = DimGray` |
| `lblFullName` | Label | `170, 18` | AutoSize | `-`; Segoe UI 13 Bold |
| `lblUsernameCaption` | Label | `24, 65` | AutoSize | `Username`; DimGray |
| `lblUsername` | Label | `170, 65` | AutoSize | `-` |
| `lblEmailCaption` | Label | `24, 100` | AutoSize | `Email`; DimGray |
| `lblEmail` | Label | `170, 100` | AutoSize | `-` |
| `lblPositionCaption` | Label | `24, 135` | AutoSize | `Position`; DimGray |
| `lblPosition` | Label | `170, 135` | AutoSize | `-` |
| `lblSalaryCaption` | Label | `24, 170` | AutoSize | `Monthly salary`; DimGray |
| `lblSalary` | Label | `170, 170` | AutoSize | `-` |
| `lblHireDateCaption` | Label | `24, 205` | AutoSize | `Hire date`; DimGray |
| `lblHireDate` | Label | `170, 205` | AutoSize | `-` |
| `lblStatusCaption` | Label | `24, 240` | AutoSize | `Status`; DimGray |
| `lblStatus` | Label | `170, 240` | AutoSize | `-` |
| `lblProfileHint` | Label | `26, 285` | `480, 30` | Contact-HR hint; DimGray |
| `btnProfileRefresh` | Button | `26, 325` | `120, 30` | `Refresh` |

**Work Time tab**

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `pnlTimeTop` | Panel | `tabTime` | `3, 3` | `886, 46` | `Dock = Top` |
| `lblTimeMonth` | Label | `pnlTimeTop` | `8, 16` | AutoSize | `Month` |
| `dtpTimeMonth` | DateTimePicker | `pnlTimeTop` | `57, 12` | `140, 23` | `MMMM yyyy`; `ShowUpDown = True` |
| `btnTimeRefresh` | Button | `pnlTimeTop` | `210, 10` | `95, 27` | `Refresh` |
| `dgvTime` | DataGridView | `tabTime` | `3, 49` | `886, 436` | `Dock = Fill` |
| `pnlTimeBottom` | Panel | `tabTime` | `3, 485` | `886, 52` | `Dock = Bottom` |
| `lblWorkDate` | Label | `pnlTimeBottom` | `8, 18` | AutoSize | `Date` |
| `dtpWorkDate` | DateTimePicker | `pnlTimeBottom` | `45, 14` | `120, 23` | `Format = Short` |
| `lblWorkHours` | Label | `pnlTimeBottom` | `183, 18` | AutoSize | `Hours` |
| `numHours` | NumericUpDown | `pnlTimeBottom` | `228, 14` | `70, 23` | Decimal places 2; increment 0.25; min 0.25; max 24; value 8 |
| `lblWorkNote` | Label | `pnlTimeBottom` | `316, 18` | AutoSize | `Note` |
| `txtWorkNote` | TextBox | `pnlTimeBottom` | `355, 14` | `250, 23` | `MaxLength = 255` |
| `btnAddEntry` | Button | `pnlTimeBottom` | `620, 10` | `120, 30` | `Add entry` |
| `btnDeleteEntry` | Button | `pnlTimeBottom` | `748, 10` | `120, 30` | `Delete selected` |

For the Work Time tab, add `dgvTime` first, then `pnlTimeTop` and `pnlTimeBottom` (or use Document
Outline to match that order). The grid fills the remaining space between the docked panels. If the
panels are on the wrong z-order, they can overlap the grid.

Grid settings: `AllowUserToAddRows = False`, `ReadOnly = True`, `RowHeadersVisible = False`,
`SelectionMode = FullRowSelect`, `AutoSizeColumnsMode = Fill`, `BorderStyle = None`. Leave the grid
without columns; `ConfigureGrid()` adds `Id` (hidden), `Date`, `Hours`, and `Note`.

**Pay tab**

| Control | Type | Location | Size | Text / properties |
| --- | --- | --- | --- | --- |
| `lblPayMonth` | Label | `24, 26` | AutoSize | `Month` |
| `dtpPayMonth` | DateTimePicker | `73, 22` | `140, 23` | `MMMM yyyy`; `ShowUpDown = True` |
| `btnPayRefresh` | Button | `226, 20` | `95, 27` | `Refresh` |
| `picPay` | PictureBox | `24, 75` | `56, 56` | `SizeMode = Zoom`; no image in designer |
| `lblPaySalary` | Label | `95, 88` | AutoSize | `Monthly salary: 0.00`; Segoe UI 14 Bold |
| `lblPayHours` | Label | `26, 150` | AutoSize | `Hours worked: 0.00` |
| `lblPayDays` | Label | `26, 175` | AutoSize | `Days worked: 0` |
| `lblPayHint` | Label | `26, 210` | `500, 30` | Fixed-salary hint; DimGray |

**Code-behind**

Paste `EmployeeDashboardForm.vb`. It must:

- Close if `Session.CurrentUser` is `Nothing`.
- Set the form icon, tab icons (profile/clock/payroll), refresh/add/delete/logout button icons, and
  the 56px payroll image.
- Configure the work-time grid.
- Set all three month pickers to today.
- Refresh the profile by re-reading the employee from the database and updating
  `Session.CurrentUser`.
- Show full name, username, email (`-` if blank), position (`Not assigned` if blank), salary
  (`N2`), hire date (`yyyy-MM-dd` or `-`), and status.
- Load the selected month's entries for the signed-in employee.
- Add a work entry for the selected date/hours/note, then refresh time and pay; repository
  validation handles future dates, hours <= 0, hours > 24, and daily totals over 24.
- Delete only the signed-in employee's entry after confirmation.
- Calculate pay from the selected month's entries: salary is fixed; hours are summed; days are the
  distinct work dates.
- Log out by clearing `Session.CurrentUser` and closing.

**Event handlers**

| Event | Handler |
| --- | --- |
| `Load` | `EmployeeDashboardForm_Load` |
| `btnProfileRefresh.Click` | `btnProfileRefresh_Click` |
| `btnTimeRefresh.Click` | `btnTimeRefresh_Click` |
| `btnAddEntry.Click` | `btnAddEntry_Click` |
| `btnDeleteEntry.Click` | `btnDeleteEntry_Click` |
| `btnPayRefresh.Click` | `btnPayRefresh_Click` |
| `btnLogout.Click` | `btnLogout_Click` |

### 5.6 `EmployeeEditForm`

This is the dialog used by HR to add or edit accounts. It needs two constructors: the parameterless
one is required by the Windows Forms designer; the `Employee` overload is used at runtime.

**Form properties**

| Property | Value |
| --- | --- |
| `(Name)` | `EmployeeEditForm` |
| `ClientSize` | `460, 565` |
| `Font` | `Segoe UI, 9pt` |
| `AutoScaleMode` | `Font` |
| `AutoScaleDimensions` | `7, 15` |
| `FormBorderStyle` | `FixedDialog` |
| `MaximizeBox` | `False` |
| `MinimizeBox` | `False` |
| `ShowInTaskbar` | `False` |
| `StartPosition` | `CenterParent` |
| `Text` | `Add Account` |
| `AcceptButton` | `btnSave` |
| `CancelButton` | `btnCancel` |

**Controls** (all parented to the form except the password fields)

| Control | Type | Parent | Location | Size | Text / properties |
| --- | --- | --- | --- | --- | --- |
| `lblTitle` | Label | form | `30, 18` | AutoSize | `Add Account`; Segoe UI 15 Bold |
| `lblFullName` | Label | form | `32, 65` | AutoSize | `Full name` |
| `txtFullName` | TextBox | form | `32, 85` | `396, 23` | `MaxLength = 100` |
| `lblUsername` | Label | form | `32, 120` | AutoSize | `Username` |
| `txtUsername` | TextBox | form | `32, 140` | `396, 23` | `MaxLength = 50` |
| `lblEmail` | Label | form | `32, 175` | AutoSize | `Email (optional)` |
| `txtEmail` | TextBox | form | `32, 195` | `396, 23` | `MaxLength = 150` |
| `lblRole` | Label | form | `32, 230` | AutoSize | `Role` |
| `cmbRole` | ComboBox | form | `32, 250` | `190, 23` | `DropDownStyle = DropDownList` |
| `lblStatus` | Label | form | `238, 230` | AutoSize | `Status` |
| `cmbStatus` | ComboBox | form | `238, 250` | `190, 23` | `DropDownStyle = DropDownList` |
| `lblPosition` | Label | form | `32, 285` | AutoSize | `Position` |
| `txtPosition` | TextBox | form | `32, 305` | `190, 23` | `MaxLength = 100` |
| `lblSalary` | Label | form | `238, 285` | AutoSize | `Monthly salary` |
| `numSalary` | NumericUpDown | form | `238, 305` | `190, 23` | 2 decimals; increment 50; max 1,000,000; thousands separator |
| `lblHireDate` | Label | form | `32, 340` | AutoSize | `Hire date` |
| `dtpHireDate` | DateTimePicker | form | `32, 360` | `240, 23` | `Format = Short`; `ShowCheckBox = True` |
| `pnlPassword` | Panel | form | `32, 400` | `396, 85` | visible for add; hidden for edit |
| `lblPassword` | Label | `pnlPassword` | `0, 0` | AutoSize | `Password` |
| `txtPassword` | TextBox | `pnlPassword` | `0, 20` | `190, 23` | `MaxLength = 200`; `UseSystemPasswordChar = True` |
| `lblConfirm` | Label | `pnlPassword` | `206, 0` | AutoSize | `Confirm password` |
| `txtConfirm` | TextBox | `pnlPassword` | `206, 20` | `190, 23` | `MaxLength = 200`; `UseSystemPasswordChar = True` |
| `btnSave` | Button | form | `32, 512` | `190, 36` | `Save`; Segoe UI 10 |
| `btnCancel` | Button | form | `238, 512` | `190, 36` | `Cancel` |

The password panel is at design coordinates `(32, 400)`, but its children use coordinates relative
to the panel: password label/box at `x = 0`, confirmation label/box at `x = 206`.

**Code-behind**

Paste `EmployeeEditForm.vb`. Preserve these details:

```vb
Public Sub New()
    Me.New(Nothing)
End Sub

Public Sub New(employee As Employee)
    InitializeComponent()
    _employee = employee
    _isEdit = employee IsNot Nothing
End Sub
```

The parameterless constructor is required by the designer. Without it, Visual Studio may refuse to
open the form.

On `Load`:

- Set the form icon and the save-button icon.
- Add `Employee`/`HR` to `cmbRole` and `Pending`/`Active`/`Inactive` to `cmbStatus`.
- In edit mode:
  - Set title to `Edit Account`.
  - Fill every field from `_employee`.
  - Select the role and status.
  - Clamp salary into the numeric control's range.
  - Set/unset the hire-date check box.
  - Hide `pnlPassword`.
  - Shrink `ClientSize.Height` to `500`.
  - Move both buttons to `Top = 430`.
- In add mode, set title to `Add Account`, select `Employee`, select `Active`.

On save:

- Validate full name (>= 2), username (>= 3, no spaces/tabs), optional email containing `@`.
- When editing the currently signed-in HR user, refuse to remove their HR role or deactivate them.
- In add mode, require an 8+ character password and a matching confirmation.
- Build an `Employee`, using a nullable hire date only when `dtpHireDate.Checked`.
- Insert with a password in add mode; update without a password in edit mode.
- Handle MySQL error 1062 as "username already taken".
- Set `DialogResult.OK` and close on success.
- Use `SetBusy` and `UseWaitCursor`.

**Event handlers**

| Event | Handler |
| --- | --- |
| `Load` | `EmployeeEditForm_Load` |
| `btnSave.Click` | `btnSave_Click` |

`btnCancel` uses the form's `CancelButton`; no separate handler is required.

---

## 6. Add startup behavior and the self-test

### 6.1 Startup form

In **Project Properties > Application**, set the startup form to `LoginForm`. This writes
`My Project/Application.myapp` with `MainForm` set to `LoginForm`. The application uses the VB
application framework (`MyType = WindowsForms`), visual styles, and "shut down when main form
closes".

Do not launch `HrDashboardForm` or `EmployeeDashboardForm` directly. They require
`Session.CurrentUser` and will close themselves if it is missing.

### 6.2 Application events

In **Project Properties > Application**, click **View Application Events** and paste
`ApplicationEvents.vb`:

```vb
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            For Each argument As String In e.CommandLine
                If String.Equals(argument, "--self-test", StringComparison.OrdinalIgnoreCase) Then
                    Dim result = SelfTest.Run()
                    Console.WriteLine(result.Report)
                    Environment.Exit(If(result.Passed, 0, 1))
                End If
            Next
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show(
                e.Exception.Message,
                "Unexpected error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Sub
    End Class
End Namespace
```

The self-test does not touch MySQL. It checks password verification, leap-year month ranges, and the
payroll total. It writes `%TEMP%\employee-management-selftest.txt` and exits with code 0 on success,
1 on failure.

---

## 7. Build, run, and test

### 7.1 Build

```powershell
dotnet restore
dotnet build
```

A clean build should produce 0 warnings and 0 errors. If Visual Studio's designer reports a build
error, fix the compiler error first; the designer loads compiled types.

### 7.2 Configure and run

1. Create the database.
2. Set `ConnectionStrings.EmployeeDb` in `appsettings.json`.
3. Change `SeedHr.Password` to a real password.
4. Run with `dotnet run` or `F5`.
5. Confirm the login status changes from `Connecting to database...` to `Connected.` and the
   `Sign In` button enables.

The first run creates the tables and the HR account if no HR account exists.

### 7.3 Self-test

```powershell
dotnet run --no-build -- --self-test
```

Check the exit code (`$LASTEXITCODE`). If the WinExe does not show console output in your terminal,
open `%TEMP%\employee-management-selftest.txt`. Expected text: `Self-test passed.`

### 7.4 Manual acceptance test

1. Sign in as the seeded HR account.
2. Open **Employees** and confirm the HR row appears.
3. Log out, choose **New employee? Create an account**, and create a user.
4. Try to sign in as that user: the app must say the account is pending.
5. Sign in as HR, select the pending row, click **Approve**, and confirm the status changes.
6. Sign in as the employee.
7. Open **Work Time**, add 8 hours for today with a note; confirm it appears and the payroll hours
   update on the **Pay** tab.
8. Try a future date and more than 24 total hours for one day; both must be rejected.
9. Sign in as HR and open **Payroll**; confirm the employee's fixed salary, hours, and days appear,
   and the total label is fully visible at the right edge.
10. Resize the HR and employee windows; grids should grow/shrink and the logout/total controls
    should stay anchored.

### 7.5 Visual checks at the tested size

At 100% display scaling with Segoe UI 9pt, the HR window client area is 1040x680. The outer window
is about 1056x724. Confirm:

- The **Deactivate** button ends at x = 812 inside the 1026px toolbar; it is not clipped.
- **Delete entry** ends at x = 725; it is not clipped.
- **Total monthly payroll** occupies `(736, 12)` to `(1016, 32)`; it is right-aligned and visible.
- Employee **Refresh** buttons are 95-120px wide and their text is not cut off.
- The login attribution link sits at the lower right, not on top of the status label.

---

## 8. Troubleshooting

### The form opens as code, not in the designer

- Build the project first. The designer cannot load a type that does not compile.
- Confirm the form was added with **Add New Item > Windows Form**, not as a class.
- Confirm the `.vbproj` has `SubType=Form` for the `.vb` file and `DependentUpon` for the
  `.Designer.vb` file.
- Close all designer tabs, rebuild, and reopen the form.
- For `EmployeeEditForm`, confirm the parameterless `Public Sub New()` exists.

### The form is huge, clipped, or controls are cut off

- The form font is probably not 9pt. Select the form and set `Font = Segoe UI, 9pt`.
- Check `AutoScaleMode = Font` and `AutoScaleDimensions = 7, 15`.
- Do not set an individual control to a larger font unless the layout table says so.
- If you changed the form font after placing controls, the designer may have scaled the layout.
  Reset the form font first, then correct positions from the tables.

### The designer shows no icons

Expected. Icons are embedded resources assigned in `Load`. Run the app to see them. If they are
missing at runtime:

- Confirm the PNGs are in `Icons` and named exactly `app.png`, `profile.png`, and so on.
- Confirm the `EmbeddedResource` item and `LogicalName` are present.
- Rebuild.
- Confirm the assembly contains the resources; a missing resource returns `Nothing` and the UI falls
  back to text-only buttons.

### `Missing configuration file` or empty connection string

- `appsettings.json` must be next to the executable. Set **Copy to Output Directory = Copy if
  newer** and rebuild.
- Set `ConnectionStrings.EmployeeDb` to a non-empty connection string. The app rejects empty values
  and values containing `CHANGE_ME`.
- Do not edit only the project-root file and then run an old build; the output copy may be stale.

### `Unable to connect to any of the specified MySQL hosts`

- Confirm the MySQL service is running and the host/port are correct.
- Confirm the database exists.
- Confirm the user can connect from this machine.
- Check firewall/VPN rules for a remote database.

### `Access denied` or `CREATE command denied`

- Grant the application user privileges on `employee_management` (at minimum SELECT, INSERT, UPDATE,
  DELETE, and CREATE for automatic table creation).
- Or run `schema.sql` as an administrator and grant only the DML privileges.

### `No HR account exists and SeedHr is not configured`

- Set `SeedHr.Username` and `SeedHr.Password` in the output `appsettings.json`.
- Or insert an HR row manually with a valid PBKDF2 hash. Easiest is to set the seed before the first
  run.
- The seed runs only when no HR exists. Changing the password later does not change the existing
  account.

### `That username is already taken`

This is the handled MySQL duplicate-key error 1062. Choose another username. The unique index is on
`employees.username`.

### The grid is empty after loading

- Check that the database has rows and the connection string points to the intended database.
- Confirm `ConfigureGrids`/`ConfigureGrid` ran before adding rows.
- If the grid has columns added in the designer, remove them; the code clears and recreates columns.
- Confirm the hidden `Id` column exists before code reads `Cells("Id")`.

### The payroll total is not visible

- Set `AutoSize = False`.
- Set `Size = 280, 20`.
- Set `TextAlign = MiddleRight`.
- Set `Anchor = Top, Right`.
- Keep the label in `pnlPayrollTop`, not directly on the tab page.

### Password text is visible

Set `UseSystemPasswordChar = True` on every password textbox:
`LoginForm.txtPassword`, `SignupForm.txtPassword`, `SignupForm.txtConfirm`,
`EmployeeEditForm.txtPassword`, and `EmployeeEditForm.txtConfirm`.

### Enter/Escape do not work in a dialog

- `LoginForm.AcceptButton = btnLogin`
- `SignupForm.AcceptButton = btnCreate`, `CancelButton = btnCancel`
- `EmployeeEditForm.AcceptButton = btnSave`, `CancelButton = btnCancel`

### Dashboard closes immediately

`Session.CurrentUser` is `Nothing`. Start from `LoginForm` and let it open the dashboard. Do not
change the startup form to a dashboard.

### The Work Time layout overlaps

In the employee Work Time tab, the grid must fill between the top and bottom panels. Verify the
parent of each control in Document Outline and use `Dock = Fill` for `dgvTime`, `Dock = Top` for
`pnlTimeTop`, and `Dock = Bottom` for `pnlTimeBottom`. If necessary, send the panels to the front or
adjust their order in Document Outline.

---

## 9. Project layout

```
studentManagementSystem/
  ApplicationEvents.vb
  LoginForm.vb
  LoginForm.Designer.vb
  SignupForm.vb
  SignupForm.Designer.vb
  HrDashboardForm.vb
  HrDashboardForm.Designer.vb
  EmployeeDashboardForm.vb
  EmployeeDashboardForm.Designer.vb
  EmployeeEditForm.vb
  EmployeeEditForm.Designer.vb
  Models.vb
  Session.vb
  Payroll.vb
  SelfTest.vb
  UiIcons.vb
  appsettings.json
  schema.sql
  studentManagementSystem.vbproj
  Data/
    AppSettings.vb
    Db.vb
    EmployeeRepository.vb
    WorkEntryRepository.vb
  Security/
    PasswordHasher.vb
  Icons/
    app.png
    employees.png
    clock.png
    payroll.png
    profile.png
    add.png
    edit.png
    approve.png
    block.png
    search.png
    refresh.png
    delete.png
    logout.png
    ATTRIBUTION.md
  My Project/
    Application.myapp
    Application.Designer.vb
```

---

## Appendix A: Complete source code

The complete code for every non-designer file follows. Paste each file into the matching item in
Solution Explorer. The `.Designer.vb` files are generated by the designer; do not paste them
manually.

### `Models.vb`

```vb
Public Enum UserRole
    Employee
    Hr
End Enum

Public Enum AccountStatus
    Pending
    Active
    Inactive
End Enum

Public Class Employee
    Public Property Id As Integer
    Public Property FullName As String = ""
    Public Property Username As String = ""
    Public Property Email As String = ""
    Public Property PasswordHash As String = ""
    Public Property PasswordSalt As String = ""
    Public Property Role As UserRole = UserRole.Employee
    Public Property Position As String = ""
    Public Property MonthlySalary As Decimal
    Public Property HireDate As Date?
    Public Property Status As AccountStatus = AccountStatus.Pending
End Class

Public Class WorkEntry
    Public Property Id As Integer
    Public Property EmployeeId As Integer
    Public Property EmployeeName As String = ""
    Public Property WorkDate As Date
    Public Property Hours As Decimal
    Public Property Note As String = ""
End Class

Public Class PayrollRow
    Public Property EmployeeId As Integer
    Public Property FullName As String = ""
    Public Property Position As String = ""
    Public Property MonthlySalary As Decimal
    Public Property TotalHours As Decimal
    Public Property DaysWorked As Integer
End Class

Public Class EmployeeOption
    Public Property Id As Integer
    Public Property Name As String = ""

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Module DomainText
    Public Function RoleText(role As UserRole) As String
        Return If(role = UserRole.Hr, "HR", "EMPLOYEE")
    End Function

    Public Function ParseRole(value As String) As UserRole
        If String.Equals(value, "HR", StringComparison.OrdinalIgnoreCase) Then Return UserRole.Hr
        Return UserRole.Employee
    End Function

    Public Function StatusText(status As AccountStatus) As String
        Select Case status
            Case AccountStatus.Active
                Return "ACTIVE"
            Case AccountStatus.Inactive
                Return "INACTIVE"
            Case Else
                Return "PENDING"
        End Select
    End Function

    Public Function StatusDisplay(status As AccountStatus) As String
        Select Case status
            Case AccountStatus.Active
                Return "Active"
            Case AccountStatus.Inactive
                Return "Inactive"
            Case Else
                Return "Pending"
        End Select
    End Function

    Public Function ParseStatus(value As String) As AccountStatus
        If String.Equals(value, "ACTIVE", StringComparison.OrdinalIgnoreCase) Then Return AccountStatus.Active
        If String.Equals(value, "INACTIVE", StringComparison.OrdinalIgnoreCase) Then Return AccountStatus.Inactive
        Return AccountStatus.Pending
    End Function
End Module
```

### `Session.vb`

```vb
Public Module Session
    Public Property CurrentUser As Employee

    Public ReadOnly Property IsHr As Boolean
        Get
            Return CurrentUser IsNot Nothing AndAlso CurrentUser.Role = UserRole.Hr
        End Get
    End Property
End Module
```

### `Payroll.vb`

```vb
Public Module Payroll
    Public Function MonthRange(year As Integer, month As Integer) As (StartDate As Date, EndDateExclusive As Date)
        Dim startDate As New Date(year, month, 1)
        Return (startDate, startDate.AddMonths(1))
    End Function

    Public Function MonthlyTotal(rows As IEnumerable(Of PayrollRow)) As Decimal
        Dim total As Decimal = 0D
        For Each row As PayrollRow In rows
            total += row.MonthlySalary
        Next
        Return total
    End Function
End Module
```

### `Security\PasswordHasher.vb`

```vb
Imports System.Security.Cryptography

Public Module PasswordHasher
    Private Const Iterations As Integer = 100_000
    Private Const SaltSize As Integer = 16
    Private Const HashSize As Integer = 32

    Public Function Create(password As String) As (Hash As String, Salt As String)
        Dim salt(SaltSize - 1) As Byte
        RandomNumberGenerator.Fill(salt)
        Return (Convert.ToBase64String(Derive(password, salt)), Convert.ToBase64String(salt))
    End Function

    Public Function Verify(password As String, hashBase64 As String, saltBase64 As String) As Boolean
        If String.IsNullOrEmpty(hashBase64) OrElse String.IsNullOrEmpty(saltBase64) Then Return False

        Try
            Dim salt = Convert.FromBase64String(saltBase64)
            Dim expected = Convert.FromBase64String(hashBase64)
            Dim actual = Derive(password, salt)
            Return CryptographicOperations.FixedTimeEquals(actual, expected)
        Catch ex As FormatException
            Return False
        End Try
    End Function

    Private Function Derive(password As String, salt As Byte()) As Byte()
        Return Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize)
    End Function
End Module
```

### `Data\AppSettings.vb`

```vb
Imports System.IO
Imports System.Text.Json

Public Class SeedHrSettings
    Public Property Username As String = ""
    Public Property Password As String = ""
End Class

Public Module AppSettings
    Private ReadOnly SettingsPath As String = Path.Combine(AppContext.BaseDirectory, "appsettings.json")

    Public Function GetConnectionString() As String
        Using document = ReadSettings()
            Dim connectionString As String = ""

            Dim connectionStrings As JsonElement
            If document.RootElement.TryGetProperty("ConnectionStrings", connectionStrings) Then
                Dim employeeDb As JsonElement
                If connectionStrings.TryGetProperty("EmployeeDb", employeeDb) AndAlso employeeDb.ValueKind = JsonValueKind.String Then
                    connectionString = employeeDb.GetString()
                End If
            End If

            If String.IsNullOrWhiteSpace(connectionString) OrElse
               connectionString.Contains("CHANGE_ME", StringComparison.OrdinalIgnoreCase) Then
                Throw New InvalidOperationException(
                    $"Set ConnectionStrings.EmployeeDb in {SettingsPath} before running the app.")
            End If

            Return connectionString.Trim()
        End Using
    End Function

    Public Function GetSeedHr() As SeedHrSettings
        Dim result As New SeedHrSettings()

        Using document = ReadSettings()
            Dim seedHr As JsonElement
            If document.RootElement.TryGetProperty("SeedHr", seedHr) Then
                Dim username As JsonElement
                If seedHr.TryGetProperty("Username", username) AndAlso username.ValueKind = JsonValueKind.String Then
                    result.Username = username.GetString()
                End If

                Dim password As JsonElement
                If seedHr.TryGetProperty("Password", password) AndAlso password.ValueKind = JsonValueKind.String Then
                    result.Password = password.GetString()
                End If
            End If
        End Using

        Return result
    End Function

    Private Function ReadSettings() As JsonDocument
        If Not File.Exists(SettingsPath) Then
            Throw New InvalidOperationException($"Missing configuration file: {SettingsPath}")
        End If

        Try
            Return JsonDocument.Parse(File.ReadAllText(SettingsPath))
        Catch ex As JsonException
            Throw New InvalidOperationException($"appsettings.json is not valid JSON: {ex.Message}", ex)
        End Try
    End Function
End Module
```

### `Data\Db.vb`

```vb
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
```

### `Data\EmployeeRepository.vb`

```vb
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
```

### `Data\WorkEntryRepository.vb`

```vb
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
```

### `UiIcons.vb`

```vb
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Reflection

Public Module UiIcons
    Private ReadOnly Cache As New Dictionary(Of String, Image)()
    Private _appIcon As Icon

    Public Function GetIcon(name As String, size As Integer) As Image
        Dim key = name & ":" & size.ToString()
        If Cache.ContainsKey(key) Then Return Cache(key)

        Using iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Icons." & name & ".png")
            If iconStream Is Nothing Then Return Nothing

            Using source = Image.FromStream(iconStream)
                Dim bitmap As New Bitmap(size, size, PixelFormat.Format32bppArgb)
                Using g = Graphics.FromImage(bitmap)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality
                    g.SmoothingMode = SmoothingMode.HighQuality
                    g.Clear(Color.Transparent)
                    g.DrawImage(source, 0, 0, size, size)
                End Using

                Cache(key) = bitmap
                Return bitmap
            End Using
        End Using
    End Function

    Public Function AppIcon() As Icon
        If _appIcon Is Nothing Then
            Dim image = GetIcon("app", 64)
            If image IsNot Nothing Then
                Using bitmap As New Bitmap(image)
                    _appIcon = Icon.FromHandle(bitmap.GetHicon())
                End Using
            End If
        End If

        Return _appIcon
    End Function

    Public Sub StyleButton(button As Button, iconName As String, Optional size As Integer = 16)
        Dim image = GetIcon(iconName, size)
        If image Is Nothing Then Return

        button.Image = image
        button.ImageAlign = ContentAlignment.MiddleLeft
        button.TextImageRelation = TextImageRelation.ImageBeforeText
        button.Padding = New Padding(3, 0, 3, 0)
    End Sub

    Public Sub StyleTabs(tabs As TabControl, imageList As ImageList, ParamArray iconNames As String())
        imageList.ImageSize = New Size(20, 20)
        imageList.ColorDepth = ColorDepth.Depth32Bit

        For Each name As String In iconNames
            imageList.Images.Add(name, GetIcon(name, 20))
        Next

        tabs.ImageList = imageList
        For index As Integer = 0 To Math.Min(tabs.TabPages.Count, iconNames.Length) - 1
            tabs.TabPages(index).ImageIndex = index
        Next
    End Sub
End Module
```

### `SelfTest.vb`

```vb
Imports System.Diagnostics
Imports System.IO

Public Module SelfTest
    Public Function Run() As (Passed As Boolean, Report As String)
        Dim failures As New List(Of String)()

        Try
            Dim created = PasswordHasher.Create("correct horse battery staple")
            If Not PasswordHasher.Verify("correct horse battery staple", created.Hash, created.Salt) Then
                failures.Add("Password verification failed for the correct password.")
            End If

            If PasswordHasher.Verify("wrong password", created.Hash, created.Salt) Then
                failures.Add("Password verification accepted a wrong password.")
            End If
        Catch ex As Exception
            failures.Add("Password hashing threw: " & ex.Message)
        End Try

        Dim february = Payroll.MonthRange(2024, 2)
        If february.StartDate <> New Date(2024, 2, 1) Then
            failures.Add("MonthRange start date is wrong.")
        End If
        If february.EndDateExclusive <> New Date(2024, 3, 1) Then
            failures.Add("MonthRange end date is wrong.")
        End If
        If (february.EndDateExclusive - february.StartDate).Days <> 29 Then
            failures.Add("MonthRange did not account for a leap year.")
        End If

        Dim rows As New List(Of PayrollRow) From {
            New PayrollRow With {.MonthlySalary = 1000D},
            New PayrollRow With {.MonthlySalary = 2500.5D}
        }
        If Payroll.MonthlyTotal(rows) <> 3500.5D Then
            failures.Add("Payroll.MonthlyTotal returned the wrong total.")
        End If

        Dim report As String
        If failures.Count = 0 Then
            report = "Self-test passed."
        Else
            report = "Self-test failures:" & Environment.NewLine &
                     String.Join(Environment.NewLine, failures)
        End If

        Try
            File.WriteAllText(
                Path.Combine(Path.GetTempPath(), "employee-management-selftest.txt"),
                report)
        Catch ex As Exception
            Debug.WriteLine("Could not write self-test report: " & ex.Message)
        End Try

        Return (failures.Count = 0, report)
    End Function
End Module
```

### `ApplicationEvents.vb`

```vb
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            For Each argument As String In e.CommandLine
                If String.Equals(argument, "--self-test", StringComparison.OrdinalIgnoreCase) Then
                    Dim result = SelfTest.Run()
                    Console.WriteLine(result.Report)
                    Environment.Exit(If(result.Passed, 0, 1))
                End If
            Next
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show(
                e.Exception.Message,
                "Unexpected error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Sub
    End Class
End Namespace
```

### `LoginForm.vb`

```vb
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
```

### `SignupForm.vb`

```vb
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
```

### `HrDashboardForm.vb`

```vb
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
```

### `EmployeeDashboardForm.vb`

```vb
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
```

### `EmployeeEditForm.vb`

```vb
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
```



## Appendix B: Database schema

The full `schema.sql` is reproduced in section 3.1, and the application also creates the tables on
startup. Keep the two definitions in sync if you change a column.

## Appendix C: Icon attribution

The app uses Flaticon icons under the Flaticon free license. The login screen links back to
Flaticon. Keep the link and the attribution file when redistributing.

| File | Icon | Flaticon ID |
| --- | --- | --- |
| `app.png` | Briefcase | 6822200 |
| `employees.png` | Employee management | 12253849 |
| `clock.png` | Clock | 16106466 |
| `payroll.png` | Payroll receipt | 6737424 |
| `profile.png` | Profile | 15190513 |
| `add.png` | Add employee | 16968011 |
| `edit.png` | Edit | 7270001 |
| `approve.png` | Check / approve | 13958672 |
| `block.png` | Block | 5543586 |
| `search.png` | Search | 45391 |
| `refresh.png` | Refresh | 60918 |
| `delete.png` | Trash / delete | 70245 |
| `logout.png` | Log out | 4400629 |

Search Flaticon for the ID or title to find the original author page. The full per-icon links are in
`Icons/ATTRIBUTION.md`.

---

## Definition of done

- The solution builds with 0 errors and 0 warnings.
- `dotnet run --no-build -- --self-test` exits with code 0.
- The login screen connects, the seeded HR account signs in, and the HR dashboard opens.
- A new signup is pending and cannot sign in until approved.
- HR can add/edit/approve/deactivate accounts.
- Employee work entries respect the future-date and 24-hour rules.
- Payroll shows fixed monthly salary, total hours, distinct days, and a visible total.
- Every form opens in the Visual Studio designer with Segoe UI 9pt and no clipped controls.
