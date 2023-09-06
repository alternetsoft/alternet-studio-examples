#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.IO;
using System.Windows.Forms;

using Alternet.Syntax.Parsers.Roslyn;
using Customize.Dialogs;

namespace Customize
{
    public partial class Form1 : Form
    {
        private const string CustomizeDesc = "Display Editor options dialog";

        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";
        private SyntaxSettings globalSettings = new SyntaxSettings();
        private DlgSyntaxSettings options = new DlgSyntaxSettings();

        public Form1()
        {
            InitializeComponent();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            options.SyntaxSettings.Assign(globalSettings);
            if (options.ShowDialog() == DialogResult.OK)
            {
                globalSettings.Assign(options.SyntaxSettings);
                globalSettings.ApplyToEdit(syntaxEdit1);
            }
        }

        private void OptionsButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btOptions);
            if (str != CustomizeDesc)
                toolTip1.SetToolTip(btOptions, CustomizeDesc);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            globalSettings.LoadFromEdit(syntaxEdit1);

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);

            syntaxEdit1.Lexer = csParser1;
            string applicationGlobalSettingsFilePath = GetApplicationGlobalSettingsFilePath();
            if (File.Exists(applicationGlobalSettingsFilePath))
            {
                globalSettings.LoadFile(applicationGlobalSettingsFilePath);
                globalSettings.ApplyToEdit(syntaxEdit1);
            }
        }

        private string GetApplicationGlobalSettingsFilePath()
        {
            const string FolderName = "Alternet.Editor.Demo.GlobalSettings";
            const string FileName = "GlobalSettings.v9.xml";

            string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string p = Path.Combine(applicationDataPath, FolderName);

            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }

            return Path.Combine(p, FileName);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            globalSettings.SaveFile(GetApplicationGlobalSettingsFilePath());
        }
    }
}
