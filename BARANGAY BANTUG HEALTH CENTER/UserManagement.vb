Public Class UserManagement
    Private Sub Users_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")

        rbBHW.Checked = True
        chkActive.Checked = True
        txtpassword.PasswordChar = "*"
        lblpassmsg.Text = ""
        lblpassmsg.Visible = False

        fillUsers()
        clearFields()
    End Sub

    Public Sub fillUsers()
        GetQuery("SELECT userid, username, fullname, role, status FROM users", "users")

        lvusers.Items.Clear()

        For i = 0 To ds.Tables("users").Rows.Count - 1
            lvusers.Items.Add(ds.Tables("users").Rows(i).Item("userid").ToString)
            With lvusers.Items(i).SubItems
                .Add(ds.Tables("users").Rows(i).Item("username").ToString)
                .Add("********")
                .Add(ds.Tables("users").Rows(i).Item("fullname").ToString)
                .Add(ds.Tables("users").Rows(i).Item("role").ToString)
                .Add(ds.Tables("users").Rows(i).Item("status").ToString)
            End With
        Next
    End Sub

    Private Sub lvusers_DoubleClick(sender As Object, e As EventArgs) Handles lvusers.DoubleClick
        If lvusers.SelectedItems.Count = 0 Then Exit Sub

        cid = Integer.Parse(lvusers.FocusedItem.SubItems(0).Text)
        GetQuery("SELECT username, passwordhash, fullname, role, status FROM users WHERE userid = " & cid, "users")

        txtusername.Text = ds.Tables("users").Rows(0).Item("username").ToString
        txtpassword.Text = "********" ' Show masked password
        txtfullname.Text = ds.Tables("users").Rows(0).Item("fullname").ToString

        ' --- ROLE ---
        Dim r As String = ds.Tables("users").Rows(0).Item("role").ToString
        If r = "admin" Then rbAdmin.Checked = True
        If r = "bns" Then rbBNS.Checked = True
        If r = "bhw" Then rbBHW.Checked = True

        ' --- STATUS ---
        Dim st As String = ds.Tables("users").Rows(0).Item("status").ToString
        chkActive.Checked = (st = "active")

        ' Enable editing fields
        txtusername.Enabled = True
        txtpassword.Enabled = True
        txtfullname.Enabled = True
        rbAdmin.Enabled = True
        rbBNS.Enabled = True
        rbBHW.Enabled = True
        chkActive.Enabled = True

        ' Clear password message
        lblpassmsg.Text = ""
        lblpassmsg.Visible = False
    End Sub

    Public Sub clearFields()
        txtusername.Text = ""
        txtpassword.Text = ""
        txtfullname.Text = ""

        rbBHW.Checked = True
        chkActive.Checked = True

        ' Clear selection (set to 0 for new user)
        cid = 0

        ' Enable all fields for new entry
        txtusername.Enabled = True
        txtpassword.Enabled = True
        txtfullname.Enabled = True
        rbAdmin.Enabled = True
        rbBNS.Enabled = True
        rbBHW.Enabled = True
        chkActive.Enabled = True

        ' Clear password message
        lblpassmsg.Text = ""
        lblpassmsg.Visible = False
    End Sub

    Private Function GetSelectedRole() As String
        If rbAdmin.Checked Then Return "admin"
        If rbBNS.Checked Then Return "bns"
        Return "bhw"
    End Function

    Private Function GetSelectedStatus() As String
        If chkActive.Checked Then Return "active"
        Return "inactive"
    End Function

    ' Validate password strength when user types in password field
    Private Sub txtpassword_TextChanged(sender As Object, e As EventArgs) Handles txtpassword.TextChanged
        If cid > 0 AndAlso txtpassword.Text = "********" Then
            ' For existing users with masked password, clear message
            lblpassmsg.Text = ""
            lblpassmsg.Visible = False
            Return
        End If

        ValidatePasswordStrength()
    End Sub

    Private Sub ValidatePasswordStrength()
        Dim password As String = txtpassword.Text.Trim()

        If password = "" Then
            lblpassmsg.Text = ""
            lblpassmsg.Visible = False
            Return
        End If

        ' Check minimum length
        If password.Length < 8 Then
            lblpassmsg.Text = "Password must be at least 8 characters"
            lblpassmsg.ForeColor = Color.Red
            lblpassmsg.Visible = True
            Return
        End If

        ' Check if contains at least one number
        Dim hasNumber As Boolean = False
        For Each c As Char In password
            If Char.IsDigit(c) Then
                hasNumber = True
                Exit For
            End If
        Next

        If Not hasNumber Then
            lblpassmsg.Text = "Password must contain at least one number"
            lblpassmsg.ForeColor = Color.Red
            lblpassmsg.Visible = True
            Return
        End If

        ' Password meets requirements
        If cid = 0 Then
            ' For new users
            lblpassmsg.Text = "Strong password"
            lblpassmsg.ForeColor = Color.Green
            lblpassmsg.Visible = True
        Else
            ' For existing users (password changed)
            lblpassmsg.Text = "Valid password"
            lblpassmsg.ForeColor = Color.Green
            lblpassmsg.Visible = True
        End If
    End Sub

    Private Function IsValidPassword(password As String) As Boolean
        If password.Length < 8 Then Return False

        ' Check if contains at least one number
        For Each c As Char In password
            If Char.IsDigit(c) Then Return True
        Next

        Return False
    End Function

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtusername.Text.Trim = "" Or txtfullname.Text.Trim = "" Then
            MsgBox("Username and Fullname are required!", MsgBoxStyle.Information, "Missing Data")
            Exit Sub
        End If

        ' If adding new user, password is required and must be valid
        If cid = 0 Then
            If txtpassword.Text.Trim = "" Then
                MsgBox("Password is required for new user!", MsgBoxStyle.Information, "Missing Data")
                Exit Sub
            End If

            If Not IsValidPassword(txtpassword.Text.Trim) Then
                MsgBox("Password must be at least 8 characters long and contain at least one number!", MsgBoxStyle.Exclamation, "Weak Password")
                Exit Sub
            End If
        End If

        ' For existing users, if password is changed, validate it
        If cid > 0 AndAlso txtpassword.Text <> "********" AndAlso txtpassword.Text.Trim <> "" Then
            If Not IsValidPassword(txtpassword.Text.Trim) Then
                MsgBox("Password must be at least 8 characters long and contain at least one number!", MsgBoxStyle.Exclamation, "Weak Password")
                Exit Sub
            End If
        End If

        If cid = 0 Then
            ' ADD NEW USER
            ' Check duplicate username
            GetQuery("SELECT username FROM users WHERE username = '" & txtusername.Text.Trim & "'", "checkuser")
            If ds.Tables("checkuser").Rows.Count > 0 Then
                MsgBox("Username already exists!", MsgBoxStyle.Exclamation, "Duplicate")
                Exit Sub
            End If

            Dim passwordHash As String = GenerateSecureHash(txtpassword.Text.Trim)
            Dim selectedRole As String = GetSelectedRole()
            Dim selectedStatus As String = GetSelectedStatus()

            If MsgBox("Are you sure you want to add this user?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm") = MsgBoxResult.Yes Then
                SetQuery("INSERT INTO users (username, passwordhash, fullname, role, status) VALUES (" &
                         "'" & txtusername.Text.Trim & "'," &
                         "'" & passwordHash & "'," &
                         "'" & txtfullname.Text.Trim & "'," &
                         "'" & selectedRole & "'," &
                         "'" & selectedStatus & "')")

                LogAction("user", "Add", "Added new user: " & txtfullname.Text.Trim())
                MsgBox("User added!", MsgBoxStyle.Information)
                clearFields()
                fillUsers()
            End If
        Else
            ' UPDATE EXISTING USER
            ' Duplicate check
            GetQuery("SELECT username FROM users WHERE username = '" & txtusername.Text.Trim & "' AND userid <> " & cid, "checkuser")
            If ds.Tables("checkuser").Rows.Count > 0 Then
                MsgBox("Username already exists!", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            Dim selectedRole As String = GetSelectedRole()
            Dim selectedStatus As String = GetSelectedStatus()
            Dim updateQuery As String

            If txtpassword.Text.Trim <> "" AndAlso txtpassword.Text <> "********" Then
                ' Only update password if it's changed (not the masked value) and valid
                Dim passwordHash As String = GenerateSecureHash(txtpassword.Text.Trim)
                updateQuery =
                    "UPDATE users SET " &
                    "username = '" & txtusername.Text.Trim & "', " &
                    "passwordhash = '" & passwordHash & "', " &
                    "fullname = '" & txtfullname.Text.Trim & "', " &
                    "role = '" & selectedRole & "', " &
                    "status = '" & selectedStatus & "' " &
                    "WHERE userid = " & cid

                LogAction("user", "Update", "Updated user: " & txtfullname.Text.Trim())
            Else
                updateQuery =
                    "UPDATE users SET " &
                    "username = '" & txtusername.Text.Trim & "', " &
                    "fullname = '" & txtfullname.Text.Trim & "', " &
                    "role = '" & selectedRole & "', " &
                    "status = '" & selectedStatus & "' " &
                    "WHERE userid = " & cid

                LogAction("user", "Update", "Updated user: " & txtfullname.Text.Trim())
            End If

            If MsgBox("Are you sure you want to update this user?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm") = MsgBoxResult.Yes Then
                SetQuery(updateQuery)
                MsgBox("User updated!", MsgBoxStyle.Information)
                clearFields()
                fillUsers()
            End If
        End If
    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        clearFields()
    End Sub

    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        Dim s As String = txtsearch.Text.Trim()
        Dim query As String

        If s = "" Then
            query = "SELECT userid, username, fullname, role, status FROM users"
        Else
            query =
                "SELECT userid, username, fullname, role, status FROM users WHERE " &
                "username LIKE '%" & s & "%' OR " &
                "fullname LIKE '%" & s & "%' OR " &
                "role LIKE '%" & s & "%' OR " &
                "status LIKE '%" & s & "%'"
        End If

        GetQuery(query, "users")
        lvusers.Items.Clear()

        For i = 0 To ds.Tables("users").Rows.Count - 1
            lvusers.Items.Add(ds.Tables("users").Rows(i).Item("userid").ToString)
            With lvusers.Items(i).SubItems
                .Add(ds.Tables("users").Rows(i).Item("username").ToString)
                .Add("********")
                .Add(ds.Tables("users").Rows(i).Item("fullname").ToString)
                .Add(ds.Tables("users").Rows(i).Item("role").ToString)
                .Add(ds.Tables("users").Rows(i).Item("status").ToString)
            End With
        Next
    End Sub
End Class