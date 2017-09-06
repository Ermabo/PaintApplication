using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shape shapeToDraw;
        private Point startingPoint;
        private Point currentPoint;
        private Brush strokeColor;
        private Brush fillColor;

        public MainWindow()
        {
            // Initializes the brushes with a default value to avoid null-crash.
            strokeColor = Brushes.White;
            fillColor = Brushes.White;
            InitializeComponent();
        }

        /// <summary>
        /// Initializes an ellipse/rectangle with all the necessary properties. Also adds it to the canvas.
        /// </summary>
        private void ShapeInit()
        {
            if (shapeToDraw != null)
            {
            shapeToDraw.Stroke = strokeColor;
            shapeToDraw.Fill = fillColor;

            Canvas.SetLeft(shapeToDraw, startingPoint.X);
            Canvas.SetTop(shapeToDraw, startingPoint.Y);

            DrawingSurface.Children.Add(shapeToDraw);
            }
        }

        /// <summary>
        /// Sets the shape to chosen size according to how you move the mouse.
        /// </summary>
        private void DragShape()
        {
            if (shapeToDraw != null)
            {
                // Makes sure that the end point is always bigger than the starting point. Otherwise we get a negative value on width.
                if (currentPoint.X >= startingPoint.X)
                {
                    shapeToDraw.Width = (currentPoint.X - startingPoint.X);
                }

                // Flips the end point with the start point if you move the mouse "behind" the start point to avoid crash due to negative value.
                else
                {
                    Canvas.SetLeft(shapeToDraw, currentPoint.X);
                    shapeToDraw.Width = (startingPoint.X - currentPoint.X);
                }

                // Same as the above if/else statement, but this one sets the height (Y) instead.
                if (currentPoint.Y >= startingPoint.Y)
                {
                    shapeToDraw.Height = (currentPoint.Y - startingPoint.Y);
                }

                else
                {
                    Canvas.SetTop(shapeToDraw, currentPoint.Y);
                    shapeToDraw.Height = (startingPoint.Y - currentPoint.Y);
                }
            }
        }

        private void btnStrokeColor_Click(object sender, RoutedEventArgs e)
        {
            // Sets the stroke brush to the same color as th background of the button that was lick.
            strokeColor = ((Button)sender).Background;
            grdSelectedStroke.Background = ((Button)sender).Background;
        }

        private void btnFillColor_Click(object sender, RoutedEventArgs e)
        {
            // Sets the fill brush to the same color as the background of the button that was clicked.
            fillColor = ((Button)sender).Background;
            grdSelectedFill.Background = ((Button)sender).Background;
        }

        private void DrawingSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                startingPoint = e.GetPosition(DrawingSurface);

                // Creates an ellipse and initializes it if the user checked the radio button for "Circle".
                if (rbtnCircle.IsChecked == true)
                {
                    shapeToDraw = new Ellipse();
                    ShapeInit();
                }

                // Creates a rectangle and initializes it if the user checked the radio button for "Square".
                else if (rbtnSquare.IsChecked == true)
                {
                    shapeToDraw = new Rectangle();
                    ShapeInit();
                }

                // Creates a line and adds it to the canvas if the user checked the radio button for "Line".
                else if (rbtnLine.IsChecked == true)
                {
                    shapeToDraw = new Line();
                    shapeToDraw.Stroke = strokeColor;

                    ((Line)shapeToDraw).X1 = startingPoint.X;
                    ((Line)shapeToDraw).Y1 = startingPoint.Y;

                    DrawingSurface.Children.Add(shapeToDraw);
                }

                // Creates a polyline and adds it to the canvas if the user checked the radio button for "Free".
                else if (rbtnFree.IsChecked == true)
                {
                    shapeToDraw = new Polyline();
                    shapeToDraw.Stroke = strokeColor;

                    ((Polyline)shapeToDraw).Points.Add(startingPoint); // Sets the starting point of the "Free line".

                    DrawingSurface.Children.Add(shapeToDraw);
                }
            }
        }

        private void DrawingSurface_MouseMove(object sender, MouseEventArgs e)
        {
            // We "draw"/resize the shape for as long as the left mouse buton is pressed down.
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(DrawingSurface);

                if (rbtnFree.IsChecked == true)
                {
                    // Adds the mouse cursor's current posistion to the "Free line", causing a line to be drawn where you move the mouse.
                    ((Polyline)shapeToDraw).Points.Add(currentPoint);
                }

                else if (rbtnLine.IsChecked == true)
                {
                    // Draws the end point for the line at the mouse cursor's posistion.
                    ((Line)shapeToDraw).X2 = currentPoint.X;
                    ((Line)shapeToDraw).Y2 = currentPoint.Y;
                }

                else
                    DragShape(); // We end up here if we have chosen to draw a Square/Circle.
            } 
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            // We end up here if the hit the "Clear" button, thus clearing the canvas of all the drawn objects.
            DrawingSurface.Children.Clear();
        }
    }
}
