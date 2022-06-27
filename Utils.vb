'***********************************************************************************************************************
' Utilitiy module
' 
' This is module contains utility routines, application global defines etc, non managed code references.

Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Drawing

Module Utils

#Region " Definitions "
    Friend Enum GLYPHTYPES
        gtALL = -1
        gtSIMPLESYMBOLS = 1
        gtEXTENDEDSYMBOLS = 2
        gtNUMERIC = 4
        gtCYRILLIC = 8
        gtGREEKCOPTIC = 16
        gtEASTERNEUROPEAN = 32
        gtWESTERNEUROPEAN = 64
        gtSIMPLEUPPERCASE = 128
        gtEXTENDEDUPPERCASE = 256
        gtSIMPLELOWERCASE = 512
        gtEXTENDEDLOWERCASE = 1024
        gtUDL1 = 2048         ' user defined list
        gtUDL2 = 4096
        gtUDL3 = 8192
    End Enum
    Friend Enum OVERIDESTATE
        osNONE = 0
        osDISABLE = 1
        osENABLE = 2
    End Enum

    Friend Const ERRCANTFINDDLLENTRYPOINT As Integer = 453 ' Can't find DLL entry point

    Friend Const REGAPPNAME As String = "TOCR Viewer1.1" ' key in registry
    Friend Const PIXELSTOTWIPS As Integer = 15

    Friend JobNo As Integer             ' engine job slot number
    Friend XDPI As Integer              ' horizontal dots per inch
    Friend YDPI As Integer              ' vertical dots per inch
    Friend OpenInitDir As String        ' input folder for images
    Friend SaveInitDir As String        ' output folder to save results in
    Friend JobInfo_EG As TOCRJOBINFO_EG
    Friend FontNames() As String

    ' process options
    Friend LanguageDisableGlyphs() As Boolean      ' disabled glyphs array
    Friend ManualDisableGlyphs() As Boolean        ' disabled glyphs array
    Friend OverideGlyphs() As OVERIDESTATE         ' disabled/enable glyphs array
    Friend TOCRGL4() As UShort
    Friend GlyphType() As GLYPHTYPES

    ' All characters (hex unicode points)
    Friend Const TOCRGL4STR As String = "0,21,22,23,24,25,26,27,28,29,2A,2B,2C,2D,2E,2F,30,31,32,33,34,35,36,37,38,39,3A,3B,3C,3D,3E,3F," & _
    "40,41,42,43,44,45,46,47,48,49,4A,4B,4C,4D,4E,4F,50,51,52,53,54,55,56,57,58,59,5A,5B,5C,5D,5E,5F," & _
    "0,61,62,63,64,65,66,67,68,69,6A,6B,6C,6D,6E,6F,70,71,72,73,74,75,76,77,78,79,7A,7B,7C,7D,7E,0," & _
    "20AC,0,0,0,0,0,2020,2021,0,2030,160,0,152,0,17D,0,0,0,0,0,0,2022,0,0,0,2122,161,0,153,0,17E,178," & _
    "0,A1,A2,A3,A4,A5,0,A7,0,A9,AA,AB,AC,0,AE,AF,B0,B1,B2,B3,0,B5,B6,B7,0,B9,0,BB,BC,BD,BE,BF," & _
    "C0,C1,C2,C3,C4,C5,C6,C7,C8,C9,CA,CB,CC,CD,CE,CF,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,DA,DB,DC,DD,DE,DF," & _
    "E0,E1,E2,E3,E4,E5,E6,E7,E8,E9,EA,EB,EC,ED,EE,EF,F0,F1,F2,F3,F4,F5,F6,F7,F8,F9,FA,FB,FC,FD,FE,FF," & _
    "100,101,102,103,104,105,106,107,108,109,10A,10B,10C,10D,10E,10F,0,111,112,113,114,115,116,117,118,119,11A,11B,11C,11D,11E,11F," & _
    "120,121,122,123,124,125,126,127,128,129,12A,12B,12C,12D,12E,12F,130,131,0,0,134,135,136,137,138,139,0,13B,13C,13D,13E,13F," & _
    "140,141,142,143,144,145,146,147,148,149,14A,14B,14C,14D,14E,14F,150,151,154,155,156,157,158,159,15A,15B,15C,15D,15E,15F,162,163," & _
    "164,165,166,167,168,169,16A,16B,16C,16D,16E,16F,170,171,172,173,174,175,176,177,179,17A,17B,17C,17F,1FA,1FB,1FC,1FD,1FE,1FF,386," & _
    "388,389,38A,38C,38E,38F,390,391,392,393,394,395,396,397,398,399,39A,39B,39C,39D,39E,39F,3A0,3A1,3A3,3A4,3A5,3A6,3A7,3A8,3A9,3AA," & _
    "3AB,3AC,3AD,3AE,3AF,3B0,3B1,3B2,3B3,3B4,3B5,3B6,3B7,3B8,3B9,3BA,3BB,3BC,3BD,3BE,3BF,3C0,3C1,3C2,3C3,3C4,3C5,3C6,3C7,3C8,3C9,3CA," & _
    "3CB,3CC,3CD,3CE,401,402,403,404,405,406,407,408,409,40A,40B,40C,40E,40F,410,411,412,413,414,415,416,417,418,419,41A,41B,41C,41D," & _
    "41E,41F,420,421,422,423,424,425,426,427,428,429,42A,42B,42C,42D,42E,42F,430,431,432,433,434,435,436,437,438,439,43A,43B,43C,43D," & _
    "43E,43F,440,441,442,443,444,445,446,447,448,449,44A,44B,44C,44D,44E,44F,451,452,453,454,455,456,457,458,459,45A,45B,45C,45E,45F," & _
    "490,491,1E80,1E81,1E82,1E83,1E84,1E85,1EF2,1EF3,20A3,20A4,2105,0,215B,215C,215D,215E,2190,2191,2192,2193,2194,2195,0,2202,0,220F,2211,221A,221E,221F,2229," & _
    "222B,2248,2260,2261,2264,2265,2302,2310,25A1,25AA,25AB,25AC,25B2,25BA,25BC,25C4,25CA,0,25CF,25E6,263A,263B,263C,2640,2642,2660,2663,2665,2666,266A,266B"


    Friend Enum RESULTSVIEW
        Results   ' Show the results as normal
        PImage    ' Show the processed image
    End Enum

    Friend Enum RESULTSFMT
        None    ' no fomratting
        Text    ' old style formatting using the default font
        RTF     ' richtext formatting
    End Enum

    ' Structure to help formatting of text in this Viewer
    Friend Structure FORMATINFO
        Dim FontName As String          ' default font
        Dim FontSize As Single
        Dim FontBold As Boolean
        Dim FontItalic As Boolean
        Dim ResultsWindow As RESULTSVIEW
        Dim Fmt As RESULTSFMT
        Dim CollapseY As Boolean            ' flag if to collapse large vertical spaces to a single space
        Dim LeftMargin As Boolean           ' flag if to keep left margin
        Dim MinXSpaceFac As Single          ' size of a horizontal space that determines if to align text
        Dim MinYSpaceFac As Single          ' size of vertical space that determines if to add blank lines
    End Structure ' FORMATINFO
    Friend ResultsFormatInfo As FORMATINFO

    Private Structure WORDINFO          ' used to help format text
        Dim Txt As String               ' characters in a word
        Dim LineNo As Integer           ' line number word is on
        Dim ResultsInd As Integer       ' index into TOCRRESULTSITEM structure of first character in word
        Dim MinX As Integer             ' minimum X pixel on image of word
        Dim MaxX As Integer             ' maximum X pixel on image of word
        Dim MinY As Integer             ' minimum Y pixel on image of word
        Dim MaxY As Integer             ' maximum Y pixel on image of word
        Dim abc As GDI.ABCFLOAT         ' abc spacing for word
        Dim fs As Single                ' fontsize
        Dim fsCnt As Integer            ' count of characters for font size
    End Structure ' WORDINFO

#End Region

#Region " GDI Declares "
    ' Declarations for things in gdi32.dll
    Friend Class GDI
        Friend Const DIB_RGB_COLORS As Integer = 0
        Friend Const BI_RGB As Integer = 0
        Friend Const BI_BITFIELDS As Integer = 3
        Friend Const SRCCOPY As Integer = &HCC0020&
        Friend Const NOTSRCCOPY As Integer = &H330008

        <StructLayout(LayoutKind.Sequential, pack:=4)> _
        Friend Structure RGBQUAD
            Dim rgbBlue As Byte
            Dim rgbGreen As Byte
            Dim rgbRed As Byte
            Dim rgbReserved As Byte
        End Structure ' RGBQUAD

        <StructLayout(LayoutKind.Sequential, pack:=4)> _
        Friend Structure BITMAPINFOHEADER
            Dim biSize As Integer
            Dim biWidth As Integer
            Dim biHeight As Integer
            Dim biPlanes As Short
            Dim biBitCount As Short
            Dim biCompression As Integer
            Dim biSizeImage As Integer
            Dim biXPelsPerMeter As Integer
            Dim biYPelsPerMeter As Integer
            Dim biClrUsed As Integer
            Dim biClrImportant As Integer
        End Structure ' BITMAPINFOHEADER

        ' For Format1bppIndexed
        <StructLayout(LayoutKind.Sequential, pack:=4)> _
        Friend Structure BITMAPINFO1
            Dim bmih As BITMAPINFOHEADER
            <VBFixedArray(2), MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
            Public clrs As RGBQUAD()
        End Structure ' BITMAPINFO

        ' For Format4bppIndexed
        <StructLayout(LayoutKind.Sequential, pack:=4)> _
        Friend Structure BITMAPINFO4
            Dim bmih As BITMAPINFOHEADER
            <VBFixedArray(16), MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)> _
            Public clrs As RGBQUAD()
        End Structure ' BITMAPINFO

        ' For Format8bppIndexed
        <StructLayout(LayoutKind.Sequential, pack:=4)> _
        Friend Structure BITMAPINFO8
            Dim bmih As BITMAPINFOHEADER
            <VBFixedArray(256), MarshalAs(UnmanagedType.ByValArray, SizeConst:=256)> _
            Public clrs As RGBQUAD()
        End Structure ' BITMAPINFO

        <StructLayout(LayoutKind.Sequential)> _
         Structure ABCFLOAT
            Dim abcfA As Single
            Dim abcfB As Single
            Dim abcfC As Single
        End Structure ' ABCFLOAT

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure SDK_SIZE
            Dim cx As Integer
            Dim cy As Integer
        End Structure

        Friend Declare Function DeleteObject Lib "gdi32.dll" (ByVal hObject As IntPtr) As Boolean
        Friend Declare Function CreateCompatibleDC Lib "gdi32.dll" (ByVal hRefDC As IntPtr) As IntPtr
        Friend Declare Function DeleteDC Lib "gdi32.dll" (ByVal hdc As IntPtr) As Boolean
        Friend Declare Function SelectObject Lib "gdi32.dll" (ByVal hdc As IntPtr, ByVal hObject As IntPtr) As IntPtr
        Friend Declare Function BitBlt Lib "gdi32.dll" (ByVal hdc As IntPtr, ByVal nXDest As Integer, ByVal nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hdcSrc As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, ByVal dwRop As Integer) As Boolean
        Friend Declare Function CreateDIBSection Lib "gdi32.dll" (ByVal hdc As IntPtr, ByRef pbmi As BITMAPINFO1, ByVal iUsage As UInt32, ByRef ppvBits As IntPtr, ByVal hSection As IntPtr, ByVal dwOffset As UInt32) As IntPtr
        Friend Declare Function CreateDIBSection Lib "gdi32.dll" (ByVal hdc As IntPtr, ByRef pbmi As BITMAPINFO4, ByVal iUsage As UInt32, ByRef ppvBits As IntPtr, ByVal hSection As IntPtr, ByVal dwOffset As UInt32) As IntPtr
        Friend Declare Function CreateDIBSection Lib "gdi32.dll" (ByVal hdc As IntPtr, ByRef pbmi As BITMAPINFO8, ByVal iUsage As UInt32, ByRef ppvBits As IntPtr, ByVal hSection As IntPtr, ByVal dwOffset As UInt32) As IntPtr

        
        'Friend Declare Function GetCharABCWidthsFloat Lib "gdi32" Alias "GetCharABCWidthsFloatA" (ByVal hdc As IntPtr, ByVal iFirstChar As Integer, ByVal iLastChar As Integer, ByRef lpABCF As ABCFLOAT) As Integer
        'Friend Declare Function GetTextExtentPoint32 Lib "gdi32" Alias "GetTextExtentPoint32A" (ByVal hdc As IntPtr, ByVal lpsz As String, ByVal cbString As Integer, ByRef lpSize As SDK_SIZE) As Integer
        'Friend Declare Function GetTextExtentExPoint Lib "gdi32" Alias "GetTextExtentExPointA" (ByVal hdc As IntPtr, ByVal lpszStr As String, ByVal cchString As Integer, ByVal nMaxExtent As Integer, ByRef lpnFit As Integer, ByVal alpDx As Integer, ByRef lpSize As SDK_SIZE) As Integer

        Friend Declare Function GetCharABCWidthsFloatW Lib "gdi32" (ByVal hdc As IntPtr, ByVal iFirstChar As Integer, ByVal iLastChar As Integer, ByRef lpABCF As ABCFLOAT) As Integer
        Friend Declare Function GetTextExtentPoint32W Lib "gdi32" (ByVal hdc As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal lpsz As String, ByVal cbString As Integer, ByRef lpSize As SDK_SIZE) As Integer
        Friend Declare Function GetTextExtentExPointW Lib "gdi32" (ByVal hdc As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal lpszStr As String, ByVal cchString As Integer, ByVal nMaxExtent As Integer, ByRef lpnFit As Integer, ByVal alpDx As Integer, ByRef lpSize As SDK_SIZE) As Integer
        Friend Declare Function GdiFlush Lib "gdi32" () As Integer

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure FIXED
            Dim fract As Short
            Dim Value As Short
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure MAT2
            Dim eM11 As FIXED
            Dim eM12 As FIXED
            Dim eM21 As FIXED
            Dim eM22 As FIXED
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure POINTAPI
            Dim X As Integer
            Dim Y As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure GLYPHMETRICS
            Dim gmBlackBoxX As Integer
            Dim gmBlackBoxY As Integer
            Dim gmptGlyphOrigin As POINTAPI
            Dim gmCellIncX As Short
            Dim gmCellIncY As Short
        End Structure
        Friend Declare Function GetGlyphOutlineW Lib "gdi32" (ByVal hdc As IntPtr, ByVal uChar As Integer, ByVal fuFormat As Integer, ByRef lpgm As GLYPHMETRICS, ByVal cbBuffer As Integer, ByVal lpBuffer As Integer, ByRef lpmat2 As MAT2) As Integer

    End Class
