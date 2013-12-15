using UnityEngine;
using System.Collections;

public class BoxTranslateZ : MonoBehaviour {
	public bool isSelected = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void translateCubeZ (float zOffset) {
		if(isSelected){
			Vector3 newPosition = transform.position;
			newPosition.z += zOffset;
			transform.position = newPosition;
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
