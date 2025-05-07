#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;

namespace VisualTheme
{
    public partial class Form1 : Form
    {
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "VisualTheme.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;
            csharpSource.HighlightReferences = true;
            InitializeVisualThemeComboBox();
            syntaxEdit1.VisualTheme = new CustomVisualTheme();
            syntaxEdit1.Source = csharpSource;
            syntaxEdit1.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
        }

        private void InitializeVisualThemeComboBox()
        {
            visualThemeComboBox.DataSource = Enum.GetValues(typeof(VisualThemeType));
            visualThemeComboBox.SelectedItem = VisualThemeType.None;
        }

        private void VisualThemeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.VisualThemeType = (VisualThemeType)visualThemeComboBox.SelectedItem;
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class CustomVisualTheme : StandardVisualTheme
#pragma warning restore SA1402 // File may only contain a single type
    {
        public CustomVisualTheme()
            : base("MyCustomTheme")
        {
        }

        protected override VisualThemeColors GetColors()
        {
            var colors = DarkVisualTheme.Instance.Colors.Clone();
            colors.Reswords = System.Drawing.Color.Red;
            colors.WindowBackground = System.Drawing.Color.FromArgb(40, 40, 40);
            return colors;
        }
    }
}
