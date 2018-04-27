using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRodArrayVariable", menuName = "Variables/Rod Array Variable")]
public class RodArrayVariable : ScriptableObject
{
    public FishingRodSO[] value;
}
