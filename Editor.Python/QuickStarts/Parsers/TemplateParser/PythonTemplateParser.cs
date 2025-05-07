#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Xml.XPath;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Python;
using Alternet.Syntax.Parsers.Python.CodeCompletion;
using Alternet.Syntax.Parsers.Python.SemanticModel;

namespace TemplateParser
{
    public class PythonTemplateParser : PythonNETParser
    {
        private DocTemplate currentTemplate = null;
        private IDictionary<string, DocTemplate> templates = new Dictionary<string, DocTemplate>();

        public override void Reset()
        {
            base.Reset();
            templates.Clear();
        }

        public override void CodeCompletion(string text, StringItemInfo[] textData, Point position, CodeCompletionArgs e)
        {
            base.CodeCompletion(text, textData, position, e);
            if (e.Provider != null && e.Provider is IParameterInfo)
            {
                var members = e.Provider as IParameterInfo;
                var info = members.First() as PythonListMember;
                if (info != null)
                {
                    if (templates.ContainsKey(info.Name))
                    {
                        var template = templates[info.Name];
                        info.Description = template.Description;
                        for (int i = 0; i < info.Parameters.Count; i++)
                        {
                            var parameter = info.Parameters[i];
                            if (i < template.Parameters.Count)
                            {
                                var tempParam = template.Parameters[i];
                                if (string.Compare(parameter.Name, tempParam.Name, true) == 0)
                                {
                                    parameter.Description = tempParam.Description;
                                    parameter.DataType = tempParam.Type;
                                }
                            }
                        }

                        if (template.RecommendedParams.Count > 0)
                        {
                            e.Provider = GetRecommendedParameters(info, template);
                            e.StartPosition = e.EndPosition;
                            e.NeedShow = e.Provider.Count > 0;
                            e.ToolTip = false;
                        }
                    }
                }
            }
        }

        protected virtual ICodeCompletionProvider GetRecommendedParameters(PythonListMember source, DocTemplate template)
        {
            var result = new PythonParameterInfo();
            result.ShowParams = false;
            result.ShowDescriptions = true;

            void AddParameter(IParameterMember parameter, ref string desc)
            {
                desc += string.Format("<br>Parameters:<br>{0}({1}): {2}<br>", parameter.Name, parameter.DataType, parameter.Description);
            }

            void AddMember(string recomended)
            {
                string[] recs = recomended.Split('=');
                string recName = recs.Length > 0 ? recs[0] : string.Empty;
                string recomend = recs.Length > 1 ? recs[1] : string.Empty;
                string filter = string.Empty;
                if (source.CurrentParamIndex >= 0)
                {
                    if (source.CurrentParamIndex >= source.Parameters.Count)
                        return;

                    filter = source.Parameters[source.CurrentParamIndex].Name;
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    if (string.Compare(filter, recName, true) != 0)
                        return;
                }

                IListMember member = result.CreateListMember();
                member.Name = string.Format("{0}={1}", recName, recomend);
                string desc = template.Description;
                member.MemberType = source.MemberType;
                member.ImageIndex = source.ImageIndex;
                member.InsertText = recomend;
                member.Parameters = new ParameterMembers();
                foreach (var parameter in source.Parameters)
                {
                    if (!string.IsNullOrEmpty(filter) && string.Compare(parameter.Name, filter, true) != 0)
                        continue;
                    AddParameter(parameter, ref desc);
                }

                if (!string.IsNullOrEmpty(template.Returns))
                {
                    desc += string.Format("Returns:<br>{0}", template.Returns);
                }

                member.Description = desc;
                result.AddListMember(member);
            }

            foreach (var recomended in template.RecommendedParams)
            {
                AddMember(recomended);
            }

            return result;
        }

        protected override IPythonClassifier CreateClassifier()
        {
            var classifier = base.CreateClassifier();
            classifier.CustomIdentifiers.Add("doc_template");
            CustomIdentifierStyle = (int)LexToken.Directive;
            return classifier;
        }

        protected override bool IdentifierExpected()
        {
            if (Token == (int)PythonLexerToken.CustomIdentifier_Literal)
            {
                MoveNext();
                return true;
            }

            return base.IdentifierExpected();
        }

