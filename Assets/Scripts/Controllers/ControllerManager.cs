using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public enum ControllerType
    {
        Character,
        Vehicle
    }
    public ControllerType controllerType;

    public Camera camera;
    private CameraFollow cameraFollow;

    [Header("Controller Ref")]
    public FirstPersonViewController firstPersonViewController;
    public VehicleController vehicleController;
    public UiNotification notification;

    private bool inCar;
    
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

    private float CalculateDstCharToCar()
    {
        return Vector3.Distance(firstPersonViewController.transform.position,
            vehicleController.transform.position);
    }

    private void Start()
    {
        if(camera != null)
            cameraFollow = camera.GetComponent<CameraFollow>();
        
        camera.transform.parent = firstPersonViewController.transform;
        camera.transform.localPosition = firstPersonViewController.cameraPosition.localPosition;
    }

    private void ChangeControll()
    {
        if (controllerType == ControllerType.Character)
            controllerType = ControllerType.Vehicle;
        else
            controllerType = ControllerType.Character;

        UpdateCamera();
    }

    private void UpdateCamera()
    {
        switch (controllerType)
        {
            case ControllerType.Character:
            {
                camera.transform.localPosition = firstPersonViewController.cameraPosition.position;
                camera.transform.rotation = firstPersonViewController.transform.rotation;
                camera.transform.parent = firstPersonViewController.transform;
                
                firstPersonViewController.transform.parent = null;
                firstPersonViewController.transform.rotation = Quaternion.Euler(Vector3.zero);
                firstPersonViewController.GetComponent<Rigidbody>().isKinematic = false;
                firstPersonViewController.GetComponent<Collider>().enabled = true;
                
                cameraFollow.enabled = false;

                inCar = false;
            } break;
            case ControllerType.Vehicle:
            {
                camera.transform.parent = null;

                
                firstPersonViewController.GetComponent<Rigidbody>().isKinematic = true;
                firstPersonViewController.GetComponent<Collider>().enabled = false;
                firstPersonViewController.transform.parent = vehicleController.transform;
                firstPersonViewController.transform.localPosition = vehicleController.seatPosition.localPosition;
                firstPersonViewController.transform.rotation = vehicleController.seatPosition.rotation;
                
                cameraFollow.enabled = true;

                inCar = true;
            } break;
        }
    }

    private void Update()
    {
        if (CalculateDstCharToCar() < 2f && !inCar)
        {
            if(Input.GetKeyDown(KeyCode.F))
                ChangeControll();
            
            notification.ShowCarSeatNotification();
        }
        else
        {
            notification.HideCarSeatNotification();
        }

        if(Input.GetKey(KeyCode.Escape) && !Application.isEditor)
            Application.Quit();
        
        switch (controllerType)
        {
            case ControllerType.Character:
            {
                firstPersonViewController.MouseLook(MouseInput);
                if(Input.GetButtonDown("Jump"))
                    firstPersonViewController.Jump();
            } break;
            case ControllerType.Vehicle:
            {
                if(Input.GetKeyDown(KeyCode.X))
                    vehicleController.ResetCar();
            } break;
        }
    }

    private void FixedUpdate()
    {
        switch (controllerType)
        {
            case ControllerType.Character:
            {
                firstPersonViewController.CharacterControll(KeyInput);
            } break;
            case ControllerType.Vehicle:
            {
                vehicleController.ControllVehicle(KeyInput);
            } break;
        }
    }
}
