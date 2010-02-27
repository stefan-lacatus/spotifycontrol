' credits go to Eduardo Oliveira from codeproject.com
Public Class AutoUpdate

    Public Function AutoUpdate(ByRef CommandLine As String, ByVal RemotePath As String) As Boolean
        Dim Key As String = "&**#@!" ' any unique sequence of characters
        ' the file with the update information
        Dim sfile As String = "update.dat"
        ' the Assembly name 
        Dim AssemblyName As String = _
                System.Reflection.Assembly.GetEntryAssembly.GetName.Name
        ' where are the files for a specific system
        Dim RemoteUri As String = RemotePath & AssemblyName & "/"
        ' clean up the command line getting rid of the key
        CommandLine = Replace(Microsoft.VisualBasic.Command(), Key, "")
        Try
            ' try to delete the AutoUpdate program, 
            ' since it is not needed anymore
            System.IO.File.Delete(Application.StartupPath & "\autoupdate.exe")
        Catch ex As Exception
        End Try
        ' Verify if was called by the autoupdate
        If InStr(Microsoft.VisualBasic.Command(), Key) > 0 Then
            Try
                ' try to delete the AutoUpdate program, 
                ' since it is not needed anymore
                System.IO.File.Delete(Application.StartupPath & "\autoupdate.exe")
            Catch ex As Exception
            End Try
            ' return false means that no update is needed
            Return False
        Else
            ' was called by the user
            Dim ret As Boolean = False ' Default - no update needed
            '   Try
            Dim myWebClient As New System.Net.WebClient 'the webclient
            ' Download the update info file to the memory, 
            ' read and close the stream
            Dim file As New System.IO.StreamReader(myWebClient.OpenRead(RemoteUri & sfile))
            Dim Contents As String = file.ReadToEnd()
            file.Close()
            ' if something was read
            If Contents <> "" Then
                ' Break the contents 
                Dim x() As String = Split(Contents, "|")
                ' the first parameter is the version. if it's 
                ' greater then the current version starts the 
                ' update process
                If x(0) > Application.ProductVersion Then
                    If MessageBox.Show("A new version has been found. Update?", "SpotifyControl", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                        Return ret
                        Exit Function
                    End If
                    ' assembly the parameter to be passed to the auto 
                    ' update program
                    ' x(1) is the files that need to be 
                    ' updated separated by "?"
                    Dim arg As String = Application.ExecutablePath & "|" & _
                                RemoteUri & "|" & x(1) & "|" & Key & "|" & _
                                Microsoft.VisualBasic.Command()
                    ' Download the auto update program to the application 
                    ' path, so you always have the last version runing
                    My.Computer.Network.DownloadFile(RemotePath & "autoupdate.exe", _
                       Application.StartupPath & "\autoupdate.exe")
                    ' Call the auto update program with all the parameters
                    System.Diagnostics.Process.Start( _
                        Application.StartupPath & "\autoupdate.exe", arg)
                    ' return true - auto update in progress
                    ret = True
                End If
            End If
            '   Catch ex As Exception
            ' if there is an error return true, 
            ' what means that the application
            ' should be closed
            ' ret = True
            ' something went wrong... 
            ' MsgBox("There was a problem runing the Auto Update." & vbCr & _
            '  "Please Contact cyberwolf008@gmail.com" & vbCr & ex.Message, _
            '  MsgBoxStyle.Critical)
            ' End Try
            Return ret
        End If
    End Function

End Class
