using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunButton : MonoBehaviour
{
    public IntVariable playerCurrency;
    public GunArrayVariable playerguns;
    public GunSO thisGun;
    public Button button;

    public GameObject nameText;
    public GameObject priceText;

    void Start()
    {
        thisGun = GetComponent<GunHolder>().gunType;
        button = GetComponent<Button>();
    }

    void Update()
    {
        for (int i = 0; i < playerguns.value.Length; i++)
        {
            if (playerguns.value[i] == thisGun)
            {
                button.interactable = false;
            }
        }

        nameText.GetComponent<Text>().text = thisGun.gunType;
        priceText.GetComponent<Text>().text = thisGun.price.ToString();
    }

    public void BuyGun()
    {
        if (playerCurrency.value >= thisGun.price)
        {
            playerCurrency.value -= thisGun.price;

            for (int i = 0; i < playerguns.value.Length; i++)
            {
                if (playerguns.value[i] == null)
                {
                    playerguns.value[i] = thisGun;
                    break;
                }
            }
        }
    }
}
