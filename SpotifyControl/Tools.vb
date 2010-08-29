Imports System.Runtime.InteropServices

Module Tools
#Region "For Aero Glass"
    <Flags()> Friend Enum DwmBlurBehindDwFlags As UInteger
        DWM_BB_ENABLE = &H1
        DWM_BB_BLURREGION = &H2
        DWM_BB_TRANSITIONONMAXIMIZED = &H4
    End Enum
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure DWM_BLURBEHIND
        Public dwFlags As DwmBlurBehindDwFlags
        Public fEnable As Boolean
        Public hRgnBlur As IntPtr
        Public fTransitionOnMaximized As Boolean
    End Structure
    <DllImport("dwmapi.dll")> _
    Friend Sub DwmEnableBlurBehindWindow(ByVal hwnd As IntPtr, ByRef blurBehind As DWM_BLURBEHIND)
    End Sub
    <DllImport("dwmapi.dll", EntryPoint:="DwmIsCompositionEnabled")> _
    Friend Function DwmIsCompositionEnabled(ByRef enabled As Boolean) As Integer
    End Function
#End Region
    ' downloads a file using a simple HttpWebRequest/Response combination.
    Public Function DownloadFile(ByVal uri As String) As System.IO.Stream
        Dim request As Net.HttpWebRequest
        Dim response As Net.HttpWebResponse
        request = Net.HttpWebRequest.Create(uri)
        Threading.Thread.Sleep(1000)
        request.Method = "GET"
        request.UserAgent = "SpotifyControl"
        request.Timeout = Threading.Timeout.Infinite
        request.KeepAlive = False
        request.ProtocolVersion = Net.HttpVersion.Version10
        response = request.GetResponse
        Return response.GetResponseStream
    End Function
    Private Function GetAeroSupport() As String
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
    ' takes a form as parameter and will try to make the form Aero, returning if the operation succeeded or not.
    Public Function MakeAero(ByVal Form As Windows.Forms.Form) As Boolean
        If GetAeroSupport() = "No aero" Then
            Form.BackColor = Color.Gray
            Form.Opacity = 80 / 100
            Return False
            Exit Function
        End If
        ' check if aero is enabled
        Dim aeroEnabled As Boolean
        DwmIsCompositionEnabled(aeroEnabled)
        If aeroEnabled Then
            ' On Error Resume Next
            ' black is the color that gets transformed to glass
            Form.BackColor = Color.Black
            Form.Opacity = 1
            ' make the entire form glassy and blurry
            Dim Aux As DWM_BLURBEHIND
            Aux.dwFlags = DwmBlurBehindDwFlags.DWM_BB_ENABLE
            Aux.fEnable = True
            Aux.hRgnBlur = vbNull
            DwmEnableBlurBehindWindow(Form.Handle, Aux)
            Return True
        Else
            Form.BackColor = Color.Gray
            Form.Opacity = 80 / 100
            Return False
        End If
    End Function

    Public Sub MakeRound(ByRef Form As Windows.Forms.Form)
        ' add some round edges to the form
        Dim p As New Drawing2D.GraphicsPath()
        p.StartFigure()
        p.AddArc(New Rectangle(0, 0, 20, 20), 180, 90)
        p.AddLine(20, 0, Form.Width - 20, 0)
        p.AddArc(New Rectangle(Form.Width - 20, 0, 20, 20), -90, 90)
        p.AddLine(Form.Width, 20, Form.Width, Form.Height - 20)
        p.AddArc(New Rectangle(Form.Width - 20, Form.Height - 20, 20, 20), 0, 90)
        p.AddLine(Form.Width - 20, Form.Height, 20, Form.Height)
        p.AddArc(New Rectangle(0, Form.Height - 20, 20, 20), 90, 90)
        p.CloseFigure()
        Form.Region = New Region(p)
        p.Dispose()
    End Sub
End Module
