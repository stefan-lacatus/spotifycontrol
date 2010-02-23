' based on someone's else work(80% of it). Don't remember where got it from, but credit goes to the original author


Public Class ControllerClass

    ' import all the needed shit in here
    Private Declare Auto Function FindWindow Lib "user32" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Auto Function SendMessage Lib "user32" (ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    Private Declare Auto Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean
    Private Declare Auto Function keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer) As Boolean
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Private Declare Auto Function GetWindowText Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String, ByVal cch As IntPtr) As IntPtr
    Private Declare Auto Function SetWindowText Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String) As Boolean
    Private Declare Auto Function EnumChildWindows Lib "user32" (ByVal hWndParent As Long, ByVal lpEnumFunc As Long, ByVal lParam As Long) As Long
    Private Declare Function ShowWindow Lib "user32" (ByVal hWnd As System.IntPtr, ByVal nCmdShow As Long) As Long

    ' declare the global hotkeys
    Private Const WM_KEYDOWN = &H100
    Private Const WM_KEYUP = &H101
    Private Const WM_MOUSEACTIVATE = &H21
    Private Const KEYEVENTF_EXTENDEDKEY As Integer = &H1S
    Private Const KEYEVENTF_KEYUP As Integer = &H2S


    Private w As Integer

    Public Sub New()
        ' get the spotify window
        w = FindWindow("SpotifyMainWindow", vbNullString)
    End Sub
    Public Function BringToTop() As Boolean
        ShowWindow(w, 9)
        Return 0
    End Function
    Public Function PlayPause() As Boolean
        SendMessage(w, WM_KEYDOWN, Keys.Space, 0)
        SendMessage(w, WM_KEYUP, Keys.Space, 0)
        Return 0
    End Function

    Public Function PlayPrev() As Boolean
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        Return 0
    End Function

    Public Function PlayNext() As Boolean
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        Return 0
    End Function

    Public Function VolumeUp() As Boolean
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        Return 0
    End Function

    Public Function Mute() As Boolean
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.ShiftKey, &H1D, 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100)
        keybd_event(Keys.ShiftKey, &H1D, KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        Return 0
    End Function

    Public Function VolumeDown() As Boolean
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        Return 0
    End Function

    Public Function GetNowplaying() As String
        Dim lpText As String
        lpText = New String(Chr(0), 100)
        Dim intLength As Integer = GetWindowText(w, lpText, lpText.Length)
        If (intLength <= 0) OrElse (intLength > lpText.Length) Then Return "Unknown"
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

    Public Function Search(ByVal s As String, ByVal AndPlay As Boolean) As Boolean
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.L, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.L, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SendKeys.SendWait(s & Chr(13))
        If AndPlay Then
            Sleep(100)
            SendKeys.SendWait(Chr(9) & Chr(9) & Chr(13))
        End If
        Return 0
    End Function
End Class

