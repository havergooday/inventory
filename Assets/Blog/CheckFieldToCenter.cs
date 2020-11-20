using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckFieldToCenter : MonoBehaviour {

	private Color defaultColor = new Color(1, 1, 1, 0.392f);
	private Color selectColor = new Color(0.75f, 1, 0.75f, 0.392f);
	private Color connerColor = new Color(0.18f, 1, 0.18f, 0.392f);

	public int m_totalIndex = 30;

	//node
	public GameObject m_nodeParents;
	public GameObject m_node;
	public int m_horizontalCount = 6;
	private int m_vertialCount;
	public IntVector2 m_nodeSize = new IntVector2(48, 48);

	private Node[] m_nodeArray;

	public int m_currentOverIndex = -1;

	[Header("테스트")]
	public int ItemHorizontalCount = 3;
	public int ItemVerticalCount = 3;

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


			m_nodeArray[i] = new Node();
			m_nodeArray[i].ID = i;
			m_nodeArray[i].Gameobject = obj;
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

	//색깔변경
	private void ChangeColor(GameObject obj, Color color)
	{
		obj.GetComponent<Image>().color = color;
	}
	private void ResetColor()
	{
		for (int i = 0; i < m_nodeArray.Length; i++)
		{
			ChangeColor(m_nodeArray[i].Gameobject, defaultColor);
		}
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (m_currentOverIndex < 0) return;

			ResetColor();

			//계산공식
			int calHorizonRight = ItemHorizontalCount / 2;
			int calVerticalUp = ItemVerticalCount / 2;
			int calHorizonLeft = calHorizonRight + (ItemHorizontalCount % 2) - 1;
			int calVerticalDown = calVerticalUp + (ItemVerticalCount % 2) - 1;

			int calCenter = m_currentOverIndex;

			//십자(+)형태로 점검
			if (m_currentOverIndex + calHorizonRight >= m_currentOverIndex - (m_currentOverIndex % m_horizontalCount) + m_horizontalCount)
			{
				Debug.Log("호라이즌 오버+");
				calHorizonRight -= 1;
				calHorizonLeft += 1;
				calCenter -= 1;
				//return;
			}
			if (m_currentOverIndex - calHorizonLeft < (m_currentOverIndex / m_horizontalCount) * m_horizontalCount)
			{
				Debug.Log("호라이즌 오버-");
				calHorizonRight += 1;
				calHorizonLeft -= 1;
				calCenter += 1;
			}
			if (m_currentOverIndex + (calVerticalUp * m_horizontalCount) >= m_totalIndex)
			{
				Debug.Log("버티컬 오버+");
				calVerticalUp -= 1;
				calVerticalDown += 1;
				calCenter -= m_horizontalCount;
			}
			if (m_currentOverIndex - (calVerticalDown * m_horizontalCount) < 0)
			{
				Debug.Log("버티컬 오버-");
				calVerticalUp += 1;
				calVerticalDown -= 1;
				calCenter += m_horizontalCount;
			}

			if (m_currentOverIndex + calHorizonRight >= m_currentOverIndex - (m_currentOverIndex % m_horizontalCount) + m_horizontalCount
				|| m_currentOverIndex - calHorizonLeft < (m_currentOverIndex / m_horizontalCount) * m_horizontalCount
				|| m_currentOverIndex + (calVerticalUp * m_horizontalCount) > m_totalIndex
				|| m_currentOverIndex - (calVerticalDown * m_horizontalCount) < 0
				|| calCenter < 0
				|| calCenter > m_totalIndex)
			{
				Debug.Log("칸 부족");
				return;
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

			////전체 index를 계산
			for (int k = 0; k < ItemVerticalCount; k++)
			{
				for (int i = 0; i < ItemHorizontalCount; i++)
				{
					int num = (m_horizontalCount * k) + (leftDown + i);
					ChangeColor(m_nodeArray[num].Gameobject, selectColor);
				}
			}
			
			ChangeColor(m_nodeArray[leftUp].Gameobject, connerColor);
			ChangeColor(m_nodeArray[rightUp].Gameobject, connerColor);
			ChangeColor(m_nodeArray[leftDown].Gameobject, connerColor);
			ChangeColor(m_nodeArray[rightDown].Gameobject, connerColor);
		}
	}
}
