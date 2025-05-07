using System;
using System.Diagnostics;
using Alternet.UI;
using Alternet.Drawing;

namespace ScriptSpace
{
    public class ScriptClass
    {
        static bool initialized;
        static SolidBrush arenaBackgroundBrush;
        static SolidBrush dotBrush;

        static double currentAngle;
        static double radius;
        static double dotRadius;
        static PointD center;

        static double DegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return radians;
        }

        static void InitializeIfNeeded(RectD bounds)
        {
            if (initialized)
                return;

            arenaBackgroundBrush = new SolidBrush(Color.DarkBlue);
            dotBrush = new SolidBrush(Color.White);

            double maxSide = Math.Max(bounds.Width, bounds.Height);
            radius = (maxSide - (maxSide / 3)) / 2;
            dotRadius = maxSide / 20;

            center = new PointD(
                bounds.Left + bounds.Width / 2,
                bounds.Top + bounds.Height / 2);

            initialized = true;
        }

        public static void OnPaint(Graphics g, RectD bounds)
        {
            InitializeIfNeeded(bounds);

            var arenaBounds = bounds;
            arenaBounds.Inflate(-2, -2);
            g.FillEllipse(arenaBackgroundBrush, arenaBounds);

            var radians = DegreesToRadians(currentAngle);

            var dotCenter = new PointD(
                center.X + (int)(Math.Cos(radians) * radius),
                center.Y + (int)(Math.Sin(radians) * radius));

            g.FillEllipse(
                dotBrush,
                new RectD(
                    new PointD(dotCenter.X - dotRadius, dotCenter.Y - dotRadius), 
                    new SizeD(new PointD(dotRadius * 2, dotRadius * 2)))
                );
        }

        static double ConstrainAngle(double x)
        {
            x %= 360;
            if (x < 0)
                x += 360;

            return x;
        }

        public static void OnUpdate(int deltaTimeMs)
        {
            currentAngle += deltaTimeMs * 0.1;
            currentAngle = ConstrainAngle(currentAngle);
            Debug.WriteLine("Current Angle: " + currentAngle);
        }
        public static void Main()
        {
        }
    }
}
