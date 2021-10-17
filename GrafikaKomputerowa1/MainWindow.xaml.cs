using GrafikaKomputerowa1.Shapes;
using GrafikaKomputerowa1.States;
using System;
using System.Windows;
using System.Windows.Input;

namespace GrafikaKomputerowa1
{
    public partial class MainWindow : Window
    {
        private readonly Canvas canvas = new();
        private readonly Scene scene = new();

        private State currentState;

        public MainWindow()
        {
            InitializeComponent();

            SwitchState<DraggingState>();

            scene.Shapes.Add(Line.Between(100, 100, 200, 150));
            scene.Shapes.Add(Circle.Create(300, 300, 64));
        }

        private void SwitchState<T>() where T : State
        {
            currentState = (State)Activator.CreateInstance(typeof(T), scene)!;
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

        private void CreatePolygonButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<DrawingPolygonState>();
        }

        private void CreateCircleButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<DrawingCircleState>();
        }

        private void RemoveShapeButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<RemovingShapeState>();
        }

        private void RemoveVertexButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<RemovingVertexState>();
        }

        private void SplitEdgeButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<SplittingEdgeState>();
        }

        private void ResizeCircleButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<ResizingCircleState>();
        }

        private void MoveEdgeButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<DraggingLineState>();
        }

        private void MoveShapeButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState<DraggingShapeState>();
        }
    }
}
