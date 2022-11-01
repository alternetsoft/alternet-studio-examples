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
using System.Drawing;
using System.Windows.Forms;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;

namespace AlternetStudio.Demo
{
    internal class CodeNavigationBar : IDisposable
    {
        private readonly ComboBox classesComboBox;
        private readonly ComboBox methodsComboBox;
        private readonly CodeExplorer codeExplorer;
        private readonly Func<IScriptEdit> getActiveEditorFunc;
        private readonly ImageList imageList;
        private readonly Timer updateTimer = new Timer();
        private int updateCount;
        private bool isDisposed;
        private bool updateExplorer = false;

        public CodeNavigationBar(
            ComboBox classesComboBox,
            ComboBox methodsComboBox,
            CodeExplorer codeExplorer,
            Func<IScriptEdit> getActiveEditorFunc,
            ImageList imageList)
        {
            this.classesComboBox = classesComboBox;
            this.methodsComboBox = methodsComboBox;
            this.codeExplorer = codeExplorer;
            this.getActiveEditorFunc = getActiveEditorFunc;
            this.imageList = imageList;
            updateTimer.Interval = 200;
            updateTimer.Tick += UpdateTimer_Tick;

            classesComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            methodsComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            classesComboBox.DrawItem += ComboBox_DrawItem;
            methodsComboBox.DrawItem += ComboBox_DrawItem;

            classesComboBox.Format += ComboBox_Format;
            methodsComboBox.Format += ComboBox_Format;
        }

        private IScriptEdit ActiveSyntaxEdit => getActiveEditorFunc();

        public void Dispose()
        {
            if (isDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            updateTimer.Stop();

            classesComboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
            methodsComboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;

            classesComboBox.DrawItem -= ComboBox_DrawItem;
            methodsComboBox.DrawItem -= ComboBox_DrawItem;

            classesComboBox.Format -= ComboBox_Format;
            methodsComboBox.Format -= ComboBox_Format;

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
                await CodeUtils.FillClassesAsync(classesComboBox, string.Empty, Point.Empty, null);
                CodeUtils.FillMethods(methodsComboBox, string.Empty, Point.Empty, classesComboBox, null);

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

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updateCount > 0)
                return;
            var edit = ActiveSyntaxEdit;

            if (edit != null && sender is ComboBox)
            {
                var parser = CodeEditExtensions.DefaultParser;
                Point position = CodeUtils.GetSelectedPosition((ComboBox)sender, edit.FileName, parser);
                if (!position.IsEmpty)
                {
                    edit.Position = position;
                    edit.Focus();
                }
            }
        }

        private void ComboBox_Format(object sender, ListControlConvertEventArgs e)
        {
            var text = CodeUtils.FormatText(e.ListItem);
            if (!string.IsNullOrEmpty(text))
                e.Value = text;
        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            CodeUtils.DrawItem((ComboBox)sender, e, imageList);
        }
    }
}