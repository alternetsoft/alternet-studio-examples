#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;

namespace LineStyles
{
    public partial class Form1 : Form
    {
        private const string StartDesc = "Display line style";
        private const string StepOverDesc = "Move line style to the next line";
        private const string SetBreakpointDesc = "Set breakpoint bookmark";
        private const string LineStyleBeyondEolDesc = "Line style applicable beyond end of line";
        private const string LineStyleColorDesc = "Line style color";
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";
        private bool startDebug;
        private int startLine = 44;
        private int endLine = 0;
        private int index;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
            syntaxEdit1.Lexer = csParser1;
            if (syntaxEdit1.Find("Main", SearchOptions.EntireScope))
            {
                syntaxEdit1.Selection.Clear();
                startLine = syntaxEdit1.Position.Y + 2;
            }

            IEditLineStyle lineStyle = new EditLineStyle();
            lineStyle.BackColor = Color.Black;
            lineStyle.ForeColor = Color.FromArgb(255, 241, 129);
            lineStyle.Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors;

            lineStyle.ImageIndex = 12;
            chbLineStyleBeyondEol.Checked = (LineStyleOptions.BeyondEol & lineStyle.Options) != 0;
            cbLineStyleColor.SelectedColor = lineStyle.ForeColor;
            syntaxEdit1.LineStyles.Add(lineStyle);

            syntaxEdit1.LineStyles.Add(new EditLineStyle() // breakpoint style
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(171, 97, 107),
                Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                ImageIndex = 11,
            });

            endLine = syntaxEdit1.Lines.Count - 2;
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
            syntaxEdit1.MakeVisible(new Point(0, startLine + index));
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
                while ((index < (endLine - startLine)) && (syntaxEdit1.Source.Lines[startLine + index].Trim() == string.Empty))
                    index++;

                syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                syntaxEdit1.MakeVisible(new Point(0, startLine + index));
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
            syntaxEdit1.Source.BookMarks.ToggleBookMark(syntaxEdit1.Position, 11);
            syntaxEdit1.Source.LineStyles.ToggleLineStyle(syntaxEdit1.Position.Y, 0, 1);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void StepOverButton_Click(object sender, EventArgs e)
        {
            StepOver();
        }

        private void SereakpointTextBoxButton_Click(object sender, EventArgs e)
        {
            SetBreakpoint();
        }

        private void StartMenuItem_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void StepOverMenuItem_Click(object sender, EventArgs e)
        {
            StepOver();
        }

        private void SereakpointTextBoxMenuItem_Click(object sender, EventArgs e)
        {
            SetBreakpoint();
        }

        private void LineStyleBeyondEolCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (syntaxEdit1.LineStyles.Count > 0)
            {
                foreach (IEditLineStyle lineStyle in syntaxEdit1.LineStyles)
                {
                    lineStyle.Options = chbLineStyleBeyondEol.Checked ? lineStyle.Options | LineStyleOptions.BeyondEol :
                        lineStyle.Options & ~LineStyleOptions.BeyondEol;
                }

                syntaxEdit1.Invalidate();
            }
        }

        private void LineStyleColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (syntaxEdit1.LineStyles.Count > 0)
            {
                IEditLineStyle lineStyle = syntaxEdit1.LineStyles[0];
                lineStyle.ForeColor = cbLineStyleColor.SelectedColor;
                syntaxEdit1.Invalidate();
            }
        }

        private void StartButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btStart);
            if (str != StartDesc)
                toolTip1.SetToolTip(btStart, StartDesc);
        }

        private void StepOverButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btStepOver);
            if (str != StepOverDesc)
                toolTip1.SetToolTip(btStepOver, StepOverDesc);
        }

        private void SereakpointTextBoxButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSetBreakpoint);
            if (str != SetBreakpointDesc)
                toolTip1.SetToolTip(btSetBreakpoint, SetBreakpointDesc);
        }

        private void LineStyleBeyondEolCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLineStyleBeyondEol);
            if (str != LineStyleBeyondEolDesc)
                toolTip1.SetToolTip(chbLineStyleBeyondEol, LineStyleBeyondEolDesc);
        }

        private void LineStyleColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLineStyleColor);
            if (str != LineStyleColorDesc)
                toolTip1.SetToolTip(cbLineStyleColor, LineStyleColorDesc);
        }
    }
}
