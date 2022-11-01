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

        public ContextMenu DefaultMenu
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

        public virtual bool UseDefaultMenu
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

        public bool CanCut
        {
            get
            {
                return Selection != null && Selection.CanCut();
            }
        }

        public bool CanDelete
        {
            get
            {
                return Selection != null && Selection.CanDelete();
            }
        }

        public bool CanCopy
        {
            get
            {
                return Selection != null && Selection.CanCopy();
            }
        }

        public bool CanPaste
        {
            get
            {
                return Selection != null && Selection.CanPaste();
            }
        }

        public void Search()
        {
            SearchDialog.Execute(this, false, false);
        }

        public void Replace()
        {
            SearchDialog.Execute(this, false, true);
        }

        public void Cut()
        {
            Selection.Cut();
        }

        public void Delete()
        {
            Selection.DeleteRight();
        }

        public void Copy()
        {
            Selection.Copy();
        }

        public void Paste()
        {
            Selection.Paste();
        }

        protected virtual void UpdateMenu()
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

        protected virtual void OnDefaultMenuChanged()
        {
        }

        protected virtual void OnUseDefaultMenuChanged()
        {
        }

        protected virtual void InitDefaultMenu()
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

        protected void CutCommandClick()
        {
            if (CanCut)
                Cut();
        }

        protected void DeleteCommandClick()
        {
            if (CanDelete)
                Delete();
        }

        protected void CopyCommandClick()
        {
            if (CanCopy)
                Copy();
        }

        protected void PasteCommandClick()
        {
            if (CanPaste)
                Paste();
        }

        protected void SearchCommandClick()
        {
            Search();
        }

        protected void ReplaceCommandClick()
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