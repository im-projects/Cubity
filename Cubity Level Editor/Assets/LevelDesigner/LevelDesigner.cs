using UnityEngine;
using System.Collections;

public class LevelDesigner : MonoBehaviour {
	
	public GameObject prefab;
	public Vector3 gizmoPosition;
	public int depth = 0;
	public Color gizmosColor = Color.grey;
	public Vector3 rotation;
	public Quaternion editorLookFromQuat = Quaternion.identity;

	private GameObject tileContainer;
	private const string TILE_CONTAINER_NAME = "LevelTiles";

	void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireCube(new Vector3(gizmoPosition.x,gizmoPosition.y,gizmoPosition.z),prefab.transform.localScale);
	}
	
	private GameObject CreateTileContainer()
	{
		tileContainer = (GameObject.Find(TILE_CONTAINER_NAME)) ? GameObject.Find(TILE_CONTAINER_NAME) : new GameObject(TILE_CONTAINER_NAME);
		tileContainer.transform.position = Vector3.zero;
		return tileContainer;
	}

	public GameObject GetTileContainer()
	{
		return (tileContainer != null) ? tileContainer : CreateTileContainer();
	}
}
