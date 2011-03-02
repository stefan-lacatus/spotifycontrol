Public Class Track

    Private _TrackName As String
    Public Property TrackName() As String
        Get
            Return _TrackName
        End Get
        Set(ByVal value As String)
            _TrackName = value
        End Set
    End Property

    Private _ArtistName As String
    Public Property ArtistName() As String
        Get
            Return _ArtistName
        End Get
        Set(ByVal value As String)
            _ArtistName = value
        End Set
    End Property

    Private _CoverUrl As String
    Public Property CoverURL() As String
        Get
            Return _CoverUrl
        End Get
        Set(ByVal value As String)
            _CoverUrl = value
        End Set
    End Property

    Private _AlbumName As String
    Public Property AlbumName() As String
        Get
            Return _AlbumName
        End Get
        Set(ByVal value As String)
            _AlbumName = value
        End Set
    End Property
    Private _TrackLength As String
    Public Property TrackLength() As String
        Get
            Return _TrackLength
        End Get
        Set(ByVal value As String)
            _TrackLength = value
        End Set
    End Property


End Class