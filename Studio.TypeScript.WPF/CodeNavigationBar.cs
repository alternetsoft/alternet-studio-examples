#region Copyright (c) 2016-2022 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Windows.Controls;
using System.Windows.Threading;

using Alternet.Editor.Common.Wpf;
using Alternet.Editor.TypeScript.Wpf;

namespace AlternetStudio.TypeScript.Wpf.Demo
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

        private async void DelayedUpdateCodeWindows()
        {
            IScriptEdit edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                updateCount++;
                try
                {
                    var parser = CodeEditExtensions.DefaultParser;
                    parser.ReplaceDocument(edit.FileName, edit.Text);

                    await CodeUtils.FillClassesAsync(classesComboBox, edit.FileName, edit.Position, parser);
                    CodeUtils.FillMethods(methodsComboBox, edit.FileName, edit.Position, classesComboBox, parser);

                    if (updateExplorer)
                        await codeExplorer.UpdateExplorerAsync(edit.FileName, parser);
                }
                finally
                {
                    updateCount--;
                }
            }
            else
            {
                await CodeUtils.FillClassesAsync(classesComboBox, string.Empty, System.Drawing.Point.Empty, null);
                CodeUtils.FillMethods(methodsComboBox, string.Empty, System.Drawing.Point.Empty, classesComboBox, null);

                if (updateExplorer)
                    await codeExplorer.UpdateExplorerAsync(string.Empty, null);
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
                var parser = CodeEditExtensions.DefaultParser;
                System.Drawing.Point position = CodeUtils.GetSelectedPosition((ComboBox)sender, edit.FileName, parser);
                if (!position.IsEmpty)
                {
                    edit.Position = position;
                    edit.Focus();
                }
            }
        }
    }
}