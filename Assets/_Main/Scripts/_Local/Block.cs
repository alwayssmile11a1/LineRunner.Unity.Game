using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour {

    public float moveSpeed = -2f;
    public float fadeSpeed = 2f;
    public float xOffset = -1f;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update () {
		
        //Disappear when out of view
        if(mainCam.transform.position.x - mainCam.orthographicSize * mainCam.aspect - transform.position.x > xOffset)
        {
            Disappear();
        }


	}

    private void Disappear()
    {
        transform.position -= Vector3.down * moveSpeed * Time.deltaTime;

    }


}
