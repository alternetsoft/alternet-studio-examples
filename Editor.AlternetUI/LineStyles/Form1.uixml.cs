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
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace LineStyles
{
    public partial class Form1 : Window
    {
        private readonly CsParser csParser1 = new(new CsSolution());

        private bool startDebug;
        private int startLine = 44;
        private int endLine = 0;
        private int index;

        private enum KnownImageIndex
        {
            Breakpoint = 11,

            TraceLine,
        }

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

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

        private void Form1_Load(object sender, EventArgs e)
        {
            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            if (textSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                textSource.Lexer = csParser1;
            }

            if (syntaxEdit1.Find("Main"))
            {
                syntaxEdit1.Selection.Clear();
                startLine = syntaxEdit1.Position.Y + 2;
            }

            IEditLineStyle lineStyle = new EditLineStyle();
            lineStyle.BackColor = Color.Black;
            lineStyle.ForeColor = Color.Yellow;
            lineStyle.Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors;

            lineStyle.ImageIndex = (int)KnownImageIndex.TraceLine;
            chbLineStyleBeyondEol.IsChecked = (LineStyleOptions.BeyondEol & lineStyle.Options) != 0;
            cbLineStyleColor.Value = lineStyle.ForeColor;
            syntaxEdit1.LineStyles.Add(lineStyle);

            // breakpoint style
            syntaxEdit1.LineStyles.Add(new EditLineStyle()
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(171, 97, 107),
                Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                ImageIndex = (int)KnownImageIndex.Breakpoint,
            });

            endLine = syntaxEdit1.Lines.Count - 2;

            syntaxEdit1.GutterClick += SyntaxEdit1_GutterClick;
            cbLineStyleColor.SelectedItemChanged += LineStyleColorComboBox_SelectedIndexChanged;
            chbLineStyleBeyondEol.CheckedChanged += LineStyleBeyondEolCheckBox_CheckedChanged;
            btSetBreakpoint.Click += SetBreakpointTextBoxButton_Click;
            btStepOver.Click += StepOverButton_Click;
            btStart.Click += StartButton_Click;
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
            syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
            syntaxEdit1.MakeVisible(new System.Drawing.Point(0, startLine + index));
            Debug();
            startDebug = !startDebug;
        }

        private void StepOver()
        {
            if (index < (endLine - startLine))
            {
                if (syntaxEdit1.Source.LineStyles.GetLineStyle(startLine + index) >= 0)
                    syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                index++;
                while ((index < (endLine - startLine))
                    && (syntaxEdit1.Source.Lines[startLine + index].Trim() == string.Empty))
                    index++;

                syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                syntaxEdit1.MakeVisible(new System.Drawing.Point(0, startLine + index));
            }
            else
            {
                syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                Debug();
                startDebug = !startDebug;
            }
        }

        private void SetBreakpoint()
        {
            syntaxEdit1.Source.LineStyles.ToggleLineStyle(syntaxEdit1.Position.Y, 0, 1);
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
                IEditLineStyle lineStyle = syntaxEdit1.LineStyles[0];
                lineStyle.ForeColor = cbLineStyleColor.Value;
                syntaxEdit1.Invalidate();
            }
        }
    }
}