using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject Building;
    public void Update()
	{
		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				if(hit.collider.gameObject.tag=="map")
				{
					Instantiate(Building,new Vector3(Mathf.Floor(hit.point.x),0,Mathf.Floor(hit.point.z)),Quaternion.identity);
				}
			}
		}
	}
}