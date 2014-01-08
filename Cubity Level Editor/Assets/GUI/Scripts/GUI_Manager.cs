using UnityEngine;
using System.Collections;

public class GUI_Manager : MonoBehaviour {
	
	public string m_defaultLayer;
	public GUI_Container m_layerContainer;

	private bool m_hasOpenLayers = false;


	public void Update()
	{
		CheckInputs();
	}

	public void CloseAllMenus()
	{
		if(!m_hasOpenLayers) return;

		m_layerContainer.CloseAllLayers();
		m_hasOpenLayers = false;
	}

	// try to open a specific GUILayer
	public void OpenMenu(string layerName)
	{
		m_hasOpenLayers = (m_layerContainer.TryOpenLayer(layerName) || m_hasOpenLayers);
	}

	private void CheckInputs()
	{
		// ESC toggles Menu
		if(Input.GetKeyDown (KeyCode.Escape) && !m_layerContainer.GetAnimationIsRunning())
		{
			if(!m_hasOpenLayers)
			{
				OpenMenu(m_defaultLayer);
			}
			else
			{
				CloseAllMenus();
			}
		} 
	}
}
