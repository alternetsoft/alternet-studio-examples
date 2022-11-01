﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Common;

namespace AlternetStudio.Wpf
{
    /// <summary>
    /// Interaction logic for AttachToProcessDialog.xaml
    /// </summary>
    public partial class AttachToProcessDialog : Window
    {
        public AttachToProcessDialog()
        {
            InitializeComponent();

            ReloadProcesses();
        }

        public Process SelectedProcess { get; private set; }

        private void ReloadProcesses()
        {
            IOrderedEnumerable<Process> processes;
            try
            {
                processes = ProcessService.GetProcesses().OrderBy(x => x.ProcessName);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while loading processes:\n" + e.ToString());
                return;
            }

            processesListView.ItemsSource = processes.ToArray();

            UpdateControls();
        }

        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            Accept();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadProcesses();
        }

        private void Accept()
        {
            var validateResult = ProcessService.ValidateAttach(SelectedProcess);
            if (validateResult.IsFailure)
            {
                MessageBox.Show(validateResult.ErrorMessage + "\nPlease select a different process to attach.");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void ProcessesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Accept();
        }

        private void ProcessesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProcess = processesListView.SelectedItem as Process;
            UpdateControls();
        }

        private void UpdateControls()
        {
            attachButton.IsEnabled = SelectedProcess != null;
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