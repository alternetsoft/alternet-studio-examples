Imports System
Imports Alternet.UI
Imports Alternet.Drawing

Public Class Catcher
    Inherits System.ComponentModel.Component

    Public Sub CatchButton()
        ScriptGlobalClass.RunButton.Text = "Catch me if you can"
    End Sub

    Public Sub ChangeButtonLocation(ByVal obj As Object, ByVal args As EventArgs)
        Dim autoRand As New Random()
        Dim x As Integer = CInt(35 * autoRand.Next(0, 10) + 1)
        Dim y As Integer = CInt(15 * autoRand.Next(0, 10) + 1)
        ScriptGlobalClass.RunButton.Margin = New Thickness(x, y, 0, 0)
    End Sub

    Public Sub RunButtonClick(ByVal obj As Object, ByVal args As EventArgs)
        t.Stop()
        ScriptGlobalClass.RunButton.Text = "Test Button"
    End Sub

    Public Shared Function Main() As Catcher
        Dim f As New Catcher()
        f.CatchButton()
        Return f
    End Function

    Private t As Timer

    Public Sub New()
        t = New Timer()
        t.Interval = 1000
        AddHandler t.Tick, AddressOf ChangeButtonLocation
        AddHandler ScriptGlobalClass.RunButton.Click, AddressOf RunButtonClick
        t.Start()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            RemoveHandler ScriptGlobalClass.RunButton.Click, AddressOf RunButtonClick
            t.Stop()
            t.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

End Class