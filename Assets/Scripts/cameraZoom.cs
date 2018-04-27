using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoom : MonoBehaviour {

    float minSize = 10.68f;
    float maxSize = 35f;

    Camera cam;

    public GameObject player;
    FishingRodSO rod;

	void Start () {
        cam = GetComponent<Camera>();
	}
	
	void Update () {
        rod = player.GetComponent<playerController>().fishingRod;

        if (rod != null)
        {
            float rodLevel = rod.level;
            float sizeDifference = maxSize - minSize;
            float levelNormalized = (rodLevel - 1) / 3;

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, minSize + sizeDifference * levelNormalized, 0.1f);
        }
	}
}
