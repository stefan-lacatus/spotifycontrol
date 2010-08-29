Imports System.Runtime.InteropServices

Public Class MainForm

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function SetProcessWorkingSetSize(ByVal handle As IntPtr, ByVal min As IntPtr, ByVal max As IntPtr) As Boolean
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function GetCurrentProcess() As IntPtr
    End Function
    Public CurrentController, ControllerArray(2) As IController
    Public Shared CurrentTrack As New Track
    Dim LastVolume As Integer
    Dim WithEvents Play, NextS, PrevS, BringTop, Mute, VolUp, VolDown As New Shortcut
    ' used for possible workaround the 26-second problem
    Dim WithEvents ApplicationUpdate As New System.ComponentModel.BackgroundWorker
    Private Sub DownloadSomething() Handles ApplicationUpdate.DoWork
        Try
            Dim MyAutoUpdate As New AutoUpdate
            ' where the updates are downloaded from
            Dim RemotePath As String = "http://dl.dropbox.com/u/329033/SpotifyController/"
            If MyAutoUpdate.AutoUpdate(vbNullString, RemotePath) Then
                Application.Exit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpotifyController_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' call the function to unregister the hotkeys
        UnRegisterMyHotKeys()
        SettingManager.CloseServer()
    End Sub
    Private Sub SpotifyController_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ApplicationUpdate.RunWorkerAsync()
        Me.MaximizeBox = False
        TrackInfo.Show()
        TrackInfo.Hide()
        ' make the borders disappear
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Tools.MakeRound(Me)
        ' declare some tooltips to associate to the controls on the main window
        Dim CloseToolTip, PlayPauseToolTip, PrevToolTip, NextToolTip, LyricToolTip As New Windows.Forms.ToolTip
        CloseToolTip.SetToolTip(CloseImg, "Close MediaControl")
        PlayPauseToolTip.SetToolTip(PlayPauseImg, "Play/Pause the current song")
        PrevToolTip.SetToolTip(PrevImg, "Plays the previous song")
        NextToolTip.SetToolTip(NextImg, "Plays the next song")
        LyricToolTip.SetToolTip(LyricImg, "Find the lyrics for the current song")
        ' if on Win7 or Vista give aero feel to the form
        Tools.MakeAero(Me)
        InitControllers()
        AddHandler CurrentController.TrackStateChanged, AddressOf TrackStateChanged
        PlayPauseImg.Image = My.Resources.Play
        NowPlayingBox.Text = CurrentController.GetNowplaying(False)
        ' TODO: Find a way to get the current volume and not feed this values with shit
        LastVolume = 10
        VolumeControl.Value = 10
        ' load the hotkey settings from file
        LoadSettings()
        ' since the user rarely interacts with the app it will behave mostly like a minimized application
        SetProcessWorkingSetSize(GetCurrentProcess(), -1, -1)
        Dim a As New Timer
        a.Interval = 500
        a.Start()
        AddHandler a.Tick, AddressOf RefreshControllers
    End Sub

    Private Sub InitControllers()
        ' init each element of the array with a new controller
        ControllerArray(0) = New ControllerSpotify
        ControllerArray(1) = New ControllerAIMP2
        ControllerArray(2) = New ControllerWinamp
        CurrentController = ControllerArray(1)
        ' add the controller name to the dropdown list
        For Each controller As IController In ControllerArray
            ControllerDropDown.Items.Add(controller.Name)
        Next
        RefreshControllers()
        ControllerDropDown.SelectedItem = CurrentController.Name
    End Sub
    Private Sub RefreshControllers()
        ' see if the controller is actually running and get the one that is currently running/playing to be the active one
        For Each controller As IController In ControllerArray
            If controller.Name <> CurrentController.Name Then
                If controller.State = IController.StateType.Playing Then
                    If CurrentController.State = IController.StateType.Playing Then
                        CurrentController.PlayPause()
                        CurrentController = controller
                        SyncControllers()
                        Exit Sub
                    Else
                        CurrentController = controller
                        SyncControllers()
                        Exit Sub
                    End If
                End If
                If (controller.State And IController.StateType.Running) And CurrentController.State = IController.StateType.Closed Then
                    CurrentController = controller
                    SyncControllers()
                    Exit Sub
                End If
            End If
        Next
    End Sub

    Private Sub ControllerDropDown_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ControllerDropDown.SelectedIndexChanged
        For Each controller As IController In ControllerArray
            If controller.Name = ControllerDropDown.SelectedItem.ToString Then
                CurrentController = controller
                SyncControllers()
                Exit Sub
            End If
        Next
    End Sub

    Private Sub SyncControllers()
        ControllerDropDown.SelectedItem = CurrentController.Name
        AddHandler CurrentController.TrackStateChanged, AddressOf TrackStateChanged
        CurrentController.GetNowplaying(True)
    End Sub

    Private Sub BringToTop() Handles BringTop.Pressed
        CurrentController.BringToTop()
    End Sub
    Private Sub playpause() Handles Play.Pressed, PlayPauseImg.Click
        CurrentController.PlayPause()
        If PlayPauseImg.Tag = "Pause" Then
            PlayPauseImg.Image = My.Resources.Pause_PNG
            PlayPauseImg.Tag = "Play"
        Else
            PlayPauseImg.Image = My.Resources.Play
            PlayPauseImg.Tag = "Pause"
        End If
    End Sub
    Private Sub PlayNextBtn_Click() Handles NextImg.Click, NextS.Pressed
        CurrentController.PlayNext()
        ' Me.Text = CurrentController.GetNowplaying
    End Sub

    Private Sub PlayPrevBtn_Click() Handles PrevImg.Click, PrevS.Pressed
        CurrentController.PlayPrev()
        '  Me.Text = CurrentController.GetNowplaying
    End Sub

    Private Sub MuteBtn_Click() Handles MuteImg.Click, Mute.Pressed
        CurrentController.Mute()
        If LastVolume > 0 Then
            LastVolume = 0
            VolumeControl.Value = 0
        Else
            LastVolume = 10
            VolumeControl.Value = 10
        End If
    End Sub

    Private Sub VolumeControl_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VolumeControl.Scroll
        While VolumeControl.Value <> LastVolume
            If VolumeControl.Value < LastVolume Then
                LastVolume = LastVolume - 1
                CurrentController.VolumeDown()
            Else
                CurrentController.VolumeUp()
                LastVolume = LastVolume + 1
            End If
        End While
    End Sub

    Private Sub VolUpBtn_Click() Handles VolUpImg.Click, VolUp.Pressed
        ' only if we will not go too high with the lastVolume
        If LastVolume < 10 Then
            CurrentController.VolumeUp()
            LastVolume = LastVolume + 1
            VolumeControl.Value = LastVolume
        End If
    End Sub

    Private Sub VolDownBtn_Click() Handles VolDownImg.Click, VolDown.Pressed
        ' make sure we are now to low with the volume
        If LastVolume > 0 Then
            CurrentController.VolumeDown()
            LastVolume = LastVolume - 1
            VolumeControl.Value = LastVolume
        End If
    End Sub

    Private Sub NowPlayingBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NowPlayingBox.DoubleClick
        If (CurrentController.State And IController.StateType.Running) And NowPlayingBox.Text <> "Nothing Playing" Then
            Application.DoEvents()
            TrackInfo.LoadMe(True)
        End If
    End Sub
