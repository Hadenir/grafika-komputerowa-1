using GrafikaKomputerowa1.Shapes;
using System.Windows;
using System.Windows.Input;

namespace GrafikaKomputerowa1.States
{
    public abstract class State
    {
        protected Scene Scene { get; private set; }

        protected State(Scene scene)
        {
            Scene = scene;
        }

        public State Handle(MouseEventArgs args, Vertex mousePosition)
        {
            args.Handled = true;

            if (args is MouseButtonEventArgs buttonArgs)
            {
                return buttonArgs.ButtonState switch
                {
                    MouseButtonState.Pressed => OnMouseDown(mousePosition, buttonArgs.ChangedButton),
                    MouseButtonState.Released => OnMouseUp(mousePosition, buttonArgs.ChangedButton),
                    _ => this,
                };
            }

            return OnMouseMove(mousePosition);
        }

        public State Handle(KeyEventArgs args)
        {
            if (args.IsDown)
            {
                args.Handled = true;
                return OnKeyDown(args.Key);
            }

            return this;
        }

        protected State DefaultState() => new DraggingState(Scene);

        protected virtual State OnMouseDown(Vertex mousePosition, MouseButton button) => this;
        protected virtual State OnMouseUp(Vertex mousePosition, MouseButton button) => this;
        protected virtual State OnMouseMove(Vertex mousePosition) => this;
        protected virtual State OnKeyDown(Key key) => this;
    }
}
