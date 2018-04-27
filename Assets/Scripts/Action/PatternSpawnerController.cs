using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawnerController : MonoBehaviour
{
    public IntVariable enemyCount;


    public void SpawnPattern(GameObject chosenPattern)
    {
        StartCoroutine(SpawnPatternIE(chosenPattern));
    }
    public IEnumerator SpawnPatternIE(GameObject pattern)
    {
        GameObject patternInstance = Instantiate(pattern, transform);

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 50000; i++)
        {
            if (enemyCount.value == 0)
            {
                yield return new WaitForSeconds(1.9f);
                Destroy(patternInstance);
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }

    }
}
