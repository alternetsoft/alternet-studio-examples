namespace AlternetStudio.Demo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Supress for Visual Studio-generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Designer generated code.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Designer generated code.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Some fields must be public")]
    partial class NewFormDialog
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
            this.laFormName = new System.Windows.Forms.Label();
            this.tbFormName = new System.Windows.Forms.TextBox();
            this.laLanguage = new System.Windows.Forms.Label();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btLocation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // laFormName
            // 
            this.laFormName.AutoSize = true;
            this.laFormName.Location = new System.Drawing.Point(12, 53);
            this.laFormName.Name = "laFormName";
            this.laFormName.Size = new System.Drawing.Size(48, 13);
            this.laFormName.TabIndex = 4;
            this.laFormName.Text = "Location";
            // 
            // tbFormName
            // 
            this.tbFormName.Location = new System.Drawing.Point(15, 70);
            this.tbFormName.Name = "tbFormName";
            this.tbFormName.Size = new System.Drawing.Size(361, 20);
            this.tbFormName.TabIndex = 5;
            // 
            // laLanguage
            // 
            this.laLanguage.AutoSize = true;
            this.laLanguage.Location = new System.Drawing.Point(12, 9);
            this.laLanguage.Name = "laLanguage";
            this.laLanguage.Size = new System.Drawing.Size(55, 13);
            this.laLanguage.TabIndex = 0;
            this.laLanguage.Text = "Language";
            // 
            // cbLanguage
            // 
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Location = new System.Drawing.Point(15, 26);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(411, 21);
            this.cbLanguage.TabIndex = 1;
            this.cbLanguage.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(270, 100);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 7;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(351, 100);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 8;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btLocation
            // 
            this.btLocation.Location = new System.Drawing.Point(382, 68);
            this.btLocation.Name = "btLocation";
            this.btLocation.Size = new System.Drawing.Size(44, 23);
            this.btLocation.TabIndex = 6;
            this.btLocation.Text = "...";
            this.btLocation.UseVisualStyleBackColor = true;
            this.btLocation.Click += new System.EventHandler(this.LocationButton_Click);
            // 
            // NewFormDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(443, 139);
            this.Controls.Add(this.laLanguage);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.laFormName);
            this.Controls.Add(this.tbFormName);
            this.Controls.Add(this.btLocation);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Name = "NewFormDialog";
            this.Text = "Add new Form";
            this.Load += new System.EventHandler(this.DlgNewForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label laFormName;
        public System.Windows.Forms.TextBox tbFormName;
        private System.Windows.Forms.Label laLanguage;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btLocation;
    }
}