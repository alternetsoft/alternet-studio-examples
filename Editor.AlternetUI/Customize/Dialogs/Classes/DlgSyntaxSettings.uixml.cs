using System;
using System.Collections.Generic;
using System.Linq;

using Alternet.Common;
using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Lexer;
using Alternet.UI;
using Customize.Serialization;

namespace Customize.Dialogs.Classes
{
    public partial class DlgSyntaxSettings : Window
    {
        private ISyntaxSettings syntaxSettings;
        private readonly PanelListBoxAndCards mainPanel;
        private bool isFontControlsUpdating = false;
        private bool isControlUpdating;
        private Color curForeColor = SystemColors.ControlText;
        private Color curBkColor = SystemColors.Control;
        private FontStyle curFontStyle;
        private System.Drawing.FontStyle curFntStyle;
        private string curDesc = string.Empty;

        private string stringControl = "control";
        private string stringCtrl = "CTRL";
        private string stringAlt = "alt";
        private string stringShift = "shift";
        private int currentKeys = -1;

        public DlgSyntaxSettings()
        {
            InitializeComponent();
            syntaxSettings = CreateSyntaxSettings();

            mainPanel = new();
            mainPanel.FillPanel.Padding = 5;
            mainPanel.RightPanel.MinWidth = 150;
            mainPanel.VerticalAlignment = VerticalAlignment.Fill;
            mainPanel.Parent = this;

            mainPanel.BindApplicationLog();
            mainPanel.Add("General", ScrollViewer.CreateWithChild(GeneralPanel));
            mainPanel.BottomVisible = false;
            mainPanel.Add("Additional", ScrollViewer.CreateWithChild(AdditionalPanel));
            mainPanel.Add("Fonts and Color", ScrollViewer.CreateWithChild(FontsAndColorPanel));
            mainPanel.Add("Keyboard", ScrollViewer.CreateWithChild(KeyboardPanel));

            mainPanel.Name = "mainPanel";
            mainPanel.CardPanel.Name = "cardPanel";

            Load += DlgSyntaxSettings_Load;

            mainPanel.LeftListBox.SelectFirstItem();

            SetSizeToContent(WindowSizeToContentMode.GrowWidthAndHeight, 20);
        }

        public Button ButtonAccept => OkButton;

        public Button ButtonCancel => CancelButton;

        public Button ButtonApply => ApplyButton;

        public event EventHandler? OkButtonClick;

        public event EventHandler? ApplyButtonClick;

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

        public bool Execute(EditorSettingsTab hiddenTabs)
        {
            UpdateHiddenTabs(hiddenTabs);
            Show();
            return true;
        }

        protected virtual ISyntaxSettings CreateSyntaxSettings()
        {
            return new SyntaxSettings();
        }

        private void UpdateHiddenTabs(EditorSettingsTab hiddenTabs)
        {
        }

        private void DlgSyntaxSettings_Load(object? sender, System.EventArgs e)
        {
            ControlsFromSettings();
            StylesListBox.SelectedIndexChanged += new EventHandler(OnStyleSelected);
            StylesListBox.SelectedIndex = 0;
            chbBold.CheckedChanged += new EventHandler(FontStyleChange);
            chbItalic.CheckedChanged += new EventHandler(FontStyleChange);
            chbUnderline.CheckedChanged += new EventHandler(FontStyleChange);
            DescriptionTextBox.TextChanged += new EventHandler(DescriptionChanged);
            cbFontName.SelectedIndexChanged += new EventHandler(FontNameChanged);
            FontSizeTextBox.TextChanged += new EventHandler(FontSizeChanged);
            OkButton.Click += OkButton_Click;
            ApplyButton.Click += ApplyButton_Click;
            cbForeColor.SelectedIndexChanged += ForeColorComboBox_SelectedIndexChanged;
            cbBackColor.SelectedIndexChanged += BackColorComboBox_SelectedIndexChanged;
            tbShowCommands.TextChanged += ShowCommandsTextBox_TextChanged;
            lbEventHandlers.SelectedIndexChanged += EventHandlersListBox_SelectedIndexChanged;
            cbColorThemes.SelectedIndexChanged += VisualThemesComboBox_SelectedIndexChanged;
            cbColorThemes.MouseLeave += VisualThemesComboBox_Leave;
            AddColorThemeButton.Click += AddVisualThemeButton_Click;
            DeleteColorThemeButton.Click += DeleteVisualThemeButton_Click;
            cbFontName.SelectedIndexChanged += FontNameComboBox_SelectedIndexChanged;
            FontSizeTextBox.MouseLeave += FontSizeTextBox_Leave;
            UpdateShortcutButton.Click += UpdateShortcutButton_Click;
            cbShortcuts.SelectedIndexChanged += Shortcuts_SelectedIndexChanged;
        }

