#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;

namespace CodeOutlining
{
    public partial class Form1 : Form
    {
        private const string AutomaticDesc = "Choose outlining mode: automatic or custom";
        private const string ShowHintsDesc = "Display text of the collapsed outline section in the popup window when mouse pointer is over the outline button";
        private const string AllowOutliningDesc = "Enable outlining";
        private const string DrawOnGutterDesc = "Draw outline images and lines on gutter";
        private const string DrawLinesDesc = "Draw lines for expanded outline section";
        private const string DrawButtonsDesc = "Draw the outline buttons substituting content of the collapsed section";
        private CsParser csParser1 = new CsParser();
        private Parser parser1 = new Parser();
        private Timer timer;
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "CodeOutlining.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
            textSource1.Lexer = csParser1;
        }

        private void DoTimer(object sender, EventArgs e)
        {
            DoCustomOutlining();
            timer.Stop();
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
                if (syntaxEdit1.Find(@"\[.*\]", SearchOptions.EntireScope | SearchOptions.RegularExpressions, new System.Text.RegularExpressions.Regex(@"\[.*\]", System.Text.RegularExpressions.RegexOptions.Singleline)))
                {
                    IList<IRange> ranges = new List<IRange>();
                    Point start = syntaxEdit1.Position;
                    while (syntaxEdit1.FindNext())
                    {
                        ranges.Add(new OutlineRange(start, PrevPosition(syntaxEdit1.Position), 0, "..."));
                        start = syntaxEdit1.Position;
                    }

                    ranges.Add(new OutlineRange(start, new Point(syntaxEdit1.Lines[syntaxEdit1.Lines.Count - 1].Length, syntaxEdit1.Lines.Count - 1), 0, "..."));
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
            syntaxEdit1.Source = automatic ? textSource1 : textSource2;
            if (!automatic)
                DoCustomOutlining();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textSource2.Lexer = parser1;
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += new EventHandler(DoTimer);

            cbAutomatic.SelectedIndex = 0;

            chbAllowOutlining.Checked = syntaxEdit1.Outlining.AllowOutlining;
            chbDrawOnGutter.Checked = (OutlineOptions.DrawOnGutter & syntaxEdit1.Outlining.OutlineOptions) != 0;
            chbDrawLines.Checked = (OutlineOptions.DrawLines & syntaxEdit1.Outlining.OutlineOptions) != 0;
            chbDrawButtons.Checked = (OutlineOptions.DrawButtons & syntaxEdit1.Outlining.OutlineOptions) != 0;
            chbShowHints.Checked = (OutlineOptions.ShowHints & syntaxEdit1.Outlining.OutlineOptions) != 0;

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                textSource1.LoadFile(fileInfo.FullName);

            fileInfo = new FileInfo(dir + @"Resources\Editor\schemes\ini.xml");
            if (fileInfo.Exists)
                parser1.Scheme.LoadFile(fileInfo.FullName);
        }

        private void AllowOutliningCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Outlining.AllowOutlining = chbAllowOutlining.Checked;
            if (chbAllowOutlining.Checked && (cbAutomatic.SelectedIndex != 0))
                DoCustomOutlining();
        }

        private void DrawOnGutterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbDrawOnGutter.Checked ? syntaxEdit1.Outlining.OutlineOptions
            | OutlineOptions.DrawOnGutter : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawOnGutter;
        }

        private void DrawLinesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbDrawLines.Checked ? syntaxEdit1.Outlining.OutlineOptions
            | OutlineOptions.DrawLines : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawLines;
        }

        private void DrawButtonsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbDrawButtons.Checked ? syntaxEdit1.Outlining.OutlineOptions
                | OutlineOptions.DrawButtons : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawButtons;
        }

        private void ShowHintsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Outlining.OutlineOptions = chbShowHints.Checked ? syntaxEdit1.Outlining.OutlineOptions
                | OutlineOptions.ShowHints : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.ShowHints;
        }

        private void AutomaticComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOutlining(cbAutomatic.SelectedIndex == 0);
        }

        private void SyntaxEdit1_SourceStateChanged(object sender, NotifyEventArgs e)
        {
            object source1 = (object)syntaxEdit1.Source;
            if (source1.Equals(textSource2))
            {
                if ((e.State & NotifyState.Edit) != 0)
                    timer.Start();
            }
        }

        private void AutomaticComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbAutomatic);
            if (str != AutomaticDesc)
                toolTip1.SetToolTip(cbAutomatic, AutomaticDesc);
        }

        private void AllowOutliningCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbAllowOutlining);
            if (str != AllowOutliningDesc)
                toolTip1.SetToolTip(chbAllowOutlining, AllowOutliningDesc);
        }

        private void DrawOnGutterCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawOnGutter);
            if (str != DrawOnGutterDesc)
                toolTip1.SetToolTip(chbDrawOnGutter, DrawOnGutterDesc);
        }

        private void DrawLinesCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawLines);
            if (str != DrawLinesDesc)
                toolTip1.SetToolTip(chbDrawLines, DrawLinesDesc);
        }

        private void DrawButtonsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawButtons);
            if (str != DrawButtonsDesc)
                toolTip1.SetToolTip(chbDrawButtons, DrawButtonsDesc);
        }

        private void ShowHintsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowHints);
            if (str != ShowHintsDesc)
                toolTip1.SetToolTip(chbShowHints, ShowHintsDesc);
        }
    }
}
