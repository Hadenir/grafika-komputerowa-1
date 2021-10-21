using GrafikaKomputerowa1.Shapes;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class ClearingConstraintsState : State
    {
        public ClearingConstraintsState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            var clickedLine = Scene.GetClickedLine(mousePosition);

            Shape clickedShape;
            if (clickedVertex is not null && Scene.GetShape(clickedVertex) is Circle circle)
                clickedShape = circle;
            else if (clickedLine is not null)
                clickedShape = clickedLine;
            else
                return this;

            clickedShape.ClearConstraints();

            return DefaultState();
        }
    }
}
