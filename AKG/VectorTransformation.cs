using System;
using System.Numerics;
using System.Threading.Tasks;

namespace AKG
{
    public static class VectorTransformation
    {
        public static Vector3 XAxis = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 YAxis = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 ZAxis = new Vector3(1.0f, 1.0f, 1.0f);

        public static Vector3 eye = new Vector3(0.0f, 0.0f, 100.0f);
        public static Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
        public static Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

        public static Vector3 light = new Vector3(-100.0f, 5.0f, 0.0f);

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

        public static void TransformVectors(float angleX, float angleY, float angleZ, float scale, float mov_x, float mov_y, float mov_Z)
        {
            Model.model = new Vector4[Model.listV.Count];

            var View = Matrix4x4.CreateLookAt(eye, target, up);
            var Projection = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspect, near, far);

            var Scale = Matrix4x4.CreateScale(scale);
            var Rotation = Matrix4x4.CreateFromYawPitchRoll(angleY, angleX, angleZ);
            var Translation = Matrix4x4.CreateTranslation(new Vector3(mov_x, mov_y, mov_Z));

            Matrix4x4 Projection_View_Model = Scale * Rotation * Translation * View * Projection;

            Parallel.For(0, Model.listV.Count,
                   (i) =>
                   {
                       Model.model[i] = Vector4.Transform(Model.listV[i], Projection_View_Model);
                       Model.model[i] /= Model.model[i].W;
                       Model.model[i] = Vector4.Transform(Model.model[i], Viewport);
                   }
                );
        }
    }
}
