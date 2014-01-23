using UnityEngine;
using System.Collections;

public class SarahPseudoTouchControlScript : MonoBehaviour {

	public bool PlayerLocked = false;

	public bool simulateCubes = true;

	public bool simulateCamera = true;
	public bool simulateWalkingForwardBackward = false;
	public bool simulateClickToWalk = false;
	public bool simulateJump = false;

	private MoveCameraAndroidScript moveCameraScript;
	private RotatePlayerAndroidScript rotatePlayerScript;

	private float cameraRotationX = 0.0f;
	private float cameraRotationY = 0.0f;

	// Use this for initialization
	void Start () {
		//get all scripts needed
		moveCameraScript = transform.Find("MainCamera").GetComponent<MoveCameraAndroidScript>();
		rotatePlayerScript = transform.Find("CameraCube").GetComponent<RotatePlayerAndroidScript>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if(PlayerLocked == false) {
			//do camera/player stuff
			if (Input.touches.Length > 0)
			{
				if (Input.touches[0].phase == TouchPhase.Moved)
				{
					if(simulateCamera) {
						Vector2 delta = Input.touches[0].deltaPosition;
						float rotationY = delta.y * Time.deltaTime;
						rotationY = Mathf.Clamp (rotationY, -90, 90);

						moveCameraScript.rotateCamera(rotationY);

						float rotationX = delta.x * Time.deltaTime;
						rotationX = Mathf.Clamp (rotationX, -90, 90);

						rotatePlayerScript.rotatePlayer(rotationX);
					}

					if(simulateWalkingForwardBackward) {

					}

					if(simulateJump) {

					}
				}

				if(simulateClickToWalk) {

				}
			}
		}

		//do objects stuff
	}
}
			