namespace Margin
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbColumnsVisible = new System.Windows.Forms.CheckBox();
            this.laColumnPenColor = new System.Windows.Forms.Label();
            this.laMarginColor = new System.Windows.Forms.Label();
            this.laMarginPos = new System.Windows.Forms.Label();
            this.chbShowMargin = new System.Windows.Forms.CheckBox();
            this.nudMarginPos = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.laUserMarginBkColor = new System.Windows.Forms.Label();
            this.laUserMarginForeColor = new System.Windows.Forms.Label();
            this.laUserMarginText = new System.Windows.Forms.Label();
            this.tbUserMarginText = new System.Windows.Forms.TextBox();
            this.laUserMarginWidth = new System.Windows.Forms.Label();
            this.nudUserMarginWidth = new System.Windows.Forms.NumericUpDown();
            this.chbPaintUserMargin = new System.Windows.Forms.CheckBox();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbColumnsPenColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbMarginColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbUserMarginBkColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbUserMarginForeColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.pnSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMarginPos)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUserMarginWidth)).BeginInit();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.groupBox2);
            this.pnSettings.Controls.Add(this.groupBox1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 181);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbColumnsPenColor);
            this.groupBox2.Controls.Add(this.cbMarginColor);
            this.groupBox2.Controls.Add(this.chbColumnsVisible);
            this.groupBox2.Controls.Add(this.laColumnPenColor);
            this.groupBox2.Controls.Add(this.laMarginColor);
            this.groupBox2.Controls.Add(this.laMarginPos);
            this.groupBox2.Controls.Add(this.chbShowMargin);
            this.groupBox2.Controls.Add(this.nudMarginPos);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(334, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 132);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            // 
            // chbColumnsVisible
            // 
            this.chbColumnsVisible.AutoSize = true;
            this.chbColumnsVisible.Location = new System.Drawing.Point(15, 74);
            this.chbColumnsVisible.Name = "chbColumnsVisible";
            this.chbColumnsVisible.Size = new System.Drawing.Size(103, 17);
            this.chbColumnsVisible.TabIndex = 17;
            this.chbColumnsVisible.Text = "Display Columns";
            this.chbColumnsVisible.CheckedChanged += new System.EventHandler(this.ColumnsVisibleCheckBox_CheckedChanged);
            this.chbColumnsVisible.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ColumnsVisibleCheckBox_MouseMove);
            // 
            // laColumnPenColor
            // 
            this.laColumnPenColor.AutoSize = true;
            this.laColumnPenColor.Location = new System.Drawing.Point(12, 102);
            this.laColumnPenColor.Name = "laColumnPenColor";
            this.laColumnPenColor.Size = new System.Drawing.Size(72, 13);
            this.laColumnPenColor.TabIndex = 27;
            this.laColumnPenColor.Text = "Column Color:";
            // 
            // laMarginColor
            // 
            this.laMarginColor.AutoSize = true;
            this.laMarginColor.Location = new System.Drawing.Point(12, 47);
            this.laMarginColor.Name = "laMarginColor";
            this.laMarginColor.Size = new System.Drawing.Size(69, 13);
            this.laMarginColor.TabIndex = 25;
            this.laMarginColor.Text = "Margin Color:";
            // 
            // laMarginPos
            // 
            this.laMarginPos.AutoSize = true;
            this.laMarginPos.Location = new System.Drawing.Point(12, 22);
            this.laMarginPos.Name = "laMarginPos";
            this.laMarginPos.Size = new System.Drawing.Size(82, 13);
            this.laMarginPos.TabIndex = 17;
            this.laMarginPos.Text = "Margin Position:";
            // 
            // chbShowMargin
            // 
            this.chbShowMargin.AutoSize = true;
            this.chbShowMargin.Location = new System.Drawing.Point(6, 3);
            this.chbShowMargin.Name = "chbShowMargin";
            this.chbShowMargin.Size = new System.Drawing.Size(95, 17);
            this.chbShowMargin.TabIndex = 16;
            this.chbShowMargin.Text = "Display Margin";
            this.chbShowMargin.CheckedChanged += new System.EventHandler(this.ShowMarginCheckBox_CheckedChanged);
            this.chbShowMargin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowMarginCheckBox_MouseMove);
            // 
            // nudMarginPos
            // 
            this.nudMarginPos.Location = new System.Drawing.Point(123, 22);
            this.nudMarginPos.Name = "nudMarginPos";
            this.nudMarginPos.Size = new System.Drawing.Size(129, 20);
            this.nudMarginPos.TabIndex = 18;
            this.nudMarginPos.ValueChanged += new System.EventHandler(this.MarginPosNumeric_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbUserMarginBkColor);
            this.groupBox1.Controls.Add(this.cbUserMarginForeColor);
            this.groupBox1.Controls.Add(this.laUserMarginBkColor);
            this.groupBox1.Controls.Add(this.laUserMarginForeColor);
            this.groupBox1.Controls.Add(this.laUserMarginText);
            this.groupBox1.Controls.Add(this.tbUserMarginText);
            this.groupBox1.Controls.Add(this.laUserMarginWidth);
            this.groupBox1.Controls.Add(this.nudUserMarginWidth);
            this.groupBox1.Controls.Add(this.chbPaintUserMargin);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 132);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // laUserMarginBkColor
            // 
            this.laUserMarginBkColor.AutoSize = true;
            this.laUserMarginBkColor.Location = new System.Drawing.Point(22, 105);
            this.laUserMarginBkColor.Name = "laUserMarginBkColor";
            this.laUserMarginBkColor.Size = new System.Drawing.Size(62, 13);
            this.laUserMarginBkColor.TabIndex = 38;
            this.laUserMarginBkColor.Text = "Back Color:";
            // 
            // laUserMarginForeColor
            // 
            this.laUserMarginForeColor.AutoSize = true;
            this.laUserMarginForeColor.Location = new System.Drawing.Point(22, 77);
            this.laUserMarginForeColor.Name = "laUserMarginForeColor";
            this.laUserMarginForeColor.Size = new System.Drawing.Size(58, 13);
            this.laUserMarginForeColor.TabIndex = 36;
            this.laUserMarginForeColor.Text = "Fore Color:";
            // 
            // laUserMarginText
            // 
            this.laUserMarginText.AutoSize = true;
            this.laUserMarginText.Location = new System.Drawing.Point(22, 51);
            this.laUserMarginText.Name = "laUserMarginText";
            this.laUserMarginText.Size = new System.Drawing.Size(91, 13);
            this.laUserMarginText.TabIndex = 34;
            this.laUserMarginText.Text = "User Margin Text:";
            // 
            // tbUserMarginText
            // 
            this.tbUserMarginText.Location = new System.Drawing.Point(126, 48);
            this.tbUserMarginText.Name = "tbUserMarginText";
            this.tbUserMarginText.Size = new System.Drawing.Size(129, 20);
            this.tbUserMarginText.TabIndex = 33;
            this.tbUserMarginText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserMarginTextTextBox_KeyDown);
            this.tbUserMarginText.Leave += new System.EventHandler(this.UserMarginTextTextBox_Leave);
            this.tbUserMarginText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserMarginTextTextBox_MouseMove);
            // 
            // laUserMarginWidth
            // 
            this.laUserMarginWidth.AutoSize = true;
            this.laUserMarginWidth.Location = new System.Drawing.Point(22, 24);
            this.laUserMarginWidth.Name = "laUserMarginWidth";
            this.laUserMarginWidth.Size = new System.Drawing.Size(98, 13);
            this.laUserMarginWidth.TabIndex = 31;
            this.laUserMarginWidth.Text = "User Margin Width:";
            // 
            // nudUserMarginWidth
            // 
            this.nudUserMarginWidth.Location = new System.Drawing.Point(126, 22);
            this.nudUserMarginWidth.Name = "nudUserMarginWidth";
            this.nudUserMarginWidth.Size = new System.Drawing.Size(129, 20);
            this.nudUserMarginWidth.TabIndex = 32;
            this.nudUserMarginWidth.ValueChanged += new System.EventHandler(this.UserMarginWidthNumeric_ValueChanged);
            // 
            // chbPaintUserMargin
            // 
            this.chbPaintUserMargin.AutoSize = true;
            this.chbPaintUserMargin.Location = new System.Drawing.Point(7, 3);
            this.chbPaintUserMargin.Name = "chbPaintUserMargin";
            this.chbPaintUserMargin.Size = new System.Drawing.Size(120, 17);
            this.chbPaintUserMargin.TabIndex = 30;
            this.chbPaintUserMargin.Text = "Display User Margin";
            this.chbPaintUserMargin.CheckedChanged += new System.EventHandler(this.PaintUserMarginCheckBox_CheckedChanged);
            this.chbPaintUserMargin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PaintUserMarginCheckBox_MouseMove);
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(5, 5);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(657, 39);
            this.pnDescription.TabIndex = 8;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(657, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "Margin indicates a special column visually, while User Margin allows displaying c" +
    "ustom information associated with the lines.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbColumnsPenColor
            // 
            this.cbColumnsPenColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbColumnsPenColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColumnsPenColor.FormattingEnabled = true;
            this.cbColumnsPenColor.Location = new System.Drawing.Point(123, 102);
            this.cbColumnsPenColor.Name = "cbColumnsPenColor";
            this.cbColumnsPenColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbColumnsPenColor.Size = new System.Drawing.Size(129, 21);
            this.cbColumnsPenColor.TabIndex = 41;
            this.cbColumnsPenColor.SelectedIndexChanged += new System.EventHandler(this.ColumnsPenColorComboBox_SelectedIndexChanged);
            this.cbColumnsPenColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ColumnsPenColorComboBox_MouseMove);
            // 
            // cbMarginColor
            // 
            this.cbMarginColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbMarginColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMarginColor.FormattingEnabled = true;
            this.cbMarginColor.Location = new System.Drawing.Point(123, 48);
            this.cbMarginColor.Name = "cbMarginColor";
            this.cbMarginColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbMarginColor.Size = new System.Drawing.Size(129, 21);
            this.cbMarginColor.TabIndex = 40;
            this.cbMarginColor.SelectedIndexChanged += new System.EventHandler(this.MarginColorComboBox_SelectedIndexChanged);
            this.cbMarginColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MarginColorComboBox_MouseMove);
            // 
            // cbUserMarginBkColor
            // 
            this.cbUserMarginBkColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbUserMarginBkColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserMarginBkColor.FormattingEnabled = true;
            this.cbUserMarginBkColor.Location = new System.Drawing.Point(126, 102);
            this.cbUserMarginBkColor.Name = "cbUserMarginBkColor";
            this.cbUserMarginBkColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbUserMarginBkColor.Size = new System.Drawing.Size(129, 21);
            this.cbUserMarginBkColor.TabIndex = 40;
            this.cbUserMarginBkColor.SelectedIndexChanged += new System.EventHandler(this.UserMarginBkColorComboBox_SelectedIndexChanged);
            this.cbUserMarginBkColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserMarginBkColorComboBox_MouseMove);
            // 
            // cbUserMarginForeColor
            // 
            this.cbUserMarginForeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbUserMarginForeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserMarginForeColor.FormattingEnabled = true;
            this.cbUserMarginForeColor.Location = new System.Drawing.Point(126, 74);
            this.cbUserMarginForeColor.Name = "cbUserMarginForeColor";
            this.cbUserMarginForeColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbUserMarginForeColor.Size = new System.Drawing.Size(129, 21);
            this.cbUserMarginForeColor.TabIndex = 39;
            this.cbUserMarginForeColor.SelectedIndexChanged += new System.EventHandler(this.UserMarginForeColorComboBox_SelectedIndexChanged);
            this.cbUserMarginForeColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserMarginForeColorComboBox_MouseMove);
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.EditMargin.Position = 60;
            this.syntaxEdit1.EditMargin.Visible = true;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 181);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 5;
            this.syntaxEdit1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 440);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Margin";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMarginPos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUserMarginWidth)).EndInit();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label laUserMarginBkColor;
        private System.Windows.Forms.Label laUserMarginForeColor;
        private System.Windows.Forms.Label laUserMarginText;
        private System.Windows.Forms.TextBox tbUserMarginText;
        private System.Windows.Forms.Label laUserMarginWidth;
        private System.Windows.Forms.NumericUpDown nudUserMarginWidth;
        private System.Windows.Forms.CheckBox chbPaintUserMargin;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.Common.ColorBox cbUserMarginBkColor;
        private Alternet.Editor.Common.ColorBox cbUserMarginForeColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private Alternet.Editor.Common.ColorBox cbColumnsPenColor;
        private Alternet.Editor.Common.ColorBox cbMarginColor;
        private System.Windows.Forms.CheckBox chbColumnsVisible;
        private System.Windows.Forms.Label laColumnPenColor;
        private System.Windows.Forms.Label laMarginColor;
        private System.Windows.Forms.Label laMarginPos;
        private System.Windows.Forms.CheckBox chbShowMargin;
        private System.Windows.Forms.NumericUpDown nudMarginPos;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}