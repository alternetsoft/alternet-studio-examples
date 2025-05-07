namespace CustomizeToolbox
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Designer generated code.")]
    public partial class Form1
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
            this.pnDescription = new System.Windows.Forms.Panel();
            this.btLoad = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btCustomize = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.formDesignerControl1 = new Alternet.FormDesigner.WinForms.FormDesignerControl();
            this.toolboxControl = new CustomToolboxControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl)).BeginInit();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.btLoad);
            this.pnDescription.Controls.Add(this.btSave);
            this.pnDescription.Controls.Add(this.btCustomize);
            this.pnDescription.Controls.Add(this.btAdd);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(763, 68);
            this.pnDescription.TabIndex = 10;
            // 
            // btLoad
            // 
            this.btLoad.Location = new System.Drawing.Point(656, 39);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(95, 23);
            this.btLoad.TabIndex = 13;
            this.btLoad.Text = "Load Toolbox";
            this.btLoad.UseVisualStyleBackColor = true;
            this.btLoad.Click += new System.EventHandler(this.LoadButton_Click);
            this.btLoad.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadButton_MouseMove);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(536, 39);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(114, 23);
            this.btSave.TabIndex = 12;
            this.btSave.Text = "Save Toolbox";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.SaveButton_Click);
            this.btSave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SaveButton_MouseMove);
            // 
            // btCustomize
            // 
            this.btCustomize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCustomize.Location = new System.Drawing.Point(656, 7);
            this.btCustomize.Name = "btCustomize";
            this.btCustomize.Size = new System.Drawing.Size(95, 23);
            this.btCustomize.TabIndex = 11;
            this.btCustomize.Text = "Customize Tabs";
            this.btCustomize.UseVisualStyleBackColor = true;
            this.btCustomize.Click += new System.EventHandler(this.CustomizeButton_Click);
            this.btCustomize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CustomizeButton_MouseMove);
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.Location = new System.Drawing.Point(536, 7);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(114, 23);
            this.btAdd.TabIndex = 9;
            this.btAdd.Text = "Add New Library...";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.AddButton_Click);
            this.btAdd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AddButton_MouseMove);
            // 
            // laDescription
            // 
            this.laDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(763, 68);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "Toolbox control can be customized by adding new categories and toolbox items.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "toolBoxContent";
            this.openFileDialog1.Filter = "ToolBox content files (*.xml) |*.xml";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "toolBoxContent";
            this.saveFileDialog1.Filter = "ToolBox content files (*.xml) |*.xml";
            // 
            // formDesignerControl1
            // 
            this.formDesignerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formDesignerControl1.Location = new System.Drawing.Point(288, 68);
            this.formDesignerControl1.Name = "formDesignerControl1";
            this.formDesignerControl1.Size = new System.Drawing.Size(475, 341);
            this.formDesignerControl1.TabIndex = 16;
            this.formDesignerControl1.Text = "formDesignerControl1";
            this.formDesignerControl1.ToolboxControl = this.toolboxControl;
            // 
            // toolboxControl
            // 
            this.toolboxControl.AutoScroll = true;
            this.toolboxControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.toolboxControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolboxControl.FormDesignerControl = this.formDesignerControl1;
            this.toolboxControl.Location = new System.Drawing.Point(0, 68);
            this.toolboxControl.Name = "toolboxControl";
            this.toolboxControl.Size = new System.Drawing.Size(288, 341);
            this.toolboxControl.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 409);
            this.Controls.Add(this.formDesignerControl1);
            this.Controls.Add(this.toolboxControl);
            this.Controls.Add(this.pnDescription);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customize Toolbox";
            this.pnDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Button btCustomize;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Label laDescription;
        private CustomToolboxControl toolboxControl;
        private Alternet.FormDesigner.WinForms.FormDesignerControl formDesignerControl1;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}