using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {

	private GameManager m_gameManager;

	void Start()
	{
		m_gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>() as GameManager;
		if(m_gameManager == null) Debug.LogWarning("GameManager not found");
	}

	void OnTriggerEnter(Collider theCollider)
	{
		if(theCollider.gameObject.tag.Equals("Player"))
		{
			m_gameManager.LoadNextLevel();
		}
	}
}
