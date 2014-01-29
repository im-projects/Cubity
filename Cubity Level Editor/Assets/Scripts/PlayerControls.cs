using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	
	public EControlsMode m_controlsMode = EControlsMode.SWIPE;
	public ControlsSettings m_controlsModeSettingsSwipe;
	public ControlsSettings m_controlsModeSettingsPNC;
	public ClimbScript m_climbScript;
	public Camera m_camera;
	public float m_cameraRotationFactor = 2;
	
	private bool isGrounded = true;
	private Vector3 targetPosition;

	private const int raycastLength = 200;
	private int layerMask;

	[System.Serializable]
	public class ControlsSettings
	{
//		public float cameraSensitivity = 90;
		public float moveSpeed = 7f;
		public float jumpForce = 10f;
	}

	public enum EControlsMode
	{
		SWIPE,
		POINT_AND_CLICK
	}

	private void Start()
	{
		if(m_camera == null) m_camera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
		if(m_climbScript == null) m_climbScript = GameObject.FindObjectOfType<ClimbScript>();

		// use layermask to ignore player layer 
		// more info here: http://answers.unity3d.com/questions/8715/how-do-i-use-layermasks.html
		layerMask = ~(1 << LayerMask.NameToLayer("Player"));
	}

	private void Update()
	{
		if(m_controlsMode.Equals(EControlsMode.SWIPE))
		{
			//Climb TODO stop other movements while climbing??
			if(m_climbScript.isClimbing() == true) {
				Vector3 temp = rigidbody.velocity;
				temp.x=0;
				temp.z=0;
				rigidbody.velocity = temp;
				rigidbody.AddForce(Vector3.up * m_controlsModeSettingsSwipe.jumpForce *80* Time.deltaTime);
				//transform.Translate(Vector3.up * m_controlMode1.jumpForce * Time.deltaTime);
			}
			if(m_climbScript.isEndClimb() == true) {
				rigidbody.AddRelativeForce(Vector3.forward * m_controlsModeSettingsSwipe.jumpForce *50* Time.deltaTime);
				//transform.Translate(Vector3.forward * m_controlMode1.jumpForce * Time.deltaTime);
				m_climbScript.changeForward(1);
				
				Vector3 temp = rigidbody.velocity;
				temp.y = 0;
				rigidbody.velocity = temp;
				rigidbody.AddForce(Vector3.up * m_controlsModeSettingsSwipe.jumpForce *80* Time.deltaTime);
			}
			
			//TODO check ray downwards for grounding
			RaycastHit hitFloor;
			Vector3 rayDirection = new Vector3(0,-1,0);
			float distance = 0.8f;
			if(Physics.Raycast(transform.position,rayDirection,out hitFloor,distance)){
				//the ray collided with something, you can interact
				// with the hit object now by using hit.collider.gameObject
				if(!isGrounded) {
					isGrounded = true;
//					print ("Grounded");
				}
			}
			else{
				//nothing was below your gameObject within 10m.
				if(isGrounded) {
					isGrounded = false;
//					print ("not Grounded");
				}
			}
		}
		else if(m_controlsMode.Equals(EControlsMode.POINT_AND_CLICK))
		{
			//walk with clicking
			if(targetPosition != Vector3.zero)
			{
				targetPosition.y = transform.position.y;

				float dist = Vector2.Distance(new Vector2(transform.position.x,transform.position.z),new Vector2(targetPosition.x,targetPosition.z));

				if(dist < 1.0f) {
//					Debug.Log ("close: " + dist + " : " + new Vector2(transform.position.x,transform.position.z) + " : " + new Vector2(targetPosition.x,targetPosition.z));
					Vector3 temp = rigidbody.velocity * dist;
					if(dist < 0.5f) {
//						Debug.Log ("there!");
						temp = Vector3.zero;
						targetPosition = Vector3.zero;
					}
					rigidbody.velocity = temp;
				}
				else {
					Vector3 direction = targetPosition - transform.position;
					
					direction = Vector3.Normalize(direction);
					rigidbody.AddForce(direction * m_controlsModeSettingsPNC.moveSpeed *3);
				}
			}
			
			//Climb TODO stop other movements while climbing??
			if(m_climbScript.isClimbing() == true) {
				Vector3 temp = rigidbody.velocity;
				temp.x=0;
				temp.z=0;
				rigidbody.velocity = temp;
				transform.Translate(Vector3.up * m_controlsModeSettingsPNC.jumpForce * Time.deltaTime);
			}
			if(m_climbScript.isEndClimb() == true) {
				transform.Translate(Vector3.forward * m_controlsModeSettingsPNC.jumpForce * Time.deltaTime);
				m_climbScript.changeForward(1);
			}
			
			/*
			//Climb TODO stop other movements while climbing??
			if(climbScript.isClimbing() == true) {
				Vector3 temp = rigidbody.velocity;
				temp.x=0;
				temp.z=0;
				rigidbody.velocity = temp;
				rigidbody.AddForce(Vector3.up * m_controlMode2Settings.jumpForce *80* Time.deltaTime);
				targetPosition=transform.position;
			}
			else if(climbScript.isEndClimb() == true) {
				rigidbody.AddRelativeForce(Vector3.forward * m_controlMode2Settings.jumpForce *50* Time.deltaTime);
				climbScript.changeForward(1);
				
				Vector3 temp = rigidbody.velocity;
				temp.y = 0;
				rigidbody.velocity = temp;
				rigidbody.AddForce(Vector3.up * m_controlMode2Settings.jumpForce *80* Time.deltaTime);
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
//					print ("Grounded");
				}
			}
			else{
				//nothing was below your gameObject within 10m.
				if(isGrounded) {
					isGrounded = false;
//					print ("not Grounded");
				}
			}
		}
	}

	public void rotateCamera(float rotationY) {
		m_camera.transform.localRotation *= Quaternion.AngleAxis(rotationY * m_cameraRotationFactor, Vector3.left);
	}

	public void rotatePlayer(float rotationX) {
		this.transform.localRotation *= Quaternion.AngleAxis(rotationX, Vector3.up);
		//rigidbody.AddRelativeTorque(Vector3.up *50* rotationX);
	}
	
	public void movePlayerByDestination(Vector3 screenCoordinate) {
		if(m_controlsMode.Equals(EControlsMode.SWIPE))
		{
			Debug.LogWarning("Swipe Controls Mode can't handle screenCoordinate vector");
		}
		else if(m_controlsMode.Equals(EControlsMode.POINT_AND_CLICK))
		{
			Ray ray = Camera.main.ScreenPointToRay(screenCoordinate);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, raycastLength, layerMask)) {
				if(hit.collider != null) {
					targetPosition = hit.point;
				}
			}
		}
	}

	public void movePlayerByDistance(float moveDistance)
	{
		if(m_controlsMode.Equals(EControlsMode.SWIPE))
		{
			//transform.Translate (Vector3.forward * m_controlMode1.moveSpeed * moveDistance);
			this.rigidbody.AddRelativeForce(Vector3.forward * m_controlsModeSettingsSwipe.moveSpeed * 600 * moveDistance); //800 too bouncy >.<
		}
		else if(m_controlsMode.Equals(EControlsMode.POINT_AND_CLICK))
		{
			Debug.LogWarning("Point and Click Controls Mode can't handle distance float");
		}
	}

	public void jump() {
		if(m_controlsMode.Equals(EControlsMode.SWIPE))
		{
			if(isGrounded) {
				this.rigidbody.AddForce(Vector3.up * m_controlsModeSettingsSwipe.jumpForce *800* Time.deltaTime);
			}
		}
		else if(m_controlsMode.Equals(EControlsMode.POINT_AND_CLICK))
		{
			if(isGrounded) {
				this.rigidbody.AddForce(Vector3.up * m_controlsModeSettingsPNC.jumpForce *800* Time.deltaTime);
				targetPosition = transform.position;
			}
		}
	}

	public void StopPlayerMovement()
	{
		targetPosition = Vector3.zero;
		ZeroizeRigidbodyForces();
	}

	private void ZeroizeRigidbodyForces()
	{
		this.rigidbody.velocity = Vector3.zero;
		this.rigidbody.angularVelocity = Vector3.zero;
	}
}
