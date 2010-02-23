Public Class TrackInfo
    Dim increaseOpacity As Boolean ' this will remember if the form is going visible or invisible
    Dim MaxOpacity, TimeVisible As Integer
    Dim MyLastFmApi As LastFmApi
    Private Sub TrackInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TimeVisible = 2000
        MaxOpacity = 100
        TimeVisibleTimer.Interval = TimeVisible
        ' set the opacity to 0. This will make the form invisible
        Me.Opacity = 0
        ' make the window borderless. Looks better this way
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        ' Activate the timer that will make the opacity vary
        OpacityTimer.Enabled = True
        increaseOpacity = True
        TrackTitleLbl.Text = SpotifyController.MySpotify.GetTrackTitle
        ArtistLbl.Text = SpotifyController.MySpotify.GetTrackArtist
        MyLastFmApi = New LastFmApi(ArtistLbl.Text, TrackTitleLbl.Text, "12bd97e8e4d2b71db9edc62d7a7b65cd")
        AlbumLbl.Text = MyLastFmApi.GetAlbumOfTrack
        LoadWebImageToPictureBox(AlbumArtBox, MyLastFmApi.GetAlbumArt)
    End Sub

    Private Sub OpacityTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpacityTimer.Tick
        If increaseOpacity = True Then
            If Me.Opacity < MaxOpacity / 100 Then
                Me.Opacity = Me.Opacity + 5 / 100 ' increase the opacity by 5%
            Else
                OpacityTimer.Enabled = False
                ' the application will now stay visible for 5 sec
                System.Threading.Thread.Sleep(TimeVisible)
                increaseOpacity = False ' the opacity will decrease
                OpacityTimer.Enabled = True
            End If
        Else
            If Me.Opacity > 0 Then
                Me.Opacity = Me.Opacity - 5 / 100 ' decrease the opacity by 5%
            Else
                OpacityTimer.Enabled = False
                Me.Close() 'close the form
            End If
        End If
    End Sub

    Private Sub TimeVisibleTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeVisibleTimer.Tick
        ' the form will now begin to decrease in opacity
        TimeVisibleTimer.Enabled = False '  the form has been visible for 5s.
        increaseOpacity = False ' the opacity will decrease
        OpacityTimer.Enabled = True
    End Sub

    Private Sub TrackInfo_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        ' if the mouse enters the form it will get 20% of MaxOpacity and the timer will be stopped until the mouse leaves the form
        Me.Opacity = 0.2 * (MaxOpacity / 100)
        OpacityTimer.Enabled = False
        TimeVisibleTimer.Enabled = False
    End Sub

    Private Sub TrackInfo_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        ' the application will remain visible 1s after the mouse leaves the form
        TimeVisibleTimer.Interval = 1000
        TimeVisibleTimer.Enabled = True
    End Sub
    Public Function LoadWebImageToPictureBox(ByVal pb As PictureBox, ByVal ImageURL As String) As Boolean
        Dim objImage As IO.MemoryStream
        Dim objwebClient As Net.WebClient
        Dim sURL As String = Trim(ImageURL)
        Dim bAns As Boolean
        Try
            objwebClient = New Net.WebClient
            objImage = New IO.MemoryStream(objwebClient.DownloadData(sURL))
            pb.Image = Image.FromStream(objImage)
            bAns = True
        Catch ex As Exception
            bAns = False
        End Try
        Return bAns
    End Function
End Class