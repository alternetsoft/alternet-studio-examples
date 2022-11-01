#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

using Alternet.Common;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Microsoft.CodeAnalysis;

namespace SyntaxEditor_Wpf
{
    public class CodeParsing
    {
        public static void RegisterCode(string extension, string[] files)
        {
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            solution.RegisterCodeFiles(files);
        }

        public static void RegisterAssemblies(string extension, string[] references, TechnologyEnvironment technology = TechnologyEnvironment.Wpf, bool keepExisting = false)
        {
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            solution.WithDefaultAssemblies(technology, keepExisting).RegisterAssemblies(references);
        }

        public static void UnregisterAssemblies(string extension, TechnologyEnvironment technology = TechnologyEnvironment.Wpf)
        {
            var solution = GetSolution(extension);
            if (solution == null)
                return;

            solution.WithDefaultAssemblies(technology);
        }

        public static void UnregisterCode(string extension, string[] files)
        {
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            solution.UnregisterCodeFiles(files);
        }

        public static void NavigateClasses(ComboBox comboBox, RoslynParser parser, System.Drawing.Point position)
        {
            Document document = parser != null ? parser.Repository.Document as Document : null;
            if (document == null)
                return;
            CodeUtils.NavigateClasses(comboBox, document, position);
        }

        public static void NavigateMethods(ComboBox comboBox, RoslynParser parser, System.Drawing.Point position, ComboBox classComboBox)
        {
            Document document = parser != null ? parser.Repository.Document as Document : null;
            if (document == null)
                return;

            CodeUtils.NavigateMethods(comboBox, document, position, classComboBox);
        }

        public static void FillClasses(ComboBox comboBox, RoslynParser parser, System.Drawing.Point position)
        {
            Document document = parser != null ? parser.Repository.Document as Document : null;
            CodeUtils.FillClasses(comboBox, document, position);
        }

        public static void FillMethods(ComboBox comboBox, RoslynParser parser, System.Drawing.Point position, ComboBox classComboBox)
        {
            Document document = parser != null ? parser.Repository.Document as Document : null;
            CodeUtils.FillMethods(comboBox, document, position, classComboBox);
        }

        public static bool SelectItem(ComboBox comboBox, RoslynParser parser, out System.Drawing.Point position)
        {
            position = System.Drawing.Point.Empty;
            Document document = parser != null ? parser.Repository.Document as Document : null;
            if (document == null)
                return false;

            return CodeUtils.SelectItem(comboBox, document, out position);
        }

        private static IRoslynSolution GetSolution(string extension)
        {
            switch (extension.ToLower())
            {
                case ".cs":
                    return CsSolution.DefaultSolution;
                case ".vb":
                    return VbSolution.DefaultSolution;
                default:
                    return null;
            }
        }
    }
}
