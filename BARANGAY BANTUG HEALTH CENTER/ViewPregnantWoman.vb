Public Class ViewPregnantWoman

    Public residentId As Integer

    Private Sub ViewPregnantWoman_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")

            If residentId > 0 Then
                LoadResidentDetails()
                LoadMedicationRecords()
                LoadVitalSignsRecords()
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading pregnant woman details: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadResidentDetails()
        Try
            Dim query As String =
                "SELECT r.fullname, r.birthdate, r.contact, p.purokname, " &
                "TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) AS age, " &
                "pd.trimester, pd.lastcheckup " &
                "FROM residents r " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "LEFT JOIN pregnantdetails pd ON r.residentid = pd.residentid AND pd.end_date IS NULL " &
                "LEFT JOIN classification c ON r.conditionid = c.classid " &
                "WHERE r.status='Alive' AND c.classname='Pregnant' AND r.residentid = " & residentId

            GetQuery(query, "pregnant_profile")

            If ds.Tables("pregnant_profile").Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables("pregnant_profile").Rows(0)

                lblName.Text = row("fullname").ToString
                lblPurok.Text = If(IsDBNull(row("purokname")), "", row("purokname").ToString)
                lblAge.Text = row("age").ToString & " yrs"
                lblContact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString)

                Dim triText As String = ""
                If Not IsDBNull(row("trimester")) Then
                    Select Case row("trimester").ToString()
                        Case "1" : triText = "First Trimester"
                        Case "2" : triText = "Second Trimester"
                        Case "3" : triText = "Third Trimester"
                    End Select
                End If
                lblTrimester.Text = triText

                lblLastCheckup.Text = If(IsDBNull(row("lastcheckup")), "",
                                         Format(row("lastcheckup"), "yyyy-MM-dd"))
            End If

        Catch ex As Exception
            MessageBox.Show("Error fetching pregnant woman details: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadMedicationRecords()
        Try
            Dim query As String =
                "SELECT m.medicationid, m.medicinename, m.recorddate " &
                "FROM medication m " &
                "INNER JOIN pregnantdetails pd ON m.pregnantid = pd.pregnantid " &
                "WHERE pd.residentid = " & residentId &
                " ORDER BY m.recorddate DESC"

            GetQuery(query, "med_list")

            lvMed.Items.Clear()
            For Each row As DataRow In ds.Tables("med_list").Rows
                Dim item As New ListViewItem(row("medicationid").ToString)
                item.SubItems.Add(If(IsDBNull(row("recorddate")), "", Format(row("recorddate"), "yyyy-MM-dd")))
                item.SubItems.Add(If(IsDBNull(row("medicinename")), "", row("medicinename").ToString))
                lvMed.Items.Add(item)
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading medication records: " & ex.Message)
        End Try
    End Sub


    Private Sub LoadVitalSignsRecords()
        Try
            Dim query As String =
                "SELECT v.vitalid, v.recorddate, v.bloodpressure, v.temperature, v.fetalheartbeat " &
                "FROM vitalsigns v " &
                "LEFT JOIN pregnantdetails pd ON v.pregnantid = pd.pregnantid " &
                "WHERE pd.residentid = " & residentId &
                " ORDER BY v.recorddate DESC"

            GetQuery(query, "vital_list")

            lvvitals.Items.Clear()
            For Each row As DataRow In ds.Tables("vital_list").Rows
                Dim item As New ListViewItem(row("vitalid").ToString)
                item.SubItems.Add(If(IsDBNull(row("recorddate")), "", Format(row("recorddate"), "yyyy-MM-dd")))
                item.SubItems.Add(If(IsDBNull(row("bloodpressure")), "", row("bloodpressure").ToString))
                item.SubItems.Add(If(IsDBNull(row("temperature")), "", row("temperature").ToString))
                item.SubItems.Add(If(IsDBNull(row("fetalheartbeat")), "", row("fetalheartbeat").ToString))
                lvvitals.Items.Add(item)
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading vital signs records: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Dim masterForm As New MasterlistofPregnantWoman()
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
            Dim updateForm As New UpdateRecordPregnantWoman()
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
