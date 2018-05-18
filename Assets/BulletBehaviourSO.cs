using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletBehaviour", menuName = "Action/Bullet Behaviour")]
public class BulletBehaviourSO : ScriptableObject {

    Vector3 velocity;

    public float dragAmount;

    public Vector3 VelocityUpdate(Vector3 velocityIN)
    {
        velocity = velocityIN;

        velocity = Drag(velocity);

        return velocity;
    }

    Vector3 Drag(Vector3 velocityIN)
    {
        Vector3 velocityOUT;

        velocityOUT = Vector3.Lerp(velocityIN, Vector3.zero, dragAmount);

        return velocityOUT;
    }
}
