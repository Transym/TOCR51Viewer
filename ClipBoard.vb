'***********************************************************************************************************************
' Clipboard Monitor form
' 
' This class just raises an event when the Clipboard changes.  It is a forn so that
' the WndProc routine can be overridden.

Imports System.Windows.Forms

Public Class ClipboardMonitor
    Inherits System.Windows.Forms.Form

#Region " Definitions "
    'Constants for API Calls...
    Private Const WM_DRAWCLIPBOARD As Integer = &H308
    Private Const WM_CHANGECBCHAIN As Integer = &H30D

    'Handle for next clipboard viewer...
    Private mNextClipBoardViewerHWnd As IntPtr

    'API declarations...
    Declare Auto Function SetClipboardViewer Lib "user32" (ByVal HWnd As IntPtr) As IntPtr
    Declare Auto Function ChangeClipboardChain Lib "user32" (ByVal HWnd As IntPtr, ByVal HWndNext As IntPtr) As Boolean
    Declare Auto Function SendMessage Lib "User32" (ByVal HWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Long
#End Region

#Region " Events "
    Public Event Changed()
#End Region

#Region " Contructor "
    Public Sub New()
        'To register this form as a clipboard viewer...
        mNextClipBoardViewerHWnd = SetClipboardViewer(Me.Handle)
    End Sub
#End Region

#Region " Message Process "
    'Override WndProc to get messages...
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case Is = WM_DRAWCLIPBOARD 'The clipboard has changed...
                RaiseEvent Changed()
                SendMessage(mNextClipBoardViewerHWnd, m.Msg, m.WParam, m.LParam)
            Case Is = WM_CHANGECBCHAIN 'Another clipboard viewer has removed itself...
                If m.WParam = CType(mNextClipBoardViewerHWnd, IntPtr) Then
                    mNextClipBoardViewerHWnd = m.LParam
                Else
                    SendMessage(mNextClipBoardViewerHWnd, m.Msg, m.WParam, m.LParam)
                End If
        End Select

        MyBase.WndProc(m)
    End Sub
#End Region

#Region " Dispose "
    'Form overrides dispose to clean up...
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            Try
                'Set the next clipboard viewer back to the original... 
                ChangeClipboardChain(Me.Handle, mNextClipBoardViewerHWnd)
            Finally
                MyBase.Dispose(disposing)
            End Try
        End If
    End Sub
#End Region

End Class


