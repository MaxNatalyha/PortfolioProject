using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleController : MonoBehaviour
{

    public Transform centerOfMass;
    
    [Header("Wheels Colliders")]
    public WheelCollider frontRight;
    public WheelCollider frontLeft;
    public WheelCollider backRight;
    public WheelCollider backLeft;

    [Header("Wheels Meshes")]
    public Transform frontRightTransform;
    public Transform frontLeftTransform;
    public Transform backRightTransform;
    public Transform backLeftTransform;

    [Header("VFX")] 
    public ParticleSystem sprintPS;
    public TrailRenderer sprintTrail;
    public TrailRenderer backWheelLeftTrail;
    public TrailRenderer backWheelRightTrail;

    
    [Space(10)]
    public float breakingForce = 300f;
    //public float maxTurnAngle = 15f;

    public float offsetCentre;
    public float innerRadius;
    public float outRadius;
    public bool visualizeSteerRadius;
    public Transform seatPosition;
    
    private float currentEngineTotalPower = 500f;
    private float engineTotalPower = 0f;
    private float currentBreakForce = 0f;
    
    //private float currentTurnAngle = 0f;
    private Rigidbody carRigidBody;

    public float downForceValue = 10f;

    public Text debugText;
    public Text debugText2;
    private float startYpos;

    private void Start()
    {
        carRigidBody = GetComponent<Rigidbody>();
        //carRigidBody.centerOfMass = centerOfMass.localPosition;
        sprintTrail.emitting = false;
        backWheelLeftTrail.emitting = false;
        backWheelRightTrail.emitting = false;
        startYpos = transform.position.y;
    }
    

    public void ControllVehicle(Vector2 keyInput)
    {
        debugText.text = carRigidBody.velocity.ToString();
        debugText2.text = engineTotalPower.ToString();

        //if(Input.GetKeyDown(KeyCode.F))
        //    ChangeSuspension();
        
        if (Input.GetButtonDown("Sprint"))
        {
            carRigidBody.AddForce(transform.forward * 1000f, ForceMode.Impulse);
        }
        
        if (Input.GetButton("Sprint") && backLeft.isGrounded && backRight.isGrounded)
        {
            //carRigidBody.AddForce(transform.forward * 1000f);
            currentEngineTotalPower = 2000f;
            sprintTrail.emitting = true;
        }
        else
        {
            currentEngineTotalPower = 500f;
            sprintTrail.emitting = false;
        }

        if (Input.GetKey(KeyCode.Space) && backLeft.isGrounded && backRight.isGrounded)
        {
            currentBreakForce = breakingForce;
            backWheelLeftTrail.emitting = true;
            backWheelRightTrail.emitting = true;
        }
        else
        {
            currentBreakForce = 0f;
            backWheelLeftTrail.emitting = false;
            backWheelRightTrail.emitting = false;
        }

        engineTotalPower = currentEngineTotalPower * keyInput.y;

        AddDownForce();
        MoveVehicle();
        BrakeVehicle();
        SteerVehicle(keyInput);

        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);
    }

    private void AddDownForce()
    {
        carRigidBody.AddForce(-transform.up * downForceValue * carRigidBody.velocity.magnitude);
    }

    private void CalculateEnginePower()
    {
        
    }

    private void ChangeSuspension()
    {
        frontLeft.suspensionDistance = 1f;
        frontRight.suspensionDistance = 1f;
        backLeft.suspensionDistance = 1f;
        backRight.suspensionDistance = 1f;

        backLeft.center += new Vector3(-0.05f, 0f, 0f);
        backRight.center += new Vector3(0.05f, 0f, 0f);

        carRigidBody.mass = 500f;
        backLeft.mass = 50f;
        backRight.mass = 50f;

        /*
        frontLeft.radius = 1f;
        frontRight.radius = 1f;
        backLeft.radius = 1f;
        backRight.radius = 1f;
        */
    }
    
    private void MoveVehicle()
    {
        //frontRight.motorTorque = currentAcceleration;
        //frontLeft.motorTorque = currentAcceleration;
        
        backLeft.motorTorque = engineTotalPower / 2;
        backRight.motorTorque = engineTotalPower / 2;
    }

    private void BrakeVehicle()
    {
        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
    }

    private void SteerVehicle(Vector2 keyInput)
    {
        //currentTurnAngle = maxTurnAngle * GetInput().y;
        
        if (keyInput.x > 0 ) {
            frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(3f / (5f + frontLeft.radius)) * keyInput.x;
            frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(3f / (5f - frontRight.radius)) * keyInput.x;

        } else if (keyInput.x < 0 ) {                                                          
            frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(3f / (5f - frontLeft.radius)) * keyInput.x;
            frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(3f / (5f + frontRight.radius)) * keyInput.x;
        } else {
            frontLeft.steerAngle = 0;
            frontRight.steerAngle = 0;
        }
        
        
        //frontLeft.steerAngle = currentTurnAngle;
        //frontRight.steerAngle = currentTurnAngle;
    }

    private void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        // Delete
        Vector3 rot = rotation.eulerAngles;
        rot = rot - new Vector3(0, 0, 90);
        rotation = Quaternion.Euler(rot);
        //

        trans.position = position;
        trans.rotation = rotation;
    }

    private void OnDrawGizmos()
    {        
        Debug.DrawRay(frontLeft.transform.position, -frontLeftTransform.up * 2f, Color.red);
        Debug.DrawRay(frontRight.transform.position, frontRightTransform.up * 2f, Color.red);
        Debug.DrawRay(backLeft.transform.position, -backLeftTransform.up * 2f, Color.red);
        Debug.DrawRay(backRight.transform.position, backRightTransform.up * 2f, Color.red);

        
        Debug.DrawRay(transform.position, transform.forward * 10f,Color.green);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(carRigidBody.worldCenterOfMass, .5f);
            
        }

        if (visualizeSteerRadius)
        {
            Vector3 smallRadCentre = backRightTransform.position + backRightTransform.up * offsetCentre;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(smallRadCentre, .5f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(smallRadCentre, innerRadius);
            Gizmos.DrawWireSphere(smallRadCentre, outRadius);
        }
    }

    public void ResetCar()
    {
        transform.position = new Vector3(0, startYpos, 0);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        carRigidBody.velocity = Vector3.zero;
    }
}
