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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor.Common;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private NavigationHistory navigationHistory = new NavigationHistory();

        private void InitializeNavigationHistory()
        {
            historyBackwardContextMenu.ItemClicked += HistoryBackwardContextMenu_ItemClicked;

            historyBackwardToolSplitButton.ButtonClick += HistoryBackward_ButtonClick;
            historyForwardToolButton.Click += HistoryForward_Click;

            historyBackwardToolSplitButton.Enabled = false;
            historyForwardToolButton.Enabled = false;
        }

        private void HistoryForward_Click(object sender, EventArgs e)
        {
            var pair = new KeyValuePair<string, SymbolLocation>();
            int index = navigationHistory.GetPrevHistory(ref pair);
            if (index >= 0)
                GoToHistoryItem(pair.Key, pair.Value, index, false);
        }

        private void HistoryBackward_ButtonClick(object sender, EventArgs e)
        {
            var pair = new KeyValuePair<string, SymbolLocation>();
            int index = navigationHistory.GetNextHistory(ref pair);
            if (index >= 0)
                GoToHistoryItem(pair.Key, pair.Value, index, false);
        }

        private void HistoryBackwardContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string text = e.ClickedItem.Text;
            if (!string.IsNullOrEmpty(text))
                GoToHistoryItem(text, e.ClickedItem.Tag, historyBackwardContextMenu.Items.IndexOf(e.ClickedItem), true);
        }

        private void GoToHistoryItem(string text, object tag, int newIndex, bool relative)
        {
            var location = tag as SymbolLocation;
            if (location == null)
                return;

            navigationHistory.BeginUpdate();
            try
            {
                navigationHistory.UpdateHistory(historyBackwardContextMenu.Items, newIndex, historyBackwardToolSplitButton, historyForwardToolButton, relative);
                var edit = OpenFile(location.FileName);
                if (edit != null)
                {
                    edit.Position = new Point(0, location.Line);
                    edit.Focus();
                }
            }
            finally
            {
                navigationHistory.EndUpdate();
            }
        }
    }
}