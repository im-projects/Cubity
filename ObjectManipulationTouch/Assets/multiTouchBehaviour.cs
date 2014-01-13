using UnityEngine;
using System.Collections;

public class multiTouchBehaviour : MonoBehaviour {

	private GameObject selectedObj = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch touch in Input.touches) {
			//singleTouch
			if(Input.touchCount == 1) {
				/*
				 * doubleclick selects obj
				 */
				if(touch.tapCount == 2) {
					print("taps: " + touch.tapCount);
					sendRay(touch);
				}

				//singleTouchGesture
				else if(selectedObj != null) {
					print ("single touch gesture init");
					singleTouchGesture(touch);
				}
			} //end singleTouch

			//multiTouchGesture
			if(Input.touchCount == 2) {
				print("no of touches: " + Input.touchCount);
				Touch t0 = Input.GetTouch(0);
				Touch t1 = Input.GetTouch(1);
				if(selectedObj != null && (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)) {
					//TODO desired behaviour?
					print ("2 touch gesture init");
					twoTouchGesture(t0, t1);
				}
			}

		} //end foreach touch()
	} //end update()

	public void sendRay(Touch touch) {
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
				movePlayer();
			}
		}
	}

	public void movePlayer() {
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
			
		if (tag == "rotate") {
			float deltaPos = t0.deltaPosition.y;
			selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
			
		} else if (tag == "scale") {
			if(t0.deltaPosition.y > t0.deltaPosition.x) {
				selectedObj.transform.localScale += new Vector3(0, 0.1F, 0);
			} else {
				selectedObj.transform.localScale += new Vector3(0.1F, 0, 0);
			}
			
		} else {
			print ("ERROR: obj does not react on multitouch");
		}
	}
	
	public void singleTouchGesture(Touch touch) {
		string tag = selectedObj.tag;
		if (selectedObj != null) {
			if (tag == "moveX") {
				float deltaPos = touch.deltaPosition.x / 80;
				selectedObj.transform.Translate (Vector3.right * deltaPos, Space.Self);
				
			} else if (tag == "moveY") {
				float deltaPos = touch.deltaPosition.y / 80;
				selectedObj.transform.Translate (Vector3.up * deltaPos, Space.Self);
				
			} else if (tag == "moveZ") {
				float deltaPos = touch.deltaPosition.y / 80;
				selectedObj.transform.Translate (Vector3.forward * deltaPos, Space.Self);
				
			} else {
				print ("ERROR: obj does not react on single touch");
			}
		}
	}
}
