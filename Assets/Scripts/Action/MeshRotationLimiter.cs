using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRotationLimiter : MonoBehaviour
{
    public GameObject mesh;
    public float y;
    public Rigidbody rb;

	void Update ()
    {
        y = transform.eulerAngles.y;
        mesh.transform.position = transform.position;

        mesh.transform.LookAt(rb.velocity.normalized + transform.position);

        /*
        //Vector3 rotationNormalized = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y * (4 / 360), transform.localEulerAngles.z);
        Vector3 rotationNormalized = rb.velocity.normalized * 4;
        Vector3 rotationRounded = new Vector3(rotationNormalized.x, Mathf.RoundToInt(rotationNormalized.y) * (360/4), rotationNormalized.z);
        Vector3 rotationReverted = rotationRounded * (360 / 4);
        mesh.transform.eulerAngles = rotationRounded;
        
        
        

        if (135 < y && y < 180)
        {
            mesh.transform.eulerAngles = new Vector3 (mesh.transform.eulerAngles.x, 180, mesh.transform.eulerAngles.z);
        }
        else if (90 < y && y < 135)
        {
            mesh.transform.eulerAngles = new Vector3(mesh.transform.eulerAngles.x, 90, mesh.transform.eulerAngles.z);
        }
        else if (45 < y && y < 90)
        {
            mesh.transform.eulerAngles = new Vector3(mesh.transform.eulerAngles.x, 90, mesh.transform.eulerAngles.z);
        }
        else if (0 < y && y < 45)
        {
            mesh.transform.eulerAngles = new Vector3(mesh.transform.eulerAngles.x, 0, mesh.transform.eulerAngles.z);
        }
        if (-135 > y && y > -180)
        {
            mesh.transform.eulerAngles = new Vector3(mesh.transform.eulerAngles.x, -180, mesh.transform.eulerAngles.z);
        }
        else if (-90 > y && y > -135)
        {
            mesh.transform.eulerAngles = new Vector3(mesh.transform.eulerAngles.x, -90, mesh.transform.eulerAngles.z);
        }
        else if (-45 > y && y > -90)
        {
            mesh.transform.eulerAngles = new Vector3(mesh.transform.eulerAngles.x, -90, mesh.transform.eulerAngles.z);
        }
        else if (-0 > y && y > -45)
        {
            mesh.transform.eulerAngles = new Vector3(mesh.transform.eulerAngles.x, -0, mesh.transform.eulerAngles.z);
        }

    */
    }
}
