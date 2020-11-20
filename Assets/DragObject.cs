using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {

	public delegate void OnClickEvent(GameObject obj);
	public static event OnClickEvent OnClick;

	private GameObject m_targetObject;
	private bool m_isDrag = false;
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A))
		{
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Debug.Log(ray.GetPoint(100));
			//Debug.Log(ray.direction);
			//RaycastHit hit;
			//if (Physics.Raycast(ray, out hit))
			//{
			//	Debug.Log(hit.transform.gameObject);
			//	OnClick(hit.transform.gameObject);
			//}
			//Debug.Log(Input.mousePosition);
			//Vector3 mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
			//Debug.Log(mousePos);
			//Debug.Log(mousePos2D);
			//RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity);
			//if (hit)
			//{
			//	Debug.Log(hit.collider.name);
			//	OnClick(hit.transform.gameObject);
			//}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
			if (hit.collider != null)
			{
				Debug.Log(hit.transform.gameObject);
				OnClick(hit.transform.gameObject);
				m_isDrag = true;
				m_targetObject = hit.transform.gameObject;
			}
		
		}
		else if (Input.GetMouseButtonUp(0))
		{
			m_isDrag = false;
			m_targetObject = null;
		}
		
	}

	private void FixedUpdate()
	{
		if (m_isDrag)
		{
			Debug.Log(Input.mousePosition);

			m_targetObject.transform.localPosition = Input.mousePosition;
		}
	}
}
