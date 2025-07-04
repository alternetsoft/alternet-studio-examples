﻿namespace CodeSnippets
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Supress for Visual Studio-generated code")]
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnSettings = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbVBSnippets = new System.Windows.Forms.RadioButton();
            this.rbCSharpSnippets = new System.Windows.Forms.RadioButton();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.csharpSource = new Alternet.Editor.TextSource.TextSource(this.components);
            this.vbSource = new Alternet.Editor.TextSource.TextSource(this.components);
            this.pnSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.groupBox1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 116);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbVBSnippets);
            this.groupBox1.Controls.Add(this.rbCSharpSnippets);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 67);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Snippets";
            // 
            // rbVBSnippets
            // 
            this.rbVBSnippets.AutoSize = true;
            this.rbVBSnippets.Location = new System.Drawing.Point(8, 38);
            this.rbVBSnippets.Name = "rbVBSnippets";
            this.rbVBSnippets.Size = new System.Drawing.Size(81, 17);
            this.rbVBSnippets.TabIndex = 1;
            this.rbVBSnippets.Text = "VB snippets";
            this.rbVBSnippets.CheckedChanged += new System.EventHandler(this.CSharpSnippetsRadioButton_CheckedChanged);
            this.rbVBSnippets.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VBSnippetsRadioButton_MouseMove);
            // 
            // rbCSharpSnippets
            // 
            this.rbCSharpSnippets.AutoSize = true;
            this.rbCSharpSnippets.Checked = true;
            this.rbCSharpSnippets.Location = new System.Drawing.Point(8, 18);
            this.rbCSharpSnippets.Name = "rbCSharpSnippets";
            this.rbCSharpSnippets.Size = new System.Drawing.Size(81, 17);
            this.rbCSharpSnippets.TabIndex = 0;
            this.rbCSharpSnippets.TabStop = true;
            this.rbCSharpSnippets.Text = "C# snippets";
            this.rbCSharpSnippets.CheckedChanged += new System.EventHandler(this.CSharpSnippetsRadioButton_CheckedChanged);
            this.rbCSharpSnippets.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CSharpSnippetsRadioButton_MouseMove);
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(5, 5);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(657, 39);
            this.pnDescription.TabIndex = 8;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Left;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(621, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "Code Editor provides a way to insert frequently used fragmens of the code into th" +
    "e editor. Press \"Ctrl + K + X\" to see the snippets";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 116);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 5;
            this.syntaxEdit1.Text = "";
            // 
            // csharpSource
            // 
            this.csharpSource.OptimizedForMemory = false;
            this.csharpSource.Text = @"using System;
/// <summary>
/// Just a sample code to allow user have a look to our product and test it functionality both Editor and Syntax Parsing.
/// </summary>
public class Person
{
	public int age;
	public string name;
}

public class MainClass
{
	static void Main()
	{
		Person p = new Person();

		Console.Write(""Name: {0}, Age: {1}"", p.name, p.age);
	}
}";
            // 
            // vbSource
            // 
            this.vbSource.OptimizedForMemory = false;
            this.vbSource.Text = @"Imports System

''' <summary>
''' Just a sample code to allow user have a look to our product and test it functionality both Editor and Syntax Parsing.
''' </summary>
Public Class Person
	Public age As Integer
	Public name As String
End Class

Public Class MainClass

	Public Sub New()
		Dim p As Person = New Person
		Console.Write(""Name: {0}, Age: {1}"", p.name, p.age)
	End Sub

End Class";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 375);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Snippets";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVBSnippets;
        private System.Windows.Forms.RadioButton rbCSharpSnippets;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
        private Alternet.Editor.TextSource.TextSource csharpSource;
        private Alternet.Editor.TextSource.TextSource vbSource;
    }
}