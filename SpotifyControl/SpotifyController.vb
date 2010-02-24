Imports System.Runtime.InteropServices


Public Class SpotifyController
    Public MySpotify As New ControllerClass
    Dim LastVolume As Integer
    Dim TrackChangeIndex As Integer
    Dim WithEvents Play, NextS, PrevS, BringTop, Mute, VolUp, VolDown As New Shortcut
#Region "For Aero Glass"
    <Flags()> Public Enum DwmBlurBehindDwFlags As UInteger
        DWM_BB_ENABLE = &H1
        DWM_BB_BLURREGION = &H2
        DWM_BB_TRANSITIONONMAXIMIZED = &H4
    End Enum
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure DWM_BLURBEHIND
        Public dwFlags As DwmBlurBehindDwFlags
        Public fEnable As Boolean
        Public hRgnBlur As IntPtr
        Public fTransitionOnMaximized As Boolean
    End Structure
    <DllImport("dwmapi.dll")> _
    Private Shared Sub DwmEnableBlurBehindWindow(ByVal hwnd As IntPtr, ByRef blurBehind As DWM_BLURBEHIND)
    End Sub
#End Region
    Private Sub SpotifyController_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' call the function to unregister the hotkeys
        UnRegisterMyHotKeys()
    End Sub
    Private Sub SpotifyController_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MaximizeBox = False
        ' make the borders dissapear
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        ' if on Win7 or Vista give aero feel to the form
        If GetAeroSupport() = "Supports aero" Then
            On Error Resume Next
            ' black is the color that gets transformed to glass
            Me.BackColor = Color.Black
            ' make the entire form glassy and blurry
            Dim Aux As DWM_BLURBEHIND
            Aux.dwFlags = DwmBlurBehindDwFlags.DWM_BB_ENABLE
            Aux.fEnable = True
            Aux.hRgnBlur = vbNull
            DwmEnableBlurBehindWindow(Me.Handle, Aux)
        End If
        TextBox1.Text = MySpotify.GetNowplaying
        ' TODO: Find a way to get the current volume and not feed this values with shit
        VolumeControl.Value = 10
        LastVolume = 10
        ' load the hotkey settings from file
        LoadSettings()
        ' register the hotkeys
        RegisterMyHotKeys()
        TrackInfo.Show()
        TrackInfo.Hide()
    End Sub
    Private Sub BringToTop() Handles BringTop.Pressed
        MySpotify.BringToTop()
    End Sub
    Private Sub playpause() Handles Play.Pressed, PlayPauseImg.Click
        MySpotify.PlayPause()
        If PlayPauseImg.Tag = "Pause" Then
            PlayPauseImg.Image = My.Resources.Play1
            PlayPauseImg.Tag = "Play"
        Else
            PlayPauseImg.Image = My.Resources.Pause_PNG
            PlayPauseImg.Tag = "Pause"
        End If
    End Sub
    Private Sub PlayNextBtn_Click() Handles NextImg.Click, NextS.Pressed
        MySpotify.PlayNext()
        ' Me.Text = MySpotify.GetNowplaying
        TextBox1.Text = MySpotify.GetNowplaying
    End Sub

    Private Sub PlayPrevBtn_Click() Handles PrevImg.Click, PrevS.Pressed
        MySpotify.PlayPrev()
        '  Me.Text = MySpotify.GetNowplaying
        TextBox1.Text = MySpotify.GetNowplaying
    End Sub

    Private Sub MuteBtn_Click() Handles MuteImg.Click, Mute.Pressed
        MySpotify.Mute()
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
                MySpotify.VolumeDown()
            Else
                MySpotify.VolumeUp()
                LastVolume = LastVolume + 1
            End If
        End While
    End Sub

    Private Sub VolUpBtn_Click() Handles VolUpImg.Click, VolUp.Pressed
        ' only if we will not go too high with the lastVolume
        If LastVolume < 10 Then
            MySpotify.VolumeUp()
            LastVolume = LastVolume + 1
            VolumeControl.Value = LastVolume
        End If
    End Sub

    Private Sub VolDownBtn_Click() Handles VolDownImg.Click, VolDown.Pressed
        ' make sure we are now to low with the volume
        If LastVolume > 0 Then
            MySpotify.VolumeDown()
            LastVolume = LastVolume - 1
            VolumeControl.Value = LastVolume
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SongCheck.Tick
        TextBox1.Text = MySpotify.GetNowplaying
    End Sub

