using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	Vector3 startPosition;
	private Transform m_startParent;

	public void OnBeginDrag(PointerEventData eventData)
	{
		//transform.position = eventData.position;
		//startPosition = transform.position;
		m_startParent = this.transform.parent;
		this.GetComponent<CanvasGroup>().blocksRaycasts = false;
		this.transform.SetParent(this.transform.parent.parent);
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;//Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.GetComponent<CanvasGroup>().blocksRaycasts = true;
		this.transform.SetParent(m_startParent);
		//transform.position = startPosition;
	}

}
