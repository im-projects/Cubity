using UnityEngine;
using System.Collections;

public class BoxScale : MonoBehaviour {

	public bool isSelected = false;
	Vector3 initialScale;
	private float currentScale;
	
	// Use this for initialization
	void Start () {
		initialScale = transform.localScale;
		currentScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void scaleCube (float scaleFactor) {
		if(isSelected){
			Vector3 newScale = transform.localScale*scaleFactor;
			currentScale *= scaleFactor;
			//beschränkung skalierung hardcoded, später mit variablen
			if(currentScale < 0.1f) {
				newScale = initialScale*0.1f;
				currentScale = 0.1f;
			}
			if(currentScale > 10.0f) {
				newScale = initialScale*10.0f;
				currentScale = 10.0f;
			}
			transform.localScale = newScale;
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
