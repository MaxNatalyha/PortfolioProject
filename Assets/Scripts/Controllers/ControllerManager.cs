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

    private float interactRadius = 2f;
    private bool inCar;
    
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        controllerType = ControllerType.Character;
        
        SetControll();
        
        if(camera != null)
            cameraFollow = camera.GetComponent<CameraFollow>();
        
        camera.transform.parent = firstPersonViewController.transform;
        camera.transform.localPosition = firstPersonViewController.cameraPosition.localPosition;
    }

    private void Update()
    {
        if (CalculateDstCharToCar() < interactRadius && !inCar)
        {
            notification.ShowCarSeatNotification();
        }
        else
        {
            notification.HideCarSeatNotification();
        }
        
        if (Input.GetKey(KeyCode.Escape) && !Application.isEditor)
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.F) && CalculateDstCharToCar() < interactRadius)
        {
            if (controllerType == ControllerType.Character)
                controllerType = ControllerType.Vehicle;
            else
                controllerType = ControllerType.Character;

            SetControll();
            SetCamera();
        }
    }
    
    private void SetControll()
    {
        switch (controllerType)
        {
            case ControllerType.Character:
            {
                firstPersonViewController.isActive = true;
                vehicleController.isActive = false;
                ExitFromCar();
            }
                break;
            case ControllerType.Vehicle:
            {
                firstPersonViewController.isActive = false;
                vehicleController.isActive = true;
                SitToCar();
            }
                break;
        }
    }
    
    private void SetCamera()
    {
        switch (controllerType)
        {
            case ControllerType.Character:
            {
                camera.transform.localPosition = firstPersonViewController.cameraPosition.position;
                camera.transform.rotation = firstPersonViewController.transform.rotation;
                camera.transform.parent = firstPersonViewController.transform;
                cameraFollow.enabled = false;
            }
                break;
            case ControllerType.Vehicle:
            {
                camera.transform.parent = null;
                cameraFollow.enabled = true;
            }
                break;
        }
    }

    private void SitToCar()
    {
        firstPersonViewController.GetComponent<Rigidbody>().isKinematic = true;
        firstPersonViewController.GetComponent<Collider>().enabled = false;
        firstPersonViewController.transform.parent = vehicleController.transform;
        firstPersonViewController.transform.localPosition = vehicleController.seatPosition.localPosition;
        firstPersonViewController.transform.rotation = vehicleController.seatPosition.rotation;
        
        inCar = true;
    }

    private void ExitFromCar()
    {
        firstPersonViewController.transform.parent = null;
        firstPersonViewController.transform.rotation = Quaternion.Euler(Vector3.zero);
        firstPersonViewController.GetComponent<Rigidbody>().isKinematic = false;
        firstPersonViewController.GetComponent<Collider>().enabled = true;

        inCar = false;
    }

    
    private float CalculateDstCharToCar()
    {
        return Vector3.Distance(firstPersonViewController.transform.position,
            vehicleController.transform.position);
    }
}
