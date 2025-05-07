namespace Gutter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnSettings = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudLineNumbersStart = new System.Windows.Forms.NumericUpDown();
            this.nudLineNumbersRightIndent = new System.Windows.Forms.NumericUpDown();
            this.laLineNumbersLeftIndent = new System.Windows.Forms.Label();
            this.nudLineNumbersLeftIndent = new System.Windows.Forms.NumericUpDown();
            this.cbLineNumbersAlign = new System.Windows.Forms.ComboBox();
            this.laLineNumbersRightIndent = new System.Windows.Forms.Label();
            this.laLineNumbersAlign = new System.Windows.Forms.Label();
            this.laLineNumbersStart = new System.Windows.Forms.Label();
            this.cbLineNumbersBackColor = new Alternet.Editor.Common.ColorBox();
            this.cbLineNumbersForeColor = new Alternet.Editor.Common.ColorBox();
            this.laLineNumbersBackColor = new System.Windows.Forms.Label();
            this.laLineNumbersForeColor = new System.Windows.Forms.Label();
            this.chbLineNumbers = new System.Windows.Forms.CheckBox();
            this.chbLinesOnGutter = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbGradientEndColor = new Alternet.Editor.Common.ColorBox();
            this.cbGradientBeginColor = new Alternet.Editor.Common.ColorBox();
            this.cbPenColor = new Alternet.Editor.Common.ColorBox();
            this.cbGutterColor = new Alternet.Editor.Common.ColorBox();
            this.laGradientEndColor = new System.Windows.Forms.Label();
            this.laGradientBeginColor = new System.Windows.Forms.Label();
            this.laPenColor = new System.Windows.Forms.Label();
            this.chbGradientGutter = new System.Windows.Forms.CheckBox();
            this.nudGutterWidth = new System.Windows.Forms.NumericUpDown();
            this.chbShowGutter = new System.Windows.Forms.CheckBox();
            this.laGutterWidth = new System.Windows.Forms.Label();
            this.laGutterColor = new System.Windows.Forms.Label();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit();
            this.pnSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersRightIndent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersLeftIndent)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGutterWidth)).BeginInit();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.groupBox2);
            this.pnSettings.Controls.Add(this.panel1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 172);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudLineNumbersStart);
            this.groupBox2.Controls.Add(this.nudLineNumbersRightIndent);
            this.groupBox2.Controls.Add(this.laLineNumbersLeftIndent);
            this.groupBox2.Controls.Add(this.nudLineNumbersLeftIndent);
            this.groupBox2.Controls.Add(this.cbLineNumbersAlign);
            this.groupBox2.Controls.Add(this.laLineNumbersRightIndent);
            this.groupBox2.Controls.Add(this.laLineNumbersAlign);
            this.groupBox2.Controls.Add(this.laLineNumbersStart);
            this.groupBox2.Controls.Add(this.cbLineNumbersBackColor);
            this.groupBox2.Controls.Add(this.cbLineNumbersForeColor);
            this.groupBox2.Controls.Add(this.laLineNumbersBackColor);
            this.groupBox2.Controls.Add(this.laLineNumbersForeColor);
            this.groupBox2.Controls.Add(this.chbLineNumbers);
            this.groupBox2.Controls.Add(this.chbLinesOnGutter);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(336, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 123);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            // 
            // nudLineNumbersStart
            // 
            this.nudLineNumbersStart.Location = new System.Drawing.Point(254, 72);
            this.nudLineNumbersStart.Name = "nudLineNumbersStart";
            this.nudLineNumbersStart.Size = new System.Drawing.Size(64, 20);
            this.nudLineNumbersStart.TabIndex = 23;
            this.nudLineNumbersStart.ValueChanged += new System.EventHandler(this.LineNumbersStartNumeric_ValueChanged);
            // 
            // nudLineNumbersRightIndent
            // 
            this.nudLineNumbersRightIndent.Location = new System.Drawing.Point(254, 46);
            this.nudLineNumbersRightIndent.Name = "nudLineNumbersRightIndent";
            this.nudLineNumbersRightIndent.Size = new System.Drawing.Size(64, 20);
            this.nudLineNumbersRightIndent.TabIndex = 22;
            this.nudLineNumbersRightIndent.ValueChanged += new System.EventHandler(this.LineNumbersRightIndentNumeric_ValueChanged);
            // 
            // laLineNumbersLeftIndent
            // 
            this.laLineNumbersLeftIndent.AutoSize = true;
            this.laLineNumbersLeftIndent.Location = new System.Drawing.Point(182, 23);
            this.laLineNumbersLeftIndent.Name = "laLineNumbersLeftIndent";
            this.laLineNumbersLeftIndent.Size = new System.Drawing.Size(61, 13);
            this.laLineNumbersLeftIndent.TabIndex = 27;
            this.laLineNumbersLeftIndent.Text = "Left Indent:";
            // 
            // nudLineNumbersLeftIndent
            // 
            this.nudLineNumbersLeftIndent.Location = new System.Drawing.Point(254, 20);
            this.nudLineNumbersLeftIndent.Name = "nudLineNumbersLeftIndent";
            this.nudLineNumbersLeftIndent.Size = new System.Drawing.Size(64, 20);
            this.nudLineNumbersLeftIndent.TabIndex = 21;
            this.nudLineNumbersLeftIndent.ValueChanged += new System.EventHandler(this.LineNumbersLeftIndentNumeric_ValueChanged);
            // 
            // cbLineNumbersAlign
            // 
            this.cbLineNumbersAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLineNumbersAlign.ItemHeight = 13;
            this.cbLineNumbersAlign.Location = new System.Drawing.Point(254, 98);
            this.cbLineNumbersAlign.Name = "cbLineNumbersAlign";
            this.cbLineNumbersAlign.Size = new System.Drawing.Size(64, 21);
            this.cbLineNumbersAlign.TabIndex = 24;
            this.cbLineNumbersAlign.SelectedIndexChanged += new System.EventHandler(this.LineNumbersAlignComboBox_SelectedIndexChanged);
            this.cbLineNumbersAlign.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineNumbersAlignComboBox_MouseMove);
            // 
            // laLineNumbersRightIndent
            // 
            this.laLineNumbersRightIndent.AutoSize = true;
            this.laLineNumbersRightIndent.Location = new System.Drawing.Point(182, 49);
            this.laLineNumbersRightIndent.Name = "laLineNumbersRightIndent";
            this.laLineNumbersRightIndent.Size = new System.Drawing.Size(68, 13);
            this.laLineNumbersRightIndent.TabIndex = 28;
            this.laLineNumbersRightIndent.Text = "Right Indent:";
            // 
            // laLineNumbersAlign
            // 
            this.laLineNumbersAlign.AutoSize = true;
            this.laLineNumbersAlign.Location = new System.Drawing.Point(182, 100);
            this.laLineNumbersAlign.Name = "laLineNumbersAlign";
            this.laLineNumbersAlign.Size = new System.Drawing.Size(33, 13);
            this.laLineNumbersAlign.TabIndex = 26;
            this.laLineNumbersAlign.Text = "Align:";
            // 
            // laLineNumbersStart
            // 
            this.laLineNumbersStart.AutoSize = true;
            this.laLineNumbersStart.Location = new System.Drawing.Point(182, 74);
            this.laLineNumbersStart.Name = "laLineNumbersStart";
            this.laLineNumbersStart.Size = new System.Drawing.Size(32, 13);
            this.laLineNumbersStart.TabIndex = 25;
            this.laLineNumbersStart.Text = "Start:";
            // 
            // cbLineNumbersBackColor
            // 
            this.cbLineNumbersBackColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbLineNumbersBackColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLineNumbersBackColor.FormattingEnabled = true;
            this.cbLineNumbersBackColor.Location = new System.Drawing.Point(71, 74);
            this.cbLineNumbersBackColor.Name = "cbLineNumbersBackColor";
            this.cbLineNumbersBackColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbLineNumbersBackColor.Size = new System.Drawing.Size(87, 21);
            this.cbLineNumbersBackColor.TabIndex = 14;
            this.cbLineNumbersBackColor.SelectedIndexChanged += new System.EventHandler(this.LineNumbersBackColorComboBox_SelectedIndexChanged);
            this.cbLineNumbersBackColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineNumbersBackColorComboBox_MouseMove);
            // 
            // cbLineNumbersForeColor
            // 
            this.cbLineNumbersForeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbLineNumbersForeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLineNumbersForeColor.FormattingEnabled = true;
            this.cbLineNumbersForeColor.Location = new System.Drawing.Point(71, 49);
            this.cbLineNumbersForeColor.Name = "cbLineNumbersForeColor";
            this.cbLineNumbersForeColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbLineNumbersForeColor.Size = new System.Drawing.Size(87, 21);
            this.cbLineNumbersForeColor.TabIndex = 13;
            this.cbLineNumbersForeColor.SelectedIndexChanged += new System.EventHandler(this.LineNumbersForeColorComboBox_SelectedIndexChanged);
            this.cbLineNumbersForeColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineNumbersForeColorComboBox_MouseMove);
            // 
            // laLineNumbersBackColor
            // 
            this.laLineNumbersBackColor.AutoSize = true;
            this.laLineNumbersBackColor.Location = new System.Drawing.Point(11, 78);
            this.laLineNumbersBackColor.Name = "laLineNumbersBackColor";
            this.laLineNumbersBackColor.Size = new System.Drawing.Size(62, 13);
            this.laLineNumbersBackColor.TabIndex = 20;
            this.laLineNumbersBackColor.Text = "Back Color:";
            // 
            // laLineNumbersForeColor
            // 
            this.laLineNumbersForeColor.AutoSize = true;
            this.laLineNumbersForeColor.Location = new System.Drawing.Point(11, 53);
            this.laLineNumbersForeColor.Name = "laLineNumbersForeColor";
            this.laLineNumbersForeColor.Size = new System.Drawing.Size(58, 13);
            this.laLineNumbersForeColor.TabIndex = 19;
            this.laLineNumbersForeColor.Text = "Fore Color:";
            // 
            // chbLineNumbers
            // 
            this.chbLineNumbers.AutoSize = true;
            this.chbLineNumbers.Location = new System.Drawing.Point(6, 0);
            this.chbLineNumbers.Name = "chbLineNumbers";
            this.chbLineNumbers.Size = new System.Drawing.Size(128, 17);
            this.chbLineNumbers.TabIndex = 11;
            this.chbLineNumbers.Text = "Display Line Numbers";
            this.chbLineNumbers.CheckedChanged += new System.EventHandler(this.LineNumbersCheckBox_CheckedChanged);
            this.chbLineNumbers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineNumbersCheckBox_MouseMove);
            // 
            // chbLinesOnGutter
            // 
            this.chbLinesOnGutter.AutoSize = true;
            this.chbLinesOnGutter.Location = new System.Drawing.Point(14, 22);
            this.chbLinesOnGutter.Name = "chbLinesOnGutter";
            this.chbLinesOnGutter.Size = new System.Drawing.Size(116, 17);
            this.chbLinesOnGutter.TabIndex = 12;
            this.chbLinesOnGutter.Text = "Lines on the Gutter";
            this.chbLinesOnGutter.CheckedChanged += new System.EventHandler(this.LinesOnGutterCheckBox_CheckedChanged);
            this.chbLinesOnGutter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LinesOnGutterCheckBox_MouseMove);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(5, 44);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.panel1.Size = new System.Drawing.Size(331, 123);
            this.panel1.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbGradientEndColor);
            this.groupBox1.Controls.Add(this.cbGradientBeginColor);
            this.groupBox1.Controls.Add(this.cbPenColor);
            this.groupBox1.Controls.Add(this.cbGutterColor);
            this.groupBox1.Controls.Add(this.laGradientEndColor);
            this.groupBox1.Controls.Add(this.laGradientBeginColor);
            this.groupBox1.Controls.Add(this.laPenColor);
            this.groupBox1.Controls.Add(this.chbGradientGutter);
            this.groupBox1.Controls.Add(this.nudGutterWidth);
            this.groupBox1.Controls.Add(this.chbShowGutter);
            this.groupBox1.Controls.Add(this.laGutterWidth);
            this.groupBox1.Controls.Add(this.laGutterColor);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 123);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            // 
            // cbGradientEndColor
            // 
            this.cbGradientEndColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGradientEndColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGradientEndColor.FormattingEnabled = true;
            this.cbGradientEndColor.Location = new System.Drawing.Point(233, 76);
            this.cbGradientEndColor.Name = "cbGradientEndColor";
            this.cbGradientEndColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbGradientEndColor.Size = new System.Drawing.Size(87, 21);
            this.cbGradientEndColor.TabIndex = 6;
            this.cbGradientEndColor.SelectedIndexChanged += new System.EventHandler(this.GradientEndColorComboBox_SelectedIndexChanged);
            this.cbGradientEndColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GradientEndColorComboBox_MouseMove);
            // 
            // cbGradientBeginColor
            // 
            this.cbGradientBeginColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGradientBeginColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGradientBeginColor.FormattingEnabled = true;
            this.cbGradientBeginColor.Location = new System.Drawing.Point(233, 49);
            this.cbGradientBeginColor.Name = "cbGradientBeginColor";
            this.cbGradientBeginColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbGradientBeginColor.Size = new System.Drawing.Size(87, 21);
            this.cbGradientBeginColor.TabIndex = 5;
            this.cbGradientBeginColor.SelectedIndexChanged += new System.EventHandler(this.GradieneginColorComboBoxTextBox_SelectedIndexChanged);
            this.cbGradientBeginColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GradieneginColorComboBoxTextBox_MouseMove);
            // 
            // cbPenColor
            // 
            this.cbPenColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbPenColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPenColor.FormattingEnabled = true;
            this.cbPenColor.Location = new System.Drawing.Point(79, 49);
            this.cbPenColor.Name = "cbPenColor";
            this.cbPenColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbPenColor.Size = new System.Drawing.Size(87, 21);
            this.cbPenColor.TabIndex = 4;
            this.cbPenColor.SelectedIndexChanged += new System.EventHandler(this.PenColorComboBox_SelectedIndexChanged);
            this.cbPenColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PenColorComboBox_MouseMove);
            // 
            // cbGutterColor
            // 
            this.cbGutterColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGutterColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGutterColor.FormattingEnabled = true;
            this.cbGutterColor.Location = new System.Drawing.Point(79, 21);
            this.cbGutterColor.Name = "cbGutterColor";
            this.cbGutterColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbGutterColor.Size = new System.Drawing.Size(87, 21);
            this.cbGutterColor.TabIndex = 3;
            this.cbGutterColor.SelectedIndexChanged += new System.EventHandler(this.GutterColorComboBox_SelectedIndexChanged);
            this.cbGutterColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GutterColorComboBox_MouseMove);
            // 
            // laGradientEndColor
            // 
            this.laGradientEndColor.AutoSize = true;
            this.laGradientEndColor.Location = new System.Drawing.Point(171, 78);
            this.laGradientEndColor.Name = "laGradientEndColor";
            this.laGradientEndColor.Size = new System.Drawing.Size(56, 13);
            this.laGradientEndColor.TabIndex = 24;
            this.laGradientEndColor.Text = "End Color:";
            // 
            // laGradientBeginColor
            // 
            this.laGradientBeginColor.AutoSize = true;
            this.laGradientBeginColor.Location = new System.Drawing.Point(172, 52);
            this.laGradientBeginColor.Name = "laGradientBeginColor";
            this.laGradientBeginColor.Size = new System.Drawing.Size(59, 13);
            this.laGradientBeginColor.TabIndex = 23;
            this.laGradientBeginColor.Text = "Start Color:";
            // 
            // laPenColor
            // 
            this.laPenColor.AutoSize = true;
            this.laPenColor.Location = new System.Drawing.Point(7, 52);
            this.laPenColor.Name = "laPenColor";
            this.laPenColor.Size = new System.Drawing.Size(57, 13);
            this.laPenColor.TabIndex = 22;
            this.laPenColor.Text = "Line Color:";
            // 
            // chbGradientGutter
            // 
            this.chbGradientGutter.AutoSize = true;
            this.chbGradientGutter.Location = new System.Drawing.Point(175, 22);
            this.chbGradientGutter.Name = "chbGradientGutter";
            this.chbGradientGutter.Size = new System.Drawing.Size(88, 17);
            this.chbGradientGutter.TabIndex = 1;
            this.chbGradientGutter.Text = "Use Gradient";
            this.chbGradientGutter.CheckedChanged += new System.EventHandler(this.GradientGutterCheckBox_CheckedChanged);
            this.chbGradientGutter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GradientGutterCheckBox_MouseMove);
            // 
            // nudGutterWidth
            // 
            this.nudGutterWidth.Location = new System.Drawing.Point(79, 75);
            this.nudGutterWidth.Name = "nudGutterWidth";
            this.nudGutterWidth.Size = new System.Drawing.Size(87, 20);
            this.nudGutterWidth.TabIndex = 2;
            this.nudGutterWidth.ValueChanged += new System.EventHandler(this.GutterWidthNumeric_ValueChanged);
            // 
            // chbShowGutter
            // 
            this.chbShowGutter.AutoSize = true;
            this.chbShowGutter.Location = new System.Drawing.Point(7, 0);
            this.chbShowGutter.Name = "chbShowGutter";
            this.chbShowGutter.Size = new System.Drawing.Size(92, 17);
            this.chbShowGutter.TabIndex = 0;
            this.chbShowGutter.Text = "Display Gutter";
            this.chbShowGutter.CheckedChanged += new System.EventHandler(this.ShowGutterCheckBox_CheckedChanged);
            this.chbShowGutter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowGutterCheckBox_MouseMove);
            // 
            // laGutterWidth
            // 
            this.laGutterWidth.AutoSize = true;
            this.laGutterWidth.Location = new System.Drawing.Point(7, 78);
            this.laGutterWidth.Name = "laGutterWidth";
            this.laGutterWidth.Size = new System.Drawing.Size(70, 13);
            this.laGutterWidth.TabIndex = 12;
            this.laGutterWidth.Text = "Gutter Width:";
            // 
            // laGutterColor
            // 
            this.laGutterColor.AutoSize = true;
            this.laGutterColor.Location = new System.Drawing.Point(7, 26);
            this.laGutterColor.Name = "laGutterColor";
            this.laGutterColor.Size = new System.Drawing.Size(66, 13);
            this.laGutterColor.TabIndex = 17;
            this.laGutterColor.Text = "Gutter Color:";
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
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(657, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "The gutter area can be used to display additional graphical information related t" +
    "o the text content.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 172);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 5;
            this.syntaxEdit1.Text = "";
            this.syntaxEdit1.PaintBackground += new System.Windows.Forms.PaintEventHandler(this.SyntaxEdit1_PaintBackground);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 431);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gutter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersRightIndent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersLeftIndent)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGutterWidth)).EndInit();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label laGradientEndColor;
        private System.Windows.Forms.Label laGradientBeginColor;
        private System.Windows.Forms.Label laPenColor;
        private System.Windows.Forms.CheckBox chbGradientGutter;
        private System.Windows.Forms.NumericUpDown nudGutterWidth;
        private System.Windows.Forms.CheckBox chbShowGutter;
        private System.Windows.Forms.Label laGutterWidth;
        private System.Windows.Forms.Label laGutterColor;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label laLineNumbersBackColor;
        private System.Windows.Forms.Label laLineNumbersForeColor;
        private System.Windows.Forms.CheckBox chbLineNumbers;
        private System.Windows.Forms.CheckBox chbLinesOnGutter;
        private Alternet.Editor.Common.ColorBox cbGutterColor;
        private Alternet.Editor.Common.ColorBox cbGradientEndColor;
        private Alternet.Editor.Common.ColorBox cbGradientBeginColor;
        private Alternet.Editor.Common.ColorBox cbPenColor;
        private Alternet.Editor.Common.ColorBox cbLineNumbersBackColor;
        private Alternet.Editor.Common.ColorBox cbLineNumbersForeColor;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.NumericUpDown nudLineNumbersStart;
        private System.Windows.Forms.NumericUpDown nudLineNumbersRightIndent;
        private System.Windows.Forms.Label laLineNumbersLeftIndent;
        private System.Windows.Forms.NumericUpDown nudLineNumbersLeftIndent;
        private System.Windows.Forms.ComboBox cbLineNumbersAlign;
        private System.Windows.Forms.Label laLineNumbersRightIndent;
        private System.Windows.Forms.Label laLineNumbersAlign;
        private System.Windows.Forms.Label laLineNumbersStart;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}