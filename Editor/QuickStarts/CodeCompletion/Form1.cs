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
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor.TextSource;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace CodeCompletion
{
    public partial class Form1 : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string AutomaticDesc = "Code completion performed by the parser";
        private const string ManualDesc = "Code completion customized by handling NeedCodeCompletion event handler";
        private CsParser csParser1 = new CsParser();
        private VbParser vbParser1 = new VbParser();
        private TextSource csharpSource = new TextSource();
        private TextSource vbSource = new TextSource();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            syntaxEdit1.Lexer = csParser1;
            cbLanguages.SelectedIndex = 0;
        }

        private void SyntaxEdit1_NeedCodeCompletion(object sender, Alternet.Syntax.CodeCompletionArgs e)
        {
            if (rbAutomatic.Checked)
                return;

            if ((e.CompletionType == CodeCompletionType.ListMembers) || (e.CompletionType == CodeCompletionType.CompleteWord) ||
                ((e.CompletionType == CodeCompletionType.None) && (e.KeyChar == '.')))
            {
                if ((e.Provider != null) && (e.Provider is IListMembers))
                {
                    var selItem = e.SelIndex >= 0 ? e.Provider[e.SelIndex] : null;

                    e.Provider.Sort();

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
                    e.ToolTip = false;
                    if (selItem != null)
                        e.SelIndex = e.Provider.IndexOf(selItem);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);

            fileInfo = new FileInfo(dir + @"Resources\Editor\text\vb_net.txt");
            if (fileInfo.Exists)
                vbSource.LoadFile(fileInfo.FullName);

            csharpSource.Lexer = csParser1;
            vbSource.Lexer = vbParser1;
            csharpSource.HighlightReferences = true;
            vbSource.HighlightReferences = true;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbLanguages.SelectedIndex)
            {
                case 0:
                    syntaxEdit1.Source = csharpSource;
                    break;
                case 1:
                    syntaxEdit1.Source = vbSource;
                    break;
                default:
                    syntaxEdit1.Source = csharpSource;
                    break;
            }
        }

        private void AutomaticRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            syntaxEdit1.CodeCompletionBox.Sorted = rbAutomatic.Checked;
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }

        private void AutomaticRadioButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(rbAutomatic);
            if (str != AutomaticDesc)
                toolTip1.SetToolTip(rbAutomatic, AutomaticDesc);
        }

        private void ManualRadioButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(rbManual);
            if (str != ManualDesc)
                toolTip1.SetToolTip(rbManual, ManualDesc);
        }
    }
}
