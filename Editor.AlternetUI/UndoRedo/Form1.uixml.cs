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
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace UndoRedo
{
    public partial class Form1 : Window
    {
        private readonly TextSource cSharpSource = new();
        private readonly CsParser csParser1 = new(new CsSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Source = cSharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            cSharpSource.OptimizedForMemory = false;

            Form1_Load(this, EventArgs.Empty);

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileInfo fileInfo = new(DemoUtils.GetResourceFileFullPath(@"Editor/Text/c#.cs"));

            if (cSharpSource.LoadOrAddNotFound(fileInfo.FullName))
            {
                cSharpSource.Lexer = csParser1;
            }

            syntaxEdit1.Selection.Options |= SelectionOptions.RtfClipboard;

            cSharpSource.HighlightReferences = true;
            chbLineModificators.Checked
                = (GutterOptions.PaintLineModificators & syntaxEdit1.Gutter.Options) != 0;
            chbGroupUndo.Checked = (UndoOptions.GroupUndo & syntaxEdit1.Source.UndoOptions) != 0;
            chbUndoNavigations.Checked
                = (UndoOptions.UndoNavigations & syntaxEdit1.Source.UndoOptions) != 0;
            cbChangedLineColor.Select(syntaxEdit1.Gutter.LineModificatorChangedColor);
            cbSavedLineColor.Select(syntaxEdit1.Gutter.LineModificatorSavedColor);
            syntaxEdit1.SourceStateChanged += SyntaxEdit1_SourceStateChanged;
            SaveButton.Click += SaveButton_Click;
            UndoButton.Click += UndoButton_Click;
            RedoButton.Click += RedoButton_Click;
            chbLineModificators.CheckedChanged += LineModificatorCheckBox_CheckedChanged;
            chbGroupUndo.CheckedChanged += GroupUndoCheckBox_CheckedChanged;
            chbUndoNavigations.CheckedChanged += UndoNavigationsCheckBox_CheckedChanged;
            cbChangedLineColor.ValueChanged += ChangedColorComboBox_SelectedIndexChanged;
            cbSavedLineColor.ValueChanged += SavedColorComboBox_SelectedIndexChanged;
        }

        private void CbChangedLineColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
        }

        private void UpdateUndoList()
        {
            UndoButton.Enabled = syntaxEdit1.Source.CanUndo();
            RedoButton.Enabled = syntaxEdit1.Source.CanRedo();

            ListSource items = new ();

            foreach (UndoData undoData in syntaxEdit1.Source.UndoList)
            {
                items.Add(new(undoData.Operation.ToString()));
            }

            UndoOperationsListBox.DoInsideUpdate(() =>
            {
                UndoOperationsListBox
                    .SetItemsFast(items, VirtualListBox.SetItemsKind.ChangeField);
                UndoOperationsListBox.ScrollToLastRow();
            });
        }

        private void UndoButton_Click(object? sender, EventArgs e)
        {
            syntaxEdit1.Source.Undo();
            if (syntaxEdit1.Source.UndoList.Count != UndoOperationsListBox.Count)
                UpdateUndoList();
            syntaxEdit1.Focus();
        }

        private void RedoButton_Click(object? sender, EventArgs e)
        {
            syntaxEdit1.Source.Redo();
            if (syntaxEdit1.Source.UndoList.Count != UndoOperationsListBox.Count)
                UpdateUndoList();
            syntaxEdit1.Focus();
        }

        private void SyntaxEdit1_SourceStateChanged(object? sender, NotifyEventArgs e)
        {
            if (syntaxEdit1.Source.UndoList.Count != UndoOperationsListBox.Count)
                UpdateUndoList();
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            syntaxEdit1.Modified = false;
        }

        private void LineModificatorCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbLineModificators.Checked
                ? syntaxEdit1.Gutter.Options | GutterOptions.PaintLineModificators
                : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLineModificators;
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
            syntaxEdit1.Source.UndoOptions = chbUndoNavigations.Checked
                ? syntaxEdit1.Source.UndoOptions | UndoOptions.UndoNavigations
                : syntaxEdit1.Source.UndoOptions & ~UndoOptions.UndoNavigations;
        }
    }
}
