namespace CallMethod
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.scriptRun = new Alternet.Scripter.IronPython.ScriptRun();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.runScriptButton = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.displayPanel = new CallMethod.DisplayPanel();
            this.pnEdit = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.ironPythonParser1 = new Alternet.Syntax.Parsers.Python.IronPythonParser();
            this.pnDescription.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // scriptRun
            // 
            //this.scriptRun.ScriptMode = Alternet.Scripter.ScriptMode.Debug;
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.runScriptButton);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(615, 34);
            this.pnDescription.TabIndex = 4;
            // 
            // runScriptButton
            // 
            this.runScriptButton.Location = new System.Drawing.Point(262, 8);
            this.runScriptButton.Name = "runScriptButton";
            this.runScriptButton.Size = new System.Drawing.Size(64, 20);
            this.runScriptButton.TabIndex = 3;
            this.runScriptButton.Text = "Run Script";
            this.runScriptButton.UseVisualStyleBackColor = true;
            this.runScriptButton.Click += new System.EventHandler(this.RunScriptButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(615, 34);
            this.laDescription.TabIndex = 2;
            this.laDescription.Text = "This demo shows how to execute script methods.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.displayPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(615, 143);
            this.panel2.TabIndex = 5;
            // 
            // displayPanel
            // 
            this.displayPanel.BackColor = System.Drawing.Color.Black;
            this.displayPanel.Location = new System.Drawing.Point(223, 5);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(129, 130);
            this.displayPanel.TabIndex = 3;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DisplayPanel_Paint);
            // 
            // pnEdit
            // 
            this.pnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEdit.Location = new System.Drawing.Point(0, 177);
            this.pnEdit.Name = "pnEdit";
            this.pnEdit.Size = new System.Drawing.Size(615, 206);
            this.pnEdit.TabIndex = 17;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 383);
            this.Controls.Add(this.pnEdit);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnDescription);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Call method";
            this.pnDescription.ResumeLayout(false);
            this.pnDescription.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Alternet.Scripter.IronPython.ScriptRun scriptRun;
        private CallMethod.DisplayPanel displayPanel;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnEdit;
        private System.Windows.Forms.Button runScriptButton;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ToolTip toolTip1;
        private Alternet.Syntax.Parsers.Python.IronPythonParser ironPythonParser1;
    }
}
