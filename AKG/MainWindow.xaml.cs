using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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

        private static string _file = "Shovel Knight";

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
                    renderer.DrawModel(this);
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
                    renderer.DrawModel(this);
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
                    renderer.DrawModel(this);
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
                    renderer.DrawModel(this);
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

                Model.ReadFile(this,
                               "..\\..\\..\\objects\\" + _file + "\\Model.obj",
                               "..\\..\\..\\objects\\" + _file + "\\textures\\diffuse.png",
                               "..\\..\\..\\objects\\" + _file + "\\textures\\specular.png",
                               "..\\..\\..\\objects\\" + _file + "\\textures\\normal.png",
                               "..\\..\\..\\objects\\" + _file + "\\textures\\mrao.png");

                VectorTransformation.UpdateViewPort();
                VectorTransformation.UpdateCameraBasicVectors();
                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);

                renderer.DrawModel(this);

                StartSerialPortHandle();
            };
        }

        private void StartSerialPortHandle()
        {
            Task.Run(() => SerialPortHandler.Start());
            Task.Run(() =>
            {
                while (true)
                {
                    if (SerialPortHandler.RotationBuffer.Count > 0)
                    {
                        float value = SerialPortHandler.RotationBuffer[0];
                        float angleShiftValue = Math.Abs(value - (float)angleY);

                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, () =>
                        {
                            if(angleShiftValue > 0.2f)
                            {
                                angleY = value;

                                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                                renderer.DrawModel(this);
                            }
                        });

                        SerialPortHandler.RotationBuffer.RemoveAt(0);
                    }

                    if (SerialPortHandler.LightBuffer.Count > 0)
                    {
                        int value = SerialPortHandler.LightBuffer[0];
                        int lightShiftValue = Math.Abs(value - (int)VectorTransformation.light.Z);

                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, () =>
                        {
                            if (lightShiftValue > 10)
                            {
                                VectorTransformation.light.Z =
                                    VectorTransformation.light.Z < value ?
                                    VectorTransformation.light.Z + lightShiftValue :
                                    VectorTransformation.light.Z - lightShiftValue;

                                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                                renderer.DrawModel(this);
                            }
                        });

                        SerialPortHandler.LightBuffer.RemoveAt(0);
                    }
                }
            });
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
                    lbPos.Content = movement.X + ", " + movement.Y + ", " + movement.Z;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.A:
                    movement.X -= 1;
                    lbPos.Content = movement.X + ", " + movement.Y + ", " + movement.Z;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.Q:
                    movement.Y -= 1;
                    lbPos.Content = movement.X + ", " + movement.Y + ", " + movement.Z;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.E:
                    movement.Y += 1;
                    lbPos.Content = movement.X + ", " + movement.Y + ", " + movement.Z;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.W:
                    movement.Z += 1;
                    lbPos.Content = movement.X + ", " + movement.Y + ", " + movement.Z;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.S:
                    movement.Z -= 1;
                    lbPos.Content = movement.X + ", " + movement.Y + ", " + movement.Z;

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.NumPad6:
                    VectorTransformation.eye.X += 1;
                    lbCamera.Content = VectorTransformation.eye.X + ", " + VectorTransformation.eye.Y + ", " + VectorTransformation.eye.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.NumPad4:
                    VectorTransformation.eye.X -= 1;
                    lbCamera.Content = VectorTransformation.eye.X + ", " + VectorTransformation.eye.Y + ", " + VectorTransformation.eye.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.NumPad7:
                    VectorTransformation.eye.Y -= 1;
                    lbCamera.Content = VectorTransformation.eye.X + ", " + VectorTransformation.eye.Y + ", " + VectorTransformation.eye.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.NumPad9:
                    VectorTransformation.eye.Y += 1;
                    lbCamera.Content = VectorTransformation.eye.X + ", " + VectorTransformation.eye.Y + ", " + VectorTransformation.eye.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.NumPad8:
                    VectorTransformation.eye.Z += 1;
                    lbCamera.Content = VectorTransformation.eye.X + ", " + VectorTransformation.eye.Y + ", " + VectorTransformation.eye.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.NumPad5:
                    VectorTransformation.eye.Z -= 1;
                    lbCamera.Content = VectorTransformation.eye.X + ", " + VectorTransformation.eye.Y + ", " + VectorTransformation.eye.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.Up:
                    VectorTransformation.light.Z += 10;
                    lbLight.Content = VectorTransformation.light.X + ", " + VectorTransformation.light.Y + ", " + VectorTransformation.light.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.Down:
                    VectorTransformation.light.Z -= 10;
                    lbLight.Content = VectorTransformation.light.X + ", " + VectorTransformation.light.Y + ", " + VectorTransformation.light.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.Left:
                    VectorTransformation.light.X -= 10;
                    lbLight.Content = VectorTransformation.light.X + ", " + VectorTransformation.light.Y + ", " + VectorTransformation.light.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.Right:
                    VectorTransformation.light.X += 10;
                    lbLight.Content = VectorTransformation.light.X + ", " + VectorTransformation.light.Y + ", " + VectorTransformation.light.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.RightShift:
                    VectorTransformation.light.Y += 10;
                    lbLight.Content = VectorTransformation.light.X + ", " + VectorTransformation.light.Y + ", " + VectorTransformation.light.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
                case System.Windows.Input.Key.RightCtrl:
                    VectorTransformation.light.Y -= 10;
                    lbLight.Content = VectorTransformation.light.X + ", " + VectorTransformation.light.Y + ", " + VectorTransformation.light.Z;

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
                    renderer.DrawModel(this);
                    break;
            }
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            VectorTransformation.width = (float)img.ActualWidth;
            VectorTransformation.height = (float)img.ActualHeight;

            VectorTransformation.UpdateViewPort();
            VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement);
            
            renderer.DrawModel(this);
        }

        private void RotationButton_Checked(object sender, RoutedEventArgs e)
        {
            SerialPortHandler.SendCommand("R");
        }

        private void LightningButton_Checked(object sender, RoutedEventArgs e)
        {
            SerialPortHandler.SendCommand("L");
        }
    }
}
