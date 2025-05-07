using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Alternet.Common.DotNet.DefaultAssemblies.DotNetCore;
using PackageReference;

namespace Alternet.Scripter.Roslyn
{
    public static class NuGetReferenceResolver
    {
        private static string tempBuildPath = Path.Combine(Path.GetTempPath(), "Alternet Scripter NuGET Output");

        private static IDictionary<string, IList<string>> cachedReferences = new Dictionary<string, IList<string>>();

        public static void ClearCache()
        {
            cachedReferences.Clear();
        }

        public static IList<string> ResolveReferences(string frameworkVersion, IList<string> packageReferences, IList<string> unresolvedReferences)
        {
            if (packageReferences.Count == 0)
                return new List<string>();

            string key = GetRefefenceKey(frameworkVersion, packageReferences);
            IList<string> result;
            if (cachedReferences.TryGetValue(key, out result))
                return result;

            var projectFile = BuildCsproj(frameworkVersion, packageReferences);

            if (!string.IsNullOrEmpty(projectFile))
            {
                string projectDir = Path.GetDirectoryName(projectFile);
                var errorsPath = Path.Combine(projectDir, "errors.log");
                if (File.Exists(errorsPath))
                    File.Delete(errorsPath);

                var buildArgs = $" --interactive -nologo -flp:errorsonly;logfile=\"{errorsPath}\" \"{projectFile}\"";
                var location = CurrentRuntimeDotNetCoreInstallLocator.TryGetDotNetCoreInstallLocation();
                string dotNetExecutable = Path.Combine(location, "dotnet.exe");
                Process process = null;
                var exitCode = 0;
                try
                {
                    process = RunProcess(dotNetExecutable, tempBuildPath, $"build {buildArgs}");
                    process.WaitForExit();
                    exitCode = process.ExitCode;
                }
                finally
                {
                    process?.Dispose();
                }

                if (exitCode == 0)
                {
                    var refPath = Path.Combine(projectDir, MSBuildHelper.ReferencesFile);
                    if (File.Exists(refPath))
                    {
                        result = ReadReferences(refPath);
                        cachedReferences.Add(key, result);
                        return result;
                    }
                }
            }

            if (unresolvedReferences != null)
            {
                foreach (var reference in packageReferences)
                {
                    unresolvedReferences.Add(reference);
                }
            }

            return new List<string>();
        }

        private static string GetRefefenceKey(string frameworkVersion, IList<string> packageReferences)
        {
            string result = string.Empty;

            foreach (var packageReference in packageReferences)
            {
                if (string.IsNullOrEmpty(result))
                    result = packageReference;
                else
                    result += "," + packageReference;
            }

            return string.Format("{0}:{1}", frameworkVersion, result);
        }

        private static Process RunProcess(string path, string workingDirectory, string arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    WorkingDirectory = workingDirectory,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                },
                EnableRaisingEvents = true,
            };

            var exitTcs = new TaskCompletionSource<object>();
            process.Exited += (o, e) => exitTcs.TrySetResult(null);
            process.Start();

            return process;
        }

        private static IList<string> ReadReferences(string path)
        {
            return File.ReadAllLines(path);
        }

        private static string DetectTargetFramework()
        {
            string framework = string.Empty;
            var dotnetPaths = new[] { Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432"), "dotnet") };
            var sdkPath = (from path in dotnetPaths
                           let fullPath = Path.Combine(path, "sdk")
                           where Directory.Exists(fullPath)
                           select fullPath).FirstOrDefault();

            if (sdkPath != null)
            {
                foreach (var directory in Directory.GetDirectories(sdkPath))
                {
                    var versionName = Path.GetFileName(directory);
                    if (Version.TryParse(versionName, out var version) && version.Major > 1)
                    {
                        var name = version.Major < 5 ? ".NET Core" : ".NET";
                        framework = version.Major < 5 ? $"netcoreapp{version.Major}.{version.Minor}" : $"net{version.Major}.{version.Minor}";
                    }
                }
            }

            return framework;
        }

        private static string BuildCsproj(string frameworkVersion, IList<string> references)
        {
            if (string.IsNullOrEmpty(frameworkVersion))
                frameworkVersion = DetectTargetFramework();

            var csproj = MSBuildHelper.CreateCsproj(true, frameworkVersion, references);

            var csprojPath = Path.Combine(tempBuildPath, "temp.csproj");

            try
            {
                if (!Directory.Exists(tempBuildPath))
                {
                    Directory.CreateDirectory(tempBuildPath);
                }

                csproj.Save(csprojPath);
            }
            catch (Exception)
            {
                return null;
            }

            return csprojPath;
        }
    }
}
