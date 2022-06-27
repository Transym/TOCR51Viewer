'***********************************************************************************************************************
' Batch processing form
' 
' This form allows the user to process batches of files or acquiredimages
' and works in two modes:  1)Setting the batch options and 2) showing results.
'
' Processing options are taken from the main interactive form.

Imports System.Runtime.InteropServices
Public Class Batch

#Region " Definitions "
    Private Const PAGERANGEVALIDCHARS As String = "0123456789,L- "

    Private InputFolder As String                       ' input folder for files
    Private InputFiles() As String                      ' an array of file names chosen in the input folder
    Private PageRange(,) As Integer                     ' user list of selected pages
    Private NumPageRanges As Integer                    ' number of elements of PageRange
    Private LogStrm As System.IO.StreamWriter = Nothing
    Private OutputFmt As RESULTSFMT

    Private Enum OCRSTATES As Integer       ' state of the OCR engine
        Idle = 0        ' engine is idle
        Active = 1      ' engine is working
        Abort = 2       ' user has requested an abort
        AppClose = 3    ' application is closing
    End Enum

    Private OCRState As OCRSTATES = OCRSTATES.AppClose
#End Region

#Region " Event handlers "
    Private Sub AllPagesRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllPagesRadioButton.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub AllOutputInOneRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllOutputInOneRadioButton.CheckedChanged
        UpdateControls()
        UpdateExample()
    End Sub

    Private Sub AppendExtensionRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AppendExtensionRadioButton.CheckedChanged
        UpdateExample()
    End Sub

    ' So that the form is correctly placed next time it loads
    Private Sub Batch_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Height = Me.Height - Me.ClientRectangle.Height + OptionsPanel.Top + OptionsPanel.Height
    End Sub

    Private Sub Batch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If TWAINAcquireRadioButton.Enabled Then
            On Error Resume Next
            TOCRTWAINShowUI(CShort(TWAINShowCheckBox.Checked))
            If Err.Number = ERRCANTFINDDLLENTRYPOINT Then
                TWAINAcquireRadioButton.Enabled = False
                InputFileRadioButton.Checked = True
            End If
            On Error GoTo 0
        End If

        EnablePanel("Options")
        OutputFmt = ResultsFormatInfo.Fmt
        If OutputFmt = RESULTSFMT.RTF Then
            ExtensionTextBox.Text = GetSetting(REGAPPNAME, "Batch", "Output Extension RTF", "rtf")
        Else
            ExtensionTextBox.Text = GetSetting(REGAPPNAME, "Batch", "Output Extension", "txt")
        End If
        UpdateControls()

        OCRState = OCRSTATES.Idle
    End Sub

    Private Sub CreateLogFileCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateLogFileCheckBox.CheckedChanged
        UpdateControls()
    End Sub

    ' Don't allow invalid characters in the extension text box
    Private Sub ExtensionTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ExtensionTextBox.KeyPress
        If ("\/:*?<>|""" & ControlChars.Back).Contains(e.KeyChar) Then
            Beep()
            e.Handled = True
        End If
    End Sub

    Private Sub ExtensionTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtensionTextBox.TextChanged
        UpdateExample()
    End Sub

    Private Sub InputFilesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputFilesButton.Click
        Dim OpenFileDialog As New OpenFileDialog
        Static FilterIndex As Integer
        Dim Fil As String       ' loop counter
        Dim Txt As String       ' utility
        Dim G As Graphics = Graphics.FromHwnd(InputFilesLabel.Handle)

        With OpenFileDialog
            .InitialDirectory = InputFolder
            .Title = "Select Input File"
            .FilterIndex = FilterIndex
            .Filter = "Bitmaps (*.bmp, *.dib, *.gif, *.jpeg, *.jpg, *.png, *.tif, *.tiff)|*.bmp;*.dib;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|All Files (*.*)|*.*"
            .FileName = ""
            .CheckFileExists = True
            .CheckPathExists = True
            .ShowReadOnly = False
            .Multiselect = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                InputFiles = .FileNames
                InputFolder = New System.IO.FileInfo(.FileName).DirectoryName
                Txt = ""
                For Each Fil In InputFiles
                    Txt &= (New System.IO.FileInfo(Fil)).Name & " "
                    If G.MeasureString(Txt, InputFilesLabel.Font).Width > InputFilesLabel.Width Then Exit For
                Next Fil
                InputFilesLabel.Text = Txt
                FilterIndex = .FilterIndex
            End If
            .Dispose()
        End With
        G.Dispose()
    End Sub

    Private Sub InputFileRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputFileRadioButton.CheckedChanged
        UpdateControls()
        UpdateExample()
    End Sub

    Private Sub LogFileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogFileButton.Click
        Dim SaveFileDialog As New SaveFileDialog
        Static FilterIndex As Integer = 1

        With SaveFileDialog
            .Title = "Select Log File"
            .Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            .FileName = LogFileLabel.Text
            .InitialDirectory = New System.IO.FileInfo(.FileName).DirectoryName
            .CheckFileExists = False
            .CheckPathExists = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                LogFileLabel.Text = .FileName
                FilterIndex = .FilterIndex
            End If
            .Dispose()
        End With
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OKButton.Click
        If InputFileRadioButton.Checked Then
            If InputFilesLabel.Text = "" Then
                MsgBox("You have not selected any input files", MsgBoxStyle.Exclamation)
                If InputFilesButton.Enabled Then InputFilesButton.Focus()
                Return
            End If
        End If

        If AllOutputInOneRadioButton.Checked Then
            If OutputFileLabel.Text = "" Then
                MsgBox("You have not selected an output file", MsgBoxStyle.Exclamation)
                If OutputFileButton.Enabled Then OutputFileButton.Focus()
                Return
            End If
            Try
                If System.IO.File.Exists(OutputFileLabel.Text) Then
                    If MsgBox("Are you sure you wish to overwrite " & OutputFileLabel.Text, MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation) = MsgBoxResult.No Then Return
                End If
                Try
                    Dim oWrite As New System.IO.StreamWriter(OutputFileLabel.Text)
                    oWrite.Close()
                    Kill(OutputFileLabel.Text)
                Catch ex As Exception
                    MsgBox("Output file:" & Environment.NewLine & Environment.NewLine & OutputFileLabel.Text & Environment.NewLine & Environment.NewLine & ex.Message, MsgBoxStyle.Exclamation)
                    If OutputFileButton.Enabled Then OutputFileButton.Focus()
                    Return
                End Try
            Catch ex As Exception
                MsgBox("Output file:" & Environment.NewLine & Environment.NewLine & OutputFileLabel.Text & Environment.NewLine & Environment.NewLine & ex.Message, MsgBoxStyle.Exclamation)
                If OutputFileButton.Enabled Then OutputFileButton.Focus()
                Return
            End Try
        Else
            If OutputFolderLabel.Text = "" Then
                MsgBox("You have not selected an output folder", MsgBoxStyle.Exclamation)
                Return
            End If
            If Not System.IO.Directory.Exists(OutputFolderLabel.Text) Then
                MsgBox("Output folder:" & Environment.NewLine & Environment.NewLine & OutputFolderLabel.Text & Environment.NewLine & Environment.NewLine & "Does not exist", MsgBoxStyle.Exclamation)
                If OutputFolderButton.Enabled Then OutputFolderButton.Focus()
                Return
            End If
        End If

        If CreateLogFileCheckBox.Checked Then
            If LogFileLabel.Text = "" Then
                MsgBox("You have not selected a log file", MsgBoxStyle.Exclamation)
                If LogFileButton.Enabled Then LogFileButton.Focus()
                Return
            End If
            Try
                Dim oWrite As New System.IO.StreamWriter(LogFileLabel.Text, AppendLogRadioButton.Checked)
                oWrite.Close()
            Catch ex As Exception
                MsgBox("Log file:" & Environment.NewLine & Environment.NewLine & LogFileLabel.Text & Environment.NewLine & Environment.NewLine & ex.Message, MsgBoxStyle.Exclamation)
                If LogFileButton.Enabled Then LogFileButton.Focus()
                Return
            End Try
        End If

        If SomePagesRadioButton.Checked Then
            If Not CheckPageRanges() Then
                If PagesTextBox.Enabled Then PagesTextBox.Focus()
                Return
            End If
        End If

        If InputFileRadioButton.Checked Then
            InputFolderLabel.Text = InputFolder
        Else
            InputFolderLabel.Text = "TWAIN Device"
        End If
        If AllOutputInOneRadioButton.Checked Then
            OutputFolderLabel2.Text = OutputFileLabel.Text
        Else
            OutputFolderLabel2.Text = OutputFolderLabel.Text
        End If
        If CreateLogFileCheckBox.Checked Then
            LogFileLabel2.Text = LogFileLabel.Text
        Else
            LogFileLabel2.Text = "<no logging>"
        End If

        If InputFileRadioButton.Checked Then
            InputFilesLabel2.Text = "0 of " & CStr(InputFiles.GetUpperBound(0) + 1)
        Else
            InputFilesLabel2.Text = "0"
        End If
        OutputFilesLabel.Text = "0"
        FailedFilesLabel.Text = "0"
        CurrentFileLabel.Text = ""

        EnablePanel("Results")
    End Sub

    Private Sub OutputFileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputFileButton.Click
        Dim SaveFileDialog As New SaveFileDialog
        Static FilterIndex As Integer = 1

        With SaveFileDialog
            .Title = "Select Ouput File"
            .Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            .FilterIndex = FilterIndex
            .FileName = OutputFileLabel.Text
            .InitialDirectory = New System.IO.FileInfo(.FileName).DirectoryName
            .CheckFileExists = False
            .CheckPathExists = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                OutputFileLabel.Text = .FileName
                FilterIndex = .FilterIndex
            End If
            .Dispose()
        End With
    End Sub

    Private Sub OutputFolderButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputFolderButton.Click
        Dim FolderBrowserDialog As New FolderBrowserDialog

        With FolderBrowserDialog
            .Description = "Select the folder for output files"
            .ShowNewFolderButton = True
            .SelectedPath = OutputFolderLabel.Text
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                OutputFolderLabel.Text = .SelectedPath
                If Not OutputFolderLabel.Text.EndsWith("\") Then OutputFolderLabel.Text &= "\"
                UpdateExample()
            End If
            .Dispose()
        End With
    End Sub

    Private Sub OutputPerInputFileRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputPerInputFileRadioButton.CheckedChanged
        UpdateControls()
        UpdateExample()
    End Sub

    Private Sub OutputPerInputPageRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputPerInputPageRadioButton.CheckedChanged
        UpdateControls()
        UpdateExample()
    End Sub

    Private Sub OverwriteExistingCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OverwriteExistingCheckBox.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub OverwriteExtensionRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OverwriteExtensionRadioButton.CheckedChanged
        UpdateExample()
    End Sub

    Private Sub PagesTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles PagesTextBox.KeyPress
        If Not (PAGERANGEVALIDCHARS & ControlChars.Back).Contains(e.KeyChar) Then
            Beep()
            e.Handled = True
        End If
    End Sub

    Private Sub ResultsPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ResultsPanel.Paint
        Dim G As Graphics = e.Graphics
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, 441, 157))
        G.DrawLine(Pens.Black, 0, 68, 441, 68)
    End Sub

    Private Sub SaveImagesCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveImagesCheckBox.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub SomePagesRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SomePagesRadioButton.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub StartStopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StartStopButton.Click
        Select Case OCRState
            Case OCRSTATES.Idle

                Select Case StartStopButton.Text
                    Case "Start"
                        Cursor.Current = Cursors.WaitCursor
                        StartStopButton.Text = "Stop"
                        BatchProcess()
                        StartStopButton.Text = "OK"
                        Cursor.Current = Cursors.Default

                    Case "Stop"
                        Cursor.Current = Cursors.Default
                        StartStopButton.Text = "OK"

                    Case "OK"
                        Cursor.Current = Cursors.Default
                        EnablePanel("Options")
                End Select ' cmdStart.Caption

            Case OCRSTATES.AppClose
                Me.Close()
                Return

            Case Else
                OCRState = OCRSTATES.Abort
        End Select ' mOCRState
    End Sub

    Private Sub TWAINAcquireRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TWAINAcquireRadioButton.CheckedChanged
        UpdateControls()
        UpdateExample()
    End Sub

    Private Sub TWAINSelectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TWAINSelectButton.Click
        TOCRTWAINSelectDS()
    End Sub

    Private Sub TWAINShowCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TWAINShowCheckBox.CheckedChanged
        TOCRTWAINShowUI(CShort(TWAINShowCheckBox.Checked))
    End Sub
