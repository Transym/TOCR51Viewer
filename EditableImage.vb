'***********************************************************************************************************************
' Editable Image form
' 
' This class provides the class for displaying and maniplating the image
' on the Viewer program.  As an 'exercise' it has been coded in such a way that
' with a small effort it could be turned into a toolbox control.

Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing

<ToolboxItem(True)> _
Public Class EditableImage
    Inherits System.Windows.Forms.Control

#Region " Definitions "
    <EditorBrowsable(EditorBrowsableState.Never), Description("Change zoom automatically. Overrides the Zoom setting."), _
    Category("Appearance")> _
    Public Enum ZoomStyleEnum
        ''' <summary>
        ''' Fit image to width of display area.
        ''' </summary>
        FitToWidth
        ''' <summary>
        ''' Fit image to height of display area.
        ''' </summary>
        FitToHeight
        ''' <summary>
        ''' Fit image to height or width of display area. No scroll bars.
        ''' </summary>
        FitToDisplay
        ''' <summary>
        ''' Use the Zoom percentage setting.
        ''' </summary>
        FitToNothing
    End Enum

    Private WithEvents HScrollBar1 As New System.Windows.Forms.HScrollBar       ' horizontal scrollbar
    Private WithEvents VScrollBar1 As New System.Windows.Forms.VScrollBar       ' vertical scrollbar
    Private WithEvents ScrollBox1 As New System.Windows.Forms.PictureBox        ' filler box between ends of scrollbars

    Private DummySC As New System.Windows.Forms.ScrollableControl
    Public HScrollbar As New MyScrollBarProperties(DummySC, HScrollBar1)
    Public VScrollbar As New MyScrollBarProperties(DummySC, VScrollBar1)

    Private Const MINZOOMPERCEN As Integer = 1  ' minimum zoom percentage (must be > 0)
    Private Const MAXZOOMPERCEN As Integer = 6400 ' maximum zoom percentage

    Private BMP As Bitmap = Nothing             ' image for control

    Private MyFastRenderFormat As Imaging.PixelFormat
    Private MyUseFastRenderFormat As Boolean = True
    Private MyClearColor As Color = Color.White ' color to use when clearing image/selection
    Private MyZoomPercent As Single = 100       ' zoom of image
    Private MyZoomStyle As ZoomStyleEnum = ZoomStyleEnum.FitToNothing
    Private ModRotations As Integer = 0          ' keep track of rotations for unequal x/y resolution files
    Private MyRaisePaintedEvent As Boolean = False  ' raise the painted event

    Private ZoomX As Single = MyZoomPercent / 100   ' fractional zoom for x dimension
    Private ZoomY As Single = MyZoomPercent / 100   ' fractional zoom for y dimension
    Private MyConvertToMonochrome As Boolean = False  ' convert images to black and white on input
    Private MyNormaliseAspectRatio As Boolean = True  ' make the Y DPI the same as the X DPI

    Private ImageHave As Boolean = False        ' flag if have an image
    Private ImageMouse As Point                 ' position of mouse on image
    Private ImageOriginalFormat As Imaging.PixelFormat  ' original format of loaded image
    Private ImageOriginalGDIPalette() As GDI.RGBQUAD ' GDI palette if input image is indexed
    Private ImageSelectX As Integer             ' left pixel of selection area in bitmap
    Private ImageSelectY As Integer             ' top pixel of selection area in bitmap
    Private ImageSelectWidth As Integer         ' width of selection area in bitmap
    Private ImageSelectHeight As Integer        ' height of selection area in bitmap
    Private ImgModified As Boolean = False         ' flag if Image has been modified

    ' Canvas is the zoomed image (all or some will be viewable in the display)
    Private CanvasLeft As Integer               ' left position of canvas
    Private CanvasTop As Integer                ' top position of canvas
    Private CanvasWidth As Integer              ' width of canvas (= image width * zoom)
    Private CanvasHeight As Integer             ' height of canvas (= image height * zoom)
    Private CanvasMouseX As Integer             ' current mouse X (pixel in canvas)
    Private CanvasMouseY As Integer             ' current mouse Y (pixel in canvas)
    Private DisplayWidth As Integer             ' visiable width (account for scrollbars)
    Private DisplayHeight As Integer            ' visible height (accounting for scrollbars)

    ' Selection rectangle data
    Private SelectAllow As Boolean = True       ' allow use of the selection rectangle
    Private SelectHave As Boolean = False       ' do we have a selection rectangle
    Private Selecting As Boolean = False        ' selection rectangle is being drawn
    Private SelectMoving As Boolean = False     ' moving/resizing the selection box
    Private SelectImageXfrom As Integer         ' start of X selection area (in bitmap)
    Private SelectImageYfrom As Integer         ' start of Y selection area (in bitmap)
    Private SelectImageXto As Integer           ' end of X selection area (in bitmap) can be less than start
    Private SelectImageYto As Integer           ' end of Y selection area (in bitmap) can be less than start
    Private SelectCanvasXfrom As Integer        ' start X of selection (pixel in canvas)
    Private SelectCanvasYfrom As Integer        ' start Y of selection (pixel in canvas)
    Private SelectCanvasXto As Integer          ' end X of selection (pixel in canvas)
    Private SelectCanvasYTo As Integer          ' end Y of selection (pixel in canvas)
    Private SelCanvasOffsetX As Integer         ' offset into sizing box (pixels in canvas)
    Private SelCanvasOffsetY As Integer         ' offset into sizing box (pixels in canvas)
    Private SelSizeBoxXfrom(8) As Integer       ' selection sizing box coords (pixels in canvas)
    Private SelSizeBoxYfrom(8) As Integer       '  these are numbered 4 0 5
    Private SelSizeBoxXto(8) As Integer         '                     3   1
    Private SelSizeBoxYto(8) As Integer         '                     7 2 6 and 8 is the whole box
    Private SelSizeBoxNo As Integer             ' selected size box

    ' Paste bitmap
    Private PasteBMP As Bitmap = Nothing
    Private PasteHave As Boolean = False        ' do we have a paste bitmap
    Private PasteMoving As Boolean = False      ' moving the paste bitmap
    Private PasteImageXfrom As Integer          ' X pixel in image where paste will go
    Private PasteImageYfrom As Integer          ' Y pixel in image where paste will go
    Private PasteCanvasXfrom As Integer         ' start X of paste (pixel in canvas)
    Private PasteCanvasYfrom As Integer         ' start Y of paste (pixel in canvas)
    Private PasteCanvasXto As Integer           ' end X of paste (pixel in canvas)
    Private PasteCanvasYto As Integer           ' end Y of paste (pixel in canvas)
    Private PasteCanvasOffsetX As Integer       ' offset into paste image (pixels in canvas)
    Private PasteCanvasOffsetY As Integer       ' offset into paste image (pixels in canvas)
#End Region

#Region " Constructor/Destructor "
    Public Sub New()
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.SuspendLayout()
        Me.DoubleBuffered = True

        Me.Controls.Add(HScrollBar1)
        Me.Controls.Add(VScrollBar1)
        Me.Controls.Add(ScrollBox1)

        ScrollBox1.Visible = False
        ScrollBox1.BackColor = Me.BackColor
        ScrollBox1.TabStop = False
        ScrollBox1.Enabled = False

        ' Get th pixel format of the screen to speed things up
        Dim g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
        Dim B As Bitmap = New Bitmap(1, 1, g)
        g.Dispose()
        MyFastRenderFormat = B.PixelFormat
        B.Dispose()

        Me.ResumeLayout()
    End Sub

    Protected Overrides Sub Finalize()
        If Not (BMP Is Nothing) Then BMP.Dispose()
        If Not (PasteBMP Is Nothing) Then PasteBMP.Dispose()
        MyBase.Finalize()
    End Sub
#End Region

