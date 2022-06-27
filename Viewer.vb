'***********************************************************************************************************************
' Viewer form
'
' This is the main form for the application

Imports System.Runtime.InteropServices

Public Class Viewer

#Region " Definitions "
    ' The use of colour images results in a very large increase in memory
    ' This parameter can be used to revert the Viewer program to monochrome
    Private SaveMemory As Boolean = False

    Private CurrentFile As String           ' currently loaded file or blank
    Private CurrentPage As Integer = 0      ' current page in a multipage file
    Private NumPages As Integer = 0         ' number of pages in a multipage file
    Private HaveSelection As Boolean = False ' flag if the user has selected an aree of the image
    Private HaveImage As Boolean = False    ' flag if an image is loaded
    Private HaveClipboardImage As Boolean   ' flag if an image is on the clipboard (see ClipboardMonitorClass)
    Private TextEdited As Boolean = False   ' flag if user has edited the text results
    Private ResultsEx_EG As TOCRRESULTSEX_EG ' OCR Results - also used to flag if have any results
    Private ImageZooms(0 To 0) As Single     ' zoom of each image loaded (percentage)
    Private ResultsZooms(0 To 0) As Single   ' zoom of each results page (percentage)

    Private Enum OCRSTATES As Integer       ' state of the OCR engine
        Idle = 0        ' engine is idle
        Active = 1      ' engine is working
        Abort = 2       ' user has requested an abort
        AppClose = 3    ' application is closing
    End Enum

    Private OCRState As OCRSTATES = OCRSTATES.Idle
    Friend WithEvents Page As New EditableImage                        ' class to display the current image
    Private WithEvents NUD As New System.Windows.Forms.NumericUpDown    ' page up/down control
    Private WithEvents ClipMon As New ClipboardMonitor                  ' monitors the clipboard for images
#End Region

