#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Alternet.FormDesigner.WinForms;

namespace Alternet.FormDesigner.Demo
{
    public static class FormSettingsService
    {
        public static FormSettings LoadSettings(EditorFormDesignerDataSource source)
        {
            var settingsFileName = GetSettingsFileName(source);

            if (File.Exists(settingsFileName))
            {
                using (var fs = new FileStream(settingsFileName, FileMode.Open))
                    return LoadSettings(fs);
            }
            else
                return FormSettings.Default;
        }

        public static FormSettings LoadSettings(Stream stream)
        {
            return Serializer.Deserialize(stream);
        }

        public static void SaveSettings(FormSettings settings, EditorFormDesignerDataSource source)
        {
            var settingsFileName = GetSettingsFileName(source);

            using (var fs = new FileStream(settingsFileName, FileMode.Create))
                SaveSettings(settings, fs);
        }

        public static void SaveSettings(FormSettings settings, Stream stream)
        {
            Serializer.Serialize(settings, stream);
        }

        private static string GetSettingsFileName(EditorFormDesignerDataSource source)
        {
            var userCodeFileName = source.UserCodeFileName;
            var path = Path.GetDirectoryName(userCodeFileName);
            var name = Path.GetFileNameWithoutExtension(userCodeFileName);
            return Path.Combine(path, name + ".DesignerSettings.xml");
        }

        public static class Serializer
        {
            public static void Serialize(FormSettings settings, Stream stream)
            {
                var serializer = new XmlSerializer(typeof(SettingsData));
                var data = GetSettingsData(settings);
                serializer.Serialize(stream, data);
            }

            public static FormSettings Deserialize(Stream stream)
            {
                var serializer = new XmlSerializer(typeof(SettingsData));
                var data = (SettingsData)serializer.Deserialize(stream);
                return LoadData(data);
            }

            private static FormSettings LoadData(SettingsData data)
            {
                return new FormSettings(
                    data.AssemblyReferences.Select(x => new FormSettings.AssemblyReference(x.Path)).ToArray());
            }

            private static SettingsData GetSettingsData(FormSettings settings)
            {
                return new SettingsData
                {
                    AssemblyReferences = settings.AssemblyReferences.Select(
                        x => new SettingsData.AssemblyReference
                        {
                            Path = x.AssemblyPath,
                        }).ToArray(),
                };
            }

            [Serializable]
            [XmlRoot(ElementName = "Settings", Namespace = "http://alternetsoft.com/es/formdesigner")]
            public class SettingsData
            {
                public AssemblyReference[] AssemblyReferences { get; set; }

                [Serializable]
                [XmlRoot("Reference")]
                public class AssemblyReference
                {
                    public string Path { get; set; }
                }
            }
        }
    }
}