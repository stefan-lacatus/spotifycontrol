Imports System.Runtime.InteropServices

Public Class ControllerSpotify : Implements IController
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

    Public ReadOnly Property Name() As String Implements IController.Name
        Get
            Return "Spotify"
        End Get
    End Property
    Public ReadOnly Property Author() As String Implements IController.Author
        Get
            Return "CyberWolf08"
        End Get
    End Property

    Private SpotifyProcess As Process
    Private SpotifyHandle As IntPtr = IntPtr.Zero
    Private refreshTimer As Timer
    Private Const RefreshRate = 1000

    Public Event TrackStateChanged(ByVal Title As String, ByVal state As IController.StateType) Implements IController.TrackStateChanged

    Public Property Active() As Boolean Implements IController.Active

    Private _SpotifyState As IController.StateType = IController.StateType.Closed
    Public Property SpotifyState() As IController.StateType Implements IController.State
        Get
            Return _SpotifyState
        End Get
        Set(ByVal value As IController.StateType)
            If value <> _SpotifyState Then
                _SpotifyState = value
                OnTrackStateChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnTrackStateChanged(ByVal e As EventArgs)
        RaiseEvent TrackStateChanged(TitleCache, _SpotifyState)
    End Sub

    Public ReadOnly Property TrackTitle() As String Implements IController.TrackTitle
        Get
            Return GetTrackTitle()
        End Get
    End Property

    Public ReadOnly Property TrackArtist() As String Implements IController.TrackArtist
        Get
            Return GetTrackArtist()
        End Get
    End Property

    Public ReadOnly Property TrackAlbum() As String Implements IController.TrackAlbum
        Get
            ' we don't know the album name. Retrun nothing.
            Return ""
        End Get
    End Property

    Public Sub New()
        LoadMe()
        refreshTimer = New Timer
        refreshTimer.Interval = RefreshRate
        AddHandler refreshTimer.Tick, AddressOf RefreshController
        refreshTimer.Start()
    End Sub
    Public Sub LoadMe() Implements IController.LoadMe
        Dim auxProcess() As Process = Process.GetProcessesByName("spotify")
        If auxProcess.Length > 0 Then
            SpotifyProcess = New Process
            SpotifyProcess = auxProcess(0)
            SpotifyHandle = SpotifyProcess.MainWindowHandle
            Me.SpotifyState = IController.StateType.Paused
        End If
    End Sub
    Private Sub RefreshController()
        'if winamp  has been closed then wait for it to be opened again
        If Me.SpotifyState = IController.StateType.Closed Then
            LoadMe()
        End If
        ' Try
        GetNowplaying(False)
        ' Catch ex As Exception
        'MsgBox(ex.Message)
        '  End Try
        '   Debug.Print(CurrentController.SpotifyState)

    End Sub
    Public Sub Mute() Implements IController.Mute
        If _SpotifyState And IController.StateType.Running Then
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
    Public Sub PlayPause() Implements IController.PlayPause
        If _SpotifyState And IController.StateType.Running Then
            PostMessage(SpotifyHandle, &H319, IntPtr.Zero, New IntPtr(&HE0000L))
        End If
    End Sub
    Public Sub PlayPrev() Implements IController.PlayPrev
        If _SpotifyState And IController.StateType.Running Then
            PostMessage(SpotifyHandle, &H319, IntPtr.Zero, New IntPtr(&HC0000L))
        End If
    End Sub
    Public Sub PlayNext() Implements IController.PlayNext
        If _SpotifyState And IController.StateType.Running Then
            PostMessage(SpotifyHandle, &H319, IntPtr.Zero, New IntPtr(&HB0000L))
        End If
    End Sub
    Public Sub VolumeUp() Implements IController.VolumeUp
        If _SpotifyState And IController.StateType.Running Then
            ' this will press the ctrl key then send a the KeyUP to the spotifyHandle
            keybd_event(Keys.ControlKey, &H1D, 0, 0)
            PostMessage(SpotifyHandle, &H100, Keys.Up, 0)
            ' wait a little
            Threading.Thread.Sleep(100)
            ' release the ctrlkey
            keybd_event(Keys.ControlKey, &H1D, &H2S, 0)
        End If
    End Sub
    Public Sub VolumeDown() Implements IController.VolumeDown
        If _SpotifyState And IController.StateType.Running Then
            ' this will press the ctrl key then send a the KeyDown to the spotifyHandle
            keybd_event(Keys.ControlKey, &H1D, 0, 0)
            PostMessage(SpotifyHandle, &H100, Keys.Down, 0)
            ' wait a little
            Threading.Thread.Sleep(100)
            ' release the ctrlkey
            keybd_event(Keys.ControlKey, &H1D, &H2S, 0)
        End If
    End Sub
    Public Sub BringToTop() Implements IController.BringToTop
        If _SpotifyState And IController.StateType.Running Then
            ShowWindow(SpotifyHandle, 1)
            SetForegroundWindow(SpotifyHandle)
            SetFocus(SpotifyHandle)
        End If
    End Sub
    Private TitleCache As String
    Public Function GetNowplaying(ByVal forced As Boolean) As String Implements IController.GetNowplaying
        If forced = True Then
            ' must return something
            RaiseEvent TrackStateChanged(TitleCache, _SpotifyState)
            Return TitleCache
        End If
        Dim lpText As String
        lpText = New String(Chr(0), 100)
        Dim intLength As Integer = GetWindowText(SpotifyHandle, lpText, lpText.Length)
        If (intLength <= 0) OrElse (intLength > lpText.Length) Then
            TitleCache = "Spotify Closed"
            If SpotifyProcess IsNot Nothing Then
                SpotifyProcess = Nothing
                SpotifyHandle = Nothing
                Me.SpotifyState = IController.StateType.Closed
            End If
            Return TitleCache
        End If
        Dim strTitle As String = lpText.Substring(0, intLength)
        strTitle = Mid(strTitle, 11)
        If strTitle.Length > 0 Then
            Me.SpotifyState = IController.StateType.Playing
            If strTitle <> TitleCache Then
                ' the song has changed
                TitleCache = strTitle
                RaiseEvent TrackStateChanged(TitleCache, _SpotifyState)
            End If
            Return strTitle
        Else
            Me.SpotifyState = IController.StateType.Paused
            If (TitleCache = "") Then
                TitleCache = "Nothing Playing"
            End If
            Return TitleCache
        End If
    End Function


    Private Function GetTrackTitle() As String
        ' this function returns the track title of the NotPlaying Song
        Try
            Dim ArtistTrack As String = GetNowplaying(False)
            Dim Track As String
            Track = ArtistTrack.Substring(InStr(ArtistTrack, " – ") + 2, ArtistTrack.Count - InStr(ArtistTrack, " – ") - 2)
            Return Track
        Catch ex As System.ArgumentOutOfRangeException
            Return ""
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Private Function GetTrackArtist() As String
        ' this function retuns the artist of the NowPlaying Song
        Try
            Dim ArtistTrack As String = GetNowplaying(False)
            Dim Artist As String
            Artist = ArtistTrack.Substring(0, InStr(ArtistTrack, " – ") - 1)
            Return Artist
        Catch ex As System.ArgumentOutOfRangeException
            Return ""
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class


