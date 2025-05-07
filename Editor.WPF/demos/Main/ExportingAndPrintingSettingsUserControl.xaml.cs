#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Serialization;

using Microsoft.Win32;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class ExportingAndPrintingSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;
        private FlowDocumentExportFlags printingFlags;
        private bool printSelectionOnly;

        public ExportingAndPrintingSettingsUserControl()
        {
            InitializeComponent();
        }

        public TextEditor Editor
        {
            get
            {
                return editor;
            }

            set
            {
                if (editor == value)
                    return;

                editor = value;

                if (editor != null)
                    UpdateDataFromEditor();
            }
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
        }

        private void ExportToRtfButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".rtf";
            dialog.Filter = "Rich Text Files(*.rtf)|*.rtf";

            if (dialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                Editor.Source.SaveFile(dialog.FileName, new RtfExport());
            }
        }

        private void ExportToHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".html";
            dialog.Filter = "HTML Files(*.html)|*.html";

            if (dialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                Editor.Source.SaveFile(dialog.FileName, new HtmlExport());
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (editor.Printing.ExecutePrintDialog() == true)
                editor.Printing.Print();
        }

        private void PrintPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Printing.ExecutePrintPreviewDialog(this);
        }

        private void PrintOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Printing.ExecutePrintOptionsDialog();
        }

        private void ExportToXpsButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xps";
            dialog.Filter = "XPS Files(*.xps)|*.xps";

            if (dialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                var fd = Editor.Source.GetFlowDocument(
                    new FlowDocumentExportOptions { Flags = FlowDocumentExportFlags.None, Editor = editor });

                using (FileStream fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var writer = new XpsSerializerFactory().CreateSerializerWriter(fs);

                    fd.ColumnWidth = double.PositiveInfinity;
                    writer.Write(((IDocumentPaginatorSource)fd).DocumentPaginator);
                }
            }
        }

        private void UpdatePrintingSettings()
        {
            if (editor != null)
            {
                var flags = FlowDocumentExportFlags.None;
                editor.Printing.Options = PrintOptions.UseColors | PrintOptions.UseSyntax;
                if (IncludeLineNumbersCheckBox != null && IncludeLineNumbersCheckBox.IsChecked.Value)
                {
                    flags |= FlowDocumentExportFlags.IncludeLineNumbers;
                    editor.Printing.Options |= PrintOptions.LineNumbers;
                }

                if (IgnoreTextColorsCheckBox != null && IgnoreTextColorsCheckBox.IsChecked.Value)
                {
                    flags |= FlowDocumentExportFlags.IgnoreTextColors;
                    editor.Printing.Options = editor.Printing.Options & ~PrintOptions.UseColors;
                }

                printingFlags = flags;

                printSelectionOnly = PrintSelectionOnlyCheckBox != null && PrintSelectionOnlyCheckBox.IsChecked.Value;
                if (printSelectionOnly)
                    editor.Printing.Options |= PrintOptions.PrintSelection;
            }
        }

        private void IncludeLineNumbersCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdatePrintingSettings();
        }

        private void IgnoreTextColorsCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdatePrintingSettings();
        }

        private void PrintSelectionOnlyCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdatePrintingSettings();
        }

        private void UpdateDataFromEditor()
        {
            UpdatePrintingSettings();
        }
    }
}
