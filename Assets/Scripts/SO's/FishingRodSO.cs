using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFishingRod", menuName = "Fishing/Fishing Rod")]
public class FishingRodSO : ScriptableObject
{
    public string rodType;
    public int level;
    public float castStrength;
    public int price;
}
