#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
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
            this.saveFileDialog.Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb";
        }

        public NewFormDialog(string location, string fileName, string[] supportedLanguages, int langIndex = 0)
            : this()
        {
            tbFormName.Text = Path.Combine(location, fileName);
            this.supportedLanguages = supportedLanguages;
            this.langIndex = langIndex;
        }

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

        public virtual string NamespaceName
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual string DesignedClassName
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual IList<string> Imports
        {
            get
            {
                return new List<string>();
            }
        }

        private void FillProjectLanguages()
        {
            foreach (string lang in supportedLanguages)
            {
                cbLanguage.Items.Add(lang);
            }

            cbLanguage.SelectedIndex = langIndex;
        }

        private void UpdateFileName()
        {
            if (!string.IsNullOrEmpty(FileLocation))
            {
                var filePath = Path.GetDirectoryName(FileLocation);
                var name = Path.GetFileNameWithoutExtension(FileLocation);
                switch (cbLanguage.SelectedIndex)
                {
                    case 0:
                        FileLocation = Path.Combine(filePath, name + ".cs");
                        break;
                    case 1:
                        FileLocation = Path.Combine(filePath, name + ".vb");
                        break;
                }
            }
        }

        private void DlgNewForm_Load(object sender, EventArgs e)
        {
            FillProjectLanguages();
        }

        private void LocationButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = Path.GetFileName(FileLocation);
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(FileLocation);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                tbFormName.Text = saveFileDialog.FileName;
        }

        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFileName();
        }
    }
}
