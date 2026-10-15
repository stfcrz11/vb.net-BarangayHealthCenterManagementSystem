Public Class TheMainForm

    Public CurrentUserRole As String

    Private Sub LoadFormInPanel(ByVal frm As Form)
        PanelContainer.Controls.Clear()
        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill
        PanelContainer.Controls.Add(frm)
        frm.Show()
    End Sub

    Private Sub SetActiveControl(activeControl As Control)
        Dim activeBackColor As Color = Color.Lavender
        Dim activeForeColor As Color = Color.MidnightBlue
        Dim activeFont As New Font("HoloLens MDL2 Assets", 15.75!, FontStyle.Bold)

        Dim inactiveBackColor As Color = Color.MidnightBlue
        Dim inactiveForeColor As Color = Color.WhiteSmoke
        Dim inactiveFont As New Font("Bookman Old Style", 12.0!, FontStyle.Regular)

        For Each ctrl As Control In {lbldashboard, btnchild, btnyouth, btnpregnantwomen, btnsenior, btnreports, btnrefdata, btnsystemlog, btnAdult, btnUsers}
            If ctrl Is activeControl Then
                ctrl.BackColor = activeBackColor
                ctrl.ForeColor = activeForeColor
                ctrl.Font = activeFont
            Else
                ctrl.BackColor = inactiveBackColor
                ctrl.ForeColor = inactiveForeColor
                ctrl.Font = inactiveFont
            End If
        Next
    End Sub

    Private Sub TheMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")

        LoadFormInPanel(New Dashboard())
        SetActiveControl(lbldashboard)

        ApplyRoleRestrictions()
        UpdatePopulation()
        DisplayCurrentUserName()
    End Sub

    Private Sub UpdatePopulation()
        Try
            GetQuery("SELECT COUNT(*) AS total FROM residents", "population")

            If ds.Tables.Contains("population") AndAlso ds.Tables("population").Rows.Count > 0 Then
                lblpopulation.Text = ds.Tables("population").Rows(0)("total").ToString()
            Else
                lblpopulation.Text = "0"
            End If
        Catch ex As Exception
            MsgBox("Error retrieving population: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub ApplyRoleRestrictions()
        Select Case CurrentUserRole.ToLower()
            Case "admin"

            Case "bns"
                btnrefdata.Visible = False
                btnsystemlog.Visible = False
                btnUsers.Visible = False

            Case "bhw"
                btnreports.Visible = False
                btnrefdata.Visible = False
                btnsystemlog.Visible = False
                btnUsers.Visible = False

            Case Else
                btnchild.Visible = False
                btnyouth.Visible = False
                btnpregnantwomen.Visible = False
                btnsenior.Visible = False
                btnreports.Visible = False
                btnrefdata.Visible = False
                btnsystemlog.Visible = False
                btnUsers.Visible = False
        End Select
    End Sub

    Private Sub lbldashboard_Click(sender As Object, e As EventArgs) Handles lbldashboard.Click
        LoadFormInPanel(New Dashboard())
        SetActiveControl(lbldashboard)
    End Sub

    Private Sub btnchild_Click(sender As Object, e As EventArgs) Handles btnchild.Click
        LoadFormInPanel(New Masterlist0to59MonthsOld())
        SetActiveControl(btnchild)
    End Sub

    Private Sub btnyouth_Click(sender As Object, e As EventArgs) Handles btnyouth.Click
        LoadFormInPanel(New Masterlist1to19YearsOld())
        SetActiveControl(btnyouth)
    End Sub

    Private Sub btnAdult_Click(sender As Object, e As EventArgs) Handles btnAdult.Click
        LoadFormInPanel(New Masterlist20to59YearsOld())
        SetActiveControl(btnAdult)
    End Sub

    Private Sub btnpregnantwomen_Click(sender As Object, e As EventArgs) Handles btnpregnantwomen.Click
        LoadFormInPanel(New MasterlistofPregnantWoman())
        SetActiveControl(btnpregnantwomen)
    End Sub

    Private Sub btnsenior_Click(sender As Object, e As EventArgs) Handles btnsenior.Click
        LoadFormInPanel(New MasterlistSeniorCitizens())
        SetActiveControl(btnsenior)
    End Sub

    Private Sub btnreports_Click(sender As Object, e As EventArgs) Handles btnreports.Click
        LoadFormInPanel(New Reports())
        SetActiveControl(btnreports)
    End Sub

    Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
        LoadFormInPanel(New UserManagement())
        SetActiveControl(btnUsers)
    End Sub

    Private Sub btnrefdata_Click(sender As Object, e As EventArgs) Handles btnrefdata.Click
        LoadFormInPanel(New ReferenceData())
        SetActiveControl(btnrefdata)
    End Sub

    Private Sub btnsystemlog_Click(sender As Object, e As EventArgs) Handles btnsystemlog.Click
        LoadFormInPanel(New SystemLogs())
        SetActiveControl(btnsystemlog)
    End Sub

    Private Sub Logout_Click(sender As Object, e As EventArgs) Handles btnlogout.Click, btnout.Click
        If MsgBox("Are you sure you want to log out?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Cancel") = MsgBoxResult.Yes Then
            Dim loginForm As New LogIn()
            loginForm.Show()
            Me.Close()

            LogAction("user", "Log Out", "Successfully logged out")

        End If
    End Sub

    Private Sub DisplayCurrentUserName()
        Try
            Dim query As String = "SELECT fullname FROM users WHERE userid = " & CurrentUser.ID
            GetQuery(query, "currentUser")

            If ds.Tables.Contains("currentUser") AndAlso ds.Tables("currentUser").Rows.Count > 0 Then
                lblName.Text = ds.Tables("currentUser").Rows(0)("fullname").ToString()
            Else
                lblName.Text = "Unknown User"
            End If
        Catch ex As Exception
            MsgBox("Error retrieving user name: " & ex.Message, MsgBoxStyle.Critical)
            lblName.Text = "Unknown User"
        End Try
    End Sub
End Class