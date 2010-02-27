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
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.CancelBtn = New System.Windows.Forms.Button()
        Me.CheckGroupBox1 = New SpotifyControl.CheckGroupBox()
        Me.HotKeyTbl = New System.Windows.Forms.DataGridView()
        Me.Action = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HotKey = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UpdateBtn = New System.Windows.Forms.Button()
        Me.CheckGroupBox1.SuspendLayout()
        CType(Me.HotKeyTbl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StartSpotifyChkBox
        '
        Me.StartSpotifyChkBox.AutoSize = True
        Me.StartSpotifyChkBox.Location = New System.Drawing.Point(13, 5)
        Me.StartSpotifyChkBox.Name = "StartSpotifyChkBox"
        Me.StartSpotifyChkBox.Size = New System.Drawing.Size(187, 17)
        Me.StartSpotifyChkBox.TabIndex = 0
        Me.StartSpotifyChkBox.Text = "Start Spotify on application launch"
        Me.StartSpotifyChkBox.UseVisualStyleBackColor = True
        '
        'SaveBtn
        '
        Me.SaveBtn.Location = New System.Drawing.Point(19, 275)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(75, 23)
        Me.SaveBtn.TabIndex = 4
        Me.SaveBtn.Text = "Save"
        Me.SaveBtn.UseVisualStyleBackColor = True
        '
        'CancelBtn
        '
        Me.CancelBtn.Location = New System.Drawing.Point(101, 274)
        Me.CancelBtn.Name = "CancelBtn"
        Me.CancelBtn.Size = New System.Drawing.Size(75, 23)
        Me.CancelBtn.TabIndex = 5
        Me.CancelBtn.Text = "Cancel"
        Me.CancelBtn.UseVisualStyleBackColor = True
        '
        'CheckGroupBox1
        '
        Me.CheckGroupBox1.Checked = False
        Me.CheckGroupBox1.Controls.Add(Me.HotKeyTbl)
        Me.CheckGroupBox1.Location = New System.Drawing.Point(13, 53)
        Me.CheckGroupBox1.Name = "CheckGroupBox1"
        Me.CheckGroupBox1.Size = New System.Drawing.Size(228, 207)
        Me.CheckGroupBox1.TabIndex = 3
        Me.CheckGroupBox1.TabStop = False
        Me.CheckGroupBox1.Text = "Global Hotkeys"
        '
        'HotKeyTbl
        '
        Me.HotKeyTbl.AllowUserToAddRows = False
        Me.HotKeyTbl.AllowUserToDeleteRows = False
        Me.HotKeyTbl.AllowUserToResizeColumns = False
        Me.HotKeyTbl.AllowUserToResizeRows = False
        Me.HotKeyTbl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.HotKeyTbl.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Action, Me.HotKey})
        Me.HotKeyTbl.Location = New System.Drawing.Point(6, 29)
        Me.HotKeyTbl.MultiSelect = False
        Me.HotKeyTbl.Name = "HotKeyTbl"
        Me.HotKeyTbl.ReadOnly = True
        Me.HotKeyTbl.RowHeadersVisible = False
        Me.HotKeyTbl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.HotKeyTbl.Size = New System.Drawing.Size(216, 172)
        Me.HotKeyTbl.TabIndex = 1
        '
        'Action
        '
        Me.Action.HeaderText = "Action"
        Me.Action.Name = "Action"
        Me.Action.ReadOnly = True
        '
        'HotKey
        '
        Me.HotKey.HeaderText = "Hotkey"
        Me.HotKey.Name = "HotKey"
        Me.HotKey.ReadOnly = True
        '
        'UpdateBtn
        '
        Me.UpdateBtn.Location = New System.Drawing.Point(12, 28)
        Me.UpdateBtn.Name = "UpdateBtn"
        Me.UpdateBtn.Size = New System.Drawing.Size(109, 19)
        Me.UpdateBtn.TabIndex = 6
        Me.UpdateBtn.Text = "Check for updates"
        Me.UpdateBtn.UseVisualStyleBackColor = True
        '
        'SettingManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(248, 306)
        Me.Controls.Add(Me.UpdateBtn)
        Me.Controls.Add(Me.CancelBtn)
        Me.Controls.Add(Me.SaveBtn)
        Me.Controls.Add(Me.CheckGroupBox1)
        Me.Controls.Add(Me.StartSpotifyChkBox)
        Me.Name = "SettingManager"
        Me.Text = "SettingManager"
        Me.CheckGroupBox1.ResumeLayout(False)
        CType(Me.HotKeyTbl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StartSpotifyChkBox As System.Windows.Forms.CheckBox
    Friend WithEvents CheckGroupBox1 As SpotifyControl.CheckGroupBox
    Friend WithEvents SaveBtn As System.Windows.Forms.Button
    Friend WithEvents HotKeyTbl As System.Windows.Forms.DataGridView
    Friend WithEvents Action As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HotKey As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CancelBtn As System.Windows.Forms.Button
    Friend WithEvents UpdateBtn As System.Windows.Forms.Button
End Class
