namespace RunScriptMethods
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
            this.scriptRun = new Alternet.Scripter.ScriptRun(this.components);
            this.pnDescription = new System.Windows.Forms.Panel();
            this.ActionsComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.runScriptButton = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.pnEdit = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // scriptRun
            // 
            this.scriptRun.ScriptMode = Alternet.Scripter.ScriptMode.Debug;
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.ActionsComboBox);
            this.pnDescription.Controls.Add(this.label1);
            this.pnDescription.Controls.Add(this.runScriptButton);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Margin = new System.Windows.Forms.Padding(4);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(820, 72);
            this.pnDescription.TabIndex = 4;
            // 
            // ActionsComboBox
            // 
            this.ActionsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ActionsComboBox.Location = new System.Drawing.Point(672, 11);
            this.ActionsComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.ActionsComboBox.Name = "ActionsComboBox";
            this.ActionsComboBox.Size = new System.Drawing.Size(144, 24);
            this.ActionsComboBox.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(622, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "Action:";
            // 
            // runScriptButton
            // 
            this.runScriptButton.Location = new System.Drawing.Point(343, 42);
            this.runScriptButton.Margin = new System.Windows.Forms.Padding(4);
            this.runScriptButton.Name = "runScriptButton";
            this.runScriptButton.Size = new System.Drawing.Size(85, 25);
            this.runScriptButton.TabIndex = 3;
            this.runScriptButton.Text = "Run Script";
            this.runScriptButton.UseVisualStyleBackColor = true;
            this.runScriptButton.Click += new System.EventHandler(this.RunScriptButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(820, 42);
            this.laDescription.TabIndex = 2;
            this.laDescription.Text = "This demo shows how to execute script methods and instantiate classes defined in scripts via reflection.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnEdit
            // 
            this.pnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEdit.Location = new System.Drawing.Point(0, 42);
            this.pnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.pnEdit.Name = "pnEdit";
            this.pnEdit.Size = new System.Drawing.Size(820, 429);
            this.pnEdit.TabIndex = 17;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 471);
            this.Controls.Add(this.pnEdit);
            this.Controls.Add(this.pnDescription);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Script Actions";
            this.pnDescription.ResumeLayout(false);
            this.pnDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Alternet.Scripter.ScriptRun scriptRun;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Panel pnEdit;
        private System.Windows.Forms.Button runScriptButton;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox ActionsComboBox;
        private System.Windows.Forms.Label label1;
    }
}
