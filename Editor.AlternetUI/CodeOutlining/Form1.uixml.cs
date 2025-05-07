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
using Alternet.Editor.TextSource;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Common;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace CodeOutlining
{
    public partial class Form1 : Window
    {
        private TextSource? csharpSource = new();
        private TextSource? textSource2 = new();
        private CsParser? csParser1 = new(new CsSolution());
        private Parser? parser1 = new();
        private Timer? timer = new();

        public Form1()
        {
            InitializeComponent();
            cbAutomatics.Items.AddRange(
                [
                    "Automatic",
                    "Custom"
                ]
            );

            syntaxEdit1.Source = csharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            csharpSource.OptimizedForMemory = false;
            textSource2.OptimizedForMemory = false;

            if (CommandLineArgs.ParseAndGetIsDark())
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;

            Form1_Load(this, EventArgs.Empty);

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            SafeDispose(ref timer);
            syntaxEdit1.Source = null;
            if (csharpSource is not null)
                csharpSource.Lexer = null;
            if(textSource2 is not null)
                textSource2.Lexer = null;
            SafeDispose(ref csharpSource);
            SafeDispose(ref textSource2);
            SafeDispose(ref csParser1);
            SafeDispose(ref parser1);

            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/");

            var cSharpLoaded
                = csharpSource?.LoadOrAddNotFound(dirInfo.FullName + @"Text/c#.cs") ?? false;

            var customLoaded
                = textSource2?.LoadOrAddNotFound(dirInfo.FullName + @"Text/customOutlining.txt")
                ?? false;

            var fileInfo = new FileInfo(dirInfo.FullName + @"Schemes/ini.xml");
            if (fileInfo.Exists && parser1 is not null)
            {
                parser1.Scheme.LoadFile(fileInfo.FullName);

                parser1.Scheme.MakeForeColorLighterLighterIfDark(syntaxEdit1);
            }

            if (csharpSource is not null)
            {
                csharpSource.Lexer = csParser1;
                csharpSource.HighlightReferences = true;
            }

            if (textSource2 is not null && customLoaded)
                textSource2.Lexer = parser1;

            if(cSharpLoaded)
                syntaxEdit1.Source = csharpSource;

            cbAutomatics.SelectedIndex = 0;
            chbAllowOutlining.IsChecked = syntaxEdit1.Outlining.AllowOutlining;
            chbDrawOnGutter.IsChecked
                = (OutlineOptions.DrawOnGutter & syntaxEdit1.Outlining.OutlineOptions) != 0;
            chbDrawLines.IsChecked
                = (OutlineOptions.DrawLines & syntaxEdit1.Outlining.OutlineOptions) != 0;
            chbDrawButtons.IsChecked
                = (OutlineOptions.DrawButtons & syntaxEdit1.Outlining.OutlineOptions) != 0;
            chbShowHints.IsChecked = (OutlineOptions.ShowHints & syntaxEdit1.Outlining.OutlineOptions)
                != 0;

            chbAllowOutlining.CheckedChanged += AllowOutliningCheckBox_CheckedChanged;
            chbDrawOnGutter.CheckedChanged += DrawOnGutterCheckBox_CheckedChanged;
            chbDrawLines.CheckedChanged += DrawLinesCheckBox_CheckedChanged;
            chbDrawButtons.CheckedChanged += DrawButtonsCheckBox_CheckedChanged;
            chbShowHints.CheckedChanged += ShowHintsCheckBox_CheckedChanged;
            cbAutomatics.SelectedIndexChanged += AutomaticComboBox_SelectedIndexChanged;
            syntaxEdit1.SourceStateChanged += SyntaxEdit1_SourceStateChanged;

            if(timer is not null)
            {
                timer.Interval = 500;
                timer.Tick += new EventHandler(DoTimer);
            }
        }

        private void SyntaxEdit1_SourceStateChanged(object? sender, NotifyEventArgs e)
        {
            object source1 = (object)syntaxEdit1.Source;
            if (source1.Equals(textSource2))
            {
                if ((e.State & NotifyState.Edit) != 0)
                    timer?.Start();
            }
        }

        private void AutomaticComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateOutlining(cbAutomatics.SelectedIndex == 0);
        }

        private void ShowHintsCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbShowHints.IsChecked
                ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.ShowHints
                : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.ShowHints;
        }

        private void DrawButtonsCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbDrawButtons.IsChecked
                ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.DrawButtons
                : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawButtons;
        }

        private void DrawLinesCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbDrawLines.IsChecked
                ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.DrawLines
                : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawLines;
        }

        private void DrawOnGutterCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbDrawOnGutter.IsChecked
                ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.DrawOnGutter
            : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawOnGutter;
        }

        private void AllowOutliningCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Outlining.AllowOutlining = chbAllowOutlining.IsChecked;
            if (chbAllowOutlining.IsChecked && (cbAutomatics.SelectedIndex != 0))
                DoCustomOutlining();
        }

        private void DoTimer(object? sender, EventArgs e)
        {
            DoCustomOutlining();
            timer?.Stop();
        }

        private Point PrevPosition(Point position)
        {
            Point pos = position;
            if (pos.Y > 0)
            {
                pos.Y--;
                pos.X = Math.Max(0, syntaxEdit1.Strings[pos.Y].Length - 1);
            }
            else
                pos.X--;
            return pos;
        }

        private void DoCustomOutlining()
        {
            Point oldPos = syntaxEdit1.Position;
            syntaxEdit1.Source.BeginUpdate();
            try
            {
#pragma warning disable
                Regex regexp = new(
                    @"\[.*\]",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
#pragma warning restore

                if (syntaxEdit1.Find(
                    @"\[.*\]",
                    SearchOptions.EntireScope | SearchOptions.RegularExpressions, regexp))
                {
                    var ranges = new List<IRange>();
                    Point start = syntaxEdit1.Position;
                    while (syntaxEdit1.FindNext())
                    {
                        ranges.Add(new OutlineRange(
                            start,
                            PrevPosition(syntaxEdit1.Position),
                            0,
                            "..."));
                        start = syntaxEdit1.Position;
                    }

                    var pt = new Point(
                        syntaxEdit1.Lines[syntaxEdit1.Lines.Count - 1].Length,
                        syntaxEdit1.Lines.Count - 1);

                    ranges.Add(new OutlineRange(start, pt , 0, "..."));
                    syntaxEdit1.Outlining.SetOutlineRanges(ranges, true);
                }

                syntaxEdit1.Selection.Clear();
            }
            finally
            {
                syntaxEdit1.MoveTo(oldPos);
                syntaxEdit1.Source.EndUpdate();
            }
        }

        private void UpdateOutlining(bool automatic)
        {
            syntaxEdit1.Source = automatic ? csharpSource : textSource2;
            if (!automatic)
                DoCustomOutlining();
        }
    }
}
