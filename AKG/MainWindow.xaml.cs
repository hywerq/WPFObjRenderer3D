﻿using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

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
        private static double zoom = 10;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
                TransformMatrix.width = (float)window.ActualWidth;
                TransformMatrix.height = (float)window.ActualHeight;

                DrawModel();
            };

            Model.ReadFile("Sting-Sword-lowpoly.obj");

            VectorTransformation.UpdateCameraBasicVectors();
            VectorTransformation.InitVectors();
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
        }

        public void DrawModel()
        {
            canvas.Children.Clear();

            foreach (var vector in Model.listF)
            {
                canvas.Children.Add(DrawLine(Model.listV[vector[0] - 1], Model.listV[vector[3] - 1]));
                canvas.Children.Add(DrawLine(Model.listV[vector[3] - 1], Model.listV[vector[6] - 1]));
                canvas.Children.Add(DrawLine(Model.listV[vector[6] - 1], Model.listV[vector[0] - 1]));
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

        private Line DrawLine(Vector3 v1, Vector3 v2)
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