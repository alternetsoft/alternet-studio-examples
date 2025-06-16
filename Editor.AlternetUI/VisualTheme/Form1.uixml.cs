#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.IO;

using Alternet.UI;

using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace VisualTheme
{
    public partial class Form1 : Window
    {
        private CsParser csParser1 = new CsParser(new CsSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            InitEdit();
            syntaxEdit1.Outlining.AllowOutlining = true;

            Form1_Load(this, EventArgs.Empty);

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void InitEdit()
        {
            syntaxEdit1.Transparent = true;
            syntaxEdit1.Scrolling.Options |= ScrollingOptions.SystemScrollbars;

            syntaxEdit1.Outlining.AllowOutlining = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            if (textSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                textSource.Lexer = csParser1;
            }

            syntaxEdit1.HighlightReferences = true;
            InitializeVisualThemeComboBox();

            visualThemes.SelectedIndex = (int)syntaxEdit1.VisualThemeType;
            visualThemes.SelectedIndexChanged += VisualThemeComboBox_SelectedIndexChanged;
        }

        private void InitializeVisualThemeComboBox()
        {
            foreach (VisualThemeType value in Enum.GetValues(typeof(VisualThemeType)))
                visualThemes.Add(value.ToString());
        }

        private void VisualThemeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.VisualThemeType = (VisualThemeType?)visualThemes.SelectedIndex
                ?? VisualThemeType.None;
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
