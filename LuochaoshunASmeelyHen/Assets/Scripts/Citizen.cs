using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Citizen 
{
    public float age, health, illbility;
    public byte happiness;
    public Vector2 pos;
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
