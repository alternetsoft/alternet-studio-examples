#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Scripter;

namespace CallMethod
{
    public class ViewModel
    {
        private DisplayPanel displayPanel;
        private bool scriptRunning;
        private ScriptRun scriptRun = new ScriptRun();
        private DispatcherTimer updateTimer;
        private Stopwatch updateDeltaStopwatch = new Stopwatch();
        private MainWindow window;
        private IScriptEdit edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private IDefaultAssembliesProvider defaultAssembliesProvider = DefaultAssembliesProviderFactory.CreateDefaultAssembliesProvider();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            displayPanel = new DisplayPanel();
            displayPanel.Width = 150;
            displayPanel.Height = 150;
            displayPanel.Background = new SolidColorBrush(Colors.Black);
            displayPanel.Margin = new Thickness(0, 5, 0, 0);
            Grid.SetRow(displayPanel, 1);
            if ((window != null) && (window.MainGrid != null))
                window.MainGrid.Children.Add(displayPanel);
            displayPanel.OnRendering += DisplayPanel_OnRendering;

            CreateEditor(window.EditBorder);

            scriptRun.ScriptMode = ScriptMode.Debug;
            scriptRun.ScriptHost.GenerateModulesOnDisk = false;

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);
            updateTimer.Tick += UpdateTimer_Tick;
            UpdateButtons();

            languages.Add("C#");
            languages.Add("VB");
            Language = "C#";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string Language
        {
            get
            {
                return lang;
            }

            set
            {
                if (lang != value)
                {
                    lang = value;
                    OnPropertyChanged("Language");
                    UpdateSource();
                }
            }
        }

        public ICommand RunScript { get; set; }

        public void StartScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(edit.Text);
                scriptRun.ScriptSource.WithDefaultReferences();
                scriptRun.ScriptSource.References.Add("WindowsBase");
                scriptRun.ScriptSource.References.Add("PresentationCore");
                scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                        return;
                    }
                }

                displayPanel.Background = new SolidColorBrush(Colors.Transparent);
                scriptRunning = true;
                UpdateButtons();
                displayPanel.InvalidateVisual();
                updateTimer.Start();
            }));
        }

        public void StopScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRunning = false;
                UpdateButtons();
                updateTimer.Stop();
                displayPanel.Background = new SolidColorBrush(Colors.Black);
                displayPanel.InvalidateVisual();
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethodWpf.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethodWpf.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource()
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (lang)
            {
                case "C#":
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
            edit.RegisterAssemblies(defaultAssembliesProvider.GetDefaultAssemblies(Alternet.Common.TechnologyEnvironment.Wpf));
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForCSharp(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, border);
        }

        private IScriptEdit CreateEditor(string fileName, Border border)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();
            edit.FileName = fileName;

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);

            return edit;
        }

        private void UpdateButtons()
        {
            if (window != null)
                window.ScriptButton.Content = scriptRunning ? "Stop Script" : "Start Script";
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!scriptRunning)
                return;

            scriptRun.RunMethod("OnUpdate", null, new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds });
            updateDeltaStopwatch.Restart();
            displayPanel.InvalidateVisual();
        }

        private void RunScriptClick()
        {
            if (scriptRunning)
                StopScript();
            else
                StartScript();
        }

        private void DisplayPanel_OnRendering(object sender, DrawingContext dc)
        {
            if (!scriptRunning)
                return;

            Rect bounds = VisualTreeHelper.GetDescendantBounds(displayPanel);
            scriptRun.RunMethod("OnRender", null, new object[] { dc, bounds });
            updateDeltaStopwatch.Restart();
        }
    }
}