using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatVector2 : MonoBehaviour {

	public float x, y;

	public FloatVector2(float num1, float num2)
	{
		x = num1;
		y = num2;
	}

	public static string String(IntVector2 temp)
	{
		return "(" + temp.x + ", " + temp.y + ")";
	}
}
