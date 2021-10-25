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


            var line1 = Line.Between(100, 50, 350, 150);
            var circle1 = Circle.Create(100, 150, 0);
            circle1.ConstrainedTangentLine = line1;
            line1.ConstrainedTangetCircle = circle1;
            circle1.Update();

            var line2 = new Line(line1.End, new Vertex(350, 50));
            var polygon1 = Polygon.WithSegments(new[]
            {
                line1,
                line2,
                new Line(line2.End, line1.Start),
            });

            var circle2 = Circle.Create(500, 500, 124);
            circle2.ConstrainedCenter = true;

            var line3 = Line.Between(300, 300, 300, 400);
            var line5 = Line.Between(400, 400, 400, 300);
            var line4 = new Line(line3.End, line5.Start);
            var line6 = new Line(line5.End, line3.Start);

            line3.ConstrainedLengthLine = line5;
            line5.ConstrainedLengthLine = line3;

            var polygon2 = Polygon.WithSegments(new[]
            {
                line3, line4, line5, line6
            });

            scene.Shapes.Add(circle1);
            scene.Shapes.Add(polygon1);
            scene.Shapes.Add(circle2);
            scene.Shapes.Add(polygon2);

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
