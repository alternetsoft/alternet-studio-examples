#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.IO;
using System.Windows.Forms;

using Alternet.FormDesigner.WinForms;

namespace LoadAndSave
{
    public partial class Form1 : Form
    {
        private string fileName = string.Empty;
        private bool modified = false;
        private FormDesignerDataSource source = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string folder = GetTestFilesDirectoryPath();
            if (Directory.Exists(folder))
            {
                var designerFiles = Directory.GetFiles(folder, "*.Designer.*", SearchOption.AllDirectories);

                foreach (var designerFile in designerFiles)
                {
                    var extension = Path.GetExtension(designerFile);

                    var userCodeFile = designerFile.Replace(".Designer" + extension, extension);
                    OpenAllFormFiles(userCodeFile);
                    UpdateFormTitle();
                    break;
                }
            }

            formDesignerControl1.DesignedContentChanged += OnDesignedContentChanged;
        }

        private void OnDesignedContentChanged(object sender, EventArgs e)
        {
            modified = true;
            UpdateFormTitle();
        }

        private void OpenDesigner(string fileName)
        {
            this.fileName = Path.GetFileName(fileName);
            formDesignerControl1.ReferencedAssemblies = GetReferencedAssemblies(fileName);
            formDesignerControl1.ImportedNamespaces = GetImportedNamespaces(fileName);
            formDesignerControl1.Source = source;
        }

        private FormDesignerDataSource GetDesignerSource(string formId)
        {
            FormDesignerDataSource ds;
            ds = new FormDesignerDataSource(formId);

            return ds;
        }

        private DesignerReferencedAssemblies GetReferencedAssemblies(string fileName)
        {
            return Path.GetExtension(fileName).ToLower().Equals(".vb") ? DesignerReferencedAssemblies.DefaultForVisualBasic : DesignerReferencedAssemblies.DefaultForCSharp;
        }

        private DesignerImportedNamespaces GetImportedNamespaces(string fileName)
        {
            if (!Path.GetExtension(fileName).ToLower().Equals(".vb"))
                return null;

            return DesignerImportedNamespaces.DefaultForVisualBasic;
        }

        private string GetTestFilesDirectoryPath()
        {
            const string Subdirectory = @"Resources\Designer\WinForms";

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directory = Path.Combine(baseDirectory, Subdirectory);

            if (!Directory.Exists(directory))
                directory = Path.Combine(Path.GetFullPath(baseDirectory.TrimEnd('\\') + @"\..\..\..\..\..\..\"), Subdirectory);

            return directory;
        }

        private void SaveDesignerFiles(IFormDesignerControl designer)
        {
            designer.Save();
            modified = false;
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            string title = modified ? string.Format("Form Designer - {0} *", fileName) : string.Format("Form Designer - {0}", fileName);
            Text = title;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "C# Files|*.Designer.cs|VB.NET Files|*.Designer.vb|All Files|*.*",
                Multiselect = false,
                InitialDirectory = GetTestFilesDirectoryPath(),
            };

            var result = dialog.ShowDialog(this);

            if (result != DialogResult.OK)
                return;

            var designerFileName = dialog.FileName;

            if (!designerFileName.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase) &&
                !designerFileName.EndsWith(".Designer.vb", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The file must have an extension .Designer.cs or .Designer.vb");
                return;
            }

            var extension = Path.GetExtension(designerFileName);
            var userCodeFile = designerFileName.Replace(".Designer" + extension, extension);
            OpenAllFormFiles(userCodeFile);
        }

        private void OpenAllFormFiles(string userCodeFile)
        {
            source = GetDesignerSource(userCodeFile);

            OpenDesigner(userCodeFile);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveDesignerFiles(formDesignerControl1);
        }
    }
}
