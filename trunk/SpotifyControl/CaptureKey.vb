Imports System.Windows.Forms

Public Class CaptureKey
    Dim WinKey, AltKey, ShiftKey, ControlKey As Boolean ' the variables that check for modifier change
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.Alt = True Then
            TextBox1.Text = "Alt + "
            AltKey = True
            ControlKey = False
            ShiftKey = False
        ElseIf e.Control = True Then
            TextBox1.Text = "Ctrl + "
            ControlKey = True
            ShiftKey = False
            AltKey = False
        ElseIf e.Shift = True Then
            TextBox1.Text = "Shift + "
            ShiftKey = True
            ControlKey = False
            AltKey = False
        End If

    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If Not (e.KeyCode = Keys.Alt Or e.KeyCode = Keys.ControlKey Or e.KeyCode = Keys.ShiftKey) Then
            TextBox1.Text = TextBox1.Text + e.KeyCode.ToString
        End If
        MsgBox(TextBox1.Text)
        Me.Close()
    End Sub

    Private Sub CaptureKey_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' reset all variables
        TextBox1.Text = vbNullString
        WinKey = False
        AltKey = False
        ShiftKey = False
        ControlKey = False
    End Sub
End Class
