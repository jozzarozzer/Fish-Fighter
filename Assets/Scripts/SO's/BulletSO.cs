using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "Action/Bullet")]
public class BulletSO : ScriptableObject
{
    public string bulletType;
    public float damage;
    public float velocity;
    public float lifeTime;

    public bool multiBullet;
    public bool piercesEnemies;
    public int enemyPierceAmount;

    public GameObject bulletObj;
    public Mesh bulletMesh;

    //public sprite/mesh
}
