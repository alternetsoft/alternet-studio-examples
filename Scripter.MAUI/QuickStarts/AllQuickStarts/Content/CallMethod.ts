///<reference path="clr.d.ts" />


var initialized = false;
var arenaBackgroundBrush: any;
var dotBrush: any;

var currentAngle: number;
var radius: number;
var dotRadius: number;
var center: any;

var Int32T = host.type("System.Int32");

function ToInt(value: number) {
    return host.cast(Int32T, value);
}

function DegreesToRadians(degrees: any) {
    return (System.MathF.PI / 180) * degrees;
}

function InitializeIfNeeded(bounds: any) {
    if (initialized)
        return;

    arenaBackgroundBrush = Alternet.Drawing.Color.DarkBlue.AsBrush;
    dotBrush = Alternet.Drawing.Color.White.AsBrush;

    var maxSide = System.Math.Max(bounds.Width, bounds.Height);
    radius = (maxSide - (maxSide / 3)) / 2;
    dotRadius = maxSide / 20;

    center = bounds.Center;

    currentAngle = 0;

    initialized = true;
}

function OnPaint(g: any, bounds: any) {
    InitializeIfNeeded(bounds);

    var arenaBounds = bounds;
    arenaBounds.Inflate(-2, -2);
    g.FillEllipse(arenaBackgroundBrush, arenaBounds);

    var radians = DegreesToRadians(currentAngle);

    var dotCenter = new Alternet.Drawing.PointD(
        ToInt(center.X + (System.Math.Cos(radians) * radius)),
        ToInt(center.Y + (System.Math.Sin(radians) * radius)));

    g.FillEllipse(
        dotBrush,
        new Alternet.Drawing.RectD(ToInt(dotCenter.X - dotRadius), ToInt(dotCenter.Y - dotRadius), ToInt(dotRadius * 2), ToInt(dotRadius * 2)));
}

function ConstrainAngle(x: number): number {
    x %= 360;
    if (x < 0)
        x += 360;

    return x;
}

function OnUpdate(deltaTimeMs: number) {
    currentAngle += deltaTimeMs * 0.1;
    currentAngle = ConstrainAngle(currentAngle);
}
