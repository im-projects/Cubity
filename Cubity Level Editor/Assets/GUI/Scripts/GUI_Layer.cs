using UnityEngine;
using System.Collections;

public class GUI_Layer : MonoBehaviour {

	public bool m_animate = false;
	public float m_animationSpeed = 20f;

	private bool m_isOpen = false;
	private bool m_animationRunning = false;

	private float m_animationDistanceMax = 8f;
	private float m_animationDistanceMin = 0.2f; // this is a hack. it should stop at 0


	// show the Layer
	public void Open ()
	{
		if(m_isOpen || m_animationRunning) return;

		m_isOpen = true;
		this.gameObject.SetActive(true);

		if(m_animate)
			StartCoroutine("PlayOpenAnimation");
		else
			OpenInstantly();
	}
	
	// hide the Layer
	public void Close ()
	{
		if(!m_isOpen || m_animationRunning) return;

		m_isOpen = false;

		if(m_animate)
			StartCoroutine("PlayCloseAnimation");
		else
			CloseInstantly();
	}

	void OpenInstantly ()
	{
		this.transform.localPosition -= new Vector3(0, m_animationDistanceMax, 0);
		m_animationRunning = false;
	}

	void CloseInstantly ()
	{
		this.transform.localPosition += new Vector3(0, m_animationDistanceMax, 0);
		m_animationRunning = false;
	}

	IEnumerator PlayOpenAnimation() {
		m_animationRunning = true;
		while(this.transform.localPosition.y > m_animationDistanceMin)
		{
			this.transform.localPosition += new Vector3(0, m_animationSpeed*-1, 0) * Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		m_animationRunning = false;
	}
	
	IEnumerator PlayCloseAnimation() {
		m_animationRunning = true;
		while(this.transform.localPosition.y < m_animationDistanceMax)
		{
			this.transform.localPosition += new Vector3(0, m_animationSpeed, 0) * Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		this.gameObject.SetActive(false);
		m_animationRunning = false;
	}

	// Getter
	public bool GetAnimationIsRunning() {
		return m_animationRunning;
	}
}
