'***********************************************************************************************************************
' About form
' 
' "About" information form.
'
Imports System.Diagnostics.FileVersionInfo
Imports Microsoft.Win32

Public Class About

#Region " Event handlers "
    Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Licence As String           ' licence string
        Dim DllName As String           ' TOCRdll filename string
        Dim LicVolume As Integer        ' licensed volume
        Dim LicTime As Integer          ' licenced time
        Dim Remainder As Integer        ' remainder on licence
        Dim Features As Integer         ' licence features
        Dim Status As Integer           ' API return status
        Dim Info As FileVersionInfo

        ViewerVersionLabel.Text = "Viewer version " & Application.ProductVersion

#If PLATFORM = "x86" Then
        ' for 32 bit build use Environment.SpecialFolder.SystemX86
        DllName = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86)
        DllName = DllName & "\TOCRDll.DLL"
        Info = FileVersionInfo.GetVersionInfo(DllName)
        EngineVersionLabel.Text = "Engine version " & Info.FileMajorPart & "." & Info.FileMinorPart & "." & Info.FileBuildPart & "." & Info.FilePrivatePart
        ViewerVersionLabel.Text &= " (x86)"
        EngineVersionLabel.Text &= " (x86)"
#End If

#If PLATFORM = "x64" Then
        ' for 64 bit build use Environment.SystemDirectory
        DllName = Environment.SystemDirectory & "\TOCRDll.DLL"
        Info = FileVersionInfo.GetVersionInfo(DllName)
        EngineVersionLabel.Text = "Engine version " & Info.FileMajorPart & "." & Info.FileMinorPart & "." & Info.FileBuildPart & "." & Info.FilePrivatePart
        ViewerVersionLabel.Text &= " (x64)"
        EngineVersionLabel.Text &= " (x64)"
#End If

        Licence = Space$(19)
        Status = TOCRGetLicenceInfoEx(JobNo, Licence, LicVolume, LicTime, Remainder, Features)

        If Status = TOCR_OK Then
            LicenceLabel.Text = "Licence " & Licence
            If LicVolume > 0 Or LicTime > 0 Then
                VolumeLabel.Text = CStr(Remainder)
                If LicTime > 0 Then
                    VolumeLabel.Text = VolumeLabel.Text & " days"
                Else
                    VolumeLabel.Text = VolumeLabel.Text & " A4 pages"
                End If
                VolumeLabel.Text = VolumeLabel.Text & " remaining on licence"
                If Remainder <= 0 Then VolumeLabel.ForeColor = Color.Red
            Else
                VolumeLabel.Text = ""
            End If

            Select Case Features
                Case TOCRLICENCE_STANDARD
                    FeaturesLabel.Text = "Features STANDARD licence"
                Case TOCRLICENCE_EURO
                    If Licence = "5AD4-1D96-F632-8912" Then
                        FeaturesLabel.Text = "Features EURO TRIAL licence"
                    Else
                        FeaturesLabel.Text = "Features EURO licence"
                    End If
                Case TOCRLICENCE_EUROUPGRADE
                    FeaturesLabel.Text = "Features EURO upgrade"
                Case TOCRLICENCE_V3SE
                    If Licence = "2E72-2B35-643A-0851" Then
                        FeaturesLabel.Text = "Features V3 Trial"
                    Else
                        FeaturesLabel.Text = "Features V3 SE"
                    End If
                Case TOCRLICENCE_V3SEUPGRADE
                    FeaturesLabel.Text = "Features V3 SE upgrade"
                Case TOCRLICENCE_V3PRO
                    FeaturesLabel.Text = "Features V3 Pro/V4"
                Case TOCRLICENCE_V3SEPROUPGRADE
                    FeaturesLabel.Text = "Features V3 Pro upgrade"
                Case TOCRLICENCE_V5
                    If Licence = "6B7D-55AA-332A-2163" Then
                        FeaturesLabel.Text = "Features V5 TRIAL licence"
                    Else
                        FeaturesLabel.Text = "Features V5 licence"
                    End If
                Case TOCRLICENCE_V5UPGRADE3, TOCRLICENCE_V5UPGRADE12
                    FeaturesLabel.Text = "Features V5 upgrade"
                Case Else
                    FeaturesLabel.Text = ""
            End Select
        End If
    End Sub
#End Region

    Private Sub ProductLabel_Click(sender As Object, e As EventArgs) Handles ProductLabel.Click

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class