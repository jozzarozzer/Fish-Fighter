using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    public float rotationAmount;

    private void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (Random.value - 0.5f) * 360, transform.eulerAngles.z);

    }

    // Update is called once per frame
    void Update () {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotationAmount, transform.eulerAngles.z);
	}
}
