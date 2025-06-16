namespace TextDifference
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            mainMenu = new System.Windows.Forms.MenuStrip();
            FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenFile1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenFile2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            CompareMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            syntaxEdit1 = new Alternet.Editor.SyntaxEdit(components);
            splitter1 = new System.Windows.Forms.Splitter();
            syntaxEdit2 = new Alternet.Editor.SyntaxEdit(components);
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            mainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileMenuItem });
            mainMenu.Location = new System.Drawing.Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new System.Drawing.Size(778, 24);
            mainMenu.TabIndex = 0;
            mainMenu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { OpenFile1MenuItem, OpenFile2MenuItem, toolStripMenuItem1, CompareMenuItem });
            FileMenuItem.Name = "FileMenuItem";
            FileMenuItem.Size = new System.Drawing.Size(37, 20);
            FileMenuItem.Text = "File";
            // 
            // OpenFile1MenuItem
            // 
            OpenFile1MenuItem.Name = "OpenFile1MenuItem";
            OpenFile1MenuItem.Size = new System.Drawing.Size(180, 22);
            OpenFile1MenuItem.Text = "Open File 1";
            OpenFile1MenuItem.Click += OpenFile1MenuItem_Click;
            // 
            // OpenFile2MenuItem
            // 
            OpenFile2MenuItem.Name = "OpenFile2MenuItem";
            OpenFile2MenuItem.Size = new System.Drawing.Size(180, 22);
            OpenFile2MenuItem.Text = "Open File 2";
            OpenFile2MenuItem.Click += OpenFile1MenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // CompareMenuItem
            // 
            CompareMenuItem.Name = "CompareMenuItem";
            CompareMenuItem.Size = new System.Drawing.Size(180, 22);
            CompareMenuItem.Text = "Compare";
            CompareMenuItem.Click += CompareMenuItem_Click;
            // 
            // syntaxEdit1
            // 
            syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Left;
            syntaxEdit1.Location = new System.Drawing.Point(0, 24);
            syntaxEdit1.Name = "syntaxEdit1";
            syntaxEdit1.Size = new System.Drawing.Size(383, 484);
            syntaxEdit1.TabIndex = 1;
            syntaxEdit1.Text = "syntaxEdit1";
            // 
            // splitter1
            // 
            splitter1.Location = new System.Drawing.Point(383, 24);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(5, 484);
            splitter1.TabIndex = 2;
            splitter1.TabStop = false;
            // 
            // syntaxEdit2
            // 
            syntaxEdit2.BackColor = System.Drawing.SystemColors.Window;
            syntaxEdit2.Dock = System.Windows.Forms.DockStyle.Fill;
            syntaxEdit2.Location = new System.Drawing.Point(388, 24);
            syntaxEdit2.Name = "syntaxEdit2";
            syntaxEdit2.Size = new System.Drawing.Size(390, 484);
            syntaxEdit2.TabIndex = 3;
            syntaxEdit2.Text = "syntaxEdit2";
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(778, 508);
            Controls.Add(syntaxEdit2);
            Controls.Add(splitter1);
            Controls.Add(syntaxEdit1);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Text Difference";
            Load += Form1_Load;
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFile1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFile2MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CompareMenuItem;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.Splitter splitter1;
        private Alternet.Editor.SyntaxEdit syntaxEdit2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}