#End Region

#Region " Private routines "
    ' Main batch processing routine
    Private Sub BatchProcess()
        Dim JI_EG As New TOCRJOBINFO_EG ' job processing information
        Dim PrvErrorMode As Integer ' previous API error mode for gJobNo
        Dim PrvDefErrorMode As Integer ' previous API error mode for default
        Dim Status As Integer       ' returned API status
        Dim OutputDir As String     ' result soutput folder
        Dim OutputName As String    ' name of single results file

        JI_EG.Initialize()

        OutputFilesLabel.Text = "0"
        FailedFilesLabel.Text = "0"

        If AllOutputInOneRadioButton.Checked Then
            OutputName = New System.IO.FileInfo(OutputFileLabel.Text).Name
            OutputDir = New System.IO.FileInfo(OutputFileLabel.Text).DirectoryName & "\"
        Else
            OutputName = ""
            OutputDir = OutputFolderLabel.Text
        End If

        On Error Resume Next

        ' Open the log file

        If CreateLogFileCheckBox.Checked Then
            If AppendLogRadioButton.Checked Then
                LogStrm = System.IO.File.AppendText(LogFileLabel.Text)
            Else
                LogStrm = System.IO.File.CreateText(LogFileLabel.Text)
            End If
            If Err.Number <> 0 Then
                MsgBox(Err.Description & Environment.NewLine & LogFileLabel.Text, MsgBoxStyle.Critical)
                Return
            End If
            LogStrm.WriteLine("Batch run started " & Today & " " & TimeOfDay)
            If TWAINAcquireRadioButton.Checked Then
                LogStrm.WriteLine("Input: " & InputFolderLabel.Text)
            Else
                LogStrm.WriteLine("Input folder: " & InputFolderLabel.Text)
            End If
            If AllOutputInOneRadioButton.Checked Then
                LogStrm.WriteLine("Output file: " & OutputFileLabel.Text)
            Else
                LogStrm.WriteLine("Output folder: " & OutputDir)
            End If
        End If

        SaveSettings()

        OCRState = OCRSTATES.Active

        ' Turn off error reporting - not only for the Job but also the default
        ' because if you try to access the job after it has failed the default is used

        TOCRGetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, PrvErrorMode)
        TOCRSetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_NONE)

        TOCRGetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, PrvDefErrorMode)
        TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_NONE)

        DeepCopyProcessOptions(JobInfo_EG.ProcessOptions, JI_EG.ProcessOptions)

        ' Get the position of characters after rotating
        JI_EG.ProcessOptions.ResultsReference = TOCRRESULTSREFERENCE_AFTER

        If InputFileRadioButton.Checked Then
            BatchProcessFiles(JI_EG, OutputDir, OutputName)
        Else
            BatchProcessTWAIN(JI_EG, OutputDir, OutputName)
        End If

        ' Close the log file

        If CreateLogFileCheckBox.Checked And Not LogStrm Is Nothing Then
            LogStrm.Close()
            LogStrm.Dispose()
            LogStrm = Nothing
        End If

        If OCRState = OCRSTATES.Abort Or OCRState = OCRSTATES.AppClose Then
            TOCRShutdown(JobNo)
            System.Windows.Forms.Application.DoEvents() : System.Threading.Thread.Sleep(100) : System.Windows.Forms.Application.DoEvents()
            TOCRInitialise(JobNo)
        End If

        ' Restore error reporting

        TOCRSetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, PrvErrorMode)
        TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, PrvDefErrorMode)

        If OCRState = OCRSTATES.AppClose Then
            Me.Close()
            Return
        End If

        OCRState = OCRSTATES.Idle
        Return
    End Sub

    ' Process files (as opposed to TWAIN acquired images)
    Private Sub BatchProcessFiles(ByRef JI_EG As TOCRJOBINFO_EG, ByVal OutputDir As String, ByVal OutputName As String)
        Dim MsgString As String         ' string for MsgBox
        Dim OutputStub As String = ""   ' current output file name (minus extension)
        Dim OutputExt As String = ""    ' current output file extension
        Dim NumPages As Integer         ' number of pages in a multipage file
        Dim PageNo As Integer           ' loop counter for pages
        Dim NumInputFiles As Integer    ' number of files opened
        Dim SeparatorText As String = "" ' text to separate results when more than one set in a file
        Dim FirstPage As Boolean        ' flag if first page of a multi-page file
        Dim FirstPageNo As Integer      ' minimum page number that can be processed
        Dim LastPageNo As Integer       ' maximum page number that can be processed
        Dim MinPageNo As Integer        ' first page number to process
        Dim MaxPageNo As Integer        ' last page number to process
        Dim PageDigits As Integer       ' maximum page number digits
        Dim Done As Boolean             ' flag if file processing successful
        Dim Msg As String               ' a message for the log file
        Dim Status As Integer           ' returned API status
        Dim FilNo As Integer            ' loop counter
        Dim ImageTmp As Image = Nothing
        Dim FrameDimension As Imaging.FrameDimension

        For FilNo = 0 To InputFiles.GetUpperBound(0)
            On Error GoTo BPFNextFile

            MsgString = ""

            NumInputFiles = NumInputFiles + 1
            InputFilesLabel2.Text = CStr(NumInputFiles) & " of " & CStr(InputFiles.GetUpperBound(0) + 1)

            Done = False
            PageNo = 0
            NumPages = 1

            Msg = DetectFileType(InputFiles(FilNo))
            If Msg = "GIF" Then
                FrameDimension = Imaging.FrameDimension.Time
            Else
                FrameDimension = Imaging.FrameDimension.Page
            End If
            Select Case Msg
                Case "BMP"
                    JI_EG.JobType = TOCRJOBTYPE_DIBFILE
                    NumPages = 1
                    MinPageNo = 1
                    MaxPageNo = 1
                Case "TIF"
                    JI_EG.JobType = TOCRJOBTYPE_TIFFFILE
                    Status = TOCRGetNumPages(JobNo, InputFiles(FilNo), TOCRJOBTYPE_TIFFFILE, NumPages)
                    If Status <> TOCR_OK Then GoTo BPFNextFile
                    MinPageNo = 1
                    MaxPageNo = NumPages
                Case Else
                    JI_EG.JobType = TOCRJOBTYPE_MMFILEHANDLE
                    ImageTmp = Image.FromFile(InputFiles(FilNo))
                    NumPages = ImageTmp.GetFrameCount(FrameDimension)
                    MinPageNo = 1
                    MaxPageNo = NumPages
            End Select

            ' Check there are pages to process

            If AllPagesRadioButton.Checked Then
                FirstPageNo = MinPageNo
                LastPageNo = MaxPageNo
            Else

                ' reduce the range to only those required
                FirstPageNo = Integer.MaxValue
                LastPageNo = Integer.MinValue

                For PageNo = MinPageNo To MaxPageNo
                    If IsPageInRange(PageNo, NumPages) Then
                        If PageNo < FirstPageNo Then FirstPageNo = PageNo
                        If PageNo > LastPageNo Then LastPageNo = PageNo
                    End If
                Next PageNo
                If FirstPageNo > LastPageNo Then
                    LogMessage(InputFiles(FilNo), "does not contain any pages in range ")
                    GoTo BPFNextFile
                End If
            End If

            ' Get the extension on the file for output (may add back later)

            If Not AllOutputInOneRadioButton.Checked Then
                OutputExt = New System.IO.FileInfo(InputFiles(FilNo)).Extension
                OutputStub = New System.IO.FileInfo(InputFiles(FilNo)).Name
                OutputStub = OutputStub.Substring(0, OutputStub.Length - OutputExt.Length)
                PageDigits = CStr(LastPageNo).Length
            End If

            If OCRState <> OCRSTATES.Active Then Exit For

            ' Process each page in the file

            JI_EG.InputFile = InputFiles(FilNo)
            FirstPage = True
            For PageNo = FirstPageNo To LastPageNo

                If AllPagesRadioButton.Checked Or IsPageInRange(PageNo, NumPages) Then

                    System.Windows.Forms.Application.DoEvents()

                    If NumPages > 1 Then
                        MsgString = InputFiles(FilNo) & " Page " & CStr(PageNo) & " of " & CStr(NumPages)
                    Else
                        MsgString = InputFiles(FilNo)
                    End If

                    ' Create the output filename and check if need to delete

                    If Not AllOutputInOneRadioButton.Checked Then
                        If OutputPerInputPageRadioButton.Checked And NumPages > 1 Then
                            OutputName = New String("0"c, PageDigits) & CStr(PageNo)
                            OutputName = OutputName.Substring(OutputName.Length - PageDigits)
                            OutputName = OutputStub & "_Page_" & OutputName
                        Else
                            OutputName = OutputStub
                        End If

                        If AppendExtensionRadioButton.Checked Then ' put the original extension back
                            OutputName = OutputName & OutputExt
                        End If
                        OutputName = OutputName & "." & ExtensionTextBox.Text

                        If FirstPage Or OutputPerInputPageRadioButton.Checked Then
                            If System.IO.File.Exists(OutputDir & OutputName) Then
                                If OverwriteExistingCheckBox.Checked Then
                                    Kill(OutputDir & OutputName)
                                Else
                                    LogMessage(MsgString, OutputName & " exists and was not overwritten")
                                    GoTo BPFNextFile
                                End If
                            End If
                        End If
                    End If

                    If OCRState <> OCRSTATES.Active Then Exit For

                    ' OCR the page

                    If Not OutputPerInputPageRadioButton.Checked Then
                        SeparatorText = Separator(New System.IO.FileInfo(InputFiles(FilNo)).Name, PageNo, NumPages)
                    End If

                    If JI_EG.JobType = TOCRJOBTYPE_MMFILEHANDLE Then
                        JI_EG.PageNo = 0
                        ImageTmp.SelectActiveFrame(FrameDimension, PageNo - 1) ' Zero based
                        Dim BMP As New Bitmap(ImageTmp)
                        BMP.SetResolution(ImageTmp.HorizontalResolution, ImageTmp.VerticalResolution)
                        JI_EG.hMMF = ConvertBitmapToMMF(BMP)
                        BMP.Dispose()
                        If JI_EG.hMMF <> IntPtr.Zero Then
                            Done = OCR(JI_EG, MsgString, OutputDir & OutputName, SeparatorText)
                        End If
                    Else
                        JI_EG.PageNo = PageNo - 1 ' 0 based
                        Done = OCR(JI_EG, MsgString, OutputDir & OutputName, SeparatorText)
                    End If

                    If OCRState <> OCRSTATES.Active Then Exit For

                    FirstPage = False

                End If ' IsPageInRange(PageNo) Then

            Next PageNo

            ' - - - - - - - - Error Handler and Cleanup - - - - -
