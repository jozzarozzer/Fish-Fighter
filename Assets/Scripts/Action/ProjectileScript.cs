using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    public BulletSO bulletData;

    public GameObject terrainHitFX;

    //public Attack attack;
    public float damage;
    public bool playerAttack;
    public bool enemyAttack;

    public Rigidbody rigidBody;
    public float lifetime;
    public float velocityMult;
    Vector3 velocity = Vector3.zero;

    public Vector3 rotation;

    Vector3 collidePosition;
    Vector3 positionLastFrame;
    Vector3[] previousPositions = new Vector3[10];
    Vector3 startPosition;
    Vector3 castPosition;

    public LayerMask terrainMask;

    int enemiesPierced;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(deathTimer(lifetime));
        transform.eulerAngles += rotation;

        GetComponent<AudioSend>().SendAudio();

        enemiesPierced = 0;

        startPosition = transform.position;
        castPosition = startPosition;
    }

    void Update()
    {
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
        velocity = direction.normalized * velocityMult;
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
        hitEffect.transform.LookAt(positionLastFrame);
        //could possibly instead just make the hit 
        //effects point in the direction of the velocity, 
        //and then if I want one (like this one) to be 
        //inversed, just do it in the prefab.
    }

    void HitTerrain(Collider terrainCollider)
    {
        HitEffect(terrainHitFX);

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

                transform.position = new Vector3 (hit.point.x, transform.position.y, hit.point.z);

                Vector3 reflectVelocity = Vector3.Reflect(velocity, normal);

                velocity = new Vector3 (reflectVelocity.x, velocity.y, reflectVelocity.z);
            }
        }

        

        //Die();
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
