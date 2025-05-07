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
using System.Windows.Controls;
using System.Windows.Input;
using Alternet.Common;
using Alternet.Editor.Wpf;
using Alternet.Syntax;

namespace LspMultipleFiles
{
    internal class TextEditorExt : TextEditor
    {
        private ContextMenu defaultMenu;
        private bool useDefaultMenu = true;
        private MenuItem miCut;
        private MenuItem miDelete;
        private MenuItem miCopy;
        private MenuItem miPaste;
        private MenuItem miSearch;
        private MenuItem miReplace;

        public TextEditorExt()
            : base()
        {
            InitDefaultMenu();
        }

        public event EventHandler<SymbolLocationEventArgs> GoToDefinitionComplete;

        public event EventHandler<RangeListEventArgs> FindReferencesComplete;

        public new ContextMenu DefaultMenu
        {
            get
            {
                if (defaultMenu == null)
                    InitDefaultMenu();
                return defaultMenu;
            }

            set
            {
                if (defaultMenu != value)
                {
                    defaultMenu = value;
                    OnDefaultMenuChanged();

                    RaisePropertyChanged(nameof(DefaultMenu));
                }
            }
        }

        public new virtual bool UseDefaultMenu
        {
            get
            {
                return useDefaultMenu;
            }

            set
            {
                if (useDefaultMenu != value)
                {
                    useDefaultMenu = value;
                    OnUseDefaultMenuChanged();

                    RaisePropertyChanged(nameof(UseDefaultMenu));
                }
            }
        }

        public new bool CanCut
        {
            get
            {
                return Selection != null && Selection.CanCut();
            }
        }

        public new bool CanDelete
        {
            get
            {
                return Selection != null && Selection.CanDelete();
            }
        }

        public new bool CanCopy
        {
            get
            {
                return Selection != null && Selection.CanCopy();
            }
        }

        public new bool CanPaste
        {
            get
            {
                return Selection != null && Selection.CanPaste();
            }
        }

        public new void Search()
        {
            SearchDialog.Execute(this, false, false);
        }

        public new void Replace()
        {
            SearchDialog.Execute(this, false, true);
        }

        public new void Cut()
        {
            Selection.Cut();
        }

        public new void Delete()
        {
            Selection.DeleteRight();
        }

        public new void Copy()
        {
            Selection.Copy();
        }

        public new void Paste()
        {
            Selection.Paste();
        }

        protected new virtual void UpdateMenu()
        {
            miCut.IsEnabled = CanCut;
            miCopy.IsEnabled = CanCopy;
            miPaste.IsEnabled = CanPaste;
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            if (useDefaultMenu)
            {
                ContextMenu = DefaultMenu;
                UpdateMenu();
            }
        }

        protected new virtual void OnDefaultMenuChanged()
        {
        }

        protected new virtual void OnUseDefaultMenuChanged()
        {
        }

        protected new virtual void InitDefaultMenu()
        {
            defaultMenu = new ContextMenu();

            miCut = new MenuItem();
            miCut.Header = StringConsts.MenuCutCaption;
            var cutCommand = new RelayCommand(CutCommandClick);
            miCut.Command = cutCommand;
            miCut.InputGestureText = "Ctrl+X";

            miDelete = new MenuItem();
            miDelete.Header = StringConsts.MenuDeleteCaption;
            var deleteCommand = new RelayCommand(DeleteCommandClick);
            miDelete.Command = deleteCommand;
            miDelete.InputGestureText = "Del";

            miCopy = new MenuItem();
            miCopy.Header = StringConsts.MenuCopyCaption;
            var copyCommand = new RelayCommand(CopyCommandClick);
            miCopy.Command = copyCommand;
            miCopy.InputGestureText = "Ctrl+C";

            miPaste = new MenuItem();
            miPaste.Header = StringConsts.MenuPasteCaption;
            var pasteCommand = new RelayCommand(PasteCommandClick);
            miPaste.Command = pasteCommand;
            miPaste.InputGestureText = "Ctrl+V";

            miSearch = new MenuItem();
            miSearch.Header = StringConsts.SearchCaption;
            var searchCommand = new RelayCommand(SearchCommandClick);
            miSearch.Command = searchCommand;
            miSearch.InputGestureText = "Ctrl+F";

            miReplace = new MenuItem();
            miReplace.Header = StringConsts.ReplaceCaption.Replace("&", string.Empty);
            var replaceCommand = new RelayCommand(ReplaceCommandClick);
            miReplace.Command = replaceCommand;
            miReplace.InputGestureText = "Ctrl+H";

            defaultMenu.Items.Add(miCut);
            defaultMenu.Items.Add(miCopy);
            defaultMenu.Items.Add(miPaste);
            defaultMenu.Items.Add(miDelete);
            defaultMenu.Items.Add(new Separator());
            defaultMenu.Items.Add(miSearch);
            defaultMenu.Items.Add(miReplace);

            var gotoDefinitionMenuItem = new MenuItem();
            gotoDefinitionMenuItem.Header = "Go to Definition";
            gotoDefinitionMenuItem.Command = new RelayCommand(GotoDefinitionMenuItem_Click);
            gotoDefinitionMenuItem.Name = "cmiGotoDefinition";
            gotoDefinitionMenuItem.InputGestureText = "F12";
            InputBindings.Add(new KeyBinding(gotoDefinitionMenuItem.Command, new KeyGesture(Key.F12)));

            var findReferencesMenuItem = new MenuItem();
            findReferencesMenuItem.Header = "Find References";
            findReferencesMenuItem.Command = new RelayCommand(FindReferencesMenuItem_Click);
            findReferencesMenuItem.Name = "cmiFindReferences";
            findReferencesMenuItem.InputGestureText = "Shift + F12";
            InputBindings.Add(new KeyBinding(findReferencesMenuItem.Command, new KeyGesture(Key.F12, ModifierKeys.Shift, "Shift + F12")));

            defaultMenu.Items.Add(new Separator());
            defaultMenu.Items.Add(gotoDefinitionMenuItem);
            defaultMenu.Items.Add(findReferencesMenuItem);
        }

        protected new void CutCommandClick()
        {
            if (CanCut)
                Cut();
        }

        protected new void DeleteCommandClick()
        {
            if (CanDelete)
                Delete();
        }

        protected new void CopyCommandClick()
        {
            if (CanCopy)
                Copy();
        }

        protected new void PasteCommandClick()
        {
            if (CanPaste)
                Paste();
        }

        protected new void SearchCommandClick()
        {
            Search();
        }

        protected new void ReplaceCommandClick()
        {
            Replace();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F12)
                GoToDefinition();
            else if (e.Key == Key.F12 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                FindReferences();
        }

        private void FindReferencesMenuItem_Click()
        {
            FindReferences();
        }

        private void GotoDefinitionMenuItem_Click()
        {
            GoToDefinition();
        }

        private void FindReferences()
        {
            var parser = (SyntaxParser)Lexer;
            var references = new RangeList();
            var count = parser.FindReferences(Position, references, allDocuments: true);
            if (count == 0)
                return;

            FindReferencesComplete?.Invoke(this, new RangeListEventArgs(references));
        }

        private void GoToDefinition()
        {
            var parser = (SyntaxParser)Lexer;
            var declaration = parser.FindDeclaration(Position);
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