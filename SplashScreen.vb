'***********************************************************************************************************************
' Viewer form
' 
' This is the main form for the application
Option Strict Off
Option Explicit On

Friend Class SplashScreen
    Inherits System.Windows.Forms.Form

    ' Use a timer rather than VS SplashScreen because SplashScreen is TopMost and that puts the form
    ' above any popup error messages that may occur at startup
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Me.Close()
    End Sub

    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class