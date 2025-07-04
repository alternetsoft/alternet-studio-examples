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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Alternet.Common.Wpf;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Wpf;

using Microsoft.Win32;

namespace AlternetStudio.IronPython.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        private const int OutputTabIndex = 0;
        private const int CallStackTabIndex = 2;
        private const int VariablesInScopeTabIndex = 3;
        private const int WatchesTabIndex = 4;
        private const int ErrorsTabIndex = 5;
        private const int FindResultTabIndex = 6;

        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

        #endregion

        public MainWindow()
        {
            InitializeScripter();
            InitializeEditors();
            InitializeComponent();
            InitializeCodeNavigationBar();
            InitEditorsContextMenu();
            InitImages();
            BookMarkManager.SharedBookMarks.Activate += new EventHandler<ActivateEventArgs>(DoActivate);
            BookMarkManager.SharedBookMarks.BookMarkAdded += SharedBookMarks_BookMarkAdded;
            BookMarkManager.SharedBookMarks.BookMarkRemoved += SharedBookMarks_BookMarkRemoved;
        }

        protected override void OnClosed(EventArgs e)
        {
            FinalizeCodeSearch();

            base.OnClosed(e);
        }

        private static ImageSource LoadImageSource(string imageName)
        {
            return ScaledImageLoader.GetImage(typeof(MainWindow), "AlternetStudio.IronPython.Wpf.Demo.Resources.", imageName);
        }

        private string GetFileNameFromNode(TreeViewItem node)
        {
            var tag = node.Tag;

            string fileName = null;

            var formNodeData = tag as FormNodeData;
            if (formNodeData != null)
            {
                fileName = formNodeData.FileName;
            }
            else
                fileName = tag as string;

            return fileName;
        }

        private void UpdateControls()
        {
            UpdateEditorButtons();
            UpdateStatusBar();
            UpdateDebugButtons();
        }

        private Image LoadImage(string imageName)
        {
            var source = LoadImageSource(imageName);
            return new Image
            {
                Source = source,
                Style = (Style)FindResource("ToolbarImageStyle"),
            };
        }

        #region Files and Projects

        private void UpdatePageHeader(TabItem page, string text, string toolTip)
        {
            if (page.Header is TextBlock)
            {
                if (((TextBlock)page.Header).Text != text)
                    page.Header = new TextBlock { Text = text, ToolTip = toolTip };
            }
        }

        #endregion

        #region Toolbar, Statusbar and event handlers

        private void InitImages()
        {
            newMenuItem.Icon = LoadImage("NewFile");

            newStripSplitImage.Source = LoadImageSource("NewFile");

            openMenuItem.Icon = LoadImage("OpenFile");
            openToolButton.Content = LoadImage("OpenFile");
            saveMenuItem.Icon = LoadImage("Save");
            saveToolButton.Content = LoadImage("Save");

            saveMenuItemAll.Icon = LoadImage("SaveAll");
            saveMenuItemAs.Icon = LoadImage("SaveAs");
            exitMenuItem.Icon = LoadImage("Exit");

            gotoToolButton.Content = LoadImage("GoToDefinition");
            historyBackwardToolButton.Content = LoadImage("Backwards");
            historyForwardToolButton.Content = LoadImage("Forwards");

            printMenuItem.Icon = LoadImage("Print");
            printToolButton.Content = LoadImage("Print");
            printPreviewMenuItem.Icon = LoadImage("PrintPreview");
            printPreviewToolButton.Content = LoadImage("PrintPreview");

            findMenuItem.Icon = LoadImage("FindInFile");
            findToolButton.Content = LoadImage("FindInFile");
            replaceMenuItem.Icon = LoadImage("ReplaceInFiles");
            replaceToolButton.Content = LoadImage("ReplaceInFiles");

            undoMenuItem.Icon = LoadImage("Undo");
            undoToolButton.Content = LoadImage("Undo");
            redoMenuItem.Icon = LoadImage("Redo");
            redoToolButton.Content = LoadImage("Redo");
            cutMenuItem.Icon = LoadImage("Cut");
            cutToolButton.Content = LoadImage("Cut");
            copyMenuItem.Icon = LoadImage("Copy");
            copyToolButton.Content = LoadImage("Copy");
            pasteMenuItem.Icon = LoadImage("Paste");
            pasteToolButton.Content = LoadImage("Paste");
            selectAllMenuItem.Icon = LoadImage("SelectAll");

            toggleBookmarkToolButton.Content = LoadImage("Bookmark");
            prevBookmarkToolButton.Content = LoadImage("PreviousBookmark");
            nextBookmarkToolButton.Content = LoadImage("NextBookmark");
            clearAllBookmarksToolButton.Content = LoadImage("ClearBookmark");
        }

        private void NewStripSplitButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            if (button != null)
            {
                button.ContextMenu.IsOpen = true;
            }
        }

        private void UpdateButtons()
        {
            UpdateEditorButtons();
            UpdateStatusBar();
            UpdateDebugButtons();
        }

        private void UpdateStatusBar()
        {
            IScriptEdit edit = ActiveSyntaxEdit;
            var status = edit?.Status;
            positionStatusLabel.Content = status?.Position ?? string.Empty;
            modifiedStatusLabel.Content = status?.Modified ?? string.Empty;
            overwriteStatusLabel.Content = status?.Overwrite ?? " ";
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCodeSearch();
            InitializeExplorerTrees();
            InitializeToolbar();
            LocateStartupDirectory();
            LoadStartupFile();
            UpdateControls();
            InitializeNavigationHistory();
            InitializeDebugger();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Keyboard.IsKeyDown(Key.F5) && debugMenu.StartWithoutDebugMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StartWithoutDebugMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.S) && saveMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    SaveMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.P) && printMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    PrintMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.O) && openMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    OpenMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.Z) && undoMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    UndoMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.Y) && redoMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    RedoMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.X) && cutMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    CutMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.C) && copyMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    CopyMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.V) && pasteMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    PasteMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.A) && selectAllMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    SelectAllMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F) && findMenuItem.IsEnabled)
                {
                    FindMenuItem_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                }

                if (Keyboard.IsKeyDown(Key.H) && replaceMenuItem.IsEnabled)
                {
                    ReplaceMenuItem_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                }

                if (Keyboard.IsKeyDown(Key.G) && gotoMenuItem.IsEnabled)
                {
                    GotoMenuItem_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                }
            }
            else
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (Keyboard.IsKeyDown(Key.F12) && findReferencesMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    FindReferencesMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F9) && debugMenu.EvaluateMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    EvaluateMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F5) && debugMenu.StopDebugMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StopDebugMenuItem_Click(this, new RoutedEventArgs());
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.F12) && gotoDefinitionMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    GotoDefinitionMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.Delete) && deleteMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    DeleteMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F5) && debugMenu.StartDebugMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StartDebugMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F11) && debugMenu.StepIntoMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StepIntoMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F10) && debugMenu.StepOverMenuIem.IsEnabled)
                {
                    e.Handled = true;
                    StepOverMenuItem_Click(this, new RoutedEventArgs());
                }
            }
        }
        #endregion
    }
}