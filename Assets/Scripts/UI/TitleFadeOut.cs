using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleFadeOut : MonoBehaviour {

    bool fade;

	// Use this for initialization
	void Start () {
        fade = false;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.P))
        {
            fade = true;
        }

        if (fade)
        {
            //GetComponent<TextMesh>().color = Vector4.Lerp(GetComponent<TextMesh>().color, Color.clear, 0.1f);
            gameObject.transform.position += Vector3.up * 30;
        }
	}
}
