using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playrcontrol : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMoveInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform head;
    public float minLook;
    public float maxLook;
    private float headCurRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    // components
    private Rigidbody rb;

    private void Awake()
    {   //get our components
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //To lock/hide the cursor and prevent accidental clicks
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }
    
    void Move()
    {
        Vector3 dir = transform.forward * curMoveInput.y+transform.right * curMoveInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;      //Se we can be able to fall, jump without modifing the Y velocity
                                    //Now we need to add this to the Velocity
        rb.velocity = dir;          //Now assignt the move keys in the events/main and test
    }

    void CameraLook()
    {
        headCurRot += mouseDelta.y * lookSensitivity;
        headCurRot = Mathf.Clamp(headCurRot, minLook, maxLook);
        head.localEulerAngles = new Vector3(-headCurRot, 0, 0);  //To Look up & down.
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);  
    }

    public  void OnLookInput (InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }   //Look Input

    public void OnMoveInput (InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMoveInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
        }
    }

    //Called when Spacebar is pressed down (Managed by Input System
    public void OnJumpInput (InputAction.CallbackContext context)
    {
        //Checks whether this is the first frame we're pressing the button or not
        if(context.phase == InputActionPhase.Started)
        {
            //Checks whether we are standing on ground or not
            if (IsGrounded())
            {
                //If yes, add force in the upward direction
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
    bool IsGrounded()
    {
        //Here we need to shoot 4 raycasts to detect the surface/ground
        Ray[] rays = new Ray[4]
        /*{
            new Ray(transform.position + (transform.forward * 0.2f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f), Vector3.down),
        };  */      // These are shot right from the bottom of the player (Capsule Collider)
        
        //Lets raise the raycast statrting point
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.03f), Vector3.down),      //Here we have raised each ray by 3cms i.e, 0.03f
            new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.03f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.03f), Vector3.down),
            new Ray(transform.position +(-transform.right * 0.2f) +(Vector3.up * 0.03f), Vector3.down),
        };

        for(int i = 0; i<rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))  //Number of Rays, max distance (0.1f means 10 cms), Layer mask to apply
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;   //By default, the rays will be in whits color. so we can change the color by adding this line.

        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }
}
