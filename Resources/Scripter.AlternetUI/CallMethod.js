///<reference path="clr.d.ts" />

var initialized = false;
var arenaBackgroundBrush;
var dotBrush;

var currentAngle = 0;
var radius = 0;
var dotRadius = 0;
var center;
var Int32T = host.type("System.Int32");

function DegreesToRadians(degrees) {
    return (Math.PI / 180) * degrees;
}

function ToInt(value) {
    return host.cast(Int32T, value);
}

function InitializeIfNeeded(bounds) {
    if (initialized)
        return;

    arenaBackgroundBrush = Alternet.Drawing.Color.DarkBlue.AsBrush;
    dotBrush = Alternet.Drawing.Color.White.AsBrush;

    var maxSide = System.Math.Max(bounds.Width, bounds.Height);
    radius = (maxSide - (maxSide / 3)) / 2;
    dotRadius = maxSide / 20;

    center = new Alternet.Drawing.PointD(
        ToInt(bounds.Left + bounds.Width / 2),
        ToInt(bounds.Top + bounds.Height / 2));

    currentAngle = 0;

    initialized = true;
}

function OnPaint(g, bounds) {
    InitializeIfNeeded(bounds);

    var arenaBounds = bounds;
    arenaBounds.Inflate(-2, -2);
    g.FillEllipse(arenaBackgroundBrush, arenaBounds);

    var radians = DegreesToRadians(currentAngle);



    var dotCenter = new Alternet.Drawing.PointD(
        center.X + ToInt(System.Math.Cos(radians) * radius),
        center.Y + ToInt(System.Math.Sin(radians) * radius));

    g.FillEllipse(
        dotBrush,
        new Alternet.Drawing.RectD(dotCenter.X - dotRadius, dotCenter.Y - dotRadius, dotRadius * 2, dotRadius * 2));
}

function ConstrainAngle(x) {
    x %= 360;
    if (x < 0)
        x += 360;

    return x;
}

function OnUpdate(deltaTimeMs) {
    currentAngle += deltaTimeMs * 0.1;
    currentAngle = ConstrainAngle(currentAngle);
}
