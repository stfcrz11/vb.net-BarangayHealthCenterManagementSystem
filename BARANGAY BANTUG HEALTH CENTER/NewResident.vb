Public Class NewResident

    Private userTouchedDOB As Boolean = False

    Private Sub NewResident_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")

            FillPurok()
            FillSex()

            btnSave.Enabled = True
            txtflname.Enabled = True
            txtage.Enabled = False
            txtguardian.Enabled = False
            txtcontact.Enabled = True
            dtpdob.Enabled = True
            cbPurok.Enabled = True
            cbSex.Enabled = True

            lblguardian.Visible = False
            txtguardian.Visible = False

            txtage.Text = "0"
            userTouchedDOB = False

            dtpdob.MinDate = New Date(1900, 1, 1)
            dtpdob.MaxDate = Date.Today
            dtpdob.Value = Date.Today

            Dim tt As New ToolTip()
            tt.SetToolTip(txtage, "Age is automatically calculated and cannot be changed.")

        Catch ex As Exception
            MsgBox("Error loading form: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub FillPurok()
        Try
            GetQuery("SELECT * FROM purok", "purok")
            cbPurok.Items.Clear()
            For i As Integer = 0 To ds.Tables("purok").Rows.Count - 1
                cbPurok.Items.Add(New With {
                    .Text = ds.Tables("purok").Rows(i)("purokname").ToString(),
                    .Value = ds.Tables("purok").Rows(i)("purokid")
                })
            Next
            cbPurok.DisplayMember = "Text"
            cbPurok.ValueMember = "Value"
        Catch ex As Exception
            MsgBox("Error loading Purok: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub FillSex()
        Try
            GetQuery("SELECT * FROM sex", "sex")
            cbSex.Items.Clear()
            For i As Integer = 0 To ds.Tables("sex").Rows.Count - 1
                cbSex.Items.Add(New With {
                    .Text = ds.Tables("sex").Rows(i)("sexname").ToString(),
                    .Value = ds.Tables("sex").Rows(i)("sexid")
                })
            Next
            cbSex.DisplayMember = "Text"
            cbSex.ValueMember = "Value"
        Catch ex As Exception
            MsgBox("Error loading Sex: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Function GetClassificationId(age As Integer) As Integer
        Try
            Dim className As String = ""
            Select Case age
                Case 0 To 4
                    className = "Child"
                Case 5 To 19
                    className = "Youth"
                Case 20 To 59
                    className = "Adult"
                Case Is >= 60
                    className = "Senior"
                Case Else
                    className = "Adult"
            End Select

            GetQuery("SELECT classid FROM classification WHERE classname = '" & className & "' LIMIT 1", "classification")
            If ds.Tables("classification").Rows.Count > 0 Then
                Return CInt(ds.Tables("classification").Rows(0)("classid"))
            Else
                Return 0
            End If

        Catch ex As Exception
            MsgBox("Error getting classification: " & ex.Message, MsgBoxStyle.Critical)
            Return 0
        End Try
    End Function

    Private Sub CalculateAge()
        Try
            Dim today As Date = Date.Today
            Dim age As Integer = today.Year - dtpdob.Value.Year
            If dtpdob.Value > today.AddYears(-age) Then age -= 1

            txtage.Text = age.ToString()

            If age <= 19 Then
                lblguardian.Visible = True
                txtguardian.Visible = True
                txtguardian.Enabled = True
            Else
                lblguardian.Visible = False
                txtguardian.Visible = False
                txtguardian.Clear()
            End If

        Catch ex As Exception
            MsgBox("Error calculating age: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub dtpdob_CloseUp(sender As Object, e As EventArgs) Handles dtpdob.CloseUp
        userTouchedDOB = True
        CalculateAge()
    End Sub

    Private Sub dtpdob_KeyDown(sender As Object, e As KeyEventArgs) Handles dtpdob.KeyDown
        userTouchedDOB = True
        CalculateAge()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If String.IsNullOrEmpty(txtflname.Text) Or cbPurok.SelectedIndex = -1 Or cbSex.SelectedIndex = -1 Or
                String.IsNullOrEmpty(txtage.Text) Or String.IsNullOrEmpty(txtcontact.Text) Then

                MsgBox("Please complete all required fields!", MsgBoxStyle.Exclamation)
                Return
            End If

            Dim sexVal As Integer = CInt(CallByName(cbSex.SelectedItem, "Value", CallType.Get))
            Dim purokVal As Integer = CInt(CallByName(cbPurok.SelectedItem, "Value", CallType.Get))
            Dim ageVal As Integer = CInt(txtage.Text.Trim())
            Dim classVal As Integer = GetClassificationId(ageVal)

            Dim guardianValue As String = If(txtguardian.Visible AndAlso Not String.IsNullOrEmpty(txtguardian.Text.Trim()),
                                            "'" & txtguardian.Text.Trim() & "'", "NULL")

            Dim query As String = "INSERT INTO residents (fullname, birthdate, age, sexid, purokid, contact, guardianname, classid, status) VALUES(" &
                    "'" & txtflname.Text.Trim() & "', " &
                    "'" & dtpdob.Value.ToString("yyyy-MM-dd") & "', " &
                    ageVal & ", " &
                    sexVal & ", " &
                    purokVal & ", " &
                    "'" & txtcontact.Text.Trim() & "', " &
                    guardianValue & ", " &
                    classVal & ", 'Alive')"

            If MsgBox("Are you sure you want to add this resident?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                SetQuery(query)
                LogAction("residents", "Add", "Added new resident: " & txtflname.Text.Trim())
                MsgBox("Resident added successfully!", MsgBoxStyle.Information)
                ResetForm()
            End If

        Catch ex As Exception
            MsgBox("Error saving resident: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub ResetForm()
        txtflname.Clear()
        txtage.Text = "0"
        txtguardian.Clear()
        txtcontact.Clear()
        cbPurok.SelectedIndex = -1
        cbSex.SelectedIndex = -1
        dtpdob.Value = Date.Today
        lblguardian.Visible = False
        txtguardian.Visible = False
        userTouchedDOB = False
        txtflname.Focus()
    End Sub

    Private Sub txtContact_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcontact.KeyPress
        AllowOnlyNumericContact(txtContact, e)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            If txtflname.Text <> "" Or txtage.Text <> "" Or txtguardian.Text <> "" Or
               txtcontact.Text <> "" Or cbPurok.SelectedIndex <> -1 Or
               cbSex.SelectedIndex <> -1 Then

                Dim result As MsgBoxResult
                result = MsgBox("Are you sure you want to cancel?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm Cancel")
                If result = MsgBoxResult.Yes Then ResetForm()
            Else
                ResetForm()
            End If
        Catch ex As Exception
            MsgBox("Error cancelling: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub NewResident_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.S Then
            e.SuppressKeyPress = True
            btnSave.PerformClick()
        End If
    End Sub

End Class
