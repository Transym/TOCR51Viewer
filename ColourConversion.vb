'***********************************************************************************************************************
' Colour Conversion Form
'
' This form allows the user to select the algorithm and threshold to use to convert a colour image(RGB) to
' monochrome.

Public Class ColourConversion

#Region " Definitions "
    Private BWX As Integer      ' X coordinate of top left pixel of monochrome image
    Private BWY As Integer      ' Y coordinate of top left pixel of monochrome image
    Private BWWidth As Integer  ' width of monochrome image
    Private BWHeight As Integer ' height of monochrome image
    Private AspectRatio As Single = 1.0F

    Private WithEvents Page As New EditableImage

    Private AlgoName() As String = {"Average", "Luma (BT.601)", "Luma (BT.709)", "Desaturation", "Decomposition (maximum)", "Decomposition (minimum)", "Red", "Green", "Blue"}
    Private AlgoFormula() As String = {"(R+G+3)/3", "0.299*R + 0.587*G + 0.114*B", "0.2126*R + 0.7152*G + 0.0722*B", "(max(R,G,B) + min(R,G,B))/2", "max(R,G,B)", "min(R,G,B)", "R", "G", "B"}
#End Region

#Region " Event Handlers "
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'ConvertBitmap(BMPClr, Imaging.PixelFormat.Format32bppRgb) ' To get rid of any transparency
        'ConvertBitmap(BMPClr, FastRenderFormat)

        'TODO: Set up correct variable and processes to save and use
        'Show options according to last set
        'Select Case GCAlgorithm.Fmt
        '    Case GCAlgorithm.None
        HistogramCGRadioButton.Checked = True
        '    Case GCAlgorithm.Mean
        '        MeanCCRadioButton.Checked = True
        '    Case GCAlgorithm.Histogram
        '        HistogramCCRadioButton.Checked = True
        'End Select

        Me.ClrPanel.Controls.Add(Page)
        ToolTip1.SetToolTip(Page, "Colour image")

        Page.Image = Viewer.Page.Image
        Page.Dock = DockStyle.Fill
        Page.Visible = True
        Page.AllowSelect = False
        Page.RaisePaintedEvent = True

        Dim Hres As Single = Page.Image.HorizontalResolution
        Dim Vres As Single = Page.Image.VerticalResolution

        If Hres > 0 And Hres > 0 Then
            AspectRatio = Hres / Vres
        Else
            AspectRatio = 1.0
        End If
        If Page.VScrollbar.Visible Then
            BWWidth = Math.Min(Page.Image.Width, Page.Width - Page.VScrollbar.Width)
        Else
            BWWidth = Math.Min(Page.Image.Width, Page.Width)
        End If
        If Page.HScrollbar.Visible Then
            BWHeight = Math.Min(CInt(Page.Image.Height * AspectRatio), Page.Height - Page.HScrollbar.Height)
        Else
            BWHeight = Math.Min(CInt(Page.Image.Height * AspectRatio), Page.Height)
        End If

        ThresholdHScrollBar.Value = CInt(JobInfo_EG.ProcessOptions.CCThreshold * 100)
        AlgoNUD.Value = JobInfo_EG.ProcessOptions.CCAlgorithm

        If JobInfo_EG.ProcessOptions.CGAlgorithm > 0 Then
            ColourTabControl.SelectTab(1)

            Select Case JobInfo_EG.ProcessOptions.CGAlgorithm
                Case TOCRJOBCC_HISTOGRAM
                    HistogramCGRadioButton.Checked = True
                Case TOCRJOBCC_REGIONS
                    RegionCGRadioButton.Checked = True
                Case TOCRJOBCC_MEAN
                    MeanCGRadioButton.Checked = True
            End Select
        Else
            ColourTabControl.SelectTab(0)
        End If

        Call AlgoNUD_ValueChanged(sender, e)
    End Sub

    Private Sub AlgoNUD_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlgoNUD.ValueChanged
        If "ThresholdTabPage" = ColourTabControl.SelectedTab.Name Then
            AlgoNameLabel.Text = AlgoName(GetThresholdAlgoNum())
            AlgoFormulaLabel.Text = "White is  " & AlgoFormula(CInt(AlgoNUD.Value)) & " >= Threshold% * 255"
        End If

        RepaintBWPanel()
    End Sub

    Private Sub BWPanel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles BWPanel.Paint
        RepaintBWPanel()
    End Sub

    Private Function GetThresholdAlgoNum() As Integer
        Dim result As Integer
        result = CInt(AlgoNUD.Value)
        Return result
    End Function

    Private Function GetAutoAlgoNum() As Integer
        Dim result As Integer
        If "ThresholdTabPage" = ColourTabControl.SelectedTab.Name Then
            result = 0
        Else
            If HistogramCGRadioButton.Checked = True Then
                result = TOCRJOBCC_HISTOGRAM
            ElseIf RegionCGRadioButton.Checked = True Then
                result = TOCRJOBCC_REGIONS
            ElseIf MeanCGRadioButton.Checked = True Then
                result = TOCRJOBCC_MEAN
            Else
                result = 0
            End If
        End If

        Return result
    End Function

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OKButton.Click
        With JobInfo_EG.ProcessOptions
            .CCAlgorithm = GetThresholdAlgoNum()
            .CCThreshold = CSng(ThresholdHScrollBar.Value / 100)
            .CGAlgorithm = GetAutoAlgoNum()
        End With
    End Sub

    Private Sub Page_Painted(ByVal Left As Integer, ByVal Top As Integer) Handles Page.Painted
        BWX = -Left
        BWY = -Top
        RepaintBWPanel()
    End Sub

    Private Sub ThresholdHScrollBar_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ThresholdHScrollBar.ValueChanged
        RepaintBWPanel()
    End Sub
#End Region

#Region " Private Routines "
    Private Sub RepaintBWPanel()
        Dim BMPBw As New Bitmap(BWWidth, CInt(BWHeight / AspectRatio), Page.Image.PixelFormat)
        Dim FG As Graphics = Graphics.FromImage(BMPBw)

        FG.PageUnit = GraphicsUnit.Pixel
        FG.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighSpeed
        FG.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        FG.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        FG.DrawImage(Page.Image, New RectangleF(0, 0, BWWidth, CInt(BWHeight / AspectRatio)), New RectangleF(-BWX, -BWY, BWWidth, CInt(BWHeight / AspectRatio)), GraphicsUnit.Pixel)
        FG.Dispose()

        ThresholdLabel.Text = ThresholdHScrollBar.Value.ToString & "%"
        ConvertBitmap2BW(BMPBw, CInt(AlgoNUD.Value), CSng(ThresholdHScrollBar.Value / 100))

        Dim EG As Graphics = BWPanel.CreateGraphics
        EG.PageUnit = GraphicsUnit.Pixel
        EG.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighSpeed
        EG.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        EG.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        EG.DrawImage(BMPBw, New RectangleF(0, 0, BWWidth, CInt(BWHeight)), New RectangleF(0, 0, BWWidth, CInt(BWHeight / AspectRatio)), GraphicsUnit.Pixel)
        EG.Dispose()

        BMPBw.Dispose()
    End Sub
#End Region

    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles AutomaticTabPage.Click

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles AutomaticPanel.Paint

    End Sub
End Class