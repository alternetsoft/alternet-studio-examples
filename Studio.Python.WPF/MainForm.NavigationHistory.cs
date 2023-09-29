#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Alternet.Common;
using Alternet.Editor.Common.Wpf;

namespace AlternetStudio.Python.Wpf.Demo
{
    public partial class MainWindow
    {
        private NavigationHistory navigationHistory = new NavigationHistory();

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, SymbolLocation> pair = new KeyValuePair<string, SymbolLocation>();
            int index = navigationHistory.GetNextHistory(ref pair);
            if (index >= 0)
                HistoryClick(pair.Key, pair.Value, index, false);
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, SymbolLocation> pair = new KeyValuePair<string, SymbolLocation>();
            int index = navigationHistory.GetPrevHistory(ref pair);
            if (index >= 0)
                HistoryClick(pair.Key, pair.Value, index, false);
        }

        private void BackwardMenu_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.ContextMenu.IsEnabled = true;
            btn.ContextMenu.PlacementTarget = sender as Button;
            btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            btn.ContextMenu.IsOpen = true;
        }

        private void Backward_ItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null)
                return;

            string text = item.Header.ToString();
            if (!string.IsNullOrEmpty(text))
                HistoryClick(text, item.Tag, backwardMenu.ContextMenu.Items.IndexOf(item), true);
        }

        private void HistoryClick(string text, object tag, int newIndex, bool relative)
        {
            SymbolLocation location = tag as SymbolLocation;
            if (location == null)
                return;

            int pos = text.IndexOf('-');
            if (pos >= 0)
            {
                navigationHistory.BeginUpdate();
                try
                {
                    string code = text.Substring(pos + 1);
                    if (code.StartsWith(" - "))
                        code = code.Substring(3);

                    navigationHistory.UpdateHistory(backwardMenu.ContextMenu.Items, newIndex, historyBackwardToolButton, backwardMenu, historyForwardToolButton, relative, Backward_ItemClick);
                    IScriptEdit edit = OpenFile(location.FileName);
                    if (edit != null)
                    {
                        edit.Position = new System.Drawing.Point(0, location.Line);
                        edit.Focus();
                    }
                }
                finally
                {
                    navigationHistory.EndUpdate();
                }
            }
        }

        private void InitializeNavigationHistory()
        {
            historyBackwardToolButton.IsEnabled = false;
            backwardMenu.IsEnabled = false;
            historyForwardToolButton.IsEnabled = false;
        }
    }
}