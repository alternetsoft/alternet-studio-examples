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
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Alternet.Common;

namespace AlternetStudio.Wpf.Demo
{
    public static class AssemblyHelper
    {
        private static List<string> gacFolders = new List<string>()
        {
            "GAC_32", "GAC_64", "GAC_MSIL",
        };

        private static List<string> gacSubFolders = new List<string>()
        {
            "%systemroot%\\assembly",
        };

        public static List<AssemblyData> GACAssemblies { get; } = new List<AssemblyData>();

        public static bool GACLoaded { get; internal set; } = false;

        public static AssemblyData FromFile(string fileName)
        {
            bool nuget = false;
            return FromFile(fileName, ref nuget);
        }

        public static AssemblyData FromFile(string fileName, ref bool nuget)
        {
            string version = string.Empty;
            string name = fileName;
            try
            {
                nuget = IsNugetReference(fileName, ref name, ref version);
                if (!nuget)
                {
                    if (Path.IsPathRooted(fileName))
                    {
                        var asmName = AssemblyName.GetAssemblyName(fileName);
                        version = asmName.Version.ToString();
                    }
                }
            }
            catch
            {
            }

            var data = new AssemblyData();
            data.Name = name;
            data.Version = version;
            data.Checked = false;
            data.IsNuGet = nuget;

            return data;
        }

        public static void LoadAssembliesFromGAC()
        {
            GACAssemblies.Clear();
            foreach (string gac in gacSubFolders)
            {
                foreach (string folder in gacFolders)
                {
                    string path = Path.Combine(
                        Environment.ExpandEnvironmentVariables(gac),
                        folder);

                    if (Directory.Exists(path))
                    {
                        string[] files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            if (file.Contains("Resources"))
                                continue;
                            GACAssemblies.Add(FromFile(file));
                        }
                    }
                }
            }

            GACAssemblies.Sort();
            GACLoaded = true;
        }

        public static string GetNugetName(AssemblyData data)
        {
            string result = data.Name;
            if (data.IsNuGet)
                result = string.Format("{0}{1},{2}", StringConsts.NugetPrefix, data.Name, data.Version);
            return result;
        }

        public static bool IsNugetReference(string reference, ref string name, ref string version)
        {
            bool nuget = reference.StartsWith(StringConsts.NugetPrefix);
            if (nuget)
            {
                name = reference.Substring(StringConsts.NugetPrefix.Length);
                int i = name.IndexOf(',');
                if (i >= 0)
                {
                    version = name.Substring(i + 1).Trim();
                    name = name.Substring(0, i);
                }
            }

            return nuget;
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class AssemblyData : IComparable<AssemblyData>
#pragma warning restore SA1402 // File may only contain a single type
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public bool Checked { get; set; }

        public bool IsNuGet { get; set; } = false;

        public int CompareTo(AssemblyData obj)
        => string.Compare(Path.GetFileName(Name), Path.GetFileName(obj.Name));
    }
}
