Public Class ReferenceData

    Private Sub ReferenceDataManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect("localhost", "bhcims", "3306", "root", "")
        FillClassif()
        FillPurok()
        FillSex()

        txtclassification.Enabled = False
        btnSaveClassif.Enabled = False

        txtpurok.Enabled = False
        btnSavePurok.Enabled = False

        txtsex.Enabled = False
        btnSaveSex.Enabled = False

        lblErrorMessage.Visible = False
    End Sub

    Private Sub FillClassif()
        Try
            GetQuery("SELECT * FROM classification ORDER BY classid", "classification")
            lvClassif.Items.Clear()
            For i = 0 To ds.Tables("classification").Rows.Count - 1
                lvClassif.Items.Add(ds.Tables("classification").Rows(i).Item("classid").ToString)
                lvClassif.Items(i).SubItems.Add(ds.Tables("classification").Rows(i).Item("classname").ToString)
            Next
        Catch ex As Exception
            lblErrorMessage.Text = "Error loading Classification: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub lvClassif_DoubleClick(sender As Object, e As EventArgs) Handles lvClassif.DoubleClick
        Try
            cid = lvClassif.FocusedItem.SubItems(0).Text
            GetQuery("SELECT * FROM classification WHERE classid='" & cid & "'", "classification")
            txtclassification.Text = ds.Tables("classification").Rows(0).Item("classname").ToString
            txtclassification.Enabled = True
            btnSaveClassif.Enabled = True
            updating = True
            lblErrorMessage.Text = "Double-clicked. You can now edit."
            lblErrorMessage.ForeColor = Color.DarkBlue
            lblErrorMessage.Visible = True
        Catch ex As Exception
            lblErrorMessage.Text = "Error selecting Classification: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub btnAddClassif_Click(sender As Object, e As EventArgs) Handles btnaddclassif.Click
        txtclassification.Text = ""
        txtclassification.Enabled = True
        btnSaveClassif.Enabled = True
        updating = False
        lblErrorMessage.Text = "Enter new Classification and click Save."
        lblErrorMessage.ForeColor = Color.DarkBlue
        lblErrorMessage.Visible = True
    End Sub

    Private Sub btnSaveClassif_Click(sender As Object, e As EventArgs) Handles btnSaveClassif.Click
        Try
            If String.IsNullOrEmpty(txtclassification.Text) Then Exit Sub

            If System.Text.RegularExpressions.Regex.IsMatch(txtclassification.Text, "\d") Then
                lblErrorMessage.Text = "Classification cannot contain numbers!"
                lblErrorMessage.ForeColor = Color.Red
                lblErrorMessage.Visible = True
                Exit Sub
            End If

            If updating Then
                SetQuery("UPDATE classification SET classname='" & txtclassification.Text.Trim & "' WHERE classid='" & cid & "'")
                LogAction("classification", "Update", "Updated a classification: " & txtsex.Text.Trim())
            Else
                SetQuery("INSERT INTO classification (classname) VALUES('" & txtclassification.Text.Trim & "')")
                LogAction("classification", "Added", "Added new classification: " & txtsex.Text.Trim())
            End If
            FillClassif()
            ResetClassif()
            lblErrorMessage.Text = "Classification saved successfully!"
            lblErrorMessage.ForeColor = Color.DarkBlue
            lblErrorMessage.Visible = True
        Catch ex As Exception
            lblErrorMessage.Text = "Error saving Classification: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub


    Private Sub btnClearClassif_Click(sender As Object, e As EventArgs) Handles btnclearclassif.Click
        ResetClassif()
    End Sub

    Private Sub ResetClassif()
        txtclassification.Text = ""
        txtclassification.Enabled = False
        btnSaveClassif.Enabled = False
        cid = Nothing
        updating = False
        lblErrorMessage.Visible = False
    End Sub

    Private Sub FillPurok()
        Try
            GetQuery("SELECT * FROM purok ORDER BY purokid", "purok")
            lvPurok.Items.Clear()
            For i = 0 To ds.Tables("purok").Rows.Count - 1
                lvPurok.Items.Add(ds.Tables("purok").Rows(i).Item("purokid").ToString)
                lvPurok.Items(i).SubItems.Add(ds.Tables("purok").Rows(i).Item("purokname").ToString)
            Next
        Catch ex As Exception
            lblErrorMessage.Text = "Error loading Purok: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub lvPurok_DoubleClick(sender As Object, e As EventArgs) Handles lvPurok.DoubleClick
        Try
            cid = lvPurok.FocusedItem.SubItems(0).Text
            GetQuery("SELECT * FROM purok WHERE purokid='" & cid & "'", "purok")
            txtpurok.Text = ds.Tables("purok").Rows(0).Item("purokname").ToString
            txtpurok.Enabled = True
            btnSavePurok.Enabled = True
            updating = True
            lblErrorMessage.Text = "Double-clicked. You can now edit."
            lblErrorMessage.ForeColor = Color.DarkBlue
            lblErrorMessage.Visible = True
        Catch ex As Exception
            lblErrorMessage.Text = "Error selecting Purok: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub btnAddPurok_Click(sender As Object, e As EventArgs) Handles btnaddpurok.Click
        txtpurok.Text = ""
        txtpurok.Enabled = True
        btnSavePurok.Enabled = True
        updating = False
        lblErrorMessage.Text = "Enter new Purok and click Save."
        lblErrorMessage.ForeColor = Color.DarkBlue
        lblErrorMessage.Visible = True
    End Sub

    Private Sub btnSavePurok_Click(sender As Object, e As EventArgs) Handles btnSavePurok.Click
        Try
            If String.IsNullOrEmpty(txtpurok.Text) Then Exit Sub

            ' Validation: must not contain numbers
            If Not System.Text.RegularExpressions.Regex.IsMatch(txtpurok.Text, "^\d+$") Then
                lblerrormessage.Text = "Purok must be numeric only!"
                lblerrormessage.ForeColor = Color.Red
                lblerrormessage.Visible = True
                Exit Sub
            End If

            If updating Then
                SetQuery("UPDATE purok SET purokname='" & txtpurok.Text.Trim & "' WHERE purokid='" & cid & "'")
                LogAction("purok", "Update", "Updated a purok: " & txtpurok.Text.Trim())
            Else
                SetQuery("INSERT INTO purok (purokname) VALUES('" & txtpurok.Text.Trim & "')")
                LogAction("purok", "Add", "Added new purok: " & txtpurok.Text.Trim())
            End If
            FillPurok()
            ResetPurok()
            lblErrorMessage.Text = "Purok saved successfully!"
            lblErrorMessage.ForeColor = Color.DarkBlue
            lblErrorMessage.Visible = True
        Catch ex As Exception
            lblErrorMessage.Text = "Error saving Purok: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub btnClearPurok_Click(sender As Object, e As EventArgs) Handles btnclearpurok.Click
        ResetPurok()
    End Sub

    Private Sub ResetPurok()
        txtpurok.Text = ""
        txtpurok.Enabled = False
        btnSavePurok.Enabled = False
        cid = Nothing
        updating = False
        lblErrorMessage.Visible = False
    End Sub

    Private Sub FillSex()
        Try
            GetQuery("SELECT * FROM sex ORDER BY sexid", "sex")
            lvSx.Items.Clear()
            For i = 0 To ds.Tables("sex").Rows.Count - 1
                lvSx.Items.Add(ds.Tables("sex").Rows(i).Item("sexid").ToString)
                lvSx.Items(i).SubItems.Add(ds.Tables("sex").Rows(i).Item("sexname").ToString)
            Next
        Catch ex As Exception
            lblErrorMessage.Text = "Error loading Sex: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub lvSex_DoubleClick(sender As Object, e As EventArgs) Handles lvSx.DoubleClick
        Try
            cid = lvSx.FocusedItem.SubItems(0).Text
            GetQuery("SELECT * FROM sex WHERE sexid='" & cid & "'", "sex")
            txtsex.Text = ds.Tables("sex").Rows(0).Item("sexname").ToString
            txtsex.Enabled = True
            btnSaveSex.Enabled = True
            updating = True
            lblErrorMessage.Text = "Double-clicked. You can now edit."
            lblErrorMessage.ForeColor = Color.DarkBlue
            lblErrorMessage.Visible = True
        Catch ex As Exception
            lblErrorMessage.Text = "Error selecting Sex: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub btnAddSex_Click(sender As Object, e As EventArgs) Handles btnaddsex.Click
        txtsex.Text = ""
        txtsex.Enabled = True
        btnSaveSex.Enabled = True
        updating = False
        lblErrorMessage.Text = "Enter new Sex and click Save."
        lblErrorMessage.ForeColor = Color.DarkBlue
        lblErrorMessage.Visible = True
    End Sub

    Private Sub btnSaveSex_Click(sender As Object, e As EventArgs) Handles btnSaveSex.Click
        Try
            If String.IsNullOrEmpty(txtsex.Text) Then Exit Sub

            If System.Text.RegularExpressions.Regex.IsMatch(txtsex.Text, "\d") Then
                lblErrorMessage.Text = "Sex cannot contain numbers!"
                lblErrorMessage.ForeColor = Color.Red
                lblErrorMessage.Visible = True
                Exit Sub
            End If

            If updating Then
                SetQuery("UPDATE sex SET sexname='" & txtsex.Text.Trim & "' WHERE sexid='" & cid & "'")
                LogAction("sex", "Update", "Updated a sex: " & txtsex.Text.Trim())
            Else
                SetQuery("INSERT INTO sex (sexname) VALUES('" & txtsex.Text.Trim & "')")
                LogAction("sex", "Add", "Added new sex: " & txtsex.Text.Trim())
            End If
            FillSex()
            ResetSex()
            lblErrorMessage.Text = "Sex saved successfully!"
            lblErrorMessage.ForeColor = Color.DarkBlue
            lblErrorMessage.Visible = True
        Catch ex As Exception
            lblErrorMessage.Text = "Error saving Sex: " & ex.Message
            lblErrorMessage.ForeColor = Color.Red
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub btnClearSex_Click(sender As Object, e As EventArgs) Handles btnclearsex.Click
        ResetSex()
    End Sub

    Private Sub ResetSex()
        txtsex.Text = ""
        txtsex.Enabled = False
        btnSaveSex.Enabled = False
        cid = Nothing
        updating = False
        lblErrorMessage.Visible = False
    End Sub

End Class
