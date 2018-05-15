using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthButton : MonoBehaviour {

    public int healthPackAmount;
    public int price;

    public IntVariable playerHealthVariable;
    public IntVariable playerCurrency;
    public IntVariable playerMaxHealth;

    public Text healthDisplay;
    public Text priceDisplay;
    public Text healthAmountDisplay;

    private void Update()
    {
        healthDisplay.text = playerHealthVariable.value + " I " + playerMaxHealth.value;
        priceDisplay.text = price.ToString();
        healthAmountDisplay.text = healthPackAmount.ToString();
    }

    public void addHealth()
    {
        if (playerCurrency.value >= price)
        {
            playerHealthVariable.value += healthPackAmount;
            playerCurrency.value -= price;

            if (playerHealthVariable.value > playerMaxHealth.value)
            {
                playerHealthVariable.value = playerMaxHealth.value;
            }
        }
    }
}
