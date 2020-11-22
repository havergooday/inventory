using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	[HideInInspector]
	public string m_itemName;
	[HideInInspector]
	public string m_itemDescription;
	[HideInInspector]
	public Sprite m_itemIcon;
	[HideInInspector]
	public Sprite m_itemImage;
	[HideInInspector]
	public int m_horizonCount;
	[HideInInspector]
	public int m_verticalCount;
}
