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
using System.ComponentModel;
using System.IO;

using Alternet.Common;
using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace CodeCompletion
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Members

        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private bool auto = false;
        private bool manual = false;
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();

        #endregion

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);

            csParser1.Options |= SyntaxOptions.QuickInfoTips;
            csharpSource.Lexer = csParser1;
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            edit.NeedCodeCompletion += new CodeCompletionEvent(SyntaxEdit1_NeedCodeCompletion);
            edit.Source = csharpSource;
            this.edit = edit;
            Auto = true;
            Manual = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Auto
        {
            get
            {
                return auto;
            }

            set
            {
                if (auto != value)
                {
                    auto = value;
                    OnPropertyChanged("Auto");
                }
            }
        }

        public bool Manual
        {
            get
            {
                return manual;
            }

            set
            {
                if (manual != value)
                {
                    manual = value;
                    OnPropertyChanged("Manual");
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SyntaxEdit1_NeedCodeCompletion(object sender, Alternet.Syntax.CodeCompletionArgs e)
        {
            if (e.Provider != null)
                e.Provider.Sort();

            if (auto)
                return;
            if ((e.CompletionType == CodeCompletionType.ListMembers) || (e.CompletionType == CodeCompletionType.CompleteWord) ||
                ((e.CompletionType == CodeCompletionType.None) && (e.KeyChar == '.')))
            {
                if ((e.Provider != null) && (e.Provider is IListMembers))
                {
                    IListMembers p = (IListMembers)e.Provider;
                    p.ShowDescriptions = true;
                    p.ShowResults = false;
                    p.ShowQualifiers = false;

                    IListMember m = p.InsertListMember(0);
                    m.Name = "print";
                    m.DisplayText = "<b>print</b>";
                    m.DataType = "void";
                    m.Qualifier = "public";
                    m.ImageIndex = (int)MemberImageIndex.Method;
                    m.Description = "void print(ref string line)";

                    m = p.InsertListMember(0);
                    m.Name = "evaluate";
                    m.DisplayText = "<b>evaluate</b>";
                    m.DataType = "double";
                    m.Qualifier = "protected";
                    m.ImageIndex = (int)MemberImageIndex.Method;
                    m.Description = "double evaluate(string expression)";
                    e.NeedShow = true;
                    e.Provider = p;
                    e.ToolTip = false;
                }
            }
            else
                if ((e.CompletionType == CodeCompletionType.ParameterInfo) && (e.Provider != null) && (e.Provider is ParameterInfo))
                {
                    foreach (IListMember member in e.Provider)
                    {
                        member.Name = "<b>" + member.Name + "</b>";
                        for (int i = 0; i < member.Parameters.Count; i++)
                        {
                            UpdateParamText(member.Parameters[i], i, member.CurrentParamIndex == i);
                        }
                    }
                }
                else
                    if ((e.CompletionType == CodeCompletionType.QuickInfo) && (e.Provider != null) && (e.Provider is QuickInfo))
                    {
                        foreach (IQuickInfoItem item in e.Provider)
                            UpdateQuickInfoText(item);
                    }
                    else
                        e.Provider = null;
        }

        private void UpdateQuickInfoText(IQuickInfoItem item)
        {
            int startPos = item.Text.IndexOf('(');
            int endPos = item.Text.IndexOf(')');

            if ((startPos >= 0) && (endPos > startPos))
            {
                item.Text = item.Text.Insert(endPos, "</b></i>");
                item.Text = item.Text.Insert(startPos, "<b><i>");
            }
        }

        private void UpdateParamText(IParameterMember param, int index, bool current)
        {
            param.Text = JoinWithSpace(new string[] { param.Qualifier, param.DataType, string.Format("param{0}", index + 1) });
            if (current)
                param.Text = "<b><i>" + param.Text + "</i></b>";
        }

        private string JoinWithSpace(string[] arr)
        {
            string result = string.Empty;
            foreach (string s in arr)
            {
                if (result == string.Empty)
                    result = s;
                else
                    if (s != string.Empty)
                        result += Consts.Space + s;
            }

            return result;
        }
    }
}
