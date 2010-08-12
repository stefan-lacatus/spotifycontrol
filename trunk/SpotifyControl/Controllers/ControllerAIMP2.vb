Imports System.Runtime.InteropServices
Imports System.Runtime.ConstrainedExecution

Public Class ControllerAIMP2 : Implements IController, IDisposable
#Region "Function Imports"
    <DllImport("user32.dll")> _
    Friend Shared Function SetFocus(ByVal hWnd As IntPtr)
    End Function
    <DllImport("user32.dll")> _
    Friend Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function OpenFileMapping(ByVal dwDesiredAccess As Integer, <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandle As Boolean, ByVal lpName As String) As IntPtr
    End Function
    <DllImport("Kernel32", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function MapViewOfFile(ByVal hFileMapping As IntPtr, ByVal dwDesiredAccess As Integer, ByVal dwFileOffsetHigh As Integer, ByVal dwFileOffsetLow As Integer, ByVal dwNumberOfBytesToMap As Integer) As IntPtr
    End Function
    <ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("Kernel32", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function UnmapViewOfFile(ByVal pvBaseAddress As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, ExactSpelling:=True)> _
    Public Shared Function CloseHandle(ByVal hObject As IntPtr) As Integer
    End Function

#End Region

#Region "Constants"
    Const WM_USER = &H400
    Const WM_AIMP_COMMAND = WM_USER + &H75
    Const WM_AIMP_STATUS_GET = 1
    Const WM_AIMP_STATUS_SET = 2
    Const AIMP_STATUS_CHANGE = 1
    Const WM_AIMP_CALLFUNC = 3
    Const AIMP_STS_VOLUME = 1
    Const AIMP_STS_Player = 4
    Const AIMP_INFO_UPDATE = 5
    Const AIMP_TRACK_POS_CHANGED = 14
    Const AIMP_PLAY = 15
    Const AIMP_PAUSE = 16
    Const AIMP_STOP = 17
    Const AIMP_NEXT = 18
    Const AIMP_PREV = 19
    Const AIMP_STS_MUTE = 5
    Const AIMP2_RemoteFileSize = 2048
    Const FILE_MAP_READ As Int32 = &H4
    Const VolumeStep = 10
#End Region

#Region "Structures"
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)> _
    Public Structure TAIMP2FileInfo
        Public cbSizeOf As Integer
        '
        Public nActive As Boolean
        Public nBitRate As Integer
        Public nChannels As Integer
        Public nDuration As Integer
        Public nFileSize As Long
        Public nRating As Integer
        Public nSampleRate As Integer
        Public nTrackID As Integer
        '
        Public nAlbumLen As Integer
        Public nArtistLen As Integer
        Public nDateLen As Integer
        Public nFileNameLen As Integer
        Public nGenreLen As Integer
        Public nTitleLen As Integer
        '
        Public sAlbum As String
        Public sArtist As String
        Public sDate As String
        Public sFileName As String
        Public sGenre As String
        Public sTitle As String
    End Structure
    ' end TAIMP2FileInfo
#End Region

    Private AimpProcess As Process
    Private AimpRemoteHwnd As IntPtr = IntPtr.Zero
    Dim AInfo As TAIMP2FileInfo
    Dim AFile, InfoPtr As IntPtr
    Public Event TrackStateChanged(ByVal Title As String, ByVal state As IController.StateType) Implements IController.TrackStateChanged
    ' the initial volume value
    Dim Volume As Integer = 100
#Region "Properties"

    Public ReadOnly Property Name() As String Implements IController.Name
        Get
            Return "AIMP2"
        End Get
    End Property
    Public ReadOnly Property Author() As String Implements IController.Author
        Get
            Return "CyberWolf08"
        End Get
    End Property
    Public Property Active() As Boolean Implements IController.Active

    Private _State As IController.StateType = IController.StateType.Closed
    Public Property State() As IController.StateType Implements IController.State
        Get
            Return _State
        End Get
        Set(ByVal value As IController.StateType)
            ' check if the controller state has changed
            If value <> _State Then
                _State = value
                ' the value has changed. raise the trackstatechanged event
                OnTrackStateChanged(EventArgs.Empty)
            End If
        End Set
    End Property



    Public ReadOnly Property TrackTitle() As String Implements IController.TrackTitle
        Get
            Return AInfo.sTitle
        End Get
    End Property

    Public ReadOnly Property TrackArtist() As String Implements IController.TrackArtist
        Get
            Return AInfo.sArtist
        End Get
    End Property

    Public ReadOnly Property TrackAlbum() As String Implements IController.TrackAlbum
        Get
            Return AInfo.sAlbum
        End Get
    End Property

#End Region
    Public Sub New()
        LoadMe()
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        UnmapViewOfFile(InfoPtr)
        CloseHandle(AFile)
    End Sub
    Public Sub LoadMe() Implements IController.LoadMe
        Dim auxProcess() As Process = Process.GetProcessesByName("AIMP2")
        If auxProcess.Length > 0 Then
            AimpProcess = New Process
            AimpProcess = auxProcess(0)
            AimpRemoteHwnd = FindWindow("AIMP2_RemoteInfo", "AIMP2_RemoteInfo")
            Me.State = IController.StateType.Paused
            'open and map the info file
            AFile = OpenFileMapping(FILE_MAP_READ, True, "AIMP2_RemoteInfo")
            InfoPtr = (MapViewOfFile(AFile, FILE_MAP_READ, 0, 0, AIMP2_RemoteFileSize))
        End If
    End Sub
    Protected Overridable Sub OnTrackStateChanged(ByVal e As EventArgs)
        RaiseEvent TrackStateChanged(GetNowplaying, _State)
    End Sub
    Public Sub PlayPause() Implements IController.PlayPause
        If _State And IController.StateType.Running Then
            SendMessage(AimpRemoteHwnd, WM_AIMP_COMMAND, WM_AIMP_CALLFUNC, AIMP_PAUSE)
            GetNowplaying()
        End If
    End Sub
    Public Sub PlayPrev() Implements IController.PlayPrev
        If _State And IController.StateType.Running Then
            SendMessage(AimpRemoteHwnd, WM_AIMP_COMMAND, WM_AIMP_CALLFUNC, AIMP_PREV)
            GetNowplaying()
        End If
    End Sub
    Public Sub PlayNext() Implements IController.PlayNext
        If _State And IController.StateType.Running Then
            SendMessage(AimpRemoteHwnd, WM_AIMP_COMMAND, WM_AIMP_CALLFUNC, AIMP_NEXT)
            GetNowplaying()
        End If
    End Sub
    Public Sub VolumeUp() Implements IController.VolumeUp
        If Volume <= 100 Then
            Volume += VolumeStep
            SetVolume(Volume)
        End If
    End Sub
    Public Sub VolumeDown() Implements IController.VolumeDown
        If Volume >= 0 Then
            Volume -= VolumeStep
            SetVolume(Volume)
        End If
    End Sub
    Public Sub Mute() Implements IController.Mute
        SetVolume(0)
    End Sub
    Public Sub BringToTop() Implements IController.BringToTop
        If _State And IController.StateType.Running Then
            ShowWindow(AimpProcess.MainWindowHandle, 1)
            SetFocus(AimpProcess.MainWindowHandle)
        End If
    End Sub
    Private TitleCache As String
    Public Function GetNowplaying() As String Implements IController.GetNowplaying
        'check if the player is closed
        If Me.State = IController.StateType.Closed Then Return "AIMP2 Closed"
        ' check whether the player was closed
        If AimpProcess.HasExited Then
            Me.State = IController.StateType.Closed
            UnmapViewOfFile(InfoPtr)
            CloseHandle(AFile)

        End If
        ' check whether the player state has changed
        Select Case (SendMessage(AimpRemoteHwnd, WM_AIMP_COMMAND, WM_AIMP_STATUS_GET, AIMP_STS_Player))
            Case 0 : Me.State = IController.StateType.Running
            Case 1 : Me.State = IController.StateType.Playing
            Case 2 : Me.State = IController.StateType.Paused
        End Select
        Dim ABuf As IntPtr
        Dim auxAinfo As TAIMP2FileInfo
        ' assign all the integer variables inside the Afile structure
        auxAinfo = CType(Marshal.PtrToStructure(InfoPtr, GetType(TAIMP2FileInfo)), TAIMP2FileInfo)
        ' move to the end of the integer values and start assigning the string variables
        ABuf = InfoPtr.ToInt32 + Marshal.SizeOf(auxAinfo)
        If TitleCache = "" Or TitleCache <> Marshal.PtrToStringAuto(ABuf) Then
            AInfo = auxAinfo
            TitleCache = Marshal.PtrToStringAuto(ABuf)
            ' start getting strings from ptr with the lengths obtained before 
            AInfo.sAlbum = Marshal.PtrToStringAuto(ABuf, AInfo.nAlbumLen)
            ' move to the next value
            ABuf = ABuf.ToInt32 + AInfo.nAlbumLen * 2
            AInfo.sArtist = Marshal.PtrToStringAuto(ABuf, AInfo.nArtistLen)
            ABuf = ABuf.ToInt32 + AInfo.nArtistLen * 2
            AInfo.sDate = Marshal.PtrToStringAuto(ABuf, AInfo.nDateLen)
            ABuf = ABuf.ToInt32 + AInfo.nDateLen * 2
            AInfo.sFileName = Marshal.PtrToStringAuto(ABuf, AInfo.nFileNameLen)
            ABuf = ABuf.ToInt32 + AInfo.nFileNameLen * 2
            AInfo.sGenre = Marshal.PtrToStringAuto(ABuf, AInfo.nGenreLen)
            ABuf = ABuf.ToInt32 + AInfo.nGenreLen * 2
            AInfo.sTitle = Marshal.PtrToStringAuto(ABuf, AInfo.nTitleLen)
            ' raise the trackstatechanged event to update the main window
            RaiseEvent TrackStateChanged(AInfo.sArtist & " - " & AInfo.sTitle, Me.State)
        End If
        ' return the needed values
        Return AInfo.sArtist & " - " & AInfo.sTitle
    End Function

    'sets the volume to the vol value(0->100)
    Private Sub SetVolume(ByVal Vol As Integer)
        If _State And IController.StateType.Running Then
            ' if we try to set a volume to big or too low exit the sub
            If Volume > 100 Or Volume < 0 Then Exit Sub
            ' transform  vol  to a long type value
            Dim aux As Integer
            aux = ((AIMP_STS_VOLUME * &H10000) + Vol)
            ' send the message to the AimpRemoteHwnd
            SendMessage(AimpRemoteHwnd, WM_AIMP_COMMAND, WM_AIMP_STATUS_SET, aux)
        End If
    End Sub
End Class


