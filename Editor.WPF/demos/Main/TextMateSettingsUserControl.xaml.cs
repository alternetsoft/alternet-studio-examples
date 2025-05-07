#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Windows.Controls;
using Microsoft.CodeAnalysis.Editing;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class TextMateSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;
        private ViewModel model;

        public TextMateSettingsUserControl()
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
                if (editor == null)
                    return;
                if (model == null)
                {
                    model = new ViewModel(editor);
                    this.DataContext = model;
                }
                else
                {
                    model.RestoreEditor();
                }
            }
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
        }
    }
}
