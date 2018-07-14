using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour {

    public float moveSpeed = -2f;
    public float fadeSpeed = 2f;
    public float xOffset = -1f;

	private void Update () {
		
        //Disappear when out of view
        if(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect - transform.position.x > xOffset)
        {
            Disappear();
        }


	}

    private void Disappear()
    {
        transform.position -= Vector3.down * moveSpeed * Time.deltaTime;

    }


}
