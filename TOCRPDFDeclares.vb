'***************************************************************************
' Module:     TOCRPDFDeclares
'
' TOCRPDF declares - part of TOCR Version 5.0.0.0

#Const SUPERSEDED = False ' disallow superseded routines

'
'
'
'
'
'
'
'            CHANGED ALL LONGs to INTEGERs
'            THAT INCLUDES THE ULONG to UINTEGER IN CHARPTR_WITH_LEN
'
'            CHANGED ALL BOOLEANs TO USHORTs to MATCH DLL
'			??? VBBOOL is defined as a signed short - so I have used Short for now
'			??? VB now has system.Boolean - is that prefered and what is the 'c' equivilent type?
'
'
'


Option Strict On
Option Explicit On 

Imports System.Runtime.InteropServices

Module TOCRPDFDeclares

#Region " Structures "
    <StructLayout(LayoutKind.Sequential)> _
    Structure CHARPTR_WITH_LEN
        Public charPtr As IntPtr
        Public len As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
	Structure PDFExtractorHandle
		Public DataHandle As IntPtr
	End Structure

    <StructLayout(LayoutKind.Sequential)> _
	Structure PDFExtractorMemDocHandle
		Public DataHandle As IntPtr
	End Structure

    <StructLayout(LayoutKind.Sequential)> _
	Structure PDFExtractorPageHandle
		Public DataHandle As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure PageSize
        Public width As Double
        Public height As Double
    End Structure

    Public Enum TocrResultStage As Integer
        TRS_Extra_Text_Page = 500 ' there was too much text to fit on one page so this page was created to accomodate the excess
        TRS_Text = 400
        TRS_Processed_Image = 300
        TRS_Input_Image = 200
        TRS_Original_Page = 100
end enum

    <StructLayout(LayoutKind.Sequential)> _
    Structure TocrResultsInfo
        Public OriginalFileName As String
        Public OriginalPageNumber As Long
        Public OutputStage As TocrResultStage
        Public bContainedInThisDocument As Boolean
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure DpiPair
        Public DpiX As Double
        Public DpiY As Double
    End Structure

#End Region

#Region " Declares "
    ' 3 WAYS TO DECLARE A STRING FUNCTION
    'Declare Function testfn Lib "TOCRDLL" _
    '(<MarshalAs(UnmanagedType.LPWStr)> ByVal UniStr As String, ByVal ANsiStr As String, ByVal lens2 As Integer, ByRef ju As TOCRJOBINFO_EG) As Integer
    'Declare Ansi Function testfn Lib "TOCRDLL" _
    '(<MarshalAs(UnmanagedType.LPWStr)> ByVal UniStr As String, ByVal ANsiStr As String, ByVal lens2 As Integer, ByRef ju As TOCRJOBINFO_EG) As Integer
    'Declare Unicode Function testfn Lib "TOCRDLL" _
    '(ByVal UniStr As String, <MarshalAs(UnmanagedType.LPStr)> ByVal ANsiStr As String, ByVal lens2 As Integer, ByRef ju As TOCRJOBINFO_EG) As Integer
