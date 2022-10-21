using System;
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

        private static double _angleX = -1;
        private static double _angleY = 0;
        private static double _angleZ = 0;
        private static double _scale = 0.00001;
        private static float[] _movement = { 0, 0, 0 };

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
                    scale += scale;
                    lbscale.Content = scale.ToString("#.##########");

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    DrawModel();
                    break;
                case System.Windows.Input.Key.G:
                    scale -= scale;
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
    }
}
