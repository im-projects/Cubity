using UnityEngine;
using System.Collections;

public class GUI_Container : MonoBehaviour {

	public GUI_Layer[] m_theLayers; 


	public bool TryOpenLayer (string layerName)
	{
		GUI_Layer theLayer = null;
		foreach(GUI_Layer aLayer in m_theLayers)
		{
			if(aLayer.gameObject.name == layerName)
			{
				theLayer = aLayer;
			}
		}

		if(theLayer != null)
		{
			theLayer.Open();
			return true;
		}
		Debug.LogWarning("Could not find GUI Layer");
		return false;
	}	

	public void CloseAllLayers ()
	{
		foreach(GUI_Layer aLayer in m_theLayers)
		{
			aLayer.Close();
		}
	}

	public bool GetAnimationIsRunning ()
	{
		foreach(GUI_Layer aLayer in m_theLayers)
		{
			if(aLayer.GetAnimationIsRunning())
				return true;
		}
		return false;
	}
}
