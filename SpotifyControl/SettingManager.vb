Public Class SettingManager
    Public MyHotKeyManager(5) As HotKeyManager
    Private Sub SettingManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DefaultValues() As String = New String() {"Play/Pause", "Next", "Previous", "Mute", "Volume Up", "Volume Down"}
        HotKeyTbl.RowCount = 5
        For index = 0 To 5
            HotKeyTbl.Item(0, index).Value = DefaultValues(index)
        Next
        ' the second comumn should be loaded from file. 
        For index = 0 To 5
            HotKeyTbl.Item(1, index).Value = "None"
        Next
    End Sub
    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles HotKeyTbl.DoubleClick
        ' show the capturekey dialog
        CaptureKey.ShowDialog()
    End Sub
End Class
