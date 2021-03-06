using GrafikaKomputerowa1.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DraggingShapeState : State
    {
        private Shape? selectedShape;
        private List<Vertex>? offsets;

        public DraggingShapeState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;
            if (selectedShape is not null) return DefaultState();

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            selectedShape = Scene.GetShape(clickedVertex);
            offsets = selectedShape?.GetVertices().Select(x => x.Offset(mousePosition)).ToList();

            return this;
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            if (selectedShape is not null)
            {
                if (selectedShape is Circle circle && circle.ConstrainedCenter) return this;

                var vertices = selectedShape.GetVertices().ToList();
                for (int i = 0; i < vertices.Count(); i++)
                {
                    var offset = offsets![i];
                    var newPosition = new Vertex(mousePosition.X + offset.X, mousePosition.Y + offset.Y);
                    vertices[i].Move(newPosition);
                }

                selectedShape.Update();
                if (selectedShape is Polygon polygon) polygon.Segments.ForEach(x => x.Update());
            }

            return this;
        }
    }
}
