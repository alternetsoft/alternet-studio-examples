#region Copyright (c) 2016-2017 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2017 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2017 Alternet Software

using System.Collections.Generic;

using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace SyntaxEditor_Wpf
{
    public class VbBuildUtils
    {
        public static string GetFormattedText(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            string s = string.Empty;
            if (node != null)
            {
                if ((node.Kind() == SyntaxKind.ClassBlock) || (node.Kind() == SyntaxKind.StructureBlock))
                {
                    s = GetNodeName(node);
                    Microsoft.CodeAnalysis.SyntaxNode parent = node.Parent;
                    while (parent != null)
                    {
                        string parentName = GetNodeName(parent);
                        s = (parentName != string.Empty) ? string.Format("{0}.{1}", parentName, s) : s;
                        parent = parent.Parent;
                    }
                }
                else
                {
                    string nodeName = GetNodeName(node);
                    s = IsParamNode(node.Kind()) ? GetParamText(node) : string.Empty;
                    s = (s != string.Empty) ? string.Format("{0}{1}", nodeName, s) : nodeName;
                }
            }

            return s;
        }

        public static void GetNodeList(ref IList<Microsoft.CodeAnalysis.SyntaxNode> list, Microsoft.CodeAnalysis.SyntaxNode root, SyntaxKind[] types)
        {
            if (root != null)
            {
                foreach (SyntaxKind type in types)
                {
                    if (root.Kind() == type)
                    {
                        list.Add(root);
                        break;
                    }
                }

                foreach (Microsoft.CodeAnalysis.SyntaxNode child in root.ChildNodes())
                {
                    GetNodeList(ref list, child, types);
                }
            }
        }

        public static string GetIdentifierName(NameSyntax node)
        {
            if (node is IdentifierNameSyntax)
                return ((IdentifierNameSyntax)node).Identifier.Text;
            else
                if (node is QualifiedNameSyntax)
                    return GetIdentifierName(((QualifiedNameSyntax)node).Left) + "." + GetIdentifierName(((QualifiedNameSyntax)node).Right);
                else
                    return node.GetText().ToString();
        }

        public static string GetNodeName(Microsoft.CodeAnalysis.SyntaxNode snode)
            {
                switch (snode.Kind())
                {
                    case SyntaxKind.Parameter:
                        ParameterSyntax parNode = (ParameterSyntax)snode;
                        if (parNode != null)
                            return parNode.Identifier.Identifier.Text;
                        return string.Empty;
                    case SyntaxKind.ClassBlock:
                        ClassBlockSyntax classNode = (ClassBlockSyntax)snode;
                        if (classNode != null)
                            return classNode.ClassStatement.Identifier.Text;
                        return string.Empty;
                    case SyntaxKind.ConstructorBlock:
                        ConstructorBlockSyntax constrNode = (ConstructorBlockSyntax)snode;
                        if (constrNode != null)
                            return constrNode.SubNewStatement.NewKeyword.Text;
                        return string.Empty;
                    case SyntaxKind.EnumBlock:
                        EnumBlockSyntax enumNode = (EnumBlockSyntax)snode;
                        return string.Empty;
                    case SyntaxKind.EventBlock:
                        EventBlockSyntax eventNode = (EventBlockSyntax)snode;
                        return string.Empty;
                    case SyntaxKind.FieldDeclaration:
                        FieldDeclarationSyntax fieldNode = (FieldDeclarationSyntax)snode;
                        if (fieldNode != null)
                        {
                            string res = string.Empty;
                            for (int i = 0; i < fieldNode.Declarators.Count; i++)
                            {
                                string declarator = string.Empty;
                                foreach (ModifiedIdentifierSyntax syntax in fieldNode.Declarators[i].Names)
                                    declarator = (declarator != string.Empty) ? string.Format("{0}, {1}", declarator, syntax.Identifier.Text) : syntax.Identifier.Text;
                                res += declarator;
                            }

                            return res;
                        }

                        return string.Empty;
                    case SyntaxKind.InterfaceBlock:
                        InterfaceBlockSyntax intfNode = (InterfaceBlockSyntax)snode;
                        if (intfNode != null)
                            return intfNode.InterfaceStatement.Identifier.Text;
                        return string.Empty;
                    case SyntaxKind.SubBlock:
                    case SyntaxKind.FunctionBlock:
                        MethodBlockSyntax methodNode = (MethodBlockSyntax)snode;
                        if (methodNode != null)
                        {
                            return methodNode.SubOrFunctionStatement.Identifier.Text;
                        }

                        return string.Empty;
                    case SyntaxKind.NamespaceBlock:
                        NamespaceBlockSyntax nnode = (NamespaceBlockSyntax)snode;
                        return string.Empty;
                    case SyntaxKind.PropertyBlock:
                        PropertyBlockSyntax propNode = (PropertyBlockSyntax)snode;
                        return string.Empty;
                    case SyntaxKind.StructureBlock:
                        StructureBlockSyntax structNode = (StructureBlockSyntax)snode;
                        if (structNode != null)
                            return structNode.StructureStatement.Identifier.Text;
                        return string.Empty;
                    case SyntaxKind.UsingBlock:
                        UsingBlockSyntax usingNode = (UsingBlockSyntax)snode;
                        return string.Empty;
                    default:
                        return string.Empty;
                }
            }

        private static string JoinWithSpace(string[] arr)
        {
            string result = string.Empty;
            foreach (string s in arr)
            {
                if (result == string.Empty)
                    result = s;
                else
                    if (s != string.Empty)
                        result += '\u0020' + s;
            }

            return result;
        }

        private static Microsoft.CodeAnalysis.SyntaxNode GetParamList(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            foreach (Microsoft.CodeAnalysis.SyntaxNode child in node.ChildNodes())
            {
                if (child.Kind() == SyntaxKind.ParameterList)
                {
                    return child;
                }
                else
                {
                    Microsoft.CodeAnalysis.SyntaxNode result = GetParamList(child);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        private static string GetParamText(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            string result = string.Empty;
            Microsoft.CodeAnalysis.SyntaxNode paramList = GetParamList(node);
            if (paramList != null)
            {
                foreach (Microsoft.CodeAnalysis.SyntaxNode child in paramList.ChildNodes())
                {
                    if (child.Kind() == SyntaxKind.Parameter)
                    {
                        ParameterSyntax parNode = (ParameterSyntax)child;
                        string qualifier = string.Empty;
                        string type = string.Empty;
                        foreach (Microsoft.CodeAnalysis.SyntaxNode parChild in parNode.ChildNodes())
                        {
                            if (parChild.Kind() == SyntaxKind.SimpleAsClause)
                            {
                                SimpleAsClauseSyntax parType = (SimpleAsClauseSyntax)parChild;
                                type = parType.Type.ToString();
                                break;
                            }
                        }

                        string parText = JoinWithSpace(new string[] { qualifier, type, GetNodeName(child) });
                        result = (result != string.Empty) ? result + ", " + parText : parText;
                    }
                }
            }

            return "(" + result + ")";
        }

        private static bool IsParamNode(SyntaxKind nodeType)
        {
            return (nodeType == SyntaxKind.FunctionBlock) || (nodeType == SyntaxKind.SubBlock) || (nodeType == SyntaxKind.ConstructorBlock);
        }
    }
}
