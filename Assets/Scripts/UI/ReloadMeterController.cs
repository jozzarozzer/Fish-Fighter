using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadMeterController : MonoBehaviour {

    public bool follow;

    public GameObject meterObject;
    public Image meterImage;

    public GameObject player;
    PlayerControllerAction playerScript;

    public RectTransform rect;

    public Camera cam;

    Vector3 offset;

	
	void Start ()
    {
        meterImage = meterObject.GetComponent<Image>();
        playerScript = player.GetComponent<PlayerControllerAction>();
        rect = GetComponent<RectTransform>();
        offset = rect.localPosition;
	}
	
	
	void Update ()
    {
        if (playerScript.reloadingProgress == 1)
        {
            meterImage.enabled = false;
        }
        else
        {
            meterImage.enabled = true;
            meterImage.fillAmount = playerScript.reloadingProgress;
        }



        Vector3 playerScreenPos = cam.WorldToViewportPoint(player.transform.position);
        Vector3 playerUIPos = new Vector3(playerScreenPos.x * Screen.width, playerScreenPos.y * Screen.height, playerScreenPos.z);
        if (follow)
        {
            rect.position = playerUIPos + offset;
        }
        
	}
}
