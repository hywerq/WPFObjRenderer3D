using System;
using System.Numerics;
using static AKG.VectorTransformation;

namespace AKG
{
    public static class TransformMatrix
    {
        public static Vector3 eye = new Vector3(0.0f, 0.0f, 0.0f);
        public static Vector3 target = new Vector3(5.0f, 0.0f, 0.0f);
        public static Vector3 up = new Vector3(0.0f, 0.0f, 0.0f);

        public static float width;
        public static float height;

        private static float near = 1.0f;
        private static float far = 6.0f;

        private static float x_min = 3.0f;
        private static float y_min = 3.0f;

        private static float fov = 3.0f;
        private static float aspect = 3.0f;

        public static Matrix4x4 View = new
        (
            XAxis.X, XAxis.Y, XAxis.Z, -(XAxis.X * eye.X + XAxis.Y * eye.Y + XAxis.Z * eye.Z),
            YAxis.X, YAxis.Y, YAxis.Z, -(YAxis.X * eye.X + YAxis.Y * eye.Y + YAxis.Z * eye.Z),
            ZAxis.X, ZAxis.Y, ZAxis.Z, -(ZAxis.X * eye.X + ZAxis.Y * eye.Y + ZAxis.Z * eye.Z),
            0, 0, 0, 1
        );

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

        public static Matrix4x4 RotateMatrixX = new(
            1.0f, 0, 0, 0,
            0, 1.0f, 0, 0,
            0, 0, 1.0f, 0,
            0, 0, 0, 1.0f
        );

        public static Matrix4x4 RotateMatrixY = new(
            1.0f, 0, 0, 0,
            0, 1.0f, 0, 0,
            0, 0, 1.0f, 0,
            0, 0, 0, 1.0f
        );

        public static Matrix4x4 RotateMatrixZ = new(
            1.0f, 0, 0, 0,
            0, 1.0f, 0, 0,
            0, 0, 1.0f, 0,
            0, 0, 0, 1.0f
        );

        public static Matrix4x4 ScaleMatrix = new(
            1.0f, 0, 0, 0,
            0, 1.0f, 0, 0,
            0, 0, 1.0f, 0,
            0, 0, 0, 1.0f
        );

        public static Matrix4x4 MoveMatrix = new(
            1.0f, 0, 0, 0,
            0, 1.0f, 0, 0,
            0, 0, 1.0f, 0,
            0, 0, 0, 1.0f
        );
    }
}
