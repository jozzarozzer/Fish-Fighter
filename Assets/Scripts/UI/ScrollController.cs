using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour {

    public Vector3 startPosition;

    public Vector3 endPosition;

    public Vector3 transformPosition;

    public GameObject scrollbarObj;

    public Scrollbar scrollbar;

	void Start () {
        startPosition = transform.position;
        endPosition = transform.position + transformPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 positionDifference = endPosition - startPosition;

        //transform.localPosition = startPosition + positionDifference * scrollbar.value;
        transform.localPosition = Vector3.zero + positionDifference * scrollbar.value;
    }
}
