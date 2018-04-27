using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quality
{
    trash, common, rare, epic, legendary
}

[CreateAssetMenu(fileName = "NewFish", menuName = "Fishing/Fish")]
public class FishSO : ScriptableObject
{
    public Quality quality;

    public string fishType;

    public GameObject[] patternArray;

    public int reward;
}
