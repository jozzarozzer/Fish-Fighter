using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewZone", menuName = "Fishing/Zone")]
public class ZoneSO : ScriptableObject
{
    public string zoneType;

    public FishSO[] fishArray;
}
