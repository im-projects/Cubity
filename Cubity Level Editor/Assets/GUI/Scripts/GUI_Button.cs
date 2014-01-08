using UnityEngine;
using System.Collections;

public class GUI_Button : MonoBehaviour {

	public Color m_colorDefault = Color.white;
	public Color m_colorHover = Color.gray;
	public Color m_colorActive = Color.black;
	public float m_hoverTime = 10;

	public delegate void ButtonEventHandler(GameObject theSender);
	public event ButtonEventHandler FireButtonEvent;

	private bool m_active;
	private bool m_hover;


	private void Start()
	{
		this.renderer.material.color = m_colorDefault;
	}

	private void Update()
	{
		if(!m_active && !m_hover)
		{
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, m_colorDefault, Time.deltaTime * m_hoverTime);
		}
		else if(m_hover && !m_active)
		{
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, m_colorHover, Time.deltaTime * m_hoverTime);
		}
		else if(m_active)
		{
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, m_colorActive, Time.deltaTime * m_hoverTime);
		}


	}

	private void OnMouseDown()
	{
		m_active = true;
	}

	private void OnMouseUp()
	{
		m_active = false;

		if (FireButtonEvent != null)
		{
			FireButtonEvent(this.gameObject);
		}
	}

	private void OnMouseEnter()
	{
		m_hover = true;
	}

	
	private void OnMouseExit()
	{
		m_hover = false;
	}
}
