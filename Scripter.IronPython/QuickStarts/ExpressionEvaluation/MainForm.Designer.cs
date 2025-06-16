namespace ExpressionEvaluation
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
            this.btRun = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.pnEdit = new System.Windows.Forms.Panel();
            this.tbExpression = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.pnDescription.SuspendLayout();
            this.pnEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.btRun);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(528, 39);
            this.pnDescription.TabIndex = 1;
            // 
            // btRun
            // 
            this.btRun.Location = new System.Drawing.Point(259, 8);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(75, 23);
            this.btRun.TabIndex = 4;
            this.btRun.Text = "Evaluate";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(528, 39);
            this.laDescription.TabIndex = 3;
            this.laDescription.Text = "This demo shows how to execute script expressions.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnEdit
            // 
            this.pnEdit.Controls.Add(this.tbExpression);
            this.pnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEdit.Location = new System.Drawing.Point(0, 39);
            this.pnEdit.Name = "pnEdit";
            this.pnEdit.Size = new System.Drawing.Size(528, 126);
            this.pnEdit.TabIndex = 16;
            // 
            // tbExpression
            // 
            this.tbExpression.Location = new System.Drawing.Point(15, 14);
            this.tbExpression.Name = "tbExpression";
            this.tbExpression.Size = new System.Drawing.Size(330, 20);
            this.tbExpression.TabIndex = 0;
            this.tbExpression.Text = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 165);
            this.Controls.Add(this.pnEdit);
            this.Controls.Add(this.pnDescription);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Evaluate Expression";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnDescription.ResumeLayout(false);
            this.pnDescription.PerformLayout();
            this.pnEdit.ResumeLayout(false);
            this.pnEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Alternet.Scripter.IronPython.ScriptRun scriptRun;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Panel pnEdit;
        private System.Windows.Forms.TextBox tbExpression;
        private System.Windows.Forms.Button btRun;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}