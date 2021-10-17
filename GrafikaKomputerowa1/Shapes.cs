using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GrafikaKomputerowa1.Shapes
{
    public class Vertex
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(Vertex other)
        {
            X = other.X;
            Y = other.Y;
        }

        public Vertex Offset(Vertex other) => new(X - other.X, Y - other.Y);

        public Vertex Clone() => new(X, Y);

        public static Vertex From(Point point) => new((int)point.X, (int)point.Y);
    }
    public interface Shape
    {
        public IEnumerable<Vertex> GetVertices();
    }

    public class Line : Shape
    {
        public Vertex Start { get; set; }
        public Vertex End { get; set; }

        public int LengthSqrd => (End.X - Start.X) * (End.X - Start.X) + (End.Y - Start.Y) * (End.Y - Start.Y);

        public Line(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }

        public IEnumerable<Vertex> GetVertices() => new[] { Start, End };

        public static Line Between(int x1, int y1, int x2, int y2) => new(new Vertex(x1, y1), new Vertex(x2, y2));
    }

    public class Circle : Shape
    {
        public Vertex Center { get; set; }
        public int Radius { get; set; }

        public Circle(Vertex center, int radius)
        {
            Center = center;
            Radius = radius;
        }

        public IEnumerable<Vertex> GetVertices() => new[] { Center };

        public static Circle Create(int x, int y, int r) => new(new Vertex(x, y), r);
    }

    public class Polygon : Shape
    {
        public List<Line> Segments { get; } = new();

        public IEnumerable<Vertex> GetVertices() => Segments.Select(x => x.Start);

        public static Polygon WithSegments(IEnumerable<Line> segments)
        {
            if (segments.First().Start != segments.Last().End) throw new ArgumentException("Ill-formed polygon!");

            var polygon = new Polygon();
            polygon.Segments.AddRange(segments);
            return polygon;
        }
    }
}
