'***********************************************************************************************************************
' Adjust DPI form
' 
' This form allows the user to change the Dots Per Inch of an Image.

Public Class AdjustPPM

#Region " Definitions "
    Private MinDPI As Integer           ' maximum Dots Per Inch
    Private MaxDPI As Integer           ' minimum Dots Per Inch
#End Region

#Region " Event handlers "
    Private Sub AdjustPPM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim FValue As Single        ' utility

        OKButton.DialogResult = Windows.Forms.DialogResult.None ' in case it has been set in the design window

        XDPIComboBox.Text = CStr(XDPI)
        YDPIComboBox.Text = CStr(YDPI)

        FValue = TOCRMAXPPM * 2.54 / 100
        MaxDPI = CInt(FValue)
        If FValue < MaxDPI Then MaxDPI -= 1

        FValue = TOCRMINPPM * 2.54 / 100
        MinDPI = CInt(FValue)
        If FValue > MinDPI Then MinDPI += 1

        ' GDI+ will not allow 0 resolution
        'InfoLabel.Text = "Please enter values for the X and Y resolutions between " & CStr(MinDPI) & " and " & CStr(MaxDPI) & ", or 0 dots per inch"
        InfoLabel.Text = "Please enter values for the X and Y resolutions between " & CStr(MinDPI) & " and " & CStr(MaxDPI) & " dots per inch"
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Dim Result As DialogResult = Windows.Forms.DialogResult.Cancel  ' button's dialog result
        Dim Value As Integer                                            ' utility

        Me.DialogResult = Windows.Forms.DialogResult.None

        If Not ChkDPI(XDPIComboBox) Then Return
        If Not ChkDPI(YDPIComboBox) Then Return

        Value = CInt(XDPIComboBox.Text)
        If Value <> 0 Then Value = Math.Max(Math.Min(Value, MaxDPI), MinDPI)
        If XDPI <> Value Then
            XDPI = Value
            Result = Windows.Forms.DialogResult.OK
        End If

        Value = CInt(YDPIComboBox.Text)
        If Value <> 0 Then Value = Math.Max(Math.Min(Value, MaxDPI), MinDPI)
        If YDPI <> Value Then
            YDPI = Value
            Result = Windows.Forms.DialogResult.OK
        End If

        Me.DialogResult = Result
    End Sub

    Private Sub XDPIComboBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles XDPIComboBox.GotFocus
        XDPIComboBox.SelectAll()
    End Sub

    Private Sub XDPIComboBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles XDPIComboBox.KeyPress
        If Not ("0123456789" & ControlChars.Back).Contains(e.KeyChar) Then
            Beep()
            e.Handled = True
        End If
    End Sub

    Private Sub YDPIComboBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles YDPIComboBox.GotFocus
        YDPIComboBox.SelectAll()
    End Sub

    Private Sub YDPIComboBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YDPIComboBox.KeyPress
        If Not ("0123456789." & ControlChars.Back).Contains(e.KeyChar) Then
            Beep()
            e.Handled = True
        End If
    End Sub
#End Region

#Region " Private routines"
    Private Function ChkDPI(ByVal cmb As ComboBox) As Boolean
        Dim Value As Integer        ' utility

        Value = CInt(cmb.Text)

        ' GDI+ will not allow 0 resolution
        'If Value <> 0 And (Value < MinDPI Or Value > MaxDPI) Then
        If Value < MinDPI Or Value > MaxDPI Then
            MsgBox("Value must 0 or be greater than " & CStr(MinDPI - 1) & " and less than " & CStr(MaxDPI + 1), MsgBoxStyle.Exclamation)
            cmb.Focus()
            Return False
        Else
            Return True
        End If
    End Function
#End Region

End Class