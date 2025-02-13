﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

	[SerializeField]
	private NodeManager m_inventoryNodeManager;

	[SerializeField]
	private NodeManager m_inventoryNodeManager2;

	[SerializeField]
	private HandScript m_hand;

	private NodeManager m_currentNodeManager;
	private int m_currentOverIndex;

	private NodeManager m_prevNodemanager;

	public void Awake()
	{
		m_inventoryNodeManager.NodeOverEvent += EnterNode;
		m_inventoryNodeManager.NodeOutEvnet += OutNode;

		m_inventoryNodeManager2.NodeOverEvent += EnterNode;
		m_inventoryNodeManager2.NodeOutEvnet += OutNode;

	}

	private void EnterNode(Node i)
	{
		m_currentNodeManager = i.Manager;
		m_currentOverIndex = i.ID;
	}
	private void OutNode()
	{
		m_prevNodemanager = m_currentNodeManager;
		m_currentNodeManager = null;
		m_currentOverIndex = -1;
	}

	public void Update()
	{

		if (m_hand.GrabItemObject != null)
		{//쥐고있음
			m_hand.GrabItemObject.transform.position = Input.mousePosition;
		}

		if (m_currentNodeManager == null)
		{
			if (m_prevNodemanager != null)
			{
				m_prevNodemanager.ClearNodeColor();
				m_prevNodemanager = null;
			}
			return;
		}


		if (Input.GetMouseButtonDown(0))
		{
			Node m_currentNode = m_currentNodeManager.GetNodeByIndex(m_currentOverIndex);

			if (m_hand.GrabItemObject == null)
			{//손이 비어있는 상태
				if (m_currentOverIndex >= 0)
				{//템창 위다
					if (m_currentNode.ItemObject != null)
					{//템이 있다

						//템은 손으로 이동
						m_hand.GrabItemObject = m_currentNode.ItemObject;
						//인벤 노드 초기화
						m_currentNodeManager.InitialNode((m_currentNode));
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
				if (m_currentOverIndex < 0)
				{//인벤 바깥에 뒀을때 처리
					Debug.Log("null");
				}
				else
				{
					int[] items = m_currentNodeManager.CalItemPosition(m_currentOverIndex, m_hand.GrabItemObject.m_horizonCount, m_hand.GrabItemObject.m_verticalCount);

					int clashCount = m_currentNodeManager.CheckClashCount(items);
					if (clashCount == 0)
					{
						//m_hand.m_grapItemID = null;
						m_currentNodeManager.SetItem(items, m_hand.GrabItemObject);
						m_hand.GrabItemObject = null;
					}
					else if (clashCount == 1)
					{
						//한개겹침
						//잠깐 치워둠
						Item temp = m_hand.GrabItemObject;

						Node tempNode = m_currentNodeManager.CheckClash(items);
						//템을 주움
						m_hand.GrabItemObject = tempNode.ItemObject;
						m_currentNodeManager.InitialNode(tempNode);

						//치워뒀던거 내려둠
						m_currentNodeManager.SetItem(items, temp);
					}
					else
					{
						//여러개 겹침
					}

					for (int i = 0; i < m_currentNodeManager.m_nodeArray.Length; i++)
					{
						ChangeColor(m_currentNodeManager.GetNodeByIndex(i).Gameobject, Constant.defaultColor);
					}
				}
			}
		}


		//놓기전 색깔 검사
		if (m_hand.GrabItemObject != null)
		{//쥐고있음
			//m_hand.GrabItemObject.transform.position = Input.mousePosition;
			if (m_currentOverIndex >= 0)
			{
				int[] items = m_currentNodeManager.CalItemPosition(m_currentOverIndex, m_hand.GrabItemObject.m_horizonCount, m_hand.GrabItemObject.m_verticalCount);

				m_currentNodeManager.ClearNodeColor();

				if (m_currentNodeManager.CheckClashCount(items) == 1)
				{//템이 있는데 한개다

					for (int i = 0; i < items.Length; i++)
					{
						ChangeColor(m_currentNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.selectColor);
					}

					Node tempNode = m_currentNodeManager.CheckClash(items);
					ChangeColor(tempNode.ItemObject.gameObject, Constant.emphasisColor);
				}
				else if (m_currentNodeManager.CheckClashCount(items) >= 2)
				{
					for (int i = 0; i < items.Length; i++)
					{
						ChangeColor(m_currentNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.cantdropColor);
					}
				}
				else
				{//템이 없다
			
					for (int i = 0; i < items.Length; i++)
					{
						ChangeColor(m_currentNodeManager.GetNodeByIndex(items[i]).Gameobject, Constant.selectColor);
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
