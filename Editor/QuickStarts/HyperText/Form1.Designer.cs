namespace HyperText
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
            this.laFontStyle = new System.Windows.Forms.Label();
            this.cbFontStyle = new System.Windows.Forms.ComboBox();
            this.chbCustomHypertext = new System.Windows.Forms.CheckBox();
            this.laUrlColor = new System.Windows.Forms.Label();
            this.chbHighlightUrls = new System.Windows.Forms.CheckBox();
            this.cbUrlColor = new Alternet.Editor.Common.ColorBox();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.pnSettings.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.laFontStyle);
            this.pnSettings.Controls.Add(this.cbFontStyle);
            this.pnSettings.Controls.Add(this.chbCustomHypertext);
            this.pnSettings.Controls.Add(this.laUrlColor);
            this.pnSettings.Controls.Add(this.chbHighlightUrls);
            this.pnSettings.Controls.Add(this.cbUrlColor);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 99);
            this.pnSettings.TabIndex = 8;
            // 
            // laFontStyle
            // 
            this.laFontStyle.AutoSize = true;
            this.laFontStyle.Location = new System.Drawing.Point(137, 73);
            this.laFontStyle.Name = "laFontStyle";
            this.laFontStyle.Size = new System.Drawing.Size(57, 13);
            this.laFontStyle.TabIndex = 16;
            this.laFontStyle.Text = "Font Style:";
            // 
            // cbFontStyle
            // 
            this.cbFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFontStyle.Location = new System.Drawing.Point(196, 70);
            this.cbFontStyle.Name = "cbFontStyle";
            this.cbFontStyle.Size = new System.Drawing.Size(148, 21);
            this.cbFontStyle.TabIndex = 17;
            this.cbFontStyle.SelectedIndexChanged += new System.EventHandler(this.FontStyleComboBox_SelectedIndexChanged);
            this.cbFontStyle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FontStyleComboBox_MouseMove);
            // 
            // chbCustomHypertext
            // 
            this.chbCustomHypertext.Location = new System.Drawing.Point(12, 68);
            this.chbCustomHypertext.Name = "chbCustomHypertext";
            this.chbCustomHypertext.Size = new System.Drawing.Size(120, 24);
            this.chbCustomHypertext.TabIndex = 14;
            this.chbCustomHypertext.Text = "Custom Hypertext";
            this.chbCustomHypertext.CheckedChanged += new System.EventHandler(this.CustomHypertextCheckBox_CheckedChanged);
            this.chbCustomHypertext.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CustomHypertextCheckBox_MouseMove);
            // 
            // laUrlColor
            // 
            this.laUrlColor.AutoSize = true;
            this.laUrlColor.Location = new System.Drawing.Point(137, 41);
            this.laUrlColor.Name = "laUrlColor";
            this.laUrlColor.Size = new System.Drawing.Size(59, 13);
            this.laUrlColor.TabIndex = 15;
            this.laUrlColor.Text = "URL Color:";
            // 
            // chbHighlightUrls
            // 
            this.chbHighlightUrls.AutoSize = true;
            this.chbHighlightUrls.Checked = true;
            this.chbHighlightUrls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbHighlightUrls.Location = new System.Drawing.Point(12, 40);
            this.chbHighlightUrls.Name = "chbHighlightUrls";
            this.chbHighlightUrls.Size = new System.Drawing.Size(97, 17);
            this.chbHighlightUrls.TabIndex = 0;
            this.chbHighlightUrls.Text = "Highlight URLs";
            this.chbHighlightUrls.CheckedChanged += new System.EventHandler(this.HighlightUrlsCheckBox_CheckedChanged);
            this.chbHighlightUrls.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlightUrlsCheckBox_MouseMove);
            // 
            // cbUrlColor
            // 
            this.cbUrlColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbUrlColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUrlColor.FormattingEnabled = true;
            this.cbUrlColor.Location = new System.Drawing.Point(196, 38);
            this.cbUrlColor.Name = "cbUrlColor";
            this.cbUrlColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbUrlColor.Size = new System.Drawing.Size(148, 21);
            this.cbUrlColor.TabIndex = 12;
            this.cbUrlColor.SelectedIndexChanged += new System.EventHandler(this.UrlColorComboBox_SelectedIndexChanged);
            this.cbUrlColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UrlColorComboBox_MouseMove);
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
            this.laDescription.Text = "Code Editor can Highlight and navigate through hyperlinks displayed in the text.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.HyperText.HighlightHyperText = true;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 99);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 9;
            this.syntaxEdit1.Text = "";
            this.syntaxEdit1.JumpToUrl += new Alternet.Editor.UrlJumpEvent(this.SyntaxEdit1_JumpToUrl);
            this.syntaxEdit1.CheckHyperText += new Alternet.Editor.TextSource.HyperTextEvent(this.SyntaxEdit1_CheckHyperText);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 358);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hyper Text";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.pnSettings.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private Alternet.Editor.Common.ColorBox cbUrlColor;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Label laFontStyle;
        private System.Windows.Forms.ComboBox cbFontStyle;
        private System.Windows.Forms.CheckBox chbCustomHypertext;
        private System.Windows.Forms.Label laUrlColor;
        private System.Windows.Forms.CheckBox chbHighlightUrls;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}