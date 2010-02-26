Public Class SettingManager
    Public MyHotKeyManager(5) As HotKeyManager
    Private Sub SettingManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckGroupBox1.Checked = True
        Dim DefaultValues() As String = New String() {"Play/Pause", "Next", "Previous", "Mute", "Volume Up", "Volume Down"}
        HotKeyTbl.RowCount = 6
        For index = 0 To 5
            HotKeyTbl.Item(0, index).Value = DefaultValues(index)
        Next
        Try
            ' the second comumn should be loaded from file. 
            For index = 0 To 5
                Select Case MyHotKeyManager(index).MainKeyModifier
                    Case 1
                        HotKeyTbl.Item(1, index).Value = "Alt + " & MyHotKeyManager(index).MainKey.ToString
                    Case 2
                        HotKeyTbl.Item(1, index).Value = "Ctrl + " & MyHotKeyManager(index).MainKey.ToString
                    Case 4
                        HotKeyTbl.Item(1, index).Value = "Shift + " & MyHotKeyManager(index).MainKey.ToString
                    Case 8
                        HotKeyTbl.Item(1, index).Value = "Win + " & MyHotKeyManager(index).MainKey.ToString
                    Case vbNull
                        HotKeyTbl.Item(1, index).Value = "None"
                End Select

            Next
        Catch
        End Try
    End Sub
    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles HotKeyTbl.DoubleClick
        ' show the capturekey dialog
        CaptureKey.ShowDialog()
    End Sub

    Private Sub SaveBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        SaveSettings()
        Me.Close()
    End Sub

    Private Sub CancelBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub SaveSettings()
        Dim SettingWriter As System.IO.StreamWriter
        Try

            ' if the file exists display a msgbox with options
            If System.IO.File.Exists(Application.StartupPath & "//Settings.ini") Then
                If MsgBox("A settings file already exists. Overwrite?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    System.IO.File.Delete(Application.StartupPath & "//Settings.ini")
                Else
                    Exit Try
                End If
            End If
            SettingWriter = System.IO.File.CreateText(Application.StartupPath & "//Settings.ini")
            ' write the global hotkeys to file
            For index = 0 To 5
                SettingWriter.WriteLine(MyHotKeyManager(index).MainKey)
                SettingWriter.WriteLine(MyHotKeyManager(index).MainKeyModifier)
            Next
            'SettingWriter.Close()
        Catch ex As Exception

            ' MsgBox(ex.Message)
        End Try
        SettingWriter.Close()
    End Sub
End Class
