using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Action/Gun")]
public class GunSO : ScriptableObject
{
    public string gunType;
    public float fireRate;
    public float recoil;
    public int price;
    public int level;

    public BulletSO bulletType;
}
