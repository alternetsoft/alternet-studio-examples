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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;

namespace Bookmarks
{
    public partial class Form1 : Form
    {
        private const string ShowBookmarksDesc = "Show or hide bookmarks";
        private const string LineBookmarksDesc = "Display triangle at bookmark position inside the text";
        private const string SetBookmarksDesc = "Set an indexed bookmark at the current Edit position";
        private const string ClearBookmarksDesc = "Clear all bookmarks";
        private const string UnindexBookmarksDesc = "Set an unindexed bookmark at the current Edit position";
        private const string SetUrlBookmarksDesc = "Set a bookmark with url and description at the current Edit position";
        private const string NextBookmarksDesc = "Move Edit control's caret to the location of the next unindexed bookmark";
        private const string PrevBookmarksDesc = "Move Edit control's caret to the location of the previous unindexed bookmark";

        private string dir = Application.StartupPath + @"\";

        private IDictionary<TabPage, ISyntaxEdit> editors = new Dictionary<TabPage, ISyntaxEdit>();
        private ImageList customGutterImages = new ImageList();

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "Bookmarks.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            customGutterImages = LoadImageList("CustomImages");
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            OpenProject();

            foreach (var editor in editors.Values)
            {
                (editor.Lexer as CsParser)?.ReparseText();
            }

            tcEditors.SelectedIndexChanged += (s, x) =>
            {
                var editor = GetEditor(tcEditors.SelectedTab);
                var parser = editor?.Lexer as CsParser;
                parser?.ReparseText();
            };
        }

        private ImageList LoadImageList(string resource)
        {
            return ImageListHelper.LoadImageListFromStrip(typeof(Form1).Assembly, string.Format("Bookmarks.Resources.{0}.png", resource));
        }

        private ISyntaxEdit GetEditor(TabPage key)
        {
            ISyntaxEdit result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private ISyntaxEdit GetActiveSyntaxEdit()
        {
            // getting syntaxedit being focused
            return GetEditor(tcEditors.SelectedTab);
        }

        private void OpenProject()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
                return;

            NewFile(dir + @"Resources\Editor\text\child1.cs");
            NewFile(dir + @"Resources\Editor\text\child2.cs");
            NewFile(dir + @"Resources\Editor\text\main.cs");
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            chbShowBookmarks.Checked = (GutterOptions.PaintBookMarks & edit.Gutter.Options) != 0;
            chbLineBookmarks.Checked = edit.Gutter.DrawLineBookmarks;
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

            TabPage page = new TabPage(ExtractFileName(fileName));
            tcEditors.TabPages.Add(page);

            ISyntaxEdit edit = new SyntaxEdit();
            TextSource source = new TextSource();
            editors.Add(page, edit);
            edit.Source = source;
            edit.Dock = DockStyle.Fill;
            edit.Bounds = new Rectangle(0, 0, page.ClientRectangle.Width, page.ClientRectangle.Height);
            page.Controls.Add(edit as Control);
            edit.Source.Lexer = parser;

            edit.Source.BookMarks.Shared = true;
            BookMarkManager.Register(source);
            BookMarkManager.SharedBookMarks.Activate += new EventHandler<ActivateEventArgs>(DoActivate);

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                parser.FileName = fileName;
                edit.LoadFile(fileName);
                edit.Source.FileName = fileName;

                parser.Prepare(fileName, edit.Lines);
                parser.ReparseText();
            }

            Random rnd = new Random();
            edit.Source.BookMarks.ToggleBookMark(rnd.Next(edit.Lines.Count - 1), int.MaxValue);
            tcEditors.SelectedTab = page;

