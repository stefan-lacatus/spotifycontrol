' this class will remember the hotkeys used by the application
' a better way to handle this should be found because it's too much code
Public Class HotKeyManager
    Dim _MainKey As Keys
    Dim _MainKeyModifier As Integer

    Public Property MainKey() As Keys
        Get
            Return _MainKey
        End Get
        Set(ByVal Value As Keys)
            _MainKey = Value
        End Set
    End Property
    Public Property MainKeyModifier() As Integer
        Get
            Return _MainKeyModifier
        End Get
        Set(ByVal Value As Integer)
            _MainKeyModifier = Value
        End Set
    End Property
End Class
