using UnityEngine;
using System.Collections;

public class MouseScaleInput : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 curScreenPoint;
	private Vector3 boxCenterPoint;
	private float oldDistance;
	public BoxScale boxScaleScript;
	
	// Use this for initialization
	void Start () {
		boxScaleScript = gameObject.GetComponent<BoxScale>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown () {
		if (Input.GetMouseButton(0)) {
			print ("Box Clicked!");
			boxScaleScript.setSelect(true);
		}

		boxCenterPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, boxCenterPoint.z);

		oldDistance = (screenPoint - boxCenterPoint).magnitude;
	}
	
	void OnMouseUp() {
		if (Input.GetMouseButtonUp(0)) {
			print ("Box Released!");
			boxScaleScript.setSelect(false);
		}
	}
	
	void OnMouseDrag() {
		if(boxScaleScript.isSelected) {
			//scale depends on drag direction -> from center bigger, to center smaller
			//if drag goes over center it will scale according to which distance from the center is bigger
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, boxCenterPoint.z);
			float newDistance = (curScreenPoint - boxCenterPoint).magnitude;
			float scaleDistance = newDistance - oldDistance;
			float scaleFactor = 1;
			scaleFactor += (scaleDistance/80);

			boxScaleScript.scaleCube(scaleFactor);
			oldDistance = newDistance;
		}
	}
}
