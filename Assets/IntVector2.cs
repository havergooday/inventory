using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntVector2 {

	public int x, y;

	public IntVector2(int num1, int num2)
	{
		x = num1;
		y = num2;
	}

	public static string String (IntVector2 temp)
	{
		return "(" + temp.x + ", " + temp.y + ")";
	}

	
}
