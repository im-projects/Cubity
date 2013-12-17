using UnityEngine;
using System.Collections;

public class touchBehaviour : MonoBehaviour {

	private GameObject selectedObj = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary) {
				if(selectedObj == null) {
					Ray ray = Camera.main.ScreenPointToRay (touch.position);
					RaycastHit hit;
					//ray intersects any collider
					if (Physics.Raycast (ray, out hit)) {
						//ray hits rigidbody
						//rigidbody should be used when obj is moving
						if (hit.rigidbody != null) {
							print ("box selected");
							selectedObj = hit.rigidbody.gameObject;
							initSelection();
						}
						else {
							//TODO: impl movement of player
						}
					}
				} 
				
			} else if (touch.phase == TouchPhase.Moved) {
				initMove(touch);
				print ("box moved");
			} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				releaseSelection();
				print ("Box Released!");
			} else {
				print ("unknown touchphase");
			}
		} //end foreach touch()
	} //end Update()

	public void initSelection() {
		print ("selection init");
		selectedObj.renderer.material.color = Color.green;
		print ("select color chosen");
	}

	public void initMove(Touch touch) {
		if (selectedObj != null) {
			if (selectedObj.tag == "moveX") {
					float deltaPos = touch.deltaPosition.x / 10;
					selectedObj.transform.Translate (Vector3.right * deltaPos, Space.Self);

			} else if (selectedObj.tag == "moveY") {
					float deltaPos = touch.deltaPosition.y / 10;
					selectedObj.transform.Translate (Vector3.up * deltaPos, Space.Self);

			} else if (selectedObj.tag == "moveZ") {

					float deltaPos = touch.deltaPosition.y / 10;
					selectedObj.transform.Translate (Vector3.forward * deltaPos, Space.Self);
			} else {
					print ("obj has no tag");
			}
		}
	}

	public void releaseSelection() {
		print ("release selection");
		if (selectedObj != null) {
			selectedObj.renderer.material.color = Color.red;
			selectedObj = null;
		}
	}

}