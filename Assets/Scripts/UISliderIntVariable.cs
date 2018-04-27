using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderIntVariable : MonoBehaviour
{

    public IntVariable intVariable;
    public IntVariable intVariableMax;
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update ()
    {
        slider.maxValue = intVariableMax.value;
        slider.value = intVariable.value;
	}
}
