using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioClipPrefabController : MonoBehaviour
{

    AudioSource source;
    float timePassed;

	void Start ()
    {
        source = GetComponent<AudioSource>();
        timePassed = 0;
	}
	

	void Update ()
    {
        timePassed += Time.deltaTime;

        if (timePassed > source.clip.length + 1)
        {
            Destroy(gameObject);
        }
    }
}
