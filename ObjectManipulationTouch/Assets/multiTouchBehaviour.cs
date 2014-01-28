using UnityEngine;
using System.Collections;

public class multiTouchBehaviour : MonoBehaviour {

	private MoveCameraAndroidScript moveCameraScript;
	private RotatePlayerAndroidScript rotatePlayerScript;
	private MovePlayerForwardBackwardAndroidScript movePlayerScript;
	private MovePlayerByClickingAndroidScript moveByClickingScript;

	private static GameObject selectedObj = null;
	private Vector3 currentTouchPoint;
	private Vector3 previousTouchPoint;
	private Quaternion originalRotation;
	private Quaternion offsetRotation;
	private bool setOriginalRotation;
	private float singleTapDuration = 0;
	private bool singleTapActive = false;
	private Touch singleTapTouch;

	private bool touchMoved = false;

	private bool pinch = false;
	private bool pinchApart = false;
	private bool pinchContract = false;

	// Use this for initialization
	void Start () {
		//get all scripts needed
		moveCameraScript = FindObjectOfType<MoveCameraAndroidScript>();
		rotatePlayerScript = FindObjectOfType<RotatePlayerAndroidScript>();
		movePlayerScript = FindObjectOfType<MovePlayerForwardBackwardAndroidScript>();
		moveByClickingScript = FindObjectOfType<MovePlayerByClickingAndroidScript>();

		currentTouchPoint = new Vector3(0,0,0);
		previousTouchPoint = new Vector3(0,0,0);
		setOriginalRotation = true;
	} //end Start()
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 1) {
			performActionDoubleTouch(Input.touches);
			return;
		} else { // touchCount = 1

			if (singleTapActive) {
				singleTapDuration += Time.deltaTime;
				if (singleTapDuration >= 0.2f) {
					singleTapActive = false;
					performActionSingleTouchSingleTap (singleTapTouch);
				}
			}

			foreach (Touch touch in Input.touches) {

				if ((touch.phase == TouchPhase.Ended) && (!touchMoved)) {
					int touchCount = Input.touchCount;
					int tapCount = touch.tapCount;
					// print ("no of fingers: " + touchCount + " / taps: " + tapCount + " / deltaTime: " + touch.deltaTime);

					if (tapCount == 1)  {
						singleTapDuration = 0;
						singleTapActive = true;
						singleTapTouch = touch;
					} else if (tapCount == 2) {
						singleTapActive = false;
						performActionSingleTouchDoubleTap(touch);
					}
				} else if (touch.phase == TouchPhase.Moved) {
					previousTouchPoint = currentTouchPoint;
					currentTouchPoint.x = touch.position.x;
					currentTouchPoint.y = touch.position.y;
					performActionSingleTouchSingleMove(touch);
					touchMoved = true;
				} else if (touch.phase == TouchPhase.Began) {
					setOriginalRotation = true;
					currentTouchPoint.x = touch.position.x;
					currentTouchPoint.y = touch.position.y;
					touchMoved = false;
				}
			}
		}
	}

	private void performActionSingleTouchDoubleTap(Touch touch)
	{
		print("doubleTap");
		//single touch, double click
		Rigidbody rb = sendRay(touch);
		//raycast hit rigidbody
		if(rb != null) {
			//double click on selected object
			if(rb.gameObject == selectedObj) {
				releaseSelection();
			}
			//double click on cube
			else {
				releaseSelection();
				selectedObj = rb.gameObject;
				initSelection();
			}
		} //else: do nothing
	} //end single touch, double click

		
	private void performActionSingleTouchSingleTap(Touch touch)
	{
		print ("singleTap");
		if (selectedObj == null) {
			Vector3 movement = new Vector3(touch.position.x, touch.position.y, 0f);
			moveByClickingScript.movePlayer(movement);
		}
	}


	private void performActionSingleTouchSingleMove(Touch touch) 
	{
		print ("move");

		//float moveDistance = touch.deltaPosition.y * Time.deltaTime;
		//movePlayerScript.movePlayer(moveDistance);
	  
	  	//cube action, singleTouchGesture: moveX, moveY, moveZ, rotate
		if (selectedObj != null) {
			print ("single touch gesture init");
			singleTouchGesture (touch); //= Input.GetTouch(0)
		} //end singleTouch			
		else { //selectedObj == null			
			/*Vector3 movement = new Vector3(touch.position.x, touch.position.y, 0f);
			moveByClickingScript.movePlayer(movement);*/


			float rotationY = touch.deltaPosition.y * Time.deltaTime * 10.0f;
			rotationY = Mathf.Clamp (rotationY, -90, 90);
			float rotationX = touch.deltaPosition.x * Time.deltaTime * 10.0f;
			rotationX = Mathf.Clamp (rotationX, -90, 90);
			rotatePlayerScript.rotatePlayer (rotationX); //left and right
			moveCameraScript.rotateCamera (rotationY); //up and down
		}
	}

	private void performActionDoubleTouch(Touch[] touches) 
	{
		Touch t0 = touches [0];
		Touch t1 = touches [1];

		//cube action, multiTouchGesture: pinch (scale)
		if (selectedObj != null && (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)) {
			twoTouchGesture (t0, t1);
		}

		if (selectedObj != null && (t0.phase == TouchPhase.Ended || t1.phase == TouchPhase.Ended)) {
			pinch = false;
			pinchApart = false;
			pinchContract = false;
		}

		//player jumps or camera move
		if (selectedObj == null && (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)) {
			//2 fingers parallel to make character jump
			if (t0.deltaPosition.y > t0.deltaPosition.x && t1.deltaPosition.y > t1.deltaPosition.x) {
				print ("JUMP");
				moveByClickingScript.jump ();
			}
		}
	}



	//(0, 0) is lower left corner
	//print ("pixel coord : x" + touch.position.x + " " + touch.position.y);
	public Rigidbody sendRay(Touch touch) {
		//releaseSelection ();
		Ray ray = Camera.main.ScreenPointToRay (touch.position);
		RaycastHit hit;
		//ray intersects any collider
		if (Physics.Raycast (ray, out hit)) {
			//ray hits rigidbody
			//rigidbody should be used when obj is moving
			return hit.rigidbody;
		}
		return null;
	}

	public void initSelection() {
		print ("selection init");
		selectedObj.renderer.material.color = Color.green;
		//print ("<init> selected obj tag: " + selectedObj.tag);
	}

	public void releaseSelection() {
		print ("selection release");
		if (selectedObj != null) {
			selectedObj.renderer.material.color = Color.red;
			selectedObj = null;
		}
	}

	public void twoTouchGesture(Touch t0, Touch t1) {
		//TODO check if fingers are above obj; OTHERWISE move camera
		string tag = selectedObj.tag;
		checkPinch (t0, t1);

		if (tag == "scale" && pinch == true) {
			print("PINCH SCALE");
			if(pinchApart) {
				//scale up
				selectedObj.transform.localScale += new Vector3 (0.1F, 0, 0);
				
			} else if(pinchContract) {
				//scale down
				selectedObj.transform.localScale += new Vector3 (-0.1F, 0, 0);
				
			} else { 
				print ("unknown pinch");
			}
			
		} else {
			print ("ERROR: obj does not react on multitouch");
		}
	} //end twoTouchGesture()

	public void checkPinch(Touch t0, Touch t1) {
		float prevDistance = Vector2.Distance (t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);
		float currDistance = Vector2.Distance (t0.position, t1.position);

		if (prevDistance < currDistance) {
			pinch = true;
			pinchApart = true;
			pinchContract = false;
			print ("fingers moving apart");
		} else if (currDistance < prevDistance) {
			pinch = true;
			pinchContract = true;
			pinchApart = false;
			print ("fingers moving together");
		} else {
			print ("no pinch");
			pinch = false;
		}
	} //end checkPinch()
	
	public void singleTouchGesture(Touch touch) {
		string tag = selectedObj.tag;
		float deltaPos;
		if (selectedObj != null) {

			Vector3 FirstPos = Camera.main.WorldToScreenPoint(selectedObj.transform.position);
			currentTouchPoint.z = FirstPos.z;
			previousTouchPoint.z = FirstPos.z;
			Vector3 currentPos = Camera.main.ScreenToWorldPoint(currentTouchPoint);
			Vector3 prevPos = Camera.main.ScreenToWorldPoint(previousTouchPoint);

			if (tag == "moveX") {
				float difference = currentPos.x - prevPos.x;
				selectedObj.transform.rigidbody.AddRelativeForce(new Vector3(difference * 500, 0, 0));

				//deltaPos = touch.deltaPosition.x / 80;
				//selectedObj.transform.Translate (Vector3.right * deltaPos, Space.Self);
				
			} else if (tag == "moveY") {
				float difference = currentPos.y - prevPos.y;
				selectedObj.transform.rigidbody.AddRelativeForce(new Vector3(0, difference * 500, 0));

				//deltaPos = touch.deltaPosition.y / 80;
				//selectedObj.transform.Translate (Vector3.up * deltaPos, Space.Self);
				
			} else if (tag == "moveZ") {
				float difference = currentPos.z - prevPos.z;
				selectedObj.transform.rigidbody.AddRelativeForce(new Vector3(0, 0, difference * 500));

				//deltaPos = touch.deltaPosition.y / 80;
				//selectedObj.transform.Translate (Vector3.forward * deltaPos, Space.Self);
			
			} else if (tag == "rotate") {
				/*if(setOriginalRotation) {
					originalRotation = selectedObj.transform.rotation;
					Vector3 firstPosition = selectedObj.transform.position;
					Vector3 currentDir = currentPos - firstPosition;
					offsetRotation = Quaternion.Inverse (Quaternion.LookRotation (currentDir));
					setOriginalRotation = false;
				}
				Vector3 direction = currentPos - prevPos;
				Quaternion newRotation = Quaternion.LookRotation (direction) * originalRotation * offsetRotation;
				newRotation.y = originalRotation.y;
				newRotation.z = originalRotation.z;
				selectedObj.transform.rotation = newRotation;*/

				Vector3 direction = currentPos - prevPos;

				selectedObj.transform.rigidbody.AddRelativeTorque(direction*100);

				//deltaPos = touch.deltaPosition.y;
				//selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
					
			} else {
				print ("ERROR: obj does not react on single touch");
			}
		}
	} //end singleTouchGesture()
	
} //end class
