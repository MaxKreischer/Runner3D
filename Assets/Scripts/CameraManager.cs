﻿using System.Collections;
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
    private Vector3 mouseMovement;
    private Vector3 camOrigin;
    private float circleValueX;
    private float radius;

    public void InitCam(ref Transform playerTf)
    {
        mouseMovement = new Vector3(0, 0, 0);
        circleValueX = 0;
        radius = 4.0f;
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
        camOrigin = transform.position;

    }


    public void updateTransform(ref Transform playerTf, ref InputManager iM)
    {
        //Move camera such that its position --relative to the player-- stays the same
        /*
        camOrigin = new Vector3(Mathf.Lerp(transform.position.x, playerTf.position.x, Time.deltaTime * 10f),
                                            Mathf.Lerp(transform.position.y, playerTf.position.y + CameraUpShift, Time.deltaTime ), 
                                            Mathf.Lerp(transform.position.z, playerTf.position.z, Time.deltaTime * 10f) );
       */
        camOrigin = new Vector3(playerTf.position.x,playerTf.position.y + CameraUpShift, playerTf.position.z);

        //Movement of the camera around the player
        //Imagine a circle around the player. When we move the mouse to the left, we move the camera on the circle anti-clockwise. If we move the mouse to the right, we move the camera on the circle clockwise.
        mouseMovement = iM.mouseMovement();
        circleValueX -= mouseMovement.x;
        circleValueX = circleValueX % 360f;
       
        transform.position = camOrigin + new Vector3(Mathf.Sin(circleValueX * Mathf.Deg2Rad)*radius, 0, Mathf.Cos(-circleValueX * Mathf.Deg2Rad)*radius);

        //Rotate the camera. It is a little bit fake, because I do not calculate the rotation of the camera to look at the player, but rather just use the "circle movement" from the mouse as above
        transform.rotation = Quaternion.Euler(25f,180 + circleValueX, 0);


        //Rotate camera through mouse control resp. analog stick from the controller
        /*
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
        */
    }
}
