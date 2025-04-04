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
	public Renderer mapRenderer;
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
<<<<<<< HEAD
					if(d==Vector2.left || d == Vector2.right)
					{
                        Rmap[before].LC = true;
                        Rmap[before].RC = true;

                    }
                    if (d==Vector2.down || d == Vector2.up)
					{
                        Rmap[before].DC = true;
                        Rmap[before].UC = true;

                    }
                    before = p;
=======
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
>>>>>>> 8ff2de93e4267467e996a8f0ae3edc55247052d6
				}
			}
		}
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
}