using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour {

	public delegate void OnClickEvent(GameObject obj);
	public event OnClickEvent OnClick;

	public GameObject m_clickObject;

	public Transform m_inventoryParent;
	public Transform m_clickParent;

	public InventoryManager m_inventory;

	void Update () {

		if (m_clickObject != null)
		{
			m_clickObject.transform.position = Input.mousePosition;
			m_clickObject.transform.SetParent(m_clickParent);
			m_clickObject.GetComponent<Image>().raycastTarget = false;

			if (Input.GetMouseButtonDown(0))
			{
				Vector3 pos = new Vector3();
				//Debug.Log(m_inventory.GetItemPosition().x);
				//Debug.Log(m_inventory.GetItemPosition().y);
				//if ((m_inventory.GetItemPosition().x == 0
				//	&& m_inventory.GetItemPosition().y == 0)
				//	|| true)
				//{
				//	m_clickObject.transform.SetParent(this.transform);
				//}
				//else
				//{
				//	pos = new Vector3(m_inventory.GetItemPosition().x, m_inventory.GetItemPosition().y, 0);
				//	m_clickObject.transform.SetParent(m_inventoryParent);
				//}

				m_clickObject.GetComponent<RectTransform>().position = pos;
				m_clickObject.GetComponent<Image>().raycastTarget = true;
				m_clickObject = null;
				m_inventory.InitialItemPosition();
			}
		}
	}

	public void OnItemClicked()
	{
		if(OnClick != null)
			OnClick(m_clickObject);
	}

	//private void ReleaseGrap()
	//{
	//	if (m_clickObject == null)
	//		return;
	//
	//	m_clickObject.transform.SetParent(this.transform);
	//	m_clickObject.GetComponent<Image>().raycastTarget = true;
	//	m_clickObject = null;
	//}



}
