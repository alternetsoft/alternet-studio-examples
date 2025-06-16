Imports System
Imports System.Diagnostics
Imports Alternet.UI
Imports Alternet.Drawing

Namespace ScriptSpace
    Public Class ScriptClass
        Private Shared initialized As Boolean
        Private Shared arenaBackgroundBrush As SolidBrush
        Private Shared dotBrush As SolidBrush

        Private Shared currentAngle As Double
        Private Shared radius As Double
        Private Shared dotRadius As Double
        Private Shared center As PointD

        Private Shared Function DegreesToRadians(degrees As Double) As Double
            Dim radians As Double = (Math.PI / 180) * degrees
            Return radians
        End Function

        Private Shared Sub InitializeIfNeeded(bounds As RectD)
            If initialized Then
                Return
            End If

            arenaBackgroundBrush = New SolidBrush(Color.DarkBlue)
            dotBrush = New SolidBrush(Color.White)

            Dim maxSide As Double = Math.Max(bounds.Width, bounds.Height)
            radius = (maxSide - (maxSide / 3)) / 2
            dotRadius = maxSide / 20

            center = New PointD(
                bounds.Left + bounds.Width / 2,
                bounds.Top + bounds.Height / 2)

            initialized = True
        End Sub

        Public Shared Sub OnPaint(g As Graphics, bounds As RectD)
            InitializeIfNeeded(bounds)

            Dim arenaBounds As RectD = bounds
            arenaBounds.Inflate(-2, -2)
            g.FillEllipse(arenaBackgroundBrush, arenaBounds)

            Dim radians As Double = DegreesToRadians(currentAngle)

            Dim dotCenter As PointD = New PointD(
                center.X + CInt(Math.Cos(radians) * radius),
                center.Y + CInt(Math.Sin(radians) * radius))

            g.FillEllipse(
                dotBrush,
                New RectD(
                    New PointD(dotCenter.X - dotRadius, dotCenter.Y - dotRadius),
                    New SizeD(New PointD(dotRadius * 2, dotRadius * 2)))
                )
        End Sub

        Private Shared Function ConstrainAngle(x As Double) As Double
            x = x Mod 360
            If x < 0 Then
                x += 360
            End If

            Return x
        End Function

        Public Shared Sub OnUpdate(deltaTimeMs As Integer)
            currentAngle += deltaTimeMs * 0.1
            currentAngle = ConstrainAngle(currentAngle)
            Debug.WriteLine("Current Angle: " & currentAngle)
        End Sub

        Public Shared Sub Main()
        End Sub
    End Class
End Namespace