            edit.Gutter.AlphaImages = InitCustomImages(edit.Gutter.AlphaImages);
            edit.Gutter.AlphaImagesHighDpi = InitCustomImages(edit.Gutter.AlphaImagesHighDpi);
        }

        private AlphaImageList InitCustomImages(AlphaImageList alphaImages)
        {
            IList<Image> images = new List<Image>();

            foreach (Image image in alphaImages.Images)
                images.Add(image);

            foreach (Image image in customGutterImages.Images)
                images.Add(image);
            return new AlphaImageList(images, alphaImages.ImageSize);
        }

        private void DoActivate(object sender, ActivateEventArgs e)
        {
            foreach (TabPage page in editors.Keys)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                {
                    tcEditors.SelectedTab = page;
                    e.Source = edit.Source;
                    break;
                }
            }
        }

        private void SetBookmark()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                int index = BookMarkManager.SharedBookMarks.NextBookMark(1, edit.Source.FileName);
                BookMarkManager.SharedBookMarks.ToggleBookMark(edit.Position, 1, index, -1, string.Empty, string.Empty, string.Empty, null, edit.Source.FileName);
            }
        }

        private void SetUnindexedBookmark(Point pos)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            int imgIndex = int.MaxValue;
            if (edit != null)
                edit.Source.BookMarks.SetBookMark(pos, imgIndex);
        }

        private void ShowBookmarksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TabPage page in tcEditors.TabPages)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null)
                {
                    edit.Gutter.Options = chbShowBookmarks.Checked ? edit.Gutter.Options
                | GutterOptions.PaintBookMarks : edit.Gutter.Options & ~GutterOptions.PaintBookMarks;
                }
            }
        }

        private void LineBookmarksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TabPage page in tcEditors.TabPages)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null)
                    edit.Gutter.DrawLineBookmarks = chbLineBookmarks.Checked;
            }
        }

        private void SeookmarkTextBoxButton_Click(object sender, EventArgs e)
        {
            SetBookmark();
        }

        private void ClearBookmarksButton_Click(object sender, EventArgs e)
        {
            foreach (TabPage page in tcEditors.TabPages)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null)
                    edit.Source.BookMarks.ClearAllBookMarks();
            }
        }

        private void SetUnindexedBookmarksButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
                SetUnindexedBookmark(edit.Position);
        }

        private void SetCustomBookmarkButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                int index = BookMarkManager.SharedBookMarks.NextBookMark(1, edit.Source.FileName);
                int imgIndex = edit.Gutter.AlphaImages.Images.Count - new Random().Next(3) - 1;
                BookMarkManager.SharedBookMarks.ToggleBookMark(edit.Position, 1, imgIndex, imgIndex, "Address", "visit our site", "http://www.alternetsoft.com", null, edit.Source.FileName);
            }
        }

        private void ShowBookmarksCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowBookmarks);
            if (str != ShowBookmarksDesc)
                toolTip1.SetToolTip(chbShowBookmarks, ShowBookmarksDesc);
        }

        private void LineBookmarksCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLineBookmarks);
            if (str != LineBookmarksDesc)
                toolTip1.SetToolTip(chbLineBookmarks, LineBookmarksDesc);
        }

        private void SeookmarkTextBoxButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSetBookmark);
            if (str != SetBookmarksDesc)
                toolTip1.SetToolTip(btSetBookmark, SetBookmarksDesc);
        }

        private void ClearBookmarksButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btClearBookmarks);
            if (str != ClearBookmarksDesc)
                toolTip1.SetToolTip(btClearBookmarks, ClearBookmarksDesc);
        }

        private void SetUnindexedBookmarksButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSetUnindexedBookmarks);
            if (str != UnindexBookmarksDesc)
                toolTip1.SetToolTip(btSetUnindexedBookmarks, UnindexBookmarksDesc);
        }

        private void SetCustomBookmarkButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSetCustomBookmark);
            if (str != SetUrlBookmarksDesc)
                toolTip1.SetToolTip(btSetCustomBookmark, SetUrlBookmarksDesc);
        }

        private void NexookmarkTextBoxButton_Click(object sender, EventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoNextBookMark();
            var edit = GetActiveSyntaxEdit();
            edit?.Focus();
        }

        private void PrevBookmarkButton_Click(object sender, EventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoPrevBookMark();
            var edit = GetActiveSyntaxEdit();
            edit?.Focus();
        }

        private void NexookmarkTextBoxButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btNextBookmark);
            if (str != NextBookmarksDesc)
                toolTip1.SetToolTip(btNextBookmark, NextBookmarksDesc);
        }

        private void PrevBookmarkButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrevBookmark);
            if (str != PrevBookmarksDesc)
                toolTip1.SetToolTip(btPrevBookmark, PrevBookmarksDesc);
        }
    }
}
