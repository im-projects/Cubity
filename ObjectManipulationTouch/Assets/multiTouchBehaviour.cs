using UnityEngine;
using System.Collections;

public class multiTouchBehaviour : MonoBehaviour {

	private MoveCameraAndroidScript moveCamera;
	private RotatePlayerAndroidScript rotatePlayer;
	private MovePlayerForwardBackwardAndroidScript movePlayer;
	private MovePlayerByClickingAndroidScript movePlayerByClicking;

	public static GameObject selectedObj = null;
//	public static int touchCount = 0;
//	public static int tapCount = 0;

	private bool pinch = false;
	private bool pinchApart = false;
	private bool pinchContract = false;

	// Use this for initialization
	void Start () {
		//get all scripts needed
		moveCamera = FindObjectOfType<MoveCameraAndroidScript>();
		rotatePlayer = FindObjectOfType<RotatePlayerAndroidScript>();
		movePlayer = FindObjectOfType<MovePlayerForwardBackwardAndroidScript>();
		movePlayerByClicking = FindObjectOfType<MovePlayerByClickingAndroidScript>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch touch in Input.touches) {
			int touchCount = Input.touchCount;
			print ("no of fingers: " + touchCount);
			int tapCount = touch.tapCount;
			print("taps: " + tapCount);
			

			//single touch, single click
			if(touchCount == 1 && tapCount == 1) {
				//cube action, singleTouchGesture: moveX, moveY, moveZ, rotate
				if(selectedObj != null) {
					print ("single touch gesture init");
					singleTouchGesture(touch); //= Input.GetTouch(0)
				} //end singleTouch

				else {
					//camera move
					if(touch.phase == TouchPhase.Moved) {
						//TODO
						rotatePlayer.rotatePlayer(touch.deltaPosition.x * Time.deltaTime); //left and right
						moveCamera.rotateCamera(touch.deltaPosition.x * Time.deltaTime); //up and down
					} 
					//player move
					else {
						
					}
				}
			} 

			//single touch, double click
			else if (touchCount == 1 && tapCount == 2) {
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
			}

			//multi touch
			else if (touchCount == 2) {
				Touch t0 = Input.GetTouch(0);
				Touch t1 = Input.GetTouch(1);

				//cube action, multiTouchGesture: pinch (scale)
				if(selectedObj != null && (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)) {
					twoTouchGesture(t0, t1);
				}

				if(selectedObj != null && (t0.phase == TouchPhase.Ended || t1.phase == TouchPhase.Ended)) {
					pinch = false;
					pinchApart = false;
					pinchContract = false;
				}
				/*
				//player jumps or camera move
				if(selectedObj == null && (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved) {
					//2 fingers parallel to make character jump
					if(t0.deltaPosition.y > t0.deltaPosition.x && t1.deltaPosition.y > t1.deltaPosition.x) {
						print("JUMP");
					}
				}
				*/
			}

			//more than 2 fingers on screen
			else {
				print ("ERROR: no action intended for " + touchCount + " fingers on screen");
			}
		} //end foreach touch()
	} //end update()


	//(0, 0) is lower left corner
	//print ("pixel coord : x" + touch.position.x + " " + touch.position.y);
	public static float movementOnXAxis(Touch t0) {
		//movement on x axis
		if (t0.deltaPosition.x > t0.deltaPosition.y) {
			Vector2 prevPos = t0.position - t0.deltaPosition;
			Vector2 currPos = t0.position;

			//movement to left
			if(prevPos.x > currPos.x) {
				return -t0.deltaPosition.x;
			} else {
				return t0.deltaPosition.x;
			}
		}
		return 0;
	}

	public static float movementOnYAxis(Touch t0) {
		//movement on y axis
		if (t0.deltaPosition.y > t0.deltaPosition.x) {
			Vector2 prevPos = t0.position - t0.deltaPosition;
			Vector2 currPos = t0.position;

			//movement: down
			if(prevPos.y > currPos.y) {
				return -t0.deltaPosition.y;
			} else {
				return t0.deltaPosition.y;
			}
		}
		return 0;
	}

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
		checkPinch (t0, t1);

		if (tag == "scale" && pinch == true) {
			print("PINCH SCALE");
			if(pinchApart) {
				//scale up
				selectedObj.transform.localScale += new Vector3 (0.1F, 0, 0);
				/*
				if (t0.deltaPosition.y > t0.deltaPosition.x) {
					selectedObj.transform.localScale += new Vector3 (0, 0.1F, 0);
				} else {
					selectedObj.transform.localScale += new Vector3 (0.1F, 0, 0);
				}
				*/
			} else if(pinchContract) {
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
