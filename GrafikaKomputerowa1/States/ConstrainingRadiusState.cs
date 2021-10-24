using GrafikaKomputerowa1.Shapes;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class ConstrainingRadiusState : State
    {
        public ConstrainingRadiusState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            var clickedShape = Scene.GetShape(clickedVertex);
            if (clickedShape is Circle circle)
            {
                circle.ConstrainedRadius = true;
                return DefaultState();
            }

            return this;
        }
    }
}
