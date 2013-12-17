using UnityEngine;
using System.Collections;

public class touchBehaviour : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 firstPosition;
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	//private Vector3 offset;

	private GameObject selectedObj = null;
	//private Vector3 positionSelectObj;

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
						if (hit.rigidbody != null) {
							print ("box selected");
							selectedObj = hit.rigidbody.gameObject;
							initSelection();
						}
						else {
							//TODO: impl movement of player
						}
					}
				} 
				
			} else if (touch.phase == TouchPhase.Moved) {
				initMove();
				print ("box moved");
			} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				releaseSelection();
				print ("Box Released!");
				selectedObj = null;
			} else {
				print ("unknown touchphase");
			}
		} //end foreach touch()
	} //end Update()

	public void initSelection() {
		print ("selection init");
		screenPoint = Camera.main.WorldToScreenPoint(selectedObj.transform.position);
		firstPosition = selectedObj.transform.position;
		selectedObj.renderer.material.color = Color.green;
		print ("select color chosen");
	}

	public void initMove() {
		if(selectedObj.tag == "moveX") {
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			float xOffset = curPosition.x - firstPosition.x;
			print ("XOffset " + xOffset);
			translate(xOffset, selectedObj.tag);
			firstPosition.x += xOffset;
			
		} else if(selectedObj.tag == "moveY") {
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			float yOffset = curPosition.y - firstPosition.y;
			print ("YOffset " + yOffset);
			translate(yOffset, selectedObj.tag);
			firstPosition.y += yOffset;
			
		} else if(selectedObj.tag == "moveZ") {
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			float zOffset = curPosition.z - firstPosition.z;
			print ("ZOffset " + zOffset);
			translate(zOffset, selectedObj.tag);
			firstPosition.z += zOffset;
		}

	}

	public void translate(float offset, string tag) {
		//if(isSelected){
			Vector3 newPosition = transform.position;
			if(tag == "moveX") {
				newPosition.x += offset;
			} else if(tag == "moveY") {
				newPosition.y += offset;
			} else if(tag == "moveZ") {
				newPosition.z += offset;
			}
			selectedObj.transform.position = newPosition;
		//}
	}

	public void releaseSelection() {
		print ("release selection");
		//TODO ev fehlerquelle: reset screenPoint + firstPos
		//screenPoint = null;
		//firstPosition = null;
		selectedObj.renderer.material.color = Color.red;
		selectedObj = null;
	}

}