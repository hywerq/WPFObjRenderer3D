using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;

namespace AKG
{
    public static class VectorTransformation
    {
        public static Vector3 XAxis = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 YAxis = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 ZAxis = new Vector3(1.0f, 1.0f, 1.0f);

        public static Vector3 eye = new Vector3(0.0f, 0.0f, 40.0f);
        public static Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
        public static Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

        public static Vector3 light = new Vector3(0f, 40f, 40f);

        public static float width = 1;
        public static float height = 1;

        public static float near = 1.0f;
        public static float far = 100.0f;

        public static float x_min = 0.0f;
        public static float y_min = 0.0f;

        public static float fov = MathF.PI / 4;
        public static float aspect = width / height;


        public static Matrix4x4 Viewport = new(
            (width / 2.0f), 0, 0, 0,
            0, (-height / 2.0f), 0, 0,
            0, 0, 1.0f, 0,
            (x_min + width / 2.0f), (y_min + height / 2.0f), 0, 1.0f
        );


        public static void UpdateCameraBasicVectors()
        {
            ZAxis = Vector3.Normalize(eye - target);
            XAxis = Vector3.Normalize(up * ZAxis);
            YAxis = up;
        }

        public static void UpdateViewPort()
        {
            Viewport.M11 = (width / 2.0f);
            Viewport.M41 = (x_min + width / 2.0f);
            Viewport.M22 = (-height / 2.0f);
            Viewport.M42 = (y_min + height / 2.0f);
            aspect = width / height;
        }

        public static void TransformVectors(float angleX, float angleY, float angleZ, float scale, Vector3 mov)
        {
            Model.screenVertices = new Vector4[Model.listV.Count];
            Model.worldVertices = new Vector3[Model.listV.Count];
            Model.worldNormals = new Vector3[Model.listVn.Count];
            Model.textures = new Vector2[Model.listVt.Count];
            if (Model.normalMap != null)
            {
                Model.fileNormals = new Vector3[Model.fileNormalsOrig.GetLength(0), Model.fileNormalsOrig.GetLength(1)];
            }

            var Scale = Matrix4x4.CreateScale(scale);
            var Rotation = Matrix4x4.CreateFromYawPitchRoll(angleY, angleX, angleZ);
            var Translation = Matrix4x4.CreateTranslation(mov);

            Matrix4x4 World = Scale * Rotation * Translation;

            var View = Matrix4x4.CreateLookAt(eye, target, up);
            var Projection = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspect, near, far);

            Matrix4x4 Projection_View_Model = World * View * Projection;

            if (Model.listV.Count > 0)
            {
                Parallel.ForEach(Partitioner.Create(0, Model.listV.Count), range =>
                       {
                           for (int i = range.Item1; i < range.Item2; i++)
                           {
                               Model.worldVertices[i] = Vector3.Transform(Model.listV[i], World);
                               Model.screenVertices[i] = Vector4.Transform(Model.listV[i], Projection_View_Model);
                               Model.screenVertices[i] /= Model.screenVertices[i].W;
                               Model.screenVertices[i] = Vector4.Transform(Model.screenVertices[i], Viewport);
                           }
                       }
                );

                Parallel.ForEach(Partitioner.Create(0, Model.listVn.Count), range =>
                       {
                           for (int i = range.Item1; i < range.Item2; i++)
                           {
                               Model.worldNormals[i] = Vector3.TransformNormal(Model.listVn[i], World);
                           }
                       }
                );

                if (Model.normalMap != null)
                {
                    Parallel.For(0, Model.fileNormalsOrig.GetLength(0), i =>
                        {
                            Parallel.For(0, Model.fileNormalsOrig.GetLength(1), j =>
                                {
                                    Model.fileNormals[i, j] = Vector3.TransformNormal(Model.fileNormalsOrig[i, j], World);
                                }
                            );
                        }
                    );
                }

                Parallel.ForEach(Partitioner.Create(0, Model.listVt.Count), range =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            Model.textures[i] = Model.listVt[i];
                        }
                    }
                );
            }
        }
    }
}