#Region " Events "
    ''' <summary>
    ''' Occurs when the mouse has moved on the image.
    ''' Position of mouse in image.
    ''' </summary>
    <Description("Occurs when the mouse has moved on the image. Position of mouse in image.")> _
    Public Event ImageMouseMove(ByVal P As Point)
    ''' <summary>
    ''' Occurs when the selection rectangle has changed.
    ''' If there is no selection rectangle the returned width is zero.
    ''' </summary>
    <Description("Occurs when the selection rectangle has changed.")> _
    Public Event SelectionChanged(ByVal rect As Rectangle)

    ''' <summary>
    ''' Occurs when the image has been modified.
    ''' </summary>
    <Description("Occurs when the image has been modified.")> _
    Public Event ImageModified()

    ''' <summary>
    ''' May occur when the image is painted.
    ''' Returns the top left pixel of the image paint area.
    ''' </summary>
    <Description("May occur when the image is painted. Returns the top left pixel of the image paint area.")> _
    Public Event Painted(ByVal Left As Integer, ByVal Top As Integer)
#End Region

#Region " Public Properties "
    ''' <summary>
    ''' Get the minimum percentage image zoom.
    ''' </summary>
    <Description("Gets the minimum percentage image zoom.")> _
    Public ReadOnly Property MinZoomPercent() As Integer
        Get
            MinZoomPercent = MINZOOMPERCEN
        End Get
    End Property

    ''' <summary>
    ''' Get the maximum percentage image zoom.
    ''' </summary>
    <Description("Gets the maximum percentage image zoom.")> _
    Public ReadOnly Property MaxZoomPercent() As Integer
        Get
            MaxZoomPercent = MAXZOOMPERCEN
        End Get
    End Property

    ''' <summary>
    ''' Gets the selection rectangle (or a zero width rectangle if there is none).
    ''' </summary>
    <Description("Gets the selection rectangle.")> _
    Public ReadOnly Property SelectionRectangle() As Rectangle
        Get
            If SelectHave Then
                Return New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight)
            Else
                Return New Rectangle(0, 0, 0, 0)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the color to use when clearing the image or selection.
    ''' </summary>
    <Browsable(True), Description("Color to use when clearing the image or selection."), Category("Appearance")> _
    Public Property ClearColor() As Color
        Get
            Return MyClearColor
        End Get
        Set(ByVal value As Color)
            MyClearColor = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the control will be double buffered.
    ''' </summary>
    <Browsable(True), Description("Indicates whether the control will be double buffered."), Category("Behavior")> _
    Public Shadows Property DoubleBuffered() As Boolean
        Get
            Return (MyBase.DoubleBuffered)
        End Get
        Set(ByVal value As Boolean)
            MyBase.DoubleBuffered = value
        End Set
    End Property

    ''' <summary>
    ''' The bitmap format that renders fastest on the display.
    ''' See UseFastRenderFormat.
    ''' </summary>
    <Browsable(True), Description("The bitmap format that renders fastest on the display. See UseFastRenderFormat."), _
    Category("Appearance")> _
    Public Property FastRenderFormat() As Imaging.PixelFormat
        Get
            Return MyFastRenderFormat
        End Get
        Set(ByVal value As Imaging.PixelFormat)
            If MyFastRenderFormat <> value Then
                MyFastRenderFormat = value
                If ImageHave Then
                    If MyUseFastRenderFormat Then
                        ConvertBitmap(BMP, MyFastRenderFormat)
                        If PasteHave Then
                            ConvertBitmap(PasteBMP, MyFastRenderFormat)
                        End If
                    Else
                        If MyConvertToMonochrome Then
                            ConvertBitmap(BMP, Imaging.PixelFormat.Format1bppIndexed)
                            If PasteHave Then
                                ConvertBitmap(PasteBMP, Imaging.PixelFormat.Format1bppIndexed)
                            End If
                        Else
                            ConvertBitmap(BMP, ImageOriginalFormat, ImageOriginalGDIPalette)
                            If PasteHave Then
                                ConvertBitmap(PasteBMP, ImageOriginalFormat, ImageOriginalGDIPalette)
                            End If
                        End If
                    End If
                    Me.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Use the bitmap format specified in FastRenderFormat to display the image.
    ''' </summary>
    <Browsable(True), Description("Use the bitmap format specifeid in FastRenderFormat to display the image."), Category("Appearance")> _
    Public Property UseFastRenderFormat() As Boolean
        Get
            Return MyUseFastRenderFormat
        End Get
        Set(ByVal value As Boolean)
            If MyUseFastRenderFormat <> value Then
                MyUseFastRenderFormat = value
                If ImageHave Then
                    If MyUseFastRenderFormat Then
                        ConvertBitmap(BMP, MyFastRenderFormat)
                        If PasteHave Then
                            ConvertBitmap(PasteBMP, MyFastRenderFormat)
                        End If
                    Else
                        If MyConvertToMonochrome Then
                            ConvertBitmap(BMP, Imaging.PixelFormat.Format1bppIndexed)
                            If PasteHave Then
                                ConvertBitmap(PasteBMP, Imaging.PixelFormat.Format1bppIndexed)
                            End If
                        Else
                            ConvertBitmap(BMP, ImageOriginalFormat, ImageOriginalGDIPalette)
                            If PasteHave Then
                                ConvertBitmap(PasteBMP, ImageOriginalFormat, ImageOriginalGDIPalette)
                            End If
                        End If
                    End If
                    Me.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets forced conversion of image to monochrome.
    ''' </summary>
    <Browsable(True), Description("Force conversion of image to monochrome."), Category("Behavior")> _
    Public Property ConvertToMonochrome() As Boolean
        Get
            Return MyConvertToMonochrome
        End Get
        Set(ByVal value As Boolean)
            If MyConvertToMonochrome <> value Then
                MyConvertToMonochrome = value
                If ImageHave Then
                    If MyConvertToMonochrome Then
                        If BMP.PixelFormat <> Imaging.PixelFormat.Format1bppIndexed Then
                            ConvertBitmap(BMP, Imaging.PixelFormat.Format1bppIndexed)
                            If MyUseFastRenderFormat Then
                                ConvertBitmap(BMP, MyFastRenderFormat)
                            End If
                        End If
                        If PasteHave Then
                            ConvertBitmap(PasteBMP, Imaging.PixelFormat.Format1bppIndexed)
                            If MyUseFastRenderFormat Then
                                ConvertBitmap(PasteBMP, MyFastRenderFormat)
                            End If
                        End If
                        Me.Invalidate()
                    End If
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the image to display.
    ''' </summary>
    <Browsable(True), Description("The image to display."), Category("Appearance")> _
    Public Property Image() As Bitmap
        Get
            If ImageHave Then
                ' Must return the bitmap and note a converted image else ...Image. methods fail
                Return BMP
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal value As Bitmap)
            If SelectHave Then
                RaiseEvent SelectionChanged(New Rectangle(0, 0, 0, 0))
                SelectHave = False
            End If

            Selecting = False
            SelectMoving = False

            ' Don't need to remove the paste bitmap when the image changes
            'If PasteHave Then
            '    PasteBMP = Nothing
            '    PasteHave = False
            'End If
            PasteMoving = False

            BMP = value
            ReDim ImageOriginalGDIPalette(0)
            ImageHave = Not (value Is Nothing)
            If ImageHave Then
                ImageOriginalFormat = BMP.PixelFormat
                ModRotations = 0

                ' Save the palette for returning images
                If (ImageOriginalFormat And Imaging.PixelFormat.Indexed) <> 0 Then
                    GetGDIPalette(BMP, ImageOriginalGDIPalette)
                End If

                ' Convert to black and white
                If MyConvertToMonochrome And BMP.PixelFormat <> Imaging.PixelFormat.Format1bppIndexed Then
                    ConvertBitmap(BMP, Imaging.PixelFormat.Format1bppIndexed)
                End If

                ' Use a format that renders very quickly
                If MyUseFastRenderFormat Then
                    ConvertBitmap(BMP, MyFastRenderFormat)
                End If

                'CanvasLeft = 0
                'CanvasTop = 0
            End If ' ImageHave
            Me.Refresh()
        End Set
    End Property

    ''' <summary>
    ''' Gets the image in a specified format.
    ''' </summary>
    <Browsable(False), Description("The image to display."), Category("Appearance")> _
    Public ReadOnly Property Image(ByVal Destformat As Imaging.PixelFormat) As Bitmap
        Get
            If ImageHave Then
                Dim BMPtmp As New Bitmap(BMP)
                BMPtmp.SetResolution(BMP.HorizontalResolution, BMP.VerticalResolution)
                ConvertBitmap(BMPtmp, Destformat)
                Return BMPtmp
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the selection image
    ''' </summary>
    <Browsable(False), Description("The selection image.")> _
    Public ReadOnly Property SelectionImage() As Bitmap
        Get
            If SelectHave Then
                Dim BMPtmp As Bitmap = GetSafeSelectionBitmap()
                ConvertBitmap(BMPtmp, BMP.PixelFormat)
                Return BMPtmp
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the selection image in a specified format.
    ''' </summary>
    <Browsable(False), Description("The selection image.")> _
    Public ReadOnly Property SelectionImage(ByVal Destformat As Imaging.PixelFormat) As Bitmap
        Get
            If SelectHave Then
                Dim BMPtmp As Bitmap = GetSafeSelectionBitmap()
                ConvertBitmap(BMPtmp, Destformat)
                Return BMPtmp
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether the vertical DPI should equal the horizontal DPI
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(True), Description("Indcates whether the vertical DPI should be set to the horizontal DPI."), Category("Appearance")> _
    Public Property NormaliseAspectRatio() As Boolean
        Get
            Return MyNormaliseAspectRatio
        End Get
        Set(ByVal value As Boolean)
            If MyNormaliseAspectRatio <> value Then
                MyNormaliseAspectRatio = value
                If ImageHave Then
                    AdjustZoom()
                    Me.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether a user can draw a selection rectangle.
    ''' </summary>
    ''' <value></value>
    <Browsable(True), Description("Allow the user to draw a selection rectangle."), Category("Behavior")> _
    Public Property AllowSelect() As Boolean
        Get
            Return SelectAllow
        End Get
        Set(ByVal value As Boolean)
            If SelectAllow <> value Then
                SelectAllow = value
                If Selecting Or SelectMoving Then
                    SelectHave = False
                    SelectMoving = False
                    SelectHave = False
                    Cursor = Parent.Cursor
                    Me.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a flag if the image has been modified.
    ''' </summary>
    <Browsable(False), Description("Gets or sets a flag if the image has been modified.")> _
    Public Property Modified() As Boolean
        Get
            Modified = ImgModified
        End Get
        Set(ByVal value As Boolean)
            ImgModified = value
            If value Then RaiseEvent ImageModified()
        End Set
    End Property

    ''' <summary>
    ''' Maximum percentage zoom.
    ''' </summary>
    <Browsable(False)> _
    Public ReadOnly Property ZoomMaximum() As Single
        Get
            Return MAXZOOMPERCEN
        End Get
    End Property

    ''' <summary>
    ''' Minimum percentage zoom.
    ''' </summary>
    <Browsable(False)> _
    Public ReadOnly Property ZoomMinimum() As Single
        Get
            Return MINZOOMPERCEN
        End Get
    End Property

    ''' <summary>
    ''' Get or sets the zoom behaviour.
    ''' </summary>
    <Browsable(True), Description("Get or sets the zoom behaviour.")> _
      Public Property ZoomStyle() As ZoomStyleEnum
        Get
            Return MyZoomStyle
        End Get
        Set(ByVal value As ZoomStyleEnum)
            If MyZoomStyle <> value Then
                MyZoomStyle = value
                AdjustZoom()
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the percentage zoom of the image.
    ''' Minimum is 4, maximum is 6400.
    ''' </summary>
    <Description("The percentage zoom of the image. Minimum is 4, maximum 6400.")> _
    Public Property Zoom() As Single
        Get
            Return MyZoomPercent
        End Get
        Set(ByVal NewZoomPercent As Single)
            ' Old values (pre zoom change) used to caclulate scroll bar values
            Dim OldHValue As Integer
            Dim OldHMax As Integer
            Dim OldVValue As Integer
            Dim OldVMax As Integer
            Dim OldZoomX As Single
            Dim OldZoomY As Single
            Dim Value As Integer

            NewZoomPercent = Math.Min(Math.Max(NewZoomPercent, MINZOOMPERCEN), MAXZOOMPERCEN)
            If NewZoomPercent <> MyZoomPercent Then
                MyZoomPercent = NewZoomPercent
                If ImageHave Then
                    OldHValue = HScrollBar1.Value
                    OldHMax = HScrollBar1.Maximum
                    OldVValue = VScrollBar1.Value
                    OldVMax = VScrollBar1.Maximum
                    OldZoomX = ZoomX
                    OldZoomY = ZoomY

                    ZoomX = MyZoomPercent / 100
                    ZoomY = NormalisedAspect(ZoomX)

                    AdjustZoom()

                    If MyZoomStyle = ZoomStyleEnum.FitToNothing And (Not Me.DesignMode) Then
                        ' Zoom about a point X,Y in the display
                        Dim x As Single
                        Dim y As Single
                        x = HScrollBar1.Width / 2.0!
                        y = VScrollBar1.Height / 2.0!
                        If HScrollBar1.Visible Then
                            Value = CType((x * (1.0 / OldZoomX - 1.0 / ZoomX) + OldHValue / OldHMax * BMP.Width) * HScrollBar1.Maximum / BMP.Width, Integer)
                            Value = Math.Max(Math.Min(Value, HScrollBar1.Maximum - HScrollBar1.LargeChange + 1), 0)
                            HScrollBar1.Value = Value
                        End If
                        If VScrollBar1.Visible Then
                            Value = CType((y * (1.0 / OldZoomY - 1.0 / ZoomY) + OldVValue / OldVMax * BMP.Height) * VScrollBar1.Maximum / BMP.Height, Integer)
                            Value = Math.Max(Math.Min(Value, VScrollBar1.Maximum - VScrollBar1.LargeChange + 1), 0)
                            VScrollBar1.Value = Value
                        End If
                    Else
                        HScrollBar1.Value = HScrollBar1.Minimum
                        VScrollBar1.Value = VScrollBar1.Minimum
                    End If
                    MyBase.Refresh()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a flag for whether the painted event should be raised
    ''' </summary>
    <Browsable(True), Description("Gets or sets a flag for whether the painted event should be raised")> _
    Public Property RaisePaintedEvent() As Boolean
        Get
            Return MyRaisePaintedEvent
        End Get
        Set(ByVal value As Boolean)
            MyRaisePaintedEvent = value
        End Set
    End Property
#End Region

#Region " Public Methods "
    ''' <summary>
    ''' Refresh the image
    ''' </summary>
    <Description("Refresh the image.")> _
    Public Overrides Sub Refresh()
        AdjustZoom()
        MyBase.Refresh()
    End Sub

    ''' <summary>
    ''' Set the image or selection to the ClearColor.
    ''' </summary>
    <Description("Set the image or selection to the ClearColor.")> _
    Public Sub Clear()
        If ImageHave Or SelectHave Then
            Try
                Dim Converted As Boolean = False        ' flag if have changed bitmap format in order to perform Clear
                Dim OldFormat As Imaging.PixelFormat = BMP.PixelFormat
                Dim brush As Brush = New SolidBrush(MyClearColor)

                ' Check bitmap format will support graphics method Rectangle
                If (BMP.PixelFormat And Imaging.PixelFormat.Indexed) <> 0 Then
                    ConvertBitmap(BMP, Imaging.PixelFormat.Format24bppRgb)
                    Converted = True
                End If
                Dim G As Graphics = Graphics.FromImage(BMP)
                SetHighSpeedGraphics(G, Drawing2D.PixelOffsetMode.None)
                If SelectHave Then
                    G.FillRectangle(brush, New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight))
                Else
                    G.FillRectangle(brush, New Rectangle(0, 0, BMP.Width, BMP.Height))
                End If
                G.Dispose()
                GC.Collect()

                ' Convert bitmap back to original format
                If Converted Then
                    ConvertBitmap(BMP, OldFormat, ImageOriginalGDIPalette)
                End If
                ImgModified = True
                RaiseEvent ImageModified()
                Me.Invalidate()
            Catch ex As Exception
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Copy the image or selection to the clipboard.
    ''' </summary>
    <Description("Copy the image or selection to the clipboard.")> _
    Public Sub Copy()
        If ImageHave Or SelectHave Then
            Try
                Dim BMPtmp As Bitmap
                If SelectHave Then
                    BMPtmp = GetSafeSelectionBitmap()
                Else
                    BMPtmp = New Bitmap(BMP)
                End If
                BMPtmp.SetResolution(BMP.HorizontalResolution, BMP.VerticalResolution)
                If MyConvertToMonochrome Then
                    ConvertBitmap(BMPtmp, Imaging.PixelFormat.Format1bppIndexed)
                Else
                    ConvertBitmap(BMPtmp, ImageOriginalFormat, ImageOriginalGDIPalette)
                End If
                Clipboard.SetImage(BMPtmp)
                BMPtmp.Dispose()
            Catch ex As Exception
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Cut the image or selection.
    ''' </summary>
    <Description("Cut the image or selection.")> _
    Public Sub Cut()
        Copy()
        Clear()
    End Sub

    ''' <summary>
    ''' Invert the image or selection.
    ''' </summary>
    <Description("Invert the image or selection.")> _
    Public Sub Invert()
        If ImageHave Or SelectHave Then
            Try
                If SelectHave Then
                    Dim BMPtmp As Bitmap = GetSafeSelectionBitmap()
                    Dim Converted As Boolean = False        ' flag if have changed bitmap format
                    Dim OldFormat As Imaging.PixelFormat = BMP.PixelFormat

                    InvertBitmap(BMPtmp)

                    ' Check bitmap format will support graphics DrawImage
                    If (BMP.PixelFormat And Imaging.PixelFormat.Indexed) <> 0 Then
                        ConvertBitmap(BMP, Imaging.PixelFormat.Format24bppRgb)
                        Converted = True
                    End If
                    Dim G As Graphics = Graphics.FromImage(BMP)
                    SetHighSpeedGraphics(G, Drawing2D.PixelOffsetMode.Half)
                    G.DrawImage(BMPtmp, ImageSelectX, ImageSelectY)
                    G.Dispose()
                    BMPtmp.Dispose()
                    GC.Collect()

                    ' Convert bitmap back to original format
                    If Converted Then
                        ConvertBitmap(BMP, OldFormat, ImageOriginalGDIPalette)
                    End If
                Else
                    InvertBitmap(BMP) ' InvertBitmap uses GDI and so can cope with indexed bitmaps
                End If

                ImgModified = True
                RaiseEvent ImageModified()
                Me.Invalidate()
            Catch ex As Exception
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try
        End If
    End Sub


    ''' <summary>
    ''' Paste an image from the clipboard/or argument onto the image.
    ''' </summary>
    <Description("Paste an image from the clipboard/or argument onto the image.")> _
    Public Sub Paste(Optional ByVal BMPin As Bitmap = Nothing)
        If ImageHave Then
            If (BMP Is Nothing) Or Clipboard.ContainsImage Then
                Try
                    If Not (BMPin Is Nothing) Then
                        PasteBMP = BMPin
                    Else
                        PasteBMP = CType(Clipboard.GetImage, Bitmap)
                    End If
                    If MyConvertToMonochrome Then
                        ConvertBitmap(PasteBMP, Imaging.PixelFormat.Format1bppIndexed)
                    End If
                    If MyUseFastRenderFormat Then
                        ConvertBitmap(PasteBMP, MyFastRenderFormat)
                    End If
                    PasteCanvasXfrom = 0
                    PasteCanvasYfrom = 0
                    PasteImageXfrom = ConvertCanvasToImageX(PasteCanvasXfrom, False)
                    PasteImageYfrom = ConvertCanvasToImageY(PasteCanvasYfrom, False)
                    PasteCanvasXto = CType(PasteCanvasXfrom + Math.Round(PasteBMP.Width * ZoomX) - 1, Integer)
                    PasteCanvasYto = CType(PasteCanvasYfrom + Math.Round(PasteBMP.Height * ZoomY) - 1, Integer)
                    PasteMoving = False
                    PasteHave = True
                    If SelectHave Then
                        RaiseEvent SelectionChanged(New Rectangle(0, 0, 0, 0))
                        SelectHave = False
                    End If
                    Selecting = False
                    SelectMoving = False

                    ImgModified = True
                    RaiseEvent ImageModified()
                    Me.Invalidate()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Rotate or flip the image or selection.
    ''' </summary>
    <Description("Rotate or flip the image or selection.")> _
    Public Sub RotateFlip(ByVal Action As RotateFlipType)
        If ImageHave Then
            Dim Rot90or270 As Boolean       ' flag if rotated bu 90 or 270 degrees

            Rot90or270 = (Action = RotateFlipType.Rotate90FlipNone Or Action = RotateFlipType.Rotate90FlipX Or Action = RotateFlipType.Rotate90FlipY Or Action = RotateFlipType.Rotate90FlipXY Or _
                          Action = RotateFlipType.Rotate270FlipNone Or Action = RotateFlipType.Rotate270FlipX Or Action = RotateFlipType.Rotate270FlipY Or Action = RotateFlipType.Rotate270FlipXY)

            Try
                If SelectHave And (ImageSelectWidth <> ImageSelectHeight) And Rot90or270 Then
                    Return
                End If

                Dim Converted As Boolean = False
                Dim OldFormat As Imaging.PixelFormat = BMP.PixelFormat

                If (BMP.PixelFormat And Imaging.PixelFormat.Indexed) <> 0 Then
                    ConvertBitmap(BMP, Imaging.PixelFormat.Format24bppRgb)
                    Converted = True
                End If

                If SelectHave Then
                    Dim BMPtmp As Bitmap = GetSafeSelectionBitmap()
                    ' Even though the graphics unit is set to pixel in the DrawImage below if a 90, 270 rotation and
                    ' unequal x/y resolution and a selection then it will not draw to the correct place unless...
                    If Rot90or270 And (BMP.HorizontalResolution <> BMP.VerticalResolution) Then
                        BMPtmp.SetResolution(BMP.VerticalResolution, BMP.HorizontalResolution)
                    End If

                    BMPtmp.RotateFlip(Action)

                    Dim G As Graphics = Graphics.FromImage(BMP)
                    G.PageUnit = GraphicsUnit.Pixel
                    SetHighSpeedGraphics(G, Drawing2D.PixelOffsetMode.Half)
                    G.DrawImage(BMPtmp, ImageSelectX, ImageSelectY, New Rectangle(0, 0, ImageSelectWidth, ImageSelectHeight), GraphicsUnit.Pixel)
                    G.Dispose()
                    BMPtmp.Dispose()
                    GC.Collect()
                Else
                    BMP.RotateFlip(Action)

                    ' Zoom on unequal x/y resolution files is affected by 90, 270 degree rotations
                    If Rot90or270 Then
                        ModRotations = (ModRotations + 1) Mod 2 ' see AdjustZoom
                    End If
                    AdjustZoom()
                End If


                If Converted Then
                    ConvertBitmap(BMP, OldFormat, ImageOriginalGDIPalette)
                End If

                ImgModified = True
                RaiseEvent ImageModified()
                Me.Invalidate()
            Catch ex As Exception
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Select the whole image.
    ''' </summary>
    <Description("Selects the whole image.")> _
    Public Sub SelectAll()
        If ImageHave Then
            SelectImageXfrom = 0
            SelectImageYfrom = 0
            SelectImageXto = BMP.Width - 1
            SelectImageYto = BMP.Height - 1
            SelectMoving = False
            Selecting = False
            SelectHave = True
            ImageSelectX = 0
            ImageSelectY = 0
            ImageSelectWidth = BMP.Width
            ImageSelectHeight = BMP.Height
            RaiseEvent SelectionChanged(New Rectangle(0, 0, BMP.Width, BMP.Height))
            Me.Invalidate()
        End If
    End Sub
#End Region

#Region " Event Handlers "
    Private Sub HScrollBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScrollBar1.ValueChanged
        Try
            CanvasLeft = CType(CType((HScrollBar1.Width - CanvasWidth), Long) * HScrollBar1.Value / (HScrollBar1.Maximum - HScrollBar1.LargeChange + 1), Integer)
            If Selecting Or SelectMoving Then RaiseEvent SelectionChanged(New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight))
            Me.Invalidate()
        Catch
        End Try
    End Sub

    ' Intercept Keystrokes (otherwise they go to the scrollbars)
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        Const WM_KEYDOWN As Integer = &H100
        Const WM_SYSKEYDOWN As Integer = &H104
        Dim Value As Boolean = False

        If ImageHave And (msg.Msg = WM_KEYDOWN) Or (msg.Msg = WM_SYSKEYDOWN) Then
            Value = True
            Select Case (keyData)
                Case Keys.Left
                    If HScrollBar1.Visible Then HScrollBar1.Value = Math.Max(HScrollBar1.Value - HScrollBar1.SmallChange, HScrollBar1.Minimum)
                Case Keys.Right
                    If HScrollBar1.Visible Then HScrollBar1.Value = Math.Min(HScrollBar1.Value + HScrollBar1.SmallChange, HScrollBar1.Maximum - HScrollBar1.LargeChange + 1)
                Case Keys.Down
                    If VScrollBar1.Visible Then VScrollBar1.Value = Math.Min(VScrollBar1.Value + VScrollBar1.SmallChange, VScrollBar1.Maximum - VScrollBar1.LargeChange + 1)
                Case Keys.Up
                    If VScrollBar1.Visible Then VScrollBar1.Value = Math.Max(VScrollBar1.Value - VScrollBar1.SmallChange, VScrollBar1.Minimum)
                Case Keys.PageDown
                    If VScrollBar1.Visible Then VScrollBar1.Value = Math.Min(VScrollBar1.Value + VScrollBar1.LargeChange, VScrollBar1.Maximum - VScrollBar1.LargeChange + 1)
                Case Keys.PageUp
                    If VScrollBar1.Visible Then VScrollBar1.Value = Math.Max(VScrollBar1.Value - VScrollBar1.LargeChange, VScrollBar1.Minimum)
                Case Keys.Escape
                    If PasteHave Or PasteMoving Then
                        Cursor = Parent.Cursor
                        PasteBMP.Dispose()
                        PasteBMP = Nothing
                        PasteHave = False
                        PasteMoving = False
                        Me.Invalidate()
                    End If
                    If SelectHave Or Selecting Or SelectMoving Then
                        Cursor = Parent.Cursor
                        SelectHave = False
                        Selecting = False
                        SelectMoving = False
                        RaiseEvent SelectionChanged(New Rectangle(0, 0, 0, 0))
                        Me.Invalidate()
                    End If
                Case Else
                    If PasteHave Then
                        PasteBitmap()
                    Else
                        Value = False
                    End If
            End Select
        End If
        Return Value
    End Function

    ' ScrollBox1 is the filler box between ends of the scrollbars
    Private Sub EditableImage_BackColorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.BackColorChanged
        ScrollBox1.BackColor = Me.BackColor
    End Sub

    Private Sub EditableImage_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Dim BMPTmp As Bitmap

        Try
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim MyFiles() As String

                ' Assign the files to an array.
                MyFiles = CType(e.Data.GetData(DataFormats.FileDrop), String())
                BMPTmp = New Bitmap(MyFiles(0))
                Me.Image = BMPTmp
            End If
        Catch ex As Exception
            MessageBox.Show("Unknown file type", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub EditableImage_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub EditableImage_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        Dim SizNo As Integer            ' loop counter
        Dim ZoomCompX As Integer        ' half a zoomed box in X
        Dim ZoomCompY As Integer        ' half a zoomed box in Y

        CanvasMouseX = e.X
        CanvasMouseY = e.Y

        Me.Focus()  ' doesn't happen by default

        If ImageHave And e.Button = Windows.Forms.MouseButtons.Left _
            And ConvertCanvasToImageX(e.X, False) < BMP.Width And ConvertCanvasToImageY(e.Y, False) < BMP.Height Then

            Selecting = False
            SelectMoving = False
            If SelectHave Then
                ' Just a double check we are sizing the selection box
                For SizNo = 0 To 8
                    If e.X <= SelSizeBoxXto(SizNo) And e.X >= SelSizeBoxXfrom(SizNo) And e.Y <= SelSizeBoxYto(SizNo) And e.Y >= SelSizeBoxYfrom(SizNo) Then
                        ' because the user can click anywhere in a sizing box (or the selection area) you need
                        ' to calculate an offset to avoid the selection area jumping
                        ' the ZoomComps just add half a box to the offset when the zoom is greater than 100%
                        ' so that moving left/right and up/down are consistent

                        ZoomCompX = CType(Math.Floor(ZoomX / 2), Integer)
                        ZoomCompY = CType(Math.Floor(ZoomY / 2), Integer)

                        If SelSizeBoxNo = 8 Then
                            SelCanvasOffsetX = SelSizeBoxXfrom(8) - e.X + ZoomCompX
                            SelCanvasOffsetY = SelSizeBoxYfrom(8) - e.Y + ZoomCompY
                        Else
                            Select Case SelSizeBoxNo
                                Case 0 ' top mid
                                    SelCanvasOffsetY = +ZoomCompY
                                Case 1 ' mid right
                                    SelCanvasOffsetX = -ZoomCompX
                                Case 2 ' bottom mid
                                    SelCanvasOffsetY = -ZoomCompY
                                Case 3 ' mid left
                                    SelCanvasOffsetX = +ZoomCompX
                                Case 4 ' top left
                                    SelCanvasOffsetX = ZoomCompX
                                    SelCanvasOffsetY = ZoomCompY
                                Case 5 ' top right
                                    SelCanvasOffsetX = -ZoomCompX
                                    SelCanvasOffsetY = +ZoomCompY
                                Case 6 ' bottom right
                                    SelCanvasOffsetX = -ZoomCompX
                                    SelCanvasOffsetY = -ZoomCompY
                                Case 7 ' bottom left
                                    SelCanvasOffsetX = +ZoomCompX
                                    SelCanvasOffsetY = -ZoomCompY
                            End Select ' SelSizeBoxNo
                            SelCanvasOffsetX = CType(SelCanvasOffsetX + (SelSizeBoxXfrom(SelSizeBoxNo) + SelSizeBoxXto(SelSizeBoxNo)) / 2 - e.X, Integer)
                            SelCanvasOffsetY = CType(SelCanvasOffsetY + (SelSizeBoxYfrom(SelSizeBoxNo) + SelSizeBoxYto(SelSizeBoxNo)) / 2 - e.Y, Integer)
                        End If

                        SelectImageXfrom = ConvertCanvasToImageX(SelSizeBoxXfrom(8), True)
                        SelectImageYfrom = ConvertCanvasToImageY(SelSizeBoxYfrom(8), True)
                        ' At odd ImageZooms (150% say) the selection area will never match the image
                        ' and there is a tendency to bounce
                        If MyZoomPercent > 100 Then
                            SelectImageXto = CType(SelectImageXfrom + (SelSizeBoxXto(8) - SelSizeBoxXfrom(8) + 1) / ZoomX - 1, Integer)
                            SelectImageYto = CType(SelectImageYfrom + (SelSizeBoxYto(8) - SelSizeBoxYfrom(8) + 1) / ZoomY - 1, Integer)
                        Else
                            SelectImageXto = ConvertCanvasToImageX(SelSizeBoxXto(8), True)
                            SelectImageYto = ConvertCanvasToImageY(SelSizeBoxYto(8), True)
                        End If
                        ImageSelectX = Math.Min(SelectImageXfrom, SelectImageXto)
                        ImageSelectY = Math.Min(SelectImageYfrom, SelectImageYto)
                        ImageSelectWidth = Math.Abs(SelectImageXto - SelectImageXfrom) + 1
                        ImageSelectHeight = Math.Abs(SelectImageYto - SelectImageYfrom) + 1

                        SelSizeBoxNo = SizNo
                        SelectMoving = True
                        Exit For

                    End If
                Next SizNo
            End If ' SelectHave

            If PasteHave Then
                If e.X <= PasteCanvasXto And e.X >= PasteCanvasXfrom And e.Y <= PasteCanvasYto And e.Y >= PasteCanvasYfrom Then
                    PasteMoving = True
                    ZoomCompX = CType(Math.Floor(ZoomX / 2), Integer)
                    ZoomCompY = CType(Math.Floor(ZoomY / 2), Integer)
                    PasteCanvasOffsetX = PasteCanvasXfrom - e.X + ZoomCompX
                    PasteCanvasOffsetY = PasteCanvasYfrom - e.Y + ZoomCompY
                End If
            End If

            If SelectAllow And Not (SelectMoving Or PasteMoving) Then
                SelectHave = False
                Selecting = False
                If PasteHave Then
                    PasteBitmap()
                Else ' start selecting
                    MyBase.Refresh()
                    Selecting = True
                    SelectImageXfrom = ConvertCanvasToImageX(e.X, True)
                    SelectImageYfrom = ConvertCanvasToImageY(e.Y, True)
                    SelectImageXto = SelectImageXfrom
                    SelectImageYto = SelectImageYfrom
                    ImageSelectX = Math.Min(SelectImageXfrom, SelectImageXto)
                    ImageSelectY = Math.Min(SelectImageYfrom, SelectImageYto)
                    ImageSelectWidth = Math.Abs(SelectImageXto - SelectImageXfrom) + 1
                    ImageSelectHeight = Math.Abs(SelectImageYto - SelectImageYfrom) + 1
                    SelectCanvasXfrom = e.X ' so a single click will erase the selection area
                    SelectCanvasYfrom = e.Y
                End If
            End If

            If Selecting Or SelectMoving Then RaiseEvent SelectionChanged(New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight))
        End If ' ImageHave And e.Button = Windows.Forms.MouseButtons.Left and ConvertCanvasToImageX(e.X, False) < BMP.Width And ...

    End Sub

    Private Sub EditableImage_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseWheel
        If VScrollBar1.Visible Then VScrollBar1.Value = Math.Min(Math.Max(VScrollBar1.Value - VScrollBar1.SmallChange * CInt(e.Delta / 120), VScrollBar1.Minimum), VScrollBar1.Maximum - VScrollBar1.LargeChange + 1)
    End Sub

    Private Sub EditableImage_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If ImageHave Then
            ImageMouse.X = ConvertCanvasToImageX(e.X, False)
            ImageMouse.Y = ConvertCanvasToImageY(e.Y, False)

            If ImageMouse.X >= 0 And ImageMouse.X < BMP.Width And ImageMouse.Y >= 0 And ImageMouse.Y < BMP.Height Then
                RaiseEvent ImageMouseMove(ImageMouse)
            End If

            If e.Button = Windows.Forms.MouseButtons.Left And (Selecting Or SelectMoving Or PasteMoving) Then
                Dim EnableTimerX As Boolean
                Dim EnableTimerY As Boolean

                MoveUpdate(e.X, e.Y)
                MyBase.Refresh()
                EnableTimerX = (e.X < 0) Or (e.X >= DisplayWidth)
                EnableTimerY = (e.Y < 0) Or (e.Y >= DisplayHeight)

                If (HScrollBar1.Visible And EnableTimerX) Or _
                   (VScrollBar1.Visible And EnableTimerY) Then
                    ScrollTimer.Enabled = True
                End If
            End If

            If SelectHave Or PasteHave Then ResetMousePointer(e.X, e.Y)
            If Selecting Or SelectMoving Then RaiseEvent SelectionChanged(New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight))
        End If ' ImageHave

        CanvasMouseX = e.X
        CanvasMouseY = e.Y
    End Sub

    Private Sub EditableImage_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        If ImageHave And e.Button = Windows.Forms.MouseButtons.Left Then
            If SelectHave Or Selecting Or SelectMoving Or PasteMoving Then
                ScrollTimer.Enabled = False

                MoveUpdate(e.X, e.Y)

                ' Must have something selected
                ' A straight click will clear the selection area

                If Selecting Then
                    SelectHave = Not ((SelectCanvasXfrom = SelectCanvasXto) And (SelectCanvasYfrom = SelectCanvasYTo))
                End If

                Selecting = False
                If SelectHave Then
                    RaiseEvent SelectionChanged(New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight))
                Else
                    RaiseEvent SelectionChanged(New Rectangle(0, 0, 0, 0))
                End If
                Me.Invalidate(False)

                SelectMoving = False
                PasteMoving = False
            End If
            ResetMousePointer(e.X, e.Y)
        End If
    End Sub

    Private Sub EditableImage_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Dim g As Graphics = e.Graphics
        Dim Left As Integer     ' left position in bitmap
        Dim Top As Integer      ' top position in bitmap
        Dim pen As Pen          ' used for selection and paste rectangles

        g.PageUnit = GraphicsUnit.Pixel
        SetHighSpeedGraphics(g, Drawing2D.PixelOffsetMode.Half)

        If Me.DesignMode And Not ImageHave Then
            pen = New Pen(Color.Black)
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.None
            g.DrawRectangle(pen, New Rectangle(0, 0, Me.Width - 1, Me.Height - 1))

            Dim fnt As New Font("Verdana", 8)
            Dim StringSize As SizeF = g.MeasureString(Me.Name, fnt)
            g.DrawString(Me.Name, fnt, New SolidBrush(Color.Black), (Me.Width - StringSize.Width) / 2, (Me.Height - StringSize.Height) / 2)
            pen.Dispose()
            Return
        End If

        If Not ImageHave Then
            g.Clear(Me.BackColor)
        Else
            Left = CType(Math.Floor(HScrollBar1.Value / (HScrollBar1.Maximum) * BMP.Width - 0.0), Integer)
            Top = CType(VScrollBar1.Value / VScrollBar1.Maximum * BMP.Height, Integer)

            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            ' At extremely small ImageZooms the src rectangle can get very large which causes GDI to fail
            g.DrawImage(BMP, New RectangleF(0, 0, DisplayWidth, DisplayHeight), New RectangleF(Left, Top, Math.Min(32767, DisplayWidth / ZoomX), Math.Min(32767, DisplayHeight / ZoomY)), GraphicsUnit.Pixel)
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.None

            If Selecting Or SelectHave Or SelectMoving Then
                Dim SelRectL As Integer     ' left of selection rectangle
                Dim SelRectR As Integer     ' right of selection rectangle
                Dim SelRectT As Integer     ' top of selection rectangle
                Dim SelRectB As Integer     ' bottom of selection rectangle
                Dim HSize As Integer        ' half size of a sizing box
                Dim XHalf As Integer        ' horizontal midpoint of selection rectangle
                Dim YHalf As Integer        ' vertical midpoint of selection rectangle

                SelRectL = ImageSelectX
                SelRectR = ImageSelectX + ImageSelectWidth - 1

                SelRectL = ConvertImageToCanvasX(SelRectL)
                If ZoomX <= 1 Then
                    SelRectR = ConvertImageToCanvasX(SelRectR)
                Else
                    SelRectR = CType(Math.Round((SelRectR + 1 - Math.Round(-CanvasLeft / ZoomX)) * ZoomX) - 1, Integer)
                End If

                SelRectT = ImageSelectY
                SelRectB = ImageSelectY + ImageSelectHeight - 1

                SelRectT = ConvertImageToCanvasY(SelRectT)
                If ZoomY <= 1 Then
                    SelRectB = ConvertImageToCanvasY(SelRectB)
                Else
                    SelRectB = CType(Math.Round((SelRectB + 1 - Math.Round(-CanvasTop / ZoomY)) * ZoomY) - 1, Integer)
                End If

                If Selecting Then
                    pen = New Pen(Color.Black)
                    pen.DashStyle = Drawing2D.DashStyle.Dot
                    If SelRectR - SelRectL <= 0 Then
                        g.DrawLine(pen, SelRectL, SelRectT, SelRectL, SelRectB)
                    ElseIf SelRectB - SelRectT < 0 Then
                        g.DrawLine(pen, SelRectL, SelRectT, SelRectR, SelRectT)
                    Else
                        g.DrawRectangle(pen, New Rectangle(SelRectL, SelRectT, SelRectR - SelRectL, SelRectB - SelRectT))
                    End If
                Else
                    ' reset poisition of size boxes
                    If (SelRectR - SelRectL - 1) / 12 > 1 And (SelRectB - SelRectT - 1) / 12 > 1 Then
                        HSize = 2
                    Else
                        HSize = 1
                    End If

                    XHalf = CType((SelRectL + SelRectR) / 2, Integer)
                    YHalf = CType((SelRectT + SelRectB) / 2, Integer)

                    SelSizeBoxXfrom(0) = XHalf - HSize      ' top mid
                    SelSizeBoxYfrom(0) = SelRectT - HSize
                    SelSizeBoxXto(0) = XHalf + HSize
                    SelSizeBoxYto(0) = SelRectT + HSize
                    SelSizeBoxXfrom(1) = SelRectR - HSize   ' mid right
                    SelSizeBoxYfrom(1) = YHalf - HSize
                    SelSizeBoxXto(1) = SelRectR + HSize
                    SelSizeBoxYto(1) = YHalf + HSize
                    SelSizeBoxXfrom(2) = XHalf - HSize      ' bottom mid
                    SelSizeBoxYfrom(2) = SelRectB - HSize
                    SelSizeBoxXto(2) = XHalf + HSize
                    SelSizeBoxYto(2) = SelRectB + HSize
                    SelSizeBoxXfrom(3) = SelRectL - HSize   ' mid left
                    SelSizeBoxYfrom(3) = YHalf - HSize
                    SelSizeBoxXto(3) = SelRectL + HSize
                    SelSizeBoxYto(3) = YHalf + HSize
                    SelSizeBoxXfrom(4) = SelRectL - HSize   ' top left
                    SelSizeBoxYfrom(4) = SelRectT - HSize
                    SelSizeBoxXto(4) = SelRectL + HSize
                    SelSizeBoxYto(4) = SelRectT + HSize
                    SelSizeBoxXfrom(5) = SelRectR - HSize   ' top right
                    SelSizeBoxYfrom(5) = SelRectT - HSize
                    SelSizeBoxXto(5) = SelRectR + HSize
                    SelSizeBoxYto(5) = SelRectT + HSize
                    SelSizeBoxXfrom(6) = SelRectR - HSize   ' bottom right
                    SelSizeBoxYfrom(6) = SelRectB - HSize
                    SelSizeBoxXto(6) = SelRectR + HSize
                    SelSizeBoxYto(6) = SelRectB + HSize
                    SelSizeBoxXfrom(7) = SelRectL - HSize   ' bottom left
                    SelSizeBoxYfrom(7) = SelRectB - HSize
                    SelSizeBoxXto(7) = SelRectL + HSize
                    SelSizeBoxYto(7) = SelRectB + HSize
                    ' Point 8 is to help determine if in the selection rectangle
                    SelSizeBoxXfrom(8) = SelRectL
                    SelSizeBoxYfrom(8) = SelRectT
                    SelSizeBoxXto(8) = SelRectR
                    SelSizeBoxYto(8) = SelRectB

                    pen = New Pen(SystemColors.Highlight)
                    pen.DashStyle = Drawing2D.DashStyle.Dash
                    If SelRectR - SelRectL <= 0 Then
                        g.DrawLine(pen, SelRectL, SelRectT, SelRectL, SelRectB)
                    ElseIf SelRectB - SelRectT < 0 Then
                        g.DrawLine(pen, SelRectL, SelRectT, SelRectR, SelRectT)
                    Else
                        g.DrawRectangle(pen, New Rectangle(SelRectL, SelRectT, SelRectR - SelRectL, SelRectB - SelRectT))
                    End If
                    Dim I As Integer
                    pen.DashStyle = Drawing2D.DashStyle.Solid
                    For I = 0 To 7
                        g.DrawRectangle(pen, New Rectangle(SelSizeBoxXfrom(I), SelSizeBoxYfrom(I), SelSizeBoxXto(I) - SelSizeBoxXfrom(I), SelSizeBoxYto(I) - SelSizeBoxYfrom(I)))
                    Next
                End If
            End If

            If PasteHave Then
                PasteCanvasXfrom = ConvertImageToCanvasX(PasteImageXfrom)
                PasteCanvasYfrom = ConvertImageToCanvasY(PasteImageYfrom)
                'these two so that whatever caused a paint (maybe zoom changed) then the paste bitmap is in step
                PasteCanvasXto = CType(PasteCanvasXfrom + Math.Round(PasteBMP.Width * ZoomX) - 1, Integer)
                PasteCanvasYto = CType(PasteCanvasYfrom + Math.Round(PasteBMP.Height * ZoomY) - 1, Integer)

                g.SetClip(New Rectangle(CanvasLeft, CanvasTop, CanvasWidth, CanvasHeight))

                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                g.DrawImage(PasteBMP, New RectangleF(PasteCanvasXfrom, PasteCanvasYfrom, PasteBMP.Width * ZoomX, PasteBMP.Height * ZoomY), New RectangleF(0, 0, PasteBMP.Width, PasteBMP.Height), GraphicsUnit.Pixel)
                pen = New Pen(Color.Black)
                pen.DashStyle = Drawing2D.DashStyle.Dash
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.None
                g.DrawRectangle(pen, New Rectangle(PasteCanvasXfrom, PasteCanvasYfrom, CType(PasteBMP.Width * ZoomX, Integer) - 1, CType(PasteBMP.Height * ZoomY, Integer) - 1))
            End If
        End If

        If MyRaisePaintedEvent Then
            RaiseEvent Painted(Left, Top)
        End If
    End Sub


    Private Sub EditableImage_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If ImageHave Then
            Me.SuspendLayout() ' else clientsize can change whilst this procedure runs
            AdjustZoom()
            Me.ResumeLayout()
            Me.Invalidate()
        Else
            AdjustScrollbars()
        End If
    End Sub

    ' The scroll timer is used to continue scrolling when the selection rectangle drags outside the editable image and the
    ' user stops moving the cursor.  It also changes the scroll acceleration.
    Private Sub ScrollTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrollTimer.Tick
        Const DECELERATE As Integer = 2     ' deceleration factor for scrolling

        Dim Value As Integer                ' utility value
        Dim NewHsbVal As Integer            ' new value for horizontal scrollbar
        Dim NewVsbVal As Integer            ' new value for vertical scrollbar
        Dim PixelInc As Integer             ' increment to a scrollbar to achive a pixel move
        Dim IncWay As Integer               ' whether to increment or decrement a scrollbar

        If Not (Selecting Or SelectMoving Or PasteMoving) Then
            ScrollTimer.Enabled = False
            Return
        End If

        Try
            NewHsbVal = HScrollBar1.Value
            NewVsbVal = VScrollBar1.Value

            If HScrollBar1.Visible Then

                ' Decide if going to change the scrollbar by setting IncWay to +1 or -1

                IncWay = 0
                If CanvasMouseX < 0 And HScrollBar1.Value > HScrollBar1.Minimum Then
                    Value = 0 ' for acceleration
                    IncWay = -1
                Else
                    If CanvasMouseX >= DisplayWidth And HScrollBar1.Value < HScrollBar1.Maximum - HScrollBar1.LargeChange Then
                        Value = DisplayWidth
                        IncWay = 1
                    End If
                End If

                If IncWay <> 0 Then

                    ' Calculate the increment required for a 1 pixel shift

                    PixelInc = CType((HScrollBar1.Maximum - HScrollBar1.LargeChange - HScrollBar1.Minimum + 1) / (CanvasWidth - HScrollBar1.Width), Integer)
                    If PixelInc < 1 Then PixelInc = 1

                    ' Accelerate the increment

                    PixelInc = CType(PixelInc * (Math.Floor(Math.Abs(CanvasMouseX - Value) / DECELERATE) + 1), Integer)

                    Value = Math.Min(Math.Max(HScrollBar1.Value + PixelInc * IncWay, HScrollBar1.Minimum), HScrollBar1.Maximum - HScrollBar1.LargeChange)
                    NewHsbVal = Value
                End If
            End If

            If VScrollBar1.Visible Then

                ' Decide if going to change the scrollbar by setting IncWay to +1 or -1

                IncWay = 0
                If CanvasMouseY < 0 And VScrollBar1.Value > VScrollBar1.Minimum Then
                    Value = 0
                    IncWay = -1
                Else
                    If CanvasMouseY >= DisplayHeight And VScrollBar1.Value < VScrollBar1.Maximum - VScrollBar1.LargeChange Then
                        Value = DisplayHeight
                        IncWay = 1
                    End If
                End If

                If IncWay <> 0 Then

                    ' Calculate the increment required for a 1 pixel shift

                    PixelInc = CType((VScrollBar1.Maximum - VScrollBar1.LargeChange - VScrollBar1.Minimum + 1) / (CanvasHeight - VScrollBar1.Height), Integer)
                    If PixelInc < 1 Then PixelInc = 1

                    ' Accelerate the increment

                    PixelInc = CType(PixelInc * (Math.Floor(Math.Abs(CanvasMouseY - Value) / DECELERATE) + 1), Integer)

                    Value = Math.Min(Math.Max(VScrollBar1.Value + PixelInc * IncWay, VScrollBar1.Minimum), VScrollBar1.Maximum - VScrollBar1.LargeChange)
                    NewVsbVal = Value
                End If
            End If

            ' Ensure only one paint event

            If NewHsbVal <> HScrollBar1.Value Or NewVsbVal <> VScrollBar1.Value Then
                Me.SuspendLayout()
                HScrollBar1.Value = NewHsbVal
                VScrollBar1.Value = NewVsbVal

                If SelectHave Or PasteHave Or Selecting Then
                    MoveUpdate(CanvasMouseX, CanvasMouseY)
                End If

                Me.Invalidate()
                Me.ResumeLayout()
            End If
        Catch
        End Try
    End Sub


    Private Sub VScrollBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VScrollBar1.ValueChanged
        Try
            CanvasTop = CType(CType((VScrollBar1.Height - CanvasHeight), Long) * (VScrollBar1.Value - VScrollBar1.Minimum) / (VScrollBar1.Maximum - VScrollBar1.LargeChange), Integer)
            If Selecting Or SelectMoving Then RaiseEvent SelectionChanged(New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight))
            Me.Invalidate()
        Catch
        End Try
    End Sub
#End Region

#Region " Private routines "
    Private Sub AdjustScrollbars()
        Dim HVis As Boolean     ' flag if to make horizontal scrollbar visible
        Dim VVis As Boolean     ' flag if to make vertical scrollbar visible
        Dim Value As Double     ' utility

        Me.SuspendLayout()
        If ImageHave And Me.Width > 0 And Me.Height > 0 Then
            HVis = (Math.Round(BMP.Width * ZoomX) > Me.ClientSize.Width)
            VVis = (Math.Round(BMP.Height * ZoomY) > Me.ClientSize.Height)
            If HVis And Not VVis Then
                VVis = (Math.Round(BMP.Height * ZoomY) > (Me.ClientSize.Height - HScrollBar1.Height))
            End If
            If VVis And Not HVis Then
                HVis = (Math.Round(BMP.Width * ZoomX) > (Me.ClientSize.Width - VScrollBar1.Width))
            End If
            If HVis Then
                HScrollBar1.Location = New Point(0, Me.ClientSize.Height - HScrollBar1.Height)
                If VVis Then
                    HScrollBar1.Size = New Size(Me.ClientSize.Width - VScrollBar1.Width, HScrollBar1.Height)
                Else
                    HScrollBar1.Size = New Size(Me.ClientSize.Width, HScrollBar1.Height)
                End If
                Value = Math.Min(BMP.Width * ZoomX, Integer.MaxValue - 1)
                HScrollBar1.Minimum = 0
                HScrollBar1.Maximum = CType(Value, Integer)
                HScrollBar1.LargeChange = Math.Max(0, HScrollBar1.Width)
                HScrollBar1.SmallChange = CType(Math.Max(4, ZoomX), Integer)
                Value = (1 - HScrollBar1.Value / HScrollBar1.Maximum) * BMP.Width * ZoomX
                If Value < HScrollBar1.Width Then
                    HScrollBar1.Value = HScrollBar1.Maximum - HScrollBar1.LargeChange + 1
                End If
            End If
            If VVis Then
                VScrollBar1.Location = New Point(Me.ClientSize.Width - VScrollBar1.Width, 0)
                If HVis Then
                    VScrollBar1.Size = New Size(VScrollBar1.Width, Me.ClientSize.Height - HScrollBar1.Height)
                Else
                    VScrollBar1.Size = New Size(VScrollBar1.Width, Me.ClientSize.Height)
                End If
            End If
            Value = Math.Min(BMP.Height * ZoomY, Integer.MaxValue - 1)
            VScrollBar1.Minimum = 0
            VScrollBar1.Maximum = CType(Value, Integer)
            VScrollBar1.LargeChange = Math.Max(0, VScrollBar1.Height)
            VScrollBar1.SmallChange = CType(Math.Max(4, ZoomY), Integer)
            Value = (1 - VScrollBar1.Value / VScrollBar1.Maximum) * BMP.Height * ZoomY
            If Value < VScrollBar1.Height Then
                VScrollBar1.Value = VScrollBar1.Maximum - VScrollBar1.LargeChange + 1
            End If
        Else
            HVis = False
            VVis = False
        End If ' HaveImage

        If VVis Then
            DisplayWidth = Me.ClientSize.Width - VScrollBar1.Width
        Else
            DisplayWidth = Me.ClientSize.Width
            VScrollBar1.Value = VScrollBar1.Minimum
        End If

        If HVis Then
            DisplayHeight = Me.ClientSize.Height - HScrollBar1.Height
        Else
            DisplayHeight = Me.ClientSize.Height
            HScrollBar1.Value = HScrollBar1.Minimum
        End If

        HScrollBar1.Visible = HVis
        VScrollBar1.Visible = VVis
        ScrollBox1.Visible = HVis And VVis
        If ScrollBox1.Visible Then
            ScrollBox1.Location = New Point(HScrollBar1.Width, VScrollBar1.Height)
            ScrollBox1.Size = New Size(VScrollBar1.Width, HScrollBar1.Height)
        End If
        Me.ResumeLayout()
    End Sub

    Private Sub AdjustZoom()
        Dim NewZoomX As Single
        Dim NewZoomY As Single

        If ImageHave Then
            Select Case MyZoomStyle
                Case ZoomStyleEnum.FitToWidth
                    NewZoomX = CType(Me.ClientSize.Width / BMP.Width, Single)
                    ZoomX = NewZoomX
                    ZoomY = NormalisedAspect(NewZoomX)
                    If Math.Round(BMP.Height * ZoomY) > Me.ClientSize.Height Then
                        NewZoomX = CType((Me.ClientSize.Width - VScrollBar1.Width) / BMP.Width, Single)
                        NewZoomY = DeNormalisedAspect(CType((Me.ClientSize.Height + 0.4999) / BMP.Height, Single))
                        If NewZoomY > NewZoomX Then NewZoomX = NewZoomY
                        ZoomX = NewZoomX
                        ZoomY = NormalisedAspect(NewZoomX)
                    End If
                Case ZoomStyleEnum.FitToHeight
                    NewZoomY = CType(Me.ClientSize.Height / BMP.Height, Single)
                    ZoomX = DeNormalisedAspect(NewZoomY)
                    ZoomY = NewZoomY
                    If Math.Round(BMP.Width * ZoomX) > Me.ClientSize.Width Then
                        NewZoomY = CType((Me.ClientSize.Height - HScrollBar1.Height) / BMP.Height, Single)
                        NewZoomX = NormalisedAspect(CType((Me.ClientSize.Width + 0.4999) / BMP.Width, Single))
                        If NewZoomX > NewZoomY Then NewZoomY = NewZoomX
                        ZoomX = DeNormalisedAspect(NewZoomY)
                        ZoomY = NewZoomY
                    End If
                Case ZoomStyleEnum.FitToDisplay
                    NewZoomX = CType(Me.ClientSize.Width / BMP.Width, Single)
                    NewZoomY = CType(Me.ClientSize.Height / BMP.Height, Single)
                    If NewZoomX < DeNormalisedAspect(NewZoomY) Then
                        ZoomX = NewZoomX
                        ZoomY = NormalisedAspect(NewZoomX)
                    Else
                        ZoomY = NewZoomY
                        ZoomX = DeNormalisedAspect(ZoomY)
                    End If
                Case Else
                    If ModRotations = 0 Then
                        ZoomX = MyZoomPercent / 100
                        ZoomY = NormalisedAspect(ZoomX)
                    Else
                        ZoomY = MyZoomPercent / 100
                        ZoomX = DeNormalisedAspect(ZoomY)
                    End If
            End Select
            CanvasWidth = CType(BMP.Width * ZoomX, Integer)
            CanvasHeight = CType(BMP.Height * ZoomY, Integer)
            AdjustScrollbars()
        End If
    End Sub

    Private Function ConvertCanvasToImageX(ByVal X As Integer, ByVal Limit As Boolean) As Integer
        Dim Value As Integer    ' utility

        Value = CType(Math.Round(-CanvasLeft / ZoomX) + Math.Floor(X / ZoomX), Integer)
        If Limit Then
            Value = Math.Min(Math.Max(Value, 0), BMP.Width - 1)
        End If
        Return Value
    End Function

    Private Function ConvertCanvasToImageY(ByVal Y As Integer, ByVal Limit As Boolean) As Integer
        Dim Value As Integer    ' utility

        Value = CType(Math.Round(-CanvasTop / ZoomY) + Math.Floor(Y / ZoomY), Integer)
        If Limit Then
            Value = Math.Min(Math.Max(Value, 0), BMP.Height - 1)
        End If
        Return Value
    End Function

    Private Function ConvertImageToCanvasX(ByVal ImgX As Integer) As Integer
        Return CType(Math.Round((ImgX - Math.Round(-CanvasLeft / ZoomX)) * ZoomX), Integer)
    End Function

    Private Function ConvertImageToCanvasY(ByVal ImgY As Integer) As Integer
        Return CType(Math.Round((ImgY - Math.Round(-CanvasTop / ZoomY)) * ZoomY), Integer)
    End Function

    ' Moves/sizes the selection rectangle (if any)
    ' Moves the paste image (if any)
    Private Sub MoveUpdate(ByVal X As Integer, ByVal Y As Integer)
        Dim Wid As Integer      ' a width
        Dim Ht As Integer       ' an height

        If SelectMoving Then
            Select Case SelSizeBoxNo
                Case 0 ' top mid
                    SelectCanvasYfrom = Y + SelCanvasOffsetY
                    SelectImageYfrom = ConvertCanvasToImageY(SelectCanvasYfrom, True)
                Case 1 ' mid right
                    SelectCanvasXto = X + SelCanvasOffsetX
                    SelectImageXto = ConvertCanvasToImageX(SelectCanvasXto, True)
                Case 2 ' bottom mid
                    SelectCanvasYTo = Y + SelCanvasOffsetY
                    SelectImageYto = ConvertCanvasToImageY(SelectCanvasYTo, True)
                Case 3 ' mid left
                    SelectCanvasXfrom = X + SelCanvasOffsetX
                    SelectImageXfrom = ConvertCanvasToImageX(SelectCanvasXfrom, True)
                Case 4 ' top left
                    SelectCanvasXfrom = X + SelCanvasOffsetX
                    SelectCanvasYfrom = Y + SelCanvasOffsetY
                    SelectImageXfrom = ConvertCanvasToImageX(SelectCanvasXfrom, True)
                    SelectImageYfrom = ConvertCanvasToImageY(SelectCanvasYfrom, True)
                Case 5 ' top right
                    SelectCanvasXto = X + SelCanvasOffsetX
                    SelectCanvasYfrom = Y + SelCanvasOffsetY
                    SelectImageXto = ConvertCanvasToImageX(SelectCanvasXto, True)
                    SelectImageYfrom = ConvertCanvasToImageY(SelectCanvasYfrom, True)
                Case 6 ' bottom right
                    SelectCanvasXto = X + SelCanvasOffsetX
                    SelectCanvasYTo = Y + SelCanvasOffsetY
                    SelectImageXto = ConvertCanvasToImageX(SelectCanvasXto, True)
                    SelectImageYto = ConvertCanvasToImageY(SelectCanvasYTo, True)
                Case 7 ' bottom left
                    SelectCanvasXfrom = X + SelCanvasOffsetX
                    SelectCanvasYTo = Y + SelCanvasOffsetY
                    SelectImageXfrom = ConvertCanvasToImageX(SelectCanvasXfrom, True)
                    SelectImageYto = ConvertCanvasToImageY(SelectCanvasYTo, True)
                Case 8 ' inside
                    ' Maintain rectangle width and height
                    Wid = SelectCanvasXto - SelectCanvasXfrom
                    Ht = SelectCanvasYTo - SelectCanvasYfrom

                    SelectCanvasXfrom = X + SelCanvasOffsetX
                    SelectCanvasXto = SelectCanvasXfrom + Wid
                    SelectCanvasYfrom = Y + SelCanvasOffsetY
                    SelectCanvasYTo = SelectCanvasYfrom + Ht

                    Wid = SelectImageXto - SelectImageXfrom
                    Ht = SelectImageYto - SelectImageYfrom

                    SelectImageXfrom = ConvertCanvasToImageX(SelectCanvasXfrom, True)
                    If SelectImageXfrom + Wid >= BMP.Width Then
                        SelectImageXfrom = BMP.Width - 1 - Wid
                    End If
                    SelectImageYfrom = ConvertCanvasToImageY(SelectCanvasYfrom, True)
                    If SelectImageYfrom + Ht >= BMP.Height Then
                        SelectImageYfrom = BMP.Height - 1 - Ht
                    End If
                    SelectImageXto = SelectImageXfrom + Wid
                    SelectImageYto = SelectImageYfrom + Ht
            End Select ' SelSizeBoxNo
            ImageSelectX = Math.Min(SelectImageXfrom, SelectImageXto)
            ImageSelectY = Math.Min(SelectImageYfrom, SelectImageYto)
            ImageSelectWidth = Math.Abs(SelectImageXto - SelectImageXfrom) + 1
            ImageSelectHeight = Math.Abs(SelectImageYto - SelectImageYfrom) + 1
        End If ' SelectMoving

        If Selecting Then
            SelectCanvasXto = X
            SelectCanvasYTo = Y
            SelectImageXto = ConvertCanvasToImageX(SelectCanvasXto, True)
            SelectImageYto = ConvertCanvasToImageY(SelectCanvasYTo, True)
            ImageSelectX = Math.Min(SelectImageXfrom, SelectImageXto)
            ImageSelectY = Math.Min(SelectImageYfrom, SelectImageYto)
            ImageSelectWidth = Math.Abs(SelectImageXto - SelectImageXfrom) + 1
            ImageSelectHeight = Math.Abs(SelectImageYto - SelectImageYfrom) + 1
        End If ' Selecting

        If PasteMoving Then
            PasteCanvasXfrom = X + PasteCanvasOffsetX
            PasteCanvasYfrom = Y + PasteCanvasOffsetY
            PasteImageXfrom = ConvertCanvasToImageX(PasteCanvasXfrom, False)
            PasteImageYfrom = ConvertCanvasToImageY(PasteCanvasYfrom, False)
            PasteCanvasXto = CType(PasteCanvasXfrom + Math.Round(PasteBMP.Width * ZoomX) - 1, Integer)
            PasteCanvasYto = CType(PasteCanvasYfrom + Math.Round(PasteBMP.Height * ZoomY) - 1, Integer)
        End If ' PasteMoving
    End Sub

    Private Sub PasteBitmap()
        Dim G As Graphics = Graphics.FromImage(BMP)

        SetHighSpeedGraphics(G, Drawing2D.PixelOffsetMode.Half)
        G.DrawImage(PasteBMP, New RectangleF(PasteImageXfrom, PasteImageYfrom, PasteBMP.Width, PasteBMP.Height), New RectangleF(0, 0, PasteBMP.Width, PasteBMP.Height), GraphicsUnit.Pixel)
        G.Dispose()
        PasteBMP.Dispose()
        GC.Collect()

        PasteHave = False
        PasteMoving = False
        Me.Invalidate()
    End Sub


    ' If have a selection box or paste area and mouse may be over it
    Private Sub ResetMousePointer(ByVal X As Integer, ByVal Y As Integer)
        Dim SizNo As Integer         ' loop counter
        Dim MyCursor As Cursor

        If Not (Selecting Or SelectMoving Or PasteMoving) Then
            MyCursor = Parent.Cursor
            If PasteHave Then
                If X <= PasteCanvasXto And X >= PasteCanvasXfrom And Y <= PasteCanvasYto And Y >= PasteCanvasYfrom Then
                    MyCursor = Cursors.SizeAll
                End If
            End If
            If SelectHave Then
                For SizNo = 0 To 8
                    If X <= SelSizeBoxXto(SizNo) And X >= SelSizeBoxXfrom(SizNo) And Y <= SelSizeBoxYto(SizNo) And Y >= SelSizeBoxYfrom(SizNo) Then
                        Select Case SizNo
                            Case 4, 6
                                MyCursor = Cursors.SizeNWSE
                            Case 0, 2
                                MyCursor = Cursors.SizeNS
                            Case 5, 7
                                MyCursor = Cursors.SizeNESW
                            Case 1, 3
                                MyCursor = Cursors.SizeWE
                            Case 8
                                MyCursor = Cursors.SizeAll
                        End Select ' SizNo
                        SelSizeBoxNo = SizNo
                        Exit For
                    End If
                Next SizNo
            End If
            Cursor = MyCursor
        End If ' Not (Selecting Or SelectMoving Or PasteMoving)
    End Sub

    Private Sub SetHighSpeedGraphics(ByRef G As Graphics, ByVal OffsetMode As Drawing2D.PixelOffsetMode)
        G.PageUnit = GraphicsUnit.Pixel
        G.PixelOffsetMode = OffsetMode
        G.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        G.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        G.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
    End Sub

    ' Return the selection bitmap in a non indexed format so that can get Graphics from it
    Private Function GetSafeSelectionBitmap() As Bitmap
        Dim BMPtmp As Bitmap

        If (BMP.PixelFormat And Imaging.PixelFormat.Indexed) = 0 Then
            BMPtmp = New Bitmap(ImageSelectWidth, ImageSelectHeight, BMP.PixelFormat)
        Else
            BMPtmp = New Bitmap(ImageSelectWidth, ImageSelectHeight, Imaging.PixelFormat.Format24bppRgb)
        End If
        BMPtmp.SetResolution(BMP.HorizontalResolution, BMP.VerticalResolution)
        Dim G As Graphics = Graphics.FromImage(BMPtmp)
        SetHighSpeedGraphics(G, Drawing2D.PixelOffsetMode.Half)
        G.DrawImage(BMP, 0, 0, New Rectangle(ImageSelectX, ImageSelectY, ImageSelectWidth, ImageSelectHeight), GraphicsUnit.Pixel)
        G.Dispose()

        Return BMPtmp
    End Function

    Private Function NormalisedAspect(ByVal ZoomIn As Single) As Single
        If ImageHave And MyNormaliseAspectRatio And BMP.HorizontalResolution > 0 And BMP.VerticalResolution > 0 And BMP.HorizontalResolution <> BMP.VerticalResolution Then
            Return ZoomIn * BMP.HorizontalResolution / BMP.VerticalResolution
        Else
            Return ZoomIn
        End If
    End Function

    Private Function DeNormalisedAspect(ByVal ZoomIn As Single) As Single
        If ImageHave And MyNormaliseAspectRatio And BMP.HorizontalResolution > 0 And BMP.VerticalResolution > 0 And BMP.HorizontalResolution <> BMP.VerticalResolution Then
            Return ZoomIn / BMP.HorizontalResolution * BMP.VerticalResolution
        Else
            Return ZoomIn
        End If
    End Function
#End Region

End Class

' This class is used to display scrollbar properties without allowing the user to change them (except .Value)
' Typically it woulde be used in another class as follows:
'       Private WithEvents HScrollBar1 As New System.Windows.Forms.HScrollBar
'       Private WithEvents VScrollBar1 As New System.Windows.Forms.VScrollBar
'       Private DummySC As New System.Windows.Forms.ScrollableControl
'       Public HScrollbar As New MyScrollBarProperties(DummySC, HScrollBar1)
'       Public VScrollbar As New MyScrollBarProperties(DummySC, VScrollBar1)
Public Class MyScrollBarProperties
    Inherits System.Windows.Forms.HScrollProperties

#Region " Definitions "
    Private Enum MyScrollbarType
        Horizontal
        Vertical
    End Enum

    Private HScroll As New System.Windows.Forms.HScrollBar
    Private VScroll As New System.Windows.Forms.VScrollBar
    Private Orientation As MyScrollbarType
#End Region

#Region " Constructor "
    Public Sub New(ByVal container As System.Windows.Forms.ScrollableControl, ByVal value As HScrollBar)
        MyBase.New(container)
        HScroll = value
        Orientation = MyScrollbarType.Horizontal
    End Sub

    Public Sub New(ByVal container As System.Windows.Forms.ScrollableControl, ByVal value As VScrollBar)
        MyBase.New(container)
        VScroll = value
        Orientation = MyScrollbarType.Vertical
    End Sub
#End Region

#Region " Properties "
    ''' <summary>
    ''' Gets whether the scrollbar can be used on the container
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property Enabled() As Boolean
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.Visible
            Else
                Return VScroll.Visible
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets a numeric value that represents the current position of the scroll bar box.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads Property Value() As Integer
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.Value
            Else
                Return VScroll.Value
            End If
        End Get
        Set(ByVal value As Integer)
            If Orientation = MyScrollbarType.Horizontal Then
                HScroll.Value = Math.Min(Math.Max(value, HScroll.Minimum), HScroll.Maximum - HScroll.LargeChange) ' -.LargeChange to prevent the image scrolling of the display
            Else
                VScroll.Value = Math.Min(Math.Max(value, VScroll.Minimum), VScroll.Maximum - VScroll.LargeChange) ' -.LargeChange to prevent the image scrolling of the display
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets whether the scroll bar can be seen be the user.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property Visible() As Boolean
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.Visible
            Else
                Return VScroll.Visible
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the upper limit of the scrollable range.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property Maximum() As Integer
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.Maximum
            Else
                Return VScroll.Maximum
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the lower limit of the scrollable range.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property Minimum() As Integer
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.Minimum
            Else
                Return VScroll.Minimum
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the distance to move the scroll bar in response to a large scroll command.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property LargeChange() As Integer
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.LargeChange
            Else
                Return VScroll.LargeChange
            End If
        End Get
    End Property


    ''' <summary>
    ''' Gets the distance to move the scroll bar in response to a small scroll command.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property SmallChange() As Integer
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.SmallChange
            Else
                Return VScroll.SmallChange
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the width of the scroll bar.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property Width() As Integer
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.Width
            Else
                Return VScroll.Width
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the height of the scroll bar.
    ''' </summary>
    <Browsable(False)> _
    Public Overloads ReadOnly Property Height() As Integer
        Get
            If Orientation = MyScrollbarType.Horizontal Then
                Return HScroll.Height
            Else
                Return VScroll.Height
            End If
        End Get
    End Property
#End Region

End Class

