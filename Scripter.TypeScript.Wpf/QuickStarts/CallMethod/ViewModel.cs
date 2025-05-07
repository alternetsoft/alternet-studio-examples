#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Common.TypeScript;

using Alternet.Editor.Common.Wpf;
using Alternet.Editor.TypeScript.Wpf;
using Alternet.Scripter.TypeScript;
using Microsoft.ClearScript;
using Host = Alternet.Common.TypeScript.HostObjects;

namespace CallMethod.TypeScript
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

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            InitDefaultHostAssemblies();

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

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);
            updateTimer.Tick += UpdateTimer_Tick;
            UpdateButtons();

            languages.Add("TypeScript");
            languages.Add("JavaScript");
            Language = "TypeScript";
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

        public bool CompileScript()
        {
            scriptRun.ScriptSource.FromScriptCode(edit.Text);
            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return false;
                }
            }

            return true;
        }

        public void StartScript()
        {
            if (!CompileScript())
                return;
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
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

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(TechnologyEnvironment.Wpf, options: Host.HostItemOptions.GlobalMembers | Host.HostItemOptions.GenerateDescriptions);
            TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\CallMethodWpf.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\CallMethodWpf.js";
            language = ScriptLanguage.JavaScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript";
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
                case "TypeScript":
                    GetSourceParametersForTS(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath, out language);
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
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForTS(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, border);
        }

        private IScriptEdit CreateEditor(string fileName, Border border)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();

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

            CatchAndDisplayExceptions(() => scriptRun.RunFunction("OnUpdate", new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds }));
            updateDeltaStopwatch.Restart();
            displayPanel.InvalidateVisual();
        }

        private void CatchAndDisplayExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException tex)
            {
                StopScript();
                string message;
                if (tex.InnerException is ScriptEngineException)
                    message = ((ScriptEngineException)tex.InnerException).ErrorDetails;
                else
                    message = tex.ToString();
                window.Dispatcher.BeginInvoke((Action)(() => MessageBox.Show(message)));
            }
            catch (Exception e)
            {
                StopScript();
                window.Dispatcher.BeginInvoke((Action)(() => MessageBox.Show(e.ToString())));
            }
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

            // Workaround for ClearScript bug: https://github.com/microsoft/ClearScript/issues/112
            var host = (ExtendedHostFunctions)((ClearScriptScriptHost)scriptRun.ScriptHost).Script.host;
            var castedDc = host.cast<DrawingContext>(dc);

            Rect bounds = VisualTreeHelper.GetDescendantBounds(displayPanel);
            CatchAndDisplayExceptions(() => scriptRun.RunFunction("OnRender", new object[] { castedDc, bounds }));
            updateDeltaStopwatch.Restart();
        }
    }
}