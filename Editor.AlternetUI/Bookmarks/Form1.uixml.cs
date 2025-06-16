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
using System.Collections.Generic;
using System.Drawing;

using Alternet.UI;

using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace Bookmarks
{
    public partial class Form1 : Window
    {
        private readonly CsSolution csSolution = new();
        private readonly Dictionary<AbstractControl, SyntaxEdit> editors = [];

        public Form1()
        {
            InitializeComponent();

            Form1_Load(this, EventArgs.Empty);

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
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
            btSetBookmark.Click += SetBookmarkClick;
            btSetUnindexedBookmark.Click += SetUnindexedBookmarkClick;
            btNextBookmark.Click += NexBookmarkClick;
            btClearBookmarks.Click += ClearBookmarksClick;
            btSetCustomBookmarks.Click += SetCustomBookmarkClick;
            btPrevBookmark.Click += PrevBookmarkClick;
            chbDrawBookmarks.CheckedChanged += ShowBookmarksCheckBox_CheckedChanged;
            chbDrawLineBookmarks.CheckedChanged += LineBookmarksCheckBox_CheckedChanged;

            if (CommandLineArgs.ParseAndGetBool("-DevTools"))
            {
                LogUtils.CreateDeveloperTools();
            }

            OpenProject();

            RunWhenIdle(() =>
            {
                foreach (var editor in editors.Values)
                {
                    (editor.Lexer as CsParser)?.ReparseText();
                }
            });

            tcEditors.SelectedPageChanged += (s, e) =>
            {
                var editor = GetEditor(tcEditors.SelectedPage);
                var parser = editor?.Lexer as CsParser;
                parser?.ReparseText();
            };
        }

        private SyntaxEdit? GetEditor(AbstractControl? key)
        {
            if (key != null && editors.TryGetValue(key, out SyntaxEdit? result))
                return result;
            return null;
        }

        private SyntaxEdit? GetActiveSyntaxEdit()
        {
            return GetEditor(tcEditors.SelectedControl);
        }

        private void OpenProject()
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");
            if (!dirInfo.Exists)
                return;

            NewFile(dirInfo.FullName + @"child1.cs");
            NewFile(dirInfo.FullName + @"child2.cs");
            NewFile(dirInfo.FullName + @"main.cs");
            SyntaxEdit? edit = GetActiveSyntaxEdit();
            if (edit is null)
                return;
            chbDrawBookmarks.IsChecked = (GutterOptions.PaintBookMarks & edit.Gutter.Options) != 0;
            chbDrawLineBookmarks.IsChecked = edit.Gutter.DrawLineBookmarks;
        }

        private static string ExtractFileName(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new(fileName);
                return info.Name;
            }
            else
                return string.Empty;
        }

        private void NewFile(string fileName)
        {
            CsParser parser = new(csSolution);

            parser.TextReparsed += (s, e) =>
            {
                if(s is CsParser csp)
                    App.IdleLog($"Parser.TextReparsed: {csp.FileName}");
            };

            TabPage page = new(ExtractFileName(fileName));

            SyntaxEdit edit = new();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                edit.VisualThemeType = VisualThemeType.Dark;
            }

            TextSource source = new();
            editors.Add(page, edit);
            edit.Source = source;
            edit.Dock = DockStyle.Fill;
            edit.Outlining.AllowOutlining = true;
            edit.Gutter.DrawLineBookmarks = true;
            edit.Gutter.Options |=
                GutterOptions.PaintLineNumbers |
                GutterOptions.PaintCodeActions |
                GutterOptions.PaintBookMarks |
                GutterOptions.PaintLinesBeyondEof;
            edit.Gutter.Options &= ~(
                GutterOptions.PaintCodeActionsOnGutter | GutterOptions.PaintLinesOnGutter);

            edit.Parent = page;
            edit.Source.Lexer = parser;

            tcEditors.Pages.Add(page);

            edit.Source.BookMarks.Shared = true;
            BookMarkManager.Register(source);
            BookMarkManager.SharedBookMarks.Activate += DoActivate;

            FileInfo fileInfo = new(fileName);

            if (fileInfo.Exists)
            {
                parser.FileName = fileName;
                edit.Source.LoadFile(fileName);
                edit.Source.FileName = fileName;
                parser.Prepare(fileName, edit.Lines);
                parser.ReparseText();
            }
            else
            {
                edit.Source.Lines.Clear();
                edit.Source.Lines.Add($"File not found: {fileName}");
            }

            Random rnd = new();
            if(edit.Lines.Count > 0)
                edit.Source.BookMarks.ToggleBookMark(rnd.Next(edit.Lines.Count - 1), int.MaxValue);
            tcEditors.SelectedControl = page;
        }

        private void DoActivate(object? sender, ActivateEventArgs e)
        {
            foreach (var page in editors.Keys)
            {
                var edit = GetEditor(page);
                if (edit != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                {
                    tcEditors.SelectedControl = page;
                    e.Source = edit.Source;
                    break;
                }
            }
        }

        private void SetBookmark()
        {
            var edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                int index = BookMarkManager.SharedBookMarks.NextBookMark(1, edit.Source.FileName);
                BookMarkManager.SharedBookMarks.ToggleBookMark(
                    edit.Position,
                    1,
                    index,
                    -1,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    null,
                    edit.Source.FileName);
            }
        }

        private void SetUnindexedBookmark(Point pos)
        {
            var edit = GetActiveSyntaxEdit();
            int imgIndex = int.MaxValue;
            if (edit != null)
                edit.Source.BookMarks.SetBookMark(pos, imgIndex);
        }

        private void ShowBookmarksCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            foreach (var page in tcEditors.Pages)
            {
                var edit = GetEditor(page);
                if (edit != null)
                {
                    edit.Gutter.Options = chbDrawBookmarks.IsChecked ? edit.Gutter.Options
                | GutterOptions.PaintBookMarks : edit.Gutter.Options & ~GutterOptions.PaintBookMarks;
                }
            }
        }

        private void LineBookmarksCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            foreach (var page in tcEditors.Pages)
            {
                var edit = GetEditor(page);
                if (edit != null)
                    edit.Gutter.DrawLineBookmarks = chbDrawLineBookmarks.IsChecked;
            }
        }

        private void SetBookmarkClick(object? sender, EventArgs e)
        {
            SetBookmark();
        }

        private void ClearBookmarksClick(object? sender, EventArgs e)
        {
            foreach (var page in tcEditors.Pages)
            {
                var edit = GetEditor(page);
                if (edit != null)
                    edit.Source.BookMarks.ClearAllBookMarks();
            }
        }

        private void SetUnindexedBookmarkClick(object? sender, EventArgs e)
        {
            var edit = GetActiveSyntaxEdit();
            if (edit != null)
                SetUnindexedBookmark(edit.Position);
        }

        private void SetCustomBookmarkClick(object? sender, EventArgs e)
        {
            var edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                int index = BookMarkManager.SharedBookMarks.NextBookMark(1, edit.Source.FileName);
                int imgIndex = edit.Gutter.AlphaImages.Images.Count - new Random().Next(3) - 1;
                BookMarkManager.SharedBookMarks.ToggleBookMark(
                    edit.Position,
                    1,
                    index,
                    imgIndex,
                    "Address",
                    "visit our site",
                    "http://www.alternetsoft.com",
                    null,
                    edit.Source.FileName);
            }
        }

        private void NexBookmarkClick(object? sender, EventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoNextBookMark();
            var edit = GetActiveSyntaxEdit();
            tcEditors.SelectedPage = edit?.Parent;
            edit?.SetFocusIfPossible();
        }

        private void PrevBookmarkClick(object? sender, EventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoPrevBookMark();
            var edit = GetActiveSyntaxEdit();
            tcEditors.SelectedPage = edit?.Parent;
            edit?.SetFocusIfPossible();
        }
    }
}
