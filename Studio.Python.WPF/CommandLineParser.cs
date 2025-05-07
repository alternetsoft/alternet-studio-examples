#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;

namespace AlternetStudio.Python.Wpf.Demo
{
    public enum PythonMode
    {
        Embedded,

        System,

        Application,
    }

    public static class CommandLineParser
    {
        public static CommandLineParameters ParseCommandLine(string[] args)
        {
            PythonMode mode = PythonMode.Embedded;
            string pythonPath = string.Empty;

            foreach (string arg in args)
            {
                if (StartsWith(arg, "-mode="))
                    mode = GetMode(arg);
                else
                if (StartsWith(arg, "-pythonpath="))
                    pythonPath = GetArgument(arg);
            }

            return new CommandLineParameters(mode, pythonPath);
        }

        private static bool StartsWith(string arg, string str)
        {
            return arg.StartsWith(str, StringComparison.OrdinalIgnoreCase);
        }

        private static PythonMode GetMode(string arg)
        {
            int idx = arg.IndexOf("=");
            var mode = idx >= 0 ? arg.Substring(idx + 1) : arg;
            switch (mode.ToLower())
            {
                case "embedded":
                    return PythonMode.Embedded;
                case "system":
                    return PythonMode.System;
                case "application":
                    return PythonMode.Application;
                default:
                    return PythonMode.Embedded;
            }
        }

        private static string GetArgument(string arg)
        {
            int idx = arg.IndexOf("=");
            return idx >= 0 ? arg.Substring(idx + 1) : arg;
        }

        public class CommandLineParameters
        {
            public CommandLineParameters(PythonMode mode, string pythonPath)
            {
                Mode = mode;
                PythonPath = pythonPath;
            }

            public PythonMode Mode { get; set; }

            public string PythonPath { get; set; }
        }
    }
}
