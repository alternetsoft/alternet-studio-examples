using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.FormDesigner;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private static readonly bool CallTestAction = true;

        private bool testEnvironmentPrepared;

        public static void ForEachComponent(Action<Type> action)
        {
            AssemblyUtilities.RunActionForDerivedTypes(typeof(Component), (type) =>
            {
                if (AssemblyUtilities.TypeIsDescendant(type, typeof(Form)))
                    return;
                action(type);
            });
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (Consts.IsDebugDefinedAndAttached)
            {
                if (e.Alt && e.Control && e.Shift && e.KeyCode == Keys.F12 && CallTestAction)
                {
                    TestCopyPasteControls();
                    e.Handled = true;
                }
            }
        }

        private void LogToOutput(string s)
        {
            outputControl.CustomLog(s + Environment.NewLine);
        }

        private void ClearComponentsOnTheForm()
        {
            var host = DesignerHost;
            if (host is null || host.Container is null)
                return;
            var tx = host.CreateTransaction();
            try
            {
                var components = host.Container.Components;
                for (int i = components.Count - 1; i >= 0; i--)
                {
                    if (components[i] is Form)
                        continue;
                    host.DestroyComponent(components[i]);
                }
            }
            finally
            {
                tx.Commit();
            }
        }

        private void DesignerSelectAll()
        {
            var designer = ActiveFormDesigner;
            designer?.DesignerCommands.SelectAll();
        }

        private void DesignerDelete()
        {
            var designer = ActiveFormDesigner;
            designer?.DesignerCommands.Delete();
        }

        private void DesignerPaste()
        {
            var designer = ActiveFormDesigner;
            designer?.DesignerCommands.Paste();
        }

        private void DesignerCopy()
        {
            var designer = ActiveFormDesigner;
            designer?.DesignerCommands.Copy();
        }

        private void DesignerCut()
        {
            var designer = ActiveFormDesigner;
            designer?.DesignerCommands.Cut();
        }

        [Conditional("DEBUG")]
        private void TestCopyPasteControl(Type type)
        {
            if (!AssemblyUtilities.HasConstructorNoParams(type))
                return;

            var host = DesignerHost;
            if (host is null || host.Container is null)
                return;

            LogToOutput("=====================");
            LogToOutput($"TestCopyPaste: {type.FullName}");

            using var tx = host.CreateTransaction();

            try
            {
                ClearComponentsOnTheForm();
                Application.DoEvents();
                DesignerSelectAll();
                DesignerDelete();
                Application.DoEvents();

                LogToOutput("Create");
                PlaceItemAtDefaultLocation(type);
                Application.DoEvents();

                LogToOutput("SelectAll");
                DesignerSelectAll();
                Application.DoEvents();

                LogToOutput("Cut");
                DesignerCut();
                Application.DoEvents();

                LogToOutput("Paste");
                DesignerPaste();
                Application.DoEvents();

                LogToOutput("Delete");
                DesignerSelectAll();
                DesignerDelete();
                Application.DoEvents();
            }
            catch (Exception e)
            {
                LogToOutput(e.ToString());
            }
            finally
            {
                tx.Commit();
            }
        }

        private void PrepareTestEnvironment()
        {
            if (testEnvironmentPrepared)
                return;
            testEnvironmentPrepared = true;
            XmlSerialization.LogSerializationError = (e) =>
            {
                LogToOutput("===============");
                LogToOutput($"XmlSerialization error:");
                LogToOutput(e.ToString());
                LogToOutput("===============");
            };
        }

        [Conditional("DEBUG")]
        private void TestCopyPasteControls()
        {
            string[] badTypes = new string[]
            {
                "DataGridView",
                "ToolStrip",
                "System.Diagnostics.Process",
                "DebugMenu",
                "PerformanceCounter",
                "DebugCodeEdit",
            };

            PrepareTestEnvironment();

            var host = DesignerHost;
            if (host is null || host.Container is null)
                return;

            int maxCount = 0;
            int currentIndex = 0;

            ForEachComponent((type) =>
            {
                if (StringExtensions.ContainsRange(type.FullName, badTypes))
                    return;

                if (maxCount != 0 && currentIndex >= maxCount)
                    return;
                currentIndex++;
                TestCopyPasteControl(type);
            });
        }
    }
}
