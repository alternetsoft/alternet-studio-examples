Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Public Class ExternalClass
    Private ReadOnly owner As Form1

    Friend Sub New(owner As Form1)
        Me.owner = owner
    End Sub

    Public Property X As Integer = 100

    Public Property Y As Integer = 200

    Public Property MainWindowText As String
        Get
            Return owner.Text
        End Get
        Set(value As String)
            owner.Text = value
        End Set
    End Property
End Class
