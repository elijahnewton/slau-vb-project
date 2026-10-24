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
