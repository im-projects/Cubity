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
	private bool m_deathAnimationRunning = false;
	private InputManager m_inputManager;

	void Start()
	{
		if(m_player == null) m_player = GameObject.FindGameObjectWithTag("Player");

		m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		m_inputManager = this.gameObject.GetComponent<InputManager>();

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
//		Debug.Log ("reset player");
		m_player.transform.position = m_currentSpawnpoint.position;
		m_player.transform.rotation = m_currentSpawnpoint.rotation;
		m_player.GetComponent<PlayerControls>().StopPlayerMovement();
		m_inputManager.releaseSelection();
	}

	public void SetCurrentSpawnpoint(Transform newSpawnPoint)
	{
		m_currentSpawnpoint = newSpawnPoint;
	}

	public IEnumerator BackgroundTransition()
	{
		m_deathAnimationRunning = true;
		while(m_camera.backgroundColor.r < 0.5f)
		{
			m_camera.backgroundColor += new Color(0.01f,0,0);
			yield return new WaitForEndOfFrame();
		}
		ResetPlayer();
		m_camera.backgroundColor = new Color(0,0,0);
		m_deathAnimationRunning = false;
	}

	public void ChangePlayerControls(PlayerControls.EControlsMode theMode)
	{
		m_player.GetComponent<PlayerControls>().m_controlsMode = theMode;
	}

	public bool GetDeathAnimationRunning()
	{
		return m_deathAnimationRunning;
	}
}
