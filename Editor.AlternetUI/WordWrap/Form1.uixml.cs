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

namespace WordWrap
{
    public partial class Form1 : Window
    {
        private readonly CsParser csParser1 = new(new CsSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.EditMargin.Position = 80;
            syntaxEdit1.EditMargin.Visible = true;
            syntaxEdit1.Outlining.AllowOutlining = true;

            Idle += Form1_Idle;

            Form1_Load(this, EventArgs.Empty);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            if (textSource.LoadOrAddNotFound(dirInfo.FullName + @"spell.txt"))
            {
                textSource.Lexer = csParser1;
            }

            chbWordWrap.IsChecked = syntaxEdit1.WordWrap;
            chbWrapAtMargin.IsChecked = syntaxEdit1.WrapAtMargin;

            chbWordWrap.CheckedChanged += WordWrapCheckBox_CheckedChanged;
            chbWrapAtMargin.CheckedChanged += WrapAtMarginCheckBox_CheckedChanged;
        }

        private void WordWrapCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.WordWrap = chbWordWrap.IsChecked;
        }

        private void WrapAtMarginCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.WrapAtMargin = chbWrapAtMargin.IsChecked;
        }
    }
}
