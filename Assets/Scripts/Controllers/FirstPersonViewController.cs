using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class FirstPersonViewController : MonoBehaviour
{
    [Header("Speed Settings")]
    public float walkSpeed = 10f;
    public float backwardSpeed = 6f;
    public float strafeSpeed = 4f;
    public float runSpeed = 15f;
    public float inAirSpeed = 4f;
    
    
    public float jumpForce = 5f;

    public LayerMask groundMask;
    public Transform cameraPosition;
    public Camera camera;
    
    [Header("Debug UI")]
    public Text debugText;
    public Text debugText2;

    [Header("Mouse sensetivity")]
    [Range(1, 100)] public float mouseSensetivity = 50;

    private Rigidbody playerRigidbody;

    private float currentSpeed = 0f;
    private float slopeAngle;
    private float lookMinX = -90f;
    private float lookMaxX = 90f;
    private bool isGrounded;
    public bool isActive;
    
    private Vector2 MouseInput =>
        new Vector2
        {
            x = Input.GetAxisRaw("Mouse Y"),
            y = Input.GetAxisRaw("Mouse X")
        };

    private Vector2 KeyInput =>
        new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
    
    
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }
    
    public void MouseLook(Vector2 mouseInput)
    {
        transform.localRotation *= Quaternion.Euler(0f, mouseInput.y * mouseSensetivity * Time.deltaTime, 0f);
        camera.transform.localRotation *= Quaternion.Euler(-mouseInput.x * mouseSensetivity * Time.deltaTime, 0f,0f);

        camera.transform.localRotation = ClampRotationAroundXAxis(camera.transform.localRotation);
    }

    private void Update()
    {
        if (isActive)
        {
            MouseLook(MouseInput);
            if (Input.GetButtonDown("Jump"))
                Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            GroundCheck();

            if (!Input.anyKey)
                return;

            if ((Mathf.Abs(KeyInput.x) > float.Epsilon) || (Mathf.Abs(KeyInput.y) > float.Epsilon))
            {
                UpdateCurrentSpeed(KeyInput);
                Vector3 moveAmount = ((transform.forward * KeyInput.y) + (transform.right * KeyInput.x)) *
                                     currentSpeed * Time.fixedDeltaTime;

                float normalisedSlope = (slopeAngle / 90f) * -1f;
                debugText2.text = normalisedSlope.ToString();

                moveAmount += (moveAmount * normalisedSlope);

                Move(moveAmount);

            }
        }
    }

    public void GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down,out hitInfo, 1.5f, groundMask))
        {
            slopeAngle = (Vector3.Angle(hitInfo.normal, transform.forward) - 90);

            debugText.text = "Grounded on " + hitInfo.transform.name;
            debugText.text += "\nSlope Angle: " + slopeAngle.ToString("N0") + "°";
            isGrounded = true;
        }
        else
        {
            debugText.text = "Not Grounded";
            isGrounded = false;
        }
    }

    private void UpdateCurrentSpeed(Vector2 input)
    {
        if (!isGrounded)
        {
            currentSpeed = inAirSpeed;
            return;
        }

        if (Input.GetButton("Sprint"))
        {
            currentSpeed = runSpeed;
            return;
        }
        
        if (input.y > 0)
        {
            currentSpeed = walkSpeed;
        }

        if (input.y < 0)
        {
            currentSpeed = backwardSpeed;
        }

        if (input.x > 0 || input.x < 0)
        {
            currentSpeed = strafeSpeed;
        }

    }

    private void Move(Vector3 moveAmount)
    { 
        playerRigidbody.position += moveAmount;
        //playerRigidbody.MovePosition(playerRigidbody.position + moveAmount);
    }

    public void Jump()
    {
        if(isGrounded)
            playerRigidbody.AddForce((transform.up) * jumpForce, ForceMode.Impulse);
    }
    
    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, lookMinX, lookMaxX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
