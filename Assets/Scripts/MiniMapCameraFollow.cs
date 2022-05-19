using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MiniMapCameraFollow : MonoBehaviour
{
    public Transform followObject;

    private void LateUpdate()
    {
        transform.position = new Vector3(followObject.position.x, transform.position.y, followObject.position.z);
    }
}
