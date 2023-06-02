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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Serialization;
using Alternet.Editor.TextSource;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;

using WeCantSpell.Hunspell;

namespace Alternet.CodeEditor.Demo
{
    public partial class MainForm : Form
    {
        private const string SFileFilter = "Text files (*.txt)|*.txt|C # files (*.cs)|*.cs|All files (*.*)|*.*";
        private const string StepOverDesc = "Move line style to the next line";
        private const string SetBreakpointDesc = "Set breakpoint bookmark";
        private const string ShowGutterDesc = "Display gutter area";
        private const string ShowBookmarksDesc = "Show or hide bookmarks";
        private const string LineBookmarksDesc = "Display triangle at bookmark position inside the text";
        private const string SetBookmarksDesc = "Set several indexed bookmarks";
        private const string ShowMarginDesc = "Draw vertical line at Margin column";
        private const string AllowDragMarginDesc = "Perform drag operation for Edit Margin";
        private const string ShowMarginHintsDesc = "Allow display some hint when mouse pointer is over the margin area.";
        private const string ColumnsVisibleDesc = "Draw vertical lines at the given text columns.";
        private const string WordWrapDesc = "Wrap words to the beginning of the next line when necessary";
        private const string WrapAtMarginDesc = "Wrap words at the margin position.";
        private const string ShowScrollHintDesc = "Display hint text in the popup window when user moving the thumb";
        private const string SmoothScrollDesc = "Scroll edit control content immediately when user moving the thumb";
        private const string AllowSplitDesc = "Allow splitting Edit control horizontally and vertically.";
        private const string ScrollButtonDesc = "Display a collection of horizontal and vertical buttons at the Editor scroll bar";
        private const string PageLayoutDesc = "Select the way of viewing Edit control's content";
        private const string PageSizeDesc = "Select paper size";
        private const string HorzRulerDesc = "Display a horizontal ruler";
        private const string VertRulerDesc = "Display a vertical ruler";
        private const string RulerUnitsDesc = "Measurement units of the pages rulers";
        private const string RulerAllowDragDesc = "Allow dragging ruler indentations";
        private const string RulerDiscreteDesc = "Change ruler indentation in discrete steps";
        private const string RulerDisplayDragLinesDesc = "Displays dotted line when ruler indentation being dragged";
        private const string CheckSpellingDesc = "Perform spelling check of the text content in the editor";
        private const string HighlightURLDesc = "Highlight hyperlinks in the text";
        private const string ShowHyperTextHintsDesc = "Display hint for hypertext section when user moves mouse over the hypertext";
        private const string AllowOutliningDesc = "Enable outlining";
        private const string DrawOnGutterDesc = "Draw outline images and lines on gutter";
        private const string DrawLinesDesc = "Draw lines for expanded outline section";
        private const string DrawButtonsDesc = "Draw the outline buttons substituting content of the collapsed section";
        private const string ShowHintsDesc = "Display text of the collapsed outline section in the popup window when mouse pointer is over the outline button";
        private const string RTFDesc = "Display dialog allow to save Editor content in the RTF format";
        private const string HTMLDesc = "Display dialog allow to save Editor content in the HTML format";
        private const string PrintPreviewDesc = "Display Print Preview Dialog";
        private const string PrintDesc = "Display Print Dialog";
        private const string PageSetupDesc = "Display Page Setup Dialog";
        private const string PrintOptionsDesc = "Display Print Options Dialog";
        private const string DisableSelectionDesc = "Disable selecting any text";
        private const string DisableDraggingDesc = "Disable dragging the selected text";
        private const string SelectBeyondEolDesc = "Draw selection beyond end of line.";
        private const string UseColorsDesc = "Preserve colors of the text when drawing selection";
        private const string HideSelectionDesc = "Hide selection when control losts focus";
        private const string SelectLineOnDblClickDesc = "Select whole line instead of single word when user double clicks on the text";
        private const string HighlightSelectedWordsDesc = "Specifies that the Edit control should select all instances of the chosen words.";
        private const string PersistentBlocksDesc = "Retain selection when the cursor is moved, until a new block is selecte.";
        private const string OverwriteBlocksDesc = "Replace selected text with whatever is typed next";
        private const string BeyondEolDesc = "Allows user navigate beyond end of line";
        private const string BeyondEofDesc = "Allows user navigate beyond end of file";
        private const string UpAtLineBeginDesc = "Curet position should move to the previous line when user click Left key and caret locates at the line begin";
        private const string DownAtLineEndDesc = "Curet position should move to the next line when user click Right key at the end of the line";
        private const string MoveOnRightButtonDesc = "Curent position should move to the mouse pointer when user clicks right mouse button";
        private const string LineNumbersDesc = "Draw line numbers";
        private const string LinesOnGutterDesc = "Draw numbers of lines on gutter area";
        private const string LinesBeyondEofDesc = "Numbers of lines should be drawn beyond end of file";
        private const string LineModificatorDesc = "Draw line modificator indicators";
        private const string HighlightCurrentLineDesc = "Current line in Edit control should be highlighted";
        private const string LineNumbersAlignDesc = "Choose line numbers alignment";
        private const string SeparateLinesDesc = "Draw horizontal lines to visualy separate lines in Edit control";
        private const string WhiteSpaceVisibleDesc = "Display white-space symbols such as spaces, tabs, end-of line or end-of-file markers";
        private const string TransparentDesc = "Draw edit control background";
        private const string HighlightReferencesDesc = "Highlight found references";
        private const string QuickInfoTipsDesc = "Display quick info tooltip when mouse is moved over control";
        private const string DrawColumnsIndentDesc = "Display columns indentation marks";
        private const string HighlightBracesDesc = "Highlight matching braces in the text";
        private const string UseRoundRectDesc = "Draw rectanguar frame around matching braces";
        private const string TempHighlightBracesDesc = "Remove highlighting of the matched brace after small delay";
        private const string LoadDesc = "Load Editor content from file";
        private const string SaveDesc = "Save Editor content to file";
        private const string FindDesc = "Display Search Dialog";
        private const string ReplaceDesc = "Display Replace Dialog";
        private const string GotoDesc = "Display Goto Line Dialog";
        private const string ResString = "Alternet.CodeEditor.Demo.Resources.{0}";

        private const string SXmlFileFilter = "Rtf files (*.rtf)|*.rtf|Html files (*.html; *.htm)|*.html;*.htm|Xml files (*.xml)|*.xml|All files (*.*)|*.*";
        private int scrollBoxUpdate;
        private string dir = Application.StartupPath + @"\";
        private Alternet.Syntax.Parsers.Roslyn.CsParser csParser1 = new Alternet.Syntax.Parsers.Roslyn.CsParser();
        private Parser parser1 = new Parser();
        private PropertyGrid propertyGrid = new PropertyGrid();
        private ArrayList obsolete = new ArrayList();
        private ComponentTypeDescriptorEx descriptor;
        private DefaultComponentContainer container = new DefaultComponentContainer();
        private SpellChecker spellChecker = new SpellChecker();
        private IDictionary<TreeNode, DemoItem> dedicatedParsers = new Dictionary<TreeNode, DemoItem>();
        private IDictionary<TreeNode, SchemeItem> otherParsers = new Dictionary<TreeNode, SchemeItem>();
        private ScrollBarAnnotationTypeAppearance customErrorAppearance;
        private ScrollBarAnnotationTypeAppearance defaultErrorAppearance;
        private int startLine = 44;
        private int endLine = 0;
        private int index;

        private string[] obsoleteProps =
        {
            "AccessibleDescription",
            "AccessibleName",
            "AccessibleRole",
        };

        public MainForm()
        {
            InitializeComponent();

            InitializeCustomAnnotationsDemo();
            InitializeErrorAppearanceDemo();
            InitializeScrollBarVisualStyleComboBox();
            InitializeVisualThemeComboBox();
            EnableOrDisableAllAnnotations();
        }

        private void Annotations_CustomAnnotationsRequested(object sender, ScrollBarCustomAnnotationsEventArgs e)
        {
            if (customAnnotationsCheckBox.Checked)
                e.Annotations = GetCustomAnnotations();
        }

        private IEnumerable<ScrollBarAnnotationPaintData> GetCustomAnnotations()
        {
            for (int line = 1; line <= textSource1.Lines.Count; line++)
            {
                if (line % 10 == 0)
                {
                    yield return new ScrollBarAnnotationPaintData(
                        ScrollBarAnnotationType.Custom,
                        line,
                        ScrollBarAnnotationHorizontalAlignment.Center,
                        ScrollBarAnnotationVerticalAlignment.Center,
                        Color.Magenta,
                        5);
                }
            }
        }

        private void InitializeFonts()
        {
            syntaxEdit.SearchDialog.Font = Font;
            syntaxEdit.GotoLineDialog.Font = Font;
        }

        private void InitializeCustomAnnotationsDemo()
        {
            syntaxEdit.Scrolling.Annotations.CustomAnnotationsRequested += Annotations_CustomAnnotationsRequested;
        }

        private void InitializeErrorAppearanceDemo()
        {
            defaultErrorAppearance = syntaxEdit.Scrolling.Annotations.GetAnnotationTypeAppearance(ScrollBarAnnotationType.SyntaxError);
            customErrorAppearance = new ScrollBarAnnotationTypeAppearance(
                ScrollBarAnnotationHorizontalAlignment.Center,
                ScrollBarAnnotationVerticalAlignment.Center,
                Color.Cyan,
                10);
        }

        private void InitializeScrollBarVisualStyleComboBox()
        {
            scrollBarsVisualStyleComboBox.DataSource = Enum.GetValues(typeof(ScrollBarVisualStyle));
            scrollBarsVisualStyleComboBox.SelectedItem = ScrollBarVisualStyle.VisualStudio;
        }

