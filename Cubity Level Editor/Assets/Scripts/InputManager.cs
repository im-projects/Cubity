using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private GameObject selectedObj = null;
	private GameManager gameManager;

	void Start()
	{
		gameManager = this.GetComponent<GameManager>();
		if(gameManager == null) Debug.LogWarning("GameManager not found!");
	}

	// Update is called once per frame
	void Update () {
		if(gameManager.m_gameIsPaused) return;

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				Debug.Log (touch.phase);
				if(selectedObj == null) {
					Ray ray = Camera.main.ScreenPointToRay (touch.position);
					RaycastHit hit;
					//ray intersects any collider
					if (Physics.Raycast (ray, out hit)) {
						//ray hits rigidbody
						//rigidbody should be used when obj is moving
						if (hit.rigidbody != null) {
							if(hit.rigidbody.gameObject.GetComponent<CubeManager>() != null)
							{
//								print ("box selected");
								selectedObj = hit.rigidbody.gameObject;
								InitSelection();
							}
						}
						else {
							//TODO: impl movement of player
						}
					}
				} 
				
			} else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
				if(selectedObj != null) {
					InitMove(touch);
//					print ("box moved");
				}
				else {
					//TODO: impl movement of player
				}
			} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				if(selectedObj != null) {
					ZeroizeRigidbodyForce ();
					ResetSelection ();
	//				print ("Box Released!");
				}
			} else {
				Debug.LogWarning ("unknown touchphase");
			}
		}
	}

	private void InitMove(Touch touch) {
		float deltaPos = 0;

		switch (selectedObj.GetComponent<CubeManager>().cubeType)
		{
		case CubeManager.ECubeType.TRANSLATE_X:
			deltaPos = touch.deltaPosition.x / 80;
			selectedObj.transform.Translate (Vector3.right * deltaPos, Space.Self);
			break;
		case CubeManager.ECubeType.TRANSLATE_Y:
			deltaPos = touch.deltaPosition.y / 80;
			selectedObj.transform.Translate (Vector3.up * deltaPos, Space.Self);
			break;
		case CubeManager.ECubeType.TRANSLATE_Z:
			deltaPos = touch.deltaPosition.y / 80;
			selectedObj.transform.Translate (Vector3.forward * deltaPos, Space.Self);
			break;
		case CubeManager.ECubeType.ROTATE_X:
			deltaPos = touch.deltaPosition.y;
			selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
			break;
		case CubeManager.ECubeType.ROTATE_Y:
			deltaPos = touch.deltaPosition.y;
			selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
			break;
		case CubeManager.ECubeType.ROTATE_Z:
			deltaPos = touch.deltaPosition.y;
			selectedObj.transform.Rotate (Vector3.right * deltaPos, Space.Self);
			break;
		case CubeManager.ECubeType.SCALE_X:
			if(touch.deltaPosition.y > touch.deltaPosition.x) {
				selectedObj.transform.localScale += new Vector3(0, 0.1F, 0);
			} else {
				selectedObj.transform.localScale += new Vector3(0.1F, 0, 0);
			}
			break;
		case CubeManager.ECubeType.SCALE_Y:
			if(touch.deltaPosition.y > touch.deltaPosition.x) {
				selectedObj.transform.localScale += new Vector3(0, 0.1F, 0);
			} else {
				selectedObj.transform.localScale += new Vector3(0.1F, 0, 0);
			}
			break;
		case CubeManager.ECubeType.SCALE_Z:
			if(touch.deltaPosition.y > touch.deltaPosition.x) {
				selectedObj.transform.localScale += new Vector3(0, 0.1F, 0);
			} else {
				selectedObj.transform.localScale += new Vector3(0.1F, 0, 0);
			}
			break;
		case CubeManager.ECubeType.NONE:
			Debug.LogWarning("Cube doesn't have ECubeType");
			break;
		default:
			Debug.LogWarning("Cube doesn't have ECubeType");
			break;
		}
	}
	
	private void InitSelection() {
		Debug.Log ("InitSelection");
		selectedObj.GetComponent<CubeManager>().SetSelected(true);
	}
	
	private void ResetSelection ()
	{
		Debug.Log ("ResetSelection");
		selectedObj.GetComponent<CubeManager>().SetSelected(false);
		selectedObj = null;
	}

	private void ZeroizeRigidbodyForce ()
	{
		// sets rigidbody velocity to zero, this avoid the cube to move after a collision
		selectedObj.rigidbody.velocity = Vector3.zero;
		selectedObj.rigidbody.angularVelocity = Vector3.zero;
	}
}