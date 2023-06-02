#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;

namespace PageLayout
{
    public partial class Form1 : Form
    {
        private const string PageLayoutDesc = "Select the way of viewing Edit control's content";
        private const string PageSizeDesc = "Select paper size";
        private const string HorzRulerDesc = "Display a horizontal ruler";
        private const string VertRulerDesc = "Display a vertical ruler";
        private const string RulerUnitsDesc = "Measurement units of the pages rulers";
        private const string RulerAllowDragDesc = "Allow dragging ruler indentations";
        private const string RulerDiscreteDesc = "Change ruler indentation in discrete steps";
        private const string RulerDisplayDragLinesDesc = "Displays dotted line when ruler indentation being dragged";
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";

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

            string[] s = Enum.GetNames(typeof(PageType));
            cbPageLayout.Items.AddRange(s);
            cbPageLayout.SelectedIndex = (int)syntaxEdit1.Pages.PageType;
            foreach (PaperSize psize in syntaxEdit1.Printing.PrinterSettings.PaperSizes)
            {
                if (!cbPageSize.Items.Contains(psize.Kind))
                    cbPageSize.Items.Add(psize.Kind);
            }

            if (cbPageSize.Items.Count > 0)
            {
                cbPageSize.SelectedIndex = Math.Max(cbPageSize.Items.IndexOf(syntaxEdit1.Pages.DefaultPage.PageKind), 0);
                cbPageSize.Enabled = true;
            }
            else
                cbPageSize.Enabled = false;

            s = Enum.GetNames(typeof(RulerUnits));
            cbRulerUnits.Items.AddRange(s);
            cbRulerUnits.SelectedIndex = (int)syntaxEdit1.Pages.RulerUnits;
            chbRulerAllowDrag.Checked = (RulerOptions.AllowDrag & syntaxEdit1.Pages.RulerOptions) != 0;
            chbRulerDiscrete.Checked = (RulerOptions.Discrete & syntaxEdit1.Pages.RulerOptions) != 0;
            chbRulerDisplayDragLines.Checked = (RulerOptions.DisplayDragLine & syntaxEdit1.Pages.RulerOptions) != 0;
            chbHorzRuler.Checked = (EditRulers.Horizonal & syntaxEdit1.Pages.Rulers) != 0;
            chbVertRuler.Checked = (EditRulers.Vertical & syntaxEdit1.Pages.Rulers) != 0;
        }

        private void PageLayoutComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Pages.PageType = (PageType)cbPageLayout.SelectedIndex;
        }

        private void PageSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object obj = Enum.Parse(typeof(PaperKind), cbPageSize.Text);
            if (obj is PaperKind)
            {
                for (int i = syntaxEdit1.Pages.Count - 1; i >= 0; i--)
                    syntaxEdit1.Pages[i].PageKind = (PaperKind)obj;
            }
        }

        private void RulerUnitsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Pages.RulerUnits = (RulerUnits)cbRulerUnits.SelectedIndex;
        }

        private void HorzRulerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Pages.Rulers = chbHorzRuler.Checked ? syntaxEdit1.Pages.Rulers
                | EditRulers.Horizonal : syntaxEdit1.Pages.Rulers & ~EditRulers.Horizonal;
        }

        private void VertRulerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Pages.Rulers = chbVertRuler.Checked ? syntaxEdit1.Pages.Rulers
                | EditRulers.Vertical : syntaxEdit1.Pages.Rulers & ~EditRulers.Vertical;
        }

        private void RulerAllowDragCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Pages.RulerOptions = chbRulerAllowDrag.Checked ? syntaxEdit1.Pages.RulerOptions
                | RulerOptions.AllowDrag : syntaxEdit1.Pages.RulerOptions & ~RulerOptions.AllowDrag;
        }

        private void RulerDiscreteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Pages.RulerOptions = chbRulerDiscrete.Checked ? syntaxEdit1.Pages.RulerOptions
                | RulerOptions.Discrete : syntaxEdit1.Pages.RulerOptions & ~RulerOptions.Discrete;
        }

        private void RulerDisplayDragLinesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Pages.RulerOptions = chbRulerDisplayDragLines.Checked ? syntaxEdit1.Pages.RulerOptions
                | RulerOptions.DisplayDragLine : syntaxEdit1.Pages.RulerOptions & ~RulerOptions.DisplayDragLine;
        }

        private void PageLayoutComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbPageLayout);
            if (str != PageLayoutDesc)
                toolTip1.SetToolTip(cbPageLayout, PageLayoutDesc);
        }

        private void PageSizeComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbPageSize);
            if (str != PageSizeDesc)
                toolTip1.SetToolTip(cbPageSize, PageSizeDesc);
        }

        private void HorzRulerCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHorzRuler);
            if (str != HorzRulerDesc)
                toolTip1.SetToolTip(chbHorzRuler, HorzRulerDesc);
        }

        private void VertRulerCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbVertRuler);
            if (str != VertRulerDesc)
                toolTip1.SetToolTip(chbVertRuler, VertRulerDesc);
        }

        private void RulerUnitsComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbRulerUnits);
            if (str != RulerUnitsDesc)
                toolTip1.SetToolTip(cbRulerUnits, RulerUnitsDesc);
        }

        private void RulerAllowDragCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbRulerAllowDrag);
            if (str != RulerAllowDragDesc)
                toolTip1.SetToolTip(chbRulerAllowDrag, RulerAllowDragDesc);
        }

        private void RulerDiscreteCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbRulerDiscrete);
            if (str != RulerDiscreteDesc)
                toolTip1.SetToolTip(chbRulerDiscrete, RulerDiscreteDesc);
        }

        private void RulerDisplayDragLinesCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbRulerDisplayDragLines);
            if (str != RulerDisplayDragLinesDesc)
                toolTip1.SetToolTip(chbRulerDisplayDragLines, RulerDisplayDragLinesDesc);
        }
    }
}
