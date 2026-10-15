Public Class LogIn

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")
        txtpwd.PasswordChar = "*"
        lblerrormessage.Visible = False

    End Sub

    Private Sub LoginForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtemail.Focus()
    End Sub

    Private Sub Clear()
        txtemail.Clear()
        txtpwd.Clear()
    End Sub
    Private Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click

        If String.IsNullOrEmpty(txtemail.Text) Or String.IsNullOrEmpty(txtpwd.Text) Then
            ShowError("Username and Password are Required!")
            Return
        End If

        GetQuery("SELECT userid, passwordhash, fullname, role, status FROM users WHERE username = '" & txtemail.Text & "' AND status = 'active'", "users")

        If ds.Tables("users").Rows.Count = 0 Then
            ShowError("Invalid username or password. Try again.")
            Clear()
            Return
        End If

        Dim storedHash As String = ds.Tables("users").Rows(0)("passwordhash").ToString()
        Dim inputHash As String = GenerateSecureHash(txtpwd.Text)

        If inputHash = storedHash Then
            CurrentUser.ID = CInt(ds.Tables("users").Rows(0)("userid"))
            CurrentUser.Name = ds.Tables("users").Rows(0)("fullname").ToString()
            CurrentUser.Role = ds.Tables("users").Rows(0)("role").ToString()
            CurrentUser.Status = ds.Tables("users").Rows(0)("status").ToString()

            Select Case CurrentUser.Role.ToLower()
                Case "admin"
                    Clear()
                    MsgBox("Welcome, Midwife " & CurrentUser.Name & "!", vbInformation)

                Case "bns"
                    Clear()
                    MsgBox("Welcome, BNS " & CurrentUser.Name & "!", vbInformation)

                Case "bhw"
                    Clear()
                    MsgBox("Welcome, BHW " & CurrentUser.Name & "!", vbInformation)
                Case Else
                    ShowError("Unauthorized role. Contact system administrator.")
                    Clear()
                    Exit Sub
            End Select

            Dim main As New TheMainForm()
            main.CurrentUserRole = CurrentUser.Role   ' Pass the logged-in role
            main.Show()
            Me.Hide()

            Clear()
            lblerrormessage.Visible = False

            LogAction("user", "Log In", "Successfully logged in")

        Else
            ShowError("Invalid username or password. Try again.")
        End If

    End Sub

    'Private Function GenerateSecureHash(input As String) As String
    '    Using sha256 As Security.Cryptography.SHA256 = Security.Cryptography.SHA256.Create()
    '        Dim bytes As Byte() = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input))
    '        Dim builder As New System.Text.StringBuilder()

    '        For i As Integer = 0 To bytes.Length - 1
    '            builder.Append(bytes(i).ToString("x2"))
    '        Next

    '        Return builder.ToString()
    '    End Using
    'End Function

    Private Sub ShowError(message As String)
        lblerrormessage.Text = message
        lblerrormessage.ForeColor = Color.Red
        lblerrormessage.Visible = True
        ErrorTimer.Stop()
        ErrorTimer.Start()
    End Sub

    Private Sub ErrorTimer_Tick(sender As Object, e As EventArgs) Handles ErrorTimer.Tick
        lblerrormessage.Visible = False
        ErrorTimer.Stop()
    End Sub

    Private Sub showpwd_Click(sender As Object, e As EventArgs) Handles showpwd.Click
        showpwd.Visible = False
        hidepwd.Visible = True
        txtpwd.PasswordChar = Nothing
    End Sub

    Private Sub hidepwd_Click(sender As Object, e As EventArgs) Handles hidepwd.Click
        showpwd.Visible = True
        hidepwd.Visible = False
        txtpwd.PasswordChar = "*"
    End Sub

    Private Sub btnexit_Click(sender As Object, e As EventArgs) Handles btnexit.Click
        Application.Exit()
    End Sub

    Private Sub txtemail_KeyDown(sender As Object, e As KeyEventArgs) Handles txtemail.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtpwd.Focus()
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.Down Then
            txtpwd.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtpwd_KeyDown(sender As Object, e As KeyEventArgs) Handles txtpwd.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnlogin.PerformClick()
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.Up Then
            txtemail.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub


End Class
