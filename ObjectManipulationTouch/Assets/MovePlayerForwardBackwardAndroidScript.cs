using UnityEngine;
using System.Collections;

public class MovePlayerForwardBackwardAndroidScript : MonoBehaviour {

	public float moveSpeed = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void movePlayer(float moveDistance) {
		transform.Translate (Vector3.forward * moveSpeed * moveDistance);
	}
}
