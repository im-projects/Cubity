using UnityEngine;
using System.Collections;

public class CameraCubeMovement : MonoBehaviour {

	public float cameraSensitivity = 90;
	public float normalMoveSpeed = 10;
	public float jumpForce = 10;
	public float currentJump = 0;

	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	private bool isGrounded = true;

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

		//Jump On Space
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
			print ("Jump");
			isGrounded = false;
			currentJump = jumpForce;
			//transform.Translate (Vector3.up * normalMoveSpeed * Time.deltaTime);
		}

		transform.Translate (Vector3.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
		transform.Translate (Vector3.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);

		//Jump
		if(currentJump > 6.5f) {
			transform.Translate(Vector3.up * currentJump * Time.deltaTime);
			currentJump -= 0.1f;
			if(currentJump <= 6.5f) {
				print ("Stop Jumping");
			}
		}

		//transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		//transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
	
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name != "wall")
		{
			print ("Grounded");
			isGrounded = true;
		}
	}
}
