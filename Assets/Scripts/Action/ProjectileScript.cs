using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    public BulletSO bulletData;

    //public Attack attack;
    public float damage;
    public bool playerAttack;
    public bool enemyAttack;
    public bool attackCollidersOn;

    public Rigidbody rigidBody;
    public float lifetime;
    public float velocityMult;
    Vector3 velocity = Vector3.zero;

    public Vector3 rotation;

    void Start()
    {
        /*
        damage = attack.damage;
        playerAttack = attack.playerAttack;
        enemyAttack = attack.enemyAttack;
        */

        rigidBody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(deathTimer(lifetime));
        transform.eulerAngles += rotation;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 10)
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCollidersOn)
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
        rigidBody.velocity = velocity;
    }

    public void SetVelocity(Vector3 direction)
    {
        velocity = direction.normalized * velocityMult;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public IEnumerator deathTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Die();
    }
}
