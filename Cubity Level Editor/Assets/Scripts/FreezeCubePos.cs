using UnityEngine;
using System.Collections;

public class FreezeCubePos : MonoBehaviour {

	private Vector3 m_initPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
		m_initPosition = this.transform.position;
	}
	
	void FixedUpdate()
	{
		this.transform.position = m_initPosition;
	}
}
