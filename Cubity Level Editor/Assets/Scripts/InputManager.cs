using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private GameManager m_gameManager;

	private PlayerControls m_playerControls;
	
	private static GameObject selectedObj = null;
	private Vector3 currentTouchPoint;
	private Vector3 previousTouchPoint;
	private float singleTapDuration = 0;
	private bool singleTapActive = false;
	private Touch singleTapTouch;
	
	private bool touchMoved = false;
	
	private bool pinch = false;
	private bool pinchApart = false;
	private bool pinchContract = false;
	
	private const int raycastLength = 500;
	private int layerMask;
	
	// Use this for initialization
	void Start () {

		m_gameManager = this.GetComponent<GameManager>();
		if(m_gameManager == null) Debug.LogWarning("GameManager not found!");

		//get all scripts needed
		m_playerControls = FindObjectOfType<PlayerControls>();
		
		currentTouchPoint = new Vector3(0,0,0);
		previousTouchPoint = new Vector3(0,0,0);
		
		// use layermask to ignore player layer 
		// more info here: http://answers.unity3d.com/questions/8715/how-do-i-use-layermasks.html
		layerMask = ~(1 << LayerMask.NameToLayer("Player"));
	} //end Start()
		
	// Update is called once per frame
	void Update () {
		if(m_gameManager.m_gameIsPaused) return;

//		if (Input.touchCount == 0) {
//			singleTapActive = false;
//		} else 
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
					int tapCount = touch.tapCount;
					
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
					currentTouchPoint.x = touch.position.x;
					currentTouchPoint.y = touch.position.y;
					touchMoved = false;
				}
			}
		}
	}
	
	private void performActionSingleTouchDoubleTap(Touch touch)
	{
//		Debug.Log("doubleTap");
		//single touch, double click
		Rigidbody rb = sendRay(touch);
		//raycast hit rigidbody
		if(rb != null) {
			if(rb.gameObject == selectedObj){
				releaseSelection();
			}
			//double click on new object which has a CubeManager skript
			else if(rb.gameObject.GetComponent<CubeManager>() != null) {
				releaseSelection();
				selectedObj = rb.gameObject;
				initSelection();
			}
		} //else: do nothing
	} //end single touch, double click
	
	
	private void performActionSingleTouchSingleTap(Touch touch)
	{
		if (selectedObj == null) 
		{
			if(m_playerControls.m_controlsMode.Equals(PlayerControls.EControlsMode.POINT_AND_CLICK))
			{
				Vector3 screenCoordinate = new Vector3(touch.position.x, touch.position.y, 0f);
				m_playerControls.movePlayerByDestination(screenCoordinate);
			}
		}
	}
	
	
	private void performActionSingleTouchSingleMove(Touch touch) 
	{
		//cube action, singleTouchGesture: moveX, moveY, moveZ, rotate
		if (selectedObj != null) {
//			Debug.Log ("single touch gesture init");
			singleTouchGesture (touch); //= Input.GetTouch(0)
		} //end singleTouch			
		else { //selectedObj == null	

//			Vector3 screenCoordinate = new Vector3(touch.position.x, touch.position.y, 0f);
//			m_playerControls.movePlayerByDestination(screenCoordinate);

			float rotationY = touch.deltaPosition.y * Time.deltaTime * 10.0f;
			rotationY = Mathf.Clamp (rotationY, -90, 90);
			float rotationX = touch.deltaPosition.x * Time.deltaTime * 10.0f;
			rotationX = Mathf.Clamp (rotationX, -90, 90);
			m_playerControls.rotatePlayer (rotationX); //left and right
			m_playerControls.rotateCamera (rotationY); //up and down
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
//			if (t0.deltaPosition.y > t0.deltaPosition.x && t1.deltaPosition.y > t1.deltaPosition.x) {
//				Debug.Log ("JUMP");
//				m_playerControls.jump ();
//			}

			if(m_playerControls.m_controlsMode.Equals(PlayerControls.EControlsMode.SWIPE))
			{
				if((t0.position.y+t0.deltaPosition.y) != t0.position.y)
				{
					float moveDistance = t0.deltaPosition.y * Time.deltaTime;
					m_playerControls.movePlayerByDistance(moveDistance);
				}
			}
		}
	}

	//(0, 0) is lower left corner
	//Debug.Log ("pixel coord : x" + touch.position.x + " " + touch.position.y);
	public Rigidbody sendRay(Touch touch) {
		//releaseSelection ();
		Ray ray = Camera.main.ScreenPointToRay (touch.position);
		RaycastHit hit;
		//ray intersects any collider
		if (Physics.Raycast (ray, out hit, raycastLength, layerMask)) {
			//ray hits rigidbody
			//rigidbody should be used when obj is moving
			// do not return player if he accidentially hits himself
			if(hit.transform.gameObject.tag.Equals("Player")) return null;
			return hit.rigidbody;
		}
		return null;
	}
	
	public void initSelection() {
//		Debug.Log ("selection init: " + selectedObj.name);
		selectedObj.GetComponent<CubeManager>().SetSelected(true);
//		selectedObj.renderer.material.color = Color.green;
		//Debug.Log ("<init> selected obj tag: " + selectedObj.tag);
	}
	
	public void releaseSelection() {
//		Debug.Log ("selection release");
		if (selectedObj != null) {
			selectedObj.GetComponent<CubeManager>().SetSelected(false);
			selectedObj = null;
		}
	}
	
	public void twoTouchGesture(Touch t0, Touch t1) {

		checkPinch (t0, t1);

		CubeManager cubeManager = selectedObj.GetComponent<CubeManager>();

		if(pinch){
			switch (cubeManager.cubeType)
			{
			case CubeManager.ECubeType.SCALE_X:
				if(pinchApart && selectedObj.transform.localScale.x < cubeManager.maxScaleX) {
					selectedObj.transform.localScale += new Vector3 (0.2f, 0, 0);
					if(selectedObj.transform.localScale.x > cubeManager.maxScaleX){
						selectedObj.transform.localScale = new Vector3(cubeManager.maxScaleX,selectedObj.transform.localScale.y,selectedObj.transform.localScale.z);
					}
				} else if(pinchContract && selectedObj.transform.localScale.x > cubeManager.minScaleX) {
					selectedObj.transform.localScale += new Vector3 (-0.2f, 0, 0);
					if(selectedObj.transform.localScale.x < cubeManager.minScaleX){
						selectedObj.transform.localScale = new Vector3(cubeManager.minScaleX,selectedObj.transform.localScale.y,selectedObj.transform.localScale.z);
					}
				}
				break;
			case CubeManager.ECubeType.SCALE_Y:
				if(pinchApart && selectedObj.transform.localScale.y < cubeManager.maxScaleY) {
					selectedObj.transform.localScale += new Vector3 (0, 0.2f, 0);
					if(selectedObj.transform.localScale.y > cubeManager.maxScaleY){
						selectedObj.transform.localScale = new Vector3(selectedObj.transform.localScale.x,cubeManager.maxScaleY,selectedObj.transform.localScale.z);
					}
				} else if(pinchContract && selectedObj.transform.localScale.y > cubeManager.minScaleY) {
					selectedObj.transform.localScale += new Vector3 (0, -0.2f, 0);
					if(selectedObj.transform.localScale.y < cubeManager.minScaleY){
						selectedObj.transform.localScale = new Vector3(selectedObj.transform.localScale.x,cubeManager.minScaleY,selectedObj.transform.localScale.z);
					}
				}
				break;
			case CubeManager.ECubeType.SCALE_Z:
				if(pinchApart && selectedObj.transform.localScale.z < cubeManager.maxScaleZ) {
					selectedObj.transform.localScale += new Vector3 (0, 0, 0.2f);
					if(selectedObj.transform.localScale.z > cubeManager.maxScaleZ){
						selectedObj.transform.localScale = new Vector3(selectedObj.transform.localScale.x,selectedObj.transform.localScale.y,cubeManager.maxScaleZ);
					}
				} else if(pinchContract && selectedObj.transform.localScale.z > cubeManager.minScaleZ) {
					selectedObj.transform.localScale += new Vector3 (0, 0, -0.2f);
					if(selectedObj.transform.localScale.z < cubeManager.minScaleZ){
						selectedObj.transform.localScale = new Vector3(selectedObj.transform.localScale.x,selectedObj.transform.localScale.y,cubeManager.minScaleZ);
					}
				}
				break;
			default:
				Debug.LogWarning("Cube doesn't react on multitoch");
				break;
			}
		}

	} //end twoTouchGesture()
	
	public void checkPinch(Touch t0, Touch t1) {
		float prevDistance = Vector2.Distance (t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);
		float currDistance = Vector2.Distance (t0.position, t1.position);
		
		if (prevDistance < currDistance) {
			pinch = true;
			pinchApart = true;
			pinchContract = false;
//			Debug.Log ("fingers moving apart");
		} else if (currDistance < prevDistance) {
			pinch = true;
			pinchContract = true;
			pinchApart = false;
//			Debug.Log ("fingers moving together");
		} else {
			Debug.LogWarning ("no pinch - what are you doing???");
			pinch = false;
		}
	} //end checkPinch()
	
	public void singleTouchGesture(Touch touch) {
		if (selectedObj != null) {
			
			Vector3 FirstPos = Camera.main.WorldToScreenPoint(selectedObj.transform.position);
			currentTouchPoint.z = FirstPos.z;
			previousTouchPoint.z = FirstPos.z;
			Vector3 currentPos = Camera.main.ScreenToWorldPoint(currentTouchPoint);
			Vector3 prevPos = Camera.main.ScreenToWorldPoint(previousTouchPoint);

			float difference = 0;

			switch (selectedObj.GetComponent<CubeManager>().cubeType)
			{
			case CubeManager.ECubeType.TRANSLATE_X:
				difference = currentPos.x - prevPos.x;
				selectedObj.transform.rigidbody.AddRelativeForce(new Vector3(difference * 500, 0, 0));
				break;
			case CubeManager.ECubeType.TRANSLATE_Y:
				difference = currentPos.y - prevPos.y;
				selectedObj.transform.rigidbody.AddRelativeForce(new Vector3(0, difference * 500, 0));
				break;
			case CubeManager.ECubeType.TRANSLATE_Z:
				difference = currentPos.z - prevPos.z;
				selectedObj.transform.rigidbody.AddRelativeForce(new Vector3(0, 0, difference * 500));
				break;
			case CubeManager.ECubeType.ROTATE_X:
//				RotateCube(currentPos, prevPos);
				difference = currentPos.y - prevPos.y;
				selectedObj.transform.Rotate (transform.right * difference * 4f);
				break;
			case CubeManager.ECubeType.ROTATE_Y:
//				RotateCube(currentPos, prevPos);
				difference = currentPos.x - prevPos.x;
				selectedObj.transform.Rotate (transform.up * difference * 4f);
				break;
			case CubeManager.ECubeType.ROTATE_Z:
//				RotateCube(currentPos, prevPos);
				difference = currentPos.y - prevPos.y;		
				selectedObj.transform.Rotate (transform.forward * difference * 4f);
				break;
			case CubeManager.ECubeType.NONE:
				Debug.LogWarning("Cube doesn't have ECubeType");
				break;
			default:
				Debug.Log("Cube doesn't react on singleTouch");
				break;
			}

		}
	} //end singleTouchGesture()

//	private void RotateCube(Vector3 currentPos, Vector3 prevPos)
//	{
//		Vector3 center = selectedObj.transform.position;
//		Vector3 camera = Camera.main.transform.position;
//		Vector3 camVector = camera - center;
//		Vector3 targetDelta = currentPos - prevPos;
//		
//		//get the angle between transform.forward and target delta
//		float angleDiff = Vector3.Angle(camVector, targetDelta);
//		
//		// get its cross product, which is the axis of rotation to
//		// get from one vector to the other
//		Vector3 cross = Vector3.Cross(camVector, targetDelta);
//		
//		// apply torque along that axis according to the magnitude of the angle.
//		
//		Vector3 dragVector = cross * angleDiff;
//		
//		selectedObj.transform.rigidbody.AddRelativeTorque(dragVector * 2);
//	}
}