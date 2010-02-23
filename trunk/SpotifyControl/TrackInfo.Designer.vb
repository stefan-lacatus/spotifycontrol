<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TrackInfo
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
        Me.AlbumArtBox = New System.Windows.Forms.PictureBox()
        Me.OpacityTimer = New System.Windows.Forms.Timer()
        Me.TimeVisibleTimer = New System.Windows.Forms.Timer()
        Me.TrackTitleLbl = New System.Windows.Forms.Label()
        Me.AlbumLbl = New System.Windows.Forms.Label()
        Me.ArtistLbl = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'AlbumArtBox
        '
        Me.AlbumArtBox.Location = New System.Drawing.Point(12, 12)
        Me.AlbumArtBox.Name = "AlbumArtBox"
        Me.AlbumArtBox.Size = New System.Drawing.Size(84, 68)
        Me.AlbumArtBox.TabIndex = 0
        Me.AlbumArtBox.TabStop = False
        '
        'OpacityTimer
        '
        '
        'TimeVisibleTimer
        '
        Me.TimeVisibleTimer.Interval = 5000
        '
        'TrackTitleLbl
        '
        Me.TrackTitleLbl.AutoSize = True
        Me.TrackTitleLbl.Location = New System.Drawing.Point(102, 12)
        Me.TrackTitleLbl.Name = "TrackTitleLbl"
        Me.TrackTitleLbl.Size = New System.Drawing.Size(39, 13)
        Me.TrackTitleLbl.TabIndex = 1
        Me.TrackTitleLbl.Text = "Label1"
        '
        'AlbumLbl
        '
        Me.AlbumLbl.AutoSize = True
        Me.AlbumLbl.Location = New System.Drawing.Point(102, 40)
        Me.AlbumLbl.Name = "AlbumLbl"
        Me.AlbumLbl.Size = New System.Drawing.Size(39, 13)
        Me.AlbumLbl.TabIndex = 2
        Me.AlbumLbl.Text = "Label2"
        '
        'ArtistLbl
        '
        Me.ArtistLbl.AutoSize = True
        Me.ArtistLbl.Location = New System.Drawing.Point(102, 67)
        Me.ArtistLbl.Name = "ArtistLbl"
        Me.ArtistLbl.Size = New System.Drawing.Size(39, 13)
        Me.ArtistLbl.TabIndex = 3
        Me.ArtistLbl.Text = "Label3"
        '
        'TrackInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 93)
        Me.Controls.Add(Me.ArtistLbl)
        Me.Controls.Add(Me.AlbumLbl)
        Me.Controls.Add(Me.TrackTitleLbl)
        Me.Controls.Add(Me.AlbumArtBox)
        Me.Name = "TrackInfo"
        Me.Text = "TrackInfo"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AlbumArtBox As System.Windows.Forms.PictureBox
    Friend WithEvents OpacityTimer As System.Windows.Forms.Timer
    Friend WithEvents TimeVisibleTimer As System.Windows.Forms.Timer
    Friend WithEvents TrackTitleLbl As System.Windows.Forms.Label
    Friend WithEvents AlbumLbl As System.Windows.Forms.Label
    Friend WithEvents ArtistLbl As System.Windows.Forms.Label
End Class
