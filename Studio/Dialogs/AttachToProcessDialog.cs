using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;

namespace AlternetStudio.Demo
{
    public partial class AttachToProcessDialog : Form
    {
        private string watermark = "Filter processes...";
        private IOrderedEnumerable<Process> processes = null;
        private string filter = string.Empty;

        public AttachToProcessDialog()
        {
            InitializeComponent();
            DisplayScalingHelper.SetFont(this, new Font("Segoe UI", 9));

            ReloadProcesses();
            FilterProcessTextBox.ForeColor = SystemColors.GrayText;
            FilterProcessTextBox.Text = watermark;
            FilterProcessTextBox.Leave += FilterProcessTextBox_Leave;
            FilterProcessTextBox.Enter += FilterProcessTextBox_Enter;
            FilterProcessTextBox.TextChanged += FilterProcessTextBox_TextChanged;
        }

        public string Filter
        {
            get
            {
                return filter;
            }

            set
            {
                if (filter != value)
                {
                    filter = value;
                    OnFilterChanged();
                }
            }
        }

        public Process SelectedProcess { get; private set; }

        protected virtual void OnFilterChanged()
        {
            ReloadProcesses(false);
        }

        private void ReloadProcesses(bool forceReload = true)
        {
            try
            {
                if (forceReload || processes == null)
                {
                    processes = ProcessService.GetProcesses().OrderBy(x => x.ProcessName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while loading processes:\n" + e.ToString());
                return;
            }

            IOrderedEnumerable<Process> filteredProcesses = string.IsNullOrEmpty(filter) ? processes : processes.Where(x => x.ProcessName.Contains(filter, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.ProcessName);
            FillProcessesListView(filteredProcesses, forceReload);

            UpdateControls();
        }

        private void FillProcessesListView(IOrderedEnumerable<Process> processes, bool forceReload)
        {
            processesListView.BeginUpdate();
            try
            {
                processesListView.Items.Clear();
                SelectedProcess = null;

                foreach (var process in processes)
                {
                    processesListView.Items.Add(
                        new ListViewItem(
                            new[]
                            {
                            process.ProcessName,
                            process.Id.ToString(),
                            process.MainWindowTitle,
                            })
                        { Tag = process });
                }

                if (forceReload)
                {
                    processColumnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    idColumnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    titleColumnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            finally
            {
                processesListView.EndUpdate();
            }
        }

        private void AttachButton_Click(object sender, EventArgs e)
        {
            Accept();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ReloadProcesses();
        }

        private void ProcessesListView_DoubleClick(object sender, EventArgs e)
        {
            Accept();
        }

        private void Accept()
        {
            var validateResult = ProcessService.ValidateAttach(SelectedProcess);
            if (validateResult.IsFailure)
            {
                MessageBox.Show(validateResult.ErrorMessage + "\nPlease select a different process to attach.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ProcessesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedProcess = processesListView.SelectedIndices.Count == 0 ?
                null :
                (Process)processesListView.Items[processesListView.SelectedIndices[0]].Tag;

            UpdateControls();
        }

        private void UpdateControls()
        {
            attachButton.Enabled = SelectedProcess != null;
        }

        private void FilterProcessTextBox_Enter(object sender, EventArgs e)
        {
            if (string.Compare(FilterProcessTextBox.Text, watermark) == 0)
            {
                FilterProcessTextBox.Text = string.Empty;
                FilterProcessTextBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void FilterProcessTextBox_Leave(object sender, EventArgs e)
        {
            if (FilterProcessTextBox.Text.Length == 0)
            {
                FilterProcessTextBox.Text = watermark;
                FilterProcessTextBox.ForeColor = SystemColors.GrayText;
            }
        }

        private void FilterProcessTextBox_TextChanged(object sender, EventArgs e)
        {
            Filter = string.Compare(FilterProcessTextBox.Text, watermark) == 0 ? string.Empty : FilterProcessTextBox.Text;
        }

        private static class ProcessService
        {
            public static IEnumerable<Process> GetProcesses()
            {
                return Process.GetProcesses();
            }

            public static Result ValidateAttachBitness(Process process)
            {
                var targetIs64Bit = ProcessUtilities.Is64BitProcess(process);
                var thisProcessIs64Bit = ProcessUtilities.Is64BitProcess(Process.GetCurrentProcess());

                if (thisProcessIs64Bit != targetIs64Bit)
                    return Result.Fail($"The process has different bitness than the debugger process. The target process is {GetBitnessString(targetIs64Bit)}, but the debugger is {GetBitnessString(thisProcessIs64Bit)}.");

                return Result.Ok();
            }

            public static Result ValidateAttach(Process process)
            {
                if (process.Id == Process.GetCurrentProcess().Id)
                    return Result.Fail($"Cannot attach to the debugger process.");

                var bitnessResult = ValidateAttachBitness(process);
                if (bitnessResult.IsFailure)
                    return bitnessResult;

                if (!ProcessUtilities.IsProcessManaged(process))
                    return Result.Fail($"Process '{process.ProcessName}' is not managed(has no .NET CLR loaded)");

                return Result.Ok();
            }

            private static string GetBitnessString(bool is64Bit) => $"{(is64Bit ? "64" : "32")} bit";
        }
    }
}