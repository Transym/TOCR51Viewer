<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResultsFormatting
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ResultsFormatting))
        Me.CanclButton = New System.Windows.Forms.Button()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CollapseCheckBox = New System.Windows.Forms.CheckBox()
        Me.LeftCheckBox = New System.Windows.Forms.CheckBox()
        Me.XSpaceLabel = New System.Windows.Forms.Label()
        Me.XSpaceTextBox = New System.Windows.Forms.TextBox()
        Me.YSpaceLabel = New System.Windows.Forms.Label()
        Me.YSpaceTextBox = New System.Windows.Forms.TextBox()
        Me.NoFormatRadioButton = New System.Windows.Forms.RadioButton()
        Me.TextFormatRadioButton = New System.Windows.Forms.RadioButton()
        Me.RTFFormatRadioButton = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'CanclButton
        '
        Me.CanclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CanclButton.Location = New System.Drawing.Point(166, 240)
        Me.CanclButton.Name = "CanclButton"
        Me.CanclButton.Size = New System.Drawing.Size(89, 25)
        Me.CanclButton.TabIndex = 10
        Me.CanclButton.Text = "Cancel"
        Me.CanclButton.UseVisualStyleBackColor = True
        '
        'OKButton
        '
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.OKButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OKButton.Location = New System.Drawing.Point(33, 240)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OKButton.Size = New System.Drawing.Size(89, 25)
        Me.OKButton.TabIndex = 9
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'CollapseCheckBox
        '
        Me.CollapseCheckBox.AutoSize = True
        Me.CollapseCheckBox.Location = New System.Drawing.Point(78, 110)
        Me.CollapseCheckBox.Name = "CollapseCheckBox"
        Me.CollapseCheckBox.Size = New System.Drawing.Size(103, 17)
        Me.CollapseCheckBox.TabIndex = 3
        Me.CollapseCheckBox.Text = "&Collapse vertical"
        Me.ToolTip1.SetToolTip(Me.CollapseCheckBox, "Collapse large vertical white spaces to a single blank line")
        Me.CollapseCheckBox.UseVisualStyleBackColor = True
        '
        'LeftCheckBox
        '
        Me.LeftCheckBox.AutoSize = True
        Me.LeftCheckBox.Location = New System.Drawing.Point(78, 135)
        Me.LeftCheckBox.Name = "LeftCheckBox"
        Me.LeftCheckBox.Size = New System.Drawing.Size(78, 17)
        Me.LeftCheckBox.TabIndex = 4
        Me.LeftCheckBox.Text = "&Left margin"
        Me.ToolTip1.SetToolTip(Me.LeftCheckBox, "Keep the left margin in the results")
        Me.LeftCheckBox.UseVisualStyleBackColor = True
        '
        'XSpaceLabel
        '
        Me.XSpaceLabel.AutoSize = True
        Me.XSpaceLabel.Location = New System.Drawing.Point(33, 170)
        Me.XSpaceLabel.Name = "XSpaceLabel"
        Me.XSpaceLabel.Size = New System.Drawing.Size(76, 13)
        Me.XSpaceLabel.TabIndex = 5
        Me.XSpaceLabel.Text = "&X space factor"
        Me.ToolTip1.SetToolTip(Me.XSpaceLabel, "Determines the size of space between words that will cause the word to be 'placed" & _
        "'")
        '
        'XSpaceTextBox
        '
        Me.XSpaceTextBox.Location = New System.Drawing.Point(118, 166)
        Me.XSpaceTextBox.Name = "XSpaceTextBox"
        Me.XSpaceTextBox.Size = New System.Drawing.Size(99, 20)
        Me.XSpaceTextBox.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.XSpaceTextBox, "Determines the size of space between words that will cause the word to be 'placed" & _
        "'")
        '
        'YSpaceLabel
        '
        Me.YSpaceLabel.AutoSize = True
        Me.YSpaceLabel.Location = New System.Drawing.Point(33, 195)
        Me.YSpaceLabel.Name = "YSpaceLabel"
        Me.YSpaceLabel.Size = New System.Drawing.Size(76, 13)
        Me.YSpaceLabel.TabIndex = 7
        Me.YSpaceLabel.Text = "&Y space factor"
        Me.ToolTip1.SetToolTip(Me.YSpaceLabel, "Determines the size of space between lines that will cause blank lines to be inse" & _
        "rted")
        '
        'YSpaceTextBox
        '
        Me.YSpaceTextBox.Location = New System.Drawing.Point(120, 194)
        Me.YSpaceTextBox.Name = "YSpaceTextBox"
        Me.YSpaceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.YSpaceTextBox.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.YSpaceTextBox, "Determines the size of space between lines that will cause blank lines to be inse" & _
        "rted")
        '
        'NoFormatRadioButton
        '
        Me.NoFormatRadioButton.AutoSize = True
        Me.NoFormatRadioButton.Location = New System.Drawing.Point(68, 20)
        Me.NoFormatRadioButton.Name = "NoFormatRadioButton"
        Me.NoFormatRadioButton.Size = New System.Drawing.Size(88, 17)
        Me.NoFormatRadioButton.TabIndex = 0
        Me.NoFormatRadioButton.TabStop = True
        Me.NoFormatRadioButton.Text = "&No formatting"
        Me.ToolTip1.SetToolTip(Me.NoFormatRadioButton, "No formatting of results")
        Me.NoFormatRadioButton.UseVisualStyleBackColor = True
        '
        'TextFormatRadioButton
        '
        Me.TextFormatRadioButton.AutoSize = True
        Me.TextFormatRadioButton.Location = New System.Drawing.Point(68, 43)
        Me.TextFormatRadioButton.Name = "TextFormatRadioButton"
        Me.TextFormatRadioButton.Size = New System.Drawing.Size(95, 17)
        Me.TextFormatRadioButton.TabIndex = 1
        Me.TextFormatRadioButton.TabStop = True
        Me.TextFormatRadioButton.Text = "&Text formatting"
        Me.ToolTip1.SetToolTip(Me.TextFormatRadioButton, "Old stye text formatting using the default font")
        Me.TextFormatRadioButton.UseVisualStyleBackColor = True
        '
        'RTFFormatRadioButton
        '
        Me.RTFFormatRadioButton.AutoSize = True
        Me.RTFFormatRadioButton.Location = New System.Drawing.Point(68, 69)
        Me.RTFFormatRadioButton.Name = "RTFFormatRadioButton"
        Me.RTFFormatRadioButton.Size = New System.Drawing.Size(120, 17)
        Me.RTFFormatRadioButton.TabIndex = 2
        Me.RTFFormatRadioButton.TabStop = True
        Me.RTFFormatRadioButton.Text = "&Rich Text formatting"
        Me.ToolTip1.SetToolTip(Me.RTFFormatRadioButton, "Rich text formatting.")
        Me.RTFFormatRadioButton.UseVisualStyleBackColor = True
        '
        'ResultsFormatting
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CanclButton
        Me.ClientSize = New System.Drawing.Size(297, 284)
        Me.Controls.Add(Me.RTFFormatRadioButton)
        Me.Controls.Add(Me.TextFormatRadioButton)
        Me.Controls.Add(Me.NoFormatRadioButton)
        Me.Controls.Add(Me.YSpaceTextBox)
        Me.Controls.Add(Me.YSpaceLabel)
        Me.Controls.Add(Me.XSpaceTextBox)
        Me.Controls.Add(Me.XSpaceLabel)
        Me.Controls.Add(Me.LeftCheckBox)
        Me.Controls.Add(Me.CollapseCheckBox)
        Me.Controls.Add(Me.CanclButton)
        Me.Controls.Add(Me.OKButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ResultsFormatting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Format Results"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CanclButton As System.Windows.Forms.Button
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents CollapseCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents LeftCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents XSpaceLabel As System.Windows.Forms.Label
    Friend WithEvents XSpaceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents YSpaceLabel As System.Windows.Forms.Label
    Friend WithEvents YSpaceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NoFormatRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents TextFormatRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents RTFFormatRadioButton As System.Windows.Forms.RadioButton
End Class
