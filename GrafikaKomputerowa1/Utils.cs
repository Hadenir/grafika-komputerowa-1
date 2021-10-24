using GrafikaKomputerowa1.Shapes;
using System;

namespace GrafikaKomputerowa1
{
    public static class Utils
    {
        public static int DistanceToLineSqrd(Line line, Vertex point)
        {
            var px = line.End.X - line.Start.X;
            var py = line.End.Y - line.Start.Y;
            var norm = line.LengthSqrd;
            var u = ((point.X - line.Start.X) * px + (point.Y - line.Start.Y) * py) / (float)norm;
            u = Math.Clamp(u, 0, 1);
            var x = (int)(line.Start.X + u * px);
            var y = (int)(line.Start.Y + u * py);
            var dx = x - point.X;
            var dy = y - point.Y;

            return dx * dx + dy * dy;
        }
    }

}
