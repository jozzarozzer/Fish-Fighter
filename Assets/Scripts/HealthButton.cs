using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthButton : MonoBehaviour {

    public int healthPackAmount;
    public int price;
    int playerHealth;

    public IntVariable playerHealthVariable;
    public IntVariable playerCurrency;
    public IntVariable playerMaxHealth;

    public Text healthDisplay;

    void Start () {
        playerHealth = GetComponent<IntVariableHolder>().fishingRodType.value; //omg johnny, you're stupid for setting up the intvariableholder this way, leaving this here for reference
	}

    private void Update()
    {
        healthDisplay.text = playerHealthVariable.value + " I " + playerMaxHealth.value;
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