#Region "Move the window by dragging it with the mouse"
    Private x As Integer = 0
    Private y As Integer = 0
    Private Sub Me_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown, NowPlayingBox.MouseDown
        'Start to move the form
        x = e.X
        y = e.Y
    End Sub

    Private Sub Me_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove, NowPlayingBox.MouseMove
        'Move and refresh
        If (x <> 0 And y <> 0) Then
            Me.Location = New Point(Me.Left + e.X - x, Me.Top + e.Y - y)
        End If
    End Sub

    Private Sub Me_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp, NowPlayingBox.MouseUp
        'Reset the mouse point
        x = 0
        y = 0
    End Sub
#End Region
    Private Sub TrackStateChanged(ByVal Title As String, ByVal state As IController.StateType)
        RefreshControllers()
        NowPlayingBox.Text = Title
        If state = IController.StateType.Paused Then
            ' If PlayPauseImg.Tag <> "Play" Then
            PlayPauseImg.Image = My.Resources.Play
            PlayPauseImg.Tag = "Play"
            'End If
            'CurrentTrack.ArtistName = "Nothing Playing"
            ' CurrentTrack.TrackName = "Nothing Playing"
            ' CurrentTrack.AlbumName = "Nothing Playing"
            'CurrentTrack.CoverURL = vbNullString
        ElseIf state And IController.StateType.Running And Title <> "Nothing Playing" Then
            CurrentTrack.ArtistName = CurrentController.TrackArtist
            CurrentTrack.TrackName = CurrentController.TrackTitle
            CurrentTrack.AlbumName = CurrentController.TrackAlbum
            If PlayPauseImg.Tag <> "Pause" Then
                PlayPauseImg.Image = My.Resources.Pause_PNG
                PlayPauseImg.Tag = "Pause"
            End If

            Application.DoEvents()
            TrackInfo.LoadMe(False)
            If LyricsForm.IsVisible = True Then
                ' refresh the lyrics form
                LyricsForm.LoadMe()
            End If
        ElseIf state = IController.StateType.Closed Then
            NowPlayingBox.Text = CurrentController.Name & " Closed"
            PlayPauseImg.Tag = "Pause"
            PlayPauseImg.Image = My.Resources.Play
            CurrentTrack.ArtistName = "Spotify Closed"
            CurrentTrack.TrackName = "Spotify Closed"
            CurrentTrack.AlbumName = "Spotify Closed"
            CurrentTrack.CoverURL = vbNullString
        End If
    End Sub

    Private Sub CloseImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseImg.Click
        Me.Close()
    End Sub

    Private Sub SettingImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingImg.Click
        ' show the setting manager dialog
        If SettingManager.ShowDialog = Windows.Forms.DialogResult.OK Then
            ' if changes were made and the user saves them
            UnRegisterMyHotKeys()
            Application.DoEvents()
            RegisterMyHotKeys()
        End If
    End Sub
    Private Sub RegisterMyHotKeys()
        Try
            Play.Register(1, SettingManager.MyHotKeyManager(0).MainKeyModifier, SettingManager.MyHotKeyManager(0).MainKey)
            NextS.Register(2, SettingManager.MyHotKeyManager(1).MainKeyModifier, SettingManager.MyHotKeyManager(1).MainKey)
            PrevS.Register(3, SettingManager.MyHotKeyManager(2).MainKeyModifier, SettingManager.MyHotKeyManager(2).MainKey)
            BringTop.Register(4, SettingManager.MyHotKeyManager(1).MainKeyModifier, SettingManager.MyHotKeyManager(1).MainKey)
            Mute.Register(5, SettingManager.MyHotKeyManager(3).MainKeyModifier, SettingManager.MyHotKeyManager(3).MainKey)
            VolUp.Register(6, SettingManager.MyHotKeyManager(4).MainKeyModifier, SettingManager.MyHotKeyManager(4).MainKey)
            VolDown.Register(7, SettingManager.MyHotKeyManager(5).MainKeyModifier, SettingManager.MyHotKeyManager(5).MainKey)
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub UnRegisterMyHotKeys()
        Try
            ' UNRegister the hotkeys
            Play.Unregister(1)
            NextS.Unregister(2)
            PrevS.Unregister(3)
            BringTop.Unregister(4)
            Mute.Unregister(5)
            VolUp.Unregister(6)
            VolDown.Unregister(7)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub LoadSettings()
        Try
            Dim SettingsReader As System.IO.StreamReader
            If IO.File.Exists(Application.StartupPath & "//Settings.ini") Then
                SettingsReader = System.IO.File.OpenText(Application.StartupPath & "//Settings.ini")
            Else
                Throw New ApplicationException("File Not Found")
            End If
            ' read all the global hotkeys values into an auxiliary HotKeyManager
            Dim AuxHotKeyManager As HotKeyManager
            For index = 0 To 5
                AuxHotKeyManager = New HotKeyManager
                AuxHotKeyManager.MainKey = SettingsReader.ReadLine()
                AuxHotKeyManager.MainKeyModifier = SettingsReader.ReadLine()
                ' Add the values to the array that contains all the keys
                SettingManager.MyHotKeyManager(index) = AuxHotKeyManager
            Next
            SettingManager.Password = SettingsReader.ReadLine
            SettingsReader.Close()
            RegisterMyHotKeys()
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub
    Private Sub LyricImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LyricImg.Click
        If (CurrentController.State And IController.StateType.Running) And (NowPlayingBox.Text <> "Nothing Playing") Then
            LyricsForm.LoadMe()
        End If
    End Sub

