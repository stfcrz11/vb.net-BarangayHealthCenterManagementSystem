Public Class View1to19YearsOld

    Public residentId As Integer

    Private Sub View1to19YearsOld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Connect("localhost", "bhcims", "3306", "root", "")

            If residentId > 0 Then
                LoadResidentDetails()
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading form: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadResidentDetails()
        Try
            Dim query As String =
                "SELECT r.fullname, r.birthdate, r.guardianname, r.contact, " &
                "p.purokname, s.sexname, TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) AS age " &
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
                lblGuardian.Text = If(IsDBNull(row("guardianname")), "", row("guardianname").ToString)
                lblContact.Text = If(IsDBNull(row("contact")), "", row("contact").ToString)
            End If

        Catch ex As Exception
            MessageBox.Show("Error fetching resident information: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Dim masterForm As New Masterlist1to19YearsOld()

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
            Dim updateForm As New UpdateRecord1to19YearsOld()
            updateForm.residentID = residentId

            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()
            updateForm.TopLevel = False
            updateForm.FormBorderStyle = FormBorderStyle.None
            updateForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(updateForm)
            updateForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error opening edit screen: " & ex.Message)
        End Try
    End Sub

End Class
