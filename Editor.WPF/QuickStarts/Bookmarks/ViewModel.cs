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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Advanced;

namespace Bookmarks
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TabControl tcEditors;

        private bool drawBookmarks;
        private bool drawLineBookmarks;
        private IDictionary<TabItem, TextEditor> editors = new Dictionary<TabItem, TextEditor>();

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            SetBookmark = new RelayCommand(SetBookmarkClick);
            SetIndexBookmark = new RelayCommand(SetIndexBookmarkClick);
            ClearBookmarks = new RelayCommand(ClearBookmarkClick);
            SetCustomBookmark = new RelayCommand(SetCustomBookmarkClick);
            NextBookmark = new RelayCommand(NextBookmarkClick);
            PrevBookmark = new RelayCommand(PrevBookmarkClick);
        }

        public ViewModel(TabControl tcEditors)
            : this()
        {
            this.tcEditors = tcEditors;
            OpenProject();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DrawLineBookmarks
        {
            get
            {
                return drawLineBookmarks;
            }

            set
            {
                if (drawLineBookmarks != value)
                {
                    drawLineBookmarks = value;
                    OnPropertyChanged("DrawLineBookmarks");
                    UpdateLineDrawBookmarks();
                }
            }
        }

        public bool DrawBookmarks
        {
            get
            {
                return drawBookmarks;
            }

            set
            {
                if (drawBookmarks != value)
                {
                    drawBookmarks = value;
                    OnPropertyChanged("DrawBookmarks");
                    UpdateDrawBookmarks();
                }
            }
        }

        public ICommand SetIndexBookmark { get; set; }

        public ICommand SetBookmark { get; set; }

        public ICommand ClearBookmarks { get; set; }

        public ICommand SetCustomBookmark { get; set; }

        public ICommand NextBookmark { get; set; }

        public ICommand PrevBookmark { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private TextEditor GetEditor(TabItem key)
        {
            TextEditor result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private void UpdateDrawBookmarks()
        {
            foreach (TabItem page in tcEditors.Items)
            {
                TextEditor edit = GetEditor(page);
                if (edit != null)
                    edit.PaintBookMarks = drawBookmarks;
            }
        }

        private void UpdateLineDrawBookmarks()
        {
            foreach (TabItem page in tcEditors.Items)
            {
                TextEditor edit = GetEditor(page);
                if (edit != null)
                    edit.DrawLineBookmarks = drawLineBookmarks;
            }
        }

        private TextEditor GetActiveSyntaxEdit()
        {
            // getting syntaxedit being focused
            return GetEditor(tcEditors.SelectedItem as TabItem);
        }

        private void OpenProject()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
                return;

            NewFile(dir + @"Resources\Editor\Text\child1.cs");
            NewFile(dir + @"Resources\Editor\Text\child2.cs");
            NewFile(dir + @"Resources\Editor\Text\main.cs");
            TextEditor edit = GetActiveSyntaxEdit();
        }

        private string ExtractFileName(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new FileInfo(fileName);
                return info.Name;
            }
            else
                return string.Empty;
        }

        private void NewFile(string fileName)
        {
            CsParser parser = new CsParser();

            TabItem page = new TabItem();
            page.Header = ExtractFileName(fileName);
            tcEditors.Items.Add(page);

            TextEditor edit = new TextEditor();
            edit.LineNumbersVisible = true;

            TextSource source = new TextSource();
            editors.Add(page, edit);
            page.Content = edit;
            edit.Source = source;
            edit.Source.Lexer = parser;

            edit.Source.BookMarks.Shared = true;
            DrawBookmarks = edit.PaintBookMarks;
            DrawLineBookmarks = edit.DrawLineBookmarks;

            BookMarkManager.Register(source);
            BookMarkManager.SharedBookMarks.Activate += new EventHandler<ActivateEventArgs>(DoActivate);

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                parser.FileName = fileName;
                edit.Source.LoadFile(fileName);
                edit.Source.FileName = fileName;
            }

            Random rnd = new Random();
            edit.Source.BookMarks.ToggleBookMark(rnd.Next(edit.Lines.Count - 1), int.MaxValue);
            tcEditors.SelectedIndex = tcEditors.Items.Count - 1;
        }

        private void DoActivate(object sender, ActivateEventArgs e)
        {
            foreach (TabItem page in editors.Keys)
            {
                TextEditor edit = GetEditor(page);
                if (edit != null && edit.Source != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                {
                    tcEditors.SelectedItem = page;
                    e.Source = edit.Source;
                    break;
                }
            }
        }

        private void SetBookmarkClick()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                BookMarkManager.SharedBookMarks.ToggleBookMark(edit.Position, 1, int.MaxValue, -1, string.Empty, string.Empty, string.Empty, null, edit.Source.FileName);
            }
        }

        private void SetIndexBookmarkClick()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                int index = BookMarkManager.SharedBookMarks.NextBookMark(1, edit.Source.FileName);
                BookMarkManager.SharedBookMarks.ToggleBookMark(edit.Position, 1, index, -1, string.Empty, string.Empty, string.Empty, null, edit.Source.FileName);
            }
        }

        private void SetUnindexedBookmark(int line)
        {
            TextEditor edit = GetActiveSyntaxEdit();
            int imgIndex = int.MaxValue;
            if (edit != null)
                edit.Source.BookMarks.SetBookMark(edit.Position, imgIndex);
        }

        private void ClearBookmarkClick()
        {
            foreach (TabItem page in tcEditors.Items)
            {
                TextEditor edit = GetEditor(page);
                if (edit != null)
                    edit.Source.BookMarks.ClearAllBookMarks();
            }
        }

        private void SetCustomBookmarkClick()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                int index = BookMarkManager.SharedBookMarks.NextBookMark(1, edit.Source.FileName);
                int imgIndex = 0;
                BookMarkManager.SharedBookMarks.ToggleBookMark(edit.Position, 1, imgIndex, index, "Address", "visit our site", "http://www.alternetsoft.com", null, edit.Source.FileName);
            }
        }

        private void NextBookmarkClick()
        {
            BookMarkManager.SharedBookMarks.GotoNextBookMark();
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.Dispatcher.BeginInvoke(
                    DispatcherPriority.ContextIdle,
                    new Action(delegate()
                {
                    edit.Focus();
                }));
            }
        }

        private void PrevBookmarkClick()
        {
            BookMarkManager.SharedBookMarks.GotoPrevBookMark();
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.Dispatcher.BeginInvoke(
                    DispatcherPriority.ContextIdle,
                    new Action(delegate()
                {
                    edit.Focus();
                }));
            }
        }
    }
}
