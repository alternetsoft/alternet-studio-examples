#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;

namespace AlternetStudio.Demo.RemoteControl
{
    public static class CommandLineParser
    {
        public static bool TryParseCommandLine(string[] args, out RemoteControlParameters remoteControlParameters)
        {
            string mainScriptFile = null;
            string[] myCodeModules = null;
            int processId = 0;
            string ipcPortName = null;
            string ipcObjectUri = null;
            string[] references = null;
            string[] codeFiles = null;
            string globalCode = null;

            foreach (string arg in args)
            {
                if (StartsWith(arg, "-mainScriptFile="))
                    mainScriptFile = GetArgument(arg);
                else
                if (StartsWith(arg, "-myCodeModules="))
                    myCodeModules = GetArrayArgument(arg);
                else
                if (StartsWith(arg, "-controlledProcessId="))
                    processId = int.Parse(GetArgument(arg));
                else
                if (StartsWith(arg, "-references="))
                    references = GetArrayArgument(arg);
                else
                if (StartsWith(arg, "-scriptCodeFiles="))
                    codeFiles = GetArrayArgument(arg);
                else
                if (StartsWith(arg, "-globalCode="))
                    globalCode = GetArgument(arg);
                else
                if (StartsWith(arg, "-ipcPortName="))
                    ipcPortName = GetArgument(arg);
                else
                if (StartsWith(arg, "-ipcObjectUri="))
                    ipcObjectUri = GetArgument(arg);
            }

            if (mainScriptFile != null && processId != 0)
            {
                remoteControlParameters = new RemoteControlParameters(
                    mainScriptFile,
                    myCodeModules,
                    processId,
                    references,
                    codeFiles,
                    globalCode,
                    !string.IsNullOrEmpty(ipcPortName) ? ipcPortName : null,
                    !string.IsNullOrEmpty(ipcObjectUri) ? ipcObjectUri : null);

                return true;
            }

            remoteControlParameters = null;
            return false;
        }

        private static bool StartsWith(string arg, string str)
        {
            return arg.StartsWith(str, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetArgument(string arg)
        {
            int idx = arg.IndexOf("=");
            return idx >= 0 ? arg.Substring(idx + 1) : arg;
        }

        private static string[] GetArrayArgument(string arg) => GetArgument(arg).Split(',');
    }
}