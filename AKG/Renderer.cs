using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AKG
{
    public class Renderer
    {
        private Image image;

        public Renderer(Image image)
        {
            this.image = image;
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

            Rasterization(bitmap);

            bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            bitmap.Unlock();

            image.Source = bitmap;
        }

        private void DrawVector(WriteableBitmap bitmap, Vector4 v1, Vector4 v2)
        {
            float x = v1.X;
            float y = v1.Y;

            float L = Math.Abs(v1.X - v2.X) > Math.Abs(v1.Y - v2.Y) ? Math.Abs(v1.X - v2.X) : Math.Abs(v1.Y - v2.Y);

            for (int i = 0; i < L; i++)
            {
                x += (v2.X - v1.X) / L;
                y += (v2.Y - v1.Y) / L;

                DrawPixel(bitmap, Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y)), Vector3.One);
            }
        }

        private unsafe void DrawPixel(WriteableBitmap bitmap, int x, int y, Vector3 color)
        {
            if (x > 0 && y > 0 && x < VectorTransformation.width && y < VectorTransformation.height)
            {
                byte* temp = (byte*)bitmap.BackBuffer + y * bitmap.BackBufferStride + x * bitmap.Format.BitsPerPixel / 8;

                temp[0] = (byte)Math.Min(color.X * 255, 255);
                temp[1] = (byte)Math.Min(color.Y * 255, 255);
                temp[2] = (byte)Math.Min(color.Z * 255, 255);
                temp[3] = 255;
            }
        }

        // TODO PixelWidth

        private void Rasterization(WriteableBitmap bitmap)
        {
            float?[,] z_buff = new float?[bitmap.PixelHeight, bitmap.PixelWidth];

            foreach (var vector in Model.listF)
            {
                for (int j = 3; j < vector.Length - 3; j += 3)
                {
                    Vector4[] triangle = { Model.model[vector[0] - 1], Model.model[vector[j] - 1], Model.model[vector[j + 3] - 1] };

                    Vector4 edge1 = triangle[1] - triangle[0];
                    Vector4 edge2 = triangle[2] - triangle[0];
                    if (edge1.X * edge2.Y - edge1.Y * edge2.X <= 0)
                    {
                        continue;
                    }                    

                    Array.Sort(triangle, (x, y) => x.Y.CompareTo(y.Y));

                    Vector4 koeff_01 = ((triangle[1] - triangle[0]) / (triangle[1].Y - triangle[0].Y));
                    Vector4 koeff_02 = ((triangle[2] - triangle[0]) / (triangle[2].Y - triangle[0].Y));
                    Vector4 koeff_12 = ((triangle[2] - triangle[1]) / (triangle[2].Y - triangle[1].Y));

                    int minY = Math.Max((int)MathF.Ceiling(triangle[0].Y), 0);
                    int maxY = Math.Min((int)MathF.Ceiling(triangle[2].Y), bitmap.PixelHeight - 1);

                    for (int y = minY; y < maxY; y++)
                    {
                        Vector4 a = y > triangle[1].Y ? triangle[1] + (y - triangle[1].Y) * koeff_12 : triangle[0] + (y - triangle[0].Y) * koeff_01;
                        Vector4 b = triangle[0] + (y - triangle[0].Y) * koeff_02;

                        if (a.X > b.X)
                        {
                            (a, b) = (b, a);
                        }

                        int minX = Math.Max((int)MathF.Ceiling(a.X), 0);
                        int maxX = Math.Min((int)MathF.Ceiling(b.X), bitmap.PixelWidth - 1);

                        Vector4 koeff_ab = (b - a) / (b.X - a.X);
                        for (int x = minX; x < maxX; x++)
                        {
                            Vector4 p = a + (x - a.X) * koeff_ab;
                            if (z_buff[y, x] == null || z_buff[y, x] > p.Z)
                            {
                                z_buff[y, x] = p.Z;
                                DrawPixel(bitmap, x, y, new(p.Z));
                            }
                        }
                    }
                }
            }
        }
    }
}
