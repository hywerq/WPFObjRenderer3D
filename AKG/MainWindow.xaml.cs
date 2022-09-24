using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace AKG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static double MaxDepth = 0, MinDepth = 0, focus;

        public MainWindow()
        {
            InitializeComponent();

            TransformMatrix.width = (float)window.Width;
            TransformMatrix.height = (float)window.Height - 20;

            Model.ReadFile("lego.obj");

            VectorTransformation.UpdateCameraBasicVectors();
            VectorTransformation.TransformVectors();

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
                canvas.Children.Add(DrawLine(Model.listV[vector[6] - 1 ], Model.listV[vector[0] - 1], false));
            }
        }

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

            return x * 11 * k + center;
        }

        private static double ConvertY(double y, double depth, double center, bool perspective)
        {
            double k;

            if (perspective) 
                k = (focus - depth) / (focus - MinDepth);
            else k = 1;

            return -y * 11 * k + center;
        }

        private static void FindMinMaxDepth()
        {
            MaxDepth = 0;
            MinDepth = 0;

            foreach(var item in Model.listF)
            {
                for (int i = 0; i < item.Length; i+=3)
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