        private void Shortcuts_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbShortcuts.SelectedIndex >= 0)
            {
                currentKeys = cbShortcuts.SelectedIndex.Value;
            }
        }

        private void UpdateShortcutButton_Click(object? sender, EventArgs e)
        {
            if (lbEventHandlers.SelectedIndex == null)
                return;

            int index = lbEventHandlers.SelectedIndex.Value;
            var oldText = cbShortcuts.Items[currentKeys].ToString();
            string newText = cbShortcuts.Text;
            if (string.Compare(oldText, newText) == 0)
            {
                return;
            }

            var keys = KeyUtils.KeyDataFromString(oldText ?? string.Empty);
            var eventName = index >= 0 ? lbEventHandlers.Items[index].ToString() : string.Empty;
            foreach (IKeyData keyData in syntaxSettings.EventDataList)
            {
                if (string.IsNullOrEmpty(keyData.EventName))
                    continue;
                if (eventName?.StartsWith(keyData.EventName) ?? false)
                {
                    string parName = (eventName.Length > keyData.EventName.Length)
                        ? eventName.Remove(0, keyData.EventName.Length) : string.Empty;
                    if ((keyData.Param == null) || (keyData.Param.ToString() == parName))
                    {
                        if (keyData.Keys == keys)
                        {
                            keyData.Keys
                                = KeyUtils.KeyDataFromString(newText.Replace("+", string.Empty));
                            cbShortcuts.Items[currentKeys] = newText;
                        }
                    }
                }
            }
        }

        private void FontSizeTextBox_Leave(object? sender, EventArgs e)
        {
            UpdateActiveVisualThemeFont();
        }

        private void UpdateActiveVisualThemeFont()
        {
            if (isFontControlsUpdating)
                return;

            if ((cbFontName.SelectedIndex >= 0) && (FontSizeTextBox.Text != null))
            {
                var fontName = cbFontName.SelectedItem?.ToString();
                if (fontName is not null)
                {
                    syntaxSettings.Font = new Font(
                        fontName,
                        GetInt(FontSizeTextBox.Text, 10),
                        FontStyle.Regular);
                }
                // todo
                //syntaxSettings.Font = new Font(fontName, GetInt(FontSizeTextBox.Text, 10), FontInfos.GetAvailableFontStyle(new FontFamily(fontName), FontStyle.Regular));
            }
        }

        private void FontNameComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateActiveVisualThemeFont();
        }

        private void VisualThemesComboBox_Leave(object? sender, EventArgs e)
        {
            if (cbColorThemes.SelectedIndex == -1)
            {
                if (syntaxSettings.VisualThemes.ActiveTheme is null)
                    return;

                if ((!syntaxSettings.VisualThemes.ActiveTheme.ReadOnly)
                    && (cbColorThemes.Text != string.Empty))
                {
                    syntaxSettings.VisualThemes.ActiveTheme.Name = cbColorThemes.Text;
                    FillVisualThemes();
                }
            }
        }

        private void DeleteVisualThemeButton_Click(object? sender, EventArgs e)
        {
            IVisualThemes colorThemes = syntaxSettings.VisualThemes;

            if (colorThemes.ActiveThemeIndex != -1)
            {
                if (!colorThemes[colorThemes.ActiveThemeIndex].ReadOnly)
                {
                    colorThemes.RemoveAt(colorThemes.ActiveThemeIndex);
                    colorThemes.ActiveThemeIndex--;
                    FillVisualThemes();
                    cbColorThemes.SelectedIndex = colorThemes.ActiveThemeIndex;
                }
            }
        }

        private void AddVisualThemeButton_Click(object? sender, EventArgs e)
        {
            IVisualThemes colorThemes = syntaxSettings.VisualThemes;
            var info = colorThemes.ActiveTheme?.GetSerializationInfo();

            if (info is null)
                return;

            VisualTheme theme = new();

            info.Load();
            theme.SetSerializationInfo(info);
            colorThemes.Add(theme);

            int newVisualThemeIndex = colorThemes.Count - 1;
            string name = colorThemes[newVisualThemeIndex].Name;
            colorThemes[newVisualThemeIndex].Name = "Copy of " + name;
            colorThemes[newVisualThemeIndex].ReadOnly = false;
            FillVisualThemes();
            colorThemes.ActiveThemeIndex = newVisualThemeIndex;
            cbColorThemes.SelectedIndex = newVisualThemeIndex;
        }

        private void VisualThemesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if ((cbColorThemes.SelectedIndex >= 0)
                && (cbColorThemes.SelectedIndex < syntaxSettings.VisualThemes.Count))
            {
                syntaxSettings.VisualThemes.ActiveThemeIndex = cbColorThemes.SelectedIndex.Value;

                var activeThemeReadonly = syntaxSettings.VisualThemes.ActiveTheme?.ReadOnly ?? true;

                DeleteColorThemeButton.Enabled = !activeThemeReadonly;
                UpdateFontControls();
                FillStyles();
                StyleSelected();
            }
        }

        private void EventHandlersListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lbEventHandlers.SelectedIndex == null)
                return;

            UpdateShortcut(lbEventHandlers.SelectedIndex.Value);
        }

        private void UpdateShortcut(int index)
        {
            cbShortcuts.Text = string.Empty;
            cbShortcuts.Items.Clear();
            var eventName = index >= 0 ? lbEventHandlers.Items[index].ToString() : string.Empty;
            foreach (IKeyData keyData in syntaxSettings.EventDataList)
            {
                if (string.IsNullOrEmpty(keyData.EventName))
                    continue;

                if (eventName?.StartsWith(keyData.EventName) ?? false)
                {
                    string parName = (eventName.Length > keyData.EventName.Length)
                        ? eventName.Remove(0, keyData.EventName.Length) : string.Empty;
                    if (((keyData.Param == null) && string.IsNullOrEmpty(parName))
                        || ((keyData.Param != null) && keyData.Param.ToString() == parName))
                    {
                        string s = ApplyKeyState(keyData);
                        string ss = (s != string.Empty)
                            ? string.Format("{0}, {1}", s, KeyDataToString(keyData.Keys))
                            : KeyDataToString(keyData.Keys);
                        cbShortcuts.Items.Add(ss);
                    }
                }
            }

            if (cbShortcuts.Items.Count > 0)
                cbShortcuts.SelectedIndex = 0;
        }

        private string ApplyKeyState(IKeyData key)
        {
            string result = string.Empty;
            if (key.State > 0)
            {
                IKeyData[] keys = syntaxSettings.EventDataList.ToArray();
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
                if (s[i].IndexOf(stringControl, StringComparison.OrdinalIgnoreCase) >= 0)
                    isCtrl = true;
                else
                    if (s[i].IndexOf(stringAlt, StringComparison.OrdinalIgnoreCase) >= 0)
                    isAlt = true;
                else
                        if (s[i].IndexOf(stringShift, StringComparison.OrdinalIgnoreCase) >= 0)
                    isShift = true;
                else
                    result = (result != string.Empty) ? string.Format("{0} + {1}", result, s[i]) : s[i];
            }

            if (isAlt)
                result = (result != string.Empty)
                    ? string.Format("{0} + {1}", stringAlt.ToUpper(), result) : stringAlt.ToUpper();
            if (isShift)
                result = (result != string.Empty)
                    ? string.Format("{0} + {1}", stringShift.ToUpper(), result) : stringShift.ToUpper();
            if (isCtrl)
                result = (result != string.Empty)
                    ? string.Format("{0} + {1}", stringCtrl, result) : stringCtrl;
            return result;
        }

        private void ShowCommandsTextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateEventHandlers();
        }

        private void BackColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isControlUpdating)
                return;
            curBkColor = cbBackColor.Value ?? curBkColor;
            StyleFromControl();
        }

        private void ForeColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isControlUpdating)
                return;
            curForeColor = cbForeColor.Value ?? curForeColor;
            StyleFromControl();
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            SettingsFromControl();
            OkButtonClick?.Invoke(sender, e);
        }

        private void ApplyButton_Click(object? sender, EventArgs e)
        {
            SettingsFromControl();
            ApplyButtonClick?.Invoke(sender, e);
        }

        private void DescriptionChanged(object? sender, EventArgs e)
        {
            if (isControlUpdating)
                return;
            curDesc = DescriptionTextBox.Text;
            StyleFromControl();
        }

        private void StyleFromControl()
        {
            if (StylesListBox.SelectedItem == null)
                return;
            var style = GetSelectedStyle();
            if (style is not null)
            {
                style.ForeColor = curForeColor;
                style.BackColor = curBkColor;
                style.FontStyle = curFontStyle.FromAlternet();
                style.Desc = curDesc;
            }

            UpdateStyleControls();
        }

        private void FontStyleChange(object? sender, EventArgs e)
        {
            if (isControlUpdating)
                return;
            curFontStyle = FontStyle.Regular;
            if (chbBold.Checked)
                curFontStyle |= FontStyle.Bold;
            else
                curFontStyle &= ~FontStyle.Bold;
            if (chbItalic.Checked)
                curFontStyle |= FontStyle.Italic;
            else
                curFontStyle &= ~FontStyle.Italic;
            if (chbUnderline.Checked)
                curFontStyle |= FontStyle.Underline;
            else
                curFontStyle &= ~FontStyle.Underline;
            StyleFromControl();
        }

        private void FillStyles()
        {
            StylesListBox.Items.Clear();
            if (syntaxSettings.LexStyles is null)
                return;
            for (int i = 0; i < syntaxSettings.LexStyles.Count; i++)
                StylesListBox.Items.Add(syntaxSettings.LexStyles[i].Desc);
        }

        private void FillVisualThemes()
        {
            cbColorThemes.BeginUpdate();
            cbColorThemes.Items.Clear();

            foreach (IVisualTheme colorTheme in SyntaxSettings.VisualThemes)
            {
                cbColorThemes.Items.Add(colorTheme.Name);
            }

            cbColorThemes.SelectedIndex = SyntaxSettings.VisualThemes.ActiveThemeIndex;

            cbColorThemes.EndUpdate();
        }

        private string[] GetFonts()
        {
            return FontFamily.Families.Select(f => f.Name).ToArray();
            //FontFamily[] fonts = FontFamily.Families.ToArray();
            //string[] result = new string[fonts.Length];
            //for (int i = 0; i < fonts.Length; i++)
            //    result[i] = fonts[i].Name;
            //return result;
        }

        private void FillEventHandlers()
        {
            UpdateEventHandlers();
        }

        private void UpdateEventHandlers()
        {
            lbEventHandlers.BeginUpdate();
            try
            {
                lbEventHandlers.Items.Clear();
                IList<string> lst = new List<string>();
                foreach (IKeyData keyData in syntaxSettings.EventDataList)
                {
                    if (string.IsNullOrEmpty(keyData.EventName))
                        continue;
                    string s = (keyData.Param != null)
                        ? string.Format("{0}{1}", keyData.EventName, keyData.Param.ToString())
                        : keyData.EventName;
                    if (s != string.Empty)
                    {
                        if (tbShowCommands.Text != string.Empty)
                        {
                            if ((s.IndexOf(tbShowCommands.Text) >= 0)
                                && (lbEventHandlers.Items.IndexOf(s) < 0))
                                lst.Add(s);
                        }
                        else
                            if (lbEventHandlers.Items.IndexOf(s) < 0)
                            lst.Add(s);
                    }
                }

                lst = lst.OrderBy((x) => x).ToList();
                lbEventHandlers.Items.AddRange(lst);
                if (lbEventHandlers.Items.Count > 0)
                    lbEventHandlers.SelectedIndex = 0;
            }
            finally
            {
                lbEventHandlers.EndUpdate();
            }
        }

        private void UpdateFontControls()
        {
            try
            {
                isFontControlsUpdating = true;

                cbFontName.SelectedIndex
                    = cbFontName.Items.IndexOf(syntaxSettings.Font?.Name ?? string.Empty);
                FontSizeTextBox.Text = syntaxSettings.Font?.Size.ToString() ?? string.Empty;
            }
            finally
            {
                isFontControlsUpdating = false;
            }

            FontNameChanged(this, new EventArgs());
            OnStyleSelected(this, new EventArgs());
        }

        private ILexStyle? GetSelectedStyle()
        {
            if (StylesListBox.SelectedIndex != null)
                return syntaxSettings.LexStyles?[StylesListBox.SelectedIndex.Value];
            return null;
        }
        private void StyleSelected()
        {
            if (StylesListBox.SelectedIndex == null)
                return;

            DescriptionTextBox.Enabled
                = syntaxSettings.IsDescriptionEnabled(StylesListBox.SelectedIndex.Value);
            lbDescription.Enabled
                = syntaxSettings.IsDescriptionEnabled(StylesListBox.SelectedIndex.Value);
            gbFontAttributes.Enabled
                = syntaxSettings.IsFontStyleEnabled(StylesListBox.SelectedIndex.Value);

            lbBackColor.Enabled = syntaxSettings.IsBackColorEnabled(StylesListBox.SelectedIndex.Value);
            cbBackColor.Enabled = syntaxSettings.IsBackColorEnabled(StylesListBox.SelectedIndex.Value);
            var style = GetSelectedStyle();
            if (style != null)
            {
                lbDescription.Enabled = true;
                DescriptionTextBox.Enabled = true;

                lbForeColor.Enabled = style.ForeColorEnabled;
                cbForeColor.Enabled = style.ForeColorEnabled;
                lbBackColor.Enabled = style.BackColorEnabled;
                cbBackColor.Enabled = style.BackColorEnabled;
                chbBold.Enabled = style.BoldEnabled;
                chbItalic.Enabled = style.ItalicEnabled;
                chbUnderline.Enabled = style.UnderlineEnabled;

                curForeColor = style.ForeColor;
                curBkColor = style.BackColor;
                curFontStyle = (Alternet.Drawing.FontStyle)style.FontStyle;
                curDesc = style.Desc;
            }

            UpdateStyleControls();
        }

        private void UpdateStyleControls()
        {
            isControlUpdating = true;
            try
            {
                var foreSampleEnabled = cbForeColor.Enabled;
                var backSampleEnabled = cbBackColor.Enabled;

                samplePanelParent.Visible = backSampleEnabled && foreSampleEnabled;

                chbBold.Checked = (curFontStyle & FontStyle.Bold) != 0;
                chbItalic.Checked = (curFontStyle & FontStyle.Italic) != 0;
                chbUnderline.Checked = (curFontStyle & FontStyle.Underline) != 0;
                DescriptionTextBox.Text = curDesc;

                lbSampleText.Font = new Font(
                    lbSampleText.RealFont.Name,
                    (int)lbSampleText.RealFont.Size,
                    curFontStyle);

                lbSampleText.Visible = foreSampleEnabled;

                cbForeColor.SelectedItem = cbForeColor.FindOrAdd(curForeColor);
                cbBackColor.SelectedItem = cbBackColor.FindOrAdd(curBkColor);
                lbSampleText.ForeColor = curForeColor;

                var thColor = syntaxSettings.VisualThemes
                    .ActiveTheme?[VisualThemeConsts.WindowColorInternalName].BackColor;

                lbSampleText.BackgroundColor = (curBkColor != Color.Empty)
                    ? curBkColor
                    : thColor;

                SamplePanel.BackColor = lbSampleText.BackColor;
                WriteSampleText();
            }
            finally
            {
                isControlUpdating = false;
            }
        }

        private void WriteSampleText()
        {
        }

        private void OnStyleSelected(object? sender, EventArgs e)
        {
            StyleSelected();
        }

        private void FontNameChanged(object? sender, System.EventArgs e)
        {
            if (isControlUpdating || (cbFontName.SelectedItem == null))
                return;
            try
            {
                lbSampleText.Font
                    = new Font(cbFontName.Text, lbSampleText.RealFont.Size, curFontStyle);
            }
            catch
            {
            }

            WriteSampleText();
        }

        private static int GetInt(string s, int defaultValue)
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

        private void FontSizeChanged(object? sender, System.EventArgs e)
        {
            if (isControlUpdating)
                return;
            isControlUpdating = true;
            try
            {
                int fontSize = Math.Max(
                    Math.Min(GetInt(FontSizeTextBox.Text, 10), EditConsts.MaxFontSize),
                    1);
                if (FontSizeTextBox.Text != fontSize.ToString())
                    FontSizeTextBox.Text = fontSize.ToString();
                lbSampleText.Font = new Font(lbSampleText.RealFont.Name, fontSize, curFontStyle);
                WriteSampleText();
            }
            finally
            {
                isControlUpdating = false;
            }
        }

        private void SettingsFromControl()
        {
            bool vert = chbVertScrollBar.Checked;
            bool horz = chbHorzScrollBar.Checked;
            bool forced = false;// chbForced.Checked;
            if (horz)
            {
                if (vert)
                    syntaxSettings.ScrollBars = forced
                        ? RichTextBoxScrollBars.ForcedBoth : RichTextBoxScrollBars.Both;
                else
                    syntaxSettings.ScrollBars = forced
                        ? RichTextBoxScrollBars.ForcedHorizontal : RichTextBoxScrollBars.Horizontal;
            }
            else
            {
                if (vert)
                    syntaxSettings.ScrollBars = forced
                        ? RichTextBoxScrollBars.ForcedVertical : RichTextBoxScrollBars.Vertical;
                else
                    syntaxSettings.ScrollBars = RichTextBoxScrollBars.None;
            }

            if (chbDragAndDrop.Checked)
            {
                syntaxSettings.SelectionOptions
                    = syntaxSettings.SelectionOptions & ~SelectionOptions.DisableDragging;
            }
            else
            {
                syntaxSettings.SelectionOptions
                    = syntaxSettings.SelectionOptions | SelectionOptions.DisableDragging;
            }

            syntaxSettings.ShowMargin = chbShowMargin.Checked;
            syntaxSettings.WordWrap = chbWordWrap.Checked;
            syntaxSettings.WhiteSpaceVisible = chbWhiteSpace.Checked;
            syntaxSettings.ShowGutter = chbShowGutter.Checked;
            syntaxSettings.GutterWidth = nudGutterWidth.Value;
            syntaxSettings.MarginPos = nudMarginPosition.Value;

            if (chbLineNumbers.Checked)
                syntaxSettings.GutterOptions |= GutterOptions.PaintLineNumbers;
            else
                syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineNumbers;

            if (chbLineNumbersOnGutter.Checked)
                syntaxSettings.GutterOptions |= GutterOptions.PaintLinesOnGutter;
            else
                syntaxSettings.GutterOptions &= ~GutterOptions.PaintLinesOnGutter;

            if (chbLineModificator.Checked)
                syntaxSettings.GutterOptions |= GutterOptions.PaintLineModificators;
            else
                syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineModificators;

            if (chbLineSeparator.Checked)
                syntaxSettings.SeparatorOptions |= SeparatorOptions.SeparateLines;
            else
                syntaxSettings.SeparatorOptions &= ~SeparatorOptions.SeparateLines;

            if (chbBeyondEol.Checked)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEol;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEol;

            if (chbBeyondEof.Checked)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEof;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEof;

            if (chbMoveOnRightButton.Checked)
                syntaxSettings.NavigateOptions |= NavigateOptions.MoveOnRightButton;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.MoveOnRightButton;

            if (chbShowHints.Checked)
                syntaxSettings.OutlineOptions |= OutlineOptions.ShowHints;
            else
                syntaxSettings.OutlineOptions &= ~OutlineOptions.ShowHints;

            syntaxSettings.HighlightHyperText = chbHighlightUrls.Checked;
            syntaxSettings.AllowOutlining = chbAllowOutlining.Checked;
            syntaxSettings.UseSpaces = rbInsertSpaces.IsChecked;

            string[] s = tbTabStops.Text.Split(',');
            int[] tabs = new int[s.Length];
            int j = 0;
            for (int i = 0; i < s.Length; i++)
            {
                j = GetInt(s[i], EditConsts.DefaultTabStop);
                tabs[i] = (j <= 0) ? EditConsts.DefaultTabStop : j;
            }

            syntaxSettings.TabStops = tabs;

            var fnt = syntaxSettings.Font ?? Control.DefaultFont;
            syntaxSettings.Font = new Font(
                fnt.Name,
                Math.Max(Math.Min(GetInt(FontSizeTextBox.Text, 10), EditConsts.MaxFontSize),
                1),
                fnt.Style);
        }

        private void ControlsFromSettings()
        {
            FillStyles();
            FillVisualThemes();
            cbFontName.Items.AddRange(GetFonts());
            cbKeyboardSchemes.Items.Clear();
            cbKeyboardSchemes.Items.Add("Default Settings");
            cbKeyboardSchemes.SelectedIndex = 0;
            FillEventHandlers();
            chbDragAndDrop.Checked
                = (syntaxSettings.SelectionOptions & SelectionOptions.DisableDragging) == 0;
            switch (syntaxSettings.ScrollBars)
            {
                case RichTextBoxScrollBars.ForcedBoth:
                    {
                        chbVertScrollBar.Checked = true;
                        chbHorzScrollBar.Checked = true;
                        //chbForced.Checked = true;
                        break;
                    }

                case RichTextBoxScrollBars.Both:
                    {
                        chbVertScrollBar.Checked = true;
                        chbHorzScrollBar.Checked = true;
                        //chbForced.Checked = false;
                        break;
                    }

                case RichTextBoxScrollBars.ForcedHorizontal:
                case RichTextBoxScrollBars.Horizontal:
                    {
                        chbVertScrollBar.Checked = false;
                        chbHorzScrollBar.Checked = true;
                        //chbForced.Checked = false;
                        break;
                    }

                case RichTextBoxScrollBars.ForcedVertical:
                case RichTextBoxScrollBars.Vertical:
                    {
                        chbVertScrollBar.Checked = true;
                        chbHorzScrollBar.Checked = false;
                        //chbForced.Checked = false;
                        break;
                    }

                case RichTextBoxScrollBars.None:
                    {
                        chbVertScrollBar.Checked = false;
                        chbHorzScrollBar.Checked = false;
                        //chbForced.Checked = false;
                        break;
                    }
            }

            chbShowMargin.Checked = syntaxSettings.ShowMargin;
            chbWordWrap.Checked = syntaxSettings.WordWrap;
            chbLineNumbers.Checked
                = (syntaxSettings.GutterOptions & GutterOptions.PaintLineNumbers) != 0;
            chbLineNumbersOnGutter.Checked
                = (syntaxSettings.GutterOptions & GutterOptions.PaintLinesOnGutter) != 0;
            chbShowGutter.Checked = syntaxSettings.ShowGutter;
            nudGutterWidth.Value = (int)syntaxSettings.GutterWidth;
            nudMarginPosition.Value = (int)syntaxSettings.MarginPos;
            chbBeyondEol.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEol) != 0;
            chbBeyondEof.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEof) != 0;
            chbMoveOnRightButton.Checked
                = (syntaxSettings.NavigateOptions & NavigateOptions.MoveOnRightButton) != 0;
            chbHighlightUrls.Checked = syntaxSettings.HighlightHyperText;
            chbAllowOutlining.Checked = syntaxSettings.AllowOutlining;
            chbShowHints.Checked = (syntaxSettings.OutlineOptions & OutlineOptions.ShowHints) != 0;
            rbInsertSpaces.IsChecked = syntaxSettings.UseSpaces;
            rbKeepTabs.IsChecked = !syntaxSettings.UseSpaces;
            chbWhiteSpace.Checked = syntaxSettings.WhiteSpaceVisible;
            chbLineModificator.Checked
                = (syntaxSettings.GutterOptions & GutterOptions.PaintLineModificators) != 0;
            chbLineSeparator.Checked
                = (syntaxSettings.SeparatorOptions & SeparatorOptions.SeparateLines) != 0;

            string[] s = new string[syntaxSettings.TabStops.Length];
            for (int i = 0; i < syntaxSettings.TabStops.Length; i++)
                s[i] = syntaxSettings.TabStops[i].ToString();
            tbTabStops.Text = string.Join(",", s);
            UpdateFontControls();
        }
    }
}
