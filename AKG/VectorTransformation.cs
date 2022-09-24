using System.Numerics;

namespace AKG
{
    public static class VectorTransformation
    {
        public static Vector3 XAxis = new Vector3(120.0f, 0.0f, 0.0f );
        public static Vector3 YAxis = new Vector3(0.0f, 160.0f, 0.0f);
        public static Vector3 ZAxis = new Vector3(200.0f, 0.0f, 200.0f);

        public static void UpdateCameraBasicVectors()
        {
            ZAxis = Vector3.Normalize(TransformMatrix.eye - TransformMatrix.target);
            XAxis = Vector3.Normalize(TransformMatrix.up * ZAxis);
            YAxis = TransformMatrix.up;
        }

        private static void TransformVector(Vector3 vector, Vector3[,] matrix)
        {
/*            vector.X = vector.X * matrix[0, 0] + vector.Y * matrix[0, 1] + vector.Z * matrix[0, 2];
            vector.Y = vector.X * matrix[1, 0] + vector.Y * matrix[1, 1] + vector.Z * matrix[1, 2];
            vector.Z = vector.X * matrix[2, 0] + vector.Y * matrix[2, 1] + vector.Z * matrix[2, 2];*/
        }

        public static void TransformVectors()
        {
            foreach(var vector in Model.listV)
            {
                Vector3.Transform(vector, TransformMatrix.PerspectiveProjection);
                //Vector3.Transform(vector, TransformMatrix.OrthographicProjection);
                Vector3.Transform(vector, TransformMatrix.AnglePerspectiveProjection);
                Vector3.Transform(vector, TransformMatrix.Viewport);
            }
        }
    }
}