#End Region

#Region " Kernel Declares "
    ' Declarations for things in kernel32.dll
    Friend Class KRN
        Friend Const PAGE_READWRITE As Integer = 4
        Friend Const FILE_MAP_WRITE As Integer = 2
        Friend Const FILE_MAP_READ As Integer = 4

        Friend Declare Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean
        Friend Declare Function CreateFileMappingMy Lib "kernel32" Alias "CreateFileMappingA" (ByVal hFile As Integer, ByVal lpFileMappigAttributes As Integer, ByVal flProtect As Integer, ByVal dwMaximumSizeHigh As Integer, ByVal dwMaximumSizeLow As Integer, ByVal lpName As Integer) As IntPtr
        Friend Declare Function MapViewOfFileMy Lib "kernel32" Alias "MapViewOfFile" (ByVal hFileMappingObject As IntPtr, ByVal dwDesiredAccess As Integer, ByVal dwFileOffsetHigh As Integer, ByVal dwFileOffsetLow As Integer, ByVal dwNumberOfBytesToMap As Integer) As IntPtr
        Friend Declare Function UnmapViewOfFileMy Lib "kernel32" Alias "UnmapViewOfFile" (ByVal lpBaseAddress As IntPtr) As Integer
#If Win64 Then
        Friend Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal lpvDest As Int64, ByVal lpvSrc As IntPtr, ByVal cbCopy As Integer)
#Else
        Friend Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal lpvDest As Int32, ByVal lpvSrc As IntPtr, ByVal cbCopy As Integer)
#End If
        Friend Declare Function GlobalLock Lib "kernel32" (ByVal hMem As IntPtr) As IntPtr
        Friend Declare Function GlobalUnlock Lib "kernel32" (ByVal hMem As IntPtr) As Integer
        Friend Declare Function GlobalFree Lib "kernel32" (ByVal hMem As IntPtr) As Integer
    End Class
#End Region

#Region " User Declares "
    ' Declarations for things in user32.dll
    Friend Class USR
        Friend Declare Function GetDC Lib "user32.dll" (ByVal hWnd As IntPtr) As IntPtr
        Friend Declare Function ReleaseDC Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal hDC As IntPtr) As Boolean
    End Class
#End Region

#Region " Friend routines "

    ' Converta a bitmap to monochrome - using a specified algorithm and threshold
    Friend Sub ConvertBitmap2BW(ByRef BMPIn As Bitmap, ByRef CCAlgo As Integer, ByRef CCThresh As Single)

        Dim Row As Integer, Col As Integer
        Dim Thresh As Integer
        Dim I As Integer
        Dim Value As Integer

        Select Case CCAlgo
            Case TOCRJOBCC_AVERAGE
                Thresh = CInt(CCThresh * 256 * 3)
            Case TOCRJOBCC_LUMA_BT601
                Thresh = CInt(CCThresh * 256 * 1000)
            Case TOCRJOBCC_LUMA_BT709
                Thresh = CInt(CCThresh * 256 * 10000)
            Case TOCRJOBCC_DESATURATION
                Thresh = CInt(CCThresh * 256 * 2)
            Case TOCRJOBCC_DECOMPOSITION_MAX, TOCRJOBCC_DECOMPOSITION_MIN, TOCRJOBCC_RED, TOCRJOBCC_GREEN, TOCRJOBCC_BLUE
                Thresh = CInt(CCThresh * 256)
        End Select


        Dim BMPData As Imaging.BitmapData

        ' Should work for .Format24bpprgb but when gthe input is .Format32bppPArgb a black line can appear on the
        ' extreme left and bottom (regardless of what you set the rgbBytes to).  I haven't investigatewd this any further.

        BMPData = BMPIn.LockBits(New Rectangle(New Point, BMPIn.Size), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)
        Dim Ptr As System.IntPtr = BMPData.Scan0

        Dim NumBytes As Integer = Math.Abs(BMPData.Stride) * BMPIn.Height
        Dim rgbBytes(NumBytes - 1) As Byte

        System.Runtime.InteropServices.Marshal.Copy(Ptr, rgbBytes, 0, NumBytes)

        For Col = 0 To BMPIn.Width - 1
            For Row = 0 To BMPIn.Height - 1
                I = Math.Abs(BMPData.Stride) * Row + Col * 4
                Select Case CCAlgo
                    Case TOCRJOBCC_AVERAGE
                        Value = CInt(rgbBytes(I)) + rgbBytes(I + 1) + rgbBytes(I + 2)
                    Case TOCRJOBCC_LUMA_BT601
                        Value = 299I * rgbBytes(I) + 587I * rgbBytes(I + 1) + 114I * rgbBytes(I + 2)
                    Case TOCRJOBCC_LUMA_BT709
                        Value = 2126I * rgbBytes(I) + 7152I * rgbBytes(I + 1) + 722I * rgbBytes(I + 2)
                    Case TOCRJOBCC_DESATURATION
                        Value = CInt(Math.Max(Math.Max(rgbBytes(I), rgbBytes(I + 1)), rgbBytes(I + 2))) + Math.Min(Math.Min(rgbBytes(I), rgbBytes(I + 1)), rgbBytes(I + 2))
                    Case TOCRJOBCC_DECOMPOSITION_MAX
                        Value = Math.Max(Math.Max(rgbBytes(I), rgbBytes(I + 1)), rgbBytes(I + 2))
                    Case TOCRJOBCC_DECOMPOSITION_MIN
                        Value = Math.Min(Math.Min(rgbBytes(I), rgbBytes(I + 1)), rgbBytes(I + 2))
                    Case TOCRJOBCC_RED
                        Value = rgbBytes(I + 2)
                    Case TOCRJOBCC_GREEN
                        Value = rgbBytes(I + 1)
                    Case TOCRJOBCC_BLUE
                        Value = rgbBytes(I)
                End Select

                If Value < Thresh Then
                    Value = 0
                Else
                    Value = 255
                End If

                rgbBytes(I) = CByte(Value) ' Blue
                rgbBytes(I + 1) = rgbBytes(I) ' Green
                rgbBytes(I + 2) = rgbBytes(I) ' Red
            Next
        Next
        System.Runtime.InteropServices.Marshal.Copy(rgbBytes, 0, Ptr, NumBytes)
        BMPIn.UnlockBits(BMPData)

        Return

    End Sub

    ' This function is used to convert the memory blocks to bitmaps
    ' It expects the memory block to contain a BitmapInfoHeader, zero or more palette entries and then bitmap data
    Friend Function ConvertBlockToBitmap(ByVal Ptr As IntPtr) As Bitmap
        Dim BMP As Bitmap = Nothing
        Dim BIH As GDI.BITMAPINFOHEADER
        Dim DataPtr As IntPtr
        Dim PalPtr As IntPtr
        Dim HdrSize As Integer
        Dim PixFormat As Imaging.PixelFormat
        Dim PalEntries As Integer
        Dim rgb As GDI.RGBQUAD
        Dim Row As Integer
        Dim Stride As Integer
        Dim BMPData As Imaging.BitmapData
        Dim XDPI As Single
        Dim YDPI As Single
#If Win64 Then
        Dim Offset As Int64
#Else
        Dim Offset As Int32
#End If


        If Ptr.Equals(IntPtr.Zero) Then Return Nothing

        BIH = CType(Marshal.PtrToStructure(Ptr, GetType(GDI.BITMAPINFOHEADER)), GDI.BITMAPINFOHEADER)
        HdrSize = BIH.biSize
#If Win64 Then
        PalPtr = New IntPtr(Ptr.ToInt64() + HdrSize)
#Else
        PalPtr = New IntPtr(Ptr.ToInt32() + HdrSize)
#End If

        ' Most of these formats are untested
        PixFormat = Imaging.PixelFormat.Format1bppIndexed
        Select Case BIH.biBitCount
            Case 1
                HdrSize += 2 * Marshal.SizeOf(rgb)
                PixFormat = Imaging.PixelFormat.Format1bppIndexed
                PalEntries = 2
            Case 4
                HdrSize += 16 * Marshal.SizeOf(rgb)
                PixFormat = Imaging.PixelFormat.Format4bppIndexed
                PalEntries = BIH.biClrUsed
            Case 8
                HdrSize += 256 * Marshal.SizeOf(rgb)
                PixFormat = Imaging.PixelFormat.Format8bppIndexed
                PalEntries = BIH.biClrUsed
            Case 16
                ' Account for the 3 DWORD colour mask
                If BIH.biCompression = GDI.BI_BITFIELDS Then HdrSize += 12
                PixFormat = Imaging.PixelFormat.Format16bppRgb555
                PalEntries = 0
            Case 24
                PixFormat = Imaging.PixelFormat.Format24bppRgb
                PalEntries = 0
            Case 32
                ' Account for the 3 DWORD colour mask
                If BIH.biCompression = GDI.BI_BITFIELDS Then HdrSize += 12
                PixFormat = Imaging.PixelFormat.Format32bppRgb
                PalEntries = 0
            Case Else
        End Select

        BMP = New Bitmap(BIH.biWidth, Math.Abs(BIH.biHeight), PixFormat)
        If PalEntries > 0 Then
            'PalPtr = New IntPtr(Ptr.ToInt64() + BIH.biSize)
            Dim Pal As Imaging.ColorPalette
            Dim PalEntry As Integer
            Pal = BMP.Palette
            For PalEntry = 0 To PalEntries - 1
                rgb = CType(Marshal.PtrToStructure(PalPtr, GetType(GDI.RGBQUAD)), GDI.RGBQUAD)
                Pal.Entries(PalEntry) = Color.FromArgb(rgb.rgbRed, rgb.rgbGreen, rgb.rgbBlue)
                PalPtr = New IntPtr(PalPtr.ToInt64() + Marshal.SizeOf(rgb))
            Next PalEntry
            BMP.Palette = Pal
        End If

        ' Load the bitmap in reverse order because GDI+ is top down whilst GDI is bottom up
        BMPData = BMP.LockBits(New Rectangle(New Point, BMP.Size), Imaging.ImageLockMode.ReadWrite, PixFormat)
        Stride = Math.Abs(BMPData.Stride)
#If Win64 Then
        DataPtr = New IntPtr(Ptr.ToInt64() + HdrSize + Stride * (BMP.Height - 1))
        Offset = BMPData.Scan0.ToInt64()
#Else
        DataPtr = New IntPtr(Ptr.ToInt32() + HdrSize + Stride * (BMP.Height - 1))
        Offset = BMPData.Scan0.ToInt32()
#End If
        For Row = 0 To BMP.Height - 1
            KRN.CopyMemory(Offset, DataPtr, Stride)
            Offset += Stride
#If Win64 Then
            DataPtr = New IntPtr(DataPtr.ToInt64 - Stride)
#Else
            DataPtr = New IntPtr(DataPtr.ToInt32 - Stride)
