#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

using Alternet.Common;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Advanced;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SQLDOMParser
{
    public class SqlWrapRepository : SqlRepository
    {
        private IList<FunctionItem> functions;

        public SqlWrapRepository(bool caseSensitive, ISyntaxTree tree)
            : base(caseSensitive, tree)
        {
            functions = new List<FunctionItem>();
            RegisterResword("ADD", TSqlTokenType.Add);
            RegisterResword("ALTER", TSqlTokenType.Alter);
            RegisterResword("ALL", TSqlTokenType.All);
            RegisterResword("AND", TSqlTokenType.And);
            RegisterResword("ANY", TSqlTokenType.Any);
            RegisterResword("AS", TSqlTokenType.As);
            RegisterResword("ASC", TSqlTokenType.Asc);
            RegisterResword("BEGIN", TSqlTokenType.Begin);
            RegisterResword("BETWEEN", TSqlTokenType.Between);
            RegisterResword("BY", TSqlTokenType.By);
            RegisterResword("CASE", TSqlTokenType.Case);
            RegisterResword("CHECK", TSqlTokenType.Check);
            RegisterResword("COLUMN", TSqlTokenType.Column);
            RegisterResword("CREATE", TSqlTokenType.Create);
            RegisterResword("DATABASE", TSqlTokenType.Database);
            RegisterResword("DEFAULT", TSqlTokenType.Default);
            RegisterResword("DELETE", TSqlTokenType.Delete);
            RegisterResword("DESC", TSqlTokenType.Desc);
            RegisterResword("DISTINCT", TSqlTokenType.Distinct);
            RegisterResword("DROP", TSqlTokenType.Drop);
            RegisterResword("END", TSqlTokenType.End);
            RegisterResword("EXEC", TSqlTokenType.Exec);
            RegisterResword("EXISTS", TSqlTokenType.Exists);
            RegisterResword("FOREIGN", TSqlTokenType.Foreign);
            RegisterResword("FROM", TSqlTokenType.From);
            RegisterResword("FULL", TSqlTokenType.Full);
            RegisterResword("GROUP", TSqlTokenType.Group);
            RegisterResword("HAVING", TSqlTokenType.Having);
            RegisterResword("IN", TSqlTokenType.In);
            RegisterResword("INDEX", TSqlTokenType.Index);
            RegisterResword("INNER", TSqlTokenType.Inner);
            RegisterResword("INSERT", TSqlTokenType.Insert);
            RegisterResword("INTO", TSqlTokenType.Into);
            RegisterResword("IS", TSqlTokenType.Is);
            RegisterResword("JOIN", TSqlTokenType.Join);
            RegisterResword("KEY", TSqlTokenType.Key);
            RegisterResword("LIKE", TSqlTokenType.Like);
            RegisterResword("LEFT", TSqlTokenType.Left);
            RegisterResword("NOT", TSqlTokenType.Not);
            RegisterResword("NULL", TSqlTokenType.Null);
            RegisterResword("ON", TSqlTokenType.On);
            RegisterResword("OR", TSqlTokenType.Or);
            RegisterResword("ORDER", TSqlTokenType.Order);
            RegisterResword("OUTER", TSqlTokenType.Outer);
            RegisterResword("PRIMARY", TSqlTokenType.Primary);
            RegisterResword("PROCEDURE", TSqlTokenType.Procedure);
            RegisterResword("RIGHT", TSqlTokenType.Right);
            RegisterResword("SELECT", TSqlTokenType.Select);
            RegisterResword("SET", TSqlTokenType.Set);
            RegisterResword("TABLE", TSqlTokenType.Table);
            RegisterResword("TOP", TSqlTokenType.Top);
            RegisterResword("TRUNCATE", TSqlTokenType.Truncate);
            RegisterResword("UNION", TSqlTokenType.Union);
            RegisterResword("UNIQUE", TSqlTokenType.Unique);
            RegisterResword("UPDATE", TSqlTokenType.Update);
            RegisterResword("VALUES", TSqlTokenType.Values);
            RegisterResword("VIEW", TSqlTokenType.View);
            RegisterResword("WHERE", TSqlTokenType.Where);
            RegisterResword("WHILE", TSqlTokenType.While);

            SchemaName = "dbo";
        }

        public IList<FunctionItem> Functions
        {
            get
            {
                return functions;
            }
        }

        public virtual void FillAllMembers(IListMembers members)
        {
            FillTables(members);
            FillFunctions(members, false);
        }

        public virtual void FillTableMembers(IListMembers members)
        {
            FillTables(members);
            FillFunctions(members, true);
        }

        public virtual bool FindTable(string tableName)
        {
            return FindTableByName(tableName) != null;
        }

        public virtual bool FindFunction(string functionName)
        {
            return FindFunctionByName(functionName) != null;
        }

        public virtual void FillTableFields(IListMembers members, string tableName, string filter)
        {
            TableItem tableItem = FindTableByName(tableName);
            if (tableItem != null)
                FillTableFields(tableItem, members, filter);
        }

        public virtual void FillFunctionParams(IListMembers members, string functionName, int paramIndex)
        {
            FunctionItem functionItem = FindFunctionByName(functionName);
            if (functionItem != null)
                FillMember(members, functionItem, functionName, paramIndex, CodeCompletionScope.Static);
        }

        public override object GetMethodType(
            string text,
            ISyntaxNode node,
            ref string name,
            ref Point position,
            ref Point endPos,
            out int paramIndex,
            out int paramCount,
            out CodeCompletionScope scope)
        {
            ISyntaxNode invNode = GetInvocationNode(text, node, position, out paramIndex, out paramCount);
            name = string.Empty;
            scope = CodeCompletionScope.None;
            if (invNode != null)
            {
                position = invNode.Position;
                endPos = invNode.Range.EndPoint;
                name = invNode.HasChildren ? invNode.ChildList[0].Name : string.Empty;
                FunctionItem function = FindFunctionByName(name);
                scope = CodeCompletionScope.Static;
                position = invNode.Position;
                return function;
            }

            return null;
        }

        public override void FillMember(IListMembers members, object member, string name, CodeCompletionScope scope)
        {
            base.FillMember(members, member, name, scope);
            if (member is FunctionItem)
                AddFunctionMember(members, (FunctionItem)member, -1);
        }

        public override void FillMember(IListMembers members, object member, string name, int paramIndex, CodeCompletionScope scope)
        {
            if (member is FunctionItem)
                AddFunctionMember(members, (FunctionItem)member, paramIndex);
        }

        public override void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
            base.FillMembers(node, position, members, member, name, scope, ref selIndex);
            if (member is ISyntaxNode)
                FillFunctions(members, false);
        }

        public override void FillReswords(IListMembers members, string filter)
        {
            System.Collections.IDictionaryEnumerator en = Reswords.GetEnumerator();
            en.Reset();
            while (en.MoveNext())
            {
                string key = (string)en.Key;
                string val = en.Value.ToString();
                if (filter == string.Empty || CaseSensitive ? key.StartsWith(filter) : key.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase))
                    FillResword(members, val, SyntaxParserConsts.SnippetIndex);
            }

            if (string.IsNullOrEmpty(SchemaName))
                FillTables(members, filter);
            else
                FillResword(members, SchemaName, SyntaxParserConsts.NamespaceIndex);
        }

        public virtual void RegisterResword(string resword, TSqlTokenType type)
        {
            Reswords.Add(resword, type);
        }

        public override void LoadDataFromXml(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            if (doc.DocumentElement != null)
            {
                XmlNode node = null;

                node = doc.SelectSingleNode("Data/tables");
                if (node != null)
                    AddTables(node);

                node = doc.SelectSingleNode("Data/views");
                if (node != null)
                    AddViews(node);

                node = doc.SelectSingleNode("Data/functions");
                if (node != null)
                    AddFunctions(node);
            }
        }

        protected override void FillTables(IListMembers members)
        {
            FillTables(members, string.Empty);
        }

        protected override void FillTables(IListMembers members, string filter)
        {
            foreach (TableItem item in Tables)
            {
                if (filter == string.Empty || CaseSensitive ? item.TableName.StartsWith(filter) : item.TableName.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase))
                {
                    IListMember member = members.AddListMember();
                    member.Name = item.TableName;
                    member.ImageIndex = SyntaxParserConsts.ClassIndex;
                }
            }
        }

        protected virtual void FillTableFields(TableItem table, IListMembers members, string filter)
        {
            foreach (string s in table.Fields)
            {
                if (filter == string.Empty || CaseSensitive ? s.StartsWith(filter) : s.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase))
                {
                    IListMember member = members.AddListMember();
                    member.ImageIndex = SyntaxParserConsts.FieldIndex;
                    member.Name = s;
                }
            }
        }

        protected virtual void FillFunctions(IListMembers members, bool tableFunctions)
        {
            foreach (FunctionItem item in functions)
            {
                if (!tableFunctions || item.IsTableFunction)
                {
                    IListMember member = members.AddListMember();
                    member.Name = item.FunctionName;
                    member.ImageIndex = SyntaxParserConsts.MethodIndex;
                }
            }
        }

        protected override void FillResword(IListMembers members, string name, int index)
        {
            AddMember(members, name, index);
        }

        private bool GetQualifiedName(string text, ref string name, ref Point position)
        {
            name = string.Empty;
            if (text == null)
                return false;
            if (text.Trim() == string.Empty)
                return true;
            int start = Math.Min(text.Length, position.X);
            int end = start;

            for (int i = start - 1; i >= 0; i--)
            {
                if (IsQualifiedChar(text[i]))
                    start = i;
                else
                    break;
            }

            while (end < text.Length && IsQualifiedChar(text[end]))
                end++;
            position = new Point(start, position.Y);
            if (end > start)
                name = text.Substring(start, end - start);
            return end > start;
        }

        private bool IsQualifiedChar(char ch)
        {
            return (((int)ch >= 'a') && ((int)ch <= 'z')) || (((int)ch >= 'A') && ((int)ch <= 'Z')) ||
                (((int)ch >= '0') && ((int)ch <= '9')) || (ch == '_') || (ch == '.');
        }

        private FunctionItem FindFunctionByName(string name)
        {
            foreach (FunctionItem item in functions)
            {
                if (string.Compare(item.FunctionName, name, !CaseSensitive) == 0)
                    return item;
            }

            return null;
        }

        private void AddFields(TableItem table, XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    XmlAttribute attr = child.Attributes["name"];
                    if (attr != null)
                        table.Fields.Add(attr.Value);
                }
            }
        }

        private void AddParamters(FunctionItem function, XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    switch (child.Name)
                    {
                        case "paramInfo":
                            XmlAttribute attr = child.Attributes["paramName"];
                            if (attr != null)
                            {
                                ParamItem param = new ParamItem(attr.Value);
                                attr = child.Attributes["paramType"];
                                if (attr != null)
                                    param.Type = attr.Value;
                                attr = child.Attributes["paramDescription"];
                                if (attr != null)
                                    param.Description = attr.Value;
                                function.Params.Add(param);
                            }

                            break;
                        case "Description":
                            function.Description = node.InnerText;
                            break;
                    }
                }
            }
        }

        private void AddTable(XmlNode node)
        {
            XmlAttribute attr = node.Attributes["name"];
            if (attr != null)
            {
                TableItem table = new TableItem(attr.Value);
                AddFields(table, node);
                Tables.Add(table);
            }
        }

        private void AddView(XmlNode node)
        {
            XmlAttribute attr = node.Attributes["name"];
            if (attr != null)
            {
                ViewItem view = new ViewItem(attr.Value);
                AddFields(view, node);
                Tables.Add(view);
            }
        }

        private void AddFunction(XmlNode node)
        {
            XmlAttribute attr = node.Attributes["name"];
            if (attr != null)
            {
                FunctionItem function = new FunctionItem(attr.Value);
                AddParamters(function, node);
                attr = node.Attributes["scope"];
                function.IsTableFunction = (attr != null) && (string.Compare(attr.Value, "table", true) == 0);
                functions.Add(function);
            }
        }

        private void AddTables(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node)
                {
                    AddTable(child);
                }
            }
        }

        private void AddViews(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node)
                {
                    AddView(child);
                }
            }
        }

        private void AddFunctions(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node)
                {
                    AddFunction(child);
                }
            }
        }

        private ISyntaxNode GetInvocationNode(string text, ISyntaxNode node, Point position, out int paramIndex, out int paramCount)
        {
            paramIndex = -1;
            paramCount = 0;
            while (node != null)
            {
                if (node.NodeType == (int)MsSqlNodeType.ArgumentList)
                {
                    if (((node.Position.Y < position.Y) || ((node.Position.Y == position.Y) && (node.Position.X <= position.X))) &&
                        !(text != null && position.X > 0 && position.X <= text.Length && text[position.X - 1] == ')'))
                    {
                        paramIndex = 0;
                        if (node.HasChildren)
                        {
                            ISyntaxNode exprNode = node.ChildList[0];
                            if ((exprNode != null) && exprNode.HasChildren && (exprNode.NodeType == (int)MsSqlNodeType.ExpressionList))
                            {
                                foreach (ISyntaxNode child in exprNode.ChildList)
                                {
                                    Point pt = child.Range.EndPoint;
                                    if ((pt.Y < position.Y) || ((pt.Y == position.Y) && (pt.X < position.X)))
                                        paramIndex++;
                                    paramCount++;
                                }
                            }
                        }
                    }
                }

                if (node.NodeType == (int)MsSqlNodeType.InvocationExpression)
                    return node;
                node = node.Parent;
            }

            return null;
        }

        private void AddFunctionMember(IListMembers members, FunctionItem item, int paramIndex)
        {
            int index = SyntaxParserConsts.MethodIndex;
            IListMember member = AddMember(members, item.FunctionName, index);
            member.Description = item.Description;
            member.Parameters = new ParameterMembers();
            foreach (ParamItem param in item.Params)
            {
                IParameterMember par = member.Parameters.AddParameterMember();
                par.Name = param.Name;
                par.DataType = param.Type;
                par.Description = param.Description;
            }

            UpdateMethodParams(member, paramIndex);
            member.Name += member.ParamText;
        }

        public class ViewItem : TableItem
        {
            public ViewItem(string viewName)
                : base(viewName)
            {
            }
        }

        public class ParamItem
        {
            private string name;
            private string type;
            private string desc;

            public ParamItem(string name)
            {
                this.name = name;
            }

            public string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }

            public string Type
            {
                get
                {
                    return type;
                }

                set
                {
                    type = value;
                }
            }

            public string Description
            {
                get
                {
                    return desc;
                }

                set
                {
                    desc = value;
                }
            }
        }

        public class FunctionItem
        {
            private string functionName;
            private string description;
            private bool isTableFunction;
            private IList<ParamItem> pars;

            public FunctionItem()
            {
                pars = new List<ParamItem>();
            }

            public FunctionItem(string functionName)
                : this()
            {
                this.functionName = functionName;
            }

            public string FunctionName
            {
                get
                {
                    return functionName;
                }

                set
                {
                    functionName = value;
                }
            }

            public string Description
            {
                get
                {
                    return description;
                }

                set
                {
                    description = value;
                }
            }

            public bool IsTableFunction
            {
                get
                {
                    return isTableFunction;
                }

                set
                {
                    isTableFunction = value;
                }
            }

            public IList<ParamItem> Params
            {
                get
                {
                    return pars;
                }
            }
        }
    }
}
