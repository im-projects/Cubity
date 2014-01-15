using UnityEngine;
using System.Collections;

public class CameraCubeMovement : MonoBehaviour {

	public float cameraSensitivity = 90;
	public float normalMoveSpeed = 10;

	public float jumpForce = 10;
	public float currentJump = 0;

	private Vector3 targetPosition;
	//private Quaternion targetRotation;

	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	private bool isGrounded = true;

	// Use this for initialization
	void Start () {
		targetPosition = transform.position;
		//targetRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		//rotate player (camera)
		if (Input.GetMouseButton(1)){
			rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
			//rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
			//rotationY = Mathf.Clamp (rotationY, -90, 90);
			transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
			//targetRotation = transform.localRotation;
		}

		//Jump On Space
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
			print ("Jump");
			isGrounded = false;
			currentJump = jumpForce;
			//transform.Translate (Vector3.up * normalMoveSpeed * Time.deltaTime);
		}

		//walk with wasd
		transform.Translate (Vector3.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
		transform.Translate (Vector3.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);

		//walk with clicking
		targetPosition = transform.position; //for only walking when clicking - stop on end of click
		//TODO only walk when nothing selected!!
		if(Input.GetMouseButton(0))
		{
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			float hitdist = 0.0f;
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if(hit.collider != null) {
					targetPosition = hit.point;
					//if(hit.collider.gameObject.tag == "floor"){ 
						//to use on top of cubes cubes need floor on top!!!
						/*if (playerPlane.Raycast (ray, out hitdist)) {
							//Vector3 targetPoint = ray.GetPoint(hitdist);
							targetPosition = ray.GetPoint(hitdist);
							//targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
							//transform.rotation = targetRotation;
						}*/
					//}
				}
			}
		}
		float dist = Vector3.Distance(transform.position, targetPosition);
		float currMoveSpeed = (normalMoveSpeed * Time.deltaTime) / dist;
		//transform.localRotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * normalMoveSpeed);

		float fixedY = transform.position.y;
		transform.position = Vector3.Lerp (transform.position, targetPosition, currMoveSpeed);
		Vector3 yFix = transform.position;
		yFix.y = fixedY;
		transform.position = yFix;

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
	

		//TODO check ray downwards for grounding
	}


	//grounding and other collsion stuff
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag != "wall") //if grounding works with ray - delete this
		{
			print ("Grounded");
			isGrounded = true;
			targetPosition = transform.position;
		}
	}
}
