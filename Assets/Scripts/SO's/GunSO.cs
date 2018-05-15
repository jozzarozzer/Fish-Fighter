using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Action/Gun")]
public class GunSO : ScriptableObject
{
    public string gunType;
    public float fireRate; //time between being able to input and gun shoots. Make sure it is longer than the burstFireRate * burstFireAmount;
    public float burstFireRate; //time between bullets when burst firing
    public int burstFireAmount;
    public float recoil;
    public int price;
    public int level;
    public int ammoMax;
    public int reloadTime;

    public bool canHold;
    public bool burstFire;

    public BulletSO bulletType;
}
