# Employee Management System

VB.NET WinForms app with two roles:

- **HR** manages accounts, approves employee signups, assigns positions and monthly salaries, reviews work time, and views monthly payroll.
- **Employee** signs up, waits for HR approval, records daily hours, and sees profile and monthly pay.

Data is stored in a remote MySQL database. Passwords use PBKDF2-SHA256 with a per-user salt.

## Requirements

- .NET 10 SDK (Windows)
- MySQL 5.7+ or 8.x with a database and user
- Visual Studio 2022 17.14+ or Visual Studio 2026 for the Windows Forms designer

## Setup

1. Create the database (or run `schema.sql`):

   ```sql
   CREATE DATABASE employee_management CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```

2. Set the connection string in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "EmployeeDb": "Server=HOST;Port=3306;Database=employee_management;User ID=USER;Password=PASSWORD;"
     },
     "SeedHr": {
       "Username": "admin",
       "Password": "Admin@123"
     }
   }
   ```

   Change `SeedHr` before the first run. The HR account is created only when no HR account exists.
   Do not commit real credentials.

3. Build and run:

   ```powershell
   dotnet build
   dotnet run
   ```

   `appsettings.json` is copied to the output folder on build. Edit the copy in `bin/Debug/net10.0-windows/`
   or edit the project file and rebuild.

## Using the app

1. Sign in with the seeded HR account, or create an employee account from the login screen.
2. New employee accounts are **Pending** and cannot sign in until HR approves them.
3. In the HR dashboard:
   - **Employees**: add/edit accounts, approve pending accounts, deactivate accounts, set position and monthly salary.
   - **Work Time**: review or delete work entries by employee and month.
   - **Payroll**: monthly salary total for all active employees.
4. Employees record work time manually (up to 24 hours per day) and view their fixed monthly salary.

## Editing the UI in Visual Studio

Open `studentManagementSystem.slnx` (or `studentManagementSystem.vbproj` directly), then
double-click any form in Solution Explorer to open the Windows Forms designer. Every form has a
matching `.Designer.vb` file linked with `DependentUpon`, so controls can be dragged, dropped, and
resized normally. Keep UI changes in the designer; the `.vb` files only contain event handlers and
runtime logic.

Form sizes are fixed to the design scale with `Segoe UI 9pt` so the layout matches the designer on
systems that use a larger Windows text size. Icons are applied at runtime in each form's `Load`
handler (see `UiIcons.vb`), so the designer shows empty image slots for those controls.

## Icons

The UI uses black outline icons from [Flaticon](https://www.flaticon.com). They are embedded in the
assembly from the `Icons` folder. See `Icons/ATTRIBUTION.md` for the per-icon list and license note.
The login screen includes an "Icons by Flaticon" link, which must stay in place when redistributing
the app.

## Self-test

```powershell
dotnet run -- --self-test
```

Exit code `0` means the password hashing and payroll checks passed. The report is written to
`%TEMP%\employee-management-selftest.txt`.

## Notes

- Tables are created automatically on startup using `CREATE TABLE IF NOT EXISTS`, which requires the
  MySQL user to have `CREATE` permission. Otherwise run `schema.sql` manually.
- Payroll is a calculated summary only; the app does not record payments.
- Passwords cannot currently be changed inside the app.
