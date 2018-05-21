using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    public BulletSO bulletData;

    public GameObject terrainHitFX;

    //public Attack attack;
    public bool playerAttack;
    public bool enemyAttack;
    public bool bounce;

    public Rigidbody rigidBody;
    public float lifetime;
    Vector3 velocity = Vector3.zero;

    public Vector3 rotation;

    Vector3 collidePosition;
    Vector3 positionLastFrame;
    Vector3[] previousPositions = new Vector3[10];
    Vector3 startPosition;
    Vector3 castPosition;

    public LayerMask terrainMask;

    float startTime;

    

    int enemiesPierced;

    bool customBehaviour;
    public BulletBehaviourSO bulletBehaviour;

    void Start()
    {
        lifetime = bulletData.lifeTime;

        rigidBody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(deathTimer(lifetime));
        transform.eulerAngles += rotation;

        if (GetComponent<AudioSend>())
        {
            GetComponent<AudioSend>().SendAudio();
        }

        enemiesPierced = 0;

        startPosition = transform.position;
        castPosition = startPosition;



        if (bulletBehaviour != null) { customBehaviour = true; }
        else { customBehaviour = false; }

        startTime = Time.time;        
    }

    void Update()
    {
        if (customBehaviour)
        {
            velocity = bulletBehaviour.VelocityUpdate(velocity);
            rigidBody.position = bulletBehaviour.PositionUpdate(rigidBody.position, startTime);
        }       

        rigidBody.velocity = velocity;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < previousPositions.Length; i++)
        {
            if (i == 0)
            {
                previousPositions[i] = Vector3.zero;
            }
            else
            {
                previousPositions[i - 1] = previousPositions[i];
            }
        }
        previousPositions[previousPositions.Length - 1] = transform.position;
    }

    public void SetVelocity(Vector3 direction)
    {
        velocity = direction.normalized * bulletData.velocity;
    }

    public IEnumerator deathTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        collidePosition = collider.gameObject.transform.position;

        if (collider.gameObject.GetComponent<Tags>())
        {
            if (collider.gameObject.GetComponent<Tags>().terrain)
            {
                HitTerrain(collider);
            }
            else if (collider.gameObject.GetComponent<Tags>().enemy)
            {
                HitEnemy(collider.gameObject);
            }
        }
        else
        {
            //Die();
        }

        
    }

    void HitEffect(GameObject hitEffectIN)
    {
        GameObject hitEffect = Instantiate(hitEffectIN, transform.position, Quaternion.identity);
        hitEffect.transform.LookAt(previousPositions[1]);

        //could possibly instead just make the hit 
        //effects point in the direction of the velocity, 
        //and then if I want one (like this one) to be 
        //inversed, just do it in the prefab.
    }

    void HitTerrain(Collider terrainCollider)
    {
        HitEffect(terrainHitFX);

        if (bounce)
        {
            Bounce(terrainCollider);
        }
        else
        {
            Die();
        }
        

        

        //Die();
    }

    void Bounce(Collider terrainCollider)
    {
        Vector3 normal;

        Vector3 approxNormal = transform.position - terrainCollider.ClosestPointOnBounds(transform.position);
        RaycastHit hit;

        castPosition = previousPositions[2];

        var dir = transform.position - castPosition;

        if (Physics.Raycast(castPosition, dir, out hit, 1000, terrainMask)) //lower range after debugging is done
        {
            if (hit.collider.gameObject.GetComponent<Tags>() != null && hit.collider.gameObject.GetComponent<Tags>().terrain)
            {
                normal = hit.normal;

                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                Vector3 reflectVelocity = Vector3.Reflect(velocity, normal);

                velocity = new Vector3(reflectVelocity.x, velocity.y, reflectVelocity.z);
            }
        }
    }
    
    void HitEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<Tags>().piercedByBullets)
        {
            return;
        }
        if (bulletData.piercesEnemies && enemiesPierced < bulletData.enemyPierceAmount)
        {
            enemiesPierced += 1; //The i-frames of the enemy should protect from multiple measurements of the same collider
        }
        else
        {
            Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPosition, transform.position);
    }
}
