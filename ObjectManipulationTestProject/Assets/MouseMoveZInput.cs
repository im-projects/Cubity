using UnityEngine;
using System.Collections;

public class MouseMoveZInput : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 firstPosition;
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	private Vector3 offset;
	private Vector3 screenOffset;
	private Vector3 curScreenPosition;
	private Vector3 firstScreenPoint;
//	private float factorx = 0.05f;
//	private float factory = 0.05f;
	public BoxTranslateZ boxTranslateScript;
	
	// Use this for initialization
	void Start () {
		boxTranslateScript = gameObject.GetComponent<BoxTranslateZ>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown () {
		if (Input.GetMouseButton(0)) {
			print ("Box Clicked!");
			boxTranslateScript.setSelect(true);
		}
		
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		firstPosition = gameObject.transform.position;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		//screenOffset = screenPoint - new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
	}
	
	void OnMouseUp() {
		if (Input.GetMouseButtonUp(0)) {
			print ("Box Released!");
			boxTranslateScript.setSelect(false);
		}
	}
	
	void OnMouseDrag() {
		if(boxTranslateScript.isSelected) {
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z); //+ screenOffset;
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

			float zOffset = curPosition.z - firstPosition.z;

			//float xOffset = (curScreenPoint.x - screenPoint.x) * factorx;
			//float yOffset = (curScreenPoint.y - screenPoint.y) * factory;
			//float zOffset = -(xOffset + yOffset);
			//print ("zOffset " + zOffset + " with xOffset " + xOffset + " and yOffset " + yOffset);
			boxTranslateScript.translateCubeZ(zOffset);
			firstPosition.z = curPosition.z;
			screenPoint = curScreenPoint;

		}
	}
}
