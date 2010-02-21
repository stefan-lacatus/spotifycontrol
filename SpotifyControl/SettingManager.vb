Public Class SettingManager

    Private Sub SettingManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim key As Keys
        For Each key In [Enum].GetValues(GetType(Keys))
            PlayPauseKey.Items.Add(key.ToString)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        CaptureKey.ShowDialog()
    End Sub
End Class