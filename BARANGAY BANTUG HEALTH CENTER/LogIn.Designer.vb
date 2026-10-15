<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogIn
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LogIn))
        Me.pbLogo = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnexit = New System.Windows.Forms.Button()
        Me.lblerrormessage = New System.Windows.Forms.Label()
        Me.showpwd = New System.Windows.Forms.PictureBox()
        Me.hidepwd = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnlogin = New System.Windows.Forms.Button()
        Me.txtemail = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtpwd = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ErrorTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.pbLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.showpwd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.hidepwd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbLogo
        '
        Me.pbLogo.BackColor = System.Drawing.Color.MidnightBlue
        Me.pbLogo.Image = CType(resources.GetObject("pbLogo.Image"), System.Drawing.Image)
        Me.pbLogo.Location = New System.Drawing.Point(140, 197)
        Me.pbLogo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pbLogo.Name = "pbLogo"
        Me.pbLogo.Size = New System.Drawing.Size(218, 209)
        Me.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbLogo.TabIndex = 234
        Me.pbLogo.TabStop = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.MidnightBlue
        Me.Label1.Location = New System.Drawing.Point(-4, -2)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(489, 936)
        Me.Label1.TabIndex = 235
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.MidnightBlue
        Me.Label3.Font = New System.Drawing.Font("Bookman Old Style", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label3.Location = New System.Drawing.Point(24, 411)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(430, 204)
        Me.Label3.TabIndex = 236
        Me.Label3.Text = "BARANGAY HEALTH CENTER INFORMATION MANAGEMENT SYSTEM"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Bookman Old Style", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label4.Location = New System.Drawing.Point(571, 126)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(723, 105)
        Me.Label4.TabIndex = 237
        Me.Label4.Text = "Supporting community health through secure and reliable information."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnexit
        '
        Me.btnexit.BackColor = System.Drawing.Color.DarkBlue
        Me.btnexit.FlatAppearance.BorderColor = System.Drawing.Color.DarkCyan
        Me.btnexit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnexit.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnexit.Font = New System.Drawing.Font("Segoe UI Emoji", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnexit.ForeColor = System.Drawing.Color.White
        Me.btnexit.Location = New System.Drawing.Point(1280, 34)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(47, 39)
        Me.btnexit.TabIndex = 238
        Me.btnexit.Text = "X"
        Me.btnexit.UseVisualStyleBackColor = False
        '
        'lblerrormessage
        '
        Me.lblerrormessage.BackColor = System.Drawing.Color.Transparent
        Me.lblerrormessage.Font = New System.Drawing.Font("Lucida Sans", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblerrormessage.Location = New System.Drawing.Point(653, 555)
        Me.lblerrormessage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblerrormessage.Name = "lblerrormessage"
        Me.lblerrormessage.Size = New System.Drawing.Size(522, 35)
        Me.lblerrormessage.TabIndex = 247
        Me.lblerrormessage.Text = "label"
        '
        'showpwd
        '
        Me.showpwd.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.showpwd.Image = CType(resources.GetObject("showpwd.Image"), System.Drawing.Image)
        Me.showpwd.Location = New System.Drawing.Point(1117, 496)
        Me.showpwd.Name = "showpwd"
        Me.showpwd.Size = New System.Drawing.Size(33, 29)
        Me.showpwd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.showpwd.TabIndex = 245
        Me.showpwd.TabStop = False
        '
        'hidepwd
        '
        Me.hidepwd.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.hidepwd.Image = CType(resources.GetObject("hidepwd.Image"), System.Drawing.Image)
        Me.hidepwd.Location = New System.Drawing.Point(1117, 496)
        Me.hidepwd.Name = "hidepwd"
        Me.hidepwd.Size = New System.Drawing.Size(33, 29)
        Me.hidepwd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.hidepwd.TabIndex = 246
        Me.hidepwd.TabStop = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Bookman Old Style", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(574, 231)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(306, 35)
        Me.Label5.TabIndex = 244
        Me.Label5.Text = "Log in to your account"
        '
        'btnlogin
        '
        Me.btnlogin.BackColor = System.Drawing.Color.DarkBlue
        Me.btnlogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnlogin.Font = New System.Drawing.Font("Bookman Old Style", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnlogin.ForeColor = System.Drawing.Color.White
        Me.btnlogin.Location = New System.Drawing.Point(809, 627)
        Me.btnlogin.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnlogin.Name = "btnlogin"
        Me.btnlogin.Size = New System.Drawing.Size(243, 64)
        Me.btnlogin.TabIndex = 243
        Me.btnlogin.Text = "Log In"
        Me.btnlogin.UseVisualStyleBackColor = False
        '
        'txtemail
        '
        Me.txtemail.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtemail.Font = New System.Drawing.Font("Bookman Old Style", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtemail.Location = New System.Drawing.Point(657, 366)
        Me.txtemail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtemail.Multiline = True
        Me.txtemail.Name = "txtemail"
        Me.txtemail.Size = New System.Drawing.Size(518, 51)
        Me.txtemail.TabIndex = 239
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Bookman Old Style", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(661, 336)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(182, 35)
        Me.Label2.TabIndex = 241
        Me.Label2.Text = "Username"
        '
        'txtpwd
        '
        Me.txtpwd.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.txtpwd.Font = New System.Drawing.Font("Bookman Old Style", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtpwd.Location = New System.Drawing.Point(657, 484)
        Me.txtpwd.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtpwd.Multiline = True
        Me.txtpwd.Name = "txtpwd"
        Me.txtpwd.Size = New System.Drawing.Size(518, 54)
        Me.txtpwd.TabIndex = 240
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Bookman Old Style", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(659, 454)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(162, 35)
        Me.Label6.TabIndex = 242
        Me.Label6.Text = "Password"
        '
        'ErrorTimer
        '
        Me.ErrorTimer.Interval = 3000
        '
        'LogIn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(1370, 865)
        Me.Controls.Add(Me.lblerrormessage)
        Me.Controls.Add(Me.showpwd)
        Me.Controls.Add(Me.hidepwd)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnlogin)
        Me.Controls.Add(Me.txtemail)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtpwd)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnexit)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pbLogo)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "LogIn"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LogIn"
        CType(Me.pbLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.showpwd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.hidepwd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbLogo As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents lblerrormessage As System.Windows.Forms.Label
    Friend WithEvents showpwd As System.Windows.Forms.PictureBox
    Friend WithEvents hidepwd As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnlogin As System.Windows.Forms.Button
    Friend WithEvents txtemail As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtpwd As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ErrorTimer As System.Windows.Forms.Timer
End Class
