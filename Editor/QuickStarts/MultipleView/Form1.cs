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
using System.IO;
using System.Windows.Forms;

using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;

namespace MultipleView
{
    public partial class Form1 : Form
    {
        private const string AllowSplitHorzDesc = "Allow splitting Edit control horizontally.";
        private const string AllowSplitVertDesc = "Allow splitting  Edit control vertically.";
        private const string HorzButtonDesc = "Display a collection of horizontal buttons at the left side of the horizontal scroll bar";
        private const string VertButtonDesc = "Display a collection of vertical buttons at the bottom side of vertical scroll bar";
        private const string ShowScrollHintDesc = "Display hint text in the popup window when user moving the thumb";
        private const string SmoothScrollDesc = "Scroll edit control content immediately when user moving the thumb";
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";
        private int scrollBoxUpdate;

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
                csharpSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;

            chbAllowSplitHorz.Checked = (ScrollingOptions.AllowSplitHorz & syntaxEdit1.Scrolling.Options) != 0;
            chbAllowSplitVert.Checked = (ScrollingOptions.AllowSplitVert & syntaxEdit1.Scrolling.Options) != 0;
            chbHorzButton.Checked = (ScrollingOptions.HorzButtons & syntaxEdit1.Scrolling.Options) != 0;
            chbVertButton.Checked = (ScrollingOptions.VertButtons & syntaxEdit1.Scrolling.Options) != 0;
            chbShowScrollHint.Checked = (syntaxEdit1.Scrolling.Options & ScrollingOptions.ShowScrollHint) != 0;
            chbSmoothScroll.Checked = (syntaxEdit1.Scrolling.Options & ScrollingOptions.SmoothScroll) != 0;
            chbSystemScrollBars.Checked = (syntaxEdit1.Scrolling.Options & ScrollingOptions.SystemScrollbars) != 0;
        }

        private void UpdateScrollBoxes(object sender)
        {
            if (scrollBoxUpdate > 0)
                return;
            scrollBoxUpdate++;
            try
            {
                if (chbAllowSplitHorz != sender)
                    chbAllowSplitHorz.Checked = (ScrollingOptions.AllowSplitHorz & syntaxEdit1.Scrolling.Options) != 0;
                if (chbAllowSplitVert != sender)
                    chbAllowSplitVert.Checked = (ScrollingOptions.AllowSplitVert & syntaxEdit1.Scrolling.Options) != 0;
                if (chbHorzButton != sender)
                    chbHorzButton.Checked = (ScrollingOptions.HorzButtons & syntaxEdit1.Scrolling.Options) != 0;
                if (chbVertButton != sender)
                    chbVertButton.Checked = (ScrollingOptions.VertButtons & syntaxEdit1.Scrolling.Options) != 0;
                if (chbSystemScrollBars != sender)
                    chbSystemScrollBars.Checked = (syntaxEdit1.Scrolling.Options & ScrollingOptions.SystemScrollbars) != 0;
                if (chbVerticalAnnotations != sender)
                    chbVerticalAnnotations.Checked = (syntaxEdit1.Scrolling.Options & ScrollingOptions.VerticalScrollBarAnnotations) != 0;
            }
            finally
            {
                scrollBoxUpdate--;
            }
        }

