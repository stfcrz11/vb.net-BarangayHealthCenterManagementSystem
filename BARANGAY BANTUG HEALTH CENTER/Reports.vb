Imports Microsoft.Reporting.WinForms
Imports System.IO

Public Class Reports

    Private Sub ReportsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ds = New DataSet()

        dtResidents = Nothing
        dtPregnantMasterlist = Nothing
        dtPregnantDetails = Nothing
        dtMUAC = Nothing
        dtSenior = Nothing

        Connect("localhost", "bhcims", "3306", "root", "")
        dtpgenerate.Value = Date.Today
        dtpgenerate.Enabled = False

        cbReportType.Items.AddRange(New String() {
            "Masterlist of Residents",
            "Masterlist of Pregnant Women",
            "Pregnant Women Details",
            "MUAC Status Report",
            "Senior Citizens Health Report"
        })
        cbReportType.SelectedIndex = 0

        lblReport.Text = cbReportType.SelectedItem.ToString()

        LoadReport(lblReport.Text)
    End Sub

    Private Sub cbReportType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbReportType.SelectedIndexChanged
        lblReport.Text = cbReportType.SelectedItem.ToString()
        LoadReport(lblReport.Text)
    End Sub

    Private Sub LoadReport(reportName As String)
        ReportViewer1.Reset()
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.ReleaseSandboxAppDomain()

        Dim dt As DataTable = Nothing
        Dim reportPath As String = ""

        Select Case reportName
            Case "Masterlist of Residents"
                reportPath = "Reports\ResidentsReport.rdlc"
                If dtResidents Is Nothing Then dtResidents = GetResidentsData()
                dt = dtResidents

            Case "Masterlist of Pregnant Women"
                reportPath = "Reports\PregnantMasterlist.rdlc"
                If dtPregnantMasterlist Is Nothing Then dtPregnantMasterlist = GetPregnantMasterlistData()
                dt = dtPregnantMasterlist

            Case "Pregnant Women Details"
                reportPath = "Reports\PregnantDetailsReport.rdlc"
                If dtPregnantDetails Is Nothing Then dtPregnantDetails = GetPregnantDetailsData()
                dt = dtPregnantDetails

            Case "MUAC Status Report"
                reportPath = "Reports\MUACReport.rdlc"
                If dtMUAC Is Nothing Then dtMUAC = GetMUACData()
                dt = dtMUAC

            Case "Senior Citizens Health Report"
                reportPath = "Reports\SeniorReport.rdlc"
                If dtSenior Is Nothing Then dtSenior = GetSeniorData()
                dt = dtSenior
        End Select

        ' Add DataSource
        Dim rds As New ReportDataSource("DataSet1", dt)
        ReportViewer1.LocalReport.DataSources.Add(rds)
        ReportViewer1.LocalReport.ReportPath = reportPath

        ' Refresh the report
        ReportViewer1.RefreshReport()

        ' LOG THE ACTION
        LogAction("reports", "View", reportName & " report generated")

    End Sub

    ' -------------------- Data Retrieval --------------------
    Private Function GetResidentsData() As DataTable
        Dim query As String =
            "SELECT r.residentid, r.fullname, s.sexname, r.age, " &
            "r.birthdate AS birthdate, p.purokname, c.classname, r.status " &
            "FROM residents r " &
            "LEFT JOIN sex s ON r.sexid = s.sexid " &
            "LEFT JOIN purok p ON r.purokid = p.purokid " &
            "LEFT JOIN classification c ON r.classid = c.classid " &
            "ORDER BY r.fullname ASC"

        GetQuery(query, "residents")
        Dim dt = ds.Tables("residents")

        If Not dt.Columns.Contains("birthdateFormatted") Then
            dt.Columns.Add("birthdateFormatted", GetType(String))
        End If

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("birthdate")) Then
                row("birthdateFormatted") =
                    Convert.ToDateTime(row("birthdate")).ToString("MM-dd-yyyy")
            End If
        Next

        Return dt
    End Function


    Private Function GetPregnantMasterlistData() As DataTable
        Dim query As String =
            "SELECT r.residentid, r.fullname, r.age, p.purokname, pd.trimester, pd.lastcheckup " &
            "FROM pregnantdetails pd " &
            "INNER JOIN residents r ON pd.residentid = r.residentid " &
            "LEFT JOIN purok p ON r.purokid = p.purokid"

        GetQuery(query, "pregnant")
        Dim dt = ds.Tables("pregnant")

        If Not dt.Columns.Contains("lastcheckupFormatted") Then
            dt.Columns.Add("lastcheckupFormatted", GetType(String))
        End If

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("lastcheckup")) Then
                row("lastcheckupFormatted") =
                    Convert.ToDateTime(row("lastcheckup")).ToString("MM-dd-yyyy")
            End If
        Next

        Return dt
    End Function


    Private Function GetPregnantDetailsData() As DataTable
        Dim query As String =
            "SELECT pd.pregnantid, r.fullname, pd.trimester, pd.start_date, pd.end_date, pd.lastcheckup " &
            "FROM pregnantdetails pd " &
            "INNER JOIN residents r ON pd.residentid = r.residentid"

        GetQuery(query, "pregdetails")
        Dim dt = ds.Tables("pregdetails")

        Dim cols = {"start_date", "end_date", "lastcheckup"}

        For Each c In cols
            If Not dt.Columns.Contains(c & "Formatted") Then
                dt.Columns.Add(c & "Formatted", GetType(String))
            End If
        Next

        For Each row As DataRow In dt.Rows
            For Each c In cols
                If Not IsDBNull(row(c)) Then
                    row(c & "Formatted") =
                        Convert.ToDateTime(row(c)).ToString("MM-dd-yyyy")
                End If
            Next
        Next

        Return dt
    End Function


    Private Function GetMUACData() As DataTable
        Dim query As String =
            "SELECT m.muacid, r.fullname, s.sexname, " &
            "TIMESTAMPDIFF(MONTH, r.birthdate, CURDATE()) AS age_in_months, " &
            "m.weight, m.height, m.muacstatus, m.recorddate " &
            "FROM muacstatus m " &
            "INNER JOIN residents r ON m.residentid = r.residentid " &
            "LEFT JOIN sex s ON r.sexid = s.sexid"

        GetQuery(query, "muac")
        Dim dt = ds.Tables("muac")

        If Not dt.Columns.Contains("recorddateFormatted") Then
            dt.Columns.Add("recorddateFormatted", GetType(String))
        End If

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("recorddate")) Then
                row("recorddateFormatted") =
                    Convert.ToDateTime(row("recorddate")).ToString("MM-dd-yyyy")
            End If
        Next

        Return dt
    End Function

    Private Function GetSeniorData() As DataTable
        Dim query As String =
            "SELECT s.seniorrecordid, r.fullname, p.purokname, sx.sexname, r.age, s.illness, s.bloodpressure, s.temperature, s.recorddate " &
            "FROM seniorhealthrecords s " &
            "INNER JOIN residents r ON s.residentid = r.residentid " &
            "LEFT JOIN sex sx ON r.sexid = sx.sexid " &
            "LEFT JOIN purok p ON r.purokid = p.purokid"

        GetQuery(query, "senior")
        Dim dt = ds.Tables("senior")

        If Not dt.Columns.Contains("recorddateFormatted") Then
            dt.Columns.Add("recorddateFormatted", GetType(String))
        End If

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("recorddate")) Then
                row("recorddateFormatted") =
                    Convert.ToDateTime(row("recorddate")).ToString("MM-dd-yyyy")
            End If
        Next

        Return dt
    End Function

End Class
