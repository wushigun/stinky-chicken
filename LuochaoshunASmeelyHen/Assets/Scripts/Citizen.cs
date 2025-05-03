using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Citizen 
{
	public Vector2 pos;
    public float age, health, illbility;
    public byte happiness;
	public purpose purpose;
	public Vector2 workPlace,homePlace;
}
public enum purpose
{
	Rest,
	Work
}
[System.Serializable]
public class Family
{
    public List<Citizen> citizens;
    public float wealth;
    public ResBuilding home;

}

public class ResBuilding : Building
{

}

public class Building
{
    public GameObject building;
	public Vector2 pos;
}

enum EduLv//Education Level
{
    uneducated,
    I,
    II,
    IIIA,
    IIIB,
    IVA,
    IVB
}
