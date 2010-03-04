Imports System.Runtime.InteropServices

Public Class ControllerClass
#Region "Function Imports"
    <DllImport("user32.dll")> _
    Friend Shared Function PostMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function
    <DllImport("user32.dll")> _
    Friend Shared Function SetFocus(ByVal hWnd As IntPtr)
    End Function
    <DllImport("user32.dll")> _
    Friend Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    <DllImport("user32.dll")> _
    Friend Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function
    <DllImport("user32", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Friend Shared Function GetWindowText(ByVal hWnd As IntPtr, <Out(), MarshalAs(UnmanagedType.LPTStr)> ByVal lpString As String, ByVal nMaxCount As Integer) As Integer
    End Function
    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer) As Boolean
    End Function
#End Region


    Private SpotifyProcess As New Process
    Private SpotifyHandle As IntPtr = IntPtr.Zero
    Private _SpotifyState As String = "Closed"
    Public Property SpotifyState() As String
        Get
            Return _SpotifyState
        End Get
        Set(ByVal value As String)
            _SpotifyState = value
        End Set
    End Property
    Public Sub New()
        LoadMe()
    End Sub
    Public Sub LoadMe()
        Dim auxProcess() As Process = Process.GetProcessesByName("spotify")
        If auxProcess.Length > 0 Then
            SpotifyProcess = auxProcess(0)
            Me.SpotifyState = "Running"
            SpotifyHandle = SpotifyProcess.MainWindowHandle
        End If
    End Sub
    Public Sub Mute()
        If Me.SpotifyState = "Running" Then
            ' this will press the ctrl key and the shift key then send a the KeyDown to the spotifyHandle
            keybd_event(Keys.ControlKey, &H1D, 0, 0)
            keybd_event(Keys.ShiftKey, &H1D, 0, 0)
            PostMessage(SpotifyHandle, &H100, Keys.Down, 0)
            ' wait a little
            Threading.Thread.Sleep(100)
            ' release the ctrlkey and shift key
            keybd_event(Keys.ControlKey, &H1D, &H2S, 0)
            keybd_event(Keys.ShiftKey, &H1D, &H2S, 0)
        End If
    End Sub
    Public Sub PlayPause()
        If Me.SpotifyState = "Running" Then
            PostMessage(SpotifyHandle, &H319, IntPtr.Zero, New IntPtr(&HE0000L))
        End If
    End Sub
    Public Sub PlayPrev()
        If Me.SpotifyState = "Running" Then
            PostMessage(SpotifyHandle, &H319, IntPtr.Zero, New IntPtr(&HC0000L))
        End If
    End Sub
    Public Sub PlayNext()
        If Me.SpotifyState = "Running" Then
            PostMessage(SpotifyHandle, &H319, IntPtr.Zero, New IntPtr(&HB0000L))
        End If
    End Sub
    Public Sub VolumeUp()
        If Me.SpotifyState = "Running" Then
            ' this will press the ctrl key then send a the KeyUP to the spotifyHandle
            keybd_event(Keys.ControlKey, &H1D, 0, 0)
            PostMessage(SpotifyHandle, &H100, Keys.Up, 0)
            ' wait a little
            Threading.Thread.Sleep(100)
            ' release the ctrlkey
            keybd_event(Keys.ControlKey, &H1D, &H2S, 0)
        End If
    End Sub
    Public Sub VolumeDown()
        If Me.SpotifyState = "Running" Then
            ' this will press the ctrl key then send a the KeyDown to the spotifyHandle
            keybd_event(Keys.ControlKey, &H1D, 0, 0)
            PostMessage(SpotifyHandle, &H100, Keys.Down, 0)
            ' wait a little
            Threading.Thread.Sleep(100)
            ' release the ctrlkey
            keybd_event(Keys.ControlKey, &H1D, &H2S, 0)
        End If
    End Sub
    Public Sub BringToTop()
        If Me.SpotifyState = "Running" Then
            ShowWindow(SpotifyHandle, 1)
            SetForegroundWindow(SpotifyHandle)
            SetFocus(SpotifyHandle)
        End If
    End Sub
    Public Function GetNowplaying() As String
        Dim lpText As String
        lpText = New String(Chr(0), 100)
        Dim intLength As Integer = GetWindowText(SpotifyHandle, lpText, lpText.Length)
        If (intLength <= 0) OrElse (intLength > lpText.Length) Then
            If SpotifyProcess IsNot Nothing Then
                SpotifyProcess = Nothing
                SpotifyHandle = Nothing
                _SpotifyState = "Closed"
            End If
            Return "Spotify Closed"
        End If

        Dim strTitle As String = lpText.Substring(0, intLength)
        strTitle = Mid(strTitle, 11)
        If strTitle.Length > 0 Then
            Return strTitle
        Else
            Return "Nothing Playing"
        End If
    End Function

    Public Function GetTrackTitle() As String
        ' this function returns the track title of the NotPlaying Song
        Dim ArtistTrack As String = GetNowplaying()
        Dim Track As String
        Track = ArtistTrack.Substring(InStr(ArtistTrack, " – ") + 2, ArtistTrack.Count - InStr(ArtistTrack, " – ") - 2)
        Return Track
    End Function

    Public Function GetTrackArtist() As String
        ' this function retuns the artist of the NowPlaying Song
        Dim ArtistTrack As String = GetNowplaying()
        Dim Artist As String
        Artist = ArtistTrack.Substring(0, InStr(ArtistTrack, " – ") - 1)
        Return Artist
    End Function
End Class
