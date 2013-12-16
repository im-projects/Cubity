using UnityEngine;
using System.Collections;

public class touchBehaviour : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 firstPosition;
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	private Vector3 offset;
	public BoxTranslateX moveX;
	public BoxTranslateY moveY;
	public BoxTranslateZ moveZ;
	
	
	GameObject selectedObj;
	Vector3 positionSelectObj;
	
	// Use this for initialization
	void Start () {
		moveX = gameObject.GetComponent<BoxTranslateX> ();
		//moveY = gameObject.GetComponent<BoxTranslateY> ();
		//moveZ = gameObject.GetComponent<BoxTranslateZ> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.rigidbody != null) {
						selectedObj = hit.rigidbody.gameObject;
						
						if (selectedObj.tag == "moveX") {
							moveX.setSelect(true);
							screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
							firstPosition = gameObject.transform.position;
							offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
						} else if (selectedObj.tag == "moveY") {
							print ("to implement");
						} else if (selectedObj.tag == "moveZ") {
							print ("to implement");
						} else {
							print("obj has no tag!");
						}
					}
				} else {
					moveX.setSelect(false);
				}
				
			}
			if (touch.phase == TouchPhase.Moved) {
				if(selectedObj.tag == "moveX") {
					if(moveX.isSelected) {
						curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
						float xOffset = curPosition.x - firstPosition.x;
						print ("XOffset " + xOffset);
						moveX.translateCubeX(xOffset);
						firstPosition.x += xOffset;
					}
				}
			}
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				print ("Box Released!");
				moveX.setSelect(false);
			}
		}
	}
}