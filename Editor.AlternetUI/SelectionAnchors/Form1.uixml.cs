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
using Alternet.Common;
using Alternet.Drawing;
using System.Drawing;
using Alternet.Common.AlternetUI;

namespace SelectionAnchors
{
    public partial class Form1 : Window
    {
        private readonly CsParser csParser1 = new(new CsSolution());

        static Form1()
        {
        }

        public Form1()
        {
            InitializeComponent();

            var eomResult = EditorOnMobileHelper.Initialize(syntaxEdit1);

            logListBox.BoundToApplicationLog = true;

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.KeyList.AddNormal(Keys.Alt | Keys.Divide, () =>
            {
                App.Log("Alt + / pressed");
                syntaxEdit1.ShowInsertableHint("InsertableText");
            });

            syntaxEdit1.KeyList.AddNormal(Keys.Alt | Keys.Shift | Keys.Divide, () =>
            {
                App.Log("Alt + Shift + / pressed");
                syntaxEdit1.ShowErrorHint("This is error text");
            });

            /*
            syntaxEdit1.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Shift || e.Key == Key.Alt)
                {
                    return;
                }

                App.Log($"{e.Key} / {e.KeyCode}");
            };
            */

            syntaxEdit1.EditMargin.Position = 80;
            syntaxEdit1.EditMargin.Visible = true;
            syntaxEdit1.Outlining.AllowOutlining = true;

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;

            syntaxEdit1.Touch += (s, e) =>
            {
                App.Log($"Touch {e.ActionType} at {e.Location}");
            };

            syntaxEdit1.Handler.EnableTouchEvents(TouchEventsMask.All);

            syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;

            syntaxEdit1.ContextMenuStrip = syntaxEdit1.DefaultMenu;

            Control.DefaultUseInternalContextMenu = false;

            if (DebugUtils.IsDebugDefinedAndAttached && AllowToggleInternalScrollBars)
            {
            }

            syntaxEdit1.Text = "Loading...";

            FormUtils.BindShown(this, () =>
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
            });
        }

        public bool AllowToggleInternalScrollBars { get; } = false;

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
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
