#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class LineStylesSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;
        private int startLine = 23;
        private int endLine = 64;
        private int index;

        public LineStylesSettingsUserControl()
        {
            InitializeComponent();
        }

        public TextEditor Editor
        {
            get
            {
                return editor;
            }

            set
            {
                if (editor == value)
                    return;

                if (value == null)
                    DeinitializeEditor();

                editor = value;

                if (editor != null)
                    InitializeEditor();
            }
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
        }

        private void StepOverButton_Click(object sender, RoutedEventArgs e)
        {
            if (index < (endLine - startLine))
            {
                if (editor.Source.LineStyles.GetLineStyle(startLine + index) == 0)
                    editor.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                index++;
                while ((index < (endLine - startLine)) && (editor.Source.Lines[startLine + index].Trim() == string.Empty))
                    index++;
                editor.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                editor.MakeVisible(new System.Drawing.Point(0, startLine + index));
            }
            else
            {
                editor.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                index = 0;
            }
        }

        private void SetBreakpointButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Source.BookMarks.ToggleBookMark(editor.Position, 11);
            editor.Source.LineStyles.ToggleLineStyle(editor.Position.Y, 0, 1);
        }

        private void InitializeEditor()
        {
            var style = new EditLineStyle();
            style.BackColor = System.Drawing.Color.Black;
            style.ForeColor = System.Drawing.Color.FromArgb(255, 241, 129);
            style.Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors;
            style.ImageIndex = 12;
            editor.LineStyles.Add(style);

            editor.LineStyles.Add(new EditLineStyle() // breakpoint style
            {
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.FromArgb(171, 97, 107),
                ImageIndex = 11,
            });

            if (editor.Find("Main", SearchOptions.EntireScope))
            {
                editor.Selection.Clear();
                startLine = editor.Position.Y + 1;
            }

            endLine = editor.Lines.Count - 2;
        }

        private void DeinitializeEditor()
        {
            editor.LineStyles.RemoveAt(editor.LineStyles.Count - 1);
            foreach (var style in editor.Source.LineStyles.ToArray())
                editor.Source.LineStyles.RemoveLineStyle(style.Line);

            editor.Source.BookMarks.ClearAllBookMarks();

            editor.ReadOnly = false;
        }
    }
}
