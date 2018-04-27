using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUIText : MonoBehaviour
{

    public IntVariable playerCurrency;
    int shownAmount;

	void Update ()
    {
        if (playerCurrency.value < shownAmount)
        {
            shownAmount -= 10;
        }
        if (playerCurrency.value > shownAmount)
        {
            shownAmount = playerCurrency.value;
        }
                GetComponent<Text>().text = shownAmount.ToString() + "G";
	}
}
