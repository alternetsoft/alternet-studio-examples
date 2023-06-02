#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Drawing;
using System.Windows.Forms;

using Alternet.Common;

namespace Alternet.Editor
{
    class Test
    {
        void X()
        {
            int tttt = 0;

            tttt++;
            tttt--;
        }
    }

    #region GutterOptions
    /// <summary>
    /// Defines gutter appearance and behaviour displayed at the left size of the Edit control.
    /// This enumeration has a <c>FlagsAttribute</c> attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum GutterOptions
    {
        /// <summary>
        ///    Specifies that no flags are in effect.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Specifies that numbers of lines should be drawn.
        /// </summary>
        PaintLineNumbers = 0x1,

        /// <summary>
        /// Specifies that numbers of lines should be drawn at the gutter area rather than beyond the gutter.
        /// </summary>
        PaintLinesOnGutter = 0x2,

        /// <summary>
        /// Specifies that numbers of lines should be drawn beyond end of file.
        /// </summary>
        PaintLinesBeyondEof = 0x4,

        /// <summary>
        /// Specifies that bookmarks should be drawn.
        /// </summary>
        PaintBookMarks = 0x8,

        /// <summary>
        /// Specifies that line modificators (color stitch that indicates that the line content is modified, unmodified or saved) should be drawn.
        /// </summary>
        PaintLineModificators = 0x10,

        /// <summary>
        /// Specifies that user margin (allowing to draw additional information) should be drawn.
        /// </summary>
        PaintUserMargin = 0x20,

        /// <summary>
        /// Specifies that entire line should be selected while user holds "Shift" key and left clicks mouse on gutter.
        /// </summary>
        SelectLineOnClick = 0x40
    }

    #endregion

    #region IGutter
    /// <summary>
    /// Represents properties and methods to operate with gutter at the left side of the Edit control.
    /// </summary>
    public interface IGutter : IUpdate, IDisposable
    {
        /// <summary>
        /// When implemented by a class, occurs when the <c>IGutter</c> clicked.
        /// </summary>
        event EventHandler Click;

        /// <summary>
        /// When implemented by a class, occurs when the <c>IGutter</c> double-clicked.
        /// </summary>
        event EventHandler DoubleClick;

        /// <summary>
        /// When implemented by a class, occurs when user margin part of each line is drawing.
        /// </summary>
        event DrawUserMarginEvent DrawUserMargin;

        /// <summary>
        /// When implemented by a class, gets or sets the width of the gutter.
        /// </summary>
        int Width
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets display width of the gutter, including line numbers and outlining area.
        /// </summary>
        int DisplayWidth
        {
            get;
        }

