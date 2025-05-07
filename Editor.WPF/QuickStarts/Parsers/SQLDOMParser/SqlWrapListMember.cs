#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Advanced;

namespace SQLDOMParser
{
    public class SqlWrapListMember : ListMember
    {
        public SqlWrapListMember(IListMembers owner)
            : base(owner)
        {
        }

        public override string Description
        {
            get
            {
                if ((Attributes & MemberAttribute.NoDescription) == 0)
                {
                    string result = Desc;
                    return result != string.Empty ? result : base.Description;
                }
                else
                    return base.Description;
            }
        }

        protected virtual string Desc
        {
            get
            {
                string result = string.Empty;
                switch (MemberType)
                {
                    case SyntaxParserConsts.MethodIndex:
                    case SyntaxParserConsts.PropertyIndex:
                        {
                            if ((Parameters != null) && (CurrentParamIndex >= 0) && (CurrentParamIndex < Parameters.Count))
                            {
                                string pars = Parameters[CurrentParamIndex].Description;
                                if (pars != string.Empty)
                                    return pars;
                            }
                        }

                        break;
                }

                return result;
            }
        }

        public override string GetParamText(bool useFormatting)
        {
            string result = base.GetParamText(useFormatting);
            return ((result != null) && (result != string.Empty)) ? result : GetParamText(useFormatting && ((Owner == null) || Owner.UseHtmlFormatting), false);
        }

        protected virtual string GetParamText(bool useFormatting, bool compact)
        {
            string result = string.Empty;

            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Count; i++)
                {
                    IParameterMember par = Parameters[i];
                    string parText = compact ? par.Name : ((par.Text != string.Empty) ? par.Text : JoinWithSpace(new string[] { par.Qualifier, par.DataType, par.Name }));
                    if (useFormatting && i == CurrentParamIndex)
                        parText = SyntaxConsts.DefaultBoldTag + parText + SyntaxConsts.DefaultBoldEndTag;

                    result = (result != string.Empty) ? result + ", " + parText : parText;
                }
            }

            switch (MemberType)
            {
                case SyntaxParserConsts.PropertyIndex:
                    return (result != string.Empty) ? "[" + result + "]" : result;
                default:
                    return "(" + result + ")";
            }
        }
    }
}
