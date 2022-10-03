using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AKG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static double angleX = 0;
        private static double angleY = 0;
        private static double angleZ = 0;
        private static double zoom = 1;
        private static double zoom_level = 0.1;
        private static float[] movment = { 0, 0, 0 };

        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
                TransformMatrix.width = (float)window.ActualWidth;
                TransformMatrix.height = (float)window.ActualHeight;

                Model.ReadFile("Sting-Sword-lowpoly.obj");

                VectorTransformation.UpdateViewPort();

                VectorTransformation.UpdateCameraBasicVectors();
                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);

                DrawModel();
            };
        }

        private void window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.I:
                    angleX += 0.1;
                    lbRotateX.Content = angleX.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.K:
                    angleX -= 0.1;
                    lbRotateX.Content = angleX.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.J:
                    angleY += 0.1;
                    lbRotateY.Content = angleY.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.L:
                    angleY -= 0.1;
                    lbRotateY.Content = angleY.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.U:
                    angleZ += 0.1;
                    lbRotateZ.Content = angleZ.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.O:
                    angleZ -= 0.1;
                    lbRotateZ.Content = angleZ.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.T:
                    zoom += zoom_level;
                    lbZoom.Content = zoom.ToString("#.#####");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.G:
                    zoom -= zoom_level;
                    lbZoom.Content = zoom.ToString("#.#####");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.W:
                    movment[0] += 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.S:
                    movment[0] -= 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.A:
                    movment[1] -= 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.D:
                    movment[1] += 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.E:
                    movment[2] += 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.Q:
                    movment[2] -= 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)zoom, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
            }
        }

        public void DrawModel()
        {
            canvas.Children.Clear();

            foreach (var vector in Model.listF)
            {
                canvas.Children.Add(DrawLine(Model.model[vector[0] - 1], Model.model[vector[3] - 1]));
                canvas.Children.Add(DrawLine(Model.model[vector[3] - 1], Model.model[vector[6] - 1]));
                canvas.Children.Add(DrawLine(Model.model[vector[6] - 1], Model.model[vector[9] - 1]));
                canvas.Children.Add(DrawLine(Model.model[vector[9] - 1], Model.model[vector[0] - 1]));
            }
        }

        /*
        private static void DrawLine(Canvas canvas, Vector3 v1, Vector3 v2)
        {
            float x1 = v1.X;
            float y1 = v1.Y;

            float x2 = v2.X;
            float y2 = v2.Y;

            int x_start = Convert.ToInt32(Math.Round(x1));
            int y_start = Convert.ToInt32(Math.Round(y1));

            int x_end = Convert.ToInt32(Math.Round(x2));
            int y_end = Convert.ToInt32(Math.Round(y2));

            int L = (x_end - x_start) > (y_end - y_start) ? (x_end - x_start + 1) : (y_end - y_start + 1);

            float x = x1;
            float y = y1;
            for (int i = 0; i < L; i++)
            {
                x += (x_end - x_start) / L;
                y += (y_end - y_start) / L;

                Line line = new Line();
                line.X1 = Convert.ToInt32(Math.Round(x));
                line.Y1 = Convert.ToInt32(Math.Round(y));
                line.X2 = Convert.ToInt32(Math.Round(x));
                line.Y2 = Convert.ToInt32(Math.Round(y));
                line.Stroke = Brushes.Beige;
                line.StrokeThickness = 1;

                canvas.Children.Add(line);
            }
        }
        */

        private Line DrawLine(Vector4 v1, Vector4 v2)
        {
            Line line = new Line();

            line.Stroke = Brushes.Beige;
            line.StrokeThickness = 1;

            line.X1 = v1.X * zoom + GetWindowCenterWidth();
            line.Y1 = v1.Y * zoom + GetWindowCenterHeight();
            line.X2 = v2.X * zoom + GetWindowCenterWidth();
            line.Y2 = v2.Y * zoom + GetWindowCenterHeight();

            return line;
        }

        private float GetWindowCenterWidth()
        {
            return TransformMatrix.width / 2;
        }

        private float GetWindowCenterHeight()
        {
            return TransformMatrix.height / 2;
        }
    }
}
