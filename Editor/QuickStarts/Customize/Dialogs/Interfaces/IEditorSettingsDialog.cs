#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Customize.Dialogs
{
    #region EditorSettingsTab

    /// <summary>
    /// Defines list of tabs for editor settings dialog.
    /// </summary>
    [Flags]
    public enum EditorSettingsTab
    {
        /// <summary>
        /// No tab.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// General tab.
        /// </summary>
        General = 0x1,

        /// <summary>
        /// Additional settings tab.
        /// </summary>
        Additional = 0x2,

        /// <summary>
        /// Fonts and Colors tab.
        /// </summary>
        FontsAndColors = 0x4,

        /// <summary>
        /// Keymapping tab.
        /// </summary>
        Keymapping = 0x8,
    }
    #endregion

    /// <summary>
    /// Represents properties and methods to change editor settings.
    /// </summary>
    public interface IEditorSettingsDialog
    {
        /// <summary>
        /// When implemented by a class, occurs when user requests help for a control.
        /// </summary>
        event HelpEventHandler HelpRequested;

        /// <summary>
        /// When implemented by a class, gets or sets object that implements <c>ISyntaxSettings</c> for this dialog.
        /// </summary>
        ISyntaxSettings SyntaxSettings
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a font for the dialog to use.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// When implemented by a class, runs a print options dialog box.
        /// </summary>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; otherwise, DialogResult.Cancel.</returns>
        DialogResult ShowDialog();

        /// <summary>
        /// When implemented by a class, runs a print options dialog box.
        /// </summary>
        /// <param name="owner">Any object that implements IWin32Window that represents the top-level window that will own the modal dialog box.</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; otherwise, DialogResult.Cancel.</returns>
        DialogResult ShowDialog(IWin32Window owner);

        /// <summary>
        /// When implemented by a class, initializes and runs a editor settings dialog box.
        /// </summary>
        /// <param name="hiddenTabs">specifies hidden tabs in the syntax settings dialog</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; otherwise, DialogResult.Cancel.</returns>
        DialogResult Execute(EditorSettingsTab hiddenTabs);

        /// <summary>
        /// When implemented by a class, initializes and runs a editor settings dialog box.
        /// </summary>
        /// <param name="hiddenTabs">specifies hidden tabs in the syntax settings dialog</param>
        /// <param name="owner">Any object that implements IWin32Window that represents the top-level window that will own the modal dialog box.</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; otherwise, DialogResult.Cancel.</returns>
        DialogResult Execute(EditorSettingsTab hiddenTabs, IWin32Window owner);
    }
}
