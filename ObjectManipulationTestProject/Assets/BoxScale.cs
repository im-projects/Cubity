using UnityEngine;
using System.Collections;

public class BoxScale : MonoBehaviour {

	public bool isSelected = false;
	public float MaxScaleInPercent = 200.0f; //doppelt so groß
	public float MinScaleInPercent = 50.0f; //halb so groß
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
			if(currentScale < (MinScaleInPercent/100)) {
				newScale = initialScale*(MinScaleInPercent/100);
				currentScale = (MinScaleInPercent/100);
			}
			if(currentScale > (MaxScaleInPercent/100)) {
				newScale = initialScale*(MaxScaleInPercent/100);
				currentScale = (MaxScaleInPercent/100);
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
