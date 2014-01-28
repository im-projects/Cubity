using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public string m_nextLevel;
	public GameObject m_player;
	public Transform m_initSpawnpoint;
	
	[HideInInspector]
	public bool m_gameIsPaused = false;

	private Transform m_currentSpawnpoint;
	private Camera m_camera;

	void Start()
	{
		if(m_player == null) m_player = GameObject.FindGameObjectWithTag("Player");

		m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

		m_currentSpawnpoint = m_initSpawnpoint;
		ResetPlayer();
	}

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

	public void ResetPlayer ()
	{
		m_player.transform.position = m_currentSpawnpoint.position;
		m_player.transform.rotation = m_currentSpawnpoint.rotation;
	}

	public void SetCurrentSpawnpoint(Transform newSpawnPoint)
	{
		m_currentSpawnpoint = newSpawnPoint;
	}

	public IEnumerator BackgroundTransition()
	{
		while(m_camera.backgroundColor.r < 0.4f)
		{
			Debug.Log (m_camera.backgroundColor.r);
			m_camera.backgroundColor += new Color(0.015f,0,0);
			Debug.Log (m_camera.backgroundColor.r);
			yield return new WaitForEndOfFrame();
		}
		ResetPlayer();
		m_camera.backgroundColor = new Color(0,0,0);
	}
}
