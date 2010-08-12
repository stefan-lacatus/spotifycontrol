Imports System.Runtime.InteropServices
Imports System.Runtime.ConstrainedExecution
Imports System.ComponentModel

Public Class ControllerWinamp : Implements IController, IDisposable
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
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure WinampExtendedFileInfo
        Public Filename As UInteger
        Public MetaData As UInteger
        Public ReturnValue As UInteger
        Public MaxReturnValueLength As Integer
    End Structure

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, _
                                             ByVal iSize As Integer, ByRef lpNumberOfBytesRead As Integer) As Boolean
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function WriteProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, _
                                             ByVal iSize As Integer, ByRef lpNumberOfBytesRead As Integer) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function GetWindowThreadProcessId(ByVal hwnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
    End Function

    <DllImport("kernel32.dll")> _
    Private Shared Function OpenProcess(ByVal dwDesiredAccess As Integer, <MarshalAs(UnmanagedType.Bool)> _
                                        ByVal bInheritHandle As Boolean, ByVal dwProcessId As Integer) As IntPtr
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, ExactSpelling:=True)> _
    Public Shared Function CloseHandle(ByVal hObject As IntPtr) As Integer
    End Function
    <DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function VirtualAllocEx(ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As UInteger, _
                                    ByVal flAllocationType As UInteger, ByVal flProtect As UInteger) As IntPtr
    End Function
    <DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function VirtualFreeEx(ByVal hProcess As IntPtr, ByVal pAddress As IntPtr, ByVal size As Integer, ByVal freeType As Integer) As Boolean
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Friend Shared Function GetWindowText(ByVal hWnd As IntPtr, <Out(), MarshalAs(UnmanagedType.LPTStr)> ByVal lpString As String, ByVal nMaxCount As Integer) As Integer
    End Function
#End Region

#Region "Constants"
    Private Const WM_COMMAND = &H111
    Private Const WM_USER = &H400
    Private Const WM_WA_IPC = WM_USER
    Private Const IPC_PLAYING_FILE = 3003
    Private Const IPC_CB_MISC = 603
    Private Const IPC_CB_MISC_TITLE = 0
    Private Const IPC_CB_MISC_STATUS = 2
    Private Const IPC_GETLISTPOS = 125
    Private Const IPC_GETPLAYLISTFILE = 211
    Private Const IPC_GET_EXTENDED_FILE_INFO = 290
    Private Const IPC_GET_EXTENDED_FILE_INFO_HOOKABLE = 296
    Private Const GWL_WNDPROC = -4
    Private Const WA_SETVOLUME = 122
    Private Const VolumeStep = 25.5
    Private Const METASIZE = 128
    Private Const MaxDataLength = 10000

    ' IPC command we can send to winamp
    Protected Enum IPCCommand
        GetVersion = 0
        GetStatus = 104
        GetFilename = 3031
        GetTitle = 3034
        ExtendedFileInfo = 3026
    End Enum
    ' misc commands we can send to winamp
    Protected Enum Command
        Play = 40045
        PlayPause = 40046
        [Stop] = 40047
        PrevTrack = 40198
        NextTrack = 40048
    End Enum
#Region "Memory Management Constants"
    Friend Const MEM_COMMIT As Integer = &H1000
    Friend Const MEM_RELEASE As Integer = &H8000
    Friend Const MEM_RESERVE As Integer = &H2000
    Friend Const PAGE_READWRITE As Integer = &H4
    Friend Const PROCESS_VM_OPERATION As Integer = &H8
    Friend Const PROCESS_VM_READ As Integer = &H10
    Friend Const PROCESS_VM_WRITE As Integer = &H20
#End Region

#End Region
#Region "Structures"
    ' Information about a song.
    Private Class Song
        Public Property Title() As String
        Public Property Artist() As String
        Public Property Album() As String
        Public Property Year() As String
        ' Whether the song has any metadata. If false, only the title will be available.
        Public Property HasMetadata() As Boolean
        Public Property Filename() As String
        Private m_Filename As String
    End Class

