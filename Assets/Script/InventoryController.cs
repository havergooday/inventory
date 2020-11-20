using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

	[SerializeField]
	private NodeManager m_inventoryNodeManager;

	[SerializeField]
	private HandScript m_hand;

	public void Update()
	{
		//test
		m_hand.m_grapItemID = "1";


		if (string.IsNullOrEmpty(m_hand.m_grapItemID) == false)
		{

		}

		/*
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A))
		{
			int[] items = m_inventoryNodeManager.CalItemPosition(3, 3);
			if (items != null)
			{
				for (int k = 0; k < items.Length; k++)
				{
					Debug.Log(items[k]);
					ChangeColor(m_inventoryNodeManager.GetDataByIndex(items[k]).Gameobject, new Color(0.75f, 1, 0.75f, 0.392f));
				}
			}
		}
		*/

		if (Input.GetMouseButtonDown(0))
		{
			if (m_hand.GrapItemObject == null)
			{//손이 비어있는 상태
				if (m_inventoryNodeManager.m_currentOverIndex >= 0)
				{//템창 위다
					Debug.Log(m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex).ItemObject);
					if (m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex).ItemObject != null)
					{//템이 있다
					 //템 줍기
						//템은 손으로 이동
						//m_hand.m_grapItemID = m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex).StorageID;;
						m_hand.GrapItemObject = m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex).ItemObject;

						//인벤 노드 초기화
						m_inventoryNodeManager.InitialNode((m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex)));
					}
					else
					{//탬이 없다
						//아무일도 없읍니다
					}
				}
				else
				{//템창 바깥 클릭
				}
			}
			else
			{
				//아이템 내려두기
				if (m_inventoryNodeManager.m_currentOverIndex < 0)
				{//인벤 바깥에 뒀을때 처리
					Debug.Log("null");
				}
				else
				{
					int[] items = m_inventoryNodeManager.CalItemPosition(3, 3);
					m_inventoryNodeManager.SetItem(items, m_hand.GrapItemObject);

					m_hand.m_grapItemID = null;
					m_hand.GrapItemObject = null;
				}
			}
		}

		if (m_hand.GrapItemObject != null)
		{
			m_hand.GrapItemObject.transform.position = Input.mousePosition;
		}
	}
	private void ChangeColor(GameObject obj, Color color)
	{
		obj.GetComponent<Image>().color = color;
	}

}
