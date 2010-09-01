Public Class LyricsForm
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
        Downloader.WorkerSupportsCancellation = True
    End Sub
    Public Sub LoadMe()
        IsVisible = True
        ArtistName = MainForm.CurrentTrack.ArtistName
        TrackName = MainForm.CurrentTrack.TrackName
        Me.Text = String.Format("Lyrics for  {0} - {1}", ArtistName, TrackName)
        Me.Show()
        Downloader.CancelAsync()
        DwlBtn_Click()
    End Sub

    Private Sub DwlBtn_Click() Handles DwlBtn.Click
        LyricBox.Text = "Please wait. Downloading Lyrics..."
        If LyricProvider.SelectedIndex = 0 Then
            Source = "ChartLyr"
        ElseIf LyricProvider.SelectedIndex = 1 Then
            Source = "LyrDB"
        End If
        If Not Downloader.IsBusy Then
            Downloader.RunWorkerAsync()
        End If
    End Sub
    Private Sub DownloadLyrics() Handles Downloader.DoWork
        If Source = "ChartLyr" Then
            AuxString = ChartLyricsAPI.GetLyrics(ArtistName, TrackName)
        ElseIf Source = "LyrDB" Then
            AuxString = LyrDB_Api.FindLyr(TrackName, ArtistName)
        End If
    End Sub
    Private Sub FinishedDownload() Handles Downloader.RunWorkerCompleted
        LyricBox.Text = AuxString
    End Sub

    Private Sub OnTopChkBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnTopChkBox.CheckedChanged
        ' set the lyrics box to be on top or not
        Me.TopMost = OnTopChkBox.Checked
    End Sub
End Class