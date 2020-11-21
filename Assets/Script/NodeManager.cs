using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class NodeManager : MonoBehaviour {

	public int m_totalIndex = 30;

	//node
	public GameObject m_nodeParents;
	public GameObject m_node;
	public int m_horizontalCount = 6;
	private int m_vertialCount;
	public IntVector2 m_nodeSize = new IntVector2(48,48);

	private Node[] m_nodeArray;

	public int m_currentOverIndex = -1;

	public GameObject TestItem;
	public GameObject TestItem2;

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
			obj.name = "[" + i%m_horizontalCount + ", " + i/m_horizontalCount + "]\n" + i;
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
			int[] tests = new int[] { 0, 1, 2, 6, 7, 8, 12, 13, 14 };
			SetItem(tests, TestItem);

			int[] tests2 = new int[] { 21, 22, 27, 28 };
			SetItem(tests2, TestItem2);

			//0, Hcount, Hcount*Vcount -hcount +1 ,Hcount*Vcount
			//0,3, 9-3+1), 9
			//0,2, 4-2+1, 4
			//0, 3, 6-3+1, 6
			//0,1,1-1+1,1;
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log(GetNodeByIndex(0).Gameobject.transform.localPosition.x);
			Debug.Log(GetNodeByIndex(0).Gameobject.transform.localPosition.y);
			Debug.Log(GetNodeByIndex(14).Gameobject.transform.localPosition.x);
			Debug.Log(GetNodeByIndex(14).Gameobject.transform.localPosition.y);

			float xdel = GetNodeByIndex(14).Gameobject.transform.localPosition.x - GetNodeByIndex(0).Gameobject.transform.localPosition.x;
			float ydel = GetNodeByIndex(14).Gameobject.transform.localPosition.y - GetNodeByIndex(0).Gameobject.transform.localPosition.y;

			xdel *= 0.5f;
			ydel *= 0.5f;

			TestItem.transform.localPosition = new Vector3(
				GetNodeByIndex(0).Gameobject.transform.localPosition.x + xdel,
				GetNodeByIndex(0).Gameobject.transform.localPosition.y + ydel,
				0);
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

	public int[] CalItemPosition(int ItemHorizontalCount, int ItemVerticalCount)
	{
		if (m_currentOverIndex < 0) return null;

		int calHorizonRight = ItemHorizontalCount / 2;
		int calVerticalUp = ItemVerticalCount / 2;
		int calHorizonLeft = calHorizonRight + (ItemHorizontalCount % 2) -1;
		int calVerticalDown = calVerticalUp + (ItemVerticalCount % 2) -1;
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

	public bool SetItem(int[] ints, GameObject itemObj)
	{
		//칸 겹침 유효성 체크는 일단 넘어갑시다
		//겹치는게 한개이상이면 안놔짐
		if (CheckNodeToItem(ints, itemObj) >= 1)
		{
			Debug.Log("겹침: " + CheckNodeToItem(ints, itemObj));
			return false;
		}

		for (int i = 0; i < ints.Length; i++)
		{
			GetNodeByIndex(ints[i]).ItemObject = itemObj;

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
		itemObj.gameObject.transform.localPosition = GetNodeByIndex(ints[0]).CenterPositon;
		return true;
	}

	public int CheckNodeToItem(int[] ints, GameObject itemObj)
	{
		int clashCount = 0;
		GameObject objTemp = null;

		for (int i = 1; i < ints.Length; i++)
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

}
