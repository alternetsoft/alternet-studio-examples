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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

using Alternet.Common;
using Alternet.Syntax.Lexer;

namespace Alternet.Editor.Wpf
{
    /// <summary>
    /// Interaction logic for DlgSyntaxSettings.xaml
    /// </summary>
    public partial class DlgSyntaxSettings : Window, IEditorSettingsDialog
    {
        private const int OpenFolderImage = 0;
        private const int CloseFolderImage = 1;
        private const int SelectedImage = 2;
        private const int UnSelectedImage = 3;

        private ISyntaxSettings syntaxSettings;
        private System.Drawing.Color curForeColor;
        private System.Drawing.Color curBkColor;
        private FontStyle curFontStyle;
        private System.Drawing.FontStyle curFntStyle;
        private FontWeight curFontWeight;
        private string curDesc;
        private bool isControlUpdating;
        private bool isFontControlsUpdating = false;

        private TreeViewItem rootNode;
        private TreeViewItem generalNode;
        private TreeViewItem fontsNode;
        private TreeViewItem keyboardNode;
        private TreeViewItem additionalNode;

        private string scontrol = "control";
        private string sctrl = "CTRL";
        private string salt = "alt";

        private string sshift = "shift";
        private bool inSelection = false;
        private bool selecting = false;

        /// <summary>
        /// Initializes a new instance of the <c>DlgSyntaxSettings</c> class with default settings.
        /// </summary>
        public DlgSyntaxSettings()
        {
            InitializeComponent();
            syntaxSettings = CreateSyntaxSettings();
            rootNode = (TreeViewItem)tvProperties.Items[0];
            generalNode = (TreeViewItem)rootNode.Items[0];
            additionalNode = (TreeViewItem)rootNode.Items[1];
            fontsNode = (TreeViewItem)rootNode.Items[2];
            keyboardNode = (TreeViewItem)rootNode.Items[3];
        }

        /// <summary>
        /// Gets or sets object that implements <c>ISyntaxSettings</c> for this dialog.
        /// </summary>
        public ISyntaxSettings SyntaxSettings
        {
            get
            {
                return syntaxSettings;
            }

            set
            {
                syntaxSettings.Assign(value);
            }
        }

        /// <summary>
        /// Initializes and runs a editor settings dialog box.
        /// </summary>
        /// <param name="hiddenTabs">specifies hidden tabs in the syntax settings dialog.</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; otherwise, DialogResult.Cancel.</returns>
        public bool Execute(EditorSettingsTab hiddenTabs)
        {
            UpdateHiddenTabs(hiddenTabs);
            return ShowDialog().Value;
        }

        protected virtual ISyntaxSettings CreateSyntaxSettings()
        {
            return new SyntaxSettings();
        }

