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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

#if !NETFRAMEWORK
using System.Reflection;
using System.Runtime.Loader;
#endif

using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;

namespace IsolatedScript
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private Timer updateTimer;
        private Stopwatch updateDeltaStopwatch = new Stopwatch();
        private volatile bool scriptRunning;
        private ScriptLanguage language;

        private IScriptEdit edit;

        private Isolated<IsolatedScriptRun> isolated;

        public MainForm()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "IsolatedScript.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

            CreateEditor(pnEdit);

            updateTimer = new Timer();
            updateTimer.Interval = 50;
            updateTimer.Tick += UpdateTimer_Tick;
            UpdateButtons();

            cbLanguages.SelectedIndex = 0;
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                isolated = new Isolated<IsolatedScriptRun>();
                try
                {
                    MyObject myObject = new MyObject(laAngle);
                    isolated.Value.StartScript(edit.Text, myObject, language);
                }
                catch
                {
                }

                scriptRunning = true;
                UpdateButtons();
                displayPanel.Refresh();
                updateTimer.Start();
            }));
        }

        public void StopScript()
        {
            BeginInvoke((Action)(() =>
            {
                isolated.Dispose();

                laAngle.Text = string.Empty;
                scriptRunning = false;
                UpdateButtons();
                updateTimer.Stop();
                displayPanel.Refresh();
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        private IScriptEdit CreateEditor(string fileName, Control parent)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();
            edit.FileName = fileName;
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            LoadFile(edit, fileName);

            return edit;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor(Control parent)
        {
            string sourceFileSubPath;
            GetSourceParametersForCSharp(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, parent);
        }

        private void UpdateButtons()
        {
            runScriptButton.Text = scriptRunning ? "Stop Script" : "Start Script";
        }

        private void RunScriptButton_Click(object sender, EventArgs e)
        {
            if (scriptRunning)
                StopScript();
            else
                StartScript();
        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!scriptRunning)
                return;

            if (isolated != null)
            {
                isolated.Value.RunScript(e.Graphics, displayPanel.ClientRectangle);
            }

            updateDeltaStopwatch.Restart();
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
            displayPanel.Refresh();
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallIsolatedMethod.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallIsolatedMethod.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            switch (index)
            {
                case 0:
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSource(cbLanguages.SelectedIndex);
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
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
            label.Text = currentAngle.ToString();
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
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
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

        public void RunScript(Graphics graph, Rectangle rect)
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public sealed class Isolated<T> : IDisposable
        where T : MarshalByRefObject
    {
#if NETFRAMEWORK
        private AppDomain domain;
#else
        private AssemblyLoadContext assemblyLoadContext;
#endif

        private T value;

        public Isolated()
        {
#if NETFRAMEWORK
            domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), null, AppDomain.CurrentDomain.SetupInformation);
            Type type = typeof(T);
            value = (T)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
#else
            assemblyLoadContext = new IsolatedAssemblyLoadContext(name: "Isolated:" + Guid.NewGuid(), isCollectible: true);
            Type type = typeof(T);
            var asm = assemblyLoadContext.LoadFromAssemblyName(type.Assembly.GetName());
            value = (T)asm.CreateInstance(type.FullName);
#endif
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
#if NETFRAMEWORK
            if (domain != null)
            {
                AppDomain.Unload(domain);

                domain = null;
            }
#else
            assemblyLoadContext.Unload();
#endif
        }
    }

#if !NETFRAMEWORK

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
    public class IsolatedAssemblyLoadContext : AssemblyLoadContext
    {
        public IsolatedAssemblyLoadContext(string name, bool isCollectible = false)
            : base(name, isCollectible: true)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return Default.Assemblies
                .FirstOrDefault(x => x.FullName == assemblyName.FullName);
        }
    }
#endif

}
