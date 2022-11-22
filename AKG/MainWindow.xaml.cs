using System.Numerics;
using System.Windows;

namespace AKG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Renderer renderer;

        private static double _angleX = 0;
        private static double _angleY = 0;
        private static double _angleZ = 0;
        private static double _scale = 1;
        public static Vector3 movement = new(0, 0, 0);

        public double angleX
        {
            get { return _angleX; }
            set
            {
                if (value != _angleX)
                {
                    _angleX = value;
                    lbRotateX.Content = angleX.ToString("#.##");

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                }
            }
        }

        public double angleY
        {
            get { return _angleY; }
            set
            {
                if (value != _angleY)
                {
                    _angleY = value;
                    lbRotateY.Content = angleY.ToString("#.##");

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                }
            }
        }

        public double angleZ
        {
            get { return _angleZ; }
            set
            {
                if (value != _angleZ)
                {
                    _angleZ = value;
                    lbRotateZ.Content = angleZ.ToString("#.##");

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                }
            }
        }

        public double scale
        {
            get { return _scale; }
            set
            {
                if (value != _scale)
                {
                    _scale = value;
                    lbscale.Content = scale.ToString("#.##########");

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            renderer = new Renderer(img);

            Loaded += delegate
            {
                VectorTransformation.width = (float)img.ActualWidth;
                VectorTransformation.height = (float)img.ActualHeight;

                Model.ReadFile("..\\..\\..\\objects\\Model.obj");

                VectorTransformation.UpdateViewPort();

                VectorTransformation.UpdateCameraBasicVectors();
                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);

                renderer.DrawModel();
            };
        }

        private void window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.I:
                    angleX += 0.1;
                    break;
                case System.Windows.Input.Key.K:
                    angleX -= 0.1;
                    break;
                case System.Windows.Input.Key.J:
                    angleY += 0.1;
                    break;
                case System.Windows.Input.Key.L:
                    angleY -= 0.1;
                    break;
                case System.Windows.Input.Key.U:
                    angleZ += 0.1;
                    break;
                case System.Windows.Input.Key.O:
                    angleZ -= 0.1;
                    break;
                case System.Windows.Input.Key.T:
                    scale += 0.1;
                    break;
                case System.Windows.Input.Key.G:
                    scale -= 0.1;
                    break;
                case System.Windows.Input.Key.D:
                    movement.X += 1;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.A:
                    movement.X -= 1;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.Q:
                    movement.Y -= 1;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.E:
                    movement.Y += 1;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.W:
                    movement.Z += 1;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.S:
                    movement.Z -= 1;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.NumPad6:
                    VectorTransformation.eye.X += 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.NumPad4:
                    VectorTransformation.eye.X -= 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.NumPad7:
                    VectorTransformation.eye.Y -= 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.NumPad9:
                    VectorTransformation.eye.Y += 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.NumPad8:
                    VectorTransformation.eye.Z += 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.NumPad5:
                    VectorTransformation.eye.Z -= 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.Up:
                    VectorTransformation.light.Z += 10;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.Down:
                    VectorTransformation.light.Z -= 10;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.Left:
                    VectorTransformation.light.X -= 10;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.Right:
                    VectorTransformation.light.X += 10;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.RightShift:
                    VectorTransformation.light.Y += 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.RightCtrl:
                    VectorTransformation.light.Y -= 1;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel();
                    break;
            }
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            VectorTransformation.width = (float)img.ActualWidth;
            VectorTransformation.height = (float)img.ActualHeight;

            VectorTransformation.UpdateViewPort();
            VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
            
            renderer.DrawModel();
        }
    }
}
