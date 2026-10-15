Public Class View0to59MonthsOld

    Public residentId As Integer

    Private Sub View0to59MonthsOld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")

        If residentId > 0 Then
            LoadResidentDetails()
            LoadMUACRecords()
            LoadVaccinationRecords()
        End If
    End Sub

    Private Sub LoadResidentDetails()
        Try
            Dim query As String =
                "SELECT r.fullname, r.birthdate, r.guardianname, r.contact, " &
                "p.purokname, s.sexname, TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) AS months_old " &
                "FROM residents r " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "WHERE r.residentid = " & residentId

            GetQuery(query, "resident")

            If ds.Tables("resident").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("resident").Rows(0)
                lblName.Text = row("fullname").ToString
                lblPurok.Text = If(IsDBNull(row("purokname")), "", row("purokname").ToString)
                lblAge.Text = row("months_old").ToString & " mos"
                lblDob.Text = Format(row("birthdate"), "yyyy-MM-dd")
                lblSex.Text = If(IsDBNull(row("sexname")), "", row("sexname").ToString)
                lblGuardian.Text = If(IsDBNull(row("guardianname")), "", row("guardianname").ToString)
                lblContact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading resident details: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadMUACRecords()
        Try
            Dim query As String =
                "SELECT residentid, recorddate, weight, height, muacstatus " &
                "FROM muacstatus " &
                "WHERE residentid = " & residentId & " " &
                "ORDER BY recorddate DESC"

            GetQuery(query, "muac")

            lvMuac.Items.Clear()

            For Each row As DataRow In ds.Tables("muac").Rows
                With lvMuac.Items.Add(row("residentid").ToString())
                    .SubItems.Add(If(IsDBNull(row("recorddate")), "", Format(row("recorddate"), "yyyy-MM-dd")))
                    .SubItems.Add(If(IsDBNull(row("weight")), "", row("weight").ToString() & " kg"))
                    .SubItems.Add(If(IsDBNull(row("height")), "", row("height").ToString() & " cm"))
                    .SubItems.Add(If(IsDBNull(row("muacstatus")), "", row("muacstatus").ToString()))
                End With
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading MUAC records: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadVaccinationRecords()
        Try
            Dim query As String =
                "SELECT residentid, recorddate, vaccinetype, deworming, vitamin " &
                "FROM vaccinationrecords " &
                "WHERE residentid = " & residentId & " " &
                "ORDER BY recorddate DESC"

            GetQuery(query, "vax")

            lvVax.Items.Clear()

            For Each row As DataRow In ds.Tables("vax").Rows
                With lvVax.Items.Add(row("residentid").ToString())
                    .SubItems.Add(If(IsDBNull(row("recorddate")), "", Format(row("recorddate"), "yyyy-MM-dd")))
                    .SubItems.Add(If(IsDBNull(row("vaccinetype")), "", row("vaccinetype").ToString()))
                    .SubItems.Add(If(IsDBNull(row("deworming")), "", row("deworming").ToString()))
                    .SubItems.Add(If(IsDBNull(row("vitamin")), "", row("vitamin").ToString()))
                End With
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading vaccination records: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
         Try
            Dim masterForm As New Masterlist0to59MonthsOld()

            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()
            masterForm.TopLevel = False
            masterForm.FormBorderStyle = FormBorderStyle.None
            masterForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(masterForm)
            masterForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error returning: " & ex.Message)
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            Dim updateForm As New UpdateRecord0to59MonthsOld()
            updateForm.residentId = residentId

            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()
            updateForm.TopLevel = False
            updateForm.FormBorderStyle = FormBorderStyle.None
            updateForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(updateForm)
            updateForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error opening edit form: " & ex.Message)
        End Try
    End Sub

End Class