#Region " Event handlers "
    ' User has clicked on Help/About
    Private Sub AboutTOCRViewerMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutTOCRViewerMenuItem.Click
        About.ShowDialog()
    End Sub

    ' User has clicked on on Image/AdjustDPI
    Private Sub AdjustDPIMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdjustDPIMenuItem.Click
        XDPI = CInt(Page.Image.HorizontalResolution)
        YDPI = CInt(Page.Image.VerticalResolution)

        AdjustPPM.ShowDialog()
        If AdjustPPM.DialogResult = Windows.Forms.DialogResult.OK Then
            Page.Image.SetResolution(CSng(XDPI), CSng(YDPI))
            Page.Modified = True
            Page.Refresh()
        End If
    End Sub

    ' User has clicked on File/Acquire Images
    Private Sub AcquireImagesMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AcquireImagesMenuItem.Click
        Dim NumImages As Integer = 0        ' number of images acquired
        Dim PageNo As Integer               ' loop counter
        Dim I As Integer                    ' utility
        Dim CurrentCursor As Cursor = Cursor

        If KeepTextChanges() Then Return

        Cursor = Cursors.WaitCursor
        Try
            TOCRTWAINAcquire(NumImages)
        Catch ex As Exception
            If Err.Number = ERRCANTFINDDLLENTRYPOINT Then
                Cursor = CurrentCursor
                Call NoTwain()
                Return
            End If
        End Try

        If NumImages > 0 Then
            CurrentFile = ""
            FileNameLabel.Text = ""
            Try
                ' Remove all controls except the Focus picture box
                Dim HaveEI As Boolean
                Do
                    HaveEI = False
                    Dim Cntrl As Control
                    For Each Cntrl In SplitContainer1.Panel1.Controls
                        If TypeOf Cntrl Is EditableImage Then
                            Cntrl.Dispose()
                            HaveEI = True
                        End If
                    Next Cntrl
                Loop While HaveEI

                Dim hMem(0 To NumImages - 1) As IntPtr
                Dim MemGC As GCHandle
                MemGC = GCHandle.Alloc(hMem, GCHandleType.Pinned)
                TOCRTWAINGetImages(MemGC.AddrOfPinnedObject)
                MemGC.Free()

                NumPages = 0
                NUD.Value = 1
                CurrentPage = 0
                NUD.Maximum = NumImages
                NumPages = NumImages

                ReDim ImageZooms(0 To NumPages - 1)
                ReDim ResultsZooms(0 To NumPages - 1)

                For PageNo = 0 To NumPages - 1
                    If NumPages > 1 Then
                        FileNameLabel.Text = "Loading image " & (PageNo + 1).ToString
                        StatusBar.Refresh()
                        Application.DoEvents()
                    End If

                    Dim P As New EditableImage
                    P.Dock = DockStyle.Fill
                    P.NormaliseAspectRatio = True

                    P.UseFastRenderFormat = (NumPages = 1)
                    P.ConvertToMonochrome = True
                    P.DoubleBuffered = True

                    Dim bmp As Bitmap
                    bmp = ConvertMemoryBlockToBitmap(hMem(I))
                    KRN.GlobalFree(hMem(I))
                    P.Image = bmp

                    P.Name = CStr(PageNo) ' so can use IndexOfKey method to refer to page

                    ImageZooms(PageNo) = Page.Zoom
                    ResultsZooms(PageNo) = ResultsRichTextBox.ZoomFactor * 100

                    P.Zoom = Page.Zoom
                    If NumPages > 1 Then
                        P.Tag = "Acquired image " & CStr(PageNo + 1) & " of " & CStr(NumPages)
                    Else
                        P.Tag = "Acquired image"
                    End If
                    P.ContextMenuStrip = ImageContextMenu
                    P.Refresh()
                    SplitContainer1.Panel1.Controls.Add(P)
                Next
                ImageFocusPictureBox.SendToBack()

                HaveImage = True
                ChangePage()

            Catch ex As Exception
                HaveImage = False
                NumPages = 0
                TOCRTWAINGetImages(IntPtr.Zero)
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try

            ResultsRichTextBox.Text = ""
            ResultsEx_EG.Hdr.NumItems = 0
            TextEdited = False

            UpdateControls()
            GC.Collect()
        End If
        Cursor = CurrentCursor
    End Sub

    ' User has clicked on File/Batch
    Private Sub BatchMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BatchMenuItem.Click
        Batch.ShowDialog()

        On Error Resume Next
        TOCRTWAINShowUI(CShort(ShowDeviceUIMenuItem.Checked)) ' batch may have changed this
        If Err.Number = ERRCANTFINDDLLENTRYPOINT Then Call NoTwain(True)
    End Sub

    ' Clear Selection action
    Private Sub ClearMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearMenuItem.Click, ClearPopMenuItem.Click, ClearButton.Click
        Page.Clear()
    End Sub

    ' Called when the contents of clipboard changes
    Private Sub ClipMon_Changed() Handles ClipMon.Changed
        HaveClipboardImage = Clipboard.ContainsImage() ' Doesn't work with GDI images
        If HaveImage Then
            PasteMenuItem.Enabled = HaveClipboardImage
            PasteButton.Enabled = HaveClipboardImage
        Else
            PasteMenuItem.Enabled = False
            PasteButton.Enabled = False
        End If
    End Sub

    Private Sub ColourConversionMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColourConversionMenuItem.Click
        ColourConversion.showdialog()
    End Sub

    ' Copy Image/Selection action
    Private Sub CopyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyMenuItem.Click, CopyPopMenuItem.Click, CopyButton.Click
        Page.Copy()
    End Sub

    ' Copy selected text
    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        ResultsRichTextBox.Copy()
    End Sub

    ' Cut Image/Selection action
    Private Sub CutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutMenuItem.Click, CutPopMenuItem.Click, CutButton.Click
        Page.Cut()
    End Sub

    ' Cut selected text
    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        ResultsRichTextBox.Cut()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        ResultsRichTextBox.SelectedText = ""
    End Sub

    ' User has clicked on File/Exit
    Private Sub ExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenuItem.Click
        Me.Close()
    End Sub

    ' User is typing in the text region of the image Zoom combo box
    Private Sub ImageZoomComboBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ImageZoomComboBox.KeyDown
        If e.KeyCode = Keys.Return Then
            If Not ImageZoomComboBox.Text.EndsWith("%") Then ImageZoomComboBox.Text &= "%"
            e.SuppressKeyPress = True
            ChangeImageZoom()
        Else
            If Not (e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Or _
                e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Or _
                (e.KeyCode >= 48 And e.KeyCode <= 57)) Then
                Beep()
                e.SuppressKeyPress = True
            End If
        End If
    End Sub

    ' User has changed image zoom
    Private Sub ImageZoomComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageZoomComboBox.SelectedIndexChanged
        ChangeImageZoom()
    End Sub

    ' Invert Image/Selection action
    Private Sub InvertMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles InvertMenuItem.Click, InvertPopMenuItem.Click, InvertButton.Click
        Page.Invert()
    End Sub

    ' Flip Image/Selection horizontally action
    Private Sub FlipHorizontallyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FlipHorizontallyMenuItem.Click, FlipHorizontallyPopMenuItem.Click, FlipHorizontallyButton.Click
        Page.RotateFlip(RotateFlipType.RotateNoneFlipX)
    End Sub

    ' Flip Image/Selection vertically action
    Private Sub FlipVerticallyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FlipVerticallyMenuItem.Click, FlipVerticallyPopMenuItem.Click, FlipVerticallyButton.Click
        Page.RotateFlip(RotateFlipType.RotateNoneFlipY)
    End Sub

    ' User has clicked on View/Focus bars
    Private Sub FocusbarsMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FocusbarsMenuItem.Click
        ImageFocusPictureBox.Visible = Not ImageFocusPictureBox.Visible
        ResultsFocusPictureBox.Visible = ImageFocusPictureBox.Visible
        FocusbarsMenuItem.Checked = ImageFocusPictureBox.Visible
    End Sub

    ' User has clicked on Help/Help Topics
    Private Sub HelpTopicsMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HelpTopicsMenuItem.Click
        Help.ShowHelp(Me, "TOCRVIEWER.CHM", HelpNavigator.TableOfContents)
        ' this also works
        'WinHelp(Me.Handle, "TOCRVIEWER.CHP", 11, 0)
    End Sub

    ' User has clicked on Options/Lock zoom.  This will ensure that in a multipage scanario all images and resultswill have
    ' the same zoom.
    Private Sub LockZoomMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LockZoomMenuItem.Click
        LockZoomMenuItem.Checked = Not LockZoomMenuItem.Checked
        If Not LockZoomMenuItem.Checked Then
            ResultsZoomComboBox.Text = ResultsZooms(CurrentPage).ToString & "%"
            If HaveImage Then
                ImageZoomComboBox.Text = ImageZooms(CurrentPage).ToString & "%"
            End If
        End If
    End Sub

    ' User has clicked on the Page Number up/down control
    Private Sub NUD_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUD.ValueChanged
        If NumPages > 0 Then
            If NUD.Value = 0 Then
                NUD.Value = NumPages
                Return
            End If
            If NUD.Value > NumPages Then
                NUD.Value = 1
                Return
            End If
            If CurrentPage <> CType(NUD.Value, Integer) - 1 Then
                If KeepTextChanges() Then
                    NUD.Value = CurrentPage + 1
                    Return
                End If
                ResultsRichTextBox.Text = ""
                ResultsEx_EG.Hdr.NumItems = 0
                TextEdited = False
                CurrentPage = CType(NUD.Value, Integer) - 1
                ChangePage()
            End If
        End If
    End Sub

    Private Sub DisplayProcessedImageDialog(ByRef MMF As System.IntPtr)
        Dim bmp As Bitmap

        bmp = ConvertMMFToBitmap(MMF)
        If Not IsNothing(bmp) Then
            'Dim EdPanel As New EditableImage

            ProcessedImage.EdPanel.Dock = DockStyle.Fill
            ProcessedImage.EdPanel.NormaliseAspectRatio = True

            If SaveMemory Then
                ' The EditableImageControl uses the mean Vverage algorithm for converting to monochrome
                ProcessedImage.EdPanel.UseFastRenderFormat = (NumPages = 1)
                ProcessedImage.EdPanel.ConvertToMonochrome = True
            Else
                ProcessedImage.EdPanel.UseFastRenderFormat = True
                ProcessedImage.EdPanel.ConvertToMonochrome = False
            End If
            ProcessedImage.EdPanel.DoubleBuffered = True

            ' Remove alpha channel
            If (bmp.PixelFormat And Imaging.PixelFormat.Alpha) <> 0 Or (bmp.PixelFormat And Imaging.PixelFormat.PAlpha) <> 0 Then
                ConvertBitmap(bmp, Imaging.PixelFormat.Format32bppRgb) ' To get rid of any transparency
                ConvertBitmap(bmp, ProcessedImage.EdPanel.FastRenderFormat)
            End If

            ProcessedImage.EdPanel.Image = bmp
            ProcessedImage.EdPanel.Zoom = Page.Zoom
            ProcessedImage.EdPanel.Refresh()
            '            ProcessedImage.Panel1.Controls.Add(P) ' Add processed image to Panel in dialog
            ' ProcessedImage.Controls.Add(P) ' Add processed image to Panel in dialog
            ProcessedImage.ShowDialog()
        Else
            MsgBox("No processed image to display")
        End If


    End Sub

    Private Sub DisplayProcessedImage(ByRef MMF As System.IntPtr)
        Dim bmp As Bitmap
        Dim PageNo As Integer
        PageNo = NumPages

        bmp = ConvertMMFToBitmap(MMF)
        If Not IsNothing(bmp) Then
            Dim P As New EditableImage

            P.Dock = DockStyle.Fill
            P.NormaliseAspectRatio = True

            If SaveMemory Then
                ' The EditableImageControl uses the mean Average algorithm for converting to monochrome
                P.UseFastRenderFormat = (NumPages = 1)
                P.ConvertToMonochrome = True
            Else
                P.UseFastRenderFormat = True
                P.ConvertToMonochrome = False
            End If
            P.DoubleBuffered = True
            NumPages = NumPages + 1
            ' Remove alpha channel
            If (bmp.PixelFormat And Imaging.PixelFormat.Alpha) <> 0 Or (bmp.PixelFormat And Imaging.PixelFormat.PAlpha) <> 0 Then
                ConvertBitmap(bmp, Imaging.PixelFormat.Format32bppRgb) ' To get rid of any transparency
                ConvertBitmap(bmp, P.FastRenderFormat)
            End If

            P.Image = bmp

            P.Name = CStr(PageNo)

            ReDim Preserve ImageZooms(0 To NumPages - 1)
            ReDim Preserve ResultsZooms(0 To NumPages - 1)

            ImageZooms(PageNo) = Page.Zoom
            ResultsZooms(PageNo) = ResultsRichTextBox.ZoomFactor * 100
            ResultsZooms(PageNo) = ResultsRichTextBox.ZoomFactor * 100
            P.Zoom = Page.Zoom
            'Set the label of the image for paging
            If NumPages > 1 Then
                P.Tag = "Actual image processed by TOCR service"
            Else
                P.Tag = "No Processed Page returned"
            End If
            P.ContextMenuStrip = ImageContextMenu
            P.Refresh()
            SplitContainer1.Panel1.Controls.Add(P) ' Add new page control to dialog screen

            CurrentPage = NumPages - 1
            NUD.Maximum = NumPages + 1

            'Check if off, as may need to enable and set maximum
            If NumPages > 1 Then
                If CurrentPage > NumPages - 1 Then
                    CurrentPage = 0 ' Set back to zero if gone over limit
                End If
            End If
            NUD.Value = CurrentPage + 1

            ChangePage()
            'Set the Load message, and upload to screen
            FileNameLabel.Text = "Loading actual image processed by TOCR service ..."
            StatusBar.Refresh()
            Application.DoEvents() ' Puts the latest image on screen
            FileNameLabel.Text = CType(Page.Tag, String)

        Else
            'BlankPages = BlankPages + 1
            FileNameLabel.Text = "Page " & (PageNo + 1).ToString & " is blank"
            StatusBar.Refresh()
            Application.DoEvents()
        End If

        ImageFocusPictureBox.SendToBack()

    End Sub


    Private Sub Show_PI_image()
        'Code to present the procesed image
        Dim MMF As System.IntPtr
        If ExtraInfGetMMF(JobNo, MMF) Then
            DisplayProcessedImageDialog(MMF) 'If want in pop-up 
        Else
            'Error - no image returned from TOCR
            Err.Description = "Failed to extract the processed image"
            If OCRState = OCRSTATES.Active Then OCRState = OCRSTATES.Abort
        End If
    End Sub

    Private Sub Show_OCR_Results()
        'Run the original code to present the results
        If GetResults_EG(JobNo, ResultsEx_EG) Then
            ' Trace.WriteLine("Done - GetResultsEG")
            TextEdited = False

            If ResultsEx_EG.Hdr.NumItems > 0 Then
                ResultsFontMenuItem.Enabled = False   ' don't allow the font to change while processing
                ResultsFormattingMenuItem.Enabled = False
                ResultsEx_EG.Hdr.YPixelsPerInch = CInt(Page.Image.VerticalResolution)
                FormatResults(ResultsEx_EG, Me.ResultsRichTextBox)
                ResultsFontMenuItem.Enabled = True
                ResultsFormattingMenuItem.Enabled = True
            Else
                ResultsRichTextBox.Text = ""
                ' Trace.WriteLine("Done - ResultsRichTextBox")
                MsgBox("No characters identified", MsgBoxStyle.Information)
                'Trace.WriteLine("Done - MsgBox")
            End If
        Else
            If OCRState = OCRSTATES.Active Then OCRState = OCRSTATES.Abort
        End If
    End Sub

    Private Sub TOCR_Btn_Click_Util(BtnID As Integer)
        Dim SendFile As Boolean                 ' flag if to send the OCR a file name or memory mapped file handle
        Dim FileType As String                  ' type of image file BMP or TIFF
        Dim PageNo As Integer                   ' loop counter
        Dim I As Integer                        ' utility
        Dim EI As EditableImage                 ' utility
        Dim MMFhandle As IntPtr = IntPtr.Zero   ' handle to memory mapped file
        Dim Status As Integer                   ' engine return status
        Dim JobStatus As Integer                ' status of OCR job
        Dim ExtraInfFlags As Integer            ' status flags for extra information
        Dim Progress As Single                  ' % progress of OCR
        Dim AutoRotated As Boolean              ' flag if engine has auto rotated the image
        Dim AutoOrientation As Integer = 0      ' amount of autorotation (90, 180 or 270 degrees)
        Dim UnexpectedError As Boolean = True   ' flag for unexpected errors

        If Not HaveImage Then Return
        If KeepTextChanges() Then Return
        Trace.WriteLine("Done - KeepTextChanges")
        ' Check DPI are within limits

        XDPI = CInt(Page.Image.HorizontalResolution)
        YDPI = CInt(Page.Image.HorizontalResolution)
        ' GDI+ will not allow 0 resolution
        'If (XDPI <> 0 And (XDPI < TOCRMINPPM * 2.54 / 100 Or XDPI > TOCRMAXPPM * 2.54 / 100)) Or _
        '   (YDPI <> 0 And (YDPI < TOCRMINPPM * 2.54 / 100 Or YDPI > TOCRMAXPPM * 2.54 / 100)) Then
        If (XDPI < TOCRMINPPM * 2.54 / 100 Or XDPI > TOCRMAXPPM * 2.54 / 100) Or _
           (YDPI < TOCRMINPPM * 2.54 / 100 Or YDPI > TOCRMAXPPM * 2.54 / 100) Then
            AdjustPPM.ShowDialog()
            If AdjustPPM.DialogResult = Windows.Forms.DialogResult.OK Then
                ' Set the resolution on all pages
                For PageNo = 0 To NumPages - 1
                    I = SplitContainer1.Panel1.Controls.IndexOfKey(CStr(PageNo))
                    EI = CType(SplitContainer1.Panel1.Controls.Item(I), EditableImage)
                    EI.Image.SetResolution(CSng(XDPI), CSng(YDPI))
                Next
                Page.Modified = True
            Else
                Return
            End If
        End If

        SendFile = False
        If (Not Page.Modified) And (Not HaveSelection) Then
            FileType = DetectFileType(CurrentFile)
            SendFile = True

            Select Case FileType
                Case "BMP"
                    JobInfo_EG.JobType = TOCRJOBTYPE_DIBFILE
                Case "TIF"
                    JobInfo_EG.JobType = TOCRJOBTYPE_TIFFFILE
                Case "PDF"
                    JobInfo_EG.JobType = TOCRJOBTYPE_PDFFILE
                Case Else
                    SendFile = False
            End Select
        End If

        If Not SendFile Then
            JobInfo_EG.JobType = TOCRJOBTYPE_MMFILEHANDLE
            JobInfo_EG.PageNo = 0
            If HaveSelection Then
                MMFhandle = ConvertBitmapToMMF(Page.SelectionImage)
            Else
                MMFhandle = ConvertBitmapToMMF(Page.Image)
            End If
            If MMFhandle.Equals(IntPtr.Zero) Then
                MsgBox("Failed to create a memory mapped file", MsgBoxStyle.Exclamation)
                Return
            End If
            JobInfo_EG.hMMF = MMFhandle
        Else
            JobInfo_EG.InputFile = CurrentFile
            JobInfo_EG.PageNo = CInt(Page.Name)
        End If

        On Error GoTo OCRERRS

        StatusLabel.Text = "Initialising"
        StatusLabel.Visible = True
        AutoRotated = False
        OCRState = OCRSTATES.Active
        UpdateControls()
        JobInfo_EG.ProcessOptions.ResultsReference = TOCRRESULTSREFERENCE_AFTER


        ' DO YOU NEED TO PIN THE HANDLE
        ' Setting the resolutions to zero causes an error
        ' why are the resolutions zero
        JobInfo_EG.ProcessOptions.ExtraInfFlags = 1

        If TOCRDoJob_EG(JobNo, JobInfo_EG) = TOCR_OK Then
            If Not MMFhandle.Equals(IntPtr.Zero) Then
                KRN.CloseHandle(MMFhandle)
                MMFhandle = IntPtr.Zero
            End If
            Do
                Status = TOCRGetJobStatusEx2(JobNo, JobStatus, Progress, AutoOrientation, ExtraInfFlags)

                ' Just Doevents absorbs too much CPU whilst Sleep
                ' alone will lock the application - so this is a compromise

                Application.DoEvents() : System.Threading.Thread.Sleep(100) : Application.DoEvents()

                ' Application is closing

                If OCRState = OCRSTATES.AppClose Then Return

                If Status = TOCR_OK Then
                    If Progress <= 1 Then StatusLabel.Text = _
                        Format$(Progress * 100, "0.0") & "%"

                    ' Check for and mimic auto rotation

                    If Not AutoRotated And (AutoOrientation <> 0) Then
                        If AutoOrientation = TOCRJOBORIENT_90 Then Page.RotateFlip(RotateFlipType.Rotate90FlipNone) : Page.Modified = True
                        If AutoOrientation = TOCRJOBORIENT_180 Then Page.RotateFlip(RotateFlipType.Rotate180FlipNone) : Page.Modified = True
                        If AutoOrientation = TOCRJOBORIENT_270 Then Page.RotateFlip(RotateFlipType.Rotate270FlipNone) : Page.Modified = True
                        AutoRotated = True
                    End If
                Else
                    StatusLabel.Text = "FAILED"
                End If
            Loop While Status = TOCR_OK And JobStatus = TOCRJOBSTATUS_BUSY And OCRState = OCRSTATES.Active
        End If

        If (Status = TOCR_OK) And (JobStatus = TOCRJOBSTATUS_DONE) And (OCRState = OCRSTATES.Active) Then
            If (0 = BtnID) Then
                Show_OCR_Results()
            Else
                Show_PI_image()
            End If
        End If

        UnexpectedError = False
        ' Trace.WriteLine("Done - UnexpectedError")

