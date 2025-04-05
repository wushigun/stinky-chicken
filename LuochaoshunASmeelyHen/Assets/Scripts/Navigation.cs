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

        priorityQuene.Add(points[start], 0);

        while (priorityQuene.Count > 0)
        { 
            visPoint curPoint = GetLeastCostPoint(priorityQuene);
            if (curPoint == points[end])
                return GetPath(parentDictionary);
            priorityQuene.Remove(curPoint);
            checkedDictionary.Add(curPoint);


        }
        return null;
    }
    public List<visPoint> GetPath(Dictionary<visPoint, visPoint> parentDictionary)
    {
        return null;
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
        points.Add(pos, new());
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
    public List<visStraight> road=new();
    public void addRoad(visPoint endpoint,float distance)
    {
        visStraight newStraight = new visStraight();
        newStraight.endPoint = endpoint;
        newStraight.distanceCost = distance;
        road.Add(newStraight);
    }
}
public class visStraight
{
    public visPoint endPoint;
    public float distanceCost;
}