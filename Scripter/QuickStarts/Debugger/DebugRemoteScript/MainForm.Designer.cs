namespace DebugRemoteScript
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Designer generated code")]
    public partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkUseDirectAPI = new System.Windows.Forms.CheckBox();
            this.laDescription = new System.Windows.Forms.Label();
            this.debuggerControlToolbar = new Alternet.Scripter.Debugger.UI.DebuggerControlToolbar();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.laLanguages = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btRun = new System.Windows.Forms.Button();
            this.scriptRun = new Alternet.Scripter.ScriptRun(this.components);
            this.pnEdit = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.debugMenu1 = new Alternet.Scripter.Debugger.UI.DebugMenu();
            this.panel1.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkUseDirectAPI);
            this.panel1.Controls.Add(this.laDescription);
            this.panel1.Controls.Add(this.debuggerControlToolbar);
            this.panel1.Controls.Add(this.cbLanguages);
            this.panel1.Controls.Add(this.laLanguages);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btRun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 122);
            this.panel1.TabIndex = 0;
            // 
            // chkUseDirectAPI
            // 
            this.chkUseDirectAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkUseDirectAPI.AutoSize = true;
            this.chkUseDirectAPI.Location = new System.Drawing.Point(502, 83);
            this.chkUseDirectAPI.Name = "chkUseDirectAPI";
            this.chkUseDirectAPI.Size = new System.Drawing.Size(186, 17);
            this.chkUseDirectAPI.TabIndex = 29;
            this.chkUseDirectAPI.Text = "Use direct API for script execution";
            this.chkUseDirectAPI.Checked = true;
            this.chkUseDirectAPI.UseVisualStyleBackColor = true;
            this.chkUseDirectAPI.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UseDirectAPICheckBox_MouseMove);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.laDescription.Location = new System.Drawing.Point(0, 25);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(714, 25);
            this.laDescription.TabIndex = 28;
            this.laDescription.Text = "This demo shows how to embedd debugger into your application and access applicati" +
    "on objects from script using remoting.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // debuggerControlToolbar
            // 
            this.debuggerControlToolbar.Location = new System.Drawing.Point(0, 0);
            this.debuggerControlToolbar.Name = "debuggerControlToolbar";
            this.debuggerControlToolbar.Size = new System.Drawing.Size(714, 25);
            this.debuggerControlToolbar.TabIndex = 26;
            this.debuggerControlToolbar.Text = "debuggerControlToolbar1";
            // 
            // cbLanguages
            // 
            this.cbLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.Items.AddRange(new object[] {
            "C#",
            "Visual Basic"});
            this.cbLanguages.Location = new System.Drawing.Point(578, 56);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(115, 21);
            this.cbLanguages.TabIndex = 24;
            this.cbLanguages.SelectedIndexChanged += new System.EventHandler(this.LanguagesComboBox_SelectedIndexChanged);
            this.cbLanguages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LanguagesComboBox_MouseMove);
            // 
            // laLanguages
            // 
            this.laLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laLanguages.AutoSize = true;
            this.laLanguages.Location = new System.Drawing.Point(514, 59);
            this.laLanguages.Name = "laLanguages";
            this.laLanguages.Size = new System.Drawing.Size(58, 13);
            this.laLanguages.TabIndex = 23;
            this.laLanguages.Text = "Language:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(21, 88);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(78, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Hello World";
            // 
            // btRun
            // 
            this.btRun.Location = new System.Drawing.Point(21, 56);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(75, 23);
            this.btRun.TabIndex = 0;
            this.btRun.Text = "Run script";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // pnEdit
            // 
            this.pnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEdit.Location = new System.Drawing.Point(0, 146);
            this.pnEdit.Name = "pnEdit";
            this.pnEdit.Size = new System.Drawing.Size(714, 296);
            this.pnEdit.TabIndex = 16;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugMenu1});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(714, 24);
            this.mainMenu.TabIndex = 5;
            this.mainMenu.Text = "menuStrip1";
            // 
            // debugMenu1
            // 
            this.debugMenu1.Name = "miDebug";
            this.debugMenu1.Size = new System.Drawing.Size(54, 20);
            this.debugMenu1.Text = "Debug";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 442);
            this.Controls.Add(this.pnEdit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debug remote script";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btRun;
        private Alternet.Scripter.ScriptRun scriptRun;
        private System.Windows.Forms.Panel pnEdit;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label laLanguages;
        private System.Windows.Forms.ToolTip toolTip1;
        private Alternet.Scripter.Debugger.UI.DebuggerControlToolbar debuggerControlToolbar;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.MenuStrip mainMenu;
        private Alternet.Scripter.Debugger.UI.DebugMenu debugMenu1;
        private System.Windows.Forms.CheckBox chkUseDirectAPI;
        public System.Windows.Forms.TextBox textBox1;
    }
}