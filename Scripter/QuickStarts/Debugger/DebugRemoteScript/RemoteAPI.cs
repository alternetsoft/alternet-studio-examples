#if NETFRAMEWORK
#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

using System.Windows.Forms;

namespace DebugRemoteScript
{
    public class RemoteAPI
    {
        private const string DefaultIpcObjectUri = "ScriptDebuggerRemoteControl";
        private const string DefaultIpcPortName = "ScriptDebuggerIpcServer";
        private const string DefaultIpcClientChannel = "ClientChannel";
        private static IScriptAPI scriptAPI;

        public static bool UseRemoteAPI { get; set; } = true;

        public static IScriptAPI ScriptAPI
        {
            get
            {
                return scriptAPI;
            }

            set
            {
                scriptAPI = value;
                ScriptAPIDebugWrapper.ScriptAPI = value;
            }
        }

        public static IScriptAPI StartClient(string ipcPortName = null, string ipcObjectUri = null, string ipcClientChannel = null)
        {
            var properties = new Hashtable();
            properties["portName"] = ipcClientChannel ?? DefaultIpcClientChannel;
            properties["typeFilterLevel"] = TypeFilterLevel.Full;

            return (IScriptAPI)Activator.GetObject(
                typeof(IScriptAPI),
                string.Format("ipc://{0}/{1}", ipcPortName ?? DefaultIpcPortName, ipcObjectUri ?? DefaultIpcObjectUri));
        }

        public static IChannel StartServer(string ipcPortName = null, string ipcObjectUri = null)
        {
            var properties = new Hashtable();
            properties["portName"] = ipcPortName ?? DefaultIpcPortName;
            properties["typeFilterLevel"] = TypeFilterLevel.Full;

            var channel = new IpcChannel(
                properties,
                null,
                new BinaryServerFormatterSinkProvider
                {
                    TypeFilterLevel = TypeFilterLevel.Full,
                });

            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ScriptAPIDebugWrapper), ipcObjectUri ?? DefaultIpcObjectUri, WellKnownObjectMode.Singleton);

            return channel;
        }

        public static IScriptAPI InitializeAPI(string ipcPortName, string ipcObjectUri)
        {
            if (!UseRemoteAPI)
                return ScriptAPI;

            return StartClient(!string.IsNullOrEmpty(ipcPortName) ? ipcPortName : null, !string.IsNullOrEmpty(ipcObjectUri) ? ipcObjectUri : null, !string.IsNullOrEmpty(ipcPortName) ? ipcPortName + "ClientChannel" : null);
        }

        public static IScriptAPI InitializeAPI(string[] args)
        {
            string ipcPortName;
            string ipcObjectUri;
            ParseCommandLineArgs(args, out ipcPortName, out ipcObjectUri);
            return InitializeAPI(ipcPortName, ipcObjectUri);
        }

        public static void ParseCommandLineArgs(string[] args, out string ipcPortName, out string ipcObjectUri)
        {
            ipcPortName = null;
            ipcObjectUri = null;
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

        private static string ExtractCommandLineArg(string arg)
        {
            int idx = arg.IndexOf("=");
            return idx >= 0 ? arg.Substring(idx + 1) : arg;
        }
    }
}
#endif