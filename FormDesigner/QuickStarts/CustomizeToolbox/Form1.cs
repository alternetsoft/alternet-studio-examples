#region Copyright (c) 2016-2022 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
#if !NET6_0
using System.Messaging;
#endif
using System.Reflection;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.FormDesigner.WinForms;

namespace CustomizeToolbox
{
    public partial class Form1 : Form
    {
        private const string AddDesc = "Open add assembly dialog";
        private const string CustomizeDesc = "Customize Toolbox content";
        private const string SaveDesc = "Save Toolbox content to file";
        private const string LoadDesc = "Load Toolbox content from file";

        private string[] assemblySearchPaths = { };
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string fileName = string.Empty;

        public Form1()
        {
            InitializeComponent();
            assemblySearchPaths = new string[] { FrameworkPath };
            toolboxControl.FormDesignerControl = formDesignerControl1;
            ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
        }

        private string FrameworkPath
        {
            get
            {
                return System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            }
        }

        public void FillWithCustomItems(bool useMetroIcons)
        {
            toolboxControl.BeginUpdate();
            try
            {
                foreach (var pair in ToolboxCustomItems.TypesByCategoryName)
                {
                    foreach (var type in pair.Value)
                    {
                        CustomToolboxImage customBitmap = null;
                        if (useMetroIcons)
                            customBitmap = TryLoadMetroIconForStandardItem(type.Name);

                        toolboxControl.AddItemForType(pair.Key, type, customBitmap);
                    }
                }
            }
            finally
            {
                toolboxControl.EndUpdate();
            }
        }

        private void AddAssembly(string asmName, string tabName)
        {
            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFrom(asmName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load assembly: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                toolboxControl.AddItemsFromAssembly(tabName, assembly);
                if (toolboxControl.DoesCategoryExist(tabName))
                    toolboxControl.SelectedCategory = tabName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add toolbox items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.InitialDirectory = Application.StartupPath;
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            AddAssembly(dialog.FileName, "Custom Controls");
        }

        private string ResolveAssemblyName(string name)
        {
            if (Path.IsPathRooted(name))
                return new FileInfo(name).Exists ? name : string.Empty;

            if (string.IsNullOrEmpty(baseDirectory) || string.IsNullOrWhiteSpace(baseDirectory))
                return string.Empty;
            string ext = Path.GetExtension(name);

            if (string.Compare(ext, ".dll", true) != 0 && string.Compare(ext, ".exe", true) != 0)
                name = name + ".dll";

            string path = Path.Combine(baseDirectory, name);
            if (new FileInfo(path).Exists)
                return path;

            foreach (string basePath in assemblySearchPaths)
            {
                if (string.IsNullOrEmpty(basePath) || string.IsNullOrWhiteSpace(basePath))
                    continue;
                path = Path.Combine(basePath, name);
                if (new FileInfo(path).Exists)
                    return path;
            }

            return string.Empty;
        }

        private CustomToolboxImage TryLoadMetroIconForStandardItem(string typeName)
        {
            var imageStream = typeof(ToolboxControl).Assembly.GetManifestResourceStream(
                string.Format(
                    "Alternet.FormDesigner.WinForms.Classes.Toolbox.MetroToolboxIcons.{0}.png",
                    typeName));

            var imageStreamHighDpi = typeof(ToolboxControl).Assembly.GetManifestResourceStream(
                string.Format(
                    "Alternet.FormDesigner.WinForms.Classes.Toolbox.MetroToolboxIcons.HighDpi.{0}_HighDpi.png",
                    typeName));

            if (imageStream == null && imageStreamHighDpi == null)
                return null;

            return new CustomToolboxImage(Image.FromStream(imageStream), Image.FromStream(imageStreamHighDpi));
        }

        private void RemoveCategory(string categoryName)
        {
            if (toolboxControl.DoesCategoryExist(categoryName))
                 toolboxControl.RemoveCategory(categoryName);
        }

        private void CustomizeButton_Click(object sender, EventArgs e)
        {
            toolboxControl.BeginUpdate();
            try
            {
                RemoveCategory("Common Controls");
                RemoveCategory("Text Controls");
                RemoveCategory("Advanced Controls");
                RemoveCategory("Containers");
                RemoveCategory("Menus & Toolbars");
                RemoveCategory("Data");
                RemoveCategory("Components");
                RemoveCategory("Printing");
                RemoveCategory("Dialogs");

                FillWithCustomItems(true);
            }
            finally
            {
                toolboxControl.EndUpdate();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                try
                {
                    toolboxControl.Save(fileStream);
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                try
                {
                    toolboxControl.Load(fileStream);
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private void AddButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btAdd);
            if (str != AddDesc)
                toolTip1.SetToolTip(btAdd, AddDesc);
        }

        private void CustomizeButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btCustomize);
            if (str != CustomizeDesc)
                toolTip1.SetToolTip(btCustomize, CustomizeDesc);
        }

        private void SaveButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btSave);
            if (str != SaveDesc)
                toolTip1.SetToolTip(btSave, SaveDesc);
        }

