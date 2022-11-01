namespace DebugMyScript
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Supress for Visual Studio-generated code")]
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.runScriptButton = new System.Windows.Forms.Button();
            this.scriptRun = new Alternet.Scripter.ScriptRun();
            this.startDebuggerButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.laDescription = new System.Windows.Forms.Label();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.laLanguages = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.displayPanel = new DebugMyScript.DisplayPanel();
            this.SuspendLayout();
            // 
            // runScriptButton
            // 
            this.runScriptButton.Location = new System.Drawing.Point(68, 215);
            this.runScriptButton.Name = "runScriptButton";
            this.runScriptButton.Size = new System.Drawing.Size(121, 23);
            this.runScriptButton.TabIndex = 0;
            this.runScriptButton.Text = "Run Script";
            this.runScriptButton.UseVisualStyleBackColor = true;
            this.runScriptButton.Click += new System.EventHandler(this.RunScriptButton_Click);
            // 
            // scriptRun
            // 
            this.scriptRun.ScriptMode = Alternet.Scripter.ScriptMode.Debug;
            // 
            // startDebuggerButton
            // 
            this.startDebuggerButton.Location = new System.Drawing.Point(68, 244);
            this.startDebuggerButton.Name = "startDebuggerButton";
            this.startDebuggerButton.Size = new System.Drawing.Size(121, 23);
            this.startDebuggerButton.TabIndex = 0;
            this.startDebuggerButton.Text = "Start Debugger";
            this.startDebuggerButton.UseVisualStyleBackColor = true;
            this.startDebuggerButton.Click += new System.EventHandler(this.StartDebuggerButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(68, 280);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Visible = false;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(257, 26);
            this.laDescription.TabIndex = 4;
            this.laDescription.Text = "This demo shows how to debug script with external debugger.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbLanguages
            // 
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.Items.AddRange(new object[] {
            "C#",
            "Visual Basic"});
            this.cbLanguages.Location = new System.Drawing.Point(68, 188);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(121, 21);
            this.cbLanguages.TabIndex = 20;
            this.cbLanguages.SelectedIndexChanged += new System.EventHandler(this.LanguagesComboBox_SelectedIndexChanged);
            this.cbLanguages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LanguagesComboBox_MouseMove);
            // 
            // laLanguages
            // 
            this.laLanguages.AutoSize = true;
            this.laLanguages.Location = new System.Drawing.Point(4, 191);
            this.laLanguages.Name = "laLanguages";
            this.laLanguages.Size = new System.Drawing.Size(58, 13);
            this.laLanguages.TabIndex = 19;
            this.laLanguages.Text = "Language:";
            // 
            // displayPanel
            // 
            this.displayPanel.BackColor = System.Drawing.Color.Black;
            this.displayPanel.Location = new System.Drawing.Point(68, 33);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(129, 130);
            this.displayPanel.TabIndex = 3;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DisplayPanel_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 308);
            this.Controls.Add(this.cbLanguages);
            this.Controls.Add(this.laLanguages);
            this.Controls.Add(this.laDescription);
            this.Controls.Add(this.displayPanel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.startDebuggerButton);
            this.Controls.Add(this.runScriptButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debug My Script";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runScriptButton;
        private Alternet.Scripter.ScriptRun scriptRun;
        private System.Windows.Forms.Button startDebuggerButton;
        private System.Windows.Forms.TextBox textBox1;
        private DebugMyScript.DisplayPanel displayPanel;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label laLanguages;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}