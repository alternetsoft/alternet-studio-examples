namespace MultipleView
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
            Alternet.Editor.ScrollingButton scrollingButton1 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton2 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton3 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton4 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton5 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton6 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton7 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton8 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton9 = new Alternet.Editor.ScrollingButton();
            Alternet.Editor.ScrollingButton scrollingButton10 = new Alternet.Editor.ScrollingButton();

            this.pnSettings = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbVerticalAnnotations = new System.Windows.Forms.CheckBox();
            this.chbSystemScrollBars = new System.Windows.Forms.CheckBox();
            this.chbSmoothScroll = new System.Windows.Forms.CheckBox();
            this.chbShowScrollHint = new System.Windows.Forms.CheckBox();
            this.chbVertButton = new System.Windows.Forms.CheckBox();
            this.chbHorzButton = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbAllowSplitHorz = new System.Windows.Forms.CheckBox();
            this.chbAllowSplitVert = new System.Windows.Forms.CheckBox();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.csharpSource = new Alternet.Editor.TextSource.TextSource();
            this.panel2 = new System.Windows.Forms.Panel();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.syntaxEdit2 = new Alternet.Editor.SyntaxEdit();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.pnSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.pnSettings.Size = new System.Drawing.Size(667, 124);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbVerticalAnnotations);
            this.groupBox2.Controls.Add(this.chbSystemScrollBars);
            this.groupBox2.Controls.Add(this.chbSmoothScroll);
            this.groupBox2.Controls.Add(this.chbShowScrollHint);
            this.groupBox2.Controls.Add(this.chbVertButton);
            this.groupBox2.Controls.Add(this.chbHorzButton);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(169, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(493, 75);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scrolling";
            // 
            // chbVerticalAnnotations
            // 
            this.chbVerticalAnnotations.AutoSize = true;
            this.chbVerticalAnnotations.Location = new System.Drawing.Point(343, 23);
            this.chbVerticalAnnotations.Name = "chbVerticalAnnotations";
            this.chbVerticalAnnotations.Size = new System.Drawing.Size(120, 17);
            this.chbVerticalAnnotations.TabIndex = 15;
            this.chbVerticalAnnotations.Text = "Scrollbar Annotations";
            this.chbVerticalAnnotations.Checked = true;
            this.chbVerticalAnnotations.CheckedChanged += new System.EventHandler(this.VerticalAnnotationsCheckBox_CheckedChanged);
            // 
            // chbSystemScrollBars
            // 
            this.chbSystemScrollBars.AutoSize = true;
            this.chbSystemScrollBars.Location = new System.Drawing.Point(343, 46);
            this.chbSystemScrollBars.Name = "chbSystemScrollBars";
            this.chbSystemScrollBars.Size = new System.Drawing.Size(89, 17);
            this.chbSystemScrollBars.TabIndex = 16;
            this.chbSystemScrollBars.Text = "System Scroll";
            this.chbSystemScrollBars.Visible = true;
            this.chbSystemScrollBars.CheckedChanged += new System.EventHandler(this.SystemScrolarsLisoxCheckBoxTextBox_CheckedChanged);
            // 
            // chbSmoothScroll
            // 
            this.chbSmoothScroll.AutoSize = true;
            this.chbSmoothScroll.Location = new System.Drawing.Point(223, 47);
            this.chbSmoothScroll.Name = "chbSmoothScroll";
            this.chbSmoothScroll.Size = new System.Drawing.Size(91, 17);
            this.chbSmoothScroll.TabIndex = 14;
            this.chbSmoothScroll.Text = "Smooth Scroll";
            this.chbSmoothScroll.CheckedChanged += new System.EventHandler(this.SmoothScrollCheckBox_CheckedChanged);
            this.chbSmoothScroll.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SmoothScrollCheckBox_MouseMove);
            // 
            // chbShowScrollHint
            // 
            this.chbShowScrollHint.AutoSize = true;
            this.chbShowScrollHint.Location = new System.Drawing.Point(223, 23);
            this.chbShowScrollHint.Name = "chbShowScrollHint";
            this.chbShowScrollHint.Size = new System.Drawing.Size(111, 17);
            this.chbShowScrollHint.TabIndex = 13;
            this.chbShowScrollHint.Text = "Display Scroll Hint";
            this.chbShowScrollHint.CheckedChanged += new System.EventHandler(this.ShowScrollHintCheckBox_CheckedChanged);
            this.chbShowScrollHint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowScrollHintCheckBox_MouseMove);
            // 
            // chbVertButton
            // 
            this.chbVertButton.AutoSize = true;
            this.chbVertButton.Location = new System.Drawing.Point(8, 47);
            this.chbVertButton.Name = "chbVertButton";
            this.chbVertButton.Size = new System.Drawing.Size(197, 17);
            this.chbVertButton.TabIndex = 12;
            this.chbVertButton.Text = "Vertical Scroll Bar Additional Buttons";
            this.chbVertButton.CheckedChanged += new System.EventHandler(this.VeruttonCheckBoxTextBox_CheckedChanged);
            this.chbVertButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VeruttonCheckBoxTextBox_MouseMove);
            // 
            // chbHorzButton
            // 
            this.chbHorzButton.AutoSize = true;
            this.chbHorzButton.Location = new System.Drawing.Point(8, 23);
            this.chbHorzButton.Name = "chbHorzButton";
            this.chbHorzButton.Size = new System.Drawing.Size(209, 17);
            this.chbHorzButton.TabIndex = 11;
            this.chbHorzButton.Text = "Horizontal Scroll Bar Additional Buttons";
            this.chbHorzButton.CheckedChanged += new System.EventHandler(this.HorzButtonCheckBox_CheckedChanged);
            this.chbHorzButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HorzButtonCheckBox_MouseMove);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(5, 44);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.panel1.Size = new System.Drawing.Size(164, 75);
            this.panel1.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbAllowSplitHorz);
            this.groupBox1.Controls.Add(this.chbAllowSplitVert);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(159, 75);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Split";
            // 
            // chbAllowSplitHorz
            // 
            this.chbAllowSplitHorz.AutoSize = true;
            this.chbAllowSplitHorz.Location = new System.Drawing.Point(14, 23);
            this.chbAllowSplitHorz.Name = "chbAllowSplitHorz";
            this.chbAllowSplitHorz.Size = new System.Drawing.Size(124, 17);
            this.chbAllowSplitHorz.TabIndex = 9;
            this.chbAllowSplitHorz.Text = "Allow Horizontal Split";
            this.chbAllowSplitHorz.CheckedChanged += new System.EventHandler(this.AllowSplitHorzCheckBox_CheckedChanged);
            this.chbAllowSplitHorz.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AllowSplitHorzCheckBox_MouseMove);
            // 
            // chbAllowSplitVert
            // 
            this.chbAllowSplitVert.AutoSize = true;
            this.chbAllowSplitVert.Location = new System.Drawing.Point(14, 47);
            this.chbAllowSplitVert.Name = "chbAllowSplitVert";
            this.chbAllowSplitVert.Size = new System.Drawing.Size(112, 17);
            this.chbAllowSplitVert.TabIndex = 10;
            this.chbAllowSplitVert.Text = "Allow Vertical Split";
            this.chbAllowSplitVert.CheckedChanged += new System.EventHandler(this.AllowSplitVertCheckBox_CheckedChanged);
            this.chbAllowSplitVert.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AllowSplitVertCheckBox_MouseMove);
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
            this.laDescription.Text = "This demo shows ability to visually split the edit control to provide different v" +
    "iews of the same text as well as various scrolling options.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // csharpSource
            // 
            this.csharpSource.OptimizedForMemory = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.syntaxEdit1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 124);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(667, 180);
            this.panel2.TabIndex = 7;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 0);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            scrollingButton1.Description = "Page Normal Mode";
            scrollingButton1.ImageIndex = 0;
            scrollingButton1.Images = this.buttonsImageList;
            scrollingButton1.Name = "Normal";
            scrollingButton2.Description = "Page Layout Mode";
            scrollingButton2.ImageIndex = 1;
            scrollingButton2.Images = this.buttonsImageList;
            scrollingButton2.Name = "PageLayout";
            scrollingButton3.Description = "Page Breaks Mode";
            scrollingButton3.ImageIndex = 2;
            scrollingButton3.Images = this.buttonsImageList;
            scrollingButton3.Name = "PageBreaks";
            this.syntaxEdit1.Scrolling.HorzButtons.Add(scrollingButton1);
            this.syntaxEdit1.Scrolling.HorzButtons.Add(scrollingButton2);
            this.syntaxEdit1.Scrolling.HorzButtons.Add(scrollingButton3);
            this.syntaxEdit1.Scrolling.Options = ((Alternet.Editor.ScrollingOptions)((Alternet.Editor.ScrollingOptions.SmoothScroll | Alternet.Editor.ScrollingOptions.UseScrollDelta | Alternet.Editor.ScrollingOptions.VerticalScrollBarAnnotations)));
            this.syntaxEdit1.Scrolling.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            scrollingButton4.Description = "Page Down";
            scrollingButton4.ImageIndex = 3;
            scrollingButton4.Images = this.buttonsImageList;
            scrollingButton4.Name = "PageDown";
            scrollingButton5.Description = "Page Up";
            scrollingButton5.ImageIndex = 4;
            scrollingButton5.Images = this.buttonsImageList;
            scrollingButton5.Name = "PageUp";
            this.syntaxEdit1.Scrolling.VertButtons.Add(scrollingButton4);
            this.syntaxEdit1.Scrolling.VertButtons.Add(scrollingButton5);
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 180);
            this.syntaxEdit1.Source = this.csharpSource;
            this.syntaxEdit1.TabIndex = 8;
            this.syntaxEdit1.ScrollButtonClick += new System.EventHandler(this.SyntaxEdit1_ScrollButtonClick);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 304);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(667, 3);
            this.splitter2.TabIndex = 8;
            this.splitter2.TabStop = false;
            // 
            // syntaxEdit2
            // 
            this.syntaxEdit2.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit2.Location = new System.Drawing.Point(0, 307);
            this.syntaxEdit2.Name = "syntaxEdit2";
            this.syntaxEdit2.Outlining.ImageSize = 8;
            scrollingButton6.Description = "Page Normal Mode";
            scrollingButton6.ImageIndex = 0;
            scrollingButton6.Images = this.buttonsImageList;
            scrollingButton6.Name = "Normal";
            scrollingButton7.Description = "Page Layout Mode";
            scrollingButton7.ImageIndex = 1;
            scrollingButton7.Images = this.buttonsImageList;
            scrollingButton7.Name = "PageLayout";
            scrollingButton8.Description = "Page Breaks Mode";
            scrollingButton8.ImageIndex = 2;
            scrollingButton8.Images = this.buttonsImageList;
            scrollingButton8.Name = "PageBreaks";
            this.syntaxEdit2.Scrolling.HorzButtons.Add(scrollingButton6);
            this.syntaxEdit2.Scrolling.HorzButtons.Add(scrollingButton7);
            this.syntaxEdit2.Scrolling.HorzButtons.Add(scrollingButton8);
            this.syntaxEdit2.Scrolling.Options = ((Alternet.Editor.ScrollingOptions)((Alternet.Editor.ScrollingOptions.SmoothScroll | Alternet.Editor.ScrollingOptions.UseScrollDelta | Alternet.Editor.ScrollingOptions.VerticalScrollBarAnnotations)));
            this.syntaxEdit2.Scrolling.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            scrollingButton9.Description = "Page Down";
            scrollingButton9.ImageIndex = 3;
            scrollingButton9.Images = this.buttonsImageList;
            scrollingButton9.Name = "PageDown";
            scrollingButton10.Description = "Page Up";
            scrollingButton10.ImageIndex = 4;
            scrollingButton10.Images = this.buttonsImageList;
            scrollingButton10.Name = "PageUp";
            this.syntaxEdit2.Scrolling.VertButtons.Add(scrollingButton9);
            this.syntaxEdit2.Scrolling.VertButtons.Add(scrollingButton10);
            this.syntaxEdit2.SearchGlobal = false;
            this.syntaxEdit2.Size = new System.Drawing.Size(667, 76);
            this.syntaxEdit2.Source = this.csharpSource;
            this.syntaxEdit2.TabIndex = 10;
            this.syntaxEdit2.ScrollButtonClick += new System.EventHandler(this.SyntaxEdit2_ScrollButtonClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 383);
            this.Controls.Add(this.syntaxEdit2);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multiple Views & Split View";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbSystemScrollBars;
        private System.Windows.Forms.CheckBox chbSmoothScroll;
        private System.Windows.Forms.CheckBox chbShowScrollHint;
        private System.Windows.Forms.CheckBox chbVertButton;
        private System.Windows.Forms.CheckBox chbHorzButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbAllowSplitHorz;
        private System.Windows.Forms.CheckBox chbAllowSplitVert;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.TextSource.TextSource csharpSource;
        private System.Windows.Forms.Panel panel2;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.Splitter splitter2;
        private Alternet.Editor.SyntaxEdit syntaxEdit2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chbVerticalAnnotations;
    }
}