OCRERRS:
        If Not MMFhandle.Equals(IntPtr.Zero) Then
            KRN.CloseHandle(MMFhandle)
            MMFhandle = IntPtr.Zero
        End If

        If OCRState = OCRSTATES.AppClose Then Return

        If UnexpectedError Or OCRState = OCRSTATES.Abort Then
            TOCRShutdown(TOCRSHUTDOWNALL)
#If RESELLER1 Then
            TOCRInitialise(JobNo, RegNo)
#Else
            TOCRInitialise(JobNo)
#End If
        End If

        OCRState = OCRSTATES.Idle

        ResultsFontMenuItem.Enabled = True
        ResultsFormattingMenuItem.Enabled = True

        TextEdited = False

        UpdateControls()
        StatusLabel.Visible = False

        If Err.Number <> 0 Then
            MsgBox("Unexpected VB error" & Environment.NewLine & Environment.NewLine & Err.Description, MsgBoxStyle.Exclamation)
            Return
        End If

        Return

    End Sub

    ' OCR Image/Selection action
    Private Sub OCRImageMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OCRImageMenuItem.Click, OCRButton.Click
        TOCR_Btn_Click_Util(0)
    End Sub

    ' Open a dialogue to load a file action
    Private Sub OpenButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenButton.Click, OpenMenuItem.Click
        Dim OpenFileDialog As New OpenFileDialog
        Static FilterIndex As Integer

        If KeepTextChanges() Then Return

        With OpenFileDialog
            .InitialDirectory = OpenInitDir
            .Title = "Select Input File"
            .FilterIndex = FilterIndex
            .Filter = "Bitmaps (*.bmp, *.dib, *.gif, *.jpeg, *.jpg, *.png, *.tif, *.tiff, *.pdf)|*.bmp;*.dib;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff;*.pdf|All Files (*.*)|*.*"
            .FileName = ""
            .CheckFileExists = True
            .CheckPathExists = True
            .ShowReadOnly = False
            .Multiselect = False
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                CurrentFile = .FileName
                OpenInitDir = New System.IO.FileInfo(.FileName).DirectoryName
                OpenFile()
                FilterIndex = .FilterIndex
            End If
            .Dispose()
        End With
    End Sub

    ' Change the focus bar to indicate the image has got focus
    Private Sub Page_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Page.GotFocus
        ImageFocusPictureBox.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
    End Sub

    Private Sub Page_ImageModified() Handles Page.ImageModified
        If Not FileNameLabel.Text.StartsWith("Modified ") Then
            Page.Tag = "Modified " & CType(Page.Tag, String)
            FileNameLabel.Text = CType(Page.Tag, String)
        End If
    End Sub

    Private Sub Page_ImageMouseMove(ByVal P As System.Drawing.Point) Handles Page.ImageMouseMove
        MouseCoordsLabel.Text = CStr(P.X) & "," & CStr(P.Y)
    End Sub

    ' Change the focus bar to indicate the image has lost focus
    Private Sub Page_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Page.LostFocus
        ImageFocusPictureBox.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption)
    End Sub

    Private Sub Page_SelectionChanged(ByVal rect As System.Drawing.Rectangle) Handles Page.SelectionChanged
        Dim Bool As Boolean         ' utility

        If rect.Width <> 0 Then
            SelectRectLabel.Text = CStr(rect.X) & "," & CStr(rect.Y) & " " & CStr(rect.Width) & "x" & CStr(rect.Height)
            ImageMenu.Text = "&Selection"
        Else
            SelectRectLabel.Text = ""
            ImageMenu.Text = "&Image"
        End If

        If HaveSelection <> (rect.Width <> 0) Then
            HaveSelection = (rect.Width <> 0)
            ToggleSelection()
        End If

        If HaveSelection Then
            Bool = (rect.Width = rect.Height)
        Else
            Bool = True
        End If
        Rotate90Button.Enabled = Bool
        Rotate90MenuItem.Enabled = Bool
        Rotate270Button.Enabled = Bool
        Rotate270MenuItem.Enabled = Bool
    End Sub

    ' Allow the user to open an image and then paste onto the current image
    Private Sub PasteFromMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteFromMenuItem.Click, PasteFromPopMenuItem.Click
        Dim OpenFileDialog As New OpenFileDialog
        Static FilterIndex As Integer

        With OpenFileDialog
            .InitialDirectory = OpenInitDir
            .Title = "Select Input File"
            .FilterIndex = FilterIndex
            .Filter = "Bitmaps (*.bmp, *.dib, *.gif, *.jpeg, *.jpg, *.png, *.tif, *.tiff)|*.bmp;*.dib;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|All Files (*.*)|*.*"
            .FileName = ""
            .CheckFileExists = True
            .CheckPathExists = True
            .ShowReadOnly = False
            .Multiselect = False
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Page.Paste(New Bitmap(.FileName))
            End If
            FilterIndex = .FilterIndex
            .Dispose()
        End With

    End Sub

    Private Sub PasteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteMenuItem.Click, PastePopMenuItem.Click, PasteButton.Click
        Page.Paste()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        ResultsRichTextBox.Paste()
    End Sub

    ' Tailor the popup menu (right hand clcik n the image)
    Private Sub ImagePopupMenu_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ImageContextMenu.Opening
        Dim Txt As String       ' utility

        If HaveImage Then
            If HaveSelection Then
                Txt = " selection"
            Else
                Txt = " image"
            End If
            CutPopMenuItem.Visible = HaveSelection
            PastePopMenuItem.Visible = HaveClipboardImage
            CutPopMenuItem.Text = "Cut" & Txt
            CopyPopMenuItem.Text = "Copy" & Txt
            ClearPopMenuItem.Text = "Clear" & Txt
            InvertPopMenuItem.Text = "Invert" & Txt
            RotatePopMenu.Text = "Rotate" & Txt
            FlipPopMenu.Text = "Flip" & Txt
            SaveImagePopMenuItem.Text = "Save" & Txt & " ..."

            CutPopMenuItem.Enabled = CutMenuItem.Enabled
            PastePopMenuItem.Enabled = PasteMenuItem.Enabled
            CopyPopMenuItem.Enabled = CopyMenuItem.Enabled
            ClearPopMenuItem.Enabled = ClearMenuItem.Enabled
            InvertPopMenuItem.Enabled = InvertMenuItem.Enabled
            RotatePopMenu.Enabled = RotateMenu.Enabled
            Rotate90PopMenuItem.Enabled = Rotate90MenuItem.Enabled
            Rotate270PopMenuItem.Enabled = Rotate90MenuItem.Enabled
            FlipPopMenu.Enabled = FlipMenu.Enabled
            PasteFromPopMenuItem.Enabled = PasteMenuItem.Enabled
            SelectAllPopMenuItem.Enabled = SelectAllMenuItem.Enabled
            SaveImagePopMenuItem.Enabled = OpenMenuItem.Enabled
        End If
    End Sub

    ' User has clicked on Options/Processing options
    Private Sub ProcessingOptionsMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProcessingOptionsMenuItem.Click
        ProcessOptions.ShowDialog()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoToolStripMenuItem.Click
        ResultsRichTextBox.Redo()
    End Sub

    ' User has clicked on Options/Results font
    Private Sub ResultsFontMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsFontMenuItem.Click
        Dim fdlg As New FontDialog

        If ResultsFormatInfo.Fmt = RESULTSFMT.RTF Then
            If KeepTextChanges() Then Return
        End If

        fdlg.ShowColor = True
        fdlg.ShowEffects = True
        fdlg.FontMustExist = True
        fdlg.Font = ResultsRichTextBox.Font
        fdlg.Color = ResultsRichTextBox.ForeColor
        If fdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            If fdlg.Font.Name <> ResultsRichTextBox.Font.Name Or _
                fdlg.Font.Size <> ResultsRichTextBox.Font.Size Or _
                fdlg.Font.Bold <> ResultsRichTextBox.Font.Bold Or _
                fdlg.Font.Italic <> ResultsRichTextBox.Font.Italic Then
                ResultsRichTextBox.Font = fdlg.Font
                If ResultsFormatInfo.Fmt = RESULTSFMT.RTF Then
                    FormatResults(ResultsEx_EG, ResultsRichTextBox)
                End If
                TextEdited = False
            End If
            ResultsRichTextBox.ForeColor = fdlg.Color
        End If
    End Sub

    ' User has clicked on Options/Format results
    Private Sub ResultsFormattingMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsFormattingMenuItem.Click
        If KeepTextChanges() Then Return

        ResultsFormatting.ShowDialog()
        If ResultsFormatting.DialogResult = Windows.Forms.DialogResult.OK Then
            If HaveImage Then
                ResultsEx_EG.Hdr.YPixelsPerInch = CInt(Page.Image.VerticalResolution)
            End If
            If ResultsEx_EG.Hdr.NumItems > 0 Then
                FormatResults(ResultsEx_EG, Me.ResultsRichTextBox)
            End If
            TextEdited = False
        End If
    End Sub

    ' Change the focus bar to indicate the text results has got focus
    Private Sub ResultsRichTextBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResultsRichTextBox.GotFocus
        ResultsFocusPictureBox.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
    End Sub

    ' Change the focus bar to indicate the text results has lost focus
    Private Sub ResultsRichTextBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResultsRichTextBox.LostFocus
        ResultsFocusPictureBox.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption)
    End Sub

    Private Sub ResultsRichTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResultsRichTextBox.TextChanged
        TextEdited = True
        SaveResultsMenuItem.Enabled = (ResultsRichTextBox.Text <> "") And (OCRState = OCRSTATES.Idle)
        SaveResultsButton.Enabled = SaveResultsMenuItem.Enabled
    End Sub

    ' User is typing in the text region of the results Zoom combo box
    Private Sub ResultsZoomComboBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ResultsZoomComboBox.KeyDown
        If e.KeyCode = Keys.Return Then
            If Not ResultsZoomComboBox.Text.EndsWith("%") Then ResultsZoomComboBox.Text &= "%"
            e.SuppressKeyPress = True
            ChangeResultsZoom()
        Else
            If Not (e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Or _
                e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Or _
                (e.KeyCode >= 48 And e.KeyCode <= 57)) Then
                Beep()
                e.SuppressKeyPress = True
            End If
        End If
    End Sub

    ' User has changed reults zoom
    Private Sub ResultsZoomComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResultsZoomComboBox.SelectedIndexChanged
        ChangeResultsZoom()
    End Sub

    ' Rotate Image/Selection by 90 degrees clockwise action
    Private Sub Rotate90MenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rotate90MenuItem.Click, Rotate90PopMenuItem.Click, Rotate90Button.Click
        Page.RotateFlip(RotateFlipType.Rotate90FlipNone)
    End Sub

    ' Rotate Image/Selection by 180 degrees clockwise action
    Private Sub Rotate180MenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rotate180MenuItem.Click, Rotate180PopMenuItem.Click, Rotate180Button.Click
        Page.RotateFlip(RotateFlipType.Rotate180FlipNone)
    End Sub

    ' Rotate Image/Selection by 270 degrees clockwise action
    Private Sub Rotate270MenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rotate270MenuItem.Click, Rotate270PopMenuItem.Click, Rotate270Button.Click
        Page.RotateFlip(RotateFlipType.Rotate270FlipNone)
    End Sub

    ' Save Image/Selection to a file action
    Private Sub SaveImageMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveImageMenuItem.Click, SaveImagePopMenuItem.Click
        Dim SaveFileDialog As New SaveFileDialog
        Dim Fmt As Imaging.ImageFormat = Imaging.ImageFormat.Bmp
        Static FilterIndex As Integer

        With SaveFileDialog

            .InitialDirectory = OpenInitDir
            If HaveSelection Then
                .Title = "Save selection"
            Else
                .Title = "Save image"
            End If
            .FilterIndex = FilterIndex
            .Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg;*.jpeg;*.jpe)|*.jpg;*.jpeg;*.jpe|GIF (*.gif)|*.gif|TIFF (*.tif;*.tiff)|*.tif;*.tiff|PNG (*.png)|*.png"
            .FileName = ""
            .CheckFileExists = False
            .CheckPathExists = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Select Case .FilterIndex
                    Case 1
                        Fmt = Imaging.ImageFormat.Bmp
                    Case 2
                        Fmt = Imaging.ImageFormat.Jpeg
                    Case 3
                        Fmt = Imaging.ImageFormat.Gif
                    Case 4
                        Fmt = Imaging.ImageFormat.Tiff
                    Case 5
                        Fmt = Imaging.ImageFormat.Png
                End Select
                Dim BMP As Bitmap
                If HaveSelection Then
                    BMP = Page.SelectionImage(Imaging.PixelFormat.Format1bppIndexed)
                Else
                    BMP = Page.Image(Imaging.PixelFormat.Format1bppIndexed)
                End If
                BMP.Save(.FileName, Fmt)
                FilterIndex = .FilterIndex
            End If
        End With

    End Sub

    ' Save text results to a file action
    Private Sub SaveResultsMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveResultsMenuItem.Click, SaveResultsButton.Click
        Dim SaveDialog As New SaveFileDialog
        Static Filename As String

        With SaveDialog
            .Title = "Save results"
            .InitialDirectory = SaveInitDir
            If ResultsFormatInfo.Fmt = RESULTSFMT.RTF Then
                .FilterIndex = 2
            Else
                .FilterIndex = 1
            End If
            .Filter = "Text File (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*"
            .FileName = Filename
            .DefaultExt = "txt"
            .CheckPathExists = True
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Filename = (New System.IO.DirectoryInfo(.FileName)).Name
                SaveInitDir = .FileName.Substring(0, .FileName.Length - Filename.Length - 1)

                Dim ResStrm As System.IO.StreamWriter
                ResStrm = System.IO.File.CreateText(.FileName)
                If .FilterIndex = 2 Then
                    ResStrm.Write(ResultsRichTextBox.Rtf)
                Else
                    ResStrm.Write(ResultsRichTextBox.Text.Replace(vbLf, Environment.NewLine))
                End If
                ResStrm.Close()
                TextEdited = False
            End If
        End With
    End Sub

    ' Select the whole image action
    Private Sub SelectAllMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectAllMenuItem.Click, SelectAllPopMenuItem.Click
        Page.SelectAll()
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        ResultsRichTextBox.SelectAll()
    End Sub

    ' Select the TWAIN input device
    Private Sub SelectTWAINDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectTWAINDeviceMenuItem.Click
        On Error Resume Next
        TOCRTWAINSelectDS()
        If Err.Number = ERRCANTFINDDLLENTRYPOINT Then Call NoTwain()
    End Sub

    ' Flag if to show the TWAIN User Interface dialoque before acquiring images
    Private Sub ShowDeviceUIMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowDeviceUIMenuItem.Click
        On Error Resume Next
        ShowDeviceUIMenuItem.Checked = Not ShowDeviceUIMenuItem.Checked
        TOCRTWAINShowUI(CShort(ShowDeviceUIMenuItem.Checked))
        If Err.Number = ERRCANTFINDDLLENTRYPOINT Then Call NoTwain()
    End Sub

    ' User has dragged files onto the empty image area
    Private Sub SplitContainer1_Panel1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles SplitContainer1.Panel1.DragEnter
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    ' User has dropped files onto the empty image area
    Private Sub SplitContainer1_Panel1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles SplitContainer1.Panel1.DragDrop
        Dim MyFiles() As String     ' array of dropped files (only first is used)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            MyFiles = CType(e.Data.GetData(DataFormats.FileDrop), String())
            CurrentFile = MyFiles(0)
            If KeepTextChanges() Then Return
            OpenFile()
        End If
    End Sub

    ' Stop OCRing action
    Private Sub StopOCRMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StopOCRMenuItem.Click, StopOCRButton.Click
        OCRState = OCRSTATES.Abort
    End Sub

    ' Set the options for the results textbox context menu
    Private Sub TextPopupMenu_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextContextMenu.Opening
        UndoToolStripMenuItem.Enabled = ResultsRichTextBox.CanUndo
        PasteToolStripMenuItem.Enabled = ResultsRichTextBox.CanPaste(DataFormats.GetFormat(DataFormats.Text))
        CopyToolStripMenuItem.Enabled = ResultsRichTextBox.SelectedText <> ""
        CutToolStripMenuItem.Enabled = CopyToolStripMenuItem.Enabled
        DeleteToolStripMenuItem.Enabled = CopyToolStripMenuItem.Enabled
        SelectAllToolStripMenuItem.Enabled = ResultsRichTextBox.Text <> ""
        RedoToolStripMenuItem.Enabled = ResultsRichTextBox.CanRedo
    End Sub

    Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
        ResultsRichTextBox.Undo()
    End Sub

    Private Sub Viewer_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged

    End Sub

    ' Application is closing
    Private Sub Viewer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SaveSettings()
        TOCRShutdown(JobNo)
    End Sub

    ' Check if any text result edits need to be saved prior to exiting
    Private Sub Viewer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = KeepTextChanges()
    End Sub

    ' Application is starting up
    Private Sub Viewer_FormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Status As Integer                       ' status returned bu OCR engine
        Dim NumSlots As Integer                     ' number of available job slots
        Dim Ver() As String = Application.ProductVersion.Split("."c)    ' application version number (for form title)
        Dim SlotUse() As Integer                    ' array of job slot usage
        Dim Found As Boolean = False                ' flag if found a free slot
        Dim PinGC As GCHandle                       ' handle used to pin SlotUSe array
        Dim El() As String = TOCRGL4STR.Split(","c)


        Me.Text = "TOCR Viewer " & Ver(0) & "." & Ver(1)

        ResultsEx_EG.Hdr.NumItems = 0 ' There are no results

        StatusLabel.Text = ""
        FileNameLabel.Text = ""
        MouseCoordsLabel.Text = ""
        SelectRectLabel.Text = ""

        ' Check for available job slots
        NumSlots = TOCRGetJobDBInfo(IntPtr.Zero)
        If NumSlots > 0 Then
            ReDim SlotUse(0 To NumSlots - 1)

            PinGC = GCHandle.Alloc(SlotUse, GCHandleType.Pinned)
            Status = TOCRGetJobDBInfo(PinGC.AddrOfPinnedObject)
            PinGC.Free()
            If Status = TOCR_OK Then
                For JobNo = 0 To NumSlots - 1
                    If SlotUse(JobNo) = TOCRJOBSLOT_FREE Then
                        Found = True
                        Exit For
                    End If
                Next JobNo

                If Not Found Then
                    MsgBox("No free OCR engines", MsgBoxStyle.Exclamation And MsgBoxStyle.ApplicationModal)
                    End
                End If
            Else
                MsgBox("Failed to get number of job slot usage", MsgBoxStyle.Exclamation)
            End If
        Else
            MsgBox("Failed to get number of job slots", MsgBoxStyle.Exclamation)
        End If

        SplashScreen.Show()

        Me.Show()

        JobInfo_EG.Initialize()
        ReDim TOCRGL4(El.GetUpperBound(0))
        ReDim GlyphType(TOCRGL4.GetUpperBound(0))
        ReDim LanguageDisableGlyphs(TOCRGL4.GetUpperBound(0))
        ReDim ManualDisableGlyphs(TOCRGL4.GetUpperBound(0))
        ReDim OverideGlyphs(TOCRGL4.GetUpperBound(0))

        LoadSettings()

        If SaveMemory Then
            ColourConversionMenuItem.Visible = False
            ' The EditableImageControl uses the mean Average algorithm for converting to monochrome
            JobInfo_EG.ProcessOptions.CCAlgorithm = 0
            JobInfo_EG.ProcessOptions.CCThreshold = 0
            JobInfo_EG.ProcessOptions.CGAlgorithm = 0
        End If

        If Not TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX) = TOCR_OK Then End
        Status = TOCRInitialise(JobNo)
        If Status <> 0 Then End

        ' Must be called after TOCRInitialise
        GetFontNames()

        NumPages = 0
        NUD.Minimum = 0
        NUD.Value = 1
        NUD.TextAlign = HorizontalAlignment.Right
        NUD.TabStop = False
        NUD.CausesValidation = True

        ToolBar.Items.Add(New ToolStripControlHost(NUD))

        Call ShowDeviceUIMenuItem_Click(Me, New System.EventArgs)

        UpdateControls()
        Dim Args() As String = System.Environment.GetCommandLineArgs
        If Args.GetUpperBound(0) > 0 Then
            If System.IO.File.Exists(Args(1)) Then
                CurrentFile = Args(1)
                OpenFile()
            End If
        End If

        TextEdited = False

    End Sub

    ' User has clicked on Options/View status bar
    Private Sub ViewStatusBarMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBarMenuItem.Click
        StatusBar.Visible = Not StatusBar.Visible
        StatusBarMenuItem.Checked = StatusBar.Visible
    End Sub

    ' User has clicked on Options/View toolbar
    Private Sub ViewToolbarMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolBarMenuItem.Click
        ToolBar.Visible = Not ToolBar.Visible
        ToolBarMenuItem.Checked = ToolBar.Visible
    End Sub
