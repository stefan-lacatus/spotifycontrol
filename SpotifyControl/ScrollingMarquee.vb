' based on a modified version of a control made by .NetNinja(www.vbforums.com)
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Public Class ScrollingMarquee
    Inherits System.Windows.Forms.Label

    Friend WithEvents tmrMain, tmrBefore As System.Windows.Forms.Timer

    Private startPosition As Integer = 0
    Private _MarqueeText As String = Me.Text
    Private _LeftToRight As Direction = Direction.Right
    Private _ScrollSpeed As Integer = 5
    Private _ShadowColor As Color = Color.White
    Private _TimeBefore As Integer = 1000

    Private Structure tSize
        Dim X As Long
        Dim Y As Long
    End Structure

    Public Enum Direction
        Left
        Right
    End Enum

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        AddHandler MyBase.Paint, AddressOf OnPaint
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.tmrMain = New System.Windows.Forms.Timer(Me.components)
        Me.tmrBefore = New System.Windows.Forms.Timer(Me.components)
        Me.tmrMain.Enabled = False
        Me.tmrBefore.Enabled = True
        Me.Name = "ScrollingMarquee"
        Me.Size = New System.Drawing.Size(120, 13)
    End Sub


    <Category("Marquee")> _
    <Description("Gets/Sets the direction of the control")> _
    Public Property ScrollLeftToRight() As Direction
        Get
            Return _LeftToRight
        End Get
        Set(ByVal Value As Direction)
            _LeftToRight = Value
            Invalidate()
        End Set
    End Property

    <Category("Marquee")> _
    <Description("Gets/Sets the time before the text start scrolling(in miliseconds)")> _
    Public Property TimeBeforStart() As Integer
        Get
            Return _TimeBefore
        End Get
        Set(ByVal value As Integer)
            _TimeBefore = value
            tmrBefore.Interval = value
        End Set
    End Property

    <Category("Marquee")> _
    <Description("Gets/Sets the scroll speed of the control. Values can be from 1 to 10.")> _
    Public Property ScrollSpeed() As Integer
        Get
            ScrollSpeed = _ScrollSpeed
        End Get
        Set(ByVal Value As Integer)

            If Value < 1 Then
                _ScrollSpeed = 1
            ElseIf Value > 10 Then
                _ScrollSpeed = 10
            Else
                _ScrollSpeed = Value
            End If

            Me.tmrMain.Interval = Value * 10
            Invalidate()
        End Set
    End Property

    <Category("Marquee")> _
    <Description("Gets/Sets the color of the shadow text.")> _
    Public Property ShadowColor() As Color
        Get
            ShadowColor = _ShadowColor
        End Get
        Set(ByVal Value As Color)
            _ShadowColor = Value
            Invalidate()
        End Set
    End Property


    Private Sub ScrollingMarquee_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.HandleCreated
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
    End Sub

    Private Sub tmrMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMain.Tick
        If Me.Text.Length = 0 Then Exit Sub
        Invalidate()
    End Sub

    Private Sub trmBefore_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrBefore.Tick
        tmrMain.Enabled = True
        tmrBefore.Enabled = False
    End Sub

    Private Sub ScrollingMarquee_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Invalidate()
    End Sub

    Private Sub ScrollingMarquee_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        Invalidate()
    End Sub

    Private Sub ScrollingMarquee_TextChanged() Handles MyBase.TextChanged
        ' reset the text position so it can be fully visible again
        startPosition = 0
        ' leave the text static for _TimeBefore miliseconds
        tmrMain.Enabled = False
        tmrBefore.Enabled = True
    End Sub

    Private Overloads Sub OnPaint(ByVal sender As Object, ByVal e As PaintEventArgs)
        Dim str As String = Me.Text
        Dim g As Graphics = e.Graphics
        Dim szf As SizeF

        g.SmoothingMode = SmoothingMode.HighQuality
        szf = g.MeasureString(Me.Text, Me.Font)

        If _LeftToRight = Direction.Right Then
            If startPosition > Me.Width Then
                startPosition = -szf.Width
            Else
                startPosition += 1
            End If
        ElseIf _LeftToRight = Direction.Left Then
            If startPosition < -szf.Width Then
                startPosition = szf.Width
            Else
                startPosition -= 1
            End If
        End If
        Debug.WriteLine(startPosition)
        g.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), startPosition, 0 + (Me.Height / 2) - (szf.Height / 2))
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
