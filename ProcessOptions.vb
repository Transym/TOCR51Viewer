'***********************************************************************************************************************
' Process Options form
' 
' This form allows the user to change TOCR Processing options.

Public Class ProcessOptions

#Region " Definitions "
    Private Const NOTVALID As Integer = -1          ' invalid character/glyph number

    Private IgnoreCheckBoxChange As Boolean         ' flag to inhibit and event handler
    'Private DisableGlyphs() As Boolean              ' disabled glyphs array

    Private Enum LANGUAGES
        gtNULL = 0
        gtBELARUSIAN = 1 'be
        gtBULGARIAN = 2 'bu
        gtBOSNIAN = 3 'bs
        gtCATALAN = 4 'ca
        gtCZECH = 5 'cs 
        gtDANISH = 6 'da
        gtGERMAN = 7 'de
        gtGREEK = 8 'el
        gtENGLISH = 9 'en
        gtSPANISH = 10 'es
        gtESTONIAN = 11 'et
        gtBASQUE = 12 'eu
        gtFINNISH = 13 'fi
        gtFRENCH = 14 'fr
        gtCROATIAN = 15 'hr
        gtHUNGARIAN = 16 'hu
        gtICELANDIC = 17 'is
        gtITALIAN = 18 'it
        gtLITHUANIAN = 19 'lt
        gtLATVIAN = 20 'lv
        gtMACEDONIAN = 21 'mk
        gtDUTCH = 22 'nl
        gtNORWEGIAN = 23 'no
        gtPOLISH = 24 'pl
        gtPORTUGESE = 25 'pt
        gtROMANIAN = 26 'ro
        gtRUSSIAN = 27 'ru
        gtSERBOCROATIAN = 28 'sh
        gtSLOVAKIAN = 29 'sk
        gtSLOVENIAN = 30 'sl
        gtALBANIAN = 31 'sq
        gtSERBIAN = 32 'sr
        gtSWEDISH = 33 'sv
        gtTURKISH = 34 'tr
        gtUKRANIAN = 35 'uk

        gtLUXEMBOURGISH = 36 'lb
        gtGALICISN = 37 'gls
        gtNEAPOLITAN = 38 'nap
        gtLOMBARDIAN = 39 'lmo
        gtSICILIAN = 40 'scn
        gtPIEDMONTESE = 41 'pms
        gtWESTFRIESAN = 42 'fy
        gtWESTFLEMISH = 43 'vls
        gtLIMBURGIAN = 44 'li
        gtNORWEGIANYNORSK = 45 'nn

    End Enum
    Private Enum GLYPHSTATES
        gsEMPTY = 0
        gsENABLED = 1
        gsDISABLED_BY_LANGUAGE = 2
        gsDISABLED_BY_MANUAL = 4
        gsDISABLED_BY_LANGUAGE_AND_MANUAL = 6
        gsDISABLED_BY_OVERRIDE = 8
        gsDISABLED_BY_OVERRIDE_AND_LANGUAGE = 10
        gsDISABLED_BY_OVERRIDE_AND_MANUAL = 12
        gsDISABLED_BY_OVERRIDE_LANGUAGE_AND_MANUAL = 14
        gsENABLED_BY_OVERRIDE = 16
    End Enum



    Private Const SIMPLESYMBOLS As String = ",21,22,23,24,25,26,27,28,29,2A,2B,2C,2D,2E,2F,3A,3B,3C,3D,3E,3F,40,5B,5C,5D,5E,5F,7B,7C,7D,7E,20AC," & _
    "2020,2021,2030,2022,2122,A1,A2,A3,A4,A5,A7,A9," & _
    "AA,AB,AC,AE,AF,B0,B1,B2,B3,B5,B6,B7,B9,BB,BC,BD,BE,BF,D7,F7,"

    Private Const EXTENDEDSYMBOLS As String = ",20A3,20A4,2105," & _
    "215B,215C,215D,215E,2190,2191,2192,2193,2194,2195,2202,220F,2211,221A,221E,221F,2229,222B,2248,2260,2261,2264,2265,2302,2310,25A1,25AA,25AB,25AC,25B2," & _
    "25BA,25BC,25C4,25CA,25CF,25E6,263A,263B,263C,2640,2642,2660,2663,2665,2666,266A,266B,"

    Private Const NUMERIC As String = ",30,31,32,33,34,35,36,37,38,39,"

    Private Const SIMPLEUPPERCASE As String = ",41,42,43,44,45,46,47,48,49,4A,4B,4C,4D,4E,4F,50,51,52,53,54,55,56,57,58,59,5A," & _
        "C0,C1,C2,C3,C4,C5,C6,C7,C8,C9,CA,CB,CC,CD,CE,CF,D0,D1,D2,D3,D4,D5,D6,D8,D9,DA,DB,DC,DD,DE,160,152,17D,178,"

    Private Const EXTENDEDUPPERCASE As String = ",100,102,104,106,108,10A,10C,10E,112,114,116,118,11A,11C,11E,120,122,124,126,128,12A,12C,12E,130," & _
        "134,136,139,13B,13D,13F,141,143,145,147,14A,14C,14E,150,154,156,158,15A,15C,15E,162,164,166,168,16A,16C,16E,170,172,174,176,179,17B," & _
        "1FA,1FC,1FE,386,388,389,38A,38C,38E,38F,391,392,393,394,395,396,397,398,399,39A,39B,39C,39D,39E,39F,3A0,3A1,3A3,3A4,3A5,3A6,3A7,3A8,3A9,3AA,3AB," & _
        "401,402,403,404,405,406,407,408,409,40A,40B,40C,40E,40F,410,411,412,413,414,415,416,417,418,419,41A,41B,41C,41D,41E,41F,420,421,422,423,424,425," & _
        "426,427,428,429,42A,42B,42C,42D,42E,42F,490,1E80,1E82,1E84,1EF2,"

    Private Const SIMPLELOWERCASE As String = ",61,62,63,64,65,66,67,68,69,6A,6B,6C,6D,6E,6F,70,71,72,73,74,75,76,77,78,79,7A," & _
         "DF,E0,E1,E2,E3,E4,E5,E6,E7,E8,E9,EA,EB,EC,ED,EE,EF,F0,F1,F2,F3,F4,F5,F6,F8,F9,FA,FB,FC,FD,FE,FF,161,153,17E,"

    Private Const EXTENDEDLOWERCASE As String = ",101,103,105,107,109,10B,10D,10F,111,113,115,117,119,11B,11D,11F,121,123,125,127,129,12B,12D,12F," & _
        "131,135,137,138,13C,13E,140,142,144,146,148,149,14B,14D,14F,151,155,157,159,15B,15D,15F,163,165,167,169,16B,16D,16F,171,173,175,177,17A,17C,17F," & _
        "1FB,1FD,1FF,390,3AC,3AD,3AE,3AF,3B0,3B1,3B2,3B3,3B4,3B5,3B6,3B7,3B8,3B9,3BA,3BB,3BC,3BD,3BE,3BF,3C0,3C1,3C2,3C3,3C4,3C5,3C6,3C7,3C8,3C9,3CA,3CB,3CC,3CD,3CE," & _
        "430,431,432,433,434,435,436,437,438,439,43A,43B,43C,43D,43E,43F,440,441,442,443,444,445,446,447,448,449,44A,44B,44C,44D,44E,44F,451,452,453,454,455,456," & _
        "457,458,459,45A,45B,45C,45E,45F,491,1E81,1E83,1E85,1EF3,"

    Private Const CYRILLIC As String = ",401,402,403,404,405,406,407,408,409,40A,40B,40C,40E,40F,410,411,412,413,414,415,416,417,418,419,41A,41B,41C,41D,41E,41F,420,421,422,423,424,425,426,427,428,429,42A," & _
        "42B,42C,42D,42E,42F,430,431,432,433,434,435,436,437,438,439,43A,43B,43C,43D,43E,43F,440,441,442,443,444,445,446,447,448,449,44A," & _
        "44B,44C,44D,44E,44F,451,452,453,454,455,456,457,458,459,45A,45B,45C,45E,45F,490,491,"

    Private Const GREEKCOPTIC As String = ",386,388,389,38A,38C,38E,38F,390,391,392,393,394,395,396," & _
        "397,398,399,39A,39B,39C,39D,39E,39F,3A0,3A1,3A3,3A4,3A5,3A6,3A7,3A8,3A9,3AA,3AB,3AC,3AD,3AE,3AF,3B0,3B1,3B2,3B3,3B4,3B5,3B6,3B7," & _
        "3B8,3B9,3BA,3BB,3BC,3BD,3BE,3BF,3C0,3C1,3C2,3C3,3C4,3C5,3C6,3C7,3C8,3C9,3CA,3CB,3CC,3CD,3CE,"

    Private Const EASTERNEUROPEAN As String = ",1E80,1E81,1E82,1E83,1E84,1E85,1EF2,1EF3," & _
        "100,101,102,103,104,105,106,107,108,109,10A,10B,10C,10D,10E,10F,111,112,113,114,115,116,117,118,119,11A,11B,11C,11D,11E,11F,120," & _
        "121,122,123,124,125,126,127,128,129,12A,12B,12C,12D,12E,12F,130,131," & _
        "134,135,136,137,138,139,13B,13C,13D,13E,13F,140,141,142,143,144,145,146,147,148,149,14A,14B,14C,14D,14E,14F,150,151,154,155," & _
        "156,157,158,159,15A,15B,15C,15D,15E,15F,162,163,164,165,166,167,168,169,16A,16B,16C,16D,16E,16F,170,171,172,173,174,175,176,177,179,17A,17B," & _
        "17C,17F,1FA,1FB,1FC,1FD,1FE,1FF,"

    Private Const WESTERNEUROPEAN As String = ",41,42,43,44,45,46,47,48,49,4A,4B,4C,4D,4E,4F,50,51,52,53,54,55,56,57,58,59,5A," & _
        "61,62,63,64,65,66,67,68,69,6A,6B,6C,6D,6E,6F,70,71,72,73,74,75,76,77,78,79,7A,160,152,17D,161,153,17E,178," & _
        "C0,C1,C2,C3,C4,C5,C6,C7,C8,C9,CA,CB,CC," & _
        "CD,CE,CF,D0,D1,D2,D3,D4,D5,D6,D8,D9,DA,DB,DC,DD,DE,DF,E0,E1,E2,E3,E4,E5,E6,E7,E8,E9,EA,EB,EC," & _
        "ED,EE,EF,F0,F1,F2,F3,F4,F5,F6,F8,F9,FA,FB,FC,FD,FE,FF,"

    '    Private Const BELARUSIAN As String = ",30,31,32,33,34,35,36,37,38,39,8a,9a,9e," & _
    '        "C1,C9,D6,DF,E0,E1,E2,E3,E4,E5,E6,E7,E8,E9,EA,EB,ED,EE,F0,F1,F3,F4,F5,F6,F8,FA,FC,FD," & _
    '        "101,105,107,10C,10D,113,117,119,11F,12B,131,141,142,144,14D,159,15D,167,16F,177,18A," & _
    '        "190,192,196,198,1A1,1A2,1A3,1A4,1A6,1A7,1A8,1A9,1AA,1AC,1AD,1AE,1AF,1B0,1B1,1B2,1B4," & _
    '        "1B5,1B6,1B7,1B8,1B9,1BA,1BB,1BC,1BE,1C1,1C2,1C4,1C5,1C6,1C7,1C8,1C9,1CA,1CB,1CD,1CE," & _
    '        "1CF,1D0,1D1,1D2,1D3,1D4,1D5,1D6,1D7,1D8,1D9,1DA,1DB,1DC,1DD,1DE,1DF,1E0,1E1,1E2,1E3," & _
    '        "1E4,1E5,1E6,1E7,1E8,1E9,1EA,1EB,1EC,1ED,1EE,1EF,1F0,1F1,1F2,1F3,1F4,1F5,1F6,1F7,1F8," & _
    '        "1F9,1FA,1FB,1FC,1FD,1FE,1FF,200,201,202,203,204,205,206,207,208,209,20A,20B,20C,20D," & _
    '        "20E,20F,210,211,212,213,214,215,216,217,218,219,21A,21B,21C,21D,21E,21F,220,221"

    '    Private Const BELARUSIAN As String = ",48,49,50,51,52,53,54,55,56,57,138,154,158,193,201," & _
    '        "214,223,224,225,226,227,228,229,230,231,232,233,234,235,237,238,240,241,243,244,245," & _
    '        "246,248,250,252,253,257,261,263,268,269,275,279,281,287,299,305,321,322,324,333,345," & _
    '        "349,359,367,375,394,400,402,406,408,417,418,419,420,422,423,424,425,426,428,429,430," & _
    '        "431,432,433,434,436,437,438,439,440,441,442,443,444,446,449,450,452,453,454,455,456," & _
    '        "457,458,459,461,462,463,464,465,466,467,468,469,470,471,472,473,474,475,476,477,478," & _
    '        "479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,497,498,499," & _
    '        "500,501,502,503,504,505,506,507,508,509,510,511,512,513,514,515,516,517,518,519,520," & _
    '        "521,522,523,524,525,526,527,528,529,530,531,532,533,534,535,536,537,538,539,540,541,542,543,544,545,"

    'as Basic English for testing
    'Private ReadOnly LANG(2)() As integer = {{41,42,43,44,45,46,47,48,49,&h4A,4B,4C,4D,4E,4F,50,51,52,53,54,55,56,57,58,59,5A, & _
    '    61,62,63,64,65,66,67,68,69,6A,6B,6C,6D,6E,6F,70,71,72,73,74,75,76,77,78,79,7A}, & _
    'Private ReadOnly LANG_1() As Integer = {&H41, &H42, &H43, &H44, &H45, &H46, &H47, &H48, &H49, &H4A, &H4B, &H4C, &H4D, &H4E, &H4F, &H50, &H51, &H52, &H53, &H54, &H55, &H56, &H57, &H58, &H59, &H5A}
    'Private ReadOnly LANG_2() As Integer = {&H61, &H62, &H63, &H64, &H65, &H66, &H67, &H68, &H69, &H6A, &H6B, &H6C, &H6D, &H6E, &H6F, &H70, &H71, &H72, &H73, &H74, &H75, &H76, &H77, &H78, &H79, &H7A}
    'Private ReadOnly LANG() As Array = {LANG_1, LANG_2}
    'Private LANG()() As Integer = {New Integer() {1, 2}, New Integer() {&H41, &H42}}
    'Private ReadOnly LANG_0() As Integer = {&H41, &H42} ' TODO - create dynamically from other tab
    'Private ReadOnly LANG_BE() As Integer = {48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 138, 154, 158, 193, 201,
    '    214, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 237, 238, 240, 241, 243, 244, 245,
    '    246, 248, 250, 252, 253, 257, 261, 263, 268, 269, 275, 279, 281, 287, 299, 305, 321, 322, 324, 333, 345,
    '    349, 359, 367, 375, 394, 400, 402, 406, 408, 417, 418, 419, 420, 422, 423, 424, 425, 426, 428, 429, 430,
    '    431, 432, 433, 434, 436, 437, 438, 439, 440, 441, 442, 443, 444, 446, 449, 450, 452, 453, 454, 455, 456,
    '    457, 458, 459, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477, 478,
    '    479, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499,
    '    500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 512, 513, 514, 515, 516, 517, 518, 519, 520,
    '    521, 522, 523, 524, 525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538, 539, 540, 541,
    '    542, 543, 544, 545}
    'Private ReadOnly LANG_BG() As Integer = {&H63, &H64}
    ''TODO: Update and continue above list then add to LANG
    'Private LANG()() As Integer = {LANG_0, LANG_BE, LANG_BG}


    'Private DisableChars As String ' JobIno_EG.ProcessOptions.DisableCharW() as a string

