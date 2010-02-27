Public Class LyricsForm
    Dim myChartLyrics As ChartLyricsAPI
    Dim mylyrDB As LyrDB_Api
    Dim ArtistName, TrackName As String
    Private Sub LyricsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
        LyricProvider.SelectedIndex = 0
        LyricProvider.DropDownStyle = ComboBoxStyle.DropDownList
        ArtistName = SpotifyController.MySpotify.GetTrackArtist
        TrackName = SpotifyController.MySpotify.GetTrackTitle
        Me.Text = "Lyrics for  " & ArtistName & " - " & TrackName
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If LyricProvider.SelectedIndex = 0 Then
            myChartLyrics = New ChartLyricsAPI(ArtistName, TrackName)
            LyricBox.Text = myChartLyrics.GetLyrics & vbNewLine & "From" & myChartLyrics.GetWebURL
        ElseIf LyricProvider.SelectedIndex = 1 Then
            mylyrDB = New LyrDB_Api()
            Dim aux As String
            aux = mylyrDB.FindLyrID(TrackName, ArtistName)
            LyricBox.Text = mylyrDB.FindLyr(aux)
        End If
    End Sub
End Class