        /// <summary>
        /// When implemented by a class, gets display area of the gutter, including line numbers and outlining area,
        /// but not including line modificators if they're painted outside gutter.
        /// </summary>
        int DisplayArea
        {
            get;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a Brush object used to paint gutter.
        /// </summary>
        Brush Brush
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a Pen object used to paint gutter line.
        /// </summary>
        Pen Pen
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets background color of the gutter.
        /// </summary>
        Color BrushColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets color of the gutter line.
        /// </summary>
        Color PenColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether the gutter area is visible.
        /// </summary>
        bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets the rectangle that represents gutter area.
        /// </summary>
        Rectangle Rect
        {
            get;
        }

        /// <summary>
        /// When implemented by a class, gets or sets index of the first line being painted on the gutter.
        /// </summary>
        int LineNumbersStart
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets line numbers indentation from the left gutter border.
        /// </summary>
        int LineNumbersLeftIndent
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets line numbers indentation from the right gutter border.
        /// </summary>
        int LineNumbersRightIndent
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets foreground color of the line numbers.
        /// </summary>
        Color LineNumbersForeColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets background color of the line numbers.
        /// </summary>
        Color LineNumbersBackColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets line numbers alignment information.
        /// </summary>
        StringAlignment LineNumbersAlignment
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets outlining indentation from the left gutter border.
        /// </summary>
        int OutliningLeftIndent
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets outlining indentation from the right gutter border.
        /// </summary>
        int OutliningRightIndent
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a <c>GutterOptions</c> that determine gutter appearance and behaviour.
        /// </summary>
        GutterOptions Options
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets an ImageList object that contains collection of images for gutter.
        /// </summary>
        ImageList Images
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value that specifies index of item in the image collection used to paint bookmark.
        /// </summary>
        int BookMarkImageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value that specifies index of item in the image collection used to paint special mark indicating the wrapped line.
        /// </summary>
        int WrapImageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether Edit control should draw triangle at bookmark position inside line.
        /// </summary>
        bool DrawLineBookmarks
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a color of the line bookmarks.
        /// </summary>
        Color LineBookmarksColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether Edit control should display text describing bookmark in form of tooltip window when mouse pointer is over the gutter bookmark.
        /// </summary>
        bool ShowBookmarkHints
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a color of the line modificators(color stitch that indicates that the line content is unmodified, modified or saved) in the modified state.
        /// </summary>
        Color LineModificatorChangedColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a color of the line modificators(color stitch that indicates that the line content is unmodified, modified or saved) in the saved state.
        /// </summary>
        Color LineModificatorSavedColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets an outline image back color..
        /// </summary>
        Color OutlineImageBackColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a background color of the highlighted outline area.
        /// </summary>
        Color HighlightOutlineAreaColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets the width of the user margin area.
        /// </summary>
        int UserMarginWidth
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets foreground color for the user margin.
        /// </summary>
        Color UserMarginForeColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets background color of the user margin.
        /// </summary>
        Color UserMarginBackColor
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets text of the user margin.
        /// </summary>
        string UserMarginText
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets maximum count of numbers in the line number.
        /// </summary>
        int MaxLineNumberLength
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, copies the contents from another <c>IGutter</c> object.
        /// </summary>
        /// <param name="source">Specifies <c>IGutter</c> to assign.</param>
        void Assign(IGutter source);

        /// <summary>
        /// When implemented by a class, draws gutter on the specified graphic surface.
        /// </summary>
        /// <param name="painter">Represents <c>IPainter</c> surface to draw on.</param>
        /// <param name="rect">Rectangular area of gutter to draw.</param>
        /// <param name="startLine">The first display line to draw.</param>
        void Paint(IPainter painter, Rectangle rect, int startLine);

        /// <summary>
        /// When implemented by a class, draws gutter on the specified graphic surface.
        /// </summary>
        /// <param name="painter">Represents <c>IPainter</c> surface to draw on.</param>
        /// <param name="rect">Rectangular area of gutter to draw.</param>
        void Paint(IPainter painter, Rectangle rect);

        /// <summary>
        /// When implemented by a class, fills hitTestInfo parameter by information about a part of the control at specified coordinate.
        /// </summary>
        /// <param name="x">Specifies horizontal coordinate of position to check.</param>
        /// <param name="y">Specifies vertical coordinate of position to check.</param>
        /// <param name="hitTestInfo">Retrieves information about part of control.</param>
        void GetHitTest(int x, int y, IHitTestInfo hitTestInfo);

        /// <summary>
        /// When implemented by a class, invalidates line number area if it needs to be altered.
        /// <param name="updateWidth">Specifies whether line number width needs to be recalculated.</param>
        /// </summary>
        bool InvalidateLineNumberArea(bool updateWidth);

        /// <summary>
        /// When implemented by a class, raises the <c>Click</c> event.
        /// </summary>
        /// <param name="e">The EventArgs that contains data to this event.</param>
        void OnClick(EventArgs e);

        /// <summary>
        /// When implemented by a class, raises the <c>DoubleClick</c> event.
        /// </summary>
        /// <param name="e">The EventArgs that contains data to this event.</param>
        void OnDoubleClick(EventArgs e);

        /// <summary>
        /// When implemented by a class, resets the <c>Width</c> to the default value.
        /// </summary>
        /// <summary>
        /// When implemented by a class, resets the <c>BrushColor</c> to the default value.
        /// </summary>
        void ResetBrushColor();

        /// <summary>
        /// When implemented by a class, resets the <c>PenColor</c> to the default value.
        /// </summary>
        void ResetPenColor();

        /// <summary>
        /// When implemented by a class, resets the <c>Visible</c> to the default value.
        /// </summary>
        void ResetVisible();

        /// <summary>
        /// When implemented by a class, resets the <c>LineNumbersStart</c> to the default value.
        /// </summary>
        void ResetLineNumbersStart();

        /// <summary>
        /// When implemented by a class, resets the <c>LineNumbersLeftIndent</c> to the default value.
        /// </summary>
        void ResetLineNumbersLeftIndent();

        /// <summary>
        /// When implemented by a class, resets the <c>LineNumbersRightIndent</c> to the default value.
        /// </summary>
        void ResetLineNumbersRightIndent();

        /// <summary>
        /// When implemented by a class, resets the <c>LineNumbersForeColor</c> to the default value.
        /// </summary>
        void ResetLineNumbersForeColor();

        /// <summary>
        /// When implemented by a class, resets the <c>LineNumbersBackColor</c> to the default value.
        /// </summary>
        void ResetLineNumbersBackColor();

        /// <summary>
        /// When implemented by a class, resets the <c>LineNumbersAlignment</c> to the default value.
        /// </summary>
        void ResetLineNumbersAlignment();

        /// <summary>
        /// When implemented by a class, resets the <c>OutliningLeftIndentt</c> to the default value.
        /// </summary>
        void ResetOutliningLeftIndent();

        /// <summary>
        /// When implemented by a class, resets the <c>OutliningRightIndent</c> to the default value.
        /// </summary>
        void ResetOutliningRightIndent();

        /// <summary>
        /// When implemented by a class, resets the <c>Options</c> to the default value.
        /// </summary>
        void ResetOptions();

        /// <summary>
        /// When implemented by a class, resets the <c>BookMarkImageIndex</c> to the default value.
        /// </summary>
        void ResetBookMarkImageIndex();

        /// <summary>
        /// When implemented by a class, resets the <c>WrapImageIndex</c> to the default value.
        /// </summary>
        void ResetWrapImageIndex();

        /// <summary>
        /// When implemented by a class, resets the <c>DrawLineBookmarks</c> to the default value.
        /// </summary>
        void ResetDrawLineBookmarks();

        /// <summary>
        /// When implemented by a class, resets the <c>LineBookmarksColor</c> to the default value.
        /// </summary>
        void ResetLineBookmarksColor();

        /// <summary>
        /// When implemented by a class, resets the <c>ShowBookmarkHints</c> to the default value.
        /// </summary>
        void ResetShowBookmarkHints();

        /// <summary>
        /// When implemented by a class, resets the <c>LineModificatorChangedColor</c> to the default value.
        /// </summary>
        void ResetLineModificatorChangedColor();

        /// <summary>
        /// When implemented by a class, resets the <c>LineModificatorSavedColor</c> to the default value.
        /// </summary>
        void ResetLineModificatorSavedColor();

        /// <summary>
        /// When implemented by a class, resets the <c>OutlineImageBackColor</c> to the default value.
        /// </summary>
        void ResetOutlineImageBackColor();

        /// <summary>
        /// When implemented by a class, resets the <c>HighlightOutlineAreaColor</c> to the default value.
        /// </summary>
        void ResetHighlightOutlineAreaColor();

        /// <summary>
        /// When implemented by a class, resets the <c>Width</c> to the default value.
        /// </summary>
        void ResetWidth();

        /// <summary>
        /// When implemented by a class, resets the <c>UserMarginWidth</c> to the default value.
        /// </summary>
        void ResetUserMarginWidth();

        /// <summary>
        /// When implemented by a class, resets the <c>UserMarginForeColor</c> to the default value.
        /// </summary>
        void ResetUserMarginForeColor();

        /// <summary>
        /// When implemented by a class, resets the <c>UserMarginBackColor</c> to the default value.
        /// </summary>
        void ResetUserMarginBackColor();

        /// <summary>
        /// When implemented by a class, resets the <c>UserMarginText</c> to the default value.
        /// </summary>
        void ResetUserMarginText();
    }

    #endregion IGutter
}
