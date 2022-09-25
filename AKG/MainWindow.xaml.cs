using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System;

namespace AKG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static double MaxDepth, MinDepth, focus;
        private static double angleX = 0;
        private static double angleY = 0;
        private static double angleZ = 0;
        private static double zoom = 1;

        public MainWindow()
        {
            InitializeComponent();

            TransformMatrix.width = 1920;
            TransformMatrix.height = 1080;

            Model.ReadFile("Sting-Sword-lowpoly.obj");

            VectorTransformation.UpdateCameraBasicVectors();
            VectorTransformation.InitVectors();

            DrawModel();
        }

        private void window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.W:
                    angleX += 0.1;
                    lbRotateX.Content = angleX;

                    TransformMatrix.RotateMatrixX.M22 = (float)Math.Cos(angleX);
                    TransformMatrix.RotateMatrixX.M23 = -(float)Math.Sin(angleX);
                    TransformMatrix.RotateMatrixX.M32 = (float)Math.Sin(angleX);
                    TransformMatrix.RotateMatrixX.M33 = (float)Math.Cos(angleX);

                    VectorTransformation.TransformVectorsRotateX();
                    break;
                case System.Windows.Input.Key.S:
                    angleX -= 0.1;
                    lbRotateX.Content = angleX;

                    TransformMatrix.RotateMatrixX.M22 = (float)Math.Cos(angleX);
                    TransformMatrix.RotateMatrixX.M23 = -(float)Math.Sin(angleX);
                    TransformMatrix.RotateMatrixX.M32 = (float)Math.Sin(angleX);
                    TransformMatrix.RotateMatrixX.M33 = (float)Math.Cos(angleX);

                    VectorTransformation.TransformVectorsRotateX();
                    break;
                case System.Windows.Input.Key.A:
                    angleY += 0.1;
                    lbRotateY.Content = angleY;

                    TransformMatrix.RotateMatrixY.M11 = (float)Math.Cos(angleY);
                    TransformMatrix.RotateMatrixY.M13 = (float)Math.Sin(angleY);
                    TransformMatrix.RotateMatrixY.M31 = -(float)Math.Sin(angleY);
                    TransformMatrix.RotateMatrixY.M33 = (float)Math.Cos(angleY);

                    VectorTransformation.TransformVectorsRotateY();
                    break;
                case System.Windows.Input.Key.D:
                    angleY -= 0.1;
                    lbRotateY.Content = angleY;

                    TransformMatrix.RotateMatrixY.M11 = (float)Math.Cos(angleY);
                    TransformMatrix.RotateMatrixY.M13 = (float)Math.Sin(angleY);
                    TransformMatrix.RotateMatrixY.M31 = -(float)Math.Sin(angleY);
                    TransformMatrix.RotateMatrixY.M33 = (float)Math.Cos(angleY);

                    VectorTransformation.TransformVectorsRotateY();
                    break;
                case System.Windows.Input.Key.Q:
                    angleZ += 0.1;
                    lbRotateZ.Content = angleZ;

                    TransformMatrix.RotateMatrixZ.M11 = (float)Math.Cos(angleZ);
                    TransformMatrix.RotateMatrixZ.M13 = (float)Math.Sin(angleZ);
                    TransformMatrix.RotateMatrixZ.M31 = -(float)Math.Sin(angleZ);
                    TransformMatrix.RotateMatrixZ.M33 = (float)Math.Cos(angleZ);

                    VectorTransformation.TransformVectorsRotateZ();
                    break;
                case System.Windows.Input.Key.E:
                    angleZ -= 0.1;
                    lbRotateZ.Content = angleZ;

                    TransformMatrix.RotateMatrixZ.M11 = (float)Math.Cos(angleZ);
                    TransformMatrix.RotateMatrixZ.M12 = -(float)Math.Sin(angleZ);
                    TransformMatrix.RotateMatrixZ.M21 = (float)Math.Sin(angleZ);
                    TransformMatrix.RotateMatrixZ.M22 = (float)Math.Cos(angleZ);

                    VectorTransformation.TransformVectorsRotateZ();
                    break;
                case System.Windows.Input.Key.I:
                    zoom += 0.1;
                    lbZoom.Content = zoom;

                    TransformMatrix.ScaleMatrix.M11 = (float)zoom;
                    TransformMatrix.ScaleMatrix.M22 = (float)zoom;
                    TransformMatrix.ScaleMatrix.M33 = (float)zoom;

                    VectorTransformation.TransformVectorsScale();
                    break;
                case System.Windows.Input.Key.K:
                    zoom -= 0.1;
                    lbZoom.Content = zoom;

                    TransformMatrix.ScaleMatrix.M11 = (float)zoom;
                    TransformMatrix.ScaleMatrix.M22 = (float)zoom;
                    TransformMatrix.ScaleMatrix.M33 = (float)zoom;

                    VectorTransformation.TransformVectorsScale();
                    break;
            }

            DrawModel();
        }

        public void DrawModel()
        {
            canvas.Children.Clear();
            FindMinMaxDepth();

            foreach (var vector in Model.listF)
            {
                canvas.Children.Add(DrawLine(Model.listV[vector[0] - 1], Model.listV[vector[3] - 1], false));
                canvas.Children.Add(DrawLine(Model.listV[vector[3] - 1], Model.listV[vector[6] - 1], false));
                canvas.Children.Add(DrawLine(Model.listV[vector[6] - 1], Model.listV[vector[0] - 1], false));

                //canvas.Children.Add(DrawLine(Model.listV[vector[0] - 1], Model.listV[vector[3] - 1]));
                //canvas.Children.Add(DrawLine(Model.listV[vector[3] - 1], Model.listV[vector[6] - 1]));
                //canvas.Children.Add(DrawLine(Model.listV[vector[6] - 1], Model.listV[vector[0] - 1]));
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



        private static Line DrawLine(Vector3 v1, Vector3 v2, bool perspective)
        {
            Line l = new Line();

            l.HorizontalAlignment = HorizontalAlignment.Left;
            l.VerticalAlignment = VerticalAlignment.Top;
            l.X1 = ConvertX(v1.X, v1.Y, TransformMatrix.width / 2, perspective);
            l.Y1 = ConvertY(v1.Z, v1.Y, TransformMatrix.height / 2, perspective);
            l.X2 = ConvertX(v2.X, v2.Y, TransformMatrix.width / 2, perspective);
            l.Y2 = ConvertY(v2.Z, v2.Y, TransformMatrix.height / 2, perspective);
            l.Stroke = Brushes.Beige;
            l.StrokeThickness = 1;

            return l;
        }

        private static double ConvertX(double x, double depth, double center, bool perspective)
        {
            double k;

            if (perspective)
            {
                k = (focus - depth) / (focus - MinDepth);
            }
            else k = 1;

            var buf = x * 0.0001 * k + center;
            return buf;
        }

        private static double ConvertY(double y, double depth, double center, bool perspective)
        {
            double k;

            if (perspective)
                k = (focus - depth) / (focus - MinDepth);
            else k = 1;

            var buf = -y * 0.0001 * k + center;
            return buf;
        }

        private static void FindMinMaxDepth()
        {
            MaxDepth = 0;
            MinDepth = 0;

            foreach (var item in Model.listF)
            {
                for (int i = 0; i < item.Length; i += 3)
                {
                    Vector3 v1 = Model.listV[item[i] - 1];
                    if (v1.Y < MinDepth)
                        MinDepth = v1.Y;
                    if (v1.Y > MaxDepth)
                        MaxDepth = v1.Y;
                }
            }
            focus = (MaxDepth - MinDepth) * 4;
        }

    }
}
