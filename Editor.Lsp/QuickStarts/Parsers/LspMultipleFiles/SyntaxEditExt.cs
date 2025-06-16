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
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Syntax;

namespace LspMultipleFiles
{
    internal class SyntaxEditExt : SyntaxEdit
    {
        public event EventHandler<SymbolLocationEventArgs> GoToDefinitionComplete;

        public event EventHandler<RangeListEventArgs> FindReferencesComplete;

        protected override void InitDefaultMenu()
        {
            base.InitDefaultMenu();

            var gotoDefinitionMenuItem = new ToolStripMenuItem(StringConsts.GotoDefinition, null, GotoDefinitionMenuItem_Click);
            gotoDefinitionMenuItem.ShortcutKeys = Keys.F12;

            var findReferencesMenuItem = new ToolStripMenuItem("Find References", null, FindReferencesMenuItem_Click);
            findReferencesMenuItem.ShortcutKeys = Keys.Shift | Keys.F12;

            DefaultMenu.Items.Add("-");
            DefaultMenu.Items.Add(gotoDefinitionMenuItem);
            DefaultMenu.Items.Add(findReferencesMenuItem);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyData == Keys.F12)
                GoToDefinition();
            else if (e.KeyCode == Keys.F12 && e.Shift)
                FindReferences();
        }

        private void FindReferencesMenuItem_Click(object sender, EventArgs e)
        {
            FindReferences();
        }

        private void GotoDefinitionMenuItem_Click(object sender, EventArgs e)
        {
            GoToDefinition();
        }

        private async void FindReferences()
        {
            var parser = (ISyntaxParser)Lexer;
            var references = new RangeList();
            var count = await parser.FindReferencesAsync(Position, references, allDocuments: true);
            if (count == 0)
                return;

            FindReferencesComplete?.Invoke(this, new RangeListEventArgs(references));
        }

        private async void GoToDefinition()
        {
            var parser = (ISyntaxParser)Lexer;
            var declaration = await parser.FindDeclarationAsync(Position);
            if (declaration == null)
                return;

            GoToDefinitionComplete?.Invoke(this, new SymbolLocationEventArgs(declaration));
        }

        public class SymbolLocationEventArgs : EventArgs
        {
            public SymbolLocationEventArgs(SymbolLocation symbolLocation)
            {
                SymbolLocation = symbolLocation;
            }

            public SymbolLocation SymbolLocation { get; }
        }

        public class RangeListEventArgs : EventArgs
        {
            public RangeListEventArgs(IRangeList ranges)
            {
                Ranges = ranges;
            }

            public IRangeList Ranges { get; }
        }
    }
}