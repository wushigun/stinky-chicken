using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Navigation : MonoBehaviour
{
	//地图信息
    public Dictionary<Vector2,visPoint> points = new();
	
	//更新地图信息
	public void UpdatePoints(Dictionary<Vector2,RoadTile> RoadTile)
	{
		//清除地图信息
		if(points!=null)
			points.Clear();
		//添加地图信息
		foreach(Vector2 pos in RoadTile.Keys)
		{
			visPoint vp = new();
			vp.pos = pos;
			vp.RC = RoadTile[pos].RC;
			vp.LC = RoadTile[pos].LC;
			vp.UC = RoadTile[pos].UC;
			vp.DC = RoadTile[pos].DC;
			
			//花费运算
			vp.cost = 1;
			
			points.Add(pos,vp);
		}
	}
	
	//A*寻路
    public List<visPoint> SearchPath(Vector2 start,Vector2 end)
    {
        if (!(points.ContainsKey(start) && points.ContainsKey(end)))
            return null;
        //visStraight path = new visStraight();
        
        Dictionary<visPoint,float> priorityQuene = new Dictionary<visPoint,float>();
        Dictionary<visPoint,visPoint> parentDictionary = new Dictionary<visPoint,visPoint>();
        List<visPoint> checkedDictionary = new();

        priorityQuene.Add(points[start], 0);//ni zhe zhi chou ji 

        while (priorityQuene.Count > 0)
        { 
			//Debug.Log(priorityQuene.Count);
            visPoint curPoint = GetLeastCostPoint(priorityQuene);
            if (curPoint == points[end])
                return GetPath(parentDictionary, points[end], points[start]);
            checkedDictionary.Add(curPoint);

            for (int i = 0;i<4;i++)
            {
                visPoint vp = GetRoadOfCorrectDirection(i,curPoint);
				
				if(vp!=null)
				{
					if (checkedDictionary.Contains(vp))
						continue;

					float cost = priorityQuene[curPoint]+vp.cost+ManhattanDistance(vp.pos,end);
					if (priorityQuene.ContainsKey(vp))
					{
						if (cost < priorityQuene[vp])
						{
							priorityQuene[vp] = cost;
							parentDictionary[vp] = curPoint;
						}
					}
					else
					{
						priorityQuene.Add(vp, cost);
						parentDictionary.Add(vp, curPoint);
					}
				}
            }
            priorityQuene.Remove(curPoint);
        }
        return null;
    }
	//获取各个方向的路
	public visPoint GetRoadOfCorrectDirection(int count,visPoint curPoint)
	{
		switch(count)
		{
			case 0:
				if(curPoint.RC)
					return points[curPoint.pos + Vector2.right];
				break;
			case 1:
				if(curPoint.LC)
					return points[curPoint.pos + Vector2.left];
				break;
			case 2:
				if(curPoint.UC)
					return points[curPoint.pos + Vector2.up];
				break;
			case 3:
				if(curPoint.DC)
					return points[curPoint.pos + Vector2.down];
				break;
		}
		return null;
	}
	//通过回溯获取路径
    public List<visPoint> GetPath(Dictionary<visPoint, visPoint> parentDictionary,visPoint end,visPoint start)
    {
        List<visPoint> path = new List<visPoint>();
		if(!parentDictionary.ContainsKey(end))
			return null;
        visPoint vp = parentDictionary[end];
        while (parentDictionary.ContainsKey(vp))
        { 
            path.Add(vp);
            vp = parentDictionary[vp];
        }
        if (vp != start)
            path = null;
        return path;
    }
	//获取最低花费
    public visPoint GetLeastCostPoint(Dictionary<visPoint, float> priorityQuene)
    {
        List<visPoint> priorityList = new List<visPoint>(priorityQuene.Keys);
        visPoint current = priorityList[0];
        foreach (visPoint point in priorityList)
        {
            if (priorityQuene[point] < priorityQuene[current])
                current = point;
        }
        return current;
    }
	//获取曼哈顿距离 s=|x|+|y|
    public float ManhattanDistance(Vector2 endPoint, Vector2 startPoint)
    {
        return Mathf.Abs(endPoint.x - startPoint.x) + Mathf.Abs(endPoint.y - startPoint.y);
    }
    /*public void addPoint(Vector2 pos)
    {
        visPoint vp = new visPoint();
        vp.pos = pos;
        points.Add(pos, vp);
    }*/
    /*public void addRoad(Vector2 start, Vector2 end, bool isBil)
    { 
        if(!(points.ContainsKey(start)&&points.ContainsKey(end)))
            return;
        points[start].addRoad(points[end],ManhattanDistance(start,end));
        if (isBil)
            points[end].addRoad(points[start], ManhattanDistance(start, end));
    }*/
}
public class visPoint
{
    public Vector2 pos;
	public bool RC,LC,UC,DC;
	public float cost;
    /*public List<visStraight> road=new();
    public void addRoad(visPoint endpoint,float distance)
    {
        visStraight newStraight = new visStraight();
        newStraight.endPoint = endpoint;
        newStraight.distanceCost = distance;
        road.Add(newStraight);
    }
    public void removeRoad(visStraight vs)
    {
        vs.endPoint.road.Remove(vs);
        road.Remove(vs);
    }
    public void removeAllRoad()
    {
        for (int i = 0; i < road.Count; i++)
        { 
            removeRoad(road[i]);
        }
    }*/
}
/*public class visStraight
{
    public visPoint endPoint;
    public float distanceCost;
}*/