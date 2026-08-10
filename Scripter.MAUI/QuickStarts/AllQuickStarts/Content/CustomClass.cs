using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ExternalAssembly
{
    /// <summary>
    /// Test class
    /// </summary>
    public class CustomClass
    {
        /// <summary>
        /// Test method which takes two parameters and runs "echo" command.
        /// </summary>
        /// <param name="firstParam">The first parameter</param>
        /// <param name="secondParam">The second parameter</param>
        public void TestMethod(int firstParam, bool secondParam)
        {
            RunCommand(string.Format(
                "echo \"first param is '{0}', second param is '{1}'\"",
                firstParam,
                secondParam));
        }

        /// <summary>
        /// Gets or sets a test property.
        /// </summary>
        public int TestProperty { get; set; }

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
        }
    }
}
