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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class HitTestDemoUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public HitTestDemoUserControl()
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
                    DetachEditor();

                editor = value;

                if (editor != null)
                    AttachEditor();
            }
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
        }

        private void AttachEditor()
        {
            UpdateHitTestResultDisplay(null, null);

            var window = Window.GetWindow(editor);

            window.SizeChanged += Window_SizeChanged;
            window.LocationChanged += Window_LocationChanged;
            window.Deactivated += Window_Deactivated;

            editor.PreviewMouseDown += Editor_PreviewMouseDown;
        }

        private void Window_SizeChanged(object sender, EventArgs e)
        {
            HitPopup.IsOpen = false;
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            HitPopup.IsOpen = false;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            HitPopup.IsOpen = false;
        }

        private void DetachEditor()
        {
            var window = Window.GetWindow(editor);

            window.SizeChanged -= Window_SizeChanged;
            window.LocationChanged -= Window_LocationChanged;
            window.Deactivated -= Window_Deactivated;

            editor.PreviewMouseDown -= Editor_PreviewMouseDown;
            HitPopup.IsOpen = false;
        }

        private void Editor_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var hitPoint = e.GetPosition(editor.ContentArea);

            var hittInfo = new HitTestInfo();
            editor.GetHitTest(hitPoint, hittInfo);

            UpdateHitTestResultDisplay(hittInfo, hitPoint);

            HitPopup.IsOpen = false;
            HitPopup.IsOpen = true;

            e.Handled = true;
        }

        private string PrepareString(string s)
        {
            return s.Replace("\t", "\\t").Trim();
        }

        private void UpdateHitTestResultDisplay(HitTestInfo info, Point? point)
        {
            var data = new List<object>();

            if (point != null)
                data.Add(new { Name = "Point", Value = point });

            if (info != null)
            {
                data.Add(new { Name = "HitTest", Value = info.HitTest });

                if (info.Line != -1)
                    data.Add(new { Name = "Line", Value = info.Line });

                if (info.Pos != -1)
                    data.Add(new { Name = "Pos", Value = info.Pos });

                if (!string.IsNullOrEmpty(info.String))
                    data.Add(new { Name = "String", Value = PrepareString(info.String) });

                if (!string.IsNullOrEmpty(info.Word))
                    data.Add(new { Name = "Word", Value = PrepareString(info.Word) });

                if (!string.IsNullOrEmpty(info.Url))
                    data.Add(new { Name = "Url", Value = PrepareString(info.Url) });

                if (info.GutterImage != -1)
                    data.Add(new { Name = "GutterImage", Value = info.GutterImage });

                if (info.OutlineIndex != -1)
                    data.Add(new { Name = "OutlineIndex", Value = info.OutlineIndex });

                if (info.Style != -1)
                    data.Add(new { Name = "Style", Value = info.Style });

                if (info.TextStyle != Syntax.TextStyle.None)
                    data.Add(new { Name = "TextStyle", Value = info.TextStyle });

                if (info.SyntaxError != null)
                    data.Add(new { Name = "Syntax Error", Value = info.SyntaxError.Description });
            }

            HitTestInfoTable.ItemsSource = data;
            HintTextBlock.Visibility = data.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateDataFromEditor()
        {
        }
    }
}
