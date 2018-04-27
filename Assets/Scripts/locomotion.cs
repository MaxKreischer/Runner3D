using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomotion : MonoBehaviour {
    Animator animator = new Animator();
    Rigidbody rigi = new Rigidbody();
    
    public float turningRate = 60f;
    public float moveSpeed = 4f;

    private float maxSpeed = 5f;
    private float idletime = 0;
    private float rotAngle;
    private float lookAngle = 0f;
    
    
    
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
    }
	

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "ground")
        {
            animator.SetBool("jumped", false);
            rigi.drag = 0f;
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "ground")
        {
            animator.SetBool("jumped", false);
            rigi.drag = 999f;
            
        }
    }

    public void updateRigidbody(ref InputManager iM)
    {
        if(Mathf.Abs(iM.horizontal) > 0.01f || Mathf.Abs(iM.Forward) > 0.01f)
        {
            rigi.drag = 0f;
            rigi.angularDrag = 0f;
        }
        else
        {
            rigi.drag = 999f;
        }
        lookAngle += iM.horizontal * Time.deltaTime;
        if(lookAngle > 360 || lookAngle <-360)
        {
            lookAngle = 0f;
        }
        //Vector3 camForwardProjection = new Vector3(camTransform.position.x, 0f, camTransform.position.z);
        transform.rotation = Quaternion.Euler(0f, lookAngle*turningRate, 0f);
        rigi.velocity += transform.forward * iM.Forward * moveSpeed / Time.deltaTime;
        //rigi.velocity += new Vector3(rigi.velocity.x, rigi.velocity.y, forward * moveSpeed / Time.deltaTime);
        if (rigi.velocity.magnitude > maxSpeed)
        {
            rigi.velocity = rigi.velocity.normalized * maxSpeed;

        }
    }

    
}
/*
        if (Input.GetKey(KeyCode.W))
        {
            rigi.velocity += transform.forward * 10f * Time.deltaTime; 
            idletime = 0; 
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigi.velocity += transform.forward * -10f * Time.deltaTime;
            idletime = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0f, 30f*Time.deltaTime, 0f);
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles += new Vector3(0f, -30f * Time.deltaTime, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("jumped", true);
            rigi.velocity += new Vector3(0f, 10f, 0f);
            idletime = 0;
        }
       
        if (Mathf.Abs(rigi.velocity.z) > maxSpeed)
        {
            rigi.velocity = new Vector3(rigi.velocity.x, rigi.velocity.y, Mathf.Sign(rigi.velocity.z) * maxSpeed);
        }
        animator.SetFloat("speedPercent", Mathf.Abs(rigi.velocity.z)/maxSpeed);
        //transform.position += new Vector3(0f, 0f, -2f);
        //Debug.Log(transform.position);
        */


/*
    forwardSpeed = Input.GetAxis("Vertical");
    horizSpeed = Input.GetAxis("Horizontal");
    jumpAxis = Input.GetAxis("Jump");
    directionCtrlVec = new Vector3(horizSpeed, 0f, forwardSpeed).normalized;

    rotAngle = Mathf.Atan2(horizSpeed, forwardSpeed)*Mathf.Rad2Deg;

    transform.rotation.ToAngleAxis(out angle, out axis);
    angle = angle * Mathf.Deg2Rad;


    CalculateRotationMatrixRad(angle, "y", ref RotMat);
    animator.SetFloat("speedPercent", rigi.velocity.magnitude / maxSpeed);

    // Mixed motion
    if ( (Mathf.Abs(forwardSpeed) > fwdVelThresh) && (Mathf.Abs(horizSpeed) > fwdVelThresh) )
    {
        animator.SetBool("fwdIsZero", false);
        rigi.velocity += RotateVec(new Vector3(0f, 0f, forwardSpeed), RotMat);
        transform.eulerAngles += new Vector3(0f, turningRate * Time.deltaTime*Mathf.Sign(horizSpeed), 0f);

    }
    // Strict sideways motion
    else if ((Mathf.Abs(forwardSpeed) < fwdVelThresh) && (Mathf.Abs(horizSpeed) > fwdVelThresh))
    {
        animator.SetBool("fwdIsZero", false);
        transform.eulerAngles += new Vector3(0f, turningRate * Time.deltaTime * Mathf.Sign(horizSpeed), 0f);
    }
    //Strict forwards motion
    else if ((Mathf.Abs(forwardSpeed) > fwdVelThresh) && (Mathf.Abs(horizSpeed) < fwdVelThresh))
    {
        animator.SetBool("fwdIsZero", false);
        rigi.velocity += RotateVec(new Vector3(0f, 0f, forwardSpeed), RotMat);
    }
    else if (Mathf.Abs(forwardSpeed) <= fwdVelThresh)
    {
        if (rigi.velocity.magnitude > 1f)
        {
            rigi.velocity = new Vector3(0f, 0f, 0f);
        }
        else
        {
            animator.SetBool("fwdIsZero", true);
        }

    }

    if (jumpAxis > 0f)
        {
            animator.SetTrigger("jumpTrigger");
            float x_vel = rigi.velocity.x;
            float z_vel = rigi.velocity.z;
            rigi.AddForce(new Vector3(x_vel*50f, 300f, z_vel*50f), ForceMode.Impulse);
        }
        if(transform.position.y > 0.15f)
        {
            rigi.AddForce(new Vector3(0f, -100f, 0f), ForceMode.Impulse);
        }
 */