#Region "Move the window by dragging it with the mouse"
    Private x As Integer = 0
    Private y As Integer = 0

    Private Sub TextBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.DoubleClick
        TrackInfo.LoadMe()
    End Sub
    Private Sub Me_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown, TextBox1.MouseDown
        'Start to move the form.
        x = e.X
        y = e.Y
    End Sub

    Private Sub Me_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove, TextBox1.MouseMove
        'Move and refresh.
        If (x <> 0 And y <> 0) Then
            Me.Location = New Point(Me.Left + e.X - x, Me.Top + e.Y - y)
            Me.Refresh()
        End If
    End Sub

    Private Sub Me_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp, TextBox1.MouseUp
        'Reset the mouse point.
        x = 0
        y = 0
    End Sub
#End Region

    Friend Function GetAeroSupport() As String
        Dim strVersion As String = "No aero"
        Select Case Environment.OSVersion.Platform
            Case PlatformID.Win32NT
                Select Case Environment.OSVersion.Version.Major
                    Case 3I
                        strVersion = "No aero"
                    Case 4I
                        strVersion = "No aero"
                    Case 5I
                        strVersion = "No aero"
                    Case 6I
                        strVersion = "Supports aero"
                End Select
        End Select
        Return strVersion
    End Function

    Private Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TrackChangeIndex = TrackChangeIndex + 1
        Dim g As Graphics = TextBox1.CreateGraphics
        Dim textSize As SizeF
        ' measure the text so we can resize the window to fit the text and to look nice
        'TOTO: Doesn't work quite well, also we should make sure it doesn't go off-screan
        textSize = g.MeasureString(TextBox1.Text, TextBox1.Font)
        Me.Width = TextBox1.Location.X + textSize.Width + 61
        SettingImg.Location = New Point(TextBox1.Location.X + textSize.Width + 1, SettingImg.Location.Y)
        CloseImg.Location = New Point(TextBox1.Location.X + textSize.Width + 27, CloseImg.Location.Y)
        ' check if the spotify stopped Playing or started playing
        If TextBox1.Text = "Nothing Playing" Then
            PlayPauseImg.Image = My.Resources.Play1
            PlayPauseImg.Tag = "Play"
        ElseIf TextBox1.Text <> "Unknown" And TextBox1.Text <> vbNullString Then
            PlayPauseImg.Image = My.Resources.Pause_PNG
            Application.DoEvents()
            If TrackChangeIndex <> 2 Then
                TrackInfo.LoadMe()
            End If
        ElseIf TextBox1.Text = "Unknown" Then
            MySpotify.FindSpotiyWindow()
            PlayPauseImg.Tag = "Pause"
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
            MsgBox(ex.Message)
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
            SettingsReader = System.IO.File.OpenText(Application.StartupPath & "//Settings.ini")
            ' read all the global hotkeys values into an auxiliary HotKeyManager
            Dim AuxHotKeyManager As HotKeyManager
            For index = 0 To 5
                AuxHotKeyManager = New HotKeyManager
                AuxHotKeyManager.MainKey = SettingsReader.ReadLine()
                AuxHotKeyManager.MainKeyModifier = SettingsReader.ReadLine()
                ' Add the values to the array that contains all the keys
                SettingManager.MyHotKeyManager(index) = AuxHotKeyManager
            Next
            SettingsReader.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    ' Loops for a specificied period of time (milliseconds)
    Private Sub wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            ' Allows UI to remain responsive
            Application.DoEvents()
        Loop
        sw.Stop()
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
        If M.Msg = 786 Then RaiseEvent Pressed(M.WParam.ToInt32)
        MyBase.WndProc(M)
    End Sub
End Class
