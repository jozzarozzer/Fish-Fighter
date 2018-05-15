using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNumberScript : MonoBehaviour
{

    public bool buttonPressed;
    Button button;
    public Vector3 downPos;
    public Vector3 upPos;

    [Header("upColors")]
    public ColorBlock upColors;

    [Header("downColors")]
    public ColorBlock downColors;

    void Start ()
    {
        button = GetComponent<Button>();
        //downPos = transform.position;
        //upPos = transform.position + new Vector3(0,6,0);
	}
	

	void Update ()
    {
		if (buttonPressed)
        {
            transform.localPosition = upPos;
            button.colors =  upColors;
            button.interactable = false;
        }
        else
        {
            transform.localPosition = downPos;
            button.colors = downColors;
            button.interactable = true;
        }
	}

    public void ButtonSwitch()
    {
        buttonPressed = !buttonPressed;
    }
}
