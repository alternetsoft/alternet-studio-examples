using System;
using System.Collections.Generic;
using System.Linq;

using Alternet.Common;
using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Syntax.Lexer;
using Alternet.UI;

namespace Alternet.Editor.CustomizeDialog.AlternetUI
{
    public partial class DlgSyntaxSettings : Window
    {
        private readonly SpeedButton? shortcutPopupButton;
        private readonly PanelListBoxAndCards mainPanel;
        private readonly AbstractControl additionalPanel;
        private readonly ISyntaxSettings syntaxSettings;
        private readonly AbstractControl? keyboardPanel;

        private bool isFontControlsUpdating = false;
        private bool isControlUpdating;
        private Color curForeColor = SystemColors.ControlText;
        private Color curBkColor = SystemColors.Control;
        private FontStyle curFontStyle;
        private string curDesc = string.Empty;

        private CheckBox? chbBeyondEol;
        private CheckBox? chbBeyondEof;
        private CheckBox? chbMoveOnRightButton;
        private CheckBox? chbAllowOutlining;
        private CheckBox? chbShowHints;
        private TextBox? tbTabStops;
        private RadioButton? rbInsertSpaces;
        private RadioButton? rbKeepTabs;

        private ListPicker? cbKeyboardSchemes;
        private TextBox? tbShowCommands;
#pragma warning disable
        private StdTreeView? lbEventHandlers;
#pragma warning restore
        private ToolBar? ShortcutsToolBar;
        private VirtualListBox? cbShortcuts;

        static DlgSyntaxSettings()
        {
        }

