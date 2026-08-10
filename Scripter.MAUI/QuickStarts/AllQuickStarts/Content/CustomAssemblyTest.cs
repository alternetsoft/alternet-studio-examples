using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using ExternalAssembly;

public class TestClass
{
    public void UseExternal()
    {
        CustomClass customClass = new CustomClass();
        customClass.TestMethod(1, true);
    }

    public static void Main()
    {
        TestClass F = new TestClass();
        F.UseExternal();
    }

    public TestClass()
    {
    }

    public static void RunCommand(string command)
    {
        string shell, shellArgs;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            shell = "cmd.exe";
            shellArgs = $"/C {command} && pause";
        }
        else
        {
            shell = "/bin/bash";
            var pressAnyKey = "read -n 1 -s -r -p \"Press any key to continue...\"";

            shellArgs = $"-c \"{command} && {pressAnyKey}\"";
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = shell,
                Arguments = shellArgs,
                UseShellExecute = true,
            }
        };

        process.Start();
        process.WaitForExit();
    }
}