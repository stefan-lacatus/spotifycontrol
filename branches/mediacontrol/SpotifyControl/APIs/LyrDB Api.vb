' check here http://www.lyrdb.com/services/lws-tech.php for information about the requests 
Public Module LyrDB_Api
    Dim request As New Net.WebClient
    Public Function FindLyr(ByVal TrackName As String, ByVal ArtistName As String) As String
        Dim MyUrl, stringResponse, LyrID As String
        ' the link will have the following syntax
        ' example: "http://webservices.lyrdb.com/lookup.php?q=ArtistName|TrackName&for=match&agent=agent"
        MyUrl = String.Format("http://webservices.lyrdb.com/lookup.php?q={0}|{1}&for=match&agent=SpotifyControl", ArtistName, TrackName)
        Try
            stringResponse = request.DownloadString(MyUrl)
            'request.Dispose()
            LyrID = stringResponse.Substring(0, InStr(stringResponse, "\") - 1)
        Catch
            LyrID = "Not Found"
        End Try
        ' the link will have the following syntax http://www.lyrdb.com/getlyr.php?q=LyrID
        myUrl = "http://www.lyrdb.com/getlyr.php?q=" & LyrID
        Try
            stringResponse = request.DownloadString(myUrl)
        Catch ex As Exception
            stringResponse = "Not Found"
        End Try
        Return stringResponse
    End Function
End Module
