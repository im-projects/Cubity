using UnityEngine;
using System.Collections;

public class touchBehaviourNew : MonoBehaviour {
	
	private GameObject selectedObj = null;
	
	//single Touch Variables for Moving -> Variablen aus MouseMoveInput Skripten
	private Vector3 TouchStartScreenPoint; //screenPoint
	private Vector3 TouchCurScreenPoint; //curScreenPoint
	private Vector3 CubeStartPosition; //firstPosition
	private Vector3 CubeCurPosition; //curPosition
	private Vector3 offset; // offset
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary) {
				if(selectedObj == null) {
					Ray ray = Camera.main.ScreenPointToRay (touch.position);
					RaycastHit hit;
					//ray intersects any collider
					if (Physics.Raycast (ray, out hit)) {
						//ray hits rigidbody
						//rigidbody should be used when obj is moving
						if (hit.rigidbody != null) {
							print ("box selected");
							selectedObj = hit.rigidbody.gameObject;
							
							//init single touch variables -> what is done in OnMouseDown
							if (selectedObj.tag == "moveX") {
								TouchStartScreenPoint = Camera.main.WorldToScreenPoint(selectedObj.transform.position);
								CubeStartPosition = selectedObj.transform.position;
								offset = CubeStartPosition - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, TouchStartScreenPoint.z));
							}
							
							initSelection();
						}
						else {
							//TODO: impl movement of player
						}
					}
				} 
				
			} else if (touch.phase == TouchPhase.Moved) {
				initMove(touch);
				print ("box moved");
			} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				releaseSelection();
				print ("Box Released!");
			} else {
				print ("unknown touchphase");
			}
		} //end foreach touch()
	} //end Update()
	
	public void initSelection() {
		print ("selection init");
		selectedObj.renderer.material.color = Color.green;
		print ("select color chosen");
	}
	
	public void initMove(Touch touch) {
		if (selectedObj != null) {
			if (selectedObj.tag == "moveX") { //what is done in OnMouseDrag + funktion in boxtranslatescript die aufgerufen wird -> einfach code reingehaut
				TouchCurScreenPoint = new Vector3(touch.position.x, touch.position.y, TouchStartScreenPoint.z);
				CubeCurPosition = Camera.main.ScreenToWorldPoint(TouchCurScreenPoint) + offset;
				float xOffset = CubeCurPosition.x - CubeStartPosition.x;
				Vector3 newPosition = transform.position;
				newPosition.x += xOffset;
				transform.position = newPosition;
				CubeStartPosition.x += xOffset;
				
				//float deltaPos = touch.deltaPosition.x / 80;
				//selectedObj.transform.Translate (Vector3.right * deltaPos, Space.Self);
				
			} else if (selectedObj.tag == "moveY") {
				float deltaPos = touch.deltaPosition.y / 80;
				selectedObj.transform.Translate (Vector3.up * deltaPos, Space.Self);
				
			} else if (selectedObj.tag == "moveZ") {
				float deltaPos = touch.deltaPosition.y / 80;
				selectedObj.transform.Translate (Vector3.forward * deltaPos, Space.Self);
				
			} else if (selectedObj.tag == "rotate") {
				float deltaPos = touch.deltaPosition.y;
				selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
				
			} else if (selectedObj.tag == "scale") {
				if(touch.deltaPosition.y > touch.deltaPosition.x) {
					selectedObj.transform.localScale += new Vector3(0, 0.1F, 0);
				} else {
					selectedObj.transform.localScale += new Vector3(0.1F, 0, 0);
				}
				
			} else {
				print ("obj has no tag");
			}
		}
	}
	
	public void releaseSelection() {
		print ("release selection");
		if (selectedObj != null) {
			selectedObj.renderer.material.color = Color.red;
			selectedObj = null;
		}
	}
	
}