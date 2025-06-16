namespace SnippetsParsers
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
            this.laDescription = new System.Windows.Forms.Label();
            this.pnSettings = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbClass = new System.Windows.Forms.RadioButton();
            this.rbMethod = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbClassLess = new System.Windows.Forms.RadioButton();
            this.rbReadonly = new System.Windows.Forms.RadioButton();
            this.rbPartial = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbVisualBasic = new System.Windows.Forms.RadioButton();
            this.rbCSharp = new System.Windows.Forms.RadioButton();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.pnDescriptions = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.readonlyCodeLabel2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.readonlyCodeLabel1 = new System.Windows.Forms.Label();
            this.pnEditors = new System.Windows.Forms.Panel();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.csSnippetSource = new Alternet.Editor.TextSource.TextSource(this.components);
            this.seBottom = new Alternet.Editor.SyntaxEdit(this.components);
            this.seTop = new Alternet.Editor.SyntaxEdit(this.components);
            this.vbSnippetSource = new Alternet.Editor.TextSource.TextSource(this.components);
            this.vbSource = new Alternet.Editor.TextSource.TextSource(this.components);
            this.csSource = new Alternet.Editor.TextSource.TextSource(this.components);
            this.rbHidden = new System.Windows.Forms.RadioButton();
            this.pnSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.pnDescriptions.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnEditors.SuspendLayout();
            this.SuspendLayout();
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(876, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "This demo project demonstrates how to use C# and VB parsers for code snippets.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.groupBox2);
            this.pnSettings.Controls.Add(this.groupBox3);
            this.pnSettings.Controls.Add(this.groupBox1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(886, 121);
            this.pnSettings.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbClass);
            this.groupBox2.Controls.Add(this.rbMethod);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(464, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(417, 72);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Method or Class snippet";
            // 
            // rbClass
            // 
            this.rbClass.AutoSize = true;
            this.rbClass.Location = new System.Drawing.Point(16, 43);
            this.rbClass.Name = "rbClass";
            this.rbClass.Size = new System.Drawing.Size(50, 17);
            this.rbClass.TabIndex = 1;
            this.rbClass.TabStop = true;
            this.rbClass.Text = "Class";
            this.rbClass.UseVisualStyleBackColor = true;
            this.rbClass.CheckedChanged += new System.EventHandler(this.MethodRadioButton_CheckedChanged);
            // 
            // rbMethod
            // 
            this.rbMethod.AutoSize = true;
            this.rbMethod.Checked = true;
            this.rbMethod.Location = new System.Drawing.Point(16, 19);
            this.rbMethod.Name = "rbMethod";
            this.rbMethod.Size = new System.Drawing.Size(61, 17);
            this.rbMethod.TabIndex = 0;
            this.rbMethod.TabStop = true;
            this.rbMethod.Text = "Method";
            this.rbMethod.UseVisualStyleBackColor = true;
            this.rbMethod.CheckedChanged += new System.EventHandler(this.MethodRadioButton_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbHidden);
            this.groupBox3.Controls.Add(this.rbClassLess);
            this.groupBox3.Controls.Add(this.rbReadonly);
            this.groupBox3.Controls.Add(this.rbPartial);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(198, 44);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(266, 72);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Partial or Readonly Lines";
            // 
            // rbClassLess
            // 
            this.rbClassLess.AutoSize = true;
            this.rbClassLess.Location = new System.Drawing.Point(92, 43);
            this.rbClassLess.Name = "rbClassLess";
            this.rbClassLess.Size = new System.Drawing.Size(71, 17);
            this.rbClassLess.TabIndex = 3;
            this.rbClassLess.Text = "Class-less";
            this.rbClassLess.UseVisualStyleBackColor = true;
            this.rbClassLess.CheckedChanged += new System.EventHandler(this.PartialRadioButton_CheckedChanged);
            // 
            // rbReadonly
            // 
            this.rbReadonly.AutoSize = true;
            this.rbReadonly.Location = new System.Drawing.Point(92, 19);
            this.rbReadonly.Name = "rbReadonly";
            this.rbReadonly.Size = new System.Drawing.Size(70, 17);
            this.rbReadonly.TabIndex = 1;
            this.rbReadonly.Text = "Readonly";
            this.rbReadonly.UseVisualStyleBackColor = true;
            this.rbReadonly.CheckedChanged += new System.EventHandler(this.PartialRadioButton_CheckedChanged);
            // 
            // rbPartial
            // 
            this.rbPartial.AutoSize = true;
            this.rbPartial.Checked = true;
            this.rbPartial.Location = new System.Drawing.Point(16, 19);
            this.rbPartial.Name = "rbPartial";
            this.rbPartial.Size = new System.Drawing.Size(54, 17);
            this.rbPartial.TabIndex = 0;
            this.rbPartial.TabStop = true;
            this.rbPartial.Text = "Partial";
            this.rbPartial.UseVisualStyleBackColor = true;
            this.rbPartial.CheckedChanged += new System.EventHandler(this.PartialRadioButton_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbVisualBasic);
            this.groupBox1.Controls.Add(this.rbCSharp);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(193, 72);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Language";
            // 
            // rbVisualBasic
            // 
            this.rbVisualBasic.AutoSize = true;
            this.rbVisualBasic.Location = new System.Drawing.Point(16, 43);
            this.rbVisualBasic.Name = "rbVisualBasic";
            this.rbVisualBasic.Size = new System.Drawing.Size(82, 17);
            this.rbVisualBasic.TabIndex = 1;
            this.rbVisualBasic.Text = "Visual Basic";
            this.rbVisualBasic.UseVisualStyleBackColor = true;
            this.rbVisualBasic.CheckedChanged += new System.EventHandler(this.CSharpRadioButton_CheckedChanged);
            // 
            // rbCSharp
            // 
            this.rbCSharp.AutoSize = true;
            this.rbCSharp.Checked = true;
            this.rbCSharp.Location = new System.Drawing.Point(16, 19);
            this.rbCSharp.Name = "rbCSharp";
            this.rbCSharp.Size = new System.Drawing.Size(39, 17);
            this.rbCSharp.TabIndex = 0;
            this.rbCSharp.TabStop = true;
            this.rbCSharp.Text = "C#";
            this.rbCSharp.UseVisualStyleBackColor = true;
            this.rbCSharp.CheckedChanged += new System.EventHandler(this.CSharpRadioButton_CheckedChanged);
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(5, 5);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(876, 39);
            this.pnDescription.TabIndex = 8;
            // 
            // pnDescriptions
            // 
            this.pnDescriptions.Controls.Add(this.panel2);
            this.pnDescriptions.Controls.Add(this.panel3);
            this.pnDescriptions.Controls.Add(this.panel1);
            this.pnDescriptions.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnDescriptions.Location = new System.Drawing.Point(0, 121);
            this.pnDescriptions.Name = "pnDescriptions";
            this.pnDescriptions.Size = new System.Drawing.Size(200, 497);
            this.pnDescriptions.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 355);
            this.panel2.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(0, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 338);
            this.label3.TabIndex = 9;
            this.label3.Text = "Code in the editor is a full source code, and associated parser allows class-less" +
    " scripts, i.e global code without class or method declaration.";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.readonlyCodeLabel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 445);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 52);
            this.panel3.TabIndex = 6;
            // 
            // readonlyCodeLabel2
            // 
            this.readonlyCodeLabel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.readonlyCodeLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readonlyCodeLabel2.Location = new System.Drawing.Point(0, 0);
            this.readonlyCodeLabel2.Name = "readonlyCodeLabel2";
            this.readonlyCodeLabel2.Size = new System.Drawing.Size(200, 52);
            this.readonlyCodeLabel2.TabIndex = 7;
            this.readonlyCodeLabel2.Text = "Readonly code:";
            this.readonlyCodeLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.readonlyCodeLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 90);
            this.panel1.TabIndex = 4;
            // 
            // readonlyCodeLabel1
            // 
            this.readonlyCodeLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readonlyCodeLabel1.Location = new System.Drawing.Point(0, 0);
            this.readonlyCodeLabel1.Name = "readonlyCodeLabel1";
            this.readonlyCodeLabel1.Size = new System.Drawing.Size(196, 86);
            this.readonlyCodeLabel1.TabIndex = 7;
            this.readonlyCodeLabel1.Text = "Readonly code:";
            this.readonlyCodeLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnEditors
            // 
            this.pnEditors.Controls.Add(this.syntaxEdit1);
            this.pnEditors.Controls.Add(this.seBottom);
            this.pnEditors.Controls.Add(this.seTop);
            this.pnEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEditors.Location = new System.Drawing.Point(200, 121);
            this.pnEditors.Name = "pnEditors";
            this.pnEditors.Size = new System.Drawing.Size(686, 497);
            this.pnEditors.TabIndex = 7;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 90);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.AllowOutlining = true;
            this.syntaxEdit1.Size = new System.Drawing.Size(686, 355);
            this.syntaxEdit1.Source = this.csSnippetSource;
            this.syntaxEdit1.SyntaxPaint.ReadonlyBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.syntaxEdit1.TabIndex = 11;
            // 
            // seBottom
            // 
            this.seBottom.BackColor = System.Drawing.SystemColors.Window;
            this.seBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.seBottom.Location = new System.Drawing.Point(0, 445);
            this.seBottom.Name = "seBottom";
            this.seBottom.ReadOnly = true;
            this.seBottom.Size = new System.Drawing.Size(686, 52);
            this.seBottom.SyntaxPaint.ReadonlyBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.seBottom.TabIndex = 10;
            this.seBottom.Text = "";
            // 
            // seTop
            // 
            this.seTop.BackColor = System.Drawing.SystemColors.Window;
            this.seTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.seTop.Location = new System.Drawing.Point(0, 0);
            this.seTop.Name = "seTop";
            this.seTop.ReadOnly = true;
            this.seTop.Size = new System.Drawing.Size(686, 90);
            this.seTop.SyntaxPaint.ReadonlyBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.seTop.TabIndex = 6;
            this.seTop.Text = "";
            // 
            // rbHidden
            // 
            this.rbHidden.AutoSize = true;
            this.rbHidden.Location = new System.Drawing.Point(16, 43);
            this.rbHidden.Name = "rbHidden";
            this.rbHidden.Size = new System.Drawing.Size(59, 17);
            this.rbHidden.TabIndex = 2;
            this.rbHidden.Text = "Hidden";
            this.rbHidden.UseVisualStyleBackColor = true;
            this.rbHidden.CheckedChanged += new System.EventHandler(this.PartialRadioButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 618);
            this.Controls.Add(this.pnEditors);
            this.Controls.Add(this.pnDescriptions);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snippets Parsing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.pnDescriptions.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnEditors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.TextSource.TextSource vbSnippetSource;
        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Panel pnDescriptions;
        private System.Windows.Forms.Panel pnEditors;
        private Alternet.Editor.SyntaxEdit seTop;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private Alternet.Editor.SyntaxEdit seBottom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label readonlyCodeLabel2;
        private System.Windows.Forms.Label readonlyCodeLabel1;
        private Alternet.Editor.TextSource.TextSource vbSource;
        private Alternet.Editor.TextSource.TextSource csSnippetSource;
        private Alternet.Editor.TextSource.TextSource csSource;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbReadonly;
        private System.Windows.Forms.RadioButton rbPartial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVisualBasic;
        private System.Windows.Forms.RadioButton rbCSharp;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbClass;
        private System.Windows.Forms.RadioButton rbMethod;
        private System.Windows.Forms.RadioButton rbClassLess;
        private System.Windows.Forms.RadioButton rbHidden;
    }
}