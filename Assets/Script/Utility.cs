using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility {

	public static void ChangeColor(GameObject obj, Color color)
	{
		obj.GetComponent<Image>().color = color;
	}
}
