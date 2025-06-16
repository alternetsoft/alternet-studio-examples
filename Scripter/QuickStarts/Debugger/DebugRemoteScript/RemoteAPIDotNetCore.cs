#if NETCOREAPP

#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using EasyPipes;

namespace DebugRemoteScript
{
    public class RemoteAPI
    {
        private const string DefaultIpcPortName = "ScriptAPIIpcServer";

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
            var client = new Client(ipcPortName ?? DefaultIpcPortName);
            var api = client.GetServiceProxy<IScriptAPI>();
            return api;
        }

        public static Server StartServer(string ipcPortName = null, string ipcObjectUri = null)
        {
            var server = new Server(ipcPortName ?? DefaultIpcPortName);
            server.RegisterService<IScriptAPI>(new ScriptAPIDebugWrapper());
            server.Start();
            return server;
        }

        public static void StopServer(object server)
        {
            var srv = server as Server;
            if (srv != null)
            {
                srv.DeregisterService<IScriptAPI>();
                srv.Stop();
            }
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