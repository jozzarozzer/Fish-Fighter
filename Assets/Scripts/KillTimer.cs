using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTimer : MonoBehaviour
{

    public float killTime;

    private void Start()
    {
        StartCoroutine(KillTime());
    }

    IEnumerator KillTime()
    {
        yield return new WaitForSeconds(killTime);
        Destroy(gameObject);
    }


}
