using GrafikaKomputerowa1.Shapes;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class ConstrainingTangentState : State
    {
        private Circle? selectedCircle;
        private Line? selectedLine;

        public ConstrainingTangentState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            var clickedVertex = Scene.GetClickedVertex(mousePosition);
            var clickedShape = Scene.GetShape(clickedVertex);
            var clickedLine = Scene.GetClickedLine(mousePosition);
            if(clickedShape is Circle circle && selectedCircle is null)
                selectedCircle = circle;
            else if (clickedLine is not null && selectedLine is null)
                selectedLine = clickedLine;
            else if(clickedShape is Line line && selectedLine is null)
                selectedLine = line;

            if (selectedCircle is not null && selectedLine is not null)
            {
                selectedCircle.ClearConstraints();
                selectedCircle.ConstrainedTangentLine = selectedLine;
                selectedLine.ClearConstraints();
                selectedLine.ConstrainedTangetCircle = selectedCircle;
                selectedCircle.Update();

                return DefaultState();
            }

            return this;
        }
    }
}
