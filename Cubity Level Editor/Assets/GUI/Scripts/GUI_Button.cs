using UnityEngine;
using System.Collections;

public class GUI_Button : MonoBehaviour {

	public EButtonEventType m_eventType = EButtonEventType.NO_VALUE;
	public string m_eventString = "";
	public Color m_colorDefault = Color.white;
	public Color m_colorHover = Color.gray;
	public Color m_colorActive = Color.black;
	public float m_hoverTime = 10;

	public delegate void ButtonEventHandler(GameObject theSender);
	public event ButtonEventHandler FireButtonEvent;
	public delegate void ButtonEventHandlerWithValue(GameObject theSender, string theValue);
	public event ButtonEventHandlerWithValue FireButtonEventWithString;
	
	private bool m_active;
	private bool m_hover;


	public enum EButtonEventType
	{
		NO_VALUE,
		EVENT_WITH_STRING
	}

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

		switch(m_eventType)
		{
		case EButtonEventType.NO_VALUE:
		{
			if (FireButtonEvent != null)
			{
				FireButtonEvent(this.gameObject);
			}
			break;
		}
		case EButtonEventType.EVENT_WITH_STRING:
		{
			if (FireButtonEventWithString != null)
			{
				FireButtonEventWithString(this.gameObject, m_eventString);
			}
			break;
		}
		default:
		{
			if (FireButtonEvent != null)
			{
				FireButtonEvent(this.gameObject);
			}
			break;
		}
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