#If Debug Then
    #If Win64 Then
        'Debug and Win64 version
            Declare Function PDFExtractorHandle_New Lib "TOCRPDF64d" _
            (ByRef pExtractor As PDFExtractorHandle) As Integer
            Declare Function PDFExtractorHandle_Delete Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle) As Integer

            Declare Function PDFExtractorMemDocHandle_New Lib "TOCRPDF64d" _
            (ByRef pMemDoc As PDFExtractorMemDocHandle) As Integer
            Declare Function PDFExtractorMemDocHandle_Delete Lib "TOCRPDF64d" _
            (ByVal MemDoc As PDFExtractorMemDocHandle) As Integer

            Declare Function PDFExtractorPageHandle_New Lib "TOCRPDF64d" _
            (ByRef pPage As PDFExtractorPageHandle) As Integer
            Declare Function PDFExtractorPageHandle_Delete Lib "TOCRPDF64d" _
            (ByVal Page As PDFExtractorPageHandle) As Integer

            Declare Function PDFExtractorHandle_Load Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
             ByVal AddressOfFirstCharacterOfInFileName As Int64) As Integer
            Declare Function PDFExtractorHandle_GetPageCount Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
                  ByRef pCount As UInteger) As Integer
            Declare Function PDFExtractorHandle_FindPageSize Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
             ByRef pWidth As Double, ByRef pHeight As Double) As Integer
            Declare Function PDFExtractorHandle_GetTocrImageInfo Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
             ByRef ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer
            Declare Function PDFExtractorHandle_GetPage Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
             ByVal Page As PDFExtractorPageHandle, ByVal nPage As UInteger) As Integer

            Declare Function PDFExtractorHandle_GetRecommendedDPIForPageSize Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal width As Double,ByVal height As Double, 
             ByVal ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer

            Declare Function PDFExtractorHandle_PageToDibMem Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
             ByRef p_cpwl As CHARPTR_WITH_LEN, ByVal ColourMode As Short, ByVal pDpiX As Double,
             ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

            Declare Function PDFExtractorHandle_PageToDibFile Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
             ByVal AddressOfFirstCharacterOfOutFileName As Int64, ByVal ColourMode As Short, ByVal pDpiX As Double,
             ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

            Declare Function PDFExtractorHandle_AddAppendix Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal AddressOfFirstCharacterOfInFileName As Int64, 
             ByVal AddressOfFirstCharacterOfOutFileName As Int64, ByVal AddressOfFirstCharacterOfApendix As Int64) As Integer

            Declare Function PDFExtractorHandle_AddAppendixEx Lib "TOCRPDF64d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal AddressOfFirstCharacterOfInFileName As Int64, ByVal AddressOfFirstCharacterOfOutFileName As Int64,
             ByVal Appendix As TOCRRESULTSEX_EG, ByVal AddressOfFirstCharacterOfTitle As Int64, ByVal width As Double,
             ByVal height As Double, ByVal dpiX As Double, ByVal dpiY As Double, ByVal ResultsInfo As TocrResultsInfo) As Integer

	        Declare Function PDFExtractorHandle_GetLastExceptionText Lib "TOCRPDF64d" _
	        (ByVal Extractor As PDFExtractorHandle, ByVal err As Integer, ByRef p_cpwl As CHARPTR_WITH_LEN) As Integer
    #Else
        'Debug and Win32 version
            Declare Function PDFExtractorHandle_New Lib "TOCRPDF32d" _
            (ByRef pExtractor As PDFExtractorHandle) As Integer
            Declare Function PDFExtractorHandle_Delete Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle) As Integer

            Declare Function PDFExtractorMemDocHandle_New Lib "TOCRPDF32d" _
            (ByRef pMemDoc As PDFExtractorMemDocHandle) As Integer
            Declare Function PDFExtractorMemDocHandle_Delete Lib "TOCRPDF32d" _
            (ByVal MemDoc As PDFExtractorMemDocHandle) As Integer

            Declare Function PDFExtractorPageHandle_New Lib "TOCRPDF32d" _
            (ByRef pPage As PDFExtractorPageHandle) As Integer
            Declare Function PDFExtractorPageHandle_Delete Lib "TOCRPDF32d" _
            (ByVal Page As PDFExtractorPageHandle) As Integer

            Declare Function PDFExtractorHandle_Load Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
                ByVal AddressOfFirstCharacterOfInFileName As Int32) As Integer
            Declare Function PDFExtractorHandle_GetPageCount Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
                  ByRef pCount As UInteger) As Integer
            Declare Function PDFExtractorHandle_FindPageSize Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
             ByRef pWidth As Double, ByRef pHeight As Double) As Integer
            Declare Function PDFExtractorHandle_GetTocrImageInfo Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
                ByRef ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer
            Declare Function PDFExtractorHandle_GetPage Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
                  ByVal Page As PDFExtractorPageHandle, ByVal nPage As UInteger) As Integer

            Declare Function PDFExtractorHandle_GetRecommendedDPIForPageSize Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal width As Double,ByVal height As Double, 
                ByVal ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer

            Declare Function PDFExtractorHandle_PageToDibMem Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
                ByRef p_cpwl As CHARPTR_WITH_LEN, ByVal ColourMode As Short, ByVal pDpiX As Double,
                ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

            Declare Function PDFExtractorHandle_PageToDibFile Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
                ByVal AddressOfFirstCharacterOfOutFileName As Int32, ByVal ColourMode As Short, ByVal pDpiX As Double,
                ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

            Declare Function PDFExtractorHandle_AddAppendix Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal AddressOfFirstCharacterOfInFileName As Int32,
                ByVal AddressOfFirstCharacterOfOutFileName As Int32, ByVal AddressOfFirstCharacterOfApendix As Int32) As Integer

            Declare Function PDFExtractorHandle_AddAppendixEx Lib "TOCRPDF32d" _
            (ByVal Extractor As PDFExtractorHandle, ByVal AddressOfFirstCharacterOfInFileName As Int32, ByVal AddressOfFirstCharacterOfOutFileName As Int32,
                ByVal Appendix As TOCRRESULTSEX_EG, ByVal AddressOfFirstCharacterOfTitle As Int32, ByVal width As Double,
                ByVal height As Double, ByVal dpiX As Double, ByVal dpiY As Double, ByVal ResultsInfo As TocrResultsInfo) As Integer

	       Declare Function PDFExtractorHandle_GetLastExceptionText Lib "TOCRPDF32d" _
	        (ByVal Extractor As PDFExtractorHandle, ByVal err As Integer, ByRef p_cpwl As CHARPTR_WITH_LEN) As Integer
    #End If
