using UnityEngine;
using System.Collections;

public class CameraCubeMovement : MonoBehaviour {

	public float cameraSensitivity = 90;
	public float normalMoveSpeed = 10;

	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton(1)){
			rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
			//rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
			//rotationY = Mathf.Clamp (rotationY, -90, 90);
			transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
		}

		transform.Translate (Vector3.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
		transform.Translate (Vector3.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
		//transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		//transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
	
	}
}
