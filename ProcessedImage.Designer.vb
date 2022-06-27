<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProcessedImage
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
    Public EdPanel As EditableImage

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.EdPanel = New _Viewer.EditableImage()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'EdPanel
        '
        Me.EdPanel.AllowSelect = True
        Me.EdPanel.ClearColor = System.Drawing.Color.White
        Me.EdPanel.ConvertToMonochrome = False
        Me.EdPanel.DoubleBuffered = True
        Me.EdPanel.FastRenderFormat = System.Drawing.Imaging.PixelFormat.Format32bppPArgb
        Me.EdPanel.Image = Nothing
        Me.EdPanel.Location = New System.Drawing.Point(0, 0)
        Me.EdPanel.Modified = False
        Me.EdPanel.Name = "EdPanel"
        Me.EdPanel.NormaliseAspectRatio = True
        Me.EdPanel.RaisePaintedEvent = False
        Me.EdPanel.Size = New System.Drawing.Size(0, 0)
        Me.EdPanel.TabIndex = 0
        Me.EdPanel.UseFastRenderFormat = True
        Me.EdPanel.Zoom = 100.0!
        Me.EdPanel.ZoomStyle = _Viewer.EditableImage.ZoomStyleEnum.FitToNothing
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(99, 26)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'ProcessedImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(562, 362)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.EdPanel)
        Me.Name = "ProcessedImage"
        Me.Text = "Image processed by TOCR service"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    'Friend WithEvents Panel1 As System.Windows.Forms.Panel
End Class
