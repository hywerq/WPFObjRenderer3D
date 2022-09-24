using System;
using System.Numerics;
using System.Windows.Controls;
using static AKG.VectorTransformation;

namespace AKG
{
    public static class TransformMatrix
    {
        public static Vector3 eye = new Vector3(600.0f, 700.0f, 0.0f);
        public static Vector3 target = new Vector3(500.0f, 100.0f, 0.0f);
        public static Vector3 up = new Vector3(1000.0f, 0.0f, 100.0f);

        private static Vector3 zero = new Vector3(0.0f, 0.0f, 0.0f);
        private static Vector3 one = new Vector3(1.0f, 1.0f, 1.0f);

        public static float width;
        public static float height;
        
        private static float near = 100.0f;
        private static float far = 600.0f;

        private static float x_min = 300.0f;
        private static float y_min = 300.0f;

        private static float fov = 300.0f;
        private static float aspect = 300.0f;

        public static Vector3[,] ObserverSpace = 
        {
            { XAxis, XAxis, XAxis, -(XAxis * eye) },
            { YAxis, YAxis, YAxis, -(YAxis * eye) },
            { ZAxis, ZAxis, ZAxis, -(ZAxis * eye) },
            { zero, zero, zero, one },
        };

        public static Matrix4x4 OrthographicProjection = new (
            (2.0f / width), 0, 0, 0,
            0, (2.0f / height), 0, 0,
            0, 0, (1 / (near - far)), (near / (near - far)),
            0, 0, 0, 1
        );

        public static Matrix4x4 PerspectiveProjection = new(
            (2.0f * near / width), 0, 0, 0,
            0, (2.0f * near / height), 0, 0,
            0, 0, (far / (near - far)), (near * far/ (near - far)),
            0, 0, -1.0f, 0
        );

        public static Matrix4x4 AnglePerspectiveProjection = new(
            (1.0f / aspect * (float)Math.Tan(fov / 2)), 0, 0, 0,
            0, (1.0f / (float)Math.Tan(fov / 2)), 0, 0,
            0, 0, (far / (near - far)), (near * far / (near - far)),
            0, 0, -1.0f, 0
        );

        public static Matrix4x4 Viewport = new(
            (width / 2.0f), 0, 0, (x_min + width / 2.0f),
            0, (-height / 2.0f), 0, (y_min + height / 2.0f),
            0, 0, 1.0f, 0,
            0, 0, 0, 1.0f
        );

        /*        public static float[,] EulerBrick =
        {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };*/
    }
}
