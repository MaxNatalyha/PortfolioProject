using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCompas : MonoBehaviour
{
    public RectTransform compasTrans;
    public Transform character;

    public void FixedUpdate()
    {
        float angleDelta = Vector3.SignedAngle(Vector3.forward, character.transform.forward, Vector3.up);
        compasTrans.rotation = Quaternion.Euler(0f, 0f, angleDelta);
        //Debug.Log(angleDelta);
    }
}
