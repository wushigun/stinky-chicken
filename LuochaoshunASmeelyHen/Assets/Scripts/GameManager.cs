using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject Building;
	public Dictionary<Vector2,Tile> map = new Dictionary<Vector2,Tile>();
	public Dictionary<Vector2,RoadTile> Rmap = new Dictionary<Vector2,RoadTile>();
	public int width,height;
	public Color Road,nRoad;
	public Material material;
	void Start()
	{
		for(int i=0;i<width;i++)
		{
			for(int j=0;j<height;j++)
			{
				Tile m_tile = new Tile();
				map.Add(new Vector2(i,j),m_tile);
			}
		}//init map info
	}
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
					//Instantiate(Building,new Vector3(Mathf.Floor(hit.point.x),0,Mathf.Floor(hit.point.z)),Quaternion.identity);
					Vector2 p =new Vector2(Mathf.Floor(hit.point.z),Mathf.Floor(hit.point.x));
					if(!Rmap.ContainsKey(p))
					{
						RoadTile m_rtile = new RoadTile();
						Rmap.Add(p,m_rtile);
					}
				}
			}
		}
		Texture2D texture = new Texture2D(width,height);
		Color[] colorMap = new Color[height*width];
		for(int i=0;i<width;i++)
		{
			for(int j=0;j<height;j++)
			{
				if(Rmap!=null)
				{
					if(Rmap.ContainsKey(new Vector2(i,j)))
						colorMap[i*width+j] = Road;
					else
						colorMap[i*width+j] = nRoad;
				}
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colorMap);
		texture.Apply();
		
		material.mainTexture = texture;
	}
}