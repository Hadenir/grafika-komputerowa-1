using GrafikaKomputerowa1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrafikaKomputerowa1
{
    public class Scene
    {
        private const int Epsilon = 10;
        public List<Shape> Shapes { get; } = new();
        public IEnumerable<Vertex> Vertices => Shapes.SelectMany(x => x.GetVertices());

        public Vertex? GetClickedVertex(Vertex point)
        {
            Vertex? closest = null;
            int minDistanceSqrd = int.MaxValue;
            foreach (var vertex in Vertices)
            {
                var distanceSqrd = new Line(point, vertex).LengthSqrd;
                if (distanceSqrd < Epsilon * Epsilon && distanceSqrd < minDistanceSqrd)
                {
                    minDistanceSqrd = distanceSqrd;
                    closest = vertex;
                }
            }

            return closest;
        }

        public Line? GetClickedLine(Vertex point)
        {
            Line? closest = null;
            int minDistanceSqrd = int.MaxValue;

            var lines = Shapes.Where(x => x is Line).Cast<Line>()
                .Concat(Shapes.Where(x => x is Polygon).Cast<Polygon>().SelectMany(x => x.Segments));

            foreach (var line in lines)
            {
                var distanceSqrd = DistanceToLineSqrd(line, point);
                if (distanceSqrd < Epsilon * Epsilon && distanceSqrd < minDistanceSqrd)
                {
                    minDistanceSqrd = distanceSqrd;
                    closest = line;
                }
            }

            return closest;
        }

        private int DistanceToLineSqrd(Line line, Vertex point)
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
