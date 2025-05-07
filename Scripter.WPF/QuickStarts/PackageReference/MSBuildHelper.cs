using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Alternet.Common.Projects.DotNet;

namespace Alternet.Scripter.Roslyn
{
    internal static class MSBuildHelper
    {
        public const string ReferencesFile = "references.txt";
        public const string AnalyzersFile = "analyzers.txt";

        public static XDocument CreateCsproj(bool isCore, string targetFramework, IEnumerable<string> references) =>
            new XDocument(new XElement(
                "Project",
                ImportSdkProject("Microsoft.NET.Sdk", "Sdk.props"),
                BuildProperties(targetFramework),
                References(references),
                ReferenceAssemblies(isCore),
                ImportSdkProject("Microsoft.NET.Sdk", "Sdk.targets"),
                CoreCompileTarget()));

        private static XElement ReferenceAssemblies(bool isCore) =>
                isCore ? new XElement("ItemGroup") : new XElement("ItemGroup", new XElement(
                    "PackageReference",
                    new XAttribute("Include", "Microsoft.NETFramework.ReferenceAssemblies"),
                    new XAttribute("Version", "*")));

        private static XElement References(IEnumerable<string> references) =>
            new XElement("ItemGroup", references.Select(Reference).ToArray());

        private static XElement Reference(string reference)
        {
            string name, version;
            DotNetProjectExtension.ParseNuGetReference(reference, out name, out version);

            var element = new XElement("PackageReference", new XAttribute("Include", name));

            if (!string.IsNullOrEmpty(version))
            {
                element.Add(new XAttribute("Version", version));
            }

            element.Add(new XElement("IncludeAssets", "compile"));
            element.Add(new XElement("PrivateAssets", "all"));

            return element;
        }

        private static XElement BuildProperties(string targetFramework) =>
            new XElement(
                "PropertyGroup",
                new XElement("TargetFramework", targetFramework),
                new XElement("OutputType", "Exe"),
                new XElement("OutputPath", "bin"),
                new XElement("UseAppHost", false),
                new XElement("AppendTargetFrameworkToOutputPath", false),
                new XElement("AppendRuntimeIdentifierToOutputPath", false),
                new XElement("CopyBuildOutputToOutputDirectory", false),
                new XElement("GenerateAssemblyInfo", false));

        private static XElement CoreCompileTarget() =>
            new XElement(
                "Target",
                new XAttribute("Name", "CoreCompile"),
                WriteLinesToFile(ReferencesFile, "@(ReferencePathWithRefAssemblies)"),
                WriteLinesToFile(AnalyzersFile, "@(Analyzer)"));

        private static XElement WriteLinesToFile(string file, string lines) =>
            new XElement(
                "WriteLinesToFile",
                new XAttribute("File", file),
                new XAttribute("Lines", lines),
                new XAttribute("Overwrite", true));

        private static XElement ImportSdkProject(string sdk, string project) =>
            new XElement(
                "Import",
                new XAttribute("Sdk", sdk),
                new XAttribute("Project", project));
    }
}
