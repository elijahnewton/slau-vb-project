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
