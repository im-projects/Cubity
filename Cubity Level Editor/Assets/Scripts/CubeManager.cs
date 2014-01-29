using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour {
	
	public ECubeType cubeType = ECubeType.NONE;
	public Material materialX;
	public Material materialXActive;
	public Material materialY;
	public Material materialYActive;
	public Material materialZ;
	public Material materialZActive;

	public float maxScaleX = 6;
	public float maxScaleY = 6;
	public float maxScaleZ = 6;
	public float minScaleX = 4;
	public float minScaleY = 4;
	public float minScaleZ = 4;

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
			if (child.gameObject.name.Equals ("plane_east") || child.gameObject.name.Equals ("plane_west")){
				child.gameObject.renderer.material = (isSelected) ? materialXActive : materialX;
			}
			if (child.gameObject.name.Equals ("plane_top") || child.gameObject.name.Equals ("plane_bottom")){
				child.gameObject.renderer.material = (isSelected) ? materialYActive : materialY;
			}
			if (child.gameObject.name.Equals ("plane_north") || child.gameObject.name.Equals ("plane_south")){
				child.gameObject.renderer.material = (isSelected) ? materialZActive : materialZ;
			}
		}
	}

	public void SetSelected(bool value)
	{
		isSelected = value;
		SetMaterial();
	}
}
