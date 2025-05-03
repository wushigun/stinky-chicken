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
<<<<<<< HEAD
	public Color Resident,Industry;
	public Renderer mapRenderer;
	public Navigation Navigation;
	
	public List<Citizen> Citizens = new List<Citizen>();
	
=======
	public Color Resident,industry;
	public Renderer mapRenderer;
	public Navigation Navigation;

	public Dictionary<Citizen, Vector2> Citmap;


>>>>>>> 7f7ad307fc571a56b895fa7c00188e7be04a1bef
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
	float time = 0;
    public void Update()
	{
		switch(mode)
		{
			//道路
			case 0:
				BuildRoad();
				DestroyRoad();
				break;
			default:
				ChangeZone(mode-1);
				CancleZone(mode-1);
				break;
		}
		if(time >= 2f)
		{
			ChangeCitizenState();
			time = 0;
		}
		DrawMap();
		DebugAmount();
		time += Time.deltaTime;
	}
	
	//Bug:无法在先放住宅后再放工业来寻路
	//Bug:无法到达正确的位置，卡在路中间
	public void ChangeCitizenState()
	{
		for(int i = 0;i<Citizens.Count;i++)
		{
			switch(Citizens[i].purpose)
			{
				case purpose.Work:
					if(Citizens[i].workPlace!=new Vector2(-1,-1))
					{
						List<visPoint> path = Navigate(
						Citizens[i].pos,Citizens[i].workPlace);
						if(path!=null && path.Count>=2){
							Citizens[i].pos = path[path.Count-2].pos;
						}
						else
							Citizens[i].purpose=purpose.Rest;
					}
					else
					{
						DistributeRandomWork(i);
					}
					break;
				case purpose.Rest:
					if(Citizens[i].homePlace!=new Vector2(-1,-1))
					{
						List<visPoint> path = Navigate(
						Citizens[i].pos,Citizens[i].homePlace);
						if(path!=null && path.Count>=2)
							Citizens[i].pos = path[path.Count-2].pos;
						else
							Citizens[i].purpose=purpose.Work;
					}
					else
					{
						DistributeRandomHome(i);
					}
					break;
			}
		}
	}
<<<<<<< HEAD
=======

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
>>>>>>> 7f7ad307fc571a56b895fa7c00188e7be04a1bef
	
	//<summary>导航</summary>
	public List<visPoint> Navigate(Vector2 start,Vector2 end)
	{
		Navigation.UpdatePoints(Rmap);
		return Navigation.SearchPath(start,end);
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
					if(map[p].zoneType==ZoneType.LowInd)
						colorMap[i*width+j]=Industry;
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
	
	public void DistributeRandomWork(int order)
	{
		List<Vector2> Ind = FindZoneType(ZoneType.LowInd);
		if(Ind.Count<=0)
		{
			Citizens[order].workPlace = new Vector2(-1,-1);
			return;
		}
		Citizens[order].workPlace = GetCloseToRoad(Ind[Random.Range(0,Ind.Count)]);
	}
	
	public void DistributeRandomHome(int order)
	{
		List<Vector2> Res = FindZoneType(ZoneType.LowRes);
		if(Res.Count<=0)
		{
			Citizens[order].homePlace = new Vector2(-1,-1);
			return;
		}
		Citizens[order].homePlace = GetCloseToRoad(Res[Random.Range(0,Res.Count)]);
	}
	
	public List<Vector2> FindZoneType(ZoneType zt)
	{
		List<Vector2> zone = new List<Vector2>();
		foreach(Vector2 t in map.Keys)
		{
			if(map[t].zoneType == zt)
				zone.Add(t);
		}
		return zone;
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
	
	//获取靠近的路面
	public Vector2 GetCloseToRoad(Vector2 pos)
	{
		if(Rmap.ContainsKey(pos+Vector2.right))
			return pos+Vector2.right;
		if(Rmap.ContainsKey(pos+Vector2.down))
			return pos+Vector2.down;
		if(Rmap.ContainsKey(pos+Vector2.up))
			return pos+Vector2.up;
		if(Rmap.ContainsKey(pos+Vector2.left))
			return pos+Vector2.left;
		return new Vector2(-1,-1);
	}
	
	//<summary>规划区域</summary>
	public void ChangeZone(int order)
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
<<<<<<< HEAD
					if(GetCloseToRoad(p)!=new Vector2(-1,-1))
					{
						map[p].zoneType = (ZoneType)order;
						if((ZoneType)order==ZoneType.LowRes)
						{
							Citizen c = new Citizen();
							c.pos = GetCloseToRoad(p);
							c.purpose = purpose.Work;
							c.homePlace = GetCloseToRoad(p);
							Citizens.Add(c);
							DistributeRandomWork(Citizens.Count-1);
						}
					}
=======
					if(Rmap.ContainsKey(p))
						map[p].zoneType = ZoneType.LowRes;
>>>>>>> 7f7ad307fc571a56b895fa7c00188e7be04a1bef
				}
			}
		}
	}
	
	//<summary>取消规划</summary>
	public void CancleZone(int order)
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
	
	public int Statistics(Vector2 pos)
	{
		int count = 0;
		foreach(Citizen Cit in Citizens)
		{
			if(Cit.pos == pos)
				count++;
		}
		return count;
	}
	
	public void DebugAmount()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit))
		{
			if(hit.collider.gameObject.tag=="map")
			{
				//Instantiate(Building,new Vector3(Mathf.Floor(hit.point.x),0,Mathf.Floor(hit.point.z)),Quaternion.identity);
				Vector2 p =new Vector2(Mathf.Floor(hit.point.z),Mathf.Floor(hit.point.x));
				Debug.Log(Statistics(p));
			}
		}
	}
	
	//0--道路 n--第n-1个区域
	public void changeBuildMode(int order)
	{
		mode = order;
	}
}