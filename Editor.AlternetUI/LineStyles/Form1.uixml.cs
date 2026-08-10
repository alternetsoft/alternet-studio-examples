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

using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace LineStyles
{
    public partial class Form1 : Window
    {

        private readonly CsParser csParser1 = new(new CsSolution());

        private int traceLineStyleIndex;
        private int breakPointStyleIndex;
        private int customStyleIndex;

        private bool startDebug;
        private int startLine = 44;
        private int endLine = 0;
        private int index;

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;
            syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;

            var images = syntaxEdit1.Gutter.AlphaImages;
            var imagesHighDpi = syntaxEdit1.Gutter.AlphaImagesHighDpi;

            var isDark = syntaxEdit1.Gutter.BrushColor.IsDark();
            var image = KnownSvgImages.ImgGear.AsNormalImage(images.ImageSize.Height, isDark);
            var highDpiImage
                = KnownSvgImages.ImgGear.AsNormalImage(imagesHighDpi.ImageSize.Height, isDark);

            var gearImageIndex = syntaxEdit1.Gutter.AddImage(image, highDpiImage);

            syntaxEdit1.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            FileInfo fileInfo = new(DemoUtils.GetResourceFileFullPath(@"Editor/Text/c#.cs"));

            if (textSource.LoadOrAddNotFound(fileInfo.FullName))
            {
                textSource.Lexer = csParser1;
            }

            if (syntaxEdit1.Find("Main"))
            {
                syntaxEdit1.Selection.Clear();
                startLine = syntaxEdit1.Position.Y + 2;
            }

            IEditLineStyle traceLineStyle = new EditLineStyle
            {
                Name = "Trace Line",
                BackColor = Color.Black,
                ForeColor = LightDarkColors.Yellow,
                Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                ImageIndex = (int)KnownGutterImageIndex.TraceLine,
            };

            syntaxEdit1.LineStyles.Add(traceLineStyle);
            traceLineStyleIndex = syntaxEdit1.LineStyles.Count - 1;

            chbLineStyleBeyondEol.IsChecked = (LineStyleOptions.BeyondEol & traceLineStyle.Options) != 0;
            cbLineStyleColor.Value = traceLineStyle.ForeColor;

            var breakpointStyle = new EditLineStyle()
            {
                Name = "Breakpoint",
                BackColor = Color.White,
                ForeColor = Color.FromArgb(171, 97, 107),
                Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                ImageIndex = (int)KnownGutterImageIndex.Breakpoint,
            };
            syntaxEdit1.LineStyles.Add(breakpointStyle);
            breakPointStyleIndex = syntaxEdit1.LineStyles.Count - 1;

            endLine = syntaxEdit1.Lines.Count - 2;

            syntaxEdit1.GutterClick += SyntaxEdit1_GutterClick;
            cbLineStyleColor.ValueChanged += LineStyleColorComboBox_SelectedIndexChanged;
            chbLineStyleBeyondEol.CheckedChanged += LineStyleBeyondEolCheckBox_CheckedChanged;
            btSetBreakpoint.Click += SetBreakpointTextBoxButton_Click;
            btStepOver.Click += StepOverButton_Click;
            btStart.Click += StartButton_Click;

            var customLineStyle = new EditLineStyle
            {
                Name = "Custom Line Style",
                BackColor = Color.DarkOliveGreen,
                ForeColor = Color.White,
                Options = LineStyleOptions.BeyondEol,
                ImageIndex = gearImageIndex,
            };

            syntaxEdit1.LineStyles.Add(customLineStyle);
            customStyleIndex = syntaxEdit1.LineStyles.Count - 1;

            btSetCustom.Click += (s, e) =>
            {
                syntaxEdit1.Source.LineStyles.ToggleLineStyle(
                    syntaxEdit1.Position.Y,
                    0,
                    customStyleIndex);
            };

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void SyntaxEdit1_GutterClick(object? sender, EventArgs e)
        {
            SetBreakpoint();
        }

        private void Debug()
        {
            index = 0;
            btStart.Text = startDebug ? "Start" : "Stop";
            btStepOver.Enabled = !startDebug;
        }

        private void Start()
        {
            syntaxEdit1.Source.LineStyles.ToggleLineStyle(
                startLine + index,
                1,
                traceLineStyleIndex);
            syntaxEdit1.MakeVisible(new System.Drawing.Point(0, startLine + index));
            Debug();
            startDebug = !startDebug;
        }

        private void StepOver()
        {
            if (index < (endLine - startLine))
            {
                if (syntaxEdit1.Source.LineStyles.GetLineStyle(startLine + index) >= 0)
                {
                    syntaxEdit1.Source.LineStyles.ToggleLineStyle(
                        startLine + index,
                        1,
                        traceLineStyleIndex);
                }

                index++;
                while ((index < (endLine - startLine))
                    && (syntaxEdit1.Source.Lines[startLine + index].Trim() == string.Empty))
                    index++;

                syntaxEdit1.Source.LineStyles.ToggleLineStyle(
                    startLine + index,
                    1,
                    traceLineStyleIndex);
                syntaxEdit1.MakeVisible(new System.Drawing.Point(0, startLine + index));
            }
            else
            {
                syntaxEdit1.Source.LineStyles.ToggleLineStyle(
                    startLine + index,
                    1,
                    style: traceLineStyleIndex);
                Debug();
                startDebug = !startDebug;
            }
        }

        private void SetBreakpoint()
        {
            syntaxEdit1.Source.LineStyles.ToggleLineStyle(
                syntaxEdit1.Position.Y,
                0,
                breakPointStyleIndex);
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            Start();
        }

        private void StepOverButton_Click(object? sender, EventArgs e)
        {
            StepOver();
        }

        private void SetBreakpointTextBoxButton_Click(object? sender, EventArgs e)
        {
            SetBreakpoint();
        }

        private void LineStyleBeyondEolCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (syntaxEdit1.LineStyles.Count > 0)
            {
                foreach (IEditLineStyle lineStyle in syntaxEdit1.LineStyles)
                {
                    lineStyle.Options = chbLineStyleBeyondEol.IsChecked
                        ? lineStyle.Options | LineStyleOptions.BeyondEol :
                        lineStyle.Options & ~LineStyleOptions.BeyondEol;
                }

                syntaxEdit1.Invalidate();
            }
        }

        private void LineStyleColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (syntaxEdit1.LineStyles.Count > 0)
            {
                IEditLineStyle lineStyle = syntaxEdit1.LineStyles[traceLineStyleIndex];
                lineStyle.ForeColor = cbLineStyleColor.Value;
                syntaxEdit1.Invalidate();
            }
        }
    }
}