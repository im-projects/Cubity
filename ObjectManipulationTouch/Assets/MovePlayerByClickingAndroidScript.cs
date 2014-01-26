using UnityEngine;
using System.Collections;

public class MovePlayerByClickingAndroidScript : MonoBehaviour {

	public float moveSpeed = 0.1f;
	public float jumpForce = 1;
	private bool isGrounded = true;
	private Vector3 targetPosition;
	ClimbScript climbScript;
	
	
	// Use this for initialization
	void Start () {
		targetPosition = transform.position;
		climbScript = transform.Find("ClimbColliderObject").GetComponent<ClimbScript>();
	}
	
	// Update is called once per frame
	void Update () {

		//walk
		float dist = Vector3.Distance(transform.position, targetPosition);
		float currMoveSpeed = (moveSpeed * Time.deltaTime) / dist;
		
		float fixedY = transform.position.y;
		transform.position = Vector3.Lerp (transform.position, targetPosition, currMoveSpeed);
		Vector3 yFix = transform.position;
		yFix.y = fixedY;
		transform.position = yFix;


		//Climb TODO stop other movements while climbing??
		if(climbScript.isClimbing() == true) {
			targetPosition = transform.position;
			Vector3 temp = rigidbody.velocity;
			temp.x=0;
			temp.z=0;
			rigidbody.velocity = temp;
			rigidbody.AddForce(Vector3.up * jumpForce *80* Time.deltaTime);
			//transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
		}
		if(climbScript.isEndClimb() == true) {
			targetPosition = transform.position;
			rigidbody.AddRelativeForce(Vector3.forward * jumpForce *50* Time.deltaTime);
			//transform.Translate(Vector3.forward * jumpForce * Time.deltaTime);
			climbScript.changeForward(1);
			
			Vector3 temp = rigidbody.velocity;
			temp.y = 0;
			rigidbody.velocity = temp;
			rigidbody.AddForce(Vector3.up * jumpForce *80* Time.deltaTime);
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
	
	public void movePlayer(Vector3 destination) {
		targetPosition = destination;
	}
}