        private void AllowSplitHorzCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit1.Scrolling.Options = chbAllowSplitHorz.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.AllowSplitHorz : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.AllowSplitHorz;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
            UpdateScrollBoxes(chbAllowSplitHorz);
        }

        private void AllowSplitVertCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit1.Scrolling.Options = chbAllowSplitVert.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.AllowSplitVert : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.AllowSplitVert;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
            UpdateScrollBoxes(chbAllowSplitVert);
        }

        private void HorzButtonCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit1.Scrolling.Options = chbHorzButton.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.HorzButtons : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.HorzButtons;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
            UpdateScrollBoxes(chbHorzButton);
        }

        private void VeruttonCheckBoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit1.Scrolling.Options = chbVertButton.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.VertButtons : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.VertButtons;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
            UpdateScrollBoxes(chbVertButton);
        }

        private void SyntaxEdit1_ScrollButtonClick(object sender, EventArgs e)
        {
            if (sender is IScrollingButton)
            {
                switch (((IScrollingButton)sender).Name)
                {
                    case "Normal":
                        {
                            syntaxEdit1.Pages.PageType = PageType.Normal;
                            break;
                        }

                    case "PageLayout":
                        {
                            syntaxEdit1.Pages.PageType = PageType.PageLayout;
                            break;
                        }

                    case "PageBreaks":
                        {
                            syntaxEdit1.Pages.PageType = PageType.PageBreaks;
                            break;
                        }

                    case "PageUp":
                        {
                            syntaxEdit1.MovePageUp();
                            break;
                        }

                    case "PageDown":
                        {
                            syntaxEdit1.MovePageDown();
                            break;
                        }
                }
            }
        }

        private void SyntaxEdit2_ScrollButtonClick(object sender, EventArgs e)
        {
            if (sender is IScrollingButton)
            {
                switch (((IScrollingButton)sender).Name)
                {
                    case "Normal":
                        {
                            syntaxEdit2.Pages.PageType = PageType.Normal;
                            break;
                        }

                    case "PageLayout":
                        {
                            syntaxEdit2.Pages.PageType = PageType.PageLayout;
                            break;
                        }

                    case "PageBreaks":
                        {
                            syntaxEdit2.Pages.PageType = PageType.PageBreaks;
                            break;
                        }

                    case "PageUp":
                        {
                            syntaxEdit2.MovePageUp();
                            break;
                        }

                    case "PageDown":
                        {
                            syntaxEdit2.MovePageDown();
                            break;
                        }
                }
            }
        }

        private void ShowScrollHintCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Scrolling.Options = chbShowScrollHint.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.ShowScrollHint : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.ShowScrollHint;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
        }

        private void SmoothScrollCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Scrolling.Options = chbSmoothScroll.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.SmoothScroll : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.SmoothScroll;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
        }

        private void SystemScrolarsLisoxCheckBoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit1.Scrolling.Options = chbSystemScrollBars.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.SystemScrollbars : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.SystemScrollbars;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
            UpdateScrollBoxes(chbSystemScrollBars);
        }

        private void AllowSplitHorzCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbAllowSplitHorz);
            if (str != AllowSplitHorzDesc)
                toolTip1.SetToolTip(chbAllowSplitHorz, AllowSplitHorzDesc);
        }

        private void AllowSplitVertCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbAllowSplitVert);
            if (str != AllowSplitVertDesc)
                toolTip1.SetToolTip(chbAllowSplitVert, AllowSplitVertDesc);
        }

        private void HorzButtonCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHorzButton);
            if (str != HorzButtonDesc)
                toolTip1.SetToolTip(chbHorzButton, HorzButtonDesc);
        }

        private void VeruttonCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbVertButton);
            if (str != VertButtonDesc)
                toolTip1.SetToolTip(chbVertButton, VertButtonDesc);
        }

        private void ShowScrollHintCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowScrollHint);
            if (str != ShowScrollHintDesc)
                toolTip1.SetToolTip(chbShowScrollHint, ShowScrollHintDesc);
        }

        private void SmoothScrollCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSmoothScroll);
            if (str != SmoothScrollDesc)
                toolTip1.SetToolTip(chbSmoothScroll, SmoothScrollDesc);
        }

        private void VerticalAnnotationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit1.Scrolling.Options = chbVerticalAnnotations.Checked ? syntaxEdit1.Scrolling.Options
                | ScrollingOptions.VerticalScrollBarAnnotations : syntaxEdit1.Scrolling.Options & ~ScrollingOptions.VerticalScrollBarAnnotations;
            syntaxEdit2.Scrolling.Options = syntaxEdit1.Scrolling.Options;
            UpdateScrollBoxes(chbVerticalAnnotations);
            syntaxEdit1.Invalidate(true);
            syntaxEdit2.Invalidate(true);
        }
    }
}
