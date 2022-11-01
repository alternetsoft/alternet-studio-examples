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
using Alternet.Syntax.Parsers.Roslyn;

namespace WordWrap
{
    public partial class Form1 : Form
    {
        private const string WordWrapDesc = "Wrap words to the beginning of the next line when necessary";
        private const string WrapAtMarginDesc = "Wrap words at the margin position.";
        private CsParser parser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";

        /// <summary>
        /// Initializes a new instance of the Form1 class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load event handler.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\spell.txt");
            if (fileInfo.Exists)
            {
                syntaxEdit1.LoadFile(fileInfo.FullName);
            }

            syntaxEdit1.Lexer = parser1;

            chbWordWrap.Checked = syntaxEdit1.WordWrap;
            chbWrapAtMargin.Checked = syntaxEdit1.WrapAtMargin;
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void WordWrapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.WordWrap = chbWordWrap.Checked;
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void WrapAtMarginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.WrapAtMargin = chbWrapAtMargin.Checked;
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">MouseEventArgs e</param>
        private void WordWrapCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = this.toolTip1.GetToolTip(this.chbWordWrap);
            if (str != WordWrapDesc)
            {
                toolTip1.SetToolTip(chbWordWrap, WordWrapDesc);
            }
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">MouseEventArgs e</param>
        private void WrapAtMarginCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbWrapAtMargin);
            if (str != WrapAtMarginDesc)
            {
                toolTip1.SetToolTip(chbWrapAtMargin, WrapAtMarginDesc);
            }
        }
    }
}
