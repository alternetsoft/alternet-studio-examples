#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;

namespace RunScriptMethods
{
    public partial class MainForm : Form
    {
        private IScriptEdit edit;
        private IDictionary<string, Action> actions = new Dictionary<string, Action>();
        private Action current = null;
        private ScriptGlobalItem item;
        private ExternalClass externalClass;
        private HostGlobalObject hostGlobalObject = new HostGlobalObject();

        public MainForm()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "RunScriptMethods.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

            externalClass = new ExternalClass(this);

            item = new ScriptGlobalItem("External", externalClass);

            scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
            scriptRun.ScriptHost.GenerateModulesOnDisk = false;
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
            scriptRun.ScriptMode = ScriptMode.Debug;
            scriptRun.ScriptSource.References.Clear();

            // Add default references so MsgBox is recognized
            scriptRun.ScriptSource.WithDefaultReferences(ScriptTechnologyEnvironment.WindowsForms);

            scriptRun.ScriptSource.References.Add(typeof(ExternalClass).Assembly.Location);

            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);

            scriptRun.ScriptHost.HostGlobalObject = hostGlobalObject;

            RegisterDemoAction("Run Methods", RunMethods);
            RegisterDemoAction("Script Assembly", ScriptAssemblyViaReflection);
            ActionsComboBox.SelectedIndexChanged += ActionsComboBox_SelectedIndexChanged;
            ActionsComboBox.SelectedIndex = 0;

            string fileName = GetFilePath();

            edit = new ScriptCodeEdit();
            edit.Dock = DockStyle.Fill;
            edit.Parent = pnEdit;

            edit.Text = "Loading text...";

            ControlUtilities.BindFormInitializer(this, () =>
            {
                edit.InitSyntax();
                LoadFile(edit, fileName);
            });

            Log("Application started.");
        }

        public void Log(string message)
        {
            Debug.WriteLine(message);
            Console.WriteLine(message);
        }

        public async void RunMethods()
        {
            if (!InitScript())
                return;

            var assembly = scriptRun.ScriptHost.ScriptAssembly;

            var inst = assembly.CreateInstance("RunScriptMethods.ScriptClass");
            if (scriptRun.HasType("RunScriptMethods.ScriptClass", ignoreCase: false))
            {
                Log("Type 'RunScriptMethods.ScriptClass' is defined.");
            }
            else
            {
                Log("Type 'RunScriptMethods.ScriptClass' is not defined.");
            }

            try
            {
                scriptRun.RunMethod("Main", null, new object[] { "hello from script with public class" }, throwNotFound: true);
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }

            try
            {
                await scriptRun.RunMethodAsync("MainAsync", null, new object[] { "async hello from script with public class" });
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }

            if (scriptRun.HasMethod("GetData", isStatic: true))
            {
                int result = (int)scriptRun.RunMethod("GetData", null, new object[] { });
                Log("GetData returned: " + result.ToString());
            }

            if (!scriptRun.HasMethod("InstanceMethod", isStatic: true))
            {
                MethodInfo methodInfo = scriptRun.FindMethod("InstanceMethod", isStatic: false);
                if (methodInfo != null)
                {
                    var result = methodInfo?.Invoke(inst, null);
                    Log("InstanceMethod returned: " + result.ToString());
                }
            }
        }

        public bool InitScript()
        {
            scriptRun.ScriptSource.FromScriptFile(GetFilePath());

            if (!scriptRun.Compile())
            {
                Log("Script compilation errors:\n" + scriptRun.ScriptHost.CompilerErrorsAsString());
                return false;
            }

            scriptRun.ScriptHost.InitGlobalScriptObjects();
            return true;
        }

        public void ScriptAssemblyViaReflection()
        {
            if (!InitScript())
                return;

            var assembly = scriptRun.ScriptHost.ScriptAssembly;

            var inst = assembly.CreateInstance("RunScriptMethods.ScriptClass");

            bool testMethodWithException = true;

            if (testMethodWithException)
            {
                MethodInfo methodInfo = inst.GetType().GetMethod("WithException");

                try
                {
                    Log($"Invoke method with exception in the script");
                    Log(string.Empty);
                    methodInfo?.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    Exception originalEx = ex;

                    if (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }

                    Log($"Caught exception from script: {ex.Message}");
                    Log("Stack trace:");

                    StackTrace st = new StackTrace(ex, fNeedFileInfo: true);
                    for (int i = 0; i < st.FrameCount; i++)
                    {
                        StackFrame frame = st.GetFrame(i);
                        string methodName = frame.GetMethod().Name;
                        string filePath = frame.GetFileName();
                        int lineNumber = frame.GetFileLineNumber();
                        int columnNumber = frame.GetFileColumnNumber();

                        Log(
                            $"Method: {methodName}, File: {filePath}, Line: {lineNumber}, Column: {columnNumber}");
                    }

                    Log(string.Empty);
                }
            }

            var propInfoShared = inst.GetType().GetProperty("GlobalProperrty");
            propInfoShared.SetValue(null, "new shared value from main app");

            var valShared = propInfoShared.GetValue(null);
            Log($"Global Property: {propInfoShared.Name}, Value: {valShared}");

            var propInfoInstance = inst.GetType().GetProperty("InstanceProperty");
            propInfoInstance.SetValue(inst, "new instance value from main app");
            var valInstance = propInfoInstance.GetValue(inst);
            Log($"Instance Property: {propInfoInstance.Name}, Value: {valInstance}");
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                if (current != null)
                {
                    Log("=================");
                    Log($"Running action: {ActionsComboBox.Text}");
                    Log(string.Empty);
                    current();
                }
            }));
        }

        public void RegisterDemoAction(string title, Action action)
        {
            actions.Add(title, action);
            ActionsComboBox.Items.Add(title);
        }

        private string GetFilePath()
        {
            return GetSourceFileFullPath("ScriptClass.cs");
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

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void ActionsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            current = ActionsComboBox.SelectedIndex >= 0 && ActionsComboBox.SelectedIndex < actions.Count ? actions[ActionsComboBox.Text] : null;
        }

        private void RunScriptButton_Click(object sender, EventArgs e)
        {
            StartScript();
        }
    }
}
