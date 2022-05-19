using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform objectToFollow;
    public Vector3 offset;
    public float followSpeed = 10f;
    public float lookSpeed = 10f;
    
    public void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 _targetPos = objectToFollow.position + 
                             objectToFollow.forward * offset.z +
                             objectToFollow.right * offset.x + 
                             objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }
}
