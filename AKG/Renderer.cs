using System;
using System.Drawing;
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

        private Vector3 normal, lightDirection;

        private float[] ambientColor = new float[] { 9, 56, 97 };
        private float[] diffuseColor = new float[] { 87, 171, 105 };
        private float[] specularColor = new float[] { 212, 21, 21 };

        private float ambientFactor = 1f;
        private float diffuseFactor = 0.5f;
        private float specularFactor = 0.5f;

        private float glossFactor = 1f;

        private Vector3 objectColor = new(0.3f, 0.7f, 0.23f);
        private Vector3 lightColor = new(100f, 100f, 0f);

        public Renderer(Image image)
        {
            this.image = image;
        }

        private int[] AmbientLightning()
        {
            int[] values = new int[3];

            for (int i = 0; i < 3; i++)
            {
                values[i] = (int)(ambientColor[i] * ambientFactor);
            }

            return values;
        }

        private int[] DiffuseLightning(Vector3 color)
        {
            int[] values = new int[3];

            if(color.X < 0)
            {
                color = new(0);
            }

            values[0] = (int)(diffuseFactor * color.X * diffuseColor[0]);
            values[1] = (int)(diffuseFactor * color.Y * diffuseColor[1]);
            values[2] = (int)(diffuseFactor * color.Z * diffuseColor[2]);
            
            return values;
        }

        private int[] SpecularLightning()
        {
            Vector3 reflection = Vector3.Reflect(lightDirection, normal);
            float RV = Vector3.Dot(reflection, new Vector3(1f, 0, 0));

            if (RV < 0)
            {
                RV = 0;
            }

            int[] values = new int[3];
            double temp = Math.Pow(RV, glossFactor);

            values[0] = (int)(specularFactor * temp * specularColor[0]);
            values[1] = (int)(specularFactor * temp * specularColor[1]);
            values[2] = (int)(specularFactor * temp * specularColor[2]);

            return values;
        }

        private unsafe void DrawPixel(WriteableBitmap bitmap, int x, int y, Vector3 color)
        {
            if (x > 0 && y > 0 && x < VectorTransformation.width && y < VectorTransformation.height)
            {
                byte* temp = (byte*)bitmap.BackBuffer + y * bitmap.BackBufferStride + x * bitmap.Format.BitsPerPixel / 8;


                //2
                temp[0] = (byte)Math.Min(color.Z * 255, 255);
                temp[1] = (byte)Math.Min(color.Y * 255, 255);
                temp[2] = (byte)Math.Min(color.X * 255, 255);
                temp[3] = 255;

                //3
                //int[] ambientValues = AmbientLightning();
                //int[] diffuseValues = DiffuseLightning(color);
                //int[] specularValues = SpecularLightning();

                //temp[3] = 255;
                //temp[2] = (byte)Math.Min(ambientValues[0] + diffuseValues[0] + specularValues[0], 255);
                //temp[1] = (byte)Math.Min(ambientValues[1] + diffuseValues[1] + specularValues[1], 255);
                //temp[0] = (byte)Math.Min(ambientValues[2] + diffuseValues[2] + specularValues[2], 255);
            }
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

            //2
            //Random rand = new Random();
            //var tint = new int[] { rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255) };

            Rasterization(bitmap);

            bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            bitmap.Unlock();

            image.Source = bitmap;
        }

        private void Rasterization(WriteableBitmap bitmap)
        {
            float?[,] z_buff = new float?[bitmap.PixelHeight, bitmap.PixelWidth];

            foreach (var vector in Model.listF)
            {
                for (int j = 3; j < vector.Length - 3; j += 3)
                {
                    Vector4[] screenTriangle = { Model.screenVertices[vector[0] - 1], Model.screenVertices[vector[j] - 1], Model.screenVertices[vector[j + 3] - 1] };
                    Vector3[] worldTriangle = { Model.worldVertices[vector[0] - 1], Model.worldVertices[vector[j] - 1], Model.worldVertices[vector[j + 3] - 1] };

                    // Отбраковка невидимых поверхностей.
                    Vector4 edge1 = screenTriangle[2] - screenTriangle[0];
                    Vector4 edge2 = screenTriangle[1] - screenTriangle[0];
                    if (edge1.X * edge2.Y - edge1.Y * edge2.X <= 0)
                    {
                        continue;
                    }

                    Vector3 worldEdge1 = worldTriangle[1] - worldTriangle[0];
                    Vector3 worldEdge2 = worldTriangle[2] - worldTriangle[0];

                    // Модель освещения Ламберта.
                    normal = Vector3.Cross(worldEdge1, worldEdge2);
                    normal = Vector3.Normalize(normal);

                    // Отбраковка невидимых поверхностей.
                    //Vector3 buf = new(worldTriangle[0].X, worldTriangle[0].Y, worldTriangle[0].Z);
                    //if (Vector3.Dot(normal, VectorTransformation.eye - buf) < 0)
                    //{
                    //    continue;
                    //}

                    // Растеризация.
                    if (screenTriangle[0].Y > screenTriangle[1].Y)
                    {
                        (screenTriangle[0], screenTriangle[1]) = (screenTriangle[1], screenTriangle[0]);
                        (worldTriangle[0], worldTriangle[1]) = (worldTriangle[1], worldTriangle[0]);
                    }
                    if (screenTriangle[0].Y > screenTriangle[2].Y)
                    {
                        (screenTriangle[0], screenTriangle[2]) = (screenTriangle[2], screenTriangle[0]);
                        (worldTriangle[0], worldTriangle[2]) = (worldTriangle[2], worldTriangle[0]);
                    }
                    if (screenTriangle[1].Y > screenTriangle[2].Y)
                    {
                        (screenTriangle[1], screenTriangle[2]) = (screenTriangle[2], screenTriangle[1]);
                        (worldTriangle[1], worldTriangle[2]) = (worldTriangle[2], worldTriangle[1]);
                    }


                    Vector4 screenKoeff01 = (screenTriangle[1] - screenTriangle[0]) / (screenTriangle[1].Y - screenTriangle[0].Y);
                    Vector3 worldKoeff01 = (worldTriangle[1] - worldTriangle[0]) / (screenTriangle[1].Y - screenTriangle[0].Y);

                    Vector4 screenKoeff02 = (screenTriangle[2] - screenTriangle[0]) / (screenTriangle[2].Y - screenTriangle[0].Y);
                    Vector3 worldKoeff02 = (worldTriangle[2] - worldTriangle[0]) / (screenTriangle[2].Y - screenTriangle[0].Y);

                    Vector4 screenKoeff03 = (screenTriangle[2] - screenTriangle[1]) / (screenTriangle[2].Y - screenTriangle[1].Y);
                    Vector3 worldKoeff03 = (worldTriangle[2] - worldTriangle[1]) / (screenTriangle[2].Y - screenTriangle[1].Y);

                    int minY = Math.Max((int)MathF.Ceiling(screenTriangle[0].Y), 0);
                    int maxY = Math.Min((int)MathF.Ceiling(screenTriangle[2].Y), bitmap.PixelHeight);

                    for (int y = minY; y < maxY; y++)
                    {
                        Vector4 screenA = y < screenTriangle[1].Y ? screenTriangle[0] + (y - screenTriangle[0].Y) * screenKoeff01 :
                                                                    screenTriangle[1] + (y - screenTriangle[1].Y) * screenKoeff03; 
                        Vector4 screenB = screenTriangle[0] + (y - screenTriangle[0].Y) * screenKoeff02;

                        Vector3 worldA = y < screenTriangle[1].Y ? worldTriangle[0] + (y - screenTriangle[0].Y) * worldKoeff01 :
                                                                   worldTriangle[1] + (y - screenTriangle[1].Y) * worldKoeff03;
                        Vector3 worldB = worldTriangle[0] + (y - screenTriangle[0].Y) * worldKoeff02;

                        if (screenA.X > screenB.X)
                        {
                            (screenA, screenB) = (screenB, screenA);
                            (worldA, worldB) = (worldB, worldA);
                        }

                        int minX = Math.Max((int)MathF.Ceiling(screenA.X), 0);
                        int maxX = Math.Min((int)MathF.Ceiling(screenB.X), bitmap.PixelWidth);

                        Vector4 screenKoeff = (screenB - screenA) / (screenB.X - screenA.X);
                        Vector3 worldKoeff = (worldB - worldA) / (screenB.X - screenA.X);
                        for (int x = minX; x < maxX; x++)
                        {
                            Vector4 pScreen = screenA + (x - screenA.X) * screenKoeff;
                            Vector3 pWorld = worldA + (x - screenA.X) * worldKoeff;

                            //pScreen = Vector4.Normalize(pScreen);
                            //pWorld = Vector3.Normalize(pWorld);

                            // Z-буффер.
                            if (!(pScreen.Z > z_buff[y, x]))
                            {
                                z_buff[y, x] = pScreen.Z;
                                lightDirection = VectorTransformation.light - pWorld;
                                var distance = lightDirection.LengthSquared();
                                lightDirection = Vector3.Normalize(lightDirection);
                                var attenuation = 1 / Math.Max(distance, 0.01f);
                                float intensity = Math.Max(Vector3.Dot(normal, lightDirection), 0);
                                DrawPixel(bitmap, x, y, intensity * objectColor * attenuation * lightColor);
                            }
                        }
                    }
                }
            }
        }
    }
}
