using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour {
	
	public ECubeType cubeType = ECubeType.NONE;
	public Material materialNormal;
	public Material materialSelected;

	private bool isSelected = false;
	
	public enum ECubeType
	{
		NONE,
		TRANSLATE_X,
		TRANSLATE_Y,
		TRANSLATE_Z,
		ROTATE_X,
		ROTATE_Y,
		ROTATE_Z,
		SCALE_X,
		SCALE_Y,
		SCALE_Z
	}

	void Start()
	{
		SetMaterial();
	}

	private void SetMaterial ()
	{
		Transform[] children = GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			if (
				child.gameObject.name.Equals ("plane_north") || 
				child.gameObject.name.Equals ("plane_east") || 
				child.gameObject.name.Equals ("plane_south") || 
				child.gameObject.name.Equals ("plane_west")
				) 
			{
				child.gameObject.renderer.material = (isSelected) ? materialSelected : materialNormal;
			}
		}
	}

	public void SetSelected(bool value)
	{
		isSelected = value;
		SetMaterial();
	}
}
