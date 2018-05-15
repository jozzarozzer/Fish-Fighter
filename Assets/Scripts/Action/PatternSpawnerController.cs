using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawnerController : MonoBehaviour
{
    public IntVariable enemyCount;
    GameObject patternInstance;


    public void SpawnPattern(GameObject chosenPattern)
    {
        SpawnPatternIE(chosenPattern);
    }
    public void SpawnPatternIE(GameObject pattern)
    {
        patternInstance = Instantiate(pattern, transform);
    }

    public void DestroyPattern()
    {
        Destroy(patternInstance);
    }
}
