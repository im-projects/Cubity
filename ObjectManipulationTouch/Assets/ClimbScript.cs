using UnityEngine;
using System.Collections;

public class ClimbScript : MonoBehaviour {
	//public float MAXHEIGHT = 20.0f;
	//private float CurHeight = 0.0f;

	public float MAXFORWARD = 10.0f;
	private float CurForward = 0.0f;

	private bool Climbing = false;
	private bool EndClimb = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if(CurHeight >= MAXHEIGHT) {
			Climbing = false;
			CurHeight = 0.0f;
		}*/
		if(CurForward >= MAXFORWARD) {
			print ("WalkEnd");
			EndClimb = false;
			CurForward = 0.0f;
		}
	
	}

	public bool isClimbing(){
		return Climbing;
	}

	public bool isEndClimb(){
		return EndClimb;
	}
	public void changeForward(float addme) {
		CurForward += addme;
	}

	/*public void changeHeight(float addme) {
		CurHeight += addme;
	}*/

	void OnTriggerEnter (Collider col) {
		//check if Grounded - else always climbs because it collides while falling
		//if(col.gameObject.tag != "wall") {	//change to check if col = climbCollider of the object
		if(col.gameObject.name == "ClimbingWall") { //&& EndClimb == false) {
			//only climb when true
			print("Start Climbing");
			print(col.gameObject.tag);

			Climbing = true;
		}
	}

	void OnTriggerExit (Collider col) {
		//if(col.gameObject.tag != "wall") {	//change to check if col = climbCollider of the object
		if(col.gameObject.name == "ClimbingWall" && Climbing == true){
			//only stop climbing when true
			print("Stop Climbing");

			Climbing = false;
			//CurHeight = 0.0f;
			EndClimb = true;
			print ("Walk");
		}
	}
}
