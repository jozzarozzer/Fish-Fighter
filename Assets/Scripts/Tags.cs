using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    [Header("Generic")]
    public bool terrain;
    public bool player;
    public bool cam;
    public bool nonSolid;

    [Header("Action Area")]    
    public bool enemy;
    public bool boss;
    public bool bullet;
    public bool piercedByBullets;


    [Header("Fishing Area")]
    public bool waterZone;
}