#End If
        Next

        ' Use the copy instead of the above if you are going to flip below
        'KRN.CopyMemory(BMPData.Scan0.ToInt32(), DataPtr, Math.Abs(BMPData.Stride * BMP.Height))
        BMP.UnlockBits(BMPData)

        ' Flip the bitmap (GDI+ bitmap scan lines are top down, GDI are bottom up)
        ' DO NOT DO THIS.  GDI+ has trouble manipulating indexed bitmaps
        'BMP.RotateFlip(RotateFlipType.RotateNoneFlipY)

        ' Reset the resolutions

        XDPI = CSng(Int(BIH.biXPelsPerMeter * 2.54 / 100 + 0.5))
        YDPI = CSng(Int(BIH.biYPelsPerMeter * 2.54 / 100 + 0.5))
        If XDPI > 0 And YDPI > 0 Then BMP.SetResolution(XDPI, YDPI)

        Return BMP
    End Function

    ' This function is used to convert the global memory blocks (from TWAIN) to bitmaps
    Friend Function ConvertMemoryBlockToBitmap(ByVal hMem As IntPtr) As Bitmap
        Dim Ptr As IntPtr
        Dim BMP As Bitmap = Nothing

        Ptr = KRN.GlobalLock(hMem)
        If Not Ptr.Equals(IntPtr.Zero) Then
            BMP = ConvertBlockToBitmap(Ptr)
            KRN.GlobalUnlock(hMem)
        End If

        Return BMP

    End Function

    ' Convert a memory mapped file returned from TOCR to a GDI+ bitmap
    Friend Function ConvertMMFToBitmap(ByVal MMFHandle As IntPtr) As Bitmap
        Dim Ptr As IntPtr
        Dim BMP As Bitmap = Nothing

        Ptr = KRN.MapViewOfFileMy(MMFHandle, KRN.FILE_MAP_READ, 0, 0, 0)
        If Not Ptr.Equals(IntPtr.Zero) Then
            BMP = ConvertBlockToBitmap(Ptr)
            KRN.UnmapViewOfFileMy(Ptr)
        End If

        Return BMP

    End Function

    ' Convert a bitmap to a memory mapped file.
    ' It does this by constructing a GDI bitmap in a byte array and copying this to a memory mapped file.
    Friend Function ConvertBitmapToMMF(ByVal BMPIn As Bitmap) As IntPtr

        Dim BMP As Bitmap
        Dim BIH As GDI.BITMAPINFOHEADER
        Dim BMPData As Imaging.BitmapData
        Dim ImageSize As Integer                ' size of image in bytes
        Dim MMFBytes() As Byte                  ' array of bytes holding the memory mapped files
        Dim MMFsize As Integer                  ' size of memory memory mapped file (and array MMFBytes)
        Dim MMFhandle As IntPtr = IntPtr.Zero   ' handle to memory mapped file
        Dim MMFview As IntPtr = IntPtr.Zero     ' pointer to view of memory mapped file
        Dim MMFBytesGC As GCHandle              ' handle to pin MMFBytes
        Dim PalEntries As Integer               ' number of entries in a palette (if any)
        Dim PalEntry As Integer                 ' loop counter
        Dim rgb As GDI.RGBQUAD                  ' rgb value of a palette entry
        Dim Offset As Integer                   ' address offset
        Dim Stride As Integer                   ' stride of bitmap data
        Dim Row As Integer                      ' loop counter
        Dim Ptr As IntPtr

        ConvertBitmapToMMF = IntPtr.Zero

        BMP = BMPIn.Clone(New Rectangle(New Point, BMPIn.Size), BMPIn.PixelFormat)
        BMP.SetResolution(BMPIn.HorizontalResolution, BMPIn.VerticalResolution)

        ' AVOID Indexed bitmaps because GDI+ support is patchy and fragile
        If (BMP.PixelFormat And Imaging.PixelFormat.Indexed) <> 0 Then
            ConvertBitmap(BMP, Imaging.PixelFormat.Format24bppRgb)
        End If

        ' Use ImageLockMode.ReadWrite even if you are just reading otherwise reading from Scan0 can give a protection violation
        ' on 1bpp.  
        BMPData = BMP.LockBits(New Rectangle(New Point, BMP.Size), Imaging.ImageLockMode.ReadWrite, BMP.PixelFormat)

        Stride = Math.Abs(BMPData.Stride)
        ImageSize = Math.Abs(Stride * BMPData.Height)
        PalEntries = BMP.Palette.Entries.Length

        BIH.biWidth = BMP.Width
        BIH.biHeight = BMP.Height
        BIH.biPlanes = 1
        BIH.biSize = Marshal.SizeOf(BIH)
        BIH.biClrImportant = 0
        BIH.biCompression = GDI.BI_RGB
        BIH.biSizeImage = ImageSize
        BIH.biXPelsPerMeter = CInt(BMP.HorizontalResolution * 100 / 2.54)
        BIH.biYPelsPerMeter = CInt(BMP.VerticalResolution * 100 / 2.54)

        ' Most of these formats are untested and the alpha channel is ignored
        Select Case BMP.PixelFormat
            Case Imaging.PixelFormat.Format1bppIndexed
                BIH.biBitCount = 1
            Case Imaging.PixelFormat.Format4bppIndexed
                BIH.biBitCount = 4
            Case Imaging.PixelFormat.Format8bppIndexed
                BIH.biBitCount = 8
            Case Imaging.PixelFormat.Format16bppArgb1555, Imaging.PixelFormat.Format16bppGrayScale, Imaging.PixelFormat.Format16bppRgb555, Imaging.PixelFormat.Format16bppRgb565
                BIH.biBitCount = 16
                PalEntries = 0
            Case Imaging.PixelFormat.Format24bppRgb
                BIH.biBitCount = 24
                PalEntries = 0
            Case Imaging.PixelFormat.Format32bppArgb, Imaging.PixelFormat.Format32bppPArgb, Imaging.PixelFormat.Format32bppRgb
                BIH.biBitCount = 32
                PalEntries = 0
        End Select
        BIH.biClrUsed = PalEntries

        MMFsize = Marshal.SizeOf(BIH) + PalEntries * Marshal.SizeOf(GetType(GDI.RGBQUAD)) + ImageSize
        ReDim MMFBytes(MMFsize)

        MMFBytesGC = GCHandle.Alloc(MMFBytes, GCHandleType.Pinned)
        Marshal.StructureToPtr(BIH, MMFBytesGC.AddrOfPinnedObject, True)
        Offset = Marshal.SizeOf(BIH)
        For PalEntry = 0 To PalEntries - 1
            rgb.rgbRed = BMP.Palette.Entries(PalEntry).R
            rgb.rgbGreen = BMP.Palette.Entries(PalEntry).G
            rgb.rgbBlue = BMP.Palette.Entries(PalEntry).B
            Marshal.StructureToPtr(rgb, Marshal.UnsafeAddrOfPinnedArrayElement(MMFBytes, Offset), False)
            Offset = Offset + Marshal.SizeOf(rgb)
        Next
        MMFBytesGC.Free()
        Marshal.Copy(BMPData.Scan0, MMFBytes, Offset, ImageSize)
        BMP.UnlockBits(BMPData)
        BMPData = Nothing

        BMP.Dispose()
        BMP = Nothing

        MMFhandle = KRN.CreateFileMappingMy(&HFFFFFFFF, 0&, KRN.PAGE_READWRITE, 0, MMFsize, 0&)
        If Not MMFhandle.Equals(IntPtr.Zero) Then
            MMFview = KRN.MapViewOfFileMy(MMFhandle, KRN.FILE_MAP_WRITE, 0, 0, 0)
            If MMFview.Equals(IntPtr.Zero) Then
                KRN.CloseHandle(MMFhandle)
            Else
                ' Flip the bitmap (GDI+ bitmap scan lines are top down, GDI are bottom up)
                'Marshal.Copy(MMFBytes, 0, MMFview, MMFsize) ' Use this line iy you flipped the bitmap above

                Offset = MMFsize - ImageSize
                Marshal.Copy(MMFBytes, 0, MMFview, Offset)
                Ptr = New IntPtr(MMFview.ToInt64() + MMFsize - Stride)
                For Row = 0 To ImageSize \ Stride - 1
                    Marshal.Copy(MMFBytes, Offset, Ptr, Stride)
                    Offset += Stride
                    Ptr = New IntPtr(Ptr.ToInt64() - Stride)
                Next
                KRN.UnmapViewOfFileMy(MMFview)
                ConvertBitmapToMMF = MMFhandle
            End If
        End If

        MMFBytes = Nothing

        If MMFhandle.Equals(IntPtr.Zero) Then
            MsgBox("Failed to convert bitmap", MsgBoxStyle.Critical, "ConvertBitmapToMMF")
        End If

        Return ConvertBitmapToMMF
    End Function

    Friend Sub ConvertBitmap(ByRef BMPIn As Bitmap, ByVal DestFormat As Imaging.PixelFormat)
        If BMPIn.PixelFormat = DestFormat Then
            Return
        Else
            If (DestFormat And Imaging.PixelFormat.Indexed) <> 0 Then
                ConvertToIndexedBitmap(BMPIn, DestFormat, GetDefaultGDIPalette(DestFormat))
                Return
            Else
                Dim BMPtmp As New Bitmap(BMPIn.Width, BMPIn.Height, DestFormat)
                BMPtmp.SetResolution(BMPIn.HorizontalResolution, BMPIn.VerticalResolution)

                Dim G As Graphics = Graphics.FromImage(BMPtmp)

                G.PageUnit = GraphicsUnit.Pixel
                G.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                G.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                G.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
                G.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed

                G.DrawImage(BMPIn, New Rectangle(0, 0, BMPIn.Width, BMPIn.Height))
                G.Dispose()

                BMPIn.Dispose()
                BMPIn = BMPtmp
                Return
            End If
        End If
    End Sub

    Friend Sub ConvertBitmap(ByRef BMPIn As Bitmap, ByVal DestFormat As Imaging.PixelFormat, ByVal GDIPalette() As GDI.RGBQUAD)
        If BMPIn.PixelFormat = DestFormat Then
            Return
        Else
            If (DestFormat And Imaging.PixelFormat.Indexed) <> 0 Then
                ConvertToIndexedBitmap(BMPIn, DestFormat, GDIPalette)
                Return
            Else
                Dim BMPtmp As New Bitmap(BMPIn.Width, BMPIn.Height, DestFormat)
                BMPtmp.SetResolution(BMPIn.HorizontalResolution, BMPIn.VerticalResolution)

                Dim G As Graphics = Graphics.FromImage(BMPtmp)

                G.PageUnit = GraphicsUnit.Pixel
                G.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                G.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                G.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
                G.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed

                G.DrawImage(BMPIn, New Rectangle(0, 0, BMPIn.Width, BMPIn.Height))
                G.Dispose()

                BMPIn.Dispose()
                BMPIn = BMPtmp
                Return
            End If
        End If
    End Sub

    ' Use GDI bitblt to invert a bitmap
    Friend Sub InvertBitmap(ByRef BMPIn As Bitmap)
        Dim hbmIn As IntPtr = BMPIn.GetHbitmap()
        Dim hbm As IntPtr = BMPIn.GetHbitmap()

        Dim scrnDC As IntPtr = USR.GetDC(IntPtr.Zero)
        Dim hDCIn As IntPtr = GDI.CreateCompatibleDC(scrnDC)

        Dim OldhbmIn As IntPtr = GDI.SelectObject(hDCIn, hbmIn)

        Dim hDC As IntPtr = GDI.CreateCompatibleDC(scrnDC)
        Dim Oldhbm As IntPtr = GDI.SelectObject(hDC, hbm)

        GDI.BitBlt(hDC, 0, 0, BMPIn.Width, BMPIn.Height, hDCIn, 0, 0, GDI.NOTSRCCOPY)

        Dim BMP As Bitmap = Bitmap.FromHbitmap(hbm)

        BMP.SetResolution(BMPIn.HorizontalResolution, BMPIn.VerticalResolution)

        GDI.DeleteObject(GDI.SelectObject(hDCIn, OldhbmIn))
        GDI.DeleteObject(GDI.SelectObject(hDC, Oldhbm))

        GDI.DeleteDC(hDCIn)
        GDI.DeleteDC(hDC)
        USR.ReleaseDC(IntPtr.Zero, scrnDC)

        BMPIn.Dispose()
        BMPIn = BMP
        Return

    End Sub

    Friend Sub GetGDIPalette(ByVal BMPIn As Bitmap, ByRef Pal() As GDI.RGBQUAD)
        Dim NumEntries As Integer = 0       ' number of palette entries
        Dim EntNo As Integer                ' loop counter

        Select Case BMPIn.PixelFormat
            Case Imaging.PixelFormat.Format1bppIndexed
                NumEntries = 2
            Case Imaging.PixelFormat.Format4bppIndexed
                NumEntries = 16
            Case Imaging.PixelFormat.Format8bppIndexed
                NumEntries = 156
        End Select
        If NumEntries > 0 Then
            Dim aRGB As Integer
            ReDim Pal(NumEntries - 1)
            For EntNo = 0 To NumEntries - 1
                aRGB = BMPIn.Palette.Entries(EntNo).ToArgb '- &HFF000000
                Pal(EntNo).rgbRed = CType(aRGB And &HFF, Byte)
                Pal(EntNo).rgbGreen = CType((aRGB >> 8) And &HFF, Byte)
                Pal(EntNo).rgbBlue = CType((aRGB >> 16) And &HFF, Byte)
            Next
        Else
            ReDim Pal(0)
        End If
    End Sub

    ' Detect the type of file by the magic numbers embedded in the file and
    ' return a string "BMP", "TIF", "GIF", "PDF", or ""
    Friend Function DetectFileType(ByVal FileName As String) As String
        ' Magic numbers embedded in files - these are mutually exclusive
        Const BMP_ID As Short = &H4D42                ' bitmap fileheader file type
        Const TIF_BO_LE As Short = &H4949             ' TIFF byte order little endian
        Const TIF_BO_BE As Short = &H4D4D             ' TIFF byte order big endian
        Const TIF_ID_LE As Short = &H2A               ' TIFF version little endian
        Const TIF_ID_BE As Short = &H2A00             ' TIFF version big endian
        Const GIF_ID1 As Short = &H4947               ' GIF 1st short
        Const GIF_ID2 As Short = &H3846               ' GIF 2nd short
        Const PDF_ID1 As Short = &H5025               ' PDF 1st short
        Const PDF_ID2 As Short = &H4644               ' PDF 2nd short

        Dim value As Short

        DetectFileType = ""

        On Error GoTo DFTErrs
        Using reader As New System.IO.BinaryReader(System.IO.File.Open(FileName, System.IO.FileMode.Open))
            value = reader.ReadInt16()
            Select Case value
                Case Is = BMP_ID
                    DetectFileType = "BMP"
                Case TIF_BO_LE
                    value = reader.ReadInt16()
                    If value = TIF_ID_LE Then DetectFileType = "TIF"
                Case TIF_BO_BE
                    value = reader.ReadInt16()
                    If value = TIF_ID_BE Then DetectFileType = "TIF"
                Case GIF_ID1
                    value = reader.ReadInt16()
                    If value = GIF_ID2 Then DetectFileType = "GIF"
                Case PDF_ID1
                    value = reader.ReadInt16()
                    If value = PDF_ID2 Then DetectFileType = "PDF"
            End Select
            reader.Close()
        End Using

        Return DetectFileType

