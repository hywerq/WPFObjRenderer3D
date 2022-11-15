using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

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


            Random rand = new Random();
            var tint = new int[] { rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255) };
            Rasterization(bitmap, tint);

            bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            bitmap.Unlock();

            image.Source = bitmap;
        }

        private unsafe void DrawPixel(WriteableBitmap bitmap, int x, int y, Vector3 color, int[] tint)
        {
            if (x > 0 && y > 0 && x < VectorTransformation.width && y < VectorTransformation.height)
            {
                byte* temp = (byte*)bitmap.BackBuffer + y * bitmap.BackBufferStride + x * bitmap.Format.BitsPerPixel / 8;

                temp[0] = (byte)Math.Min(color.X * tint[0], 255);
                temp[1] = (byte)Math.Min(color.Y * tint[1], 255);
                temp[2] = (byte)Math.Min(color.Z * tint[2], 255);
                temp[3] = 255;
            }
        }

        private void Rasterization(WriteableBitmap bitmap, int[] tint)
        {
            float?[,] z_buff = new float?[bitmap.PixelHeight, bitmap.PixelWidth];

            foreach (var vector in Model.listF)
            {
                for (int j = 3; j < vector.Length - 3; j += 3)
                {
                    Vector4[] screenTriangle = { Model.screenVertices[vector[0] - 1], Model.screenVertices[vector[j] - 1], Model.screenVertices[vector[j + 3] - 1] };
                    Vector4[] worldTriangle = { Model.worldVertices[vector[0] - 1], Model.worldVertices[vector[j] - 1], Model.worldVertices[vector[j + 3] - 1] };

                    // Отбраковка невидимых поверхностей.
                    //Vector4 edge1 = screenTriangle[1] - screenTriangle[0];
                    //Vector4 edge2 = screenTriangle[2] - screenTriangle[0];
                    //if (edge1.X * edge2.Y - edge1.Y * edge2.X > 0)
                    //{
                    //    continue;
                    //}

                    Vector4 edge1 = worldTriangle[1] - worldTriangle[0];
                    Vector4 edge2 = worldTriangle[2] - worldTriangle[0];

                    // Модель освещения Ламберта.
                    Vector3 normal = new((edge1.Y * edge2.Z - edge1.Z * edge2.Y), (edge1.Z * edge2.X - edge1.X * edge2.Z), (edge1.X * edge2.Y - edge1.Y * edge2.X));
                    normal = Vector3.Normalize(normal);

                    // Отбраковка невидимых поверхностей.
                    if (Vector3.Dot(normal, VectorTransformation.eye) < 0)
                    {
                        continue;
                    }

                    // Растеризация.
                    if (screenTriangle[0].Y > screenTriangle[1].Y)
                    {
                        (screenTriangle[0], screenTriangle[1]) = (screenTriangle[1], screenTriangle[0]);
                        (worldTriangle[0],  worldTriangle[1])  = (worldTriangle[1],  worldTriangle[0]);
                    }
                    if (screenTriangle[0].Y > screenTriangle[2].Y)
                    {
                        (screenTriangle[0], screenTriangle[2]) = (screenTriangle[2], screenTriangle[0]);
                        (worldTriangle[0],  worldTriangle[2])  = (worldTriangle[2],  worldTriangle[0]);
                    }
                    if (screenTriangle[1].Y > screenTriangle[2].Y)
                    {
                        (screenTriangle[1], screenTriangle[2]) = (screenTriangle[2], screenTriangle[1]);
                        (worldTriangle[1],  worldTriangle[2])  = (worldTriangle[2],  worldTriangle[1]);
                    }


                    Vector4 screenKoeff01 = (screenTriangle[1] - screenTriangle[0]) / (screenTriangle[1].Y - screenTriangle[0].Y);
                    Vector4 worldKoeff01 =  (worldTriangle[1]  - worldTriangle[0])  / (screenTriangle[1].Y - screenTriangle[0].Y);

                    Vector4 screenKoeff02 = (screenTriangle[2] - screenTriangle[0]) / (screenTriangle[2].Y - screenTriangle[0].Y);
                    Vector4 worldKoeff02 =  (worldTriangle[2]  - worldTriangle[0])  / (screenTriangle[2].Y - screenTriangle[0].Y);

                    Vector4 screenKoeff03 = (screenTriangle[2] - screenTriangle[1]) / (screenTriangle[2].Y - screenTriangle[1].Y);
                    Vector4 worldKoeff03 =  (worldTriangle[2]  - worldTriangle[1])  / (screenTriangle[2].Y - screenTriangle[1].Y);

                    int minY = Math.Max((int)MathF.Ceiling(screenTriangle[0].Y), 0);
                    int maxY = Math.Min((int)MathF.Ceiling(screenTriangle[2].Y), bitmap.PixelHeight - 1);

                    for (int y = minY; y < maxY; y++)
                    {
                        Vector4 screenA = y > screenTriangle[1].Y ? screenTriangle[1] + (y - screenTriangle[1].Y) * screenKoeff03 :
                                                                    screenTriangle[0] + (y - screenTriangle[0].Y) * screenKoeff01;
                        Vector4 screenB = screenTriangle[0] + (y - screenTriangle[0].Y) * screenKoeff02;

                        Vector4 worldA = y > worldTriangle[1].Y   ? worldTriangle[1] + (y - screenTriangle[1].Y) * worldKoeff03 : 
                                                                    worldTriangle[0] + (y - screenTriangle[0].Y) * worldKoeff01;
                        Vector4 worldB = worldTriangle[0]   + (y - screenTriangle[0].Y) * worldKoeff02;

                        if (screenA.X > screenB.X)
                        {
                            (screenA, screenB) = (screenB, screenA);
                            (worldA, worldB) = (worldB, worldA);
                        }

                        int minX = Math.Max((int)MathF.Ceiling(screenA.X), 0);
                        int maxX = Math.Min((int)MathF.Ceiling(screenB.X), bitmap.PixelWidth - 1);

                        Vector4 screenKoeff = (screenB - screenA) / (screenB.X - screenA.X);
                        Vector4 worldKoeff = (worldB - worldA) / (screenB.X - screenA.X);
                        for (int x = minX; x < maxX; x++)
                        {
                            Vector4 pScreen = screenA + (x - screenA.X) * screenKoeff;
                            Vector4 pWorld  = worldA  + (x - screenA.X) * worldKoeff;
                            pScreen = Vector4.Normalize(pScreen);
                            pWorld = Vector4.Normalize(pWorld);

                            // Z-буффер.
                            if (z_buff[y, x] == null || z_buff[y, x] > pScreen.Z)
                            {
                                z_buff[y, x] = pScreen.Z;
                                Vector3 lightDir = new(VectorTransformation.light.X - pWorld.X, VectorTransformation.light.Y - pWorld.Y, VectorTransformation.light.Z - pWorld.Z);
                                lightDir = Vector3.Normalize(lightDir);
                                float color = Vector3.Dot(normal, lightDir);
                                DrawPixel(bitmap, x, y, new(color), tint);
                            }
                        }
                    }
                }
            }
        }
    }
}
