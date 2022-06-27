<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Viewer
    Inherits System.Windows.Forms.Form
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Viewer))
        Me.MenuBar = New System.Windows.Forms.MenuStrip()
        Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator1FileMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.SelectTWAINDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowDeviceUIMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AcquireImagesMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator2FileMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.SaveResultsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator3FileMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.OCRImageMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OCRSelectionMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StopOCRMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator4FileMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.BatchMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator5FileMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.CutMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator1ImageMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.InvertMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator2ImageMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.RotateMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.Rotate90MenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Rotate180MenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Rotate270MenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlipMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlipHorizontallyMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlipVerticallyMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator3ImageMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.SaveImageMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteFromMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectAllMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator4ImageMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.AdjustDPIMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.FocusbarsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolBarMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusBarMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsFontMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsFormattingMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColourConversionMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProcessingOptionsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LockZoomMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpTopicsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator1HelpMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutTOCRViewerMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ImageFocusPictureBox = New System.Windows.Forms.PictureBox()
        Me.ResultsRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.TextContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsFocusPictureBox = New System.Windows.Forms.PictureBox()
        Me.ImageContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CutPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PastePopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator1PopMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.InvertPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator2PopMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.RotatePopMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.Rotate90PopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Rotate180PopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Rotate270PopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlipPopMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlipHorizontallyPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlipVerticallyPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Separator3PopMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.SaveImagePopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteFromPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectAllPopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusBar = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FileNameLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SelectRectLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MouseCoordsLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SeparatorButton1 = New System.Windows.Forms.ToolStripSeparator()
        Me.SeparatorButton2 = New System.Windows.Forms.ToolStripSeparator()
        Me.SeparatorButton3 = New System.Windows.Forms.ToolStripSeparator()
        Me.SeparatorButton4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ImageZoomLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ImageZoomComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me.ResultsZoomLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ResultsZoomComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me.SeparatorButton5 = New System.Windows.Forms.ToolStripSeparator()
        Me.PageLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ToolBar = New System.Windows.Forms.ToolStrip()
        Me.OpenButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveResultsButton = New System.Windows.Forms.ToolStripButton()
        Me.OCRButton = New System.Windows.Forms.ToolStripButton()
        Me.PIButton = New System.Windows.Forms.ToolStripButton()
        Me.StopOCRButton = New System.Windows.Forms.ToolStripButton()
        Me.CutButton = New System.Windows.Forms.ToolStripButton()
        Me.CopyButton = New System.Windows.Forms.ToolStripButton()
        Me.PasteButton = New System.Windows.Forms.ToolStripButton()
        Me.ClearButton = New System.Windows.Forms.ToolStripButton()
        Me.InvertButton = New System.Windows.Forms.ToolStripButton()
        Me.Rotate90Button = New System.Windows.Forms.ToolStripButton()
        Me.Rotate180Button = New System.Windows.Forms.ToolStripButton()
        Me.Rotate270Button = New System.Windows.Forms.ToolStripButton()
        Me.FlipHorizontallyButton = New System.Windows.Forms.ToolStripButton()
        Me.FlipVerticallyButton = New System.Windows.Forms.ToolStripButton()
        Me.MenuBar.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.ImageFocusPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TextContextMenu.SuspendLayout()
        CType(Me.ResultsFocusPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ImageContextMenu.SuspendLayout()
        Me.StatusBar.SuspendLayout()
        Me.ToolBar.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuBar
        '
        Me.MenuBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.ImageMenu, Me.ViewMenu, Me.OptionsMenu, Me.HelpMenu})
        Me.MenuBar.Location = New System.Drawing.Point(0, 0)
        Me.MenuBar.Name = "MenuBar"
        Me.MenuBar.Size = New System.Drawing.Size(968, 24)
        Me.MenuBar.TabIndex = 0
        Me.MenuBar.Text = "MenuStrip1"
        '
        'FileMenu
        '
        Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenMenuItem, Me.Separator1FileMenuItem, Me.SelectTWAINDeviceMenuItem, Me.ShowDeviceUIMenuItem, Me.AcquireImagesMenuItem, Me.Separator2FileMenuItem, Me.SaveResultsMenuItem, Me.Separator3FileMenuItem, Me.OCRImageMenuItem, Me.OCRSelectionMenuItem, Me.StopOCRMenuItem, Me.Separator4FileMenuItem, Me.BatchMenuItem, Me.Separator5FileMenuItem, Me.ExitMenuItem})
        Me.FileMenu.Name = "FileMenu"
        Me.FileMenu.Size = New System.Drawing.Size(37, 20)
        Me.FileMenu.Text = "&File"
        '
        'OpenMenuItem
        '
        Me.OpenMenuItem.Name = "OpenMenuItem"
        Me.OpenMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.OpenMenuItem.Text = "&Open ..."
        '
        'Separator1FileMenuItem
        '
        Me.Separator1FileMenuItem.Name = "Separator1FileMenuItem"
        Me.Separator1FileMenuItem.Size = New System.Drawing.Size(191, 6)
        '
        'SelectTWAINDeviceMenuItem
        '
        Me.SelectTWAINDeviceMenuItem.Name = "SelectTWAINDeviceMenuItem"
        Me.SelectTWAINDeviceMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.SelectTWAINDeviceMenuItem.Text = "Select T&WAIN device ..."
        '
        'ShowDeviceUIMenuItem
        '
        Me.ShowDeviceUIMenuItem.Name = "ShowDeviceUIMenuItem"
        Me.ShowDeviceUIMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.ShowDeviceUIMenuItem.Text = "Show Device &UI"
        '
        'AcquireImagesMenuItem
        '
        Me.AcquireImagesMenuItem.Name = "AcquireImagesMenuItem"
        Me.AcquireImagesMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.AcquireImagesMenuItem.Text = "&Acquire images ..."
        '
        'Separator2FileMenuItem
        '
        Me.Separator2FileMenuItem.Name = "Separator2FileMenuItem"
        Me.Separator2FileMenuItem.Size = New System.Drawing.Size(191, 6)
        '
        'SaveResultsMenuItem
        '
        Me.SaveResultsMenuItem.Name = "SaveResultsMenuItem"
        Me.SaveResultsMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveResultsMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.SaveResultsMenuItem.Text = "&Save results ..."
        '
        'Separator3FileMenuItem
        '
        Me.Separator3FileMenuItem.Name = "Separator3FileMenuItem"
        Me.Separator3FileMenuItem.Size = New System.Drawing.Size(191, 6)
        '
        'OCRImageMenuItem
        '
        Me.OCRImageMenuItem.Name = "OCRImageMenuItem"
        Me.OCRImageMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.OCRImageMenuItem.Text = "O&CR Image"
        '
        'OCRSelectionMenuItem
        '
        Me.OCRSelectionMenuItem.Name = "OCRSelectionMenuItem"
        Me.OCRSelectionMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.OCRSelectionMenuItem.Text = "OC&R Selection"
        '
        'StopOCRMenuItem
        '
        Me.StopOCRMenuItem.Name = "StopOCRMenuItem"
        Me.StopOCRMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.StopOCRMenuItem.Text = "S&top OCR"
        '
        'Separator4FileMenuItem
        '
        Me.Separator4FileMenuItem.Name = "Separator4FileMenuItem"
        Me.Separator4FileMenuItem.Size = New System.Drawing.Size(191, 6)
        '
        'BatchMenuItem
        '
        Me.BatchMenuItem.Name = "BatchMenuItem"
        Me.BatchMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.BatchMenuItem.Text = "&Batch ..."
        '
        'Separator5FileMenuItem
        '
        Me.Separator5FileMenuItem.Name = "Separator5FileMenuItem"
        Me.Separator5FileMenuItem.Size = New System.Drawing.Size(191, 6)
        '
        'ExitMenuItem
        '
        Me.ExitMenuItem.Name = "ExitMenuItem"
        Me.ExitMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.ExitMenuItem.Text = "E&xit"
        '
        'ImageMenu
        '
        Me.ImageMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CutMenuItem, Me.CopyMenuItem, Me.PasteMenuItem, Me.Separator1ImageMenuItem, Me.InvertMenuItem, Me.ClearMenuItem, Me.Separator2ImageMenuItem, Me.RotateMenu, Me.FlipMenu, Me.Separator3ImageMenuItem, Me.SaveImageMenuItem, Me.PasteFromMenuItem, Me.SelectAllMenuItem, Me.Separator4ImageMenuItem, Me.AdjustDPIMenuItem})
        Me.ImageMenu.Name = "ImageMenu"
        Me.ImageMenu.Size = New System.Drawing.Size(52, 20)
        Me.ImageMenu.Text = "&Image"
        '
        'CutMenuItem
        '
        Me.CutMenuItem.Name = "CutMenuItem"
        Me.CutMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.CutMenuItem.Text = "Cu&t"
        '
        'CopyMenuItem
        '
        Me.CopyMenuItem.Name = "CopyMenuItem"
        Me.CopyMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.CopyMenuItem.Text = "&Copy"
        '
        'PasteMenuItem
        '
        Me.PasteMenuItem.Name = "PasteMenuItem"
        Me.PasteMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.PasteMenuItem.Text = "&Paste"
        '
        'Separator1ImageMenuItem
        '
        Me.Separator1ImageMenuItem.Name = "Separator1ImageMenuItem"
        Me.Separator1ImageMenuItem.Size = New System.Drawing.Size(140, 6)
        '
        'InvertMenuItem
        '
        Me.InvertMenuItem.Name = "InvertMenuItem"
        Me.InvertMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.InvertMenuItem.Text = "&Invert"
        '
        'ClearMenuItem
        '
        Me.ClearMenuItem.Name = "ClearMenuItem"
        Me.ClearMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.ClearMenuItem.Text = "C&lear"
        '
        'Separator2ImageMenuItem
        '
        Me.Separator2ImageMenuItem.Name = "Separator2ImageMenuItem"
        Me.Separator2ImageMenuItem.Size = New System.Drawing.Size(140, 6)
        '
        'RotateMenu
        '
        Me.RotateMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Rotate90MenuItem, Me.Rotate180MenuItem, Me.Rotate270MenuItem})
        Me.RotateMenu.Name = "RotateMenu"
        Me.RotateMenu.Size = New System.Drawing.Size(143, 22)
        Me.RotateMenu.Text = "&Rotate"
        '
        'Rotate90MenuItem
        '
        Me.Rotate90MenuItem.Name = "Rotate90MenuItem"
        Me.Rotate90MenuItem.Size = New System.Drawing.Size(137, 22)
        Me.Rotate90MenuItem.Text = "90 Degrees"
        '
        'Rotate180MenuItem
        '
        Me.Rotate180MenuItem.Name = "Rotate180MenuItem"
        Me.Rotate180MenuItem.Size = New System.Drawing.Size(137, 22)
        Me.Rotate180MenuItem.Text = "180 Degrees"
        '
        'Rotate270MenuItem
        '
        Me.Rotate270MenuItem.Name = "Rotate270MenuItem"
        Me.Rotate270MenuItem.Size = New System.Drawing.Size(137, 22)
        Me.Rotate270MenuItem.Text = "270 Degrees"
        '
        'FlipMenu
        '
        Me.FlipMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FlipHorizontallyMenuItem, Me.FlipVerticallyMenuItem})
        Me.FlipMenu.Name = "FlipMenu"
        Me.FlipMenu.Size = New System.Drawing.Size(143, 22)
        Me.FlipMenu.Text = "&Flip"
        '
        'FlipHorizontallyMenuItem
        '
        Me.FlipHorizontallyMenuItem.Name = "FlipHorizontallyMenuItem"
        Me.FlipHorizontallyMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.FlipHorizontallyMenuItem.Text = "Horizontally"
        '
        'FlipVerticallyMenuItem
        '
        Me.FlipVerticallyMenuItem.Name = "FlipVerticallyMenuItem"
        Me.FlipVerticallyMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.FlipVerticallyMenuItem.Text = "Vertically"
        '
        'Separator3ImageMenuItem
        '
        Me.Separator3ImageMenuItem.Name = "Separator3ImageMenuItem"
        Me.Separator3ImageMenuItem.Size = New System.Drawing.Size(140, 6)
        '
        'SaveImageMenuItem
        '
        Me.SaveImageMenuItem.Name = "SaveImageMenuItem"
        Me.SaveImageMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SaveImageMenuItem.Text = "&Save ..."
        '
        'PasteFromMenuItem
        '
        Me.PasteFromMenuItem.Name = "PasteFromMenuItem"
        Me.PasteFromMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.PasteFromMenuItem.Text = "Paste from ..."
        '
        'SelectAllMenuItem
        '
        Me.SelectAllMenuItem.Name = "SelectAllMenuItem"
        Me.SelectAllMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SelectAllMenuItem.Text = "Select &All"
        '
        'Separator4ImageMenuItem
        '
        Me.Separator4ImageMenuItem.Name = "Separator4ImageMenuItem"
        Me.Separator4ImageMenuItem.Size = New System.Drawing.Size(140, 6)
        '
        'AdjustDPIMenuItem
        '
        Me.AdjustDPIMenuItem.Name = "AdjustDPIMenuItem"
        Me.AdjustDPIMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.AdjustDPIMenuItem.Text = "Adjust DPI ..."
        '
        'ViewMenu
        '
        Me.ViewMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FocusbarsMenuItem, Me.ToolBarMenuItem, Me.StatusBarMenuItem})
        Me.ViewMenu.Name = "ViewMenu"
        Me.ViewMenu.Size = New System.Drawing.Size(44, 20)
        Me.ViewMenu.Text = "&View"
        '
        'FocusbarsMenuItem
        '
        Me.FocusbarsMenuItem.Name = "FocusbarsMenuItem"
        Me.FocusbarsMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.FocusbarsMenuItem.Text = "&Focus bars"
        '
        'ToolBarMenuItem
        '
        Me.ToolBarMenuItem.Checked = True
        Me.ToolBarMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolBarMenuItem.Name = "ToolBarMenuItem"
        Me.ToolBarMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ToolBarMenuItem.Text = "&Toolbar"
        '
        'StatusBarMenuItem
        '
        Me.StatusBarMenuItem.Checked = True
        Me.StatusBarMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.StatusBarMenuItem.Name = "StatusBarMenuItem"
        Me.StatusBarMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.StatusBarMenuItem.Text = "&Status bar"
        '
        'OptionsMenu
        '
        Me.OptionsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResultsFontMenuItem, Me.ResultsFormattingMenuItem, Me.ColourConversionMenuItem, Me.ProcessingOptionsMenuItem, Me.LockZoomMenuItem})
        Me.OptionsMenu.Name = "OptionsMenu"
        Me.OptionsMenu.Size = New System.Drawing.Size(61, 20)
        Me.OptionsMenu.Text = "&Options"
        '
        'ResultsFontMenuItem
        '
        Me.ResultsFontMenuItem.Name = "ResultsFontMenuItem"
        Me.ResultsFontMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ResultsFontMenuItem.Text = "Results &Font ..."
        '
        'ResultsFormattingMenuItem
        '
        Me.ResultsFormattingMenuItem.Name = "ResultsFormattingMenuItem"
        Me.ResultsFormattingMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ResultsFormattingMenuItem.Text = "&Results formatting ..."
        '
        'ColourConversionMenuItem
        '
        Me.ColourConversionMenuItem.Name = "ColourConversionMenuItem"
        Me.ColourConversionMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ColourConversionMenuItem.Text = "Colour conversion ..."
        '
        'ProcessingOptionsMenuItem
        '
        Me.ProcessingOptionsMenuItem.Name = "ProcessingOptionsMenuItem"
        Me.ProcessingOptionsMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ProcessingOptionsMenuItem.Text = "&Processing options ..."
        '
        'LockZoomMenuItem
        '
        Me.LockZoomMenuItem.Name = "LockZoomMenuItem"
        Me.LockZoomMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.LockZoomMenuItem.Text = "Lock &Zoom"
        '
        'HelpMenu
        '
        Me.HelpMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpTopicsMenuItem, Me.Separator1HelpMenuItem, Me.AboutTOCRViewerMenuItem})
        Me.HelpMenu.Name = "HelpMenu"
        Me.HelpMenu.Size = New System.Drawing.Size(44, 20)
        Me.HelpMenu.Text = "&Help"
        '
        'HelpTopicsMenuItem
        '
        Me.HelpTopicsMenuItem.Name = "HelpTopicsMenuItem"
        Me.HelpTopicsMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.HelpTopicsMenuItem.Text = "Help topics"
        '
        'Separator1HelpMenuItem
        '
        Me.Separator1HelpMenuItem.Name = "Separator1HelpMenuItem"
        Me.Separator1HelpMenuItem.Size = New System.Drawing.Size(204, 6)
        '
        'AboutTOCRViewerMenuItem
        '
        Me.AboutTOCRViewerMenuItem.Name = "AboutTOCRViewerMenuItem"
        Me.AboutTOCRViewerMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.AboutTOCRViewerMenuItem.Text = "About TOCR Viewer ..."
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 49)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AllowDrop = True
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.ImageFocusPictureBox)
        Me.SplitContainer1.Panel1MinSize = 0
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ResultsRichTextBox)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ResultsFocusPictureBox)
        Me.SplitContainer1.Panel2MinSize = 0
        Me.SplitContainer1.Size = New System.Drawing.Size(968, 331)
        Me.SplitContainer1.SplitterDistance = 652
        Me.SplitContainer1.TabIndex = 3
        '
        'ImageFocusPictureBox
        '
        Me.ImageFocusPictureBox.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ImageFocusPictureBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.ImageFocusPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.ImageFocusPictureBox.Name = "ImageFocusPictureBox"
        Me.ImageFocusPictureBox.Size = New System.Drawing.Size(650, 4)
        Me.ImageFocusPictureBox.TabIndex = 0
        Me.ImageFocusPictureBox.TabStop = False
        '
        'ResultsRichTextBox
        '
        Me.ResultsRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ResultsRichTextBox.ContextMenuStrip = Me.TextContextMenu
        Me.ResultsRichTextBox.DetectUrls = False
        Me.ResultsRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResultsRichTextBox.Location = New System.Drawing.Point(0, 4)
        Me.ResultsRichTextBox.Name = "ResultsRichTextBox"
        Me.ResultsRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.ResultsRichTextBox.Size = New System.Drawing.Size(310, 325)
        Me.ResultsRichTextBox.TabIndex = 3
        Me.ResultsRichTextBox.Text = ""
        Me.ResultsRichTextBox.WordWrap = False
        '
        'TextContextMenu
        '
        Me.TextContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.ToolStripSeparator1, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.DeleteToolStripMenuItem, Me.ToolStripSeparator2, Me.SelectAllToolStripMenuItem})
        Me.TextContextMenu.Name = "TextPopupMenu"
        Me.TextContextMenu.Size = New System.Drawing.Size(123, 170)
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.UndoToolStripMenuItem.Text = "Undo"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.RedoToolStripMenuItem.Text = "Redo"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(119, 6)
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.CutToolStripMenuItem.Text = "Cut"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(119, 6)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select All"
        '
        'ResultsFocusPictureBox
        '
        Me.ResultsFocusPictureBox.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ResultsFocusPictureBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.ResultsFocusPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.ResultsFocusPictureBox.Name = "ResultsFocusPictureBox"
        Me.ResultsFocusPictureBox.Size = New System.Drawing.Size(310, 4)
        Me.ResultsFocusPictureBox.TabIndex = 2
        Me.ResultsFocusPictureBox.TabStop = False
        '
        'ImageContextMenu
        '
        Me.ImageContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CutPopMenuItem, Me.CopyPopMenuItem, Me.PastePopMenuItem, Me.Separator1PopMenuItem, Me.InvertPopMenuItem, Me.ClearPopMenuItem, Me.Separator2PopMenuItem, Me.RotatePopMenu, Me.FlipPopMenu, Me.Separator3PopMenuItem, Me.SaveImagePopMenuItem, Me.PasteFromPopMenuItem, Me.SelectAllPopMenuItem})
        Me.ImageContextMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table
        Me.ImageContextMenu.Name = "PopupMenu"
        Me.ImageContextMenu.Size = New System.Drawing.Size(144, 242)
        '
        'CutPopMenuItem
        '
        Me.CutPopMenuItem.Name = "CutPopMenuItem"
        Me.CutPopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.CutPopMenuItem.Text = "Cut"
        '
        'CopyPopMenuItem
        '
        Me.CopyPopMenuItem.Name = "CopyPopMenuItem"
        Me.CopyPopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.CopyPopMenuItem.Text = "Copy"
        '
        'PastePopMenuItem
        '
        Me.PastePopMenuItem.Name = "PastePopMenuItem"
        Me.PastePopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.PastePopMenuItem.Text = "Paste"
        '
        'Separator1PopMenuItem
        '
        Me.Separator1PopMenuItem.Name = "Separator1PopMenuItem"
        Me.Separator1PopMenuItem.Size = New System.Drawing.Size(140, 6)
        '
        'InvertPopMenuItem
        '
        Me.InvertPopMenuItem.Name = "InvertPopMenuItem"
        Me.InvertPopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.InvertPopMenuItem.Text = "Invert"
        '
        'ClearPopMenuItem
        '
        Me.ClearPopMenuItem.Name = "ClearPopMenuItem"
        Me.ClearPopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.ClearPopMenuItem.Text = "Clear"
        '
        'Separator2PopMenuItem
        '
        Me.Separator2PopMenuItem.Name = "Separator2PopMenuItem"
        Me.Separator2PopMenuItem.Size = New System.Drawing.Size(140, 6)
        '
        'RotatePopMenu
        '
        Me.RotatePopMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Rotate90PopMenuItem, Me.Rotate180PopMenuItem, Me.Rotate270PopMenuItem})
        Me.RotatePopMenu.Name = "RotatePopMenu"
        Me.RotatePopMenu.Size = New System.Drawing.Size(143, 22)
        Me.RotatePopMenu.Text = "Rotate"
        '
        'Rotate90PopMenuItem
        '
        Me.Rotate90PopMenuItem.Name = "Rotate90PopMenuItem"
        Me.Rotate90PopMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.Rotate90PopMenuItem.Text = "90 degrees"
        '
        'Rotate180PopMenuItem
        '
        Me.Rotate180PopMenuItem.Name = "Rotate180PopMenuItem"
        Me.Rotate180PopMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.Rotate180PopMenuItem.Text = "180 degrees"
        '
        'Rotate270PopMenuItem
        '
        Me.Rotate270PopMenuItem.Name = "Rotate270PopMenuItem"
        Me.Rotate270PopMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.Rotate270PopMenuItem.Text = "270 degrees"
        '
        'FlipPopMenu
        '
        Me.FlipPopMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FlipHorizontallyPopMenuItem, Me.FlipVerticallyPopMenuItem})
        Me.FlipPopMenu.Name = "FlipPopMenu"
        Me.FlipPopMenu.Size = New System.Drawing.Size(143, 22)
        Me.FlipPopMenu.Text = "Flip"
        '
        'FlipHorizontallyPopMenuItem
        '
        Me.FlipHorizontallyPopMenuItem.Name = "FlipHorizontallyPopMenuItem"
        Me.FlipHorizontallyPopMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.FlipHorizontallyPopMenuItem.Text = "Horizontally"
        '
        'FlipVerticallyPopMenuItem
        '
        Me.FlipVerticallyPopMenuItem.Name = "FlipVerticallyPopMenuItem"
        Me.FlipVerticallyPopMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.FlipVerticallyPopMenuItem.Text = "Vertically"
        '
        'Separator3PopMenuItem
        '
        Me.Separator3PopMenuItem.Name = "Separator3PopMenuItem"
        Me.Separator3PopMenuItem.Size = New System.Drawing.Size(140, 6)
        '
        'SaveImagePopMenuItem
        '
        Me.SaveImagePopMenuItem.Name = "SaveImagePopMenuItem"
        Me.SaveImagePopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SaveImagePopMenuItem.Text = "Save ..."
        '
        'PasteFromPopMenuItem
        '
        Me.PasteFromPopMenuItem.Name = "PasteFromPopMenuItem"
        Me.PasteFromPopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.PasteFromPopMenuItem.Text = "Paste from ..."
        '
        'SelectAllPopMenuItem
        '
        Me.SelectAllPopMenuItem.Name = "SelectAllPopMenuItem"
        Me.SelectAllPopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SelectAllPopMenuItem.Text = "Select All"
        '
        'StatusBar
        '
        Me.StatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.FileNameLabel, Me.SelectRectLabel, Me.MouseCoordsLabel})
        Me.StatusBar.Location = New System.Drawing.Point(0, 380)
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.ShowItemToolTips = True
        Me.StatusBar.Size = New System.Drawing.Size(968, 22)
        Me.StatusBar.TabIndex = 5
        Me.StatusBar.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.StatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(71, 19)
        Me.StatusLabel.Text = "StatusLabel"
        Me.StatusLabel.ToolTipText = "Percentage progress"
        Me.StatusLabel.Visible = False
        '
        'FileNameLabel
        '
        Me.FileNameLabel.Name = "FileNameLabel"
        Me.FileNameLabel.Size = New System.Drawing.Size(755, 17)
        Me.FileNameLabel.Spring = True
        Me.FileNameLabel.Text = "FileNameLabel"
        Me.FileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SelectRectLabel
        '
        Me.SelectRectLabel.ForeColor = System.Drawing.SystemColors.Highlight
        Me.SelectRectLabel.Name = "SelectRectLabel"
        Me.SelectRectLabel.Size = New System.Drawing.Size(89, 17)
        Me.SelectRectLabel.Text = "SelectRectLabel"
        Me.SelectRectLabel.ToolTipText = "Selection, position and size"
        '
        'MouseCoordsLabel
        '
        Me.MouseCoordsLabel.Name = "MouseCoordsLabel"
        Me.MouseCoordsLabel.Size = New System.Drawing.Size(109, 17)
        Me.MouseCoordsLabel.Text = "MouseCoordsLabel"
        Me.MouseCoordsLabel.ToolTipText = "Mouse coordinates"
        '
        'SeparatorButton1
        '
        Me.SeparatorButton1.Name = "SeparatorButton1"
        Me.SeparatorButton1.Size = New System.Drawing.Size(6, 25)
        '
        'SeparatorButton2
        '
        Me.SeparatorButton2.Name = "SeparatorButton2"
        Me.SeparatorButton2.Size = New System.Drawing.Size(6, 25)
        '
        'SeparatorButton3
        '
        Me.SeparatorButton3.Name = "SeparatorButton3"
        Me.SeparatorButton3.Size = New System.Drawing.Size(6, 25)
        '
        'SeparatorButton4
        '
        Me.SeparatorButton4.Name = "SeparatorButton4"
        Me.SeparatorButton4.Size = New System.Drawing.Size(6, 25)
        '
        'ImageZoomLabel
        '
        Me.ImageZoomLabel.Name = "ImageZoomLabel"
        Me.ImageZoomLabel.Size = New System.Drawing.Size(40, 22)
        Me.ImageZoomLabel.Text = "Image"
        Me.ImageZoomLabel.ToolTipText = "Image Zoom"
        '
        'ImageZoomComboBox
        '
        Me.ImageZoomComboBox.Items.AddRange(New Object() {"25%", "50%", "75%", "100%", "200%", "400%", "800%"})
        Me.ImageZoomComboBox.Name = "ImageZoomComboBox"
        Me.ImageZoomComboBox.Size = New System.Drawing.Size(75, 25)
        Me.ImageZoomComboBox.Text = "100%"
        Me.ImageZoomComboBox.ToolTipText = "Image zoom"
        '
        'ResultsZoomLabel
        '
        Me.ResultsZoomLabel.Name = "ResultsZoomLabel"
        Me.ResultsZoomLabel.Size = New System.Drawing.Size(44, 22)
        Me.ResultsZoomLabel.Text = "Results"
        Me.ResultsZoomLabel.ToolTipText = "Results Zoom"
        '
        'ResultsZoomComboBox
        '
        Me.ResultsZoomComboBox.Items.AddRange(New Object() {"25%", "50%", "75%", "100%", "200%", "400%", "800%"})
        Me.ResultsZoomComboBox.Name = "ResultsZoomComboBox"
        Me.ResultsZoomComboBox.Size = New System.Drawing.Size(75, 25)
        Me.ResultsZoomComboBox.Text = "100%"
        Me.ResultsZoomComboBox.ToolTipText = "Results Zoom"
        '
        'SeparatorButton5
        '
        Me.SeparatorButton5.Name = "SeparatorButton5"
        Me.SeparatorButton5.Size = New System.Drawing.Size(6, 25)
        '
        'PageLabel
        '
        Me.PageLabel.Name = "PageLabel"
        Me.PageLabel.Size = New System.Drawing.Size(33, 22)
        Me.PageLabel.Text = "Page"
        '
        'ToolBar
        '
        Me.ToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenButton, Me.SaveResultsButton, Me.SeparatorButton1, Me.OCRButton, Me.PIButton, Me.StopOCRButton, Me.SeparatorButton2, Me.CutButton, Me.CopyButton, Me.PasteButton, Me.ClearButton, Me.InvertButton, Me.SeparatorButton3, Me.Rotate90Button, Me.Rotate180Button, Me.Rotate270Button, Me.FlipHorizontallyButton, Me.FlipVerticallyButton, Me.SeparatorButton4, Me.ImageZoomLabel, Me.ImageZoomComboBox, Me.ResultsZoomLabel, Me.ResultsZoomComboBox, Me.SeparatorButton5, Me.PageLabel})
        Me.ToolBar.Location = New System.Drawing.Point(0, 24)
        Me.ToolBar.Name = "ToolBar"
        Me.ToolBar.Size = New System.Drawing.Size(968, 25)
        Me.ToolBar.TabIndex = 4
        Me.ToolBar.Text = "ToolStrip1"
        '
        'OpenButton
        '
        Me.OpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenButton.Image = Global._Viewer.My.Resources.Resources.Open
        Me.OpenButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenButton.Name = "OpenButton"
        Me.OpenButton.Size = New System.Drawing.Size(23, 22)
        Me.OpenButton.Text = "OpenButton"
        Me.OpenButton.ToolTipText = "Open"
        '
        'SaveResultsButton
        '
        Me.SaveResultsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SaveResultsButton.Image = Global._Viewer.My.Resources.Resources.Save
        Me.SaveResultsButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveResultsButton.Name = "SaveResultsButton"
        Me.SaveResultsButton.Size = New System.Drawing.Size(23, 22)
        Me.SaveResultsButton.Text = "SaveResultsButton"
        Me.SaveResultsButton.ToolTipText = "Save results"
        '
        'OCRButton
        '
        Me.OCRButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OCRButton.Image = Global._Viewer.My.Resources.Resources.Tocr
        Me.OCRButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OCRButton.Name = "OCRButton"
        Me.OCRButton.Size = New System.Drawing.Size(23, 22)
        Me.OCRButton.Text = "OCR Image"
        '
        'PIButton
        '
        Me.PIButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PIButton.Image = CType(resources.GetObject("PIButton.Image"), System.Drawing.Image)
        Me.PIButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PIButton.Name = "PIButton"
        Me.PIButton.Size = New System.Drawing.Size(23, 22)
        Me.PIButton.Text = "Show Processed image"
        '
        'StopOCRButton
        '
        Me.StopOCRButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.StopOCRButton.Image = Global._Viewer.My.Resources.Resources.TocrStop
        Me.StopOCRButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.StopOCRButton.Name = "StopOCRButton"
        Me.StopOCRButton.Size = New System.Drawing.Size(23, 22)
        Me.StopOCRButton.Text = "Stop OCR"
        '
        'CutButton
        '
        Me.CutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CutButton.Image = Global._Viewer.My.Resources.Resources.Cut
        Me.CutButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CutButton.Name = "CutButton"
        Me.CutButton.Size = New System.Drawing.Size(23, 22)
        Me.CutButton.Text = "CutButton"
        Me.CutButton.ToolTipText = "Cut selection"
        '
        'CopyButton
        '
        Me.CopyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CopyButton.Image = Global._Viewer.My.Resources.Resources.Copy
        Me.CopyButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CopyButton.Name = "CopyButton"
        Me.CopyButton.Size = New System.Drawing.Size(23, 22)
        Me.CopyButton.Text = "CopyButton"
        '
        'PasteButton
        '
        Me.PasteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PasteButton.Image = Global._Viewer.My.Resources.Resources.Paste
        Me.PasteButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PasteButton.Name = "PasteButton"
        Me.PasteButton.Size = New System.Drawing.Size(23, 22)
        Me.PasteButton.Text = "PasteButton"
        Me.PasteButton.ToolTipText = "Paste"
        '
        'ClearButton
        '
        Me.ClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ClearButton.Image = Global._Viewer.My.Resources.Resources.Clear
        Me.ClearButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(23, 22)
        Me.ClearButton.Text = "ClearButton"
        Me.ClearButton.ToolTipText = "Clear selection"
        '
        'InvertButton
        '
        Me.InvertButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.InvertButton.Image = Global._Viewer.My.Resources.Resources.Invert
        Me.InvertButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.InvertButton.Name = "InvertButton"
        Me.InvertButton.Size = New System.Drawing.Size(23, 22)
        Me.InvertButton.Text = "InvertButton"
        '
        'Rotate90Button
        '
        Me.Rotate90Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.Rotate90Button.Image = Global._Viewer.My.Resources.Resources.Rotate90
        Me.Rotate90Button.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.Rotate90Button.Name = "Rotate90Button"
        Me.Rotate90Button.Size = New System.Drawing.Size(23, 22)
        Me.Rotate90Button.Text = "Rotate90Button"
        '
        'Rotate180Button
        '
        Me.Rotate180Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.Rotate180Button.Image = Global._Viewer.My.Resources.Resources.Rotate180
        Me.Rotate180Button.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.Rotate180Button.Name = "Rotate180Button"
        Me.Rotate180Button.Size = New System.Drawing.Size(23, 22)
        Me.Rotate180Button.Text = "Rotate180Button"
        '
        'Rotate270Button
        '
        Me.Rotate270Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.Rotate270Button.Image = Global._Viewer.My.Resources.Resources.Rotate270
        Me.Rotate270Button.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.Rotate270Button.Name = "Rotate270Button"
        Me.Rotate270Button.Size = New System.Drawing.Size(23, 22)
        Me.Rotate270Button.Text = "Rotate270Button"
        '
        'FlipHorizontallyButton
        '
        Me.FlipHorizontallyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.FlipHorizontallyButton.Image = Global._Viewer.My.Resources.Resources.FlipH
        Me.FlipHorizontallyButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.FlipHorizontallyButton.Name = "FlipHorizontallyButton"
        Me.FlipHorizontallyButton.Size = New System.Drawing.Size(23, 22)
        Me.FlipHorizontallyButton.Text = "FlipHorizontallyButton"
        '
        'FlipVerticallyButton
        '
        Me.FlipVerticallyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.FlipVerticallyButton.Image = Global._Viewer.My.Resources.Resources.FlipV
        Me.FlipVerticallyButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.FlipVerticallyButton.Name = "FlipVerticallyButton"
        Me.FlipVerticallyButton.Size = New System.Drawing.Size(23, 22)
        Me.FlipVerticallyButton.Text = "FlipVerticallyButton"
        '
        'Viewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(968, 402)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.StatusBar)
        Me.Controls.Add(Me.ToolBar)
        Me.Controls.Add(Me.MenuBar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuBar
        Me.Name = "Viewer"
        Me.Text = "Form1"
        Me.MenuBar.ResumeLayout(False)
        Me.MenuBar.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.ImageFocusPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TextContextMenu.ResumeLayout(False)
        CType(Me.ResultsFocusPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ImageContextMenu.ResumeLayout(False)
        Me.StatusBar.ResumeLayout(False)
        Me.StatusBar.PerformLayout()
        Me.ToolBar.ResumeLayout(False)
        Me.ToolBar.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuBar As System.Windows.Forms.MenuStrip
    Friend WithEvents FileMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents OpenMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolBarMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator1FileMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectTWAINDeviceMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AcquireImagesMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator2FileMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveResultsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator3FileMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OCRImageMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OCRSelectionMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StopOCRMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator4FileMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BatchMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator5FileMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CutMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator1ImageMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RotateMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlipMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator3ImageMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveImageMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InvertMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator2ImageMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PasteFromMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectAllMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator4ImageMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AdjustDPIMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Rotate90MenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Rotate180MenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Rotate270MenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlipHorizontallyMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlipVerticallyMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusBar As System.Windows.Forms.StatusStrip
    Friend WithEvents FileNameLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SelectRectLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MouseCoordsLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusBarMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpTopicsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutTOCRViewerMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator1HelpMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ResultsFontMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResultsFormattingMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProcessingOptionsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LockZoomMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResultsFocusPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents ImageFocusPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents FocusbarsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CutPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PastePopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator1PopMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents InvertPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator2PopMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RotatePopMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveImagePopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteFromPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectAllPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlipPopMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Separator3PopMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Rotate90PopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Rotate180PopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Rotate270PopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlipHorizontallyPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlipVerticallyPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ShowDeviceUIMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResultsRichTextBox As System.Windows.Forms.RichTextBox
    Friend WithEvents TextContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColourConversionMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveResultsButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SeparatorButton1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OCRButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents StopOCRButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SeparatorButton2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CutButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents CopyButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PasteButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ClearButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents InvertButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SeparatorButton3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Rotate90Button As System.Windows.Forms.ToolStripButton
    Friend WithEvents Rotate180Button As System.Windows.Forms.ToolStripButton
    Friend WithEvents Rotate270Button As System.Windows.Forms.ToolStripButton
    Friend WithEvents FlipHorizontallyButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents FlipVerticallyButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SeparatorButton4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ImageZoomLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ImageZoomComboBox As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ResultsZoomLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ResultsZoomComboBox As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents SeparatorButton5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PageLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolBar As System.Windows.Forms.ToolStrip
    Friend WithEvents PIButton As System.Windows.Forms.ToolStripButton

End Class
