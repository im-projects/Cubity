using UnityEngine;
using System.Collections;

public class MouseMoveXInput : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 firstPosition;
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	private Vector3 offset;
	public BoxTranslateX boxTranslateScript;

	// Use this for initialization
	void Start () {
		boxTranslateScript = gameObject.GetComponent<BoxTranslateX>();
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
	}
	
	void OnMouseUp() {
		if (Input.GetMouseButtonUp(0)) {
        	print ("Box Released!");
			boxTranslateScript.setSelect(false);
        }
	}
	
	void OnMouseDrag() {
		if(boxTranslateScript.isSelected) {
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			float xOffset = curPosition.x - firstPosition.x;
			print ("XOffset " + xOffset);
			boxTranslateScript.translateCubeX(xOffset);
			firstPosition.x += xOffset;
		}
	}
	
}
