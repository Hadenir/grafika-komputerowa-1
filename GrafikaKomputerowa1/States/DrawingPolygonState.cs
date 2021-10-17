using GrafikaKomputerowa1.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DrawingPolygonState : State
    {
        private List<Line> drawnSegments = new();

        public DrawingPolygonState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button == MouseButton.Right)
            {
                ClearSegmentsFromScene();

                return DefaultState();
            }

            if (button != MouseButton.Left) return this;

            if (drawnSegments.Count() == 0)
            {
                var newSegment = new Line(mousePosition.Clone(), mousePosition);
                drawnSegments.Add(newSegment);
                Scene.Shapes.Add(newSegment);
            }
            else
            {
                var clickedVertex = Scene.GetClickedVertex(mousePosition)!;
                var newSegment = new Line(clickedVertex, mousePosition);
                drawnSegments.Add(newSegment);
                Scene.Shapes.Add(newSegment);
            }

            return this;
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            if (drawnSegments.Count() > 0)
            {
                var lastVertex = drawnSegments.Last().End;
                lastVertex.Move(mousePosition);
            }

            return this;
        }

        protected override State OnKeyDown(Key key)
        {
            if (drawnSegments.Count() < 2 || key != Key.Escape) return this;

            {
                var lastSegment = drawnSegments.Last();
                drawnSegments.Remove(lastSegment);
                Scene.Shapes.Remove(lastSegment);
            }
            
            if(drawnSegments.Count() > 1)
            {
                ClearSegmentsFromScene();

                var firstSegment = drawnSegments.First();
                var lastSegment = drawnSegments.Last();
                var newSegment = new Line(lastSegment.End, firstSegment.Start);
                drawnSegments.Add(newSegment);

                var newPolygon = Polygon.WithSegments(drawnSegments);
                Scene.Shapes.Add(newPolygon);
            }

            return DefaultState();
        }

        private void ClearSegmentsFromScene()
        {
            foreach(var segment in drawnSegments)
                Scene.Shapes.Remove(segment);
        }
    }
}
