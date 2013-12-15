using UnityEngine;
using System.Collections;

public class BoxRotateX : MonoBehaviour {

	public int rotationAxis = 0; //0 is x axis, 1 is y axis, 2 and rest is z axis
	public bool isSelected = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void rotateCubeX (float xOffset, float yOffset) {
		//dummy method, doesn't do anything
		//zeug aus mouse rotate x input muss noch ausgelagert werden
		//methodenargumente müssn erst neu festgelegt werden
		//evt. der neue rotationswert oder die rotationsrichtung (+offset)
		if(isSelected){
			//TODO Rotation anwenden (evt. hier berechnen)

			/*xOffset = 0;
			Quaternion fromRotation = transform.rotation;
			Quaternion toRotation = Quaternion.Euler(yOffset*3,xOffset,0);
			transform.rotation = Quaternion.Lerp(fromRotation,toRotation,Time.deltaTime);
			*/

			//transform.Rotate(xOffset, 0, 0);
			//Vector3 newPosition = transform.position;
			//newPosition.x += xOffset;
			//transform.position = newPosition;
		}
	}
	
	public void setSelect(bool selected) {
		isSelected = selected;
		setMaterial();
	}
	
	//change look
	void setMaterial () {
		Material selectMat;
		if(isSelected) {
			selectMat = Resources.Load("SelectMaterial", typeof(Material)) as Material;
			
		} else {
			selectMat = Resources.Load("UnselectedMaterial", typeof(Material)) as Material;
		}
		
		gameObject.renderer.material = selectMat;
	}
}
