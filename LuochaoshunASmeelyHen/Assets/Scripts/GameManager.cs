using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject Building;
    public void Updata()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit))
		{
			if(hit.collider.gameObject.tag=="map")
			{
				Instantiate(Building,new Vector3(Mathf.Floor(hit.point.x),Mathf.Floor(hit.point.y),hit.point.z),Quaternion.identity);
			}
		}
	}
}