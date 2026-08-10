#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Skia;
using Alternet.Syntax.Parsers.Roslyn;

namespace SkiaFastRendering
{
    public partial class Form1 : Form
    {
        private const string WordWrapDesc = "Wrap words to the beginning of the next line when necessary";
        private const string WrapAtMarginDesc = "Wrap words at the margin position.";
        private const string DefaultPath = @"Resources\Editor\text\spell.txt";
        private const string DefaultVBPath = @"Resources\Editor\text\spellvb.txt";

        private Alternet.Syntax.Parsers.Advanced.CsParser parserLight = new Alternet.Syntax.Parsers.Advanced.CsParser();
        private CsParser parser1 = new CsParser();
        private VbParser parser2 = new VbParser();
        private string dir = Application.StartupPath + @"\";

        static Form1()
        {
            SkiaSyntaxEdit.DrawDebugTextAtCorner = true;

            // Log UI thread exceptions
            Application.ThreadException += (sender, e) =>
            {
                Debug.WriteLine($"Unhandled UI thread exception: {e.Exception}");
            };

            // Log non-UI thread / domain exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                    Debug.WriteLine($"Unhandled domain exception: {ex}");
                else
                    Debug.WriteLine($"Unhandled domain exception (non-exception object): {e.ExceptionObject}");
            };

            // Log unobserved task exceptions and mark them as observed
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Debug.WriteLine($"Unobserved task exception: {e.Exception}");
                e.SetObserved();
            };

            GlobalEditorInitializer.Bind();

