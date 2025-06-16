#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.IO;

using Alternet.UI;

using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Common;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;

namespace CodeCompletion
{
    public partial class Form1 : Window
    {
        private readonly Alternet.Editor.TextSource.TextSource csharpSource = new();
        private readonly Alternet.Editor.TextSource.TextSource vbSource = new();
        private readonly CsParser csParser1 = new(new CsSolution());
        private readonly VbParser vbParser1 = new(new VbSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            cbLanguages.Items.AddRange(new object[] {
                "C#",
                "Visual Basic"});

            cbLanguages.SelectedIndexChanged += LanguagesComboBox_SelectedIndexChanged;

            syntaxEdit1.Source = csharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            csharpSource.OptimizedForMemory = false;
            vbSource.OptimizedForMemory = false;

            Form1_Load(this, EventArgs.Empty);
            cbLanguages.SelectedIndex = 0;

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            FileInfo fileInfo = new(dirInfo.FullName + @"c#.cs");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
            else
                syntaxEdit1.Lines.Add($"File not found: {fileInfo.FullName}");

            fileInfo = new FileInfo(dirInfo.FullName + @"vb_net.txt");
            if (fileInfo.Exists)
                vbSource.LoadFile(fileInfo.FullName);

            csharpSource.Lexer = csParser1;
            vbSource.Lexer = vbParser1;
            csharpSource.HighlightReferences = true;
            vbSource.HighlightReferences = true;
            csParser1.Options |= SyntaxOptions.QuickInfoTips;
            syntaxEdit1.NeedCodeCompletion += SyntaxEdit1_NeedCodeCompletion;
            rbAuto.CheckedChanged += AutomaticRadioButton_CheckedChanged;
        }

        private void AutomaticRadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.CodeCompletionBox.Sorted = rbAuto.IsChecked;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Source = cbLanguages.SelectedIndex switch
            {
                0 => csharpSource,
                1 => vbSource,
                _ => csharpSource,
            };
        }

        private void SyntaxEdit1_NeedCodeCompletion(object sender, Alternet.Syntax.CodeCompletionArgs e)
        {
            if (rbAuto.IsChecked)
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
                    e.Provider = p;
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
    }
}
