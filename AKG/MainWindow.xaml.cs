﻿using System;
using System.IO.Ports;
using System.Threading;
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

        public double angleX
        {
            get { return _angleX; }
            set
            {
                if (value != _angleX)
                {
                    _angleX = value;
                    lbRotateX.Content = angleX.ToString("#.##");

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, _movement[0], _movement[1], _movement[2]);
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

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, _movement[0], _movement[1], _movement[2]);
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

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, _movement[0], _movement[1], _movement[2]); 
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

                    VectorTransformation.TransformVectors((float)_angleX, (float)_angleY, (float)_angleZ, (float)_scale, _movement[0], _movement[1], _movement[2]);
                    renderer.DrawModel();
                }
            }
        }

        public float[] movement
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

        private double _angleX = 0;
        private double _angleY = 0;
        private double _angleZ = 0;
        private double _scale = 10;
        private float[] _movement = { 0, 0, 0 };

        public MainWindow()
        {
            InitializeComponent();

            renderer = new Renderer(img);

            Loaded += delegate
            {
                VectorTransformation.width = (float)img.ActualWidth;
                VectorTransformation.height = (float)img.ActualHeight;

                Model.ReadFile("..\\..\\..\\objects\\hollow.obj");

                VectorTransformation.UpdateViewPort();

                VectorTransformation.UpdateCameraBasicVectors();
                VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);

                renderer.DrawModel();
            };

            Thread readThread = new Thread(ReadSerialPortData);
            readThread.Start();
        }

        void ReadSerialPortData()
        {
            SerialPort serialPort = new SerialPort("COM7", 115200, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 500
            };

            serialPort.Open();

            bool flagInit = true;
            double serialPortAngle = 0;

            while (serialPort.IsOpen)
            {
                serialPortAngle  = (((255 - serialPort.ReadByte()) * 360) >> 8) / 60;

                if(flagInit)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, () =>
                    {
                        angleY = serialPortAngle;
                    });

                    flagInit = false;
                }

                Dispatcher.Invoke(DispatcherPriority.Normal, () =>
                {
                    angleY += serialPortAngle > angleY + 0.1 ? 0.1
                    : serialPortAngle < angleY - 0.1 ? -0.1
                    : 0;
                });
            }

            serialPort.Close();
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
                    movement[0] += 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.A:
                    movement[0] -= 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.S:
                    movement[1] -= 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.W:
                    movement[1] += 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.E:
                    movement[2] += 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    renderer.DrawModel();
                    break;
                case System.Windows.Input.Key.Q:
                    movement[2] -= 1;
                    lbPos.Content = movement[0].ToString() + ", " + movement[1].ToString() + ", " + movement[2].ToString();

                    VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
                    renderer.DrawModel();
                    break;
            }
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            VectorTransformation.width = (float)img.ActualWidth;
            VectorTransformation.height = (float)img.ActualHeight;

            VectorTransformation.UpdateViewPort();
            VectorTransformation.TransformVectors((float)angleX, (float)angleY, (float)angleZ, (float)scale, movement[0], movement[1], movement[2]);
            
            renderer.DrawModel();
        }
    }
}