            SyntaxEdit.InstanceCreated += (s, e) =>
            {
                if (s is not SkiaSyntaxEdit edit)
                    return;

                var menu = edit.DefaultMenu;
                menu.Items.Add("-");

                var miDefaultPaint = new ToolStripMenuItem("Default Paint", null, (s, e) =>
                {
                    edit.RenderingMode = SkiaSyntaxEdit.RenderingModeKind.Default;
                });

                var miSkiaPaint = new ToolStripMenuItem("Skia Paint", null, (s, e) =>
                {
                    edit.RenderingMode = SkiaSyntaxEdit.RenderingModeKind.Skia;
                });

                var miOpenGLPaint = new ToolStripMenuItem("OpenGL Paint", null, (s, e) =>
                {
                    edit.RenderingMode = SkiaSyntaxEdit.RenderingModeKind.OpenGL;
                });

                var miClippedPaint = new ToolStripMenuItem("Clipped Paint", null, (s, e) =>
                {
                    edit.RenderingMode = SkiaSyntaxEdit.RenderingModeKind.DefaultClipped;
                });

                edit.DefaultMenu.Opening += (s, e) =>
                {
                    miDefaultPaint.Checked = edit.RenderingMode == SkiaSyntaxEdit.RenderingModeKind.Default;
                    miSkiaPaint.Checked = edit.RenderingMode == SkiaSyntaxEdit.RenderingModeKind.Skia;
                    miOpenGLPaint.Checked = edit.RenderingMode == SkiaSyntaxEdit.RenderingModeKind.OpenGL;
                    miClippedPaint.Checked = edit.RenderingMode == SkiaSyntaxEdit.RenderingModeKind.DefaultClipped;
                };

                menu.Items.Add(miDefaultPaint);
                menu.Items.Add(miSkiaPaint);
                menu.Items.Add(miOpenGLPaint);
                menu.Items.Add(miClippedPaint);
            };
        }

        /// <summary>
        /// Initializes a new instance of the Form1 class.
        /// </summary>
        public Form1()
        {
            Text = "Syntax Edit Painting via SkiaSharp and OpenGL";

            InitializeComponent();

            var asm = this.GetType().Assembly;
            var prefix = "SkiaFastRendering.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

            var pg = new PropertyGrid
            {
                Dock = DockStyle.Right,
                Width = 600,
                Visible = false,
            };

            this.Controls.Add(pg);

            syntaxEdit1.Enter += (s, e) =>
            {
            };

            syntaxEdit1.Leave += (s, e) =>
            {
                Text = string.Empty;
            };

            var menu = syntaxEdit1.DefaultMenu;

            menu.Items.Add("-");

            var miLightCSharp = new ToolStripMenuItem("Parser: C# Advanced", null, (s, e) =>
            {
                LoadAdvancedSyntax();
            });

            var miFullCSharp = new ToolStripMenuItem("Parser: C# Roslyn", null, (s, e) =>
            {
                LoadRoslynSyntax();
            });

            var miFullVB = new ToolStripMenuItem("Parser: VB Roslyn", null, (s, e) =>
            {
                LoadRoslynVBSyntax();
            });

            var miTrackSpeed = new ToolStripMenuItem("Track Speed", null, (s, e) =>
            {
                TrackSpeed();
            });

            var miShowProperties = new ToolStripMenuItem("Toggle Properties", null, (s, e) =>
            {
                pg.Visible = !pg.Visible;
                if (pg.Visible)
                {
                    pg.SelectedObject = syntaxEdit1;
                }
                else
                {
                    pg.SelectedObject = null;
                }
            });

            menu.Items.Add(miLightCSharp);
            menu.Items.Add(miFullCSharp);
            menu.Items.Add(miFullVB);
            menu.Items.Add("-");
            menu.Items.Add(miTrackSpeed);
            menu.Items.Add("-");
            menu.Items.Add(miShowProperties);

            cbRenderMode.Items.Add(SkiaSyntaxEdit.RenderingModeKind.Default);
            cbRenderMode.Items.Add(SkiaSyntaxEdit.RenderingModeKind.Skia);
            cbRenderMode.Items.Add(SkiaSyntaxEdit.RenderingModeKind.OpenGL);
            cbRenderMode.Items.Add(SkiaSyntaxEdit.RenderingModeKind.DefaultClipped);
            cbRenderMode.DropDownStyle = ComboBoxStyle.DropDownList;

            cbRenderMode.SelectedItem = syntaxEdit1.RenderingMode;

            cbRenderMode.SelectedIndexChanged += (s, e) =>
            {
                if (cbRenderMode.SelectedItem is SkiaSyntaxEdit.RenderingModeKind mode)
                    syntaxEdit1.RenderingMode = mode;
            };

            cbSyntax.Items.Add(new TextAndAction("C# Advanced", LoadAdvancedSyntax));
            cbSyntax.Items.Add(new TextAndAction("C# Roslyn", LoadRoslynSyntax));
            cbSyntax.Items.Add(new TextAndAction("VB Roslyn", LoadRoslynVBSyntax));
            cbSyntax.DropDownStyle = ComboBoxStyle.DropDownList;

            cbSyntax.SelectedItem = cbSyntax.Items[0];

            cbSyntax.SelectedIndexChanged += (s, e) =>
            {
                if (cbSyntax.SelectedItem is TextAndAction taa)
                    taa.Action();
            };
        }

        public void LoadFile(string fileName)
        {
            FileInfo fileInfo = new FileInfo(dir + fileName);
            if (!fileInfo.Exists)
            {
                dir = Path.Combine(Application.StartupPath, @"..\..\..\");
                fileInfo = new FileInfo(dir + fileName);

                if (!fileInfo.Exists)
                {
                    dir = Path.Combine(Application.StartupPath, @"..\..\..\..\..\..\");
                    fileInfo = new FileInfo(dir + fileName);
                }
            }

            if (fileInfo.Exists)
            {
                syntaxEdit1.LoadFile(fileInfo.FullName);
                syntaxEdit1.Source.FileName = fileInfo.FullName;
            }
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void WordWrapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.WordWrap = chbWordWrap.Checked;
        }

        /// <summary>
        /// Load event handler.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFile(DefaultPath);
            syntaxEdit1.Lexer = parserLight;

            syntaxEdit1.WordWrap = false;

            chbWordWrap.Checked = syntaxEdit1.WordWrap;
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">MouseEventArgs e</param>
        private void WordWrapCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = this.toolTip1.GetToolTip(this.chbWordWrap);
            if (str != WordWrapDesc)
            {
                toolTip1.SetToolTip(chbWordWrap, WordWrapDesc);
            }
        }

        private void TrackSpeed()
        {
            System.Drawing.Point? pt = null;
            syntaxEdit1.ShowSimpleHint("Tracking Painting Speed...", null, pt);

            StringBuilder log = new StringBuilder();

            DrawingTimeTrackers.TrackAllModesPainting(syntaxEdit1, 200, log);

            syntaxEdit1.HideSimpleHint();

            MessageBox.Show(log.ToString(), "Track Log", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void LoadAdvancedSyntax()
        {
            LoadFile(DefaultPath);
            syntaxEdit1.Lexer = parserLight;
        }

        private void LoadRoslynSyntax()
        {
            LoadFile(DefaultPath);
            syntaxEdit1.Lexer = parser1;
        }

        private void LoadRoslynVBSyntax()
        {
            LoadFile(DefaultVBPath);
            syntaxEdit1.Lexer = parser2;
        }

        private void BtnCountSpeed_Click(object sender, EventArgs e)
        {
            TrackSpeed();
        }

        private class TextAndAction
        {
            public string Text { get; set; }

            public Action Action { get; set; }

            public override string ToString() => Text;

            public TextAndAction(string text, Action action)
            {
                Text = text;
                Action = action;
            }
        }
    }
}
