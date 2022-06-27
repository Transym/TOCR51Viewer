Public Class ProcessedImage
    Private Sub ProcessedImage_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        ' a bug in the dlg box causes a grey rectangle to appear in the top left corner
        ' resizing the dlg box fixes this
        Width = 570
        Height = 400
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim SaveFileDialog As New SaveFileDialog
        Dim Fmt As Imaging.ImageFormat = Imaging.ImageFormat.Bmp
        Static FilterIndex As Integer

        With SaveFileDialog

            .InitialDirectory = OpenInitDir
            .Title = "Save image"
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
                BMP = Me.EdPanel.Image(Imaging.PixelFormat.Format1bppIndexed)
                BMP.Save(.FileName, Fmt)
                FilterIndex = .FilterIndex
            End If
        End With

    End Sub
End Class