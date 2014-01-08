using UnityEngine;
using System.Collections;

public class LevelDesigner : MonoBehaviour {
	
	public GameObject prefab;
	public Vector3 gizmoPosition;
	public int depth = 0;
	public Color gizmosColor = Color.grey;
	public Vector3 rotation;
	public Quaternion editorLookFromQuat = Quaternion.identity;
	
	void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireCube(new Vector3(gizmoPosition.x,gizmoPosition.y,gizmoPosition.z),prefab.transform.localScale);
	}
}
