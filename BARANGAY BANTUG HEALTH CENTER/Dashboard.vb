Public Class Dashboard

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFilters()
        UpdateTotals()
        LoadResidentsListView()
    End Sub

    Private Sub LoadFilters()
        Try
            cmbAge.Items.Clear()
            cmbAge.Items.Add("Age")
            cmbAge.Items.Add("0 to 59 months")
            cmbAge.Items.Add("1 to 19 years")
            cmbAge.Items.Add("Adult (20–59 years)")
            cmbAge.Items.Add("Senior Citizens")
            cmbAge.SelectedIndex = 0

            LoadPurokFilter()

            rbMale.Checked = False
            rbFemale.Checked = False

        Catch ex As Exception
            MsgBox("Error loading filters: " & ex.Message, MsgBoxStyle.Critical)
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

    Private Sub UpdateTotals()
        Try
            GetQuery("SELECT COUNT(*) AS total FROM residents WHERE TIMESTAMPDIFF(MONTH, birthdate, CURDATE()) BETWEEN 0 AND 59", "total0to59")
            lblTotal0to59.Text = If(ds.Tables("total0to59").Rows.Count > 0, ds.Tables("total0to59").Rows(0)("total").ToString(), "0")

            GetQuery("SELECT COUNT(*) AS total FROM residents WHERE age BETWEEN 1 AND 19", "total1to19")
            lblTotal1to19.Text = If(ds.Tables("total1to19").Rows.Count > 0, ds.Tables("total1to19").Rows(0)("total").ToString(), "0")

            GetQuery("SELECT COUNT(*) AS total FROM residents WHERE age BETWEEN 20 AND 59", "totalAdults")
            lblTotalAdult.Text = If(ds.Tables("totalAdults").Rows.Count > 0, ds.Tables("totalAdults").Rows(0)("total").ToString(), "0")

            GetQuery("SELECT COUNT(*) AS total FROM residents WHERE age >= 60", "totalSenior")
            lblTotalSeniorCitizens.Text = If(ds.Tables("totalSenior").Rows.Count > 0, ds.Tables("totalSenior").Rows(0)("total").ToString(), "0")

            GetQuery("SELECT COUNT(*) AS total " &
                      "FROM pregnantdetails p " &
                      "INNER JOIN residents r ON r.residentid = p.residentid " &
                      "WHERE p.end_date IS NULL;", "totalPregnant")
            lblTotalPregnantWomen.Text = If(ds.Tables("totalPregnant").Rows.Count > 0, ds.Tables("totalPregnant").Rows(0)("total").ToString(), "0")

        Catch ex As Exception
            MsgBox("Error calculating totals: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub LoadResidentsListView(Optional ByVal searchQuery As String = "")
        Try
            Dim query As String = "SELECT r.residentid, r.fullname, r.birthdate, r.age, s.sexname, p.purokname, r.contact, r.guardianname, c.classname, r.status " &
                                  "FROM residents r " &
                                  "LEFT JOIN sex s ON r.sexid = s.sexid " &
                                  "LEFT JOIN purok p ON r.purokid = p.purokid " &
                                  "LEFT JOIN classification c ON r.classid = c.classid " &
                                  "WHERE 1=1 "

            If Not String.IsNullOrWhiteSpace(searchQuery) Then
                query &= "AND (r.fullname LIKE '%" & searchQuery & "%' OR r.contact LIKE '%" & searchQuery & "%' OR r.guardianname LIKE '%" & searchQuery & "%') "
            End If

            If cmbAge.SelectedItem IsNot Nothing AndAlso cmbAge.SelectedItem.ToString() <> "Age" Then
                Select Case cmbAge.SelectedItem.ToString()
                    Case "0 to 59 months"
                        query &= "AND TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) BETWEEN 0 AND 59 "
                    Case "1 to 19 years"
                        query &= "AND r.age BETWEEN 1 AND 19 "
                    Case "Adult (20–59 years)"
                        query &= "AND r.age BETWEEN 20 AND 59 "
                    Case "Senior Citizens"
                        query &= "AND r.age >= 60 "
                End Select
            End If

            If rbMale.Checked Then
                query &= "AND s.sexname = 'Male' "
            ElseIf rbFemale.Checked Then
                query &= "AND s.sexname = 'Female' "
            End If

            If cmbPurok.SelectedItem IsNot Nothing AndAlso cmbPurok.SelectedIndex <> 0 Then
                Dim purokId As Integer = CallByName(cmbPurok.SelectedItem, "Value", CallType.Get)
                query &= "AND r.purokid = " & purokId & " "
            End If

            query &= "ORDER BY p.purokid, r.fullname"

            GetQuery(query, "residentsList")

            lvResidents.Items.Clear()
            For Each row As DataRow In ds.Tables("residentsList").Rows
                Dim item As New ListViewItem(row("residentid").ToString())
                item.SubItems.Add(If(row("purokname") Is DBNull.Value, "", row("purokname").ToString()))
                item.SubItems.Add(row("fullname").ToString())
                item.SubItems.Add(Convert.ToDateTime(row("birthdate")).ToString("MM-dd-yyyy"))
                item.SubItems.Add(row("age").ToString())
                item.SubItems.Add(If(row("sexname") Is DBNull.Value, "", row("sexname").ToString()))
                item.SubItems.Add(If(row("contact") Is DBNull.Value, "", row("contact").ToString()))
                item.SubItems.Add(If(row("guardianname") Is DBNull.Value, "", row("guardianname").ToString()))
                item.SubItems.Add(If(row("classname") Is DBNull.Value, "", row("classname").ToString()))
                item.SubItems.Add(If(row("status") Is DBNull.Value, "", row("status").ToString()))
                lvResidents.Items.Add(item)
            Next

        Catch ex As Exception
            MsgBox("Error loading residents: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub lblAdd_Click(sender As Object, e As EventArgs) Handles lblAdd.Click
        Using itemsForm As New NewResident()
            itemsForm.ShowDialog()
        End Using
    End Sub

    Private Sub FilterChanged(sender As Object, e As EventArgs) _
        Handles cmbAge.SelectedIndexChanged, cmbPurok.SelectedIndexChanged, txtsearch.TextChanged, rbMale.CheckedChanged, rbFemale.CheckedChanged
        LoadResidentsListView(txtsearch.Text.Trim())
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadFilters()
        UpdateTotals()
        LoadResidentsListView(txtsearch.Text.Trim())
    End Sub

End Class