        private void InitializeVisualThemeComboBox()
        {
            visualThemeComboBox.DataSource = Enum.GetValues(typeof(VisualThemeType));
            visualThemeComboBox.SelectedItem = VisualThemeType.None;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                textSource1.LoadFile(fileInfo.FullName);

            foreach (string prop in obsoleteProps)
                obsolete.Add(prop);
            descriptor = new ComponentTypeDescriptorEx(syntaxEdit, obsolete);
            container.Add(descriptor, string.Format("{0}{1}", "SyntaxEdit", "_Wrapper"));
            container.Add(textSource1, "TextSource1");
            propertyGrid.Parent = pnPropertyGrid;
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.SelectedObject = descriptor;
            propertyGrid.CommandsVisibleIfAvailable = true;
            propertyGrid.Text = "Property Grid";
            propertyGrid.ToolbarVisible = true;
            treeView1.ExpandAll();
            ProcessDedicatedLanguages();
            ProcessOtherLanguages();

            // changing default settings
            syntaxEdit.Scrolling.Options |= ScrollingOptions.ShowScrollHint;
            syntaxSplitEdit.Scrolling.Options |= ScrollingOptions.ShowScrollHint;

            if (syntaxEdit.Find("Main", SearchOptions.EntireScope))
            {
                syntaxEdit.Selection.Clear();
                startLine = syntaxEdit.Position.Y + 1;
            }

            IEditLineStyle lineStyle = new EditLineStyle();
            lineStyle.BackColor = Color.Black;
            lineStyle.ForeColor = Color.FromArgb(255, 241, 129);
            lineStyle.Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors;

            endLine = syntaxEdit.Lines.Count - 2;

            // updating controls
            FillControls();

            pictureBox1.Image = DisplayScaling.CloneAndAutoScaleImage(pictureBox1.Image);
            if (pictureBox1.Image is Bitmap)
                ((Bitmap)pictureBox1.Image).MakeTransparent(Color.White);

            // displaying gutter panel
            UpdatePanels(treeView1.Nodes[0]);
            textSource1.Lexer = csParser1;
            InitializeFonts();
            syntaxEdit.VisualTheme = new CustomVisualTheme();
        }

        private void ProcessDedicatedLanguages()
        {
            TreeNode node = treeView1.Nodes.Insert(3, "Syntax Parsing");
            dedicatedParsers.Add(node.Nodes.Add("C#"), new DemoItem(csParser1, "c#.cs"));
            dedicatedParsers.Add(node.Nodes.Add("Visual Basic .NET"), new DemoItem(new Alternet.Syntax.Parsers.Roslyn.VbParser(), "vb_net.txt"));
            dedicatedParsers.Add(node.Nodes.Add("VBScript"), new DemoItem(new VbScriptParser(), "vbs_script.txt"));
            dedicatedParsers.Add(node.Nodes.Add("Java"), new DemoItem(new JsParser(), "java.txt"));
            dedicatedParsers.Add(node.Nodes.Add("C"), new DemoItem(new CParser(), "c.txt"));
            dedicatedParsers.Add(node.Nodes.Add("XML"), new DemoItem(new XmlParser(), "xml.txt"));
            dedicatedParsers.Add(node.Nodes.Add("HTML"), new DemoItem(new HtmlScriptParser(), "html.txt"));
            dedicatedParsers.Add(node.Nodes.Add("JavaScript"), new DemoItem(new JavaScriptParser(), "java_script.txt"));
            node.Collapse();
        }

        private void ProcessOtherLanguages()
        {
            TreeNode node = treeView1.Nodes.Insert(4, "Syntax Highlighting");
            otherParsers.Add(node.Nodes.Add("x86 Assembler"), new SchemeItem("assembler.xml", "assembler.txt"));
            otherParsers.Add(node.Nodes.Add("Python"), new SchemeItem("python.xml", "python.txt"));
            otherParsers.Add(node.Nodes.Add("Ruby"), new SchemeItem("Ruby.xml", "Ruby.txt"));
            otherParsers.Add(node.Nodes.Add("TCL/TK"), new SchemeItem("tcltk.xml", "tcltk.txt"));
            otherParsers.Add(node.Nodes.Add("Unix Shell"), new SchemeItem("unix_shell.xml", "unix_shell.txt"));
            otherParsers.Add(node.Nodes.Add("HTML With Scripts"), new SchemeItem("htmlscripts.xml", "htmlscripts.txt"));
            otherParsers.Add(node.Nodes.Add("CSS"), new SchemeItem("css.xml", "Css.txt"));
            otherParsers.Add(node.Nodes.Add("PHP"), new SchemeItem("php.xml", "php.txt"));
            otherParsers.Add(node.Nodes.Add("Windows Batch Files"), new SchemeItem("batch.xml", "batch.txt"));
            otherParsers.Add(node.Nodes.Add("Windows INI Files"), new SchemeItem("ini.xml", "ini.txt"));
            otherParsers.Add(node.Nodes.Add("MS SQL"), new SchemeItem("ms_sql.xml", "Ms_Sql.txt"));
            otherParsers.Add(node.Nodes.Add("Oracle SQL"), new SchemeItem("sql_oracle.xml", "sql_oracle.txt"));
            otherParsers.Add(node.Nodes.Add("Delphi"), new SchemeItem("delphi.xml", "delphi.txt"));
            otherParsers.Add(node.Nodes.Add("Lua"), new SchemeItem("lua.xml", "Lua.txt"));
            otherParsers.Add(node.Nodes.Add("Perl"), new SchemeItem("perl.xml", "perl.txt"));
            node.Collapse();
        }

        private void LoadEditContent(string sample, ISyntaxParser parser)
        {
            string fullSample = Path.GetFullPath(Path.Combine(dir, @"Resources\Editor\text", sample));
            textSource1.Lexer = parser;
            FileInfo fileInfo = new FileInfo(fullSample);
            if (fileInfo.Exists)
                textSource1.LoadFile(fileInfo.FullName);
        }

        private void UpdateEditContent(TreeNode node)
        {
            if (dedicatedParsers.ContainsKey(node))
            {
                DemoItem item;
                if (dedicatedParsers.TryGetValue(node, out item))
                {
                    LoadEditContent(item.Sample, item.Parser);
                }
            }
            else
            {
                SchemeItem item;
                if (otherParsers.ContainsKey(node) && otherParsers.TryGetValue(node, out item))
                {
                    syntaxEdit.Lexer = parser1;
                    syntaxSplitEdit.Lexer = parser1;
                    string fullSample = Path.GetFullPath(Path.Combine(dir, @"Resources\Editor\schemes", item.Scheme));
                    FileInfo fileInfo = new FileInfo(fullSample);
                    if (fileInfo.Exists)
                        parser1.Scheme.LoadFile(fileInfo.FullName);
                    fullSample = Path.GetFullPath(Path.Combine(dir, @"Resources\Editor\text", item.Sample));
                    fileInfo = new FileInfo(fullSample);
                    if (fileInfo.Exists)
                        textSource1.LoadFile(fileInfo.FullName);
                }
            }
        }

        private Panel GetCurrentPanel(TreeNode node)
        {
            // getting current panel to display
            if (node != null)
            {
                TreeNode root = node;
                while (root.Parent != null)
                    root = root.Parent;
                if (root.Index == 0)
                {
                    switch (node.Index)
                    {
                        case 0:
                            return pnGutter;
                        case 1:
                            return pnMargin;
                        case 2:
                            return pnLineNumbers;
                        case 3:
                            return pnLineStyles;
                        case 4:
                            return pnVisualThemes;
                        case 5:
                            return pnOther;
                        case 6:
                            return pnPageLayout;
                        default:
                            return pnGutter;
                    }
                }
                else
                    if (root.Index == 1)
                {
                    switch (node.Index)
                    {
                        case 0:
                            return pnOutlining;
                        case 1:
                            return pnTextSource;
                        case 2:
                            return pnNavigate;
                        case 3:
                            return pnSelection;
                        case 4:
                            return pnWordWrap;
                        case 5:
                            return pnSpellAndUrl;
                        case 6:
                            return pnScrollbarAnnotations;
                        default:
                            return pnOutlining;
                    }
                }
                else
                        if (root.Index == 2)
                {
                    switch (node.Index)
                            {
                                case 0:
                                    return pnDialogs;
                                case 1:
                                    return pnPrinting;
                                default:
                                    return pnDialogs;
                            }
                }
                else
                            if (root.Index == 5)
                                return pnProperties;
                            else
                                if (root.Index == 6)
                                    return pnCompanyInfo;
                                else
                                    return null;
            }

            return pnGutter;
        }

        private void UpdatePanels(TreeNode node)
        {
            // displaying new panel
            Panel panel = GetCurrentPanel(node);
            if ((panel != null) && !pnManage.Controls.Contains(panel))
                pnManage.Controls.Add(panel);
            int j = pnManage.Controls.IndexOf(panel);
            for (int i = 0; i < pnManage.Controls.Count; i++)
            {
                if ((i != j) && (pnManage.Controls[i] is Panel))
                    pnManage.Controls[i].Visible = false;
            }

            bool isAbout = (panel != null) && panel.Equals(pnCompanyInfo);
            bool isTextSource = (panel != null) && panel.Equals(pnTextSource);
            bool isProperties = (panel != null) && panel.Equals(pnProperties);
            if (isAbout)
            {
                pnManage.Visible = true;
                pnManage.Dock = DockStyle.Fill;
                panel.Dock = DockStyle.Fill;
                pnEditContainer.Visible = false;
            }
            else
                if (panel == null)
                {
                    pnManage.Visible = false;
                    pnEditContainer.Visible = true;
                    UpdateEditContent(node);
                }
                else
                {
                    pnManage.Visible = true;
                    pnManage.Dock = DockStyle.Top;
                    panel.Dock = DockStyle.Top;
                    pnManage.Height = panel.Height;
                    pnEditContainer.Visible = true;
                }

            pnPropertyGrid.Visible = isProperties;
            splitter2.Visible = isProperties;
            if (panel != null)
            {
                panel.Visible = true;
                panel.BringToFront();
                UpdateEditorVisibility(!isAbout, isTextSource, pnMain.Height - panel.Height - splitter1.Height);
            }
        }

        private void UpdateEditorVisibility(bool visible, bool split, int height)
        {
            // splitting view if needed
            syntaxEdit.Visible = visible;
            syntaxSplitEdit.Visible = split;
            splitter1.Visible = split;
            if (split)
            {
                syntaxSplitEdit.Height = height / 2;
            }
        }

