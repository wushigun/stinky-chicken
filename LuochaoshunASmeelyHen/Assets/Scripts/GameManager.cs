using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject Building;
	public Dictionary<Vector2,Tile> map = new Dictionary<Vector2,Tile>();
	public Dictionary<Vector2,RoadTile> Rmap = new Dictionary<Vector2,RoadTile>();
	public int width,height;
	public Color RRoad,DRoad,LRoad,URoad,nRoad;
	public Color Resident,industry;
	public Renderer mapRenderer;
	public Navigation Navigation;

	public Dictionary<Citizen, Vector2> Citmap;


	int mode=0;
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
	Vector2 before;
    public void Update()
	{
		switch(mode)
		{
			//道路
			case 0:
				BuildRoad();
				DestroyRoad();
				break;
			case 1:
				BuildZone();
				DestroyZone();
				break;
		}
	}

	public int GetCitizensCount(Vector2 pos)
	{ 
		int count = 0;
		foreach (Citizen citizen in Citmap.Keys)
		{
			if(Citmap[citizen]==pos)
				count++;
		}
		return count;
	}
	
	//<summary>导航</summary>
	public void Navigate()
	{
		Navigation.UpdatePoints(Rmap);
		List<visPoint> path = Navigation.SearchPath(Vector2.zero,new(width-1,height-1));
		if(path!=null)
		{
			for(int i = 0;i<path.Count;i++)
			{
				Debug.Log(path[i].pos.x.ToString()+","+path[i].pos.y.ToString());
			}
		}
	}
	
	//<summary>绘制地图</summary>
	public void DrawMap()
	{

		Texture2D texture = new Texture2D(width,height);
		Color[] colorMap = new Color[height*width];
		for(int i=0;i<width;i++)
		{
			for(int j=0;j<height;j++)
			{
				if(Rmap!=null)
				{
					Vector2 p = new Vector2(i,j);
					if(Rmap.ContainsKey(p))
					{
						Color c = Color.black;
						if(Rmap[p].DC)
							c+=DRoad;
						if(Rmap[p].LC)
							c+=LRoad;
						if(Rmap[p].RC)
							c+=RRoad;
						if(Rmap[p].UC)
							c+=URoad;
						colorMap[i*width+j] = c;
					}
					else
						colorMap[i*width+j] = nRoad;
					if(map[p].zoneType==ZoneType.LowRes)
						colorMap[i*width+j]=Resident;
				}
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colorMap);
		texture.Apply();
		
		mapRenderer.sharedMaterial.mainTexture = texture;
		
		mapRenderer.transform.localScale = new Vector3(width/10,1,height/10);
		mapRenderer.transform.position = new Vector3(width/2,0,height/2);
		
	}
	
	//<summary>铺路</summary>
	public void BuildRoad()
	{
		if(Input.GetMouseButtonDown(0))
			before=new Vector2();
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
					Vector2 d = p - before;
					if(d==Vector2.left)
					{
						Rmap[before].LC = true;
						Rmap[p].RC = true;
					}
					if(d==Vector2.right)
					{
						Rmap[before].RC = true;
						Rmap[p].LC = true;
					}
					if(d==Vector2.down)
					{
						Rmap[before].DC = true;
						Rmap[p].UC = true;
					}
					if(d==Vector2.up)
					{
						Rmap[before].UC = true;
						Rmap[p].DC = true;
					}
					before = p;
				}
			}
		}
	}
	
	//<summary>毁路</summary>
	public void DestroyRoad()
	{
		if(Input.GetMouseButton(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				if(hit.collider.gameObject.tag=="map")
				{
					//Instantiate(Building,new Vector3(Mathf.Floor(hit.point.x),0,Mathf.Floor(hit.point.z)),Quaternion.identity);
					Vector2 p =new Vector2(Mathf.Floor(hit.point.z),Mathf.Floor(hit.point.x));
					if(Rmap.ContainsKey(p))
					{
						if(Rmap[p].LC && Rmap.ContainsKey(p+Vector2.left))
							Rmap[p+Vector2.left].RC=false;
						if(Rmap[p].RC && Rmap.ContainsKey(p+Vector2.right))
							Rmap[p+Vector2.right].LC=false;
						if(Rmap[p].DC && Rmap.ContainsKey(Vector2.down))
							Rmap[p+Vector2.down].UC=false;
						if(Rmap[p].UC && Rmap.ContainsKey(p+Vector2.up))
							Rmap[p+Vector2.up].DC=false;
						Rmap.Remove(p);
					}
				}
			}
		}
	}
	
	//<summary>规划(低)住宅区</summary>
	public void BuildZone()
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
					if(Rmap.ContainsKey(p))
						map[p].zoneType = ZoneType.LowRes;
				}
			}
		}
	}
	
	//<summary>取消规划</summary>
	public void DestroyZone()
	{
		if(Input.GetMouseButton(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				if(hit.collider.gameObject.tag=="map")
				{
					//Instantiate(Building,new Vector3(Mathf.Floor(hit.point.x),0,Mathf.Floor(hit.point.z)),Quaternion.identity);
					Vector2 p =new Vector2(Mathf.Floor(hit.point.z),Mathf.Floor(hit.point.x));
					map[p].zoneType = ZoneType.Null;
				}
			}
		}
	}
	
	//0--道路 1--住宅
	public void changeBuildMode(int order)
	{
		mode = order;
	}
}