Imports System.Xml

Public Class MetadataAPI
    Dim TrackXML As New XmlDocument
    Public Sub New(ByVal ArtistName As String, ByVal TrackName As String)
        Try
            Dim TrackUrl As String
            ' TrackUrl has the following syntax http://ws.spotify.com/search/1/track?q=ArtistName&TrackName
            TrackUrl = "http://ws.spotify.com/search/1/track?q=" & ArtistName & " " & TrackName
            TrackXML.Load(Tools.DownloadFile(TrackUrl))
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    ' gets the artist spotify URI
    Public Function GetArtistURL() As String
        Try
            Return TrackXML.GetElementsByTagName("artist")(0).Attributes(0).InnerText
        Catch ex As Exception
            Return "Link Not Found"
        End Try
    End Function
    ' gets the album spotify URI
    Public Function GetAlbumURL() As String
        Try
            Return TrackXML.GetElementsByTagName("album")(0).Attributes(0).InnerText
        Catch ex As Exception
            Return "Link Not Found"
        End Try
    End Function
    ' gets the album name and the release year
    Public Function GetAlbumName() As String
        Try
            Dim result As String
            result = TrackXML.GetElementsByTagName("album")(0).ChildNodes(0).InnerText
            result = result & " (" & TrackXML.GetElementsByTagName("album")(0).ChildNodes(1).InnerText & ")"
            Return result
        Catch ex As Exception
            Return "Album Name Not Found"
        End Try
    End Function
    ' gets the track length(in Minutes:Seconds format)
    Public Function GetTrackLength() As Integer
        Try
            Dim TotalTime As Integer
            TotalTime = TrackXML.GetElementsByTagName("length")(0).InnerText
            Return TotalTime
        Catch ex As Exception
            Return 0
        End Try
    End Function
End Class
