namespace Threading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBubble = new System.Windows.Forms.Button();
            this.ButtonBubbleSort = new System.Windows.Forms.Button();
            this.laBubbleSort = new System.Windows.Forms.Label();
            this.pnBubbleSort = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CancelSelection = new System.Windows.Forms.Button();
            this.ButtonSelectionSort = new System.Windows.Forms.Button();
            this.pnSelectionSort = new System.Windows.Forms.Panel();
            this.laSelectionSort = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.CancelQuick = new System.Windows.Forms.Button();
            this.pnQuickSort = new System.Windows.Forms.Panel();
            this.laQuickSort = new System.Windows.Forms.Label();
            this.ButtonQuickSort = new System.Windows.Forms.Button();
            this.scriptRun = new Alternet.Scripter.Python.ScriptRun(this.components);
            this.pnDescription.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(624, 39);
            this.pnDescription.TabIndex = 15;
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
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBubble);
            this.panel1.Controls.Add(this.ButtonBubbleSort);
            this.panel1.Controls.Add(this.laBubbleSort);
            this.panel1.Controls.Add(this.pnBubbleSort);
            this.panel1.Location = new System.Drawing.Point(12, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 362);
            this.panel1.TabIndex = 1;
            // 
            // CancelBubble
            // 
            this.CancelBubble.Enabled = false;
            this.CancelBubble.Location = new System.Drawing.Point(105, 320);
            this.CancelBubble.Name = "CancelBubble";
            this.CancelBubble.Size = new System.Drawing.Size(75, 23);
            this.CancelBubble.TabIndex = 14;
            this.CancelBubble.Text = "Cancel";
            this.CancelBubble.UseVisualStyleBackColor = true;
            this.CancelBubble.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ButtonBubbleSort
            // 
            this.ButtonBubbleSort.Location = new System.Drawing.Point(24, 320);
            this.ButtonBubbleSort.Name = "ButtonBubbleSort";
            this.ButtonBubbleSort.Size = new System.Drawing.Size(75, 23);
            this.ButtonBubbleSort.TabIndex = 12;
            this.ButtonBubbleSort.Text = "Start";
            this.ButtonBubbleSort.Click += new System.EventHandler(this.ButtonBubbleSort_Click);
            // 
            // laBubbleSort
            // 
            this.laBubbleSort.AutoSize = true;
            this.laBubbleSort.Location = new System.Drawing.Point(20, 18);
            this.laBubbleSort.Name = "laBubbleSort";
            this.laBubbleSort.Size = new System.Drawing.Size(62, 13);
            this.laBubbleSort.TabIndex = 11;
            this.laBubbleSort.Text = "Bubble Sort";
            // 
            // pnBubbleSort
            // 
            this.pnBubbleSort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnBubbleSort.Location = new System.Drawing.Point(20, 50);
            this.pnBubbleSort.Name = "pnBubbleSort";
            this.pnBubbleSort.Size = new System.Drawing.Size(160, 264);
            this.pnBubbleSort.TabIndex = 13;
            this.pnBubbleSort.Paint += new System.Windows.Forms.PaintEventHandler(this.BubbleSortPanel_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CancelSelection);
            this.panel2.Controls.Add(this.ButtonSelectionSort);
            this.panel2.Controls.Add(this.pnSelectionSort);
            this.panel2.Controls.Add(this.laSelectionSort);
            this.panel2.Location = new System.Drawing.Point(218, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 362);
            this.panel2.TabIndex = 2;
            // 
            // CancelSelection
            // 
            this.CancelSelection.Enabled = false;
            this.CancelSelection.Location = new System.Drawing.Point(105, 320);
            this.CancelSelection.Name = "CancelSelection";
            this.CancelSelection.Size = new System.Drawing.Size(75, 23);
            this.CancelSelection.TabIndex = 17;
            this.CancelSelection.Text = "Cancel";
            this.CancelSelection.UseVisualStyleBackColor = true;
            this.CancelSelection.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ButtonSelectionSort
            // 
            this.ButtonSelectionSort.Location = new System.Drawing.Point(18, 320);
            this.ButtonSelectionSort.Name = "ButtonSelectionSort";
            this.ButtonSelectionSort.Size = new System.Drawing.Size(75, 23);
            this.ButtonSelectionSort.TabIndex = 15;
            this.ButtonSelectionSort.Text = "Start";
            this.ButtonSelectionSort.Click += new System.EventHandler(this.ButtonSelectionSort_Click);
            // 
            // pnSelectionSort
            // 
            this.pnSelectionSort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnSelectionSort.Location = new System.Drawing.Point(20, 50);
            this.pnSelectionSort.Name = "pnSelectionSort";
            this.pnSelectionSort.Size = new System.Drawing.Size(160, 264);
            this.pnSelectionSort.TabIndex = 16;
            this.pnSelectionSort.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectionSortPanel_Paint);
            // 
            // laSelectionSort
            // 
            this.laSelectionSort.AutoSize = true;
            this.laSelectionSort.Location = new System.Drawing.Point(20, 18);
            this.laSelectionSort.Name = "laSelectionSort";
            this.laSelectionSort.Size = new System.Drawing.Size(73, 13);
            this.laSelectionSort.TabIndex = 12;
            this.laSelectionSort.Text = "Selection Sort";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.CancelQuick);
            this.panel3.Controls.Add(this.pnQuickSort);
            this.panel3.Controls.Add(this.laQuickSort);
            this.panel3.Controls.Add(this.ButtonQuickSort);
            this.panel3.Location = new System.Drawing.Point(424, 45);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 362);
            this.panel3.TabIndex = 3;
            // 
            // CancelQuick
            // 
            this.CancelQuick.Enabled = false;
            this.CancelQuick.Location = new System.Drawing.Point(105, 320);
            this.CancelQuick.Name = "CancelQuick";
            this.CancelQuick.Size = new System.Drawing.Size(75, 23);
            this.CancelQuick.TabIndex = 20;
            this.CancelQuick.Text = "Cancel";
            this.CancelQuick.UseVisualStyleBackColor = true;
            this.CancelQuick.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // pnQuickSort
            // 
            this.pnQuickSort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnQuickSort.Location = new System.Drawing.Point(20, 50);
            this.pnQuickSort.Name = "pnQuickSort";
            this.pnQuickSort.Size = new System.Drawing.Size(160, 264);
            this.pnQuickSort.TabIndex = 19;
            this.pnQuickSort.Paint += new System.Windows.Forms.PaintEventHandler(this.QuickSortPanel_Paint);
            // 
            // laQuickSort
            // 
            this.laQuickSort.AutoSize = true;
            this.laQuickSort.Location = new System.Drawing.Point(20, 18);
            this.laQuickSort.Name = "laQuickSort";
            this.laQuickSort.Size = new System.Drawing.Size(57, 13);
            this.laQuickSort.TabIndex = 13;
            this.laQuickSort.Text = "Quick Sort";
            // 
            // ButtonQuickSort
            // 
            this.ButtonQuickSort.Location = new System.Drawing.Point(20, 320);
            this.ButtonQuickSort.Name = "ButtonQuickSort";
            this.ButtonQuickSort.Size = new System.Drawing.Size(75, 23);
            this.ButtonQuickSort.TabIndex = 18;
            this.ButtonQuickSort.Text = "Start";
            this.ButtonQuickSort.Click += new System.EventHandler(this.StartQuickSortingButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnDescription);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sorting";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnDescription.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Alternet.Scripter.Python.ScriptRun scriptRun;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBubble;
        private System.Windows.Forms.Button ButtonBubbleSort;
        private System.Windows.Forms.Label laBubbleSort;
        private System.Windows.Forms.Panel pnBubbleSort;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button CancelSelection;
        private System.Windows.Forms.Button ButtonSelectionSort;
        private System.Windows.Forms.Panel pnSelectionSort;
        private System.Windows.Forms.Label laSelectionSort;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button CancelQuick;
        private System.Windows.Forms.Panel pnQuickSort;
        private System.Windows.Forms.Label laQuickSort;
        private System.Windows.Forms.Button ButtonQuickSort;
    }
}