        private void LoadButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoad);
            if (str != LoadDesc)
                toolTip1.SetToolTip(btLoad, LoadDesc);
        }

        private static class ToolboxCustomItems
        {
            public static readonly Dictionary<string, List<Type>> TypesByCategoryName = new Dictionary<string, List<Type>>
            {
                {
                    "Common Controls",
                    new List<Type>(new[]
                    {
                        typeof(Button),
                        typeof(CheckBox),
                        typeof(RadioButton),
                    })
                },
                {
                    "Text Controls",
                    new List<Type>(new[]
                    {
                        typeof(Label),
                        typeof(LinkLabel),
                        typeof(TextBox),
                    })
                },
                {
                    "Advanced Controls",
                    new List<Type>(new[]
                    {
                        typeof(ColorDialog),
                        typeof(FolderBrowserDialog),
                        typeof(FontDialog),
                        typeof(OpenFileDialog),
                        typeof(SaveFileDialog),
                        typeof(FlowLayoutPanel),
                        typeof(GroupBox),
                        typeof(Panel),
                        typeof(SplitContainer),
                        typeof(TabControl),
                        typeof(TableLayoutPanel),
                        typeof(CheckedListBox),
                        typeof(ComboBox),
                        typeof(DateTimePicker),
                        typeof(ListBox),
                        typeof(ListView),
                        typeof(MaskedTextBox),
                        typeof(MonthCalendar),
                        typeof(NotifyIcon),
                        typeof(NumericUpDown),
                        typeof(PictureBox),
                        typeof(ProgressBar),
                        typeof(ContextMenuStrip),
                        typeof(MenuStrip),
                        typeof(StatusStrip),
                        typeof(ToolStrip),
                        typeof(ToolStripContainer),
                        typeof(RichTextBox),
                        typeof(ToolTip),
                        typeof(TreeView),
                        typeof(BindingNavigator),
                        typeof(BindingSource),
                        typeof(DataGridView),
                        typeof(DataSet),
                        typeof(BackgroundWorker),
                        typeof(ErrorProvider),
                        typeof(EventLog),
                        typeof(FileSystemWatcher),
                        typeof(HelpProvider),
                        typeof(ImageList),
#if !NET6_0
                        typeof(MessageQueue),
#endif
                        typeof(PerformanceCounter),
                        typeof(Process),
#if !NET6_0
                        typeof(SerialPort),
#endif
                        typeof(Timer),
                        typeof(PageSetupDialog),
                        typeof(PrintDialog),
                        typeof(PrintDocument),
                        typeof(PrintPreviewControl),
                        typeof(PrintPreviewDialog),
                        typeof(WebBrowser),
                    })
                },
            };
        }
    }
}