#Else
#If Win64 Then
    'Release and Win64 version
    Declare Function PDFExtractorHandle_New Lib "TOCRPDF64" _
    (ByRef pExtractor As PDFExtractorHandle) As Integer
    Declare Function PDFExtractorHandle_Delete Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle) As Integer

    Declare Function PDFExtractorMemDocHandle_New Lib "TOCRPDF64" _
    (ByRef pMemDoc As PDFExtractorMemDocHandle) As Integer
    Declare Function PDFExtractorMemDocHandle_Delete Lib "TOCRPDF64" _
    (ByVal MemDoc As PDFExtractorMemDocHandle) As Integer

    Declare Function PDFExtractorPageHandle_New Lib "TOCRPDF64" _
    (ByRef pPage As PDFExtractorPageHandle) As Integer
    Declare Function PDFExtractorPageHandle_Delete Lib "TOCRPDF64" _
    (ByVal Page As PDFExtractorPageHandle) As Integer

    Declare Function PDFExtractorHandle_Load Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
           ByVal AddressOfFirstCharacterOfInFileName As Int64) As Integer
    Declare Function PDFExtractorHandle_GetPageCount Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
          ByRef pCount As UInteger) As Integer
    Declare Function PDFExtractorHandle_FindPageSize Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
     ByRef pWidth As Double, ByRef pHeight As Double) As Integer
    Declare Function PDFExtractorHandle_GetTocrImageInfo Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
             ByRef ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer
    Declare Function PDFExtractorHandle_GetPage Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
          ByVal Page As PDFExtractorPageHandle, ByVal nPage As UInteger) As Integer

    Declare Function PDFExtractorHandle_GetRecommendedDPIForPageSize Lib "TOCRPDF64" _
            (ByVal Extractor As PDFExtractorHandle, ByVal width As Double, ByVal height As Double,
               ByVal ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer

    Declare Function PDFExtractorHandle_PageToDibMem Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
               ByRef p_cpwl As CHARPTR_WITH_LEN, ByVal ColourMode As Short, ByVal pDpiX As Double,
                   ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

    Declare Function PDFExtractorHandle_PageToDibFile Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
                ByVal AddressOfFirstCharacterOfOutFileName As Int64, ByVal ColourMode As Short, ByVal pDpiX As Double,
                   ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

    Declare Function PDFExtractorHandle_AddAppendix Lib "TOCRPDF64" _
     (ByVal Extractor As PDFExtractorHandle,  ByVal AddressOfFirstCharacterOfInFileName As Int64,
      ByVal AddressOfFirstCharacterOfOutFileName As Int64, ByVal AddressOfFirstCharacterOfAppendix As Int64) As Integer

    Declare Function PDFExtractorHandle_AddAppendixEx Lib "TOCRPDF64" _
     (ByVal Extractor As PDFExtractorHandle, ByVal AddressOfFirstCharacterOfInFileName As Int64,  
      ByVal AddressOfFirstCharacterOfOutFileName As Int64, ByVal Appendix As TOCRRESULTSEX_EG, 
      ByVal AddressOfFirstCharacterOfTitle As Int64, ByVal width As Double, ByVal height As Double, 
      ByVal dpiX As Double, ByVal dpiY As Double, ByVal ResultsInfo As TocrResultsInfo) As Integer

    Declare Function PDFExtractorHandle_GetLastExceptionText Lib "TOCRPDF64" _
    (ByVal Extractor As PDFExtractorHandle, ByVal err As Integer, ByRef p_cpwl As CHARPTR_WITH_LEN) As Integer
#Else
        'Release and Win32 version
    Declare Function PDFExtractorHandle_New Lib "TOCRPDF32" _
    (ByRef pExtractor As PDFExtractorHandle) As Integer
    Declare Function PDFExtractorHandle_Delete Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle) As Integer

    Declare Function PDFExtractorMemDocHandle_New Lib "TOCRPDF32" _
    (ByRef pMemDoc As PDFExtractorMemDocHandle) As Integer
    Declare Function PDFExtractorMemDocHandle_Delete Lib "TOCRPDF32" _
    (ByVal MemDoc As PDFExtractorMemDocHandle) As Integer

    Declare Function PDFExtractorPageHandle_New Lib "TOCRPDF32" _
    (ByRef pPage As PDFExtractorPageHandle) As Integer
    Declare Function PDFExtractorPageHandle_Delete Lib "TOCRPDF32" _
    (ByVal Page As PDFExtractorPageHandle) As Integer

    Declare Function PDFExtractorHandle_Load Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
          ByVal AddressOfFirstCharacterOfInFileName As Int32) As Integer
    Declare Function PDFExtractorHandle_GetPageCount Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
          ByRef pCount As UInteger) As Integer
    Declare Function PDFExtractorHandle_FindPageSize Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
     ByRef pWidth As Double, ByRef pHeight As Double) As Integer
    Declare Function PDFExtractorHandle_GetTocrImageInfo Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
             ByRef ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer
    Declare Function PDFExtractorHandle_GetPage Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal MemDoc As PDFExtractorMemDocHandle,
          ByVal Page As PDFExtractorPageHandle, ByVal nPage As UInteger) As Integer

    Declare Function PDFExtractorHandle_GetRecommendedDPIForPageSize Lib "TOCRPDF32" _
            (ByVal Extractor As PDFExtractorHandle, ByVal width As Double,ByVal height As Double, 
               ByVal ColourMode As Short, ByRef pDpiX As Double, ByRef pDpiY As Double) As Integer

    Declare Function PDFExtractorHandle_PageToDibMem Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
               ByRef p_cpwl As CHARPTR_WITH_LEN, ByVal ColourMode As Short, ByVal pDpiX As Double,
                   ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

    Declare Function PDFExtractorHandle_PageToDibFile Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal Page As PDFExtractorPageHandle,
        ByVal AddressOfFirstCharacterOfOutFileName As Int32, ByVal ColourMode As Short, ByVal pDpiX As Double,
                   ByVal pDpiY As Double, ByRef pPageIsNotBlank As Short) As Integer

    Declare Function PDFExtractorHandle_AddAppendix Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal AddressOfFirstCharacterOfInFileName As Int32, ByVal AddressOfFirstCharacterOfOutFileName As Int32,
        ByVal AddressOfFirstCharacterOfAppendix As Int32) As Integer

    Declare Function PDFExtractorHandle_AddAppendixEx Lib "TOCRPDF32" _
    (ByVal Extractor As PDFExtractorHandle, ByVal AddressOfFirstCharacterOfInFileName As Int32, ByVal AddressOfFirstCharacterOfInFileName As Int32,
        ByVal Appendix As TOCRRESULTSEX_EG, ByVal AddressOfFirstCharacterOfTitle As Int32, ByVal width as double, 
        ByVal height as double, ByVal dpiX as double, ByVal dpiY as double, ByVal ResultsInfo as TocrResultsInfo) As Integer

	Declare Function PDFExtractorHandle_GetLastExceptionText Lib "TOCRPDF32" _
	(ByVal Extractor As PDFExtractorHandle, ByVal err As Integer, ByRef p_cpwl As CHARPTR_WITH_LEN) As Integer
