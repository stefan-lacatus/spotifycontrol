Imports System.Xml


Public Class LastFmApi
    Public TrackXML As New XmlDocument
    Public Sub New(ByVal ArtistName As String, ByVal TrackName As String, ByVal ApiKey As String)
        'this will get the XML of the track
        Dim XmlUrl As String = String.Format("http://ws.audioscrobbler.com/2.0/?method=track.getinfo&api_key={0}&artist={1}&track={2}", _
                                             ApiKey, ArtistName, TrackName)
        TrackXML.Load(Tools.DownloadFile(XmlUrl))
    End Sub
    Public Function GetAlbumOfTrack() As String
        ' find the album name in the XML
        Dim a As XmlNodeList
        a = TrackXML.GetElementsByTagName("title")
        Return a(0).InnerText
    End Function
    Public Function GetAlbumArt() As String
        ' Find the album art url in the XML
        Dim a As XmlNodeList
        a = TrackXML.GetElementsByTagName("image")
        '  MsgBox(a(1).InnerText)
        Return a(1).InnerText
    End Function
End Class
