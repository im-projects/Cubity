using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	private GameManager m_gameManager;
	
	// Use this for initialization
	void Start () {
		m_gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		if(m_gameManager == null) m_gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}
	
	void OnTriggerEnter(Collider theCollider)
	{
		if(theCollider.gameObject.tag.Equals("Player"))
		{
			m_gameManager.StartCoroutine("BackgroundTransition");
		}
	}
}