DFTErrs:
        Return ""
    End Function

    Friend Sub FormatResults(ByVal ResEx_EG As TOCRRESULTSEX_EG, ByVal cntrl As RichTextBox)

        If ResEx_EG.Hdr.NumItems = 0 Then
            Return
        End If

        If ResultsFormatInfo.Fmt = RESULTSFMT.RTF Then
            FormatResultsNew(ResEx_EG, cntrl)
        Else
            FormatResultsOld(ResEx_EG, cntrl)
        End If
    End Sub

    Friend Sub FormatResultsOld(ByVal ResEx_EG As TOCRRESULTSEX_EG, ByVal cntrl As RichTextBox)
        Dim ItemNo As Integer       ' loop counter
        Dim WordNo As Integer       ' loop counter
        Dim LineNo As Integer       ' loop counter
        Dim NumWords As Integer     ' number of words found
        Dim NumWordsX As Integer    ' number of words found for X calculations
        Dim NumWordsY As Integer    ' number of words found for Y calculations
        Dim NumLines As Integer     ' number of text lines found
        Dim Siz As GDI.SDK_SIZE     ' API returned Size structure
        Dim FacX As Single          ' X pixel scale factor
        Dim FacY As Single          ' Y pixel scale factor
        Dim C1 As Char              ' a character
        Dim AscC1 As Integer        ' Ascii representation of C1
        Dim MinLineHt As Integer    ' minimum line height to include in calculations
        Dim MinPageX As Integer     ' minimum X on page
        Dim LineHt As Integer       ' estimated line height
        Dim XSpace As Integer       ' word spacing in image
        Dim YSpace As Integer       ' line spacing in image
        Dim PrevMaxY As Integer     ' Max Y of previous line in image
        Dim OCRText As String       ' text of results
        Dim XWrdPos As Integer      ' desired X position of word in textbox
        Dim Line As String          ' a line of text
        Dim LinePos As Integer      ' current length of Line
        Dim NumSpaces As Integer    ' number of spaces to place between words
        Dim PrevWord As Boolean     ' flag if have previous word on same line
        Dim XSpaceThresh As Single  ' threshold used to decide if to insert spaces
        Dim YSpaceThresh As Single  ' threshold used to decide if to insert a blank line
        Dim WI() As WORDINFO        ' information on words
        Dim abcarr() As GDI.ABCFLOAT   ' stored character ABC spacing structure (to save time)
        Dim txtarr As String
        Dim Done As Boolean         ' flag if formatted text
        Dim SpWid As Integer        ' width of a space
        Dim HHei As Integer         ' height of H
        Dim ZoomFactor As Single = cntrl.ZoomFactor

        Done = False

        If ResultsFormatInfo.Fmt = RESULTSFMT.Text Then
            ReDim WI(0 To 500)
            NumLines = 0
            NumWords = 0
            MinPageX = Integer.MaxValue
            For ItemNo = 0 To ResEx_EG.Hdr.NumItems - 1
                C1 = ChrW(ResEx_EG.Item(ItemNo).OCRCharWUnicode)
                If C1 = vbCr Or C1 = " " Then
                    If WI(NumWords).Txt <> "" Then
                        If NumWords >= WI.GetUpperBound(0) Then ReDim Preserve WI(0 To NumWords + 500)
                        NumWords += 1
                    End If
                    If C1 = vbCr Then NumLines += 1
                Else
                    If WI(NumWords).Txt = "" Then
                        WI(NumWords).ResultsInd = ItemNo
                        WI(NumWords).LineNo = NumLines
                        WI(NumWords).MinX = Integer.MaxValue
                        WI(NumWords).MaxX = Integer.MinValue
                        WI(NumWords).MinY = Integer.MaxValue
                        WI(NumWords).MaxY = Integer.MinValue
                    End If
                    WI(NumWords).Txt &= C1
                    WI(NumWords).MinX = Math.Min(WI(NumWords).MinX, ResEx_EG.Item(ItemNo).XPos)
                    WI(NumWords).MaxX = Math.Max(WI(NumWords).MaxX, ResEx_EG.Item(ItemNo).XPos + ResEx_EG.Item(ItemNo).XDim - 1)
                    WI(NumWords).MinY = Math.Min(WI(NumWords).MinY, ResEx_EG.Item(ItemNo).YPos)
                    WI(NumWords).MaxY = Math.Max(WI(NumWords).MaxY, ResEx_EG.Item(ItemNo).YPos + ResEx_EG.Item(ItemNo).YDim - 1)
                    MinPageX = Math.Min(MinPageX, ResEx_EG.Item(ItemNo).XPos)
                End If
            Next
            If C1 <> vbCr And C1 <> " " Then NumWords += 1

            ' Remove left margin
            If Not ResultsFormatInfo.LeftMargin Then
                For WordNo = 0 To NumWords - 1
                    WI(WordNo).MinX -= MinPageX
                    WI(WordNo).MaxX -= MinPageX
                Next
            End If

            On Error GoTo FRerrs
            Dim G As Graphics = cntrl.CreateGraphics()
            Dim hdc As IntPtr = G.GetHdc
            Dim oldFont As IntPtr = GDI.SelectObject(hdc, cntrl.Font.ToHfont)

            ' Find the leading and trailing white space and black extent of the textbox word
            ' (and store in WI(WordNo).ABC)

            MinLineHt = CInt(ResEx_EG.Hdr.YPixelsPerInch * 100 / 2.54 * 6 / 11811.0 + 0.49999)

            If GDI.GetTextExtentPoint32W(hdc, " ", 1, Siz) <> 0 Then
                SpWid = Siz.cx
            End If
            If GDI.GetTextExtentPoint32W(hdc, "H", 1, Siz) <> 0 Then
                HHei = Siz.cy
            End If

            ReDim abcarr(600)
            txtarr = ""
            Dim txtarrind As Integer

            NumWordsX = 0
            NumWordsY = 0
            FacX = 0
            FacY = 0

            For WordNo = 0 To NumWords - 1
                Siz.cx = 0
                If WI(WordNo).Txt.Length = 1 Then    ' just one character
                    C1 = CChar(WI(WordNo).Txt)
                    AscC1 = AscW(C1)
                    txtarrind = txtarr.IndexOf(C1)
                    If txtarrind >= 0 Then
                        WI(WordNo).abc = abcarr(txtarrind)
                        Siz.cx = CInt(abcarr(txtarrind).abcfB)
                    Else
                        txtarrind = txtarr.Length
                        If GDI.GetCharABCWidthsFloatW(hdc, AscC1, AscC1, abcarr(txtarrind)) <> 0 Then
                            txtarr &= C1
                            WI(WordNo).abc = abcarr(txtarrind)
                            Siz.cx = CInt(abcarr(txtarrind).abcfB)
                        End If
                    End If
                    Siz.cy = 0
                Else
                    ' Get extent of WI(WordNo).Text
                    ' Note GetTextExtentPoint32 always returns Siz.cy = frm.TextHeight("H")
                    If GDI.GetTextExtentPoint32W(hdc, WI(WordNo).Txt, WI(WordNo).Txt.Length, Siz) <> 0 Then
                        WI(WordNo).abc.abcfA = 0
                        WI(WordNo).abc.abcfB = Siz.cx
                        WI(WordNo).abc.abcfC = 0

                        ' Get leading and trailing space and trim .abcfB

                        C1 = CChar(WI(WordNo).Txt.Substring(0, 1))
                        AscC1 = AscW(C1)
                        txtarrind = txtarr.IndexOf(C1)
                        If txtarrind >= 0 Then
                            WI(WordNo).abc.abcfA = abcarr(txtarrind).abcfA
                            WI(WordNo).abc.abcfB -= abcarr(txtarrind).abcfA
                        Else
                            txtarrind = txtarr.Length
                            If GDI.GetCharABCWidthsFloatW(hdc, AscC1, AscC1, abcarr(txtarrind)) <> 0 Then
                                txtarr &= C1
                                WI(WordNo).abc.abcfA = abcarr(txtarrind).abcfA
                                WI(WordNo).abc.abcfB -= abcarr(txtarrind).abcfA
                            End If
                        End If

                        C1 = CChar(WI(WordNo).Txt.Substring(WI(WordNo).Txt.Length - 1))
                        AscC1 = AscW(C1)
                        txtarrind = txtarr.IndexOf(C1)
                        If txtarrind >= 0 Then
                            WI(WordNo).abc.abcfC = abcarr(txtarrind).abcfC
                            WI(WordNo).abc.abcfB -= abcarr(txtarrind).abcfC
                        Else
                            txtarrind = txtarr.Length
                            If GDI.GetCharABCWidthsFloatW(hdc, AscC1, AscC1, abcarr(txtarrind)) <> 0 Then
                                txtarr &= C1
                                WI(WordNo).abc.abcfC = abcarr(txtarrind).abcfC
                                WI(WordNo).abc.abcfB -= abcarr(txtarrind).abcfC
                            End If
                        End If
                        Siz.cx = CInt(WI(WordNo).abc.abcfB)
                    End If
                End If

                ' Calculate the X and Y scale factors between the image and the textbox

                If Siz.cx > 0 Then
                    NumWordsX += 1
                    FacX = CSng(FacX + Siz.cx / (WI(WordNo).MaxX - WI(WordNo).MinX + 1))
                    If Siz.cy > 0 And (WI(WordNo).MaxY - WI(WordNo).MinY + 1) >= MinLineHt Then
                        NumWordsY += 1
                        FacY = CSng(FacY + Siz.cy / (WI(WordNo).MaxY - WI(WordNo).MinY + 1))
                    End If
                End If
            Next WordNo

            ' If got some values then can format
            If NumWordsX > 0 And FacX > 0 Then
                FacX = FacX / NumWordsX
                If NumWordsY > 0 And FacY > 0 Then
                    FacY = FacY / NumWordsY
                Else
                    FacY = 2 * FacX     ' just approximate
                End If
                XSpaceThresh = SpWid * ResultsFormatInfo.MinXSpaceFac / FacX / HHei
                YSpaceThresh = ResultsFormatInfo.MinYSpaceFac / FacY * HHei
                LineHt = CInt(HHei / FacY * 1.1) ' * 1.1 because .TextHeight overestimates
                OCRText = ""
                PrevMaxY = WI(0).MaxY

                ' Initial white space

                If Not ResultsFormatInfo.CollapseY Then
                    YSpace = WI(0).MinY
                    If YSpace > YSpaceThresh Then
                        While YSpace > 0
                            OCRText &= Environment.NewLine
                            YSpace -= LineHt
                        End While
                    End If
                End If

                WordNo = 0
                LineNo = WI(WordNo).LineNo
                While WordNo < NumWords

                    ' If there is a big white space between lines then insert blank lines

                    If WordNo > 0 Then
                        If WI(WordNo - 1).LineNo <> WI(WordNo).LineNo Then
                            YSpace = WI(WordNo).MinY - PrevMaxY
                            If YSpace > YSpaceThresh Then
                                If ResultsFormatInfo.CollapseY Then
                                    OCRText &= Environment.NewLine
                                Else
                                    While YSpace > LineHt ' > LineHt because already have a CrlF on end of previous line
                                        OCRText &= Environment.NewLine
                                        YSpace -= LineHt
                                    End While
                                End If
                            End If

                            LineNo = WI(WordNo).LineNo
                            PrevMaxY = WI(WordNo).MaxY
                        End If
                    End If

                    ' Place word on line using spaces as necessary

                    Line = ""
                    LinePos = 0
                    Do
                        If WI(WordNo).LineNo <> LineNo Then Exit Do

                        If WordNo > 0 Then
                            PrevWord = (WI(WordNo - 1).LineNo = LineNo)
                        Else
                            PrevWord = False
                        End If
                        If PrevWord Then
                            XSpace = WI(WordNo).MinX - WI(WordNo - 1).MaxX
                            NumSpaces = 1
                        Else
                            XSpace = WI(WordNo).MinX
                            NumSpaces = 0
                        End If
                        If XSpace > XSpaceThresh * (WI(WordNo).MaxY - WI(WordNo).MinY + 1) Then
                            XWrdPos = CInt((WI(WordNo).MinX) * FacX - WI(WordNo).abc.abcfA)
                            If XWrdPos > LinePos Then
                                Dim s As String
                                s = Space$(1000)
                                If GDI.GetTextExtentExPointW(hdc, s, 1000, XWrdPos - LinePos, ItemNo, 0, Siz) <> 0 Then
                                    If ItemNo > 0 Then NumSpaces = ItemNo
                                End If
                            End If
                        End If
                        Line &= Space$(NumSpaces) & WI(WordNo).Txt
                        If GDI.GetTextExtentPoint32W(hdc, Line, Line.Length, Siz) <> 0 Then
                            LinePos = Siz.cx
                        End If
                        WordNo += 1
                    Loop While WordNo < NumWords
                    OCRText &= Line & Environment.NewLine
                    LineNo += 1
                End While
                If OCRText.Substring(OCRText.Length - 2) = Environment.NewLine Then OCRText = OCRText.Substring(0, OCRText.Length - 2)

                cntrl.Text = ""
                cntrl.Text = OCRText
                cntrl.ZoomFactor = 1.0 ' you need this, otherwise the next line is ignored
                cntrl.ZoomFactor = ZoomFactor

                Done = True
            End If ' NumWordsX
            GDI.DeleteObject(GDI.SelectObject(hdc, oldFont))
            G.ReleaseHdc(hdc)
            G.Dispose()
            On Error GoTo 0
            Return
