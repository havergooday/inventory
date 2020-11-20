using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Node
{
	public enum NODE_ERROR
	{
		NONE,
		CANT,
	}

	public GameObject Gameobject { get; set; }
	public int ID { get; set; }
	public string StorageID { get; set; }
	public GameObject ItemObject;
	public int CenterNodeID;
	public List<Node> linkedNode;
}
