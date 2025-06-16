#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Windows.Controls;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Common.Projects;
using Alternet.Editor;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI;
using Alternet.Syntax;

namespace AlternetStudio.Wpf.Demo
{
    internal class CodeNavigationBar : IDisposable
    {
        private readonly ComboBox classesComboBox;
        private readonly ComboBox methodsComboBox;
        private readonly CodeExplorer codeExplorer;
        private readonly Func<IScriptEdit> getActiveEditorFunc;
        private readonly DispatcherTimer updateTimer = new DispatcherTimer();
        private int updateCount;
        private bool isDisposed;
        private bool updateExplorer = false;

        public CodeNavigationBar(
            ComboBox classesComboBox,
            ComboBox methodsComboBox,
            CodeExplorer codeExplorer,
            Func<IScriptEdit> getActiveEditorFunc)
        {
            this.classesComboBox = classesComboBox;
            this.methodsComboBox = methodsComboBox;
            this.codeExplorer = codeExplorer;
            this.getActiveEditorFunc = getActiveEditorFunc;
            updateTimer.Interval = TimeSpan.FromMilliseconds(200);
            updateTimer.Tick += UpdateTimer_Tick;

            classesComboBox.SelectionChanged += MethodsComboBox_SelectionChanged;
            methodsComboBox.SelectionChanged += MethodsComboBox_SelectionChanged;
        }

        private IScriptEdit ActiveSyntaxEdit => getActiveEditorFunc();

        public void Dispose()
        {
            if (isDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            updateTimer.Stop();

            classesComboBox.SelectionChanged -= MethodsComboBox_SelectionChanged;
            methodsComboBox.SelectionChanged -= MethodsComboBox_SelectionChanged;

            isDisposed = true;
        }

        public void Update(bool updateExplorer)
        {
            var edit = ActiveSyntaxEdit;
            this.updateExplorer |= updateExplorer;

            if (edit != null)
            {
                updateTimer.Stop();
                updateTimer.Start();
            }
            else
                DelayedUpdateCodeWindows();
        }

        private void DelayedUpdateCodeWindows()
        {
            ScriptCodeEdit edit = ActiveSyntaxEdit as ScriptCodeEdit;
            if (edit != null)
            {
                updateCount++;
                try
                {
                    var document = edit.Document();
                    CodeUtils.FillClasses(classesComboBox, document, edit.Position);
                    CodeUtils.FillMethods(methodsComboBox, document, edit.Position, classesComboBox);

                    if (updateExplorer)
                        codeExplorer.UpdateExplorer(document);
                }
                finally
                {
                    updateCount--;
                }
            }
            else
            {
                CodeUtils.FillClasses(classesComboBox, null, System.Drawing.Point.Empty);
                CodeUtils.FillMethods(methodsComboBox, null, System.Drawing.Point.Empty, classesComboBox);
                if (updateExplorer)
                    codeExplorer.UpdateExplorer(null);
            }

            updateExplorer = false;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            DelayedUpdateCodeWindows();
            updateTimer.Stop();
        }

        private void MethodsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateCount > 0)
                return;
            IScriptEdit edit = ActiveSyntaxEdit;

            if (edit != null && sender is ComboBox)
            {
                System.Drawing.Point position = CodeUtils.GetSelectedPosition((ComboBox)sender, edit.Document());
                if (!position.IsEmpty)
                {
                    edit.Position = position;
                    edit.Focus();
                }
            }
        }
    }
}