using GrafikaKomputerowa1.Shapes;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class RemovingVertexState : State
    {
        public RemovingVertexState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            if (clickedVertex is null) return this;

            var shapes = Scene.Shapes.Where(x => x.GetVertices().Contains(clickedVertex));
            if(shapes.Count() > 1)
            {
                foreach(var shape in shapes)
                    Scene.Shapes.Remove(shape);
            }
            else if(shapes.Count() == 1 && shapes.First() is Polygon polygon)
            {
                if (polygon.Segments.Count() <= 3) return this;

                var firstSegment = polygon.Segments.Single(x => x.End == clickedVertex);
                var secondSegment = polygon.Segments.Single(x => x.Start == clickedVertex);
                polygon.Segments.Remove(firstSegment);
                polygon.Segments.Remove(secondSegment);
                polygon.Segments.Add(new Line(firstSegment.Start, secondSegment.End));
            }

            return DefaultState();
        }
    }
}
