using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public Dictionary<Vector2,visPoint> points;
    public List<visPoint> SearchPath(Vector2 start,Vector2 end)
    {
        if (!(points.ContainsKey(start) && points.ContainsKey(end)))
            return null;
        visStraight path = new visStraight();
        
        Dictionary<visPoint,float> priorityQuene = new Dictionary<visPoint,float>();
        Dictionary<visPoint,visPoint> parentDictionary = new Dictionary<visPoint,visPoint>();
        List<visPoint> checkedDictionary = new();

        priorityQuene.Add(points[start], 0);//ni zhe zhi chou ji 

        while (priorityQuene.Count > 0)
        { 
            visPoint curPoint = GetLeastCostPoint(priorityQuene);
            if (curPoint == points[end])
                return GetPath(parentDictionary, points[end], points[start]);
            checkedDictionary.Add(curPoint);

            foreach (visStraight vs in curPoint.road)
            {
                visPoint vp = vs.endPoint;

                if (checkedDictionary.Contains(vp))
                    continue;

                float cost = priorityQuene[vp]+vs.distanceCost+ManhattanDistance(vp.pos,end);
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
            priorityQuene.Remove(curPoint);
        }
        return null;
    }
    public List<visPoint> GetPath(Dictionary<visPoint, visPoint> parentDictionary,visPoint end,visPoint start)
    {
        List<visPoint> path = new List<visPoint>();
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
    public float ManhattanDistance(Vector2 endPoint, Vector2 startPoint)
    {
        return Mathf.Abs(endPoint.x - startPoint.x) + Mathf.Abs(endPoint.y - startPoint.y);
    }
    public void addPoint(Vector2 pos)
    {
        visPoint vp = new visPoint();
        vp.pos = pos;
        points.Add(pos, vp);
    }
    public void addRoad(Vector2 start, Vector2 end, bool isBil)
    { 
        if(!(points.ContainsKey(start)&&points.ContainsKey(end)))
            return;
        points[start].addRoad(points[end],ManhattanDistance(start,end));
        if (isBil)
            points[end].addRoad(points[start], ManhattanDistance(start, end));
    }
}
public class visPoint
{
    public Vector2 pos;
    public List<visStraight> road=new();
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
    }
}
public class visStraight
{
    public visPoint endPoint;
    public float distanceCost;
}