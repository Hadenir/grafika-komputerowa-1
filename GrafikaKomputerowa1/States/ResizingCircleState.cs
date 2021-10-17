using GrafikaKomputerowa1.Shapes;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class ResizingCircleState : DrawingCircleState
    {
        public ResizingCircleState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;
            if (drawnCircle is not null) return DefaultState();

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            if (clickedVertex is null) return this;

            var circle = Scene.Shapes.Where(x => x is Circle).Cast<Circle>()
                .SingleOrDefault(x => x.GetVertices().Contains(clickedVertex));
            if(circle is null) return this;
            drawnCircle = circle;

            return this;
        }
    }
}
