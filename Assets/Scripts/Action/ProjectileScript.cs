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


    int enemiesPierced;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(deathTimer(lifetime));
        transform.eulerAngles += rotation;

        GetComponent<AudioSend>().SendAudio();

        enemiesPierced = 0;
    }

    void Update()
    {
        rigidBody.velocity = velocity;
    }

    private void LateUpdate()
    {
        positionLastFrame = transform.position;
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
                HitTerrain();
            }
            else if (collider.gameObject.GetComponent<Tags>().enemy)
            {
                HitEnemy(collider.gameObject);
            }
        }
        else
        {
            Die();
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

    void HitTerrain()
    {
        HitEffect(terrainHitFX);
        Die();
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
}
