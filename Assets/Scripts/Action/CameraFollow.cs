using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject target;
    public float lerpAmount;

    Vector3 targetPos;
    public Vector3 offSet;

    void Update()
    {
        targetPos = target.transform.position + offSet;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpAmount);
    }
}