        private void UpdateHiddenTabs(EditorSettingsTab hiddenTabs)
        {
            if ((hiddenTabs & EditorSettingsTab.General) != 0)
            {
                rootNode.Items.Remove(generalNode);
                tpGeneral.Visibility = System.Windows.Visibility.Hidden;
            }

            if ((hiddenTabs & EditorSettingsTab.FontsAndColors) != 0)
            {
                rootNode.Items.Remove(fontsNode);
                tpFontsAndColors.Visibility = System.Windows.Visibility.Hidden;
            }

            if ((hiddenTabs & EditorSettingsTab.Additional) != 0)
            {
                rootNode.Items.Remove(additionalNode);
                tpAdditional.Visibility = System.Windows.Visibility.Hidden;
            }

            if ((hiddenTabs & EditorSettingsTab.Keymapping) != 0)
            {
                rootNode.Items.Remove(keyboardNode);
                tpKeyboard.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private ICollection<FontFamily> GetFonts()
        {
            return System.Windows.Media.Fonts.SystemFontFamilies;
        }

        private int GetInt(string s, int defaultValue)
        {
            try
            {
                return int.Parse(s);
            }
            catch
            {
                return defaultValue;
            }
        }

        private void FillEventHandlers()
        {
            UpdateEventHandlers();
        }

        private void UpdateEventHandlers()
        {
            lbEventHandlers.Items.Clear();
            foreach (IKeyData keyData in syntaxSettings.EventData)
            {
                string s = (keyData.Param != null) ? string.Format("{0}{1}", keyData.EventName, keyData.Param.ToString()) : keyData.EventName;
                if (s != string.Empty)
                {
                    if (tbShowCommands.Text != string.Empty)
                    {
                        if ((s.IndexOf(tbShowCommands.Text) >= 0) && (lbEventHandlers.Items.IndexOf(s) < 0))
                            lbEventHandlers.Items.Add(s);
                    }
                    else
                        if (lbEventHandlers.Items.IndexOf(s) < 0)
                            lbEventHandlers.Items.Add(s);
                }
            }

            if (lbEventHandlers.Items.Count > 0)
                lbEventHandlers.SelectedIndex = 0;
        }

        private void UpdateShortcut(int index)
        {
            cbShortcuts.Text = string.Empty;
            cbShortcuts.Items.Clear();
            string eventName = lbEventHandlers.Items[index].ToString();
            foreach (IKeyData keyData in syntaxSettings.EventData)
            {
                if ((keyData.EventName != string.Empty) && eventName.StartsWith(keyData.EventName))
                {
                    string parName = (eventName.Length > keyData.EventName.Length) ? eventName.Remove(0, keyData.EventName.Length) : string.Empty;
                    if ((keyData.Param == null) || (keyData.Param.ToString() == parName))
                    {
                        string s = ApplyKeyState(keyData);
                        string ss = (s != string.Empty) ? string.Format("{0}, {1}", s, KeyDataToString(keyData.Keys)) : KeyDataToString(keyData.Keys);
                        cbShortcuts.Items.Add(ss);
                    }
                }
            }

            if (cbShortcuts.Items.Count > 0)
                cbShortcuts.SelectedIndex = 0;
        }

        private string KeyDataToString(Keys keyData)
        {
            string result = keyData.ToString();
            string[] s = result.Split(',');
            bool isCtrl = false;
            bool isAlt = false;
            bool isShift = false;
            result = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].IndexOf(scontrol, StringComparison.OrdinalIgnoreCase) >= 0)
                    isCtrl = true;
                else
                    if (s[i].IndexOf(salt, StringComparison.OrdinalIgnoreCase) >= 0)
                        isAlt = true;
                    else
                        if (s[i].IndexOf(sshift, StringComparison.OrdinalIgnoreCase) >= 0)
                            isShift = true;
                        else
                            result = (result != string.Empty) ? string.Format("{0} + {1}", result, s[i]) : s[i];
            }

            if (isAlt)
                result = (result != string.Empty) ? string.Format("{0} + {1}", salt.ToUpper(), result) : salt.ToUpper();
            if (isShift)
                result = (result != string.Empty) ? string.Format("{0} + {1}", sshift.ToUpper(), result) : sshift.ToUpper();
            if (isCtrl)
                result = (result != string.Empty) ? string.Format("{0} + {1}", sctrl, result) : sctrl;
            return result;
        }

        private string ApplyKeyState(IKeyData key)
        {
            string result = string.Empty;
            if (key.State > 0)
            {
                IKeyData[] keys = syntaxSettings.EventData;
                foreach (IKeyData keyData in keys)
                {
                    if ((keyData.LeaveState == key.State) && (keyData.State == 0))
                    {
                        result = KeyDataToString(keyData.Keys);
                        break;
                    }
                }
            }

            return result;
        }

