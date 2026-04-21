using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Editor.Wpf;
using Alternet.FormDesigner.Integration.Wpf;

namespace Alternet.FormDesigner.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SimpleFormDesignerWindow : Window
    {
        private ViewModel model;

        public SimpleFormDesignerWindow()
        {
            InitializeComponent();

            model = new ViewModel(this);
            this.DataContext = model;

            saveButton.Click += (o, e) =>
            {
                model.Save();
            };
        }

        public void OpenAllFormFiles(string xamlFile)
        {
            model.OpenAllFormFiles(xamlFile);
            laDescription.Content = System.IO.Path.GetFileName(xamlFile);
        }

        private class ViewModel : INotifyPropertyChanged
        {
            #region Private Fields
            private const string XamlExtension = ".xaml";

            private SimpleFormDesignerWindow window;
            private Dictionary<string, TabItem> codeOrXamlTabPages = new Dictionary<string, TabItem>();
            private Dictionary<string, TabItem> designerTabPages = new Dictionary<string, TabItem>();
            private Dictionary<string, EditorFormDesignerDataSource> sourcesByFormId = new Dictionary<string, EditorFormDesignerDataSource>();
            private HashSet<string> editedXamlFiles = new HashSet<string>();

            #endregion

            public ViewModel()
            {
            }

            public ViewModel(SimpleFormDesignerWindow window)
                : this()
            {
                window.documentsTabControl.SelectionChanged += DocumentsTabControl_SelectionChanged;
                this.window = window;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private IScriptEdit ActiveEditor
            {
                get
                {
                    var item = window.documentsTabControl.SelectedItem as TabItem;
                    if (item == null)
                        return null;

                    return item.Content as IScriptEdit;
                }
            }

            private IFormDesignerControl ActiveDesigner
            {
                get
                {
                    var item = window.documentsTabControl.SelectedItem as TabItem;
                    if (item == null)
                        return null;

                    return item.Content as IFormDesignerControl;
                }
            }

            public void Save()
            {
                try
                {
                    var source = GetActiveDesignerSource();
                    if (source == null)
                        return;

                    var designer = ActiveDesigner;
                    if (designer == null)
                        return;

                    if (SaveAllModifiedFiles())
                    {
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error saving form: " + e);
                }
            }

            public void OpenAllFormFiles(string xamlFileName)
            {
                var source = GetDesignerSource(xamlFileName);

                OpenCode(source.UserCodeFileName);
                OpenXaml(source.XamlFileName);
                OpenDesigner(source.XamlFileName);
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #region Private Methods

            private void DocumentsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var designer = ActiveDesigner;

                window.propertyGrid.FormDesigner = designer;
                window.toolboxControl.FormDesigner = designer;

                if (designer != null)
                    ReloadDesignerIfNeeded(ActiveDesigner);
            }

            private void ReloadDesignerIfNeeded(IFormDesignerControl designer)
            {
                try
                {
                    var xamlFileName = ((IFormDesignerDataSource)designer.Source).XamlFileName;
                    if (this.editedXamlFiles.Contains(xamlFileName))
                    {
                        designer.Reload();
                        editedXamlFiles.Remove(xamlFileName);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(window, e.Message, "Designer Loading Error");
                }
            }

            private EditorFormDesignerDataSource GetActiveDesignerSource()
            {
                EditorFormDesignerDataSource source = null;

                var activeDesigner = ActiveDesigner;
                if (activeDesigner != null)
                    source = (EditorFormDesignerDataSource)activeDesigner.Source;
                else
                {
                    var activeEditor = ActiveEditor;
                    if (activeEditor != null)
                    {
                        var fileName = activeEditor.FileName;

                        string xamlFileName = null;

                        if (fileName.EndsWith(XamlExtension, StringComparison.OrdinalIgnoreCase))
                            xamlFileName = fileName;
                        else
                        {
                            var trimmed = System.IO.Path.ChangeExtension(fileName, string.Empty).TrimEnd('.');
                            if (trimmed.EndsWith(XamlExtension, StringComparison.OrdinalIgnoreCase))
                                xamlFileName = trimmed;
                        }

                        if (xamlFileName != null)
                            source = GetDesignerSource(xamlFileName);
                    }
                }

                return source;
            }

            private bool SaveAllModifiedFiles()
            {
                foreach (TabItem item in window.documentsTabControl.Items)
                {
                    var content = item.Content;

                    var edit = content as IScriptEdit;
                    if (edit != null && edit.Modified)
                        edit.SaveFile(edit.FileName);

                    var designer = content as IFormDesignerControl;
                    if (designer != null)
                        SaveDesignerFiles(designer);
                }

                return true;
            }

            private void SaveDesignerFiles(IFormDesignerControl designer)
            {
                var source = (EditorFormDesignerDataSource)designer.Source;
                source.Save();
            }

            private void OpenCode(string codeFileName)
            {
                if (codeOrXamlTabPages.ContainsKey(codeFileName))
                {
                    window.documentsTabControl.SelectedItem = codeOrXamlTabPages[codeFileName];
                    return;
                }

                var editor = new ScriptCodeEdit();
                editor.InitSyntax();

                var source = GetDesignerSource(codeFileName.Replace(".cs", string.Empty).Replace(".vb", string.Empty));

                FormDesignerEditorHelpers.SetEditorSource(editor, codeFileName, source);
                editor.RegisterAssemblies(GetReferencedAssemblies());

                codeOrXamlTabPages[codeFileName] = AddDocumentTab(codeFileName, editor);
            }

            private void OpenXaml(string xamlFileName)
            {
                if (codeOrXamlTabPages.ContainsKey(xamlFileName))
                {
                    window.documentsTabControl.SelectedItem = codeOrXamlTabPages[xamlFileName];
                    return;
                }

                IScriptEdit editor;

                editor = new ScriptCodeEdit();
                var source = GetDesignerSource(xamlFileName);

                FormDesignerEditorHelpers.SetEditorSource(editor as ScriptCodeEdit, xamlFileName, source);

                editor.TextChanged += (o, e) =>
                {
                    if (window.documentsTabControl.SelectedContent == editor)
                        editedXamlFiles.Add(xamlFileName);
                };

                codeOrXamlTabPages[xamlFileName] = AddDocumentTab(xamlFileName, editor);
            }

            private TabItem AddDocumentTab(string filePath, object content)
            {
                var tabItem = new TabItem
                {
                    Content = content,
                    Header = System.IO.Path.GetFileName(filePath),
                };

                window.documentsTabControl.Items.Add(tabItem);
                window.documentsTabControl.SelectedItem = tabItem;

                return tabItem;
            }

            private void OpenDesigner(string xamlFileName)
            {
                if (designerTabPages.ContainsKey(xamlFileName))
                {
                    window.documentsTabControl.SelectedItem = designerTabPages[xamlFileName];
                    return;
                }

                var designer = new FormDesignerControl();
                designer.Source = GetDesignerSource(xamlFileName);
                designer.NavigateToUserMethodRequested += Designer_NavigateToUserMethodRequested;

                designer.IsSmartDiffCodeSerializationRequired = d => ((ITextSource)((EditorFormDesignerDataSource)d.Source).XamlTextSource).Edits.Any();
                designerTabPages[xamlFileName] = AddDocumentTab(xamlFileName, designer);
            }

            private string[] GetReferencedAssemblies()
            {
                return new string[] { "mscorlib", "System", "PresentationCore", "PresentationFramework", "System.Drawing", "WindowsBase", "Microsoft.VisualBasic" };
            }

            private void Designer_NavigateToUserMethodRequested(object sender, NavigateToUserMethodRequestedEventArgs e)
            {
                var designer = (FormDesignerControl)sender;

                if (designer != ActiveDesigner)
                    return;

                var source = (EditorFormDesignerDataSource)designer.Source;

                OpenCode(source.UserCodeFileName);
                SetCaretToMethod(source.UserCodeFileName, e.MethodName);

                window.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Input,
                    new Action(() =>
                    {
                        var editor = (Control)ActiveEditor;
                        if (editor != null && editor.Focusable)
                        {
                            editor.Focus();
                            Keyboard.Focus(editor);
                        }
                    }));
            }

            private void SetCaretToMethod(string userCodeFileName, string methodName)
            {
                string toFind;
                var cs = System.IO.Path.GetExtension(userCodeFileName) == ".cs";
                if (cs)
                    toFind = "void " + methodName;
                else
                    toFind = "Sub " + methodName;

                var editor = ActiveEditor;

                System.Drawing.Point oldPosition = editor.Position;

                editor.Position = new System.Drawing.Point();
                if (editor.Find(toFind))
                {
                    editor.MoveToLine(ActiveEditor.Position.Y + (cs ? 2 : 1));
                    editor.MoveLineEnd();
                }
                else
                    editor.Position = oldPosition;
            }

            private EditorFormDesignerDataSource GetDesignerSource(string xamlFileName)
            {
                EditorFormDesignerDataSource ds;
                if (!sourcesByFormId.TryGetValue(xamlFileName, out ds))
                {
                    var language = FormFilesUtility.FindUserCodeFile(xamlFileName);

                    ds = new EditorFormDesignerDataSource(
                        xamlFileName,
                        language,
                        fileName =>
                        {
                            var source = new FormDesignerTextSource();
                            source.LoadFile(fileName);
                            return source;
                        });

                    sourcesByFormId.Add(xamlFileName, ds);
                }

                return ds;
            }
            #endregion
        }
    }
}
