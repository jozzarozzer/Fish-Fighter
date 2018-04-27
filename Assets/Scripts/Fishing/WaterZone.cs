using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    public ZoneSO zone;

    Vector3 startingPosition;

	void Start () {
        startingPosition = transform.position;
	}
	
	void Update () {
        transform.position = new Vector3(startingPosition.x, startingPosition.y + Mathf.Sin(Time.time/1.3f) /5 , startingPosition.z);
	}
}
