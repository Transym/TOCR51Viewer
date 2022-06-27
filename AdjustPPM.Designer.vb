<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdjustPPM
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AdjustPPM))
        Me.InfoLabel = New System.Windows.Forms.Label
        Me.XDPIComboBox = New System.Windows.Forms.ComboBox
        Me.XDPILabel = New System.Windows.Forms.Label
        Me.YDPILabel = New System.Windows.Forms.Label
        Me.YDPIComboBox = New System.Windows.Forms.ComboBox
        Me.OKButton = New System.Windows.Forms.Button
        Me.CanclButton = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'InfoLabel
        '
        Me.InfoLabel.Location = New System.Drawing.Point(23, 12)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(233, 57)
        Me.InfoLabel.TabIndex = 0
        Me.InfoLabel.Text = "Label1"
        Me.InfoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'XDPIComboBox
        '
        Me.XDPIComboBox.FormattingEnabled = True
        Me.XDPIComboBox.Items.AddRange(New Object() {"100", "150", "200", "300", "400", "600", "1200"})
        Me.XDPIComboBox.Location = New System.Drawing.Point(56, 82)
        Me.XDPIComboBox.Name = "XDPIComboBox"
        Me.XDPIComboBox.Size = New System.Drawing.Size(62, 21)
        Me.XDPIComboBox.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.XDPIComboBox, "Horizontal Dots Per Inch")
        '
        'XDPILabel
        '
        Me.XDPILabel.AutoSize = True
        Me.XDPILabel.Location = New System.Drawing.Point(12, 85)
        Me.XDPILabel.Name = "XDPILabel"
        Me.XDPILabel.Size = New System.Drawing.Size(35, 13)
        Me.XDPILabel.TabIndex = 1
        Me.XDPILabel.Text = "&X DPI"
        '
        'YDPILabel
        '
        Me.YDPILabel.AutoSize = True
        Me.YDPILabel.Location = New System.Drawing.Point(151, 85)
        Me.YDPILabel.Name = "YDPILabel"
        Me.YDPILabel.Size = New System.Drawing.Size(35, 13)
        Me.YDPILabel.TabIndex = 3
        Me.YDPILabel.Text = "&Y DPI"
        '
        'YDPIComboBox
        '
        Me.YDPIComboBox.FormattingEnabled = True
        Me.YDPIComboBox.Items.AddRange(New Object() {"100", "150", "200", "300", "400", "600", "1200"})
        Me.YDPIComboBox.Location = New System.Drawing.Point(196, 82)
        Me.YDPIComboBox.Name = "YDPIComboBox"
        Me.YDPIComboBox.Size = New System.Drawing.Size(72, 21)
        Me.YDPIComboBox.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.YDPIComboBox, "Vertical Dots Per Inch")
        '
        'OKButton
        '
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.OKButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OKButton.Location = New System.Drawing.Point(34, 120)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OKButton.Size = New System.Drawing.Size(89, 25)
        Me.OKButton.TabIndex = 5
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'CanclButton
        '
        Me.CanclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CanclButton.Location = New System.Drawing.Point(167, 120)
        Me.CanclButton.Name = "CanclButton"
        Me.CanclButton.Size = New System.Drawing.Size(89, 25)
        Me.CanclButton.TabIndex = 6
        Me.CanclButton.Text = "Cancel"
        Me.CanclButton.UseVisualStyleBackColor = True
        '
        'AdjustPPM
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CanclButton
        Me.ClientSize = New System.Drawing.Size(284, 160)
        Me.Controls.Add(Me.CanclButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.YDPIComboBox)
        Me.Controls.Add(Me.YDPILabel)
        Me.Controls.Add(Me.XDPILabel)
        Me.Controls.Add(Me.XDPIComboBox)
        Me.Controls.Add(Me.InfoLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AdjustPPM"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "AdjustPPM"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents InfoLabel As System.Windows.Forms.Label
    Friend WithEvents XDPIComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents XDPILabel As System.Windows.Forms.Label
    Friend WithEvents YDPILabel As System.Windows.Forms.Label
    Friend WithEvents YDPIComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents CanclButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
