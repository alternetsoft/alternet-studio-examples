Imports System.Data.Common
Imports System.IO
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Security.AccessControl
Imports Alternet.Common
Imports Alternet.Scripter
Imports Microsoft.CodeAnalysis.Differencing

Public Class Form1
    Inherits Form

    Private ScriptRun1 As Alternet.Scripter.ScriptRun
    Private button1 As Button
    Private label1 As Label
    Private item As ScriptGlobalItem
    Private externalClass As ExternalClass
    Private hostGlobalObject As HostGlobalObject
    Private sampleKind As ScriptSampleKind = ScriptSampleKind.PublicClass

    Public Enum ScriptSampleKind
        PublicClass
        MultiClass
        InplaceCode
        Project
        ClassLess
        ScriptInModule
    End Enum

    Public Sub New()

        InitializeComponent()

        Me.hostGlobalObject = New HostGlobalObject()
        Me.button1 = New Button()
        Me.label1 = New Label()

        Me.label1.AutoSize = True
        Me.label1.Text = "Click button to run sample: " & sampleKind.ToString()

        Me.Text = "Alternet Simple Script Run"
        Me.Width = 900
        Me.Height = 500

        Me.button1.Text = "Run Script"
        Me.button1.Left = 5
        Me.button1.Top = 5
        Me.button1.Width = 200
        Me.button1.Height = 100

        Me.label1.Left = 5
        Me.label1.Top = 150

        AddHandler Me.button1.Click, AddressOf Me.OnButton1Click
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.label1)

        Me.ScriptRun1 = New Alternet.Scripter.ScriptRun()

        externalClass = New ExternalClass(Me)

        item = New ScriptGlobalItem("External", externalClass)

        Width = 700
        Height = 500
    End Sub

    Private Sub OnButton1Click(sender As Object, e As EventArgs)
        RunScript()
    End Sub

    Private Sub RunScript()
        PathUtilities.UseAppSubFolderAsTempPath = False

        Dim unitName As String

        Select Case sampleKind
            Case ScriptSampleKind.ClassLess
                unitName = "ScriptClassLess.vbx"
            Case ScriptSampleKind.ScriptInModule
                unitName = "ScriptInModule.vb"
            Case Else
                unitName = "ScriptInClass.vb"
        End Select

        ' Configure the ScriptRun object

        If sampleKind = ScriptSampleKind.ClassLess Then
            ScriptRun1.ScriptLanguage = ScriptLanguage.VisualBasicScript
        Else
            ScriptRun1.ScriptLanguage = ScriptLanguage.VisualBasic
        End If

        ScriptRun1.ScriptHost.GenerateModulesOnDisk = False

        AddHandler Me.ScriptRun1.ScriptError, AddressOf Me.OnScriptError

        AddHandler Me.ScriptRun1.ScriptCompiled, AddressOf Me.OnScriptCompiled

        AddHandler Me.ScriptRun1.ScriptExecuted, AddressOf Me.OnScriptExecuted

        Dim fileName As String = Path.Combine(Alternet.Common.PathUtilities.GetAppFolder(), "Content", unitName)
        Dim oneMorefileName As String = Path.Combine(Alternet.Common.PathUtilities.GetAppFolder(), "Content", "OneMoreScript.vb")
        Dim projectFileName As String = Path.Combine(Alternet.Common.PathUtilities.GetAppFolder(), "Content", "ScriptProject.vbproj")

        Dim codeInplace As String =