FRerrs:
            GDI.DeleteObject(GDI.SelectObject(hdc, oldFont))
            G.ReleaseHdc(hdc)
            G.Dispose()
            MsgBox("Unexpected VB error" & Environment.NewLine & Environment.NewLine & Err.Description, MsgBoxStyle.Exclamation)
        End If

        If Not Done Then
            OCRText = ""
            With ResEx_EG
                If .Hdr.NumItems > 0 Then
                    For ItemNo = 0 To .Hdr.NumItems - 1
                        If ChrW(.Item(ItemNo).OCRCharWUnicode) = vbCr Then
                            OCRText &= Environment.NewLine
                        Else
                            OCRText &= ChrW(.Item(ItemNo).OCRCharWUnicode)
                        End If
                    Next ItemNo
                Else
                    MsgBox("No results returned", MsgBoxStyle.Information, "FormatResults")
                End If
            End With
            cntrl.ZoomFactor = 1.0 ' you need this, otherwise the next line is ignored
            cntrl.ZoomFactor = ZoomFactor
            cntrl.Text = OCRText
        End If
        Return
    End Sub

    ' Deal with Rich Text Formatting
    Friend Sub FormatResultsNew(ByVal ResEx_EG As TOCRRESULTSEX_EG, ByVal cntrl As RichTextBox)
        Const CrW As Integer = AscW(vbCr)
        Const SpaceW As Integer = AscW(" ")

        Dim ItemNo As Integer       ' loop counter
        Dim ItemNo2 As Integer      ' loop counter
        Dim WordNo As Integer       ' loop counter
        Dim WordNo2 As Integer      ' loop counter
        Dim LineNo As Integer       ' loop counter
        Dim NumWords As Integer     ' number of words found
        Dim NumLines As Integer     ' number of text lines found
        Dim C1 As Char              ' a character
        Dim MinPageX As Integer     ' minimum X on page
        Dim OCRText As String       ' text of results
        Dim WI() As WORDINFO        ' information on words
        Dim Done As Boolean         ' flag if formatted text
        Dim ZoomFactor As Single = cntrl.ZoomFactor

        Done = False

        If ResultsFormatInfo.Fmt = RESULTSFMT.RTF Then
            Const BARREDCHARS As String = vbCr & " ""',-.:;^_`~ˆ‘’""•¬­¯°²³´·¹º" ' characters not allowed to be used to estimate font size

            Dim G As Graphics = Nothing
            Dim hdc As IntPtr = IntPtr.Zero
            Dim OldFont As IntPtr = IntPtr.Zero
            Dim DefFont As New Font(cntrl.Font.Name, cntrl.Font.Size, cntrl.Font.Style) ' default font
            Dim FontName As String      ' name of a font
            Dim PrvFontName As String   ' previous name of a font
            Dim FontStyleInfo As UShort ' TOCR font style info
            Dim PrvFontStyleInfo As UShort ' previouse TOCR font style info
            Dim FSize As Single = cntrl.Font.Size   ' font size in points
            Dim PrvFSize As Single      ' previous font size

            Dim ItemGM() As GDI.GLYPHMETRICS ' metrics for each character
            Dim ItemFS() As Single      ' fontsize for each character
            Dim Ptr() As Integer        ' pointers for sort
            Dim Swap As Integer         ' used to swap an integer
            Dim DoIt As Boolean         ' utility flag
            Dim XScale As Single        ' X scale factor (image to control)
            Dim YScale As Single        ' Y scale factor (image to control)
            Dim Txt As String           ' utility string
            Dim FStyle As FontStyle = FontStyle.Regular ' utility
            Dim MAT As GDI.MAT2         ' used by GDI.GetGlyphOutlineW


            ' Do a pointed sort on characters to put into font/style/character order ignoring carriage return and space
            ' so that we can get all the glyphmetrics with the minimum of font changing

            ReDim Ptr(0 To ResEx_EG.Hdr.NumItems - 1)
            Trace.WriteLine("Start of FormatResultsNew")
            For ItemNo = 0 To ResEx_EG.Hdr.NumItems - 1
                Ptr(ItemNo) = ItemNo    ' used for pointed sort
                'Trace.WriteLine(FontNames(ResEx_EG.Item(ItemNo).FontID) & " " & ResEx_EG.Item(ItemNo).FontID)
                If ResEx_EG.Item(ItemNo).OCRCharWUnicode = CrW Then
                    Trace.WriteLine(" ")
                Else
                    Trace.Write(ChrW(AscW("0") + ResEx_EG.Item(ItemNo).FontID))
                End If
            Next

            Trace.WriteLine("Pointed sort")
            With ResEx_EG
                For ItemNo = 0 To .Hdr.NumItems - 2
                    If .Item(Ptr(ItemNo)).OCRCharWUnicode <> CrW And .Item(Ptr(ItemNo)).OCRCharWUnicode <> SpaceW Then
                        Trace.WriteLine(ItemNo & " " & ChrW(.Item(ItemNo).OCRCharWUnicode) & " " & .Item(ItemNo).FontStyleInfo)
                        For ItemNo2 = ItemNo + 1 To .Hdr.NumItems - 1
                            If .Item(Ptr(ItemNo2)).OCRCharWUnicode <> CrW And .Item(Ptr(ItemNo2)).OCRCharWUnicode <> SpaceW Then
                                DoIt = False
                                If .Item(Ptr(ItemNo)).FontID > .Item(Ptr(ItemNo2)).FontID Then
                                    DoIt = True
                                ElseIf .Item(Ptr(ItemNo)).FontID = .Item(Ptr(ItemNo2)).FontID Then
                                    If .Item(Ptr(ItemNo)).FontStyleInfo > .Item(Ptr(ItemNo2)).FontStyleInfo Then
                                        DoIt = True
                                    ElseIf .Item(Ptr(ItemNo)).FontStyleInfo = .Item(Ptr(ItemNo2)).FontStyleInfo Then
                                        If .Item(Ptr(ItemNo)).OCRCharWUnicode > .Item(Ptr(ItemNo2)).OCRCharWUnicode Then
                                            DoIt = True
                                        End If
                                    End If
                                End If
                                If DoIt Then Swap = Ptr(ItemNo) : Ptr(ItemNo) = Ptr(ItemNo2) : Ptr(ItemNo2) = Swap
                            End If
                        Next
                    End If
                Next

                ' Now get the glyphmetrics - in order to estimate font size

                MAT.eM11.Value = 1
                MAT.eM22.Value = 1

                ReDim ItemGM(0 To .Hdr.NumItems - 1)
                ReDim ItemFS(0 To .Hdr.NumItems - 1)

                On Error GoTo FRerrs
                G = cntrl.CreateGraphics()
                hdc = G.GetHdc

                OldFont = GDI.SelectObject(hdc, cntrl.Font.ToHfont)

                Dim ItemNo2FontStyleInfo As New UShort
                PrvFontName = "" 'cntrl.Font.Name
                PrvFontStyleInfo = CUShort(cntrl.Font.Style)
                For ItemNo = 0 To .Hdr.NumItems - 1
                    ItemNo2 = Ptr(ItemNo)
                    ItemFS(ItemNo2) = 0
                    ItemGM(ItemNo2).gmBlackBoxX = 0
                    If Not BARREDCHARS.Contains(ChrW(.Item(ItemNo2).OCRCharWUnicode)) Then
                        ' Reset Font if required
                        FontName = FontNames(.Item(ItemNo2).FontID)
                        ItemNo2FontStyleInfo = .Item(ItemNo2).FontStyleInfo
                        If FontName <> PrvFontName Or ItemNo2FontStyleInfo <> PrvFontStyleInfo Then
                            FStyle = FontStyle.Regular
                            'If (.Item(ItemNo2).FontStyleInfo And TOCRRESULTSFONT_BOLD) <> 0 Then FStyle = FStyle Or FontStyle.Bold - removed in V5
                            If (ItemNo2FontStyleInfo And TOCRRESULTSFONT_ITALIC) <> 0 Then FStyle = FStyle Or FontStyle.Italic
                            If (ItemNo2FontStyleInfo And TOCRRESULTSFONT_UNDERLINE) <> 0 Then FStyle = FStyle Or FontStyle.Underline
                            If Not OldFont.Equals(IntPtr.Zero) Then
                                GDI.DeleteObject(GDI.SelectObject(hdc, OldFont))
                                OldFont = IntPtr.Zero
                            End If

                            If FontName = "" Then
                                cntrl.Font = DefFont
                            Else
                                cntrl.Font = New Font(FontName, FSize, FStyle)
                            End If

                            OldFont = GDI.SelectObject(hdc, cntrl.Font.ToHfont)
                            PrvFontStyleInfo = ItemNo2FontStyleInfo
                            PrvFontName = FontName

                        End If

                        ' Because of sort order may already have the glypmetrics for this character - check
                        DoIt = True
                        If ItemNo > 0 Then
                            If .Item(Ptr(ItemNo - 1)).FontID = .Item(ItemNo2).FontID And .Item(Ptr(ItemNo - 1)).FontStyleInfo = .Item(ItemNo2).FontStyleInfo And .Item(Ptr(ItemNo - 1)).OCRCharWUnicode = .Item(ItemNo2).OCRCharWUnicode Then
                                DoIt = False
                            End If
                        End If
                        If DoIt Then
                            If GDI.GetGlyphOutlineW(hdc, .Item(ItemNo2).OCRCharWUnicode, 1, ItemGM(ItemNo2), 0, 0, MAT) > 0 Then
                                ItemFS(ItemNo2) = cntrl.Font.Size * CSng(.Item(ItemNo2).YDim) / CSng(ItemGM(ItemNo2).gmBlackBoxY)
                            End If
                        Else
                            ItemGM(ItemNo2) = ItemGM(Ptr(ItemNo - 1))
                            ItemFS(ItemNo2) = cntrl.Font.Size * CSng(.Item(ItemNo2).YDim) / CSng(ItemGM(ItemNo2).gmBlackBoxY)
                        End If
                    End If ' Not BARREDCHARS.Contains(ChrW(.Item(ItemNo2).OCRChaW))
                Next

                ' The glyphmetrics all refer to the wrong fontsize
                Erase ItemGM

                GDI.DeleteObject(GDI.SelectObject(hdc, OldFont))
                OldFont = IntPtr.Zero
                G.ReleaseHdc(hdc)
                hdc = IntPtr.Zero
                G = Nothing
                cntrl.Font = DefFont

                ' Break into words
                ReDim WI(0 To 500)
                NumLines = 0
                NumWords = 0
                MinPageX = Integer.MaxValue
                For ItemNo = 0 To .Hdr.NumItems - 1
                    C1 = ChrW(.Item(ItemNo).OCRCharWUnicode)
                    If C1 = vbCr Or C1 = " " Then
                        If WI(NumWords).Txt <> "" Then
                            If NumWords >= WI.GetUpperBound(0) Then ReDim Preserve WI(0 To NumWords + 500)
                            NumWords += 1
                        End If
                        If C1 = vbCr Then NumLines += 1
                    Else
                        If WI(NumWords).Txt = "" Then
                            WI(NumWords).ResultsInd = ItemNo
                            WI(NumWords).LineNo = NumLines
                            WI(NumWords).MinX = Integer.MaxValue
                            WI(NumWords).MaxX = Integer.MinValue
                            WI(NumWords).MinY = Integer.MaxValue
                            WI(NumWords).MaxY = Integer.MinValue
                            WI(NumWords).fsCnt = 0
                        End If
                        WI(NumWords).Txt &= C1
                        WI(NumWords).MinX = Math.Min(WI(NumWords).MinX, .Item(ItemNo).XPos)
                        WI(NumWords).MaxX = Math.Max(WI(NumWords).MaxX, .Item(ItemNo).XPos + .Item(ItemNo).XDim - 1)
                        WI(NumWords).MinY = Math.Min(WI(NumWords).MinY, .Item(ItemNo).YPos)
                        WI(NumWords).MaxY = Math.Max(WI(NumWords).MaxY, .Item(ItemNo).YPos + .Item(ItemNo).YDim - 1)
                        If ItemFS(ItemNo) > 0 Then
                            WI(NumWords).fs += ItemFS(ItemNo)
                            WI(NumWords).fsCnt += 1
                        End If

                        MinPageX = Math.Min(MinPageX, .Item(ItemNo).XPos)
                    End If
                Next
                If C1 <> vbCr And C1 <> " " Then NumWords += 1
                NumLines += 1

                If NumWords = 0 Then Return

                ' Smooth out the font size.  Make constant for a word.

                Const MINFONTSIZE As Integer = 4
                Const MAXFONTSIZE As Integer = 40
                Const FONTSIZECHANGERATIO As Single = 0.9   ' difference in estimated font size that will cause a change

                Dim FSRat As Single         ' fontsize ratio
                Dim Sum As Single           ' utility

                If .Hdr.XPixelsPerInch >= 25 Then
                    XScale = CSng(96 / .Hdr.XPixelsPerInch)
                Else
                    XScale = CSng(96 / 300)
                End If
                If .Hdr.YPixelsPerInch >= 25 Then
                    YScale = CSng(96 / .Hdr.YPixelsPerInch)
                Else
                    YScale = CSng(96 / 300)
                End If


                ' Deal with words that have no font size
                ItemNo = 0
                For WordNo = 0 To NumWords - 1
                    If WI(WordNo).fsCnt > 0 Then
                        WI(WordNo).fs /= WI(WordNo).fsCnt
                    Else
                        ItemNo += 1
                    End If
                Next
                If ItemNo = NumWords Then ' Failed to find any fontsizes so take the default
                    FSize = DefFont.Size
                    For WordNo = 0 To NumWords - 1
                        WI(WordNo).fs = FSize
                    Next
                Else ' set zero ones to near ones
                    FSize = 0
                    For WordNo = 0 To NumWords - 1
                        If WI(WordNo).fs = 0 Then
                            If FSize > 0 Then WI(WordNo).fs = FSize
                        Else
                            FSize = WI(WordNo).fs
                        End If
                    Next
                    For WordNo = NumWords - 1 To 0 Step -1
                        If WI(WordNo).fs = 0 Then
                            If FSize > 0 Then WI(WordNo).fs = FSize
                        Else
                            FSize = WI(WordNo).fs
                        End If
                    Next
                End If

                ItemNo = 0
                For WordNo = 1 To NumWords - 1
                    If WI(WordNo).fs > WI(ItemNo).fs Then
                        FSRat = WI(ItemNo).fs / WI(WordNo).fs
                    Else
                        FSRat = WI(WordNo).fs / WI(ItemNo).fs
                    End If
                    If FSRat < FONTSIZECHANGERATIO Then
                        Sum = 0
                        For WordNo2 = ItemNo To WordNo - 1
                            Sum += WI(WordNo2).fs
                        Next
                        Sum = Math.Max(MINFONTSIZE, Math.Min(MAXFONTSIZE, CInt(Sum * XScale / (WordNo - ItemNo))))
                        For WordNo2 = ItemNo To WordNo - 1
                            WI(WordNo2).fs = Sum
                        Next
                        ItemNo = WordNo
                    End If
                Next

                If ItemNo = NumWords - 1 Then
                    WI(ItemNo).fs = Math.Max(MINFONTSIZE, Math.Min(MAXFONTSIZE, CInt(WI(ItemNo).fs * XScale)))
                Else
                    Sum = 0
                    For WordNo2 = ItemNo To NumWords - 1
                        Sum += WI(WordNo2).fs
                    Next
                    Sum = Math.Max(MINFONTSIZE, Math.Min(MAXFONTSIZE, CInt(Sum * XScale / (NumWords - ItemNo))))
                    For WordNo2 = ItemNo To NumWords - 1
                        WI(WordNo2).fs = Sum
                    Next
                End If

                ' Set Word fontsizes into characters (for convenience)
                For WordNo = 0 To NumWords - 1
                    For ItemNo = WI(WordNo).ResultsInd To WI(WordNo).ResultsInd + WI(WordNo).Txt.Length - 1
                        ItemFS(ItemNo) = WI(WordNo).fs
                    Next
                Next

                FontName = FontNames(.Item(0).FontID)
                If FontName = "" Then FontName = DefFont.Name

                FontStyleInfo = .Item(0).FontStyleInfo
                FStyle = FontStyle.Regular
                'If (FontStyleInfo And TOCRRESULTSFONT_BOLD) <> 0 Then FStyle = FStyle Or FontStyle.Bold - removed in V5
                If (FontStyleInfo And TOCRRESULTSFONT_ITALIC) <> 0 Then FStyle = FStyle Or FontStyle.Italic
                If (FontStyleInfo And TOCRRESULTSFONT_UNDERLINE) <> 0 Then FStyle = FStyle Or FontStyle.Underline
                FSize = ItemFS(0)

                cntrl.Text = "" ' resets font Zoomfactor etc

                cntrl.SelectionFont = New Font(FontName, FSize, FStyle)

                PrvFontName = FontName
                PrvFontStyleInfo = FontStyleInfo
                PrvFSize = FSize

                ItemNo = 0

                Txt = ChrW(.Item(0).OCRCharWUnicode)
                ItemNo = 1
                While ItemNo < .Hdr.NumItems
                    If .Item(ItemNo).OCRCharWUnicode <> 13 Then

                        ' Decide if need to change the font
                        If .Item(ItemNo).OCRCharWUnicode = 32 Then ' for a space we only care about underline
                            FontName = PrvFontName
                            FSize = PrvFSize
                            If (.Item(ItemNo).FontStyleInfo And TOCRRESULTSFONT_UNDERLINE) = (PrvFontStyleInfo And TOCRRESULTSFONT_UNDERLINE) Then
                                FontStyleInfo = PrvFontStyleInfo
                            Else
                                FontStyleInfo = PrvFontStyleInfo Xor TOCRRESULTSFONT_UNDERLINE
                            End If
                        Else
                            FontName = FontNames(.Item(ItemNo).FontID)
                            If FontName = "" Then FontName = DefFont.Name
                            FSize = ItemFS(ItemNo)
                            FontStyleInfo = .Item(ItemNo).FontStyleInfo
                        End If

                        If FontName = PrvFontName And FontStyleInfo = PrvFontStyleInfo And FSize = PrvFSize Then
                            Txt &= ChrW(.Item(ItemNo).OCRCharWUnicode)
                        Else
                            cntrl.SelectedText = Txt

                            Txt = ChrW(.Item(ItemNo).OCRCharWUnicode)
                            FStyle = FontStyle.Regular
                            'If (FontStyleInfo And TOCRRESULTSFONT_BOLD) <> 0 Then FStyle = FStyle Or FontStyle.Bold - removed in V5
                            If (FontStyleInfo And TOCRRESULTSFONT_ITALIC) <> 0 Then FStyle = FStyle Or FontStyle.Italic
                            If (FontStyleInfo And TOCRRESULTSFONT_UNDERLINE) <> 0 Then FStyle = FStyle Or FontStyle.Underline

                            cntrl.SelectionFont = New Font(FontName, FSize, FStyle)

                            PrvFontName = FontName
                            PrvFontStyleInfo = FontStyleInfo
                            PrvFSize = FSize
                        End If
                    Else
                        Txt &= ChrW(.Item(ItemNo).OCRCharWUnicode)
                    End If

                    ItemNo += 1
                End While

                cntrl.SelectedText = Txt

                ' Put spacing in - first left margin and vertical spacing
                ' NOTE cntrl.MultiLine must be True and cntrl.WordWrap must be False for this to work

                For ItemNo = 0 To .Hdr.NumItems - 1
                    Ptr(ItemNo) = ItemNo
                Next
                If ResultsFormatInfo.LeftMargin Then MinPageX = 0

                Dim LMargin As Integer  ' amount of left margin required
                Dim NumCR As Integer    ' number of new lines to add for vertical spacing

                LineNo = NumLines - 1
                While LineNo >= 0

                    ItemNo = cntrl.GetFirstCharIndexFromLine(LineNo)
                    If ItemNo >= 0 Then
                        LMargin = CInt((.Item(ItemNo).XPos - MinPageX) * XScale)
                        cntrl.SelectionStart = ItemNo

                        Dim Gap As Single           ' size of vertical gap
                        Dim DoThis As Boolean       ' do this line (as opposed to the one above)
                        Dim HeightThis As Single    ' height of this line

                        ' Use the smaller of two lines surrounding a vertical gap to decide whether to insert lines
                        DoThis = True
                        HeightThis = cntrl.SelectionFont.GetHeight

                        If LineNo > 0 Then
                            cntrl.SelectionStart = ItemNo - 1
                            If cntrl.SelectionFont.GetHeight < HeightThis Then
                                DoThis = False
                            Else
                                cntrl.SelectionStart = ItemNo
                            End If

                            'Just use the first character from the previous line to estimate the size (approx)
                            ItemNo2 = cntrl.GetFirstCharIndexFromLine(LineNo - 1)
                            Gap = (CInt(.Item(ItemNo).YPos) - CInt(.Item(ItemNo2).YPos + .Item(ItemNo2).YDimRef)) * YScale
                        Else
                            Gap = .Item(ItemNo).YPos * YScale
                        End If
                        NumCR = 0
                        If Gap > cntrl.SelectionFont.GetHeight * ResultsFormatInfo.MinYSpaceFac Then
                            While Gap > cntrl.SelectionFont.GetHeight
                                NumCR += 1
                                Gap -= cntrl.SelectionFont.GetHeight
                            End While
                            If ResultsFormatInfo.CollapseY Then
                                If NumCR > 1 Then NumCR = 1
                            End If

                            ' The RichTextBox converts CrLf to LF.  Since Ptr() is to keep in step with the change just insert LFs
                            cntrl.SelectedText = StrDup(NumCR, vbLf)
                            If Not DoThis Then ItemNo -= 1
                            For ItemNo2 = ItemNo To .Hdr.NumItems - 1
                                Ptr(ItemNo2) += NumCR
                            Next
                        End If
                        ' Reset for left margin
                        If LMargin > 0 Then
                            ItemNo = cntrl.GetFirstCharIndexFromLine(LineNo + NumCR)
                            cntrl.SelectionStart = ItemNo
                        End If
                        NumLines += NumCR

                        If LMargin > 0 Then
                            ' Turn off underline
                            If (cntrl.SelectionFont.Style And FontStyle.Underline) <> 0 Then cntrl.SelectionFont = New Font(cntrl.SelectionFont.Name, cntrl.SelectionFont.Size, FontStyle.Regular)
                            cntrl.SelectionTabs = New Integer() {LMargin}
                            Txt = vbTab
                            cntrl.SelectedText = Txt
                            For ItemNo2 = ItemNo To .Hdr.NumItems - 1
                                Ptr(ItemNo2) += 1
                            Next
                        End If
                    End If
                    LineNo -= 1
                End While

                ' Now other spaces

                For ItemNo = 0 To .Hdr.NumItems - 1
                    If .Item(ItemNo).OCRCharWUnicode = SpaceW Then
                        If ItemNo > 0 And ItemNo < .Hdr.NumItems - 1 Then
                            ' ItemNo2 indexes into the cntrl text array (which no longer synchs with the item array after tabs  and newlines have been added)
                            ItemNo2 = Ptr(ItemNo)
                            If cntrl.GetLineFromCharIndex(ItemNo2 - 1) = cntrl.GetLineFromCharIndex(ItemNo2 + 1) Then
                                ' Decide if a space is significant
                                If cntrl.GetPositionFromCharIndex(ItemNo2 + 1).X < (.Item(ItemNo + 1).XPos - MinPageX) * XScale Then
                                    ' Allow for larger than normal spaces after a full stop
                                    Dim XSpaceFac As Single = ResultsFormatInfo.MinXSpaceFac
                                    If .Item(ItemNo - 1).OCRCharWUnicode = &H2E Then XSpaceFac += 1
                                    If .Item(ItemNo + 1).XPos - .Item(ItemNo - 1).XPos - .Item(ItemNo - 1).XDim > Math.Max(.Item(ItemNo - 1).YDimRef, .Item(ItemNo + 1).YDimRef) * XSpaceFac Then
                                        cntrl.SelectionStart = ItemNo2
                                        ' turn off underline
                                        If (cntrl.SelectionFont.Style And FontStyle.Underline) <> 0 Then cntrl.SelectionFont = New Font(cntrl.SelectionFont.Name, cntrl.SelectionFont.Size, FontStyle.Regular)
                                        cntrl.SelectionLength = 1
                                        If cntrl.SelectionTabs.Length = 0 Then
                                            cntrl.SelectionTabs = New Integer() {CInt((.Item(ItemNo + 1).XPos - MinPageX) * XScale)}
                                        Else
                                            If cntrl.SelectionTabs.Length < 32 Then ' system limitation
                                                Dim itb() As Integer = cntrl.SelectionTabs
                                                ReDim Preserve itb(cntrl.SelectionTabs.Length)
                                                itb(cntrl.SelectionTabs.Length) = CInt((.Item(ItemNo + 1).XPos - MinPageX) * XScale)
                                                cntrl.SelectionTabs = itb
                                            End If
                                        End If
                                        cntrl.SelectedText = vbTab
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

                cntrl.SelectionFont = DefFont
            End With

            cntrl.ZoomFactor = 1.0 ' you need this, otherwise the next line is ignored
            cntrl.ZoomFactor = ZoomFactor
            Return