        public DlgSyntaxSettings()
        {
            InitializeComponent();

            syntaxSettings = CreateSyntaxSettings();

            mainPanel = new();
            mainPanel.LeftListBox.HasBorder = true;
            mainPanel.FillPanel.Padding = 0;
            mainPanel.RightPanel.MinWidth = 150;
            mainPanel.VerticalAlignment = VerticalAlignment.Fill;
            mainPanel.Parent = this;

            additionalPanel = CreateAdditionalPanel();
            keyboardPanel = CreateKeyboardPanel();

            var scrollViewerGeneralPanel = ScrollViewer.CreateWithChild(GeneralPanel);
            var scrollViewerAdditionalPanel = ScrollViewer.CreateWithChild(additionalPanel);
            var scrollViewerFontsAndColorPanel = ScrollViewer.CreateWithChild(FontsAndColorPanel);
            var scrollViewerKeyboardPanel = ScrollViewer.CreateWithChild(keyboardPanel);

            scrollViewerKeyboardPanel.ParentChanged += (s, e) =>
            {
                App.AddIdleTask(() =>
                {
                    if (lbEventHandlers?.SelectedItem is null)
                        lbEventHandlers?.SelectFirstItemAndScroll();
                    lbEventHandlers?.Refresh();
                    cbShortcuts?.Refresh();
                });
            };

            scrollViewerFontsAndColorPanel.IsScrolledWithMouseWheel = false;
            scrollViewerKeyboardPanel.IsScrolledWithMouseWheel = false;

            mainPanel.CardPanel.HasBorder = true;

            mainPanel.Add("General", scrollViewerGeneralPanel);
            mainPanel.BottomVisible = false;
            mainPanel.Add("Additional", scrollViewerAdditionalPanel);
            mainPanel.Add("Fonts and Color", scrollViewerFontsAndColorPanel);
            mainPanel.Add("Keyboard", scrollViewerKeyboardPanel);

            mainPanel.Name = "mainPanel";
            mainPanel.CardPanel.Name = "cardPanel";

            SetSizeToContent(WindowSizeToContentMode.GrowWidthAndHeight, 20);

            cbBackColor.AddColor(Color.Empty, "Default");
            cbForeColor.AddColor(Color.Empty, "Default");

            ActiveControl = mainPanel.LeftListBox;

            ShortcutsToolBar?.AddText("Shortcut(s) for selected command:");

            ContextMenu shortcutsContextMenu = new();

            var editShortCutButton = shortcutsContextMenu.Add("Edit shortcut...", () =>
            {
                EditSelectedShortcut();
            });

            var selectStateItem = shortcutsContextMenu.Add("Select category...", () =>
            {
                var dialog = PopupListBox.Default;
                dialog.ResetAfterHideEvent();
                dialog.MainControl.RemoveAll();
                dialog.Title = "Select category";

                dialog.AfterHide += AfterHideCategoryPopup;

                void AfterHideCategoryPopup(object? s, EventArgs e)
                {
                    dialog.MainControl.AfterHide -= AfterHideCategoryPopup;
                    if (dialog.PopupResult != ModalResult.Accepted)
                        return;
                    var resultItem = dialog.ResultItem;

                    if (cbShortcuts?.SelectedItem?.Value is IKeyData item)
                    {
                        item.State = (int?)dialog.ResultItem?.Value ?? 0;
                        UpdateShortcut(lbEventHandlers?.SelectedItem?.Text);
                    }
                }

                Enum.GetValues<KeyList.KeyStates>().ToList().ForEach(state =>
                {
                    dialog.MainControl.Add(new(state.ToString(), (int)state));
                });

                if (cbShortcuts?.SelectedItem?.Value is IKeyData item)
                {
                    dialog.MainControl.SelectedItem = dialog.MainControl.FindItemWithValue(item.State);
                }

                dialog.ShowPopup(shortcutPopupButton!);
            });

            shortcutsContextMenu.AddSeparator();    

            var removeShortcutItem = shortcutsContextMenu.Add("Remove shortcut", () =>
            {
                RemoveSelectedShortcut();
            });

            shortcutsContextMenu.Opening += (s, e) =>
            {
                var item = cbShortcuts?.SelectedItem?.Value as IKeyData;
                var hasSelected = cbShortcuts?.SelectedItem != null;

                removeShortcutItem.Enabled = hasSelected;
                editShortCutButton.Enabled = hasSelected;
            };

            if (ShortcutsToolBar is not null)
            {
                shortcutPopupButton = ShortcutsToolBar.AddSpeedBtnCore(
                    null,
                    KnownSvgImages.ImgMoreActions,
                    "Shortcut actions");
                shortcutPopupButton.HideToolTipOnClick = true;
                ShortcutsToolBar.SetToolDropDownMenu(shortcutPopupButton.UniqueId, shortcutsContextMenu);
                shortcutPopupButton.HorizontalAlignment = HorizontalAlignment.Right;
            }

            Load += (s, e) =>
            {
                ControlsFromSettings();
                StylesListBox.SelectedIndexChanged += OnStyleSelected;
                StylesListBox.SelectedIndex = 0;
                chbBold.CheckedChanged += FontStyleChange;
                chbItalic.CheckedChanged += FontStyleChange;
                chbUnderline.CheckedChanged += FontStyleChange;
                DescriptionTextBox.TextChanged += DescriptionChanged;
                FontSizeTextBox.TextChanged += FontSizeChanged;
                OkButton.Click += OkButton_Click;
                ApplyButton.Click += ApplyButton_Click;
                cbForeColor.ValueChanged += ForeColorComboBox_SelectedIndexChanged;
                cbBackColor.ValueChanged += BackColorComboBox_SelectedIndexChanged;

                if(tbShowCommands is not null)
                    tbShowCommands.TextChanged += ShowCommandsTextBox_TextChanged;

                if (lbEventHandlers is not null)
                    lbEventHandlers.SelectionChanged += EventHandlersListBox_SelectedIndexChanged;

                cbColorThemes.ValueChanged += VisualThemesComboBox_SelectedIndexChanged;
                AddColorThemeButton.Click += AddVisualThemeButton_Click;
                DeleteColorThemeButton.Click += DeleteVisualThemeButton_Click;
                cbFontName.ValueChanged += FontNameChanged;

                App.AddIdleTask(() =>
                {
                    mainPanel.LeftListBox.SelectFirstItemAndScroll();
                    mainPanel.LeftListBox.SetFocusIfPossible();
                    lbEventHandlers?.SelectFirstItemAndScroll();
                });
            };
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

        internal virtual void WriteSampleText()
        {
        }

        protected virtual ISyntaxSettings CreateSyntaxSettings()
        {
            return new SyntaxSettings();
        }

        protected virtual bool ReportError(string? s, Keys keys)
        {
            if (keys == Keys.None && !string.IsNullOrEmpty(s))
            {
                MessageBox.Show(
                    $"Error parsing shortcut '{s}'. Sample shortcuts: CTRL+SHIFT+ALT+C, SHIFT+F10",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return true;
            }

            return false;
        }

        protected virtual void UpdateHiddenTabs(EditorSettingsTab hiddenTabs)
        {
        }

        private Panel CreateKeyboardPanel()
        {
            var keyboardPanel = new Panel
            {
                Layout = LayoutStyle.Vertical,
                HorizontalAlignment = HorizontalAlignment.Fill,
                VerticalAlignment = VerticalAlignment.Fill,
                Visible = false,
                Padding = new Thickness(5, 0, 0, 0),
                MinChildMargin = 5,
            };

            keyboardPanel.Children.Add(new Label
            {
                Text = "Keyboard mapping scheme:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = 5,
                RowColumn = (0, 0),
            });

            cbKeyboardSchemes = new ListPicker
            {
                Margin = 5,
                VerticalAlignment = VerticalAlignment.Center,
                RowColumn = (1, 0),
            };

            keyboardPanel.Children.Add(cbKeyboardSchemes);

            keyboardPanel.Children.Add(new Label
            {
                Text = "Show commands containing:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = 5,
                RowColumn = (2, 0),
            });

            tbShowCommands = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = 5,
                RowColumn = (3, 0),
            };

            keyboardPanel.Children.Add(tbShowCommands);

#pragma warning disable
            lbEventHandlers = new StdTreeView
            {
                MinWidth = 120,
                SuggestedHeight = 210,
                Margin = 5,
                RowColumn = (4, 0),
            };
#pragma warning restore

            keyboardPanel.Children.Add(lbEventHandlers);

            var shortcutPanel = new VerticalStackPanel();

            ShortcutsToolBar = new ToolBar
            {
                MarginBottom = 5,
            };

            shortcutPanel.Children.Add(ShortcutsToolBar);

            cbShortcuts = new VirtualListBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 120,
                SuggestedHeight = 120,
            };

            shortcutPanel.Children.Add(cbShortcuts);
            keyboardPanel.Children.Add(shortcutPanel);

            return keyboardPanel;
        }

