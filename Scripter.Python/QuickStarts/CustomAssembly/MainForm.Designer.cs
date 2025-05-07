namespace CustomAssembly
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
            this.scriptRun = new Alternet.Scripter.Python.ScriptRun();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.btRun = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.tcEditors = new System.Windows.Forms.TabControl();
            this.tpEditor = new System.Windows.Forms.TabPage();
            this.tpExternal = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.pythonParser1 = new Alternet.Syntax.Parsers.Python.PythonNETParser();
            this.csParser1 = new Alternet.Syntax.Parsers.Advanced.CsParser();
            this.pnDescription.SuspendLayout();
            this.tcEditors.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.btRun);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(718, 39);
            this.pnDescription.TabIndex = 9;
            // 
            // btRun
            // 
            this.btRun.Location = new System.Drawing.Point(306, 8);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(75, 23);
            this.btRun.TabIndex = 2;
            this.btRun.Text = "Run script";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(718, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "This demo shows how to use external assemblies in scripting.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tcEditors
            // 
            this.tcEditors.Controls.Add(this.tpEditor);
            this.tcEditors.Controls.Add(this.tpExternal);
            this.tcEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEditors.Location = new System.Drawing.Point(0, 39);
            this.tcEditors.Name = "tcEditors";
            this.tcEditors.SelectedIndex = 0;
            this.tcEditors.Size = new System.Drawing.Size(718, 403);
            this.tcEditors.TabIndex = 10;
            // 
            // tpEditor
            // 
            this.tpEditor.Location = new System.Drawing.Point(4, 22);
            this.tpEditor.Name = "tpEditor";
            this.tpEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tpEditor.Size = new System.Drawing.Size(710, 377);
            this.tpEditor.TabIndex = 0;
            this.tpEditor.Text = "Source Code";
            this.tpEditor.UseVisualStyleBackColor = true;
            // 
            // tpExternal
            // 
            this.tpExternal.Location = new System.Drawing.Point(4, 22);
            this.tpExternal.Name = "tpExternal";
            this.tpExternal.Padding = new System.Windows.Forms.Padding(3);
            this.tpExternal.Size = new System.Drawing.Size(710, 377);
            this.tpExternal.TabIndex = 1;
            this.tpExternal.Text = "External Code";
            this.tpExternal.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 442);
            this.Controls.Add(this.tcEditors);
            this.Controls.Add(this.pnDescription);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom assembly";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnDescription.ResumeLayout(false);
            this.pnDescription.PerformLayout();
            this.tcEditors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Alternet.Scripter.Python.ScriptRun scriptRun;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Button btRun;
        private System.Windows.Forms.TabControl tcEditors;
        private System.Windows.Forms.TabPage tpEditor;
        private System.Windows.Forms.TabPage tpExternal;
        private System.Windows.Forms.ToolTip toolTip1;
        private Alternet.Syntax.Parsers.Python.PythonNETParser pythonParser1;
        private Alternet.Syntax.Parsers.Advanced.CsParser csParser1;
    }
}