using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public int height;
    public TerrainType TerrainType;
    public float ReslandValue, IndlandValue, ComlandValue;//Com refers to commercial
    public byte airPollution, landPollution, noisePollution;
    public ushort oil, mineral, wood, farm, windSpeed;
    public bool isPowered, isWatered;
	public ZoneType zoneType;
}

public enum TerrainType : byte
{
    Null,
    Grass,
    Water,
    SaltyWater,
    Sand,

}
public enum ZoneType : byte
{
    Null,
    LowRes,//Resident
    MidRes,
    HiRes,
    LowServ,
    MidServ,
    LowOffice,
    MidOffice,
    HighOffice,
    LowInd,//Industry
    MidInd,
    HighInd,
    PlantingAg,//broken Englsh--has to be replace to a better translation
    LivestockAg,//Agricultrue
    MineralInd,
    PetroInd,
    Forestry,
    Gov
    
}


