namespace UndoRedo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnSettings = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbUndoNavigations = new System.Windows.Forms.CheckBox();
            this.chbGroupUndo = new System.Windows.Forms.CheckBox();
            this.cbSavedColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbChangedColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.laSavedColor = new System.Windows.Forms.Label();
            this.laChangedColor = new System.Windows.Forms.Label();
            this.btUndo = new System.Windows.Forms.Button();
            this.chbLineModificator = new System.Windows.Forms.CheckBox();
            this.btRedo = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbUndoOperations = new System.Windows.Forms.ListBox();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.groupBox1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 116);
            this.pnSettings.TabIndex = 19;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbUndoNavigations);
            this.groupBox1.Controls.Add(this.chbGroupUndo);
            this.groupBox1.Controls.Add(this.cbSavedColor);
            this.groupBox1.Controls.Add(this.cbChangedColor);
            this.groupBox1.Controls.Add(this.laSavedColor);
            this.groupBox1.Controls.Add(this.laChangedColor);
            this.groupBox1.Controls.Add(this.btUndo);
            this.groupBox1.Controls.Add(this.chbLineModificator);
            this.groupBox1.Controls.Add(this.btRedo);
            this.groupBox1.Controls.Add(this.btSave);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 85);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Undo & Redo";
            // 
            // chbUndoNavigations
            // 
            this.chbUndoNavigations.AutoSize = true;
            this.chbUndoNavigations.Location = new System.Drawing.Point(246, 19);
            this.chbUndoNavigations.Name = "chbUndoNavigations";
            this.chbUndoNavigations.Size = new System.Drawing.Size(111, 17);
            this.chbUndoNavigations.TabIndex = 23;
            this.chbUndoNavigations.Text = "Undo Navigations";
            this.chbUndoNavigations.UseVisualStyleBackColor = true;
            this.chbUndoNavigations.CheckedChanged += new System.EventHandler(this.UndoNavigationsCheckBox_CheckedChanged);
            this.chbUndoNavigations.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UndoNavigationsCheckBox_MouseMove);
            // 
            // chbGroupUndo
            // 
            this.chbGroupUndo.AutoSize = true;
            this.chbGroupUndo.Location = new System.Drawing.Point(156, 19);
            this.chbGroupUndo.Name = "chbGroupUndo";
            this.chbGroupUndo.Size = new System.Drawing.Size(84, 17);
            this.chbGroupUndo.TabIndex = 22;
            this.chbGroupUndo.Text = "Group Undo";
            this.chbGroupUndo.UseVisualStyleBackColor = true;
            this.chbGroupUndo.CheckedChanged += new System.EventHandler(this.GroupUndoCheckBox_CheckedChanged);
            this.chbGroupUndo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GroupUndoCheckBox_MouseMove);
            // 
            // cbSavedColor
            // 
            this.cbSavedColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSavedColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSavedColor.FormattingEnabled = true;
            this.cbSavedColor.Location = new System.Drawing.Point(556, 59);
            this.cbSavedColor.Name = "cbSavedColor";
            this.cbSavedColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbSavedColor.Size = new System.Drawing.Size(93, 21);
            this.cbSavedColor.TabIndex = 21;
            this.cbSavedColor.SelectedIndexChanged += new System.EventHandler(this.SavedColorComboBox_SelectedIndexChanged);
            this.cbSavedColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SavedColorComboBox_MouseMove);
            // 
            // cbChangedColor
            // 
            this.cbChangedColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbChangedColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChangedColor.FormattingEnabled = true;
            this.cbChangedColor.Location = new System.Drawing.Point(556, 33);
            this.cbChangedColor.Name = "cbChangedColor";
            this.cbChangedColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbChangedColor.Size = new System.Drawing.Size(93, 21);
            this.cbChangedColor.TabIndex = 20;
            this.cbChangedColor.SelectedIndexChanged += new System.EventHandler(this.ChangedColorComboBox_SelectedIndexChanged);
            this.cbChangedColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ChangedColorComboBox_MouseMove);
            // 
            // laSavedColor
            // 
            this.laSavedColor.AutoSize = true;
            this.laSavedColor.Location = new System.Drawing.Point(442, 62);
            this.laSavedColor.Name = "laSavedColor";
            this.laSavedColor.Size = new System.Drawing.Size(96, 13);
            this.laSavedColor.TabIndex = 19;
            this.laSavedColor.Text = "Saved Lines Color:";
            // 
            // laChangedColor
            // 
            this.laChangedColor.AutoSize = true;
            this.laChangedColor.Location = new System.Drawing.Point(442, 36);
            this.laChangedColor.Name = "laChangedColor";
            this.laChangedColor.Size = new System.Drawing.Size(108, 13);
            this.laChangedColor.TabIndex = 18;
            this.laChangedColor.Text = "Changed Lines Color:";
            // 
            // btUndo
            // 
            this.btUndo.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btUndo.Enabled = false;
            this.btUndo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btUndo.ImageIndex = 0;
            this.btUndo.Location = new System.Drawing.Point(8, 48);
            this.btUndo.Name = "btUndo";
            this.btUndo.Size = new System.Drawing.Size(72, 23);
            this.btUndo.TabIndex = 10;
            this.btUndo.Text = "Undo";
            this.btUndo.Click += new System.EventHandler(this.UndoButton_Click);
            this.btUndo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UndoButton_MouseMove);
            // 
            // chbLineModificator
            // 
            this.chbLineModificator.AutoSize = true;
            this.chbLineModificator.Checked = true;
            this.chbLineModificator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbLineModificator.Location = new System.Drawing.Point(8, 19);
            this.chbLineModificator.Name = "chbLineModificator";
            this.chbLineModificator.Size = new System.Drawing.Size(143, 17);
            this.chbLineModificator.TabIndex = 9;
            this.chbLineModificator.Text = "Display Line Modificators";
            this.chbLineModificator.CheckedChanged += new System.EventHandler(this.LineModificatorCheckBox_CheckedChanged);
            this.chbLineModificator.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineModificatorCheckBox_MouseMove);
            // 
            // btRedo
            // 
            this.btRedo.Enabled = false;
            this.btRedo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btRedo.ImageIndex = 1;
            this.btRedo.Location = new System.Drawing.Point(88, 48);
            this.btRedo.Name = "btRedo";
            this.btRedo.Size = new System.Drawing.Size(72, 23);
            this.btRedo.TabIndex = 11;
            this.btRedo.Text = "Redo";
            this.btRedo.Click += new System.EventHandler(this.RedoButton_Click);
            this.btRedo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RedoButton_MouseMove);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(168, 48);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(72, 23);
            this.btSave.TabIndex = 12;
            this.btSave.Text = "Save";
            this.btSave.Click += new System.EventHandler(this.SaveButton_Click);
            this.btSave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SaveButton_MouseMove);
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(5, 5);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(657, 21);
            this.pnDescription.TabIndex = 8;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(657, 21);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "This demo shows how to use the undo/redo history.    ";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbUndoOperations);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(459, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 259);
            this.panel1.TabIndex = 20;
            // 
            // lbUndoOperations
            // 
            this.lbUndoOperations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUndoOperations.IntegralHeight = false;
            this.lbUndoOperations.Location = new System.Drawing.Point(0, 0);
            this.lbUndoOperations.Name = "lbUndoOperations";
            this.lbUndoOperations.Size = new System.Drawing.Size(208, 259);
            this.lbUndoOperations.TabIndex = 0;
            this.lbUndoOperations.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UndoOperationsLisoxTextBox_MouseMove);
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 116);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(459, 259);
            this.syntaxEdit1.TabIndex = 21;
            this.syntaxEdit1.Text = "";
            this.syntaxEdit1.SourceStateChanged += new Alternet.Editor.NotifyEvent(this.SyntaxEdit1_SourceStateChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 375);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Undo & Redo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lbUndoOperations;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btUndo;
        private System.Windows.Forms.CheckBox chbLineModificator;
        private System.Windows.Forms.Button btRedo;
        private System.Windows.Forms.Button btSave;
        private Alternet.Editor.Common.ColorBox cbSavedColor;
        private Alternet.Editor.Common.ColorBox cbChangedColor;
        private System.Windows.Forms.Label laSavedColor;
        private System.Windows.Forms.Label laChangedColor;
        private System.Windows.Forms.CheckBox chbGroupUndo;
        private System.Windows.Forms.CheckBox chbUndoNavigations;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}