#End Region

    ' Occurs when the currently playing song has been changed or the state has changed
    Public Event TrackStateChanged(ByVal Title As String, ByVal state As IController.StateType) _
        Implements IController.TrackStateChanged

    Private WinampProcess As Process
    Private WinampHandle As IntPtr = IntPtr.Zero
    ' the initial volume value
    Dim Volume As Integer = 255
    Private CurrentSong As Song

#Region "Properties"

    Public ReadOnly Property Name() As String Implements IController.Name
        Get
            Return "Winamp"
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
            Return CurrentSong.Title
        End Get
    End Property

    Public ReadOnly Property TrackArtist() As String Implements IController.TrackArtist
        Get
            Return CurrentSong.Artist
        End Get
    End Property

    Public ReadOnly Property TrackAlbum() As String Implements IController.TrackAlbum
        Get
            Return CurrentSong.Album
        End Get
    End Property
#End Region
    Public Sub New()
        LoadMe()
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose

    End Sub
    Public Sub LoadMe() Implements IController.LoadMe
        Dim auxProcess() As Process = Process.GetProcessesByName("winamp")
        If auxProcess.Length > 0 Then
            WinampProcess = New Process
            WinampProcess = auxProcess(0)
            '  ControllerWinamp = FindWindow(
            WinampHandle = WinampProcess.MainWindowHandle
            Me.State = IController.StateType.Running
            CurrentSong = New Song()
        End If
    End Sub
    Protected Overridable Sub OnTrackStateChanged(ByVal e As EventArgs)
        RaiseEvent TrackStateChanged(TitleCache, _State)
    End Sub
    Public Sub PlayPause() Implements IController.PlayPause
        If _State And IController.StateType.Running Then
            SendCommand(Command.PlayPause)
            GetNowplaying()
        End If
    End Sub
    Public Sub PlayPrev() Implements IController.PlayPrev
        If _State And IController.StateType.Running Then
            SendCommand(Command.PrevTrack)
            GetNowplaying()
        End If
    End Sub
    Public Sub PlayNext() Implements IController.PlayNext
        If _State And IController.StateType.Running Then
            SendCommand(Command.NextTrack)
            GetNowplaying()
        End If
    End Sub
    Public Sub VolumeUp() Implements IController.VolumeUp
        If Volume <= 255 Then
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
        If Volume > 0 Then
            SetVolume(0)
        Else
            SetVolume(Volume)
        End If
    End Sub
    Public Sub BringToTop() Implements IController.BringToTop
        If _State And IController.StateType.Running Then
            ShowWindow(WinampProcess.MainWindowHandle, 1)
            SetFocus(WinampProcess.MainWindowHandle)
        End If
    End Sub
    Private TitleCache As String
    Public Function GetNowplaying() As String Implements IController.GetNowplaying
        ' if winamp is closed than simply exit the function
        If Me.State = IController.StateType.Closed Then Exit Function
        Dim Status As Integer = SendMessage(WinampHandle, WM_USER, 0, IPCCommand.GetStatus)

        Select Case Status
            Case 1
                Me.State = IController.StateType.Playing
            Case 3
                Me.State = IController.StateType.Paused
            Case Else
                Me.State = IController.StateType.Running
        End Select

        Dim xstruct As New WinampExtendedFileInfo()
        Dim encoding As New System.Text.UTF8Encoding()
        Dim playlistFile, metaData, data, extendedFileInfo As IntPtr

        ' send messages to inform winamp of a readmemory
        Dim tmp As Integer = SendMessage(WinampHandle, WM_WA_IPC, 0, IPC_GETLISTPOS)
        Dim ptr As IntPtr = SendMessage(WinampHandle, WM_WA_IPC, tmp, IPC_GETPLAYLISTFILE)
        Dim file As String = PointerToString(ptr)
        'Debug.WriteLine("Currently Playing song: " & file)

        playlistFile = AllocWinamp(MaxDataLength)
        metaData = AllocWinamp(MaxDataLength)
        data = AllocWinamp(MaxDataLength)

        	
        ' allocate the memory in winamp's space
        Dim aux As IntPtr = Marshal.OffsetOf(GetType(WinampExtendedFileInfo), "MaxReturnValueLength")
        extendedFileInfo = AllocWinamp(CUInt(Marshal.SizeOf(GetType(WinampExtendedFileInfo))))
        WriteLocalToWinamp(Offset(extendedFileInfo, Marshal.OffsetOf(GetType(WinampExtendedFileInfo), "Filename")), playlistFile)
        WriteLocalToWinamp(Offset(extendedFileInfo, Marshal.OffsetOf(GetType(WinampExtendedFileInfo), "MetaData")), metaData)
        WriteLocalToWinamp(Offset(extendedFileInfo, Marshal.OffsetOf(GetType(WinampExtendedFileInfo), "ReturnValue")), data)
        WriteLocalToWinamp(Offset(extendedFileInfo, Marshal.OffsetOf(GetType(WinampExtendedFileInfo), "MaxReturnValueLength")), New IntPtr(MaxDataLength))
        WriteLocalToWinamp(playlistFile, file)
        WriteLocalToWinamp(metaData, "artist")
        SendMessage(WinampHandle, WM_WA_IPC, extendedFileInfo, IPC_GET_EXTENDED_FILE_INFO)
        CurrentSong.Artist = PointerToString(data)
        WriteLocalToWinamp(metaData, "title")
        SendMessage(WinampHandle, WM_WA_IPC, extendedFileInfo, IPC_GET_EXTENDED_FILE_INFO)
        CurrentSong.Title = PointerToString(data)
        WriteLocalToWinamp(metaData, "album")
        SendMessage(WinampHandle, WM_WA_IPC, extendedFileInfo, IPC_GET_EXTENDED_FILE_INFO)
        CurrentSong.Album = PointerToString(data)
        ' free the memory
        FreeWinamp(playlistFile)
        FreeWinamp(metaData)
        FreeWinamp(data)
        FreeWinamp(extendedFileInfo)
        ' if the song has changed raise the TrackStateChanged event
        If CurrentSong.Title & " - " & CurrentSong.Artist <> TitleCache Then
            ' the song has changed
            TitleCache = CurrentSong.Title & " - " & CurrentSong.Artist
            RaiseEvent TrackStateChanged(TitleCache, Me.State)
        End If
        ' if the metadata is empty, get info from the window tile
        If CurrentSong.Artist = "" Or CurrentSong.Title = "" Then
            Dim lpText As String
            lpText = New String(Chr(0), 100)
            Dim intLength As Integer = GetWindowText(WinampHandle, lpText, lpText.Length)
            Dim strTitle As String = lpText.Substring(0, intLength)
            If strTitle <> TitleCache Then
                ' the song has changed
                TitleCache = strTitle
                RaiseEvent TrackStateChanged(TitleCache, Me.State)
            End If
            Return strTitle
            Try
                CurrentSong.Title = strTitle.Substring(InStr(strTitle, " – ") + 2, strTitle.Count - InStr(strTitle, " – ") - 2)
                CurrentSong.Artist = strTitle.Substring(0, InStr(strTitle, " – ") - 1)
            Catch ex As Exception
                Debug.WriteLine("Error: " & ex.Message)
                Return "No info available"
            End Try
        End If
        Return CurrentSong.Title & " - " & CurrentSong.Artist
    End Function

    'sets the volume to the vol value(0->100)
    Private Sub SetVolume(ByVal Vol As Integer)
        If _State And IController.StateType.Running Then
            ' if we try to set a volume to big or too low set the volume to the max/ min value
            If Volume > 255 Then SendMessage(WinampHandle, WM_WA_IPC, 255, WA_SETVOLUME)
            If Volume < 0 Then SendMessage(WinampHandle, WM_WA_IPC, 0, WA_SETVOLUME)
            If Volume > 255 Or Volume < 0 Then Exit Sub
            SendMessage(WinampHandle, WM_WA_IPC, Vol, WA_SETVOLUME)
        End If
    End Sub

    ' sends commands using SendMessage to the winampHWnd
    Private Sub SendCommand(ByVal command As Command)
        SendMessage(WinampHandle, WM_COMMAND, command, 0)
    End Sub