#End If
#End If

#End Region

#Region " Boolean Values "
    Public Const VARIANT_TRUE As Short = -1
    Public Const VARIANT_FALSE As Short = 0
#End Region

#Region " Error Codes "
    Public Const TOCRPDF_ErrorOK As Integer = 0                 ' The default value indicating no error.
    Public Const TOCRPDF_PoDoFo_Exception As Integer = 1        ' PoDoFo::PdfError
    Public Const TOCRPDF_Standard_Exception As Integer = 2      ' std::exception
    Public Const TOCRPDF_Unknown_Error As Integer = 3           ' catch(...)
    Public Const TOCRPDF_Invalid_PDFExtractor As Integer = 4    ' PDFExtractorHandle is NULL
#End Region


#Region " Wrapper Classes "
    Public Class PDFExtractor
        Inherits PDFExtractorBase

        Public m_hExtractor As PDFExtractorHandle

        Public Sub New()
            TocrPdfResult(PDFExtractorHandle_New(m_hExtractor))
        End Sub

        Public Function Load(inFileName As String) As PDFExtractorMemDoc
            Dim pMemDoc As New PDFExtractorMemDoc(Me)
            pMemDoc.Load(inFileName)
            Load = pMemDoc
        End Function

        Public Sub AddAppendix(inFileName As String, outFileName As String, Appendix As String)
            Dim hInFileName As GCHandle
            Dim pInFileName As System.IntPtr

            hInFileName = GCHandle.Alloc(inFileName, GCHandleType.Pinned)
            pInFileName = hInFileName.AddrOfPinnedObject()
