using GrafikaKomputerowa1.Shapes;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DraggingState : State
    {
        private Vertex? selectedVertex;

        public DraggingState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            selectedVertex = Scene.GetClickedVertex(mousePosition);

            return this;
        }

        protected override State OnMouseUp(Vertex mousePosition, MouseButton button)
        {
            selectedVertex = null;
            return this;
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            if (selectedVertex is null) return this;

            var shape = Scene.GetShape(selectedVertex);
            if (shape is null || shape is Circle circle && circle.ConstrainedCenter)
                return this;

            selectedVertex.Move(mousePosition);

            if (shape is Polygon polygon)
            {
                polygon.Segments.Single(x => x.Start == selectedVertex).Update();
                polygon.Segments.Single(x => x.End == selectedVertex).Update();
            }
            else
            {
                shape.Update();
            }

            return this;
        }
    }
}