#End Region

#Region " Private routines "
    ' Change the page in a multi page file
    Private Sub ChangePage()
        Dim I As Integer = SplitContainer1.Panel1.Controls.IndexOfKey(CStr(CurrentPage))
        Dim CurrentCursor As Cursor = Cursor

        Cursor = Cursors.WaitCursor

        Page = CType(SplitContainer1.Panel1.Controls(I), EditableImage)
        Page.BringToFront()
        If LockZoomMenuItem.Checked Then
            Dim Txt As String = ImageZoomComboBox.Text
            If Txt.EndsWith("%") Then Txt = Txt.Substring(0, Txt.Length - 1)
            Page.Zoom = CSng(Txt)
            Txt = ResultsZoomComboBox.Text
            If Txt.EndsWith("%") Then Txt = Txt.Substring(0, Txt.Length - 1)
            ResultsRichTextBox.ZoomFactor = CSng(Txt) / 100
        Else
            Page.Zoom = ImageZooms(CurrentPage)
            ImageZoomComboBox.Text = CStr(Page.Zoom) & "%"
            ResultsRichTextBox.ZoomFactor = ResultsZooms(CurrentPage) / 100
            ResultsZoomComboBox.Text = CStr(ResultsZooms(CurrentPage)) & "%"
        End If

        Page_SelectionChanged(Page.SelectionRectangle)
        MouseCoordsLabel.Text = ""
        FileNameLabel.Text = CType(Page.Tag, String)

        Cursor = CurrentCursor
    End Sub

    ' Image Zoom has changed
    Private Sub ChangeImageZoom()
        Dim Txt As String       ' utility
        Dim Value As Integer    ' utility

        Txt = ImageZoomComboBox.Text
        If Txt.EndsWith("%") Then Txt = Txt.Substring(0, Txt.Length - 1)
        Value = CInt(Txt)
        If Value < Page.MinZoomPercent Or Value > Page.MaxZoomPercent Then
            MsgBox("Please enter a zoom between " & CStr(Page.MinZoomPercent) & "% and " _
                & CStr(Page.MaxZoomPercent) & "%", MsgBoxStyle.Exclamation)
            ImageZoomComboBox.Text = CStr(Page.Zoom) & "%"
            ImageZoomComboBox.SelectAll()
        Else
            Page.Zoom = CType(Txt, Single)
            ImageZooms(CurrentPage) = Page.Zoom
        End If
    End Sub

    ' Results Zoom has changed
    Private Sub ChangeResultsZoom()
        Dim Txt As String       ' utility
        Dim Value As Integer    ' utility

        Txt = ResultsZoomComboBox.Text
        If Txt.EndsWith("%") Then Txt = Txt.Substring(0, Txt.Length - 1)
        Value = CInt(Txt)
        If Value < 2 Or Value > 3200 Then
            MsgBox("Please enter a zoom between " & CStr(2) & "% and " _
                & CStr(3200) & "%", MsgBoxStyle.Exclamation)
            ResultsZoomComboBox.Text = CStr(ResultsRichTextBox.ZoomFactor * 100) & "%"
            ResultsZoomComboBox.SelectAll()
        Else
            ResultsRichTextBox.ZoomFactor = CSng(Txt) / 100
            ResultsZooms(CurrentPage) = CSng(Txt)
        End If
    End Sub

    ' Get a list of fontnames that will be matched to FontIDs returned by TOCR
    Private Sub GetFontNames()
        Dim FID As Integer
        Dim FontName As New System.Text.StringBuilder(TOCRFONTNAMELENGTH)
        Dim FontList As String
        Dim IFC As New System.Drawing.Text.InstalledFontCollection
        Dim NF As Integer

        ReDim FontNames(0)

        On Error Resume Next

        ' Get all fontnames used by TOCR
        NF = TOCRGetFontName(JobNo, -1, FontName)

        If NF <= 0 Then
            Exit Sub
        End If

        ReDim FontNames(NF - 1)

        ' 0 is no font
        For FID = 1 To NF - 1
            TOCRGetFontName(JobNo, FID, FontName)
            FontNames(FID) = FontName.ToString
            If FontNames(FID).EndsWith("*") Then
                FontNames(FID) = FontNames(FID).Substring(0, FontNames(FID).Length - 1)
            End If
        Next

        ' Get a list of all installed fonts
        FontList = Chr(0)
        For Each ff As FontFamily In IFC.Families
            FontList &= ff.Name & Chr(0)
        Next

        ' Set TOCR names to blank if the font is not installed
        For FID = 0 To NF
            ' Choose one of the installed OCRB fonts
            If FontNames(FID) = "OCRB" Then
                For Each ff As FontFamily In IFC.Families
                    If ff.Name.StartsWith("OCRB") Then
                        FontNames(FID) = ff.Name
                        Exit For
                    End If
                Next
            End If

            If FontList.IndexOf(Chr(0) & FontNames(FID) & Chr(0), StringComparison.OrdinalIgnoreCase) < 0 Then
                FontNames(FID) = "" ' null out the name so that a default can be used
            End If
        Next
    End Sub

    ' User is about to change the image/results and might have made text edits
    ' so check whether to proceed
    Private Function KeepTextChanges() As Boolean
        If TextEdited And ResultsRichTextBox.Text <> "" Then
            If MsgBox("You will lose your text edits!" & Environment.NewLine & Environment.NewLine & "Do you wish to continue?", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation) = vbNo Then
                Return True
            End If
        End If
        Return False
    End Function

    ' Load previous settings from the registry (Zoom, window size and position etc).
    ' Old registry values are in TWIPS
    Private Sub LoadSettings()
        Dim WinLeft As Integer          ' window left
        Dim WinTop As Integer           ' window top
        Dim WinWidth As Integer         ' window width
        Dim WinHeight As Integer        ' window height
        Dim Value As Integer            ' utility value
        Dim FontName As String
        Dim FontBold As Boolean
        Dim FontItalic As Boolean
        Dim FontSize As Single
        Dim FS As FontStyle
        Dim RegNo As Integer           ' value retreived from a registry string where the string is a list of values
        Dim GlyphNo As Integer           ' utility
        Dim LangNo As Integer           ' utility
        Dim TxtNo As Integer           ' utility
        Dim Txt As String               ' utility

        On Error Resume Next

        If CType(GetSetting(REGAPPNAME, "Window", "State", CStr(FormWindowState.Normal)), FormWindowState) = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Maximized
        Else
            ' Old registry values are in TWIPS
            WinLeft = CInt(CInt(GetSetting(REGAPPNAME, "Window", "Left", CStr(PIXELSTOTWIPS * Me.Location.X))) / PIXELSTOTWIPS)
            WinTop = CInt(CInt(GetSetting(REGAPPNAME, "Window", "Top", CStr(PIXELSTOTWIPS * Me.Location.Y))) / PIXELSTOTWIPS)
            WinWidth = CInt(CInt(GetSetting(REGAPPNAME, "Window", "Width", CStr(PIXELSTOTWIPS * Me.Size.Width))) / PIXELSTOTWIPS)
            WinHeight = CInt(CInt(GetSetting(REGAPPNAME, "Window", "Height", CStr(PIXELSTOTWIPS * Me.Size.Height))) / PIXELSTOTWIPS)

            ' Small check to ensure can always resize
            If WinLeft < 0 And WinWidth > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width Then WinLeft = 0
            If WinTop < 0 And WinHeight > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height Then WinHeight = 0
            Me.SetBounds(WinLeft, WinTop, WinWidth, WinHeight)
        End If

        ' Visibility of Tool, Status bars

        ToolBarMenuItem.Checked = CBool(GetSetting(REGAPPNAME, "Window", "Show Toolbar", CStr(True)))
        StatusBarMenuItem.Checked = CBool(GetSetting(REGAPPNAME, "Window", "Show Status bar", CStr(True)))
        FocusbarsMenuItem.Checked = CBool(GetSetting(REGAPPNAME, "Window", "Show Focus Bars", CStr(True)))

        StatusBar.Visible = StatusBarMenuItem.Checked
        ToolBar.Visible = ToolBarMenuItem.Checked
        ImageFocusPictureBox.Visible = FocusbarsMenuItem.Checked
        ResultsFocusPictureBox.Visible = FocusbarsMenuItem.Checked

        ' Ensure the split bar is visible

        Value = CInt(GetSetting(REGAPPNAME, "Window", "SplitPosition", CStr(SplitContainer1.SplitterDistance)))
        If Value < 0 Then Value = 0
        If Value > Me.ClientRectangle.Width - SplitContainer1.SplitterWidth Then Value = Me.ClientRectangle.Width - SplitContainer1.SplitterWidth
        SplitContainer1.SplitterDistance = Value

        ' Zoom

        Value = CInt(GetSetting(REGAPPNAME, "Source", "Zoom", "100"))
        Value = Math.Min(Math.Max(Value, Page.MinZoomPercent), Page.MaxZoomPercent)
        ImageZoomComboBox.Text = CStr(Value) & "%"
        Page.Zoom = Value
        LockZoomMenuItem.Checked = CBool(GetSetting(REGAPPNAME, "Source", "Lock Zoom", CStr(True)))

        ' Get the font for the results text box

        With ResultsRichTextBox
            FontName = GetSetting(REGAPPNAME, "Results", "FontName", .Font.Name)
            FontSize = CSng(GetSetting(REGAPPNAME, "Results", "FontSize", CStr(.Font.SizeInPoints)))
            FontItalic = CBool(GetSetting(REGAPPNAME, "Results", "FontItalic", CStr(.Font.Italic)))
            FontBold = CBool(GetSetting(REGAPPNAME, "Results", "FontBold", CStr(.Font.Bold)))

            FS = FontStyle.Regular
            If FontBold Then FS = FS Or FontStyle.Bold
            If FontItalic Then FS = FS Or FontStyle.Italic
            .Font = New System.Drawing.Font(FontName, FontSize, FS)
            .ForeColor = System.Drawing.ColorTranslator.FromOle(CInt(GetSetting(REGAPPNAME, "Results", "Colour", CStr(System.Drawing.ColorTranslator.ToOle(.ForeColor)))))
            ResultsRichTextBox.ZoomFactor = 1
        End With

        ' Get formatting information for the text box

        With ResultsFormatInfo
            Txt = GetSetting(REGAPPNAME, "Results", "FormatType", "txt")
            If CBool(GetSetting(REGAPPNAME, "Results", "Format", "True")) Then
                If Txt = "rtf" Then
                    .Fmt = RESULTSFMT.RTF
                Else
                    .Fmt = RESULTSFMT.Text
                End If
            Else
                .Fmt = RESULTSFMT.None
            End If
            .CollapseY = CBool(GetSetting(REGAPPNAME, "Results", "CollapseY", CStr(False)))
            .LeftMargin = CBool(GetSetting(REGAPPNAME, "Results", "LeftMargin", CStr(True)))
            .MinXSpaceFac = CSng(GetSetting(REGAPPNAME, "Results", "MinXSpaceFac", CStr(1.6)))
            .MinYSpaceFac = CSng(GetSetting(REGAPPNAME, "Results", "MinYSpaceFac", CStr(1.5)))
        End With

        ' Open/Save paths, save type

        OpenInitDir = GetSetting(REGAPPNAME, "Source", "Path", New System.IO.FileInfo(Application.StartupPath & "\..\..").DirectoryName)
        SaveInitDir = GetSetting(REGAPPNAME, "Results", "Path", System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))

        With JobInfo_EG.ProcessOptions
            .StructId = 0
            .InvertWholePage = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "InvertWholePage", "False")))
            .DeskewOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "DeskewOff", "False")))
            .Orientation = CByte(GetSetting(REGAPPNAME, "OCR", "Orientation", "0"))
            .NoiseRemoveOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "NoiseRemoveOff", "False")))
            .ReturnNoiseOn = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "NoiseReturn", "False")))
            .LineRemoveOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "LineRemoveOff", "False")))
            .DeshadeOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "DeshadeOff", "False")))
            .InvertOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "InvertOff", "False")))
            .SectioningOn = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "SectioningOn", "False")))
            .MergeBreakOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "MergeBreakOff", "False")))
            .LineRejectOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "LineRejectOff", "False")))
            .CharacterRejectOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "CharacterRejectOff", "False")))
            .LexMode = CByte(GetSetting(REGAPPNAME, "OCR", "LexMode", "0"))
            .OCRBOnly = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "OCRBOnly", "False")))
            .Speed = CByte(GetSetting(REGAPPNAME, "OCR", "Speed", "0"))
            .FontStyleInfoOff = CByte(CBool(GetSetting(REGAPPNAME, "OCR", "FontStyleInfoOff", "False")))
            .CCAlgorithm = CInt(GetSetting(REGAPPNAME, "OCR", "CCAlgorithm", "0"))
            .CCThreshold = CSng(GetSetting(REGAPPNAME, "OCR", "CCThreshold", CStr(0.5)))
            .CGAlgorithm = CInt(GetSetting(REGAPPNAME, "OCR", "CGAlgorithm", "0"))

            Txt = GetSetting(REGAPPNAME, "OCR", "DisableUnicodeCharacters")
            Array.Clear(ManualDisableGlyphs, 0, ManualDisableGlyphs.GetUpperBound(0) + 1)
            For TxtNo = 0 To Txt.Length - 1
                RegNo = CUShort(AscW(Txt.Substring(TxtNo, 1)))
                GlyphNo = RegNo - CUShort(AscW("0"))
                ' DisableCharW is now a raster (previously it was a list of GlyphNos)
                ManualDisableGlyphs(GlyphNo) = True
            Next

            Txt = GetSetting(REGAPPNAME, "OCR", "ForceUnicodeCharactersOff")
            Array.Clear(OverideGlyphs, 0, OverideGlyphs.GetUpperBound(0) + 1)
            For TxtNo = 0 To Txt.Length - 1
                RegNo = CUShort(AscW(Txt.Substring(TxtNo, 1)))
                GlyphNo = RegNo - CUShort(AscW("0"))
                OverideGlyphs(GlyphNo) = OVERIDESTATE.osDISABLE
            Next

            Txt = GetSetting(REGAPPNAME, "OCR", "ForceUnicodeCharactersOn")
            Array.Clear(OverideGlyphs, 0, OverideGlyphs.GetUpperBound(0) + 1)
            For TxtNo = 0 To Txt.Length - 1
                RegNo = CUShort(AscW(Txt.Substring(TxtNo, 1)))
                GlyphNo = RegNo - CUShort(AscW("0"))
                OverideGlyphs(GlyphNo) = OVERIDESTATE.osENABLE
            Next

            Txt = GetSetting(REGAPPNAME, "OCR", "DisableLanguages")
            Array.Clear(.DisableLangs, 0, .DisableLangs.GetUpperBound(0) + 1)
            For TxtNo = 0 To Txt.Length - 1
                RegNo = CUShort(AscW(Txt.Substring(TxtNo, 1)))
                LangNo = RegNo - CUShort(AscW("0"))
                .DisableLangs(LangNo) = 1
            Next
        End With
    End Sub

    ' Called when can't find TWAIN I/F
    Private Sub NoTwain(Optional ByVal Quiet As Boolean = False)
        Separator1FileMenuItem.Visible = False
        SelectTWAINDeviceMenuItem.Visible = False
        AcquireImagesMenuItem.Visible = False
        ShowDeviceUIMenuItem.Visible = False

        If Not Quiet Then
            MsgBox("This version of TOCR DLL does not support TWAIN", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub OpenFilePDF()
        Dim PageNo As Integer
        Dim BlankPages As Integer
        Dim I As Integer

        Dim Extractor As New PDFExtractor
        Dim Doc As New PDFExtractorMemDoc(Extractor)

        ' get page count
        'TocrPdfResult = PDFExtractorHandle_New(pExtractor)

        'TocrPdfResult = PDFExtractorMemDocHandle_New(pDoc)

        'TocrPdfResult = PDFExtractorHandle_Load(pExtractor, pDoc, CurrentFile)
        Doc.Load(CurrentFile)

        Dim Count As New UInteger
        'TocrPdfResult = PDFExtractorHandle_GetPageCount(pExtractor, pDoc, pCount)
        Count = Doc.GetPageCount()
        I = CInt(Count)

        NumPages = 0
        BlankPages = 0
        NUD.Value = 1
        If I > 1 Then
            NUD.Maximum = I + 1
            If CurrentPage > I - 1 Then
                CurrentPage = 0
            End If
        Else
            CurrentPage = 0
        End If
        NUD.Value = CurrentPage + 1
        NumPages = I
        ReDim ImageZooms(0 To NumPages - 1)
        ReDim ResultsZooms(0 To NumPages - 1)

        For PageNo = 0 To NumPages - 1
            If NumPages > 1 Then
                FileNameLabel.Text = "Loading page " & (PageNo + 1).ToString
                StatusBar.Refresh()
                Application.DoEvents()
            End If

            Dim P As New EditableImage
            P.Dock = DockStyle.Fill
            P.NormaliseAspectRatio = True

            If SaveMemory Then
                ' The EditableImageControl uses the mean Vverage algorithm for converting to monochrome
                P.UseFastRenderFormat = (NumPages = 1)
                P.ConvertToMonochrome = True
            Else
                P.UseFastRenderFormat = True
                P.ConvertToMonochrome = False
            End If
            P.DoubleBuffered = True

            Dim bmp As Bitmap
            bmp = Nothing

            'TocrPdfResult = PDFExtractorPageHandle_New(pPage)

            Dim pPageIsNotBlank As New Boolean
            Dim UPageNo As UInteger
            'Dim DpiX As New Double
            'Dim DpiY As New Double
            'Dim ColourMode As New Short

            ' setup some defaults
            'DpiX = 400
            'DpiY = 400
            'ColourMode = 1 ' grey 

            ' N.B. this removes all pages except the current one (PageNo)
            'TocrPdfResult = PDFExtractorHandle_Init(pExtractor, pDoc, pPage, PageNo, pPageIsNotBlank)
            UPageNo = CUInt(PageNo + 1) ' PDF pages start at 1 (not 0)
            'TocrPdfResult = PDFExtractorHandle_GetPage(pExtractor, pDoc, pPage, UPageNo)
            Dim PdfPage As PDFExtractorPage

            PdfPage = Doc.GetPage(UPageNo)

            'If (TOCRPDF_ErrorOK = TocrPdfResult) Then
            Dim p_cpwl As New CHARPTR_WITH_LEN
            Dim MMFsize As Integer = 50000000       ' size of memory memory mapped file (and array MMFBytes) a4 RGB page at 400 dpi, no compression
            Dim MMFhandle As IntPtr = IntPtr.Zero   ' handle to memory mapped file
            Dim MMFview As IntPtr = IntPtr.Zero     ' pointer to view of memory mapped file

            MMFhandle = KRN.CreateFileMappingMy(&HFFFFFFFF, 0&, KRN.PAGE_READWRITE, 0, MMFsize, 0&)

            If Not MMFhandle.Equals(IntPtr.Zero) Then
                MMFview = KRN.MapViewOfFileMy(MMFhandle, KRN.FILE_MAP_WRITE, 0, 0, 0)

                If Not MMFview.Equals(IntPtr.Zero) Then
                    p_cpwl.charPtr = MMFview
                    p_cpwl.len = CUInt(MMFsize)
                    'Dim sPageIsNotBlank As New Short

                    'TocrPdfResult = PDFExtractorHandle_PageToDibMem(pExtractor, pPage, p_cpwl, ColourMode, DpiX, DpiY, sPageIsNotBlank)
                    PdfPage.PageToDib(p_cpwl)
                    'pPageIsNotBlank = (sPageIsNotBlank <> 0)
                    If (True = PdfPage.m_PageIsNotBlank) Then
                        bmp = ConvertBlockToBitmap(p_cpwl.charPtr)
                    End If

                    KRN.UnmapViewOfFileMy(MMFview)
                End If

                KRN.CloseHandle(MMFhandle)
            End If
            'End If

            'TocrPdfResult = PDFExtractorPageHandle_Delete(pPage)
            'pPage.DataHandle = Nothing

            If Not IsNothing(bmp) Then
                ' Remove alpha channel
                If (bmp.PixelFormat And Imaging.PixelFormat.Alpha) <> 0 Or (bmp.PixelFormat And Imaging.PixelFormat.PAlpha) <> 0 Then
                    ConvertBitmap(bmp, Imaging.PixelFormat.Format32bppRgb) ' To get rid of any transparency
                    ConvertBitmap(bmp, P.FastRenderFormat)
                End If

                P.Image = bmp
                'P.Image.RotateFlip(RotateFlipType.Rotate180FlipX) - temp flip to show different

                P.Name = CStr(PageNo) ' so can use IndexOfKey method to refer to page

                ImageZooms(PageNo) = Page.Zoom
                ResultsZooms(PageNo) = ResultsRichTextBox.ZoomFactor * 100
                P.Zoom = Page.Zoom
                If NumPages > 1 Then
                    P.Tag = CurrentFile & " page " & CStr(PageNo + 1) & " of " & CStr(NumPages)
                Else
                    P.Tag = CurrentFile
                End If
                P.ContextMenuStrip = ImageContextMenu
                P.Refresh()
                SplitContainer1.Panel1.Controls.Add(P)
            Else
                BlankPages = BlankPages + 1
                FileNameLabel.Text = "Page " & (PageNo + 1).ToString & " contains no images"

                StatusBar.Refresh()
                Application.DoEvents()
            End If
        Next
        ImageFocusPictureBox.SendToBack()

        'TocrPdfResult = PDFExtractorMemDocHandle_Delete(pDoc)
        'pDoc.DataHandle = Nothing
        'TocrPdfResult = PDFExtractorHandle_Delete(pExtractor)
        'pExtractor.DataHandle = Nothing

        NumPages = NumPages - BlankPages

        If NumPages > 0 Then
            HaveImage = True
            ChangePage()
        Else
            NumPages = 0
            FileNameLabel.Text = "File contains no images"
        End If
        StatusBar.Refresh()
        Application.DoEvents()
    End Sub


    ' Open a named file, show the image and update controls accordingly.
    Private Sub OpenFile()
        Dim ImageTmp As Image
        Dim PageNo As Integer
        Dim BlankPages As Integer
        Dim I As Integer
        Dim CurrentCursor As Cursor = Cursor
        Dim FrameDimension As Imaging.FrameDimension
        Dim CurrentFileType As String
        'Dim TocrPdfResult As Long

        Cursor = Cursors.WaitCursor

        FileNameLabel.Text = ""
        ImageTmp = Nothing

        Try
            ' Remove all controls except the Focus picture box
            Dim HaveEI As Boolean
            Do
                HaveEI = False
                Dim Cntrl As Control
                For Each Cntrl In SplitContainer1.Panel1.Controls
                    If TypeOf Cntrl Is EditableImage Then
                        Cntrl.Dispose()
                        HaveEI = True
                    End If
                Next Cntrl
            Loop While HaveEI

            CurrentFileType = DetectFileType(CurrentFile)
            If CurrentFileType = "GIF" Then
                FrameDimension = Imaging.FrameDimension.Time
            Else
                FrameDimension = Imaging.FrameDimension.Page
            End If

            If CurrentFileType = "PDF" Then
                OpenFilePDF()
            Else
                ImageTmp = Image.FromFile(CurrentFile)
                I = ImageTmp.GetFrameCount(FrameDimension)
                NumPages = 0
                BlankPages = 0
                NUD.Value = 1
                If I > 1 Then
                    NUD.Maximum = I + 1
                    If CurrentPage > I - 1 Then
                        CurrentPage = 0
                    End If
                Else
                    CurrentPage = 0
                End If
                NUD.Value = CurrentPage + 1
                NumPages = I
                ReDim ImageZooms(0 To NumPages - 1)
                ReDim ResultsZooms(0 To NumPages - 1)

                For PageNo = 0 To NumPages - 1
                    If NumPages > 1 Then
                        FileNameLabel.Text = "Loading page " & (PageNo + 1).ToString
                        StatusBar.Refresh()
                        Application.DoEvents()
                    End If

                    Dim P As New EditableImage
                    P.Dock = DockStyle.Fill
                    P.NormaliseAspectRatio = True

                    If SaveMemory Then
                        ' The EditableImageControl uses the mean Vverage algorithm for converting to monochrome
                        P.UseFastRenderFormat = (NumPages = 1)
                        P.ConvertToMonochrome = True
                    Else
                        P.UseFastRenderFormat = True
                        P.ConvertToMonochrome = False
                    End If
                    P.DoubleBuffered = True

                    Dim bmp As Bitmap
                    bmp = Nothing
                    
                    If Not IsNothing(ImageTmp) Then
                        ImageTmp.SelectActiveFrame(FrameDimension, PageNo Mod NumPages)
                        bmp = New Bitmap(ImageTmp)
                        bmp.SetResolution(ImageTmp.HorizontalResolution, ImageTmp.VerticalResolution)
                    End If
                    
                    If Not IsNothing(bmp) Then
                        ' Remove alpha channel
                        If (bmp.PixelFormat And Imaging.PixelFormat.Alpha) <> 0 Or (bmp.PixelFormat And Imaging.PixelFormat.PAlpha) <> 0 Then
                            ConvertBitmap(bmp, Imaging.PixelFormat.Format32bppRgb) ' To get rid of any transparency
                            ConvertBitmap(bmp, P.FastRenderFormat)
                        End If

                        P.Image = bmp
                        'P.Image.RotateFlip(RotateFlipType.Rotate180FlipX) - temp flip to show different

                        P.Name = CStr(PageNo) ' so can use IndexOfKey method to refer to page

                        ImageZooms(PageNo) = Page.Zoom
                        ResultsZooms(PageNo) = ResultsRichTextBox.ZoomFactor * 100
                        P.Zoom = Page.Zoom
                        If NumPages > 1 Then
                            P.Tag = CurrentFile & " page " & CStr(PageNo + 1) & " of " & CStr(NumPages)
                        Else
                            P.Tag = CurrentFile
                        End If
                        P.ContextMenuStrip = ImageContextMenu
                        P.Refresh()
                        SplitContainer1.Panel1.Controls.Add(P)
                    Else
                        BlankPages = BlankPages + 1
                        FileNameLabel.Text = "Page " & (PageNo + 1).ToString & " is blank"
                        StatusBar.Refresh()
                        Application.DoEvents()
                    End If
                Next
                ImageFocusPictureBox.SendToBack()

                ImageTmp.Dispose()

                NumPages = NumPages - BlankPages

                If NumPages > 0 Then
                    HaveImage = True
                    ChangePage()
                Else
                    NumPages = 0
                    FileNameLabel.Text = "File is blank"
                    StatusBar.Refresh()
                    Application.DoEvents()
                End If
            End If

        Catch ex As Exception
            HaveImage = False
            NumPages = 0
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            If Not (ex.InnerException Is Nothing) Then
                MessageBox.Show(ex.InnerException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

        ResultsRichTextBox.Text = ""
        TextEdited = False

        UpdateControls()
        GC.Collect()

        Cursor = CurrentCursor
    End Sub

    ' Save current settings to the registry (Zoom, window size and position etc).
    Private Sub SaveSettings()
        Dim Txt As String
        Dim GlyphNo As Integer
        Dim LangNo As Integer

        If Me.WindowState = FormWindowState.Maximized Or Me.WindowState = FormWindowState.Normal Then _
            SaveSetting(REGAPPNAME, "Window", "State", CStr(Me.WindowState))

        If Me.WindowState = FormWindowState.Normal Then
            SaveSetting(REGAPPNAME, "Window", "Left", CStr(Me.Location.X * PIXELSTOTWIPS))
            SaveSetting(REGAPPNAME, "Window", "Top", CStr(Me.Location.Y * PIXELSTOTWIPS))
            SaveSetting(REGAPPNAME, "Window", "Width", CStr(Me.Size.Width * PIXELSTOTWIPS))
            SaveSetting(REGAPPNAME, "Window", "Height", CStr(Me.Size.Height * PIXELSTOTWIPS))
        End If

        SaveSetting(REGAPPNAME, "Window", "Show Toolbar", CStr(ToolBarMenuItem.Checked))
        SaveSetting(REGAPPNAME, "Window", "Show Status bar", CStr(StatusBarMenuItem.Checked))
        SaveSetting(REGAPPNAME, "Window", "Show Focus Bars", CStr(FocusbarsMenuItem.Checked))

        SaveSetting(REGAPPNAME, "Window", "SplitPosition", CStr(SplitContainer1.SplitterDistance))

        Txt = ImageZoomComboBox.Text
        If Txt.EndsWith("%") Then Txt = Txt.Substring(0, Txt.Length - 1)
        SaveSetting(REGAPPNAME, "Source", "Zoom", Txt)
        SaveSetting(REGAPPNAME, "Source", "Lock Zoom", CStr(LockZoomMenuItem.Checked))

        With ResultsRichTextBox
            SaveSetting(REGAPPNAME, "Results", "FontName", .Font.Name)
            SaveSetting(REGAPPNAME, "Results", "FontSize", CStr(.Font.Size))
            SaveSetting(REGAPPNAME, "Results", "FontBold", CStr(.Font.Bold))
            SaveSetting(REGAPPNAME, "Results", "FontItalic", CStr(.Font.Italic))
            SaveSetting(REGAPPNAME, "Results", "Colour", CStr(System.Drawing.ColorTranslator.ToOle(.ForeColor)))
        End With

        With ResultsFormatInfo
            SaveSetting(REGAPPNAME, "Results", "Format", CStr(.Fmt <> RESULTSFMT.None))
            If .Fmt = RESULTSFMT.RTF Then
                Txt = "rtf"
            Else
                Txt = "txt"
            End If
            SaveSetting(REGAPPNAME, "Results", "FormatType", Txt)
            SaveSetting(REGAPPNAME, "Results", "CollapseY", CStr(.CollapseY))
            SaveSetting(REGAPPNAME, "Results", "LeftMargin", CStr(.LeftMargin))
            SaveSetting(REGAPPNAME, "Results", "MinXSpaceFac", CStr(.MinXSpaceFac))
            SaveSetting(REGAPPNAME, "Results", "MinYSpaceFac", CStr(.MinYSpaceFac))
        End With

        SaveSetting(REGAPPNAME, "Source", "Path", OpenInitDir)

        With JobInfo_EG.ProcessOptions
            SaveSetting(REGAPPNAME, "OCR", "InvertWholePage", CStr(CBool(.InvertWholePage)))
            SaveSetting(REGAPPNAME, "OCR", "DeskewOff", CStr(CBool(.DeskewOff)))
            SaveSetting(REGAPPNAME, "OCR", "Orientation", CStr(.Orientation))
            SaveSetting(REGAPPNAME, "OCR", "NoiseRemoveOff", CStr(CBool(.NoiseRemoveOff)))
            SaveSetting(REGAPPNAME, "OCR", "NoiseReturn", CStr(CBool(.ReturnNoiseOn)))
            SaveSetting(REGAPPNAME, "OCR", "LineRemoveOff", CStr(CBool(.LineRemoveOff)))
            SaveSetting(REGAPPNAME, "OCR", "DeshadeOff", CStr(CBool(.DeshadeOff)))
            SaveSetting(REGAPPNAME, "OCR", "InvertOff", CStr(CBool(.InvertOff)))
            SaveSetting(REGAPPNAME, "OCR", "SectioningOn", CStr(CBool(.SectioningOn)))
            SaveSetting(REGAPPNAME, "OCR", "MergeBreakOff", CStr(CBool(.MergeBreakOff)))
            SaveSetting(REGAPPNAME, "OCR", "LineRejectOff", CStr(CBool(.LineRejectOff)))
            SaveSetting(REGAPPNAME, "OCR", "CharacterRejectOff", CStr(CBool(.CharacterRejectOff)))
            SaveSetting(REGAPPNAME, "OCR", "LexMode", CStr(.LexMode))
            SaveSetting(REGAPPNAME, "OCR", "OCRBOnly", CStr(CBool(.OCRBOnly)))
            SaveSetting(REGAPPNAME, "OCR", "Speed", CStr(.Speed))
            SaveSetting(REGAPPNAME, "OCR", "FontStyleInfoOff", CStr(CBool(.FontStyleInfoOff)))
            SaveSetting(REGAPPNAME, "OCR", "CCAlgorithm", CStr(.CCAlgorithm))
            SaveSetting(REGAPPNAME, "OCR", "CCThreshold", CStr(.CCThreshold))
            SaveSetting(REGAPPNAME, "OCR", "CGAlgorithm", CStr(.CGAlgorithm))

            Txt = ""
            For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
                If ManualDisableGlyphs(GlyphNo) Then
                    Txt = Txt & ChrW(AscW("0") + GlyphNo)
                End If
            Next
            SaveSetting(REGAPPNAME, "OCR", "DisableUnicodeCharacters", Txt)

            Txt = ""
            For GlyphNo = 0 To OverideGlyphs.GetUpperBound(0)
                If OverideGlyphs(GlyphNo) = OVERIDESTATE.osDISABLE Then
                    Txt = Txt & ChrW(AscW("0") + GlyphNo)
                End If
            Next
            SaveSetting(REGAPPNAME, "OCR", "ForceUnicodeCharactersOff", Txt)

            Txt = ""
            For GlyphNo = 0 To OverideGlyphs.GetUpperBound(0)
                If OverideGlyphs(GlyphNo) = OVERIDESTATE.osENABLE Then
                    Txt = Txt & ChrW(AscW("0") + GlyphNo)
                End If
            Next
            SaveSetting(REGAPPNAME, "OCR", "ForceUnicodeCharactersOn", Txt)

            Txt = ""
            For LangNo = 0 To .DisableLangs.GetUpperBound(0)
                If .DisableLangs(LangNo) <> 0 Then
                    Txt = Txt & ChrW(AscW("0") + LangNo)
                End If
            Next
            SaveSetting(REGAPPNAME, "OCR", "DisableLanguages", Txt)

        End With ' mProcessOptions

    End Sub


    ' Toggle all controls depending on wether there is a selection
    Private Sub ToggleSelection()
        If HaveSelection Then
            ImageMenu.Text = "&Selection"
            OCRButton.Image = My.Resources.TocrSel
            OCRButton.ToolTipText = "OCR selection"
            CopyButton.Image = My.Resources.CopySel
            CopyButton.ToolTipText = "Copy selection"
            InvertButton.Image = My.Resources.InvertSel
            InvertButton.ToolTipText = "Invert selection"
            Rotate90Button.Image = My.Resources.Rotate90sel
            Rotate90Button.ToolTipText = "Rotate selection 90°"
            Rotate180Button.Image = My.Resources.Rotate180Sel
            Rotate180Button.ToolTipText = "Rotate selection 180°"
            Rotate270Button.Image = My.Resources.Rotate270sel
            Rotate270Button.ToolTipText = "Rotate selection 270°"
            FlipHorizontallyButton.Image = My.Resources.FlipHSel
            FlipHorizontallyButton.ToolTipText = "Flip selection Horizontally"
            FlipVerticallyButton.Image = My.Resources.FlipVSel
            FlipVerticallyButton.ToolTipText = "Flip selection Vertically"
            CutButton.Enabled = True
            CutMenuItem.Enabled = True
            ClearButton.Enabled = True
            ClearMenuItem.Enabled = True
        Else
            ImageMenu.Text = "&Image"
            OCRButton.Image = My.Resources.Tocr
            OCRButton.ToolTipText = "OCR image"
            CopyButton.Image = My.Resources.Copy
            CopyButton.ToolTipText = "Copy image"
            InvertButton.Image = My.Resources.Invert
            InvertButton.ToolTipText = "Invert image"
            Rotate90Button.Image = My.Resources.Rotate90
            Rotate90Button.ToolTipText = "Rotate image 90°"
            Rotate180Button.Image = My.Resources.Rotate180
            Rotate180Button.ToolTipText = "Rotate image 180°"
            Rotate270Button.Image = My.Resources.Rotate270
            Rotate270Button.ToolTipText = "Rotate image 270°"
            FlipHorizontallyButton.Image = My.Resources.FlipH
            FlipHorizontallyButton.ToolTipText = "Flip image Horizontally"
            FlipVerticallyButton.Image = My.Resources.FlipV
            FlipVerticallyButton.ToolTipText = "Flip image Vertically"
            CutButton.Enabled = False
            CutMenuItem.Enabled = False
            ClearButton.Enabled = False
            ClearMenuItem.Enabled = False
            SelectRectLabel.Text = ""
        End If
    End Sub

    ' Update the enabled state of controls dependent on whether have an image or results.
    Private Sub UpdateControls()
        Dim Bool As Boolean
        Dim HaveResults As Boolean

        HaveResults = (ResultsRichTextBox.Text <> "")

        If Not HaveImage Then
            HaveSelection = False
            NumPages = 0
            MouseCoordsLabel.Text = ""
        End If

        Bool = (OCRState = OCRSTATES.Idle)
        OpenMenuItem.Enabled = Bool
        OpenButton.Enabled = Bool
        OCRImageMenuItem.Enabled = Bool And HaveImage
        OCRButton.Enabled = Bool And HaveImage
        OCRSelectionMenuItem.Enabled = Bool And HaveSelection
        StopOCRMenuItem.Enabled = Not Bool
        StopOCRButton.Enabled = Not Bool

        SelectTWAINDeviceMenuItem.Enabled = Bool
        AcquireImagesMenuItem.Enabled = Bool

        SaveResultsMenuItem.Enabled = Bool And HaveResults
        SaveResultsButton.Enabled = Bool And HaveResults
        CutMenuItem.Enabled = Bool And HaveSelection
        CutButton.Enabled = Bool And HaveSelection
        CopyMenuItem.Enabled = Bool And HaveImage
        CopyButton.Enabled = Bool And HaveImage
        PasteMenuItem.Enabled = Bool And HaveImage And HaveClipboardImage
        PasteButton.Enabled = Bool And HaveImage And HaveClipboardImage
        InvertMenuItem.Enabled = Bool And HaveImage
        InvertButton.Enabled = Bool And HaveImage
        ClearMenuItem.Enabled = Bool And HaveSelection
        ClearButton.Enabled = Bool And HaveSelection
        RotateMenu.Enabled = Bool And HaveImage
        Rotate90Button.Enabled = Bool And HaveImage
        Rotate180Button.Enabled = Bool And HaveImage
        Rotate270Button.Enabled = Bool And HaveImage
        FlipMenu.Enabled = Bool And HaveImage
        FlipHorizontallyButton.Enabled = Bool And HaveImage
        FlipVerticallyButton.Enabled = Bool And HaveImage
        SaveImageMenuItem.Enabled = Bool And HaveImage
        PasteFromMenuItem.Enabled = Bool And HaveImage
        SelectAllMenuItem.Enabled = Bool And HaveImage
        AdjustDPIMenuItem.Enabled = Bool And HaveImage
        ColourConversionMenuItem.Enabled = Bool And HaveImage

        NUD.Enabled = (NumPages > 1)
        ToggleSelection()
    End Sub
#End Region

    ' User has clicked on About button
    Private Sub About_Click(sender As Object, e As EventArgs)
        About.ShowDialog()
    End Sub

    Private Sub PIButton_Click(sender As Object, e As EventArgs) Handles PIButton.Click
        TOCR_Btn_Click_Util(1)
    End Sub
End Class