#End Region

#Region " Event handlers "

    'Tab 1 - Manual
    Private Sub NumericCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles NumericCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtNUMERIC, NumericCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub SimpleSymbolsCheckBox_CheckedStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SimpleSymbolsCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtSIMPLESYMBOLS, SimpleSymbolsCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub SimpleLowercaseCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SimpleLowercaseCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtSIMPLELOWERCASE, SimpleLowercaseCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub SimpleUppercaseLetterCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SimpleUppercaseCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtSIMPLEUPPERCASE, SimpleUppercaseCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub WesternEuropeanCheckBox_CheckedStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WesternEuropeanCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtWESTERNEUROPEAN, WesternEuropeanCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub ExtendedSymbolsCheckBox_CheckedStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExtendedSymbolsCheckbox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtEXTENDEDSYMBOLS, ExtendedSymbolsCheckbox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub ExtendedUppercaseLetterCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExtendedUppercaseCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtEXTENDEDUPPERCASE, ExtendedUppercaseCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub ExtendedLowercaseCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExtendedLowercaseCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtEXTENDEDLOWERCASE, ExtendedLowercaseCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub GreekAndCopticCheckBox_CheckedStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GreekAndCopticCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtGREEKCOPTIC, GreekAndCopticCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub CyrilicCheckBox_CheckedStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CyrillicCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtCYRILLIC, CyrillicCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub EasternEuropeanCheckBox_CheckedStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EasternEuropeanCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtEASTERNEUROPEAN, EasternEuropeanCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub AllCharactersCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AllCharactersCheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtALL, AllCharactersCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub InvertButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvertButton.MouseClick
        Dim GlyphNo As Integer  ' glyph number

        If Not IgnoreCheckBoxChange Then
            For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
                If IsValidGlyphNo(GlyphNo) Then
                    ManualDisableGlyphs(GlyphNo) = Not ManualDisableGlyphs(GlyphNo)
                End If
            Next GlyphNo
            CharactersPictureBox.Invalidate()
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub CharactersPictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CharactersPictureBox.MouseDown
        Dim XS As Integer       ' column width
        Dim YS As Integer       ' row height
        Dim Row As Integer      ' row number (0 to 17)
        Dim Col As Integer      ' column number (0 to 31)
        Dim CharNo As Integer   ' character number
        Dim GlyphNo As Integer  ' glyph number

        ' The bitmap is 32 columns by 18 rows
        XS = CInt((CharactersPictureBox.Width - 1) / 32)
        YS = CInt((CharactersPictureBox.Height - 1) / 18)

        ' Map the glyphs in the bitmap to the ASCII character set
        Col = e.X \ XS
        Row = e.Y \ YS
        'Col = 1
        'Row = 1

        ' Omit gridlines to remove edge effects and ambiguity
        If Col * XS = e.X Or Row * YS = e.Y Then Return

        GlyphNo = Row * 32 + Col
        CharNo = GlyphNoToCharNoUnchecked(GlyphNo)

        If IsValidCharNo(CharNo) Then 'V5 check

            ' Left click brings up the large image of the character and right click
            ' disables/enables the character

            If e.Button = Windows.Forms.MouseButtons.Left Then

                ' The glyph rectangle size in the enlarged bitmap is 52x45

                BigCharPictureBox.Location = New Point(-Col * 52, -Row * 45)
                BigCharPanel.Location = New Point(CInt(CharactersPictureBox.Left + Col * XS + (XS - 42) / 2), CInt(CharactersPictureBox.Top + Row * YS + (YS - 45) / 2))
                'V5 new
                CharNoTextBoxUnicode.Text = "U+" & TOCRGL4(GlyphNo).ToString("X4")
                CharNoTextBoxUnicode.Location = New Point(CInt(BigCharPanel.Left + (BigCharPanel.Width - CharNoTextBoxUnicode.Width) / 2), BigCharPanel.Top - CharNoTextBoxUnicode.Height)
                'V4
                CharNoTextBoxInternal.Text = CStr(CharNo)
                CharNoTextBoxInternal.Location = New Point(CInt(BigCharPanel.Left + (BigCharPanel.Width - CharNoTextBoxInternal.Width) / 2), BigCharPanel.Top + BigCharPanel.Height)
                Cursor.Hide()
                BigCharPanel.Visible = True
                CharNoTextBoxInternal.Visible = True
                CharNoTextBoxUnicode.Visible = True
            Else ' right click
                Dim state As GLYPHSTATES

                state = GetGlyphState(GlyphNo)

                If (IsGlyphStateDisabled(state)) Then
                    OverideGlyphs(GlyphNo) = OVERIDESTATE.osENABLE
                ElseIf (state = GLYPHSTATES.gsENABLED_BY_OVERRIDE) Then
                    OverideGlyphs(GlyphNo) = OVERIDESTATE.osNONE
                Else
                    OverideGlyphs(GlyphNo) = OVERIDESTATE.osDISABLE
                End If

                'ManualDisableGlyphs(GlyphNo) = Not ManualDisableGlyphs(GlyphNo) 'V4 CharNo - V5 GlyphNo
                CharactersPictureBox.Invalidate()
                UpdateCheckBoxes(GlyphNo) 'V4 CharNo - V5 GlyphNo
                End If
        End If
    End Sub

    Private Sub CharactersPictureBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CharactersPictureBox.MouseUp
        BigCharPanel.Visible = False
        CharNoTextBoxInternal.Visible = False
        CharNoTextBoxUnicode.Visible = False
        Cursor.Show()
    End Sub

    Private Function GetGlyphState(GlyphNo As Integer) As GLYPHSTATES
        If (IsValidGlyphNo(GlyphNo)) Then
            If (ManualDisableGlyphs(GlyphNo)) Then
                If (LanguageDisableGlyphs(GlyphNo)) Then
                    If (OverideGlyphs(GlyphNo) = OVERIDESTATE.osENABLE) Then
                        GetGlyphState = GLYPHSTATES.gsENABLED_BY_OVERRIDE
                    Else
                        GetGlyphState = GLYPHSTATES.gsDISABLED_BY_LANGUAGE_AND_MANUAL
                    End If
                Else
                    If (OverideGlyphs(GlyphNo) = OVERIDESTATE.osENABLE) Then
                        GetGlyphState = GLYPHSTATES.gsENABLED_BY_OVERRIDE
                    Else
                        GetGlyphState = GLYPHSTATES.gsDISABLED_BY_MANUAL
                    End If
                End If
            Else
                If (LanguageDisableGlyphs(GlyphNo)) Then
                    If (OverideGlyphs(GlyphNo) = OVERIDESTATE.osENABLE) Then
                        GetGlyphState = GLYPHSTATES.gsENABLED_BY_OVERRIDE
                    Else
                        GetGlyphState = GLYPHSTATES.gsDISABLED_BY_LANGUAGE
                    End If
                Else
                    If (OverideGlyphs(GlyphNo) = OVERIDESTATE.osDISABLE) Then
                        GetGlyphState = GLYPHSTATES.gsDISABLED_BY_OVERRIDE
                    Else
                        GetGlyphState = GLYPHSTATES.gsENABLED
                    End If
                End If
            End If
        Else
            GetGlyphState = GLYPHSTATES.gsEMPTY
        End If
    End Function

    Private Sub CharactersPictureBox_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles CharactersPictureBox.Paint
        'Dim CharNo As Integer   ' character number
        Dim GlyphNo As Integer  ' glyph number
        Dim G As Graphics
        Dim o As New Pen(Color.Red)
        Dim m As New Pen(Color.Orange)
        Dim l As New Pen(Color.Blue)
        Dim XS As Integer       ' column width
        Dim YS As Integer       ' row height
        Dim Row As Integer      ' row number (0 to 6)
        Dim Col As Integer      ' column number (0 to 31)
        Dim X As Integer        ' X position in picCharacters
        Dim Y As Integer        ' Y position in picCharacters
        ' Dim Manual As Boolean
        ' Dim Language As Boolean
        Dim state As GLYPHSTATES

        o.Width = 2
        m.Width = 2
        l.Width = 2

        G = e.Graphics

        For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            state = GetGlyphState(GlyphNo)

            ' The bitmap is 32 columns by 18 rows
            XS = CInt((CharactersPictureBox.Width - 1) / 32)
            YS = CInt((CharactersPictureBox.Height - 1) / 18)

            Row = GlyphNo \ 32
            Col = GlyphNo - Row * 32

            X = Col * XS + 2
            Y = Row * YS + 2

            Select Case state
                Case GLYPHSTATES.gsDISABLED_BY_LANGUAGE_AND_MANUAL
                    G.DrawLine(m, X, Y, X + XS - 4, Y + YS - 4)
                    G.DrawLine(l, X, Y + YS - 4, X + XS - 4, Y)
                Case GLYPHSTATES.gsDISABLED_BY_MANUAL
                    G.DrawLine(m, X, Y, X + XS - 4, Y + YS - 4)
                    G.DrawLine(m, X, Y + YS - 4, X + XS - 4, Y)
                Case GLYPHSTATES.gsDISABLED_BY_LANGUAGE
                    G.DrawLine(l, X, Y, X + XS - 4, Y + YS - 4)
                    G.DrawLine(l, X, Y + YS - 4, X + XS - 4, Y)
                Case GLYPHSTATES.gsDISABLED_BY_OVERRIDE
                    G.DrawLine(o, X, Y, X + XS - 4, Y + YS - 4)
                    G.DrawLine(o, X, Y + YS - 4, X + XS - 4, Y)
                Case GLYPHSTATES.gsENABLED_BY_OVERRIDE
                    G.DrawLine(o, X, Y, X + XS - 4, Y)
                    G.DrawLine(o, X + XS - 4, Y, X + XS - 4, Y + YS - 4)
                    G.DrawLine(o, X + XS - 4, Y + YS - 4, X, Y + YS - 4)
                    G.DrawLine(o, X, Y + YS - 4, X, Y)
            End Select
        Next GlyphNo
        o = Nothing
        m = Nothing
        l = Nothing
    End Sub


    Private Function IsGlyphStateDisabled(state As GLYPHSTATES) As Boolean
        If ((state = GLYPHSTATES.gsENABLED) Or (state = GLYPHSTATES.gsENABLED_BY_OVERRIDE)) Then
            IsGlyphStateDisabled = False
        Else
            IsGlyphStateDisabled = True
        End If
    End Function

    Private Function IsGlyphDisabled(GlyphNo As Integer) As Boolean
        Dim state As GLYPHSTATES

        state = GetGlyphState(GlyphNo)
        IsGlyphDisabled = IsGlyphStateDisabled(state)
    End Function

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Dim Result As DialogResult = Windows.Forms.DialogResult.Cancel  ' button's dialog result
        Dim GlyphNo As Integer          ' utility counter for the glyphs
        Dim UDL1 As String              ' list of hex unicode points for user defined list 1
        Dim UDL2 As String              ' list of hex unicode points for user defined list 2
        Dim UDL3 As String              ' list of hex unicode points for user defined list 3
        'Dim Txt As String               ' utility
        Dim CharNo As Integer            ' utility

        ' Check for changes and return Ok or Cancel as appropriate
        With JobInfo_EG.ProcessOptions
            If InvertWholePageCheckBox.Checked = Not CBool(.InvertWholePage) Then Result = Windows.Forms.DialogResult.OK
            .InvertWholePage = CByte(InvertWholePageCheckBox.Checked)

            If DeskewOffCheckBox.Checked = CBool(.DeskewOff) Then Result = Windows.Forms.DialogResult.OK
            .DeskewOff = CByte(Not DeskewOffCheckBox.Checked)

            If NoiseRemoveOffCheckBox.Checked = CBool(.NoiseRemoveOff) Then Result = Windows.Forms.DialogResult.OK
            .NoiseRemoveOff = CByte(Not NoiseRemoveOffCheckBox.Checked)

            If NoiseReturnCheckBox.Checked = CBool(.ReturnNoiseOn) Then Result = Windows.Forms.DialogResult.OK
            .ReturnNoiseOn = CByte(Not NoiseReturnCheckBox.Checked)

            If LineRemoveOffCheckBox.Checked = CBool(.LineRemoveOff) Then Result = Windows.Forms.DialogResult.OK
            .LineRemoveOff = CByte(Not LineRemoveOffCheckBox.Checked)

            If DeshadeOffCheckBox.Checked = CBool(.DeshadeOff) Then Result = Windows.Forms.DialogResult.OK
            .DeshadeOff = CByte(Not DeshadeOffCheckBox.Checked)

            If InvertOffCheckBox.Checked = CBool(.InvertOff) Then Result = Windows.Forms.DialogResult.OK
            .InvertOff = CByte(Not InvertOffCheckBox.Checked)

            If SectioningOnCheckBox.Checked = Not CBool(.SectioningOn) Then Result = Windows.Forms.DialogResult.OK
            .SectioningOn = CByte(SectioningOnCheckBox.Checked)

            If MergeBreakOffCheckBox.Checked = CBool(.MergeBreakOff) Then Result = Windows.Forms.DialogResult.OK
            .MergeBreakOff = CByte(Not MergeBreakOffCheckBox.Checked)

            If LineRejectOffCheckBox.Checked = CBool(.LineRejectOff) Then Result = Windows.Forms.DialogResult.OK
            .LineRejectOff = CByte(Not LineRejectOffCheckBox.Checked)

            If CharacterRejectOffCheckBox.Checked = CBool(.CharacterRejectOff) Then Result = Windows.Forms.DialogResult.OK
            .CharacterRejectOff = CByte(Not CharacterRejectOffCheckBox.Checked)

            If OCRBOnlyCheckBox.Checked <> CBool(.OCRBOnly) Then Result = Windows.Forms.DialogResult.OK
            .OCRBOnly = CByte(OCRBOnlyCheckBox.Checked)

            If FontInfoCheckBox.Checked <> Not CBool(.FontStyleInfoOff) Then Result = Windows.Forms.DialogResult.OK
            .FontStyleInfoOff = CByte(Not FontInfoCheckBox.Checked)

            If RotateAutoRadioButton.Checked Then .Orientation = TOCRJOBORIENT_AUTO
            If RotateOffRadioButton.Checked Then .Orientation = TOCRJOBORIENT_OFF
            If Rotate90RadioButton.Checked Then .Orientation = TOCRJOBORIENT_90
            If Rotate180RadioButton.Checked Then .Orientation = TOCRJOBORIENT_180
            If Rotate270RadioButton.Checked Then .Orientation = TOCRJOBORIENT_270


            If ExpressSpeedRadioButton.Checked Then
                .Speed = 3
            ElseIf FastSpeedRadioButton.Checked Then
                .Speed = 2
            ElseIf MediumSpeedRadioButton.Checked Then
                .Speed = 1
            Else
                .Speed = 0
            End If

            ' V4 code - 
            'If OffLexRadioButton.Checked Then
            '    .LexMode = 2
            'ElseIf OnLexRadioButton.Checked Then
            '    .LexMode = 1
            'Else
            '    .LexMode = 0
            'End If
            'TODO: new V5 code - is this ok?
            .LexMode = 0 ' i.e. automatic

        End With

        'Txt = ""
        'Array.Clear(JobInfo_EG.ProcessOptions.DisableCharW, 0, JobInfo_EG.ProcessOptions.DisableCharW.GetUpperBound(0) + 1)
        'DisNo = 0
        For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            If (IsValidGlyphNo(GlyphNo)) Then
                CharNo = GlyphNoToCharNoUnchecked(GlyphNo)
                If (IsGlyphDisabled(GlyphNo)) Then
                    If (JobInfo_EG.ProcessOptions.DisableCharW(CharNo) = 0) Then
                        JobInfo_EG.ProcessOptions.DisableCharW(CharNo) = 1 ' not zero = true
                        Result = Windows.Forms.DialogResult.OK ' something changed
                    End If
                Else
                    If (JobInfo_EG.ProcessOptions.DisableCharW(CharNo) <> 0) Then
                        JobInfo_EG.ProcessOptions.DisableCharW(CharNo) = 0 ' zero = false
                        Result = Windows.Forms.DialogResult.OK ' something changed
                    End If
                End If
            End If
        Next GlyphNo
        'If DisableChars <> Txt Then
        'DisableChars = Txt
        'Result = Windows.Forms.DialogResult.OK
        'End If

        'Now set the Diasabled Languages array on Process Options structure
        'Clear the array first
        'Array.Clear(JobInfo_EG.ProcessOptions.DisableLangs, 0, JobInfo_EG.ProcessOptions.DisableLangs.GetUpperBound(0) + 1)
        'Work out the languages on the treecontrol and put into the structure
        Dim anyChanged As Boolean
        anyChanged = False
        anyChanged = SetDisabledLangs(LanguageTree, JobInfo_EG.ProcessOptions.DisableLangs)

        'Then check if anything has changes as to whether need to enforce OK 
        If (True = anyChanged) Then
            Result = Windows.Forms.DialogResult.OK
        End If


        SaveSetting(REGAPPNAME, "OCR", "UserDefinedList1Name", UDL1CheckBox.Text)
        SaveSetting(REGAPPNAME, "OCR", "UserDefinedList2Name", UDL2CheckBox.Text)
        SaveSetting(REGAPPNAME, "OCR", "UserDefinedList3Name", UDL3CheckBox.Text)

        UDL1 = ","
        UDL2 = ","
        UDL3 = ","
        For GlyphNo = 0 To GlyphType.GetUpperBound(0)
            If (GlyphType(GlyphNo) And GLYPHTYPES.gtUDL1) <> 0 Then UDL1 &= TOCRGL4(GlyphNo).ToString("X") & ","
            If (GlyphType(GlyphNo) And GLYPHTYPES.gtUDL2) <> 0 Then UDL2 &= TOCRGL4(GlyphNo).ToString("X") & ","
            If (GlyphType(GlyphNo) And GLYPHTYPES.gtUDL3) <> 0 Then UDL3 &= TOCRGL4(GlyphNo).ToString("X") & ","
        Next
        SaveSetting(REGAPPNAME, "OCR", "UserDefinedList1", UDL1)
        SaveSetting(REGAPPNAME, "OCR", "UserDefinedList2", UDL2)
        SaveSetting(REGAPPNAME, "OCR", "UserDefinedList3", UDL3)

        Me.DialogResult = Result
    End Sub

    Private Sub ProcessOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim GlyphNo As Integer      ' loop counter
        Dim El() As String = TOCRGL4STR.Split(","c)
        Dim Txt As String
        Dim UDL1 As String          ' list of hex unicode points for user defined list 1
        Dim UDL2 As String          ' list of hex unicode points for user defined list 2
        Dim UDL3 As String          ' list of hex unicode points for user defined list 3

        UDL1CheckBox.Text = GetSetting(REGAPPNAME, "OCR", "UserDefinedList1Name", "User defined list 1")
        UDL2CheckBox.Text = GetSetting(REGAPPNAME, "OCR", "UserDefinedList2Name", "User defined list 2")
        UDL3CheckBox.Text = GetSetting(REGAPPNAME, "OCR", "UserDefinedList3Name", "User defined list 3")

        UDL1 = GetSetting(REGAPPNAME, "OCR", "UserDefinedList1")
        UDL2 = GetSetting(REGAPPNAME, "OCR", "UserDefinedList2")
        UDL3 = GetSetting(REGAPPNAME, "OCR", "UserDefinedList3")

        UDL1CheckBox.Enabled = False
        UDL2CheckBox.Enabled = False
        UDL3CheckBox.Enabled = False

        For GlyphNo = 0 To TOCRGL4.GetUpperBound(0)
            TOCRGL4(GlyphNo) = Convert.ToUInt16(El(GlyphNo), 16)
            'If (IsValidGlyphNo(GlyphNo)) Then
            ' CharNo = GlyphNoToCharNoUnchecked(GlyphNo)
            ' If JobInfo_EG.ProcessOptions.DisableCharW(CharNo) <> 0 Then
            ' 'DisableChars &= ChrW(DisNo)
            'ManualDisableGlyphs(GlyphNo) = True
            'End If
            'End If
            'If DisableChars.IndexOf(ChrW(TOCRGL4(GlyphNo))) <> -1 Then DisableGlyphs(GlyphNo) = True
            Txt = "," & El(GlyphNo) & ","
            If SIMPLESYMBOLS.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtSIMPLESYMBOLS
            If EXTENDEDSYMBOLS.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtEXTENDEDSYMBOLS
            If NUMERIC.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtNUMERIC
            If CYRILLIC.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtCYRILLIC
            If GREEKCOPTIC.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtGREEKCOPTIC
            If EASTERNEUROPEAN.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtEASTERNEUROPEAN
            If WESTERNEUROPEAN.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtWESTERNEUROPEAN
            If SIMPLEUPPERCASE.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtSIMPLEUPPERCASE
            If EXTENDEDUPPERCASE.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtEXTENDEDUPPERCASE
            If SIMPLELOWERCASE.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtSIMPLELOWERCASE
            If EXTENDEDLOWERCASE.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtEXTENDEDLOWERCASE
            If UDL1.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtUDL1 : UDL1CheckBox.Enabled = True
            If UDL2.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtUDL2 : UDL2CheckBox.Enabled = True
            If UDL3.IndexOf(Txt) >= 0 Then GlyphType(GlyphNo) = GlyphType(GlyphNo) Or GLYPHTYPES.gtUDL3 : UDL3CheckBox.Enabled = True
        Next

        ' Load up settings
        With JobInfo_EG.ProcessOptions
            InvertWholePageCheckBox.Checked = CBool(.InvertWholePage)
            DeskewOffCheckBox.Checked = Not CBool(.DeskewOff)
            NoiseRemoveOffCheckBox.Checked = Not CBool(.NoiseRemoveOff)
            NoiseReturnCheckBox.Checked = Not CBool(.ReturnNoiseOn)
            LineRemoveOffCheckBox.Checked = Not CBool(.LineRemoveOff)
            DeshadeOffCheckBox.Checked = Not CBool(.DeshadeOff)
            InvertOffCheckBox.Checked = Not CBool(.InvertOff)
            SectioningOnCheckBox.Checked = CBool(.SectioningOn)
            MergeBreakOffCheckBox.Checked = Not CBool(.MergeBreakOff)
            LineRejectOffCheckBox.Checked = Not CBool(.LineRejectOff)
            CharacterRejectOffCheckBox.Checked = Not CBool(.CharacterRejectOff)
            OCRBOnlyCheckBox.Checked = CBool(.OCRBOnly)
            FontInfoCheckBox.Checked = Not CBool(.FontStyleInfoOff)

            Select Case .Orientation
                Case TOCRJOBORIENT_AUTO
                    RotateAutoRadioButton.Checked = True
                Case TOCRJOBORIENT_OFF
                    RotateOffRadioButton.Checked = True
                Case TOCRJOBORIENT_90
                    Rotate90RadioButton.Checked = True
                Case TOCRJOBORIENT_180
                    Rotate180RadioButton.Checked = True
                Case TOCRJOBORIENT_270
                    Rotate270RadioButton.Checked = True
            End Select

            If .Speed = 3 Then
                ExpressSpeedRadioButton.Checked = True
            ElseIf .Speed = 2 Then
                FastSpeedRadioButton.Checked = True
            ElseIf .Speed = 1 Then
                MediumSpeedRadioButton.Checked = True
            Else
                SlowSpeedRadioButton.Checked = True
            End If

            'old V4 code - removed
            'If .LexMode = 2 Then
            '    OffLexRadioButton.Checked = True
            'ElseIf .LexMode = 1 Then
            '    OnLexRadioButton.Checked = True
            'Else
            '    AutoLexRadioButton.Checked = True
            'End If
        End With

        LanguageTree.ExpandAll()
        TickLanguageTreeItemsFoundInList(LanguageTree, JobInfo_EG.ProcessOptions.DisableLangs)
        UpdateLanguages(LanguageTree)

        UpdateCheckBoxes()
    End Sub

    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetButton.Click
        Dim GlyphNo As Integer

        InvertWholePageCheckBox.Checked = False
        DeskewOffCheckBox.Checked = True
        NoiseRemoveOffCheckBox.Checked = True
        NoiseReturnCheckBox.Checked = True
        LineRemoveOffCheckBox.Checked = True
        DeshadeOffCheckBox.Checked = True
        InvertOffCheckBox.Checked = True
        SectioningOnCheckBox.Checked = False
        MergeBreakOffCheckBox.Checked = True
        LineRejectOffCheckBox.Checked = True
        CharacterRejectOffCheckBox.Checked = True
        OCRBOnlyCheckBox.Checked = False
        RotateAutoRadioButton.Checked = True
        SlowSpeedRadioButton.Checked = True
        'old V4 code - deleted
        'AutoLexRadioButton.Checked = True
        FontInfoCheckBox.Checked = True

        For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            ManualDisableGlyphs(GlyphNo) = False
        Next

        UpdateCheckBoxes()
        CharactersPictureBox.Invalidate()
    End Sub

    Private Sub UDLSetButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UDL1SetButton.Click, UDL2SetButton.Click, UDL3SetButton.Click
        Dim GlyphNo As Integer
        Dim Txt As String
        Dim But As Button
        Dim cb As CheckBox
        Dim cbNo As Integer
        Dim gt As GLYPHTYPES
        Dim NumGlyphs As Integer
        Dim NuminList As Integer

        But = CType(sender, System.Windows.Forms.Button)
        Select Case But.Name
            Case "UDL1SetButton"
                cb = UDL1CheckBox
                gt = GLYPHTYPES.gtUDL1
                cbNo = 1
            Case "UDL2SetButton"
                cb = UDL2CheckBox
                gt = GLYPHTYPES.gtUDL2
                cbNo = 2
            Case "UDL3SetButton"
                cb = UDL3CheckBox
                gt = GLYPHTYPES.gtUDL3
                cbNo = 3
            Case Else
                Exit Sub
        End Select

        For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            If Not ManualDisableGlyphs(GlyphNo) Then NumGlyphs += 1
            If (GlyphType(GlyphNo) And gt) <> 0 Then NuminList += 1
        Next

        If NumGlyphs = 0 Then
            If NuminList = 0 Then Exit Sub
            If MsgBox("Are you sure you wish to clear " & cb.Text, MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then Exit Sub
        Else
            Txt = InputBox("Enter a new name for this list" & vbCrLf & vbCrLf & "or 'Cancel' the entire operation", , cb.Text)
            If Txt = "" Then Exit Sub
            cb.Text = Txt
        End If

        For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            If ManualDisableGlyphs(GlyphNo) Then
                GlyphType(GlyphNo) = GlyphType(GlyphNo) Or gt
            Else
                GlyphType(GlyphNo) = GlyphType(GlyphNo) And (Not gt)
            End If
        Next

        IgnoreCheckBoxChange = True
        If NumGlyphs = 0 Then
            cb.CheckState = CheckState.Unchecked
            cb.Enabled = False
            cb.Text = "User defined list " & cbNo.ToString
        Else
            cb.CheckState = CheckState.Unchecked
            cb.Enabled = True
            UpdateCheckBoxes()
        End If
        IgnoreCheckBoxChange = False
    End Sub

    Private Sub UDL1CheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UDL1CheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtUDL1, UDL1CheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub


    Private Sub UDL2CheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UDL2CheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtUDL2, UDL2CheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub

    Private Sub UDL3CheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UDL3CheckBox.CheckStateChanged
        If Not IgnoreCheckBoxChange Then
            ToggleGlyphType(GLYPHTYPES.gtUDL3, UDL3CheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked)
            UpdateCheckBoxes()
        End If
    End Sub
#End Region

#Region " Private routines "
    'Convert a glyph number to a character number.
    Private Function GlyphNoToCharNoUnchecked(ByVal GlyphNo As Integer) As Integer
        Dim CharNo As Integer   ' character number
        CharNo = GlyphNo + 32
        Return CharNo
    End Function ' GlyphNoToCharNo

    'Convert a character number to a glyph number.
    Private Function CharNoToGlyphNoUnchecked(ByVal CharNo As Integer) As Integer
        Dim GlyphNo As Integer   ' character number
        GlyphNo = CharNo - 32
        Return GlyphNo
    End Function ' CharNoToGlyphNo

    ' Return True or false if the glyph number is supported
    Private Function IsValidGlyphNo(ByVal GlyphNo As Integer) As Boolean
        If GlyphNo >= 0 And GlyphNo <= TOCRGL4.GetUpperBound(0) Then
            If 0 <> TOCRGL4(GlyphNo) Then
                Return True
            End If
        End If
        Return False
    End Function ' IsValidGlyphNo

    ' Return True or false if the character number is supported
    Private Function IsValidCharNo(ByVal CharNo As Integer) As Boolean
        Dim GlyphNo As Integer 'glyph no
        GlyphNo = CharNoToGlyphNoUnchecked(CharNo)
        Return IsValidGlyphNo(GlyphNo)
    End Function ' IsValidCharNo

    ' Count the number of valid glyphs of a particular type and
    ' of those count which are disabled then decide whether the checkbox should
    ' be checked, unchecked or greyed.  Called from UpdateCheckBoxes.
    Private Function GetCheckBoxState(ByVal gt As GLYPHTYPES) As System.Windows.Forms.CheckState
        Dim GlyphNo As Integer      ' Glyph number
        Dim NumGlyphs As Integer    ' number of glyphs in range
        Dim NumDisabled As Integer  ' count of disabled glyphs in range

        NumDisabled = 0
        NumGlyphs = 0
        For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            If (GlyphType(GlyphNo) And gt) <> 0 Then
                NumGlyphs += 1
                If IsGlyphDisabled(GlyphNo) Then NumDisabled += 1
            End If
        Next GlyphNo

        ' Setting the checkbox value can cause a click event so disable it

        If NumDisabled = 0 Then
            Return System.Windows.Forms.CheckState.Unchecked
        ElseIf NumDisabled = NumGlyphs Then
            Return System.Windows.Forms.CheckState.Checked
        Else
            Return System.Windows.Forms.CheckState.Indeterminate
        End If
    End Function ' GetCheckBoxState

    ' Enable or disable all characters of a particular glyph type.
    Private Sub ToggleGlyphType(ByVal gt As GLYPHTYPES, ByVal Bool As Boolean)
        Dim GlyphNo As Integer  ' glyph number

        For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            If (GlyphType(GlyphNo) And gt) <> 0 Then
                ManualDisableGlyphs(GlyphNo) = (Not ManualDisableGlyphs(GlyphNo))
            End If
        Next GlyphNo
        CharactersPictureBox.Invalidate()
    End Sub ' ToggleGlyphTYpe

    ' Enable or disable all characters of a particular language.
    'Private Sub SetLanguage(ByRef CurrLang() As Integer)
    '    Dim GlyphNo As Integer ' counter for TOCRGL4
    '    Dim CharNo As Integer ' Char no to compare with languages list
    '    Dim LangCharCount As Integer ' counter for CurrLang

    '    'Starting points for arrays
    '    GlyphNo = 0
    '    LangCharCount = 0

    '    'Preset all to True first
    '    For GlyphNo = 0 To DisableGlyphs.GetUpperBound(0)
    '        DisableGlyphs(GlyphNo) = True
    '    Next GlyphNo

    '    'Reset
    '    GlyphNo = 0

    '    While GlyphNo < DisableGlyphs.GetUpperBound(0) And LangCharCount < CurrLang.GetLength(0)
    '        CharNo = GlyphNoToCharNoUnchecked(GlyphNo)
    '        If (CurrLang(LangCharCount) = CharNo) Then
    '            DisableGlyphs(GlyphNo) = False 'If they match then don't mark and move on both
    '            LangCharCount += 1
    '            GlyphNo += 1
    '        ElseIf CharNo < CurrLang(LangCharCount) Then
    '            DisableGlyphs(GlyphNo) = True ' Do mark
    '            GlyphNo += 1
    '        Else
    '            LangCharCount += 1 'somehow language count has got behind the glyph - should not happen?
    '        End If
    '    End While


    '    CharactersPictureBox.Invalidate()
    'End Sub ' SetLanguage

    ' If a character has been enabled or disabled update the chkEnable...
    ' checkboxes to reflect the change.  The optional parameter just provides
    ' a speed up when only one character has been changed.
    Private Sub UpdateCheckBoxes(Optional ByVal GlyphNo As Integer = NOTVALID)
        Dim gt As GLYPHTYPES

        IgnoreCheckBoxChange = True

        AllCharactersCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtALL)

        ' speed up
        If AllCharactersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked Or AllCharactersCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked Then
            SimpleSymbolsCheckBox.CheckState = AllCharactersCheckBox.CheckState
            ExtendedSymbolsCheckbox.CheckState = AllCharactersCheckBox.CheckState
            NumericCheckBox.CheckState = AllCharactersCheckBox.CheckState
            CyrillicCheckBox.CheckState = AllCharactersCheckBox.CheckState
            GreekAndCopticCheckBox.CheckState = AllCharactersCheckBox.CheckState
            EasternEuropeanCheckBox.CheckState = AllCharactersCheckBox.CheckState
            WesternEuropeanCheckBox.CheckState = AllCharactersCheckBox.CheckState
            SimpleUppercaseCheckBox.CheckState = AllCharactersCheckBox.CheckState
            ExtendedUppercaseCheckBox.CheckState = AllCharactersCheckBox.CheckState
            SimpleLowercaseCheckBox.CheckState = AllCharactersCheckBox.CheckState
            ExtendedLowercaseCheckBox.CheckState = AllCharactersCheckBox.CheckState
            If UDL1CheckBox.Enabled Then UDL1CheckBox.CheckState = AllCharactersCheckBox.CheckState
            If UDL2CheckBox.Enabled Then UDL2CheckBox.CheckState = AllCharactersCheckBox.CheckState
            If UDL3CheckBox.Enabled Then UDL3CheckBox.CheckState = AllCharactersCheckBox.CheckState
            IgnoreCheckBoxChange = False
            Return
        End If

        If GlyphNo = NOTVALID Then
            gt = GLYPHTYPES.gtALL
        Else
            gt = GlyphType(GlyphNo)
        End If
        If (gt And GLYPHTYPES.gtSIMPLESYMBOLS) <> 0 Then SimpleSymbolsCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtSIMPLESYMBOLS)
        If (gt And GLYPHTYPES.gtEXTENDEDSYMBOLS) <> 0 Then ExtendedSymbolsCheckbox.CheckState = GetCheckBoxState(GLYPHTYPES.gtEXTENDEDSYMBOLS)
        If (gt And GLYPHTYPES.gtNUMERIC) <> 0 Then NumericCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtNUMERIC)
        If (gt And GLYPHTYPES.gtCYRILLIC) <> 0 Then CyrillicCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtCYRILLIC)
        If (gt And GLYPHTYPES.gtGREEKCOPTIC) <> 0 Then GreekAndCopticCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtGREEKCOPTIC)
        If (gt And GLYPHTYPES.gtEASTERNEUROPEAN) <> 0 Then EasternEuropeanCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtEASTERNEUROPEAN)
        If (gt And GLYPHTYPES.gtWESTERNEUROPEAN) <> 0 Then WesternEuropeanCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtWESTERNEUROPEAN)
        If (gt And GLYPHTYPES.gtSIMPLEUPPERCASE) <> 0 Then SimpleUppercaseCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtSIMPLEUPPERCASE)
        If (gt And GLYPHTYPES.gtEXTENDEDUPPERCASE) <> 0 Then ExtendedUppercaseCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtEXTENDEDUPPERCASE)
        If (gt And GLYPHTYPES.gtSIMPLELOWERCASE) <> 0 Then SimpleLowercaseCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtSIMPLELOWERCASE)
        If (gt And GLYPHTYPES.gtEXTENDEDLOWERCASE) <> 0 Then ExtendedLowercaseCheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtEXTENDEDLOWERCASE)
        If (gt And GLYPHTYPES.gtUDL1) <> 0 And UDL1CheckBox.Enabled Then UDL1CheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtUDL1)
        If (gt And GLYPHTYPES.gtUDL2) <> 0 And UDL2CheckBox.Enabled Then UDL2CheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtUDL2)
        If (gt And GLYPHTYPES.gtUDL3) <> 0 And UDL3CheckBox.Enabled Then UDL3CheckBox.CheckState = GetCheckBoxState(GLYPHTYPES.gtUDL3)

        IgnoreCheckBoxChange = False
    End Sub ' UpdateCheckBoxes
