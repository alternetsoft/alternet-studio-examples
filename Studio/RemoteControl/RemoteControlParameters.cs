#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

namespace AlternetStudio.Demo.RemoteControl
{
    public class RemoteControlParameters
    {
        public RemoteControlParameters(
            string mainScriptFile,
            string[] myCodeModules,
            int processId,
            string[] references,
            string[] codeFiles,
            string globalCode,
            string ipcPortName,
            string ipcObjectUri)
        {
            MainScriptFile = mainScriptFile;
            MyCodeModules = myCodeModules;
            ProcessId = processId;
            References = references;
            CodeFiles = codeFiles;
            GlobalCode = globalCode;
            IpcPortName = ipcPortName;
            IpcObjectUri = ipcObjectUri;
        }

        public string MainScriptFile { get; }

        public string[] MyCodeModules { get; }

        public int ProcessId { get; }

        public string[] References { get; }

        public string[] CodeFiles { get; }

        public string GlobalCode { get; }

        public string IpcPortName { get; }

        public string IpcObjectUri { get; }
    }
}
