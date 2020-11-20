using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickScript : MonoBehaviour
{
	public Transform m_itemsParent;
	public Transform m_inventoryParent;
	public Transform m_grapParent;

	public InventoryScript m_inventory;

	public GameObject m_clickObject;

	void Update()
	{
		if (m_clickObject != null)
		{
			m_clickObject.transform.position = Input.mousePosition;
			m_clickObject.transform.SetParent(m_grapParent);
			m_clickObject.GetComponent<Image>().raycastTarget = false;

			if (Input.GetMouseButtonDown(0))
			{
				Vector3 pos = new Vector3();

				if (m_inventory.GetItemPosition().x == 0
					&& m_inventory.GetItemPosition().y == 0)
				{
					//되돌리기
					m_clickObject.transform.SetParent(m_itemsParent.transform);
				}
				else
				{
					//해당 위치로 이동
					pos = new Vector3(m_inventory.GetItemPosition().x, m_inventory.GetItemPosition().y, 0);
					m_clickObject.transform.SetParent(m_inventoryParent);
				}

				m_clickObject.GetComponent<RectTransform>().position = pos;
				m_clickObject.GetComponent<Image>().raycastTarget = true;

				m_clickObject = null;
				m_inventory.InitialItemPosition();
			}
		}
	}

}
