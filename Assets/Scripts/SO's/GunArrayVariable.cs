using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGunArrayVariable", menuName = "Variables/Gun Array Variable")]
public class GunArrayVariable : ScriptableObject
{
    public GunSO[] value;
}