        private void FillControls()
        {
            // updating controls according to editor properties
            scrollBoxUpdate++;
            try
            {
                // margin
                nudMarginPos.Maximum = 1000;
                nudMarginPos.Value = syntaxEdit.EditMargin.Position;
                chbShowMargin.Checked = syntaxEdit.EditMargin.Visible;
                chbAllowDragMargin.Checked = syntaxEdit.EditMargin.AllowDrag;
                chbShowMarginHints.Checked = syntaxEdit.EditMargin.ShowHints;
                chbColumnsVisible.Checked = syntaxEdit.EditMargin.ColumnsVisible;

                // gutter
                syntaxEdit.LineStyles.AddLineStyle("bookmark", Color.White, Color.Red, Color.Empty, 12, LineStyleOptions.BeyondEol);
                syntaxSplitEdit.LineStyles.AddLineStyle("bookmark", Color.White, Color.Red, Color.Empty, 12, LineStyleOptions.BeyondEol);
                syntaxEdit.LineStyles.Add(new EditLineStyle() // breakpoint style
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(171, 97, 107),
                    Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                    ImageIndex = 11,
                });
                syntaxSplitEdit.LineStyles.Add(new EditLineStyle() // breakpoint style
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(171, 97, 107),
                    Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                    ImageIndex = 11,
                });
                chbShowGutter.Checked = syntaxEdit.Gutter.Visible;
                chbPaintBookMarks.Checked = (GutterOptions.PaintBookMarks & syntaxEdit.Gutter.Options) != 0;
                chbLineNumbers.Checked = (GutterOptions.PaintLineNumbers & syntaxEdit.Gutter.Options) != 0;
                chbLinesOnGutter.Checked = (GutterOptions.PaintLinesOnGutter & syntaxEdit.Gutter.Options) != 0;
                chbLinesBeyondEof.Checked = (GutterOptions.PaintLinesBeyondEof & syntaxEdit.Gutter.Options) != 0;
                chbDrawLineBookmarks.Checked = syntaxEdit.Gutter.DrawLineBookmarks;
                chbLineModificator.Checked = (GutterOptions.PaintLineModificators & syntaxEdit.Gutter.Options) != 0;
                nudGutterWidth.Maximum = syntaxEdit.Width;
                nudGutterWidth.Value = syntaxEdit.Gutter.Width;
                nudLineNumbersStart.Maximum = 10000;
                nudLineNumbersStart.Value = syntaxEdit.Gutter.LineNumbersStart;
                string[] s = Enum.GetNames(typeof(StringAlignment));
                cbLineNumbersAlign.Items.AddRange(s);
                cbLineNumbersAlign.SelectedIndex = (int)syntaxEdit.Gutter.LineNumbersAlignment;
                chbHighlightCurrentLine.Checked = (SeparatorOptions.HighlightCurrentLine & syntaxEdit.LineSeparator.Options) != 0;

                // other
                chbSeparateLines.Checked = (SeparatorOptions.SeparateLines & syntaxEdit.LineSeparator.Options) != 0;
                chbQuickInfoTips.Checked = (SyntaxOptions.QuickInfoTips & csParser1.Options) != 0;
                chbDrawColumnsIndent.Checked = syntaxEdit.SyntaxPaint.DrawColumnsIndent;
                chbWhiteSpaceVisible.Checked = syntaxEdit.WhiteSpace.Visible;
                chbHighlightBraces.Checked = (BracesOptions.Highlight & syntaxEdit.Braces.BracesOptions) != 0;
                chbUseRoundRect.Checked = syntaxEdit.Braces.UseRoundRect;
                cbTempHighlightBraces.Checked = (BracesOptions.TempHighlight & syntaxEdit.Braces.BracesOptions) != 0;
                chbTransparent.Checked = syntaxEdit.Transparent;
                chbHighlightReferences.Checked = syntaxEdit.HighlightReferences;

                // PageLayout&Ruler
                s = Enum.GetNames(typeof(PageType));
                cbPageLayout.Items.AddRange(s);
                cbPageLayout.SelectedIndex = (int)syntaxEdit.Pages.PageType;
                s = Enum.GetNames(typeof(RulerUnits));
                cbRulerUnits.Items.AddRange(s);
                cbRulerUnits.SelectedIndex = (int)syntaxEdit.Pages.RulerUnits;
                chbRulerAllowDrag.Checked = (RulerOptions.AllowDrag & syntaxEdit.Pages.RulerOptions) != 0;
                chbRulerDiscrete.Checked = (RulerOptions.Discrete & syntaxEdit.Pages.RulerOptions) != 0;
                chbRulerDisplayDragLines.Checked = (RulerOptions.DisplayDragLine & syntaxEdit.Pages.RulerOptions) != 0;
                chbHorzRuler.Checked = (EditRulers.Horizonal & syntaxEdit.Pages.Rulers) != 0;
                chbVertRuler.Checked = (EditRulers.Vertical & syntaxEdit.Pages.Rulers) != 0;

                foreach (PaperSize psize in syntaxEdit.Printing.PrinterSettings.PaperSizes)
                {
                    if (!cbPageSize.Items.Contains(psize.Kind))
                        cbPageSize.Items.Add(psize.Kind);
                }

                cbPageSize.Enabled = cbPageSize.Items.Count > 0;
                if (cbPageSize.Enabled)
                    cbPageSize.SelectedIndex = 0;

                // wordwrap
                chbWordWrap.Checked = syntaxEdit.WordWrap;
                chbWrapAtMargin.Checked = syntaxEdit.WrapAtMargin;
                chbAllowSplit.Checked = ((ScrollingOptions.AllowSplitHorz & syntaxEdit.Scrolling.Options) != 0) &
                    ((ScrollingOptions.AllowSplitVert & syntaxEdit.Scrolling.Options) != 0);
                chbScrollButtons.Checked = ((ScrollingOptions.HorzButtons & syntaxEdit.Scrolling.Options) != 0) &
                    ((ScrollingOptions.VertButtons & syntaxEdit.Scrolling.Options) != 0);
                chbSystemScrollBars.Checked = (syntaxEdit.Scrolling.Options & ScrollingOptions.SystemScrollbars) != 0;
                chbFlatScrollBars.Checked = (syntaxEdit.Scrolling.Options & ScrollingOptions.FlatScrollbars) != 0;

                // outlining
                chbAllowOutlining.Checked = syntaxEdit.Outlining.AllowOutlining;
                chbDrawOnGutter.Checked = (OutlineOptions.DrawOnGutter & syntaxEdit.Outlining.OutlineOptions) != 0;
                chbDrawLines.Checked = (OutlineOptions.DrawLines & syntaxEdit.Outlining.OutlineOptions) != 0;
                chbDrawButtons.Checked = (OutlineOptions.DrawButtons & syntaxEdit.Outlining.OutlineOptions) != 0;
                chbShowHints.Checked = (OutlineOptions.ShowHints & syntaxEdit.Outlining.OutlineOptions) != 0;

                // spell&url
                chbCheckSpelling.Checked = syntaxEdit.Spelling.CheckSpelling;
                spellChecker.CheckSpelling(syntaxEdit, chbCheckSpelling.Checked);
                chbHighlightUrls.Checked = syntaxEdit.HyperText.HighlightHyperText;
                chbShowScrollHint.Checked = (syntaxEdit.Scrolling.Options & ScrollingOptions.ShowScrollHint) != 0;
                chbSmoothScroll.Checked = (syntaxEdit.Scrolling.Options & ScrollingOptions.SmoothScroll) != 0;
                chbShowHyperTextHints.Checked = syntaxEdit.HyperText.ShowHints;

                // navigate&selection
                chbBeyondEol.Checked = (NavigateOptions.BeyondEol & syntaxEdit.NavigateOptions) != 0;
                chbBeyondEof.Checked = (NavigateOptions.BeyondEof & syntaxEdit.NavigateOptions) != 0;
                chbUpAtLineBegin.Checked = (NavigateOptions.UpAtLineBegin & syntaxEdit.NavigateOptions) != 0;
                chbDownAtLineEnd.Checked = (NavigateOptions.DownAtLineEnd & syntaxEdit.NavigateOptions) != 0;
                chbMoveOnRightButton.Checked = (NavigateOptions.MoveOnRightButton & syntaxEdit.NavigateOptions) != 0;

