using GrafikaKomputerowa1.Shapes;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DraggingState : State
    {
        private Vertex? selectedVertex;
        private Shape? selectedShape;

        public DraggingState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            selectedVertex = Scene.GetClickedVertex(mousePosition);
            selectedShape = Scene.GetShape(selectedVertex);
            if (selectedShape is Circle circle)
            {
                if(circle.ConstrainedCenter)
                {
                    selectedVertex = null;
                    selectedShape = null;
                }
            }

            return this;
        }

        protected override State OnMouseUp(Vertex mousePosition, MouseButton button)
        {
            selectedVertex = null;
            selectedShape = null;
            return this;
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            selectedVertex?.Move(mousePosition);
            selectedShape?.Update();

            return this;
        }
    }
}
