using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Alternet.Editor;

namespace SkiaFastRendering
{
    /// <summary>
    /// Provides well-known <see cref="DrawingTimeTracker"/> instances for tracking
    /// runtime durations of the operations.
    /// </summary>
    public static class DrawingTimeTrackers
    {
        /// <summary>
        /// Tracks the runtime duration of the clipped control painting operation.
        /// </summary>
        public static readonly DrawingTimeTracker ClippedPaint = new("Control.Paint.Clipped");

        /// <summary>
        /// Tracks the runtime duration of the default control painting operation.
        /// </summary>
        public static readonly DrawingTimeTracker DefaultPaint = new("Control.Paint.Default");

        /// <summary>
        /// Tracks the runtime duration of Skia-based control painting operations.
        /// </summary>
        public static readonly DrawingTimeTracker SkiaPaint = new("Control.Paint.Skia");

        /// <summary>
        /// Tracks the runtime duration of OpenGL-based control painting operations.
        /// </summary>
        public static readonly DrawingTimeTracker OpenGLPaint = new("Control.Paint.OpenGL");

        /// <summary>
        /// Gets the control currently being tracked for painting operations.
        /// </summary>
        public static System.Windows.Forms.Control TrackedControl { get; private set; }

        /// <summary>
        /// Tracks and logs the rendering performance of a
        /// control across all supported rendering modes.
        /// </summary>
        /// <remarks>This method evaluates the rendering performance of the specified
        /// <paramref name="control"/> in different rendering modes. For
        /// each mode, the method sets the rendering mode, processes pending application events,
        /// performs the rendering
        /// operation, and logs the results.</remarks>
        /// <param name="control">The control to be
        /// tested for rendering performance.</param>
        /// <param name="repeatCount">The number of times to repeat the rendering operation for each mode.
        /// Must be a non-negative integer.</param>
        /// <param name="log">The <see cref="StringBuilder"/> to which log messages will be written.</param>
        public static void TrackAllModesPainting(
            System.Windows.Forms.Control control,
            int repeatCount,
            StringBuilder log)
        {
            Prepare(control, SkiaSyntaxEdit.RenderingModeKind.Default);
            Prepare(control, SkiaSyntaxEdit.RenderingModeKind.Skia);
            Prepare(control, SkiaSyntaxEdit.RenderingModeKind.OpenGL);
            DrawingTimeTrackers.ResetPaintTrackers();
            TrackPainting(control, SkiaSyntaxEdit.RenderingModeKind.Default, repeatCount);
            TrackPainting(control, SkiaSyntaxEdit.RenderingModeKind.DefaultClipped, repeatCount);
            TrackPainting(control, SkiaSyntaxEdit.RenderingModeKind.Skia, repeatCount);
            TrackPainting(control, SkiaSyntaxEdit.RenderingModeKind.OpenGL, repeatCount);

            DrawingTimeTracker.Log(log);

            var defaultPaintTime = DrawingTimeTrackers.DefaultPaint.AverageElapsed;
            var skiaPaintTime = DrawingTimeTrackers.SkiaPaint.AverageElapsed;
            var openGLPaintTime = DrawingTimeTrackers.OpenGLPaint.AverageElapsed;
            var clippedPaintTime = DrawingTimeTrackers.ClippedPaint.AverageElapsed;

            var values = new double[] { clippedPaintTime, defaultPaintTime, skiaPaintTime, openGLPaintTime };
            var titles = new string[] { "Clipped", "Default", "Skia", "OpenGL" };

            var percentLog = DrawingTimeTracker.GetPercentRelativeToFirst(values, titles);
            log.AppendLine(percentLog);

            static void Prepare(
                System.Windows.Forms.Control c,
                SkiaSyntaxEdit.RenderingModeKind mode)
            {
                SetRenderingMode(c, mode);
                Application.DoEvents();
                c.Refresh();
                Application.DoEvents();
            }

            static void TrackPainting(
                System.Windows.Forms.Control c,
                SkiaSyntaxEdit.RenderingModeKind mode,
                int repeatCount)
            {
                SetRenderingMode(c, mode);
                Application.DoEvents();
                DrawingTimeTrackers.PaintStart(c, mode);
                DrawingTimeTrackers.TrackPainting(c, repeatCount);
                DrawingTimeTrackers.PaintStop(c, mode);
                Application.DoEvents();
            }

            static void SetRenderingMode(System.Windows.Forms.Control c, SkiaSyntaxEdit.RenderingModeKind mode)
            {
                if (c is not SkiaSyntaxEdit edit)
                    return;
                edit.RenderingMode = mode;
            }
        }

