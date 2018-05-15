using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextVariableSetter : MonoBehaviour {

    public IntVariable inputInt;
    Text UIText;

	void Start () {
        UIText = GetComponent<Text>();
	}
	
	void Update () {
        UIText.text = inputInt.value.ToString();
	}
}
