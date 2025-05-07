namespace AlternetStudio.Demo
{
    public partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.tbCompanyInfo = new System.Windows.Forms.TextBox();
            this.btClose = new System.Windows.Forms.Button();
            this.laMailTo = new System.Windows.Forms.Label();
            this.laEMail = new System.Windows.Forms.Label();
            this.laAdress = new System.Windows.Forms.Label();
            this.laWWW = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbCompanyInfo
            // 
            this.tbCompanyInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tbCompanyInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCompanyInfo.Location = new System.Drawing.Point(12, 89);
            this.tbCompanyInfo.Multiline = true;
            this.tbCompanyInfo.Name = "tbCompanyInfo";
            this.tbCompanyInfo.Size = new System.Drawing.Size(400, 106);
            this.tbCompanyInfo.TabIndex = 13;
            this.tbCompanyInfo.Text = resources.GetString("tbCompanyInfo.Text");
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(325, 202);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 12;
            this.btClose.Text = "Close";
            this.btClose.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // laMailTo
            // 
            this.laMailTo.AutoSize = true;
            this.laMailTo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.laMailTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMailTo.ForeColor = System.Drawing.Color.Blue;
            this.laMailTo.Location = new System.Drawing.Point(260, 44);
            this.laMailTo.Name = "laMailTo";
            this.laMailTo.Size = new System.Drawing.Size(129, 13);
            this.laMailTo.TabIndex = 11;
            this.laMailTo.Text = "contact@alternetsoft.com";
            this.laMailTo.Click += new System.EventHandler(this.MailToLabel_Click);
            // 
            // laEMail
            // 
            this.laEMail.AutoSize = true;
            this.laEMail.Location = new System.Drawing.Point(212, 44);
            this.laEMail.Name = "laEMail";
            this.laEMail.Size = new System.Drawing.Size(37, 13);
            this.laEMail.TabIndex = 10;
            this.laEMail.Text = "e-mail:";
            // 
            // laAdress
            // 
            this.laAdress.AutoSize = true;
            this.laAdress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.laAdress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laAdress.ForeColor = System.Drawing.Color.Blue;
            this.laAdress.Location = new System.Drawing.Point(260, 20);
            this.laAdress.Name = "laAdress";
            this.laAdress.Size = new System.Drawing.Size(140, 13);
            this.laAdress.TabIndex = 9;
            this.laAdress.Text = "http://www.alternetsoft.com";
            this.laAdress.Click += new System.EventHandler(this.AdressLabel_Click);
            // 
            // laWWW
            // 
            this.laWWW.AutoSize = true;
            this.laWWW.Location = new System.Drawing.Point(212, 20);
            this.laWWW.Name = "laWWW";
            this.laWWW.Size = new System.Drawing.Size(43, 13);
            this.laWWW.TabIndex = 8;
            this.laWWW.Text = "WWW:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(110, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 258);
            this.Controls.Add(this.tbCompanyInfo);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.laMailTo);
            this.Controls.Add(this.laEMail);
            this.Controls.Add(this.laAdress);
            this.Controls.Add(this.laWWW);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AboutBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About this demo";
            this.Load += new System.EventHandler(this.CompanyInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCompanyInfo;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label laMailTo;
        private System.Windows.Forms.Label laEMail;
        private System.Windows.Forms.Label laAdress;
        private System.Windows.Forms.Label laWWW;
        private System.Windows.Forms.PictureBox pictureBox1;

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
    }
}
