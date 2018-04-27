using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Events;

public class FishingRodButton : MonoBehaviour
{
    public IntVariable playerCurrency;
    public RodArrayVariable playerRods;
    public FishingRodSO thisRod;
    public Button button;

    public GameObject nameText;
    public GameObject priceText;

    void Start()
    {
        thisRod = GetComponent<FishingRodHolder>().fishingRodType;
        button = GetComponent<Button>();
    }

    void Update()
    {
        for (int i = 0; i < playerRods.value.Length; i++)
        {
            if (playerRods.value[i] == thisRod)
            {
                button.interactable = false;
            }
        }

        nameText.GetComponent<Text>().text = thisRod.rodType;
        priceText.GetComponent<Text>().text = thisRod.price.ToString();
    }

    public void BuyRod()
    {
        if (playerCurrency.value >= thisRod.price)
        {
            playerCurrency.value -= thisRod.price;

            for (int i = 0; i < playerRods.value.Length; i++)
            {
                if (playerRods.value[i] == null)
                {
                    playerRods.value[i] = thisRod;
                    break;
                }
            }
        }
    }
}