#End Region

    'Convert a glyph number to a character number.
    Public Function GlyphNoToCharNo(ByVal GlyphNo As Integer) As Integer
        Dim CharNo As Integer   ' character number
        If (IsValidGlyphNo(GlyphNo)) Then
            CharNo = GlyphNoToCharNoUnchecked(GlyphNo)
        Else
            CharNo = 0
        End If

        Return CharNo
    End Function ' GlyphNoToCharNo

    'Convert a character number to a glyph number.
    Private Function CharNoToGlyphNo(ByVal CharNo As Integer) As Integer
        Dim GlyphNo As Integer   ' character number
        GlyphNo = CharNoToGlyphNoUnchecked(CharNo)

        If (Not IsValidGlyphNo(GlyphNo)) Then
            GlyphNo = 0
        End If

        Return GlyphNo
    End Function ' CharNoToGlyphNo

    ' Updates all child tree nodes recursively.
    Private Sub CheckAllChildNodes(treeNode As TreeNode, nodeChecked As Boolean)
        Dim node As TreeNode
        For Each node In treeNode.Nodes
            node.Checked = nodeChecked
            If node.Nodes.Count > 0 Then
                ' If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                Me.CheckAllChildNodes(node, nodeChecked)
            End If
        Next node
    End Sub

    'Update parent node if a child node changes
    Private Sub UpdateParentNode(treeNode As TreeNode)
        Dim blnUncheck As Boolean = False

        'Loop through the child nodes.
        For Each child As TreeNode In treeNode.Nodes
            'Check to see if the current node is unchecked.
            If child.Checked = False Then
                'Set the variable.
                blnUncheck = True
            End If
        Next

        'Check the variable.
        If blnUncheck = False Then
            'Check the parent node.
            treeNode.Checked = True
        Else
            'Uncheck the parent node.
            treeNode.Checked = False
        End If
    End Sub

    Private Function MarkDisabledLangs(myLANG As Integer, child As TreeNode, ByRef DisableLangs() As Byte) As Boolean
        Dim changed As Boolean
        changed = False
        Dim langVal As Byte
        'Check to see if the current node is checked and mark in DisableLangs
        If (child.Checked = True) Then
            langVal = 0
        Else
            langVal = 1
        End If

        If (langVal <> DisableLangs(myLANG)) Then
            'Don't match , i.e. a change had taken place
            changed = True
            DisableLangs(myLANG) = langVal
        End If
        Return changed
    End Function

    Private Function SetDisabledLangs(treeView As TreeView, ByRef DisableLangs() As Byte) As Boolean
        Dim changed As Boolean
        changed = False
        Dim anyChanged As Boolean
        anyChanged = False
        'Extract Language from the Tree Nodes's name
        Dim myObj As Object
        Dim myLANG As New Integer

        For Each parent As TreeNode In treeView.Nodes
            'Use absence of Tag within range to identify if top level node is not actually a parent
            myObj = parent.Tag
            myLANG = CInt(myObj)

            If (myLANG > 0) And (myLANG < 46) Then
                'Not actually a parent node, so check if language is set at this level
                changed = MarkDisabledLangs(myLANG, parent, DisableLangs)
            Else
                'This is a parent so loop through the child nodes.
                For Each child As TreeNode In parent.Nodes
                    myObj = child.Tag
                    myLANG = CInt(myObj)

                    changed = MarkDisabledLangs(myLANG, child, DisableLangs)
                Next child
            End If
            If (True = changed) Then
                anyChanged = True
                changed = False ' Rest for next loop
            End If
        Next parent
        Return anyChanged
    End Function

    Private Function TickLanguageTreeItemFoundInList(myLANG As Integer, child As TreeNode, ByRef DisableLangs() As Byte) As Boolean
        Dim changed As Boolean
        changed = False
        Dim langVal As Byte
        'Check to see if the current node is checked and mark in DisableLangs
        If (child.Checked = True) Then
            langVal = 0
        Else
            langVal = 1
        End If

        If (langVal <> DisableLangs(myLANG)) Then
            'Don't match , i.e. a change had taken place
            changed = True
            langVal = DisableLangs(myLANG)
        End If

        If (changed) Then
            If (langVal = 0) Then
                child.Checked = True
            Else
                child.Checked = False
            End If
        End If
        Return changed
    End Function

    Private Function TickLanguageTreeItemsFoundInList(treeView As TreeView, ByRef DisableLangs() As Byte) As Boolean
        Dim changed As Boolean
        changed = False
        Dim anyChanged As Boolean
        anyChanged = False
        'Extract Language from the Tree Nodes's name
        Dim myObj As Object
        Dim myLANG As New Integer

        For Each parent As TreeNode In treeView.Nodes
            'Use absence of Tag within range to identify if top level node is not actually a parent
            myObj = parent.Tag
            myLANG = CInt(myObj)

            If (myLANG > 0) And (myLANG < 46) Then
                'Not actually a parent node, so check if language is set at this level
                changed = TickLanguageTreeItemFoundInList(myLANG, parent, DisableLangs)
            Else
                'This is a parent so loop through the child nodes.
                For Each child As TreeNode In parent.Nodes
                    myObj = child.Tag
                    myLANG = CInt(myObj)

                    changed = TickLanguageTreeItemFoundInList(myLANG, child, DisableLangs)
                Next child

                UpdateParentNode(parent)
            End If
            If (True = changed) Then
                anyChanged = True
                changed = False ' Rest for next loop
            End If
        Next parent
        Return anyChanged
    End Function

    Private Sub UpdateLanguages(treeView As TreeView)
        'If selected switch all but the language characters to a blue cross
        If Not IgnoreCheckBoxChange Then 'Only respond if not ignoring the triggers

            Dim myStru As New TOCRLANGUAGEOPTIONS_EG
            Dim myResult As New TOCRCHAROPTIONS_EG
            Dim TocrResult As New Integer
            Dim GlyphNo As Integer ' counter for TOCRGL4
            Dim CharNo As Integer ' counter for TOCRGL4

            'Have to initialise the contents of the structures, so
            myStru.Initialize()
            myResult.Initialize()

            'Work out the languages on the treecontrol and put in MyStru
            SetDisabledLangs(treeView, myStru.DisableLangs)

            'copy DisableGlyphs to myStru.DisableCharW
            ' For GlyphNo = 0 To ManualDisableGlyphs.GetUpperBound(0)
            'If (IsValidGlyphNo(GlyphNo)) Then
            '   CharNo = GlyphNoToCharNoUnchecked(GlyphNo)
            '     If (ManualDisableGlyphs(GlyphNo) = True) Then
            '            myStru.DisableCharW(CharNo) = 1
            '      Else
            '           myStru.DisableCharW(CharNo) = 0
            'End If
            'End If
            'Next GlyphNo

            'Then call out to the dll to check which chars are disabled for the given language selection
            TocrResult = TOCRPopulateCharStatusMap(myStru, myResult)


            'copy myResult.DisableCharW to DisableCharW
            For GlyphNo = 0 To LanguageDisableGlyphs.GetUpperBound(0)
                If (IsValidGlyphNo(GlyphNo)) Then
                    CharNo = GlyphNoToCharNoUnchecked(GlyphNo)
                    If (myResult.DisableCharW(CharNo) = 0) Then
                        LanguageDisableGlyphs(GlyphNo) = True
                    Else
                        LanguageDisableGlyphs(GlyphNo) = False
                    End If
                End If
            Next GlyphNo

            CharactersPictureBox.Refresh() ' Call paint fn to put blue crosses on screen

        End If
    End Sub

    ' This code assumes that we only have two levels parent and child
    'TODO: need extra function if you want programmatic change to affect childnodes
    ' If a child node changes check if the parent needs updating
    ' After a tree node's Checked property is changed, all its child nodes are updated to the same value.
    Private Sub TreeView1_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles LanguageTree.AfterCheck
        'Note: This code only executes if the user caused the checked state to change
        If e.Action <> TreeViewAction.Unknown Then
            'First work out if this is a parent or child node
            'Check to see if a parent node exists.
            Dim hasParent As Boolean = True

            If e.Node.Parent Is Nothing Then
                hasParent = False
            End If

            If hasParent Then
                ' If a child node changes check if the parent needs updating
                Me.UpdateParentNode(e.Node.Parent)
            Else

                ' After a tree node's Checked property is changed, all its child nodes are updated to the same value.
                If e.Node.Nodes.Count > 0 Then
                    ' Calls the CheckAllChildNodes method, passing in the current 
                    ' Checked value of the TreeNode whose checked state changed. 
                    Me.CheckAllChildNodes(e.Node, e.Node.Checked) 'TODO: WILL ALSO NEED TO UPDATE LANGUAGE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                End If
            End If
            'Update the blue crosses to reflect the changes in languages selected
            Me.UpdateLanguages(e.Node.TreeView)
        End If
    End Sub


    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Select Case TabControl1.SelectedIndex
            Case 0 ' User clicks on First Tab
                CharactersPictureBox.Refresh() ' Call paint fn to swtich blue crosses to red crosses
                UpdateCheckBoxes() ' to set the correct checkboxes
            Case 1 ' User clicks on Second Tab
                UpdateLanguages(LanguageTree)
        End Select
    End Sub

End Class