End Class
NotInheritable Class Shortcut : Inherits NativeWindow
    Private Declare Auto Function RegisterHotKey Lib "user32" (ByVal Handle As IntPtr, ByVal ID As Integer, ByVal Modifier As Integer, ByVal Key As Integer) As Integer
    Private Declare Auto Function UnregisterHotKey Lib "user32" (ByVal Handle As IntPtr, ByVal ID As Integer) As Integer
    Enum Modifier : None = 0 : Alt = 1 : Ctrl = 2 : Shift = 4 : WIN = 8 : End Enum
    Event Pressed(ByVal ID As Integer)
    Sub New()
        CreateHandle(New CreateParams)
    End Sub
    Sub Register(ByVal ID As Integer, ByVal Modifier As Modifier, ByVal Key As Keys)
        Dim result As Integer = RegisterHotKey(Handle, ID, Modifier, Key)
        If result = 0 Then
            MsgBox("Cannot Register " & Modifier.ToString & "+" & Key.ToString & ". Already used by other application.")
        End If
    End Sub
    Sub Unregister(ByVal ID As Integer)
        UnregisterHotKey(Handle, ID)
    End Sub
    Protected Overrides Sub WndProc(ByRef M As Message)
        If M.Msg = 786 Then
            ' raise the hotkey event
            RaiseEvent Pressed(M.WParam.ToInt32)
        ElseIf M.Msg = &H31E Then
            ' the aero state has been changed
            ' repaint the main and the info window window
            Tools.MakeAero(MainForm)
            Tools.MakeAero(TrackInfo)
        End If
        MyBase.WndProc(M)
    End Sub
End Class
