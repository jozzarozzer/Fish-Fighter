using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float health;
    bool invincible;

    public IntVariable enemyCountVariable;
    

    public EnemySO enemyData;

    public NavMeshAgent agent;

    public GameObjectVariable target;
    public GameObject hitEffect;

    Rigidbody rb;

	void Start ()
    {
        enemyCountVariable.value += 1;
        agent = GetComponent<NavMeshAgent>();
        health = enemyData.health;

        rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        HealthCheck();

        enemyData.MoveTowardsPlayer(agent, target.value.transform.position);

        
	}

    void HealthCheck()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        enemyCountVariable.value -= 1;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.GetComponent<ProjectileScript>())
        {
            var projectileScript = trigger.gameObject.GetComponent<ProjectileScript>();
            if (projectileScript.playerAttack)
            {
                var bulletDamage = projectileScript.bulletData.damage;
                if (!invincible)
                {
                    takeDamage(bulletDamage);
                }
            }
        }
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        int loopCount = 5;
        for (int i = 0; i < loopCount; i++)
        {
            GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(enemyData.invincibilityTime / (2* loopCount));
            GetComponent<MeshRenderer>().enabled = true;
            yield return new WaitForSeconds(enemyData.invincibilityTime / (2 * loopCount));
        }
        invincible = false;
    }

    void takeDamage(float damage)
    {
        StartCoroutine(Invincibility());
        health -= damage;
        if (hitEffect != null)
        {
            GameObject hitParticle = Instantiate(hitEffect, transform);
        }
    }

}
