using GrafikaKomputerowa1.Shapes;
using System;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DrawingCircleState : State
    {
        protected Circle? drawnCircle;

        public DrawingCircleState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button == MouseButton.Right)
            {
                if (drawnCircle is not null)
                    Scene.Shapes.Remove(drawnCircle);

                return DefaultState();
            }

            if (button != MouseButton.Left) return this;

            if (drawnCircle is null)
            {
                drawnCircle = new Circle(mousePosition, 0);
                Scene.Shapes.Add(drawnCircle);
                return this;
            }

            return DefaultState();
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            if (drawnCircle is not null)
            {
                var centerVertex = drawnCircle.Center;
                var radiusSqrd = new Line(centerVertex, mousePosition).LengthSqrd;
                var radius = (int)Math.Sqrt(radiusSqrd);
                drawnCircle.Radius = radius;
            }

            return this;
        }
    }
}
