using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot
{
	public GameObject gameobject;
	public Vector2 position;
	public bool isFilled;
	public GameObject slotItem;

	public Slot(GameObject _gameobject, Vector2 _position, bool _isFilled, GameObject _slotItem)
	{
		gameobject = _gameobject;
		position = _position;
		isFilled = _isFilled;
		slotItem = _slotItem;
	}

}


public class InventoryScript : MonoBehaviour {


	public Slot[,] m_slotGrid;//슬롯 오브젝트 배열
	public GameObject slotPrefab;	//슬롯 프리펩
	public IntVector2 m_gridSize;	//각 그리드 사이즈
	public GameObject m_parents;    //슬롯 오브젝트 부모

	public Transform m_itemsParent;
	public Transform m_inventoryParent;
	public Transform m_grapParent;

	public GameObject m_clickObject;

	private int m_currentVer = -1;
	private int m_currentHor = -1;

	private Vector2 m_dropPosition;


	private int m_posX;
	private int m_posY;
	private int m_origX;
	private int m_origY;



	private void Awake()
	{
		m_slotGrid = new Slot[m_gridSize.x, m_gridSize.y];
		CreateSlot();
	}
	/*
	public void Update()
	{
		if (m_clickObject != null)
		{
			CheckSize(m_clickObject.GetComponent<Item>().m_itemSize.x, m_clickObject.GetComponent<Item>().m_itemSize.y);

			m_clickObject.transform.position = Input.mousePosition;
			m_clickObject.transform.SetParent(m_grapParent);
			m_clickObject.GetComponent<Image>().raycastTarget = false;

			if (Input.GetMouseButtonDown(0))
			{
			
				if (GetItemPosition().x == 0
					&& GetItemPosition().y == 0)
				{
					//되돌리기
					m_clickObject.transform.SetParent(m_itemsParent.transform);
				}
				else
				{

					//if 그곳에 아이템이 있을경우
					////if 아이템이 두개 이상일 경우
					Vector2Int posTemp = CheckSize2(m_clickObject.GetComponent<Item>().m_itemSize.x, m_clickObject.GetComponent<Item>().m_itemSize.y);
					
					
					float xxx = (m_slotGrid[m_origX, m_origY].gameobject.GetComponent<RectTransform>().position.x + m_slotGrid[m_posX - 1, m_posY - 1].gameobject.GetComponent<RectTransform>().position.x) * 0.5f;
					float yyy = (m_slotGrid[m_origX, m_origY].gameobject.GetComponent<RectTransform>().position.y + m_slotGrid[m_posX - 1, m_posY - 1].gameobject.GetComponent<RectTransform>().position.y) * 0.5f;

					//해당 위치로 이동
					for (int x = m_origX; x < m_posX; x++)
					{
						for (int y = m_origY; y < m_posY; y++)
						{
							m_slotGrid[x, y].isFilled = true;
							ChangeColor(m_slotGrid[x, y].gameobject, new Color(0.4f, 0.4f, 0.4f, 0.392f));
						}
					}

					//Vector3 pos = new Vector3(GetItemPosition().x, GetItemPosition().y, 0);
					Vector3 pos = new Vector3(xxx, yyy, 0);
					m_clickObject.transform.SetParent(m_inventoryParent);
					m_clickObject.GetComponent<RectTransform>().position = pos;


					//for (int i = 0; i < m_slotGrid.GetLength(0); i++)
					//{
					//	for(int j = 0; j< m_slotGrid.GetLength(1); j++)
					//	{
					//		if (m_slotGrid[i, j].isFilled == true)
					//		{
					//			Debug.Log(i + ":" + j);
					//		}
					//	}
					//}
				}
			
				m_clickObject.GetComponent<Image>().raycastTarget = true;
			
				m_clickObject = null;
				InitialItemPosition();
			}


		}
	}
	*/

