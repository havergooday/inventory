using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerClickHandler
{

	public string m_itemName;
	public string m_itemDescription;
	public Sprite m_itemIcon;
	public Sprite m_itemImage;
	public IntVector2 m_itemSize;

	private InventoryScript m_clickScript;

	private void Awake()
	{
		if (this.transform.parent.parent.gameObject.GetComponent<InventoryScript>() != null)
		{
			m_clickScript = this.transform.parent.parent.gameObject.GetComponent<InventoryScript>();
		}
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		if (m_clickScript != null)
		{
			m_clickScript.m_clickObject = this.gameObject;
		}
	}
}
