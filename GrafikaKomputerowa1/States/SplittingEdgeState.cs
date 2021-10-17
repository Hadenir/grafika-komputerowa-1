using GrafikaKomputerowa1.Shapes;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class SplittingEdgeState : State
    {
        public SplittingEdgeState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            var clickedEdge = Scene.GetClickedLine(mousePosition);
            if (clickedEdge is null) return this;

            var shape = Scene.Shapes.Where(x => x is Polygon).Cast<Polygon>()
                .SingleOrDefault(x => x.GetVertices().Contains(clickedEdge.Start));
            if(shape is Polygon polygon)
            {
                polygon.Segments.Remove(clickedEdge);
                polygon.Segments.Add(new Line(clickedEdge.Start, mousePosition));
                polygon.Segments.Add(new Line(mousePosition, clickedEdge.End));
            }

            return DefaultState();
        }
    }
}