        protected override bool ParseDecorator(ISyntaxAttributes attrs)
        {
            var result = base.ParseDecorator(attrs);
            var decorator = SyntaxTree.Current?.ChildList?.LastOrDefault();
            if (IsDocTemplate(decorator))
            {
                currentTemplate = ProcessTemplate(decorator);
            }

            return result;
        }

        protected virtual DocTemplate ProcessTemplate(ISyntaxNode decorator)
        {
            string RemoveQuote(string str)
            {
                return str.Trim(new char[] { '\'' });
            }

            DocTemplate result = new DocTemplate();
            var arguments = decorator.FindNode((int)PythonNodeType.ArgumentList);
            ISyntaxNode valueNode = null;
            if (arguments != null && arguments.HasChildren)
            {
                for (int i = 0; i < arguments.ChildCount; i++)
                {
                    var child = arguments.ChildList[i];
                    switch (child.Name)
                    {
                        case "description":
                            valueNode = child.ChildList.First();
                            if (valueNode != null && valueNode.NodeType == (int)PythonNodeType.PrimaryExpression)
                                result.Description = RemoveQuote(valueNode.Name);
                            break;
                        case "params":
                            var paramsNode = child.ChildList.First();
                            if (paramsNode != null && paramsNode.NodeType == (int)PythonNodeType.ListDisplayExpression)
                            {
                                var exprList = paramsNode.FindNode((int)PythonNodeType.ExpressionList);
                                if (exprList != null && exprList.HasChildren)
                                {
                                    foreach (var member in exprList.ChildList)
                                    {
                                        if (member.HasChildren)
                                        {
                                            TemplateParameter templateParam = new TemplateParameter();
                                            result.Parameters.Add(templateParam);
                                            for (int j = 0; j < member.ChildCount; j++)
                                            {
                                                var tupleMember = member.ChildList[j];
                                                if (tupleMember.NodeType == (int)PythonNodeType.TupleMember && tupleMember.HasChildren)
                                                {
                                                    valueNode = tupleMember.ChildList.First();
                                                    if (valueNode != null && valueNode.NodeType == (int)PythonNodeType.PrimaryExpression)
                                                    {
                                                        string val = RemoveQuote(valueNode.Name);
                                                        switch (j)
                                                        {
                                                            case 0:
                                                                templateParam.Name = val;
                                                                break;
                                                            case 1:
                                                                templateParam.Type = val;
                                                                break;
                                                            case 2:
                                                                templateParam.Description = val;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        case "recommended_params":
                            var recParamsNode = child.ChildList.First();
                            if (recParamsNode != null && recParamsNode.NodeType == (int)PythonNodeType.ListDisplayExpression)
                            {
                                var exprList = recParamsNode.FindNode((int)PythonNodeType.ExpressionList);
                                if (exprList != null && exprList.HasChildren)
                                {
                                    foreach (var member in exprList.ChildList)
                                    {
                                        var tupleMember = member.FindNode((int)PythonNodeType.TupleMember);
                                        if (tupleMember != null && tupleMember.HasChildren)
                                        {
                                            valueNode = tupleMember.ChildList.First();
                                            if (valueNode != null && valueNode.NodeType == (int)PythonNodeType.PrimaryExpression)
                                            {
                                                result.RecommendedParams.Add(RemoveQuote(valueNode.Name));
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        case "returns":
                            valueNode = child.ChildList.First();
                            if (valueNode != null && valueNode.NodeType == (int)PythonNodeType.PrimaryExpression)
                                result.Returns = valueNode.Name;
                            break;
                    }
                }
            }

            return result;
        }

        protected override bool ParseMethodDeclaration(ISyntaxAttributes attrs)
        {
            var result = base.ParseMethodDeclaration(attrs);
            var method = SyntaxTree.Current.ChildList.LastOrDefault();
            if (method != null && method.NodeType == (int)PythonNodeType.Method)
            {
                templates.Add(method.Name, currentTemplate);
                currentTemplate = null;
            }

            return result;
        }

        protected virtual bool IsDocTemplate(ISyntaxNode node)
        {
            return node != null && string.Compare(node.Name, "doc_template", true) == 0;
        }

        protected class TemplateParameter
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string Description { get; set; }
        }

        protected class DocTemplate
        {
            public string Description { get; set; }

            public IList<TemplateParameter> Parameters { get; set; } = new List<TemplateParameter>();

            public IList<string> RecommendedParams { get; set; } = new List<string>();

            public string Returns { get; set; }
        }
    }
}