BPFNextFile:
            If Not ImageTmp Is Nothing Then
                ImageTmp.Dispose()
                ImageTmp = Nothing
            End If
            If Err.Number <> 0 Then
                If MsgString = "" Then
                    LogMessage(InputFiles(FilNo), Err.Description)
                Else
                    LogMessage(MsgString, Err.Description)
                End If
                'Err.Clear   ' need to resume in case of more errors
                Resume BPFNextFileResume
            End If

BPFNextFileResume:

            If Not Done Then
                FailedFilesLabel.Text = CStr(Val(FailedFilesLabel.Text) + 1)
            End If

            If OCRState <> OCRSTATES.Active Then Exit For

        Next FilNo
    End Sub

    ' Process TWAIN Images
    Private Sub BatchProcessTWAIN(ByRef JI_Eg As TOCRJOBINFO_EG, ByVal OutputDir As String, ByVal OutputName As String)
        Dim NumImages As Integer        ' number of images acquired
        Dim hMem() As IntPtr            ' array of pointers to global memory blocks containing acquired bitmaps
        Dim DIBNo As Integer            ' loop counter
        Dim Filename As String          ' file name (for separator)
        Dim ImageName As String = ""    ' current input image name
        Dim SeparatorText As String = "" ' text to separate results when more than one set in a file
        Dim Done As Boolean             ' flag if OCR successful
        Dim DoIt As Boolean             ' flag if to OCR
        Dim FileNo As Integer           ' current output file number
        Dim Status As Integer           ' returned API status
        Dim OutputBMPName As String     ' name of bitmap file
        Dim FileNumber As String        ' file number as a string
        Dim di As IO.DirectoryInfo
        Dim fiArr As IO.FileInfo()
        Dim fi As IO.FileInfo
        Dim MemGC As GCHandle           ' handle used to pin individual elements of hMem
        Dim BMP As Bitmap = Nothing

        NumImages = 0

        ReDim hMem(0)
        On Error GoTo BPTErrs

        ' Find the next output file number, if required.
        ' If overwrite files then start at 1
        FileNo = 1
        If Not OverwriteExistingCheckBox.Checked Then
            If Not AllOutputInOneRadioButton.Checked Or SaveImagesCheckBox.Checked Then
                di = New IO.DirectoryInfo(OutputFolderLabel.Text)
                fiArr = di.GetFiles("Img*." & ExtensionTextBox.Text)
                For Each fi In fiArr
                    FileNumber = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length)
                    If IsNumeric(FileNumber) Then
                        If CInt(FileNumber) >= FileNo Then FileNo = CInt(FileNumber) + 1
                    End If
                Next
                di = New IO.DirectoryInfo(OutputFolderLabel.Text)
                fiArr = di.GetFiles("Img*.bmp")
                For Each fi In fiArr
                    FileNumber = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length)
                    If IsNumeric(FileNumber) Then
                        If CInt(FileNumber) >= FileNo Then FileNo = CInt(FileNumber) + 1
                    End If
                Next
            End If
        End If

        Status = TOCRTWAINAcquire(NumImages)
        If Status = TOCERR_TWAINPARTIALACQUIRE Then
            LogMessage("TWAIN", "Error - not all images were acquired")
        End If

        If NumImages > 0 Then
            InputFilesLabel2.Text = "0 of " & CStr(NumImages)
            JI_Eg.JobType = TOCRJOBTYPE_MMFILEHANDLE
            ReDim hMem(NumImages - 1)

            MemGC = GCHandle.Alloc(hMem, GCHandleType.Pinned)
            If TOCRTWAINGetImages(MemGC.AddrOfPinnedObject) = TOCR_OK Then
                MemGC.Free()
                For DIBNo = 0 To NumImages - 1
                    InputFilesLabel2.Text = CStr(DIBNo + 1) & " of " & CStr(NumImages)
                    Filename = "Img" & CStr(FileNo)
                    If NumImages > 1 Then
                        ImageName = Filename & " " & CStr(DIBNo + 1) & " of " & CStr(NumImages)
                    Else
                        ImageName = Filename
                    End If

                    ' Convert the DIBs to bitmap handles

                    Done = False
                    BMP = ConvertMemoryBlockToBitmap(hMem(DIBNo))
                    KRN.GlobalFree(hMem(DIBNo))
                    hMem(DIBNo) = IntPtr.Zero
                    If Not BMP Is Nothing Then
                        If SaveImagesCheckBox.Checked Then
                            OutputBMPName = "Img" & CStr(FileNo) & ".bmp"
                            If System.IO.File.Exists(OutputDir & OutputBMPName) Then
                                If OverwriteExistingCheckBox.Checked Then
                                    On Error Resume Next
                                    Kill(OutputDir & OutputBMPName)
                                    If Err.Number > 0 Then
                                        LogMessage(ImageName, "Failed to delete " & OutputDir & OutputBMPName)
                                    Else
                                        BMP.Save(OutputDir & OutputBMPName, Imaging.ImageFormat.Bmp)
                                    End If
                                    On Error GoTo BPTErrs
                                Else
                                    LogMessage(ImageName, OutputDir & OutputBMPName & " exists and was not overwritten")
                                End If
                            Else
                                BMP.Save(OutputDir & OutputBMPName, Imaging.ImageFormat.Bmp)
                            End If
                        End If

                        DoIt = True
                        If Not AllOutputInOneRadioButton.Checked Then
                            OutputName = "Img" & CStr(FileNo) & "." & ExtensionTextBox.Text
                            If System.IO.File.Exists(OutputDir & OutputName) Then
                                If OverwriteExistingCheckBox.Checked Then
                                    Kill(OutputDir & OutputName)
                                Else
                                    LogMessage(ImageName, OutputName & " exists and was not overwritten")
                                    DoIt = False
                                End If
                            End If
                        End If

                        If DoIt Then
                            JI_Eg.hMMF = ConvertBitmapToMMF(BMP)
                            If JI_Eg.hMMF <> IntPtr.Zero Then
                                JI_Eg.PageNo = 0

                                If Not OutputPerInputPageRadioButton.Checked Then
                                    SeparatorText = Separator(Filename, DIBNo + 1, NumImages)
                                End If
                                Done = OCR(JI_Eg, ImageName, OutputDir & OutputName, SeparatorText)

                                KRN.CloseHandle(JI_Eg.hMMF)
                                JI_Eg.hMMF = IntPtr.Zero
                                If Done Then FileNo = FileNo + 1
                            Else
                                LogMessage(ImageName, "Failed to convert bitmap to MMF")
                            End If ' SaveMonoBitmapToMMFile(BI, hFile)
                        End If

                        BMP.Dispose()
                        BMP = Nothing
                    Else
                        LogMessage(ImageName, "Failed to get bitmap from memory block")
                    End If ' GetMonoBitmapFromDIB(BI, hDIB(DIBNo), False)

                    If Not Done Then
                        FailedFilesLabel.Text = CStr(Val(FailedFilesLabel.Text) + 1)
                    End If
                    If OCRState <> OCRSTATES.Active Then Exit For
                Next DIBNo
            Else
                MemGC.Free()
                FailedFilesLabel.Text = CStr(NumImages)
                LogMessage("TWAIN", "No images were returned")
            End If ' TOCRTWAINGetImages(hDIB(0)) = TOCR_OK
        Else
            LogMessage("TWAIN", "No images were acquired")
        End If ' NumImages

        ' - - - - - - - - Error Handler and Cleanup - - - - -
