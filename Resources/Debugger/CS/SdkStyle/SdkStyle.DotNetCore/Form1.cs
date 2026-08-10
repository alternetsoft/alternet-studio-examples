using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "HelloWorld";

            pictureBox1.Image = LoadImageFromAssembly(asm, $"{prefix}.pictureBox1.Image.png");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private static Image LoadImageFromAssembly(Assembly assembly, string resourceName)
        {
            if (assembly == null || string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentException("Assembly or resource name cannot be null.");
            }

            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly.");
                }

                return Image.FromStream(resourceStream);
            }
        }
    }
}
