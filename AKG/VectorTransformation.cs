using System.Numerics;

namespace AKG
{
    public static class VectorTransformation
    {
        public static Vector3 XAxis = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 YAxis = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 ZAxis = new Vector3(1.0f, 1.0f, 1.0f);

        public static void UpdateCameraBasicVectors()
        {
            ZAxis = Vector3.Normalize(TransformMatrix.eye - TransformMatrix.target);
            XAxis = Vector3.Normalize(TransformMatrix.up * ZAxis);
            YAxis = TransformMatrix.up;
        }
        
        //public static void InitVectors()
        //{
        //    for (int i = 0; i < Model.listV.Count; i++)
        //    {
        //        //Vector3.Transform(vector, TransformMatrix.PerspectiveProjection);
        //        //Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.Viewport);
        //        //Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.AnglePerspectiveProjection);
        //        //Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.View);
        //    }
        //}

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

            //Matrix4x4 mx_Model = Matrix4x4.Multiply(
            //    Matrix4x4.Multiply(TransformMatrix.MoveMatrix,
            //        Matrix4x4.Multiply(
            //            Matrix4x4.Multiply(TransformMatrix.RotateMatrixX, TransformMatrix.RotateMatrixY),
            //            TransformMatrix.RotateMatrixZ)),
            //        TransformMatrix.ScaleMatrix);

            //Matrix4x4 Projection_View = Matrix4x4.Multiply(TransformMatrix.AnglePerspectiveProjection, TransformMatrix.View);
            //Matrix4x4 Projection_View_Scale = Matrix4x4.Multiply(Projection_View, TransformMatrix.ScaleMatrix);
            //Matrix4x4 Projection_View_Scale_Rotation_X = Matrix4x4.Multiply(Projection_View_Scale, TransformMatrix.RotateMatrixX);
            //Matrix4x4 Projection_View_Scale_Rotation_Y = Matrix4x4.Multiply(Projection_View_Scale_Rotation_X, TransformMatrix.RotateMatrixY);
            //Matrix4x4 Projection_View_Scale_Rotation_Z = Matrix4x4.Multiply(Projection_View_Scale_Rotation_Y, TransformMatrix.RotateMatrixZ);
            //Matrix4x4 Projection_View_Model = Matrix4x4.Multiply(Projection_View_Scale_Rotation_Z, TransformMatrix.MoveMatrix);

            TransformMatrix.Viewport.M11 = (TransformMatrix.width / 2.0f);
            TransformMatrix.Viewport.M14 = (TransformMatrix.x_min + TransformMatrix.width / 2.0f);
            TransformMatrix.Viewport.M22 = (-TransformMatrix.height / 2.0f);
            TransformMatrix.Viewport.M24 = (-TransformMatrix.y_min + -TransformMatrix.height / 2.0f);

            Matrix4x4 Projection_View = Matrix4x4.Multiply(TransformMatrix.AnglePerspectiveProjection, TransformMatrix.View);
            Matrix4x4 Projection_View_Move = Matrix4x4.Multiply(Projection_View, TransformMatrix.MoveMatrix);
            Matrix4x4 Projection_View_Move_Rotation_X = Matrix4x4.Multiply(Projection_View_Move, TransformMatrix.RotateMatrixX);
            Matrix4x4 Projection_View_Move_Rotation_Y = Matrix4x4.Multiply(Projection_View_Move_Rotation_X, TransformMatrix.RotateMatrixY);
            Matrix4x4 Projection_View_Move_Rotation_Z = Matrix4x4.Multiply(Projection_View_Move_Rotation_Y, TransformMatrix.RotateMatrixZ);
            Matrix4x4 Projection_View_Model = Matrix4x4.Multiply(Projection_View_Move_Rotation_Z, TransformMatrix.ScaleMatrix);


            for (int i = 0; i < Model.listV.Count; i++)
            {
                Model.listV[i] = Vector3.Transform(Model.originalV[i], Projection_View_Model);

                Vector3 tmpPos = Model.listV[i];
                if (Projection_View_Model.M14 != 0)
                    tmpPos.X /= Projection_View_Model.M14;
                if (Projection_View_Model.M14 != 0)
                    tmpPos.Y /= Projection_View_Model.M24;
                if (Projection_View_Model.M14 != 0)
                    tmpPos.Z /= Projection_View_Model.M34;
                Model.listV[i] = tmpPos; 

                Model.listV[i] = Vector3.Transform(Model.listV[i], TransformMatrix.Viewport);
            }
        }
    }
}
