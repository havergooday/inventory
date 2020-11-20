using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	private void Awake()
	{
		DragObject.OnClick += OnClick;
	}

	private void OnClick(GameObject g)
	{
		//if (g == gameObject)
		//{
		Debug.Log("hit");
		//}
	}
}
