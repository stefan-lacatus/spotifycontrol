Public Class LyricsForm
    Dim myChartLyrics As ChartLyricsAPI
    Dim mylyrDB As LyrDB_Api
    Dim ArtistName, TrackName, AuxString As String
    Public IsVisible As Boolean
    Dim Source As String
    Dim WithEvents Downloader As New System.ComponentModel.BackgroundWorker
    Private Sub LyricsForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        IsVisible = False
        Me.Hide()
        e.Cancel = True
    End Sub
    Private Sub LyricsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
        LyricProvider.SelectedIndex = 0
        LyricProvider.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub
    Public Sub LoadMe()
        IsVisible = True
        ArtistName = SpotifyController.MySpotify.GetTrackArtist
        TrackName = SpotifyController.MySpotify.GetTrackTitle
        Me.Text = "Lyrics for  " & ArtistName & " - " & TrackName
        Me.Show()
        DwlBtn_Click()
    End Sub

    Private Sub DwlBtn_Click() Handles DwlBtn.Click
        LyricBox.Text = "Please wait. Downloading Lyrics..."
        If LyricProvider.SelectedIndex = 0 Then
            Source = "ChartLyr"
        ElseIf LyricProvider.SelectedIndex = 1 Then
            Source = "LyrDB"
        End If
        Downloader.RunWorkerAsync()
    End Sub
    Private Sub DownloadLyrics() Handles Downloader.DoWork
        If Source = "ChartLyr" Then
            myChartLyrics = New ChartLyricsAPI(ArtistName, TrackName)
            AuxString = myChartLyrics.GetLyrics & vbNewLine & vbNewLine & "From " & myChartLyrics.GetWebURL
        ElseIf Source = "LyrDB" Then
            mylyrDB = New LyrDB_Api()
            Dim aux As String
            aux = mylyrDB.FindLyrID(TrackName, ArtistName)
            AuxString = mylyrDB.FindLyr(aux)
        End If
    End Sub
    Private Sub FinishedDownload() Handles Downloader.RunWorkerCompleted
        LyricBox.Text = AuxString
    End Sub
End Class