using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackerController : MonoBehaviour
{
    public int enemyCount;
    public int enemyCountLastFrame;
    public IntVariable enemyCountVariable;
    public bool currentlyCounting;

    void Update()
    {
        enemyCount = enemyCountVariable.value;
        if (enemyCountLastFrame == 0 && enemyCount != 0)
        {
            currentlyCounting = true;
        }
    }

    void LateUpdate()
    {
        enemyCountLastFrame = enemyCount;
    }
}