        private Panel CreateAdditionalPanel()
        {
            var additionalPanel = new Panel
            {
                Layout = LayoutStyle.Vertical,
                HorizontalAlignment = HorizontalAlignment.Left,
                Visible = false,
                Padding = new Thickness(5, 0, 0, 0),
                MinChildMargin = 5,
            };

            additionalPanel.Children.Add(new Label
            {
                Text = "Navigate Options:",
                IsBold = true,
            });

            var navGrid = new Grid
            {
                RowCount = 3,
                ColumnCount = 0,
                MinChildMargin = 5,
            };

            chbBeyondEol = new CheckBox
            {
                Text = "Beyond Eol",
                Margin = new Thickness(0, 0, 5, 0),
                RowColumn = (0, 0),
            };

            navGrid.Children.Add(chbBeyondEol);

            chbBeyondEof = new CheckBox
            {
                Text = "Beyond Eof",
                Margin = new Thickness(0, 0, 5, 0),
                RowColumn = (1, 0),
            };

            navGrid.Children.Add(chbBeyondEof);

            chbMoveOnRightButton = new CheckBox
            {
                Text = "Move on Right Button",
                Margin = new Thickness(0, 0, 5, 0),
                RowColumn = (2, 0),
            };

            navGrid.Children.Add(chbMoveOnRightButton);
            additionalPanel.Children.Add(navGrid);
            additionalPanel.Children.Add(new HorizontalLine());

            additionalPanel.Children.Add(new Label
            {
                Text = "Outline Options:",
                IsBold = true,
            });

            var outlineGrid = new Grid
            {
                RowCount = 2,
                ColumnCount = 0,
                MinChildMargin = 5,
            };

            chbAllowOutlining = new CheckBox
            {
                Text = "Allow outlining",
                Margin = new Thickness(0, 0, 5, 0),
                RowColumn = (0, 0),
            };

            outlineGrid.Children.Add(chbAllowOutlining);

            chbShowHints = new CheckBox
            {
                Name = "chbShowHints",
                Text = "Show Hints",
                ToolTip = "",
                Margin = new Thickness(0, 0, 5, 0),
                RowColumn = (1, 0),
            };

            outlineGrid.Children.Add(chbShowHints);

            additionalPanel.Children.Add(outlineGrid);
            additionalPanel.Children.Add(new HorizontalLine());

            additionalPanel.Children.Add(new Label
            {
                Text = "Tab Options:",
                IsBold = true,
            });

            var tabGrid = new Grid
            {
                RowCount = 2,
                ColumnCount = 2,
                MinChildMargin = 5,
            };

            tabGrid.Children.Add(new Label
            {
                Name = "lbTabSizes",
                Text = "Tab Sizes:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = 5,
                RowColumn = (0, 0),
            });

            tbTabStops = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = 5,
                RowColumn = (1, 0),
            };

