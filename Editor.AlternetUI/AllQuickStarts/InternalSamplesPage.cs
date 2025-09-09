using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using Alternet.UI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Common;

namespace AllDemos
{
    public partial class InternalSamplesPage : PanelFormSelector
    {
        private static bool ExceptionsLogger = false;

        static InternalSamplesPage()
        {
            EditorTests.Init();
        }

        public InternalSamplesPage()
        {
            if (CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
                ExceptionsLogger = true;

            if (ExceptionsLogger)
            {
                DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
                {
                    if (e is FileNotFoundException)
                        return;

                    if (e is System.InvalidOperationException)
                        return;

                    if (e is ReflectionTypeLoadException)
                        return;

                    if (e is System.Xml.XmlException xmlException)
                    {
                        Debug.WriteLineIf(false, xmlException);
                        return;
                    }

                    Nop();
                });
                ExceptionsLogger = false;
            }
        }

        public bool IsNet471OrGreater
        {
            get
            {
                if (!Consts.IsWindows || !Consts.IsNetFramework)
                    return true;
#if NET471_OR_GREATER
                return true;
#else
                    return false;
#endif
            }
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

            /* Section: Syntax Parsers */

            AddGroup("Syntax Parsers");

            Add("Roslyn Syntax Parsing", () => {
                return new RoslynSyntaxParsing.Form1();
            });

            Add("Advanced Syntax Parsing", () =>
            {
                return new AdvancedSyntaxParsing.Form1();
            });

            if (IsNet471OrGreater)
            {
                Add("TextMate Parsing", () => {
                    return new TextMateParsing.Form1();
                });
            }

            Add("XAML Parsing", () => {
                return new XAMLParsing.Form1();
            });

            Add("SQL DOM Parser", () => {
                return new SQLDOMParser.Form1();
            });

            Add("PowerFx Parser", () => {
                return new PowerFxSyntaxParsing.Form1();
            });

            /* Section: Scripter */

            AddGroup("Scripter");

            Add("Call Method", () => {
                return new CallMethod.Form1();
            });

            Add("Object Reference", () => {
                return new ObjectReference.Form1();
            });

            /* Section: Scripter for Python */

            AddGroup("Scripter for Python");

            Add("Call Method for Python", () => {
                return new CallMethod.Python.Form1();
            });

            Add("Object Reference for Python", () => {
                return new ObjectReference.Python.Form1();
            });

            /* Section: Debugger */

            AddGroup("Debugger");

            Add("Debugger Integration C#", () => {
                return new DebuggerIntegration.Form1();
            });

            Add("Debugger Integration Python", () => {
                return new DebuggerIntegration.Python.Form1();
            });            
        }
    }
}