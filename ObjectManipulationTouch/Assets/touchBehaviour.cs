using UnityEngine;
using System.Collections;

public class touchBehaviour : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 firstPosition;
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	//private Vector3 offset;
	private bool moveX = false;
	private bool moveY = false;
	private bool moveZ = false;

	private GameObject selectedObj = null;
	private Vector3 positionSelectObj;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary) {
				print ("box selected");
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.rigidbody != null) {
						selectedObj = hit.rigidbody.gameObject;
						
						if (selectedObj.tag == "moveX") {
							select (moveX);
							screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
							firstPosition = gameObject.transform.position;
							//offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
						
						} else if (selectedObj.tag == "moveY") {
							select (moveY);
							screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
							firstPosition = gameObject.transform.position;
						
						} else if (selectedObj.tag == "moveZ") {
							select (moveZ);
							screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
							firstPosition = gameObject.transform.position;
						
						} else {
							print("obj has no tag!");
						}
					}
				} else {
					//TODO: impl movement of player
				}
				
			}
			if (touch.phase == TouchPhase.Moved) {
				if(selectedObj.tag == "moveX") {
					if(moveX) {
						curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
						float xOffset = curPosition.x - firstPosition.x;
						print ("XOffset " + xOffset);
						translate(xOffset, selectedObj.tag);
						firstPosition.x += xOffset;
					}
				
				} else if(selectedObj.tag == "moveY") {
					if(moveY) {
						curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
						float yOffset = curPosition.y - firstPosition.y;
						print ("YOffset " + yOffset);
						translate(yOffset, selectedObj.tag);
						firstPosition.y += yOffset;
					}
				
				} else if(selectedObj.tag == "moveZ") {
					if(moveZ) {
						curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
						float zOffset = curPosition.z - firstPosition.z;
						print ("ZOffset " + zOffset);
						translate(zOffset, selectedObj.tag);
						firstPosition.z += zOffset;
					}
				}
				print ("box moved");
			}
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				if(selectedObj.tag == "moveX") {
					unselect(moveX);
				} else if(selectedObj.tag == "moveY") {
					unselect(moveY);
				} else if(selectedObj.tag == "moveZ") {
					unselect(moveZ);
				} else {
					print ("no box was selected");
				}
				print ("Box Released!");
				selectedObj = null;
			}
		} //end foreach touch()
	} //end Update()

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
			transform.position = newPosition;
		//}
	}

	public void select(bool cube) {
		selectedObj.renderer.material.color = Color.green;
		//selectedObj.renderer.material = Resources.Load("SelectMaterial", typeof(Material)) as Material;
		print ("select color chosen");
		cube = true;
	}

	public void unselect(bool cube) {
		selectedObj.renderer.material.color = Color.red;
		//selectedObj.renderer.material = Resources.Load("UnselectMaterial", typeof(Material)) as Material;
		print ("unselect color chosen");
		cube = false;
	}

}