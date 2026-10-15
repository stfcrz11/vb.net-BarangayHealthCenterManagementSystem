Public Class UpdateRecord0to59MonthsOld

    Public residentId As Integer
    Private selectedVaxId As Integer = 0
    Private selectedMuacId As Integer = 0

    Private Sub UpdateRecord0to59MonthsOld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")
            LoadResidentDetails()
            LoadVaccinationRecords()
            LoadMUACRecords()
            LoadEnumValues()

            dtpdob.MinDate = Date.Today.AddYears(-5).AddDays(1)
            dtpdob.MaxDate = Date.Today

            ToggleVaccinationSection(False)
            ToggleMUACSection(False)

            Dim tt As New ToolTip()
            tt.SetToolTip(txtage, "Age is automatically calculated and cannot be changed.")

        Catch ex As Exception
            MessageBox.Show("Error loading update form: " & ex.Message)
        End Try
    End Sub

    Private Sub ToggleVaccinationSection(state As Boolean)
        dtpdate.Enabled = state
        cbvax.Enabled = state
        cbdewormed.Enabled = state
        cbvitamin.Enabled = state
        btnSaveVax.Enabled = state
    End Sub

    Private Sub ToggleMUACSection(state As Boolean)
        dtpMuacDate.Enabled = state
        nudHeight.Enabled = state
        nudWeight.Enabled = state
        cbmuac.Enabled = state
        btnSaveMS.Enabled = state
    End Sub

    Private Sub LoadResidentDetails()
        Try
            Dim query As String =
                "SELECT r.fullname, r.birthdate, r.guardianname, r.contact, " &
                "p.purokname, s.sexname, r.purokid, r.sexid " &
                "FROM residents r " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "WHERE r.residentid = " & residentId

            GetQuery(query, "resident_tmp")

            If Not ds.Tables.Contains("resident_tmp") OrElse ds.Tables("resident_tmp").Rows.Count = 0 Then
                MessageBox.Show("Resident not found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim dt As DataTable = ds.Tables("resident_tmp").Copy()

            Dim row As DataRow = dt.Rows(0)
            If row.RowState = DataRowState.Deleted Then
                MessageBox.Show("Resident record was removed from the source table.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            txtflname.Text = If(IsDBNull(row("fullname")), "", row("fullname").ToString())
            If Not IsDBNull(row("birthdate")) Then
                dtpDob.Value = Convert.ToDateTime(row("birthdate"))
                Dim birth As Date = Convert.ToDateTime(row("birthdate"))
                Dim monthsOld As Integer = DateDiff(DateInterval.Month, birth, Date.Today)
                txtage.Text = monthsOld.ToString() & " months"
            Else
                dtpDob.Value = Date.Today
                txtage.Text = ""
            End If

            txtguardian.Text = If(IsDBNull(row("guardianname")), "", row("guardianname").ToString())
            txtcontact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString())

            LoadPurokOptions()
            LoadSexOptions()
            cmbPurok.Text = If(IsDBNull(row("purokname")), "", row("purokname").ToString())
            cmbSex.Text = If(IsDBNull(row("sexname")), "", row("sexname").ToString())

        Catch ex As Exception
            MessageBox.Show("Error loading resident details: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPurokOptions()
        GetQuery("SELECT purokname FROM purok ORDER BY purokname ASC", "purok_list")
        cmbPurok.Items.Clear()
        For Each row As DataRow In ds.Tables("purok_list").Rows
            cmbPurok.Items.Add(row("purokname").ToString())
        Next
    End Sub

    Private Sub txtContact_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcontact.KeyPress
        AllowOnlyNumericContact(txtContact, e)
    End Sub

    Private Sub LoadSexOptions()
        GetQuery("SELECT sexname FROM sex ORDER BY sexname ASC", "sex_list")
        cmbSex.Items.Clear()
        For Each row As DataRow In ds.Tables("sex_list").Rows
            cmbSex.Items.Add(row("sexname").ToString())
        Next
    End Sub

    Private Sub LoadEnumValues()
        Try
            cbvax.Items.Clear()
            GetQuery("SHOW COLUMNS FROM vaccinationrecords LIKE 'vaccinetype'", "vax_enum")
            If ds.Tables("vax_enum").Rows.Count > 0 Then
                Dim enumText As String = ds.Tables("vax_enum").Rows(0)("Type").ToString().Replace("enum(", "").Replace(")", "").Replace("'", "")
                For Each v In enumText.Split(","c)
                    cbvax.Items.Add(v.Trim())
                Next
            End If

            cbdewormed.Items.Clear()
            GetQuery("SHOW COLUMNS FROM vaccinationrecords LIKE 'deworming'", "deworm_enum")
            If ds.Tables("deworm_enum").Rows.Count > 0 Then
                Dim enumText As String = ds.Tables("deworm_enum").Rows(0)("Type").ToString().Replace("enum(", "").Replace(")", "").Replace("'", "")
                For Each v In enumText.Split(","c)
                    cbdewormed.Items.Add(v.Trim())
                Next
            End If

            cbvitamin.Items.Clear()
            GetQuery("SHOW COLUMNS FROM vaccinationrecords LIKE 'vitamin'", "vit_enum")
            If ds.Tables("vit_enum").Rows.Count > 0 Then
                Dim enumText As String = ds.Tables("vit_enum").Rows(0)("Type").ToString().Replace("enum(", "").Replace(")", "").Replace("'", "")
                For Each v In enumText.Split(","c)
                    cbvitamin.Items.Add(v.Trim())
                Next
            End If

            cbmuac.Items.Clear()
            GetQuery("SHOW COLUMNS FROM muacstatus LIKE 'muacstatus'", "muac_enum")
            If ds.Tables("muac_enum").Rows.Count > 0 Then
                Dim enumText As String = ds.Tables("muac_enum").Rows(0)("Type").ToString().Replace("enum(", "").Replace(")", "").Replace("'", "")
                For Each v In enumText.Split(","c)
                    cbmuac.Items.Add(v.Trim())
                Next
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading ENUM values: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If txtflname.Text.Trim() = "" Then
                MessageBox.Show("Full name is required.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim purokName As String = cmbPurok.Text.Trim()
            Dim sexName As String = cmbSex.Text.Trim()

            Dim getPurokId = "(SELECT purokid FROM purok WHERE purokname='" & purokName & "' LIMIT 1)"
            Dim getSexId = "(SELECT sexid FROM sex WHERE sexname='" & sexName & "' LIMIT 1)"

            Dim updateQuery As String =
                "UPDATE residents SET fullname='" & txtflname.Text.Trim() & "', " &
                "birthdate='" & dtpdob.Value.ToString("MM-dd-yyyy") & "', " &
                "guardianname='" & txtguardian.Text.Trim() & "', " &
                "contact='" & txtcontact.Text.Trim() & "', " &
                "purokid=" & getPurokId & ", sexid=" & getSexId &
                " WHERE residentid=" & residentId

            SetQuery(updateQuery)
            MessageBox.Show("Child record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LogAction("residents", "Update", "Updated 0-59 months profile: " & txtflname.Text.Trim())

        Catch ex As Exception
            MessageBox.Show("Error updating record: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadVaccinationRecords()
        Try
            Dim query As String =
                "SELECT vaccinationid, recorddate, vaccinetype, deworming, vitamin " &
                "FROM vaccinationrecords WHERE residentid=" & residentId & " ORDER BY recorddate DESC"

            GetQuery(query, "vax_list")
            lvVax.Items.Clear()

            For Each row As DataRow In ds.Tables("vax_list").Rows
                Dim item As New ListViewItem(row("vaccinationid").ToString())
                item.SubItems.Add(Format(row("recorddate"), "MM-dd-yyyy"))
                item.SubItems.Add(If(IsDBNull(row("vaccinetype")), "", row("vaccinetype").ToString()))
                item.SubItems.Add(If(IsDBNull(row("deworming")), "", row("deworming").ToString()))
                item.SubItems.Add(If(IsDBNull(row("vitamin")), "", row("vitamin").ToString()))
                lvVax.Items.Add(item)
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading vaccination records: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAddVax_Click(sender As Object, e As EventArgs) Handles btnAddVax.Click
        selectedVaxId = 0
        dtpdate.Value = Date.Today
        cbvax.SelectedIndex = -1
        cbdewormed.SelectedIndex = -1
        cbvitamin.SelectedIndex = -1
        ToggleVaccinationSection(True)
        LogAction("vaccinationrecords", "Add", "Enabled vaccination add mode for: " & txtflname.Text.Trim())
    End Sub

    Private Sub lvVax_DoubleClick(sender As Object, e As EventArgs) Handles lvVax.DoubleClick
        Try
            If lvVax.SelectedItems.Count = 0 Then Return
            selectedVaxId = CInt(lvVax.SelectedItems(0).Text)

            Dim query As String = "SELECT * FROM vaccinationrecords WHERE vaccinationid=" & selectedVaxId
            GetQuery(query, "vax_detail")

            If ds.Tables("vax_detail").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("vax_detail").Rows(0)
                dtpdate.Value = Convert.ToDateTime(row("recorddate"))
                cbvax.Text = row("vaccinetype").ToString()
                cbdewormed.Text = row("deworming").ToString()
                cbvitamin.Text = row("vitamin").ToString()
                ToggleVaccinationSection(True)
                LogAction("vaccinationrecords", "Edit", "Opened vaccination record for: " & txtflname.Text.Trim())
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading vaccination record: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSaveVax_Click(sender As Object, e As EventArgs) Handles btnSaveVax.Click
        Try
            If cbvax.SelectedIndex = -1 Then
                MessageBox.Show("Please select a vaccine type.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim vaxDate As String = dtpdate.Value.ToString("MM-dd-yyyy")
            Dim vaccine As String = cbvax.Text
            Dim deworm As String = cbdewormed.Text
            Dim vit As String = cbvitamin.Text

            If selectedVaxId = 0 Then
                Dim insertQuery As String =
                    "INSERT INTO vaccinationrecords (residentid, recorddate, vaccinetype, deworming, vitamin) VALUES (" &
                    residentId & ", '" & vaxDate & "', '" & vaccine & "', '" & deworm & "', '" & vit & "')"
                SetQuery(insertQuery)
                LogAction("vaccinationrecords", "Add", "Added vaccination record for: " & txtflname.Text.Trim())
                MessageBox.Show("Vaccination record added successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim updateQuery As String =
                    "UPDATE vaccinationrecords SET recorddate='" & vaxDate & "', vaccinetype='" & vaccine & "', " &
                    "deworming='" & deworm & "', vitamin='" & vit & "' WHERE vaccinationid=" & selectedVaxId
                SetQuery(updateQuery)
                LogAction("vaccinationrecords", "Update", "Updated vaccination record for: " & txtflname.Text.Trim())
                MessageBox.Show("Vaccination record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            LoadVaccinationRecords()
            ToggleVaccinationSection(False)

        Catch ex As Exception
            MessageBox.Show("Error saving vaccination record: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadMUACRecords()
        Try
            Dim query As String =
                "SELECT muacid, recorddate, weight, height, muacstatus FROM muacstatus " &
                "WHERE residentid=" & residentId & " ORDER BY recorddate DESC"

            GetQuery(query, "muac_list")
            lvMuacStatus.Items.Clear()

            For Each row As DataRow In ds.Tables("muac_list").Rows
                Dim item As New ListViewItem(row("muacid").ToString())
                item.SubItems.Add(Format(row("recorddate"), "MM-dd-yyyy"))
                item.SubItems.Add(row("weight").ToString() & " kg")
                item.SubItems.Add(row("height").ToString() & " cm")
                item.SubItems.Add(row("muacstatus").ToString())
                lvMuacStatus.Items.Add(item)
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading MUAC records: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAddMS_Click(sender As Object, e As EventArgs) Handles btnAddMS.Click
        selectedMuacId = 0
        dtpMuacDate.Value = Date.Today
        nudHeight.Value = 45.0
        nudWeight.Value = 2.0
        cbmuac.SelectedIndex = -1
        ToggleMUACSection(True)
        LogAction("muacstatus", "Add", "Enabled MUAC add mode for: " & txtflname.Text.Trim())
    End Sub

    Private Sub lvMuacStatus_DoubleClick(sender As Object, e As EventArgs) Handles lvMuacStatus.DoubleClick
        Try
            If lvMuacStatus.SelectedItems.Count = 0 Then Return
            selectedMuacId = CInt(lvMuacStatus.SelectedItems(0).Text)

            Dim query As String = "SELECT * FROM muacstatus WHERE muacid=" & selectedMuacId
            GetQuery(query, "muac_detail")

            If ds.Tables("muac_detail").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("muac_detail").Rows(0)
                dtpMuacDate.Value = Convert.ToDateTime(row("recorddate"))
                nudHeight.Value = CDec(row("height"))
                nudWeight.Value = CDec(row("weight"))
                cbmuac.Text = row("muacstatus").ToString()
                ToggleMUACSection(True)
                LogAction("muacstatus", "Edit", "Opened MUAC record for: " & txtflname.Text.Trim())
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading MUAC record: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSaveMS_Click(sender As Object, e As EventArgs) Handles btnSaveMS.Click
        Try
            Dim muacDate As String = dtpMuacDate.Value.ToString("MM-dd-yyyy")
            Dim height As Decimal = nudHeight.Value
            Dim weight As Decimal = nudWeight.Value
            Dim muac As String = cbmuac.Text

            If selectedMuacId = 0 Then
                Dim insertQuery As String =
                    "INSERT INTO muacstatus (residentid, recorddate, weight, height, muacstatus) VALUES (" &
                    residentId & ", '" & muacDate & "', " & weight & ", " & height & ", '" & muac & "')"
                SetQuery(insertQuery)
                LogAction("muacstatus", "Add", "Added MUAC record for: " & txtflname.Text.Trim())
                MessageBox.Show("MUAC record added successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim updateQuery As String =
                    "UPDATE muacstatus SET recorddate='" & muacDate & "', weight=" & weight & ", height=" & height & ", " &
                    "muacstatus='" & muac & "' WHERE muacid=" & selectedMuacId
                SetQuery(updateQuery)
                LogAction("muacstatus", "Update", "Updated MUAC record for: " & txtflname.Text.Trim())
                MessageBox.Show("MUAC record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            LoadMUACRecords()
            ToggleMUACSection(False)

        Catch ex As Exception
            MessageBox.Show("Error saving MUAC record: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Dim viewForm As New View0to59MonthsOld()
            viewForm.residentId = residentId

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
