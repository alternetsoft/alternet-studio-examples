#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;

namespace SearchReplace
{
    public partial class Form1 : Form
    {
        private const string FindDesc = "Display Search Dialog";
        private const string ReplaceDesc = "Display Replace Dialog";
        private const string GotoDesc = "Display Goto Line Dialog";
        private const string LanguageDesc = "Choose dialogs language";
        private const string SearchMultiDocDesc = "Select search mode one or several documents";

        private string dir = Application.StartupPath + @"\";
        private IDictionary<TabPage, ISyntaxEdit> editors = new Dictionary<TabPage, ISyntaxEdit>();
        private KeyEvent findEvent;
        private bool searchMulti = false;

        public Form1()
        {
            InitializeComponent();
        }

        public void DoFind()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.DisplaySearchDialog();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            SearchManager.SharedSearch.InitSearch += new InitSearchEvent(DoInitSearch);
            SearchManager.SharedSearch.GetSearch += new GetSearchEvent(DoGetSearch);
            OpenProject();
            cbLanguages.SelectedIndex = 0;
        }

        private ISyntaxEdit GetEditor(TabPage key)
        {
            ISyntaxEdit result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private string ExtractFileName(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new FileInfo(fileName);
                return info.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        private ISyntaxEdit GetActiveSyntaxEdit()
        {
            // getting syntaxedit being focused
            return GetEditor(tcEditors.SelectedTab);
        }

        private void UpdateSearch()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                ISearch search = (ISearch)edit;
                search.SearchGlobal = searchMulti;
            }
        }

        private void DoGetSearch(object sender, GetSearchEventArgs e)
        {
            foreach (TabPage page in editors.Keys)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                {
                    tcEditors.SelectedTab = page;
                    UpdateSearch();
                    e.Search = (ISearch)edit;
                    break;
                }
            }
        }

        private void DoInitSearch(object sender, InitSearchEventArgs e)
        {
            e.Search = GetActiveSyntaxEdit();
            foreach (ISyntaxEdit edit in editors.Values)
            {
                edit.SearchGlobal = searchMulti;
                if (edit.Source != null)
                    e.SearchList.Add(edit.Source.FileName);
            }
        }

        private void OpenProject()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                return;
            }

            NewFile(dir + @"Resources\Editor\text\child1.cs");
            NewFile(dir + @"Resources\Editor\text\child2.cs");
            NewFile(dir + @"Resources\Editor\text\main.cs");
        }

        private void NewFile(string fileName)
        {
            CsParser parser = new CsParser();

            TabPage page = new TabPage(ExtractFileName(fileName));
            tcEditors.TabPages.Add(page);

            ISyntaxEdit edit = new SyntaxEdit();
            edit.Selection.Options |= SelectionOptions.DrawBorder;
            TextSource source = new TextSource();
            editors.Add(page, edit);
            edit.Source = source;
            edit.Dock = DockStyle.Fill;
            edit.Bounds = new Rectangle(0, 0, page.ClientRectangle.Width, page.ClientRectangle.Height);
            page.Controls.Add(edit as Control);
            edit.Source.Lexer = parser;

            findEvent = new KeyEvent(DoFind);
            edit.KeyList.Remove(Keys.F | Keys.Control);
            edit.KeyList.Add(Keys.F | Keys.Control, findEvent);

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                parser.FileName = fileName;
                edit.LoadFile(fileName);
                edit.Source.FileName = fileName;
            }

            UpdateSearch();

            tcEditors.SelectedTab = page;
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.DisplaySearchDialog();
            }
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            edit.DisplayReplaceDialog();
        }

        private void GotoButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.DisplayGotoLineDialog();
            }
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CultureInfo oldcInfo = Thread.CurrentThread.CurrentUICulture;
            switch (cbLanguages.SelectedIndex)
            {
                case 0:
                    StringConsts.Localize();
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 2:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 3:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 4:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 5:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 6:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
            }
        }

        private void FindButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(FindNextButton);
            if (str != FindDesc)
            {
                toolTip1.SetToolTip(FindNextButton, FindDesc);
            }
        }

        private void ReplaceButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btReplace);
            if (str != ReplaceDesc)
            {
                toolTip1.SetToolTip(btReplace, ReplaceDesc);
            }
        }

        private void GotoButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btGoto);
            if (str != GotoDesc)
            {
                toolTip1.SetToolTip(btGoto, GotoDesc);
            }
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDesc)
            {
                toolTip1.SetToolTip(cbLanguages, LanguageDesc);
            }
        }

        private void UpdateSearchModel()
        {
            SearchManager.SharedSearch.Shared = searchMulti;
            foreach (TabPage page in tcEditors.TabPages)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null)
                {
                    edit.SearchDialog?.Close();
                    edit.SearchGlobal = searchMulti;
                }
            }
        }

        private void SearchMultiDocCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            searchMulti = chbSearchMultiDoc.Checked;
            UpdateSearchModel();
        }

        private void SearchMultiDocCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSearchMultiDoc);
            if (str != SearchMultiDocDesc)
            {
                toolTip1.SetToolTip(chbSearchMultiDoc, SearchMultiDocDesc);
            }
        }
    }
}
