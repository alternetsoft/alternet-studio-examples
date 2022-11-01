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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace UndoRedo
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private int undoListCount;
        private bool undoEnabled = false;
        private bool redoEnabled = false;
        private bool lineModificatorsVisible = false;
        private bool groupUndo = false;
        private bool undoNavigations = false;

        private System.Windows.Media.Color changedLineColor;
        private System.Windows.Media.Color savedLineColor;
        private ObservableCollection<string> undoList = new ObservableCollection<string>();

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;
            UndoCommand = new RelayCommand(UndoClick);
            RedoCommand = new RelayCommand(RedoClick);
            SaveCommand = new RelayCommand(SaveClick);
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            if (edit != null)
            {
                this.edit = edit;
                edit.SourceStateChanged += new NotifyEvent(this.SyntaxEdit1_SourceStateChanged);
                edit.Source = csharpSource;
                lineModificatorsVisible = edit.LineModificatorsVisible;
                GroupUndo = (edit.Source.UndoOptions & UndoOptions.GroupUndo) != 0;
                UndoNavigations = (edit.Source.UndoOptions & UndoOptions.UndoNavigations) != 0;
                if (edit.LineModificatorChangedBrush is SolidColorBrush)
                    ChangedLineColor = ((System.Windows.Media.SolidColorBrush)edit.LineModificatorChangedBrush).Color;
                if (edit.LineModificatorSavedBrush is SolidColorBrush)
                    SavedLineColor = ((System.Windows.Media.SolidColorBrush)edit.LineModificatorSavedBrush).Color;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Media.Color ChangedLineColor
        {
            get
            {
                return changedLineColor;
            }

            set
            {
                if (changedLineColor != value)
                {
                    changedLineColor = value;
                    OnPropertyChanged("ChangedLineColor");
                    if (edit != null)
                    {
                        edit.LineModificatorChangedBrush = new System.Windows.Media.SolidColorBrush(changedLineColor);
                    }
                }
            }
        }

        public System.Windows.Media.Color SavedLineColor
        {
            get
            {
                return savedLineColor;
            }

            set
            {
                if (savedLineColor != value)
                {
                    savedLineColor = value;
                    OnPropertyChanged("SavedLineColor");
                    if (edit != null)
                    {
                        edit.LineModificatorSavedBrush = new System.Windows.Media.SolidColorBrush(savedLineColor);
                    }
                }
            }
        }

        public bool UndoEnabled
        {
            get
            {
                return undoEnabled;
            }

            set
            {
                if (undoEnabled != value)
                {
                    undoEnabled = value;
                    OnPropertyChanged("UndoEnabled");
                }
            }
        }

        public bool RedoEnabled
        {
            get
            {
                return redoEnabled;
            }

            set
            {
                if (redoEnabled != value)
                {
                    redoEnabled = value;
                    OnPropertyChanged("RedoEnabled");
                }
            }
        }

        public bool LineModificatorsVisible
        {
            get
            {
                return lineModificatorsVisible;
            }

            set
            {
                if (lineModificatorsVisible != value)
                {
                    lineModificatorsVisible = value;
                    OnPropertyChanged("LineModificatorsVisible");
                    edit.LineModificatorsVisible = lineModificatorsVisible;
                }
            }
        }

        public bool GroupUndo
        {
            get
            {
                return groupUndo;
            }

            set
            {
                if (groupUndo != value)
                {
                    groupUndo = value;
                    OnPropertyChanged("groupUndo");
                    if (edit != null)
                    {
                        edit.Source.UndoOptions = groupUndo ? edit.Source.UndoOptions | UndoOptions.GroupUndo : edit.Source.UndoOptions & ~UndoOptions.GroupUndo;
                    }
                }
            }
        }

        public bool UndoNavigations
        {
            get
            {
                return undoNavigations;
            }

            set
            {
                if (undoNavigations != value)
                {
                    undoNavigations = value;
                    OnPropertyChanged("UndoNavigations");
                    if (edit != null)
                    {
                        edit.Source.UndoOptions = undoNavigations ? edit.Source.UndoOptions | UndoOptions.UndoNavigations : edit.Source.UndoOptions & ~UndoOptions.UndoNavigations;
                    }
                }
            }
        }

        public ObservableCollection<string> UndoList
        {
            get { return undoList; }
            set { undoList = value; }
        }

        public ICommand UndoCommand { get; set; }

        public ICommand RedoCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UndoClick()
        {
            edit.Source.Undo();
            if (edit.Source.UndoList.Count != undoListCount)
                UpdateUndoList();
            edit.Focus();
        }

        private void RedoClick()
        {
            edit.Source.Redo();
            if (edit.Source.UndoList.Count != undoListCount)
                UpdateUndoList();
            edit.Focus();
        }

        private void SaveClick()
        {
            edit.Source.Modified = false;
            edit.Focus();
        }

        private void SyntaxEdit1_SourceStateChanged(object sender, NotifyEventArgs e)
        {
            if (edit.Source.UndoList.Count != undoListCount)
                UpdateUndoList();
        }

        private void UpdateUndoList()
        {
            undoListCount = edit.Source.UndoList.Count;
            undoList.Clear();
            foreach (UndoData undoData in edit.Source.UndoList)
                undoList.Add(string.Format("{0}", undoData.Operation));
            UndoEnabled = edit.Source.UndoList.Count > 0;
            RedoEnabled = edit.Source.RedoList.Count > 0;
        }
    }
}
