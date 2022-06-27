<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Batch
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Batch))
        Me.OptionsPanel = New System.Windows.Forms.Panel
        Me.LogFileGroupBox = New System.Windows.Forms.GroupBox
        Me.LogFileButton = New System.Windows.Forms.Button
        Me.LogFileLabel = New System.Windows.Forms.Label
        Me.LogLabel = New System.Windows.Forms.Label
        Me.OverwriteLogRadioButton = New System.Windows.Forms.RadioButton
        Me.AppendLogRadioButton = New System.Windows.Forms.RadioButton
        Me.CreateLogFileCheckBox = New System.Windows.Forms.CheckBox
        Me.SeparatorGroupBox = New System.Windows.Forms.GroupBox
        Me.SeparatorTextBox = New System.Windows.Forms.TextBox
        Me.SeparatorLabel = New System.Windows.Forms.Label
        Me.OutputFolderGroupBox = New System.Windows.Forms.GroupBox
        Me.ExampleLabel = New System.Windows.Forms.Label
        Me.ExaLabel = New System.Windows.Forms.Label
        Me.SaveImagesCheckBox = New System.Windows.Forms.CheckBox
        Me.OverwriteExistingCheckBox = New System.Windows.Forms.CheckBox
        Me.OverwriteExtensionRadioButton = New System.Windows.Forms.RadioButton
        Me.AppendExtensionRadioButton = New System.Windows.Forms.RadioButton
        Me.ExtensionTextBox = New System.Windows.Forms.TextBox
        Me.OutputFolderLabel = New System.Windows.Forms.Label
        Me.ExtLabel = New System.Windows.Forms.Label
        Me.OutputFolderButton = New System.Windows.Forms.Button
        Me.OutFolderLabel = New System.Windows.Forms.Label
        Me.OutputFileGroupBox = New System.Windows.Forms.GroupBox
        Me.OutputFileLabel = New System.Windows.Forms.Label
        Me.OutputFileButton = New System.Windows.Forms.Button
        Me.OutFileLabel = New System.Windows.Forms.Label
        Me.OutputGroupBox = New System.Windows.Forms.GroupBox
        Me.OutputPerInputPageRadioButton = New System.Windows.Forms.RadioButton
        Me.OutputPerInputFileRadioButton = New System.Windows.Forms.RadioButton
        Me.AllOutputInOneRadioButton = New System.Windows.Forms.RadioButton
        Me.MultiPageGroupBox = New System.Windows.Forms.GroupBox
        Me.PagesTextBox = New System.Windows.Forms.TextBox
        Me.PagesLabel = New System.Windows.Forms.Label
        Me.SomePagesRadioButton = New System.Windows.Forms.RadioButton
        Me.AllPagesRadioButton = New System.Windows.Forms.RadioButton
        Me.InFilesGroupBox = New System.Windows.Forms.GroupBox
        Me.TWAINShowCheckBox = New System.Windows.Forms.CheckBox
        Me.TWAINSelectButton = New System.Windows.Forms.Button
        Me.InputFilesButton = New System.Windows.Forms.Button
        Me.InputFilesLabel = New System.Windows.Forms.Label
        Me.TWAINAcquireRadioButton = New System.Windows.Forms.RadioButton
        Me.InputFileRadioButton = New System.Windows.Forms.RadioButton
        Me.CanclButton = New System.Windows.Forms.Button
        Me.OKButton = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ResultsPanel = New System.Windows.Forms.Panel
        Me.StartStopButton = New System.Windows.Forms.Button
        Me.CurrentFileLabel = New System.Windows.Forms.Label
        Me.CurFileLabel = New System.Windows.Forms.Label
        Me.FailedFilesLabel = New System.Windows.Forms.Label
        Me.FailFilesLabel = New System.Windows.Forms.Label
        Me.OutputFilesLabel = New System.Windows.Forms.Label
        Me.OutFilesLabel = New System.Windows.Forms.Label
        Me.InputFilesLabel2 = New System.Windows.Forms.Label
        Me.InpFilesLabel = New System.Windows.Forms.Label
        Me.LogFileLabel2 = New System.Windows.Forms.Label
        Me.LogLabel2 = New System.Windows.Forms.Label
        Me.OutputFolderLabel2 = New System.Windows.Forms.Label
        Me.OutFolderLabel2 = New System.Windows.Forms.Label
        Me.InputFolderLabel = New System.Windows.Forms.Label
        Me.InpFoldLabel = New System.Windows.Forms.Label
        Me.OptionsPanel.SuspendLayout()
        Me.LogFileGroupBox.SuspendLayout()
        Me.SeparatorGroupBox.SuspendLayout()
        Me.OutputFolderGroupBox.SuspendLayout()
        Me.OutputFileGroupBox.SuspendLayout()
        Me.OutputGroupBox.SuspendLayout()
        Me.MultiPageGroupBox.SuspendLayout()
        Me.InFilesGroupBox.SuspendLayout()
        Me.ResultsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'OptionsPanel
        '
        Me.OptionsPanel.Controls.Add(Me.LogFileGroupBox)
        Me.OptionsPanel.Controls.Add(Me.SeparatorGroupBox)
        Me.OptionsPanel.Controls.Add(Me.OutputFolderGroupBox)
        Me.OptionsPanel.Controls.Add(Me.OutputFileGroupBox)
        Me.OptionsPanel.Controls.Add(Me.OutputGroupBox)
        Me.OptionsPanel.Controls.Add(Me.MultiPageGroupBox)
        Me.OptionsPanel.Controls.Add(Me.InFilesGroupBox)
        Me.OptionsPanel.Controls.Add(Me.CanclButton)
        Me.OptionsPanel.Controls.Add(Me.OKButton)
        Me.OptionsPanel.Location = New System.Drawing.Point(0, 0)
        Me.OptionsPanel.Name = "OptionsPanel"
        Me.OptionsPanel.Size = New System.Drawing.Size(445, 577)
        Me.OptionsPanel.TabIndex = 24
        '
        'LogFileGroupBox
        '
        Me.LogFileGroupBox.Controls.Add(Me.LogFileButton)
        Me.LogFileGroupBox.Controls.Add(Me.LogFileLabel)
        Me.LogFileGroupBox.Controls.Add(Me.LogLabel)
        Me.LogFileGroupBox.Controls.Add(Me.OverwriteLogRadioButton)
        Me.LogFileGroupBox.Controls.Add(Me.AppendLogRadioButton)
        Me.LogFileGroupBox.Controls.Add(Me.CreateLogFileCheckBox)
        Me.LogFileGroupBox.Location = New System.Drawing.Point(4, 462)
        Me.LogFileGroupBox.Name = "LogFileGroupBox"
        Me.LogFileGroupBox.Size = New System.Drawing.Size(441, 81)
        Me.LogFileGroupBox.TabIndex = 35
        Me.LogFileGroupBox.TabStop = False
        Me.LogFileGroupBox.Text = "&Log File"
        '
        'LogFileButton
        '
        Me.LogFileButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogFileButton.Location = New System.Drawing.Point(404, 52)
        Me.LogFileButton.Name = "LogFileButton"
        Me.LogFileButton.Size = New System.Drawing.Size(21, 19)
        Me.LogFileButton.TabIndex = 41
        Me.LogFileButton.Text = "…"
        Me.ToolTip1.SetToolTip(Me.LogFileButton, "Select the log file name")
        Me.LogFileButton.UseVisualStyleBackColor = True
        '
        'LogFileLabel
        '
        Me.LogFileLabel.BackColor = System.Drawing.SystemColors.Window
        Me.LogFileLabel.Location = New System.Drawing.Point(56, 54)
        Me.LogFileLabel.Name = "LogFileLabel"
        Me.LogFileLabel.Size = New System.Drawing.Size(345, 17)
        Me.LogFileLabel.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.LogFileLabel, "Log file name")
        '
        'LogLabel
        '
        Me.LogLabel.AutoSize = True
        Me.LogLabel.Location = New System.Drawing.Point(30, 56)
        Me.LogLabel.Name = "LogLabel"
        Me.LogLabel.Size = New System.Drawing.Size(23, 13)
        Me.LogLabel.TabIndex = 39
        Me.LogLabel.Text = "File"
        '
        'OverwriteLogRadioButton
        '
        Me.OverwriteLogRadioButton.AutoSize = True
        Me.OverwriteLogRadioButton.Location = New System.Drawing.Point(257, 36)
        Me.OverwriteLogRadioButton.Name = "OverwriteLogRadioButton"
        Me.OverwriteLogRadioButton.Size = New System.Drawing.Size(87, 17)
        Me.OverwriteLogRadioButton.TabIndex = 38
        Me.OverwriteLogRadioButton.TabStop = True
        Me.OverwriteLogRadioButton.Text = "Overwrite log"
        Me.ToolTip1.SetToolTip(Me.OverwriteLogRadioButton, "Overwrite any existing log file")
        Me.OverwriteLogRadioButton.UseVisualStyleBackColor = True
        '
        'AppendLogRadioButton
        '
        Me.AppendLogRadioButton.AutoSize = True
        Me.AppendLogRadioButton.Location = New System.Drawing.Point(257, 17)
        Me.AppendLogRadioButton.Name = "AppendLogRadioButton"
        Me.AppendLogRadioButton.Size = New System.Drawing.Size(91, 17)
        Me.AppendLogRadioButton.TabIndex = 37
        Me.AppendLogRadioButton.TabStop = True
        Me.AppendLogRadioButton.Text = "Append to log"
        Me.ToolTip1.SetToolTip(Me.AppendLogRadioButton, "Append new messages to any existing log file")
        Me.AppendLogRadioButton.UseVisualStyleBackColor = True
        '
        'CreateLogFileCheckBox
        '
        Me.CreateLogFileCheckBox.AutoSize = True
        Me.CreateLogFileCheckBox.Location = New System.Drawing.Point(56, 17)
        Me.CreateLogFileCheckBox.Name = "CreateLogFileCheckBox"
        Me.CreateLogFileCheckBox.Size = New System.Drawing.Size(90, 17)
        Me.CreateLogFileCheckBox.TabIndex = 36
        Me.CreateLogFileCheckBox.Text = "Create log file"
        Me.ToolTip1.SetToolTip(Me.CreateLogFileCheckBox, "Create a log file")
        Me.CreateLogFileCheckBox.UseVisualStyleBackColor = True
        '
        'SeparatorGroupBox
        '
        Me.SeparatorGroupBox.Controls.Add(Me.SeparatorTextBox)
        Me.SeparatorGroupBox.Controls.Add(Me.SeparatorLabel)
        Me.SeparatorGroupBox.Location = New System.Drawing.Point(4, 408)
        Me.SeparatorGroupBox.Name = "SeparatorGroupBox"
        Me.SeparatorGroupBox.Size = New System.Drawing.Size(441, 49)
        Me.SeparatorGroupBox.TabIndex = 32
        Me.SeparatorGroupBox.TabStop = False
        Me.SeparatorGroupBox.Text = "Output &Separator"
        '
        'SeparatorTextBox
        '
        Me.SeparatorTextBox.Location = New System.Drawing.Point(56, 18)
        Me.SeparatorTextBox.Name = "SeparatorTextBox"
        Me.SeparatorTextBox.Size = New System.Drawing.Size(370, 20)
        Me.SeparatorTextBox.TabIndex = 34
        Me.ToolTip1.SetToolTip(Me.SeparatorTextBox, "Separate pages of output with this text, %c is a new line, %x is a form feed, %f " & _
                "is the input file, %p is the page number, %n is the number of pages")
        '
        'SeparatorLabel
        '
        Me.SeparatorLabel.AutoSize = True
        Me.SeparatorLabel.Location = New System.Drawing.Point(3, 21)
        Me.SeparatorLabel.Name = "SeparatorLabel"
        Me.SeparatorLabel.Size = New System.Drawing.Size(53, 13)
        Me.SeparatorLabel.TabIndex = 33
        Me.SeparatorLabel.Text = "Separator"
        '
        'OutputFolderGroupBox
        '
        Me.OutputFolderGroupBox.Controls.Add(Me.ExampleLabel)
        Me.OutputFolderGroupBox.Controls.Add(Me.ExaLabel)
        Me.OutputFolderGroupBox.Controls.Add(Me.SaveImagesCheckBox)
        Me.OutputFolderGroupBox.Controls.Add(Me.OverwriteExistingCheckBox)
        Me.OutputFolderGroupBox.Controls.Add(Me.OverwriteExtensionRadioButton)
        Me.OutputFolderGroupBox.Controls.Add(Me.AppendExtensionRadioButton)
        Me.OutputFolderGroupBox.Controls.Add(Me.ExtensionTextBox)
        Me.OutputFolderGroupBox.Controls.Add(Me.OutputFolderLabel)
        Me.OutputFolderGroupBox.Controls.Add(Me.ExtLabel)
        Me.OutputFolderGroupBox.Controls.Add(Me.OutputFolderButton)
        Me.OutputFolderGroupBox.Controls.Add(Me.OutFolderLabel)
        Me.OutputFolderGroupBox.Location = New System.Drawing.Point(4, 276)
        Me.OutputFolderGroupBox.Name = "OutputFolderGroupBox"
        Me.OutputFolderGroupBox.Size = New System.Drawing.Size(441, 125)
        Me.OutputFolderGroupBox.TabIndex = 20
        Me.OutputFolderGroupBox.TabStop = False
        Me.OutputFolderGroupBox.Text = "Output &Folder"
        '
        'ExampleLabel
        '
        Me.ExampleLabel.BackColor = System.Drawing.SystemColors.Window
        Me.ExampleLabel.Location = New System.Drawing.Point(56, 100)
        Me.ExampleLabel.Name = "ExampleLabel"
        Me.ExampleLabel.Size = New System.Drawing.Size(369, 15)
        Me.ExampleLabel.TabIndex = 31
        Me.ToolTip1.SetToolTip(Me.ExampleLabel, "Example of an output file name")
        '
        'ExaLabel
        '
        Me.ExaLabel.AutoSize = True
        Me.ExaLabel.Location = New System.Drawing.Point(6, 101)
        Me.ExaLabel.Name = "ExaLabel"
        Me.ExaLabel.Size = New System.Drawing.Size(47, 13)
        Me.ExaLabel.TabIndex = 30
        Me.ExaLabel.Text = "Example"
        '
        'SaveImagesCheckBox
        '
        Me.SaveImagesCheckBox.AutoSize = True
        Me.SaveImagesCheckBox.Location = New System.Drawing.Point(248, 79)
        Me.SaveImagesCheckBox.Name = "SaveImagesCheckBox"
        Me.SaveImagesCheckBox.Size = New System.Drawing.Size(88, 17)
        Me.SaveImagesCheckBox.TabIndex = 29
        Me.SaveImagesCheckBox.Text = "Save Images"
        Me.ToolTip1.SetToolTip(Me.SaveImagesCheckBox, "Save images to bitmap files")
        Me.SaveImagesCheckBox.UseVisualStyleBackColor = True
        '
        'OverwriteExistingCheckBox
        '
        Me.OverwriteExistingCheckBox.AutoSize = True
        Me.OverwriteExistingCheckBox.Location = New System.Drawing.Point(56, 79)
        Me.OverwriteExistingCheckBox.Name = "OverwriteExistingCheckBox"
        Me.OverwriteExistingCheckBox.Size = New System.Drawing.Size(130, 17)
        Me.OverwriteExistingCheckBox.TabIndex = 28
        Me.OverwriteExistingCheckBox.Text = "Overwrite existing files"
        Me.ToolTip1.SetToolTip(Me.OverwriteExistingCheckBox, "Overwrite any existing output files")
        Me.OverwriteExistingCheckBox.UseVisualStyleBackColor = True
        '
        'OverwriteExtensionRadioButton
        '
        Me.OverwriteExtensionRadioButton.AutoSize = True
        Me.OverwriteExtensionRadioButton.Location = New System.Drawing.Point(248, 61)
        Me.OverwriteExtensionRadioButton.Name = "OverwriteExtensionRadioButton"
        Me.OverwriteExtensionRadioButton.Size = New System.Drawing.Size(118, 17)
        Me.OverwriteExtensionRadioButton.TabIndex = 27
        Me.OverwriteExtensionRadioButton.TabStop = True
        Me.OverwriteExtensionRadioButton.Text = "Overwrite extension"
        Me.ToolTip1.SetToolTip(Me.OverwriteExtensionRadioButton, "Replace the input file extension with the extension text to create the output fil" & _
                "e name")
        Me.OverwriteExtensionRadioButton.UseVisualStyleBackColor = True
        '
        'AppendExtensionRadioButton
        '
        Me.AppendExtensionRadioButton.AutoSize = True
        Me.AppendExtensionRadioButton.Location = New System.Drawing.Point(248, 44)
        Me.AppendExtensionRadioButton.Name = "AppendExtensionRadioButton"
        Me.AppendExtensionRadioButton.Size = New System.Drawing.Size(110, 17)
        Me.AppendExtensionRadioButton.TabIndex = 26
        Me.AppendExtensionRadioButton.TabStop = True
        Me.AppendExtensionRadioButton.Text = "Append extension"
        Me.ToolTip1.SetToolTip(Me.AppendExtensionRadioButton, "Append the extension text to the input file name to create the output file name")
        Me.AppendExtensionRadioButton.UseVisualStyleBackColor = True
        '
        'ExtensionTextBox
        '
        Me.ExtensionTextBox.Location = New System.Drawing.Point(56, 44)
        Me.ExtensionTextBox.Name = "ExtensionTextBox"
        Me.ExtensionTextBox.Size = New System.Drawing.Size(41, 20)
        Me.ExtensionTextBox.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.ExtensionTextBox, "Text for the output file extension")
        '
        'OutputFolderLabel
        '
        Me.OutputFolderLabel.BackColor = System.Drawing.SystemColors.Window
        Me.OutputFolderLabel.Location = New System.Drawing.Point(56, 19)
        Me.OutputFolderLabel.Name = "OutputFolderLabel"
        Me.OutputFolderLabel.Size = New System.Drawing.Size(345, 15)
        Me.OutputFolderLabel.TabIndex = 22
        Me.ToolTip1.SetToolTip(Me.OutputFolderLabel, "Output folder")
        '
        'ExtLabel
        '
        Me.ExtLabel.AutoSize = True
        Me.ExtLabel.Location = New System.Drawing.Point(2, 47)
        Me.ExtLabel.Name = "ExtLabel"
        Me.ExtLabel.Size = New System.Drawing.Size(53, 13)
        Me.ExtLabel.TabIndex = 24
        Me.ExtLabel.Text = "Extension"
        '
        'OutputFolderButton
        '
        Me.OutputFolderButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OutputFolderButton.Location = New System.Drawing.Point(404, 17)
        Me.OutputFolderButton.Name = "OutputFolderButton"
        Me.OutputFolderButton.Size = New System.Drawing.Size(21, 19)
        Me.OutputFolderButton.TabIndex = 23
        Me.OutputFolderButton.Text = "…"
        Me.ToolTip1.SetToolTip(Me.OutputFolderButton, "Select the output folder")
        Me.OutputFolderButton.UseVisualStyleBackColor = True
        '
        'OutFolderLabel
        '
        Me.OutFolderLabel.AutoSize = True
        Me.OutFolderLabel.Location = New System.Drawing.Point(17, 19)
        Me.OutFolderLabel.Name = "OutFolderLabel"
        Me.OutFolderLabel.Size = New System.Drawing.Size(36, 13)
        Me.OutFolderLabel.TabIndex = 21
        Me.OutFolderLabel.Text = "Folder"
        '
        'OutputFileGroupBox
        '
        Me.OutputFileGroupBox.Controls.Add(Me.OutputFileLabel)
        Me.OutputFileGroupBox.Controls.Add(Me.OutputFileButton)
        Me.OutputFileGroupBox.Controls.Add(Me.OutFileLabel)
        Me.OutputFileGroupBox.Location = New System.Drawing.Point(4, 224)
        Me.OutputFileGroupBox.Name = "OutputFileGroupBox"
        Me.OutputFileGroupBox.Size = New System.Drawing.Size(441, 49)
        Me.OutputFileGroupBox.TabIndex = 16
        Me.OutputFileGroupBox.TabStop = False
        Me.OutputFileGroupBox.Text = "Output &File"
        '
        'OutputFileLabel
        '
        Me.OutputFileLabel.BackColor = System.Drawing.SystemColors.Window
        Me.OutputFileLabel.Location = New System.Drawing.Point(56, 22)
        Me.OutputFileLabel.Name = "OutputFileLabel"
        Me.OutputFileLabel.Size = New System.Drawing.Size(345, 15)
        Me.OutputFileLabel.TabIndex = 18
        Me.ToolTip1.SetToolTip(Me.OutputFileLabel, "Output file name")
        '
        'OutputFileButton
        '
        Me.OutputFileButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OutputFileButton.Location = New System.Drawing.Point(404, 20)
        Me.OutputFileButton.Name = "OutputFileButton"
        Me.OutputFileButton.Size = New System.Drawing.Size(21, 19)
        Me.OutputFileButton.TabIndex = 19
        Me.OutputFileButton.Text = "…"
        Me.ToolTip1.SetToolTip(Me.OutputFileButton, "Select the output file name")
        Me.OutputFileButton.UseVisualStyleBackColor = True
        '
        'OutFileLabel
        '
        Me.OutFileLabel.AutoSize = True
        Me.OutFileLabel.Location = New System.Drawing.Point(30, 22)
        Me.OutFileLabel.Name = "OutFileLabel"
        Me.OutFileLabel.Size = New System.Drawing.Size(23, 13)
        Me.OutFileLabel.TabIndex = 17
        Me.OutFileLabel.Text = "File"
        '
        'OutputGroupBox
        '
        Me.OutputGroupBox.Controls.Add(Me.OutputPerInputPageRadioButton)
        Me.OutputGroupBox.Controls.Add(Me.OutputPerInputFileRadioButton)
        Me.OutputGroupBox.Controls.Add(Me.AllOutputInOneRadioButton)
        Me.OutputGroupBox.Location = New System.Drawing.Point(4, 144)
        Me.OutputGroupBox.Name = "OutputGroupBox"
        Me.OutputGroupBox.Size = New System.Drawing.Size(441, 77)
        Me.OutputGroupBox.TabIndex = 12
        Me.OutputGroupBox.TabStop = False
        Me.OutputGroupBox.Text = "Output"
        '
        'OutputPerInputPageRadioButton
        '
        Me.OutputPerInputPageRadioButton.AutoSize = True
        Me.OutputPerInputPageRadioButton.Location = New System.Drawing.Point(56, 52)
        Me.OutputPerInputPageRadioButton.Name = "OutputPerInputPageRadioButton"
        Me.OutputPerInputPageRadioButton.Size = New System.Drawing.Size(144, 17)
        Me.OutputPerInputPageRadioButton.TabIndex = 15
        Me.OutputPerInputPageRadioButton.TabStop = True
        Me.OutputPerInputPageRadioButton.Text = "Output file per input page"
        Me.ToolTip1.SetToolTip(Me.OutputPerInputPageRadioButton, "Create a file for each input page")
        Me.OutputPerInputPageRadioButton.UseVisualStyleBackColor = True
        '
        'OutputPerInputFileRadioButton
        '
        Me.OutputPerInputFileRadioButton.AutoSize = True
        Me.OutputPerInputFileRadioButton.Location = New System.Drawing.Point(56, 31)
        Me.OutputPerInputFileRadioButton.Name = "OutputPerInputFileRadioButton"
        Me.OutputPerInputFileRadioButton.Size = New System.Drawing.Size(133, 17)
        Me.OutputPerInputFileRadioButton.TabIndex = 14
        Me.OutputPerInputFileRadioButton.TabStop = True
        Me.OutputPerInputFileRadioButton.Text = "Output file per input file"
        Me.ToolTip1.SetToolTip(Me.OutputPerInputFileRadioButton, "Combine all input pages for a file into an output file")
        Me.OutputPerInputFileRadioButton.UseVisualStyleBackColor = True
        '
        'AllOutputInOneRadioButton
        '
        Me.AllOutputInOneRadioButton.AutoSize = True
        Me.AllOutputInOneRadioButton.Location = New System.Drawing.Point(56, 10)
        Me.AllOutputInOneRadioButton.Name = "AllOutputInOneRadioButton"
        Me.AllOutputInOneRadioButton.Size = New System.Drawing.Size(117, 17)
        Me.AllOutputInOneRadioButton.TabIndex = 13
        Me.AllOutputInOneRadioButton.TabStop = True
        Me.AllOutputInOneRadioButton.Text = "All output in one file"
        Me.ToolTip1.SetToolTip(Me.AllOutputInOneRadioButton, "Combine all output into one file")
        Me.AllOutputInOneRadioButton.UseVisualStyleBackColor = True
        '
        'MultiPageGroupBox
        '
        Me.MultiPageGroupBox.Controls.Add(Me.PagesTextBox)
        Me.MultiPageGroupBox.Controls.Add(Me.PagesLabel)
        Me.MultiPageGroupBox.Controls.Add(Me.SomePagesRadioButton)
        Me.MultiPageGroupBox.Controls.Add(Me.AllPagesRadioButton)
        Me.MultiPageGroupBox.Location = New System.Drawing.Point(4, 76)
        Me.MultiPageGroupBox.Name = "MultiPageGroupBox"
        Me.MultiPageGroupBox.Size = New System.Drawing.Size(441, 65)
        Me.MultiPageGroupBox.TabIndex = 7
        Me.MultiPageGroupBox.TabStop = False
        Me.MultiPageGroupBox.Text = "&Multi Page Input"
        '
        'PagesTextBox
        '
        Me.PagesTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PagesTextBox.Location = New System.Drawing.Point(176, 37)
        Me.PagesTextBox.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.PagesTextBox.Name = "PagesTextBox"
        Me.PagesTextBox.Size = New System.Drawing.Size(245, 20)
        Me.PagesTextBox.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.PagesTextBox, "Comma separated list of input pages/page ranges (use L for last page)")
        '
        'PagesLabel
        '
        Me.PagesLabel.AutoSize = True
        Me.PagesLabel.Location = New System.Drawing.Point(140, 40)
        Me.PagesLabel.Name = "PagesLabel"
        Me.PagesLabel.Size = New System.Drawing.Size(32, 13)
        Me.PagesLabel.TabIndex = 10
        Me.PagesLabel.Text = "Page"
        '
        'SomePagesRadioButton
        '
        Me.SomePagesRadioButton.AutoSize = True
        Me.SomePagesRadioButton.Location = New System.Drawing.Point(56, 38)
        Me.SomePagesRadioButton.Name = "SomePagesRadioButton"
        Me.SomePagesRadioButton.Size = New System.Drawing.Size(85, 17)
        Me.SomePagesRadioButton.TabIndex = 9
        Me.SomePagesRadioButton.TabStop = True
        Me.SomePagesRadioButton.Text = "Some Pages"
        Me.ToolTip1.SetToolTip(Me.SomePagesRadioButton, "Process some input pages")
        Me.SomePagesRadioButton.UseVisualStyleBackColor = True
        '
        'AllPagesRadioButton
        '
        Me.AllPagesRadioButton.AutoSize = True
        Me.AllPagesRadioButton.Location = New System.Drawing.Point(56, 19)
        Me.AllPagesRadioButton.Name = "AllPagesRadioButton"
        Me.AllPagesRadioButton.Size = New System.Drawing.Size(69, 17)
        Me.AllPagesRadioButton.TabIndex = 8
        Me.AllPagesRadioButton.TabStop = True
        Me.AllPagesRadioButton.Text = "All Pages"
        Me.ToolTip1.SetToolTip(Me.AllPagesRadioButton, "Process all pages")
        Me.AllPagesRadioButton.UseVisualStyleBackColor = True
        '
        'InFilesGroupBox
        '
        Me.InFilesGroupBox.Controls.Add(Me.TWAINShowCheckBox)
        Me.InFilesGroupBox.Controls.Add(Me.TWAINSelectButton)
        Me.InFilesGroupBox.Controls.Add(Me.InputFilesButton)
        Me.InFilesGroupBox.Controls.Add(Me.InputFilesLabel)
        Me.InFilesGroupBox.Controls.Add(Me.TWAINAcquireRadioButton)
        Me.InFilesGroupBox.Controls.Add(Me.InputFileRadioButton)
        Me.InFilesGroupBox.Location = New System.Drawing.Point(4, 0)
        Me.InFilesGroupBox.Name = "InFilesGroupBox"
        Me.InFilesGroupBox.Size = New System.Drawing.Size(441, 73)
        Me.InFilesGroupBox.TabIndex = 0
        Me.InFilesGroupBox.TabStop = False
        Me.InFilesGroupBox.Text = "&Input Files"
        '
        'TWAINShowCheckBox
        '
        Me.TWAINShowCheckBox.AutoSize = True
        Me.TWAINShowCheckBox.Location = New System.Drawing.Point(152, 49)
        Me.TWAINShowCheckBox.Name = "TWAINShowCheckBox"
        Me.TWAINShowCheckBox.Size = New System.Drawing.Size(90, 17)
        Me.TWAINShowCheckBox.TabIndex = 6
        Me.TWAINShowCheckBox.Text = "Show Device"
        Me.ToolTip1.SetToolTip(Me.TWAINShowCheckBox, "Show the device's User Interface when acquiring")
        Me.TWAINShowCheckBox.UseVisualStyleBackColor = True
        '
        'TWAINSelectButton
        '
        Me.TWAINSelectButton.Location = New System.Drawing.Point(80, 46)
        Me.TWAINSelectButton.Name = "TWAINSelectButton"
        Me.TWAINSelectButton.Size = New System.Drawing.Size(57, 21)
        Me.TWAINSelectButton.TabIndex = 5
        Me.TWAINSelectButton.Text = "Select"
        Me.ToolTip1.SetToolTip(Me.TWAINSelectButton, "Select TWAIN input device")
        Me.TWAINSelectButton.UseVisualStyleBackColor = True
        '
        'InputFilesButton
        '
        Me.InputFilesButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputFilesButton.Location = New System.Drawing.Point(404, 20)
        Me.InputFilesButton.Name = "InputFilesButton"
        Me.InputFilesButton.Size = New System.Drawing.Size(21, 19)
        Me.InputFilesButton.TabIndex = 3
        Me.InputFilesButton.Text = "…"
        Me.ToolTip1.SetToolTip(Me.InputFilesButton, "Select input files")
        Me.InputFilesButton.UseVisualStyleBackColor = True
        '
        'InputFilesLabel
        '
        Me.InputFilesLabel.BackColor = System.Drawing.SystemColors.Window
        Me.InputFilesLabel.Location = New System.Drawing.Point(56, 22)
        Me.InputFilesLabel.Name = "InputFilesLabel"
        Me.InputFilesLabel.Size = New System.Drawing.Size(345, 15)
        Me.InputFilesLabel.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.InputFilesLabel, "Input files")
        '
        'TWAINAcquireRadioButton
        '
        Me.TWAINAcquireRadioButton.AutoSize = True
        Me.TWAINAcquireRadioButton.Location = New System.Drawing.Point(8, 48)
        Me.TWAINAcquireRadioButton.Name = "TWAINAcquireRadioButton"
        Me.TWAINAcquireRadioButton.Size = New System.Drawing.Size(61, 17)
        Me.TWAINAcquireRadioButton.TabIndex = 4
        Me.TWAINAcquireRadioButton.TabStop = True
        Me.TWAINAcquireRadioButton.Text = "Acquire"
        Me.ToolTip1.SetToolTip(Me.TWAINAcquireRadioButton, "Get input from TWAIN device")
        Me.TWAINAcquireRadioButton.UseVisualStyleBackColor = True
        '
        'InputFileRadioButton
        '
        Me.InputFileRadioButton.AutoSize = True
        Me.InputFileRadioButton.Location = New System.Drawing.Point(8, 21)
        Me.InputFileRadioButton.Name = "InputFileRadioButton"
        Me.InputFileRadioButton.Size = New System.Drawing.Size(46, 17)
        Me.InputFileRadioButton.TabIndex = 1
        Me.InputFileRadioButton.TabStop = True
        Me.InputFileRadioButton.Text = "Files"
        Me.ToolTip1.SetToolTip(Me.InputFileRadioButton, "Get input from files on disk")
        Me.InputFileRadioButton.UseVisualStyleBackColor = True
        '
        'CanclButton
        '
        Me.CanclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CanclButton.Location = New System.Drawing.Point(236, 548)
        Me.CanclButton.Name = "CanclButton"
        Me.CanclButton.Size = New System.Drawing.Size(89, 25)
        Me.CanclButton.TabIndex = 43
        Me.CanclButton.Text = "Cancel"
        Me.CanclButton.UseVisualStyleBackColor = True
        '
        'OKButton
        '
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.OKButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OKButton.Location = New System.Drawing.Point(116, 548)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OKButton.Size = New System.Drawing.Size(89, 25)
        Me.OKButton.TabIndex = 42
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'ResultsPanel
        '
        Me.ResultsPanel.BackColor = System.Drawing.SystemColors.Control
        Me.ResultsPanel.Controls.Add(Me.StartStopButton)
        Me.ResultsPanel.Controls.Add(Me.CurrentFileLabel)
        Me.ResultsPanel.Controls.Add(Me.CurFileLabel)
        Me.ResultsPanel.Controls.Add(Me.FailedFilesLabel)
        Me.ResultsPanel.Controls.Add(Me.FailFilesLabel)
        Me.ResultsPanel.Controls.Add(Me.OutputFilesLabel)
        Me.ResultsPanel.Controls.Add(Me.OutFilesLabel)
        Me.ResultsPanel.Controls.Add(Me.InputFilesLabel2)
        Me.ResultsPanel.Controls.Add(Me.InpFilesLabel)
        Me.ResultsPanel.Controls.Add(Me.LogFileLabel2)
        Me.ResultsPanel.Controls.Add(Me.LogLabel2)
        Me.ResultsPanel.Controls.Add(Me.OutputFolderLabel2)
        Me.ResultsPanel.Controls.Add(Me.OutFolderLabel2)
        Me.ResultsPanel.Controls.Add(Me.InputFolderLabel)
        Me.ResultsPanel.Controls.Add(Me.InpFoldLabel)
        Me.ResultsPanel.Location = New System.Drawing.Point(4, 2)
        Me.ResultsPanel.Name = "ResultsPanel"
        Me.ResultsPanel.Size = New System.Drawing.Size(445, 201)
        Me.ResultsPanel.TabIndex = 25
        '
        'StartStopButton
        '
        Me.StartStopButton.Location = New System.Drawing.Point(184, 172)
        Me.StartStopButton.Name = "StartStopButton"
        Me.StartStopButton.Size = New System.Drawing.Size(89, 25)
        Me.StartStopButton.TabIndex = 14
        Me.StartStopButton.Text = "Start"
        Me.StartStopButton.UseVisualStyleBackColor = True
        '
        'CurrentFileLabel
        '
        Me.CurrentFileLabel.Location = New System.Drawing.Point(88, 136)
        Me.CurrentFileLabel.Name = "CurrentFileLabel"
        Me.CurrentFileLabel.Size = New System.Drawing.Size(341, 13)
        Me.CurrentFileLabel.TabIndex = 13
        '
        'CurFileLabel
        '
        Me.CurFileLabel.Location = New System.Drawing.Point(6, 136)
        Me.CurFileLabel.Name = "CurFileLabel"
        Me.CurFileLabel.Size = New System.Drawing.Size(79, 13)
        Me.CurFileLabel.TabIndex = 12
        Me.CurFileLabel.Text = "Current File :"
        Me.CurFileLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'FailedFilesLabel
        '
        Me.FailedFilesLabel.Location = New System.Drawing.Point(88, 116)
        Me.FailedFilesLabel.Name = "FailedFilesLabel"
        Me.FailedFilesLabel.Size = New System.Drawing.Size(341, 13)
        Me.FailedFilesLabel.TabIndex = 11
        '
        'FailFilesLabel
        '
        Me.FailFilesLabel.Location = New System.Drawing.Point(6, 116)
        Me.FailFilesLabel.Name = "FailFilesLabel"
        Me.FailFilesLabel.Size = New System.Drawing.Size(79, 13)
        Me.FailFilesLabel.TabIndex = 10
        Me.FailFilesLabel.Text = "Failed Files :"
        Me.FailFilesLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'OutputFilesLabel
        '
        Me.OutputFilesLabel.Location = New System.Drawing.Point(88, 96)
        Me.OutputFilesLabel.Name = "OutputFilesLabel"
        Me.OutputFilesLabel.Size = New System.Drawing.Size(341, 13)
        Me.OutputFilesLabel.TabIndex = 9
        '
        'OutFilesLabel
        '
        Me.OutFilesLabel.Location = New System.Drawing.Point(6, 96)
        Me.OutFilesLabel.Name = "OutFilesLabel"
        Me.OutFilesLabel.Size = New System.Drawing.Size(79, 13)
        Me.OutFilesLabel.TabIndex = 8
        Me.OutFilesLabel.Text = "Output Files :"
        Me.OutFilesLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'InputFilesLabel2
        '
        Me.InputFilesLabel2.Location = New System.Drawing.Point(88, 76)
        Me.InputFilesLabel2.Name = "InputFilesLabel2"
        Me.InputFilesLabel2.Size = New System.Drawing.Size(341, 13)
        Me.InputFilesLabel2.TabIndex = 7
        '
        'InpFilesLabel
        '
        Me.InpFilesLabel.Location = New System.Drawing.Point(6, 76)
        Me.InpFilesLabel.Name = "InpFilesLabel"
        Me.InpFilesLabel.Size = New System.Drawing.Size(79, 13)
        Me.InpFilesLabel.TabIndex = 6
        Me.InpFilesLabel.Text = "Input Files :"
        Me.InpFilesLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LogFileLabel2
        '
        Me.LogFileLabel2.Location = New System.Drawing.Point(88, 48)
        Me.LogFileLabel2.Name = "LogFileLabel2"
        Me.LogFileLabel2.Size = New System.Drawing.Size(341, 13)
        Me.LogFileLabel2.TabIndex = 5
        '
        'LogLabel2
        '
        Me.LogLabel2.Location = New System.Drawing.Point(6, 48)
        Me.LogLabel2.Name = "LogLabel2"
        Me.LogLabel2.Size = New System.Drawing.Size(79, 13)
        Me.LogLabel2.TabIndex = 4
        Me.LogLabel2.Text = "Log File :"
        Me.LogLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'OutputFolderLabel2
        '
        Me.OutputFolderLabel2.Location = New System.Drawing.Point(88, 28)
        Me.OutputFolderLabel2.Name = "OutputFolderLabel2"
        Me.OutputFolderLabel2.Size = New System.Drawing.Size(341, 13)
        Me.OutputFolderLabel2.TabIndex = 3
        '
        'OutFolderLabel2
        '
        Me.OutFolderLabel2.Location = New System.Drawing.Point(6, 28)
        Me.OutFolderLabel2.Name = "OutFolderLabel2"
        Me.OutFolderLabel2.Size = New System.Drawing.Size(79, 13)
        Me.OutFolderLabel2.TabIndex = 2
        Me.OutFolderLabel2.Text = "Output Folder :"
        Me.OutFolderLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'InputFolderLabel
        '
        Me.InputFolderLabel.Location = New System.Drawing.Point(88, 8)
        Me.InputFolderLabel.Name = "InputFolderLabel"
        Me.InputFolderLabel.Size = New System.Drawing.Size(341, 13)
        Me.InputFolderLabel.TabIndex = 1
        '
        'InpFoldLabel
        '
        Me.InpFoldLabel.Location = New System.Drawing.Point(6, 8)
        Me.InpFoldLabel.Name = "InpFoldLabel"
        Me.InpFoldLabel.Size = New System.Drawing.Size(79, 13)
        Me.InpFoldLabel.TabIndex = 0
        Me.InpFoldLabel.Text = "Input Folder :"
        Me.InpFoldLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Batch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CanclButton
        Me.ClientSize = New System.Drawing.Size(448, 580)
        Me.Controls.Add(Me.OptionsPanel)
        Me.Controls.Add(Me.ResultsPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Batch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Batch Processing"
        Me.OptionsPanel.ResumeLayout(False)
        Me.LogFileGroupBox.ResumeLayout(False)
        Me.LogFileGroupBox.PerformLayout()
        Me.SeparatorGroupBox.ResumeLayout(False)
        Me.SeparatorGroupBox.PerformLayout()
        Me.OutputFolderGroupBox.ResumeLayout(False)
        Me.OutputFolderGroupBox.PerformLayout()
        Me.OutputFileGroupBox.ResumeLayout(False)
        Me.OutputFileGroupBox.PerformLayout()
        Me.OutputGroupBox.ResumeLayout(False)
        Me.OutputGroupBox.PerformLayout()
        Me.MultiPageGroupBox.ResumeLayout(False)
        Me.MultiPageGroupBox.PerformLayout()
        Me.InFilesGroupBox.ResumeLayout(False)
        Me.InFilesGroupBox.PerformLayout()
        Me.ResultsPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OptionsPanel As System.Windows.Forms.Panel
    Friend WithEvents InFilesGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents CanclButton As System.Windows.Forms.Button
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents MultiPageGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents SeparatorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents OutputFolderGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents OutputFileGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents OutputGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents LogFileGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents InputFileRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TWAINAcquireRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents InputFilesButton As System.Windows.Forms.Button
    Friend WithEvents InputFilesLabel As System.Windows.Forms.Label
    Friend WithEvents TWAINShowCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents TWAINSelectButton As System.Windows.Forms.Button
    Friend WithEvents AllPagesRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents PagesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PagesLabel As System.Windows.Forms.Label
    Friend WithEvents SomePagesRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents OutputPerInputPageRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents OutputPerInputFileRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents AllOutputInOneRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents OutFileLabel As System.Windows.Forms.Label
    Friend WithEvents OutputFileButton As System.Windows.Forms.Button
    Friend WithEvents OutFolderLabel As System.Windows.Forms.Label
    Friend WithEvents OutputFolderButton As System.Windows.Forms.Button
    Friend WithEvents OutputFileLabel As System.Windows.Forms.Label
    Friend WithEvents ExtLabel As System.Windows.Forms.Label
    Friend WithEvents OutputFolderLabel As System.Windows.Forms.Label
    Friend WithEvents ExtensionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents OverwriteExtensionRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents AppendExtensionRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents ExampleLabel As System.Windows.Forms.Label
    Friend WithEvents ExaLabel As System.Windows.Forms.Label
    Friend WithEvents SaveImagesCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents OverwriteExistingCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents SeparatorLabel As System.Windows.Forms.Label
    Friend WithEvents SeparatorTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AppendLogRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents CreateLogFileCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents LogFileLabel As System.Windows.Forms.Label
    Friend WithEvents LogLabel As System.Windows.Forms.Label
    Friend WithEvents OverwriteLogRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents LogFileButton As System.Windows.Forms.Button
    Friend WithEvents ResultsPanel As System.Windows.Forms.Panel
    Friend WithEvents OutputFolderLabel2 As System.Windows.Forms.Label
    Friend WithEvents OutFolderLabel2 As System.Windows.Forms.Label
    Friend WithEvents InputFolderLabel As System.Windows.Forms.Label
    Friend WithEvents InpFoldLabel As System.Windows.Forms.Label
    Friend WithEvents LogFileLabel2 As System.Windows.Forms.Label
    Friend WithEvents LogLabel2 As System.Windows.Forms.Label
    Friend WithEvents InpFilesLabel As System.Windows.Forms.Label
    Friend WithEvents OutputFilesLabel As System.Windows.Forms.Label
    Friend WithEvents OutFilesLabel As System.Windows.Forms.Label
    Friend WithEvents InputFilesLabel2 As System.Windows.Forms.Label
    Friend WithEvents CurrentFileLabel As System.Windows.Forms.Label
    Friend WithEvents CurFileLabel As System.Windows.Forms.Label
    Friend WithEvents FailedFilesLabel As System.Windows.Forms.Label
    Friend WithEvents FailFilesLabel As System.Windows.Forms.Label
    Friend WithEvents StartStopButton As System.Windows.Forms.Button
End Class
