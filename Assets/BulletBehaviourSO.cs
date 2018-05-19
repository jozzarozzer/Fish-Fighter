using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletBehaviour", menuName = "Action/Bullet Behaviour")]
public class BulletBehaviourSO : ScriptableObject {

    [Header("Features")]
    public bool drag;
    public bool wave;
    public float waveMult = 1;

    [Space]
    [Space]

    Vector3 velocity;
    Vector3 position;

    public float dragAmount;

    public Vector3 VelocityUpdate(Vector3 velocityIN)
    {
        velocity = velocityIN;

        if (drag) { velocity = Drag(velocity); }

        return velocity;
    }

    public Vector3 PositionUpdate(Vector3 positionIN, float startTime)
    {
        position = positionIN;

        if (wave) { position = Wave(position, startTime); }

        return position;
    }

    Vector3 Drag(Vector3 velocityIN)
    {
        Vector3 velocityOUT;

        velocityOUT = Vector3.Lerp(velocityIN, Vector3.zero, dragAmount);

        return velocityOUT;
    }

    Vector3 Wave(Vector3 positionIN, float startTime)
    {
        Vector3 positionOUT = positionIN;

        float timeAlive = Time.time - startTime;

        float velAngle = Vector3.Angle(velocity, new Vector3(1, velocity.y, 0)); //tests the angle from the east, doesn't know if the angle is north or south.
        float velPolarTest = Vector3.Angle(velocity, new Vector3(0, velocity.y, 1)); //tests the angle from the north.

        if (velPolarTest > 90) //if the angle is on the south
        {
            velAngle = -velAngle;
        }

        float velNormalAngleClockwise = velAngle - 90;

        Vector3 velocityNormal = new Vector3
        (
        velocity.magnitude * Mathf.Cos(Mathf.Deg2Rad * velNormalAngleClockwise),
        velocity.y,
        velocity.magnitude * Mathf.Sin(Mathf.Deg2Rad * velNormalAngleClockwise)
        );

        Debug.Log(velAngle);
        Debug.Log(velNormalAngleClockwise);
        Debug.Log(velocity);
        Debug.Log(velocityNormal);
        Debug.Log("-----------");

        positionOUT += new Vector3
        ( 
            velocityNormal.x * waveMult * (Mathf.Sin(timeAlive * 25) / 160),
            0,
            velocityNormal.z * waveMult * (Mathf.Sin(timeAlive * 25) / 160)
        );

        return positionOUT;
    }
}
