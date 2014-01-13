using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public string m_nextLevel;
	[HideInInspector]
	public bool m_gameIsPaused = false;

	public void LoadNextLevel()
	{
		Application.LoadLevel (m_nextLevel);
	}
	
	public void LoadLevel(string levelName)
	{
		Application.LoadLevel (levelName);
	}

	public void QuitApplication()
	{
		Application.Quit();
	}
}
