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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Integration;

namespace DebugRemoteScript
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string UseRemoteAPIDescription = "Allows to call API directly when executing script, and through .NET remoting wrapper when debugging.";
        private IScriptEdit edit;
        private IScriptAPI scriptAPI;
        private ScriptDebugger scriptDebugger;
        private ExecutionPosition executionPosition;
        private string ipcPortName = null;
        private string ipcObjectUri = null;

        public MainForm(string[] args)
        {
            InitializeComponent();
            ParseCommandLineArgs(args);
            scriptAPI = new ScriptAPI(this);
        }

        public string GetEditorText()
        {
            string result = null;
            Invoke((Action)(() => result = textBox1.Text));
            return result;
        }

        public void ShowMessage(string text)
        {
            Invoke((Action)(() => MessageBox.Show(text)));
        }

        private string ExtractCommandLineArg(string arg)
        {
            int idx = arg.IndexOf("=");
            return idx >= 0 ? arg.Substring(idx + 1) : arg;
        }

        private void ParseCommandLineArgs(string[] args)
        {
            foreach (string arg in args)
            {
                string larg = arg.ToLower();
                if (larg.StartsWith("-ipcportname="))
                    ipcPortName = ExtractCommandLineArg(arg);
                else
                if (larg.StartsWith("-ipcobjecturi="))
                    ipcObjectUri = ExtractCommandLineArg(arg);
            }
        }

        private void ScriptDebugger_DebuggerErrorOccured(object sender, DebuggerErrorOccuredEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }

        private void ScriptDebugger_StateChanged(object sender, DebuggerStateChangedEventArgs e)
        {
            UpdateDebuggerControls(e.NewState);
        }

        private void UpdateDebuggerControls(DebuggerState state)
        {
            switch (state)
            {
                case DebuggerState.Off:
                    ClearExecutionPosition();
                    break;
                case DebuggerState.Startup:
                    break;
                case DebuggerState.Running:
                    ClearExecutionPosition();
                    break;
                case DebuggerState.Stopped:
                    break;
                default:
                    break;
            }
        }

        private void ClearExecutionPosition()
        {
            var debugEdit = edit as IDebugEdit;
            if (debugEdit == null)
                return;

            if (executionPosition != null)
            {
                debugEdit.ClearDebugStyles(executionPosition);
                executionPosition = null;
            }
        }

        private void ScriptDebugger_ExecutionStopped(object sender, ExecutionStoppedEventArgs e)
        {
            var debugEdit = edit as IDebugEdit;
            if (debugEdit == null)
                return;

            ClearExecutionPosition();

            executionPosition = e.Position;

            if (e.Position != null && !string.IsNullOrEmpty(e.Position.File))
            {
                if (Path.GetFullPath(e.Position.File) == Path.GetFullPath(GetScriptFilePath()))
                    debugEdit.ExecutionStopped(e.Position);
                else
                    scriptDebugger.Continue();
            }

            if (e.StopReason == ExecutionStopReason.Exception || e.StopReason == ExecutionStopReason.UnhandledException)
            {
                MessageBox.Show(e.Exception.ToString());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateEditorAndDebugger();

            cbLanguages.SelectedIndex = 0;
            InitializeDebugger();
            UpdateDebuggerControls(DebuggerState.Off);
        }

        private void InitializeDebugger()
        {
            debuggerControlToolbar.Debugger = scriptDebugger;
            debuggerControlToolbar.DebuggerPreStartup += DebuggerPreStartup;
            debugMenu1.Debugger = scriptDebugger;
            debugMenu1.DebuggerPreStartup += DebuggerPreStartup;
        }

        private void DebuggerPreStartup(object sender, EventArgs e)
        {
            scriptRun.AssemblyKind = ScriptAssemblyKind.WindowsApplication;
            RemoteAPI.UseRemoteAPI = true;

            CompileScript();
        }

        private string GetScriptFilePath()
        {
            var sourceFileSubPath = cbLanguages.SelectedIndex == 0 ? GetSourceParametersForCSharp() : GetSourceParametersForVisualBasic();
            return GetSourceFileFullPath(sourceFileSubPath);
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditorAndDebugger()
        {
            edit = CreateEditorAndDebugger(GetScriptFilePath(), pnEdit);
        }

        private IScriptEdit CreateEditorAndDebugger(string fileName, Control parent)
        {
            var thisAssemblyLocation = GetType().Assembly.Location;

            RemoteAPI.ScriptAPI = this.scriptAPI;

            RemoteAPI.StartServer(ipcPortName, ipcObjectUri);

            var scriptSource = scriptRun.ScriptSource;

            scriptSource.WithDefaultReferences();
            scriptSource.References.Add("System.Runtime.Remoting");

            scriptSource.References.Add(thisAssemblyLocation);
            scriptRun.AssemblyKind = ScriptAssemblyKind.WindowsApplication;

            string generatedModulesPath = Path.GetDirectoryName(thisAssemblyLocation);

            scriptDebugger = new ScriptDebugger
            {
                GeneratedModulesPath = generatedModulesPath,
                EventsSyncAction = action => BeginInvoke(action),
                ScriptRun = scriptRun,
            };

            scriptDebugger.ExecutionStopped += ScriptDebugger_ExecutionStopped;
            scriptDebugger.DebuggerErrorOccured += ScriptDebugger_DebuggerErrorOccured;
            scriptDebugger.StateChanged += ScriptDebugger_StateChanged;

            scriptRun.ScriptHost.ModulesDirectoryPath = generatedModulesPath;
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;

            IScriptEdit edit;
            edit = new DebugCodeEdit();
            edit.InitSyntax();
            ((IDebugEdit)edit).Debugger = scriptDebugger;
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            LoadFile(edit, fileName);

            return edit;
        }

        private bool CompileScript()
        {
            if (edit.Modified)
                scriptRun.ScriptSource.FromScriptCode(edit.Text, edit.FileName);

            if (!scriptRun.Compiled)
            {
                scriptRun.ScriptHost.AssemblyFileName = Guid.NewGuid().ToString("N") + (scriptRun.AssemblyKind == ScriptAssemblyKind.WindowsApplication ? ".exe" : ".dll");

                if (!scriptRun.Compile())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return false;
                }
            }

            return true;
        }

        private void StartScript()
        {
            if (!RemoteAPI.UseRemoteAPI)
                scriptRun.Run(new string[] { });
            else
                scriptRun.RunProcess(new string[] { string.Format("-ipcPortName={0}", ipcPortName ?? string.Empty), string.Format("-ipcObjectUri={0}", ipcObjectUri ?? string.Empty) });
        }

        private void UpdateUseDirectAPI()
        {
            RemoteAPI.UseRemoteAPI = !chkUseDirectAPI.Checked;
            scriptRun.AssemblyKind = RemoteAPI.UseRemoteAPI ? ScriptAssemblyKind.WindowsApplication : ScriptAssemblyKind.DynamicLibrary;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            UpdateUseDirectAPI();

            if (CompileScript())
                new Task(StartScript).Start();
        }

        private string GetSourceParametersForCSharp()
        {
            return "DebugRemoteScript.cs";
        }

        private string GetSourceParametersForVisualBasic()
        {
            return "DebugRemoteScript.vb";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource(int index)
        {
            ScriptLanguage language = index == 0 ? ScriptLanguage.CSharp : ScriptLanguage.VisualBasic;
            var sourceFileFullPath = GetScriptFilePath();
            LoadFile(edit, sourceFileFullPath);
            CodeEditExtensions.RegisterAssembly(edit, System.Reflection.Assembly.GetEntryAssembly().Location);
            scriptRun.ScriptSource.FromScriptCode(edit.Text, sourceFileFullPath);
            scriptRun.ScriptLanguage = language;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSource(cbLanguages.SelectedIndex);
        }

        private void UseDirectAPICheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chkUseDirectAPI);
            if (str != UseRemoteAPIDescription)
                toolTip1.SetToolTip(chkUseDirectAPI, UseRemoteAPIDescription);
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }
    }
}
