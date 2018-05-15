using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreOpenButton : MonoBehaviour {

    public GameObject storeUI;
    public BoolVariable UIopen;

	void Start ()
    {
        UIopen.value = false;
        storeUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.P))
        {
            UIopen.value = !UIopen.value;
            storeUI.SetActive(!storeUI.activeInHierarchy);
        }
	}
}
