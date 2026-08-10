Imports System
Imports System.Drawing
Imports System.Diagnostics
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Namespace ScriptSpace
    Partial Public Class ScriptClass

        Public Shared ReadOnly Property SharedDefault As ScriptClass = New ScriptClass()

        ' Instance field initialized inline
        Private count As Integer = 5

        ' Shared (static) field initialized inline
        Public Shared globalVar As String = "olala"

        ' Compile-time constant
        Public Const Pi As Double = 3.14159265358979

        Public Shared Property GlobalProp As String
            Get
                Return globalVar
            End Get
            Set(value As String)
                globalVar = value
            End Set
        End Property

        Public Property InstanceProp As String
            Get
                Return globalVar
            End Get
            Set(value As String)
                globalVar = value
            End Set
        End Property

        ' Read-only field - can be initialized inline or in the constructor
        Private ReadOnly createdAt As Date = Date.UtcNow

        Public Shared Sub WithException()
            Throw New Exception("Exception raised from script with some message")
        End Sub

        Public Shared Sub Main(text As String)
            MsgBox("Main app says: " & text & " and " & globalVar)
            External.X = 10
            External.MainWindowText = "Text set from ScriptClass"
        End Sub

        Public Shared Function GetData() As Integer
            Return External.X
        End Function
    End Class
End Namespace
