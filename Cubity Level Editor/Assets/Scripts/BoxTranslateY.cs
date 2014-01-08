using UnityEngine;
using System.Collections;

public class BoxTranslateY : MonoBehaviour {

	public Material materialNormal;
	public Material materialSelected;
	
	[HideInInspector]
	public bool isSelected = false;

	public void TranslateCubeY (float yOffset) 
	{
		if(isSelected)
		{
			Vector3 newPosition = transform.position;
			newPosition.y += yOffset;
			transform.position = newPosition;
		}
	}
	
	public void SetSelect(bool selected) 
	{
		isSelected = selected;
		SetMaterial();
	}
	
	//change look
	private void SetMaterial () 
	{
		Transform[] children = GetComponentsInChildren<Transform>();
		foreach(Transform child in children)
		{
			if(
				child.gameObject.name.Equals("plane_north") || 
				child.gameObject.name.Equals("plane_east") || 
				child.gameObject.name.Equals("plane_south") || 
				child.gameObject.name.Equals("plane_west")
				)
			{
				child.gameObject.renderer.material = (isSelected) ? materialSelected : materialNormal;
			}
		}
	}
}
