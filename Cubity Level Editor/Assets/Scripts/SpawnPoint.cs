using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	public bool m_isCheckpoint = true;

	private GameManager m_gameManager;
	
	// Use this for initialization
	void Start () {
		m_gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		if(m_gameManager == null) m_gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}
	
	void OnTriggerEnter(Collider theCollider)
	{
		if(m_isCheckpoint && theCollider.gameObject.tag.Equals("Player"))
		{
			m_gameManager.SetCurrentSpawnpoint(this.transform);
		}
	}
}
