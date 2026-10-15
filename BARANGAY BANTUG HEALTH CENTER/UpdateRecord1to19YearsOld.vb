Public Class UpdateRecord1to19YearsOld

    Public residentID As Integer

    Private Sub UpdateRecord1to19YearsOld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")

            dtpdob.MaxDate = Date.Today.AddYears(-1)
            dtpdob.MinDate = Date.Today.AddYears(-20).AddDays(1)


            LoadPurok()
            LoadSex()

            If residentID > 0 Then
                LoadResidentDetails()
            End If

            Dim tt As New ToolTip()
            tt.SetToolTip(txtage, "Age is automatically calculated and cannot be changed.")

        Catch ex As Exception
            MessageBox.Show("Error loading update form: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPurok()
        Try
            GetQuery("SELECT * FROM purok ORDER BY purokname", "purok")
            cbPurok.Items.Clear()

            For Each row As DataRow In ds.Tables("purok").Rows
                cbPurok.Items.Add(New With {
                    .Text = row("purokname").ToString(),
                    .Value = row("purokid")
                })
            Next

            cbPurok.DisplayMember = "Text"
            cbPurok.ValueMember = "Value"

        Catch ex As Exception
            MessageBox.Show("Error loading Purok: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadSex()
        Try
            GetQuery("SELECT * FROM sex ORDER BY sexid", "sex")
            cbSex.Items.Clear()

            For Each row As DataRow In ds.Tables("sex").Rows
                cbSex.Items.Add(New With {
                    .Text = row("sexname").ToString(),
                    .Value = row("sexid")
                })
            Next

            cbSex.DisplayMember = "Text"
            cbSex.ValueMember = "Value"

        Catch ex As Exception
            MessageBox.Show("Error loading Sex: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadResidentDetails()
        Try
            Dim query As String =
                "SELECT r.fullname, r.birthdate, r.sexid, r.purokid, r.guardianname, " &
                "r.contact, TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) AS age, " &
                "s.sexname, r.conditionid " &
                "FROM residents r " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "WHERE r.residentid=" & residentID

            GetQuery(query, "res_edit")

            If ds.Tables("res_edit").Rows.Count = 0 Then Return

            Dim row As DataRow = ds.Tables("res_edit").Rows(0)

            txtfullname.Text = row("fullname").ToString()
            dtpDob.Value = Convert.ToDateTime(row("birthdate"))

            cbSex.SelectedIndex = cbSex.FindStringExact(row("sexname").ToString())
            cbPurok.SelectedIndex = cbPurok.FindStringExact(row("purokid").ToString())

            txtGuardian.Text = If(IsDBNull(row("guardianname")), "", row("guardianname").ToString())
            txtContact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString())

            UpdateAgeLogic()

            Dim pregConditionId As Integer = GetScalarValue(
                "SELECT classid FROM classification WHERE classname='Pregnant' LIMIT 1")

            Dim isPregnant As Boolean = False

            If Not IsDBNull(row("conditionid")) AndAlso CInt(row("conditionid")) = pregConditionId Then

                Dim activePreg As Integer = GetScalarValue(
                    "SELECT COUNT(*) FROM pregnantdetails WHERE residentid=" & residentID &
                    " AND end_date IS NULL")
                If activePreg > 0 Then
                    isPregnant = True
                End If
            End If

            chkPregnant.Checked = isPregnant

        Catch ex As Exception
            MessageBox.Show("Error fetching resident details: " & ex.Message)
        End Try
    End Sub

    Private Sub txtContact_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcontact.KeyPress
        AllowOnlyNumericContact(txtContact, e)
    End Sub

    Private Sub UpdateAgeLogic()
        Dim age As Integer = Date.Today.Year - dtpdob.Value.Year
        If dtpdob.Value > Date.Today.AddYears(-age) Then age -= 1

        txtage.Text = age.ToString()

        Dim sex As String = cbSex.Text
        chkPregnant.Visible = (sex = "Female" AndAlso age >= 10 AndAlso age <= 19)
    End Sub

    Private Sub dtpDob_ValueChanged(sender As Object, e As EventArgs) Handles dtpdob.ValueChanged
        UpdateAgeLogic()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim purokVal As Integer =
                CInt(CallByName(cbPurok.SelectedItem, "Value", CallType.Get))
            Dim sexVal As Integer =
                CInt(CallByName(cbSex.SelectedItem, "Value", CallType.Get))
            Dim ageVal As Integer = CInt(txtage.Text)
            Dim guardianVal As String =
                If(String.IsNullOrEmpty(txtguardian.Text.Trim()),
                   "NULL",
                   "'" & txtguardian.Text.Trim() & "'"
                  )

            Dim query As String =
                "UPDATE residents SET " &
                "fullname='" & txtfullname.Text.Trim() & "', " &
                "birthdate='" & dtpdob.Value.ToString("MM-dd-yyyy") & "', " &
                "contact='" & txtcontact.Text.Trim() & "', " &
                "guardianname=" & guardianVal & ", " &
                "sexid=" & sexVal & ", " &
                "purokid=" & purokVal & ", " &
                "age=" & ageVal & " " &
                "WHERE residentid=" & residentID

            ' --- Save Resident Record ---
            SetQuery(query)
            LogAction("residents", "Update", "Updated record: " & txtfullname.Text)

            ' --- Handle Pregnancy Logic Only for Females Aged 10–49 ---
            If cbSex.Text = "Female" AndAlso txtage.Text >= 10 AndAlso txtage.Text <= 49 Then
                If chkPregnant.Visible AndAlso chkPregnant.Checked Then
                    ' Mark as pregnant
                    Dim pregConditionId As Integer = GetScalarValue(
                        "SELECT classid FROM classification WHERE classname='Pregnant' LIMIT 1")

                    If pregConditionId > 0 Then
                        SetQuery("UPDATE residents SET conditionid = " & pregConditionId &
                                 " WHERE residentid=" & residentID)
                        LogAction("residents", "Update", "Marked as pregnant: " & txtfullname.Text)
                    End If

                    ' Check if there’s already an active pregnancy record
                    Dim pregCheck As Integer = GetScalarValue(
                        "SELECT COUNT(*) FROM pregnantdetails WHERE residentid=" & residentID &
                        " AND end_date IS NULL")

                    If pregCheck = 0 Then
                        SetQuery("INSERT INTO pregnantdetails(residentid, start_date, trimester) VALUES(" &
                                 residentID & ", CURDATE(), 1)")
                        LogAction("pregnantdetails", "Add", "Started pregnancy record for: " & txtfullname.Text)
                    End If

                ElseIf chkPregnant.Visible AndAlso Not chkPregnant.Checked Then
                    ' Only end pregnancy if the resident currently has an active pregnancy record
                    Dim activePreg As Integer = GetScalarValue(
                        "SELECT COUNT(*) FROM pregnantdetails WHERE residentid=" & residentID &
                        " AND end_date IS NULL")

                    If activePreg > 0 Then
                        SetQuery("UPDATE pregnantdetails SET end_date = CURDATE() " &
                                 "WHERE residentid=" & residentID & " AND end_date IS NULL")
                        SetQuery("UPDATE residents SET conditionid = NULL WHERE residentid=" & residentID)
                        LogAction("pregnantdetails", "Update", "Ended pregnancy record for: " & txtfullname.Text)
                    End If
                End If
            Else
                ' Clear pregnancy status if somehow marked for non-female
                SetQuery("UPDATE residents SET conditionid = NULL WHERE residentid=" & residentID)
            End If


            MessageBox.Show("Record updated successfully!")

            btnCancel.PerformClick()

        Catch ex As Exception
            MessageBox.Show("Error updating resident: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim viewForm As New View1to19YearsOld()
        viewForm.residentId = residentID

        Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
        mainForm.PanelContainer.Controls.Clear()
        viewForm.TopLevel = False
        viewForm.FormBorderStyle = FormBorderStyle.None
        viewForm.Dock = DockStyle.Fill
        mainForm.PanelContainer.Controls.Add(viewForm)
        viewForm.Show()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
