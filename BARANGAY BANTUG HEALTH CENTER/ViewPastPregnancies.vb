Public Class ViewPastPregnancies

    Public residentId As Integer = 0 ' Optional: set if you want to filter by a specific resident

    Private Sub ViewPastPregnancies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Connect to database
        Connect("localhost", "bhcims", "3306", "root", "")

        ' Load data
        LoadPastPregnancies(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadPastPregnancies(Optional ByVal searchQuery As String = "")
        Try
            Dim query As String =
                "SELECT p.pregnantid, r.fullname, TIMESTAMPDIFF(YEAR, r.birthdate, CURDATE()) AS age, " &
                "p.trimester, p.start_date, p.end_date, p.lastcheckup " &
                "FROM pregnantdetails p " &
                "INNER JOIN residents r ON p.residentid = r.residentid " &
                "WHERE p.end_date IS NOT NULL "

            ' Filter by residentId if provided
            If residentId > 0 Then
                query &= " AND p.residentid = " & residentId
            End If

            ' Search filter
            If Not String.IsNullOrWhiteSpace(searchQuery) Then
                query &= " AND r.fullname LIKE '%" & searchQuery.Replace("'", "''") & "%'"
            End If

            ' Sort by start_date descending
            query &= " ORDER BY p.start_date DESC"

            ' Debug: Print the query (optional)
            ' Debug.WriteLine(query)

            ' Execute query
            GetQuery(query, "past_preg_list")

            ' Populate ListView
            lvProfile.Items.Clear()
            For Each row As DataRow In ds.Tables("past_preg_list").Rows
                With lvProfile.Items.Add(row("pregnantid").ToString)
                    .SubItems.Add(row("fullname").ToString)
                    .SubItems.Add(row("age").ToString & " yrs")
                    .SubItems.Add(If(IsDBNull(row("trimester")), "", row("trimester").ToString))
                    .SubItems.Add(If(IsDBNull(row("start_date")), "", Format(row("start_date"), "MM-dd-yyyy")))
                    .SubItems.Add(If(IsDBNull(row("end_date")), "", Format(row("end_date"), "MM-dd-yyyy")))
                End With
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading past pregnancies: " & ex.Message)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadPastPregnancies(txtSearch.Text.Trim())
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Clear()
        LoadPastPregnancies()
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Dim mainForm As TheMainForm = CType(Application.OpenForms("TheMainForm"), TheMainForm)
            mainForm.PanelContainer.Controls.Clear()

            Dim masterForm As New MasterlistofPregnantWoman()
            masterForm.TopLevel = False
            masterForm.FormBorderStyle = FormBorderStyle.None
            masterForm.Dock = DockStyle.Fill
            mainForm.PanelContainer.Controls.Add(masterForm)
            masterForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error going back: " & ex.Message)
        End Try
    End Sub

End Class
