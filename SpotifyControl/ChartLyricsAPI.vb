' More info here http://www.chartlyrics.com/api.aspx
Imports System.Xml
Public Class ChartLyricsAPI
    Dim TrackXML As New XmlDocument
    Public Sub New(ByVal ArtistName As String, ByVal TrackName As String)
        Try
            Dim TrackUrl As String
            ' TrackUrl has the following syntax http://api.chartlyrics.com/apiv1.asmx/SearchLyric?artist=ArtistName&song=TrackName
            TrackUrl = "http://api.chartlyrics.com/apiv1.asmx/SearchLyric?artist=" & ArtistName & "&song=" & TrackName
            Dim request As Net.HttpWebRequest
            Dim response As Net.HttpWebResponse
            request = Net.HttpWebRequest.Create(TrackUrl)
            Threading.Thread.Sleep(1 * 1000)
            request.Method = "GET"
            request.UserAgent = "SpotifyControl"
            request.Timeout = Threading.Timeout.Infinite
            request.KeepAlive = False
            request.ProtocolVersion = Net.HttpVersion.Version10
            request.ContentType = "text/xml"
            response = request.GetResponse
            TrackXML.Load(response.GetResponseStream)
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    Public Function GetLyrics() As String
        Try
            Dim LyricXML As New XmlDocument
            Dim LyricURL, LyricID, LyricChkSum As String
            Dim Results As XmlNodeList
            Dim node As XmlNode
            LyricID = 0
            LyricChkSum = 0
            Results = TrackXML.GetElementsByTagName("LyricId")
            For index = 0 To Results.Count - 1
                node = Results(index)
                If node.InnerText <> 0 Then
                    LyricID = node.InnerText
                    LyricChkSum = TrackXML.GetElementsByTagName("LyricChecksum")(index).InnerText
                    Exit For
                End If
            Next
            LyricURL = "http://api.chartlyrics.com/apiv1.asmx/GetLyric?lyricId=" & LyricID & "&lyricCheckSum=" & LyricChkSum
            Dim request1 As Net.HttpWebRequest
            Dim response As Net.HttpWebResponse
            request1 = Net.HttpWebRequest.Create(LyricURL)
            Threading.Thread.Sleep(1 * 1000)
            request1.Method = "GET"
            request1.UserAgent = "SpotifyControl"
            request1.Timeout = Threading.Timeout.Infinite
            request1.KeepAlive = False
            request1.ProtocolVersion = Net.HttpVersion.Version10
            request1.ContentType = "text/xml"
            response = request1.GetResponse
            LyricXML.Load(response.GetResponseStream)
            Return LyricXML.GetElementsByTagName("Lyric")(0).InnerText
        Catch ex As Exception
            ' MsgBox(ex.Message)
            Return "Not Found"
        End Try
    End Function
    Public Function GetWebURL() As String
        Try
            Dim LyricURL As String = "Not Found"
            Dim Results As XmlNodeList
            Dim node As XmlNode
            Results = TrackXML.GetElementsByTagName("LyricId")
            For index = 0 To Results.Count - 1
                node = Results(index)
                If node.InnerText <> 0 Then
                    LyricURL = TrackXML.GetElementsByTagName("SongUrl")(index).InnerText
                    Exit For
                End If
            Next
            Return LyricURL
        Catch ex As Exception
            Return "Not Found"
        End Try
    End Function
End Class
