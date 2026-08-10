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
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace WordWrap
{
    public partial class Form1 : Window
    {
        private readonly CsParser csParser1 = new(new CsSolution());

        public Form1()
        {
            try
            {
                InitializeComponent();

                if (CommandLineArgs.ParseAndGetIsDark())
                {
                    syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
                }

                syntaxEdit1.EditMargin.Position = 80;
                syntaxEdit1.EditMargin.Visible = true;
                syntaxEdit1.Outlining.AllowOutlining = true;

                lbDescription.WordWrap = true;

                FormUtils.BindShown(this, () =>
                {
                    Form1_Load(this, EventArgs.Empty);
                    ActiveControl = syntaxEdit1;
                });
            }
            finally
            {
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileInfo fileInfo = new(DemoUtils.GetResourceFileFullPath(@"Editor/Text/spell.txt"));

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            if (textSource.LoadOrAddNotFound(fileInfo.FullName))
            {
                textSource.Lexer = csParser1;
            }

            chbWordWrap.IsChecked = syntaxEdit1.WordWrap;
            chbWrapAtMargin.IsChecked = syntaxEdit1.WrapAtMargin;

            chbWordWrap.CheckedChanged += WordWrapCheckBox_CheckedChanged;
            chbWrapAtMargin.CheckedChanged += WrapAtMarginCheckBox_CheckedChanged;

            syntaxEdit1.ContextMenuStrip = syntaxEdit1.DefaultMenu;

            if (DebugUtils.IsDebugDefinedAndAttached)
            {
            }
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
