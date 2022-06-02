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

    private float _interactRadius = 2f;
    private bool _inCar;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        controllerType = ControllerType.Character;
        
        SetControll();
        
        if(camera != null)
            cameraFollow = camera.GetComponent<CameraFollow>();
        
        camera.transform.parent = firstPersonViewController.transform;
        camera.transform.localPosition = firstPersonViewController.cameraPosition.localPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameEvents.current.OpenInventory();
            firstPersonViewController.mouseLock = !firstPersonViewController.mouseLock;
        }

        if (CalculateDstCharToCar() < _interactRadius && !_inCar)
        {
            GameEvents.current.ShowNotification("Press E to pay respect");
        }

        
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            Item wood = new Item(ItemTypes.Wood, 250, true);
            GameEvents.current.AddItemInInventory?.Invoke(wood);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Item stone = new Item(ItemTypes.Stone, 250, true);
            GameEvents.current.AddItemInInventory?.Invoke(stone);
        }        
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameEvents.current.RemoveLastItemFromInventory?.Invoke();
        }    
        
        if (Input.GetKey(KeyCode.Escape) && !Application.isEditor)
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.E) && CalculateDstCharToCar() < _interactRadius)
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
        
        _inCar = true;
    }

    private void ExitFromCar()
    {
        firstPersonViewController.transform.parent = null;
        firstPersonViewController.transform.rotation = Quaternion.Euler(Vector3.zero);
        firstPersonViewController.GetComponent<Rigidbody>().isKinematic = false;
        firstPersonViewController.GetComponent<Collider>().enabled = true;

        _inCar = false;
    }

    
    private float CalculateDstCharToCar()
    {
        return Vector3.Distance(firstPersonViewController.transform.position,
            vehicleController.transform.position);
    }
}
