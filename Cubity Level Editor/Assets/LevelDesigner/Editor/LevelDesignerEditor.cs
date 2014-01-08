using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelDesigner))]
public class LevelDesignerEditor : Editor {
	
	LevelDesigner theScript;
	EBatchMode batchmode = EBatchMode.NONE;
	EEditorLookFrom editorLookFrom = EEditorLookFrom.TOP;
	Vector3 oldTilePos;
	
	enum EBatchMode
	{
		CREATE,
		DELETE,
		NONE
	}
	
	enum EEditorLookFrom
	{
		TOP,
		LEFT,
		RIGHT,
		FRONT,
		BACK,
		BOTTOM
	}
	
	void OnEnable(){
		theScript = (LevelDesigner) target;
		
		if(!Application.isPlaying)
		{
			if(SceneView.lastActiveSceneView != null)
			{
				Tools.current = Tool.View;
				batchmode = EBatchMode.NONE;
				editorLookFrom = EEditorLookFrom.TOP;
			}
		}
	}
	
	public override void OnInspectorGUI(){
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Prefab");
		theScript.prefab = (GameObject) EditorGUILayout.ObjectField(theScript.prefab,typeof(GameObject),false);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Depth");
		theScript.depth = Mathf.RoundToInt(EditorGUILayout.Slider(theScript.depth,-20,20));
		EditorGUILayout.EndHorizontal();
		
		theScript.rotation = EditorGUILayout.Vector3Field("Rotation",theScript.rotation);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Editor Camera look at");
		editorLookFrom = (EEditorLookFrom)EditorGUILayout.EnumPopup(editorLookFrom);
		EditorGUILayout.EndHorizontal();
		
		if(GUI.changed){
			EditorUtility.SetDirty(target);
			SetEditorCameraLookFrom();
		}
	}
	
	void OnSceneGUI(){
		
		if(theScript.prefab == null)
			return;
		
		Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		Vector3 tilePos = new Vector3();
		
		switch(editorLookFrom)
		{
			case EEditorLookFrom.TOP:
				tilePos.x = Mathf.RoundToInt(ray.origin.x / theScript.prefab.transform.localScale.x) * theScript.prefab.transform.localScale.x;
				tilePos.y = theScript.depth * theScript.prefab.transform.localScale.y;
				tilePos.z = Mathf.RoundToInt(ray.origin.z / theScript.prefab.transform.localScale.z) * theScript.prefab.transform.localScale.z;
				break;
			case EEditorLookFrom.LEFT:
				tilePos.x = -theScript.depth * theScript.prefab.transform.localScale.x;
				tilePos.y = Mathf.RoundToInt(ray.origin.y / theScript.prefab.transform.localScale.y) * theScript.prefab.transform.localScale.y;
				tilePos.z = Mathf.RoundToInt(ray.origin.z / theScript.prefab.transform.localScale.z) * theScript.prefab.transform.localScale.z;
				break;
			case EEditorLookFrom.RIGHT:
				tilePos.x = theScript.depth * theScript.prefab.transform.localScale.x;
				tilePos.y = Mathf.RoundToInt(ray.origin.y / theScript.prefab.transform.localScale.y) * theScript.prefab.transform.localScale.y;
				tilePos.z = Mathf.RoundToInt(ray.origin.z / theScript.prefab.transform.localScale.z) * theScript.prefab.transform.localScale.z;
				break;
			case EEditorLookFrom.FRONT:
				tilePos.x = Mathf.RoundToInt(ray.origin.x / theScript.prefab.transform.localScale.x) * theScript.prefab.transform.localScale.x;
				tilePos.y = Mathf.RoundToInt(ray.origin.y / theScript.prefab.transform.localScale.y) * theScript.prefab.transform.localScale.y;
				tilePos.z = theScript.depth * theScript.prefab.transform.localScale.z;
				break;
			case EEditorLookFrom.BACK:
				tilePos.x = Mathf.RoundToInt(ray.origin.x / theScript.prefab.transform.localScale.x) * theScript.prefab.transform.localScale.x;
				tilePos.y = Mathf.RoundToInt(ray.origin.y / theScript.prefab.transform.localScale.y) * theScript.prefab.transform.localScale.y;
				tilePos.z = -theScript.depth * theScript.prefab.transform.localScale.z;
				break;
			case EEditorLookFrom.BOTTOM:
				tilePos.x = Mathf.RoundToInt(ray.origin.x / theScript.prefab.transform.localScale.x) * theScript.prefab.transform.localScale.x;
				tilePos.y = -theScript.depth * theScript.prefab.transform.localScale.y;
				tilePos.z = Mathf.RoundToInt(ray.origin.z / theScript.prefab.transform.localScale.z) * theScript.prefab.transform.localScale.z;
				break;
		}
		
		if(tilePos != oldTilePos){
			theScript.gizmoPosition = tilePos;
			SceneView.RepaintAll();
			oldTilePos = tilePos;
		}
		
		// input event
		Event current = Event.current;
		
		if(current.keyCode == KeyCode.C)
		{
			if(current.type == EventType.keyDown)
			{
				batchmode = EBatchMode.CREATE;
			}
			else if(current.type == EventType.keyUp)
			{
				batchmode = EBatchMode.NONE;
			}
		}
		if(current.keyCode == KeyCode.D)
		{
			if(current.type == EventType.keyDown)
			{
				batchmode = EBatchMode.DELETE;
			}
			else if(current.type == EventType.keyUp)
			{
				batchmode = EBatchMode.NONE;
			}
		}
		
		if(current.type == EventType.mouseDown)
		{
			SceneView.lastActiveSceneView.orthographic = true;
			SceneView.lastActiveSceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, theScript.editorLookFromQuat);
		}
		
