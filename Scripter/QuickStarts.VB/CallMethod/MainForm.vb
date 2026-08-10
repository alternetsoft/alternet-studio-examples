Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports Alternet.Common
Imports Alternet.Editor.Common
Imports Alternet.Editor.Roslyn
Imports Alternet.Scripter

Namespace CallMethod

    Partial Public Class MainForm
        Inherits Form

        Private scriptRun As Alternet.Scripter.ScriptRun
        Private displayPanel As CallMethod.DisplayPanel
        Private pnDescription As Panel
        Private panel2 As Panel
        Private pnEdit As Panel
        Private runScriptButton As Button
        Private laDescription As Label
        Private cbLanguages As ComboBox
        Private laLanguages As Label
        Private toolTip1 As ToolTip
        Private components As System.ComponentModel.IContainer = Nothing

        Private Const LanguageDescription As String = "Choose programming language"
        Private updateTimer As Timer
        Private updateDeltaStopwatch As New Stopwatch()
        Private scriptRunning As Boolean

        Private edit As IScriptEdit

        Public Sub New()
            InitializeComponent()
            Dim asm = Me.[GetType]().Assembly
            Dim prefix = "CallMethod.Resources"

            CreateEditor()

            scriptRun.ScriptHost.GenerateModulesOnDisk = False

            updateTimer = New Timer()
            updateTimer.Interval = 50
            AddHandler updateTimer.Tick, AddressOf UpdateTimer_Tick

            cbLanguages.SelectedIndex = 0
            UpdateButtons()
        End Sub

        Public Sub InitializeComponent()
            Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
            Me.scriptRun = New Alternet.Scripter.ScriptRun()
            Me.pnDescription = New Panel()
            Me.cbLanguages = New ComboBox()
            Me.laLanguages = New Label()
            Me.runScriptButton = New Button()
            Me.laDescription = New Label()
            Me.panel2 = New Panel()
            Me.displayPanel = New CallMethod.DisplayPanel()
            Me.pnEdit = New Panel()
            Me.toolTip1 = New ToolTip()
            Me.pnDescription.SuspendLayout()
            Me.panel2.SuspendLayout()
            Me.SuspendLayout()
            '
            ' scriptRun
            '
            Me.scriptRun.ScriptMode = Alternet.Scripter.ScriptMode.Debug
            '
            ' pnDescription
            '
            Me.pnDescription.Controls.Add(Me.cbLanguages)
            Me.pnDescription.Controls.Add(Me.laLanguages)
            Me.pnDescription.Controls.Add(Me.runScriptButton)
            Me.pnDescription.Controls.Add(Me.laDescription)
            Me.pnDescription.Dock = DockStyle.Top
            Me.pnDescription.Location = New Point(0, 0)
            Me.pnDescription.Name = "pnDescription"
            Me.pnDescription.Size = New Size(615, 34)
            Me.pnDescription.TabIndex = 4
            '
            ' cbLanguages
            '
            Me.cbLanguages.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            Me.cbLanguages.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cbLanguages.Items.AddRange(New Object() {"Visual Basic", "C#"})
            Me.cbLanguages.Location = New Point(514, 8)
            Me.cbLanguages.Name = "cbLanguages"
            Me.cbLanguages.Size = New Size(98, 21)
            Me.cbLanguages.TabIndex = 22
            AddHandler cbLanguages.SelectedIndexChanged, AddressOf LanguagesComboBox_SelectedIndexChanged
            AddHandler cbLanguages.MouseMove, AddressOf LanguagesComboBox_MouseMove
            '
            ' laLanguages
            '
            Me.laLanguages.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            Me.laLanguages.AutoSize = True
            Me.laLanguages.Location = New Point(450, 11)
            Me.laLanguages.Name = "laLanguages"
            Me.laLanguages.Size = New Size(58, 13)
            Me.laLanguages.TabIndex = 21
            Me.laLanguages.Text = "Language:"
            '
            ' runScriptButton
            '
            Me.runScriptButton.Location = New Point(262, 8)
            Me.runScriptButton.Name = "runScriptButton"
            Me.runScriptButton.Size = New Size(64, 20)
            Me.runScriptButton.TabIndex = 3
            Me.runScriptButton.Text = "Run Script"
            Me.runScriptButton.UseVisualStyleBackColor = True
            AddHandler runScriptButton.Click, AddressOf RunScriptButton_Click
            '
            ' laDescription
            '
            Me.laDescription.Dock = DockStyle.Fill
            Me.laDescription.Location = New Point(0, 0)
            Me.laDescription.Name = "laDescription"
            Me.laDescription.Size = New Size(615, 34)
            Me.laDescription.TabIndex = 2
            Me.laDescription.Text = "This demo shows how to execute script methods."
            Me.laDescription.TextAlign = ContentAlignment.MiddleLeft
            '
            ' panel2
            '
            Me.panel2.Controls.Add(Me.displayPanel)
            Me.panel2.Dock = DockStyle.Top
            Me.panel2.Location = New Point(0, 34)
            Me.panel2.Name = "panel2"
            Me.panel2.Size = New Size(615, 143)
            Me.panel2.TabIndex = 5
            '
            ' displayPanel
            '
            Me.displayPanel.BackColor = Color.Black
            Me.displayPanel.Location = New Point(223, 5)
            Me.displayPanel.Name = "displayPanel"
            Me.displayPanel.Size = New Size(129, 130)
            Me.displayPanel.TabIndex = 3
            AddHandler Me.displayPanel.Paint, AddressOf DisplayPanel_Paint
            '
            ' pnEdit
            '
            Me.pnEdit.Dock = DockStyle.Fill
            Me.pnEdit.Location = New Point(0, 177)
            Me.pnEdit.Name = "pnEdit"
            Me.pnEdit.Size = New Size(615, 206)
            Me.pnEdit.TabIndex = 17
            '
            ' MainForm
            '
            Me.AutoScaleDimensions = New SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(615, 383)
            Me.Controls.Add(Me.pnEdit)
            Me.Controls.Add(Me.panel2)
            Me.Controls.Add(Me.pnDescription)
            Me.Name = "MainForm"
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "Call method"
            Me.pnDescription.ResumeLayout(False)
            Me.pnDescription.PerformLayout()
            Me.panel2.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub

        Public Sub StartScript()
            BeginInvoke(New Action(Sub()
                                       scriptRun.ScriptSource.FromScriptCode(edit.Text)
                                       scriptRun.ScriptSource.WithDefaultReferences()
                                       scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary
                                       If Not scriptRun.Compiled Then
                                           If Not scriptRun.Compile() Then
                                               MessageBox.Show(String.Join(vbCrLf, scriptRun.ScriptHost.CompilerErrors.Select(Function(x) x.ToString()).ToArray()))
                                               Return
                                           End If
                                       End If

                                       scriptRunning = True
                                       UpdateButtons()
                                       displayPanel.Refresh()
                                       updateTimer.Start()
                                   End Sub))
        End Sub

        Public Sub StopScript()
            BeginInvoke(New Action(Sub()
                                       scriptRunning = False
                                       UpdateButtons()
                                       updateTimer.Stop()
                                       displayPanel.Refresh()
                                   End Sub))
        End Sub

        Public Function IsScriptRunning() As Boolean
            Return scriptRunning
        End Function

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

        Private Sub LoadFile(edit As IScriptEdit, fileName As String)
            If (New FileInfo(fileName)).Exists Then
                edit.LoadFile(fileName)
            End If

            edit.FileName = fileName
        End Sub

        Private Sub CreateEditor()
            Dim sourceFileSubPath As String
            Dim language As ScriptLanguage
            GetSourceParametersForCSharp(sourceFileSubPath, language)
            Dim sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath)
            edit = CreateEditor(sourceFileFullPath, pnEdit)
        End Sub

        Private Function CreateEditor(fileName As String, parent As Control) As IScriptEdit
            Dim edit As IScriptEdit
            edit = New ScriptCodeEdit()
            edit.InitSyntax()
            edit.FileName = fileName
            edit.Parent = parent
            edit.Dock = DockStyle.Fill

            LoadFile(edit, fileName)

            Return edit
        End Function

        Private Sub GetSourceParametersForCSharp(ByRef sourceFileSubPath As String, ByRef language As ScriptLanguage)
            sourceFileSubPath = "CallMethod.cs"
            language = ScriptLanguage.CSharp
        End Sub

        Private Sub GetSourceParametersForVisualBasic(ByRef sourceFileSubPath As String, ByRef language As ScriptLanguage)
            sourceFileSubPath = "CallMethod.vb"
            language = ScriptLanguage.VisualBasic
        End Sub

        Private Function GetSourceFileFullPath(sourceFileSubPath As String) As String
            Const ResourcesFolderName As String = "Resources\Scripter"
            Dim myPath = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath)
            If Not File.Exists(myPath) Then
                myPath = Path.GetFullPath(Path.Combine(Application.StartupPath & "\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath))
                If Not File.Exists(myPath) Then
                    Throw New Exception("File not found: " & myPath)
                End If
            End If
            Return myPath
        End Function

        Private Sub UpdateSource(index As Integer)
            Dim sourceFileSubPath As String
            Dim language As ScriptLanguage
            Select Case index
                Case 1
                    GetSourceParametersForCSharp(sourceFileSubPath, language)
                Case Else
                    GetSourceParametersForVisualBasic(sourceFileSubPath, language)
            End Select

            Dim sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath)
            LoadFile(edit, sourceFileFullPath)

            scriptRun.ScriptLanguage = language
        End Sub

        Private Sub UpdateButtons()
            runScriptButton.Text = If(scriptRunning, "Stop Script", "Start Script")
        End Sub

        Private Sub RunScriptButton_Click(sender As Object, e As EventArgs)
            If scriptRunning Then
                StopScript()
            Else
                StartScript()
            End If
        End Sub

        Private Sub DisplayPanel_Paint(sender As Object, e As PaintEventArgs)
            If Not scriptRunning Then
                Return
            End If

            scriptRun.RunMethod("OnPaint", Nothing, New Object() {e.Graphics, displayPanel.ClientRectangle})
            updateDeltaStopwatch.Restart()
        End Sub

        Private Sub UpdateTimer_Tick(sender As Object, e As EventArgs)
            If Not scriptRunning Then
                Return
            End If

            scriptRun.RunMethod("OnUpdate", Nothing, New Object() {CInt(updateDeltaStopwatch.ElapsedMilliseconds)})
            updateDeltaStopwatch.Restart()
            displayPanel.Refresh()
        End Sub

        Private Sub LanguagesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)
            UpdateSource(cbLanguages.SelectedIndex)
        End Sub

        Private Sub LanguagesComboBox_MouseMove(sender As Object, e As MouseEventArgs)
            Dim str = toolTip1.GetToolTip(cbLanguages)
            If str <> LanguageDescription Then
                toolTip1.SetToolTip(cbLanguages, LanguageDescription)
            End If
        End Sub

    End Class
End Namespace