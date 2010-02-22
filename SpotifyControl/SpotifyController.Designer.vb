<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpotifyController
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
        Me.VolumeControl = New System.Windows.Forms.TrackBar()
        Me.SongCheck = New System.Windows.Forms.Timer()
        Me.TextBox1 = New System.Windows.Forms.Label()
        Me.SettingImg = New System.Windows.Forms.PictureBox()
        Me.CloseImg = New System.Windows.Forms.PictureBox()
        Me.MuteImg = New System.Windows.Forms.PictureBox()
        Me.NextImg = New System.Windows.Forms.PictureBox()
        Me.VolUpImg = New System.Windows.Forms.PictureBox()
        Me.VolDownImg = New System.Windows.Forms.PictureBox()
        Me.PlayPauseImg = New System.Windows.Forms.PictureBox()
        Me.PrevImg = New System.Windows.Forms.PictureBox()
        Me.SuspendLayout()
        '
        'VolumeControl
        '
        Me.VolumeControl.Location = New System.Drawing.Point(132, 6)
        Me.VolumeControl.Name = "VolumeControl"
        Me.VolumeControl.Size = New System.Drawing.Size(72, 45)
        Me.VolumeControl.TabIndex = 4
        Me.VolumeControl.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'SongCheck
        '
        Me.SongCheck.Enabled = True
        Me.SongCheck.Interval = 2000
        '
        'TextBox1
        '
        Me.TextBox1.AutoSize = True
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(231, 6)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(57, 20)
        Me.TextBox1.TabIndex = 11
        Me.TextBox1.Text = "Label1"
        '
        'SettingImg
        '
        Me.SettingImg.BackColor = System.Drawing.Color.Transparent
        Me.SettingImg.Image = Global.SpotifyControl.My.Resources.Resources._527830478
        Me.SettingImg.Location = New System.Drawing.Point(496, 3)
        Me.SettingImg.Name = "SettingImg"
        Me.SettingImg.Size = New System.Drawing.Size(32, 26)
        Me.SettingImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.SettingImg.TabIndex = 16
        Me.SettingImg.TabStop = False
        '
        'CloseImg
        '
        Me.CloseImg.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.CloseImg.Image = Global.SpotifyControl.My.Resources.Resources.Capture
        Me.CloseImg.Location = New System.Drawing.Point(534, 6)
        Me.CloseImg.Name = "CloseImg"
        Me.CloseImg.Size = New System.Drawing.Size(31, 19)
        Me.CloseImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.CloseImg.TabIndex = 15
        Me.CloseImg.TabStop = False
        '
        'MuteImg
        '
        Me.MuteImg.BackColor = System.Drawing.Color.Transparent
        Me.MuteImg.Image = Global.SpotifyControl.My.Resources.Resources.Mute
        Me.MuteImg.Location = New System.Drawing.Point(82, 1)
        Me.MuteImg.Name = "MuteImg"
        Me.MuteImg.Size = New System.Drawing.Size(26, 28)
        Me.MuteImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.MuteImg.TabIndex = 14
        Me.MuteImg.TabStop = False
        '
        'NextImg
        '
        Me.NextImg.BackColor = System.Drawing.Color.Transparent
        Me.NextImg.Image = Global.SpotifyControl.My.Resources.Resources.Next2
        Me.NextImg.Location = New System.Drawing.Point(55, 2)
        Me.NextImg.Name = "NextImg"
        Me.NextImg.Size = New System.Drawing.Size(28, 31)
        Me.NextImg.TabIndex = 10
        Me.NextImg.TabStop = False
        '
        'VolUpImg
        '
        Me.VolUpImg.BackColor = System.Drawing.Color.Transparent
        Me.VolUpImg.Image = Global.SpotifyControl.My.Resources.Resources.VolUp
        Me.VolUpImg.Location = New System.Drawing.Point(199, 1)
        Me.VolUpImg.Name = "VolUpImg"
        Me.VolUpImg.Size = New System.Drawing.Size(26, 28)
        Me.VolUpImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.VolUpImg.TabIndex = 13
        Me.VolUpImg.TabStop = False
        '
        'VolDownImg
        '
        Me.VolDownImg.BackColor = System.Drawing.Color.Transparent
        Me.VolDownImg.Image = Global.SpotifyControl.My.Resources.Resources.VolDown
        Me.VolDownImg.Location = New System.Drawing.Point(111, 1)
        Me.VolDownImg.Name = "VolDownImg"
        Me.VolDownImg.Size = New System.Drawing.Size(26, 28)
        Me.VolDownImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.VolDownImg.TabIndex = 12
        Me.VolDownImg.TabStop = False
        '
        'PlayPauseImg
        '
        Me.PlayPauseImg.BackColor = System.Drawing.Color.Transparent
        Me.PlayPauseImg.Image = Global.SpotifyControl.My.Resources.Resources.Play1
        Me.PlayPauseImg.Location = New System.Drawing.Point(26, -2)
        Me.PlayPauseImg.Name = "PlayPauseImg"
        Me.PlayPauseImg.Size = New System.Drawing.Size(34, 35)
        Me.PlayPauseImg.TabIndex = 8
        Me.PlayPauseImg.TabStop = False
        Me.PlayPauseImg.Tag = "Play"
        '
        'PrevImg
        '
        Me.PrevImg.BackColor = System.Drawing.Color.Transparent
        Me.PrevImg.Image = Global.SpotifyControl.My.Resources.Resources.Prev2
        Me.PrevImg.Location = New System.Drawing.Point(1, 1)
        Me.PrevImg.Name = "PrevImg"
        Me.PrevImg.Size = New System.Drawing.Size(29, 32)
        Me.PrevImg.TabIndex = 9
        Me.PrevImg.TabStop = False
        '
        'SpotifyController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(566, 33)
        Me.Controls.Add(Me.CloseImg)
        Me.Controls.Add(Me.SettingImg)
        Me.Controls.Add(Me.MuteImg)
        Me.Controls.Add(Me.NextImg)
        Me.Controls.Add(Me.VolUpImg)
        Me.Controls.Add(Me.VolDownImg)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.PlayPauseImg)
        Me.Controls.Add(Me.PrevImg)
        Me.Controls.Add(Me.VolumeControl)
        Me.Name = "SpotifyController"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "SpotifyController"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VolumeControl As System.Windows.Forms.TrackBar
    Friend WithEvents SongCheck As System.Windows.Forms.Timer
    Friend WithEvents PlayPauseImg As System.Windows.Forms.PictureBox
    Friend WithEvents PrevImg As System.Windows.Forms.PictureBox
    Friend WithEvents NextImg As System.Windows.Forms.PictureBox
    Friend WithEvents TextBox1 As System.Windows.Forms.Label
    Friend WithEvents VolDownImg As System.Windows.Forms.PictureBox
    Friend WithEvents VolUpImg As System.Windows.Forms.PictureBox
    Friend WithEvents MuteImg As System.Windows.Forms.PictureBox
    Friend WithEvents CloseImg As System.Windows.Forms.PictureBox
    Friend WithEvents SettingImg As System.Windows.Forms.PictureBox

End Class
