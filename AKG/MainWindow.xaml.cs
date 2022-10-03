﻿using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private static double scale = 0.0000001;
        private static double scale_level = 0.0000001;
        private static float[] movment = { 0, 0, 0 };

        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
                TransformMatrix.width = (float)img.ActualWidth;
                TransformMatrix.height = (float)img.ActualHeight;

                Model.ReadFile("..\\..\\..\\objects\\Groda.obj");

                VectorTransformation.UpdateViewPort();

                VectorTransformation.UpdateCameraBasicVectors();
                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);

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

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.K:
                    angleX -= 0.1;
                    lbRotateX.Content = angleX.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.J:
                    angleY += 0.1;
                    lbRotateY.Content = angleY.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.L:
                    angleY -= 0.1;
                    lbRotateY.Content = angleY.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.U:
                    angleZ += 0.1;
                    lbRotateZ.Content = angleZ.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.O:
                    angleZ -= 0.1;
                    lbRotateZ.Content = angleZ.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.T:
                    scale += scale_level;
                    lbscale.Content = scale.ToString("#.##########");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.G:
                    scale -= scale_level;
                    lbscale.Content = scale.ToString("#.##########");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.W:
                    movment[0] += 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.S:
                    movment[0] -= 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.A:
                    movment[1] -= 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.D:
                    movment[1] += 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.E:
                    movment[2] += 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.Q:
                    movment[2] -= 1;
                    lbPos.Content = movment[0].ToString() + ", " + movment[1].ToString() + ", " + movment[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
                    DrawModel();
                    break;
            }
        }

        public void DrawModel()
        {
            WriteableBitmap bitmap = BitmapFactory.New((int)TransformMatrix.width, (int)TransformMatrix.height);
            img.Source = bitmap;

            foreach (var vector in Model.listF)
            {
                DrawVector(bitmap, Model.model[vector[0] - 1], Model.model[vector[3] - 1]);
                DrawVector(bitmap, Model.model[vector[3] - 1], Model.model[vector[6] - 1]);
                DrawVector(bitmap, Model.model[vector[6] - 1], Model.model[vector[9] - 1]);
                DrawVector(bitmap, Model.model[vector[9] - 1], Model.model[vector[0] - 1]);
            }
        }

        private void DrawVector(WriteableBitmap bitmap, Vector4 v1, Vector4 v2)
        {
            using (bitmap.GetBitmapContext())
            {
                bitmap.DrawLine(Convert.ToInt32(v1.X), Convert.ToInt32(v1.Y), Convert.ToInt32(v2.X), Convert.ToInt32(v2.Y), Colors.White);
            }
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TransformMatrix.width = (float)img.ActualWidth;
            TransformMatrix.height = (float)img.ActualHeight;

            VectorTransformation.UpdateViewPort();
            VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movment[0], movment[1], movment[2]);
            DrawModel();
        }


        // Working, but slow
        //public void DrawModel()
        //{
        //    canvas.Children.Clear();

        //    foreach (var vector in Model.listF)
        //    {
        //        canvas.Children.Add(DrawLine(Model.model[vector[0] - 1], Model.model[vector[3] - 1]));
        //        canvas.Children.Add(DrawLine(Model.model[vector[3] - 1], Model.model[vector[6] - 1]));
        //        canvas.Children.Add(DrawLine(Model.model[vector[6] - 1], Model.model[vector[9] - 1]));
        //        canvas.Children.Add(DrawLine(Model.model[vector[9] - 1], Model.model[vector[0] - 1]));
        //    }
        //}


        // Attempt in fast drawing, failed one
        //public void DrawModel()
        //{
        //    WriteableBitmap bitmap = new((int)TransformMatrix.width, (int)TransformMatrix.height, 96, 96, PixelFormats.Bgra32, null);
        //    bitmap.Lock();

        //    foreach (var vector in Model.listF)
        //    {
        //        DrawVector(bitmap, Model.model[vector[0] - 1], Model.model[vector[3] - 1]);
        //        DrawVector(bitmap, Model.model[vector[3] - 1], Model.model[vector[6] - 1]);
        //        DrawVector(bitmap, Model.model[vector[6] - 1], Model.model[vector[9] - 1]);
        //        DrawVector(bitmap, Model.model[vector[9] - 1], Model.model[vector[0] - 1]);
        //    }

        //    bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight)); 
        //    bitmap.Unlock();

        //    img.Source = bitmap;
        //}

        //public void DrawVector(WriteableBitmap bitmap, Vector4 v1, Vector4 v2)
        //{
        //    Vector4 v = v2 - v1;
        //    int num_of_points = 100;
        //    for (int i = 0; i < num_of_points; i++)
        //    {
        //        var smth = (v / num_of_points) * i;
        //        var a = smth.X;
        //        var b = smth.Y;
        //        lbRotateX.Content += a + " ";
        //        lbRotateY.Content += b + " ";
        //        //DrawPixel(bitmap, smth.X, smth.Y);
        //    }
        //}

        //public unsafe void DrawPixel(WriteableBitmap bitmap, byte x, byte y)
        //{
        //    byte* temp = (byte*)bitmap.BackBuffer + y * bitmap.BackBufferStride + x * bitmap.Format.BitsPerPixel / 8;

        //    temp[0] = 255;
        //    temp[1] = 255;
        //    temp[2] = 255;
        //    temp[3] = 255;
        //}


        // Drawing on canvas, very slow
        //private static void DrawLine(Canvas canvas, Vector3 v1, Vector3 v2)
        //{
        //    float x1 = v1.X;
        //    float y1 = v1.Y;

        //    float x2 = v2.X;
        //    float y2 = v2.Y;

        //    int x_start = Convert.ToInt32(Math.Round(x1));
        //    int y_start = Convert.ToInt32(Math.Round(y1));

        //    int x_end = Convert.ToInt32(Math.Round(x2));
        //    int y_end = Convert.ToInt32(Math.Round(y2));

        //    int L = (x_end - x_start) > (y_end - y_start) ? (x_end - x_start + 1) : (y_end - y_start + 1);

        //    float x = x1;
        //    float y = y1;
        //    for (int i = 0; i < L; i++)
        //    {
        //        x += (x_end - x_start) / L;
        //        y += (y_end - y_start) / L;

        //        Line line = new Line();
        //        line.X1 = Convert.ToInt32(Math.Round(x));
        //        line.Y1 = Convert.ToInt32(Math.Round(y));
        //        line.X2 = Convert.ToInt32(Math.Round(x));
        //        line.Y2 = Convert.ToInt32(Math.Round(y));
        //        line.Stroke = Brushes.Beige;
        //        line.StrokeThickness = 1;

        //        canvas.Children.Add(line);
        //    }
        //}


        // Depricated
        //private Line DrawLine(Vector4 v1, Vector4 v2)
        //{
        //    Line line = new Line();

        //    line.Stroke = Brushes.Beige;
        //    line.StrokeThickness = 1;

        //    line.X1 = v1.X * scale + GetWindowCenterWidth();
        //    line.Y1 = v1.Y * scale + GetWindowCenterHeight();
        //    line.X2 = v2.X * scale + GetWindowCenterWidth();
        //    line.Y2 = v2.Y * scale + GetWindowCenterHeight();

        //    return line;
        //}

        //private float GetWindowCenterWidth()
        //{
        //    return TransformMatrix.width / 2;
        //}

        //private float GetWindowCenterHeight()
        //{
        //    return TransformMatrix.height / 2;
        //}
    }
}
