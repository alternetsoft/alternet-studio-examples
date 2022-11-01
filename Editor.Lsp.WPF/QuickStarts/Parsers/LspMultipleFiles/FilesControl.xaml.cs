using System;
using System.Collections.Generic;
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

namespace LspMultipleFiles
{
    /// <summary>
    /// Interaction logic for FilesControl.xaml
    /// </summary>
    public partial class FilesControl : UserControl
    {
        private string rootDirectory;

        public FilesControl()
        {
            InitializeComponent();
        }

        public event EventHandler<OpenFileRequestEventArgs> OpenFileRequested;

        public string[] SearchPatterns { get; set; } = new[] { "*.*" };

        public string RootDirectory
        {
            get
            {
                return rootDirectory;
            }

            set
            {
                if (rootDirectory == value)
                    return;

                rootDirectory = value;

                Reload();
            }
        }

        public void Reload()
        {
            FileControlTreeView.Items.Clear();

            if (rootDirectory == null)
                return;
            if (!Directory.Exists(rootDirectory))
                return;

            TreeViewItem workspaceNode = new TreeViewItem();
            workspaceNode.Name = "Workspace";
            workspaceNode.Header = "Workspace";
            FileControlTreeView.Items.Add(workspaceNode);

            foreach (var file in SearchPatterns.SelectMany(x => Directory.GetFiles(rootDirectory, x, SearchOption.AllDirectories)))
            {
                string fileName = System.IO.Path.GetFileName(file);
                TreeViewItem node = new TreeViewItem();
                node.Header = fileName;
                fileName = fileName.Replace(".", string.Empty).Replace(" ", string.Empty).Replace("+", string.Empty).Replace("*", string.Empty);
                if (fileName.Contains(","))
                    fileName = fileName.Substring(0, fileName.IndexOf(','));
                node.Name = fileName;

                node.Tag = file;
                workspaceNode.Items.Add(node);
            }

            workspaceNode.IsExpanded = true;
        }

        public IEnumerable<string> GetFiles() => (FileControlTreeView.Items[0] as TreeViewItem).Items.Cast<TreeViewItem>().Select(x => (string)x.Tag);

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }

                TreeViewItem item = sender as TreeViewItem;
                var fileName = item.Tag as string;
                if (fileName == null)
                    return;

                OpenFileRequested?.Invoke(this, new OpenFileRequestEventArgs(fileName));
            }
        }

        public class OpenFileRequestEventArgs : EventArgs
        {
            public OpenFileRequestEventArgs(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; }
        }
    }
}
