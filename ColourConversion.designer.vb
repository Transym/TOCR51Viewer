<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ColourConversion
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ColourConversion))
        Me.CanclButton = New System.Windows.Forms.Button()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.ClrPanel = New System.Windows.Forms.Panel()
        Me.BWPanel = New System.Windows.Forms.Panel()
        Me.AlgoGroupBox = New System.Windows.Forms.GroupBox()
        Me.AlgoFormulaLabel = New System.Windows.Forms.Label()
        Me.AlgoNameLabel = New System.Windows.Forms.Label()
        Me.AlgoNUD = New System.Windows.Forms.NumericUpDown()
        Me.ThresholdGroupBox = New System.Windows.Forms.GroupBox()
        Me.ThresholdHScrollBar = New System.Windows.Forms.HScrollBar()
        Me.ThresholdLabel = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ColourTabControl = New System.Windows.Forms.TabControl()
        Me.ThresholdTabPage = New System.Windows.Forms.TabPage()
        Me.AutomaticTabPage = New System.Windows.Forms.TabPage()
        Me.AutomaticPanel = New System.Windows.Forms.Panel()
        Me.RegionCGRadioButton = New System.Windows.Forms.RadioButton()
        Me.MeanCGRadioButton = New System.Windows.Forms.RadioButton()
        Me.HistogramCGRadioButton = New System.Windows.Forms.RadioButton()
        Me.AlgoGroupBox.SuspendLayout()
        CType(Me.AlgoNUD, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ThresholdGroupBox.SuspendLayout()
        Me.ColourTabControl.SuspendLayout()
        Me.ThresholdTabPage.SuspendLayout()
        Me.AutomaticTabPage.SuspendLayout()
        Me.AutomaticPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'CanclButton
        '
        Me.CanclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CanclButton.Location = New System.Drawing.Point(349, 494)
        Me.CanclButton.Name = "CanclButton"
        Me.CanclButton.Size = New System.Drawing.Size(89, 25)
        Me.CanclButton.TabIndex = 5
        Me.CanclButton.Text = "Cancel"
        Me.CanclButton.UseVisualStyleBackColor = True
        '
        'OKButton
        '
        Me.OKButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(221, 494)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(89, 25)
        Me.OKButton.TabIndex = 4
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'ClrPanel
        '
        Me.ClrPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClrPanel.Location = New System.Drawing.Point(22, 175)
        Me.ClrPanel.Name = "ClrPanel"
        Me.ClrPanel.Size = New System.Drawing.Size(300, 300)
        Me.ClrPanel.TabIndex = 2
        '
        'BWPanel
        '
        Me.BWPanel.BackColor = System.Drawing.SystemColors.Control
        Me.BWPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BWPanel.Location = New System.Drawing.Point(341, 175)
        Me.BWPanel.Name = "BWPanel"
        Me.BWPanel.Size = New System.Drawing.Size(300, 300)
        Me.BWPanel.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.BWPanel, "Monochrome image")
        '
        'AlgoGroupBox
        '
        Me.AlgoGroupBox.Controls.Add(Me.AlgoFormulaLabel)
        Me.AlgoGroupBox.Controls.Add(Me.AlgoNameLabel)
        Me.AlgoGroupBox.Controls.Add(Me.AlgoNUD)
        Me.AlgoGroupBox.Location = New System.Drawing.Point(6, 6)
        Me.AlgoGroupBox.Name = "AlgoGroupBox"
        Me.AlgoGroupBox.Size = New System.Drawing.Size(619, 53)
        Me.AlgoGroupBox.TabIndex = 0
        Me.AlgoGroupBox.TabStop = False
        Me.AlgoGroupBox.Text = "Algorithm"
        Me.ToolTip1.SetToolTip(Me.AlgoGroupBox, "Colour to Monochrome conversion algorithm")
        '
        'AlgoFormulaLabel
        '
        Me.AlgoFormulaLabel.AutoSize = True
        Me.AlgoFormulaLabel.Location = New System.Drawing.Point(39, 36)
        Me.AlgoFormulaLabel.Name = "AlgoFormulaLabel"
        Me.AlgoFormulaLabel.Size = New System.Drawing.Size(39, 13)
        Me.AlgoFormulaLabel.TabIndex = 2
        Me.AlgoFormulaLabel.Text = "Label2"
        Me.ToolTip1.SetToolTip(Me.AlgoFormulaLabel, "Algorithm to detrmine 'White'")
        '
        'AlgoNameLabel
        '
        Me.AlgoNameLabel.AutoSize = True
        Me.AlgoNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AlgoNameLabel.Location = New System.Drawing.Point(39, 21)
        Me.AlgoNameLabel.Name = "AlgoNameLabel"
        Me.AlgoNameLabel.Size = New System.Drawing.Size(45, 13)
        Me.AlgoNameLabel.TabIndex = 1
        Me.AlgoNameLabel.Text = "Label1"
        Me.ToolTip1.SetToolTip(Me.AlgoNameLabel, "Algorithm name")
        '
        'AlgoNUD
        '
        Me.AlgoNUD.InterceptArrowKeys = False
        Me.AlgoNUD.Location = New System.Drawing.Point(6, 19)
        Me.AlgoNUD.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.AlgoNUD.Name = "AlgoNUD"
        Me.AlgoNUD.Size = New System.Drawing.Size(27, 20)
        Me.AlgoNUD.TabIndex = 0
        Me.AlgoNUD.ThousandsSeparator = True
        Me.ToolTip1.SetToolTip(Me.AlgoNUD, "Select 1 of 9 algorithms")
        '
        'ThresholdGroupBox
        '
        Me.ThresholdGroupBox.Controls.Add(Me.ThresholdHScrollBar)
        Me.ThresholdGroupBox.Controls.Add(Me.ThresholdLabel)
        Me.ThresholdGroupBox.Location = New System.Drawing.Point(6, 65)
        Me.ThresholdGroupBox.Name = "ThresholdGroupBox"
        Me.ThresholdGroupBox.Size = New System.Drawing.Size(619, 53)
        Me.ThresholdGroupBox.TabIndex = 1
        Me.ThresholdGroupBox.TabStop = False
        Me.ThresholdGroupBox.Text = "Threshold"
        '
        'ThresholdHScrollBar
        '
        Me.ThresholdHScrollBar.Location = New System.Drawing.Point(8, 21)
        Me.ThresholdHScrollBar.Maximum = 109
        Me.ThresholdHScrollBar.Name = "ThresholdHScrollBar"
        Me.ThresholdHScrollBar.Size = New System.Drawing.Size(570, 20)
        Me.ThresholdHScrollBar.TabIndex = 0
        '
        'ThresholdLabel
        '
        Me.ThresholdLabel.AutoSize = True
        Me.ThresholdLabel.Location = New System.Drawing.Point(577, 25)
        Me.ThresholdLabel.Name = "ThresholdLabel"
        Me.ThresholdLabel.Size = New System.Drawing.Size(33, 13)
        Me.ThresholdLabel.TabIndex = 1
        Me.ThresholdLabel.Text = "100%"
        '
        'ColourTabControl
        '
        Me.ColourTabControl.Controls.Add(Me.ThresholdTabPage)
        Me.ColourTabControl.Controls.Add(Me.AutomaticTabPage)
        Me.ColourTabControl.Location = New System.Drawing.Point(12, 12)
        Me.ColourTabControl.Name = "ColourTabControl"
        Me.ColourTabControl.SelectedIndex = 0
        Me.ColourTabControl.Size = New System.Drawing.Size(643, 157)
        Me.ColourTabControl.TabIndex = 6
        '
        'ThresholdTabPage
        '
        Me.ThresholdTabPage.Controls.Add(Me.AlgoGroupBox)
        Me.ThresholdTabPage.Controls.Add(Me.ThresholdGroupBox)
        Me.ThresholdTabPage.Location = New System.Drawing.Point(4, 22)
        Me.ThresholdTabPage.Name = "ThresholdTabPage"
        Me.ThresholdTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.ThresholdTabPage.Size = New System.Drawing.Size(635, 131)
        Me.ThresholdTabPage.TabIndex = 0
        Me.ThresholdTabPage.Text = "Threshold Colour Algorithms"
        Me.ThresholdTabPage.UseVisualStyleBackColor = True
        '
        'AutomaticTabPage
        '
        Me.AutomaticTabPage.Controls.Add(Me.AutomaticPanel)
        Me.AutomaticTabPage.Location = New System.Drawing.Point(4, 22)
        Me.AutomaticTabPage.Name = "AutomaticTabPage"
        Me.AutomaticTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.AutomaticTabPage.Size = New System.Drawing.Size(635, 131)
        Me.AutomaticTabPage.TabIndex = 1
        Me.AutomaticTabPage.Text = "Automatic Grey Algorithms"
        Me.AutomaticTabPage.UseVisualStyleBackColor = True
        '
        'AutomaticPanel
        '
        Me.AutomaticPanel.Controls.Add(Me.RegionCGRadioButton)
        Me.AutomaticPanel.Controls.Add(Me.MeanCGRadioButton)
        Me.AutomaticPanel.Controls.Add(Me.HistogramCGRadioButton)
        Me.AutomaticPanel.Location = New System.Drawing.Point(15, 18)
        Me.AutomaticPanel.Name = "AutomaticPanel"
        Me.AutomaticPanel.Size = New System.Drawing.Size(200, 98)
        Me.AutomaticPanel.TabIndex = 1
        '
        'RegionCGRadioButton
        '
        Me.RegionCGRadioButton.AutoSize = True
        Me.RegionCGRadioButton.Location = New System.Drawing.Point(24, 41)
        Me.RegionCGRadioButton.Name = "RegionCGRadioButton"
        Me.RegionCGRadioButton.Size = New System.Drawing.Size(59, 17)
        Me.RegionCGRadioButton.TabIndex = 2
        Me.RegionCGRadioButton.TabStop = True
        Me.RegionCGRadioButton.Text = "Region"
        Me.RegionCGRadioButton.UseVisualStyleBackColor = True
        Me.RegionCGRadioButton.Visible = False
        '
        'MeanCGRadioButton
        '
        Me.MeanCGRadioButton.AutoSize = True
        Me.MeanCGRadioButton.Location = New System.Drawing.Point(24, 66)
        Me.MeanCGRadioButton.Name = "MeanCGRadioButton"
        Me.MeanCGRadioButton.Size = New System.Drawing.Size(52, 17)
        Me.MeanCGRadioButton.TabIndex = 1
        Me.MeanCGRadioButton.TabStop = True
        Me.MeanCGRadioButton.Text = "Mean"
        Me.MeanCGRadioButton.UseVisualStyleBackColor = True
        Me.MeanCGRadioButton.Visible = False
        '
        'HistogramCGRadioButton
        '
        Me.HistogramCGRadioButton.AutoSize = True
        Me.HistogramCGRadioButton.Location = New System.Drawing.Point(24, 13)
        Me.HistogramCGRadioButton.Name = "HistogramCGRadioButton"
        Me.HistogramCGRadioButton.Size = New System.Drawing.Size(72, 17)
        Me.HistogramCGRadioButton.TabIndex = 0
        Me.HistogramCGRadioButton.TabStop = True
        Me.HistogramCGRadioButton.Text = "Histogram"
        Me.HistogramCGRadioButton.UseVisualStyleBackColor = True
        '
        'ColourConversion
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CanclButton
        Me.ClientSize = New System.Drawing.Size(665, 527)
        Me.Controls.Add(Me.ColourTabControl)
        Me.Controls.Add(Me.ClrPanel)
        Me.Controls.Add(Me.BWPanel)
        Me.Controls.Add(Me.CanclButton)
        Me.Controls.Add(Me.OKButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ColourConversion"
        Me.Text = "Colour Conversion"
        Me.AlgoGroupBox.ResumeLayout(False)
        Me.AlgoGroupBox.PerformLayout()
        CType(Me.AlgoNUD, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ThresholdGroupBox.ResumeLayout(False)
        Me.ThresholdGroupBox.PerformLayout()
        Me.ColourTabControl.ResumeLayout(False)
        Me.ThresholdTabPage.ResumeLayout(False)
        Me.AutomaticTabPage.ResumeLayout(False)
        Me.AutomaticPanel.ResumeLayout(False)
        Me.AutomaticPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CanclButton As System.Windows.Forms.Button
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents ClrPanel As System.Windows.Forms.Panel
    Friend WithEvents BWPanel As System.Windows.Forms.Panel
    Friend WithEvents AlgoGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AlgoFormulaLabel As System.Windows.Forms.Label
    Friend WithEvents AlgoNameLabel As System.Windows.Forms.Label
    Friend WithEvents AlgoNUD As System.Windows.Forms.NumericUpDown
    Friend WithEvents ThresholdGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ThresholdHScrollBar As System.Windows.Forms.HScrollBar
    Friend WithEvents ThresholdLabel As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ColourTabControl As System.Windows.Forms.TabControl
    Friend WithEvents ThresholdTabPage As System.Windows.Forms.TabPage
    Friend WithEvents AutomaticTabPage As System.Windows.Forms.TabPage
    Friend WithEvents AutomaticPanel As System.Windows.Forms.Panel
    Friend WithEvents MeanCGRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents HistogramCGRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents RegionCGRadioButton As System.Windows.Forms.RadioButton
End Class
