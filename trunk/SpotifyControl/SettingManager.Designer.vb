<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingManager
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
        Me.StartSpotifyChkBox = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PlayPauseModifier = New System.Windows.Forms.ComboBox()
        Me.PlayPauseKey = New System.Windows.Forms.ComboBox()
        Me.CheckGroupBox1 = New SpotifyControl.CheckGroupBox()
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.CheckGroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StartSpotifyChkBox
        '
        Me.StartSpotifyChkBox.AutoSize = True
        Me.StartSpotifyChkBox.Location = New System.Drawing.Point(13, 13)
        Me.StartSpotifyChkBox.Name = "StartSpotifyChkBox"
        Me.StartSpotifyChkBox.Size = New System.Drawing.Size(187, 17)
        Me.StartSpotifyChkBox.TabIndex = 0
        Me.StartSpotifyChkBox.Text = "Start Spotify on application launch"
        Me.StartSpotifyChkBox.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Play/Pause"
        '
        'PlayPauseModifier
        '
        Me.PlayPauseModifier.FormattingEnabled = True
        Me.PlayPauseModifier.Items.AddRange(New Object() {"None ", "AltKey", "CtrlKey", "ShiftKey ", "WinKey"})
        Me.PlayPauseModifier.Location = New System.Drawing.Point(75, 31)
        Me.PlayPauseModifier.Name = "PlayPauseModifier"
        Me.PlayPauseModifier.Size = New System.Drawing.Size(84, 21)
        Me.PlayPauseModifier.TabIndex = 1
        Me.PlayPauseModifier.Text = "Modifier"
        '
        'PlayPauseKey
        '
        Me.PlayPauseKey.FormattingEnabled = True
        Me.PlayPauseKey.Location = New System.Drawing.Point(165, 31)
        Me.PlayPauseKey.Name = "PlayPauseKey"
        Me.PlayPauseKey.Size = New System.Drawing.Size(84, 21)
        Me.PlayPauseKey.TabIndex = 2
        Me.PlayPauseKey.Text = "Key"
        '
        'CheckGroupBox1
        '
        Me.CheckGroupBox1.Checked = False
        Me.CheckGroupBox1.Controls.Add(Me.PlayPauseModifier)
        Me.CheckGroupBox1.Controls.Add(Me.PlayPauseKey)
        Me.CheckGroupBox1.Controls.Add(Me.Label1)
        Me.CheckGroupBox1.Location = New System.Drawing.Point(13, 53)
        Me.CheckGroupBox1.Name = "CheckGroupBox1"
        Me.CheckGroupBox1.Size = New System.Drawing.Size(322, 211)
        Me.CheckGroupBox1.TabIndex = 3
        Me.CheckGroupBox1.TabStop = False
        Me.CheckGroupBox1.Text = "Global Hotkeys"
        '
        'SaveBtn
        '
        Me.SaveBtn.Location = New System.Drawing.Point(96, 276)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(75, 23)
        Me.SaveBtn.TabIndex = 4
        Me.SaveBtn.Text = "Save"
        Me.SaveBtn.UseVisualStyleBackColor = True
        '
        'SettingManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(369, 311)
        Me.Controls.Add(Me.SaveBtn)
        Me.Controls.Add(Me.CheckGroupBox1)
        Me.Controls.Add(Me.StartSpotifyChkBox)
        Me.Name = "SettingManager"
        Me.Text = "SettingManager"
        Me.CheckGroupBox1.ResumeLayout(False)
        Me.CheckGroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StartSpotifyChkBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PlayPauseKey As System.Windows.Forms.ComboBox
    Friend WithEvents PlayPauseModifier As System.Windows.Forms.ComboBox
    Friend WithEvents CheckGroupBox1 As SpotifyControl.CheckGroupBox
    Friend WithEvents SaveBtn As System.Windows.Forms.Button
End Class
