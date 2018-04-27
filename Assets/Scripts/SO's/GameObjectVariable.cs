using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObjectVariable", menuName = "Variables/GameObject Variable")]
public class GameObjectVariable : ScriptableObject
{
    public GameObject value;
}
