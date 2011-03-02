<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LyricsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LyricsForm))
        Me.LyricProvider = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DwlBtn = New System.Windows.Forms.Button()
        Me.LyricBox = New System.Windows.Forms.TextBox()
        Me.OnTopChkBox = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'LyricProvider
        '
        Me.LyricProvider.Items.AddRange(New Object() {"chartlyrics.com", "lyrdb.com"})
        Me.LyricProvider.Location = New System.Drawing.Point(72, 1)
        Me.LyricProvider.Name = "LyricProvider"
        Me.LyricProvider.Size = New System.Drawing.Size(121, 21)
        Me.LyricProvider.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Get lyrics from"
        '
        'DwlBtn
        '
        Me.DwlBtn.Location = New System.Drawing.Point(199, 1)
        Me.DwlBtn.Name = "DwlBtn"
        Me.DwlBtn.Size = New System.Drawing.Size(56, 21)
        Me.DwlBtn.TabIndex = 2
        Me.DwlBtn.Text = "Search"
        Me.DwlBtn.UseVisualStyleBackColor = True
        '
        'LyricBox
        '
        Me.LyricBox.Location = New System.Drawing.Point(3, 28)
        Me.LyricBox.Multiline = True
        Me.LyricBox.Name = "LyricBox"
        Me.LyricBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LyricBox.Size = New System.Drawing.Size(252, 281)
        Me.LyricBox.TabIndex = 3
        '
        'OnTopChkBox
        '
        Me.OnTopChkBox.AutoSize = True
        Me.OnTopChkBox.Location = New System.Drawing.Point(52, 315)
        Me.OnTopChkBox.Name = "OnTopChkBox"
        Me.OnTopChkBox.Size = New System.Drawing.Size(141, 17)
        Me.OnTopChkBox.TabIndex = 4
        Me.OnTopChkBox.Text = "On top of other windows"
        Me.OnTopChkBox.UseVisualStyleBackColor = True
        '
        'LyricsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(258, 332)
        Me.Controls.Add(Me.OnTopChkBox)
        Me.Controls.Add(Me.LyricBox)
        Me.Controls.Add(Me.DwlBtn)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LyricProvider)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LyricsForm"
        Me.Text = "LyricsForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LyricProvider As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DwlBtn As System.Windows.Forms.Button
    Friend WithEvents LyricBox As System.Windows.Forms.TextBox
    Friend WithEvents OnTopChkBox As System.Windows.Forms.CheckBox
End Class
