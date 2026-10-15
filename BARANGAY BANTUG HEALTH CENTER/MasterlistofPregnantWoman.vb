Public Class MasterlistofPregnantWoman

    Private Sub MasterlistofPregnantWoman_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")
        LoadPregnantWomen(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadPregnantWomen(Optional ByVal searchQuery As String = "")
        Try
            Dim query As String =
                "SELECT r.residentid, r.fullname, " &
                "TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) AS age, " &
                "p.trimester, p.lastcheckup, p.start_date " &
                "FROM residents r " &
                "LEFT JOIN pregnantdetails p ON r.residentid = p.residentid " &
                "AND p.pregnantid = ( " &
                "    SELECT MAX(p2.pregnantid) FROM pregnantdetails p2 WHERE p2.residentid = r.residentid " &
                ") " &
                "LEFT JOIN classification c ON r.conditionid = c.classid AND c.classname='Pregnant' " &
                "WHERE r.status='Alive' AND r.conditionid IS NOT NULL "

            Dim filters As New List(Of String)

            If Not String.IsNullOrWhiteSpace(searchQuery) Then
                filters.Add("(r.fullname LIKE '%" & searchQuery & "%')")
            End If

            If rbFirst.Checked Then
                filters.Add("p.trimester = '1'")
            ElseIf rbSecond.Checked Then
                filters.Add("p.trimester = '2'")
            ElseIf rbThird.Checked Then
                filters.Add("p.trimester = '3'")
            End If

            If filters.Count > 0 Then
                query &= " AND " & String.Join(" AND ", filters)
            End If

            query &= " ORDER BY r.fullname ASC"

            GetQuery(query, "preg_list")

            lvprofile.Items.Clear()
            For Each row As DataRow In ds.Tables("preg_list").Rows
                With lvprofile.Items.Add(row("residentid").ToString)
                    .SubItems.Add(row("fullname").ToString)
                    .SubItems.Add(row("age").ToString & " yrs")
                    .SubItems.Add(If(IsDBNull(row("trimester")), "", row("trimester").ToString))
                    .SubItems.Add(If(IsDBNull(row("lastcheckup")), "", Format(row("lastcheckup"), "MM-dd-yyyy")))
                    .SubItems.Add(If(IsDBNull(row("start_date")), "", Format(row("start_date"), "MM-dd-yyyy")))
                End With
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading pregnant women list: " & ex.Message)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        LoadPregnantWomen(txtsearch.Text.Trim())
    End Sub

    Private Sub rbFirst_CheckedChanged(sender As Object, e As EventArgs) Handles rbFirst.CheckedChanged
        If rbFirst.Checked Then LoadPregnantWomen(txtSearch.Text.Trim())
    End Sub

    Private Sub rbSecond_CheckedChanged(sender As Object, e As EventArgs) Handles rbSecond.CheckedChanged
        If rbSecond.Checked Then LoadPregnantWomen(txtSearch.Text.Trim())
    End Sub

    Private Sub rbThird_CheckedChanged(sender As Object, e As EventArgs) Handles rbThird.CheckedChanged
        If rbThird.Checked Then LoadPregnantWomen(txtSearch.Text.Trim())
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            txtsearch.Clear()
            rbfirst.Checked = False
            rbsecond.Checked = False
            rbthird.Checked = False
            LoadPregnantWomen()
        Catch ex As Exception
            MessageBox.Show("Error refreshing data: " & ex.Message)
        End Try
    End Sub

    Private Sub lvprofile_DoubleClick(sender As Object, e As EventArgs) Handles lvprofile.DoubleClick
        Try
            Dim selectedId As Integer = CInt(lvprofile.FocusedItem.SubItems(0).Text)

            Dim viewForm As New ViewPregnantWoman()
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

    Private Sub btnRecords_Click(sender As Object, e As EventArgs) Handles btnRecords.Click
        Try
            Dim viewForm As New ViewPastPregnancies()

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
