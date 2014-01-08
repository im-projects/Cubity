using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider theCollider)
	{
		if(theCollider.gameObject.tag.Equals("Player"))
		{
			Debug.Log ("Player has reached the Goal Area");
		}
	}
}
