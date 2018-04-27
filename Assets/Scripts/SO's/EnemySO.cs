using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Action/Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyType;

    public float health;
    public int damage;
    public float speed;
    public float invincibilityTime;

    public void MoveTowardsPlayer(NavMeshAgent agent, Vector3 target)
    {
        agent.speed = speed;
        agent.SetDestination(target);       
    }
}
