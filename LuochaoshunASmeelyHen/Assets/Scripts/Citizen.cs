using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Citizen 
{
	public Vector2 pos;
    public float age, health, illbility;
    public byte happiness;
<<<<<<< HEAD
	public purpose purpose;
	public Vector2 workPlace,homePlace;
}
public enum purpose
{
	Rest,
	Work
=======
    public Vector2 pos;
>>>>>>> 7f7ad307fc571a56b895fa7c00188e7be04a1bef
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