        private void ControlsFromSettings()
        {
            FillStyles();
            FillColorThemes();
            ICollection<FontFamily> fonts = GetFonts();
            foreach (FontFamily family in fonts)
                cbFontName.Items.Add(family.Source);
            cbKeyboardSchemes.Items.Clear();
            cbKeyboardSchemes.Items.Add("Default Settings");
            cbKeyboardSchemes.SelectedIndex = 0;
            FillEventHandlers();
            chbDragAndDrop.IsChecked = (syntaxSettings.SelectionOptions & SelectionOptions.DisableDragging) == 0;

            chbShowMargin.IsChecked = syntaxSettings.ShowMargin;
            chbWordWrap.IsChecked = syntaxSettings.WordWrap;
            chbShowGutter.IsChecked = syntaxSettings.ShowGutter;
            chbLineNumbers.IsChecked = syntaxSettings.ShowLineNumbers;
            chbVertScrollBar.IsChecked = syntaxSettings.VerticalScrollBarVisible;
            chbHorzScrollBar.IsChecked = syntaxSettings.HorizontalScrollBarVisible;

            tbGutterWidth.Text = syntaxSettings.GutterWidth.ToString();
            tbMarginPosition.Text = syntaxSettings.MarginPos.ToString();
            chbBeyondEol.IsChecked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEol) != 0;
            chbBeyondEof.IsChecked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEof) != 0;
            chbMoveOnRightButton.IsChecked = (syntaxSettings.NavigateOptions & NavigateOptions.MoveOnRightButton) != 0;
            chbHighlightUrls.IsChecked = syntaxSettings.HighlightHyperText;
            chbAllowOutlining.IsChecked = syntaxSettings.AllowOutlining;
            chbShowHints.IsChecked = (syntaxSettings.OutlineOptions & OutlineOptions.ShowHints) != 0;
            rbInsertSpaces.IsChecked = syntaxSettings.UseSpaces;
            rbKeepTabs.IsChecked = !syntaxSettings.UseSpaces;
            chbWhiteSpace.IsChecked = syntaxSettings.WhiteSpaceVisible;
            chbLineSeparator.IsChecked = ((syntaxSettings.SeparatorOptions & SeparatorOptions.SeparateLines) != 0) ? true : false;

            string[] s = new string[syntaxSettings.TabStops.Length];
            for (int i = 0; i < syntaxSettings.TabStops.Length; i++)
                s[i] = syntaxSettings.TabStops[i].ToString();
            tbTabStops.Text = string.Join(",", s);
            UpdateFontControls();
        }

        private void UpdateFontControls()
        {
            try
            {
                isFontControlsUpdating = true;

                cbFontName.SelectedIndex = cbFontName.Items.IndexOf(syntaxSettings.Font.Name);
                tbFontSize.Text = Math.Round(syntaxSettings.Font.Size * 72 / 96, 0).ToString(); // syntaxSettings.Font.Size.ToString();
            }
            finally
            {
                isFontControlsUpdating = false;
            }

            FontNameChanged(this, null);
            OnStyleSelected(this, null);
        }

        private void SettingsFromControl()
        {
            if ((bool)chbDragAndDrop.IsChecked)
                syntaxSettings.SelectionOptions = syntaxSettings.SelectionOptions & ~SelectionOptions.DisableDragging;
            else
                syntaxSettings.SelectionOptions = syntaxSettings.SelectionOptions | SelectionOptions.DisableDragging;
            syntaxSettings.ShowMargin = (bool)chbShowMargin.IsChecked;
            syntaxSettings.WordWrap = (bool)chbWordWrap.IsChecked;
            syntaxSettings.WhiteSpaceVisible = (bool)chbWhiteSpace.IsChecked;
            syntaxSettings.ShowGutter = (bool)chbShowGutter.IsChecked;
            syntaxSettings.ShowLineNumbers = (bool)chbLineNumbers.IsChecked;
            syntaxSettings.VerticalScrollBarVisible = (bool)chbVertScrollBar.IsChecked;
            syntaxSettings.HorizontalScrollBarVisible = (bool)chbHorzScrollBar.IsChecked;
            syntaxSettings.GutterWidth = GetInt(tbGutterWidth.Text, EditConsts.DefaultGutterWidth);
            syntaxSettings.MarginPos = GetInt(tbMarginPosition.Text, EditConsts.DefaultMarginPosition);

            if ((bool)chbLineSeparator.IsChecked)
                syntaxSettings.SeparatorOptions |= SeparatorOptions.SeparateLines;
            else
                syntaxSettings.SeparatorOptions &= ~SeparatorOptions.SeparateLines;
            if ((bool)chbBeyondEol.IsChecked)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEol;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEol;
            if ((bool)chbBeyondEof.IsChecked)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEof;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEof;
            if ((bool)chbMoveOnRightButton.IsChecked)
                syntaxSettings.NavigateOptions |= NavigateOptions.MoveOnRightButton;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.MoveOnRightButton;
            if ((bool)chbShowHints.IsChecked)
                syntaxSettings.OutlineOptions |= OutlineOptions.ShowHints;
            else
                syntaxSettings.OutlineOptions &= ~OutlineOptions.ShowHints;

            syntaxSettings.HighlightHyperText = (bool)chbHighlightUrls.IsChecked;
            syntaxSettings.AllowOutlining = (bool)chbAllowOutlining.IsChecked;
            syntaxSettings.UseSpaces = (bool)rbInsertSpaces.IsChecked;

            string[] s = tbTabStops.Text.Split(',');
            int[] tabs = new int[s.Length];
            int j = 0;
            for (int i = 0; i < s.Length; i++)
            {
                j = GetInt(s[i], EditConsts.DefaultTabStop);
                tabs[i] = (j <= 0) ? EditConsts.DefaultTabStop : j;
            }

            syntaxSettings.TabStops = tabs;
            int fntSize = Math.Max(Math.Min(GetInt(tbFontSize.Text, 10), EditConsts.MaxFontSize), 1);
            double fontSize = (double)new System.Windows.FontSizeConverter().ConvertFrom(fntSize.ToString() + "pt");
            syntaxSettings.Font = new MediaFont(syntaxSettings.Font.Family, fontSize, syntaxSettings.Font.Style);
        }

        private void FillStyles()
        {
            lbStyles.Items.Clear();
            for (int i = 0; i < syntaxSettings.LexStyles.Count; i++)
                lbStyles.Items.Add(syntaxSettings.LexStyles[i].Desc);
        }

        private void FillColorThemes()
        {
            cbColorThemes.Items.Clear();

            foreach (IVisualTheme colorTheme in SyntaxSettings.VisualThemes)
            {
                cbColorThemes.Items.Add(colorTheme.Name);
            }

            cbColorThemes.SelectedIndex = SyntaxSettings.VisualThemes.ActiveThemeIndex;
        }

        private void PropertiesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (inSelection)
                return;
            inSelection = true;
            try
            {
                tcMain.SelectedIndex = (tvProperties.SelectedItem != null) ? (int)((TreeViewItem)tvProperties.SelectedItem).Tag : 0;
            }
            finally
            {
                inSelection = false;
            }
        }

        private int GetNodeLevel(TreeViewItem node)
        {
            int result = 0;
            if (node != null)
            {
                TreeViewItem parent = (TreeViewItem)VisualTreeHelper.GetParent(node);
                while (parent != null)
                {
                    node = parent;
                    result++;
                    parent = (TreeViewItem)VisualTreeHelper.GetParent(node);
                }
            }

            return result;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsFromControl();
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DlgSyntaxSettings_Activated(object sender, System.EventArgs e)
        {
            tvProperties.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFromResource();
            ControlsFromSettings();
            lbStyles.SelectionChanged += new SelectionChangedEventHandler(OnStyleSelected);
            lbStyles.SelectedIndex = 0;
            chbBold.Checked += new RoutedEventHandler(FontStyleChange);
            chbBold.Unchecked += new RoutedEventHandler(FontStyleChange);
            chbItalic.Checked += new RoutedEventHandler(FontStyleChange);
            chbItalic.Unchecked += new RoutedEventHandler(FontStyleChange);
            tbDescription.TextChanged += new TextChangedEventHandler(DescriptionChanged);
            cbFontName.SelectionChanged += new SelectionChangedEventHandler(FontNameChanged);
            tbFontSize.TextChanged += new TextChangedEventHandler(FontSizeChanged);
            ((TreeViewItem)tvProperties.Items[0]).IsExpanded = true;
            object item = (((TreeViewItem)tvProperties.Items[0]).Items.Count > 0) ? ((TreeViewItem)tvProperties.Items[0]).Items[0] : tvProperties.Items[0];
            ((TreeViewItem)item).IsSelected = true;
        }

        private void LoadFromResource()
        {
            this.Title = DlgSyntaxSettingsConsts.DlgSyntaxSettingsCaption;
            if (tvProperties.Items.Count > 0)
            {
                rootNode.Header = DlgSyntaxSettingsConsts.PropertiesOptionsCaption;
                rootNode.Tag = 0;
                if (((TreeViewItem)tvProperties.Items[0]).Items.Count >= 3)
                {
                    generalNode.Header = DlgSyntaxSettingsConsts.PropertiesGeneralCaption;
                    fontsNode.Header = DlgSyntaxSettingsConsts.PropertiesFontsColorsCaption;
                    additionalNode.Header = DlgSyntaxSettingsConsts.PropertiesAdditionalCaption;
                    keyboardNode.Header = DlgSyntaxSettingsConsts.PropertiesKeyboradCaption;
                    generalNode.Tag = 0;
                    additionalNode.Tag = 1;
                    fontsNode.Tag = 2;
                    keyboardNode.Tag = 3;
                }
            }

            tpGeneral.Header = DlgSyntaxSettingsConsts.GeneralCaption;
            tpFontsAndColors.Header = DlgSyntaxSettingsConsts.FontsAndColorsCaption.Replace("&&", " and ");
            tpAdditional.Header = DlgSyntaxSettingsConsts.AdditionalCaption;
            gbDocument.Header = DlgSyntaxSettingsConsts.DocumentCaption;
            gbGutterMargin.Header = DlgSyntaxSettingsConsts.GutterMarginCaption.Replace("&&", " and ");
            gbLineNumbers.Header = DlgSyntaxSettingsConsts.GroupBoxLineNumbersCaptionSyntaxSettingsDlg;
            chbWordWrap.Content = DlgSyntaxSettingsConsts.WordWrapCaptionSyntaxSettingsDlg;
            chbHighlightUrls.Content = DlgSyntaxSettingsConsts.HighlightUrlsCaption;
            chbDragAndDrop.Content = DlgSyntaxSettingsConsts.DragAndDropCaption.Replace("&", string.Empty);
            chbVertScrollBar.Content = DlgSyntaxSettingsConsts.VertScrollBarCaption.Replace("&", string.Empty);
            chbHorzScrollBar.Content = DlgSyntaxSettingsConsts.HorzScrollBarCaption.Replace("&", string.Empty);
            chbForced.Content = DlgSyntaxSettingsConsts.ForcedCaption;
            chbShowGutter.Content = DlgSyntaxSettingsConsts.ShowGutterCaption;
            chbShowMargin.Content = DlgSyntaxSettingsConsts.ShowMarginCaption;
            laGutterWidth.Content = DlgSyntaxSettingsConsts.GutterWidthCaption;
            laMarginPosition.Content = DlgSyntaxSettingsConsts.MarginPositionCaption;
            chbLineNumbers.Content = DlgSyntaxSettingsConsts.CheckBoxLineNumbersCaptionSyntaxSettingsDlg;
            laFont.Content = DlgSyntaxSettingsConsts.FontCaption;
            laSize.Content = DlgSyntaxSettingsConsts.SizeCaption.Replace("&", string.Empty);
            btAddColorTheme.Content = DlgSyntaxSettingsConsts.AddVisualTheme;
            btDeleteColorTheme.Content = DlgSyntaxSettingsConsts.DeleteVisualTheme;
            laDisplayItems.Content = DlgSyntaxSettingsConsts.DisplayItemsCaption.Replace("&", string.Empty);
            laDescription.Content = DlgSyntaxSettingsConsts.DescriptionCaption;
            gbFontAttributes.Header = DlgSyntaxSettingsConsts.FontAttributesCaption;
            chbBold.Content = DlgSyntaxSettingsConsts.BoldCaption;
            chbItalic.Content = DlgSyntaxSettingsConsts.ItalicCaption;
            laSample.Content = DlgSyntaxSettingsConsts.SampleCaption;
            laSampleText.Content = DlgSyntaxSettingsConsts.SampleTextCaption;
            gbNavigateOptions.Header = DlgSyntaxSettingsConsts.NavigateOptionsCaption;
            gbOutlineOptions.Header = DlgSyntaxSettingsConsts.OutlineOptionsCaption;
            gbTabOptions.Header = DlgSyntaxSettingsConsts.TabOptionsCaption;
            chbBeyondEol.Content = DlgSyntaxSettingsConsts.BeyondEolCaption;
            chbBeyondEof.Content = DlgSyntaxSettingsConsts.BeyondEofCaption;
            chbMoveOnRightButton.Content = DlgSyntaxSettingsConsts.MoveOnRightButtonCaption;
            chbAllowOutlining.Content = DlgSyntaxSettingsConsts.AllowOutliningCaption;
            chbShowHints.Content = DlgSyntaxSettingsConsts.ShowHintsCaption;
            laTabSizes.Content = DlgSyntaxSettingsConsts.TabSizesCaption;
            rbInsertSpaces.Content = DlgSyntaxSettingsConsts.InsertSpacesCaption.Replace("&", string.Empty);
            rbKeepTabs.Content = DlgSyntaxSettingsConsts.KeepTabsCaption.Replace("&", string.Empty);
            btOK.Content = DlgSyntaxSettingsConsts.OKCaptionSyntaxSettingsDlg;
            btCancel.Content = DlgSyntaxSettingsConsts.CancelCaptionSyntaxSettingsDlg;
            chbWhiteSpace.Content = DlgSyntaxSettingsConsts.WhiteSpaceCaptionSyntaxSettingsDlg;
            chbLineSeparator.Content = DlgSyntaxSettingsConsts.LineSeparatorCaptionSyntaxSettingsDlg;
            chbVertScrollBar.Content = DlgSyntaxSettingsConsts.VertScrollBarCaption.Replace("&", string.Empty);
            chbHorzScrollBar.Content = DlgSyntaxSettingsConsts.HorzScrollBarCaption.Replace("&", string.Empty);

            laKeyboardMappingScheme.Content = DlgSyntaxSettingsConsts.KeyboardMappingSchemeCaption;
            laShowCommands.Content = DlgSyntaxSettingsConsts.ShowCommandsCaption;
            laShortcuts.Content = DlgSyntaxSettingsConsts.ShortcutsCaption;
            btSaveSchemeAs.Content = DlgSyntaxSettingsConsts.SaveSchemeAsCaption;
            btDeleteScheme.Content = DlgSyntaxSettingsConsts.DeleteSchemeCaption;
        }

        private void DescriptionChanged(object sender, TextChangedEventArgs e)
        {
            if (isControlUpdating)
                return;
            curDesc = tbDescription.Text;
            StyleFromControl();
        }

        private void FontStyleChange(object sender, RoutedEventArgs e)
        {
            if (isControlUpdating)
                return;
            curFontStyle = FontStyles.Normal;
            curFontWeight = FontWeights.Normal;
            if ((bool)chbBold.IsChecked)
            {
                curFontWeight = FontWeights.Bold;
                curFntStyle |= System.Drawing.FontStyle.Bold;
            }
            else
                curFntStyle &= ~System.Drawing.FontStyle.Bold;
            if ((bool)chbItalic.IsChecked)
            {
                curFntStyle |= System.Drawing.FontStyle.Italic;
                curFontStyle = FontStyles.Italic;
            }
            else
                curFntStyle &= ~System.Drawing.FontStyle.Italic;
            StyleFromControl();
        }

        private void StyleFromControl()
        {
            if (lbStyles.SelectedItem == null)
                return;
            ILexStyle style = GetSelectedStyle();
            style.ForeColor = curForeColor;
            style.BackColor = curBkColor;
            style.FontStyle = curFntStyle;
            style.Desc = curDesc;
            UpdateStyleControls();
        }

        private void StyleSelected()
        {
            tbDescription.IsEnabled = syntaxSettings.IsDescriptionEnabled(lbStyles.SelectedIndex);
            laDescription.IsEnabled = syntaxSettings.IsDescriptionEnabled(lbStyles.SelectedIndex);
            gbFontAttributes.IsEnabled = syntaxSettings.IsFontStyleEnabled(lbStyles.SelectedIndex);
            ILexStyle style = GetSelectedStyle();
            selecting = true;
            try
            {
                if (style != null)
                {
                    laDescription.IsEnabled = true;
                    tbDescription.IsEnabled = true;

                    chbBold.IsEnabled = style.BoldEnabled;
                    chbItalic.IsEnabled = style.ItalicEnabled;
                    chbBold.IsChecked = (style.FontStyle & System.Drawing.FontStyle.Bold) != 0;
                    chbItalic.IsChecked = (style.FontStyle & System.Drawing.FontStyle.Italic) != 0;
                    curForeColor = style.ForeColor;
                    curBkColor = style.BackColor;
                    cbForeColor.SelectedColor = Color.FromArgb(style.ForeColor.A, style.ForeColor.R, style.ForeColor.G, style.ForeColor.B);
                    cbBackColor.SelectedColor = Color.FromArgb(style.BackColor.A, style.BackColor.R, style.BackColor.G, style.BackColor.B);
                    curFntStyle = style.FontStyle;
                    if ((curFntStyle & System.Drawing.FontStyle.Bold) != 0)
                        curFontWeight = FontWeights.Bold;
                    if ((curFntStyle & System.Drawing.FontStyle.Italic) != 0)
                        curFontStyle = FontStyles.Italic;
                    curDesc = style.Desc;
                }
            }
            finally
            {
                selecting = false;
            }

            UpdateStyleControls();
        }

        private void OnStyleSelected(object sender, SelectionChangedEventArgs e)
        {
            StyleSelected();
        }

        private ILexStyle GetSelectedStyle()
        {
            if (lbStyles.SelectedItem != null)
                return syntaxSettings.LexStyles[lbStyles.SelectedIndex];
            return null;
        }

        private bool EmptyColor(Color color)
        {
            return (color.R == 0) && (color.G == 0) && (color.B == 0);
        }

        private void UpdateStyleControls()
        {
            isControlUpdating = true;
            try
            {
                chbBold.IsChecked = curFontWeight == FontWeights.Bold;
                chbItalic.IsChecked = curFontStyle == FontStyles.Italic;
                tbDescription.Text = curDesc;
                laSampleText.FontStyle = curFontStyle;
                laSampleText.FontWeight = curFontWeight;
                laSampleText.Foreground = new SolidColorBrush(Color.FromArgb(curForeColor.A, curForeColor.R, curForeColor.G, curForeColor.B));
                System.Drawing.Color colorD = syntaxSettings.VisualThemes.ActiveTheme[VisualThemeConsts.WindowColorInternalName].BackColor;
                Color backColor = Color.FromArgb(curBkColor.A, curBkColor.R, curBkColor.G, curBkColor.B);
                Color color = !EmptyColor(backColor) ? backColor : Color.FromArgb(colorD.A, colorD.R, colorD.G, colorD.B);
                pnSampleText.Background = new SolidColorBrush(color);
            }
            finally
            {
                isControlUpdating = false;
            }
        }

        private void FontNameChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isControlUpdating || (cbFontName.SelectedItem == null))
                return;
            try
            {
                laSampleText.FontFamily = new FontFamily(cbFontName.Text);
                laSampleText.FontStyle = curFontStyle;
                laSampleText.FontWeight = curFontWeight;
            }
            catch
            {
            }
        }

        private void FontSizeChanged(object sender, TextChangedEventArgs e)
        {
            if (isControlUpdating)
                return;
            isControlUpdating = true;
            try
            {
                int fontSize = Math.Max(Math.Min(GetInt(tbFontSize.Text, 10), EditConsts.MaxFontSize), 1);
                if (tbFontSize.Text != fontSize.ToString())
                    tbFontSize.Text = fontSize.ToString();
                laSampleText.FontSize = fontSize;
                laSampleText.FontStyle = curFontStyle;
                laSampleText.FontWeight = curFontWeight;
            }
            finally
            {
                isControlUpdating = false;
            }
        }

        private void MarginPositionTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
                if (!e.Handled)
                {
                    try
                    {
                        string s = tbMarginPosition.Text;
                        int.Parse(s.Insert(Math.Min(tbMarginPosition.SelectionStart, s.Length), e.KeyChar.ToString()));
                    }
                    catch
                    {
                        e.Handled = true;
                        OSUtils.MessageBeep();
                    }
                }
                else
                    OSUtils.MessageBeep();
            }
        }

        private void GutterWidthTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
                if (!e.Handled)
                {
                    try
                    {
                        string s = tbGutterWidth.Text;
                        int.Parse(s.Insert(Math.Min(tbGutterWidth.SelectionStart, s.Length), e.KeyChar.ToString()));
                    }
                    catch
                    {
                        e.Handled = true;
                        OSUtils.MessageBeep();
                    }
                }
                else
                    OSUtils.MessageBeep();
            }
        }

        private void ShowCommandsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEventHandlers();
        }

        private void EventHandlersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateShortcut(lbEventHandlers.SelectedIndex);
        }

        private void ColorThemesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cbColorThemes.SelectedIndex >= 0) && (cbColorThemes.SelectedIndex < syntaxSettings.VisualThemes.Count))
            {
                syntaxSettings.VisualThemes.ActiveThemeIndex = cbColorThemes.SelectedIndex;
                btDeleteColorTheme.IsEnabled = !syntaxSettings.VisualThemes.ActiveTheme.ReadOnly;
                UpdateFontControls();
                FillStyles();
                StyleSelected();
            }
        }

        private void AddColorThemeButton_Click(object sender, RoutedEventArgs e)
        {
            IVisualThemes colorThemes = syntaxSettings.VisualThemes;

            VisualTheme theme = new VisualTheme();

            ISerializationInfo info = colorThemes.ActiveTheme.GetSerializationInfo();
            info.Load();
            theme.SetSerializationInfo(info);
            colorThemes.Add(theme);

            int newColorThemeIndex = colorThemes.Count - 1;
            string name = colorThemes[newColorThemeIndex].Name;
            colorThemes[newColorThemeIndex].Name = "Copy of " + name;
            colorThemes[newColorThemeIndex].ReadOnly = false;
            FillColorThemes();
            colorThemes.ActiveThemeIndex = newColorThemeIndex;
            cbColorThemes.SelectedIndex = newColorThemeIndex;
        }

        private void DeleteColorThemeButon_Click(object sender, RoutedEventArgs e)
        {
            IVisualThemes colorThemes = syntaxSettings.VisualThemes;

            if (colorThemes.ActiveThemeIndex != -1)
            {
                if (!colorThemes[colorThemes.ActiveThemeIndex].ReadOnly)
                {
                    colorThemes.RemoveAt(colorThemes.ActiveThemeIndex);
                    colorThemes.ActiveThemeIndex--;
                    FillColorThemes();
                    cbColorThemes.SelectedIndex = colorThemes.ActiveThemeIndex;
                }
            }
        }

        private void ColorThemesComboBox_Leave(object sender, System.EventArgs e)
        {
            if (cbColorThemes.SelectedIndex == -1)
            {
                if ((!syntaxSettings.VisualThemes.ActiveTheme.ReadOnly) && (cbColorThemes.Text != string.Empty))
                {
                    syntaxSettings.VisualThemes.ActiveTheme.Name = cbColorThemes.Text;
                    FillColorThemes();
                }
            }
        }

        private void UpdateActiveColorThemeFont()
        {
            if (isFontControlsUpdating)
                return;

            if ((cbFontName.SelectedIndex != -1) && (tbFontSize.Text != null))
            {
                string fontName = cbFontName.SelectedItem.ToString();
                int fntSize = Math.Max(Math.Min(GetInt(tbFontSize.Text, 10), EditConsts.MaxFontSize), 1);
                double fontSize = (double)new System.Windows.FontSizeConverter().ConvertFrom(fntSize.ToString() + "pt");
                syntaxSettings.Font = new MediaFont(new FontFamily(fontName), fontSize, FontStyles.Normal);
            }
        }

        private void FontNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateActiveColorThemeFont();
        }

        private void FontSizeTexBox_Leave(object sender, System.EventArgs e)
        {
            UpdateActiveColorThemeFont();
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inSelection)
                return;
            inSelection = true;
            try
            {
                if (tpGeneral.IsSelected)
                    generalNode.IsSelected = true;
                else
                    if (tpAdditional.IsSelected)
                        additionalNode.IsSelected = true;
                    else
                        if (tpFontsAndColors.IsSelected)
                            fontsNode.IsSelected = true;
                        else
                            if (tpKeyboard.IsSelected)
                                keyboardNode.IsSelected = true;
            }
            finally
            {
                inSelection = false;
            }
        }

        private void FontNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateActiveColorThemeFont();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void ForeColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (selecting)
                return;
            curForeColor = System.Drawing.Color.FromArgb(e.NewValue.A, e.NewValue.R, e.NewValue.G, e.NewValue.B);
            StyleFromControl();
        }

        private void BackColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (selecting)
                return;
            curBkColor = System.Drawing.Color.FromArgb(e.NewValue.A, e.NewValue.R, e.NewValue.G, e.NewValue.B);
            StyleFromControl();
        }
    }
}