        /// <summary>
        /// Starts tracking the clipped paint operation for the specified control.
        /// </summary>
        /// <param name="control">The control to track.</param>
        public static void ClippedPaintStart(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            ClippedPaint.Start();
        }

        /// <summary>
        /// Stops tracking the clipped paint operation for the specified control.
        /// </summary>
        /// <param name="control">The control to stop tracking.</param>
        public static void ClippedPaintStop(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            ClippedPaint.Stop();
        }

        /// <summary>
        /// Starts tracking the default paint operation for the specified control.
        /// </summary>
        /// <param name="control">The control to track.</param>
        public static void DefaultPaintStart(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            DefaultPaint.Start();
        }

        /// <summary>
        /// Stops tracking the default paint operation for the specified control.
        /// </summary>
        /// <param name="control">The control to stop tracking.</param>
        public static void DefaultPaintStop(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            DefaultPaint.Stop();
        }

        /// <summary>
        /// Starts tracking the runtime duration of Skia-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control to track.</param>
        public static void SkiaPaintStart(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            SkiaPaint.Start();
        }

        /// <summary>
        /// Stops tracking the runtime duration of Skia-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control to stop tracking.</param>
        public static void SkiaPaintStop(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            SkiaPaint.Stop();
        }

        /// <summary>
        /// Starts tracking the runtime duration of OpenGL-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control to track.</param>
        public static void OpenGLPaintStart(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            OpenGLPaint.Start();
        }

        /// <summary>
        /// Stops tracking the runtime duration of OpenGL-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control to stop tracking.</param>
        public static void OpenGLPaintStop(System.Windows.Forms.Control control)
        {
            if (TrackedControl != control)
                return;
            OpenGLPaint.Stop();
        }

        /// <summary>
        /// Stops tracking the runtime duration of OpenGL-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control to stop tracking.</param>
        public static void PaintStart(System.Windows.Forms.Control control, SkiaSyntaxEdit.RenderingModeKind mode)
        {
            TrackedControl = control;
            switch (mode)
            {
                case SkiaSyntaxEdit.RenderingModeKind.DefaultClipped:
                    ClippedPaintStart(control);
                    break;
                case SkiaSyntaxEdit.RenderingModeKind.Default:
                    DefaultPaintStart(control);
                    break;
                case SkiaSyntaxEdit.RenderingModeKind.Skia:
                    SkiaPaintStart(control);
                    break;
                case SkiaSyntaxEdit.RenderingModeKind.OpenGL:
                    OpenGLPaintStart(control);
                    break;
            }
        }

        /// <summary>
        /// Stops tracking the runtime duration of OpenGL-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control to stop tracking.</param>
        public static void PaintStop(System.Windows.Forms.Control control, SkiaSyntaxEdit.RenderingModeKind mode)
        {
            if (TrackedControl != control)
                return;
            switch (mode)
            {
                case SkiaSyntaxEdit.RenderingModeKind.DefaultClipped:
                    ClippedPaintStop(control);
                    break;
                case SkiaSyntaxEdit.RenderingModeKind.Default:
                    DefaultPaintStop(control);
                    break;
                case SkiaSyntaxEdit.RenderingModeKind.Skia:
                    SkiaPaintStop(control);
                    break;
                case SkiaSyntaxEdit.RenderingModeKind.OpenGL:
                    OpenGLPaintStop(control);
                    break;
            }

            TrackedControl = null;
        }

        /// <summary>
        /// Resets the state of all paint trackers to their default values.
        /// </summary>
        /// <remarks>This method resets the internal state of the default paint tracker,
        /// Skia paint tracker, and OpenGL paint tracker.
        /// It is typically used to clear any accumulated state or prepare the
        /// trackers for reuse.</remarks>
        public static void ResetPaintTrackers()
        {
            DefaultPaint.Reset();
            SkiaPaint.Reset();
            OpenGLPaint.Reset();
        }

        /// <summary>
        /// Tracks the painting of a specified control by refreshing it a given number of times.
        /// </summary>
        /// <remarks>This method sets the specified control as the currently tracked control,
        /// refreshes it the specified number of times, and then clears the tracking state.
        /// During each refresh, the application
        /// processes pending events to ensure UI responsiveness.</remarks>
        /// <param name="control">The <see cref="UserControl"/> to be tracked and refreshed.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="repeatCount">The number of times the control should be refreshed.
        /// Must be a non-negative integer.</param>
        public static void TrackPainting(
            System.Windows.Forms.Control control,
            int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                control.Refresh();
                Application.DoEvents();
            }
        }
    }
}
