using GrafikaKomputerowa1.Shapes;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DrawingPolygonState : State
    {
        private Shape? drawnShape;

        public DrawingPolygonState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (drawnShape is null)
            {
                if (button != MouseButton.Left) return this;

                drawnShape = new Line(mousePosition.Clone(), mousePosition);
                Scene.Shapes.Add(drawnShape);
            }
            else if (drawnShape is Line drawnLine)
            {
                if (button != MouseButton.Left) return DefaultState();

                Scene.Shapes.Remove(drawnShape);
                var startLine = new Line(mousePosition, drawnLine.Start);
                var endLine = new Line(drawnLine.End, mousePosition);
                drawnShape = Polygon.WithSegments(new[] { drawnLine, endLine, startLine });
                Scene.Shapes.Add(drawnShape);
            }
            else if (drawnShape is Polygon drawnPolygon)
            {
                if (button != MouseButton.Left) return DefaultState();

                var lastSegment = drawnPolygon.Segments.Last();
                var newVertex = lastSegment.Start.Clone();
                var newSegment = new Line(lastSegment.Start, newVertex);
                lastSegment.Start = newVertex;
                drawnPolygon.Segments.Insert(drawnPolygon.Segments.Count - 1, newSegment);
            }

            return this;
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            if (drawnShape is not null)
            {
                var lastVertex = drawnShape.GetVertices().Last();
                lastVertex.Move(mousePosition);
            }

            return this;
        }

        protected override State OnKeyDown(Key key)
        {
            if (key != Key.Escape) return this;

            if (drawnShape is not null)
                Scene.Shapes.Remove(drawnShape);

            return DefaultState();
        }
    }
}
