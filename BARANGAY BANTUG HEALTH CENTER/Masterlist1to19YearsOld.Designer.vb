<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Masterlist1to19YearsOld
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
        Dim RectangleShape6 As Microsoft.VisualBasic.PowerPacks.RectangleShape
        Me.panel1to9YearsOld = New System.Windows.Forms.Panel()
        Me.rbFemale = New System.Windows.Forms.RadioButton()
        Me.rbMale = New System.Windows.Forms.RadioButton()
        Me.cmbPurok = New System.Windows.Forms.ComboBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.cmbAge = New System.Windows.Forms.ComboBox()
        Me.lvprofile = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.ShapeContainer4 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.btnRefresh = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtsearch = New System.Windows.Forms.TextBox()
        RectangleShape6 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.panel1to9YearsOld.SuspendLayout()
        Me.SuspendLayout()
        '
        'RectangleShape6
        '
        RectangleShape6.BackColor = System.Drawing.Color.WhiteSmoke
        RectangleShape6.BorderColor = System.Drawing.SystemColors.Window
        RectangleShape6.CornerRadius = 5
        RectangleShape6.FillColor = System.Drawing.Color.MidnightBlue
        RectangleShape6.FillGradientColor = System.Drawing.Color.Blue
        RectangleShape6.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        RectangleShape6.Location = New System.Drawing.Point(609, 359)
        RectangleShape6.Name = "RectangleShape6"
        RectangleShape6.SelectionColor = System.Drawing.Color.WhiteSmoke
        RectangleShape6.Size = New System.Drawing.Size(146, 29)
        '
        'panel1to9YearsOld
        '
        Me.panel1to9YearsOld.Controls.Add(Me.txtsearch)
        Me.panel1to9YearsOld.Controls.Add(Me.Label1)
        Me.panel1to9YearsOld.Controls.Add(Me.rbFemale)
        Me.panel1to9YearsOld.Controls.Add(Me.rbMale)
        Me.panel1to9YearsOld.Controls.Add(Me.cmbPurok)
        Me.panel1to9YearsOld.Controls.Add(Me.Label36)
        Me.panel1to9YearsOld.Controls.Add(Me.cmbAge)
        Me.panel1to9YearsOld.Controls.Add(Me.lvprofile)
        Me.panel1to9YearsOld.Controls.Add(Me.Label16)
        Me.panel1to9YearsOld.Controls.Add(Me.btnRefresh)
        Me.panel1to9YearsOld.Location = New System.Drawing.Point(0, 124)
        Me.panel1to9YearsOld.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.panel1to9YearsOld.Name = "panel1to9YearsOld"
        Me.panel1to9YearsOld.Size = New System.Drawing.Size(1624, 893)
        Me.panel1to9YearsOld.TabIndex = 237
        '
        'rbFemale
        '
        Me.rbFemale.AutoSize = True
        Me.rbFemale.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbFemale.Location = New System.Drawing.Point(294, 111)
        Me.rbFemale.Name = "rbFemale"
        Me.rbFemale.Size = New System.Drawing.Size(128, 34)
        Me.rbFemale.TabIndex = 277
        Me.rbFemale.TabStop = True
        Me.rbFemale.Text = "Female"
        Me.rbFemale.UseVisualStyleBackColor = True
        '
        'rbMale
        '
        Me.rbMale.AutoSize = True
        Me.rbMale.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbMale.Location = New System.Drawing.Point(189, 111)
        Me.rbMale.Name = "rbMale"
        Me.rbMale.Size = New System.Drawing.Size(99, 34)
        Me.rbMale.TabIndex = 276
        Me.rbMale.TabStop = True
        Me.rbMale.Text = "Male"
        Me.rbMale.UseVisualStyleBackColor = True
        '
        'cmbPurok
        '
        Me.cmbPurok.Font = New System.Drawing.Font("Perpetua Titling MT", 12.0!)
        Me.cmbPurok.FormattingEnabled = True
        Me.cmbPurok.Items.AddRange(New Object() {"underweight", "normal", "overweight"})
        Me.cmbPurok.Location = New System.Drawing.Point(116, 58)
        Me.cmbPurok.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbPurok.Name = "cmbPurok"
        Me.cmbPurok.Size = New System.Drawing.Size(246, 36)
        Me.cmbPurok.TabIndex = 275
        Me.cmbPurok.Text = "PUROK"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.BackColor = System.Drawing.Color.Lavender
        Me.Label36.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.ForeColor = System.Drawing.Color.Black
        Me.Label36.Location = New System.Drawing.Point(112, 22)
        Me.Label36.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(110, 28)
        Me.Label36.TabIndex = 273
        Me.Label36.Text = "Filter by:"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbAge
        '
        Me.cmbAge.Font = New System.Drawing.Font("Perpetua Titling MT", 12.0!)
        Me.cmbAge.FormattingEnabled = True
        Me.cmbAge.Items.AddRange(New Object() {"0 to 5", "1 to 10", "11 to 20", "21 to 30", "31 to 40", "41 to 50", "51 to 59"})
        Me.cmbAge.Location = New System.Drawing.Point(384, 58)
        Me.cmbAge.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbAge.Name = "cmbAge"
        Me.cmbAge.Size = New System.Drawing.Size(246, 36)
        Me.cmbAge.TabIndex = 274
        Me.cmbAge.Text = "AGE"
        '
        'lvprofile
        '
        Me.lvprofile.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8})
        Me.lvprofile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lvprofile.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvprofile.FullRowSelect = True
        Me.lvprofile.GridLines = True
        Me.lvprofile.Location = New System.Drawing.Point(81, 162)
        Me.lvprofile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lvprofile.Name = "lvprofile"
        Me.lvprofile.Size = New System.Drawing.Size(1466, 686)
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
        Me.ColumnHeader3.Text = "Birthday"
        Me.ColumnHeader3.Width = 130
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Age"
        Me.ColumnHeader4.Width = 117
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Purok"
        Me.ColumnHeader5.Width = 118
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Sex"
        Me.ColumnHeader6.Width = 191
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Contact"
        Me.ColumnHeader7.Width = 240
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Guardian"
        Me.ColumnHeader8.Width = 378
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Lavender
        Me.Label16.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label16.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.Black
        Me.Label16.Location = New System.Drawing.Point(1407, 87)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(103, 28)
        Me.Label16.TabIndex = 239
        Me.Label16.Text = "SEARCH"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(18, 18)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1606, 80)
        Me.Panel1.TabIndex = 243
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.DimGray
        Me.Label15.Font = New System.Drawing.Font("Palatino Linotype", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label15.Location = New System.Drawing.Point(36, 75)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(1582, 2)
        Me.Label15.TabIndex = 236
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Lavender
        Me.Label14.Font = New System.Drawing.Font("Bookman Old Style", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Black
        Me.Label14.Location = New System.Drawing.Point(22, 28)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(1106, 49)
        Me.Label14.TabIndex = 235
        Me.Label14.Text = "1 TO 19 YEARS OLD MASTERLIST"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ShapeContainer4
        '
        Me.ShapeContainer4.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer4.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer4.Name = "ShapeContainer4"
        Me.ShapeContainer4.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {RectangleShape6})
        Me.ShapeContainer4.Size = New System.Drawing.Size(1629, 1050)
        Me.ShapeContainer4.TabIndex = 245
        Me.ShapeContainer4.TabStop = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.Transparent
        Me.btnRefresh.Font = New System.Drawing.Font("Stencil", 22.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ForeColor = System.Drawing.Color.Black
        Me.btnRefresh.Location = New System.Drawing.Point(21, 40)
        Me.btnRefresh.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(88, 60)
        Me.btnRefresh.TabIndex = 278
        Me.btnRefresh.Text = "🔄"
        Me.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Lavender
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(123, 115)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 28)
        Me.Label1.TabIndex = 279
        Me.Label1.Text = "Sex:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtsearch
        '
        Me.txtsearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtsearch.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsearch.Location = New System.Drawing.Point(830, 83)
        Me.txtsearch.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtsearch.Multiline = True
        Me.txtsearch.Name = "txtsearch"
        Me.txtsearch.Size = New System.Drawing.Size(556, 40)
        Me.txtsearch.TabIndex = 290
        '
        'Masterlist1to19YearsOld
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(1629, 1050)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.panel1to9YearsOld)
        Me.Controls.Add(Me.ShapeContainer4)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "Masterlist1to19YearsOld"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bantug Health Center Information Management System"
        Me.panel1to9YearsOld.ResumeLayout(False)
        Me.panel1to9YearsOld.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panel1to9YearsOld As System.Windows.Forms.Panel
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ShapeContainer4 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents lvprofile As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents rbFemale As System.Windows.Forms.RadioButton
    Friend WithEvents rbMale As System.Windows.Forms.RadioButton
    Friend WithEvents cmbPurok As System.Windows.Forms.ComboBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents cmbAge As System.Windows.Forms.ComboBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtsearch As System.Windows.Forms.TextBox

End Class