#Region "Functions for winamp metadata"
    Private Sub ReadWinampToLocal(ByVal remoteBuf As IntPtr, ByRef localBuf() As Byte, ByVal bufsize As Integer)
        Dim hWinampProcess As IntPtr = GetWinampProcessHandle(PROCESS_VM_READ)
        ReadProcessMemory(hWinampProcess, remoteBuf, localBuf, bufsize, 0)
        CloseHandle(hWinampProcess)
    End Sub
    Private Function PointerToString(ByVal pointer As IntPtr) As String
        Dim data(MaxDataLength) As Byte
        ReadWinampToLocal(pointer, data, MaxDataLength)
        ' transform the byte array into an string
        Dim enc As New System.Text.UTF8Encoding()
        Dim aux As String = enc.GetString(data)
        'Find the null terminator.
        Dim i As Integer = aux.IndexOf(ChrW(0))
        aux = aux.Substring(0, i)
        Return aux
    End Function
    Private Overloads Sub WriteLocalToWinamp(ByVal remoteBuf As IntPtr, ByVal localBuf As IntPtr)
        Dim hWinampProcess As IntPtr = GetWinampProcessHandle(PROCESS_VM_WRITE Or PROCESS_VM_OPERATION)
        ' Copy from our memory into process memory.
        Dim data() As Byte = BitConverter.GetBytes(localBuf.ToInt32())
        WriteProcessMemory(hWinampProcess, remoteBuf, data, data.Length, 0)
        CloseHandle(hWinampProcess)
    End Sub
    Private Overloads Sub WriteLocalToWinamp(ByVal remoteBuf As IntPtr, ByVal localString As String)
        Dim hWinampProcess As IntPtr = GetWinampProcessHandle(PROCESS_VM_WRITE Or PROCESS_VM_OPERATION)
        ' Copy from our memory into process memory.
        Dim data() As Byte = System.Text.Encoding.Default.GetBytes(localString + Chr(0))
        Dim byteswritten As IntPtr
        WriteProcessMemory(hWinampProcess, remoteBuf, data, data.Length, byteswritten)
        CloseHandle(hWinampProcess)
    End Sub

    'returns NULL if alloc failed, returns remote address otherwise
    Private Function AllocWinamp(ByVal bufsize As UInteger) As IntPtr
        Dim remoteBuf As IntPtr
        ' allocate chunk of memory in winamp's address space
        remoteBuf = VirtualAllocEx(WinampProcess.Handle, IntPtr.Zero, New UIntPtr(bufsize), MEM_COMMIT Or MEM_RESERVE, PAGE_READWRITE)
        ' CloseHandle(WinampProcess.Handle)
        Return remoteBuf
    End Function

    'returns 0 on success (it's winamp's problem if it fails... right?)
    Private Sub FreeWinamp(ByVal remoteBuf As IntPtr)
        If remoteBuf <> IntPtr.Zero Then
            VirtualFreeEx(WinampHandle, remoteBuf, 0, &H8000)
        End If
    End Sub

    Private Function GetWinampProcessHandle(ByVal desiredAcces As Integer) As IntPtr
        Dim pid, hWinampProcess As IntPtr
        'get the winamp process from it's window hwnd
        GetWindowThreadProcessId(WinampHandle, pid)
        ' make some checks and exit the sub if necessary 
        If pid = IntPtr.Zero Then Throw New Exception("could not find the winamp process pid")
        hWinampProcess = OpenProcess(desiredAcces, False, pid)
        If hWinampProcess = IntPtr.Zero Then Throw New Exception("could not find the winamp process handle")
        Return hWinampProcess
    End Function

    Private Function Offset(ByVal pointer As IntPtr, ByVal _offset As IntPtr) As IntPtr
        Return New IntPtr(pointer.ToInt64() + _offset.ToInt64())
    End Function
#End Region
End Class
