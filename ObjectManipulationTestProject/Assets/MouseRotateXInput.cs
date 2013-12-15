using UnityEngine;
using System.Collections;

public class MouseRotateXInput : MonoBehaviour {

	//public float cameraSensitivity = 90; //90
	//private float rotationX = 0.0f;
	//private float rotationY = 0.0f;

	private Vector3 screenPoint;
	private Vector3 firstPosition;
	private Vector3 curScreenPoint;
	//private Vector3 curPosition;
	//private Vector3 offset;
	public BoxRotateX boxRotateScript;

	Vector3 currentDir;
	//Vector3 previousDir;
	Vector3 dir;
	Quaternion offsetRotation;
	Quaternion originalRotation;

	//float xDeg = 0;
	//float yDeg = 0;
	
	// Use this for initialization
	void Start () {
		boxRotateScript = gameObject.GetComponent<BoxRotateX>();
		currentDir = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseDown () {
		if (Input.GetMouseButton(0)) {
			print ("Box Clicked!");
			boxRotateScript.setSelect(true);
		}

		originalRotation = transform.rotation;
		firstPosition = transform.position;
		screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, firstPosition.z));
		currentDir = screenPoint - firstPosition;
		offsetRotation = Quaternion.Inverse (Quaternion.LookRotation (currentDir));
		//previousDir = currentDir;
		//currentDir = dir;

		//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		//offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseUp() {
		if (Input.GetMouseButtonUp(0)) {
			print ("Box Released!");
			boxRotateScript.setSelect(false);
		}
	}
	
	void OnMouseDrag() {

		if(boxRotateScript.isSelected) {

			curScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, firstPosition.z));
			//previousDir = currentDir;
			currentDir = curScreenPoint - firstPosition;
			Quaternion newRotation = Quaternion.LookRotation (currentDir) * offsetRotation * originalRotation;


			if(boxRotateScript.rotationAxis == 0) { //x axis rotation
				newRotation.y = originalRotation.y;
				newRotation.z = originalRotation.z;
			}
			else if(boxRotateScript.rotationAxis == 1) { //y axis rotation
				newRotation.x = originalRotation.x;
				newRotation.z = originalRotation.z;
			}
			else { //z axis rotation
				newRotation.x = originalRotation.x;
				newRotation.y = originalRotation.y;
			}
		
			transform.rotation = newRotation;

		/*xDeg -= Input.GetAxis("Mouse X");
		yDeg += Input.GetAxis("Mouse Y");
		boxRotateScript.rotateCubeX(xDeg, yDeg);*/

		//float xOffset = Input.GetAxis("Mouse X");
		//Vector3 xOffset = Vector3.up * Input.GetAxis("Mouse X");
		//boxRotateScript.rotateCubeX(xOffset);
		/*	curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			float xOffset = (curPosition.y - firstPosition.y)*0.1f;
			print ("XOffset " + xOffset);
			boxRotateScript.rotateCubeX(xOffset);
			firstPosition.y += xOffset;
		*/

		/*rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
			rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
			//rotationY = Mathf.Clamp (rotationY, -90, 90);
		
			transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.down);
			transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.right);*/

		}

	}
}
