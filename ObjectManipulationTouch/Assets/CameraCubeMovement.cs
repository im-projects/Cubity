using UnityEngine;
using System.Collections;

public class CameraCubeMovement : MonoBehaviour {

	public float cameraSensitivity = 90;
	public float normalMoveSpeed = 10;

	public float jumpForce = 10;
	public float currentJump = 0;

	private Vector3 targetPosition;

	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	private bool isGrounded = true;

	ClimbScript climbScript;

	// Use this for initialization
	void Start () {
		targetPosition = transform.position;
		climbScript = transform.Find("ClimbColliderObject").GetComponent<ClimbScript>();
	}
	
	// Update is called once per frame
	void Update () {

		//rotate player (camera) ONLY SIDEWAYS - you can use your own code for this probably?
		//change to touch like in MoveCamera Script but only for screen X to world Y axis
		if (Input.GetMouseButton(1)){
			rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
			//rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
			//rotationY = Mathf.Clamp (rotationY, -90, 90);
			transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
			//targetRotation = transform.localRotation;
		}

		//walk with wasd
		//use this for forward/backward -> make +1 or -1 for Vertical Axis Value if it doesn't exist in touch
		transform.Translate (Vector3.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
		//leave out this part (for side walking)
		//transform.Translate (Vector3.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);


		//walk with clicking
		targetPosition = transform.position; //for only walking when clicking - stop on end of click
		//TODO only walk when nothing selected!!
		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); //touch.position istead of mouse position
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if(hit.collider != null) {
					targetPosition = hit.point;
				}
			}
		}
		float dist = Vector3.Distance(transform.position, targetPosition);
		float currMoveSpeed = (normalMoveSpeed * Time.deltaTime) / dist;

		float fixedY = transform.position.y;
		transform.position = Vector3.Lerp (transform.position, targetPosition, currMoveSpeed);
		Vector3 yFix = transform.position;
		yFix.y = fixedY;
		transform.position = yFix;

		//Jump On Space
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) { //just use on other touch move
			print ("Jump");
			//isGrounded = false;
			currentJump = jumpForce;
			//transform.Translate (Vector3.up * normalMoveSpeed * Time.deltaTime);
		}

		//Jump
		if(currentJump > 6.5f) {
			transform.Translate(Vector3.up * currentJump * Time.deltaTime);
			currentJump -= 0.1f;
			if(currentJump <= 6.5f) {
				print ("Stop Jumping");
			}
		}

		//Climb TODO stop other movements while climbing??
		if(climbScript.isClimbing() == true) {
			Vector3 temp = rigidbody.velocity;
			temp.x=0;
			temp.z=0;
			rigidbody.velocity = temp;
			transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
			//climbScript.changeHeight(1);
		}
		if(climbScript.isEndClimb() == true) {
			transform.Translate(Vector3.forward * jumpForce * Time.deltaTime);
			climbScript.changeForward(1);
		}

		//TODO check ray downwards for grounding
		RaycastHit hitFloor;
		Vector3 rayDirection = new Vector3(0,-1,0);
		float distance = 2.0f;
		if(Physics.Raycast(transform.position,rayDirection,out hitFloor,distance)){
			//the ray collided with something, you can interact
			// with the hit object now by using hit.collider.gameObject
			if(!isGrounded) {
				isGrounded = true;
				print ("Grounded");
			}
		}
		else{
			//nothing was below your gameObject within 10m.
			if(isGrounded) {
				isGrounded = false;
				print ("not Grounded");
			}
		}

	}


	//grounding and other collsion stuff - can probably be left out for now
	/*void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag != "wall") //if grounding works with ray - delete this
		{
			print ("Grounded");
			isGrounded = true;
			targetPosition = transform.position;
		}
	}*/
}
