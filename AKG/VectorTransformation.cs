using System.Numerics;

namespace AKG
{
    public static class VectorTransformation
    {
        public static Vector3 XAxis = new Vector3(120.0f, 0.0f, 0.0f);
        public static Vector3 YAxis = new Vector3(0.0f, 160.0f, 0.0f);
        public static Vector3 ZAxis = new Vector3(200.0f, 0.0f, 200.0f);

        public static void UpdateCameraBasicVectors()
        {
            ZAxis = Vector3.Normalize(TransformMatrix.eye - TransformMatrix.target);
            XAxis = Vector3.Normalize(TransformMatrix.up * ZAxis);
            YAxis = TransformMatrix.up;
        }
        
        public static void InitVectors()
        {
            for (int i = 0; i < Model.listV.Count; i++)
            {
                //Vector3.Transform(vector, TransformMatrix.PerspectiveProjection);
                //Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.Viewport);
                //Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.AnglePerspectiveProjection);
                //Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.View);
            }
        }

        public static void TransformVectorsRotateX()
        {
            for (int i = 0; i < Model.listV.Count; i++)
            {
                Model.listV[i] = Vector3.Transform(Model.originalV[i], TransformMatrix.RotateMatrixX);
            }
        }

        public static void TransformVectorsRotateY()
        {
            for (int i = 0; i < Model.listV.Count; i++)
            {
                Model.listV[i] = Vector3.Transform(Model.originalV[i], TransformMatrix.RotateMatrixY);
            }
        }

        public static void TransformVectorsRotateZ()
        {
            for (int i = 0; i < Model.listV.Count; i++)
            {
                Model.listV[i] = Vector3.Transform(Model.originalV[i], TransformMatrix.RotateMatrixZ);
            }
        }

        public static void TransformVectorsScale()
        {
            for (int i = 0; i < Model.listV.Count; i++)
            {
                Model.listV[i] = Vector3.Transform(Model.originalV[i], TransformMatrix.ScaleMatrix);
            }
        }
    }
}
