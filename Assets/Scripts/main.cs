using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

    public GameObject player;
    public Camera playerCamera;

    private InputManager inputManager;
    private locomotion playerLocomotion;
    private CameraManager camManager;
    private Transform playerTf;
    private Transform cameraTf;

    private Rigidbody playerRb;

    
	// Use this for initialization
	void Start () {
        inputManager = GetComponent<InputManager>();
        InitializePlayerAndCam();
        
    }
	
	// Update is called once per frame
	void Update () {
        inputManager.updateInputs(ref playerCamera);

    }
    void FixedUpdate()
    {
        updateRigidbodies();
        updateTransforms();
    }

    void updateRigidbodies()
    {
        playerLocomotion.updateRigidbody(ref inputManager);
    }

    void updateTransforms()
    {
        camManager.updateTransform(ref playerTf, ref inputManager);
    }

    void InitializePlayerAndCam()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        Camera[] camList = Camera.allCameras;
        // Player
        if (playerList.Length > 1 || playerList.Length == 0)
        {
            Debug.Log("More than one, or no player found!");
        }
        else
        {
            player = playerList[0];
            playerLocomotion = player.GetComponent<locomotion>();
            playerRb = player.GetComponent<Rigidbody>();
            playerTf = player.GetComponent<Transform>();
        }
        // Camera
        if (camList.Length > 1 || camList.Length == 0)
        {
            Debug.Log("More than one, or no Main Camera found!");
        }
        else
        {
            playerCamera = camList[0];
            cameraTf = camList[0].GetComponent<Transform>();
            camManager = playerCamera.GetComponentInParent<CameraManager>();
            camManager.InitCam(ref playerTf);
            
        }
    }
}