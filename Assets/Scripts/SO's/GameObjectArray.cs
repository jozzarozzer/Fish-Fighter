using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObjectArrayVariable", menuName = "Variables/Game Object Array Variable")]
public class GameObjectArrayVariable : ScriptableObject
{
    public GameObject[] value;
}
