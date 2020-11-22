using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetItem_4 : MonoBehaviour {

	public int m_totalIndex = 30;

	//node
	public GameObject m_nodeParents;
	public GameObject m_node;
	public int m_horizontalCount = 6;
	private int m_vertialCount;
	public IntVector2 m_nodeSize = new IntVector2(48, 48);

	private Node[] m_nodeArray;

	public int m_currentOverIndex = -1;

	public GameObject TestItem;

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

			int temp = i;
			EventTrigger.Entry enter = new EventTrigger.Entry();
			enter.eventID = EventTriggerType.PointerEnter;
			enter.callback.AddListener((eventData) => SetOverIndex(temp));
			obj.GetComponent<EventTrigger>().triggers.Add(enter);

			EventTrigger.Entry exit = new EventTrigger.Entry();
			exit.eventID = EventTriggerType.PointerExit;
			exit.callback.AddListener((eventData) => ResetOverIndex());
			obj.GetComponent<EventTrigger>().triggers.Add(exit);

			m_nodeArray[i] = new Node
			{
				ID = i,
				Gameobject = obj
			};
		}
	}
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log(m_nodeArray[7].Gameobject.transform.localPosition);

			int[] tests = new int[] { 7, 0, 1, 2, 6, 7, 8, 12, 13, 14 };
			SetItem(tests, TestItem);
		}
	}

	public void SetOverIndex(int index)
	{
		m_currentOverIndex = index;
	}

	private void ResetOverIndex()
	{
		m_currentOverIndex = -1;
	}

	private void ResetColor()
	{
		for (int i = 0; i < m_nodeArray.Length; i++)
		{
			Utility.ChangeColor(m_nodeArray[i].Gameobject, Constant.defaultColor);
		}
	}

	public void InitialNode(Node node)
	{
		List<Node> nodes = GetNodeByIndex(node.ID).linkedNode;
		for (int i = 0; i < nodes.Count; i++)
		{
			nodes[i].StorageID = null;
			nodes[i].ItemObject = null;
			//nodes[i].CenterNodeID = -1;
			nodes[i].linkedNode = null;
		}
	}
	public Node GetNodeByIndex(int index)
	{
		if (m_nodeArray.Length <= 0)
			return null;

		return m_nodeArray[index];
	}

	public int[] CalItemPosition(int ItemHorizontalCount, int ItemVerticalCount)
	{
		if (m_currentOverIndex < 0) return null;

		int calHorizonRight = ItemHorizontalCount / 2;
		int calVerticalUp = ItemVerticalCount / 2;
		int calHorizonLeft = calHorizonRight + (ItemHorizontalCount % 2) - 1;
		int calVerticalDown = calVerticalUp + (ItemVerticalCount % 2) - 1;
		int calCenter = m_currentOverIndex;

		if (m_currentOverIndex + calHorizonRight >= m_currentOverIndex - (m_currentOverIndex % m_horizontalCount) + m_horizontalCount)
		{
			calHorizonRight -= 1;
			calHorizonLeft += 1;
			calCenter -= 1;
		}
		if (m_currentOverIndex - calHorizonLeft < (m_currentOverIndex / m_horizontalCount) * m_horizontalCount)
		{
			calHorizonRight += 1;
			calHorizonLeft -= 1;
			calCenter += 1;
		}
		if (m_currentOverIndex + (calVerticalUp * m_horizontalCount) >= m_totalIndex)
		{
			calVerticalUp -= 1;
			calVerticalDown += 1;
			calCenter -= m_horizontalCount;
		}
		if (m_currentOverIndex - (calVerticalDown * m_horizontalCount) < 0)
		{
			calVerticalUp += 1;
			calVerticalDown -= 1;
			calCenter += m_horizontalCount;
		}

		if (m_currentOverIndex + calHorizonRight >= m_currentOverIndex - (m_currentOverIndex % m_horizontalCount) + m_horizontalCount
			|| m_currentOverIndex - calHorizonLeft < (m_currentOverIndex / m_horizontalCount) * m_horizontalCount
			|| m_currentOverIndex + (calVerticalUp * m_horizontalCount) >= m_totalIndex
			|| m_currentOverIndex - (calVerticalDown * m_horizontalCount) < 0
			|| calCenter < 0
			|| calCenter > m_totalIndex)
		{
			Debug.Log("칸 부족");
			return null;
		}

		int leftDown = m_currentOverIndex;
		leftDown -= calHorizonLeft;
		leftDown -= calVerticalDown * m_horizontalCount;

		int leftUp = m_currentOverIndex;
		leftUp -= calHorizonLeft;
		leftUp += calVerticalUp * m_horizontalCount;

		int rightDown = m_currentOverIndex;
		rightDown += calHorizonRight;
		rightDown -= calVerticalDown * m_horizontalCount;

		int rightUp = m_currentOverIndex;
		rightUp += calHorizonRight;
		rightUp += calVerticalUp * m_horizontalCount;

		Debug.Log("[" + leftUp + "] [" + rightUp + "]\n[" + leftDown + "] [" + rightDown + "]\nCenter : " + calCenter);

		int[] ints = new int[ItemVerticalCount * ItemHorizontalCount + 1];
		////전체 index를 계산
		for (int k = 0; k < ItemVerticalCount; k++)
		{
			for (int i = 0; i < ItemHorizontalCount; i++)
			{
				int num = (m_horizontalCount * k) + (leftDown + i);
				ints[k * ItemHorizontalCount + i + 1] = num;
			}
		}

		//array의 [0]은 calCenter
		ints[0] = calCenter;

		return ints;
	}

	public void SetItem(int[] ints, GameObject itemObj)
	{
		//array의 [0]은 calCenter

		//칸 겹침 유효성 체크는 일단 넘어갑시다
		for (int i = 1; i < ints.Length; i++)
		{
			//GetNodeByIndex(ints[i]).ItemObject = itemObj;
			//GetNodeByIndex(ints[i]).CenterNodeID = ints[0];

			for (int k = 1; k < ints.Length; k++)
			{
				GetNodeByIndex(ints[i]).linkedNode.Add(GetNodeByIndex(ints[k]));
			}
		}

		//아이템 오브젝트 위치이동 
		itemObj.transform.localPosition = GetNodeByIndex(ints[0]).Gameobject.transform.localPosition;
	}
}
