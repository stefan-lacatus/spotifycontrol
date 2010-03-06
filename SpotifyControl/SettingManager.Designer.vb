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
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.CancelBtn = New System.Windows.Forms.Button()
        Me.HotKeyTbl = New System.Windows.Forms.DataGridView()
        Me.Action = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HotKey = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UpdateBtn = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StartSrvBox = New System.Windows.Forms.Button()
        Me.StopServerBox = New System.Windows.Forms.Button()
        Me.TestSrvBox = New System.Windows.Forms.Button()
        CType(Me.HotKeyTbl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'SaveBtn
        '
        Me.SaveBtn.Location = New System.Drawing.Point(58, 359)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(75, 23)
        Me.SaveBtn.TabIndex = 4
        Me.SaveBtn.Text = "Save"
        Me.SaveBtn.UseVisualStyleBackColor = True
        '
        'CancelBtn
        '
        Me.CancelBtn.Location = New System.Drawing.Point(140, 358)
        Me.CancelBtn.Name = "CancelBtn"
        Me.CancelBtn.Size = New System.Drawing.Size(75, 23)
        Me.CancelBtn.TabIndex = 5
        Me.CancelBtn.Text = "Cancel"
        Me.CancelBtn.UseVisualStyleBackColor = True
        '
        'HotKeyTbl
        '
        Me.HotKeyTbl.AllowUserToAddRows = False
        Me.HotKeyTbl.AllowUserToDeleteRows = False
        Me.HotKeyTbl.AllowUserToResizeColumns = False
        Me.HotKeyTbl.AllowUserToResizeRows = False
        Me.HotKeyTbl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.HotKeyTbl.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Action, Me.HotKey})
        Me.HotKeyTbl.Location = New System.Drawing.Point(6, 21)
        Me.HotKeyTbl.MultiSelect = False
        Me.HotKeyTbl.Name = "HotKeyTbl"
        Me.HotKeyTbl.ReadOnly = True
        Me.HotKeyTbl.RowHeadersVisible = False
        Me.HotKeyTbl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.HotKeyTbl.Size = New System.Drawing.Size(206, 172)
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
        Me.UpdateBtn.Location = New System.Drawing.Point(8, 12)
        Me.UpdateBtn.Name = "UpdateBtn"
        Me.UpdateBtn.Size = New System.Drawing.Size(109, 19)
        Me.UpdateBtn.TabIndex = 6
        Me.UpdateBtn.Text = "Check for updates"
        Me.UpdateBtn.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TestSrvBox)
        Me.GroupBox1.Controls.Add(Me.StopServerBox)
        Me.GroupBox1.Controls.Add(Me.StartSrvBox)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 252)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(228, 90)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "WebControl"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.HotKeyTbl)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 38)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(223, 208)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "GlobalHotkeys"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(101, 19)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(116, 20)
        Me.TextBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Server Password"
        '
        'StartSrvBox
        '
        Me.StartSrvBox.Location = New System.Drawing.Point(11, 48)
        Me.StartSrvBox.Name = "StartSrvBox"
        Me.StartSrvBox.Size = New System.Drawing.Size(56, 23)
        Me.StartSrvBox.TabIndex = 2
        Me.StartSrvBox.Text = "Start"
        Me.StartSrvBox.UseVisualStyleBackColor = True
        '
        'StopServerBox
        '
        Me.StopServerBox.Location = New System.Drawing.Point(163, 48)
        Me.StopServerBox.Name = "StopServerBox"
        Me.StopServerBox.Size = New System.Drawing.Size(54, 23)
        Me.StopServerBox.TabIndex = 3
        Me.StopServerBox.Text = "Stop"
        Me.StopServerBox.UseVisualStyleBackColor = True
        '
        'TestSrvBox
        '
        Me.TestSrvBox.Enabled = False
        Me.TestSrvBox.Location = New System.Drawing.Point(85, 48)
        Me.TestSrvBox.Name = "TestSrvBox"
        Me.TestSrvBox.Size = New System.Drawing.Size(60, 23)
        Me.TestSrvBox.TabIndex = 4
        Me.TestSrvBox.Text = "Test"
        Me.TestSrvBox.UseVisualStyleBackColor = True
        '
        'SettingManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(270, 394)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.UpdateBtn)
        Me.Controls.Add(Me.CancelBtn)
        Me.Controls.Add(Me.SaveBtn)
        Me.Name = "SettingManager"
        Me.Text = "SettingManager"
        CType(Me.HotKeyTbl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SaveBtn As System.Windows.Forms.Button
    Friend WithEvents HotKeyTbl As System.Windows.Forms.DataGridView
    Friend WithEvents Action As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HotKey As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CancelBtn As System.Windows.Forms.Button
    Friend WithEvents UpdateBtn As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TestSrvBox As System.Windows.Forms.Button
    Friend WithEvents StopServerBox As System.Windows.Forms.Button
    Friend WithEvents StartSrvBox As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