FRerrs:
            If Not OldFont.Equals(IntPtr.Zero) Then GDI.DeleteObject(GDI.SelectObject(hdc, OldFont))
            If Not hdc.Equals(IntPtr.Zero) Then G.ReleaseHdc(hdc)
            If Not G Is Nothing Then G.Dispose()
            cntrl.ZoomFactor = ZoomFactor
            MsgBox("Unexpected VB error" & Environment.NewLine & Environment.NewLine & Err.Description, MsgBoxStyle.Exclamation)
        End If

        If Not Done Then
            OCRText = ""
            With ResEx_EG
                If .Hdr.NumItems > 0 Then
                    For ItemNo = 0 To .Hdr.NumItems - 1
                        If ChrW(.Item(ItemNo).OCRCharWUnicode) = vbCr Then
                            OCRText &= Environment.NewLine
                        Else
                            OCRText &= ChrW(.Item(ItemNo).OCRCharWUnicode)
                        End If
                    Next ItemNo
                End If
            End With
            cntrl.Text = OCRText
            cntrl.ZoomFactor = 1.0 ' you need this, otherwise the next line is ignored
            cntrl.ZoomFactor = ZoomFactor
        End If
        Return
    End Sub

#If SUPERSEDED Then
    ' copy of TOCRRESULTSITEMEX without the Alt[] array 
    Private Structure TOCRRESULTSITEMEXHDR
        Dim StructId As Short
        Dim OCRCha As Short
        Dim Confidence As Single
        Dim XPos As Short
        Dim YPos As Short
        Dim XDim As Short
        Dim YDim As Short
    End Structure

    ' Function to retrieve the results from the service process and load into 'ResultsEx'
    ' Remember the character numbers returned refer to the Windows character set.
    Friend Function GetResults(ByVal JobNo As Integer, ByRef ResultsEx As TOCRRESULTSEX) As Boolean
        Dim ResultsInf As Integer ' number of bytes needed for results
        Dim AddrOfItemBytes As System.IntPtr
        Dim ItemNo As Integer           ' loop counter
        Dim AltNo As Integer            ' loop counter
        Dim Bytes() As Byte             ' array of bytes of returned results
        Dim BytesGC As GCHandle         ' handle ti pin Bytes()
        Dim Offset As Integer           ' address offset into Bytes()
        Dim ItemHdr As TOCRRESULTSITEMEXHDR

        GetResults = False
        ResultsEx.Hdr.NumItems = 0
        If TOCRGetJobResultsEx(JobNo, TOCRGETRESULTS_EXTENDED, ResultsInf, IntPtr.Zero) = TOCR_OK Then
            If ResultsInf > 0 Then
                ReDim Bytes(ResultsInf - 1)
                ' pin the Bytes array so that TOCRGetJobResultsEx can write to it
                BytesGC = GCHandle.Alloc(Bytes, GCHandleType.Pinned)

                If TOCRGetJobResultsEx(JobNo, TOCRGETRESULTS_EXTENDED, ResultsInf, BytesGC.AddrOfPinnedObject) = TOCR_OK Then
                    With ResultsEx
                        .Hdr = CType(Marshal.PtrToStructure(BytesGC.AddrOfPinnedObject, GetType(TOCRRESULTSHEADER)), TOCRRESULTSHEADER)
                        If .Hdr.NumItems > 0 Then
                            ReDim .Item(.Hdr.NumItems - 1)
                            Offset = Marshal.SizeOf(GetType(TOCRRESULTSHEADER))
                            For ItemNo = 0 To .Hdr.NumItems - 1
                                AddrOfItemBytes = Marshal.UnsafeAddrOfPinnedArrayElement(Bytes, Offset)

                                ' Cannot Marshal TOCRRESULTSITEMEX so use copy of structure header
                                ' This unfortunately means a double copy of the data
                                ItemHdr = CType(Marshal.PtrToStructure(AddrOfItemBytes, GetType(TOCRRESULTSITEMEXHDR)), TOCRRESULTSITEMEXHDR)
                                With .Item(ItemNo)
                                    .Initialize()
                                    .StructId = ItemHdr.StructId
                                    .OCRCha = ItemHdr.OCRCha
                                    .Confidence = ItemHdr.Confidence
                                    .XPos = ItemHdr.XPos
                                    .YPos = ItemHdr.YPos
                                    .XDim = ItemHdr.XDim
                                    .YDim = ItemHdr.YDim
                                    Offset = Offset + Marshal.SizeOf(GetType(TOCRRESULTSITEMEXHDR))
                                    For AltNo = 0 To 4
                                        AddrOfItemBytes = Marshal.UnsafeAddrOfPinnedArrayElement(Bytes, Offset)
                                        .Alt(AltNo) = CType(Marshal.PtrToStructure(AddrOfItemBytes, GetType(TOCRRESULTSITEMEXALT)), TOCRRESULTSITEMEXALT)
                                        Offset = Offset + Marshal.SizeOf(GetType(TOCRRESULTSITEMEXALT))
                                    Next AltNo
                                End With
                            Next ItemNo
                        End If ' .Hdr.NumItems > 0

                        GetResults = True

                    End With ' results
                End If ' TOCRGetJobResults(JobNo, ResultsInf, Bytes(0)) = TOCR_OK

                BytesGC.Free()

            End If ' ResultsInf > 0
        End If ' TOCRGetJobResults(JobNo, ResultsInf, 0) = TOCR_OK
        Return GetResults
    End Function
