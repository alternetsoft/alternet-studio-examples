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

using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace UndoRedo
{
    public partial class Form1 : Window
    {
        private readonly Alternet.Editor.TextSource.TextSource csharpSource = new();
        private readonly CsParser csParser1 = new(new CsSolution());
        private int undoListCount;

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Source = csharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            csharpSource.OptimizedForMemory = false;

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
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            if (csharpSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                csharpSource.Lexer = csParser1;
            }

            syntaxEdit1.Selection.Options |= SelectionOptions.RtfClipboard;

            csharpSource.HighlightReferences = true;
            chbLineModificators.Checked = (GutterOptions.PaintLineModificators & syntaxEdit1.Gutter.Options) != 0;
            chbGroupUndo.Checked = (UndoOptions.GroupUndo & syntaxEdit1.Source.UndoOptions) != 0;
            chbUndoNavigations.Checked = (UndoOptions.UndoNavigations & syntaxEdit1.Source.UndoOptions) != 0;
            cbChangedLineColor.SelectedItem = cbChangedLineColor.FindOrAdd(syntaxEdit1.Gutter.LineModificatorChangedColor);
            cbSavedLineColor.SelectedItem = cbSavedLineColor.FindOrAdd(syntaxEdit1.Gutter.LineModificatorSavedColor);
            syntaxEdit1.SourceStateChanged += SyntaxEdit1_SourceStateChanged;
            SaveButton.Click += SaveButton_Click;
            UndoButton.Click += UndoButton_Click;
            RedoButton.Click += RedoButton_Click;
            chbLineModificators.CheckedChanged += LineModificatorCheckBox_CheckedChanged;
            chbGroupUndo.CheckedChanged += GroupUndoCheckBox_CheckedChanged;
            chbUndoNavigations.CheckedChanged += UndoNavigationsCheckBox_CheckedChanged;
            cbChangedLineColor.SelectedIndexChanged += ChangedColorComboBox_SelectedIndexChanged;
            cbSavedLineColor.SelectedIndexChanged += SavedColorComboBox_SelectedIndexChanged;
        }

        private void CbChangedLineColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UpdateUndoList()
        {
            undoListCount = syntaxEdit1.Source.UndoList.Count;
            string s = string.Empty;
            UndoOperationsListBox.BeginUpdate();
            try
            {
                UndoOperationsListBox.Items.Clear();
                foreach (UndoData undoData in syntaxEdit1.Source.UndoList)
                {
                    s = s + ((s == string.Empty) ? string.Format("{0}", undoData.Operation) : string.Format(",{0}", undoData.Operation));
                }
            }
            finally
            {
                UndoOperationsListBox.Items.AddRange(s.Split(','));
                UndoOperationsListBox.EndUpdate();
            }

            UndoButton.Enabled = syntaxEdit1.Source.UndoList.Count > 0;
            RedoButton.Enabled = syntaxEdit1.Source.RedoList.Count > 0;
        }
        private void UndoButton_Click(object? sender, EventArgs e)
        {
            syntaxEdit1.Source.Undo();
            if (syntaxEdit1.Source.UndoList.Count != undoListCount)
                UpdateUndoList();
            syntaxEdit1.Focus();
        }

        private void RedoButton_Click(object? sender, EventArgs e)
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

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            syntaxEdit1.Modified = false;
        }

        private void LineModificatorCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbLineModificators.Checked ? syntaxEdit1.Gutter.Options
                | GutterOptions.PaintLineModificators : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLineModificators;
        }

        private void ChangedColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineModificatorChangedColor = cbChangedLineColor.Value;
        }

        private void SavedColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineModificatorSavedColor = cbSavedLineColor.Value;
        }

        private void GroupUndoCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Source.UndoOptions = chbGroupUndo.Checked ? syntaxEdit1.Source.UndoOptions
                | UndoOptions.GroupUndo : syntaxEdit1.Source.UndoOptions & ~UndoOptions.GroupUndo;
        }

        private void UndoNavigationsCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Source.UndoOptions = chbUndoNavigations.Checked ? syntaxEdit1.Source.UndoOptions
                | UndoOptions.UndoNavigations : syntaxEdit1.Source.UndoOptions & ~UndoOptions.UndoNavigations;
        }
    }
}
