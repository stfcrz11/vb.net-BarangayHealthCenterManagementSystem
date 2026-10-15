Public Class SystemLogs

    Private sortDescending As Boolean = True
    Private applyDateFilter As Boolean = False

    Private Sub SystemLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")

        ' Load combo boxes
        LoadUsersToCombo()
        LoadActionsToCombo()

        ' Set date pickers
        dtpStart.Value = DateTime.Now.Date
        dtpEnd.Value = DateTime.Now.Date

        ' Setup ListView columns
        SetupListView()

        ' Load all logs initially
        LoadSystemLogs()

        ' Tooltips
        Dim tt As New ToolTip()
        tt.SetToolTip(btnRefresh, "Refresh the system logs.")
        tt.SetToolTip(btnSort, "Toggle sort order.")
        tt.SetToolTip(btnFilterDate, "Filter logs by date range.")
    End Sub

    Private Sub SetupListView()
        lvSystemLog.Clear()
        lvSystemLog.View = View.Details
        lvSystemLog.FullRowSelect = True
        lvSystemLog.GridLines = True

        lvSystemLog.Columns.Add("Log ID", 70, HorizontalAlignment.Left)
        lvSystemLog.Columns.Add("Log Date", 250, HorizontalAlignment.Left)
        lvSystemLog.Columns.Add("User", 150, HorizontalAlignment.Left)
        lvSystemLog.Columns.Add("Action", 100, HorizontalAlignment.Left)
        lvSystemLog.Columns.Add("Table", 100, HorizontalAlignment.Left)
        lvSystemLog.Columns.Add("Details", 300, HorizontalAlignment.Left)
    End Sub

    Private Sub LoadUsersToCombo()
        cmbUser.Items.Clear()
        cmbUser.Items.Add("All Users")

        GetQuery("SELECT userid, fullname FROM users ORDER BY fullname", "users")

        For Each row As DataRow In ds.Tables("users").Rows
            cmbUser.Items.Add(row("fullname").ToString())
        Next

        cmbUser.SelectedIndex = 0
    End Sub

    Private Sub LoadActionsToCombo()
        cmbAction.Items.Clear()
        cmbAction.Items.Add("All Actions")
        cmbAction.Items.Add("Add")
        cmbAction.Items.Add("Update")
        cmbAction.Items.Add("View")
        cmbAction.Items.Add("Log In")
        cmbAction.Items.Add("Log Out")
        cmbAction.SelectedIndex = 0
    End Sub

    Private Sub LoadSystemLogs()
        Try
            lvSystemLog.Items.Clear()
            Dim sortOrder As String = If(sortDescending, "DESC", "ASC")

            Dim query As String =
                "SELECT s.logid, u.fullname, s.action, s.tablename, s.details, s.logdate " &
                "FROM systemlog s " &
                "LEFT JOIN users u ON s.userid = u.userid WHERE 1=1 "

            ' Apply date filter only if enabled
            If applyDateFilter Then
                query &= " AND DATE(s.logdate) BETWEEN '" &
                         dtpStart.Value.ToString("yyyy-MM-dd") &
                         "' AND '" & dtpEnd.Value.ToString("yyyy-MM-dd") & "'"
            End If

            ' User filter
            If cmbUser.SelectedIndex > 0 Then
                query &= " AND u.fullname = '" & cmbUser.SelectedItem.ToString().Replace("'", "''") & "'"
            End If

            ' Action filter
            If cmbAction.SelectedIndex > 0 Then
                Dim actionFilter As String = cmbAction.SelectedItem.ToString().Trim().ToLower()
                query &= " AND LOWER(TRIM(s.action)) = '" & actionFilter.Replace("'", "''") & "'"
            End If

            ' Sorting
            query &= " ORDER BY s.logdate " & sortOrder

            GetQuery(query, "systemlog")

            ' Populate ListView
            For Each row As DataRow In ds.Tables("systemlog").Rows
                Dim formattedDate As String = Format(CDate(row("logdate")), "MMMM dd, yyyy hh:mm tt")
                Dim item As New ListViewItem(row("logid").ToString())
                item.SubItems.Add(formattedDate)
                item.SubItems.Add(If(IsDBNull(row("fullname")), "N/A", row("fullname").ToString()))
                item.SubItems.Add(row("action").ToString())
                item.SubItems.Add(row("tablename").ToString())
                item.SubItems.Add(If(IsDBNull(row("details")), "", row("details").ToString()))
                lvSystemLog.Items.Add(item)
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading system logs: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSort_Click(sender As Object, e As EventArgs) Handles btnSort.Click
        sortDescending = Not sortDescending
        btnSort.Text = If(sortDescending, "Sort ▼", "Sort ▲")
        LoadSystemLogs()
    End Sub

    Private Sub btnFilterDate_Click(sender As Object, e As EventArgs) Handles btnFilterDate.Click
        If dtpStart.Value > dtpEnd.Value Then
            MessageBox.Show("Start date cannot be later than end date.",
                            "Invalid Date Range",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        applyDateFilter = True
        LoadSystemLogs()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        dtpStart.Value = DateTime.Now.Date
        dtpEnd.Value = DateTime.Now.Date
        cmbUser.SelectedIndex = 0
        cmbAction.SelectedIndex = 0
        sortDescending = True
        btnSort.Text = "Sort ▼"
        applyDateFilter = False
        LoadSystemLogs()
    End Sub

    Private Sub cmbUser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUser.SelectedIndexChanged
        LoadSystemLogs()
    End Sub

    Private Sub cmbAction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAction.SelectedIndexChanged
        LoadSystemLogs()
    End Sub

End Class