            tabGrid.Children.Add(tbTabStops);

            rbInsertSpaces = new RadioButton
            {
                Text = "Insert spaces",
                IsChecked = true,
                Margin = 5,
                RowColumn = (0, 1),
            };

            tabGrid.Children.Add(rbInsertSpaces);

            rbKeepTabs = new RadioButton
            {
                Text = "Keep tabs",
                IsChecked = true,
                Margin = 5,
                RowColumn = (1, 1),
            };

            tabGrid.Children.Add(rbKeepTabs);

            additionalPanel.Children.Add(tabGrid);

            return additionalPanel;
        }

        private void RemoveSelectedShortcut()
        {
            App.AddIdleTask(() =>
            {
                if (cbShortcuts?.SelectedItem?.Value is not IKeyData itemToRemove)
                    return;

                cbShortcuts.RemoveSelectedItem();

                for (int i = 0; i < syntaxSettings.EventDataList.Count; i++)
                {
                    IKeyData keyData = syntaxSettings.EventDataList[i];
                    if (keyData == itemToRemove)
                    {
                        syntaxSettings.EventDataList.RemoveAt(i);
                        break;
                    }
                }
            });
        }

        private void EditSelectedShortcut()
        {
            if (cbShortcuts?.SelectedItem?.Value is not IKeyData item)
                return;
            var oldText = KeyUtils.KeyDataToString(item.Keys);

            TextFromUserParams prm = new()
            {
                Title = "Edit Shortcut",
                Message = "Specify shortcut for the command:",
                DefaultValue = oldText,
                OnApply = (s) =>
                {
                    string newText = s ?? string.Empty;
                    if (string.Compare(oldText, newText) == 0)
                    {
                        return;
                    }

                    var newKeys = KeyUtils.KeyDataFromString(s);
                    if (ReportError(newText, newKeys))
                        return;

                    item.Keys = newKeys;
                    UpdateShortcut(lbEventHandlers?.SelectedItem?.Text);
                },
            };

            DialogFactory.GetTextFromUserAsync(prm);
        }

        private void FontSizeTextBox_Leave(object? sender, EventArgs e)
        {
            UpdateActiveVisualThemeFont();
        }

        private void UpdateActiveVisualThemeFont()
        {
            if (isFontControlsUpdating)
                return;

            var fontName = cbFontName.Value?.ToString();
            if (fontName is not null)
            {
                syntaxSettings.Font = new Font(
                    fontName,
                    GetInt(FontSizeTextBox.Text, 10),
                    FontStyle.Regular);
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
                    cbColorThemes.Value = colorThemes.ActiveTheme?.Name;
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
            cbColorThemes.Value = colorThemes.ActiveTheme?.Name;
        }

        private int GetThemeIndex(string? themeName)
        {
            for (int i = 0; i < syntaxSettings.VisualThemes.Count; i++)
            {
                if (string.Compare(syntaxSettings.VisualThemes[i].Name, themeName, StringComparison.OrdinalIgnoreCase) == 0)
                    return i;
            }
            return -1;
        }

        private void VisualThemesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var themeIndex = GetThemeIndex(cbColorThemes.Value?.ToString());

            if ((themeIndex >= 0)
                && (themeIndex < syntaxSettings.VisualThemes.Count))
            {
                syntaxSettings.VisualThemes.ActiveThemeIndex = themeIndex;

                var activeThemeReadonly = syntaxSettings.VisualThemes.ActiveTheme?.ReadOnly ?? true;

                DeleteColorThemeButton.Enabled = !activeThemeReadonly;
                UpdateFontControls();
                FillStyles();
                StyleSelected();
            }
        }

