using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[RequireComponent(typeof(Inventory))]
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

    private float _currentSpeed = 0f;
    private float _slopeAngle;
    private float _lookMinX = -90f;
    private float _lookMaxX = 90f;
    private bool _isGrounded;
    private Inventory inventory;
    
    public bool isActive;
    public bool mouseLock = true;
    
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
        inventory = GetComponent<Inventory>();
    }
    
    private void Update()
    {
        if (isActive)
        {   
            if(!mouseLock)
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
                                     _currentSpeed * Time.fixedDeltaTime;

                float normalisedSlope = (_slopeAngle / 90f) * -1f;
                debugText2.text = normalisedSlope.ToString();

                moveAmount += (moveAmount * normalisedSlope);

                Move(moveAmount);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();

        if (itemWorld != null)
        {
            GameEvents.current.AddItemInInventory(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    public void MouseLook(Vector2 mouseInput)
    {
        transform.localRotation *= Quaternion.Euler(0f, mouseInput.y * mouseSensetivity * Time.deltaTime, 0f);
        camera.transform.localRotation *= Quaternion.Euler(-mouseInput.x * mouseSensetivity * Time.deltaTime, 0f,0f);

        camera.transform.localRotation = ClampRotationAroundXAxis(camera.transform.localRotation);
    }
    
    public void GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down,out hitInfo, 1.5f, groundMask))
        {
            _slopeAngle = (Vector3.Angle(hitInfo.normal, transform.forward) - 90);

            debugText.text = "Grounded on " + hitInfo.transform.name;
            debugText.text += "\nSlope Angle: " + _slopeAngle.ToString("N0") + "°";
            _isGrounded = true;
        }
        else
        {
            debugText.text = "Not Grounded";
            _isGrounded = false;
        }
    }

    private void UpdateCurrentSpeed(Vector2 input)
    {
        if (!_isGrounded)
        {
            _currentSpeed = inAirSpeed;
            return;
        }

        if (Input.GetButton("Sprint"))
        {
            _currentSpeed = runSpeed;
            return;
        }
        
        if (input.y > 0)
        {
            _currentSpeed = walkSpeed;
        }

        if (input.y < 0)
        {
            _currentSpeed = backwardSpeed;
        }

        if (input.x > 0 || input.x < 0)
        {
            _currentSpeed = strafeSpeed;
        }

    }

    private void Move(Vector3 moveAmount)
    { 
        playerRigidbody.position += moveAmount;
        //playerRigidbody.MovePosition(playerRigidbody.position + moveAmount);
    }

    public void Jump()
    {
        if(_isGrounded)
            playerRigidbody.AddForce((transform.up) * jumpForce, ForceMode.Impulse);
    }
    
    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, _lookMinX, _lookMaxX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
