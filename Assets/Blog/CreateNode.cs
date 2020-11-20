using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateNode : MonoBehaviour {

	public int m_totalIndex = 30;

	//node
	public GameObject m_nodeParents;
	public GameObject m_node;
	public int m_horizontalCount = 6;
	private int m_vertialCount;
	public IntVector2 m_nodeSize = new IntVector2(48, 48);

	private Node[] m_nodeArray;

	private int m_currentOverIndex = -1;

	void Awake()
	{
		initialize();

		CreateSlot();
	}

	private void Update()
	{
		Debug.Log(m_currentOverIndex);
	}

	private void initialize()
	{
		m_currentOverIndex = -1;
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
}
