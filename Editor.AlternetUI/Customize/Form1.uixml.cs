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

using Alternet.Editor.CustomizeDialog.AlternetUI;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace Customize
{
    public partial class Form1 : Window
    {
        private readonly TextSource cSharpSource = new();
        private readonly CsParser csParser1 = new(new CsSolution());

        private SyntaxSettings globalSettings = new();
        private DlgSyntaxSettings? options;

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }
            else
            {
                if(IsDarkBackground)
                    syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
                else
                    syntaxEdit1.VisualThemeType = VisualThemeType.Light;
            }

            syntaxEdit1.Source = cSharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            cSharpSource.OptimizedForMemory = false;

            globalSettings.LoadFromEdit(syntaxEdit1, false);
            globalSettings.ActiveTheme = syntaxEdit1.VisualThemeType;

            FileInfo fileInfo = new(DemoUtils.GetResourceFileFullPath(@"Editor/Text/c#.cs"));

            btOptions.Click += OptionsButton_Click;

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;

            syntaxEdit1.Text = "Loading text...";

            FormUtils.BindShown(this, () =>
            {
                if (cSharpSource.LoadOrAddNotFound(fileInfo.FullName))
                {
                    cSharpSource.Lexer = csParser1;
                    cSharpSource.HighlightReferences = true;
                }
            });
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void OptionsButton_Click(object? sender, EventArgs e)
        {
            if(options is null)
            {
                options = new()
                {
                    StartLocation = WindowStartLocation.CenterScreen,
                    CloseAction = WindowCloseAction.Hide,
                };

                options.Disposed += (s, e) =>
                {
                    options = null;
                };

                void Apply()
                {
                    globalSettings.Assign(options.SyntaxSettings);
                    globalSettings.ApplyToEdit(syntaxEdit1);
                }

                options.ApplyButtonClick += (s, e) =>
                {
                    Apply();
                };

                options.OkButtonClick += (s, e) =>
                {
                    Apply();
                    options.Close();
                };

                options.ButtonCancel.ClickAction = () =>
                {
                    options.Close();
                };
            }

            options.SyntaxSettings.Assign(globalSettings);
            options.ShowAndFocus();
        }
    }
}
