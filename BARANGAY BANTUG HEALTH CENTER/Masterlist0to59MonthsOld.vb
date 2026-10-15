Public Class Masterlist0to59MonthsOld

    Private Sub Masterlist0to59MonthsOld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")
        LoadFilters()
        LoadResidents(txtSearch.Text.Trim())

    End Sub

    Private Sub LoadFilters()
        Try
            cmbAge.Items.Clear()
            cmbAge.Items.Add("AGE")
            cmbAge.Items.Add("0 to 19")
            cmbAge.Items.Add("20 to 39")
            cmbAge.Items.Add("40 to 59")
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
                "TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) AS months_old, " &
                "r.guardianname, r.contact, p.purokname, s.sexname " &
                "FROM residents r " &
                "LEFT JOIN purok p ON r.purokid = p.purokid " &
                "LEFT JOIN sex s ON r.sexid = s.sexid " &
                "WHERE r.status='Alive' " &
                "AND TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) BETWEEN 0 AND 59 "

            Dim filters As New List(Of String)

            If Not String.IsNullOrWhiteSpace(searchQuery) Then
                filters.Add("(r.fullname LIKE '%" & searchQuery & "%' " &
                            "OR r.contact LIKE '%" & searchQuery & "%' " &
                            "OR r.guardianname LIKE '%" & searchQuery & "%')")
            End If

            Select Case cmbAge.SelectedIndex
                Case 1
                    filters.Add("TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) BETWEEN 0 AND 19")
                Case 2
                    filters.Add("TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) BETWEEN 20 AND 39")
                Case 3
                    filters.Add("TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) BETWEEN 40 AND 59")
            End Select

            If rbMale.Checked Then
                filters.Add("s.sexname = 'Male'")
            ElseIf rbFemale.Checked Then
                filters.Add("s.sexname = 'Female'")
            End If

            If cmbPurok.SelectedIndex > 0 Then
                filters.Add("r.purokid = (SELECT purokid FROM purok WHERE purokname='" & cmbPurok.Text & "')")
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
                    .SubItems.Add(row("months_old").ToString & " mos")
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

    Private Sub cmbAge_SelectedIndexChanged(sender As Object, e As EventArgs)
        LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub cmbPurok_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPurok.SelectedIndexChanged, cmbAge.SelectedIndexChanged
        LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub rbMale_CheckedChanged(sender As Object, e As EventArgs) Handles rbMale.CheckedChanged
        If rbMale.Checked Then LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub rbFemale_CheckedChanged(sender As Object, e As EventArgs) Handles rbFemale.CheckedChanged
        If rbFemale.Checked Then LoadResidents(txtsearch.Text.Trim())
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadResidents(txtSearch.Text.Trim())
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

            Dim viewForm As New View0to59MonthsOld()
            viewForm.residentId = selectedId

            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()
            viewForm.TopLevel = False
            viewForm.FormBorderStyle = FormBorderStyle.None
            viewForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(viewForm)
            viewForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error opening record: " & ex.Message)
        End Try
    End Sub

End Class
