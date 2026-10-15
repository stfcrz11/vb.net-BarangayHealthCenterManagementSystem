Public Class MasterlistSeniorCitizens

    Private Sub MasterlistSeniorCitizenProfile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")
        LoadFilters()
        LoadResidents(txtsearch.Text.Trim())

    End Sub

    Private Sub LoadFilters()
        Try
            cmbAge.Items.Clear()
            cmbAge.Items.Add("All")
            cmbAge.Items.Add("60 to 69")
            cmbAge.Items.Add("70 to 79")
            cmbAge.Items.Add("80 to 89")
            cmbAge.Items.Add("90 to 99")
            cmbAge.Items.Add("100 and above")
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
                "r.contact, p.purokname, s.sexname " &
                "FROM residents r " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "LEFT JOIN classification c ON r.classid = c.classid " &
                "WHERE r.status='Alive' " &
                "AND c.classname='Senior' " &
                "AND TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) >= 60 "

            Dim filters As New List(Of String)

            If Not String.IsNullOrWhiteSpace(searchQuery) Then
                filters.Add("(r.fullname LIKE '%" & searchQuery & "%' " &
                            "OR r.contact LIKE '%" & searchQuery & "%' " &
                            "OR p.purokname LIKE '%" & searchQuery & "%')")
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

            GetQuery(query, "senior_list")

            lvprofile.Items.Clear()
            For i As Integer = 0 To ds.Tables("senior_list").Rows.Count - 1
                With ds.Tables("senior_list").Rows(i)
                    Dim item As New ListViewItem(.Item("residentid").ToString)
                    item.SubItems.Add(.Item("fullname").ToString)
                    item.SubItems.Add(Format(.Item("birthdate"), "MM-dd-yyyy"))
                    item.SubItems.Add(.Item("years_old").ToString & " yrs")
                    item.SubItems.Add(If(IsDBNull(.Item("purokname")), "", .Item("purokname").ToString))
                    item.SubItems.Add(If(IsDBNull(.Item("sexname")), "", .Item("sexname").ToString))
                    item.SubItems.Add(If(IsDBNull(.Item("contact")), "", .Item("contact").ToString))
                    lvprofile.Items.Add(item)
                End With
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading senior citizen list: " & ex.Message)
        End Try
    End Sub

    Private Sub cmbAge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAge.SelectedIndexChanged
        LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub cmbPurok_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPurok.SelectedIndexChanged
        LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        LoadResidents(txtSearch.Text.Trim())
    End Sub

    Private Sub rbMale_CheckedChanged(sender As Object, e As EventArgs) Handles rbMale.CheckedChanged
        If rbMale.Checked Then LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub rbFemale_CheckedChanged(sender As Object, e As EventArgs) Handles rbFemale.CheckedChanged
        If rbFemale.Checked Then LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            txtSearch.Clear()
            LoadFilters()
            LoadResidents()
        Catch ex As Exception
            MessageBox.Show("Error refreshing data: " & ex.Message)
        End Try
    End Sub

    Private Sub lvprofile_DoubleClick(sender As Object, e As EventArgs) Handles lvprofile.DoubleClick
        Try
            Dim selectedId As Integer = CInt(lvprofile.FocusedItem.SubItems(0).Text)
            Dim viewForm As New ViewSeniorCitizen()
            viewForm.residentId = selectedId

            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()
            viewForm.TopLevel = False
            viewForm.FormBorderStyle = FormBorderStyle.None
            viewForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(viewForm)
            viewForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error opening health record: " & ex.Message)
        End Try
    End Sub


End Class
