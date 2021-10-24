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
                var distanceSqrd = Utils.DistanceToLineSqrd(line, point);
                if (distanceSqrd < Epsilon * Epsilon && distanceSqrd < minDistanceSqrd)
                {
                    minDistanceSqrd = distanceSqrd;
                    closest = line;
                }
            }

            return closest;
        }

        public Shape? GetShape(Vertex? vertex) => vertex is null ? null : Shapes.Single(x => x.GetVertices().Contains(vertex));

        
    }
}
