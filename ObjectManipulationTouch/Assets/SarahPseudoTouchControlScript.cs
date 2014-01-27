using UnityEngine;
using System.Collections;

public class SarahPseudoTouchControlScript : MonoBehaviour {

	public bool PlayerLocked = false;

	public bool simulateCubes = true;

	public bool simulateCamera = false;
	public bool simulateWalkingForwardBackward = true;
	public bool simulateClickToWalk = false;
	public bool simulateJump = false;

	private MoveCameraAndroidScript moveCameraScript;
	private RotatePlayerAndroidScript rotatePlayerScript;
	private MovePlayerForwardBackwardAndroidScript movePlayerScript;
	private MovePlayerByClickingAndroidScript moveByClickingScript;

	//private float cameraRotationX = 0.0f;
	//private float cameraRotationY = 0.0f;

	// Use this for initialization
	void Start () {
		//get all scripts needed
		moveCameraScript = FindObjectOfType<MoveCameraAndroidScript>();
		rotatePlayerScript = FindObjectOfType<RotatePlayerAndroidScript>();
		movePlayerScript = FindObjectOfType<MovePlayerForwardBackwardAndroidScript>();
		moveByClickingScript = FindObjectOfType<MovePlayerByClickingAndroidScript>();

	
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
						float rotationX = delta.x * Time.deltaTime;
						rotationX = Mathf.Clamp (rotationX, -90, 90);
						
						rotatePlayerScript.rotatePlayer(rotationX);
						moveCameraScript.rotateCamera(rotationY);


					}

					if(simulateWalkingForwardBackward) {
						Vector2 delta = Input.touches[0].deltaPosition;
						float moveDistance = delta.y * Time.deltaTime;

						movePlayerScript.movePlayer(moveDistance);
					}
				}

				if(simulateClickToWalk) {
					//Vector2 destination = Input.GetTouch(0).position;
					moveByClickingScript.movePlayer(Input.mousePosition);
				}

				if(simulateJump) {
					//movePlayerScript.jump();
					moveByClickingScript.jump();
				}
			}
		}

		//do objects stuff
	}
}
			