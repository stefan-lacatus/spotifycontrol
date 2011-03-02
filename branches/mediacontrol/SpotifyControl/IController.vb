Public Interface IController
    Enum StateType
        Closed = 0
        Playing = 1
        Paused = 2
        Running = Playing Or Paused
    End Enum
    ReadOnly Property Name() As String
    ReadOnly Property Author() As String
    Property State() As StateType
    Property Active() As Boolean
    ReadOnly Property TrackTitle() As String
    ReadOnly Property TrackArtist() As String
    ReadOnly Property TrackAlbum() As String
    Sub LoadMe()
    Sub PlayPause()
    Sub PlayNext()
    Sub PlayPrev()
    Sub VolumeUp()
    Sub VolumeDown()
    Sub BringToTop()
    Sub Mute()
    Function GetNowplaying(ByVal forced As Boolean) As String
    Event TrackStateChanged(ByVal Title As String, ByVal state As StateType)
End Interface
