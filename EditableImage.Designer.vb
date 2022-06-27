<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditableImage
    Inherits System.Windows.Forms.Control

    'Control overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not (BMP Is Nothing) Then BMP.Dispose()
        If Not (PasteBMP Is Nothing) Then PasteBMP.Dispose()
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Control Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  Do not modify it
    ' using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ScrollTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'ScrollTimer
        '
        Me.ScrollTimer.Interval = 50
        '
        'EditableImage
        '
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ScrollTimer As System.Windows.Forms.Timer

End Class

