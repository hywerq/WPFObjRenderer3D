using System.Numerics;

namespace AKG
{
    public static class VectorTransformation
    {
        public static Vector3 XAxis = new Vector3(10.0f, 0.0f, 0.0f);
        public static Vector3 YAxis = new Vector3(0.0f, 10.0f, 0.0f);
        public static Vector3 ZAxis = new Vector3(0.0f, 0.0f, 10.0f);

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

        public static void TransformVectors()
        {
            //for (int i = 0; i < Model.listV.Count; i++)
            //{
            //    Model.listV[i] = Vector3.Transform(Model.originalV[i], TransformMatrix.RotateMatrixX);
            //    Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.RotateMatrixY);
            //    Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.RotateMatrixZ);

            //    Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.ScaleMatrix);

            //    Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.MoveMatrix);
            //}

            Matrix4x4 mx_Model = Matrix4x4.Multiply(
                Matrix4x4.Multiply(TransformMatrix.MoveMatrix,
                    Matrix4x4.Multiply(
                        Matrix4x4.Multiply(TransformMatrix.RotateMatrixX, TransformMatrix.RotateMatrixY),
                        TransformMatrix.RotateMatrixZ)),
                    TransformMatrix.ScaleMatrix);

            Matrix4x4 Projection_View = Matrix4x4.Multiply(TransformMatrix.AnglePerspectiveProjection, TransformMatrix.View);
            Matrix4x4 Projection_View_Model = Matrix4x4.Multiply(Projection_View, mx_Model);

            for (int i = 0; i < Model.listV.Count; i++)
            {
                Model.listV[i] = Vector3.Transform(Model.originalV[i], Projection_View_Model);

                Vector3 tmpPos = Model.listV[i];
                tmpPos.X /= Projection_View_Model.M14; 
                tmpPos.Y /= Projection_View_Model.M24; 
                tmpPos.Z /= Projection_View_Model.M34;
                Model.listV[i] = tmpPos; 

                Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.Viewport);
            }
        }
    }
}
