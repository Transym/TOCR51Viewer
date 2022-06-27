<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class About
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
        Me.CompanyLabel = New System.Windows.Forms.Label()
        Me.ProductLabel = New System.Windows.Forms.Label()
        Me.ViewerVersionLabel = New System.Windows.Forms.Label()
        Me.EngineVersionLabel = New System.Windows.Forms.Label()
        Me.LicenceLabel = New System.Windows.Forms.Label()
        Me.VolumeLabel = New System.Windows.Forms.Label()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.FeaturesLabel = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CompanyLabel
        '
        Me.CompanyLabel.AutoSize = True
        Me.CompanyLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CompanyLabel.Location = New System.Drawing.Point(80, 25)
        Me.CompanyLabel.Name = "CompanyLabel"
        Me.CompanyLabel.Size = New System.Drawing.Size(228, 16)
        Me.CompanyLabel.TabIndex = 1
        Me.CompanyLabel.Text = "Transym Computer Services Ltd"
        '
        'ProductLabel
        '
        Me.ProductLabel.AutoSize = True
        Me.ProductLabel.Location = New System.Drawing.Point(80, 49)
        Me.ProductLabel.Name = "ProductLabel"
        Me.ProductLabel.Size = New System.Drawing.Size(72, 13)
        Me.ProductLabel.TabIndex = 2
        Me.ProductLabel.Text = "TOCR Viewer"
        '
        'ViewerVersionLabel
        '
        Me.ViewerVersionLabel.AutoSize = True
        Me.ViewerVersionLabel.Location = New System.Drawing.Point(80, 69)
        Me.ViewerVersionLabel.Name = "ViewerVersionLabel"
        Me.ViewerVersionLabel.Size = New System.Drawing.Size(39, 13)
        Me.ViewerVersionLabel.TabIndex = 3
        Me.ViewerVersionLabel.Text = "Label1"
        '
        'EngineVersionLabel
        '
        Me.EngineVersionLabel.AutoSize = True
        Me.EngineVersionLabel.Location = New System.Drawing.Point(80, 92)
        Me.EngineVersionLabel.Name = "EngineVersionLabel"
        Me.EngineVersionLabel.Size = New System.Drawing.Size(39, 13)
        Me.EngineVersionLabel.TabIndex = 4
        Me.EngineVersionLabel.Text = "Label1"
        '
        'LicenceLabel
        '
        Me.LicenceLabel.AutoSize = True
        Me.LicenceLabel.Location = New System.Drawing.Point(80, 113)
        Me.LicenceLabel.Name = "LicenceLabel"
        Me.LicenceLabel.Size = New System.Drawing.Size(39, 13)
        Me.LicenceLabel.TabIndex = 5
        Me.LicenceLabel.Text = "Label1"
        '
        'VolumeLabel
        '
        Me.VolumeLabel.AutoSize = True
        Me.VolumeLabel.Location = New System.Drawing.Point(80, 155)
        Me.VolumeLabel.Name = "VolumeLabel"
        Me.VolumeLabel.Size = New System.Drawing.Size(39, 13)
        Me.VolumeLabel.TabIndex = 6
        Me.VolumeLabel.Text = "Label1"
        '
        'OKButton
        '
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OKButton.Location = New System.Drawing.Point(118, 171)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OKButton.Size = New System.Drawing.Size(89, 25)
        Me.OKButton.TabIndex = 7
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'FeaturesLabel
        '
        Me.FeaturesLabel.AutoSize = True
        Me.FeaturesLabel.Location = New System.Drawing.Point(80, 134)
        Me.FeaturesLabel.Name = "FeaturesLabel"
        Me.FeaturesLabel.Size = New System.Drawing.Size(39, 13)
        Me.FeaturesLabel.TabIndex = 9
        Me.FeaturesLabel.Text = "Label1"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(9, 21)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'About
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(326, 203)
        Me.Controls.Add(Me.FeaturesLabel)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.VolumeLabel)
        Me.Controls.Add(Me.LicenceLabel)
        Me.Controls.Add(Me.EngineVersionLabel)
        Me.Controls.Add(Me.ViewerVersionLabel)
        Me.Controls.Add(Me.ProductLabel)
        Me.Controls.Add(Me.CompanyLabel)
        Me.Controls.Add(Me.PictureBox1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "About"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About TOCR Viewer"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents CompanyLabel As System.Windows.Forms.Label
    Friend WithEvents ProductLabel As System.Windows.Forms.Label
    Friend WithEvents ViewerVersionLabel As System.Windows.Forms.Label
    Friend WithEvents EngineVersionLabel As System.Windows.Forms.Label
    Friend WithEvents LicenceLabel As System.Windows.Forms.Label
    Friend WithEvents VolumeLabel As System.Windows.Forms.Label
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents FeaturesLabel As System.Windows.Forms.Label
End Class
