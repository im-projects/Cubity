using UnityEngine;
using System.Collections;

public class GUI_Manager : MonoBehaviour {

	public GameManager m_gameManager;
	public string m_defaultLayer;
	public GUI_Container m_layerContainer;

	private bool m_hasOpenLayers = false;

	private GUI_Button aButton_MainMenu_Continue;
	private GUI_Button aButton_MainMenu_LoadLevel1;
	private GUI_Button aButton_MainMenu_LoadLevel2;
	private GUI_Button aButton_MainMenu_LoadLevel3;
	private GUI_Button aButton_MainMenu_LoadLevel4;
	private GUI_Button aButton_MainMenu_LoadLevel5;
	private GUI_Button aButton_MainMenu_LoadLevel6;
	private GUI_Button aButton_MainMenu_Credits;
	private GUI_Button aButton_MainMenu_Quit;
	private GUI_Button aButton_MainMenu_Help;
	private GUI_Button aButton_Help_Back;
	private GUI_Button aButton_Help_Next;
	private GUI_Button aButton_Help2_Back;
	private GUI_Button aButton_Help2_Use;
	private GUI_Button aButton_Help2_Next;
	private GUI_Button aButton_Help2_Prev;
	private GUI_Button aButton_Help3_Back;
	private GUI_Button aButton_Help3_Use;
	private GUI_Button aButton_Help3_Prev;
	private GUI_Button aButton_Credits_Back;
	
	void Start()
	{
		aButton_MainMenu_Continue = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_Continue").GetComponent<GUI_Button>();
		aButton_MainMenu_LoadLevel1 = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_LoadLevel1").GetComponent<GUI_Button>();
		aButton_MainMenu_LoadLevel2 = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_LoadLevel2").GetComponent<GUI_Button>();
		aButton_MainMenu_LoadLevel3 = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_LoadLevel3").GetComponent<GUI_Button>();
		aButton_MainMenu_LoadLevel4 = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_LoadLevel4").GetComponent<GUI_Button>();
		aButton_MainMenu_LoadLevel5 = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_LoadLevel5").GetComponent<GUI_Button>();
		aButton_MainMenu_LoadLevel6 = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_LoadLevel6").GetComponent<GUI_Button>();
		aButton_MainMenu_Credits = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_Credits").GetComponent<GUI_Button>();
		aButton_MainMenu_Quit = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_Quit").GetComponent<GUI_Button>();
		aButton_MainMenu_Help = GameObject.Find("3DGUI/3DGUI Container/MainMenu/BTN_Help").GetComponent<GUI_Button>();
		aButton_Help_Back = GameObject.Find("3DGUI/3DGUI Container/Help/BTN_Back").GetComponent<GUI_Button>();
		aButton_Help_Next = GameObject.Find("3DGUI/3DGUI Container/Help/BTN_Next").GetComponent<GUI_Button>();
		aButton_Help2_Back = GameObject.Find("3DGUI/3DGUI Container/Help2/BTN_Back").GetComponent<GUI_Button>();
		aButton_Help2_Use = GameObject.Find("3DGUI/3DGUI Container/Help2/BTN_Use").GetComponent<GUI_Button>();
		aButton_Help2_Next = GameObject.Find("3DGUI/3DGUI Container/Help2/BTN_Next").GetComponent<GUI_Button>();
		aButton_Help2_Prev = GameObject.Find("3DGUI/3DGUI Container/Help2/BTN_Prev").GetComponent<GUI_Button>();
		aButton_Help3_Back = GameObject.Find("3DGUI/3DGUI Container/Help3/BTN_Back").GetComponent<GUI_Button>();
		aButton_Help3_Use = GameObject.Find("3DGUI/3DGUI Container/Help3/BTN_Use").GetComponent<GUI_Button>();
		aButton_Help3_Prev = GameObject.Find("3DGUI/3DGUI Container/Help3/BTN_Prev").GetComponent<GUI_Button>();
		aButton_Credits_Back = GameObject.Find("3DGUI/3DGUI Container/Credits/BTN_Back").GetComponent<GUI_Button>();
		
		aButton_MainMenu_Continue.FireButtonEvent += ResumeGame;
		aButton_MainMenu_LoadLevel1.FireButtonEventWithString += LoadLevel;
		aButton_MainMenu_LoadLevel2.FireButtonEventWithString += LoadLevel;
		aButton_MainMenu_LoadLevel3.FireButtonEventWithString += LoadLevel;
		aButton_MainMenu_LoadLevel4.FireButtonEventWithString += LoadLevel;
		aButton_MainMenu_LoadLevel5.FireButtonEventWithString += LoadLevel;
		aButton_MainMenu_LoadLevel6.FireButtonEventWithString += LoadLevel;
		aButton_MainMenu_Credits.FireButtonEvent += GoToCredits;
		aButton_MainMenu_Quit.FireButtonEvent += QuitApplication;
		aButton_MainMenu_Help.FireButtonEvent += GoToHelp;
		aButton_Help_Back.FireButtonEvent += GoToMainMenu;
		aButton_Help_Next.FireButtonEvent += GoToHelp2;
		aButton_Help2_Back.FireButtonEvent += GoToMainMenu;
		aButton_Help2_Use.FireButtonEventWithString += ChangeControlsMode;
		aButton_Help2_Next.FireButtonEvent += GoToHelp3;
		aButton_Help2_Prev.FireButtonEvent += GoToHelp;
		aButton_Help3_Back.FireButtonEvent += GoToMainMenu;
		aButton_Help3_Use.FireButtonEventWithString += ChangeControlsMode;
		aButton_Help3_Prev.FireButtonEvent += GoToHelp2;

		aButton_Credits_Back.FireButtonEvent += GoToMainMenu;

		m_layerContainer.DeactivateAllLayers();
	}

	public void Update()
	{
		CheckInputs();
	}

	public void CloseAllMenus()
	{
		m_gameManager.m_gameIsPaused = false;

		if(!m_hasOpenLayers) return;

		m_layerContainer.CloseAllLayers();
		m_hasOpenLayers = false;
	}

	// try to open a specific GUILayer
	public void OpenMenu(string layerName)
	{
		m_gameManager.m_gameIsPaused = true;
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
	
	public void ResumeGame(GameObject theSender)
	{
		CloseAllMenus();
	}
	
	public void GoToCredits(GameObject theSender)
	{
		CloseAllMenus();
		OpenMenu("Credits");
	}

	public void GoToHelp (GameObject theSender)
	{
		CloseAllMenus();
		OpenMenu("Help");
	}

	public void GoToHelp2 (GameObject theSender)
	{
		CloseAllMenus();
		OpenMenu("Help2");
	}

	public void GoToHelp3 (GameObject theSender)
	{
		CloseAllMenus();
		OpenMenu("Help3");
	}
	
	public void GoToMainMenu(GameObject theSender)
	{
		CloseAllMenus();
		OpenMenu(m_defaultLayer);
	}

	public void ChangeControlsMode (GameObject theSender, string theValue)
	{
		switch(theValue)
		{
			case "Mode1":
				m_gameManager.ChangePlayerControls(PlayerControls.EControlsMode.POINT_AND_CLICK);
				break;
			case "Mode2":
				m_gameManager.ChangePlayerControls(PlayerControls.EControlsMode.SWIPE);
				break;
		}
		ResumeGame(theSender);
	}
	
	public void LoadLevel(GameObject theSender, string levelName)
	{
		m_gameManager.LoadLevel(levelName);
	}
	
	public void QuitApplication(GameObject theSender)
	{
		m_gameManager.QuitApplication();
	}
}
