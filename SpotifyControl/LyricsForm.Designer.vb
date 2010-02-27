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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.LyricBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'LyricProvider
        '
        Me.LyricProvider.Items.AddRange(New Object() {"chartlyrics.com", "lyrdb.com"})
        Me.LyricProvider.Location = New System.Drawing.Point(79, 1)
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
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(206, 1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(56, 21)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Search"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LyricBox
        '
        Me.LyricBox.Location = New System.Drawing.Point(3, 37)
        Me.LyricBox.Multiline = True
        Me.LyricBox.Name = "LyricBox"
        Me.LyricBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LyricBox.Size = New System.Drawing.Size(259, 272)
        Me.LyricBox.TabIndex = 3
        '
        'LyricsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(267, 313)
        Me.Controls.Add(Me.LyricBox)
        Me.Controls.Add(Me.Button1)
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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents LyricBox As System.Windows.Forms.TextBox
End Class
