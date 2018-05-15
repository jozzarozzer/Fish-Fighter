using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour {

    public GameObjectVariable obj;

	void Start ()
    {
        GameObject spawnedObj = Instantiate(obj.value, transform.position, transform.rotation, transform.parent);
        spawnedObj.transform.localScale = transform.localScale;

        Destroy(gameObject);
	}
	

}