#If Win64 Then
            Dim iInFileName As Int64
            iInFileName = pInFileName.ToInt64()
#Else
            Dim iInFileName As Int32
            iInFileName = pInFileName.ToInt32()
#End If

            Dim hOutFileName As GCHandle
            Dim pOutFileName As System.IntPtr
            hOutFileName = GCHandle.Alloc(outFileName, GCHandleType.Pinned)
            pOutFileName = hOutFileName.AddrOfPinnedObject()
#If Win64 Then
            Dim iOutFileName As Int64
            iOutFileName = pOutFileName.ToInt64()
#Else
            Dim iOutFileName As Int32
            iOutFileName = pOutFileName.ToInt32()
#End If

            Dim hAppendix As GCHandle
            Dim pAppendix As System.IntPtr
            hAppendix = GCHandle.Alloc(Appendix, GCHandleType.Pinned)
            pAppendix = hAppendix.AddrOfPinnedObject()
#If Win64 Then
            Dim iAppendix As Int64
            iAppendix = pAppendix.ToInt64()
#Else
            Dim iAppendix As Int32
            iAppendix = pAppendix.ToInt32()
#End If

            TocrPdfResult(PDFExtractorHandle_AddAppendix(m_hExtractor, iInFileName, iOutFileName, iAppendix))
        End Sub


        Public Function GetRecommendedDPI(size As PageSize, ByVal ColourMode As Short) As DpiPair
            Dim pDpi As DpiPair

            TocrPdfResult(PDFExtractorHandle_GetRecommendedDPIForPageSize(m_hExtractor, size.width, size.height, ColourMode, pDpi.DpiX, pDpi.DpiY))

            GetRecommendedDPI = pDpi
        End Function

        Public Overrides Function GetLastExceptionText(TPRes As Integer) As String
            Dim msg As String

            If (TOCRPDF_PoDoFo_Exception = TPRes) Or (TOCRPDF_Standard_Exception = TPRes) Then
                Dim p_cpwl As CHARPTR_WITH_LEN
                Dim buf() As Byte
                ReDim buf(1000)

                Dim hBuf As GCHandle
                hBuf = GCHandle.Alloc(buf, GCHandleType.Pinned)

                p_cpwl.charPtr = hBuf.AddrOfPinnedObject()
                p_cpwl.len = 1000

                Dim innerTPRes As Integer = PDFExtractorHandle_GetLastExceptionText(m_hExtractor, TPRes, p_cpwl)
                hBuf.Free()

                If (TOCRPDF_ErrorOK = innerTPRes) Then
                    msg = System.Text.Encoding.ASCII.GetString(buf)
                ElseIf (TOCRPDF_PoDoFo_Exception = innerTPRes) Then
                    msg = "Unknown PoDoFo Exception"
                ElseIf (TOCRPDF_Standard_Exception = innerTPRes) Then
                    msg = "Unknown Standard Exception"
                Else
                    msg = MyBase.GetLastExceptionText(innerTPRes)
                End If
            Else
                msg = MyBase.GetLastExceptionText(TPRes)
            End If

            GetLastExceptionText = msg

        End Function

        Public Overrides Function TocrPdfResult(TPRes As Integer) As Integer
            If (TOCRPDF_ErrorOK <> TPRes) Then
                ' ask the dll for the error msg - and later ask for the stack trace
                Dim msg As String = GetLastExceptionText(TPRes)

                Dim innerException As New Exception(msg)
                Dim ex As New PDFExtractorException("TOCR PDF DLL is not able to do that. Error " & TPRes, innerException)
                Throw (ex)
                'MsgBox("TOCR PDF DLL is not able to do that. Error " & TPRes, MsgBoxStyle.Exclamation)
            End If

            TocrPdfResult = TPRes
        End Function

