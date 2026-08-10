///<reference path="clr.d.ts" />

var initialized = false;
var arenaBackgroundBrush;
var dotBrush;

var currentAngle = 0;
var radius = 0;
var dotRadius = 0;
var center;
var Int32T = host.type("System.Int32");

function DegreesToRadiansJs(degrees) {
    return (System.MathF.PI / 180) * degrees;
}

function ToIntJs(value) {
    return host.cast(Int32T, value);
}

function InitializeIfNeededJs(bounds) {
    if (initialized)
        return;

    arenaBackgroundBrush = Alternet.Drawing.Color.DarkBlue.AsBrush;
    dotBrush = Alternet.Drawing.Color.White.AsBrush;

    var maxSide = System.Math.Max(bounds.Width, bounds.Height);
    radius = (maxSide - (maxSide / 3)) / 2;
    dotRadius = maxSide / 20;

    center = new Alternet.Drawing.PointD(
        ToIntJs(bounds.Left + bounds.Width / 2),
        ToIntJs(bounds.Top + bounds.Height / 2));

    currentAngle = 0;

    initialized = true;
}

function OnPaintJs(g, bounds) {
    InitializeIfNeededJs(bounds);

    var arenaBounds = bounds;
    arenaBounds.Inflate(-2, -2);
    g.FillEllipse(arenaBackgroundBrush, arenaBounds);

    var radians = DegreesToRadiansJs(currentAngle);

    var dotCenter = new Alternet.Drawing.PointD(
        center.X + ToIntJs(System.Math.Cos(radians) * radius),
        center.Y + ToIntJs(System.Math.Sin(radians) * radius));

    g.FillEllipse(
        dotBrush,
        new Alternet.Drawing.RectD(ToIntJs(dotCenter.X - dotRadius), ToIntJs(dotCenter.Y - dotRadius), ToIntJs(dotRadius * 2), ToIntJs(dotRadius * 2)));
}

function ConstrainAngleJs(x) {
    x %= 360;
    if (x < 0)
        x += 360;

    return x;
}

function OnUpdateJs(deltaTimeMs) {
    currentAngle += deltaTimeMs * 0.1;
    currentAngle = ConstrainAngleJs(currentAngle);
}
