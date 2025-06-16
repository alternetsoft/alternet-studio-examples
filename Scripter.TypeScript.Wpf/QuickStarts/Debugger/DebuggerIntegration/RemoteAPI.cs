#if NETFRAMEWORK
#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections;
using System.Runtime.Remoting;

using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace DebuggerIntegration.TypeScript
{
#pragma warning disable SA1649 // File name should match first type name
    public interface IScriptAPI
#pragma warning restore SA1649 // File name should match first type name
    {
        string GetEditorText();

        void ShowMessage(string text);
    }

    public class RemoteAPI
    {
        private const string DefaultIpcObjectUri = "ScriptDebuggerRemoteControl";
        private const string DefaultIpcPortName = "ScriptDebuggerIpcServer";
        private const string DefaultIpcClientChannel = "ClientChannel";
        private static IScriptAPI scriptAPI;

        public static IScriptAPI StartClient(string ipcPortName = null, string ipcObjectUri = null, string ipcClientChannel = null)
        {
            var properties = new Hashtable();
            properties["portName"] = ipcClientChannel ?? DefaultIpcClientChannel;
            properties["typeFilterLevel"] = TypeFilterLevel.Full;

            return (IScriptAPI)Activator.GetObject(
                typeof(IScriptAPI),
                string.Format("ipc://{0}/{1}", ipcPortName ?? DefaultIpcPortName, ipcObjectUri ?? DefaultIpcObjectUri));
        }

        public static IChannel StartServer(IScriptAPI api, string ipcPortName = null, string ipcObjectUri = null)
        {
            scriptAPI = api;

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
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ScriptAPI), ipcObjectUri ?? DefaultIpcObjectUri, WellKnownObjectMode.Singleton);

            return channel;
        }

        public class ScriptAPI : MarshalByRefObject, IScriptAPI
        {
            public string GetEditorText()
            {
                return scriptAPI.GetEditorText();
            }

            public void ShowMessage(string text)
            {
                scriptAPI.ShowMessage(text);
            }
        }
    }
}
#endif