#Region " IDisposable Support "
        ' This method disposes the derived object's resources. 
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then
                    ' Insert code to free managed resources. 
                End If
                ' Insert code to free unmanaged resources. 
                TocrPdfResult(PDFExtractorHandle_Delete(m_hExtractor))
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' The derived class does not have a Finalize method 
        ' or a Dispose method with parameters because it inherits 
        ' them from the base class. 
#End Region

    End Class

    Public Class PDFExtractorBase
        Implements IDisposable

        Public Overridable Function GetLastExceptionText(TPRes As Integer) As String
            Dim msg As String

            If (TOCRPDF_ErrorOK = TPRes) Then
                msg = "OK"
            ElseIf (TOCRPDF_PoDoFo_Exception = TPRes) Then
                msg = "PoDoFo Exception"
            ElseIf (TOCRPDF_Standard_Exception = TPRes) Then
                msg = "Standard Exception"
            ElseIf (TOCRPDF_Invalid_PDFExtractor = TPRes) Then
                msg = "Invalid PDFExtractor handle used"
            Else
                msg = "Unknown error"
            End If

            GetLastExceptionText = msg

        End Function

        Public Overridable Function TocrPdfResult(TPRes As Integer) As Integer
            If (TOCRPDF_ErrorOK <> TPRes) Then
                Dim ex As New PDFExtractorException("TOCR PDF DLL is not able to do that. Error " & TPRes)
                Throw (ex)
                'MsgBox("TOCR PDF DLL is not able to do that. Error " & TPRes, MsgBoxStyle.Exclamation)
            End If

            TocrPdfResult = TPRes
        End Function

#Region " IDisposable Support "
        ' Keep track of when the object is disposed. 
        Protected disposed As Boolean = False

        ' This method disposes the base object's resources. 
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then
                    ' Insert code to free managed resources. 
                End If
                ' Insert code to free unmanaged resources. 
            End If
            Me.disposed = True
        End Sub

        ' Do not change or add Overridable to these methods. 
        ' Put cleanup code in Dispose(ByVal disposing As Boolean). 
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
#End Region

    End Class

    Public Class PDFExtractorUser
        Inherits PDFExtractorBase

        Public m_pExtractor As PDFExtractor

        Protected Sub New()
            ' don't call this as it would leave us with an unpopulated PDFExtractor
        End Sub

        Public Sub New(pExtractor As PDFExtractor)
            m_pExtractor = pExtractor
        End Sub

        Public Function GetRecommendedDPI(size As PageSize, ByVal ColourMode As Short) As DpiPair
            Dim pDPI As DpiPair
            pDPI = m_pExtractor.GetRecommendedDPI(size, ColourMode)

            GetRecommendedDPI = pDPI
        End Function

        Public Overrides Function GetLastExceptionText(TPRes As Integer) As String
            GetLastExceptionText = m_pExtractor.GetLastExceptionText(TPRes)
        End Function

        Public Overrides Function TocrPdfResult(TPRes As Integer) As Integer
            If (TOCRPDF_ErrorOK <> TPRes) Then
                TPRes = m_pExtractor.TocrPdfResult(TPRes)
            End If

            TocrPdfResult = TPRes
        End Function


#Region " IDisposable Support "
        ' This method disposes the derived object's resources. 
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then
                    ' Insert code to free managed resources. 
                End If
                ' Insert code to free unmanaged resources. 
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' The derived class does not have a Finalize method 
        ' or a Dispose method with parameters because it inherits 
        ' them from the base class. 
