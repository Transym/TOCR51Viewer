'***********************************************************************************************************************
' Results Formatting form
' 
' This form provides some small control to the way in which text results are displayed.
' The intention is not to write a word processor but to just make the results more
' readable.

Public Class ResultsFormatting

#Region " Event handlers"

    Private Sub FormatResults_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
        'Show options according to last set
        Select Case ResultsFormatInfo.Fmt
            Case RESULTSFMT.None
                NoFormatRadioButton.Checked = True
            Case RESULTSFMT.Text
                TextFormatRadioButton.Checked = True
            Case RESULTSFMT.RTF
                RTFFormatRadioButton.Checked = True
        End Select
        CollapseCheckBox.Checked = ResultsFormatInfo.CollapseY
        LeftCheckBox.Checked = ResultsFormatInfo.LeftMargin
        XSpaceTextBox.Text = ResultsFormatInfo.MinXSpaceFac.ToString
        YSpaceTextBox.Text = ResultsFormatInfo.MinYSpaceFac.ToString

        UpdateControls()
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Dim Result As DialogResult = Windows.Forms.DialogResult.Cancel  ' button's dialog result
        Dim ResultsWindow As RESULTSVIEW
        Dim Fmt As RESULTSFMT

        Me.DialogResult = Windows.Forms.DialogResult.None

        If Not CheckInput(XSpaceTextBox) Then Return
        If Not CheckInput(YSpaceTextBox) Then Return

        If ResultsFormatInfo.ResultsWindow <> ResultsWindow Then
            ResultsFormatInfo.ResultsWindow = ResultsWindow
            Result = Windows.Forms.DialogResult.OK
        End If

        If NoFormatRadioButton.Checked Then Fmt = RESULTSFMT.None
        If TextFormatRadioButton.Checked Then Fmt = RESULTSFMT.Text
        If RTFFormatRadioButton.Checked Then Fmt = RESULTSFMT.RTF

        If ResultsFormatInfo.Fmt <> Fmt Then
            ResultsFormatInfo.Fmt = Fmt
            Result = Windows.Forms.DialogResult.OK
        End If
        If ResultsFormatInfo.CollapseY <> CollapseCheckBox.Checked Then
            ResultsFormatInfo.CollapseY = CollapseCheckBox.Checked
            Result = Windows.Forms.DialogResult.OK
        End If
        If ResultsFormatInfo.LeftMargin <> LeftCheckBox.Checked Then
            ResultsFormatInfo.LeftMargin = LeftCheckBox.Checked
            Result = Windows.Forms.DialogResult.OK
        End If
        If ResultsFormatInfo.MinXSpaceFac <> CSng(XSpaceTextBox.Text) Then
            ResultsFormatInfo.MinXSpaceFac = CSng(XSpaceTextBox.Text)
            Result = Windows.Forms.DialogResult.OK
        End If
        If ResultsFormatInfo.MinYSpaceFac <> CSng(YSpaceTextBox.Text) Then
            ResultsFormatInfo.MinYSpaceFac = CSng(YSpaceTextBox.Text)
            Result = Windows.Forms.DialogResult.OK
        End If

        Me.DialogResult = Result
    End Sub

    Private Sub ProcessedImageRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        UpdateControls()
    End Sub

    Private Sub ResultsRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        UpdateControls()
    End Sub

    Private Sub NoFormatRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NoFormatRadioButton.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub RTFFormatRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RTFFormatRadioButton.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub TextFormatRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextFormatRadioButton.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub XSpaceTextBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles XSpaceTextBox.GotFocus
        XSpaceTextBox.SelectAll()
    End Sub

    Private Sub XSpaceTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles XSpaceTextBox.KeyPress
        If Not ("0123456789." & ControlChars.Back).Contains(e.KeyChar) Then
            Beep()
            e.Handled = True
        End If
    End Sub

    Private Sub YSpaceTextBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles YSpaceTextBox.GotFocus
        YSpaceTextBox.SelectAll()
    End Sub

    Private Sub YSpaceTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles YSpaceTextBox.KeyPress
        If Not ("0123456789." & ControlChars.Back).Contains(e.KeyChar) Then
            Beep()
            e.Handled = True
        End If
    End Sub
#End Region

#Region " Private routines"
    Private Function CheckInput(ByVal Txt As TextBox) As Boolean
        If IsNumeric(Txt.Text) Then
            Return True
        Else
            MsgBox("Value must be numeric", MsgBoxStyle.Exclamation)
            Txt.Focus()
            Return False
        End If
    End Function

    Private Sub UpdateControls()

        NoFormatRadioButton.Enabled = True
        RTFFormatRadioButton.Enabled = True
        TextFormatRadioButton.Enabled = True
        CollapseCheckBox.Enabled = True
        LeftCheckBox.Enabled = True
        XSpaceLabel.Enabled = True
        XSpaceTextBox.Enabled = True
        YSpaceLabel.Enabled = True
        YSpaceTextBox.Enabled = True

        Dim Bool As Boolean     ' utility
        Bool = Not NoFormatRadioButton.Checked

        CollapseCheckBox.Enabled = Bool
        LeftCheckBox.Enabled = Bool
        XSpaceLabel.Enabled = Bool
        XSpaceTextBox.Enabled = Bool
        YSpaceLabel.Enabled = Bool
        YSpaceTextBox.Enabled = Bool

    End Sub
#End Region

End Class