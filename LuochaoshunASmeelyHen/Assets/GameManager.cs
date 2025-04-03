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
				Instantiate(Building,new Vector3(Mathf.Floor(hit.collider.transform.position.x),Mathf.Floor(hit.collider.transform.position.y),hit.collider.transform.position.z));
			}
		}
	}
}