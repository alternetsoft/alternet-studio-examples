'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class Form1
    Inherits System.Windows.Forms.Form
    
    Private WithEvents pictureBox1 As System.Windows.Forms.PictureBox
    
    Private WithEvents label1 As System.Windows.Forms.Label
    
    Private WithEvents CloseButton As System.Windows.Forms.Button
    
    Private components As System.ComponentModel.IContainer
    
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.pictureBox1 = New System.Windows.Forms.PictureBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        CType(Me.pictureBox1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'pictureBox1
        '
        Me.pictureBox1.Image = CType(resources.GetObject("pictureBox1.Image"),System.Drawing.Image)
        Me.pictureBox1.Location = New System.Drawing.Point(35, 66)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(183, 181)
        Me.pictureBox1.TabIndex = 0
        Me.pictureBox1.TabStop = false
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(38, 17)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(200, 36)
        Me.label1.TabIndex = 1
        Me.label1.Text = "Hello, World!"
        '
        'CloseButton
        '
        Me.CloseButton.Location = New System.Drawing.Point(35, 264)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(183, 45)
        Me.CloseButton.TabIndex = 2
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = true
        '
        'Form1
        '
        Me.ClientSize = New System.Drawing.Size(289, 318)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.pictureBox1)
        Me.Name = "Form1"
        CType(Me.pictureBox1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
    End Sub
End Class
