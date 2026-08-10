Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace CallMethod

    Public Class DisplayPanel
        Inherits Panel

        Public Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            UpdateStyles()
        End Sub

    End Class

End Namespace