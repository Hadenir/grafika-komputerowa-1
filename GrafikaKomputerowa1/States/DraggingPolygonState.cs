using GrafikaKomputerowa1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DraggingPolygonState : State
    {
        private Polygon? selectedPolygon;
        private List<Vertex> offsets;

        public DraggingPolygonState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;
            if (selectedPolygon is not null) return DefaultState();

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            selectedPolygon = Scene.Shapes.Where(x => x is Polygon).Cast<Polygon>()
                .SingleOrDefault(x => x.GetVertices().Contains(clickedVertex));
            if (selectedPolygon is not null)
            {
                offsets = selectedPolygon.GetVertices().Select(x => x.Offset(mousePosition)).ToList();
            }

            return this;
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            if (selectedPolygon is not null)
            {
                var vertices = selectedPolygon.GetVertices().ToList();
                for(int i = 0; i < vertices.Count(); i++)
                {
                    var offset = offsets[i];
                    var newPosition = new Vertex(mousePosition.X + offset.X, mousePosition.Y + offset.Y);
                    vertices[i].Move(newPosition);
                }
            }

            return this;
        }
    }
}
