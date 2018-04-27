using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float CameraBackShift = 1f;
    public float CameraSideShift = 2f;
    public float CameraUpShift = 0.5f;

    private float zOffset = -0.75f;
    private Quaternion InitialCamRot;
    private Vector3 RotAngles;
    private Vector3 upDir, forwDir;

    public void InitCam(ref Transform playerTf)
    {
        transform.position = playerTf.position + new Vector3(0f, CameraUpShift, -CameraBackShift);
        Vector3 toTarget = playerTf.position - transform.position;
        Vector3 toTargetXRot = new Vector3(0f, toTarget.y, toTarget.z);
        Vector3 toTargetYRot = new Vector3(toTarget.x, 0f, toTarget.z);
        Vector3 toTargetZRot = new Vector3(toTarget.x, toTarget.y, 0f);

        float yAngle = Mathf.Acos(Vector3.Dot(new Vector3(0f, 0f, 1f), toTargetYRot.normalized)) * Mathf.Rad2Deg;
        //float xAngle = Vector3.Angle(transform.up, toTargetXRot);
        //float yAngle = Vector3.Angle(new Vector3(0f,0f,1f), toTargetYRot);
        //float zAngle = Vector3.Angle(transform.right, toTargetZRot);
        transform.rotation = Quaternion.Euler(25f, 0f, 0f);
        InitialCamRot = transform.rotation;
    }


    public void updateTransform(ref Transform playerTf, ref InputManager iM)
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerTf.position.x, Time.deltaTime * 10f),
                                            transform.position.y, 
                                            Mathf.Lerp(transform.position.z, playerTf.position.z, Time.deltaTime * 10f) + zOffset);


        
        forwDir = iM.MousePosWorld;
        //upDir = Vector3.;
        RotAngles = Quaternion.LookRotation(iM.MouseRay.direction).eulerAngles;
        if(RotAngles.x > 25)
        {
            transform.rotation = Quaternion.Euler(25f, RotAngles.y, 0f);
        }else if(RotAngles.y > 45)
        {
            transform.rotation = Quaternion.Euler(RotAngles.y, 45f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(forwDir, new Vector3(0,1,0));
        }


        //transform.rotation = Quaternion.Euler(iM.mouse_y, iM.mouse_x,0f);
        
    }
}
