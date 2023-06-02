namespace DebuggerIntegration.Python
{
    partial class DisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayForm));
            this.resultLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.DebugMeButton = new System.Windows.Forms.Button();
            this.RunMeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(16, 207);
            this.resultLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(0, 16);
            this.resultLabel.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(304, 16);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(265, 28);
            this.progressBar.TabIndex = 2;
            // 
            // DebugMeButton
            // 
            this.DebugMeButton.Location = new System.Drawing.Point(13, 16);
            this.DebugMeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DebugMeButton.Name = "DebugMeButton";
            this.DebugMeButton.Size = new System.Drawing.Size(279, 79);
            this.DebugMeButton.TabIndex = 3;
            this.DebugMeButton.Text = "Debug Me";
            this.DebugMeButton.UseVisualStyleBackColor = true;
            this.DebugMeButton.Click += DebugMeButton_Click;
            // 
            // RunMeButton
            // 
            this.RunMeButton.Location = new System.Drawing.Point(13, 124);
            this.RunMeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RunMeButton.Name = "RunMeButton";
            this.RunMeButton.Size = new System.Drawing.Size(279, 79);
            this.RunMeButton.TabIndex = 4;
            this.RunMeButton.Text = "Run Me";
            this.RunMeButton.UseVisualStyleBackColor = true;
            this.RunMeButton.Click += RunMeButton_Click;
            // 
            // DisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 554);
            this.Controls.Add(this.RunMeButton);
            this.Controls.Add(this.DebugMeButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.resultLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DisplayForm";
            this.Text = "DisplayForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button DebugMeButton;
        private System.Windows.Forms.Button RunMeButton;
    }
}