                chbDisableSelection.Checked = (SelectionOptions.DisableSelection & syntaxEdit.Selection.Options) != 0;
                chbDisableDragging.Checked = (SelectionOptions.DisableDragging & syntaxEdit.Selection.Options) != 0;
                chbSelectBeyondEol.Checked = (SelectionOptions.SelectBeyondEol & syntaxEdit.Selection.Options) != 0;
                chbUseColors.Checked = (SelectionOptions.UseColors & syntaxEdit.Selection.Options) != 0;
                chbHideSelection.Checked = (SelectionOptions.HideSelection & syntaxEdit.Selection.Options) != 0;
                chbSelectLineOnDblClick.Checked = (SelectionOptions.SelectLineOnDblClick & syntaxEdit.Selection.Options) != 0;
                HighlightSelectedWordsCheckBox.Checked = (SelectionOptions.HighlightSelectedWords & syntaxEdit.Selection.Options) != 0;
                chbPersistentBlocks.Checked = (SelectionOptions.PersistentBlocks & syntaxEdit.Selection.Options) != 0;
                chbOverwriteBlocks.Checked = (SelectionOptions.OverwriteBlocks & syntaxEdit.Selection.Options) != 0;
            }
            finally
            {
                scrollBoxUpdate--;
            }
        }

        private void UpdateScrollBoxes(object sender)
        {
            // updating checkboxes related to scrolling properties
            if (scrollBoxUpdate > 0)
                return;
            scrollBoxUpdate++;
            try
            {
                if (chbAllowSplit != sender)
                {
                    chbAllowSplit.Checked = ((ScrollingOptions.AllowSplitHorz & syntaxEdit.Scrolling.Options) != 0) &
                        ((ScrollingOptions.AllowSplitVert & syntaxEdit.Scrolling.Options) != 0);
                }

                if (chbScrollButtons != sender)
                {
                    chbScrollButtons.Checked = ((ScrollingOptions.HorzButtons & syntaxEdit.Scrolling.Options) != 0) &
                        ((ScrollingOptions.VertButtons & syntaxEdit.Scrolling.Options) != 0);
                }

                if (chbSystemScrollBars != sender)
                    chbSystemScrollBars.Checked = (syntaxEdit.Scrolling.Options & ScrollingOptions.SystemScrollbars) != 0;

                if (chbFlatScrollBars != sender)
                    chbFlatScrollBars.Checked = (syntaxEdit.Scrolling.Options & ScrollingOptions.FlatScrollbars) != 0;
            }
            finally
            {
                scrollBoxUpdate--;
            }
        }

        private void ShowGutterCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating gutter visiblility
            syntaxEdit.Gutter.Visible = chbShowGutter.Checked;
            syntaxSplitEdit.Gutter.Visible = chbShowGutter.Checked;
        }

        private void ShowMarginCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating margin visiblility
            syntaxEdit.EditMargin.Visible = chbShowMargin.Checked;
            syntaxSplitEdit.EditMargin.Visible = chbShowMargin.Checked;
        }

        private void SeparateLinesCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating lineseparator properties
            syntaxEdit.LineSeparator.Options = chbSeparateLines.Checked ? syntaxEdit.LineSeparator.Options
                | SeparatorOptions.SeparateLines : syntaxEdit.LineSeparator.Options & ~SeparatorOptions.SeparateLines;
            syntaxSplitEdit.LineSeparator.Options = syntaxEdit.LineSeparator.Options;
        }

        private void PainookMarksCheckBoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating bookmarks visibility
            syntaxEdit.Gutter.Options = chbPaintBookMarks.Checked ? syntaxEdit.Gutter.Options
                | GutterOptions.PaintBookMarks : syntaxEdit.Gutter.Options & ~GutterOptions.PaintBookMarks;
            syntaxSplitEdit.Gutter.Options = syntaxEdit.Gutter.Options;
        }

        private void DrawLineBookmarksCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating line bookmarks visibility
            syntaxEdit.Gutter.DrawLineBookmarks = chbDrawLineBookmarks.Checked;
            syntaxSplitEdit.Gutter.DrawLineBookmarks = chbDrawLineBookmarks.Checked;
        }

        private void HighlightCurrentLineCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating line separator visibility
            syntaxEdit.LineSeparator.Options = chbHighlightCurrentLine.Checked ? syntaxEdit.LineSeparator.Options
                | SeparatorOptions.HighlightCurrentLine : syntaxEdit.LineSeparator.Options & ~SeparatorOptions.HighlightCurrentLine;
            syntaxSplitEdit.LineSeparator.Options = syntaxEdit.LineSeparator.Options;
        }

        private void LineModificatorCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating line modificator visibility
            syntaxEdit.Gutter.Options = chbLineModificator.Checked ? syntaxEdit.Gutter.Options
                | GutterOptions.PaintLineModificators : syntaxEdit.Gutter.Options & ~GutterOptions.PaintLineModificators;
            syntaxSplitEdit.Gutter.Options = syntaxEdit.Gutter.Options;
        }

        private void GutterWidthNumeric_ValueChanged(object sender, System.EventArgs e)
        {
            // updating gutter width
            syntaxEdit.Gutter.Width = (int)nudGutterWidth.Value;
            syntaxSplitEdit.Gutter.Width = (int)nudGutterWidth.Value;
        }

        private void MarginPosNumeric_ValueChanged(object sender, System.EventArgs e)
        {
            // updating margin position
            syntaxEdit.EditMargin.Position = (int)nudMarginPos.Value;
            syntaxSplitEdit.EditMargin.Position = (int)nudMarginPos.Value;
        }

        private void ShowBookmarksButton_Click(object sender, System.EventArgs e)
        {
            // setting or removing sample bookmarks and line styles
            int i = syntaxEdit.ScreenToText(0, 0).Y;
            syntaxEdit.Source.BeginUpdate();
            try
            {
                if (btShowBookmarks.Text == "Set Bookmarks")
                {
                    syntaxEdit.Source.LineStyles.SetLineStyle(i + 12, 0);
                    syntaxEdit.Source.BookMarks.SetBookMark(new Point(6, i + 1), 0);
                    syntaxEdit.Source.BookMarks.SetBookMark(new Point(9, i + 15), 7);
                    syntaxEdit.Source.BookMarks.SetBookMark(new Point(14, i + 6), 10);
                    btShowBookmarks.Text = "Clear Bookmarks";
                }
                else
                {
                    syntaxEdit.Source.LineStyles.Clear();
                    syntaxEdit.Source.BookMarks.ClearAllBookMarks();
                    btShowBookmarks.Text = "Set Bookmarks";
                }
            }
            finally
            {
                syntaxEdit.Source.EndUpdate();
                syntaxEdit.Invalidate();
            }
        }

        private void LineNumbersCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating line numbers visibility
            syntaxEdit.Gutter.Options = chbLineNumbers.Checked ? syntaxEdit.Gutter.Options
                | GutterOptions.PaintLineNumbers : syntaxEdit.Gutter.Options & ~GutterOptions.PaintLineNumbers;
            syntaxSplitEdit.Gutter.Options = syntaxEdit.Gutter.Options;
        }

        private void LinesOnGutterCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating displaying line numbers on gutter
            syntaxEdit.Gutter.Options = chbLinesOnGutter.Checked ? syntaxEdit.Gutter.Options
                | GutterOptions.PaintLinesOnGutter : syntaxEdit.Gutter.Options & ~GutterOptions.PaintLinesOnGutter;
            syntaxSplitEdit.Gutter.Options = syntaxEdit.Gutter.Options;
        }

        private void LinesBeyondEofCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating displaying line numbers beyond end of file
            syntaxEdit.Gutter.Options = chbLinesBeyondEof.Checked ? syntaxEdit.Gutter.Options |
                GutterOptions.PaintLinesBeyondEof : syntaxEdit.Gutter.Options & ~GutterOptions.PaintLinesBeyondEof;
            syntaxSplitEdit.Gutter.Options = syntaxEdit.Gutter.Options;
        }

        private void LineNumbersStartNumeric_ValueChanged(object sender, System.EventArgs e)
        {
            // updating line number start
            syntaxEdit.Gutter.LineNumbersStart = (int)nudLineNumbersStart.Value;
            syntaxSplitEdit.Gutter.LineNumbersStart = (int)nudLineNumbersStart.Value;
        }

        private void LineNumbersAlignComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // updating line numbers alignment
            syntaxEdit.Gutter.LineNumbersAlignment = (StringAlignment)cbLineNumbersAlign.SelectedIndex;
            syntaxSplitEdit.Gutter.LineNumbersAlignment = (StringAlignment)cbLineNumbersAlign.SelectedIndex;
        }

        private void VisualThemeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit.VisualThemeType = (VisualThemeType)visualThemeComboBox.SelectedItem;
            syntaxSplitEdit.VisualThemeType = (VisualThemeType)visualThemeComboBox.SelectedItem;
        }

        // dialogs
        private void LoadButton_Click(object sender, System.EventArgs e)
        {
            // loading editor content from the file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textSource1.LoadFile(openFileDialog1.FileName);
            }
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            // saving editor content to the file
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                textSource1.SaveFile(saveFileDialog1.FileName);
        }

        private void FindButton_Click(object sender, System.EventArgs e)
        {
            // displaying search dialog
            syntaxEdit.DisplaySearchDialog();
        }

        private void ReplaceButton_Click(object sender, System.EventArgs e)
        {
            // displaying search dialog
            syntaxEdit.DisplayReplaceDialog();
        }

        private void GotoButton_Click(object sender, System.EventArgs e)
        {
            // displaying goto line dialog
            syntaxEdit.DisplayGotoLineDialog();
        }

        private void WordWrapCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating wordwrap mode
            syntaxEdit.WordWrap = chbWordWrap.Checked;
            syntaxSplitEdit.WordWrap = chbWordWrap.Checked;
        }

        private void WrapAtMarginCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating wrapping at margin
            syntaxEdit.WrapAtMargin = chbWrapAtMargin.Checked;
            syntaxSplitEdit.WrapAtMargin = chbWrapAtMargin.Checked;
        }

        private void WhiteSpaceVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating whitespace visibility
            syntaxEdit.WhiteSpace.Visible = chbWhiteSpaceVisible.Checked;
            syntaxSplitEdit.WhiteSpace.Visible = chbWhiteSpaceVisible.Checked;
        }

        // printing&exporting
        private void RtfButton_Click(object sender, System.EventArgs e)
        {
            // saving editor to rtf
            saveFileDialog2.FilterIndex = 1;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                syntaxEdit.SaveFile(saveFileDialog2.FileName, new RtfExport());
        }

        private void HtmuttonLisoxTextBox_Click(object sender, System.EventArgs e)
        {
            // saving editor to html
            saveFileDialog2.FilterIndex = 2;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                syntaxEdit.SaveFile(saveFileDialog2.FileName, new HtmlExport());
        }

        private void PrintPreviewButton_Click(object sender, System.EventArgs e)
        {
            // executing print preview dialog
            syntaxEdit.Printing.ExecutePrintPreviewDialog();
        }

        private void PrinuttonTextBox_Click(object sender, System.EventArgs e)
        {
            // executing print dialog
            if (syntaxEdit.Printing.ExecutePrintDialog() == DialogResult.OK)
                syntaxEdit.Printing.Print();
        }

        private void PageSetupButton_Click(object sender, System.EventArgs e)
        {
            // executing page setup dialog
            syntaxEdit.Printing.ExecutePageSetupDialog();
        }

        private void PrintOptionsButton_Click(object sender, System.EventArgs e)
        {
            // executing print options dialog
            syntaxEdit.Printing.ExecutePrintOptionsDialog();
        }

        // outlining
        private void AllowOutliningCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating outlining mode
            syntaxEdit.Outlining.AllowOutlining = chbAllowOutlining.Checked;
            syntaxSplitEdit.Outlining.AllowOutlining = syntaxEdit.Outlining.AllowOutlining;
        }

        private void DrawOnGutterCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating outlining options
            syntaxEdit.Outlining.OutlineOptions = chbDrawOnGutter.Checked ? syntaxEdit.Outlining.OutlineOptions
                | OutlineOptions.DrawOnGutter : syntaxEdit.Outlining.OutlineOptions & ~OutlineOptions.DrawOnGutter;
            syntaxSplitEdit.Outlining.OutlineOptions = syntaxEdit.Outlining.OutlineOptions;
        }

        private void DrawLinesCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating outlining options
            syntaxEdit.Outlining.OutlineOptions = chbDrawLines.Checked ? syntaxEdit.Outlining.OutlineOptions
                | OutlineOptions.DrawLines : syntaxEdit.Outlining.OutlineOptions & ~OutlineOptions.DrawLines;
            syntaxSplitEdit.Outlining.OutlineOptions = syntaxEdit.Outlining.OutlineOptions;
        }

        private void DrawButtonsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating outlining options
            syntaxEdit.Outlining.OutlineOptions = chbDrawButtons.Checked ? syntaxEdit.Outlining.OutlineOptions
                | OutlineOptions.DrawButtons : syntaxEdit.Outlining.OutlineOptions & ~OutlineOptions.DrawButtons;
            syntaxSplitEdit.Outlining.OutlineOptions = syntaxEdit.Outlining.OutlineOptions;
        }

        private void ShowHintsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating outlining options
            syntaxEdit.Outlining.OutlineOptions = chbShowHints.Checked ? syntaxEdit.Outlining.OutlineOptions
                | OutlineOptions.ShowHints : syntaxEdit.Outlining.OutlineOptions & ~OutlineOptions.ShowHints;
            syntaxSplitEdit.Outlining.OutlineOptions = syntaxEdit.Outlining.OutlineOptions;
        }

        // spell&url
        private void CheckSpellingCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating spelling options
            spellChecker.CheckSpelling(syntaxEdit, chbCheckSpelling.Checked);
            spellChecker.CheckSpelling(syntaxSplitEdit, chbCheckSpelling.Checked);
        }

        private void HighlightUrlsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating hypertext options
            syntaxEdit.HyperText.HighlightHyperText = chbHighlightUrls.Checked;
            syntaxSplitEdit.HyperText.HighlightHyperText = chbHighlightUrls.Checked;
        }

        private void ShowScrollHintCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating scrolling options
            syntaxEdit.Scrolling.Options = chbShowScrollHint.Checked ? syntaxEdit.Scrolling.Options
                | ScrollingOptions.ShowScrollHint : syntaxEdit.Scrolling.Options & ~ScrollingOptions.ShowScrollHint;
            syntaxSplitEdit.Scrolling.Options = syntaxEdit.Scrolling.Options;
        }

        private void AllowSplitCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating splitting options
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit.Scrolling.Options = chbAllowSplit.Checked ? syntaxEdit.Scrolling.Options
            | ScrollingOptions.AllowSplitHorz | ScrollingOptions.AllowSplitVert :
                syntaxEdit.Scrolling.Options & ~ScrollingOptions.AllowSplitHorz & ~ScrollingOptions.AllowSplitVert;
            syntaxSplitEdit.Scrolling.Options = syntaxEdit.Scrolling.Options;
            UpdateScrollBoxes(chbAllowSplit);
        }

        private void ScroluttonsCheckBoxLisoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating scrolling buttons
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit.Scrolling.Options = chbScrollButtons.Checked ? syntaxEdit.Scrolling.Options
                | ScrollingOptions.HorzButtons | ScrollingOptions.VertButtons :
                syntaxEdit.Scrolling.Options & ~ScrollingOptions.HorzButtons & ~ScrollingOptions.VertButtons;
            syntaxSplitEdit.Scrolling.Options = syntaxEdit.Scrolling.Options;
            UpdateScrollBoxes(chbScrollButtons);
        }

        private void SmoothScrollCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating smooth scroll
            syntaxEdit.Scrolling.Options = chbSmoothScroll.Checked ? syntaxEdit.Scrolling.Options
                | ScrollingOptions.SmoothScroll : syntaxEdit.Scrolling.Options & ~ScrollingOptions.SmoothScroll;
            syntaxSplitEdit.Scrolling.Options = syntaxEdit.Scrolling.Options;
        }

        // navigate&selection
        private void BeyondEolCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating navigate options
            syntaxEdit.NavigateOptions = chbBeyondEol.Checked ? syntaxEdit.NavigateOptions
                | NavigateOptions.BeyondEol : syntaxEdit.NavigateOptions & ~NavigateOptions.BeyondEol;
            syntaxSplitEdit.NavigateOptions = syntaxEdit.NavigateOptions;
        }

        private void BeyondEofCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating navigate options
            syntaxEdit.NavigateOptions = chbBeyondEof.Checked ? syntaxEdit.NavigateOptions
                | NavigateOptions.BeyondEof : syntaxEdit.NavigateOptions & ~NavigateOptions.BeyondEof;
            syntaxSplitEdit.NavigateOptions = syntaxEdit.NavigateOptions;
        }

        private void UpAtLineBeginCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating navigate options
            syntaxEdit.NavigateOptions = chbUpAtLineBegin.Checked ? syntaxEdit.NavigateOptions
                | NavigateOptions.UpAtLineBegin : syntaxEdit.NavigateOptions & ~NavigateOptions.UpAtLineBegin;
            syntaxSplitEdit.NavigateOptions = syntaxEdit.NavigateOptions;
        }

        private void DownAtLineEndCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating navigate options
            syntaxEdit.NavigateOptions = chbDownAtLineEnd.Checked ? syntaxEdit.NavigateOptions
                | NavigateOptions.DownAtLineEnd : syntaxEdit.NavigateOptions & ~NavigateOptions.DownAtLineEnd;
            syntaxSplitEdit.NavigateOptions = syntaxEdit.NavigateOptions;
        }

        private void MoveOnRighuttonCheckBoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating navigate options
            syntaxEdit.NavigateOptions = chbMoveOnRightButton.Checked ? syntaxEdit.NavigateOptions
                | NavigateOptions.MoveOnRightButton : syntaxEdit.NavigateOptions & ~NavigateOptions.MoveOnRightButton;
            syntaxSplitEdit.NavigateOptions = syntaxEdit.NavigateOptions;
        }

        private void DisableSelectionCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection options
            syntaxEdit.Selection.Options = chbDisableSelection.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.DisableSelection : syntaxEdit.Selection.Options & ~SelectionOptions.DisableSelection;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void DisableDraggingCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection options
            syntaxEdit.Selection.Options = chbDisableDragging.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.DisableDragging : syntaxEdit.Selection.Options & ~SelectionOptions.DisableDragging;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void SeleceyondEolCheckBoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection options
            syntaxEdit.Selection.Options = chbSelectBeyondEol.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.SelectBeyondEol : syntaxEdit.Selection.Options & ~SelectionOptions.SelectBeyondEol;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void UseColorsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection appearance
            syntaxEdit.Selection.Options = chbUseColors.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.UseColors : syntaxEdit.Selection.Options & ~SelectionOptions.UseColors;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void HideSelectionCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection appearance
            syntaxEdit.Selection.Options = chbHideSelection.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.HideSelection : syntaxEdit.Selection.Options & ~SelectionOptions.HideSelection;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void SelectLineOnDblClickCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection options
            syntaxEdit.Selection.Options = chbSelectLineOnDblClick.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.SelectLineOnDblClick : syntaxEdit.Selection.Options & ~SelectionOptions.SelectLineOnDblClick;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void HighlightSelectedWordsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection options
            syntaxEdit.Selection.Options = HighlightSelectedWordsCheckBox.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.HighlightSelectedWords : syntaxEdit.Selection.Options & ~SelectionOptions.HighlightSelectedWords;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void PersistenlocksCheckBoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection options
            syntaxEdit.Selection.Options = chbPersistentBlocks.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.PersistentBlocks : syntaxEdit.Selection.Options & ~SelectionOptions.PersistentBlocks;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void OverwriteBlocksCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating selection options
            syntaxEdit.Selection.Options = chbOverwriteBlocks.Checked ? syntaxEdit.Selection.Options
                | SelectionOptions.OverwriteBlocks : syntaxEdit.Selection.Options & ~SelectionOptions.OverwriteBlocks;
            syntaxSplitEdit.Selection.Options = syntaxEdit.Selection.Options;
        }

        private void AdressLebel_Click(object sender, System.EventArgs e)
        {
            laAdress.ForeColor = Color.Purple;
            try
            {
                System.Diagnostics.Process.Start(laAdress.Text);
            }
            catch
            {
            }
        }

        private void MailToLabel_Click(object sender, System.EventArgs e)
        {
            laMailTo.ForeColor = Color.Purple;
            try
            {
                System.Diagnostics.Process.Start("mailto:contact@alternetsoft.com");
            }
            catch
            {
            }
        }

        private void ShowHyperTextHintsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating hypertext options
            syntaxEdit.HyperText.ShowHints = chbShowHyperTextHints.Checked;
            syntaxSplitEdit.HyperText.ShowHints = chbShowHyperTextHints.Checked;
        }

        private void PageLayoutComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // updating page layout mode
            syntaxEdit.Pages.PageType = (PageType)cbPageLayout.SelectedIndex;
            syntaxSplitEdit.Pages.PageType = (PageType)cbPageLayout.SelectedIndex;
            if (cbPageSize.Items.Count > 0)
                cbPageSize.SelectedIndex = Math.Max(cbPageSize.Items.IndexOf(syntaxEdit.Pages.DefaultPage.PageKind), 0);
        }

        private void UpdatePageKind(ISyntaxEdit edit, PaperKind kind)
        {
            for (int i = 0; i < edit.Pages.Count; i++)
                edit.Pages[i].PageKind = kind;
        }

        private void PageSizeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // updating page size
            object obj = Enum.Parse(typeof(PaperKind), cbPageSize.Text);
            if (obj is PaperKind)
            {
                UpdatePageKind(syntaxEdit, (PaperKind)obj);
                UpdatePageKind(syntaxSplitEdit, (PaperKind)obj);
            }
        }

        private void RulerAllowDragCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating ruler options
            syntaxEdit.Pages.RulerOptions = chbRulerAllowDrag.Checked ? syntaxEdit.Pages.RulerOptions
                | RulerOptions.AllowDrag : syntaxEdit.Pages.RulerOptions & ~RulerOptions.AllowDrag;
            syntaxSplitEdit.Pages.RulerOptions = syntaxEdit.Pages.RulerOptions;
        }

        private void RulerDiscreteCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating ruler options
            syntaxEdit.Pages.RulerOptions = chbRulerDiscrete.Checked ? syntaxEdit.Pages.RulerOptions
                | RulerOptions.Discrete : syntaxEdit.Pages.RulerOptions & ~RulerOptions.Discrete;
            syntaxSplitEdit.Pages.RulerOptions = syntaxEdit.Pages.RulerOptions;
        }

        private void RulerDisplayDragLinesCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating ruler options
            syntaxEdit.Pages.RulerOptions = chbRulerDisplayDragLines.Checked ? syntaxEdit.Pages.RulerOptions
                | RulerOptions.DisplayDragLine : syntaxEdit.Pages.RulerOptions & ~RulerOptions.DisplayDragLine;
            syntaxSplitEdit.Pages.RulerOptions = syntaxEdit.Pages.RulerOptions;
        }

        private void HighlighracesCheckBoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating matching braces options
            if (chbHighlightBraces.Checked)
                syntaxEdit.Braces.BracesOptions = BracesOptions.Highlight;
            else
                syntaxEdit.Braces.BracesOptions = BracesOptions.None;
            syntaxSplitEdit.Braces.BracesOptions = syntaxEdit.Braces.BracesOptions;
        }

        private void UseRoundRectCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating matching braces appearance
            syntaxEdit.Braces.UseRoundRect = chbUseRoundRect.Checked;
            syntaxSplitEdit.Braces.UseRoundRect = chbUseRoundRect.Checked;
            syntaxEdit.Braces.ForeColor = chbUseRoundRect.Checked ? Color.Gray : Color.Black;
            syntaxSplitEdit.Braces.ForeColor = syntaxEdit.Braces.ForeColor;
        }

        private void AllowDragMarginCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating margin options
            syntaxEdit.EditMargin.AllowDrag = chbAllowDragMargin.Checked;
            syntaxSplitEdit.EditMargin.AllowDrag = chbAllowDragMargin.Checked;
        }

        private void ShowMarginHintsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating margin options
            syntaxEdit.EditMargin.ShowHints = chbShowMarginHints.Checked;
            syntaxSplitEdit.EditMargin.ShowHints = chbShowMarginHints.Checked;
        }

        private void TransparentCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating transparent property
            syntaxEdit.Transparent = chbTransparent.Checked;
            syntaxSplitEdit.Transparent = chbTransparent.Checked;
        }

        private void HighlightReferencesCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            syntaxEdit.HighlightReferences = chbHighlightReferences.Checked;
            syntaxSplitEdit.HighlightReferences = chbHighlightReferences.Checked;
        }

        private void RulerUnitsComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // updating ruler units
            syntaxEdit.Pages.RulerUnits = (RulerUnits)cbRulerUnits.SelectedIndex;
            syntaxSplitEdit.Pages.RulerUnits = (RulerUnits)cbRulerUnits.SelectedIndex;
        }

        private void TempHighlighracesComboBoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating braces options
            syntaxEdit.Braces.BracesOptions = cbTempHighlightBraces.Checked ? syntaxEdit.Braces.BracesOptions
                | BracesOptions.TempHighlight : syntaxEdit.Braces.BracesOptions & ~BracesOptions.TempHighlight;
            syntaxSplitEdit.Braces.BracesOptions = syntaxEdit.Braces.BracesOptions;
        }

        private void HorzRulerCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating ruler options
            syntaxEdit.Pages.Rulers = chbHorzRuler.Checked ? syntaxEdit.Pages.Rulers
                | EditRulers.Horizonal : syntaxEdit.Pages.Rulers & ~EditRulers.Horizonal;
            syntaxSplitEdit.Pages.Rulers = syntaxEdit.Pages.Rulers;
        }

        private void VertRulerCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating ruler options
            syntaxEdit.Pages.Rulers = chbVertRuler.Checked ? syntaxEdit.Pages.Rulers
                | EditRulers.Vertical : syntaxEdit.Pages.Rulers & ~EditRulers.Vertical;
            syntaxSplitEdit.Pages.Rulers = syntaxEdit.Pages.Rulers;
        }

        private void SyntaxEdit_WordSpell(object sender, Alternet.Editor.TextSource.WordSpellEventArgs e)
        {
            // sample event for checking spelling
            e.Correct = (syntaxEdit.Lexer == null) || !syntaxEdit.Lexer.Scheme.IsPlainText(e.ColorStyle.Data - 1);
        }

        private void SyntaxEdit_ScrollButtonClick(object sender, System.EventArgs e)
        {
            // sample event for scrolling buttons
            if (sender is IScrollingButton)
            {
                switch (((IScrollingButton)sender).Name)
                {
                    case "Normal":
                        {
                            syntaxEdit.Pages.PageType = PageType.Normal;
                            break;
                        }

                    case "PageLayout":
                        {
                            syntaxEdit.Pages.PageType = PageType.PageLayout;
                            break;
                        }

                    case "PageBreaks":
                        {
                            syntaxEdit.Pages.PageType = PageType.PageBreaks;
                            break;
                        }

                    case "PageUp":
                        {
                            syntaxEdit.MovePageUp();
                            break;
                        }

                    case "PageDown":
                        {
                            syntaxEdit.MovePageDown();
                            break;
                        }
                }
            }
        }

        private void SystemScrolarsCheckBoxLisoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating scrolling options
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit.Scrolling.Options = chbSystemScrollBars.Checked ? syntaxEdit.Scrolling.Options
                | ScrollingOptions.SystemScrollbars : syntaxEdit.Scrolling.Options & ~ScrollingOptions.SystemScrollbars;
            syntaxSplitEdit.Scrolling.Options = syntaxEdit.Scrolling.Options;
            UpdateScrollBoxes(chbSystemScrollBars);
        }

        private void FlatScrolarsCheckBoxLisoxTextBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // updating scrolling options
            if (scrollBoxUpdate > 0)
                return;
            syntaxEdit.Scrolling.Options = chbFlatScrollBars.Checked ? syntaxEdit.Scrolling.Options
                | ScrollingOptions.FlatScrollbars : syntaxEdit.Scrolling.Options & ~ScrollingOptions.FlatScrollbars;
            syntaxSplitEdit.Scrolling.Options = syntaxEdit.Scrolling.Options;
            UpdateScrollBoxes(chbFlatScrollBars);
        }

        private void TreeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            UpdatePanels(e.Node);
        }

        private void QuickInfoTipsCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            csParser1.Options = chbQuickInfoTips.Checked ? csParser1.Options
                | SyntaxOptions.QuickInfoTips : csParser1.Options & ~SyntaxOptions.QuickInfoTips;
        }

        private void DrawColumnsIndentCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            syntaxEdit.SyntaxPaint.DrawColumnsIndent = chbDrawColumnsIndent.Checked;
            syntaxSplitEdit.SyntaxPaint.DrawColumnsIndent = chbDrawColumnsIndent.Checked;
        }

        private void ColumnsVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            syntaxEdit.EditMargin.ColumnsVisible = chbColumnsVisible.Checked;
            syntaxSplitEdit.EditMargin.ColumnsVisible = chbColumnsVisible.Checked;
        }

        private void StepOverButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btStepOver);
            if (str != StepOverDesc)
                toolTip1.SetToolTip(btStepOver, StepOverDesc);
        }

        private void SereakpointButtonTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSetBreakpoint);
            if (str != SetBreakpointDesc)
                toolTip1.SetToolTip(btSetBreakpoint, SetBreakpointDesc);
        }

        private void ShowGutterCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowGutter);
            if (str != ShowGutterDesc)
                toolTip1.SetToolTip(chbShowGutter, ShowGutterDesc);
        }

        private void PainookMarksCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbPaintBookMarks);
            if (str != ShowBookmarksDesc)
                toolTip1.SetToolTip(chbPaintBookMarks, ShowBookmarksDesc);
        }

        private void DrawLineBookmarksCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawLineBookmarks);
            if (str != LineBookmarksDesc)
                toolTip1.SetToolTip(chbDrawLineBookmarks, LineBookmarksDesc);
        }

        private void ShowBookmarksButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btShowBookmarks);
            if (str != SetBookmarksDesc)
                toolTip1.SetToolTip(btShowBookmarks, SetBookmarksDesc);
        }

        private void ShowMarginCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowMargin);
            if (str != ShowMarginDesc)
                toolTip1.SetToolTip(chbShowMargin, ShowMarginDesc);
        }

        private void AllowDragMarginCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbAllowDragMargin);
            if (str != AllowDragMarginDesc)
                toolTip1.SetToolTip(chbAllowDragMargin, AllowDragMarginDesc);
        }

        private void ShowMarginHintsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowMarginHints);
            if (str != ShowMarginHintsDesc)
                toolTip1.SetToolTip(chbShowMarginHints, ShowMarginHintsDesc);
        }

        private void ColumnsVisibleCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbColumnsVisible);
            if (str != ColumnsVisibleDesc)
                toolTip1.SetToolTip(chbColumnsVisible, ColumnsVisibleDesc);
        }

        private void WordWrapCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbWordWrap);
            if (str != WordWrapDesc)
                toolTip1.SetToolTip(chbWordWrap, WordWrapDesc);
        }

        private void WrapAtMarginCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbWrapAtMargin);
            if (str != WrapAtMarginDesc)
                toolTip1.SetToolTip(chbWrapAtMargin, WrapAtMarginDesc);
        }

        private void ShowScrollHintCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowScrollHint);
            if (str != ShowScrollHintDesc)
                toolTip1.SetToolTip(chbShowScrollHint, ShowScrollHintDesc);
        }

        private void SmoothScrollCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSmoothScroll);
            if (str != SmoothScrollDesc)
                toolTip1.SetToolTip(chbSmoothScroll, SmoothScrollDesc);
        }

        private void AllowSplitCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbAllowSplit);
            if (str != AllowSplitDesc)
                toolTip1.SetToolTip(chbAllowSplit, AllowSplitDesc);
        }

        private void ScroluttonsCheckBoxLisoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbScrollButtons);
            if (str != ScrollButtonDesc)
                toolTip1.SetToolTip(chbScrollButtons, ScrollButtonDesc);
        }

        private void PageLayoutComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbPageLayout);
            if (str != PageLayoutDesc)
                toolTip1.SetToolTip(cbPageLayout, PageLayoutDesc);
        }

        private void PageSizeComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbPageSize);
            if (str != PageSizeDesc)
                toolTip1.SetToolTip(cbPageSize, PageSizeDesc);
        }

        private void HorzRulerCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHorzRuler);
            if (str != HorzRulerDesc)
                toolTip1.SetToolTip(chbHorzRuler, HorzRulerDesc);
        }

        private void VertRulerCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbVertRuler);
            if (str != VertRulerDesc)
                toolTip1.SetToolTip(chbVertRuler, VertRulerDesc);
        }

        private void RulerUnitsComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbRulerUnits);
            if (str != RulerUnitsDesc)
                toolTip1.SetToolTip(cbRulerUnits, RulerUnitsDesc);
        }

        private void RulerAllowDragCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbRulerAllowDrag);
            if (str != RulerAllowDragDesc)
                toolTip1.SetToolTip(chbRulerAllowDrag, RulerAllowDragDesc);
        }

        private void RulerDiscreteCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbRulerDiscrete);
            if (str != RulerDiscreteDesc)
                toolTip1.SetToolTip(chbRulerDiscrete, RulerDiscreteDesc);
        }

        private void RulerDisplayDragLinesCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbRulerDisplayDragLines);
            if (str != RulerDisplayDragLinesDesc)
                toolTip1.SetToolTip(chbRulerDisplayDragLines, RulerDisplayDragLinesDesc);
        }

        private void CheckSpellingCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbCheckSpelling);
            if (str != CheckSpellingDesc)
                toolTip1.SetToolTip(chbCheckSpelling, CheckSpellingDesc);
        }

        private void HighlightUrlsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHighlightUrls);
            if (str != HighlightURLDesc)
                toolTip1.SetToolTip(chbHighlightUrls, HighlightURLDesc);
        }

        private void ShowHyperTextHintsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowHyperTextHints);
            if (str != ShowHyperTextHintsDesc)
                toolTip1.SetToolTip(chbShowHyperTextHints, ShowHyperTextHintsDesc);
        }

        private void AllowOutliningCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbAllowOutlining);
            if (str != AllowOutliningDesc)
                toolTip1.SetToolTip(chbAllowOutlining, AllowOutliningDesc);
        }

        private void DrawOnGutterCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawOnGutter);
            if (str != DrawOnGutterDesc)
                toolTip1.SetToolTip(chbDrawOnGutter, DrawOnGutterDesc);
        }

        private void DrawLinesCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawLines);
            if (str != DrawLinesDesc)
                toolTip1.SetToolTip(chbDrawLines, DrawLinesDesc);
        }

        private void DrawButtonsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawButtons);
            if (str != DrawButtonsDesc)
                toolTip1.SetToolTip(chbDrawButtons, DrawButtonsDesc);
        }

        private void ShowHintsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowHints);
            if (str != ShowHintsDesc)
                toolTip1.SetToolTip(chbShowHints, ShowHintsDesc);
        }

        private void RtfButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btRtf);
            if (str != RTFDesc)
                toolTip1.SetToolTip(btRtf, RTFDesc);
        }

        private void PrintPreviewButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrintPreview);
            if (str != PrintPreviewDesc)
                toolTip1.SetToolTip(btPrintPreview, PrintPreviewDesc);
        }

        private void HtmuttonLisoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLineNumbers);
            if (str != HTMLDesc)
                toolTip1.SetToolTip(chbLineNumbers, HTMLDesc);
        }

        private void PrinuttonTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrint);
            if (str != PrintDesc)
                toolTip1.SetToolTip(btPrint, PrintDesc);
        }

        private void PrintSetupButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrintSetup);
            if (str != PageSetupDesc)
                toolTip1.SetToolTip(btPrintSetup, PageSetupDesc);
        }

        private void PrintOptionsButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrintOptions);
            if (str != PrintOptionsDesc)
                toolTip1.SetToolTip(btPrintOptions, PrintOptionsDesc);
        }

        private void DisableSelectionCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDisableSelection);
            if (str != DisableSelectionDesc)
                toolTip1.SetToolTip(chbDisableSelection, DisableSelectionDesc);
        }

        private void DisableDraggingCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDisableDragging);
            if (str != DisableDraggingDesc)
                toolTip1.SetToolTip(chbDisableDragging, DisableDraggingDesc);
        }

        private void SeleceyondEolCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSelectBeyondEol);
            if (str != SelectBeyondEolDesc)
                toolTip1.SetToolTip(chbSelectBeyondEol, SelectBeyondEolDesc);
        }

        private void UseColorsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbUseColors);
            if (str != UseColorsDesc)
                toolTip1.SetToolTip(chbUseColors, UseColorsDesc);
        }

        private void HideSelectionCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHideSelection);
            if (str != HideSelectionDesc)
                toolTip1.SetToolTip(chbHideSelection, HideSelectionDesc);
        }

        private void SelectLineOnDblClickCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSelectLineOnDblClick);
            if (str != SelectLineOnDblClickDesc)
                toolTip1.SetToolTip(chbSelectLineOnDblClick, SelectLineOnDblClickDesc);
        }

        private void HighlightSelectedWordsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(HighlightSelectedWordsCheckBox);
            if (str != HighlightSelectedWordsDesc)
                toolTip1.SetToolTip(HighlightSelectedWordsCheckBox, HighlightSelectedWordsDesc);
        }

        private void PersistenlocksCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbPersistentBlocks);
            if (str != PersistentBlocksDesc)
                toolTip1.SetToolTip(chbPersistentBlocks, PersistentBlocksDesc);
        }

        private void OverwriteBlocksCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbOverwriteBlocks);
            if (str != OverwriteBlocksDesc)
                toolTip1.SetToolTip(chbOverwriteBlocks, OverwriteBlocksDesc);
        }

        private void BeyondEolCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbBeyondEol);
            if (str != BeyondEolDesc)
                toolTip1.SetToolTip(chbBeyondEol, BeyondEolDesc);
        }

        private void BeyondEofCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbBeyondEof);
            if (str != BeyondEofDesc)
                toolTip1.SetToolTip(chbBeyondEof, BeyondEofDesc);
        }

        private void UpAtLineBeginCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbUpAtLineBegin);
            if (str != UpAtLineBeginDesc)
                toolTip1.SetToolTip(chbUpAtLineBegin, UpAtLineBeginDesc);
        }

        private void DownAtLineEndCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDownAtLineEnd);
            if (str != DownAtLineEndDesc)
                toolTip1.SetToolTip(chbDownAtLineEnd, DownAtLineEndDesc);
        }

        private void MoveOnRighuttonCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbMoveOnRightButton);
            if (str != MoveOnRightButtonDesc)
                toolTip1.SetToolTip(chbMoveOnRightButton, MoveOnRightButtonDesc);
        }

        private void LineNumbersCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLineNumbers);
            if (str != LineNumbersDesc)
                toolTip1.SetToolTip(chbLineNumbers, LineNumbersDesc);
        }

        private void LinesOnGutterCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLinesOnGutter);
            if (str != LinesOnGutterDesc)
                toolTip1.SetToolTip(chbLinesOnGutter, LinesOnGutterDesc);
        }

        private void LinesBeyondEofCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLinesBeyondEof);
            if (str != LinesBeyondEofDesc)
                toolTip1.SetToolTip(chbLinesBeyondEof, LinesBeyondEofDesc);
        }

        private void LineModificatorCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLineModificator);
            if (str != LineModificatorDesc)
                toolTip1.SetToolTip(chbLineModificator, LineModificatorDesc);
        }

        private void HighlightCurrentLineCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHighlightCurrentLine);
            if (str != HighlightCurrentLineDesc)
                toolTip1.SetToolTip(chbHighlightCurrentLine, HighlightCurrentLineDesc);
        }

        private void LineNumbersAlignCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLineNumbersAlign);
            if (str != LineNumbersAlignDesc)
                toolTip1.SetToolTip(cbLineNumbersAlign, LineNumbersAlignDesc);
        }

        private void SeparateLinesCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSeparateLines);
            if (str != SeparateLinesDesc)
                toolTip1.SetToolTip(chbSeparateLines, SeparateLinesDesc);
        }

        private void WhiteSpaceVisibleCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbWhiteSpaceVisible);
            if (str != WhiteSpaceVisibleDesc)
                toolTip1.SetToolTip(chbWhiteSpaceVisible, WhiteSpaceVisibleDesc);
        }

        private void TransparentCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbTransparent);
            if (str != TransparentDesc)
                toolTip1.SetToolTip(chbTransparent, TransparentDesc);
        }

        private void HighlightReferencesCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHighlightReferences);
            if (str != HighlightReferencesDesc)
                toolTip1.SetToolTip(chbHighlightReferences, HighlightReferencesDesc);
        }

        private void QuickInfoTipsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbQuickInfoTips);
            if (str != QuickInfoTipsDesc)
                toolTip1.SetToolTip(chbQuickInfoTips, QuickInfoTipsDesc);
        }

        private void DrawColumnsIndentCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDrawColumnsIndent);
            if (str != DrawColumnsIndentDesc)
                toolTip1.SetToolTip(chbDrawColumnsIndent, DrawColumnsIndentDesc);
        }

        private void HighlighracesCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHighlightBraces);
            if (str != HighlightBracesDesc)
                toolTip1.SetToolTip(chbHighlightBraces, HighlightBracesDesc);
        }

        private void UseRoundRectCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbUseRoundRect);
            if (str != UseRoundRectDesc)
                toolTip1.SetToolTip(chbUseRoundRect, UseRoundRectDesc);
        }

        private void TempHighlighracesCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbTempHighlightBraces);
            if (str != TempHighlightBracesDesc)
                toolTip1.SetToolTip(cbTempHighlightBraces, TempHighlightBracesDesc);
        }

        private void LoadButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoad);
            if (str != LoadDesc)
                toolTip1.SetToolTip(btLoad, LoadDesc);
        }

        private void SaveButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSave);
            if (str != SaveDesc)
                toolTip1.SetToolTip(btSave, SaveDesc);
        }

        private void FindButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(FindNextButton);
            if (str != FindDesc)
                toolTip1.SetToolTip(FindNextButton, FindDesc);
        }

        private void ReplaceButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btReplace);
            if (str != ReplaceDesc)
                toolTip1.SetToolTip(btReplace, ReplaceDesc);
        }

        private void GotoButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btGoto);
            if (str != GotoDesc)
                toolTip1.SetToolTip(btGoto, GotoDesc);
        }

        private void EnableOrDisableAllAnnotations()
        {
            var enabled = scrollBarAnnotationsEnabledCheckBox.Checked;

            bookmarksTypeCheckBox.Enabled =
                changedLinesTypeCheckBox.Enabled =
                cursorPositionTypeCheckBox.Enabled =
                customTypeCheckBox.Enabled =
                searchResultsTypeCheckBox.Enabled =
                syntaxErrorsTypeCheckBox.Enabled =
                enabled;

            var scrolling = syntaxEdit.Scrolling;

            if (enabled)
                scrolling.Options |= ScrollingOptions.VerticalScrollBarAnnotations;
            else
                scrolling.Options &= ~ScrollingOptions.VerticalScrollBarAnnotations;
        }

        private void ScrolarAnnotationsEnabledCheckBoxLisoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            EnableOrDisableAllAnnotations();
        }

        private void AnnotationTypeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var annotations = syntaxEdit.Scrolling.Annotations;
            var enabledKinds = annotations.EnabledAnnotationKinds;

            Action<CheckBox, ScrollBarAnnotationKinds> apply = (checkBox, kind) =>
            {
                if (checkBox.Checked)
                    enabledKinds |= kind;
                else
                    enabledKinds &= ~kind;
            };

            apply(bookmarksTypeCheckBox, ScrollBarAnnotationKinds.Bookmark);
            apply(changedLinesTypeCheckBox, ScrollBarAnnotationKinds.Change);
            apply(cursorPositionTypeCheckBox, ScrollBarAnnotationKinds.CursorPosition);
            apply(customTypeCheckBox, ScrollBarAnnotationKinds.Custom);
            apply(searchResultsTypeCheckBox, ScrollBarAnnotationKinds.SearchResult);
            apply(syntaxErrorsTypeCheckBox, ScrollBarAnnotationKinds.Error);

            annotations.EnabledAnnotationKinds = enabledKinds;
        }

        private void ScrolarsVisualStyleComboBoxLisoxTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit.Scrolling.ScrollBarsVisualStyle = (ScrollBarVisualStyle)scrollBarsVisualStyleComboBox.SelectedItem;
        }

        private void CustomAnnotationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit.Refresh();
        }

        private void ChangeErrorsAppearanceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit.Scrolling.Annotations.SetAnnotationTypeAppearance(
                ScrollBarAnnotationType.SyntaxError,
                changeErrorsAppearanceCheckBox.Checked ? customErrorAppearance : defaultErrorAppearance);
        }

        private void SaveButton_ButtonClick(object sender, EventArgs e)
        {
        }

        private void StepOver_ButtonClick(object sender, EventArgs e)
        {
            StepOver();
        }

        private void SereakpointTextBox_ButtonClick(object sender, EventArgs e)
        {
            SetBreakpoint();
        }

        private void StepOver()
        {
            if (index < (endLine - startLine))
            {
                if (syntaxEdit.Source.LineStyles.GetLineStyle(startLine + index) >= 0)
                    syntaxEdit.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                index++;
                while ((index < (endLine - startLine)) && (syntaxEdit.Source.Lines[startLine + index].Trim() == string.Empty))
                    index++;

                syntaxEdit.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                syntaxEdit.MakeVisible(new Point(0, startLine + index));
            }
            else
            {
                syntaxEdit.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                index = 0;
            }
        }

        private void SetBreakpoint()
        {
            syntaxEdit.Source.BookMarks.ToggleBookMark(syntaxEdit.Position, 11);
            syntaxEdit.Source.LineStyles.ToggleLineStyle(syntaxEdit.Position.Y, 0, 1);
        }

        #region Internal Classes

        public class SpellChecker
        {
            private WordList wordList = null;

            public SpellChecker()
            {
                try
                {
                    string dir = Application.StartupPath;
                    if (!File.Exists(Path.GetFullPath(Path.Combine(dir, "en_us.aff"))))
                        dir = Application.StartupPath + @"\..\..\..\";
                    wordList = WordList.CreateFromFiles(Path.GetFullPath(Path.Combine(dir, "en_us.dic")), Path.GetFullPath(Path.Combine(dir, "en_us.aff")));
                }
                catch
                {
                }
            }

            public void CheckSpelling(ISyntaxEdit edit, bool spell)
            {
                edit.Spelling.CheckSpelling = spell;
                if (spell)
                    edit.Spelling.WordSpell += new WordSpellEvent(WordSpell);
                else
                    edit.Spelling.WordSpell -= new WordSpellEvent(WordSpell);
            }

            private void WordSpell(object sender, WordSpellEventArgs e)
            {
                ITextSource source = (ITextSource)sender;
                bool correct = (wordList != null) ? wordList.Check(e.Text) : true;
                if (source.Lexer != null)
                {
                    LexToken tok = (LexToken)(e.ColorStyle.Data - 1);
                    if ((tok == LexToken.String) || (tok == LexToken.Comment) || (tok == LexToken.XmlComment))
                        e.Correct = (e.Text.Length <= 1) || correct;
                }
                else
                    e.Correct = (e.Text.Length <= 1) || correct;
            }
        }

        internal class ComponentTypeDescriptor : Component, ICustomTypeDescriptor
        {
            private Component owner;

            public ComponentTypeDescriptor(Component owner)
            {
                this.owner = owner;
            }

            public Component Owner
            {
                get { return owner; }
            }

            #region Methods
            AttributeCollection ICustomTypeDescriptor.GetAttributes()
            {
                return TypeDescriptor.GetAttributes(Owner, true);
            }

            string ICustomTypeDescriptor.GetClassName()
            {
                return TypeDescriptor.GetClassName(Owner, true);
            }

            string ICustomTypeDescriptor.GetComponentName()
            {
                return TypeDescriptor.GetComponentName(Owner, true);
            }

            TypeConverter ICustomTypeDescriptor.GetConverter()
            {
                return TypeDescriptor.GetConverter(Owner, true);
            }

            EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
            {
                return TypeDescriptor.GetDefaultEvent(Owner, true);
            }

            object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
            {
                return TypeDescriptor.GetEditor(Owner, editorBaseType, true);
            }

            EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
            {
                return TypeDescriptor.GetEvents(Owner, true);
            }

            PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
            {
                return TypeDescriptorGetProperties(attributes);
            }

            PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
            {
                return TypeDescriptor.GetDefaultProperty(Owner, true);
            }

            EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
            {
                return TypeDescriptor.GetEvents(Owner, attributes, true);
            }

            PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
            {
                return ((ICustomTypeDescriptor)Owner).GetProperties(new Attribute[0]);
            }

            object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
            {
                return Owner;
            }

            protected virtual PropertyDescriptorCollection TypeDescriptorGetProperties(Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(Owner, attributes, true);
            }

            #endregion
        }

        internal class ComponentTypeDescriptorEx : ComponentTypeDescriptor
        {
            private ArrayList obsolete = null;

            public ComponentTypeDescriptorEx(Component owner)
                : base(owner)
            {
            }

            public ComponentTypeDescriptorEx(Component owner, ArrayList obsolete)
                : base(owner)
            {
                this.obsolete = obsolete;
            }

            protected override PropertyDescriptorCollection TypeDescriptorGetProperties(Attribute[] attributes)
            {
                PropertyDescriptorCollection result = base.TypeDescriptorGetProperties(attributes);
                ArrayList props = new ArrayList();

                props.AddRange(result);
                RemoveObsoleteProperties(ref props);
                PropertyDescriptor[] propArray = (PropertyDescriptor[])props.ToArray(
                    typeof(PropertyDescriptor));
                return new PropertyDescriptorCollection(propArray);
            }

            private void RemoveObsoleteProperties(ref ArrayList properties)
            {
                for (int i = properties.Count - 1; i >= 0; i--)
                {
                    if (obsolete.Contains(((PropertyDescriptor)properties[i]).Name))
                        properties.Remove(properties[i]);
                }
            }
        }

        internal class CustomElementSite : ISite
        {
            #region Fields
            private IComponent fcomponent;
            private DefaultComponentContainer fcontainer;
            private string name;
            #endregion

            internal CustomElementSite(IComponent component, DefaultComponentContainer container, string name)
            {
                fcomponent = component;
                fcontainer = container;
                this.name = name;
            }

            #region Properties
            public IComponent Component
            {
                get
                {
                    return fcomponent;
                }
            }

            public IContainer Container
            {
                get
                {
                    return fcontainer;
                }
            }

            public bool DesignMode
            {
                get
                {
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }
            #endregion

            #region Methods

            public object GetService(Type service)
            {
                if (service == typeof(ITypeDescriptorFilterService))
                    return fcomponent as ITypeDescriptorFilterService;
                if (service != typeof(ISite))
                    return fcontainer.GetService(service);
                return this;
            }
            #endregion
        }

        internal class DefaultComponentContainer : Container, IContainer
        {
            #region Methods

            public new object GetService(Type service)
            {
                return base.GetService(service);
            }

            protected override ISite CreateSite(IComponent component, string name)
            {
                return new CustomElementSite(component, this, name);
            }

            #endregion
        }

        internal class CustomVisualTheme : StandardVisualTheme
        {
            public CustomVisualTheme()
                : base("MyCustomTheme")
            {
            }

            protected override VisualThemeColors GetColors()
            {
                var colors = DarkVisualTheme.Instance.Colors.Clone();
                colors.Reswords = Color.Red;
                colors.WindowBackground = Color.FromArgb(40, 40, 40);
                return colors;
            }
        }

        #endregion
    }
}
