using GrafikaKomputerowa1.Shapes;
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
            selectedVertex?.Move(mousePosition);

            return this;
        }
    }
}
