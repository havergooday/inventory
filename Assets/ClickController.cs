using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour {

	private bool m_isGrap;
	private GameObject m_obj;


	public RectTransform m_itemGrid;
	public InventoryManager m_inventory;
	public GameObject m_itemPrefab;
	public GameObject m_gridPrefab;

	private FloatVector2 m_unitSlot;

	private GameObject m_movingObject;
	private StoredItem m_movingItem;


	void Update () {

		
	}

	private void CalcSlotDimentions()
	{
		float girdWitdth = m_itemGrid.rect.width;
		float girdHeight = m_itemGrid.rect.height;

		m_unitSlot = new FloatVector2(girdHeight / m_inventory.m_size.x, girdWitdth / m_inventory.m_size.y);
	}

	private void MoveObject(GameObject gridObj, StoredItem item)
	{
		m_movingObject = gridObj;
		m_movingItem = item;

		StartCoroutine(ItemMouseFollow());
	}

	private IEnumerator ItemMouseFollow()
	{
		while (!Input.GetMouseButtonDown(0))
		{
			Vector2 mousePosition = Input.mousePosition;
			Vector2 relativePosition = new Vector2
											(mousePosition.x - m_itemGrid.sizeDelta.x,
											m_itemGrid.sizeDelta.y + mousePosition.y - m_itemGrid.sizeDelta.y);

			m_movingObject.transform.position = new Vector3(relativePosition.x, relativePosition.y, m_movingObject.transform.position.z);


			yield return null;
		}

		//RepositionMovingObject();
	}

	private void RepositionMovingObject()
	{
		if (m_movingObject != null)
		{
			int row = (int)(m_movingObject.transform.localPosition.x / m_unitSlot.x) * -1;
			int col = (int)(m_movingObject.transform.localPosition.y / m_unitSlot.y) * 1;

		}
	}

}
