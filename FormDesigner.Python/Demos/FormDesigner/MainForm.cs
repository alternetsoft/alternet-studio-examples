#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Editor.Common;
using Alternet.Editor.Python;
using Alternet.Editor.TextSource;
using Alternet.FormDesigner.Integration;
using Alternet.FormDesigner.WinForms;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;

namespace Alternet.FormDesigner.Demo
{
    public partial class MainForm : Form
    {
        private Dictionary<Tuple<string, string>, TabPage> codeTabPages = new Dictionary<Tuple<string, string>, TabPage>();
        private Dictionary<TabPage, IFormDesignerControl> formDesigners = new Dictionary<TabPage, IFormDesignerControl>();
        private HashSet<string> editedCodeFiles = new HashSet<string>();
        private Dictionary<string, EditorFormDesignerDataSource> sourcesByFormId = new Dictionary<string, EditorFormDesignerDataSource>();
        private bool updating = false;
        private IScriptRun scriptRun;

        public MainForm()
        {
            SetupPython();
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "Alternet.FormDesigner.Demo.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

            scriptRun = new ScriptRun();
            InitDefaultAssemblies();

            leftTabControl.SelectedIndex = 1;
            filesUserControl.Initialize(GetTestFilesDirectoryPath(), OpenFormFile);

            var foundForms = filesUserControl.RefreshFiles();
            if (foundForms.Any())
                OpenAllFormFiles(foundForms.First());

            OnSelectedContentTabChanged();
            AutoLoadToolbox();
        }

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"Alternet.Studio.Demo\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled(true))
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "Call Method Python Demo",
                Message = "Deploying Python and packages...",
                ProgressBarStyle = ProgressBarStyle.Marquee,
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython(true);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private IScriptEdit ActiveEditor
        {
            get
            {
                var tab = contentTabControl.TabPages.Count == 0 ? null : contentTabControl.SelectedTab;
                return tab == null ? null : tab.Tag as IScriptEdit;
            }
        }

        private FormDesignerControl ActiveFormDesigner
        {
            get
            {
                var tab = contentTabControl.TabPages.Count == 0 ? null : contentTabControl.SelectedTab;
                return tab == null ? null : tab.Tag as FormDesignerControl;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = PromptToSaveUnsavedDesigns();

            AutoSaveToolbox();

            base.OnClosing(e);
        }

        private void InitDefaultAssemblies()
        {
            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.WindowsForms;
            scriptRun.ScriptSource.Imports.Add("System.Windows.Forms");
            scriptRun.ScriptSource.Imports.Add("System.Drawing");
        }

        private void UpdateOptions()
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            updating = true;
            try
            {
                bool snapToGrid = designer.Options.UseSnapLines == false;
                showGridToolStripMenuItem.Enabled = snapToGrid;
                snapToGridToolStripMenuItem.Enabled = snapToGrid;
                gridSizeToolStripMenuItem.Enabled = snapToGrid && designer.Options.ShowGrid;

                showGridToolStripMenuItem.Checked = designer.Options.ShowGrid;
                snapToGridToolStripMenuItem.Checked = designer.Options.SnapToGrid;
                snapLinesToolStripMenuItem.Checked = designer.Options.UseSnapLines;
                smartTagsToolStripMenuItem.Checked = designer.Options.UseSmartTags;
                gridSizeSmallToolStripMenuItem.Checked = designer.Options.GridSize.Width == 8;
                gridSizeLargeToolStripMenuItem.Checked = designer.Options.GridSize.Width == 16;
            }
            finally
            {
                updating = false;
            }
        }

        private void AutoSaveToolbox()
        {
            var toolboxAutoSaveFileName = GetToolboxAutoSaveFileName();
            using (var fs = new FileStream(toolboxAutoSaveFileName, FileMode.Create))
                toolboxControl.Save(fs);
        }

        private string GetToolboxAutoSaveFileName()
        {
            var directory = Path.Combine(Path.GetTempPath(), "Alternet.FormDesigner.Python.Demo", "dotnet-" + Environment.Version.ToString(2));
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return Path.Combine(directory, "ToolboxAutoSave.xml");
        }