#End Region

    End Class

    Public Class PDFExtractorMemDoc
        Inherits PDFExtractorUser

        Public m_hMemDoc As PDFExtractorMemDocHandle

        Protected Sub New()
            ' don't call this as it would leave us with an unpopulated PDFExtractor
        End Sub

        Public Sub New(pExtractor As PDFExtractor)
            MyBase.New(pExtractor)
            TocrPdfResult(PDFExtractorMemDocHandle_New(m_hMemDoc))
        End Sub

        Public Sub Load(inFileName As String)
            Dim hInFileName As GCHandle
            Dim pInFileName As System.IntPtr
            hInFileName = GCHandle.Alloc(inFileName, GCHandleType.Pinned)
            pInFileName = hInFileName.AddrOfPinnedObject()
#If Win64 Then
            Dim iInFileName As Int64
            iInFileName = pInFileName.ToInt64()
#Else
            Dim iInFileName As Int32
            iInFileName = pInFileName.ToInt32()
#End If

            TocrPdfResult(PDFExtractorHandle_Load(m_pExtractor.m_hExtractor, m_hMemDoc, iInFileName))
        End Sub

        Public Function GetPageCount() As UInteger
            Dim count As UInteger
            TocrPdfResult(PDFExtractorHandle_GetPageCount(m_pExtractor.m_hExtractor, m_hMemDoc, count))

            GetPageCount = count
        End Function

        Public Function GetPage(nPage As UInteger) As PDFExtractorPage
            Dim pPage As New PDFExtractorPage(m_pExtractor)

            TocrPdfResult(PDFExtractorHandle_GetPage(m_pExtractor.m_hExtractor, m_hMemDoc, pPage.m_hPage, nPage))

            GetPage = pPage
        End Function

#Region " IDisposable Support "
        ' This method disposes the derived object's resources. 
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then
                    ' Insert code to free managed resources. 
                End If
                ' Insert code to free unmanaged resources. 
                TocrPdfResult(PDFExtractorMemDocHandle_Delete(m_hMemDoc))
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' The derived class does not have a Finalize method 
        ' or a Dispose method with parameters because it inherits 
        ' them from the base class. 
#End Region

    End Class

    Public Class PDFExtractorPage
        Inherits PDFExtractorUser

        Public m_hPage As PDFExtractorPageHandle
        Public m_PageIsNotBlank As Boolean
        Public m_ColourMode As Short
        Public m_DPI As DpiPair
        Public m_size As PageSize

        Protected Sub New()
            ' don't call this as it would leave us with an unpopulated PDFExtractor
        End Sub

        Public Sub New(pExtractor As PDFExtractor)
            MyBase.New(pExtractor)
            TocrPdfResult(PDFExtractorPageHandle_New(m_hPage))
            m_PageIsNotBlank = False
            m_ColourMode = 1 ' Grey8
            m_DPI.DpiX = 0
            m_DPI.DpiY = 0
            m_size.width = 0
            m_size.height = 0
        End Sub

        Public Function FindPageSize() As PageSize
            Dim size As New PageSize

            TocrPdfResult(PDFExtractorHandle_FindPageSize(m_pExtractor.m_hExtractor, m_hPage, size.width, size.height))

            FindPageSize = size
        End Function

        Public Sub PageToDib(ByRef p_cpwl As CHARPTR_WITH_LEN)
            Dim PageIsNotBlank As Short

            If ((m_DPI.DpiX = 0) And (m_DPI.DpiY = 0)) Then
                Dim size As New PageSize
                size = FindPageSize()
                m_DPI = GetRecommendedDPI(size, m_ColourMode)
            End If

            TocrPdfResult(PDFExtractorHandle_PageToDibMem(m_pExtractor.m_hExtractor, m_hPage, p_cpwl, m_ColourMode, m_DPI.DpiX, m_DPI.DpiY, PageIsNotBlank))

            If (VARIANT_FALSE = PageIsNotBlank) Then
                m_PageIsNotBlank = False
            Else
                m_PageIsNotBlank = True
            End If
        End Sub

#Region " IDisposable Support "
        ' This method disposes the derived object's resources. 
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then
                    ' Insert code to free managed resources. 
                End If
                ' Insert code to free unmanaged resources. 
                TocrPdfResult(PDFExtractorPageHandle_Delete(m_hPage))
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' The derived class does not have a Finalize method 
        ' or a Dispose method with parameters because it inherits 
        ' them from the base class. 
#End Region

    End Class

    Public Class PDFExtractorException
        Inherits ApplicationException

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As System.Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class
#End Region

End Module
