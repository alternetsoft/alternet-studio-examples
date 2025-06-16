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
using Alternet.Syntax;
using my.utils;

namespace TextDifference
{
    public partial class Form1 : Form
    {
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "TextDifference.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Text\";
            InitStyles();
            LoadFiles();
            ProcessDifferences();
        }


        private void LoadFiles()
        {
            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\1.txt");
            if (fileInfo.Exists)
            {
                syntaxEdit1.LoadFile(fileInfo.FullName);
            }

            fileInfo = new FileInfo(dir + @"Resources\Editor\Text\2.txt");
            if (fileInfo.Exists)
            {
                syntaxEdit2.LoadFile(fileInfo.FullName);
            }
        }

        private void InitStyles()
        {
            syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;
            syntaxEdit2.Gutter.Options |= GutterOptions.PaintLineNumbers;
            AddStyle(syntaxEdit1, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(syntaxEdit1, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(syntaxEdit1, System.Drawing.Color.FromArgb(192, 192, 192), System.Drawing.SystemColors.Window);
            AddStyle(syntaxEdit2, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(syntaxEdit2, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(syntaxEdit2, System.Drawing.Color.FromArgb(192, 192, 192), System.Drawing.SystemColors.Window);
        }

        private void AddStyle(SyntaxEdit edit, System.Drawing.Color backColor, System.Drawing.Color foreColor)
        {
            var style = new EditLineStyle();
            style.BackColor = backColor;
            style.ForeColor = foreColor;
            style.Options = LineStyleOptions.BeyondEol;
            edit.LineStyles.Add(style);
        }

        private void PrepareLines()
        {
            IStringItem item = null;
            for (int i = syntaxEdit1.Lines.Count - 1; i >= 0; i--)
            {
                item = syntaxEdit1.Lines.GetItem(i);
                if ((item.State & ItemState.ReadOnly) != 0)
                    syntaxEdit1.Lines.RemoveAt(i);
            }

            for (int i = syntaxEdit2.Lines.Count - 1; i >= 0; i--)
            {
                item = syntaxEdit2.Lines.GetItem(i);
                if ((item.State & ItemState.ReadOnly) != 0)
                    syntaxEdit2.Lines.RemoveAt(i);
            }
        }

        private void ProcessDifferences()
        {
            ILineStyles safeStyles = new LineStyles();

            syntaxEdit1.Lines.BeginUpdate();
            syntaxEdit2.Lines.BeginUpdate();
            syntaxEdit1.Source.BeginUpdate();
            syntaxEdit2.Source.BeginUpdate();
            try
            {
                PrepareLines();
                string s1 = syntaxEdit1.Lines.Text;
                string s2 = syntaxEdit2.Lines.Text;
                Diff.Item[] f = Diff.DiffText(s1, s2, true, true, false);

                int n = 0;
                int styleIndex = -1;
                int offsetA = 0;
                int offsetB = 0;
                int readOnlyIndex = -1;
                for (int fdx = 0; fdx < f.Length; fdx++)
                {
                    Diff.Item aItem = f[fdx];
                    styleIndex = (aItem.deletedA == aItem.insertedB) ? 0 : 1;
                    n = aItem.StartB;

                    for (int m = 0; m < aItem.deletedA; m++)
                        syntaxEdit1.Source.LineStyles.SetLineStyle(aItem.StartA + m + offsetA, styleIndex);
                    while (n < aItem.StartB + aItem.insertedB)
                    {
                        syntaxEdit2.Source.LineStyles.SetLineStyle(n + offsetB, styleIndex);
                        n++;
                    }

                    if (aItem.deletedA > aItem.insertedB)
                    {
                        readOnlyIndex = aItem.StartB + offsetB;
                        for (int m = 0; m < aItem.deletedA - aItem.insertedB; m++)
                        {
                            syntaxEdit2.Lines.Insert(readOnlyIndex, " ");
                            syntaxEdit2.Source.LineStyles.SetLineStyle(readOnlyIndex, 2);
                            syntaxEdit2.Source.SetLineReadonly(readOnlyIndex, true);
                            offsetB++;
                            readOnlyIndex++;
                        }
                    }

                    if (aItem.insertedB > aItem.deletedA)
                    {
                        readOnlyIndex = aItem.StartA + offsetA;
                        for (int m = 0; m < aItem.insertedB - aItem.deletedA; m++)
                        {
                            syntaxEdit1.Lines.Insert(readOnlyIndex, " ");
                            syntaxEdit1.Source.LineStyles.SetLineStyle(readOnlyIndex, 2);
                            syntaxEdit1.Source.SetLineReadonly(readOnlyIndex, true);
                            offsetA++;
                            readOnlyIndex++;
                        }
                    }
                }
            }
            finally
            {
                foreach (ILineStyle ls in syntaxEdit1.Source.LineStyles)
                    safeStyles.Add(new LineStyle(ls.Line, ls.Pos, ls.Index, ls.Priority, ls.Range));
                syntaxEdit1.Source.EndUpdate();
                syntaxEdit2.Source.EndUpdate();
                syntaxEdit1.Lines.EndUpdate();
                syntaxEdit2.Lines.EndUpdate();
            }

            syntaxEdit1.Source.LineStyles = safeStyles;
        }

        private void OpenFile1MenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (sender.Equals(OpenFile1MenuItem))
                    syntaxEdit1.Source.LoadFile(openFileDialog.FileName);
                else
                    syntaxEdit2.Source.LoadFile(openFileDialog.FileName);
            }
        }

        private void CompareMenuItem_Click(object sender, EventArgs e)
        {
            ProcessDifferences();
        }
    }
}
