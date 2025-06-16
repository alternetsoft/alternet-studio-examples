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
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;

namespace UndoRedo
{
    public partial class Form1 : Form
    {
        #region Private Members

        private const string LineModificatorDesc = "Draw line modificator indicators";
        private const string GroupUndoDesc = "Undo subsequent text edit operations of the same type all together";
        private const string UndoNavigationsDesc = "Undo navigate operations separately from text modifications";
        private const string UndoDesc = "Perform the last undo operation";
        private const string RedoDesc = "Perform the last redo operation";
        private const string SaveDesc = "Save and set control's text content to unmodified state";
        private const string ChangedColorDesc = "Color of the line modificators in the modified state";
        private const string SavedColorDesc = "Color of the line modificators in the saved state";
        private const string UndoOperationsDesc = "List of undo operations";
        private CsParser csParser1 = new CsParser();
        private int undoListCount;
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "UndoRedo.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void UpdateUndoList()
        {
            undoListCount = syntaxEdit1.Source.UndoList.Count;
            string s = string.Empty;
            lbUndoOperations.BeginUpdate();
            try
            {
                lbUndoOperations.Items.Clear();
                foreach (UndoData undoData in syntaxEdit1.Source.UndoList)
                {
                    s = s + ((s == string.Empty) ? string.Format("{0}", undoData.Operation) : string.Format(",{0}", undoData.Operation));
                }
            }
            finally
            {
                lbUndoOperations.Items.AddRange(s.Split(','));
                lbUndoOperations.EndUpdate();
            }

            btUndo.Enabled = syntaxEdit1.Source.UndoList.Count > 0;
            btRedo.Enabled = syntaxEdit1.Source.RedoList.Count > 0;
        }

        #endregion

        private void UndoButton_Click(object sender, EventArgs e)
        {
            syntaxEdit1.Source.Undo();
            if (syntaxEdit1.Source.UndoList.Count != undoListCount)
                UpdateUndoList();
            syntaxEdit1.Focus();
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            syntaxEdit1.Source.Redo();
            if (syntaxEdit1.Source.UndoList.Count != undoListCount)
                UpdateUndoList();
            syntaxEdit1.Focus();
        }

        private void SyntaxEdit1_SourceStateChanged(object sender, Alternet.Editor.NotifyEventArgs e)
        {
            if (syntaxEdit1.Source.UndoList.Count != undoListCount)
                UpdateUndoList();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            syntaxEdit1.Modified = false;
        }

        private void LineModificatorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbLineModificator.Checked ? syntaxEdit1.Gutter.Options
                | GutterOptions.PaintLineModificators : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLineModificators;
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
            chbLineModificator.Checked = (GutterOptions.PaintLineModificators & syntaxEdit1.Gutter.Options) != 0;
            chbGroupUndo.Checked = (UndoOptions.GroupUndo & syntaxEdit1.Source.UndoOptions) != 0;
            chbUndoNavigations.Checked = (UndoOptions.UndoNavigations & syntaxEdit1.Source.UndoOptions) != 0;
            cbChangedColor.SelectedColor = syntaxEdit1.Gutter.LineModificatorChangedColor;
            cbSavedColor.SelectedColor = syntaxEdit1.Gutter.LineModificatorSavedColor;
        }

        private void ChangedColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineModificatorChangedColor = cbChangedColor.SelectedColor;
        }

        private void SavedColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineModificatorSavedColor = cbSavedColor.SelectedColor;
        }

        private void GroupUndoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Source.UndoOptions = chbGroupUndo.Checked ? syntaxEdit1.Source.UndoOptions
                | UndoOptions.GroupUndo : syntaxEdit1.Source.UndoOptions & ~UndoOptions.GroupUndo;
        }

        private void UndoNavigationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Source.UndoOptions = chbUndoNavigations.Checked ? syntaxEdit1.Source.UndoOptions
                | UndoOptions.UndoNavigations : syntaxEdit1.Source.UndoOptions & ~UndoOptions.UndoNavigations;
        }

        private void LineModificatorCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLineModificator);
            if (str != LineModificatorDesc)
                toolTip1.SetToolTip(chbLineModificator, LineModificatorDesc);
        }

        private void GroupUndoCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbGroupUndo);
            if (str != GroupUndoDesc)
                toolTip1.SetToolTip(chbGroupUndo, GroupUndoDesc);
        }

        private void UndoNavigationsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbUndoNavigations);
            if (str != UndoNavigationsDesc)
                toolTip1.SetToolTip(chbUndoNavigations, UndoNavigationsDesc);
        }

        private void UndoButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btUndo);
            if (str != UndoDesc)
                toolTip1.SetToolTip(btUndo, UndoDesc);
        }

        private void RedoButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btRedo);
            if (str != RedoDesc)
                toolTip1.SetToolTip(btRedo, RedoDesc);
        }

        private void SaveButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSave);
            if (str != SaveDesc)
                toolTip1.SetToolTip(btSave, SaveDesc);
        }

        private void ChangedColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbChangedColor);
            if (str != ChangedColorDesc)
                toolTip1.SetToolTip(cbChangedColor, ChangedColorDesc);
        }

        private void SavedColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbSavedColor);
            if (str != SavedColorDesc)
                toolTip1.SetToolTip(cbSavedColor, SavedColorDesc);
        }

        private void UndoOperationsLisoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(lbUndoOperations);
            if (str != UndoOperationsDesc)
                toolTip1.SetToolTip(lbUndoOperations, UndoOperationsDesc);
        }
    }
}
