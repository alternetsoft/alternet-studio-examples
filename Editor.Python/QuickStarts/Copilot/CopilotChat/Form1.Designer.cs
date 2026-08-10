namespace PythonCopilotChat
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
            components = new System.ComponentModel.Container();
            pnSettings = new System.Windows.Forms.Panel();
            pnDescription = new System.Windows.Forms.Panel();
            laDescription = new System.Windows.Forms.Label();
            csharpSource = new Alternet.Editor.TextSource.TextSource(components);
            vbSource = new Alternet.Editor.TextSource.TextSource(components);
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            aiProviderTabControl = new System.Windows.Forms.TabControl();
            aiProviderTab = new System.Windows.Forms.TabPage();
            splitter1 = new System.Windows.Forms.Splitter();
            editorTabControl = new System.Windows.Forms.TabControl();
            pnSettings.SuspendLayout();
            pnDescription.SuspendLayout();
            aiProviderTabControl.SuspendLayout();
            SuspendLayout();
            // 
            // pnSettings
            // 
            pnSettings.Controls.Add(pnDescription);
            pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            pnSettings.Location = new System.Drawing.Point(0, 0);
            pnSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnSettings.Name = "pnSettings";
            pnSettings.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            pnSettings.Size = new System.Drawing.Size(1082, 71);
            pnSettings.TabIndex = 4;
            // 
            // pnDescription
            // 
            pnDescription.Controls.Add(laDescription);
            pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            pnDescription.Location = new System.Drawing.Point(7, 8);
            pnDescription.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnDescription.Name = "pnDescription";
            pnDescription.Size = new System.Drawing.Size(1068, 60);
            pnDescription.TabIndex = 8;
            // 
            // laDescription
            // 
            laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            laDescription.Location = new System.Drawing.Point(0, 0);
            laDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laDescription.Name = "laDescription";
            laDescription.Size = new System.Drawing.Size(1068, 60);
            laDescription.TabIndex = 1;
            laDescription.Text = "This demo shows how to integrate Copilot Chat with our editor and C# language parser.";
            laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // aiProviderTabControl
            // 
            aiProviderTabControl.Controls.Add(aiProviderTab);
            aiProviderTabControl.Dock = System.Windows.Forms.DockStyle.Right;
            aiProviderTabControl.Location = new System.Drawing.Point(682, 71);
            aiProviderTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            aiProviderTabControl.Name = "aiProviderTabControl";
            aiProviderTabControl.SelectedIndex = 0;
            aiProviderTabControl.Size = new System.Drawing.Size(400, 582);
            aiProviderTabControl.TabIndex = 7;
            // 
            // aiProviderTab
            // 
            aiProviderTab.Location = new System.Drawing.Point(4, 29);
            aiProviderTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            aiProviderTab.Name = "aiProviderTab";
            aiProviderTab.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            aiProviderTab.Size = new System.Drawing.Size(392, 549);
            aiProviderTab.TabIndex = 0;
            aiProviderTab.Text = "Chat";
            aiProviderTab.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            splitter1.Location = new System.Drawing.Point(677, 71);
            splitter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(5, 582);
            splitter1.TabIndex = 8;
            splitter1.TabStop = false;
            // 
            // editorTabControl
            // 
            editorTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            editorTabControl.Location = new System.Drawing.Point(0, 71);
            editorTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            editorTabControl.Name = "editorTabControl";
            editorTabControl.SelectedIndex = 0;
            editorTabControl.Size = new System.Drawing.Size(677, 582);
            editorTabControl.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1082, 653);
            Controls.Add(editorTabControl);
            Controls.Add(splitter1);
            Controls.Add(aiProviderTabControl);
            Controls.Add(pnSettings);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Python TextEditor and Copilot Chat Integration";
            Load += Form1_Load;
            pnSettings.ResumeLayout(false);
            pnDescription.ResumeLayout(false);
            aiProviderTabControl.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.TextSource.TextSource csharpSource;
        private Alternet.Editor.TextSource.TextSource vbSource;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl aiProviderTabControl;
        private System.Windows.Forms.TabPage aiProviderTab;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl editorTabControl;
    }
}