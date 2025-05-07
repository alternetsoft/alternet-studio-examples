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

using Customize.Dialogs.Classes;
using Customize.Dialogs;
using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace Customize
{
    public partial class Form1 : Window
    {
        private readonly Alternet.Editor.TextSource.TextSource csharpSource = new();
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

            syntaxEdit1.Source = csharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            csharpSource.OptimizedForMemory = false;

            globalSettings.LoadFromEdit(syntaxEdit1, false);
            globalSettings.ActiveTheme = syntaxEdit1.VisualThemeType;

            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            if (csharpSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                csharpSource.Lexer = csParser1;
                csharpSource.HighlightReferences = true;
            }

            btOptions.Click += OptionsButton_Click;

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