BPTErrs:

        For DIBNo = 0 To NumImages - 1
            If hMem(DIBNo) <> IntPtr.Zero Then
                KRN.GlobalFree(hMem(DIBNo))
                hMem(DIBNo) = IntPtr.Zero
            End If
        Next DIBNo
        If Not BMP Is Nothing Then
            BMP.Dispose()
            BMP = Nothing
        End If

        If JI_Eg.hMMF <> IntPtr.Zero Then
            KRN.CloseHandle(JI_Eg.hMMF)
            JI_Eg.hMMF = IntPtr.Zero
        End If

        If Err.Number > 0 Then
            TOCRTWAINGetImages(IntPtr.Zero)
            LogMessage(ImageName, Err.Description)
        End If

        Return
    End Sub

    ' If the user has specified 'Some Pages' check the page ranges and load these into PageRange()
    Private Function CheckPageRanges() As Boolean
        Dim Range() As String ' array of page ranges
        Dim Page() As String ' elements of a page range
        Dim No As Integer ' loop counter
        Dim No2 As Integer ' loop counter
        Dim Txt As String ' utility text string
        Dim FromPage As Integer ' from page number
        Dim ToPage As Integer ' to page number

        NumPageRanges = 0
        Txt = PagesTextBox.Text
        If Txt = "" Then
            MsgBox("No page ranges specified", MsgBoxStyle.Exclamation)
            Return False
        End If

        ' Check for Valid Characters
        For No = 0 To Txt.Length - 1
            If Not PAGERANGEVALIDCHARS.Contains(Txt.Substring(No, 1)) Then
                MsgBox("Invalid character " & Txt.Substring(No, 1), MsgBoxStyle.Exclamation)
                Return False
            End If
        Next No

        ' Split out each comma separated range

        Range = Split(Txt, ",")
        ReDim PageRange(1, Range.GetUpperBound(0))

        ' Check each range

        For No = 0 To Range.GetUpperBound(0)
            Range(No) = Trim$(Range(No))
            If Range(No) <> "" Then

                ' Split the page numbers in each range

                Page = Split(Range(No), "-")
                If Page.GetUpperBound(0) > 3 Then
                    MsgBox("Invalid page range " & Range(No), MsgBoxStyle.Exclamation)
                    Return False
                End If

                ' Check for invalid values

                Txt = ""
                For No2 = 0 To Page.GetUpperBound(0)
                    Page(No2) = (Trim$(Page(No2))).ToUpper
                    If Page(No2) = "" Then
                        MsgBox("Invalid page range " & Range(No), MsgBoxStyle.Exclamation)
                        Return False
                    End If

                    ' Can't allow 0 as we use this for the last page

                    If Page(No2) = "0" Then
                        MsgBox("Invalid zero page " & Range(No), MsgBoxStyle.Exclamation)
                        Return False
                    End If

                    ' Build a proforma for the format of the range and set each L to 0
                    ' this just saves time checking and converting

                    If Page(No2) = "L" Then
                        Page(No2) = CStr(0)
                        Txt = Txt & "L"
                    Else
                        Txt = Txt & "#"
                    End If
                Next No2

                ' Check for a valid proforma

                If Not " # L L# ## #L #L# L## L#L L#L# ".Contains(" " & Txt & " ") Then
                    MsgBox("Invalid page range format " & Range(No), MsgBoxStyle.Exclamation)
                    Return False
                End If

                Select Case Page.GetUpperBound(0) + 1
                    Case 1 ' # or L
                        PageRange(0, NumPageRanges) = CInt(Page(0))
                        PageRange(1, NumPageRanges) = CInt(Page(0))

                    Case 2 ' L# ## or #L
                        If Page(0) <> "0" Then ' ##
                            PageRange(0, NumPageRanges) = CInt(Page(0))
                            PageRange(1, NumPageRanges) = CInt(Page(1))
                        Else ' L# or #L
                            If Page(0) = "0" Then ' L#
                                PageRange(0, NumPageRanges) = -CInt(Page(1))
                            Else ' #L
                                PageRange(0, NumPageRanges) = CInt(Page(0))
                            End If
                            PageRange(1, NumPageRanges) = -CInt(Page(1))
                        End If

                    Case 3
                        If Page(0) = "0" Then ' L## or L#L
                            PageRange(0, NumPageRanges) = -CInt(Page(1))
                            PageRange(1, NumPageRanges) = CInt(Page(2))
                        Else ' #L#
                            PageRange(0, NumPageRanges) = CInt(Page(0))
                            PageRange(1, NumPageRanges) = -CInt(Page(2))
                        End If
                    Case 4 ' L#L#
                        PageRange(0, NumPageRanges) = -CInt(Page(1))
                        PageRange(1, NumPageRanges) = -CInt(Page(3))

                End Select ' NumElems

                ' Check the range is not negative

                FromPage = PageRange(0, NumPageRanges)
                ToPage = PageRange(1, NumPageRanges)
                If FromPage <= 0 Then FromPage = FromPage + 100000
                If ToPage <= 0 Then ToPage = ToPage + 100000
                If ToPage < FromPage Then
                    MsgBox("Invalid negative range " & Range(No), MsgBoxStyle.Exclamation)
                    Return False
                End If
                NumPageRanges = NumPageRanges + 1
            End If
        Next No

        If NumPageRanges = 0 Then
            MsgBox("No page ranges specified", MsgBoxStyle.Exclamation)
            Return False
        End If

        Return True
    End Function

    Private Sub DeepCopyProcessOptions(ByRef JIPIn As TOCRPROCESSOPTIONS_EG, ByRef JIPOut As TOCRPROCESSOPTIONS_EG)
        JIPOut.StructId = JIPIn.StructId
        JIPOut.InvertWholePage = JIPIn.InvertWholePage
        JIPOut.DeskewOff = JIPIn.DeskewOff
        JIPOut.Orientation = JIPIn.Orientation
        JIPOut.NoiseRemoveOff = JIPIn.NoiseRemoveOff
        JIPOut.ReturnNoiseOn = JIPIn.ReturnNoiseOn
        JIPOut.LineRemoveOff = JIPIn.LineRemoveOff
        JIPOut.DeshadeOff = JIPIn.DeshadeOff
        JIPOut.InvertOff = JIPIn.InvertOff
        JIPOut.SectioningOn = JIPIn.SectioningOn
        JIPOut.MergeBreakOff = JIPIn.MergeBreakOff
        JIPOut.LineRejectOff = JIPIn.LineRejectOff
        JIPOut.CharacterRejectOff = JIPIn.CharacterRejectOff
        JIPOut.ResultsReference = JIPIn.ResultsReference
        JIPOut.LexMode = JIPIn.LexMode
        JIPOut.OCRBOnly = JIPIn.OCRBOnly
        JIPOut.Speed = JIPIn.Speed
        JIPOut.FontStyleInfoOff = JIPIn.FontStyleInfoOff
        JIPOut.CCAlgorithm = JIPIn.CCAlgorithm
        JIPOut.CCThreshold = JIPIn.CCThreshold
        JIPOut.CGAlgorithm = JIPIn.CGAlgorithm
        JIPOut.ExtraInfFlags = JIPIn.ExtraInfFlags
        Array.Copy(JIPIn.DisableLangs, JIPOut.DisableLangs, JIPIn.DisableLangs.GetUpperBound(0) + 1)
        Array.Copy(JIPIn.DisableCharW, JIPOut.DisableCharW, JIPIn.DisableCharW.GetUpperBound(0) + 1)
    End Sub

    ' Swap between the two panels
    Private Sub EnablePanel(ByVal Which As String)
        If Which.Equals("Options", StringComparison.OrdinalIgnoreCase) Then
            OptionsPanel.Enabled = True
            ResultsPanel.Enabled = False
            ResultsPanel.SendToBack()
            Me.Height = Me.Height - Me.ClientRectangle.Height + OptionsPanel.Top + OptionsPanel.Height
            InFilesGroupBox.Focus()
        Else
            ResultsPanel.Enabled = True
            ResultsPanel.BringToFront()
            Me.Height = Me.Height - Me.ClientRectangle.Height + ResultsPanel.Top + ResultsPanel.Height
            OptionsPanel.Enabled = False
            StartStopButton.Text = "Start"
            StartStopButton.Focus()
        End If
    End Sub

    ' If the user has specified Some Pages check the page number is in one of
    ' the ranges
    Private Function IsPageInRange(ByVal PageNo As Integer, ByVal NumPages As Integer) As Boolean
        Dim No As Integer       ' loop counter
        Dim FromPage As Integer ' from page number
        Dim ToPage As Integer   ' to page number

        For No = 0 To NumPageRanges - 1
            FromPage = PageRange(0, No)
            ToPage = PageRange(1, No)
            If FromPage <= 0 Then FromPage = FromPage + NumPages
            If ToPage <= 0 Then ToPage = ToPage + NumPages
            If PageNo >= FromPage And PageNo <= ToPage Then
                Return True
            End If
        Next No
        Return False
    End Function

    ' Load Registry settings
    ' Maintain compatibility with old VB6 settings
    Private Sub LoadSettings()
        Dim Value As Integer        ' utility
        Dim Bool As Boolean         ' boolean flag

        InputFolder = GetSetting(REGAPPNAME, "Batch", "Input Folder", System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))

        If Not InputFolder.EndsWith("\") Then InputFolder &= "\"

        Value = CInt(GetSetting(REGAPPNAME, "Batch", "Input Mode", "0"))
        Value = Math.Min(Math.Max(Value, 0), 1)
        If Value = 0 Then
            InputFileRadioButton.Checked = True
        Else
            TWAINAcquireRadioButton.Checked = True
        End If

        TWAINShowCheckBox.Checked = CBool(GetSetting(REGAPPNAME, "Batch", "Show UI", "True"))

        OutputFolderLabel.Text = GetSetting(REGAPPNAME, "Batch", "Output Folder", InputFolder)
        If Not OutputFolderLabel.Text.EndsWith("\") Then OutputFolderLabel.Text &= "\"

        OutputFileLabel.Text = GetSetting(REGAPPNAME, "Batch", "Output File", OutputFolderLabel.Text & "Results.txt")

        Value = CInt(GetSetting(REGAPPNAME, "Batch", "Output Mode", "1"))
        Value = Math.Min(Math.Max(Value, 0), 2)
        If Value = 0 Then
            AllOutputInOneRadioButton.Checked = True
        ElseIf Value = 1 Then
            OutputPerInputFileRadioButton.Checked = True
        Else
            OutputPerInputPageRadioButton.Checked = True
        End If

        OverwriteExistingCheckBox.Checked = CBool(GetSetting(REGAPPNAME, "Batch", "Overwrite Output Files", "True"))
        If OutputFmt = RESULTSFMT.RTF Then
            ExtensionTextBox.Text = GetSetting(REGAPPNAME, "Batch", "Output Extension RTF", "txt")
        Else
            ExtensionTextBox.Text = GetSetting(REGAPPNAME, "Batch", "Output Extension", "txt")
        End If
        Value = CInt(GetSetting(REGAPPNAME, "Batch", "Replace Extension", "1"))
        Value = Math.Min(Math.Max(Value, 0), 1)
        If Value = 0 Then
            AppendExtensionRadioButton.Checked = True
        Else
            OverwriteExtensionRadioButton.Checked = True
        End If

        SaveImagesCheckBox.Checked = CBool(GetSetting(REGAPPNAME, "Batch", "Save Images", "True"))

        Bool = CBool(GetSetting(REGAPPNAME, "Batch", "All Pages", "True"))
        If Bool Then
            AllPagesRadioButton.Checked = True
        Else
            SomePagesRadioButton.Checked = True
        End If

        PagesTextBox.Text = GetSetting(REGAPPNAME, "Batch", "Page Number", "1")
        SeparatorTextBox.Text = GetSetting(REGAPPNAME, "Batch", "Page Separator Mask", "%c%c###### %f Page %p of %n ######%c%c")

        ' Don't need to check if the log file path is OK because if it isn't an error will be generated
        ' when it is created

        CreateLogFileCheckBox.Checked = CBool(GetSetting(REGAPPNAME, "Batch", "Log Messages", "True"))
        LogFileLabel.Text = GetSetting(REGAPPNAME, "Batch", "Log File", OutputFolderLabel.Text & "BatchLog.txt")
        Value = CInt(GetSetting(REGAPPNAME, "Batch", "Overwrite Log", "1"))
        Value = Math.Min(Math.Max(Value, 0), 1)
        If Value = 0 Then
            AppendLogRadioButton.Checked = True
        Else
            OverwriteLogRadioButton.Checked = True
        End If
    End Sub

    ' Clean up the message string for output to the log file
    ' If it has come from the OCR engine it may contain Tabs, Linefeeds etc
    Private Sub LogMessage(ByVal ImageName As String, ByVal Msg As String)
        If CreateLogFileCheckBox.Checked Then
            ' Convert tabs to spaces
            Msg = Trim$(Msg.Replace(vbTab, " "))

            ' Remove any trailing CrLf
            While Msg.EndsWith(Environment.NewLine)
                Msg = Trim$(Msg.Substring(0, Msg.Length - 2))
            End While

            ' Convert CrLf to ", "
            Msg = Trim$(Msg.Replace(Environment.NewLine, ", "))

            ' Remove any trailing LF
            While Msg.EndsWith(vbLf)
                Msg = Trim$(Msg.Substring(0, Msg.Length - 2))
            End While

            ' Convert LF to ","
            Msg = Trim$(Msg.Replace(Environment.NewLine, ", "))

            LogStrm.WriteLine(ImageName & " - " & Msg)
        End If
    End Sub

    ' OCR an image
    Private Function OCR(ByRef JI_EG As TOCRJOBINFO_EG, ByVal ImageName As String, ByVal OutputFile As String, ByVal SeparatorText As String) As Boolean
        Dim Progress As Single              ' OCR progress
        Dim OldProgress As Single           ' previous OCR progress
        Dim Dummy As Integer                ' don't care argument
        Dim Status As Integer               ' returned API status
        Dim JobStatus As Integer            ' returned Job status
        Dim ResultsEx_EG As New TOCRRESULTSEX_EG  ' results of OCR
        Dim ItemNo As Integer               ' loop counter
        Dim OutputFH As Integer             ' file handle to current output file
        Dim Msg As New System.Text.StringBuilder(TOCRJOBMSGLENGTH) ' a message for the log file
        Dim PosCha As Integer               ' utility
        Dim OpStrm As System.IO.StreamWriter = Nothing

        OutputFH = 0
        OCR = False

        On Error GoTo BPOErrs

        CurrentFileLabel.Text = ImageName & " - 0%"

        Status = TOCRDoJob_EG(JobNo, JI_EG)

        OldProgress = -1
        If Status = TOCR_OK Then
            Do
                Status = TOCRGetJobStatusEx2(JobNo, JobStatus, Progress, Dummy, Dummy)
                If Status = TOCR_OK Then
                    Progress = CInt(CInt(Progress * 1000) / 10)
                    If Progress <> OldProgress Then
                        CurrentFileLabel.Text = ImageName & " - " & Progress.ToString("0.0") & "%"
                        OldProgress = Progress
                    End If
                Else
                    CurrentFileLabel.Text = ImageName & " FAILED"
                End If
                If JobStatus = TOCRJOBSTATUS_BUSY Then
                    System.Threading.Thread.Sleep(200) : System.Windows.Forms.Application.DoEvents()
                End If
                If OCRState <> OCRSTATES.Active Then Exit Do
            Loop While Status = TOCR_OK And JobStatus = TOCRJOBSTATUS_BUSY

            ' Output results

            If Status = TOCR_OK And JobStatus = TOCRJOBSTATUS_DONE Then
                If GetResults_EG(JobNo, ResultsEx_EG) Then
                    If ResultsEx_EG.Hdr.NumItems > 0 Then

                        ' Have results so open the output file

                        OutputFH = FreeFile()
                        If Not OutputPerInputPageRadioButton.Checked And System.IO.File.Exists(OutputFile) Then
                            OpStrm = System.IO.File.AppendText(OutputFile)
                        Else
                            OpStrm = System.IO.File.CreateText(OutputFile)
                            OutputFilesLabel.Text = CStr(CInt(OutputFilesLabel.Text) + 1)
                        End If

                        Dim RTF As New RichTextBox
                        RTF.Multiline = True
                        RTF.WordWrap = False
                        RTF.DetectUrls = False
                        RTF.Font = Viewer.ResultsRichTextBox.Font
                        RTF.Text = ""
                        RTF.ZoomFactor = 1
                        FormatResults(ResultsEx_EG, RTF)


                        If ResultsFormatInfo.Fmt = RESULTSFMT.RTF Then
                            If Not OutputPerInputPageRadioButton.Checked Then
                                RTF.SelectionStart = 0
                                RTF.SelectionFont = New Font(SystemFonts.MessageBoxFont.Name, 12, FontStyle.Regular)
                                RTF.SelectedText = SeparatorText & Environment.NewLine
                            End If
                            OpStrm.WriteLine(RTF.Rtf)
                        Else
                            If Not OutputPerInputPageRadioButton.Checked Then
                                OpStrm.WriteLine(SeparatorText & Environment.NewLine & RTF.Text.Replace(vbLf, Environment.NewLine))
                            Else
                                OpStrm.WriteLine(RTF.Text.Replace(vbLf, Environment.NewLine))
                            End If
                        End If
                        OpStrm.Close()
                        OpStrm.Dispose()
                        OpStrm = Nothing
                        RTF.Dispose()

                        LogMessage(ImageName, CStr(ResultsEx_EG.Hdr.NumItems) & " characters found")
                    Else
                        LogMessage(ImageName, "No characters found")
                    End If ' Results.Hdr.NumItems > 0
                    OCR = True
                Else
                    LogMessage(ImageName, "Failed to retrieve results")
                End If ' GetResults(gJobNo, Results)
            End If ' Status = TOCR_OK And JobStatus = TOCRJOBSTATUS_DONE
        End If ' Status = TOCR_OK

        ' Check the service and log any error messages

        If Status = TOCR_OK Then
            If JobStatus = TOCRJOBSTATUS_ERROR Then
                TOCRGetJobStatusMsg(JobNo, Msg)
                LogMessage(ImageName, Msg.ToString)
            End If

            If OCRState <> OCRSTATES.Active Then
                LogMessage(ImageName, "Batch run aborted")
            End If

        Else

            ' Restart the service if it failed

            LogMessage(ImageName, "OCR Service failed")
            Status = TOCRShutdown(JobNo)
            System.Windows.Forms.Application.DoEvents() : System.Threading.Thread.Sleep(100) : System.Windows.Forms.Application.DoEvents()
            Status = TOCRInitialise(JobNo)
            If Status = TOCR_OK Then
                Status = TOCRSetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_NONE)
            Else
                OCRState = OCRSTATES.Abort
            End If
        End If

        Return OCR

        ' - - - - - - - - Error Handler - - - - - - - - - - -
BPOErrs:

        If Not OpStrm Is Nothing Then OpStrm.Close()
        LogMessage(ImageName, Err.Description)
        Err.Clear()

        Return False
    End Function

    ' Save settings to the registry
    ' Maintain compatibility with old VB6 settings
    Private Sub SaveSettings()
        Dim Txt As String       ' utility string

        SaveSetting(REGAPPNAME, "Batch", "Input Folder", InputFolder)

        If InputFileRadioButton.Checked Then
            Txt = "0"
        Else
            Txt = "1"
        End If
        SaveSetting(REGAPPNAME, "Batch", "Input Mode", Txt)

        SaveSetting(REGAPPNAME, "Batch", "Show UI", CStr(TWAINShowCheckBox.Checked))

        If AllOutputInOneRadioButton.Checked Then
            Txt = "0"
        ElseIf OutputPerInputFileRadioButton.Checked Then
            Txt = "1"
        Else
            Txt = "2"
        End If
        SaveSetting(REGAPPNAME, "Batch", "Output Mode", Txt)

        SaveSetting(REGAPPNAME, "Batch", "Output File", OutputFileLabel.Text)

        SaveSetting(REGAPPNAME, "Batch", "Output Folder", OutputFolderLabel.Text)
        SaveSetting(REGAPPNAME, "Batch", "Overwrite Output Files", CStr(OverwriteExistingCheckBox.Checked))

        If OutputFmt = RESULTSFMT.RTF Then
            SaveSetting(REGAPPNAME, "Batch", "Output Extension RTF", ExtensionTextBox.Text)
        Else
            SaveSetting(REGAPPNAME, "Batch", "Output Extension", ExtensionTextBox.Text)
        End If
        If AppendExtensionRadioButton.Checked Then
            Txt = "0"
        Else
            Txt = "1"
        End If
        SaveSetting(REGAPPNAME, "Batch", "Replace Extension", Txt)
        SaveSetting(REGAPPNAME, "Batch", "Save Images", CStr(SaveImagesCheckBox.Checked))

        SaveSetting(REGAPPNAME, "Batch", "All Pages", CStr(AllPagesRadioButton.Checked))
        SaveSetting(REGAPPNAME, "Batch", "Page Number", PagesTextBox.Text)
        SaveSetting(REGAPPNAME, "Batch", "Page Separator Mask", SeparatorTextBox.Text)

        SaveSetting(REGAPPNAME, "Batch", "Log Messages", CStr(CreateLogFileCheckBox.Checked))
        SaveSetting(REGAPPNAME, "Batch", "Log File", LogFileLabel.Text)
        If OverwriteLogRadioButton.Checked Then
            Txt = "1"
        Else
            Txt = "0"
        End If
        SaveSetting(REGAPPNAME, "Batch", "Overwrite Log", Txt)
    End Sub

    ' Construct a page Separator
    Private Function Separator(ByVal File As String, ByVal PageNo As Integer, ByVal NumPages As Integer, Optional ByVal IgnoreLineFeeds As Boolean = False) As String
        Dim Txt As String ' resultant separator

        Txt = SeparatorTextBox.Text
        Txt = Txt.Replace("%f", File)
        Txt = Txt.Replace("%x", vbFormFeed)
        If IgnoreLineFeeds Then
            Txt = Txt.Replace("%c", "")
        Else
            Txt = Txt.Replace("%c", Environment.NewLine)
        End If
        Txt = Txt.Replace("%p", CStr(PageNo))
        Txt = Txt.Replace("%n", CStr(NumPages))

        Return Txt
    End Function

    ' Enable/disable controls based on user selections
    Private Sub UpdateControls()
        Dim Txt As String


        If OutputFmt = RESULTSFMT.RTF Then
            OutputPerInputPageRadioButton.Checked = True
        End If
        AllOutputInOneRadioButton.Enabled = Not (OutputFmt = RESULTSFMT.RTF)
        OutputPerInputFileRadioButton.Enabled = Not (OutputFmt = RESULTSFMT.RTF)

        MultiPageGroupBox.Enabled = InputFileRadioButton.Checked
        OutputFileGroupBox.Enabled = AllOutputInOneRadioButton.Checked
        OutputFolderGroupBox.Enabled = Not AllOutputInOneRadioButton.Checked
        SeparatorGroupBox.Enabled = Not OutputPerInputPageRadioButton.Checked


        Txt = "Overwrite existing files"
        If TWAINAcquireRadioButton.Checked Then
            If OutputPerInputFileRadioButton.Checked Then OutputPerInputPageRadioButton.Checked = True
            If OutputFmt = RESULTSFMT.RTF Then
                OutputPerInputPageRadioButton.Text = "Output file per input image (the only option for Rich Text Format)"
            Else
                OutputPerInputPageRadioButton.Text = "Output file per input image"
            End If
            ToolTip1.SetToolTip(OutputPerInputPageRadioButton, "Create a file for each input image")
            OutputFolderGroupBox.Enabled = True ' so can enable checkboxes
            If SaveImagesCheckBox.Checked Then
                If OutputPerInputPageRadioButton.Checked Then
                    Txt = "Overwrite existing files and images"
                Else
                    Txt = "Overwrite existing images"
                End If
            End If
            OverwriteExistingCheckBox.Enabled = OutputPerInputPageRadioButton.Checked Or SaveImagesCheckBox.Checked
        Else
            AppendExtensionRadioButton.Enabled = OutputFolderGroupBox.Enabled
            If OutputFmt = RESULTSFMT.RTF Then
                OutputPerInputPageRadioButton.Text = "Output file per input page (the only option for Rich Text Format)"
            Else
                OutputPerInputPageRadioButton.Text = "Output file per input page"
            End If
            ToolTip1.SetToolTip(OutputPerInputPageRadioButton, "Create a file for each input page")
        End If
        SaveImagesCheckBox.Visible = TWAINAcquireRadioButton.Checked
        OverwriteExistingCheckBox.Text = Txt
        OutputPerInputFileRadioButton.Enabled = InputFileRadioButton.Checked And Not (OutputFmt = RESULTSFMT.RTF)
        OverwriteExtensionRadioButton.Visible = InputFileRadioButton.Checked
        AppendExtensionRadioButton.Visible = InputFileRadioButton.Checked

        InputFilesLabel.Enabled = InputFileRadioButton.Checked
        InputFilesButton.Enabled = InputFileRadioButton.Checked
        TWAINSelectButton.Enabled = TWAINAcquireRadioButton.Checked
        TWAINShowCheckBox.Enabled = TWAINAcquireRadioButton.Checked

        PagesLabel.Enabled = SomePagesRadioButton.Checked
        PagesLabel.Enabled = SomePagesRadioButton.Checked

        AppendLogRadioButton.Enabled = CreateLogFileCheckBox.Checked
        OverwriteLogRadioButton.Enabled = CreateLogFileCheckBox.Checked
        LogLabel.Enabled = CreateLogFileCheckBox.Checked
        LogFileLabel.Enabled = CreateLogFileCheckBox.Checked
        LogFileButton.Enabled = CreateLogFileCheckBox.Checked
    End Sub

    ' Update the example file name
    Private Sub UpdateExample()
        Dim Txt As String       ' example text

        If AllOutputInOneRadioButton.Checked Then
            ExampleLabel.Text = OutputFileLabel.Text
        Else
            If TWAINAcquireRadioButton.Checked Then
                Txt = OutputFolderLabel.Text & "Img12"
            Else
                Txt = OutputFolderLabel.Text & "InputFile"
                If OutputPerInputPageRadioButton.Checked Then
                    Txt = Txt & "_Page_1"
                End If
                If AppendExtensionRadioButton.Checked Then
                    Txt = Txt & ".ext"
                End If
            End If
            Txt = Txt & "." & ExtensionTextBox.Text
            ExampleLabel.Text = Txt
        End If
    End Sub
#End Region

#Region " Constructor "
    Public Sub New()
        InitializeComponent()

        LoadSettings()
    End Sub
#End Region

End Class