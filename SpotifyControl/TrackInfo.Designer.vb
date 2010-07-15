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
        Me.components = New System.ComponentModel.Container()
        Me.AlbumArtBox = New System.Windows.Forms.PictureBox()
        Me.OpacityTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TimeVisibleTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ArtistLbl = New SpotifyControl.LabelVer2()
        Me.AlbumLbl = New SpotifyControl.LabelVer2()
        Me.TrackTitleLbl = New SpotifyControl.LabelVer2()
        CType(Me.AlbumArtBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AlbumArtBox
        '
        Me.AlbumArtBox.Location = New System.Drawing.Point(12, 6)
        Me.AlbumArtBox.Name = "AlbumArtBox"
        Me.AlbumArtBox.Size = New System.Drawing.Size(90, 90)
        Me.AlbumArtBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
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
        'ArtistLbl
        '
        Me.ArtistLbl.Font = New System.Drawing.Font("Monotype Corsiva", 14.5!, System.Drawing.FontStyle.Italic)
        Me.ArtistLbl.HaloColor = System.Drawing.Color.LightGray
        Me.ArtistLbl.HaloText = True
        Me.ArtistLbl.Location = New System.Drawing.Point(108, 65)
        Me.ArtistLbl.Name = "ArtistLbl"
        Me.ArtistLbl.ScrollLeftToRight = SpotifyControl.LabelVer2.Direction.Right
        Me.ArtistLbl.Size = New System.Drawing.Size(566, 27)
        Me.ArtistLbl.TabIndex = 6
        Me.ArtistLbl.TimeBeforStart = 1000
        '
        'AlbumLbl
        '
        Me.AlbumLbl.Font = New System.Drawing.Font("Monotype Corsiva", 14.0!, System.Drawing.FontStyle.Italic)
        Me.AlbumLbl.HaloColor = System.Drawing.Color.LightGray
        Me.AlbumLbl.HaloText = True
        Me.AlbumLbl.Location = New System.Drawing.Point(108, 37)
        Me.AlbumLbl.Name = "AlbumLbl"
        Me.AlbumLbl.ScrollLeftToRight = SpotifyControl.LabelVer2.Direction.Right
        Me.AlbumLbl.Size = New System.Drawing.Size(566, 25)
        Me.AlbumLbl.TabIndex = 5
        Me.AlbumLbl.TimeBeforStart = 1000
        '
        'TrackTitleLbl
        '
        Me.TrackTitleLbl.Font = New System.Drawing.Font("Monotype Corsiva", 15.0!, System.Drawing.FontStyle.Italic)
        Me.TrackTitleLbl.HaloColor = System.Drawing.Color.LightGray
        Me.TrackTitleLbl.HaloText = True
        Me.TrackTitleLbl.Location = New System.Drawing.Point(108, 6)
        Me.TrackTitleLbl.Name = "TrackTitleLbl"
        Me.TrackTitleLbl.ScrollLeftToRight = SpotifyControl.LabelVer2.Direction.Right
        Me.TrackTitleLbl.Size = New System.Drawing.Size(566, 31)
        Me.TrackTitleLbl.TabIndex = 4
        Me.TrackTitleLbl.TimeBeforStart = 1000
        '
        'TrackInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DimGray
        Me.ClientSize = New System.Drawing.Size(270, 105)
        Me.Controls.Add(Me.ArtistLbl)
        Me.Controls.Add(Me.AlbumLbl)
        Me.Controls.Add(Me.TrackTitleLbl)
        Me.Controls.Add(Me.AlbumArtBox)
        Me.DoubleBuffered = True
        Me.Name = "TrackInfo"
        Me.ShowInTaskbar = False
        Me.Text = "TrackInfo"
        Me.TopMost = True
        CType(Me.AlbumArtBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AlbumArtBox As System.Windows.Forms.PictureBox
    Friend WithEvents OpacityTimer As System.Windows.Forms.Timer
    Friend WithEvents TimeVisibleTimer As System.Windows.Forms.Timer
    Friend WithEvents TrackTitleLbl As SpotifyControl.LabelVer2
    Friend WithEvents ArtistLbl As SpotifyControl.LabelVer2
    Friend WithEvents AlbumLbl As SpotifyControl.LabelVer2
End Class