		if((current.type == EventType.mouseDown) || (batchmode != EBatchMode.NONE))
		{
			string name = string.Format(theScript.prefab.name + "_{0}_{1}_{2}",tilePos.x,tilePos.y,tilePos.z);
			if((current.button == 0) || (batchmode == EBatchMode.CREATE))
			{
				// Create
				CreateTile(tilePos,name);
			} 
			
			if((current.button == 1) || (batchmode == EBatchMode.DELETE))
			{
				// Delete
				DeleteTile(name);
			}
		}
		
		SetGizmosColor();
		
		if(GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}
	
	void CreateTile(Vector3 tilePos, string name)
	{
		if(!GameObject.Find(name))
		{
			Vector3 pos = new Vector3(tilePos.x,tilePos.y,tilePos.z);
			Quaternion quat = new Quaternion();
			quat.eulerAngles = theScript.rotation;
			GameObject go = (GameObject) GameObject.Instantiate(theScript.prefab,pos,quat);
			go.name = name;
			go.transform.parent = theScript.GetTileContainer().transform;
		}
	}
	
	void DeleteTile(string name)
	{
		GameObject go = GameObject.Find (name);
		if(null != go)
		{
			DestroyImmediate(go);
		}
	}
	
	void SetGizmosColor()
	{
		switch(batchmode)
		{
			case EBatchMode.NONE:
				theScript.gizmosColor = Color.grey;
				break;
			case EBatchMode.CREATE:
				theScript.gizmosColor = Color.white;
				break;
			case EBatchMode.DELETE:
				theScript.gizmosColor = Color.red;
				break;
		}
	}
	
	void SetEditorCameraLookFrom()
	{
		switch(editorLookFrom)
		{
			case EEditorLookFrom.TOP:
				theScript.editorLookFromQuat = Quaternion.Euler(90f,0,0);
				break;
			case EEditorLookFrom.LEFT:
				theScript.editorLookFromQuat = Quaternion.Euler(0,90f,0);
				break;
			case EEditorLookFrom.RIGHT:
				theScript.editorLookFromQuat = Quaternion.Euler(0f,270f,0f);
				break;
			case EEditorLookFrom.FRONT:
				theScript.editorLookFromQuat = Quaternion.Euler(0f,180f,0f);
				break;
			case EEditorLookFrom.BACK:
				theScript.editorLookFromQuat = Quaternion.identity;
				break;
			case EEditorLookFrom.BOTTOM:
				theScript.editorLookFromQuat = Quaternion.Euler(270f,0f,0f);
				break;
		}
			
		SceneView.lastActiveSceneView.orthographic = true;
		SceneView.lastActiveSceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, theScript.editorLookFromQuat);
	}
	
}
