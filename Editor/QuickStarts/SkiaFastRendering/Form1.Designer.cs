using System.Windows.Forms;

namespace SkiaFastRendering
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Supress for Visual Studio-generated code")]
    partial class Form1
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pnlSettings = new System.Windows.Forms.Panel();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            pnDescription = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.Panel();
            cbSyntax = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            syntaxEdit1 = new Alternet.Editor.SkiaSyntaxEdit();
            btnCountSpeed = new System.Windows.Forms.Button();
            cbRenderMode = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            chbWordWrap = new System.Windows.Forms.CheckBox();
            pnlSettings.SuspendLayout();
            pnDescription.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSettings
            // 
            pnlSettings.Controls.Add(groupBox1);
            pnlSettings.Controls.Add(pnDescription);
            pnlSettings.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSettings.Location = new System.Drawing.Point(0, 0);
            pnlSettings.Margin = new System.Windows.Forms.Padding(4);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Padding = new System.Windows.Forms.Padding(7);
            pnlSettings.Size = new System.Drawing.Size(1200, 137);
            pnlSettings.TabIndex = 4;
            // 
            // pnDescription
            // 
            pnDescription.Controls.Add(label1);
            pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            pnDescription.Location = new System.Drawing.Point(7, 7);
            pnDescription.Name = "pnDescription";
            pnDescription.Size = new System.Drawing.Size(1186, 49);
            pnDescription.TabIndex = 12;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new System.Windows.Forms.Padding(4);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(1186, 49);
            label1.TabIndex = 11;
            label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbSyntax);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(btnCountSpeed);
            groupBox1.Controls.Add(cbRenderMode);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(chbWordWrap);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(7, 56);
            groupBox1.Margin = new System.Windows.Forms.Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(1186, 74);
            groupBox1.TabIndex = 13;
            // 
            // cbSyntax
            // 
            cbSyntax.FormattingEnabled = true;
            cbSyntax.Location = new System.Drawing.Point(266, 0);
            cbSyntax.Name = "cbSyntax";
            cbSyntax.Size = new System.Drawing.Size(165, 21);
            cbSyntax.TabIndex = 15;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(212, 2);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(55, 13);
            label3.TabIndex = 14;
            label3.Text = "Syntax:";
            // 
            // btnCountSpeed
            // 
            btnCountSpeed.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnCountSpeed.Location = new System.Drawing.Point(16, 27);
            btnCountSpeed.Name = "btnCountSpeed";
            btnCountSpeed.Size = new System.Drawing.Size(140, 23);
            btnCountSpeed.TabIndex = 13;
            btnCountSpeed.Text = "Track Speed";
            btnCountSpeed.UseVisualStyleBackColor = true;
            btnCountSpeed.Click += BtnCountSpeed_Click;
            // 
            // cbRenderMode
            // 
            cbRenderMode.FormattingEnabled = true;
            cbRenderMode.Location = new System.Drawing.Point(56, 0);
            cbRenderMode.Name = "cbRenderMode";
            cbRenderMode.Size = new System.Drawing.Size(149, 21);
            cbRenderMode.TabIndex = 12;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(16, 2);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(51, 13);
            label2.TabIndex = 11;
            label2.Text = "Mode:";
            // 
            // chbWordWrap
            // 
            chbWordWrap.AutoSize = true;
            chbWordWrap.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            chbWordWrap.Location = new System.Drawing.Point(457, 2);
            chbWordWrap.Name = "chbWordWrap";
            chbWordWrap.Size = new System.Drawing.Size(107, 21);
            chbWordWrap.TabIndex = 9;
            chbWordWrap.Text = "Word Wrap";
            chbWordWrap.CheckedChanged += WordWrapCheckBox_CheckedChanged;
            chbWordWrap.MouseMove += WordWrapCheckBox_MouseMove;
            // 
            // syntaxEdit1
            // 
            syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            syntaxEdit1.EditMargin.Position = 60;
            syntaxEdit1.ForeColor = System.Drawing.SystemColors.WindowText;
            syntaxEdit1.Location = new System.Drawing.Point(0, 225);
            syntaxEdit1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            syntaxEdit1.Name = "syntaxEdit1";
            syntaxEdit1.Outlining.AllowOutlining = true;
            syntaxEdit1.Size = new System.Drawing.Size(1800, 929);
            syntaxEdit1.TabIndex = 5;
            syntaxEdit1.Text = "";
            syntaxEdit1.Transparent = true;
            syntaxEdit1.WordWrap = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(667, 358);
            Controls.Add(syntaxEdit1);
            Controls.Add(pnlSettings);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SyntaxEdit via SkiaSharp and OpenGL";
            Load += Form1_Load;
            pnlSettings.ResumeLayout(false);
            pnDescription.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlSettings;
        private Alternet.Editor.SkiaSyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Panel groupBox1;
        private System.Windows.Forms.ComboBox cbSyntax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCountSpeed;
        private System.Windows.Forms.ComboBox cbRenderMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbWordWrap;
        private System.Windows.Forms.Label label1;
    }
}