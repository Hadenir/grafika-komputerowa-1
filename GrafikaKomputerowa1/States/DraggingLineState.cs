using GrafikaKomputerowa1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public class DraggingLineState : State
    {
        private Line? selectedLine;
        private Vertex? startOffset;
        private Vertex? endOffset;

        public DraggingLineState(Scene scene) : base(scene) { }

        protected override State OnMouseDown(Vertex mousePosition, MouseButton button)
        {
            if (button != MouseButton.Left) return this;
            if (selectedLine is not null) return DefaultState();

            var clickedLine = Scene.GetClickedLine(mousePosition);
            if (clickedLine is not null)
            {
                selectedLine = clickedLine;
                startOffset = clickedLine.Start.Offset(mousePosition);
                endOffset = clickedLine.End.Offset(mousePosition);
            }

            return this;
        }

        protected override State OnMouseMove(Vertex mousePosition)
        {
            if(selectedLine is not null)
            {
                var newStart = new Vertex(mousePosition.X + startOffset!.X, mousePosition.Y + startOffset.Y);
                var newEnd = new Vertex(mousePosition.X + endOffset!.X, mousePosition.Y + endOffset.Y);
                selectedLine.Start.Move(newStart);
                selectedLine.End.Move(newEnd);
            }

            return this;
        }
    }
}
