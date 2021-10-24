using GrafikaKomputerowa1.Shapes;
using System;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class ConstrainingLengthState : State
    {
        private Line? selectedLine;

        public ConstrainingLengthState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;

            var clickedLine = Scene.GetClickedLine(mousePosition);
            if (clickedLine is null || clickedLine == selectedLine) return this;

            if (selectedLine is null)
            {
                selectedLine = clickedLine;
                return this;
            }
            else
            {
                var line1 = selectedLine;
                var line2 = clickedLine;

                line1.ClearConstraints();
                line1.ConstrainedLengthLine = line2;
                line2.ClearConstraints();
                line2.ConstrainedLengthLine = line1;

                line1.Update();

                return DefaultState();
            }
        }
    }
}
