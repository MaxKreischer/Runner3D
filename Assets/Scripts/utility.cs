using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utility
{
    void CalculateRotationMatrixRad(float angle, string axis, ref Matrix4x4 Mat)
    {
        //Angle in Radians!

        if (axis == "x")
        {
            Mat.SetColumn(0, new Vector4(1f, 0f, 0f, 0f));
            Mat.SetColumn(1, new Vector4(Mathf.Cos(angle), Mathf.Sin(angle), 0f, 0f));
            Mat.SetColumn(2, new Vector4(-Mathf.Sin(angle), Mathf.Cos(angle), 0f, 0f));
            Mat.SetColumn(3, new Vector4(0f, 0f, 0f, 0f));
        }
        else if (axis == "y")
        {
            Mat.SetColumn(0, new Vector4(Mathf.Cos(angle), 0f, -Mathf.Sin(angle), 0f));
            Mat.SetColumn(1, new Vector4(0f, 1f, 0f, 0f));
            Mat.SetColumn(2, new Vector4(Mathf.Sin(angle), 0f, Mathf.Cos(angle), 0f));
            Mat.SetColumn(3, new Vector4(0f, 0f, 0f, 0f));
        }
        else if (axis == "z")
        {
            Mat.SetColumn(0, new Vector4(Mathf.Cos(angle), Mathf.Sin(angle), 0f, 0f));
            Mat.SetColumn(1, new Vector4(-Mathf.Sin(angle), Mathf.Cos(angle), 0f, 0f));
            Mat.SetColumn(2, new Vector4(0f, 0f, 1f, 0f));
            Mat.SetColumn(3, new Vector4(0f, 0f, 0f, 0f));
        }
        else { Debug.Log("No valid axis identifier found!"); }

    }


    Vector3 RotateVec(Vector3 vec, Matrix4x4 Mat)
    {
        Vector4 tmpVec = new Vector4(vec.x, vec.y, vec.z, 0f);
        tmpVec = Mat.MultiplyVector(tmpVec);
        return new Vector3(tmpVec.x, tmpVec.y, tmpVec.z);
    }

}
