﻿using UnityEngine;
using System.Collections;

public class MoveCameraAndroidScript : MonoBehaviour {

	public bool locked = false;
	public float cameraSensitivity = 0.01f;

	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	// Use this for initialization
	void Start () {
 
	}
 
	// Update is called once per frame
	void Update () {
		/*if(locked == false) {
        	if (Input.touches.Length > 0)
        	{
				if (Input.touches[0].phase == TouchPhase.Moved)
            	{
					//rotate player (camera) ONLY UP/DOWNWAYS - you can use your own code for this probably?
					
					//calculate rotation of the camera only up and down (screen Y axe) and use for rotating camera along X axe
					Vector2 delta = Input.touches[0].deltaPosition;
					rotationY = delta.y * Time.deltaTime;
					rotationY = Mathf.Clamp (rotationY, -90, 90);
					
					//rotate it
					//transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
					transform.localRotation *= Quaternion.AngleAxis(rotationY * 0.01f, Vector3.left);
            	}
			}
        }
		*/
	}

	public void rotateCamera(float rotationY) {
		transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
	}
}
