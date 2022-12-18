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
        
        private MainWindow window;

        //private Vector3 normal, lightDirection;

        private float ambientFactor = 1.0f;
        private float diffuseFactor = 1.0f; 
        private float specularFactor = 0.1f;
        private float glossFactor = 65f;

        Vector3 color = new Vector3(235, 163, 9);
        Vector3 specular = new Vector3(212, 21, 21);
        Vector3 mrao = new Vector3(0.91f, 0.92f, 0.92f);

        public Renderer(Image image)
        {
            this.image = image;
        }

        private Vector3 AmbientLightning(Vector3 ambientColor)
        {
            Vector3 values = new Vector3
            {
                X = ambientColor.X * ambientFactor,
                Y = ambientColor.Y * ambientFactor,
                Z = ambientColor.Z * ambientFactor
            };

            return values;
        }

        private Vector3 DiffuseLightning(Vector3 diffuseColor, float intensity)
        {
            Vector3 values = new Vector3
            {
                X = diffuseFactor * intensity * diffuseColor.X,
                Y = diffuseFactor * intensity * diffuseColor.Y,
                Z = diffuseFactor * intensity * diffuseColor.Z
            };

            return values;
        }

        private Vector3 SpecularLightning(Vector3 specularColor, Vector3 View, Vector3 lightDirection, Vector3 normal)
        {
            Vector3 reflection = Vector3.Normalize(Vector3.Reflect(lightDirection, normal));
            float RV = Math.Max(Vector3.Dot(reflection, -View), 0);

            double temp = Math.Pow(RV, glossFactor);
            Vector3 values = new Vector3
            {
                X = (float)(specularFactor * temp * specularColor.X),
                Y = (float)(specularFactor * temp * specularColor.Y),
                Z = (float)(specularFactor * temp * specularColor.Z)
            };

            return values;
        }

        private Vector3 AcesFilmic(Vector3 color)
        {
            color = new(Vector3.Dot(new(0.59719f, 0.35458f, 0.04823f), color),
                        Vector3.Dot(new(0.07600f, 0.90834f, 0.01566f), color),
                        Vector3.Dot(new(0.02840f, 0.13384f, 0.83777f), color));

            color = (color * (color + 0.0245786f * Vector3.One) - 0.000090537f * Vector3.One) /
                    (color * (0.983729f * color + 0.4329510f * Vector3.One) + 0.238081f * Vector3.One);

            color = new(Vector3.Dot(new(1.60475f, -0.53108f, -0.07367f), color),
                        Vector3.Dot(new(-0.10208f, 1.10813f, -0.00605f), color),
                        Vector3.Dot(new(-0.00327f, -0.07276f, 1.07602f), color));

            return Vector3.Clamp(color, Vector3.Zero, Vector3.One);
        }

        private Vector3 SrgbToLinear(Vector3 color)
        {
            float SrgbToLinear(float c) => (float)(c <= 0.04045f ? c / 12.92f : Math.Pow((c + 0.055f) / 1.055f, 2.4f));
            return new(SrgbToLinear(color.X), SrgbToLinear(color.Y), SrgbToLinear(color.Z));
        }

        private Vector3 LinearToSrgb(Vector3 color)
        {
            float LinearToSrgb(float c) => (float)(c <= 0.0031308f ? 12.92f * c : 1.055f * Math.Pow(c, 1 / 2.4f) - 0.055f);
            return new(LinearToSrgb(color.X), LinearToSrgb(color.Y), LinearToSrgb(color.Z));
        }

        private float ThrowbridgeReitzNormalDistribution(float roughness, Vector3 normal, Vector3 halfWayVec)
        {
            float NdotH = Math.Max(Vector3.Dot(normal, halfWayVec), 0.0f);
            float numerator = (float)Math.Pow(roughness, 2.0);
            float denominator = (float)(Math.PI * Math.Pow(Math.Pow(NdotH, 2.0) * (Math.Pow(roughness, 2.0) - 1.0) + 1.0, 2.0));
            denominator = (float)Math.Max(denominator, 0.000001f);

            return numerator / denominator;
        }

        private float GetSchlickBeckmannFactor(float roughness, Vector3 normal, Vector3 x /* V or L */)
        {
            float k = roughness / 2.0f;
            float numerator = (float)Math.Max(Vector3.Dot(normal, x), 0.0);
            float denominator = (float)Math.Max(Vector3.Dot(normal, x), 0.0) * (1.0f - k) + k;
            denominator = (float)Math.Max(denominator, 0.000001f);

            return numerator / denominator;
        }

        private float GetSmithFactor(float roughness, Vector3 normal, Vector3 view, Vector3 light)
        {
            return GetSchlickBeckmannFactor(roughness, normal, view) * GetSchlickBeckmannFactor(roughness, normal, light);
        }

        private Vector3 GetFresnelSchlickFactor(Vector3 f0, Vector3 view, Vector3 halfWayVec)
        {
            Vector3 temp = new((float)Math.Pow(1 - Math.Max(Vector3.Dot(halfWayVec, view), 0.0), 5.0));

            return f0 + (Vector3.One - f0) * temp;
        }    

        private Vector3 GetPhysicallyBasedRenderingLight(Vector3 lightColor, Vector3 view, Vector3 light, Vector3 halfWayVec, Vector3 normal)
        {
            Vector3 f0 = new Vector3(0.04f);

            if (mrao.X > 0.2 && mrao.X < 0.8)
            { 
                f0 = Vector3.Lerp(new Vector3(0.04f), color, mrao.X);
            }                         
            else if(mrao.X >= 0.8)
            {
                f0 = color;
            }

            Vector3 ks = GetFresnelSchlickFactor(f0, view, halfWayVec);
            Vector3 kd = (Vector3.One - ks) * (1.0f - mrao.X);

            Vector3 lambert = color / (float)Math.PI;

            Vector3 cookTorranceNumerator = ThrowbridgeReitzNormalDistribution(mrao.Y, normal, halfWayVec)
                * GetSmithFactor(mrao.Y, normal, view, light) * ks;
            float cookTorranceDenominator = 4.0f * (float)Math.Max(Vector3.Dot(view, normal), 0.0) 
                * (float)Math.Max(Vector3.Dot(light, normal), 0.0);
            cookTorranceDenominator = (float)Math.Max(cookTorranceDenominator, 0.000001f);

            Vector3 cookTorrance = cookTorranceNumerator / cookTorranceDenominator;
            Vector3 brdf = kd * lambert + cookTorrance;

            Vector3 result = brdf * lightColor * (float)Math.Max(Vector3.Dot(light, normal), 0.0);

            return result;
        }
         
        private unsafe void DrawPixel(WriteableBitmap bitmap, int x, int y, Vector3 light)
        {
            if (x > 0 && y > 0 && x < VectorTransformation.width && y < VectorTransformation.height)
            {
                byte* temp = (byte*)bitmap.BackBuffer + y * bitmap.BackBufferStride + x * bitmap.Format.BitsPerPixel / 8;

                float lightIntensity = 0.8f;

                light *= 255f;

                temp[3] = 255;
                temp[2] = (byte)Math.Min(lightIntensity * light.X, 255);
                temp[1] = (byte)Math.Min(lightIntensity * light.Y, 255);
                temp[0] = (byte)Math.Min(lightIntensity * light.Z, 255);
            }
        }

        public void DrawModel(MainWindow window)
        {
            this.window = window;

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

        private void Rasterization(WriteableBitmap bitmap)
        {
            float?[,] z_buff = new float?[bitmap.PixelHeight, bitmap.PixelWidth];

            //FindNormals();

            foreach (int[] vector in Model.listF)
            {
                // Итерация по треугольникам в полигоне.
                for (int j = 3; j < vector.Length - 3; j += 3)
                {
                    // Формирование треугольников в экранных и мировых координатах.
                    Vector4[] screenTriangle = { Model.screenVertices[vector[0] - 1], Model.screenVertices[vector[j] - 1], Model.screenVertices[vector[j + 3] - 1] };
                    Vector3[] worldTriangle = { Model.worldVertices[vector[0] - 1], Model.worldVertices[vector[j] - 1], Model.worldVertices[vector[j + 3] - 1] };

                    // Отбраковка невидимых поверхностей.
                    Vector4 edge1 = screenTriangle[2] - screenTriangle[0];
                    Vector4 edge2 = screenTriangle[1] - screenTriangle[0];
                    if (edge1.X * edge2.Y - edge1.Y * edge2.X <= 0)
                    {
                        continue;
                    }

                    // Поиск нормали по вершинам.
                    Vector3 vertexNormal0 = Vector3.Normalize(Model.worldNormals[vector[2] - 1]);
                    Vector3 vertexNormal1 = Vector3.Normalize(Model.worldNormals[vector[j + 2] - 1]);
                    Vector3 vertexNormal2 = Vector3.Normalize(Model.worldNormals[vector[j + 5] - 1]);

                    // Поиск текстурной координаты по вершинам.
                    Vector2 texture0 = Model.textures[vector[1] - 1]    ;/*/ screenTriangle[0].Z;*/
                    Vector2 texture1 = Model.textures[vector[j + 1] - 1];/*/ screenTriangle[1].Z;*/
                    Vector2 texture2 = Model.textures[vector[j + 4] - 1];/*/ screenTriangle[2].Z;*/

                    // Сортировка вершин треугольников в порядке "вверх-лево-право(низ)".
                    if (screenTriangle[0].Y > screenTriangle[1].Y)
                    {
                        (screenTriangle[0], screenTriangle[1]) = (screenTriangle[1], screenTriangle[0]);
                        (worldTriangle[0], worldTriangle[1]) = (worldTriangle[1], worldTriangle[0]);
                        (vertexNormal0, vertexNormal1) = (vertexNormal1, vertexNormal0);
                        (texture0, texture1) = (texture1, texture0);
                    }
                    if (screenTriangle[0].Y > screenTriangle[2].Y)
                    {
                        (screenTriangle[0], screenTriangle[2]) = (screenTriangle[2], screenTriangle[0]);
                        (worldTriangle[0], worldTriangle[2]) = (worldTriangle[2], worldTriangle[0]);
                        (vertexNormal0, vertexNormal2) = (vertexNormal2, vertexNormal0);
                        (texture0, texture2) = (texture2, texture0);
                    }
                    if (screenTriangle[1].Y > screenTriangle[2].Y)
                    {
                        (screenTriangle[1], screenTriangle[2]) = (screenTriangle[2], screenTriangle[1]);
                        (worldTriangle[1], worldTriangle[2]) = (worldTriangle[2], worldTriangle[1]);
                        (vertexNormal1, vertexNormal2) = (vertexNormal2, vertexNormal1);
                        (texture1, texture2) = (texture2, texture1);
                    }

                    // Нахождение коэффицентов в экранных и мировых координатах, коэффицента для нормалей, текстур.
                    Vector4 screenKoeff01       = (screenTriangle[1] - screenTriangle[0]) / (screenTriangle[1].Y - screenTriangle[0].Y);
                    Vector3 worldKoeff01        = (worldTriangle[1] - worldTriangle[0])   / (screenTriangle[1].Y - screenTriangle[0].Y);
                    Vector3 vertexNormalKoeff01 = (vertexNormal1 - vertexNormal0)         / (screenTriangle[1].Y - screenTriangle[0].Y);
                    Vector2 textureKoeff01      = (texture1 - texture0)                   / (screenTriangle[1].Y - screenTriangle[0].Y);

                    Vector4 screenKoeff02       = (screenTriangle[2] - screenTriangle[0]) / (screenTriangle[2].Y - screenTriangle[0].Y);
                    Vector3 worldKoeff02        = (worldTriangle[2] - worldTriangle[0])   / (screenTriangle[2].Y - screenTriangle[0].Y);
                    Vector3 vertexNormalKoeff02 = (vertexNormal2 - vertexNormal0)         / (screenTriangle[2].Y - screenTriangle[0].Y);
                    Vector2 textureKoeff02      = (texture2 - texture0)                   / (screenTriangle[2].Y - screenTriangle[0].Y);

                    Vector4 screenKoeff03       = (screenTriangle[2] - screenTriangle[1]) / (screenTriangle[2].Y - screenTriangle[1].Y);
                    Vector3 worldKoeff03        = (worldTriangle[2] - worldTriangle[1])   / (screenTriangle[2].Y - screenTriangle[1].Y);
                    Vector3 vertexNormalKoeff03 = (vertexNormal2 - vertexNormal1)         / (screenTriangle[2].Y - screenTriangle[1].Y);
                    Vector2 textureKoeff03      = (texture2 - texture1)                   / (screenTriangle[2].Y - screenTriangle[1].Y);

                    // Нахождение минимального и максимального Y для треугольника на экране и последующей итерации.
                    int minY = Math.Max((int)MathF.Ceiling(screenTriangle[0].Y), 0);
                    int maxY = Math.Min((int)MathF.Ceiling(screenTriangle[2].Y), bitmap.PixelHeight);

                    for (int y = minY; y < maxY; y++)
                    {
                        // Нахождение левого и правого значения Y.
                        Vector4 screenA = y < screenTriangle[1].Y ? screenTriangle[0] + (y - screenTriangle[0].Y) * screenKoeff01 :
                                                                    screenTriangle[1] + (y - screenTriangle[1].Y) * screenKoeff03; 
                        Vector4 screenB = screenTriangle[0] + (y - screenTriangle[0].Y) * screenKoeff02;

                        Vector3 worldA = y < screenTriangle[1].Y ? worldTriangle[0] + (y - screenTriangle[0].Y) * worldKoeff01 :
                                                                   worldTriangle[1] + (y - screenTriangle[1].Y) * worldKoeff03;
                        Vector3 worldB = worldTriangle[0] + (y - screenTriangle[0].Y) * worldKoeff02;

                        // Нахождение нормали для левого и правого Y.
                        Vector3 normalA = y < screenTriangle[1].Y ? vertexNormal0 + (y - screenTriangle[0].Y) * vertexNormalKoeff01 :
                                                                    vertexNormal1 + (y - screenTriangle[1].Y) * vertexNormalKoeff03;
                        Vector3 normalB = vertexNormal0 + (y - screenTriangle[0].Y) * vertexNormalKoeff02;

                        // Нахождение нормали для левого и правого Y.
                        Vector2 textureA = y < screenTriangle[1].Y ? texture0 + (y - screenTriangle[0].Y) * textureKoeff01 :
                                                                    texture1 + (y - screenTriangle[1].Y) * textureKoeff03;
                        Vector2 textureB = texture0 + (y - screenTriangle[0].Y) * textureKoeff02;

                        // Сортировка значений в порядке "лево-право".
                        if (screenA.X > screenB.X)
                        {
                            (screenA, screenB)   = (screenB, screenA);
                            (worldA , worldB)    = (worldB , worldA);
                            (normalA, normalB)   = (normalB, normalA);
                            (textureA, textureB) = (textureB, textureA);
                        }

                        // Нахождение минимального и максимального X для треугольника на экране и последующей итерации.
                        int minX = Math.Max((int)MathF.Ceiling(screenA.X), 0);
                        int maxX = Math.Min((int)MathF.Ceiling(screenB.X), bitmap.PixelWidth);

                        // Нахождение коэффицентов изменения X в экранных и мировых координатах, коэффицента изменения нормали.
                        Vector4 screenKoeff  = (screenB - screenA)   / (screenB.X - screenA.X);
                        Vector3 worldKoeff   = (worldB - worldA)     / (screenB.X - screenA.X);
                        Vector3 normalKoeff  = (normalB - normalA)   / (screenB.X - screenA.X);
                        Vector2 textureKoeff = (textureB - textureA) / (screenB.X - screenA.X);

                        // Сканирующая линия.
                        for (int x = minX; x < maxX; x++)
                        {
                            // Нахождение координат точки в экранных и мировых координатах.
                            Vector4 pScreen = screenA + (x - screenA.X) * screenKoeff;
                            Vector3 pWorld = worldA + (x - screenA.X) * worldKoeff;

                            // Z-буффер.
                            if (!(pScreen.Z > z_buff[y, x]))
                            {
                                z_buff[y, x] = pScreen.Z;

                                // Нахождение обратного вектора направления света.
                                Vector3 lightDirection = Vector3.Normalize(pWorld - VectorTransformation.light);
                                Vector3 viewDirection = Vector3.Normalize(pWorld - VectorTransformation.eye);

                                Vector3 view = Vector3.Normalize(VectorTransformation.eye - pWorld);
                                Vector3 light = Vector3.Normalize(VectorTransformation.light - pWorld);
                                Vector3 halfWayVector = Vector3.Normalize(view + light);

                                // original
                                Vector2 texture = textureA + (x - screenA.X) * textureKoeff;
                                //texture /= Vector2.One / pScreen.Z;

                                // affine
                                //Vector2 texture = (Vector2.One - textureKoeff) * textureA + textureKoeff * textureB;

                                // perspective
                                /*Vector2 texture = ((Vector2.One - textureKoeff) * (textureA / screenA.Z) +
                                                    textureKoeff * (textureB / screenB.Z)) /
                                                  ((Vector2.One - textureKoeff) * (1 / screenA.Z) +
                                                    textureKoeff * (1 / screenB.Z));*/

                                // Цвет объекта.
                                if (Model.textureFile != null)
                                {
                                    System.Drawing.Color objColor = Model.textureFile.GetPixel(Convert.ToInt32(texture.X * Model.textureFile.Width), Convert.ToInt32((1 - texture.Y) * Model.textureFile.Height));
                                    color = new Vector3(objColor.R, objColor.G, objColor.B) / 255f;
                                }

                                // Цвет отражения.
                                if (Model.mirrorMap != null)
                                {
                                    System.Drawing.Color spcColor = Model.mirrorMap.GetPixel(Convert.ToInt32(texture.X * Model.mirrorMap.Width), Convert.ToInt32((1 - texture.Y) * Model.mirrorMap.Height));
                                    specular = new Vector3(spcColor.R, spcColor.G, spcColor.B) / 255f;
                                }

                                // Нахождение нормали для точки.
                                Vector3 normal = Vector3.One;
                                if (Model.normalMap != null)
                                {
                                    normal = Model.fileNormals[Convert.ToInt32(texture.X * Model.fileNormals.GetLength(0)), Convert.ToInt32((1 - texture.Y) * Model.fileNormals.GetLength(1))];
                                }
                                else
                                {
                                    normal = normalA + (x - screenA.X) * normalKoeff;
                                }
                                normal = Vector3.Normalize(normal);

                                // Мрао для точки.
                                if (Model.mraoMap != null)
                                {
                                    System.Drawing.Color spcColor = Model.mraoMap.GetPixel(Convert.ToInt32(texture.X * Model.mraoMap.Width), Convert.ToInt32((1 - texture.Y) * Model.mraoMap.Height));
                                    mrao = new Vector3(spcColor.R, spcColor.G, spcColor.B) / 255f;
                                }

                                // Нахождение дистанции до источника света.
                                float distance = lightDirection.LengthSquared();
                                // Затенение объекта в зависимости от дистанции света до модели.
                                float attenuation = 1 / Math.Max(distance, 0.01f);
                                // Получение затененности каждой точки.
                                float intensity = Math.Max(Vector3.Dot(normal, -lightDirection), 0);

                                // Освещение Фонга.
                                Vector3 ambientValues = AmbientLightning(color);
                                Vector3 diffuseValues = DiffuseLightning(color, intensity * attenuation);
                                Vector3 specularValues = SpecularLightning(specular, viewDirection, lightDirection, normal);

                                // Отрисовка Гуро.
                                //DrawPixel(bitmap, x, y, ambientValues + diffuseValues + specularValues);
                                //window.lbPBR.Content = "No";

                                // Отрисовка ПБР.
                                Vector3 shade = GetPhysicallyBasedRenderingLight(new(1.0f, 1.0f, 1.0f), view, light, halfWayVector, normal);
                                DrawPixel(bitmap, x, y, shade * 6.0f);
                                window.lbPBR.Content = "Yes";
                            }
                        }
                    }
                }
            }
        }
    }
}
