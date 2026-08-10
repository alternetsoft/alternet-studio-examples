using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using Alternet.Common;

namespace SkiaFastRendering
{
    /// <summary>
    /// Tracks the runtime duration of operations using a <see cref="Stopwatch"/>.
    /// </summary>
    public class DrawingTimeTracker
    {
        private Stopwatch watch;
        private long totalElapsed;
        private int count;

        /// <summary>
        /// Initializes static members of the <see cref="DrawingTimeTracker"/> class.
        /// </summary>
        static DrawingTimeTracker()
        {
            Items = new List<DrawingTimeTracker>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingTimeTracker"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the tracker.</param>
        public DrawingTimeTracker(string name)
        {
            Name = name;
            Items.Add(this);
        }

        /// <summary>
        /// Gets the list of all <see cref="DrawingTimeTracker"/> instances.
        /// </summary>
        public static List<DrawingTimeTracker> Items { get; }

        /// <summary>
        /// Gets the name of the tracker.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the total elapsed ticks recorded by this tracker.
        /// </summary>
        public long TotalElapsed => totalElapsed;

        /// <summary>
        /// Gets the number of times the tracker has been stopped.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Gets the average elapsed ticks per tracked operation.
        /// </summary>
        public int AverageElapsed => count == 0 ? 0 : (int)((double)totalElapsed / count);

        /// <summary>
        /// Logs values and their percentage relative to the first value.
        /// The first value is treated as 100%.
        /// </summary>
        /// <param name="values">Array of numeric values (e.g. times in ms).
        /// Must be non-null and length > 0.</param>
        /// <param name="titles">Array of titles corresponding to values.
        /// Must be same length as values.</param>
        /// <param name="unit">The unit of measurement (e.g. "ms").</param>
        public static string GetPercentRelativeToFirst(
            double[] values,
            string[] titles,
            string unit = null)
        {
            if (unit is not null)
                unit += " ";

            if (values == null) throw new ArgumentNullException(nameof(values));
            if (titles == null) throw new ArgumentNullException(nameof(titles));
            if (values.Length != titles.Length)
                throw new ArgumentException("values and titles must have the same length");
            if (values.Length == 0) return string.Empty;

            double baseline = values[0];
            bool baselineIsZero = Math.Abs(baseline) < double.Epsilon;

            // compute column widths for neat alignment
            int maxTitleLen = titles.Max(t => (t ?? string.Empty).Length);

            var sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                string title = titles[i] ?? $"#{i}";
                double val = values[i];

                // format numeric value with 3 fractional digits
                string valText = val.ToString("F3", CultureInfo.InvariantCulture);

                string pctText;
                if (i == 0)
                {
                    pctText = "100.0%";
                }
                else if (baselineIsZero)
                {
                    // cannot compute percentage when baseline is 0
                    pctText = "N/A";
                }
                else
                {
                    double pct = (val / baseline) * 100.0;
                    pctText = pct.ToString("F1", CultureInfo.InvariantCulture) + "%";
                }

                // example line: "Default    :   10.123 ms (100.0%)"
                sb.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0,-" + maxTitleLen + "} : {1,9} {2}({3})",
                    title,
                    valText,
                    unit,
                    pctText);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Logs the runtime statistics of all <see cref="DrawingTimeTracker"/>
        /// instances to the debug output.
        /// </summary>
        public static void Log(StringBuilder logWriter)
        {
            logWriter.AppendLine("RunTimeTracker Log:");

            foreach (var item in Items)
            {
                if (item.Count == 0)
                    continue;

                var elapsed = (int)(item.TotalElapsed / 10000);
                var average = (float)item.AverageElapsed / 10000;

                logWriter.AppendLine(
                    $"    {item.Name}: Total={elapsed}msec Count={item.Count} Average={average:F3}msec");
            }

            logWriter.AppendLine("End of RunTimeTracker Log");
        }

        /// <summary>
        /// Resets the state of the object, clearing any accumulated data and stopping
        /// any ongoing operations.
        /// </summary>
        /// <remarks>This method resets all internal counters and timers to their initial state.
        /// After calling this method, the object is in the same state as
        /// if it were newly created.</remarks>
        public void Reset()
        {
            totalElapsed = 0;
            count = 0;
            watch = null;
        }

        /// <summary>
        /// Starts the tracker. Returns <c>true</c> if started successfully; otherwise, <c>false</c>.
        /// </summary>
        /// <returns><c>true</c> if started; <c>false</c> if already started.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the tracker
        /// is already started and debug is attached.</exception>
        public bool Start()
        {
            if (watch != null)
            {
                if (Consts.IsDebugDefinedAndAttached)
                    throw new InvalidOperationException("Tracker is already started.");
                return false;
            }

            watch = System.Diagnostics.Stopwatch.StartNew();
            return true;
        }

        /// <summary>
        /// Stops the tracker. Returns <c>true</c> if stopped successfully; otherwise, <c>false</c>.
        /// </summary>
        /// <returns><c>true</c> if stopped; <c>false</c> if already stopped.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the tracker
        /// is already stopped and debug is attached.</exception>
        public bool Stop()
        {
            if (watch == null)
            {
                if (Consts.IsDebugDefinedAndAttached)
                    throw new InvalidOperationException("Tracker is already stopped.");
                return false;
            }

            watch.Stop();
            var elapsed = watch.ElapsedTicks;
            watch = null;
            totalElapsed += elapsed;
            count++;
            return true;
        }
    }
}
