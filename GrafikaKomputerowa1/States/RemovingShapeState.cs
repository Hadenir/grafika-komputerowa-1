using GrafikaKomputerowa1.Shapes;
using System;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class RemovingShapeState : State
    {
        public RemovingShapeState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            if (clickedVertex != null)
            {
                var shapes = Scene.Shapes.Where(x => x.GetVertices().Contains(clickedVertex));
                Scene.Shapes.RemoveAll(shapes.Contains);

                return DefaultState();
            }

            return this;
        }
    }
}
