using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using Alternet.UI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.AlternetUI;
using Alternet.Common;
using Microsoft.CSharp.RuntimeBinder;

namespace AllDemos
{
    public partial class InternalSamplesPage : PanelFormSelector
    {
        private static bool ExceptionsLogger = DebugUtils.IsDebugOnWindows;

        static InternalSamplesPage()
        {
            EditorTests.Init();
            GlobalEditorInitializer.Bind();

            SyntaxEdit.InstanceCreated += (s, e) =>
            {
                if (s is not SyntaxEdit edit)
                    return;

            };
        }

        public InternalSamplesPage()
        {
            if (CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
                ExceptionsLogger = true;

            if (ExceptionsLogger)
            {
                DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
                {
                    if (e is OperationCanceledException)
                        return;

                    if (e is FileNotFoundException)
                        return;

                    if (e is System.InvalidOperationException)
                        return;

                    if (e is ReflectionTypeLoadException)
                        return;

                    if (e is System.Xml.XmlException xmlException)
                    {
                        LogUtils.DebugWriteLineIf(false, xmlException);
                        return;
                    }

                    if(e is RuntimeBinderException)
                    {
                        LogUtils.DebugWriteLineIf(false, e);
                        return;
                    }

                    LogUtils.DebugWriteLineIf(false, e);

                    Nop();
                });
                ExceptionsLogger = false;
            }

            View.RootItem.CollapseItems();
        }

        protected override void HandleOpenButtonClick(object? sender, EventArgs e)
        {
            base.HandleOpenButtonClick(sender, e);
        }

        protected override void AddDefaultItems()
        {
            /* Section: Text Editor */

            AddGroup("Text Editor");

            Add("Syntax Highlighting", () => new SyntaxHighlighting.Form1());
            Add("Word Wrap", () => new WordWrap.Form1());
            Add("Bookmarks", () => new Bookmarks.Form1());
            Add("Code Outlining", () => new CodeOutlining.Form1());
            Add("Gutter", () => new Gutter.Form1());
            Add("Hyper Text", () => new HyperText.Form1());
            Add("Line Styles", () => new LineStyles.Form1());
            Add("Customize", () => new Customize.Form1());
            Add("Margin", () => new Margin.Form1());
            Add("Miscellaneous", () => new Miscellaneous.Form1());
            Add("Selection", () => new Selection.Form1());
            Add("Visual Theme", () => new VisualTheme.Form1());
            Add("Search and Replace", () => new SearchReplace.Form1());
            
            Add("Code Completion", () => new CodeCompletion.Form1());
            Add("Code Snippets", () => new CodeSnippets.Form1());
            Add("Undo and Redo", () => new UndoRedo.Form1());
            Add("Selection Anchors", () => new SelectionAnchors.Form1());

            /* Section: Syntax Parsers */

            AddGroup("Syntax Parsers");

            Add("Roslyn Syntax Parsing", () => {
                return new RoslynSyntaxParsing.Form1();
            });

            Add("Advanced Syntax Parsing", () =>
            {
                return new AdvancedSyntaxParsing.Form1();
            });

            Add("TextMate Parsing", () => {
                return new TextMateParsing.Form1();
            });

            Add("XAML Parsing", () => {
                return new XAMLParsing.Form1();
            });

            Add("SQL DOM Parser", () => {
                return new SQLDOMParser.Form1();
            });

            Add("PowerFx Parser", () => {
                return new PowerFxSyntaxParsing.Form1();
            });

            Add("TypeScript Parser", () => {
                return new TypeScriptParsing.Form1();
            });

            /* Section: Scripter */

            AddGroup("Scripter for C# and Visual Basic");

            Add("Call Method", () => { return new CallMethod.Form1(); });
            Add("Object Reference", () => { return new ObjectReference.Form1(); });
            Add("Custom Assembly", () => { return new CustomAssembly.Form1(); });
            Add("Expression Evaluation", () => { return new ExpressionEvaluation.Form1(); });
            Add("Isolated Script", () => { return new IsolatedScript.Form1(); });
            Add("Threading", () => { return new Threading.Form1(); });

            Add("Memory Assembly", () => { return new MemoryAssembly.Form1(); });
            Add("Package Reference", () => { return new PackageReference.Form1(); });
            Add("Script Host Object", () => { return new ScriptHostObject.Form1(); });

            /* Section: Scripter for Python */

            AddGroup("Scripter for Python");

            Add("Call Method for Python", () => { return new CallMethod.Python.Form1(); });
            Add("Expression Evaluation for Python", () => { return new ExpressionEvaluation.Python.Form1(); });
            Add("Object Reference for Python", () => { return new ObjectReference.Python.Form1(); });
            Add("Threading for Python", () => { return new Threading.Python.Form1(); });

            Add("Custom Assembly for Python", () => { return new CustomAssembly.Python.Form1(); });
            Add("Memory Assembly for Python", () => { return new MemoryAssembly.Python.Form1(); });

            /* Section: Debugger */

            AddGroup("Debugger");

            Add("Debugger Integration C#", () => { return new DebuggerIntegration.Form1(); });
            Add("Debugger Integration Python", () => { return new DebuggerIntegration.Python.Form1(); });

            /* Section: Scripter for JavaScript and TypeScript */

            AddGroup("Scripter for JavaScript and TypeScript");

            Add("Mini scripts for JavaScript and TypeScript", () => { return new Mini.TypeScript.Form1(); });
            Add("Call Method for JavaScript and TypeScript", () => { return new CallMethod.TypeScript.Form1(); });
            Add("Object Reference for JavaScript and TypeScript", () => { return new ObjectReference.TypeScript.Form1(); });
        }
    }
}