"Imports System" & vbCrLf &
"Imports System.Drawing" & vbCrLf &
"Imports System.Diagnostics" & vbCrLf &
"Imports System.Windows.Forms" & vbCrLf &
"Imports Microsoft.VisualBasic" & vbCrLf &
"" & vbCrLf &
"Namespace ScriptSpace" & vbCrLf &
"    Public Class ScriptClass" & vbCrLf &
"        Public Shared Sub Main(str As String)" & vbCrLf &
"            MsgBox(""Message: "" & str)" & vbCrLf &
"        End Sub" & vbCrLf &
"    End Class" & vbCrLf &
"End Namespace"

        ' Load the code into the script runner using either a file or a code string

        ScriptRun1.AssemblyKind = ScriptAssemblyKind.DynamicLibrary
        ScriptRun1.ScriptMode = Alternet.Scripter.ScriptMode.Debug

        Select Case sampleKind
            Case ScriptSampleKind.MultiClass
                ScriptRun1.ScriptSource.Files.Add(fileName)
                ScriptRun1.ScriptSource.Files.Add(oneMorefileName)
            Case ScriptSampleKind.ClassLess
                Dim fileData As String = File.ReadAllText(fileName)
                ScriptRun1.ScriptSource.FromScriptCode(fileData)
            Case ScriptSampleKind.ScriptInModule
                ScriptRun1.ScriptSource.FromScriptFile(fileName)
            Case ScriptSampleKind.InplaceCode
                ScriptRun1.ScriptSource.FromScriptCode(codeInplace)
            Case ScriptSampleKind.PublicClass
                ScriptRun1.ScriptSource.FromScriptFile(fileName)
            Case ScriptSampleKind.Project
                ScriptRun1.ScriptSource.FromScriptProject(projectFileName)
        End Select

        ScriptRun1.ScriptSource.References.Clear()

        ' Add default references so MsgBox is recognized
        ScriptRun1.ScriptSource.WithDefaultReferences(ScriptTechnologyEnvironment.WindowsForms)
        ScriptRun1.ScriptSource.References.Add("Microsoft.VisualBasic.dll")

        ScriptRun1.ScriptSource.References.Add(GetType(ExternalClass).Assembly.Location)

        ScriptRun1.GlobalItems.Clear()
        ScriptRun1.GlobalItems.Add(item)

        ScriptRun1.ScriptHost.HostGlobalObject = hostGlobalObject

        ' Compile and run the "Main" method
        If ScriptRun1.Compile() Then
            Select Case sampleKind
                Case ScriptSampleKind.ClassLess
                    ScriptRun1.Run()
                Case ScriptSampleKind.ScriptInModule
                    ScriptRun1.RunMethod("ScriptModule.Main", Nothing, New Object() {"hello from script module"})
                Case ScriptSampleKind.InplaceCode
                    ScriptRun1.RunMethod("Main", Nothing, New Object() {"hello from script with inplace code"})
                Case ScriptSampleKind.PublicClass

                    ScriptRun1.ScriptHost.InitGlobalScriptObjects()
                    Dim assembly = ScriptRun1.ScriptHost.ScriptAssembly

                    Dim inst = assembly.CreateInstance("ScriptSpace.ScriptClass")

                    Dim testMethodWithException = True

                    If testMethodWithException Then
                        Dim methodInfo = inst.GetType().GetMethod("WithException")

                        Try
                            If methodInfo IsNot Nothing Then
                                methodInfo.Invoke(Nothing, Nothing)
                            End If
                        Catch ex As Exception

                            Dim originalEx = ex

                            If ex.InnerException IsNot Nothing Then
                                ex = ex.InnerException
                            End If

                            Debug.WriteLine("Caught exception from script:" & ex.ToString())

                            Debug.WriteLine("Stack trace:")

                            Dim st As New StackTrace(ex, True) ' True enables file info
                            For i As Integer = 0 To st.FrameCount - 1
                                Dim frame As StackFrame = st.GetFrame(i)
                                Dim methodName As String = frame.GetMethod().Name
                                Dim filePath As String = frame.GetFileName()   ' Full source file path
                                Dim lineNumber As Integer = frame.GetFileLineNumber()
                                Dim columnNumber As Integer = frame.GetFileColumnNumber()

                                Debug.WriteLine(
                                    "Method: " & methodName & ", File: " & filePath & ", Line: " & lineNumber & ", Column: " & columnNumber
                                )
                            Next

                        End Try
                    End If

                    Dim propInfoShared = inst.GetType().GetProperty("GlobalProp")

                    propInfoShared.SetValue(Nothing, "new shared value from main app")
                    Dim valShared = propInfoShared.GetValue(Nothing)

                    Dim propInfoInstance = inst.GetType().GetProperty("InstanceProp")
                    propInfoInstance.SetValue(inst, "new instance value from main app")
                    Dim valInstance = propInfoInstance.GetValue(inst)

                    If ScriptRun1.HasType("ScriptSpace.ScriptClass", True) Then
                        Debug.WriteLine("Type 'ScriptSpace.ScriptClass' is defined.")
                    Else
                        Debug.WriteLine("Type 'ScriptSpace.ScriptClass' is not defined.")
                    End If

                    Try
                        ScriptRun1.RunMethod("Main", Nothing, New Object() {"hello from script with public class"}, True)
                    Catch ex As Exception
                        MsgBox("Error: " & ex.Message)
                    End Try

                    Dim result As Integer = ScriptRun1.RunMethod("GetData", Nothing, Array.Empty(Of Object)())
                    MsgBox("GetData returned: " & result.ToString())

                Case ScriptSampleKind.MultiClass
                    ScriptRun1.RunMethod("OneMoreScript.SomeMethod", Nothing, New Object() {"OneMoreScript.SomeMethod is called"})
                    ScriptRun1.RunMethod("Main", Nothing, New Object() {"hello from main app from script project"})
                Case ScriptSampleKind.Project
                    ScriptRun1.RunMethod("OneMoreScript.SomeMethod", Nothing, New Object() {"OneMoreScript.SomeMethod is called"})
                    ScriptRun1.RunMethod("Main", Nothing, New Object() {"hello from main app from script project"})
            End Select
        Else
            ' Handle compilation errors if necessary

            Dim errorLog As New System.Text.StringBuilder()
            errorLog.AppendLine("Compilation failed with the following errors:")

            ' Access the CompilerErrors collection from the ScriptHost
            For Each TheErr As ScriptCompilationDiagnostic In ScriptRun1.ScriptHost.CompilerErrors
                errorLog.AppendLine(String.Format("Line {0}, Col {1}: {2}", TheErr.Line, TheErr.Column, TheErr.Message))
            Next

            MessageBox.Show(errorLog.ToString(), "Script Error")
            Debug.Print(errorLog.ToString())

        End If
    End Sub

    Private Sub OnScriptExecuted(sender As Object, e As EventArgs)
    End Sub

    Private Sub OnScriptCompiled(sender As Object, e As EventArgs)
    End Sub

    Private Sub OnScriptError(sender As Object, e As ScriptErrorEventArgs)
        MessageBox.Show(e.ErrorMessage, "Script Error")
    End Sub

End Class