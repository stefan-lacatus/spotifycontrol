' this class will remember the hotkeys used by the application
' a better way to handle this should be found because it's too much code
Public Class HotKeyManager
    Dim _MainKey, _MainKeyModifier As Keys
    Public Property MainKey() As Keys
        Get
            Return _MainKey
        End Get
        Set(ByVal Value As Keys)
            _MainKey = Value
        End Set
    End Property
    Public Property MainKeyModifier() As Keys
        Get
            Return _MainKeyModifier
        End Get
        Set(ByVal Value As Keys)
            _MainKeyModifier = Value
        End Set
    End Property
End Class
