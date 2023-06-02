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

using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Scripter;

namespace IsolatedScript
{
    public class ViewModel
    {
        private DisplayPanel displayPanel;
        private bool scriptRunning;
        private DispatcherTimer updateTimer;
        private Stopwatch updateDeltaStopwatch = new Stopwatch();
        private MainWindow window;
        private IScriptEdit edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private Isolated<IsolatedScriptRun> isolated;
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private ScriptLanguage language;

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
            Grid.SetRow(displayPanel, 2);
            if ((window != null) && (window.MainGrid != null))
                window.MainGrid.Children.Add(displayPanel);
            displayPanel.OnRendering += DisplayPanel_OnRendering;

            CreateEditor(window.EditBorder);

            languages.Add("C#");
            languages.Add("VB");
            Language = "C#";

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);
            updateTimer.Tick += UpdateTimer_Tick;
            UpdateButtons();
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
                isolated = new Isolated<IsolatedScriptRun>();
                try
                {
                    MyObject myObject = new MyObject(window.laAngle);
                    isolated.Value.StartScript(edit.Text, myObject, language);
                }
                catch
                {
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
                isolated.Dispose();

                window.laAngle.Content = string.Empty;
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
            sourceFileSubPath = "CallIsolatedMethodWpf.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallIsolatedMethodWpf.vb";
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
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
            edit.RegisterAssemblies(new string[] { "WindowsBase", "PresentationCore", "PresentationFramework" });
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
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

            if (isolated != null)
            {
                isolated.Value.UpdateScript((int)updateDeltaStopwatch.ElapsedMilliseconds);
            }

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
            if (isolated != null)
            {
                DrawingContextWrapper wrapper = new DrawingContextWrapper(dc);
                try
                {
                    isolated.Value.RunScript(wrapper, bounds);
                }
                catch
                {
                }
            }

            updateDeltaStopwatch.Restart();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public class MyObject : MarshalByRefObject
    {
        private Label label;

        public MyObject(Label label)
        {
            this.label = label;
        }

        public void UpdateCurrentAngle(double currentAngle)
        {
            label.Content = currentAngle.ToString();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public class IsolatedScriptRun : MarshalByRefObject
    {
        private ScriptRun scriptRun = new ScriptRun();

        public void StartScript(string scriptText, MyObject myObject, ScriptLanguage language)
        {
            scriptRun.ScriptLanguage = language;
            scriptRun.ScriptHost.GenerateModulesOnDisk = false;
            scriptRun.ScriptSource.FromScriptCode(scriptText);
            scriptRun.ScriptSource.WithDefaultReferences();
            scriptRun.ScriptSource.References.Add("WindowsBase");
            scriptRun.ScriptSource.References.Add("PresentationCore");
            scriptRun.ScriptSource.References.Add("PresentationFramework");
            scriptRun.GlobalItems.Add(new ScriptGlobalItem("DrawingContextWrapper", typeof(DrawingContextWrapper)));
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
            scriptRun.ScriptSource.SearchPaths.Add(AppDomain.CurrentDomain.BaseDirectory);
            AddScriptItem(myObject);
            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return;
                }
            }
        }

        public void RunScript(DrawingContextWrapper wrapper, Rect rect)
        {
            scriptRun.RunMethod("OnRender", null, new object[] { wrapper, rect });
        }

        public void UpdateScript(int sec)
        {
            scriptRun.RunMethod("OnUpdate", null, new object[] { sec });
        }

        private void AddScriptItem(MyObject myObject)
        {
            ScriptGlobalItem item = new ScriptGlobalItem("MyObject", typeof(MyObject), myObject);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public sealed class Isolated<T> : IDisposable
        where T : MarshalByRefObject
    {
        private AppDomain domain;
        private T value;

        public Isolated()
        {
            domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), null, AppDomain.CurrentDomain.SetupInformation);

            Type type = typeof(T);

            value = (T)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Value
        {
            get
            {
                return value;
            }
        }

        public void Dispose()
        {
            if (domain != null)
            {
                AppDomain.Unload(domain);

                domain = null;
            }
        }
    }
}