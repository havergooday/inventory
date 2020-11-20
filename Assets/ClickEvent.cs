using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour, IPointerClickHandler
{
	public ClickManager m_manager;

	private void Awake()
	{
		m_manager = this.transform.parent.GetComponent<ClickManager>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (m_manager != null)
		{
			m_manager.m_clickObject = this.gameObject;
		}
	}

}
