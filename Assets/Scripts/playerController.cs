using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  //To use the Input system Libraries in the script

public class playerController : MonoBehaviour
{
    [Header("Movement")]    //To Display the category Headings in bold text in the Inspector pannel
    public float moveSpeed;     //To Define the speed of movement
    private Vector2 movInput;       //using the moveinputs from the Input system

    [Header("Look")]
    public Transform cameraContainer;       //here the camera Container works like the head in a human body. Inorder to make our player look around we are using this.
    public float minXLook;          //Limiting the head rotation to make it feel natural
    public float maxXLook;
    private float camCurXRot;           //Tracking rotating camera in X Axis
    public float lookSensitive;         //head rotation speed

    private Vector2 mouseDelta;      //freehand mouse to prevent holding a button to rotate head to look around

    void LateUpdate()       //There should be a slight delay in canera rotation we want to rotate the camera after the mouse position is changed
    {
        CameraLook();
    }
    void CameraLook()       //Lets set the rotation of the camera up & down for this we will be rotating the camera container.
    {
        camCurXRot += mouseDelta.y * lookSensitive;      //for X rotation we'll be rotating the entire player body
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);       //Now we have to apply this limitation to the camera in the next step
        cameraContainer.localEulerAngles = new Vector3(camCurXRot, 0 , 0);
    }

    public void OnLookInput (InputAction.CallbackContext context)       //To create Look around input
    {
        mouseDelta = context.ReadValue<Vector2>();          //Reading the value of current mouse position
    }

}
