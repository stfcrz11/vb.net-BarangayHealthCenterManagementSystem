Public Class ViewSeniorCitizen

    Public residentId As Integer

    Private Sub ViewSeniorCitizen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")

            If residentId > 0 Then
                LoadResidentDetails()
                LoadHealthRecords()
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading form: " & ex.Message)
        End Try
    End Sub


    Private Sub LoadResidentDetails()
        Try
            Dim query As String =
                "SELECT r.fullname, r.birthdate, r.contact, p.purokname, s.sexname, " &
                "TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) AS age " &
                "FROM residents r " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "WHERE r.residentid = " & residentId

            GetQuery(query, "resident")

            If ds.Tables("resident").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("resident").Rows(0)
                lblName.Text = row("fullname").ToString
                lblPurok.Text = If(IsDBNull(row("purokname")), "", row("purokname").ToString)
                lblAge.Text = row("age").ToString & " yrs"
                lblDob.Text = Format(row("birthdate"), "yyyy-MM-dd")
                lblSex.Text = If(IsDBNull(row("sexname")), "", row("sexname").ToString)
                lblContact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString)
            End If

        Catch ex As Exception
            MessageBox.Show("Error fetching resident information: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadHealthRecords()
        Try
            Dim query As String =
                "SELECT sr.seniorrecordid, sr.temperature, sr.bloodpressure, sr.illness, sr.recorddate " &
                "FROM seniorhealthrecords sr " &
                "WHERE sr.residentid = " & residentId &
                " ORDER BY sr.recorddate DESC"

            GetQuery(query, "senior_health_records")

            lvHealthRecords.Items.Clear()
            For i As Integer = 0 To ds.Tables("senior_health_records").Rows.Count - 1
                With ds.Tables("senior_health_records").Rows(i)
                    Dim item As New ListViewItem(.Item("seniorrecordid").ToString)
                    item.SubItems.Add(Format(.Item("recorddate"), "yyyy-MM-dd"))
                    item.SubItems.Add(If(IsDBNull(.Item("illness")), "", .Item("illness").ToString))
                    item.SubItems.Add(If(IsDBNull(.Item("bloodpressure")), "", .Item("bloodpressure").ToString))
                    item.SubItems.Add(If(IsDBNull(.Item("temperature")), "", .Item("temperature").ToString & " °C"))
                    lvHealthRecords.Items.Add(item)
                End With
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading health records: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Dim masterForm As New MasterlistSeniorCitizens()

            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()
            masterForm.TopLevel = False
            masterForm.FormBorderStyle = FormBorderStyle.None
            masterForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(masterForm)
            masterForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error returning to masterlist: " & ex.Message)
        End Try
    End Sub


    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            Dim updateForm As New UpdateRecordSeniorCitizen()
            updateForm.residentID = residentId

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
