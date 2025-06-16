#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace AlternetStudio.Demo
{
    public partial class NewFormDialog : Form
    {
        private string[] supportedLanguages;
        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private int langIndex = 0;

        public NewFormDialog()
        {
            InitializeComponent();
            this.saveFileDialog.Filter = "Python files (*.py) |*.py";
        }

        public NewFormDialog(string location, string fileName, string[] supportedLanguages, int langIndex = 0)
            : this()
        {
            tbFormName.Text = Path.Combine(location, fileName);
            this.supportedLanguages = supportedLanguages;
            this.langIndex = langIndex;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileLocation
        {
            get
            {
                return tbFormName.Text;
            }

            set
            {
                tbFormName.Text = value;
            }
        }

        private void DlgNewForm_Load(object sender, EventArgs e)
        {
        }

        private void LocationButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = FileLocation;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                tbFormName.Text = saveFileDialog.FileName;
        }
    }
}
