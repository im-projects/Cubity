using UnityEngine;
using System.Collections;

public class GUI_EventTestScript : MonoBehaviour {

	private GUI_Button aButton;

	// Use this for initialization
	void Start () {
		aButton = GameObject.Find("3DGUI/3DGUI Container/GUI Layer/GUI_Button").GetComponent<GUI_Button>();
		aButton.FireButtonEvent += DoSth;
	}
	
	private void DoSth(GameObject theSender)
	{
		Debug.Log (theSender.name + " event was subscribed, fired and recognized");
	}
}
