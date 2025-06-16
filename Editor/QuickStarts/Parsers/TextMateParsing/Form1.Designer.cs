namespace TextMateParsing
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.SettingsPanel = new System.Windows.Forms.Panel();
            this.ThemeNamesCombobox = new System.Windows.Forms.ComboBox();
            this.ThemeNamesLabel = new System.Windows.Forms.Label();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.visualThemeComboBox = new System.Windows.Forms.ComboBox();
            this.VisualThemesLabel = new System.Windows.Forms.Label();
            this.LanguagesCombobox = new System.Windows.Forms.ComboBox();
            this.LanguagesLabel = new System.Windows.Forms.Label();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.SettingsPanel.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.Controls.Add(this.ThemeNamesCombobox);
            this.SettingsPanel.Controls.Add(this.ThemeNamesLabel);
            this.SettingsPanel.Controls.Add(this.pnDescription);
            this.SettingsPanel.Controls.Add(this.visualThemeComboBox);
            this.SettingsPanel.Controls.Add(this.VisualThemesLabel);
            this.SettingsPanel.Controls.Add(this.LanguagesCombobox);
            this.SettingsPanel.Controls.Add(this.LanguagesLabel);
            this.SettingsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SettingsPanel.Location = new System.Drawing.Point(0, 0);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Size = new System.Drawing.Size(914, 107);
            this.SettingsPanel.TabIndex = 0;
            // 
            // ThemeNamesCombobox
            // 
            this.ThemeNamesCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThemeNamesCombobox.FormattingEnabled = true;
            this.ThemeNamesCombobox.Location = new System.Drawing.Point(673, 71);
            this.ThemeNamesCombobox.Name = "ThemeNamesCombobox";
            this.ThemeNamesCombobox.Size = new System.Drawing.Size(169, 24);
            this.ThemeNamesCombobox.TabIndex = 13;
            this.ThemeNamesCombobox.SelectedIndexChanged += new System.EventHandler(this.ThemesCombobox_SelectedIndexChanged);
            // 
            // ThemeNamesLabel
            // 
            this.ThemeNamesLabel.AutoSize = true;
            this.ThemeNamesLabel.Location = new System.Drawing.Point(611, 75);
            this.ThemeNamesLabel.Name = "ThemeNamesLabel";
            this.ThemeNamesLabel.Size = new System.Drawing.Size(49, 16);
            this.ThemeNamesLabel.TabIndex = 12;
            this.ThemeNamesLabel.Text = "Colors:";
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Margin = new System.Windows.Forms.Padding(4);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(914, 48);
            this.pnDescription.TabIndex = 2;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(914, 48);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "This demo shows how to use predefined syntax highlighting TextMate schemes for va" +
    "rious languages";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // visualThemeComboBox
            // 
            this.visualThemeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.visualThemeComboBox.FormattingEnabled = true;
            this.visualThemeComboBox.Location = new System.Drawing.Point(408, 70);
            this.visualThemeComboBox.Name = "visualThemeComboBox";
            this.visualThemeComboBox.Size = new System.Drawing.Size(169, 24);
            this.visualThemeComboBox.TabIndex = 11;
            this.visualThemeComboBox.SelectedIndexChanged += new System.EventHandler(this.VisualThemesComboBox_SelectedIndexChanged);
            // 
            // VisualThemesLabel
            // 
            this.VisualThemesLabel.AutoSize = true;
            this.VisualThemesLabel.Location = new System.Drawing.Point(295, 74);
            this.VisualThemesLabel.Name = "VisualThemesLabel";
            this.VisualThemesLabel.Size = new System.Drawing.Size(100, 16);
            this.VisualThemesLabel.TabIndex = 3;
            this.VisualThemesLabel.Text = "Visual Themes:";
            // 
            // LanguagesCombobox
            // 
            this.LanguagesCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguagesCombobox.FormattingEnabled = true;
            this.LanguagesCombobox.Location = new System.Drawing.Point(99, 70);
            this.LanguagesCombobox.Name = "LanguagesCombobox";
            this.LanguagesCombobox.Size = new System.Drawing.Size(138, 24);
            this.LanguagesCombobox.TabIndex = 1;
            this.LanguagesCombobox.SelectedIndexChanged += new System.EventHandler(this.LanguagesCombobox_SelectedIndexChanged);
            // 
            // LanguagesLabel
            // 
            this.LanguagesLabel.AutoSize = true;
            this.LanguagesLabel.Location = new System.Drawing.Point(16, 74);
            this.LanguagesLabel.Name = "LanguagesLabel";
            this.LanguagesLabel.Size = new System.Drawing.Size(78, 16);
            this.LanguagesLabel.TabIndex = 0;
            this.LanguagesLabel.Text = "Languages:";
            // 
            // MainPanel
            // 
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 107);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(914, 373);
            this.MainPanel.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 480);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.SettingsPanel);
            this.Name = "Form1";
            this.Text = "TextMate Parsing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsPanel.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SettingsPanel;
        private System.Windows.Forms.ComboBox LanguagesCombobox;
        private System.Windows.Forms.Label LanguagesLabel;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ComboBox visualThemeComboBox;
        private System.Windows.Forms.Label VisualThemesLabel;
        private System.Windows.Forms.ComboBox ThemeNamesCombobox;
        private System.Windows.Forms.Label ThemeNamesLabel;
    }
}
