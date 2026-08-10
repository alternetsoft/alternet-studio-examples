#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

#if !NETFRAMEWORK
using System.Reflection;
using System.Runtime.Loader;
using Alternet.Common.DotNet;

#endif

using Alternet.Common.Python;
using Alternet.Drawing;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.Python;
using Alternet.Syntax.Parsers.Python;
using Alternet.UI;

namespace IsolatedScript.Python
{
    public partial class Form1 : Window
    {
        private Timer? updateTimer;
        private Stopwatch? updateDeltaStopwatch = new();
        private volatile bool scriptRunning;
        private Isolated<IsolatedScriptRun>? isolated;

        private PythonNETParser pythonParser1 = new();

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;

            updateTimer = new Timer();
            updateTimer.Interval = 50;
            updateTimer.Tick += UpdateTimer_Tick;

            ScriptButton.Click += RunScriptButton_Click;
            displayPanel.Paint += DisplayPanel_Paint;
            UpdateSource();
            UpdateButtons();

            lbDescription.WordWrap = true;

            ActiveControl = syntaxEdit1;
            
            syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;
            laAngle.Text = " ";
        }

        protected override void DisposeManaged()
        {
            scriptRunning = false;
            SafeDispose(ref updateTimer);

            base.DisposeManaged();
        }

        public void StartScript()
        {
            StopScript();

            BeginInvoke((Action)(() =>
            {
                isolated = new Isolated<IsolatedScriptRun>(this);
                try
                {
                    MyObject myObject = new MyObject(laAngle);
                    isolated.Value?.StartScript(syntaxEdit1.Text, myObject);
                }
                catch(Exception ex)
                {
                    BaseObject.Nop(ex);
                }

                scriptRunning = true;
                UpdateButtons();
                displayPanel.Background = Brushes.Transparent;
                displayPanel.Refresh();
                updateTimer?.Start();
            }));
        }

        public void StopScript()
        {
            BeginInvoke((Action)(() =>
            {
                SafeDispose(ref isolated);

                laAngle.Text = " ";
                scriptRunning = false;
                UpdateButtons();
                updateTimer?.Stop();
                displayPanel.Background = Brushes.Gray;
                displayPanel.Refresh();
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        private void LoadFile(SyntaxEdit edit, string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            var exists = fileInfo.Exists;
            ScriptButton.Enabled = exists;

            if (exists)
            {
                edit.Source.LoadFile(fileName);
            }
            else
            {
                syntaxEdit1.Lines.Add($"File not found: {fileInfo.FullName}");
            }

            edit.Source.FileName = fileName;
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "CallIsolatedMethod.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(ResourcesFolderName, sourceFileSubPath);
            return path;
        }

        private void UpdateSource()
        {
            syntaxEdit1.Lexer = pythonParser1;
            string sourceFileSubPath;
            GetSourceParametersForPython(out sourceFileSubPath);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(syntaxEdit1, sourceFileFullPath);
        }

        private void UpdateButtons()
        {
            ScriptButton.Text = scriptRunning ? "Stop Script" : "Start Script";
        }

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            if (scriptRunning)
                StopScript();
            else
                StartScript();
        }

        private void DisplayPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (!scriptRunning)
                return;

            isolated?.Value?.RunScript(e.Graphics, displayPanel.ClientRectangle);
            updateDeltaStopwatch?.Restart();
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (!scriptRunning || updateDeltaStopwatch is null)
                return;

            isolated?.Value?.UpdateScript((int)updateDeltaStopwatch.ElapsedMilliseconds);
            updateDeltaStopwatch.Restart();
            displayPanel.Refresh();
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
            label.Text = Math.Round(currentAngle).ToString();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public class IsolatedScriptRun : MarshalByRefObject
    {
        private ScriptRun scriptRun = new ScriptRun();
        private bool started;
        private bool hadException = false;

        public void StartScript(string scriptText, MyObject myObject)
        {
            scriptRun.ScriptSource.SetReferences(DemoUtils.DefaultScriptAssemblies);

            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Diagnostics");
            scriptRun.ScriptSource.Imports.Add("Alternet.UI");

            scriptRun.ScriptSource.FromScriptCode(scriptText);

            AddScriptItem(myObject);
            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                {
                    var errors = scriptRun.ScriptHost.CompilerErrors
                        .Select(x => x.ToString()).ToArray();

                    MessageBox.Show(string.Join("\r\n", errors));
                    return;
                }
            }

            scriptRun.Run();

            started = true;
        }

        public void RunScript(Alternet.Drawing.Graphics graph, Alternet.Drawing.RectD rect)
        {
            if (hadException || !started)
                return;
            try
            {
                scriptRun.RunFunction("OnPaint", new object[] { graph, rect });
            }
            catch
            {
                hadException = true;
                throw;
            }
        }

        public void UpdateScript(int sec)
        {
            if (hadException || !started)
                return;
            try
            {
                scriptRun.RunFunction("OnUpdate", new object[] { sec });
            }
            catch
            {
                hadException = true;
                throw;
            }
        }

        private void AddScriptItem(MyObject myObject)
        {
            IScriptGlobalItem item = new ScriptGlobalItem("MyObject", myObject);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public sealed class Isolated<T> : IDisposable
        where T : MarshalByRefObject
    {
        private Form1 form;
        private AssemblyLoadContext assemblyLoadContext;
        private T? value;

        public Isolated(Form1 form)
        {
            this.form = form;
            assemblyLoadContext = new IsolatedAssemblyLoadContext(
                name: "Isolated:" + Guid.NewGuid(),
                isCollectible: true);
            Type type = typeof(T);
            var asm = assemblyLoadContext.LoadFromAssemblyName(type.Assembly.GetName());
            value = (T?)asm.CreateInstance(type.FullName!);
        }

        public T? Value
        {
            get
            {
                return value;
            }
        }

        public void Dispose()
        {
            assemblyLoadContext.Unload();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public class IsolatedAssemblyLoadContext : AssemblyLoadContext
    {
        public IsolatedAssemblyLoadContext(string name, bool isCollectible = false)
            : base(name, isCollectible: true)
        {
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            return Default.Assemblies
                .FirstOrDefault(x => x.FullName == assemblyName.FullName);
        }
    }
}