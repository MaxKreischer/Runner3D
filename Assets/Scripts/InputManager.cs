using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour {

    
    private float _forward;
    public float horizontal;
    private int mouseIdx = 0;
    private const int mouseBufferSize = 20;
    public float[] Mouse_X_Arr = new float[mouseBufferSize];
    public float mouse_x = 0;
    public float[] Mouse_Y_Arr = new float[mouseBufferSize];
    public float mouse_y = 0;
    public bool jumpBtn;
    public Vector3 MousePosWorld;
    public Ray MouseRay;
    private Vector3 mousePos;

    public float Forward { get { return this._forward; }}

    public void updateInputs(ref Camera cam)
    {
        
        _forward     = Input.GetAxis("Vertical");
        horizontal  = Input.GetAxis("Horizontal");
        jumpBtn = Input.GetButton("Jump");
        mousePos = Input.mousePosition;
        MousePosWorld = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        MouseRay = cam.ScreenPointToRay(mousePos);
        


    }

  
}