        private void AutoLoadToolbox()
        {
            var toolboxAutoSaveFileName = GetToolboxAutoSaveFileName();
            if (File.Exists(toolboxAutoSaveFileName))
            {
                using (var fs = new FileStream(toolboxAutoSaveFileName, FileMode.Open))
                {
                    try
                    {
                        toolboxControl.Load(fs);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error autoloading the toolbox - resetting. Exception: " + e);
                        toolboxControl.Reset();
                    }
                }
            }
        }

        private void ContentTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedContentTabChanged();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null)
            {
                if (edit.CanCopy)
                    edit.Copy();
            }
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanCopy)
                    designer.DesignerCommands.Copy();
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null && edit.CanCut)
                edit.Cut();
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanCut)
                    designer.DesignerCommands.Cut();
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.Delete();
        }

        private void Designer_CommandStateChanged(object sender, EventArgs e)
        {
            UpdateCommandsState();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Designer_NavigateToUserMethodRequested(object sender, NavigateToUserMethodRequestedEventArgs e)
        {
            var designer = (FormDesignerControl)sender;

            if (designer != ActiveFormDesigner)
                return;

            var source = (EditorFormDesignerDataSource)designer.Source;

            OpenOrActivateCode(source.UserCodeFileName, source.UserCodeFileName);
            SetCaretToMethod(source.UserCodeFileName, e.MethodName);
        }

        private EditorFormDesignerDataSource GetDesignerSource(string formId)
        {
            EditorFormDesignerDataSource ds;
            if (!sourcesByFormId.TryGetValue(formId, out ds))
            {
                ds = new EditorFormDesignerDataSource(
                    formId,
                    fileName =>
                    {
                        var source = new FormDesignerTextSource();
                        source.LoadFile(fileName);
                        return source;
                    },
                    designerFileNameSuffix: "_Designer");

                ds.ResourceCultureAdded += (o, e) => filesUserControl.RefreshFiles();

                sourcesByFormId.Add(formId, ds);
            }

            return ds;
        }

        private string GetTestFilesDirectoryPath()
        {
            string subdirectory = @"Resources\Designer.Python\WinForms";
#if NETCOREAPP
            subdirectory += "-dotnetcore";
#endif

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directory = Path.Combine(baseDirectory, subdirectory);

            if (!Directory.Exists(directory))
                directory = Path.Combine(Path.GetFullPath(baseDirectory.TrimEnd('\\') + @"\..\..\..\..\..\..\"), subdirectory);

            return directory;
        }

        private void OnSelectedContentTabChanged()
        {
            var designer = ActiveFormDesigner;
            var editor = ActiveEditor;

            toolboxControl.FormDesignerControl = designer;
            propertyGridControl.FormDesignerControl = designer;
            outlineControl.FormDesignerControl = designer;

            bool haveActiveDesigner = designer != null;

            saveToolStripMenuItem.Enabled = haveActiveDesigner || editor != null;
            selectAllToolStripMenuItem.Enabled = haveActiveDesigner;

            UpdateEditorCommandsState();
            UpdateOptions();

            if (designer != null)
                ReloadDesigner(designer);
        }

        private TabPage OpenCode(string formId, string fileName)
        {
            var page = new TabPage("Code: " + Path.GetFileName(fileName));
            contentTabControl.TabPages.Add(page);

            var editor = new ScriptCodeEdit();

            editor.Dock = DockStyle.Fill;
            editor.Tag = fileName;
            editor.AllowDrop = true;

            editor.DragEnter += (o, e) => e.Effect = DragDropEffects.Copy;
            editor.DragOver += (o, e) => e.Effect = DragDropEffects.Copy;
            editor.DragDrop += (o, e) =>
            {
                var tool = e.Data.GetData(typeof(ToolboxItem)) as ToolboxItem;
                if (tool != null)
                    MessageBox.Show("Toolbox item dropped: " + tool.DisplayName);
            };

            editor.TextChanged += (o, e) =>
            {
                if (contentTabControl.SelectedTab == page)
                    editedCodeFiles.Add(fileName);
            };

            editor.InitSyntax();

            editor.StatusChanged += new EventHandler(StatusChanged);

            var source = GetDesignerSource(formId);
            FormDesignerEditorHelpers.SetEditorSource(editor, fileName, source);
            editor.FileName = fileName;
            page.Controls.Add(editor);
            page.Tag = editor;
            contentTabControl.SelectTab(page);
            UpdateEditorCommandsState();
            var key = new Tuple<string, string>(formId, fileName);
            codeTabPages.Add(key, page);

            return page;
        }

        private void StatusChanged(object sender, EventArgs e)
        {
            UpdateEditorCommandsState();
        }

        private DesignerReferencedAssemblies GetDefaultReferences()
        {
            var defaultReferences = DesignerReferencedAssemblies.DefaultForPython;
            return defaultReferences;
        }

        private DesignerReferencedAssemblies GetReferencedAssemblies(EditorFormDesignerDataSource source)
        {
            var defaultReferences = GetDefaultReferences();

            var formSettings = FormSettingsService.LoadSettings(source);
            if (formSettings.AssemblyReferences.Any())
                return defaultReferences.WithAssemblyNames(formSettings.AssemblyReferences.Select(x => x.AssemblyPath).ToArray());
            else
                return defaultReferences;
        }

        private void NavigateToCompilationError(FormDesignerControl designer, DesignerCompilerError error)
        {
            if (!string.IsNullOrEmpty(error.FilePath))
            {
                var edit = OpenOrActivateCode(designer.Source.UserCodeFileName, error.FilePath);
                if (edit != null)
                {
                    edit.MakeVisible(new Point(error.CharacterNumber, error.LineNumber), true);
                    edit.Focus();
                }
            }
        }

        private void OpenDesigner(string fileName)
        {
            var designerTab = FindDesigner(fileName);
            if (designerTab != null)
            {
                contentTabControl.SelectedTab = designerTab.Item2;
                return;
            }

            var source = GetDesignerSource(fileName);

            var designer = new FormDesignerControl
            {
                Dock = DockStyle.Fill,
                ReferencedAssemblies = GetReferencedAssemblies(source),
            };

            foreach (var assemblyName in designer.ReferencedAssemblies.AssemblyNames)
                scriptRun.ScriptSource.References.Add(assemblyName);

            designer.CompilerErrorClick += Designer_CompilerErrorClick;
            designer.Source = source;
            var page = new TabPage("Design: " + Path.GetFileNameWithoutExtension(fileName));
            contentTabControl.TabPages.Add(page);

            designer.NavigateToUserMethodRequested += Designer_NavigateToUserMethodRequested;
            designer.CommandStateChanged += Designer_CommandStateChanged;

            designer.IsSmartDiffCodeSerializationRequired = d => ((ITextSource)((EditorFormDesignerDataSource)d.Source).DesignerTextSource).Edits.Any();
            page.Controls.Add(designer);
            formDesigners.Add(page, designer);
            page.Tag = designer;
            contentTabControl.SelectTab(page);
        }

        private Tuple<IFormDesignerControl, TabPage> FindDesigner(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabPage tabPage in contentTabControl.TabPages)
            {
                IFormDesignerControl designer;

                if (!formDesigners.TryGetValue(tabPage, out designer))
                    continue;

                var path = designer.Source.UserCodeFileName;
                path = new Uri(path).LocalPath;
                if (path.Equals(canonicalPath, StringComparison.OrdinalIgnoreCase))
                {
                    return new Tuple<IFormDesignerControl, TabPage>(designer, tabPage);
                }
            }

            return null;
        }

        private void RemoveDesigner(Tuple<IFormDesignerControl, TabPage> designerTab)
        {
            if (designerTab != null)
            {
                CloseDesigner(designerTab.Item1);
                formDesigners.Remove(designerTab.Item2);
            }
        }

        private void CloseDesigner(IFormDesignerControl designer)
        {
            designer.NavigateToUserMethodRequested -= Designer_NavigateToUserMethodRequested;
            designer.CommandStateChanged -= Designer_CommandStateChanged;
            designer.CompilerErrorClick -= Designer_CompilerErrorClick;
            var userFileName = designer.Source.UserCodeFileName;
            if (sourcesByFormId.ContainsKey(userFileName))
                sourcesByFormId.Remove(userFileName);
            ((FormDesignerControl)designer).Dispose();
        }

        private void Designer_CompilerErrorClick(object sender, DesignerCompilerErrorClickEventArgs e)
        {
            NavigateToCompilationError((FormDesignerControl)sender, e.Error);
        }

        private void OpenFormFile(string formId, string fileName, FormOpenMode openMode)
        {
            var oldCursor = Cursor;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (openMode == FormOpenMode.Design)
                    OpenDesigner(fileName);
                else if (openMode == FormOpenMode.Code)
                {
                    OpenOrActivateCode(formId, fileName);
                }
                else
                    throw new Exception();

                OnSelectedContentTabChanged();
            }
            finally
            {
                Cursor = oldCursor;
            }
        }

        private TabPage FindPage(string formId, string fileName)
        {
            var key = new Tuple<string, string>(formId, fileName);

            TabPage page;
            if (codeTabPages.TryGetValue(key, out page))
                return page;
            return null;
        }

        private IScriptEdit OpenOrActivateCode(string formId, string fileName)
        {
            TabPage page = FindPage(formId, fileName);
            if (page == null)
            {
                page = OpenCode(formId, fileName);
            }
            else
                contentTabControl.SelectTab(page);

            return (IScriptEdit)page.Tag;
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null)
            {
                if (edit.CanPaste)
                    edit.Paste();
            }
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanPaste)
                    designer.DesignerCommands.Paste();
            }
        }

        private DialogResult PromptToSaveFile(string fileType, string fileName)
        {
            var message = string.Format(
                "Do you want to save changes to the {0} file: {1}",
                fileType,
                fileName);

            var result = MessageBox.Show(
                this,
                message,
                "Designer Demo",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            return result;
        }

        private bool PromptToSaveUnsavedDesigns()
        {
            var designers = contentTabControl.TabPages.Cast<TabPage>().Select(x => x.Tag).OfType<FormDesignerControl>();
            foreach (var designer in designers)
            {
                if (designer.Source.IsModified)
                {
                    DialogResult result = PromptToSaveFile("designer", designer.Source.DesignerFileName);

                    if (result == DialogResult.Yes)
                        SaveDesignerFiles(designer);
                    else if (result == DialogResult.No)
                        continue;
                    else if (result == DialogResult.Cancel)
                        return true;
                    else
                        throw new Exception();
                }
            }

            var editors = contentTabControl.TabPages.Cast<TabPage>().Select(x => x.Tag).OfType<IScriptEdit>();
            foreach (var editor in editors)
            {
                if (editor.Modified)
                {
                    DialogResult result = PromptToSaveFile("code", (string)((Control)editor).Tag);

                    if (result == DialogResult.Yes)
                        SaveEditorFile(editor);
                    else if (result == DialogResult.No)
                        continue;
                    else if (result == DialogResult.Cancel)
                        return true;
                    else
                        throw new Exception();
                }
            }

            return false;
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null && edit.CanRedo)
                edit.Redo();
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Redo();
            }
        }

        private void ReloadDesigner(IFormDesignerControl designer)
        {
            try
            {
                var designerFileName = designer.Source.DesignerFileName;
                if (this.editedCodeFiles.Contains(designerFileName))
                {
                    designer.Reload();
                    editedCodeFiles.Remove(designerFileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RunActiveFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunScript();
        }

        private bool SaveAllModifiedFiles()
        {
            foreach (TabPage tabPage in contentTabControl.TabPages)
            {
                IScriptEdit edit = tabPage.Tag as IScriptEdit;
                if (edit != null && edit.Modified)
                {
                    edit.SaveFile(edit.FileName);
                }

                var designer = tabPage.Tag as IFormDesignerControl;
                if (designer != null)
                {
                    SaveDesignerFiles(designer);
                }
            }

            return true;
        }

        private EditorFormDesignerDataSource GetActiveDesignerSource()
        {
            var designer = ActiveFormDesigner;
            if (designer != null)
                return (EditorFormDesignerDataSource)designer.Source;

            var editor = ActiveEditor;
            if (editor != null)
            {
                var fileName = editor.FileName;
                var trimmed = Path.ChangeExtension(fileName, string.Empty).TrimEnd('.');
                var extension = Path.GetExtension(fileName).TrimStart('.');
                var suffix = "_Designer";
                if (trimmed.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                    return GetDesignerSource(trimmed.Substring(0, trimmed.Length - suffix.Length) + "." + extension);

                if (extension.Equals("py", StringComparison.OrdinalIgnoreCase))
                    return GetDesignerSource(fileName);
            }

            return null;
        }

        #region Scripter

        private void RunScript()
        {
            var source = GetActiveDesignerSource();
            if (source == null)
                return;

            if (SaveAllModifiedFiles() && SetScriptSource(source))
            {
                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        var error = scriptRun.ScriptHost.CompilerErrors.FirstOrDefault();
                        MessageBox.Show(error.Message);
                        return;
                    }
                }

                scriptRun.RunAsync();
            }
        }

        private bool SetScriptSource(EditorFormDesignerDataSource source)
        {
            string fileName = source.UserCodeFileName;
            if (new FileInfo(fileName).Exists)
            {
                scriptRun.ScriptSource.FromScriptFile(fileName);
                return true;
            }

            return false;
        }

        #endregion

        private void SaveDesignerFiles(IFormDesignerControl designer)
        {
            var source = (EditorFormDesignerDataSource)designer.Source;
            var modified = source.IsModified;
            source.Save();

            if (modified)
                FormSettingsService.SaveSettings(BuildFormSettings(designer), source);
        }

        private FormSettings BuildFormSettings(IFormDesignerControl designer)
        {
            var defaultReferences = new HashSet<string>(
                GetDefaultReferences().AssemblyNames,
                StringComparer.OrdinalIgnoreCase);

            var references = designer.ReferencedAssemblies.AssemblyNames.Where(x => !defaultReferences.Contains(x)).Select(
                x => new FormSettings.AssemblyReference(x)).ToArray();

            return new FormSettings(references);
        }

        private void SaveEditorFile(IScriptEdit editor)
        {
            editor.SaveFile((string)((Control)editor).Tag);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer != null)
            {
                SaveDesignerFiles(designer);
                return;
            }

            var editor = ActiveEditor;
            if (editor != null)
                SaveEditorFile(editor);
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.SelectAll();
        }

        private void SetCaretToMethod(string userCodeFileName, string methodName)
        {
            string toFind = methodName;

            var editor = ActiveEditor;

            Point oldPosition = editor.Position;

            editor.Position = new System.Drawing.Point();
            if (editor.Find(toFind))
            {
                editor.MoveToLine(ActiveEditor.Position.Y + 2);
                editor.MoveLineEnd();
            }
            else
                editor.Position = oldPosition;
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null && edit.CanUndo)
                edit.Undo();
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Undo();
            }
        }

        private void SendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.SendToBack();
        }

        private void BringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.BringToFront();
        }

        private void LockControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.LockControls();
        }

        private void ShowGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (updating)
                return;

            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.Options.ShowGrid = showGridToolStripMenuItem.Checked;
            UpdateOptions();
            designer.Reload();
        }

        private void SnapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (updating)
                return;

            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.Options.SnapToGrid = snapToGridToolStripMenuItem.Checked;
            designer.Reload();
        }

        private void SnapLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (updating)
                return;

            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.Options.UseSnapLines = snapLinesToolStripMenuItem.Checked;
            UpdateOptions();
            designer.Reload();
        }

        private void SmartTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (updating)
                return;

            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.Options.UseSmartTags = smartTagsToolStripMenuItem.Checked;
            designer.Reload();
        }

        private void GridSizeSmallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (updating)
                return;

            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.Options.GridSize = new Size(8, 8);
            UpdateOptions();
            designer.Reload();
        }

        private void GridSizeLargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (updating)
                return;

            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.Options.GridSize = new Size(16, 16);
            UpdateOptions();
            designer.Reload();
        }

        private void LeftsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.AlignLeft();
        }

        private void CentresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.AlignHorizontalCenters();
        }

        private void RightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.AlignRight();
        }

        private void TopsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.AlignTop();
        }

        private void MiddlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.AlignVerticalCenters();
        }

        private void BottomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.AlignBottom();
        }

        private void ToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.AlignToGrid();
        }

        private void MakeEqualHorzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.HorizSpaceMakeEqual();
        }

        private void IncreaseHorzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.HorizSpaceIncrease();
        }

        private void DecreaseHorzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.HorizSpaceDecrease();
        }

        private void RemoveHorzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.HorizSpaceConcatenate();
        }

        private void MakeEqualVertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.VertSpaceMakeEqual();
        }

        private void IncreaseVertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.VertSpaceIncrease();
        }

        private void DecreaseVertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.VertSpaceDecrease();
        }

        private void RemoveVertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.VertSpaceConcatenate();
        }

        private void HorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.CenterHorizontally();
        }

        private void VerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.CenterVertically();
        }

        private void WidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.SizeToControlWidth();
        }

        private void HeightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.SizeToControlHeight();
        }

        private void BothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.SizeToControl();
        }

        private void SizeToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            designer.DesignerCommands.SizeToGrid();
        }

        private void UpdateEditorCommandsState()
        {
            IScriptEdit edit = ActiveEditor;

            bool enabled = edit != null;

            cutToolStripMenuItem.Enabled = enabled && edit.CanCut;
            copyToolStripMenuItem.Enabled = enabled && edit.CanCopy;
            pasteToolStripMenuItem.Enabled = enabled && edit.CanPaste;
            deleteToolStripMenuItem.Enabled = false;

            undoToolStripMenuItem.Enabled = enabled && edit.CanUndo;
            redoToolStripMenuItem.Enabled = enabled && edit.CanRedo;

            if (edit == null && ActiveFormDesigner != null)
                UpdateCommandsState();
        }

        private void UpdateCommandsState()
        {
            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            var commands = designer.DesignerCommands;

            this.undoToolStripMenuItem.Enabled = commands.CanUndo;
            this.redoToolStripMenuItem.Enabled = commands.CanRedo;
            this.deleteToolStripMenuItem.Enabled = commands.CanDelete;
            this.copyToolStripMenuItem.Enabled = commands.CanCopy;
            this.pasteToolStripMenuItem.Enabled = commands.CanPaste;
            this.cutToolStripMenuItem.Enabled = commands.CanCut;

            this.leftsToolStripMenuItem.Enabled = commands.CanAlignLeft;
            this.centresToolStripMenuItem.Enabled = commands.CanAlignHorizontalCenters;
            this.rightsToolStripMenuItem.Enabled = commands.CanAlignRight;
            this.topsToolStripMenuItem.Enabled = commands.CanAlignTop;
            this.middlesToolStripMenuItem.Enabled = commands.CanAlignVerticalCenters;
            this.bottomsToolStripMenuItem.Enabled = commands.CanAlignBottom;
            this.toGridToolStripMenuItem.Enabled = commands.CanAlignToGrid;
            this.widthToolStripMenuItem.Enabled = commands.CanSizeToControlWidth;
            this.heightToolStripMenuItem.Enabled = commands.CanSizeToControlHeight;
            this.bothToolStripMenuItem.Enabled = commands.CanSizeToControl;
            this.sizeToGridToolStripMenuItem.Enabled = commands.CanSizeToGrid;
            this.makeEqualHorzToolStripMenuItem.Enabled = commands.CanHorizSpaceMakeEqual;
            this.increaseHorzToolStripMenuItem.Enabled = commands.CanHorizSpaceIncrease;
            this.decreaseHorzToolStripMenuItem.Enabled = commands.CanHorizSpaceDecrease;
            this.removeHorzToolStripMenuItem.Enabled = commands.CanHorizSpaceConcatenate;
            this.makeEqualVertToolStripMenuItem.Enabled = commands.CanVertSpaceMakeEqual;
            this.increaseVertToolStripMenuItem.Enabled = commands.CanVertSpaceIncrease;
            this.decreaseVertToolStripMenuItem.Enabled = commands.CanVertSpaceDecrease;
            this.removeVertToolStripMenuItem.Enabled = commands.CanVertSpaceConcatenate;
            this.horizontallyToolStripMenuItem.Enabled = commands.CanCenterHorizontally;
            this.verticallyToolStripMenuItem.Enabled = commands.CanCenterVertically;
            this.bringToFrontToolStripMenuItem.Enabled = commands.CanBringToFront;
            this.sendToBackToolStripMenuItem.Enabled = commands.CanSendToBack;
            this.lockControlsToolStripMenuItem.Enabled = commands.CanLockControls;

            this.lockControlsToolStripMenuItem.Text = commands.IsLockControlsChecked ? "Unlock Controls" : "Lock Controls";
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewForm();
        }

        private void CreateNewForm()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Python Files|*_Designer.py|All Files|*.*",
                FileName = FindUniqueName("Form", ".py"),
                InitialDirectory = GetTestFilesDirectoryPath(),
                Title = "Create New Form",
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            var userCodeFileName = saveFileDialog.FileName;

            var source = new FormDesignerDataSource(userCodeFileName, designerFileNameSuffix: "_Designer");
            FormFilesUtility.CreateFormFiles(source, FormFilesUtility.CreateFormFilesOptions.Python);

            OpenOrActivateCode(source.UserCodeFileName, source.UserCodeFileName);
            OpenOrActivateCode(source.UserCodeFileName, source.DesignerFileName);
            OpenDesigner(source.UserCodeFileName);

            filesUserControl.RefreshFiles();
        }

        private bool IsNameUnique(string baseName)
        {
            foreach (TabPage tabPage in codeTabPages.Values)
            {
                if (tabPage.Text.ToString().StartsWith("Code: " + baseName, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private string FindUniqueName(string baseName, string extesion)
        {
            int count = 1;
            string result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + Path.GetExtension(baseName);
            while (!IsNameUnique(result))
            {
                count++;
                result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + Path.GetExtension(baseName);
            }

            return result + extesion;
        }

        private void FilesUserControl_NewFormButtonClick(object sender, EventArgs e)
        {
            CreateNewForm();
        }

        private void TestCreateNewDesignerWithDefaultSource()
        {
            var designer = new FormDesignerControl
            {
                Dock = DockStyle.Fill,
            };

            var page = new TabPage("Design: ");
            contentTabControl.TabPages.Add(page);

            page.Controls.Add(designer);
            page.Tag = designer;
            contentTabControl.SelectTab(page);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Python Files|*_Designer.py|All Files|*.*",
                Multiselect = false,
                InitialDirectory = GetTestFilesDirectoryPath(),
            };

            var result = dialog.ShowDialog(this);

            if (result != DialogResult.OK)
                return;

            var designerFileName = dialog.FileName;

            if (!designerFileName.EndsWith("_Designer.py", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The file must have an extension _Designer.py");
                return;
            }

            var extension = Path.GetExtension(designerFileName);
            var userCodeFile = designerFileName.Replace("_Designer" + extension, extension);

            OpenAllFormFiles(userCodeFile);
        }

        private void OpenAllFormFiles(string userCodeFile)
        {
            var source = GetDesignerSource(userCodeFile);

            OpenOrActivateCode(source.UserCodeFileName, source.UserCodeFileName);
            OpenOrActivateCode(source.UserCodeFileName, source.DesignerFileName);
            OpenDesigner(source.UserCodeFileName);
        }

#pragma warning disable SA1314 // Type parameter names should begin with T
        private void RemoveAllByValue<K, V>(Dictionary<K, V> dictionary, V value)
#pragma warning restore SA1314 // Type parameter names should begin with T
        {
            foreach (var key in dictionary.Where(
                kvp => EqualityComparer<V>.Default.Equals(kvp.Value, value)).Select(x => x.Key).ToArray())
                dictionary.Remove(key);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = contentTabControl.SelectedTab;
            if (tab == null)
                return;

            if (!codeTabPages.ContainsValue(tab))
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                {
                    var formId = designer.Source.UserCodeFileName;
                    CloseDesigner(designer);
                    if (formDesigners.ContainsKey(tab))
                        formDesigners.Remove(tab);
                    if (!codeTabPages.Any(x => x.Key.Item1 == formId))
                        this.sourcesByFormId.Remove(formId);
                }
            }

            contentTabControl.TabPages.Remove(tab);
            RemoveAllByValue(codeTabPages, tab);
        }

        private void ResetToolboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolboxControl.Reset();
        }

        private void LoadToolboxFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            using (var fs = new FileStream(dialog.FileName, FileMode.Open))
            {
                try
                {
                    toolboxControl.Load(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void SaveToolboxToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            using (var fs = new FileStream(dialog.FileName, FileMode.Create))
                toolboxControl.Save(fs);
        }

        private void AddControlsFromAssemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFrom(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load assembly: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                toolboxControl.AddItemsFromAssembly("Additional Controls", assembly);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add toolbox items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ToolboxControl_PlaceItemAtDefaultLocation(object sender, PlaceToolboxItemAtDefaultLocationEventArgs e)
        {
            if (toolboxControl.FormDesignerControl == null)
                MessageBox.Show("Place toolbox item at default location: " + e.Item.DisplayName);
        }
    }
}