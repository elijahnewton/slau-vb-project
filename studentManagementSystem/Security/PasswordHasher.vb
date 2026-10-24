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
