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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Microsoft.Win32;
using my.utils;

namespace TextDifference
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private MainWindow window;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Text\";
            OpenFile1Command = new RelayCommand(OpenFile1Click);
            OpenFile2Command = new RelayCommand(OpenFile2Click);
            CompareCommand = new RelayCommand(CompareClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            InitStyles();
            LoadFiles();
            ProcessDifferences();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OpenFile1Command { get; set; }

        public ICommand OpenFile2Command { get; set; }

        public ICommand CompareCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void LoadFiles()
        {
            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\1.txt");
            if (fileInfo.Exists)
            {
                window.syntaxEdit1.LoadFile(fileInfo.FullName);
            }

            fileInfo = new FileInfo(dir + @"Resources\Editor\Text\2.txt");
            if (fileInfo.Exists)
            {
                window.syntaxEdit2.LoadFile(fileInfo.FullName);
            }
        }

        private void InitStyles()
        {
            window.syntaxEdit1.LineNumbersVisible = true;
            window.syntaxEdit2.LineNumbersVisible = true;
            AddStyle(window.syntaxEdit1, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(window.syntaxEdit1, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(window.syntaxEdit1, System.Drawing.Color.FromArgb(192, 192, 192), System.Drawing.SystemColors.Window);
            AddStyle(window.syntaxEdit2, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(window.syntaxEdit2, System.Drawing.Color.FromArgb(239, 203, 5), System.Drawing.Color.Black);
            AddStyle(window.syntaxEdit2, System.Drawing.Color.FromArgb(192, 192, 192), System.Drawing.SystemColors.Window);
        }

        private void AddStyle(TextEditor edit, System.Drawing.Color backColor, System.Drawing.Color foreColor)
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
            for (int i = window.syntaxEdit1.Lines.Count - 1; i >= 0; i--)
            {
                item = window.syntaxEdit1.Lines.GetItem(i);
                if ((item.State & ItemState.ReadOnly) != 0)
                    window.syntaxEdit1.Lines.RemoveAt(i);
            }

            for (int i = window.syntaxEdit2.Lines.Count - 1; i >= 0; i--)
            {
                item = window.syntaxEdit2.Lines.GetItem(i);
                if ((item.State & ItemState.ReadOnly) != 0)
                    window.syntaxEdit2.Lines.RemoveAt(i);
            }
        }

        private void ProcessDifferences()
        {
            window.syntaxEdit1.Lines.BeginUpdate();
            window.syntaxEdit2.Lines.BeginUpdate();
            try
            {
                PrepareLines();
                string s1 = window.syntaxEdit1.Lines.Text;
                string s2 = window.syntaxEdit2.Lines.Text;
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
                        window.syntaxEdit1.Source.LineStyles.SetLineStyle(aItem.StartA + m + offsetA, styleIndex);
                    while (n < aItem.StartB + aItem.insertedB)
                    {
                        window.syntaxEdit2.Source.LineStyles.SetLineStyle(n + offsetB, styleIndex);
                        n++;
                    }

                    if (aItem.deletedA > aItem.insertedB)
                    {
                        readOnlyIndex = aItem.StartB + offsetB;
                        for (int m = 0; m < aItem.deletedA - aItem.insertedB; m++)
                        {
                            window.syntaxEdit2.Lines.Insert(readOnlyIndex, " ");
                            window.syntaxEdit2.Source.LineStyles.SetLineStyle(readOnlyIndex, 2);
                            window.syntaxEdit2.Source.SetLineReadonly(readOnlyIndex, true);
                            offsetB++;
                            readOnlyIndex++;
                        }
                    }

                    if (aItem.insertedB > aItem.deletedA)
                    {
                        readOnlyIndex = aItem.StartA + offsetA;
                        for (int m = 0; m < aItem.insertedB - aItem.deletedA; m++)
                        {
                            window.syntaxEdit1.Lines.Insert(readOnlyIndex, " ");
                            window.syntaxEdit1.Source.LineStyles.SetLineStyle(readOnlyIndex, 2);
                            window.syntaxEdit1.Source.SetLineReadonly(readOnlyIndex, true);
                            offsetA++;
                            readOnlyIndex++;
                        }
                    }
                }
            }
            finally
            {
                window.syntaxEdit1.Lines.EndUpdate();
                window.syntaxEdit2.Lines.EndUpdate();
            }
        }

        private void OpenFile1Click()
        {
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog().Value)
            {
                window.syntaxEdit1.Source.LoadFile(openFileDialog.FileName);
            }
        }

        private void OpenFile2Click()
        {
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog().Value)
            {
                window.syntaxEdit2.Source.LoadFile(openFileDialog.FileName);
            }
        }

        private void CompareClick()
        {
            ProcessDifferences();
        }
    }
}
