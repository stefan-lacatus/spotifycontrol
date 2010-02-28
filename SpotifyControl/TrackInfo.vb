Public Class TrackInfo
    Dim increaseOpacity As Boolean ' this will remember if the form is going visible or invisible
    Dim MaxOpacity, TimeVisible As Integer
    Private MyLastFmApi As LastFmApi
    Private MyMetaDataApi As MetadataAPI

    Public Sub LoadMe()
        Try
            TimeVisible = 4000
            MaxOpacity = 100
            TimeVisibleTimer.Interval = TimeVisible
            Me.Show()
            ResetControls()
            ' set the opacity to 0. This will make the form invisible
            Me.Opacity = 0
            ' make the window borderless. Looks better this way
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            ' Activate the timer that will make the opacity vary
            OpacityTimer.Enabled = True
            increaseOpacity = True
            TrackTitleLbl.Text = SpotifyController.MySpotify.GetTrackTitle
            ArtistLbl.Text = SpotifyController.MySpotify.GetTrackArtist
            ' MyLastFmApi = New LastFmApi(ArtistLbl.Text, TrackTitleLbl.Text, "12bd97e8e4d2b71db9edc62d7a7b65cd")
            'LoadWebImageToPictureBox(AlbumArtBox, MyLastFmApi.GetAlbumArt)
            MyMetaDataApi = New MetadataAPI(ArtistLbl.Text, TrackTitleLbl.Text)
            AlbumLbl.Text = MyMetaDataApi.GetAlbumName
            TrackTitleLbl.Text = TrackTitleLbl.Text & " (" & MyMetaDataApi.GetTrackLength & ")"

        Catch
        Finally
            ' measure the text so we can resize the window to fit the text and to look nice
            'TOTO: Doesn't work quite well, also we should make sure it doesn't go off-screan
            Dim g As Graphics = Me.CreateGraphics
            Dim textSize1, textSize2, textSize3 As SizeF
            ' measure the text
            textSize1 = g.MeasureString(ArtistLbl.Text, ArtistLbl.Font)
            textSize2 = g.MeasureString(TrackTitleLbl.Text, TrackTitleLbl.Font)
            textSize3 = g.MeasureString(AlbumLbl.Text, AlbumLbl.Font)
            ' get the maximum text size value and set the width of the form 
            Me.Width = ArtistLbl.Location.X + Math.Max(Math.Max(textSize1.Width, textSize2.Width), textSize3.Width) + 5
            ' this will position the form in the lower right area of the desktop
            Dim working_area As Rectangle = SystemInformation.WorkingArea
            Dim x As Integer = working_area.Left + working_area.Width - Me.Width
            Dim y As Integer = working_area.Top + working_area.Height - Me.Height
            Me.Location = New Point(x, y)
        End Try
    End Sub
    Private Sub ResetControls()
        ' this will reset all the controls in this form to the initials state
        TimeVisibleTimer.Enabled = False
        OpacityTimer.Enabled = False
        AlbumLbl.Text = "Album Not Found"
        ArtistLbl.Text = ""
        TrackTitleLbl.Text = ""
        AlbumArtBox.Image = Nothing
    End Sub
    Private Sub OpacityTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpacityTimer.Tick
        If increaseOpacity = True Then
            If Me.Opacity < MaxOpacity / 100 Then
                Me.Opacity = Me.Opacity + 5 / 100 ' increase the opacity by 5%
            Else
                OpacityTimer.Enabled = False
                ' the application will now stay visible for  sec
                TimeVisibleTimer.Enabled = True
             End If
        Else
            If Me.Opacity > 0 Then
                Me.Opacity = Me.Opacity - 5 / 100 ' decrease the opacity by 5%
            Else
                OpacityTimer.Enabled = False
                Me.Hide() 'close the form
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
        ' if the mouse enters the form it will get 90% of MaxOpacity and the timer will be stopped until the mouse leaves the form
        Me.Opacity = 0.9 * (MaxOpacity / 100)
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