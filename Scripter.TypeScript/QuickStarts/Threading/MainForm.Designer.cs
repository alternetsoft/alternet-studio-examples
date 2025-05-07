namespace Threading.TypeScript
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1120:CommentsMustContainText", Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Implementing interface method in the separate region")]
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnDescription = new System.Windows.Forms.Panel();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.laLanguages = new System.Windows.Forms.Label();
            this.laDescription = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btStartSorting = new System.Windows.Forms.Button();
            this.pnQuickSort = new System.Windows.Forms.Panel();
            this.pnSelectionSort = new System.Windows.Forms.Panel();
            this.laQuickSort = new System.Windows.Forms.Label();
            this.laSelectionSort = new System.Windows.Forms.Label();
            this.laBubbleSort = new System.Windows.Forms.Label();
            this.pnBubbleSort = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.cbLanguages);
            this.pnDescription.Controls.Add(this.laLanguages);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(624, 39);
            this.pnDescription.TabIndex = 16;
            // 
            // cbLanguages
            // 
            this.cbLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.Items.AddRange(new object[] {
            "TypeScript",
            "JavaScript"});
            this.cbLanguages.Location = new System.Drawing.Point(517, 15);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(98, 21);
            this.cbLanguages.TabIndex = 16;
            this.cbLanguages.SelectedIndexChanged += new System.EventHandler(this.LanguagesComboBox_SelectedIndexChanged);
            this.cbLanguages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LanguagesComboBox_MouseMove);
            // 
            // laLanguages
            // 
            this.laLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laLanguages.AutoSize = true;
            this.laLanguages.Location = new System.Drawing.Point(452, 18);
            this.laLanguages.Name = "laLanguages";
            this.laLanguages.Size = new System.Drawing.Size(58, 13);
            this.laLanguages.TabIndex = 15;
            this.laLanguages.Text = "Language:";
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(624, 39);
            this.laDescription.TabIndex = 3;
            this.laDescription.Text = "This demo shows how to script methods asynchronously.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btCancel
            // 
            this.btCancel.Enabled = false;
            this.btCancel.Location = new System.Drawing.Point(537, 343);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 24;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // btStartSorting
            // 
            this.btStartSorting.Location = new System.Drawing.Point(456, 343);
            this.btStartSorting.Name = "btStartSorting";
            this.btStartSorting.Size = new System.Drawing.Size(75, 23);
            this.btStartSorting.TabIndex = 23;
            this.btStartSorting.Text = "Start Sorting";
            this.btStartSorting.Click += new System.EventHandler(this.StartSortingButton_Click);
            // 
            // pnQuickSort
            // 
            this.pnQuickSort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnQuickSort.Location = new System.Drawing.Point(452, 73);
            this.pnQuickSort.Name = "pnQuickSort";
            this.pnQuickSort.Size = new System.Drawing.Size(160, 264);
            this.pnQuickSort.TabIndex = 22;
            this.pnQuickSort.Paint += new System.Windows.Forms.PaintEventHandler(this.QuickSortPanel_Paint);
            // 
            // pnSelectionSort
            // 
            this.pnSelectionSort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnSelectionSort.Location = new System.Drawing.Point(228, 73);
            this.pnSelectionSort.Name = "pnSelectionSort";
            this.pnSelectionSort.Size = new System.Drawing.Size(160, 264);
            this.pnSelectionSort.TabIndex = 21;
            this.pnSelectionSort.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectionSortPanel_Paint);
            // 
            // laQuickSort
            // 
            this.laQuickSort.AutoSize = true;
            this.laQuickSort.Location = new System.Drawing.Point(452, 41);
            this.laQuickSort.Name = "laQuickSort";
            this.laQuickSort.Size = new System.Drawing.Size(57, 13);
            this.laQuickSort.TabIndex = 20;
            this.laQuickSort.Text = "Quick Sort";
            // 
            // laSelectionSort
            // 
            this.laSelectionSort.AutoSize = true;
            this.laSelectionSort.Location = new System.Drawing.Point(228, 41);
            this.laSelectionSort.Name = "laSelectionSort";
            this.laSelectionSort.Size = new System.Drawing.Size(73, 13);
            this.laSelectionSort.TabIndex = 19;
            this.laSelectionSort.Text = "Selection Sort";
            // 
            // laBubbleSort
            // 
            this.laBubbleSort.AutoSize = true;
            this.laBubbleSort.Location = new System.Drawing.Point(13, 41);
            this.laBubbleSort.Name = "laBubbleSort";
            this.laBubbleSort.Size = new System.Drawing.Size(62, 13);
            this.laBubbleSort.TabIndex = 18;
            this.laBubbleSort.Text = "Bubble Sort";
            // 
            // pnBubbleSort
            // 
            this.pnBubbleSort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnBubbleSort.Location = new System.Drawing.Point(13, 73);
            this.pnBubbleSort.Name = "pnBubbleSort";
            this.pnBubbleSort.Size = new System.Drawing.Size(160, 264);
            this.pnBubbleSort.TabIndex = 17;
            this.pnBubbleSort.Paint += new System.Windows.Forms.PaintEventHandler(this.BubbleSortPanel_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btStartSorting);
            this.Controls.Add(this.pnQuickSort);
            this.Controls.Add(this.pnSelectionSort);
            this.Controls.Add(this.laQuickSort);
            this.Controls.Add(this.laSelectionSort);
            this.Controls.Add(this.laBubbleSort);
            this.Controls.Add(this.pnBubbleSort);
            this.Controls.Add(this.pnDescription);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Threading - TypeScript";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.pnDescription.ResumeLayout(false);
            this.pnDescription.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btStartSorting;
        private System.Windows.Forms.Panel pnQuickSort;
        private System.Windows.Forms.Panel pnSelectionSort;
        private System.Windows.Forms.Label laQuickSort;
        private System.Windows.Forms.Label laSelectionSort;
        private System.Windows.Forms.Label laBubbleSort;
        private System.Windows.Forms.Panel pnBubbleSort;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label laLanguages;
    }
}