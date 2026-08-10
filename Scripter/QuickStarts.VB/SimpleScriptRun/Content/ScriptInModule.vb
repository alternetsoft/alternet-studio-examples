Imports System
Imports System.Drawing
Imports System.Diagnostics
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Module ScriptModule
    Public Sub Main(text As String)
        MsgBox("From module: " & text)
    End Sub
End Module