Public Class Masterlist1to19YearsOld

    Private Sub Masterlist1to19YearsOld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")
        LoadFilters()
        LoadResidents(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadFilters()
        Try
            cmbAge.Items.Clear()
            cmbAge.Items.Add("AGE")
            cmbAge.Items.Add("1 to 5")
            cmbAge.Items.Add("6 to 12")
            cmbAge.Items.Add("13 to 19")
            cmbAge.SelectedIndex = 0

            LoadPurokFilter()

        Catch ex As Exception
            MessageBox.Show("Filter loading error: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPurokFilter()
        Try
            GetQuery("SELECT purokid FROM purok ORDER BY purokid ASC", "purokList")

            cmbPurok.Items.Clear()
            cmbPurok.Items.Add("PUROK")

            For Each row As DataRow In ds.Tables("purokList").Rows
                Dim purokId As Integer = Convert.ToInt32(row("purokid"))
                cmbPurok.Items.Add(New With {.Text = "Purok " & purokId, .Value = purokId})
            Next

            cmbPurok.DisplayMember = "Text"
            cmbPurok.ValueMember = "Value"
            cmbPurok.SelectedIndex = 0

        Catch ex As Exception
            MsgBox("Error loading Purok filter: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub LoadResidents(Optional ByVal searchQuery As String = "")
        Try
            Dim query As String =
                "SELECT r.residentid, r.fullname, r.birthdate, " &
                "TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) AS years_old, " &
                "r.guardianname, r.contact, p.purokname, s.sexname " &
                "FROM residents r " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "WHERE r.status='Alive' " &
                "AND TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) BETWEEN 1 AND 19 "

            Dim filters As New List(Of String)

            If Not String.IsNullOrWhiteSpace(searchQuery) Then
                filters.Add("(r.fullname LIKE '%" & searchQuery & "%' " &
                            "OR r.contact LIKE '%" & searchQuery & "%' " &
                            "OR r.guardianname LIKE '%" & searchQuery & "%')")
            End If

            If cmbAge.SelectedIndex > 0 Then
                filters.Add("TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) = " & cmbAge.Text)
            End If

            If cmbPurok.SelectedIndex > 0 Then
                filters.Add("r.purokid = (SELECT purokid FROM purok WHERE purokname='" & cmbPurok.Text & "')")
            End If

            If rbMale.Checked Then
                filters.Add("s.sexname = 'Male'")
            ElseIf rbFemale.Checked Then
                filters.Add("s.sexname = 'Female'")
            End If

            If filters.Count > 0 Then
                query &= " AND " & String.Join(" AND ", filters)
            End If

            query &= " ORDER BY r.fullname ASC"

            GetQuery(query, "res_list")

            lvprofile.Items.Clear()
            For Each row As DataRow In ds.Tables("res_list").Rows
                With lvprofile.Items.Add(row("residentid").ToString)
                    .SubItems.Add(row("fullname").ToString)
                    .SubItems.Add(Format(row("birthdate"), "MM-dd-yyyy"))
                    .SubItems.Add(row("years_old").ToString & " yrs")
                    .SubItems.Add(If(IsDBNull(row("purokname")), "", row("purokname").ToString))
                    .SubItems.Add(If(IsDBNull(row("sexname")), "", row("sexname").ToString))
                    .SubItems.Add(If(IsDBNull(row("contact")), "", row("contact").ToString))
                    .SubItems.Add(If(IsDBNull(row("guardianname")), "", row("guardianname").ToString))
                End With
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading resident list: " & ex.Message)
        End Try
    End Sub

    Private Sub cmbAge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAge.SelectedIndexChanged
        LoadResidents(txtSearch.Text.Trim())
    End Sub

    Private Sub cmbPurok_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPurok.SelectedIndexChanged
        LoadResidents(txtSearch.Text.Trim())
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadResidents(txtSearch.Text.Trim())
    End Sub

    Private Sub rbMale_CheckedChanged(sender As Object, e As EventArgs) Handles rbMale.CheckedChanged
        If rbMale.Checked Then LoadResidents(txtSearch.Text.Trim())
    End Sub

    Private Sub rbFemale_CheckedChanged(sender As Object, e As EventArgs) Handles rbFemale.CheckedChanged
        If rbFemale.Checked Then LoadResidents(txtSearch.Text.Trim())
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        Try
            txtsearch.Clear()
            LoadFilters()
            LoadResidents()
        Catch ex As Exception
            MessageBox.Show("Error refreshing data: " & ex.Message)
        End Try
    End Sub

    Private Sub lvprofile_DoubleClick(sender As Object, e As EventArgs) Handles lvprofile.DoubleClick
        Try
            Dim selectedId As Integer = CInt(lvprofile.FocusedItem.SubItems(0).Text)
            Dim birthdate As Date = CDate(lvprofile.FocusedItem.SubItems(2).Text) ' Get birthdate from ListView
            Dim ageInMonths As Integer = GetAgeInMonths(birthdate)

            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()

            ' Open 0-59 months form if applicable
            If ageInMonths <= 59 Then
                Dim babyForm As New View0to59MonthsOld()
                babyForm.residentId = selectedId
                babyForm.TopLevel = False
                babyForm.FormBorderStyle = FormBorderStyle.None
                babyForm.Dock = DockStyle.Fill
                mainForm.PanelContainer.Controls.Add(babyForm)
                babyForm.Show()
            Else
                Dim viewForm As New View1to19YearsOld()
                viewForm.residentId = selectedId
                viewForm.TopLevel = False
                viewForm.FormBorderStyle = FormBorderStyle.None
                viewForm.Dock = DockStyle.Fill
                mainForm.PanelContainer.Controls.Add(viewForm)
                viewForm.Show()
            End If

        Catch ex As Exception
            MessageBox.Show("Error opening record: " & ex.Message)
        End Try
    End Sub

    Private Function GetAgeInMonths(birthdate As Date) As Integer
        Dim today As Date = Date.Today
        Dim months As Integer = (today.Year - birthdate.Year) * 12 + today.Month - birthdate.Month
        If today.Day < birthdate.Day Then months -= 1
        Return months
    End Function


End Class
