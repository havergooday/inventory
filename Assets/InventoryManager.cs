using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[System.Serializable]
public class StoredItem
{
	public Item item;
	public IntVector2 position;

	public StoredItem(Item _item, IntVector2 _position)
	{
		item = _item;
		position = _position;
	}
}

public class InventoryManager : MonoBehaviour
{

	public IntVector2 m_size;
	public List<StoredItem> m_items;

	public GameObject[,] m_slotGrid;
	public GameObject slotPrefab;
	public IntVector2 m_gridSize;
	public GameObject m_parents;

	public ClickManager m_manager;
	private int m_currentVer = -1;
	private int m_currentHor = -1;
	private int m_sizeVer;
	private int m_sizeHor;

	private Vector2 m_dropPosition;

	private void Awake()
	{
		m_slotGrid = new GameObject[m_gridSize.x, m_gridSize.y];
		CreateSlot();

		m_sizeHor = 4;
		m_sizeVer = 3;
	}

	public void Update()
	{
		if(m_manager.m_clickObject != null)
			CheckSize(m_manager.m_clickObject.GetComponent<Item>().m_itemSize.x, m_manager.m_clickObject.GetComponent<Item>().m_itemSize.y);
	}

	private void CreateSlot()
	{
		for (int y = 0; y < m_gridSize.y; y++)
		{
			for (int x = 0; x < m_gridSize.x; x++)
			{
				GameObject obj = (GameObject)Instantiate(slotPrefab);
				
				obj.name = "[" + x + ", " + y + "]";
				obj.GetComponentInChildren<Text>().text = obj.name;
				obj.transform.SetParent(m_parents.transform);

				// 이벤트트리거 추가
				int tempX = x;
				int tempY = y;
				EventTrigger.Entry enter = new EventTrigger.Entry();
				enter.eventID = EventTriggerType.PointerEnter;
				enter.callback.AddListener( (eventData) => CheckCurrentPosition(tempX, tempY));
				obj.GetComponent<EventTrigger>().triggers.Add(enter);

				EventTrigger.Entry exit = new EventTrigger.Entry();
				exit.eventID = EventTriggerType.PointerExit;
				exit.callback.AddListener((eventData) => CheckCurrentPosition(-1, -1));
				exit.callback.AddListener((eventData) => ResetOtherGrid());
				obj.GetComponent<EventTrigger>().triggers.Add(exit);

				m_slotGrid[x, y] = obj;
			}
		}

		m_parents.GetComponent<RectTransform>().sizeDelta
			= new Vector2((m_parents.GetComponent<GridLayoutGroup>().spacing.x + m_parents.GetComponent<GridLayoutGroup>().cellSize.x) * m_gridSize.x + 30,
			(m_parents.GetComponent<GridLayoutGroup>().spacing.y + m_parents.GetComponent<GridLayoutGroup>().cellSize.y) * m_gridSize.y + 30);

	}

	private void CheckSize(int sizeHor, int sizeVer)
	{
		if (m_currentHor < 0
			|| m_currentVer < 0)
			return;

		//모든 그리드 초기화
		ResetOtherGrid();

		int posX;
		int posY;
		int origX;
		int origY;

		posX = m_currentHor + Mathf.FloorToInt(sizeHor / 2);
		posY = m_currentVer + Mathf.FloorToInt(sizeVer / 2);

		if (sizeHor % 2 != 0) posX++;
		if (sizeVer % 2 != 0) posY++;


		origX = m_currentHor - Mathf.FloorToInt(sizeHor / 2);
		origY = m_currentVer - Mathf.FloorToInt(sizeVer / 2);

		//좌측 X가 0 이하
		if (origX < 0)
		{
			posX += -1 * origX;
			origX += -1 * origX;
		}

		//우측 X가 gridSize.x 이상
		if (posX > m_gridSize.x)
		{
			origX += (m_gridSize.x - posX);
			posX += (m_gridSize.x - posX);
		}

		//상단 Y가 gridSize.y 이상
		if (posY > m_gridSize.y)
		{
			origY += (m_gridSize.y - posY);
			posY += (m_gridSize.y - posY);
		}

		//하단 Y가 0 이하
		if (origY < 0)
		{
			posY += -1 * origY;
			origY += -1 * origY;
		}

		//Debug.Log(origX + "~" + (posX-1) + "\n" + origY + "~" + (posY-1));
		
		for (int x = origX; x < posX; x++)
			{
				for (int y = origY; y < posY; y++)
				{
					ChangeColor(m_slotGrid[x, y], new Color(0.75f, 1, 0.75f, 0.392f));
				}
			}

		float xxx = (m_slotGrid[origX, origY].GetComponent<RectTransform>().position.x + m_slotGrid[posX - 1, posY - 1].GetComponent<RectTransform>().position.x) * 0.5f;
		float yyy = (m_slotGrid[origX, origY].GetComponent<RectTransform>().position.y + m_slotGrid[posX - 1, posY - 1].GetComponent<RectTransform>().position.y ) * 0.5f;

		m_dropPosition = new Vector2(xxx, yyy);
		//.GetComponent<RectTransform>().position = new Vector3(xxx,
		//														yyy,
		//														0);
	}

	public void InitialItemPosition()
	{
		m_dropPosition = Vector3.zero;
	}

	public Vector2 GetItemPosition()
	{
		return m_dropPosition;
	}

	private void ResetOtherGrid()
	{
		for (int x = 0; x < m_gridSize.x; x++)
		{
			for (int y = 0; y < m_gridSize.y; y++)
			{
				ChangeColor(m_slotGrid[x, y], new Color(1, 1, 1, 0.392f));
			}
		}

	}


	private void ChangeColor(GameObject obj, Color color)
	{
		obj.GetComponent<Image>().color = color;
		//new Color(0.75f, 1, 0.75f, 0.392f)
		//new Color(1, 1, 1, 0.392f)
	}

	private void CheckCurrentPosition(int posX, int posY)
	{
		m_currentHor = posX;
		m_currentVer = posY;
	}

	public void ChangeColorGreen(GameObject obj)
	{
		obj.GetComponent<Image>().color = new Color(0.75f, 1, 0.75f, 0.392f);
		m_currentHor = -1;
		m_currentVer = -1;
	}

	public void ChangeColorWhite(GameObject obj)
	{
		obj.GetComponent<Image>().color = new Color(1, 1, 1, 0.392f);
	}

}
