﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class NodeManager : MonoBehaviour {

	public delegate void NodeOverDelegate(Node currentNode);
	public event NodeOverDelegate NodeOverEvent;

	public delegate void NodeOutDelegate();
	public event NodeOutDelegate NodeOutEvnet;


	public int m_totalIndex = 30;

	//node
	public GameObject m_nodeParents;
	public GameObject m_node;
	public int m_horizontalCount = 6;
	private int m_vertialCount;
	public IntVector2 m_nodeSize = new IntVector2(48,48);

	public Node[] m_nodeArray;

	public int m_currentOverIndex = -1;

	public Item TestItem;
	public Item TestItem2;
	public Item TestItem3;

	private void Awake()
	{
		CreateSlot();
	}

	private void CreateSlot()
	{
		m_nodeParents.GetComponent<GridLayoutGroup>().constraintCount = m_horizontalCount;

		m_nodeArray = new Node[m_totalIndex];
		m_vertialCount = m_totalIndex / m_horizontalCount;

		for (int i = 0; i < m_totalIndex; i++)
		{
			GameObject obj = (GameObject)Instantiate(m_node);
			obj.name = "[" + i % m_horizontalCount + ", " + i / m_horizontalCount + "]\n" + i;
			obj.GetComponentInChildren<Text>().text = obj.name;
			obj.transform.SetParent(m_nodeParents.transform);

			m_nodeArray[i] = new Node
			{
				Gameobject = obj,
				ID = i,
				Manager = this,
			};

			int temp = i;
			EventTrigger.Entry enter = new EventTrigger.Entry();
			enter.eventID = EventTriggerType.PointerEnter;
			enter.callback.AddListener((eventData) => SetOverNode(m_nodeArray[temp]));
			obj.GetComponent<EventTrigger>().triggers.Add(enter);

			EventTrigger.Entry exit = new EventTrigger.Entry();
			exit.eventID = EventTriggerType.PointerExit;
			exit.callback.AddListener((eventData) => ResetOverIndex());
			obj.GetComponent<EventTrigger>().triggers.Add(exit);

		}
	}
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			int[] tests = new int[] { 0, 1, 2, 6, 7, 8, 12, 13, 14 };
			TestItem.m_horizonCount = 3;
			TestItem.m_verticalCount = 3;
			SetItem(tests, TestItem);

			int[] tests2 = new int[] { 21, 22, 27, 28 };
			TestItem2.m_horizonCount = 2;
			TestItem2.m_verticalCount = 2;
			SetItem(tests2, TestItem2);

			int[] tests3 = new int[] { 4, 5, 10, 11 };
			TestItem3.m_horizonCount = 2;
			TestItem3.m_verticalCount = 2;
			SetItem(tests3, TestItem3);
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			string str = "";
			for (int i = 0; i < m_nodeArray.Length; i++)
			{
				if (m_nodeArray[i].ItemObject != null)
				{
					str += i;
					str += "/";
				}
			}
			Debug.Log(str);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			string str = "";
			List<Node> nodes = GetNodeByIndex(m_currentOverIndex).linkedNode;
			for (int i = 0; i < nodes.Count; i++)
			{

				str += nodes[i].ID;
				str += "/";
			}
			Debug.Log(str);
		}
	}

	public void SetOverNode(Node node)
	{
		NodeOverEvent.Invoke(node);
	}

	private void ResetOverIndex()
	{
		NodeOutEvnet.Invoke();
	}

	public void InitialNode(Node node)
	{
		List<Node> nodes = GetNodeByIndex(node.ID).linkedNode;

		for (int i = 0; i < nodes.Count; i++)
		{
			nodes[i].ItemObject = null;
			nodes[i].CenterPositon = Vector3.one;
			nodes[i].linkedNode = null;
		}
	}

	public Node GetNodeByIndex(int index)
	{
		if (m_nodeArray.Length <= 0)
			return null;

		return m_nodeArray[index];
	}

	public int[] CalItemPosition(int currentPos, int ItemHorizontalCount, int ItemVerticalCount)
	{
		if (currentPos < 0) return null;

		int calHorizonRight = ItemHorizontalCount / 2;
		int calVerticalUp = ItemVerticalCount / 2;
		int calHorizonLeft = calHorizonRight + (ItemHorizontalCount % 2) -1;
		int calVerticalDown = calVerticalUp + (ItemVerticalCount % 2) -1;
		int calCenter = currentPos;

		if (currentPos + calHorizonRight >= currentPos - (currentPos % m_horizontalCount) + m_horizontalCount)
		{
			calHorizonRight -= 1;
			calHorizonLeft += 1;
			calCenter -= 1;
		}
		if (currentPos - calHorizonLeft < (currentPos / m_horizontalCount) * m_horizontalCount)
		{
			calHorizonRight += 1;
			calHorizonLeft -= 1;
			calCenter += 1;
		}
		if (currentPos + (calVerticalUp * m_horizontalCount) >= m_totalIndex)
		{
			calVerticalUp -= 1;
			calVerticalDown += 1;
			calCenter -= m_horizontalCount;
		}
		if (currentPos - (calVerticalDown * m_horizontalCount) < 0)
		{
			calVerticalUp += 1;
			calVerticalDown -= 1;
			calCenter += m_horizontalCount;
		}

		if (currentPos + calHorizonRight >= currentPos - (currentPos % m_horizontalCount) + m_horizontalCount
			|| currentPos - calHorizonLeft < (currentPos / m_horizontalCount) * m_horizontalCount
			|| currentPos + (calVerticalUp * m_horizontalCount) >= m_totalIndex
			|| currentPos - (calVerticalDown * m_horizontalCount) < 0
			|| calCenter < 0
			|| calCenter > m_totalIndex)
		{
			Debug.Log("칸 부족");
			return null;
		}

		int leftDown = currentPos;
		leftDown -= calHorizonLeft;
		leftDown -= calVerticalDown * m_horizontalCount;

		int leftUp = currentPos;
		leftUp -= calHorizonLeft;
		leftUp += calVerticalUp * m_horizontalCount;

		int rightDown = currentPos;
		rightDown += calHorizonRight;
		rightDown -= calVerticalDown * m_horizontalCount;

		int rightUp = currentPos;
		rightUp += calHorizonRight;
		rightUp += calVerticalUp * m_horizontalCount;

		//Debug.Log("[" + leftUp + "] [" + rightUp + "]\n[" + leftDown + "] [" + rightDown + "]\nCenter : " + calCenter);

		int[] ints = new int[ItemVerticalCount * ItemHorizontalCount];
		////전체 index를 계산
		for (int k = 0; k < ItemVerticalCount; k++)
		{
			for (int i = 0; i < ItemHorizontalCount; i++)
			{
				int num = (m_horizontalCount * k) + (leftDown + i);
				ints[k * ItemHorizontalCount + i] = num;
			}
		}

		return ints;
	}

	public void SetItem(int[] ints, Item item)
	{
		for (int i = 0; i < ints.Length; i++)
		{
			GetNodeByIndex(ints[i]).ItemObject = item;

			int last = ints.Length - 1;

			float xDel = GetNodeByIndex(ints[last]).Gameobject.transform.localPosition.x - GetNodeByIndex(ints[0]).Gameobject.transform.localPosition.x;
			float yDel = GetNodeByIndex(ints[last]).Gameobject.transform.localPosition.y - GetNodeByIndex(ints[0]).Gameobject.transform.localPosition.y;

			xDel *= 0.5f;
			yDel *= 0.5f;

			//GetNodeByIndex(ints[i]).CenterNodeID = intfs[0];
			GetNodeByIndex(ints[i]).CenterPositon = new Vector3(GetNodeByIndex(ints[0]).Gameobject.transform.localPosition.x + xDel, GetNodeByIndex(ints[0]).Gameobject.transform.localPosition.y + yDel, 0);
			GetNodeByIndex(ints[i]).linkedNode = new List<Node>();

			for (int k = 0; k < ints.Length; k++)
			{
				GetNodeByIndex(ints[i]).linkedNode.Add(GetNodeByIndex(ints[k]));
			}
		}

		//아이템 오브젝트 위치이동 
		item.gameObject.transform.localPosition = GetNodeByIndex(ints[0]).CenterPositon + m_nodeParents.transform.localPosition;
	}

	public int CheckClashCount(int[] ints)
	{
		int clashCount = 0;
		Item objTemp = null;

		for (int i = 0; i < ints.Length; i++)
		{
			if (GetNodeByIndex(ints[i]).ItemObject != null)
			{
				if (objTemp != GetNodeByIndex(ints[i]).ItemObject)
				{
					clashCount++;
				}
				objTemp = GetNodeByIndex(ints[i]).ItemObject;
			}
		}

		return clashCount;
	}

	public Node CheckClash(int[] ints)
	{
		Item objTemp = null;
		for (int i = 0; i < ints.Length; i++)
		{
			if (GetNodeByIndex(ints[i]).ItemObject != null)
			{
				if (objTemp != GetNodeByIndex(ints[i]).ItemObject)
				{
					return GetNodeByIndex(ints[i]);
				}
				objTemp = GetNodeByIndex(ints[i]).ItemObject;
			}
		}

		return null;
	}
}
