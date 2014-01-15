using UnityEngine;
using System.Collections;

public class multiTouchBehaviour : MonoBehaviour {

	private GameObject selectedObj = null;
	//private Touch t0 = null;
	//private Touch t1 = null;
	private bool pinch = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
					twoTouchGesture(t0, t1);
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
				selectedObj = hit.rigidbody.gameObject;
				initSelection();
			}
			else {
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
		selectedObj.renderer.material.color = Color.green;
		print ("select color chosen");
	}

	public void releaseSelection() {
		print ("release selection");
		if (selectedObj != null) {
			selectedObj.renderer.material.color = Color.red;
			selectedObj = null;
		} else {
			selectedObj = null;
		}
	}

	public void twoTouchGesture(Touch t0, Touch t1) {
		//TODO check if fingers are above obj; OTHERWISE move camera
		string tag = selectedObj.tag;

		if (tag == "scale" && pinch == true) {
			print("PINCH SCALE");
			if (t0.deltaPosition.y > t0.deltaPosition.x) {
				selectedObj.transform.localScale += new Vector3 (0, 0.1F, 0);
			} else {
				selectedObj.transform.localScale += new Vector3 (0.1F, 0, 0);
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
			print ("fingers moving apart");
		}
		if (currDistance < prevDistance) {
			print ("fingers moving together");
		}
		//else: distance bleibt gleich: parallele bewegung
	}
	
	public void singleTouchGesture(Touch touch) {
		string tag = selectedObj.tag;
		float deltaPos;
		if (selectedObj != null) {
			if (tag == "moveX") {
				deltaPos = touch.deltaPosition.x / 80;
				selectedObj.transform.Translate (Vector3.right * deltaPos, Space.Self);
				
			} else if (tag == "moveY") {
				deltaPos = touch.deltaPosition.y / 80;
				selectedObj.transform.Translate (Vector3.up * deltaPos, Space.Self);
				
			} else if (tag == "moveZ") {
				deltaPos = touch.deltaPosition.y / 80;
				selectedObj.transform.Translate (Vector3.forward * deltaPos, Space.Self);
			
			} else if (tag == "rotate1Touch") {
				deltaPos = touch.deltaPosition.y;
				selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
					
			} else {
				print ("ERROR: obj does not react on single touch");
			}
		}
	}
}
