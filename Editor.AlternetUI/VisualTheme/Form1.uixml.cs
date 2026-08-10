#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.IO;

using Alternet.UI;

using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.Drawing;

namespace VisualTheme
{
    public partial class Form1 : Window
    {
        private CsParser csParser1 = new CsParser(new CsSolution());
        private CustomVisualTheme customTheme = new ();

        public Form1()
        {
            InitializeComponent();

            syntaxEdit1.VisualTheme = customTheme;

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            InitEdit();
            syntaxEdit1.Outlining.AllowOutlining = true;

            Form1_Load(this, EventArgs.Empty);

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void InitEdit()
        {
            syntaxEdit1.Transparent = true;
            syntaxEdit1.Scrolling.Options |= ScrollingOptions.SystemScrollbars;

            syntaxEdit1.Outlining.AllowOutlining = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileInfo fileInfo = new(DemoUtils.GetResourceFileFullPath(@"Editor/Text/c#.cs"));

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            if (textSource.LoadOrAddNotFound(fileInfo.FullName))
            {
                textSource.Lexer = csParser1;
            }

            syntaxEdit1.HighlightReferences = true;

            visualThemes.EnumType = typeof(VisualThemeType);

            visualThemes.ExcludeValues = new object[]
            {
                VisualThemeType.None,
            };

            visualThemes.Value = syntaxEdit1.VisualThemeType;
            visualThemes.ValueChanged += VisualThemeComboBox_SelectedIndexChanged;
        }

        private void VisualThemeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.VisualThemeType
                = (VisualThemeType?)visualThemes.Value ?? VisualThemeType.Auto;
        }
    }

#pragma warning disable
    public class CustomVisualTheme : StandardVisualTheme
#pragma warning restore
    {
        public CustomVisualTheme()
            : base("MyCustomTheme")
        {
        }

        protected override VisualThemeColors GetColors()
        {
            var colors = DarkVisualTheme.Instance.Colors.Clone();
            colors.Reswords = LightDarkColors.Red.Dark;
            colors.WindowBackground = System.Drawing.Color.FromArgb(40, 40, 40);
            return colors;
        }
    }
}
