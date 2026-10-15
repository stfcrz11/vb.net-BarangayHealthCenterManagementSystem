Public Class UpdateRecordSeniorCitizen

    Public residentID As Integer
    Private selectedRecordID As Integer = 0

    Private Sub UpdateRecordSeniorCitizen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")
            LoadPurok()
            LoadSex()

            ToggleHealthRecordControls(False)

            dtpdob.MaxDate = Date.Today.AddYears(-60)
            dtpdob.MinDate = Date.Today.AddYears(-120)

            If residentID > 0 Then
                LoadResidentDetails()
                LoadHealthRecords()
                PrefillLatestHealthRecord()
            End If

            Dim tt As New ToolTip()
            tt.SetToolTip(txtage, "Age is automatically calculated and cannot be changed.")

        Catch ex As Exception
            MessageBox.Show("Error loading update form: " & ex.Message)
        End Try
    End Sub

    Private Sub ToggleHealthRecordControls(state As Boolean)
        dtpRecordDate.Enabled = state
        txtIllness.Enabled = state
        nudSystolic.Enabled = state
        nudDiastolic.Enabled = state
        nudTemp.Enabled = state
        btnSaveHealthRecords.Enabled = state
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
                "SELECT r.fullname, r.birthdate, r.contact, r.sexid, r.purokid, s.sexname, p.purokname " &
                "FROM residents r " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "WHERE r.residentid=" & residentID

            GetQuery(query, "resident")

            If ds.Tables("resident").Rows.Count = 0 Then Return

            Dim row As DataRow = ds.Tables("resident").Rows(0)

            txtFullname.Text = row("fullname").ToString()
            txtContact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString())
            dtpDob.Value = Convert.ToDateTime(row("birthdate"))
            txtAge.Text = CalculateAge(dtpDob.Value).ToString()

            cbSex.SelectedIndex = cbSex.FindStringExact(row("sexname").ToString())
            cbPurok.SelectedIndex = cbPurok.FindStringExact(row("purokname").ToString())

        Catch ex As Exception
            MessageBox.Show("Error loading resident details: " & ex.Message)
        End Try
    End Sub

    Private Sub txtContact_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcontact.KeyPress
        AllowOnlyNumericContact(txtContact, e)
    End Sub

    Private Function CalculateAge(birthDate As Date) As Integer
        Dim age As Integer = Date.Today.Year - birthDate.Year
        If birthDate > Date.Today.AddYears(-age) Then age -= 1
        Return age
    End Function


    Private Sub dtpDob_ValueChanged(sender As Object, e As EventArgs) Handles dtpDob.ValueChanged
        txtAge.Text = CalculateAge(dtpDob.Value).ToString()
    End Sub

    Private Sub btnSaveProfile_Click(sender As Object, e As EventArgs) Handles btnSaveProfile.Click
        Try
            Dim purokVal As Integer = CInt(CallByName(cbPurok.SelectedItem, "Value", CallType.Get))
            Dim sexVal As Integer = CInt(CallByName(cbSex.SelectedItem, "Value", CallType.Get))

            Dim query As String =
                "UPDATE residents SET " &
                "fullname='" & txtfullname.Text.Trim() & "', " &
                "birthdate='" & dtpdob.Value.ToString("MM-dd-yyyy") & "', " &
                "contact='" & txtcontact.Text.Trim() & "', " &
                "purokid=" & purokVal & ", " &
                "sexid=" & sexVal & ", " &
                "age=" & txtage.Text.Trim() & " " &
                "WHERE residentid=" & residentID

            SetQuery(query)

            LogAction("residents", "Update", "Updated senior profile: " & txtFullname.Text.Trim())

            MessageBox.Show("Profile updated successfully!")

        Catch ex As Exception
            MessageBox.Show("Error saving profile: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadHealthRecords()
        Try
            Dim query As String =
                "SELECT seniorrecordid, recorddate, illness, bloodpressure, temperature " &
                "FROM seniorhealthrecords WHERE residentid=" & residentID &
                " ORDER BY recorddate DESC"

            GetQuery(query, "senior_health")

            lvHealthRecords.Items.Clear()
            For Each row As DataRow In ds.Tables("senior_health").Rows
                Dim item As New ListViewItem(row("seniorrecordid").ToString())
                item.SubItems.Add(Format(row("recorddate"), "MM-dd-yyyy"))
                item.SubItems.Add(If(IsDBNull(row("illness")), "", row("illness").ToString()))
                item.SubItems.Add(If(IsDBNull(row("bloodpressure")), "", row("bloodpressure").ToString()))
                item.SubItems.Add(If(IsDBNull(row("temperature")), "", row("temperature").ToString()))
                lvHealthRecords.Items.Add(item)
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading health records: " & ex.Message)
        End Try
    End Sub

    Private Sub PrefillLatestHealthRecord()
        Try
            Dim query As String =
                "SELECT * FROM seniorhealthrecords WHERE residentid=" & residentID &
                " ORDER BY recorddate DESC LIMIT 1"

            GetQuery(query, "latest_record")

            If ds.Tables("latest_record").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("latest_record").Rows(0)
                dtpRecordDate.Value = Convert.ToDateTime(row("recorddate"))
                txtIllness.Text = If(IsDBNull(row("illness")), "", row("illness").ToString())

                If Not IsDBNull(row("bloodpressure")) AndAlso row("bloodpressure").ToString().Contains("/") Then
                    Dim parts() As String = row("bloodpressure").ToString().Split("/")
                    nudSystolic.Value = Convert.ToDecimal(parts(0))
                    nudDiastolic.Value = Convert.ToDecimal(parts(1))
                Else
                    nudSystolic.Value = 120
                    nudDiastolic.Value = 80
                End If

                nudTemp.Value = If(IsDBNull(row("temperature")), 36.5D, Convert.ToDecimal(row("temperature")))
            Else
                dtpRecordDate.Value = Date.Today
                txtIllness.Clear()
                nudSystolic.Value = 120
                nudDiastolic.Value = 80
                nudTemp.Value = 36.5D
            End If

            ToggleHealthRecordControls(False)

        Catch ex As Exception
            MessageBox.Show("Error prefilling latest health record: " & ex.Message)
        End Try
    End Sub

    Private Sub lvHealthRecords_DoubleClick(sender As Object, e As EventArgs) Handles lvHealthRecords.DoubleClick
        Try
            If lvHealthRecords.SelectedItems.Count = 0 Then Return
            selectedRecordID = CInt(lvHealthRecords.SelectedItems(0).Text)

            Dim query As String = "SELECT * FROM seniorhealthrecords WHERE seniorrecordid=" & selectedRecordID
            GetQuery(query, "record_detail")

            If ds.Tables("record_detail").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("record_detail").Rows(0)
                dtpRecordDate.Value = Convert.ToDateTime(row("recorddate"))
                txtIllness.Text = If(IsDBNull(row("illness")), "", row("illness").ToString())

                If Not IsDBNull(row("bloodpressure")) AndAlso row("bloodpressure").ToString().Contains("/") Then
                    Dim parts() As String = row("bloodpressure").ToString().Split("/")
                    nudSystolic.Value = Convert.ToDecimal(parts(0))
                    nudDiastolic.Value = Convert.ToDecimal(parts(1))
                End If

                nudTemp.Value = If(IsDBNull(row("temperature")), 36.5D, Convert.ToDecimal(row("temperature")))

                ToggleHealthRecordControls(True)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading selected record: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            selectedRecordID = 0
            dtpRecordDate.Value = Date.Today
            txtIllness.Clear()
            nudSystolic.Value = 120
            nudDiastolic.Value = 80
            nudTemp.Value = 36.5D

            ToggleHealthRecordControls(True)

        Catch ex As Exception
            MessageBox.Show("Error clearing fields: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSaveHealthRecords_Click(sender As Object, e As EventArgs) Handles btnSaveHealthRecords.Click
        Try
            If nudTemp.Value < 35 OrElse nudTemp.Value > 42 Then
                MessageBox.Show("Temperature must be between 35°C and 42°C.", "Invalid Temperature", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If nudSystolic.Value < 90 OrElse nudSystolic.Value > 200 OrElse nudDiastolic.Value < 60 OrElse nudDiastolic.Value > 120 Then
                MessageBox.Show("Please enter valid blood pressure values (90–200 / 60–120 mmHg).", "Invalid BP", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim bpValue As String = nudSystolic.Value.ToString() & "/" & nudDiastolic.Value.ToString()

            If selectedRecordID = 0 Then
                Dim insertQuery As String =
                    "INSERT INTO seniorhealthrecords (residentid, recorddate, illness, bloodpressure, temperature) VALUES (" &
                    residentID & ", '" & dtprecorddate.Value.ToString("MM-dd-yyyy") & "', '" &
                    txtillness.Text.Trim() & "', '" & bpValue & "', '" & nudtemp.Value & "')"

                SetQuery(insertQuery)
                LogAction("seniorhealthrecords", "Add", "Added new health record for: " & txtFullname.Text.Trim())
                MessageBox.Show("New health record added successfully!")

            Else
                Dim updateQuery As String =
                    "UPDATE seniorhealthrecords SET " &
                    "recorddate='" & dtprecorddate.Value.ToString("MM-dd-yyyy") & "', " &
                    "illness='" & txtillness.Text.Trim() & "', " &
                    "bloodpressure='" & bpValue & "', " &
                    "temperature='" & nudtemp.Value & "' " &
                    "WHERE seniorrecordid=" & selectedRecordID

                SetQuery(updateQuery)
                LogAction("seniorhealthrecords", "Update", "Updated health record for: " & txtFullname.Text.Trim())
                MessageBox.Show("Health record updated successfully!")
            End If

            LoadHealthRecords()
            PrefillLatestHealthRecord()

        Catch ex As Exception
            MessageBox.Show("Error saving health record: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Dim viewForm As New ViewSeniorCitizen()
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
