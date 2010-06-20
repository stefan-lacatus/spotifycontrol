Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class CaptureKey
    ' WinKey cannot be handled using .NET managed code.
    ' We will need to invoke the GetAsyncKeyState function from user32.dll to capture the WinKey
    Dim WinKey, AltKey, ShiftKey, ControlKey As Boolean ' the variables that check for modifier change
    <DllImport("user32.dll")> _
    Public Shared Function GetAsyncKeyState(ByVal vKey As Int32) As Short
    End Function
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        ' get the modifiers. This can be Alt, Ctrl or Shift.
        ' WinKey is handled separately
        If e.Alt = True Then
            TextBox1.Text = "Alt + "
            AltKey = True
            ' reset all others modifiers
            ControlKey = False
            ShiftKey = False
            WinKey = False
        ElseIf e.Control = True Then
            TextBox1.Text = "Ctrl + "
            ControlKey = True
            ' reset all others modifiers
            ShiftKey = False
            AltKey = False
            WinKey = False
        ElseIf e.Shift = True Then
            TextBox1.Text = "Shift + "
            ShiftKey = True
            ' reset all others modifiers
            ControlKey = False
            AltKey = False
            WinKey = False
        End If
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        ' get the full key combination : the modifier key and the normal key
        If Not (e.KeyCode = Keys.Alt Or e.KeyCode = Keys.ControlKey Or e.KeyCode = Keys.ShiftKey) Then
            TextBox1.Text = TextBox1.Text + e.KeyCode.ToString

            ' disable the WinKey Handler
            WinKeyCheck.Enabled = False
            ' 
            '   Dim aux As String = SettingManager.HotKeyLstBox.SelectedItem & TextBox1.Text
            ' SettingManager.HotKeyLstBox.Items(SettingManager.HotKeyLstBox.SelectedIndex) = aux.Split("  ")(1)
            SettingManager.HotKeyTbl.Item(1, SettingManager.HotKeyTbl.SelectedRows(0).Index).Value = TextBox1.Text
            Application.DoEvents()
            System.Threading.Thread.Sleep(500) ' this is just for the user to see what keycombination he added, no real purpose
            Dim AuxHotKeyManager As New HotKeyManager
            If AltKey = True Then
                AuxHotKeyManager.MainKeyModifier = 1
            ElseIf ShiftKey = True Then
                AuxHotKeyManager.MainKeyModifier = 4
            ElseIf ControlKey = True Then
                AuxHotKeyManager.MainKeyModifier = 2
            ElseIf WinKey = True Then
                AuxHotKeyManager.MainKeyModifier = 8
            End If
            AuxHotKeyManager.MainKey = e.KeyCode
            SettingManager.MyHotKeyManager(SettingManager.HotKeyTbl.SelectedRows(0).Index) = AuxHotKeyManager
            Me.Close()
        End If
    End Sub

    Private Sub CaptureKey_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' reset all variables
        TextBox1.Text = vbNullString
        WinKey = False
        AltKey = False
        ShiftKey = False
        ControlKey = False
        WinKeyCheck.Enabled = True
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WinKeyCheck.Tick
        WinKey = GetAsyncKeyState(Keys.LWin) Or GetAsyncKeyState(Keys.RWin)
        If WinKey = True Then
            TextBox1.Text = "Win + "
            ControlKey = False
            ShiftKey = False
            AltKey = False
        End If
    End Sub
End Class