        private void EventHandlersListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lbEventHandlers?.SelectedItem == null)
                return;

            UpdateShortcut(lbEventHandlers.SelectedItem?.Text);
        }

        private void UpdateShortcut(string? eventName)
        {
            cbShortcuts?.Items.Clear();

            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            foreach (IKeyData keyData in syntaxSettings.EventDataList)
            {
                if (string.IsNullOrEmpty(keyData.EventName))
                    continue;

                if (eventName?.StartsWith(keyData.EventName) ?? false)
                {
#pragma warning disable
                    string parName = (eventName.Length > keyData.EventName.Length)
                        ? eventName.Remove(0, keyData.EventName.Length) : string.Empty;
#pragma warning restore
                    if (((keyData.Param == null) && string.IsNullOrEmpty(parName))
                        || ((keyData.Param != null) && keyData.Param.ToString() == parName))
                    {
                        string s = ApplyKeyState(keyData);
                        string ss = (s != string.Empty)
                            ? string.Format("{0}, {1}", s, KeyUtils.KeyDataToString(keyData.Keys))
                            : KeyUtils.KeyDataToString(keyData.Keys);
                        cbShortcuts?.Add(new(ss, keyData));
                    }
                }
            }

            var hasKeys = cbShortcuts?.Items.Count > 0;

            if (hasKeys)
                cbShortcuts?.SelectFirstItem();
        }

        private void UpdateShortcut(int index)
        {
            var eventName = index >= 0
                ? lbEventHandlers?.RootItem.Items[index].ToString() : string.Empty;
            UpdateShortcut(eventName);
        }

        private string ApplyKeyState(IKeyData key)
        {
            string result = string.Empty;
            if (key.State > 0)
            {
#pragma warning disable
                IKeyData[] keys = syntaxSettings.EventDataList.ToArray();
#pragma warning restore
                foreach (IKeyData keyData in keys)
                {
                    if ((keyData.LeaveState == key.State) && (keyData.State == 0))
                    {
                        result = KeyUtils.KeyDataToString(keyData.Keys);
                        break;
                    }
                }
            }

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
                style.FontStyle = curFontStyle.ToLexFontStyle();
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
            StylesListBox.DoInsideUpdate(() =>
            {
                StylesListBox.Items.Clear();
                if (syntaxSettings.LexStyles is null)
                    return;
                for (int i = 0; i < syntaxSettings.LexStyles.Count; i++)
                    StylesListBox.Items.Add(syntaxSettings.LexStyles[i].Desc);
            });
        }

        private void FillVisualThemes()
        {
            cbColorThemes.BeginUpdate();
            cbColorThemes.Items.Clear();

            foreach (IVisualTheme colorTheme in SyntaxSettings.VisualThemes)
            {
                cbColorThemes.Add(colorTheme.Name);
            }

            cbColorThemes.Value = SyntaxSettings.VisualThemes.ActiveTheme?.Name ?? string.Empty;

            cbColorThemes.EndUpdate();
        }

        private void FillEventHandlers()
        {
            UpdateEventHandlers();
        }

        private void UpdateEventHandlers()
        {
            BaseDictionary<string, TreeViewItem> items = [];

            var filter = tbShowCommands?.Text;
            if(string.IsNullOrEmpty(filter))
                filter = null;

            foreach (IKeyData keyData in syntaxSettings.EventDataList)
            {
                if (string.IsNullOrEmpty(keyData.EventName))
                    continue;
                string s = (keyData.Param != null)
                    ? string.Format("{0}{1}", keyData.EventName, keyData.Param.ToString())
                    : keyData.EventName;
                if (s != string.Empty)
                {
                    bool matchesFilter = (filter is null)
                        || s.Contains(filter, StringComparison.CurrentCultureIgnoreCase);

                    if (!matchesFilter || items.ContainsKey(s))
                        continue;

                    var item = new TreeViewItem(s)
                    {
                        Value = keyData,
                    };

                    items.Add(s, item);
                }
            }

            lbEventHandlers?.RootItem.SetItems(items.Values.Distinct().OrderBy(item => item.Text));
            lbEventHandlers?.SelectFirstItemAndScroll();
            if(lbEventHandlers?.RootItem.ItemCount == 0)
            {
                UpdateShortcut(null);
            }
            else
                UpdateShortcut(0);
        }

        private void UpdateFontControls()
        {
            try
            {
                isFontControlsUpdating = true;

                cbFontName.Value = syntaxSettings.Font?.Name;
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

                cbForeColor.Select(curForeColor);
                cbBackColor.Select(curBkColor);
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

        private void OnStyleSelected(object? sender, EventArgs e)
        {
            StyleSelected();
        }

        private void FontNameChanged(object? sender, System.EventArgs e)
        {
            if (isControlUpdating || (cbFontName.Value == null))
                return;
            try
            {
                lbSampleText.Font = new Font(
                    cbFontName.Value?.ToString() ?? Control.DefaultFont.Name,
                    lbSampleText.RealFont.Size,
                    curFontStyle);
            }
            catch
            {
            }

            WriteSampleText();

            UpdateActiveVisualThemeFont();
        }

        private static int GetInt(string s, int defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(s))
                    return defaultValue;
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
                syntaxSettings.SelectionOptions &= ~SelectionOptions.DisableDragging;
            }
            else
            {
                syntaxSettings.SelectionOptions |= SelectionOptions.DisableDragging;
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

            if (chbBeyondEol?.Checked == true)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEol;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEol;

            if (chbBeyondEof?.Checked == true)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEof;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEof;

            if (chbMoveOnRightButton?.Checked == true)
                syntaxSettings.NavigateOptions |= NavigateOptions.MoveOnRightButton;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.MoveOnRightButton;

            if (chbShowHints?.Checked == true)
                syntaxSettings.OutlineOptions |= OutlineOptions.ShowHints;
            else
                syntaxSettings.OutlineOptions &= ~OutlineOptions.ShowHints;

            syntaxSettings.HighlightHyperText = chbHighlightUrls.Checked;
            syntaxSettings.AllowOutlining = chbAllowOutlining?.Checked ?? false;
            syntaxSettings.UseSpaces = rbInsertSpaces?.IsChecked ?? false;

            string[] s = tbTabStops?.Text.Split(',') ?? [];
            int[] tabs = new int[s.Length];
            int j;
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
            cbKeyboardSchemes?.Items.Clear();
            cbKeyboardSchemes?.Add("Default Settings");
            cbKeyboardSchemes?.SelectFirstItem();
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

            chbBeyondEol?.SetChecked((syntaxSettings.NavigateOptions & NavigateOptions.BeyondEol) != 0);
            chbBeyondEof?.SetChecked((syntaxSettings.NavigateOptions & NavigateOptions.BeyondEof) != 0);
            chbMoveOnRightButton?.SetChecked((syntaxSettings.NavigateOptions & NavigateOptions.MoveOnRightButton) != 0);
            chbHighlightUrls?.SetChecked(syntaxSettings.HighlightHyperText);
            chbAllowOutlining?.SetChecked(syntaxSettings.AllowOutlining);
            chbShowHints?.SetChecked((syntaxSettings.OutlineOptions & OutlineOptions.ShowHints) != 0);
            rbInsertSpaces?.SetChecked(syntaxSettings.UseSpaces);
            rbKeepTabs?.SetChecked(!syntaxSettings.UseSpaces);
            chbWhiteSpace.Checked = syntaxSettings.WhiteSpaceVisible;
            chbLineModificator.Checked
                = (syntaxSettings.GutterOptions & GutterOptions.PaintLineModificators) != 0;
            chbLineSeparator.Checked
                = (syntaxSettings.SeparatorOptions & SeparatorOptions.SeparateLines) != 0;

            string[] s = new string[syntaxSettings.TabStops.Length];
            for (int i = 0; i < syntaxSettings.TabStops.Length; i++)
                s[i] = syntaxSettings.TabStops[i].ToString();
            tbTabStops?.SetText(string.Join(",", s));
            UpdateFontControls();
        }
    }
}