#End If

    ' copy of TOCRRESULTSITEMEX_EG without the Alt[] array 
    Private Structure TOCRRESULTSITEMEXHDR_EG
        Dim Confidence As Single
        Dim StructId As UShort
        Dim OCRCharWUnicode As UShort 'V5 split from OCRChaW
        Dim OCRCharWInternal As UShort 'V5 split from OCRChaW
        Dim FontID As UShort
        Dim FontStyleInfo As UShort
        Dim XPos As UShort
        Dim YPos As UShort
        Dim XDim As UShort
        Dim YDim As UShort
        Dim YDimRef As UShort
        Dim Noise As UShort 'V5 addition
    End Structure

    ' Function to retrieve the results from the service process and load into 'Results_EG'
    Friend Function GetResults_EG(ByVal JobNo As Integer, ByRef Results_EG As TOCRRESULTS_EG) As Boolean
        Dim ResultsInf As Integer ' number of bytes needed for results
        Dim AddrOfItemBytes As System.IntPtr
        Dim ItemNo As Integer           ' loop counter
        Dim Bytes() As Byte             ' array of bytes of returned results
        Dim BytesGC As GCHandle         ' handle ti pin Bytes()
        Dim Offset As Integer           ' address offset into Bytes()

        GetResults_EG = False
        Results_EG.Hdr.NumItems = 0
        If TOCRGetJobResultsEx_EG(JobNo, TOCRGETRESULTS_NORMAL_EG, ResultsInf, IntPtr.Zero) = TOCR_OK Then
            If ResultsInf > 0 Then
                ReDim Bytes(ResultsInf - 1)
                ' pin the Bytes array so that TOCRGetJobResultsEx can write to it
                BytesGC = GCHandle.Alloc(Bytes, GCHandleType.Pinned)

                If TOCRGetJobResultsEx_EG(JobNo, TOCRGETRESULTS_NORMAL_EG, ResultsInf, BytesGC.AddrOfPinnedObject) = TOCR_OK Then
                    With Results_EG
                        .Hdr = CType(Marshal.PtrToStructure(BytesGC.AddrOfPinnedObject, GetType(TOCRRESULTSHEADER_EG)), TOCRRESULTSHEADER_EG)
                        If .Hdr.NumItems > 0 Then
                            ReDim .Item(.Hdr.NumItems - 1)
                            Offset = Marshal.SizeOf(GetType(TOCRRESULTSHEADER_EG))
                            For ItemNo = 0 To .Hdr.NumItems - 1
                                AddrOfItemBytes = Marshal.UnsafeAddrOfPinnedArrayElement(Bytes, Offset)
                                .Item(ItemNo) = CType(Marshal.PtrToStructure(AddrOfItemBytes, GetType(TOCRRESULTSITEM_EG)), TOCRRESULTSITEM_EG)
                                Offset = Offset + Marshal.SizeOf(GetType(TOCRRESULTSITEM_EG))
                            Next ItemNo
                        End If ' .Hdr.NumItems > 0

                        GetResults_EG = True

                    End With ' results
                End If ' TOCRGetJobResults(JobNo, ResultsInf, Bytes(0)) = TOCR_OK

                BytesGC.Free()

            End If ' ResultsInf > 0
        End If ' TOCRGetJobResults(JobNo, ResultsInf, 0) = TOCR_OK
        Return GetResults_EG
    End Function

    ' Function to retrieve the processed image from the service process and load into 'ProcessedImmage'
    Friend Function ExtraInfGetMMF(ByVal JobNo As Integer, ByRef MMF As System.IntPtr) As Boolean
        Dim Success As Boolean
        Dim test_fn As Integer
        Success = False
        test_fn = TOCRExtraInfGetMMF(JobNo, TOCREXTRAINF_RETURNBITMAP1, MMF)
        If test_fn = TOCR_OK Then
            Success = True
        End If
        Return Success
    End Function


    ' Function to retrieve the results from the service process and load into 'ResultsEx_EG'
    Friend Function GetResults_EG(ByVal JobNo As Integer, ByRef ResultsEx_EG As TOCRRESULTSEX_EG) As Boolean
        Dim ResultsInf As Integer ' number of bytes needed for results
        Dim AddrOfItemBytes As System.IntPtr
        Dim ItemNo As Integer           ' loop counter
        Dim AltNo As Integer            ' loop counter
        Dim Bytes() As Byte             ' array of bytes of returned results
        Dim BytesGC As GCHandle         ' handle ti pin Bytes()
        Dim Offset As Integer           ' address offset into Bytes()
        Dim ItemHdr As TOCRRESULTSITEMEXHDR_EG

        GetResults_EG = False
        ResultsEx_EG.Hdr.NumItems = 0
        If TOCRGetJobResultsEx_EG(JobNo, TOCRGETRESULTS_EXTENDED_EG, ResultsInf, IntPtr.Zero) = TOCR_OK Then
            If ResultsInf > 0 Then
                ReDim Bytes(ResultsInf - 1)
                ' pin the Bytes array so that TOCRGetJobResultsEx can write to it
                BytesGC = GCHandle.Alloc(Bytes, GCHandleType.Pinned)

                If TOCRGetJobResultsEx_EG(JobNo, TOCRGETRESULTS_EXTENDED_EG, ResultsInf, BytesGC.AddrOfPinnedObject) = TOCR_OK Then
                    With ResultsEx_EG
                        .Hdr = CType(Marshal.PtrToStructure(BytesGC.AddrOfPinnedObject, GetType(TOCRRESULTSHEADER_EG)), TOCRRESULTSHEADER_EG)
                        If .Hdr.NumItems > 0 Then
                            ReDim .Item(.Hdr.NumItems - 1)
                            Offset = Marshal.SizeOf(GetType(TOCRRESULTSHEADER_EG))
                            For ItemNo = 0 To .Hdr.NumItems - 1
                                AddrOfItemBytes = Marshal.UnsafeAddrOfPinnedArrayElement(Bytes, Offset)
                                ' Cannot Marshal TOCRRESULTSITEMEX so use copy of structure header
                                ' This unfortunately means a double copy of the data
                                ItemHdr = CType(Marshal.PtrToStructure(AddrOfItemBytes, GetType(TOCRRESULTSITEMEXHDR_EG)), TOCRRESULTSITEMEXHDR_EG)
                                With .Item(ItemNo)
                                    .Initialize()
                                    .StructId = ItemHdr.StructId
                                    .OCRCharWUnicode = ItemHdr.OCRCharWUnicode
                                    .Confidence = ItemHdr.Confidence
                                    .FontStyleInfo = ItemHdr.FontStyleInfo
                                    .FontID = ItemHdr.FontID
                                    .XPos = ItemHdr.XPos
                                    .YPos = ItemHdr.YPos
                                    .XDim = ItemHdr.XDim
                                    .YDim = ItemHdr.YDim
                                    .YDimRef = ItemHdr.YDimRef
                                    Offset = Offset + Marshal.SizeOf(GetType(TOCRRESULTSITEMEXHDR_EG))
                                    For AltNo = 0 To 4
                                        AddrOfItemBytes = Marshal.UnsafeAddrOfPinnedArrayElement(Bytes, Offset)
                                        .Alt(AltNo) = CType(Marshal.PtrToStructure(AddrOfItemBytes, GetType(TOCRRESULTSITEMEXALT_EG)), TOCRRESULTSITEMEXALT_EG)
                                        Offset = Offset + Marshal.SizeOf(GetType(TOCRRESULTSITEMEXALT_EG))
                                    Next AltNo
                                End With
                            Next ItemNo
                        End If ' .Hdr.NumItems > 0

                        GetResults_EG = True

                    End With ' results
                End If ' TOCRGetJobResults_EG(JobNo, ResultsInf, Bytes(0)) = TOCR_OK

                BytesGC.Free()

            End If ' ResultsInf > 0
        End If ' TOCRGetJobResults_EG(JobNo, ResultsInf, 0) = TOCR_OK

        Return GetResults_EG
    End Function
#End Region

