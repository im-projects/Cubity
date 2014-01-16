using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private GameObject selectedObj = null;
	//private Touch t0 = null;
	//private Touch t1 = null;
	private bool pinch = false;
	private bool pinchUp = false;
	private bool pinchDown = false;
	
	private GameManager gameManager;
	
	void Start()
	{
		gameManager = this.GetComponent<GameManager>();
		if(gameManager == null) Debug.LogWarning("GameManager not found!");
	}
	
	void Update () {
		if(gameManager.m_gameIsPaused) return;
		
		foreach (Touch touch in Input.touches) {
			//singleTouch
			if(Input.touchCount == 1) {
				print("taps: " + touch.tapCount);
				sendRay(touch, touch.tapCount);
				
				//singleTouchGesture: moveX, moveY, moveZ, rotate
				if(selectedObj != null) {
					print ("single touch gesture init");
					singleTouchGesture(touch);
				}
			} //end singleTouch
			
			//multiTouchGesture: scale, jump
			if(Input.touchCount == 2) {
				print("no of touches: " + Input.touchCount);
				Touch t0 = Input.GetTouch(0);
				Touch t1 = Input.GetTouch(1);
				
				//no obj selected: character/camera action
				if(selectedObj == null) {
					//TODO implement
					//2 fingers parallel to make character jump
					if(t0.deltaPosition.y > t0.deltaPosition.x && t1.deltaPosition.y > t1.deltaPosition.x) {
						print("JUMP");
					}
					
					checkPinch(t0, t1);
					//pinch for camera zoom
					if(pinch) {
						print("CAMERA ZOOM");
					}
				}
				
				//obj action
				if(selectedObj != null && (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)) {
					//TODO desired behaviour?
					print ("2 touch gesture init");
					print ("pinch = " + pinch);
					twoTouchGesture(t0, t1);
				}
				
				if (t0.phase == TouchPhase.Ended || t1.phase == TouchPhase.Ended) {
					pinch = false;
					pinchUp = false;
					pinchDown = false;
				}
			}
			
		} //end foreach touch()
	} //end update()
	
	public void sendRay(Touch touch, int tapCount) {
		releaseSelection ();
		Ray ray = Camera.main.ScreenPointToRay (touch.position);
		RaycastHit hit;
		//ray intersects any collider
		if (Physics.Raycast (ray, out hit)) {
			//ray hits rigidbody
			//rigidbody should be used when obj is moving
			if (hit.rigidbody != null) {
				print ("box selected");
				if(tapCount == 2) {
					selectedObj = hit.rigidbody.gameObject;
					initSelection();
				}
			}
			else {
				if(selectedObj != null) {
					selectedObj = null;
				}
				/*
				 * doubleclick selects obj
				 */
				if(tapCount == 1) {
					movePlayer();
				}
			}
		}
	}
	
	public void movePlayer() {
		//TODO implement
	}
	
	public void jump() {
		//TODO implement
	}
	
	public void initSelection() {
		print ("selection init");
		selectedObj.GetComponent<CubeManager>().SetSelected(true);
		print ("select color chosen");
	}
	
	public void releaseSelection() {
		print ("release selection");
		if (selectedObj != null) {
			selectedObj.GetComponent<CubeManager>().SetSelected(false);
			selectedObj = null;
		} else {
			selectedObj = null;
		}
	}
	
	public void twoTouchGesture(Touch t0, Touch t1) {
		//TODO check if fingers are above obj; OTHERWISE move camera
		string tag = selectedObj.tag;
		checkPinch (t0, t1);
		
		if (tag == "scale" && pinch == true) {
			print("PINCH SCALE");
			if(pinchUp) {
				//scale up
				selectedObj.transform.localScale += new Vector3 (0.1F, 0, 0);
				/*
				if (t0.deltaPosition.y > t0.deltaPosition.x) {
					selectedObj.transform.localScale += new Vector3 (0, 0.1F, 0);
				} else {
					selectedObj.transform.localScale += new Vector3 (0.1F, 0, 0);
				}
				*/
			} else if(pinchDown) {
				//scale down
				selectedObj.transform.localScale += new Vector3 (-0.1F, 0, 0);
				/*
				if (t0.deltaPosition.y > t0.deltaPosition.x) {
					selectedObj.transform.localScale += new Vector3 (0, -0.1F, 0);
				} else {
					selectedObj.transform.localScale += new Vector3 (-0.1F, 0, 0);
				}
				*/
			} else { 
				print ("unknown pinch");
			}
			
		} else if (tag == "rotate2Touch") {
			float deltaPos = t0.deltaPosition.y;
			selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
			
			
		} else {
			print ("ERROR: obj does not react on multitouch");
		}
	}
	
	public void checkPinch(Touch t0, Touch t1) {
		//TODO differentiate pinch together and pinch farthener
		//TODO on touchPhase End -> release; pinch = false;
		float prevDistance = Vector2.Distance (t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);
		float currDistance = Vector2.Distance (t0.position, t1.position);
		
		if (prevDistance < currDistance) {
			pinch = true;
			pinchUp = true;
			print ("fingers moving apart");
		} else if (currDistance < prevDistance) {
			pinch = true;
			pinchDown = true;
			print ("fingers moving together");
		} else {
			print ("no pinch");
		}
	}
	
	public void singleTouchGesture(Touch touch) {
		string tag = selectedObj.tag;
		float deltaPos;
		if (selectedObj != null) {
			if (tag == "moveX") {
				deltaPos = touch.deltaPosition.x / 20;
				selectedObj.transform.Translate (Vector3.right * deltaPos, Space.Self);
				
			} else if (tag == "moveY") {
				deltaPos = touch.deltaPosition.y / 20;
				selectedObj.transform.Translate (Vector3.up * deltaPos, Space.Self);
				
			} else if (tag == "moveZ") {
				deltaPos = touch.deltaPosition.y / 20;
				selectedObj.transform.Translate (Vector3.forward * deltaPos, Space.Self);
				
			} else if (tag == "rotate1Touch") {
				deltaPos = touch.deltaPosition.y;
				selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
				
			} else {
				print ("ERROR: obj does not react on single touch");
			}
		}
	}

	private void ZeroizeRigidbodyForce ()
	{
		// sets rigidbody velocity to zero, this avoid the cube to move after a collision
		selectedObj.rigidbody.velocity = Vector3.zero;
		selectedObj.rigidbody.angularVelocity = Vector3.zero;
	}
}