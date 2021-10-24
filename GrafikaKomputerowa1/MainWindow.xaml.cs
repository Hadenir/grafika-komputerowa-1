using GrafikaKomputerowa1.Shapes;
using GrafikaKomputerowa1.States;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GrafikaKomputerowa1
{
    public partial class MainWindow : Window
    {
        private readonly Canvas canvas = new();
        private readonly Scene scene = new();

        private readonly IDictionary<object, Type> ButtonToStateMap;

        private State currentState;

        public MainWindow()
        {
            InitializeComponent();

            SwitchState<DraggingState>();

            scene.Shapes.Add(Line.Between(100, 100, 200, 150));
            scene.Shapes.Add(Circle.Create(300, 300, 64));


            ButtonToStateMap = new Dictionary<object, Type> {
                {CreatePolygonButton, typeof(DrawingPolygonState)},
                {CreateCircleButton, typeof(DrawingCircleState)},
                {RemoveShapeButton, typeof(RemovingShapeState)},
                {RemoveVertexButton, typeof(RemovingVertexState)},
                {SplitEdgeButton, typeof(SplittingEdgeState)},
                {ResizeCircleButton, typeof(ResizingCircleState)},
                {MoveEdgeButton, typeof(DraggingLineState)},
                {MoveShapeButton, typeof(DraggingShapeState)},
                {ConstrainCenterButton, typeof(ConstrainingCenterState)},
                {ConstrainRadiusButton, typeof(ConstrainingRadiusState)},
                {ConstrainTangentButton, typeof(ConstrainingTangentState)},
                {ConstrainLengthButton, typeof(ConstrainingLengthState)},
                {ClearConstraintsButton, typeof(ClearingConstraintsState)},
            };
        }

        private void SwitchState<T>() where T : State
        {
            currentState = (State)Activator.CreateInstance(typeof(T), scene)!;
        }

        private void SwitchState(Type T)
        {
            currentState = (State)Activator.CreateInstance(T, scene)!;
        }

        // ============================== UI Event Handlers ============================== 

        private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasImage.Source = canvas.RecreateBitmap(e.NewSize.Width, e.NewSize.Height);
            canvas.Render(scene.Shapes);
        }

        private void CanvasImage_MouseEvent(object sender, MouseEventArgs e)
        {
            var mousePosition = Vertex.From(e.GetPosition(CanvasImage));
            currentState = currentState.Handle(e, mousePosition);
            canvas.Render(scene.Shapes);
        }

        private void Window_KeyEvent(object sender, KeyEventArgs e)
        {
            currentState = currentState.Handle(e);
            canvas.Render(scene.Shapes);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (!ButtonToStateMap.TryGetValue(sender, out var newState))
                throw new InvalidOperationException($"Undefined state for button: {button.Content}");

            SwitchState(newState);
        }
    }
}
