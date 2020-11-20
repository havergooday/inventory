using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NordController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private void Enable()
	{
		this.GetComponent<Image>().color = new Color(1, 1, 1, 0.392f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		//Debug.Log(eventData);
		this.GetComponent<Image>().color = new Color(0.75f, 1, 0.75f, 0.392f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Debug.Log(eventData);
		this.GetComponent<Image>().color = new Color(1,1,1, 0.392f);
	}
}
