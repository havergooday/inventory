using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler  {

	private bool m_isSelect = false;
	private Transform m_startParent;

	private void Awake()
	{
		
	}

	private void OnClick(GameObject obj)
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{

		if (m_isSelect)
			ResetSelectedItem();
		else
			SetSelectedItem(this.gameObject);
	}

	public void Update()
	{
		if (m_isSelect)
		{
			this.transform.position = Input.mousePosition;

			if (Input.GetMouseButtonDown(0))
			{
				ResetSelectedItem();
			}
		}
	}


	private void SetSelectedItem(GameObject obj)
	{
		m_isSelect = true;

		m_startParent = this.transform.parent;
		this.transform.SetParent(this.transform.parent.parent);
		this.GetComponent<CanvasGroup>().blocksRaycasts = false;

	}

	private void ResetSelectedItem()
	{
		m_isSelect = false;

		this.transform.SetParent(m_startParent);
		this.GetComponent<CanvasGroup>().blocksRaycasts = true;

	}
}
