' More info here http://www.chartlyrics.com/api.aspx
Imports System.Xml
Public Module ChartLyricsAPI
    Dim TrackXML As New XmlDocument
    Public Function GetLyrics(ByVal ArtistName As String, ByVal TrackName As String) As String
        Try
            Try
                Dim TrackUrl As String
                ' TrackUrl has the following syntax http://api.chartlyrics.com/apiv1.asmx/SearchLyric?artist=ArtistName&song=TrackName
                TrackUrl = String.Format("http://api.chartlyrics.com/apiv1.asmx/SearchLyric?artist={0}&song={1}", ArtistName, TrackName)
                TrackXML.Load(Tools.DownloadFile(TrackUrl))
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try
            Dim LyricXML As New XmlDocument
            Dim LyricURL, LyricID, LyricChkSum As String
            Dim Results As XmlNodeList
            Dim node As XmlNode
            Dim returnValue As String
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
            LyricURL = String.Format("http://api.chartlyrics.com/apiv1.asmx/GetLyric?lyricId={0}&lyricCheckSum={1}", LyricID, LyricChkSum)
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
            returnValue = LyricXML.GetElementsByTagName("Lyric")(0).InnerText & vbNewLine & vbNewLine & "From " & GetWebURL()
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
End Module
