Public Class SettingManager
    Public MyHotKeyManager(5) As HotKeyManager
    Public WithEvents cHTTPServer As HTTPServer
    Public Password As String
    Private Sub SettingManager_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        e.Cancel = True
    End Sub
    Private Sub SettingManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
        Dim DefaultValues() As String = New String() {"Play/Pause", "Next", "Previous", "Mute", "Volume Up", "Volume Down"}
        HotKeyTbl.RowCount = 6
        For index = 0 To 5
            HotKeyTbl.Item(0, index).Value = DefaultValues(index)
        Next
        Try
            ' the second column should be loaded from file. 
            For index = 0 To 5
                Select Case MyHotKeyManager(index).MainKeyModifier
                    Case 1
                        HotKeyTbl.Item(1, index).Value = "Alt + " & MyHotKeyManager(index).MainKey.ToString
                    Case 2
                        HotKeyTbl.Item(1, index).Value = "Ctrl + " & MyHotKeyManager(index).MainKey.ToString
                    Case 4
                        HotKeyTbl.Item(1, index).Value = "Shift + " & MyHotKeyManager(index).MainKey.ToString
                    Case 8
                        HotKeyTbl.Item(1, index).Value = "Win + " & MyHotKeyManager(index).MainKey.ToString
                    Case vbNull
                        HotKeyTbl.Item(1, index).Value = "None"
                End Select

            Next
        Catch
        End Try
    End Sub
    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles HotKeyTbl.DoubleClick
        ' show the capturekey dialog
        CaptureKey.ShowDialog()
    End Sub

    Private Sub SaveBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        SaveSettings()
        Me.Close()
    End Sub

    Private Sub CancelBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub SaveSettings()
        Dim SettingWriter As System.IO.StreamWriter
        Try
            ' if the file exists display a msgbox with options
            If System.IO.File.Exists(Application.StartupPath & "//Settings.ini") Then
                If MsgBox("A settings file already exists. Overwrite?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    System.IO.File.Delete(Application.StartupPath & "//Settings.ini")
                Else
                    Exit Try
                End If
            End If
            SettingWriter = System.IO.File.CreateText(Application.StartupPath & "//Settings.ini")
            ' write the global hotkeys to file
            For index = 0 To 5
                SettingWriter.WriteLine(MyHotKeyManager(index).MainKey)
                SettingWriter.WriteLine(MyHotKeyManager(index).MainKeyModifier)
            Next
            SettingWriter.WriteLine(Password)
            SettingWriter.Close()
        Catch ex As Exception
            SettingWriter.Close()
            ' MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub UpdateBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateBtn.Click
        Dim MyAutoUpdate As New AutoUpdate
        ' where the updates are downloaded from
        Dim RemotePath As String = "http://dl.dropbox.com/u/329033/SpotifyController/"
        If MyAutoUpdate.AutoUpdate(vbNullString, RemotePath) Then
            Dispose()
            Application.Exit()
        End If
    End Sub

    Private Sub StartSrvBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartSrvBox.Click
        cHTTPServer = New HTTPServer()
        ReDim cHTTPServer.HandledRequestList(7)
        cHTTPServer.HandledRequestList(0) = New String("/getnowplaying")
        cHTTPServer.HandledRequestList(1) = New String("/next")
        cHTTPServer.HandledRequestList(2) = New String("/prev")
        cHTTPServer.HandledRequestList(3) = New String("/playpause")
        cHTTPServer.HandledRequestList(4) = New String("/voldown")
        cHTTPServer.HandledRequestList(5) = New String("/volup")
        cHTTPServer.HandledRequestList(6) = New String("/mute")
        If PassBox.Text <> vbNullString Then
            If Password <> vbNullString And (Not System.IO.File.Exists(Application.StartupPath & "/html/test.html")) Then
                My.Computer.FileSystem.RenameFile(Application.StartupPath & "/html/" & getMD5Hash(Password) & ".html", getMD5Hash(PassBox.Text) & ".html")
            Else
                My.Computer.FileSystem.RenameFile(Application.StartupPath & "/html/test.html", getMD5Hash(PassBox.Text) & ".html")
            End If
        End If
        Password = PassBox.Text
        cHTTPServer.HandledRequestList(7) = New String("/" & Password)
        cHTTPServer.StartFile = Application.StartupPath & "/html/index.html"
        cHTTPServer.RootDirectory = Application.StartupPath & "/html"
        TestSrvBox.Enabled = True
        StartSrvBox.Enabled = False
        cHTTPServer.StartServer()

    End Sub

    Private Sub StopServerBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopServerBox.Click
        CloseServer()
    End Sub

    Private Sub TestSrvBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestSrvBox.Click
        Process.Start("http://127.0.0.1/test.html")
    End Sub

    Private Sub cHTTPServer_ON_HTTP_GET(ByVal sender As UserConnection, ByVal Data As HTTPServer.HTTPConnection) Handles cHTTPServer.ON_HTTP_GET
        'use the Data variable to see all the information attached to the connection
        'Like... Data.Request_Referer or Data.Request_Useragent 
        Select Case Data.Request_Filename
            Case "/getnowplaying"
                Threading.Thread.Sleep(300)
                Dim Response As String = "Track Name ||| Artist Name ||| Album Name ||| AlbumArt"
                Try
                    Response = MainForm.CurrentTrack.TrackName & "|||" & MainForm.CurrentTrack.ArtistName & _
                        "|||" & MainForm.CurrentTrack.AlbumName & "|||" & MainForm.CurrentTrack.CoverURL
                Catch
                End Try
                cHTTPServer.SendHTTPResponse(sender, Data, Response)
            Case "/next"
                MainForm.MySpotify.PlayNext()
                Threading.Thread.Sleep(300)
                cHTTPServer.SendHTTPResponse(sender, Data, "OK")
            Case "/prev"
                MainForm.MySpotify.PlayPrev()
                Threading.Thread.Sleep(300)
                cHTTPServer.SendHTTPResponse(sender, Data, "OK")
            Case "/playpause"
                MainForm.MySpotify.PlayPause()
                Threading.Thread.Sleep(300)
                cHTTPServer.SendHTTPResponse(sender, Data, "OK")
            Case "/mute"
                MainForm.MySpotify.Mute()
                Threading.Thread.Sleep(300)
                cHTTPServer.SendHTTPResponse(sender, Data, "OK")
            Case "/voldown"
                MainForm.MySpotify.VolumeDown()
                Threading.Thread.Sleep(300)
                cHTTPServer.SendHTTPResponse(sender, Data, "OK")
            Case "/volup"
                MainForm.MySpotify.VolumeUp()
                Threading.Thread.Sleep(300)
                cHTTPServer.SendHTTPResponse(sender, Data, "OK")
            Case Else
                If Data.Request_Filename = "/" & Password Then
                    Threading.Thread.Sleep(300)
                    cHTTPServer.SendHTTPResponse(sender, Data, "True|||" & getMD5Hash(Password) & ".html")
                End If

        End Select
    End Sub
    Public Sub CloseServer()
        If cHTTPServer Is Nothing Then
        Else
            cHTTPServer.StopServer()
            cHTTPServer = Nothing
            TestSrvBox.Enabled = False
            StartSrvBox.Enabled = True
        End If
    End Sub
    Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""

        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next

        Return strResult
    End Function
End Class
