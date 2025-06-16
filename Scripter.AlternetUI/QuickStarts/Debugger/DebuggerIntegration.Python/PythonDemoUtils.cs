using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Alternet.Common;
using Alternet.Common.Python;
using Alternet.Scripter.Debugger.UI.AlternetUI;
using Alternet.Scripter.Integration.AlternetUI;
using Alternet.Scripter.Python.Embedded;
using Alternet.UI;

using Python.Runtime;

namespace Alternet.Scripter.Python
{
    public static class PythonDemoUtils
    {
        static PythonDemoUtils()
        {
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        public static void TestGetTempFileName()
        {
            PathUtilities.UseAppSubFolderAsTempPath = true;
            var s = PathUtilities.GetTempFileName();
            App.Log($"Created temp file: '{s}', exists: {File.Exists(s)} ");
        }

        /// <summary>
        /// Test method for the internal purposes.
        /// </summary>
        public static void TestPythonPathTheme()
        {
            var s = PathUtils.GenFilePathOnDesktop("PythonPathTheme.txt");
            LogUtils.LogFileName("PythonPathTheme saved to desktop", s);
            PythonPathScheme.LogToFile(s);
        }

        public static void Initialize()
        {
            Alternet.Scripter.Python.ScriptEngine.RunProcessFunc = ProcessRunnerWithNotification.RunProcess;
        }

        public static void InstallEmbeddedPythonToFolder(string path)
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();

            embeddedPythonInstaller.LogMessage += (s) =>
            {
                App.Log(s);
            };

            embeddedPythonInstaller.InstallPath = path;

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled(true))
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "Install Embedded Python",
                Message = "Deploying Python and packages...",
            };

            progressDialog.ProgressBar.IsIndeterminate = true;

            progressDialog.Load += async (s, e) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython(true);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialogAsync();
        }

        public static void LogInstalledPackages()
        {
            var packages = ScriptEngine.GetInstalledPackages();

            App.LogBeginSection("Installed packages");

            foreach (var package in packages)
            {
                App.Log($"{package.Name}=={package.VersionAsString}");
            }

            App.LogEndSection();
        }

        public static void LogRuntimeInfo(bool onlyIfDebug = true)
        {
            if (onlyIfDebug && !DebugUtils.IsDebugDefined)
                return;

            LogUtils.LogFileName("Dll", PythonPathScheme.FindPythonDll());
            LogUtils.LogFileName("CodeEnvironment.PythonPath", CodeEnvironment.PythonPath);
            LogUtils.LogFileName("CodeEnvironment.PythonLibPath", CodeEnvironment.PythonLibPath);
            LogUtils.LogFileName(
                "CodeEnvironment.PythonSitePackagesPath",
                CodeEnvironment.PythonSitePackagesPath);

            ScriptEngine.InitializeRuntime();
            App.LogNameValue("PythonEngine.PythonHome", PythonEngine.PythonHome);
            App.LogNameValue("PythonEngine.PythonPath", PythonEngine.PythonPath);

            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                App.Log($"sys.version: {sys.version}");
                App.Log($"sys.prefix: {sys.prefix}");
                App.Log($"sys.base_prefix: {sys.base_prefix}");
                App.Log($"sys.path: {sys.path}");

                App.Log($"VIRTUAL_ENV: {Environment.GetEnvironmentVariable("VIRTUAL_ENV")}");
                App.Log($"PYTHONHOME: {Environment.GetEnvironmentVariable("PYTHONHOME")}");
                App.Log($"PYTHONPATH: {Environment.GetEnvironmentVariable("PYTHONPATH")}");
                App.Log($"PYTHONNOUSERSITE: {Environment.GetEnvironmentVariable("PYTHONNOUSERSITE")}");
            }

            LogInstalledPackages();
        }        
    }
}
