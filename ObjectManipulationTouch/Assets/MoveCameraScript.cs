using UnityEngine;
using System.Collections;

public class MoveCameraScript : MonoBehaviour {

 
	public float cameraSensitivity = 90;
	public float normalMoveSpeed = 10;
 
	private float rotationX = 0.0f;
	private float rotationY = 0.0f;
 
	void Start ()
	{
		Screen.showCursor = true;
	}
 
	void Update ()
	{
 		if (Input.GetMouseButton(1)){ // change to different touch inputs

			//rotate player (camera) ONLY UP/DOWNWAYS - you can use your own code for this probably?


			//calculate rotation of the camera only up and down (screen Y axe) and use for rotating camera along X axe
			rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
			//get axis, existing in touch? else use delta touch movement on y axe of the screen
			rotationY = Mathf.Clamp (rotationY, -90, 90);

			//rotate it
			transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

		}
 
	}
	
	
}
