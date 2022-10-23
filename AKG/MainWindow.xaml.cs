using System;
using System.Collections.Generic;
using System.Linq;
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
        public static double angleX
        {
            get { return _angleX; }
            set
            {
                if (value != _angleX)
                {
                    _angleX = value;
                }
            }
        }
        public static double angleY
        {
            get { return _angleY; }
            set
            {
                if (value != _angleY)
                {
                    _angleY = value;
                }
            }
        }
        public static double angleZ
        {
            get { return _angleZ; }
            set
            {
                if (value != _angleZ)
                {
                    _angleZ = value;
                }
            }
        }
        public static double scale
        {
            get { return _scale; }
            set
            {
                if (value != _scale)
                {
                    _scale = value;
                }
            }
        }
        public static float[] movement
        {
            get { return _movement; }
            set
            {
                if (value != _movement)
                {
                    _movement = value;
                }
            }
        }

        private static double _angleX = -0.1;
        private static double _angleY = 2.5;
        private static double _angleZ = 0;
        private static double _scale = 0.00001;
        private static float[] _movement = { 0, -11, 0 };

        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
                VectorTransformation.width = (float)img.ActualWidth;
                VectorTransformation.height = (float)img.ActualHeight;

                Model.ReadFile("..\\..\\..\\objects\\hollow.obj");

                VectorTransformation.UpdateViewPort();

                VectorTransformation.UpdateCameraBasicVectors();
                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);

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

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.K:
                    angleX -= 0.1;
                    lbRotateX.Content = angleX.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.J:
                    angleY += 0.1;
                    lbRotateY.Content = angleY.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.L:
                    angleY -= 0.1;
                    lbRotateY.Content = angleY.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.U:
                    angleZ += 0.1;
                    lbRotateZ.Content = angleZ.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.O:
                    angleZ -= 0.1;
                    lbRotateZ.Content = angleZ.ToString("#.##");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.T:
                    scale += 0.00001;
                    lbscale.Content = scale.ToString("#.##########");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.G:
                    scale -= 0.00001;
                    lbscale.Content = scale.ToString("#.##########");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.D:
                    movement[0] += 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.A:
                    movement[0] -= 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.S:
                    movement[1] -= 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.W:
                    movement[1] += 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.E:
                    movement[2] += 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.Q:
                    movement[2] -= 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
            }
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            VectorTransformation.width = (float)img.ActualWidth;
            VectorTransformation.height = (float)img.ActualHeight;

            VectorTransformation.UpdateViewPort();
            VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
            DrawModel();
        }

        public void DrawModel()
        {
            if (VectorTransformation.width == 0)
            {
                VectorTransformation.width = 1;
            }
            if (VectorTransformation.height == 0)
            {
                VectorTransformation.height = 1;
            }

            WriteableBitmap bitmap = new((int)VectorTransformation.width, (int)VectorTransformation.height, 96, 96, PixelFormats.Bgra32, null);
            bitmap.Lock();

            foreach (var vector in Model.listF)
            {
                int i;
                for (i = 0; i < vector.Length / 3; i += 3)
                {
                    DrawVector(bitmap, Model.model[vector[i] - 1], Model.model[vector[i + 3] - 1]);
                }
                DrawVector(bitmap, Model.model[vector[i] - 1], Model.model[vector[0] - 1]);
            }

            Rasterization(bitmap);

            bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            bitmap.Unlock();

            img.Source = bitmap;
        }

        public void DrawVector(WriteableBitmap bitmap, Vector4 v1, Vector4 v2)
        {
            float x = v1.X;
            float y = v1.Y;

            float L = Math.Abs(v1.X - v2.X) > Math.Abs(v1.Y - v2.Y) ? Math.Abs(v1.X - v2.X) : Math.Abs(v1.Y - v2.Y);

            for (int i = 0; i < L; i++)
            {
                x += (v2.X - v1.X) / L;
                y += (v2.Y - v1.Y) / L;
                DrawPixel(bitmap, Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y)));
            }
        }

        private unsafe void DrawPixel(WriteableBitmap bitmap, int x, int y)
        {
            if (x > 0 && y > 0 && x < VectorTransformation.width && y < VectorTransformation.height)
            {
                byte* temp = (byte*)bitmap.BackBuffer + y * bitmap.BackBufferStride + x * bitmap.Format.BitsPerPixel / 8;

                temp[0] = 255;
                temp[1] = 255;
                temp[2] = 255;
                temp[3] = 255;
            }
        }

        private void Rasterization(WriteableBitmap bitmap)
        {
            int ch = 0;
            foreach (var vector in Model.listF)
            {
                List<Vector4> points = new List<Vector4>();
                for (int i = 0; i < vector.Length / 3; i += 3)
                {
                    points.Add(Model.model[vector[i] - 1]);
                    points.Add(Model.model[vector[i + 3] - 1]);
                }

                List<Vector4> uniq = new List<Vector4>();

                foreach (var val in points)
                {
                    if (!uniq.Any(c => c.X == val.X && c.Y == val.Y))
                    {
                        uniq.Add(val);
                    }
                }

                int minX = int.MaxValue;
                int minY = int.MinValue;
                int maxX = int.MinValue;
                int maxY = int.MaxValue;

                foreach (var val in uniq)
                {
                    minX = val.X < minX ? Convert.ToInt32(Math.Abs(val.X)) : minX;
                    maxX = val.X > maxX ? Convert.ToInt32(Math.Abs(val.X)) : maxX;
                    minY = val.Y < maxY ? Convert.ToInt32(Math.Abs(val.Y)) : minY;
                    maxY = val.Y > maxY ? Convert.ToInt32(Math.Abs(val.Y)) : maxY;
                }

                //Vector4[] start = { new Vector4(minX, minY, 0, 0), new Vector4(maxX, minY, 0, 0) };
                //Vector4[] end = { new Vector4(minX, maxY, 0, 0), new Vector4(maxX, maxY, 0, 0) };

                for (; minY < maxY; minY++)
                {
                    Vector4 scanlineStart = new Vector4(minX, minY, 0, 0);
                    Vector4 scanlineEnd = new Vector4(maxX, minY, 0, 0);

                    List<Vector4> line = new List<Vector4>();
                    int i;
                    for (i = 0; i < uniq.Count - 2; i++)
                    {
                        Vector4 point = Intersection(scanlineStart, scanlineEnd, uniq[i], uniq[i + 1]);
                        if (point.X != 0 && point.Y != 0 && point.Z != 0)
                        {
                            line.Add(point);
                        }
                    }
                    Vector4 pointer = Intersection(scanlineStart, scanlineEnd, uniq[i], uniq[0]);
                    if (pointer.X != 0 && pointer.Y != 0 && pointer.Z != 0)
                    {
                        line.Add(pointer);
                    }

                    if (line.Count > 2)
                    {
                        throw new Exception("a");
                    }

                    DrawVector(bitmap, line[0], line[1]);
                }
                Console.WriteLine(ch++.ToString());
            }
        }

        static public Vector4 Intersection(Vector4 A, Vector4 B, Vector4 C, Vector4 D)
        {
            double xo = A.X, yo = A.Y, zo = A.Z;
            double p = B.X - A.X, q = B.Y - A.Y, r = B.Z - A.Z;

            double x1 = C.X, y1 = C.Y, z1 = C.Z;
            double p1 = D.X - C.X, q1 = D.Y - C.Y, r1 = D.Z - C.Z;

            double x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                (q * p1 - q1 * p);
            double y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                (p * q1 - p1 * q);
            double z = (zo * q * r1 - z1 * q1 * r - yo * r * r1 + y1 * r * r1) /
                (q * r1 - q1 * r);

            return new Vector4((float)x, (float)y, (float)z, 0);
        }
    }
}
