using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media.Media3D;

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

        public static void UpdateViewPort()
        {
            TransformMatrix.Viewport.M11 = (TransformMatrix.width / 2.0f);
            TransformMatrix.Viewport.M41 = (TransformMatrix.x_min + TransformMatrix.width / 2.0f);
            TransformMatrix.Viewport.M22 = (-TransformMatrix.height / 2.0f);
            TransformMatrix.Viewport.M42 = (TransformMatrix.y_min + TransformMatrix.height / 2.0f);
            TransformMatrix.aspect = TransformMatrix.width / TransformMatrix.height;
        }

        public static void TransformVectors(float angleX, float angleY, float angleZ, float scale, float mov_x, float mov_y, float mov_Z)
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

            Model.model.Clear();

            var View = Matrix4x4.CreateLookAt(TransformMatrix.eye, TransformMatrix.target, TransformMatrix.up);
            var Projection = Matrix4x4.CreatePerspectiveFieldOfView(TransformMatrix.fov, TransformMatrix.aspect, TransformMatrix.near, TransformMatrix.far);

            var Scale = Matrix4x4.CreateScale(scale);
            var Rotation = Matrix4x4.CreateFromYawPitchRoll(angleY, angleX, angleZ);
            var Translation = Matrix4x4.CreateTranslation(new Vector3(mov_x, mov_y, mov_Z));

            //Matrix4x4 Projection_View = Matrix4x4.Multiply(TransformMatrix.AnglePerspectiveProjection, TransformMatrix.View);
            //Matrix4x4 Projection_View_Move = Matrix4x4.Multiply(Projection_View, TransformMatrix.MoveMatrix);
            //Matrix4x4 Projection_View_Move_Rotation_X = Matrix4x4.Multiply(Projection_View_Move, TransformMatrix.RotateMatrixX);
            //Matrix4x4 Projection_View_Move_Rotation_Y = Matrix4x4.Multiply(Projection_View_Move_Rotation_X, TransformMatrix.RotateMatrixY);
            //Matrix4x4 Projection_View_Move_Rotation_Z = Matrix4x4.Multiply(Projection_View_Move_Rotation_Y, TransformMatrix.RotateMatrixZ);
            //Matrix4x4 Projection_View_Model = Matrix4x4.Multiply(Projection_View_Move_Rotation_Z, TransformMatrix.ScaleMatrix);

            Matrix4x4 Projection_View_Model = Scale * Rotation * Translation * View * Projection;


            for (int i = 0; i < Model.listV.Count; i++)
            {
                Model.model.Add(Vector4.Transform(Model.listV[i], Projection_View_Model));

                Model.model[i] /= Model.model[i].W;

                Model.model[i] = Vector4.Transform(Model.model[i], TransformMatrix.Viewport);
            }
        }
    }
}