	public void Update()
	{
		if (m_clickObject != null)
		{
			//오브젝트 마우스따라 이동
			m_clickObject.transform.position = Input.mousePosition;
			m_clickObject.transform.SetParent(m_grapParent);
			m_clickObject.GetComponent<Image>().raycastTarget = false;

			//인벤에 있는 오브젝트 잡으면 초기화 후 흰색으로
			for (int y = 0; y < m_slotGrid.GetLength(1); y++)
			{
				for (int x = 0; x < m_slotGrid.GetLength(0); x++)
				{
					if (m_slotGrid[x, y].slotItem == m_clickObject)
					{
						ChangeColor(m_slotGrid[x, y].gameobject, new Color(1, 1, 1, 0.392f));
						m_slotGrid[x, y].slotItem = null;
					}
				}
			}


			IntVector2 leftBottom = CheckSize(m_clickObject.GetComponent<Item>().m_itemSize);

			//나머지 흰색으로
			ResetOtherGrid();
			///

			//마우스오버 초록색으로
			if (leftBottom.x >= 0
					&& leftBottom.y >= 0)
			{
				IntVector2 rightTop2 = new IntVector2(m_clickObject.GetComponent<Item>().m_itemSize.x + leftBottom.x,
															m_clickObject.GetComponent<Item>().m_itemSize.y + leftBottom.y);

				for (int x = leftBottom.x; x < rightTop2.x; x++)
				{
					for (int y = leftBottom.y; y < rightTop2.y; y++)
					{
						ChangeColor(m_slotGrid[x, y].gameobject, new Color(0.75f, 1, 0.75f, 0.392f));
					}
				}
			}
			///

			//아이템 내려놨을때 겹치는 아이템이 있는경우
			bool isChange = false;
			GameObject obj = null;

			//내려뒀을때
			if (Input.GetMouseButtonDown(0))
			{

				if (leftBottom.x < 0
					&& leftBottom.y < 0)
				{
					//되돌리기
					m_clickObject.transform.SetParent(m_itemsParent.transform);
				}
				else
				{
					int itemSizeX = m_clickObject.GetComponent<Item>().m_itemSize.x;
					int itemSizeY = m_clickObject.GetComponent<Item>().m_itemSize.y;

					IntVector2 rightTop = new IntVector2(m_clickObject.GetComponent<Item>().m_itemSize.x + leftBottom.x,
															m_clickObject.GetComponent<Item>().m_itemSize.y + leftBottom.y);


					for (int x = leftBottom.x; x < rightTop.x; x++)
					{
						for (int y = leftBottom.y; y < rightTop.y; y++)
						{
							if (m_slotGrid[x, y].slotItem != null)
							{
								isChange = true;
								obj = m_slotGrid[x, y].slotItem;
							}

							m_slotGrid[x, y].slotItem = m_clickObject;

						}
					}

					//아이템 내려둘때 박스 중앙 위치
					IntVector2 dropPosition = new IntVector2(
																(int)((m_slotGrid[rightTop.x - 1, rightTop.y - 1].gameobject.GetComponent<RectTransform>().position.x + m_slotGrid[leftBottom.x, leftBottom.y].gameobject.GetComponent<RectTransform>().position.x) * 0.5f),
																(int)((m_slotGrid[rightTop.x - 1, rightTop.y - 1].gameobject.GetComponent<RectTransform>().position.y + m_slotGrid[leftBottom.x, leftBottom.y].gameobject.GetComponent<RectTransform>().position.y) * 0.5f)
															);

					Debug.Log(dropPosition.x + ":" + dropPosition.y);
					m_clickObject.GetComponent<RectTransform>().position = new Vector3(dropPosition.x, dropPosition.y, 0);
					m_clickObject.transform.SetParent(m_inventoryParent);
				}

				m_clickObject.GetComponent<Image>().raycastTarget = false;
				m_clickObject = null;
				InitialItemPosition();

				//겹치는거 있으면 교체
				if (isChange)
				{
					m_clickObject = obj;
					isChange = false;
					obj = null;
				}

			}
		}


		//오브젝트 있으면 검은색으로
		for (int y = 0; y < m_slotGrid.GetLength(1); y++)
		{
			for (int x = 0; x < m_slotGrid.GetLength(0); x++)
			{
				if(m_slotGrid[x,y].slotItem != null)
					ChangeColor(m_slotGrid[x, y].gameobject, new Color(0.4f, 0.4f, 0.4f, 0.392f));
			}
		}
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
				enter.callback.AddListener((eventData) => CheckCurrentPosition(tempX, tempY));
				obj.GetComponent<EventTrigger>().triggers.Add(enter);

				EventTrigger.Entry exit = new EventTrigger.Entry();
				exit.eventID = EventTriggerType.PointerExit;
				exit.callback.AddListener((eventData) => CheckCurrentPosition(-1, -1));
				//exit.callback.AddListener((eventData) => ResetOtherGrid());
				obj.GetComponent<EventTrigger>().triggers.Add(exit);

				Slot slot = new Slot(obj, new Vector3(tempX, tempY), false, null);

				m_slotGrid[x, y] = slot;
			}
		}

		m_parents.GetComponent<RectTransform>().sizeDelta
			= new Vector2((m_parents.GetComponent<GridLayoutGroup>().spacing.x + m_parents.GetComponent<GridLayoutGroup>().cellSize.x) * m_gridSize.x + 30,
			(m_parents.GetComponent<GridLayoutGroup>().spacing.y + m_parents.GetComponent<GridLayoutGroup>().cellSize.y) * m_gridSize.y + 30);

	}

	private IntVector2 CheckSize(IntVector2 size)
	{
		if (m_currentHor < 0
			   || m_currentVer < 0)
			return new IntVector2(-1,-1);

		int rightX;
		int rightY;
		int leftX;
		int leftY;

		rightX = m_currentHor + Mathf.FloorToInt(size.x / 2);
		rightY = m_currentVer + Mathf.FloorToInt(size.y / 2);

		if (size.x % 2 != 0) rightX++;
		if (size.y % 2 != 0) rightY++;


		leftX = m_currentHor - Mathf.FloorToInt(size.x / 2);
		leftY = m_currentVer - Mathf.FloorToInt(size.y / 2);

		//하단 Y가 0 이하
		if (leftY < 0)
		{
			rightY += -1 * leftY;
			leftY += -1 * leftY;
		}

		//상단 Y가 gridSize.y 이상
		if (rightY > m_gridSize.y)
		{
			leftY += (m_gridSize.y - rightY);
			rightY += (m_gridSize.y - rightY);
		}

		//우측 X가 gridSize.x 이상
		if (rightX > m_gridSize.x)
		{
			leftX += (m_gridSize.x - rightX);
			rightX += (m_gridSize.x - rightX);
		}

		//좌측 X가 0 이하
		if (leftX < 0)
		{
			rightX += -1 * leftX;
			leftX += -1 * leftX;
		}

		return new IntVector2(leftX, leftY);

	}

	private void CheckSize(int sizeHor, int sizeVer)
	{
		InitialItemPosition();

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

		//하단 Y가 0 이하
		if (origY < 0)
		{
			posY += -1 * origY;
			origY += -1 * origY;
		}

		//상단 Y가 gridSize.y 이상
		if (posY > m_gridSize.y)
		{
			origY += (m_gridSize.y - posY);
			posY += (m_gridSize.y - posY);
		}

		//우측 X가 gridSize.x 이상
		if (posX > m_gridSize.x)
		{
			origX += (m_gridSize.x - posX);
			posX += (m_gridSize.x - posX);
		}

		//좌측 X가 0 이하
		if (origX < 0)
		{
			posX += -1 * origX;
			origX += -1 * origX;
		}

		for (int x = origX; x < posX; x++)
		{
			for (int y = origY; y < posY; y++)
			{
				if(m_slotGrid[x, y].isFilled != true)
					ChangeColor(m_slotGrid[x, y].gameobject, new Color(0.75f, 1, 0.75f, 0.392f));
			}
		}

		float xxx = (m_slotGrid[origX, origY].gameobject.GetComponent<RectTransform>().position.x + m_slotGrid[posX - 1, posY - 1].gameobject.GetComponent<RectTransform>().position.x) * 0.5f;
		float yyy = (m_slotGrid[origX, origY].gameobject.GetComponent<RectTransform>().position.y + m_slotGrid[posX - 1, posY - 1].gameobject.GetComponent<RectTransform>().position.y) * 0.5f;

		m_dropPosition = new Vector2(xxx, yyy);
	}

	private Vector2Int CheckSize2(int sizeHor, int sizeVer)
	{
		InitialItemPosition();

		if (m_currentHor < 0
			|| m_currentVer < 0)
			return Vector2Int.zero;

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

		//하단 Y가 0 이하
		if (origY < 0)
		{
			posY += -1 * origY;
			origY += -1 * origY;
		}

		//상단 Y가 gridSize.y 이상
		if (posY > m_gridSize.y)
		{
			origY += (m_gridSize.y - posY);
			posY += (m_gridSize.y - posY);
		}

		//우측 X가 gridSize.x 이상
		if (posX > m_gridSize.x)
		{
			origX += (m_gridSize.x - posX);
			posX += (m_gridSize.x - posX);
		}

		//좌측 X가 0 이하
		if (origX < 0)
		{
			posX += -1 * origX;
			origX += -1 * origX;
		}

		for (int x = origX; x < posX; x++)
		{
			for (int y = origY; y < posY; y++)
			{
				ChangeColor(m_slotGrid[x, y].gameobject, new Color(0.75f, 1, 0.75f, 0.392f));
			}
		}


		m_posX = posX;
		m_posY = posY;
		m_origX = origX;
		m_origY = origY;

		return new Vector2Int(posX, posY);

		float xxx = (m_slotGrid[origX, origY].gameobject.GetComponent<RectTransform>().position.x + m_slotGrid[posX - 1, posY - 1].gameobject.GetComponent<RectTransform>().position.x) * 0.5f;
		float yyy = (m_slotGrid[origX, origY].gameobject.GetComponent<RectTransform>().position.y + m_slotGrid[posX - 1, posY - 1].gameobject.GetComponent<RectTransform>().position.y) * 0.5f;

		m_dropPosition = new Vector2(xxx, yyy);
	}

	public void InitialItemPosition()
	{
		m_dropPosition = Vector3.zero;
	}
	public Vector2 GetItemPosition()
	{
		return m_dropPosition;
	}


	private void CheckCurrentPosition(int posX, int posY)
	{
		m_currentHor = posX;
		m_currentVer = posY;
	}

	private void ResetOtherGrid()
	{
		for (int x = 0; x < m_gridSize.x; x++)
		{
			for (int y = 0; y < m_gridSize.y; y++)
			{
				if(m_slotGrid[x,y].isFilled == false) 
					ChangeColor(m_slotGrid[x, y].gameobject, new Color(1, 1, 1, 0.392f));
			}
		}
	}

	private void ChangeColor(GameObject obj, Color color)
	{
		obj.GetComponent<Image>().color = color;
	}
}
