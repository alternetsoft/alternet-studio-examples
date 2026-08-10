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
#endif

using Alternet.Common;
using Alternet.Drawing;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.UI;

namespace IsolatedScript
{
    public partial class Form1 : Window
    {
        private Timer? updateTimer;
        private Stopwatch? updateDeltaStopwatch = new();
        private volatile bool scriptRunning;
        private Isolated<IsolatedScriptRun>? isolated;
        private ScriptLanguage language;

        private readonly CsParser csParser = new(new CsSolution());
        private readonly VbParser vbParser = new(new VbSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            cbLanguages.Add("C#");
            cbLanguages.Add("Visual Basic");

            syntaxEdit1.Outlining.AllowOutlining = true;

            updateTimer = new Timer
            {
                Interval = 50,
            };

            cbLanguages.Value = "C#";
            UpdateButtons();

            lbDescription.WordWrap = true;

            ActiveControl = syntaxEdit1;

            syntaxEdit1.Text = "Loading text...";
            laAngle.Text = StringUtils.OneSpace;

            var assemblies = DemoUtils.DefaultScriptAssemblies;
            csParser.Repository.RegisterAssemblies(assemblies);
            vbParser.Repository.RegisterAssemblies(assemblies);

            FormUtils.BindShown(this, () =>
            {
                UpdateSource(0);
                updateTimer.Tick += UpdateTimer_Tick;
                cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;
                ScriptButton.Click += RunScriptButton_Click;
                displayPanel.Paint += DisplayPanel_Paint;
            });
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
                isolated = new Isolated<IsolatedScriptRun>();
                try
                {
                    MyObject myObject = new MyObject(laAngle);
                    isolated.Value?.StartScript(syntaxEdit1.Text, myObject, language);
                }
                catch(Exception ex)
                {
                    BaseObject.Nop(ex);
                }
                //scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text);
                //scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
                //if (!scriptRun.Compiled)
                //{
                //    if (!scriptRun.Compile())
                //    {
                //        var errors = scriptRun.ScriptHost.CompilerErrors
                //        .Select(x => x.ToString()).ToArray();
                //        MessageBox.Show(string.Join("\r\n", errors));
                //        return;
                //    }
                //}

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

                laAngle.Text = StringUtils.OneSpace;
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
                syntaxEdit1.Lexer = null;
            }

            edit.Source.FileName = fileName;
        }

        protected virtual void GetSourceParametersForCSharp(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallIsolatedMethod.cs";
            language = ScriptLanguage.CSharp;
        }

        protected virtual void GetSourceParametersForVisualBasic(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallIsolatedMethod.vb";
            language = ScriptLanguage.VisualBasic;
        }

        protected virtual string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(ResourcesFolderName, sourceFileSubPath);
            return path;
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            switch (index)
            {
                case 0:
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    syntaxEdit1.Lexer = csParser;
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    syntaxEdit1.Lexer = vbParser;
                    break;
            }

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

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var index = cbLanguages.IndexOfValue;
            if (index is null)
                return;
            UpdateSource(index.Value);
        }
    }

    public class MyObject : MarshalByRefObject
    {
        private readonly Label label;

        public MyObject(Label label)
        {
            this.label = label;
        }

        public void UpdateCurrentAngle(double currentAngle)
        {
            try
            {
                label.Text = ((int)currentAngle).ToString();
            }
            catch
            {
                label.Text = StringUtils.OneSpace;
            }
        }
    }

    public class IsolatedScriptRun : MarshalByRefObject
    {
        private readonly ScriptRun scriptRun = new ScriptRun();

        public void StartScript(string scriptText, MyObject myObject, ScriptLanguage language)
        {
            scriptRun.ScriptSource.References.Clear();
            var assemblies = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in assemblies)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }

            scriptRun.ScriptLanguage = language;
            scriptRun.ScriptHost.GenerateModulesOnDisk = false;
            scriptRun.ScriptSource.FromScriptCode(scriptText);

            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
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
        }

        public void RunScript(Alternet.Drawing.Graphics graph, Alternet.Drawing.RectD rect)
        {
            scriptRun.RunMethod("OnPaint", null, new object[] { graph, rect });
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

    public sealed class Isolated<T> : IDisposable
        where T : MarshalByRefObject
    {
        private AssemblyLoadContext assemblyLoadContext;

        private T? value;

        public Isolated()
        {
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