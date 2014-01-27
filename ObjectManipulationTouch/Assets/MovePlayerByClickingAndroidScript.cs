using UnityEngine;
using System.Collections;

public class MovePlayerByClickingAndroidScript : MonoBehaviour {

	public float cameraSensitivity = 90;
	public float moveSpeed = 5;
	
	public float jumpForce = 10;
	
	private Vector3 targetPosition;
	
	private bool isGrounded = true;
	
	ClimbScript climbScript;
	
	// Use this for initialization
	void Start () {
		targetPosition = transform.position;
		climbScript = transform.Find("ClimbColliderObject").GetComponent<ClimbScript>();
	}
	
	// Update is called once per frame
	void Update () {

		//walk with clicking
		targetPosition.y = transform.position.y;
		
		float dist = Vector3.Distance(transform.position, targetPosition);
		if(dist < 0.5f) {
			targetPosition = transform.position;
			//Vector3 temp = rigidbody.velocity * 0.1f;
			//rigidbody.velocity = temp;
		}
		else {
			Vector3 direction = targetPosition - transform.position;
			
			direction = Vector3.Normalize(direction);
			rigidbody.AddForce(direction * moveSpeed * 10);
		}

		//Climb TODO stop other movements while climbing??
		if(climbScript.isClimbing() == true) {
			Vector3 temp = rigidbody.velocity;
			temp.x=0;
			temp.z=0;
			rigidbody.velocity = temp;
			transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
		}
		if(climbScript.isEndClimb() == true) {
			transform.Translate(Vector3.forward * jumpForce * Time.deltaTime);
			climbScript.changeForward(1);
		}

		/*
		//Climb TODO stop other movements while climbing??
		if(climbScript.isClimbing() == true) {
			Vector3 temp = rigidbody.velocity;
			temp.x=0;
			temp.z=0;
			rigidbody.velocity = temp;
			rigidbody.AddForce(Vector3.up * jumpForce *80* Time.deltaTime);
			targetPosition=transform.position;
		}
		else if(climbScript.isEndClimb() == true) {
			rigidbody.AddRelativeForce(Vector3.forward * jumpForce *50* Time.deltaTime);
			climbScript.changeForward(1);
			
			Vector3 temp = rigidbody.velocity;
			temp.y = 0;
			rigidbody.velocity = temp;
			rigidbody.AddForce(Vector3.up * jumpForce *80* Time.deltaTime);
			targetPosition=transform.position;
		}
		*/
		
		//TODO check ray downwards for grounding
		RaycastHit hitFloor;
		Vector3 rayDirection = new Vector3(0,-1,0);
		float distance = 0.8f;
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
	
	public void movePlayer(Vector3 destination) {
		Ray ray = Camera.main.ScreenPointToRay(destination);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if(hit.collider != null) {
				targetPosition = hit.point;
			}
		}
	}

	public void jump() {
		if(isGrounded) {
			rigidbody.AddForce(Vector3.up * jumpForce *800* Time.deltaTime);
			targetPosition=transform.position;
		}
	}
}
