<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MasterlistofPregnantWoman
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.panelProfile = New System.Windows.Forms.Panel()
        Me.txtsearch = New System.Windows.Forms.TextBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.btnRefresh = New System.Windows.Forms.Label()
        Me.rbthird = New System.Windows.Forms.RadioButton()
        Me.rbsecond = New System.Windows.Forms.RadioButton()
        Me.rbfirst = New System.Windows.Forms.RadioButton()
        Me.lvprofile = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnRecords = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.panelProfile.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Location = New System.Drawing.Point(7, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1071, 58)
        Me.Panel1.TabIndex = 244
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.DimGray
        Me.Label15.Font = New System.Drawing.Font("Palatino Linotype", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label15.Location = New System.Drawing.Point(-5, 48)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(1070, 1)
        Me.Label15.TabIndex = 290
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Lavender
        Me.Label14.Font = New System.Drawing.Font("Bookman Old Style", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Black
        Me.Label14.Location = New System.Drawing.Point(3, 16)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(744, 32)
        Me.Label14.TabIndex = 245
        Me.Label14.Text = "PREGNANT WOMAN MASTERLIST"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'panelProfile
        '
        Me.panelProfile.Controls.Add(Me.btnRecords)
        Me.panelProfile.Controls.Add(Me.txtsearch)
        Me.panelProfile.Controls.Add(Me.Label36)
        Me.panelProfile.Controls.Add(Me.btnRefresh)
        Me.panelProfile.Controls.Add(Me.rbthird)
        Me.panelProfile.Controls.Add(Me.rbsecond)
        Me.panelProfile.Controls.Add(Me.rbfirst)
        Me.panelProfile.Controls.Add(Me.lvprofile)
        Me.panelProfile.Controls.Add(Me.Label16)
        Me.panelProfile.Location = New System.Drawing.Point(9, 79)
        Me.panelProfile.Name = "panelProfile"
        Me.panelProfile.Size = New System.Drawing.Size(1067, 615)
        Me.panelProfile.TabIndex = 245
        '
        'txtsearch
        '
        Me.txtsearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtsearch.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsearch.Location = New System.Drawing.Point(567, 37)
        Me.txtsearch.Multiline = True
        Me.txtsearch.Name = "txtsearch"
        Me.txtsearch.Size = New System.Drawing.Size(371, 26)
        Me.txtsearch.TabIndex = 290
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.BackColor = System.Drawing.Color.Lavender
        Me.Label36.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.ForeColor = System.Drawing.Color.Black
        Me.Label36.Location = New System.Drawing.Point(76, 15)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(72, 19)
        Me.Label36.TabIndex = 289
        Me.Label36.Text = "Filter by:"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.Transparent
        Me.btnRefresh.Font = New System.Drawing.Font("Stencil", 22.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ForeColor = System.Drawing.Color.Black
        Me.btnRefresh.Location = New System.Drawing.Point(12, 25)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(59, 39)
        Me.btnRefresh.TabIndex = 288
        Me.btnRefresh.Text = "🔄"
        Me.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbthird
        '
        Me.rbthird.AutoSize = True
        Me.rbthird.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbthird.Location = New System.Drawing.Point(393, 36)
        Me.rbthird.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.rbthird.Name = "rbthird"
        Me.rbthird.Size = New System.Drawing.Size(138, 25)
        Me.rbthird.TabIndex = 280
        Me.rbthird.TabStop = True
        Me.rbthird.Text = "Third Trimester"
        Me.rbthird.UseVisualStyleBackColor = True
        '
        'rbsecond
        '
        Me.rbsecond.AutoSize = True
        Me.rbsecond.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbsecond.Location = New System.Drawing.Point(217, 36)
        Me.rbsecond.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.rbsecond.Name = "rbsecond"
        Me.rbsecond.Size = New System.Drawing.Size(160, 25)
        Me.rbsecond.TabIndex = 279
        Me.rbsecond.TabStop = True
        Me.rbsecond.Text = "Second Trimester"
        Me.rbsecond.UseVisualStyleBackColor = True
        '
        'rbfirst
        '
        Me.rbfirst.AutoSize = True
        Me.rbfirst.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbfirst.Location = New System.Drawing.Point(74, 36)
        Me.rbfirst.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.rbfirst.Name = "rbfirst"
        Me.rbfirst.Size = New System.Drawing.Size(130, 25)
        Me.rbfirst.TabIndex = 278
        Me.rbfirst.TabStop = True
        Me.rbfirst.Text = "First Trimester"
        Me.rbfirst.UseVisualStyleBackColor = True
        '
        'lvprofile
        '
        Me.lvprofile.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lvprofile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lvprofile.Font = New System.Drawing.Font("Bookman Old Style", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvprofile.FullRowSelect = True
        Me.lvprofile.GridLines = True
        Me.lvprofile.Location = New System.Drawing.Point(52, 83)
        Me.lvprofile.Name = "lvprofile"
        Me.lvprofile.Size = New System.Drawing.Size(969, 454)
        Me.lvprofile.TabIndex = 240
        Me.lvprofile.UseCompatibleStateImageBehavior = False
        Me.lvprofile.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "No."
        Me.ColumnHeader1.Width = 0
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Full Name"
        Me.ColumnHeader2.Width = 191
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Age"
        Me.ColumnHeader3.Width = 130
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Trimester"
        Me.ColumnHeader4.Width = 117
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Last Checkup"
        Me.ColumnHeader5.Width = 118
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Start"
        Me.ColumnHeader6.Width = 191
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Lavender
        Me.Label16.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label16.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.Black
        Me.Label16.Location = New System.Drawing.Point(951, 38)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(69, 19)
        Me.Label16.TabIndex = 239
        Me.Label16.Text = "SEARCH"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnRecords
        '
        Me.btnRecords.BackColor = System.Drawing.Color.MidnightBlue
        Me.btnRecords.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRecords.FlatAppearance.BorderSize = 0
        Me.btnRecords.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnRecords.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRecords.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnRecords.Location = New System.Drawing.Point(814, 557)
        Me.btnRecords.Margin = New System.Windows.Forms.Padding(2)
        Me.btnRecords.Name = "btnRecords"
        Me.btnRecords.Size = New System.Drawing.Size(206, 34)
        Me.btnRecords.TabIndex = 347
        Me.btnRecords.Text = "Show Past Records"
        Me.btnRecords.UseVisualStyleBackColor = False
        '
        'MasterlistofPregnantWoman
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(1086, 690)
        Me.Controls.Add(Me.panelProfile)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "MasterlistofPregnantWoman"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "MasterlistofPregnantWoman"
        Me.Panel1.ResumeLayout(False)
        Me.panelProfile.ResumeLayout(False)
        Me.panelProfile.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents panelProfile As System.Windows.Forms.Panel
    Friend WithEvents lvprofile As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents rbthird As System.Windows.Forms.RadioButton
    Friend WithEvents rbsecond As System.Windows.Forms.RadioButton
    Friend WithEvents rbfirst As System.Windows.Forms.RadioButton
    Friend WithEvents btnRefresh As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtsearch As System.Windows.Forms.TextBox
    Friend WithEvents btnRecords As System.Windows.Forms.Button
End Class
