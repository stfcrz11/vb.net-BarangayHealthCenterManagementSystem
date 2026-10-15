Public Class UpdateRecordPregnantWoman

    Public residentID As Integer
    Private selectedVitalID As Integer = 0
    Private selectedMedID As Integer = 0
    Private currentPregnantID As Integer = 0

    Private Sub UpdateRecordPregnantWoman_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")
            LoadResidentDetails()
            LoadPregnantDetails()
            LoadVitalRecords()
            LoadMedicationRecords()
            LoadPregnancyMedications()
            TogglePregnancySection(False)
            ToggleVitalsSection(False)
            ToggleMedicationSection(False)
            dtpEnd.Enabled = False
            dtpEnd.Checked = False
        Catch ex As Exception
            MessageBox.Show("Error loading pregnant woman update form: " & ex.Message)
        End Try
    End Sub

    Private Sub TogglePregnancySection(state As Boolean)
        cbTrimester.Enabled = False
        dtpLastCheckup.Enabled = state
        btnSaveProfile.Enabled = state
        btnEndPregnancy.Enabled = state
    End Sub

    Private Sub ToggleVitalsSection(state As Boolean)
        dtpVitals.Enabled = state
        nudSystolic.Enabled = state
        nudDiastolic.Enabled = state
        nudTemp.Enabled = state
        nudHeartbeat.Enabled = state
        btnSaveVitals.Enabled = state
    End Sub

    Private Sub ToggleMedicationSection(state As Boolean)
        dtpMed.Enabled = state
        cbMedName.Enabled = state
        btnSaveMS.Enabled = state
    End Sub

    Private Function CalculateTrimester(startDate As Date) As String
        Dim totalDays As Integer = DateDiff(DateInterval.Day, startDate, Date.Today)
        Dim weeks As Integer = totalDays \ 7
        If weeks <= 13 Then
            Return "1"
        ElseIf weeks <= 26 Then
            Return "2"
        Else
            Return "3"
        End If
    End Function

    Private Function TrimesterName(value As String) As String
        Select Case value
            Case "1" : Return "First"
            Case "2" : Return "Second"
            Case "3" : Return "Third"
            Case Else : Return ""
        End Select
    End Function

    Private Sub LoadResidentDetails()
        Try
            Dim query As String = "SELECT fullname, contact, birthdate FROM residents WHERE residentid=" & residentID
            GetQuery(query, "res_preg")
            If ds.Tables("res_preg").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("res_preg").Rows(0)
                txtFullname.Text = row("fullname").ToString()
                txtContact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString())
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading resident details: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPregnantDetails()
        Try
            Dim query As String = "SELECT pregnantid, trimester, lastcheckup, start_date FROM pregnantdetails WHERE residentid=" & residentID & " AND end_date IS NULL ORDER BY start_date DESC LIMIT 1"
            GetQuery(query, "preg_details")
            If ds.Tables("preg_details").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("preg_details").Rows(0)
                currentPregnantID = CInt(row("pregnantid"))
                Dim startDate As Date = Convert.ToDateTime(row("start_date"))
                Dim autoTrimester As String = CalculateTrimester(startDate)
                cbTrimester.Text = TrimesterName(autoTrimester)
                dtpLastCheckup.Value = If(IsDBNull(row("lastcheckup")), Date.Today, Convert.ToDateTime(row("lastcheckup")))
                Dim updateQuery As String = "UPDATE pregnantdetails SET trimester='" & autoTrimester & "' WHERE pregnantid=" & currentPregnantID
                SetQuery(updateQuery)
            Else
                currentPregnantID = 0
                cbTrimester.Text = ""
                dtpLastCheckup.Value = Date.Today
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading pregnancy details: " & ex.Message)
        End Try
    End Sub

    Private Sub btnEndPregnancy_Click(sender As Object, e As EventArgs) Handles btnEndPregnancy.Click
        Try
            Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to mark this pregnancy as ended?" & vbCrLf & "This action will close the current pregnancy record.", "Confirm End of Pregnancy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If confirm = DialogResult.Yes Then
                dtpEnd.Enabled = True
                dtpEnd.Checked = True
                dtpEnd.Value = Date.Today
                MessageBox.Show("Pregnancy marked for ending. Please click Save to finalize changes.", "Marked as Ended", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                dtpEnd.Enabled = False
                dtpEnd.Checked = False
            End If
        Catch ex As Exception
            MessageBox.Show("Error confirming pregnancy end: " & ex.Message)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        TogglePregnancySection(True)
    End Sub

    Private Sub btnSaveProfile_Click(sender As Object, e As EventArgs) Handles btnSaveProfile.Click
        Try
            Dim lastCheck As String = dtpLastCheckup.Value.ToString("MM-dd-yyyy")
            Dim endDateValue As String = Nothing
            Dim confirmEnd As Boolean = False
            If dtpEnd.Checked Then
                Dim response As DialogResult = MessageBox.Show("Are you sure this pregnancy has ended?", "Confirm Pregnancy End", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If response = DialogResult.Yes Then
                    confirmEnd = True
                    endDateValue = dtpEnd.Value.ToString("MM-dd-yyyy")
                End If
            End If
            If currentPregnantID = 0 Then
                Dim startDate As Date = Date.Today
                Dim trimesterNum As String = CalculateTrimester(startDate)
                Dim insertQuery As String = "INSERT INTO pregnantdetails (residentid, trimester, lastcheckup, start_date" & If(confirmEnd, ", end_date", "") & ") VALUES (" & residentID & ", '" & trimesterNum & "', '" & lastCheck & "', '" & startDate.ToString("MM-dd-yyyy") & "'" & If(confirmEnd, ", '" & endDateValue & "'", "") & ")"
                SetQuery(insertQuery)
                LogAction("pregnantdetails", "Add", "Added new pregnancy record for: " & txtFullname.Text.Trim())
                If confirmEnd Then
                    Dim clearCondQuery As String = "UPDATE residents SET conditionid=NULL WHERE residentid=" & residentID
                    SetQuery(clearCondQuery)
                    LogAction("residents", "Update", "Cleared pregnancy condition for: " & txtFullname.Text.Trim())
                    MessageBox.Show("Pregnancy marked as ended. Resident condition cleared.", "Pregnancy Ended", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Pregnancy profile added successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                Dim query As String = "SELECT start_date FROM pregnantdetails WHERE pregnantid=" & currentPregnantID
                GetQuery(query, "start_info")
                Dim startDate As Date = Convert.ToDateTime(ds.Tables("start_info").Rows(0)("start_date"))
                Dim trimesterNum As String = CalculateTrimester(startDate)
                Dim updateQuery As String = "UPDATE pregnantdetails SET trimester='" & trimesterNum & "', lastcheckup='" & lastCheck & "'"
                If confirmEnd Then
                    updateQuery &= ", end_date='" & endDateValue & "'"
                Else
                    updateQuery &= ", end_date=NULL"
                End If
                updateQuery &= " WHERE pregnantid=" & currentPregnantID
                SetQuery(updateQuery)
                LogAction("pregnantdetails", "Update", "Updated pregnancy record for: " & txtFullname.Text.Trim())
                If confirmEnd Then
                    Dim clearCondQuery As String = "UPDATE residents SET conditionid=NULL WHERE residentid=" & residentID
                    SetQuery(clearCondQuery)
                    LogAction("residents", "Update", "Cleared pregnancy condition for: " & txtFullname.Text.Trim())
                    MessageBox.Show("Pregnancy marked as ended. Resident condition cleared.", "Pregnancy Ended", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Pregnancy profile updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            TogglePregnancySection(False)
            LoadPregnantDetails()
        Catch ex As Exception
            MessageBox.Show("Error saving pregnancy profile: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadVitalRecords()
        Try
            Dim query As String = "SELECT vitalid, recorddate, bloodpressure, temperature, fetalheartbeat FROM vitalsigns WHERE pregnantid IN (SELECT pregnantid FROM pregnantdetails WHERE residentid=" & residentID & " AND end_date IS NULL) ORDER BY recorddate DESC"
            GetQuery(query, "vital_list")
            lvVitalSigns.Items.Clear()
            For Each row As DataRow In ds.Tables("vital_list").Rows
                Dim item As New ListViewItem(row("vitalid").ToString)
                item.SubItems.Add(Format(row("recorddate"), "MM-dd-yyyy"))
                item.SubItems.Add(If(IsDBNull(row("bloodpressure")), "", row("bloodpressure").ToString()))
                item.SubItems.Add(If(IsDBNull(row("temperature")), "", row("temperature").ToString()))
                item.SubItems.Add(If(IsDBNull(row("fetalheartbeat")), "", row("fetalheartbeat").ToString()))
                lvVitalSigns.Items.Add(item)
            Next
        Catch ex As Exception
            MessageBox.Show("Error loading vital signs: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadMedicationRecords()
        Try
            Dim query As String = "SELECT medicationid, recorddate, medicinename FROM medication WHERE pregnantid = (SELECT pregnantid FROM pregnantdetails WHERE residentid=" & residentID & " AND end_date IS NULL LIMIT 1) ORDER BY recorddate DESC"
            GetQuery(query, "med_list")
            lvMed.Items.Clear()
            For Each row As DataRow In ds.Tables("med_list").Rows
                Dim item As New ListViewItem(row("medicationid").ToString)
                item.SubItems.Add(Format(row("recorddate"), "MM-dd-yyyy"))
                item.SubItems.Add(If(IsDBNull(row("medicinename")), "", row("medicinename").ToString()))
                lvMed.Items.Add(item)
            Next
        Catch ex As Exception
            MessageBox.Show("Error loading medication records: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPregnancyMedications()
        Try
            cbMedName.Items.Clear()
            Dim sql As String = "SHOW COLUMNS FROM medication LIKE 'medicinename'"
            GetQuery(sql, "enum_data")
            If ds.Tables.Contains("enum_data") AndAlso ds.Tables("enum_data").Rows.Count > 0 Then
                Dim enumText As String = ds.Tables("enum_data").Rows(0)("Type").ToString()
                enumText = enumText.Replace("enum(", "").Replace(")", "").Replace("'", "")
                Dim values() As String = enumText.Split(","c)
                For Each val As String In values
                    cbMedName.Items.Add(val.Trim())
                Next
            End If
            cbMedName.SelectedIndex = -1
        Catch ex As Exception
            MessageBox.Show("Error loading ENUM values from medication table: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAddVitals_Click(sender As Object, e As EventArgs) Handles btnAddVitals.Click
        selectedVitalID = 0
        dtpVitals.Value = Date.Today
        nudSystolic.Value = 120
        nudDiastolic.Value = 80
        nudTemp.Value = 36.5
        nudHeartbeat.Value = 140
        ToggleVitalsSection(True)
    End Sub

    Private Sub lvVitalSigns_DoubleClick(sender As Object, e As EventArgs) Handles lvVitalSigns.DoubleClick
        Try
            If lvVitalSigns.SelectedItems.Count = 0 Then Return
            selectedVitalID = CInt(lvVitalSigns.SelectedItems(0).Text)
            Dim query As String = "SELECT * FROM vitalsigns WHERE vitalid=" & selectedVitalID
            GetQuery(query, "vital_detail")
            If ds.Tables("vital_detail").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("vital_detail").Rows(0)
                dtpVitals.Value = Convert.ToDateTime(row("recorddate"))
                Dim bp As String = If(IsDBNull(row("bloodpressure")), "0/0", row("bloodpressure").ToString())
                Dim parts() As String = bp.Split("/"c)
                nudSystolic.Value = Val(parts(0))
                nudDiastolic.Value = Val(parts(1))
                nudTemp.Value = Val(If(IsDBNull(row("temperature")), 0, row("temperature")))
                nudHeartbeat.Value = Val(If(IsDBNull(row("fetalheartbeat")), 0, row("fetalheartbeat")))
                ToggleVitalsSection(True)
                LogAction("vitalsigns", "Edit", "Opened vital sign record for: " & txtFullname.Text.Trim())
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading vitals: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAddMS_Click(sender As Object, e As EventArgs) Handles btnAddMed.Click
        selectedMedID = 0
        dtpMed.Value = Date.Today
        cbMedName.SelectedIndex = -1
        ToggleMedicationSection(True)
        LogAction("medication", "Add", "Enabled adding new medication for: " & txtFullname.Text.Trim())
    End Sub

    Private Sub lvMed_DoubleClick(sender As Object, e As EventArgs) Handles lvMed.DoubleClick
        Try
            If lvMed.SelectedItems.Count = 0 Then Return
            selectedMedID = CInt(lvMed.SelectedItems(0).Text)
            Dim query As String = "SELECT * FROM medication WHERE medicationid=" & selectedMedID
            GetQuery(query, "med_detail")
            If ds.Tables("med_detail").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("med_detail").Rows(0)
                dtpMed.Value = Convert.ToDateTime(row("recorddate"))
                cbMedName.Text = If(IsDBNull(row("medicinename")), "", row("medicinename").ToString())
                ToggleMedicationSection(True)
                LogAction("medication", "Edit", "Opened medication record for: " & txtFullname.Text.Trim())
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading medication record: " & ex.Message)
        End Try
    End Sub

    Private Sub txtContact_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContact.KeyPress
        AllowOnlyNumericContact(txtContact, e)
    End Sub

    Private Sub btnSaveVitals_Click(sender As Object, e As EventArgs) Handles btnSaveVitals.Click
        Try
            If currentPregnantID = 0 Then
                MessageBox.Show("No active pregnancy record found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim recordDate As String = dtpvitals.Value.ToString("MM-dd-yyyy")
            Dim bloodPressure As String = nudSystolic.Value.ToString() & "/" & nudDiastolic.Value.ToString()
            Dim temp As Decimal = nudTemp.Value
            Dim heartbeat As Integer = CInt(nudHeartbeat.Value)
            If selectedVitalID = 0 Then
                Dim insertQuery As String = "INSERT INTO vitalsigns (pregnantid, recorddate, bloodpressure, temperature, fetalheartbeat) VALUES (" & currentPregnantID & ", '" & recordDate & "', '" & bloodPressure & "', " & temp & ", " & heartbeat & ")"
                SetQuery(insertQuery)
                MessageBox.Show("Vital sign record added successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LogAction("vitalsigns", "Add", "Added new vital record for: " & txtFullname.Text.Trim())
            Else
                Dim updateQuery As String = "UPDATE vitalsigns SET recorddate='" & recordDate & "', bloodpressure='" & bloodPressure & "', temperature=" & temp & ", fetalheartbeat=" & heartbeat & " WHERE vitalid=" & selectedVitalID
                SetQuery(updateQuery)
                MessageBox.Show("Vital sign record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LogAction("vitalsigns", "Update", "Updated vital record for: " & txtFullname.Text.Trim())
            End If
            LoadVitalRecords()
            ToggleVitalsSection(False)
        Catch ex As Exception
            MessageBox.Show("Error saving vital signs: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSaveMS_Click(sender As Object, e As EventArgs) Handles btnSaveMS.Click
        Try
            If currentPregnantID = 0 Then
                MessageBox.Show("No active pregnancy record found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If cbMedName.SelectedIndex = -1 Then
                MessageBox.Show("Please select a medication name.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim medDate As String = dtpmed.Value.ToString("MM-dd-yyyy")
            Dim medName As String = cbMedName.Text.Trim()
            If selectedMedID = 0 Then
                Dim insertQuery As String = "INSERT INTO medication (pregnantid, recorddate, medicinename) VALUES (" & currentPregnantID & ", '" & medDate & "', '" & medName & "')"
                SetQuery(insertQuery)
                MessageBox.Show("Medication record added successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LogAction("medication", "Add", "Added medication (" & medName & ") for: " & txtFullname.Text.Trim())
            Else
                Dim updateQuery As String = "UPDATE medication SET recorddate='" & medDate & "', medicinename='" & medName & "' WHERE medicationid=" & selectedMedID
                SetQuery(updateQuery)
                MessageBox.Show("Medication record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LogAction("medication", "Update", "Updated medication (" & medName & ") for: " & txtFullname.Text.Trim())
            End If
            LoadMedicationRecords()
            ToggleMedicationSection(False)
        Catch ex As Exception
            MessageBox.Show("Error saving medication record: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Dim viewForm As New ViewPregnantWoman()
            viewForm.residentId = residentID
            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()
            viewForm.TopLevel = False
            viewForm.FormBorderStyle = FormBorderStyle.None
            viewForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(viewForm)
            viewForm.Show()
        Catch ex As Exception
            MessageBox.Show("Error returning to view form: " & ex.Message)
        End Try
    End Sub

End Class