#Region " Private routines "
    Private Sub ConvertToIndexedBitmap(ByRef BMPIn As Bitmap, ByVal Format As Imaging.PixelFormat, ByVal GDIPalette() As GDI.RGBQUAD)
        Dim dummy As IntPtr
        Dim hbm As IntPtr
        Dim Oldhbm As IntPtr
        Dim BitsPerPixel As Integer
        Dim NumColors As Integer            ' number of colors in palette
        Dim EntNo As Integer                ' loop counter

        Select Case Format
            Case Imaging.PixelFormat.Format1bppIndexed
                Dim bmi As New GDI.BITMAPINFO1
                BitsPerPixel = 1
                NumColors = 2
                If GDIPalette.GetUpperBound(0) + 1 <> NumColors Then GDIPalette = GetDefaultGDIPalette(Imaging.PixelFormat.Format1bppIndexed)
                ReDim bmi.clrs(NumColors - 1)
                For EntNo = 0 To NumColors - 1
                    bmi.clrs(EntNo) = GDIPalette(EntNo)
                Next
                bmi.bmih.biSize = Marshal.SizeOf(bmi.bmih)
                bmi.bmih.biWidth = BMPIn.Width
                bmi.bmih.biHeight = BMPIn.Height
                bmi.bmih.biPlanes = 1
                bmi.bmih.biBitCount = CType(BitsPerPixel, Short)
                bmi.bmih.biCompression = GDI.BI_RGB
                bmi.bmih.biSizeImage = ((((BMPIn.Width * BitsPerPixel + 31) >> 5) << 2) * Math.Abs(BMPIn.Height))
                bmi.bmih.biXPelsPerMeter = CType(BMPIn.HorizontalResolution * 100 / 2.54, Int32)
                bmi.bmih.biYPelsPerMeter = CType(BMPIn.VerticalResolution * 100 / 2.54, Int32)
                bmi.bmih.biClrUsed = NumColors
                bmi.bmih.biClrImportant = NumColors
                hbm = GDI.CreateDIBSection(IntPtr.Zero, bmi, CType(GDI.DIB_RGB_COLORS, UInt32), dummy, IntPtr.Zero, CType(0, UInt32))
            Case Imaging.PixelFormat.Format4bppIndexed
                Dim bmi As New GDI.BITMAPINFO4
                BitsPerPixel = 4
                NumColors = 16
                If GDIPalette.GetUpperBound(0) + 1 <> NumColors Then GDIPalette = GetDefaultGDIPalette(Imaging.PixelFormat.Format4bppIndexed)
                ReDim bmi.clrs(NumColors - 1)
                For EntNo = 0 To NumColors - 1
                    bmi.clrs(EntNo) = GDIPalette(EntNo)
                Next
                bmi.bmih.biSize = Marshal.SizeOf(bmi.bmih)
                bmi.bmih.biWidth = BMPIn.Width
                bmi.bmih.biHeight = BMPIn.Height
                bmi.bmih.biPlanes = 1
                bmi.bmih.biBitCount = CType(BitsPerPixel, Short)
                bmi.bmih.biCompression = GDI.BI_RGB
                bmi.bmih.biSizeImage = ((((BMPIn.Width * BitsPerPixel + 31) >> 5) << 2) * BMPIn.Height)
                bmi.bmih.biXPelsPerMeter = CType(BMPIn.HorizontalResolution * 100 / 2.54, Int32)
                bmi.bmih.biYPelsPerMeter = CType(BMPIn.VerticalResolution * 100 / 2.54, Int32)
                bmi.bmih.biClrUsed = NumColors
                bmi.bmih.biClrImportant = NumColors
                hbm = GDI.CreateDIBSection(IntPtr.Zero, bmi, CType(GDI.DIB_RGB_COLORS, UInt32), dummy, IntPtr.Zero, CType(0, UInt32))
            Case Imaging.PixelFormat.Format8bppIndexed
                Dim bmi As New GDI.BITMAPINFO8
                BitsPerPixel = 8
                NumColors = 256
                If GDIPalette.GetUpperBound(0) + 1 <> NumColors Then GDIPalette = GetDefaultGDIPalette(Imaging.PixelFormat.Format8bppIndexed)
                ReDim bmi.clrs(NumColors - 1)  ' see the definition of BITMAPINFO()
                For EntNo = 0 To NumColors - 1
                    bmi.clrs(EntNo) = GDIPalette(EntNo)
                Next
                bmi.bmih.biSize = Marshal.SizeOf(bmi.bmih)
                bmi.bmih.biWidth = BMPIn.Width
                bmi.bmih.biHeight = BMPIn.Height
                bmi.bmih.biPlanes = 1
                bmi.bmih.biBitCount = CType(BitsPerPixel, Short)
                bmi.bmih.biCompression = GDI.BI_RGB
                bmi.bmih.biSizeImage = ((((BMPIn.Width * BitsPerPixel + 31) >> 5) << 2) * BMPIn.Height)
                bmi.bmih.biXPelsPerMeter = CType(BMPIn.HorizontalResolution * 100 / 2.54, Int32)
                bmi.bmih.biYPelsPerMeter = CType(BMPIn.VerticalResolution * 100 / 2.54, Int32)
                bmi.bmih.biClrUsed = NumColors
                bmi.bmih.biClrImportant = NumColors
                hbm = GDI.CreateDIBSection(IntPtr.Zero, bmi, CType(GDI.DIB_RGB_COLORS, UInt32), dummy, IntPtr.Zero, CType(0, UInt32))
            Case Else
                Return
        End Select

        Dim hbmIn As IntPtr = BMPIn.GetHbitmap()
        Dim OldhbmIn As IntPtr

        Dim scrnDC As IntPtr = USR.GetDC(IntPtr.Zero)
        Dim hDCIn As IntPtr = GDI.CreateCompatibleDC(scrnDC)

        OldhbmIn = GDI.SelectObject(hDCIn, hbmIn)
        Dim hDC As IntPtr = GDI.CreateCompatibleDC(scrnDC)
        Oldhbm = GDI.SelectObject(hDC, hbm)

        GDI.BitBlt(hDC, 0, 0, BMPIn.Width, BMPIn.Height, hDCIn, 0, 0, GDI.SRCCOPY)

        Dim BMPtmp As Bitmap = Bitmap.FromHbitmap(hbm)

        BMPtmp.SetResolution(BMPIn.HorizontalResolution, BMPIn.VerticalResolution)

        GDI.DeleteObject(GDI.SelectObject(hDCIn, OldhbmIn))
        GDI.DeleteObject(GDI.SelectObject(hDC, Oldhbm))
        GDI.DeleteDC(hDCIn)
        GDI.DeleteDC(hDC)
        USR.ReleaseDC(IntPtr.Zero, scrnDC)

        GDI.GdiFlush()

        BMPIn.Dispose()
        BMPIn = Nothing
        BMPIn = BMPtmp

        Return
    End Sub

    Private Function GetDefaultGDIPalette(ByRef Format As Imaging.PixelFormat) As GDI.RGBQUAD()
        Dim Pal() As GDI.RGBQUAD            ' new GDI Palette
        Dim EntNo As Integer                ' loop counter

        Select Case Format
            Case Imaging.PixelFormat.Format1bppIndexed
                Dim RGB1(,) As Byte = {{0, 0, 0}, {255, 255, 255}}
                ReDim Pal(1)
                For EntNo = 0 To 1
                    Pal(EntNo).rgbRed = RGB1(EntNo, 0)
                    Pal(EntNo).rgbGreen = RGB1(EntNo, 1)
                    Pal(EntNo).rgbBlue = RGB1(EntNo, 2)
                    Pal(EntNo).rgbReserved = 0
                Next
            Case Imaging.PixelFormat.Format4bppIndexed
                Dim RGB4(,) As Byte = {{0, 0, 0}, {128, 0, 0}, {0, 128, 0}, {128, 128, 0}, {0, 0, 128}, {128, 0, 128}, {0, 128, 128}, {128, 128, 128}, {192, 192, 192}, {255, 0, 0}, {0, 255, 0}, {255, 255, 0}, {0, 0, 255}, {255, 0, 255}, {0, 255, 255}, {255, 255, 255}}
                ReDim Pal(15)
                For EntNo = 0 To 15
                    Pal(EntNo).rgbRed = RGB4(EntNo, 0)
                    Pal(EntNo).rgbGreen = RGB4(EntNo, 1)
                    Pal(EntNo).rgbBlue = RGB4(EntNo, 2)
                    Pal(EntNo).rgbReserved = 0
                Next
            Case Imaging.PixelFormat.Format8bppIndexed
                Dim RGB8(,) As Byte = {{0, 0, 0}, {128, 0, 0}, {0, 128, 0}, {128, 128, 0}, {0, 0, 128}, {128, 0, 128}, {0, 128, 128}, {192, 192, 192}, {192, 220, 192}, {166, 202, 240}, {64, 32, 0}, {96, 32, 0}, {128, 32, 0}, {160, 32, 0}, {192, 32, 0}, {224, 32, 0}, {0, 64, 0}, _
                   {32, 64, 0}, {64, 64, 0}, {96, 64, 0}, {128, 64, 0}, {160, 64, 0}, {192, 64, 0}, {224, 64, 0}, {0, 96, 0}, {32, 96, 0}, {64, 96, 0}, {96, 96, 0}, {128, 96, 0}, {160, 96, 0}, {192, 96, 0}, {224, 96, 0}, {0, 128, 0}, _
                   {32, 128, 0}, {64, 128, 0}, {96, 128, 0}, {128, 128, 0}, {160, 128, 0}, {192, 128, 0}, {224, 128, 0}, {0, 160, 0}, {32, 160, 0}, {64, 160, 0}, {96, 160, 0}, {128, 160, 0}, {160, 160, 0}, {192, 160, 0}, {224, 160, 0}, {0, 192, 0}, _
                   {32, 192, 0}, {64, 192, 0}, {96, 192, 0}, {128, 192, 0}, {160, 192, 0}, {192, 192, 0}, {224, 192, 0}, {0, 224, 0}, {32, 224, 0}, {64, 224, 0}, {96, 224, 0}, {128, 224, 0}, {160, 224, 0}, {192, 224, 0}, {224, 224, 0}, {0, 0, 64}, _
                   {32, 0, 64}, {64, 0, 64}, {96, 0, 64}, {128, 0, 64}, {160, 0, 64}, {192, 0, 64}, {224, 0, 64}, {0, 32, 64}, {32, 32, 64}, {64, 32, 64}, {96, 32, 64}, {128, 32, 64}, {160, 32, 64}, {192, 32, 64}, {224, 32, 64}, {0, 64, 64}, _
                   {32, 64, 64}, {64, 64, 64}, {96, 64, 64}, {128, 64, 64}, {160, 64, 64}, {192, 64, 64}, {224, 64, 64}, {0, 96, 64}, {32, 96, 64}, {64, 96, 64}, {96, 96, 64}, {128, 96, 64}, {160, 96, 64}, {192, 96, 64}, {224, 96, 64}, {0, 128, 64}, _
                   {32, 128, 64}, {64, 128, 64}, {96, 128, 64}, {128, 128, 64}, {160, 128, 64}, {192, 128, 64}, {224, 128, 64}, {0, 160, 64}, {32, 160, 64}, {64, 160, 64}, {96, 160, 64}, {128, 160, 64}, {160, 160, 64}, {192, 160, 64}, {224, 160, 64}, {0, 192, 64}, _
                   {32, 192, 64}, {64, 192, 64}, {96, 192, 64}, {128, 192, 64}, {160, 192, 64}, {192, 192, 64}, {224, 192, 64}, {0, 224, 64}, {32, 224, 64}, {64, 224, 64}, {96, 224, 64}, {128, 224, 64}, {160, 224, 64}, {192, 224, 64}, {224, 224, 64}, {0, 0, 128}, _
                   {32, 0, 128}, {64, 0, 128}, {96, 0, 128}, {128, 0, 128}, {160, 0, 128}, {192, 0, 128}, {224, 0, 128}, {0, 32, 128}, {32, 32, 128}, {64, 32, 128}, {96, 32, 128}, {128, 32, 128}, {160, 32, 128}, {192, 32, 128}, {224, 32, 128}, {0, 64, 128}, _
                   {32, 64, 128}, {64, 64, 128}, {96, 64, 128}, {128, 64, 128}, {160, 64, 128}, {192, 64, 128}, {224, 64, 128}, {0, 96, 128}, {32, 96, 128}, {64, 96, 128}, {96, 96, 128}, {128, 96, 128}, {160, 96, 128}, {192, 96, 128}, {224, 96, 128}, {0, 128, 128}, _
                   {32, 128, 128}, {64, 128, 128}, {96, 128, 128}, {128, 128, 128}, {160, 128, 128}, {192, 128, 128}, {224, 128, 128}, {0, 160, 128}, {32, 160, 128}, {64, 160, 128}, {96, 160, 128}, {128, 160, 128}, {160, 160, 128}, {192, 160, 128}, {224, 160, 128}, {0, 192, 128}, _
                   {32, 192, 128}, {64, 192, 128}, {96, 192, 128}, {128, 192, 128}, {160, 192, 128}, {192, 192, 128}, {224, 192, 128}, {0, 224, 128}, {32, 224, 128}, {64, 224, 128}, {96, 224, 128}, {128, 224, 128}, {160, 224, 128}, {192, 224, 128}, {224, 224, 128}, {0, 0, 192}, _
                   {32, 0, 192}, {64, 0, 192}, {96, 0, 192}, {128, 0, 192}, {160, 0, 192}, {192, 0, 192}, {224, 0, 192}, {0, 32, 192}, {32, 32, 192}, {64, 32, 192}, {96, 32, 192}, {128, 32, 192}, {160, 32, 192}, {192, 32, 192}, {224, 32, 192}, {0, 64, 192}, _
                   {32, 64, 192}, {64, 64, 192}, {96, 64, 192}, {128, 64, 192}, {160, 64, 192}, {192, 64, 192}, {224, 64, 192}, {0, 96, 192}, {32, 96, 192}, {64, 96, 192}, {96, 96, 192}, {128, 96, 192}, {160, 96, 192}, {192, 96, 192}, {224, 96, 192}, {0, 128, 192}, _
                   {32, 128, 192}, {64, 128, 192}, {96, 128, 192}, {128, 128, 192}, {160, 128, 192}, {192, 128, 192}, {224, 128, 192}, {0, 160, 192}, {32, 160, 192}, {64, 160, 192}, {96, 160, 192}, {128, 160, 192}, {160, 160, 192}, {192, 160, 192}, {224, 160, 192}, {0, 192, 192}, _
                   {32, 192, 192}, {64, 192, 192}, {96, 192, 192}, {128, 192, 192}, {160, 192, 192}, {255, 251, 240}, {160, 160, 164}, {128, 128, 128}, {255, 0, 0}, {0, 255, 0}, {255, 255, 0}, {0, 0, 255}, {255, 0, 255}, {0, 255, 255}, {255, 255, 255}}
                ReDim Pal(255)
                For EntNo = 0 To 255
                    Pal(EntNo).rgbRed = RGB8(EntNo, 0)
                    Pal(EntNo).rgbGreen = RGB8(EntNo, 1)
                    Pal(EntNo).rgbBlue = RGB8(EntNo, 2)
                    Pal(EntNo).rgbReserved = 0
                Next
            Case Else
                ReDim Pal(0)
        End Select
        Return Pal
    End Function

#End Region

End Module
