Public Class UpdateRecord20to59YearsOld

    Public residentID As Integer

    Private Sub UpdateRecord20to59YearsOld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")

            dtpdob.MaxDate = Date.Today.AddYears(-20)
            dtpdob.MinDate = Date.Today.AddYears(-60).AddDays(1)


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
                "SELECT r.fullname, r.birthdate, r.sexid, r.purokid, " &
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
        Dim age As Integer = Date.Today.Year - dtpDob.Value.Year
        If dtpDob.Value > Date.Today.AddYears(-age) Then age -= 1

        txtage.Text = age.ToString()

        Dim sex As String = cbSex.Text
        chkPregnant.Visible = (sex = "Female" AndAlso age >= 20 AndAlso age <= 59)
    End Sub

    Private Sub dtpDob_ValueChanged(sender As Object, e As EventArgs) Handles dtpDob.ValueChanged
        UpdateAgeLogic()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim purokVal As Integer = CInt(CallByName(cbPurok.SelectedItem, "Value", CallType.Get))
            Dim sexVal As Integer = CInt(CallByName(cbSex.SelectedItem, "Value", CallType.Get))
            Dim ageVal As Integer = CInt(txtage.Text)

            Dim query As String =
                "UPDATE residents SET " &
                "fullname='" & txtfullname.Text.Trim() & "', " &
                "birthdate='" & dtpdob.Value.ToString("MM-dd-yyyy") & "', " &
                "contact='" & txtcontact.Text.Trim() & "', " &
                "sexid=" & sexVal & ", " &
                "purokid=" & purokVal & ", " &
                "age=" & ageVal & " " &
                "WHERE residentid=" & residentID

            SetQuery(query)
            LogAction("residents", "Update", "Updated record: " & txtfullname.Text.Trim())

            If cbSex.Text = "Female" AndAlso ageVal >= 10 AndAlso ageVal <= 49 Then

                If chkPregnant.Visible AndAlso chkPregnant.Checked Then
                    Dim pregConditionId As Integer = GetScalarValue(
                        "SELECT classid FROM classification WHERE classname='Pregnant' LIMIT 1")

                    If pregConditionId > 0 Then
                        SetQuery("UPDATE residents SET conditionid = " & pregConditionId & " WHERE residentid=" & residentID)
                        LogAction("residents", "Update", "Marked resident as Pregnant: " & txtfullname.Text.Trim())

                        Dim existingPreg As Integer = GetScalarValue(
                            "SELECT COUNT(*) FROM pregnantdetails WHERE residentid=" & residentID & " AND end_date IS NULL")

                        If existingPreg = 0 Then
                            Dim insertPreg As String =
                                "INSERT INTO pregnantdetails (residentid, trimester, lastcheckup, start_date) " &
                                "VALUES (" & residentID & ", '1', CURDATE(), CURDATE())"
                            SetQuery(insertPreg)
                            LogAction("pregnantdetails", "Add", "Auto-created pregnancy record (First Trimester) for: " & txtfullname.Text.Trim())
                        End If
                    End If

                ElseIf chkPregnant.Visible AndAlso Not chkPregnant.Checked Then
                    Dim confirmEnd As DialogResult = MessageBox.Show(
                        "Are you sure you want to mark this pregnancy as ended?",
                        "Confirm End of Pregnancy",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If confirmEnd = DialogResult.Yes Then
                        SetQuery("UPDATE pregnantdetails SET end_date = CURDATE() WHERE residentid=" & residentID & " AND end_date IS NULL")
                        SetQuery("UPDATE residents SET conditionid = NULL WHERE residentid=" & residentID)
                        LogAction("pregnantdetails", "Update", "Ended pregnancy record for: " & txtfullname.Text.Trim())
                    End If
                End If

            Else
                SetQuery("UPDATE residents SET conditionid = NULL WHERE residentid=" & residentID)
            End If

            MessageBox.Show("Record updated successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnCancel.PerformClick()

        Catch ex As Exception
            MessageBox.Show("Error updating resident: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim viewForm As New View20to59YearsOld()
        viewForm.residentId = residentID

        Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
        mainForm.PanelContainer.Controls.Clear()
        viewForm.TopLevel = False
        viewForm.FormBorderStyle = FormBorderStyle.None
        viewForm.Dock = DockStyle.Fill
        mainForm.PanelContainer.Controls.Add(viewForm)
        viewForm.Show()
    End Sub
End Class
