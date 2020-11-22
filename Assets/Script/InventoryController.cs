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
		if (Input.GetMouseButtonDown(0))
		{
			if (m_hand.GrabItemObject == null)
			{//손이 비어있는 상태
				if (m_inventoryNodeManager.m_currentOverIndex >= 0)
				{//템창 위다
					if (m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex).ItemObject != null)
					{//템이 있다
					 //템 줍기
						//템은 손으로 이동
						m_hand.GrabItemObject = m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex).ItemObject;
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
					int[] items = m_inventoryNodeManager.CalItemPosition(m_hand.GrabItemObject.m_horizonCount, m_hand.GrabItemObject.m_verticalCount);

					int clashCount = m_inventoryNodeManager.CheckClashCount(items);
					if (clashCount == 0)
					{
						//m_hand.m_grapItemID = null;
						m_inventoryNodeManager.SetItem(items, m_hand.GrabItemObject);
						m_hand.GrabItemObject = null;
					}
					else if (clashCount == 1)
					{
						//한개겹침
						//잠깐 치워둠
						Item temp = m_hand.GrabItemObject;
						Node grap = m_inventoryNodeManager.GetNodeByIndex(m_inventoryNodeManager.m_currentOverIndex);


						Node tempNode = m_inventoryNodeManager.CheckClash(items);
						//템을 주움
						m_hand.GrabItemObject = tempNode.ItemObject;
						m_inventoryNodeManager.InitialNode(tempNode);

						//치워뒀던거 내려둠
						m_inventoryNodeManager.SetItem(items, temp);
					}
					else
					{
						//여러개 겹침
					}
				}
			}
		}


		if (m_hand.GrabItemObject != null)
		{//쥐고있음
			m_hand.GrabItemObject.transform.position = Input.mousePosition;
			if (m_inventoryNodeManager.m_currentOverIndex >= 0)
			{
				int[] items = m_inventoryNodeManager.CalItemPosition(m_hand.GrabItemObject.m_horizonCount, m_hand.GrabItemObject.m_verticalCount);


				for (int i = 0; i < m_inventoryNodeManager.m_nodeArray.Length; i++)
				{
					if (m_inventoryNodeManager.GetNodeByIndex(i).ItemObject != null)
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(i).ItemObject.gameObject, Constant.defaultColor);
				}

				if (m_inventoryNodeManager.CheckClashCount(items) == 1)
				{//템이 있는데 한개다

					for (int i = 0; i < m_inventoryNodeManager.m_nodeArray.Length; i++)
					{
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(i).Gameobject, Constant.defaultColor);
					}
					for (int i = 0; i < items.Length; i++)
					{
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.selectColor);
					}

					Node tempNode = m_inventoryNodeManager.CheckClash(items);
					ChangeColor(tempNode.ItemObject.gameObject, Constant.emphasisColor);
				}
				else if (m_inventoryNodeManager.CheckClashCount(items) >= 2)
				{
					/*//템이 있는데 두개다
					for (int i = 0; i < items.Length; i++)
					{
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.cantdropColor);
					}*/
					for (int i = 0; i < m_inventoryNodeManager.m_nodeArray.Length; i++)
					{
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(i).Gameobject, Constant.defaultColor);
					}
					for (int i = 0; i < items.Length; i++)
					{
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.cantdropColor);
					}
				}
				else
				{//템이 없다
				 /*for (int i = 0; i < items.Length; i++)
				 {
					 ChangeColor(m_inventoryNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.selectColor);
				 }*/
					for (int i = 0; i < m_inventoryNodeManager.m_nodeArray.Length; i++)
					{
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(i).Gameobject, Constant.defaultColor);
					}
					for (int i = 0; i < items.Length; i++)
					{
						ChangeColor(m_inventoryNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.selectColor);
					}
				}
			}
			else
			{//인벤 바깥
			}
		}




	}
	private void ChangeColor(GameObject obj, Color color)
	{
		obj.GetComponent<Image>().color = color;
	}

}
